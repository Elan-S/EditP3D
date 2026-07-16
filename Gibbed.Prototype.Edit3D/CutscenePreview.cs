using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Gibbed.Prototype.FileFormats;
using Gibbed.Prototype.FileFormats.Pure3D;
using Gibbed.Prototype.FileFormats.Pure3D.Prototype2;

namespace Gibbed.Prototype.Edit3D
{
    internal sealed class CutscenePreview
    {
        public string FigFileName { get; set; }
        public string[] LoadedFileNames { get; set; }
        public string[] ReferencedP3DFileNames { get; set; }
        public List<CutsceneFightTrack> FightTracks { get; private set; }
        public List<CutsceneActorObject> Actors { get; private set; }
        public List<CutscenePropObject> Props { get; private set; }
        public List<CutsceneLightObject> Lights { get; private set; }
        public List<CutsceneCameraObject> Cameras { get; private set; }
        public List<CutsceneTimelineEvent> TimelineEvents { get; private set; }
        public List<BaseNode> PreviewNodes { get; private set; }
        public List<BaseNode> StagePreviewNodes { get; private set; }
        public float FrameRate { get; set; }
        public float DurationSeconds { get; set; }
        public int TotalFrames { get; set; }

        public CutscenePreview()
        {
            this.FightTracks = new List<CutsceneFightTrack>();
            this.Actors = new List<CutsceneActorObject>();
            this.Props = new List<CutscenePropObject>();
            this.Lights = new List<CutsceneLightObject>();
            this.Cameras = new List<CutsceneCameraObject>();
            this.TimelineEvents = new List<CutsceneTimelineEvent>();
            this.PreviewNodes = new List<BaseNode>();
            this.StagePreviewNodes = new List<BaseNode>();
            this.LoadedFileNames = new string[0];
            this.ReferencedP3DFileNames = new string[0];
            this.FrameRate = 30.0f;
            this.DurationSeconds = 0.0f;
            this.TotalFrames = 1;
        }

        public override string ToString()
        {
            return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "Cutscene ({0} fights, {1} actors, {2} cameras, {3} preview nodes)",
                this.FightTracks.Count,
                this.Actors.Count,
                this.Cameras.Count,
                this.PreviewNodes.Count);
        }

        public static CutscenePreview Build(Pure3DFile file, string figFileName, IEnumerable<string> loadedFileNames)
        {
            var preview = new CutscenePreview
            {
                FigFileName = figFileName,
                LoadedFileNames = loadedFileNames == null ? new string[0] : loadedFileNames.ToArray(),
                ReferencedP3DFileNames = FindReferencedP3DFiles(file, figFileName).ToArray(),
            };
            if (file == null)
            {
                return preview;
            }

            var nodes = Flatten(file.Nodes).ToList();
            var animations = nodes.OfType<Animation>().ToList();
            var animationNamesByHash = BuildAnimationHashLookup(animations);
            var actorNamesByHash = BuildActorHashLookup(nodes, animations);
            var previewCompositeNames = nodes.OfType<CompositeDrawable>().Select(c => c.Name)
                .Concat(nodes.OfType<P2PolySkinComposite>().Select(c => c.Name))
                .Where(n => string.IsNullOrEmpty(n) == false)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
            float timelineOffset = 0.0f;
            foreach (var fight in nodes.OfType<FightDefinition>())
            {
                var data = fight.GetChildNode<FightData>();
                var strings = data == null ? new string[0] : ExtractAsciiStrings(data.Data, 4).Take(32).ToArray();
                preview.FightTracks.Add(new CutsceneFightTrack
                {
                    Name = fight.Name,
                    Context = fight.Context,
                    DataLength = data == null || data.Data == null ? 0 : data.Data.Length,
                    Strings = strings,
                });

                if (data != null && data.Data != null)
                {
                    var events = ExtractTimelineEvents(fight.Name, data.Data, animationNamesByHash, actorNamesByHash).ToList();
                    ApplyLocalTakeTimelineOffsets(events);
                    if (events.Count > 0)
                    {
                        if (timelineOffset > 0.0f && events.Min(e => e.StartSeconds) <= 0.001f)
                        {
                            foreach (var evt in events)
                            {
                                evt.StartSeconds += timelineOffset;
                                evt.EndSeconds += timelineOffset;
                            }
                        }

                        preview.TimelineEvents.AddRange(events);
                        timelineOffset = Math.Max(timelineOffset, GetTimelineEndSeconds(events));
                    }
                }
            }

            preview.TimelineEvents = preview.TimelineEvents
                .OrderBy(e => e.StartSeconds)
                .ThenBy(e => e.Kind)
                .ThenBy(e => e.AnimationName)
                .ToList();

            var metaObjects = nodes.OfType<MetaObjectDefinition>().ToList();
            var metaStrings = metaObjects.ToDictionary(
                m => m,
                m =>
                {
                    var data = m.GetChildNode<MetaObjectData>();
                    return data == null ? new string[0] : ExtractMetaObjectStrings(data.Data).Take(32).ToArray();
                });
            foreach (var meta in metaObjects)
            {
                if (LooksLikeProp(meta) == true)
                {
                    var propData = meta.GetChildNode<MetaObjectData>();
                    var propStrings = metaStrings[meta];
                    CutscenePropObject prop;
                    if (TryReadPropSpawner(meta, propData == null ? null : propData.Data, propStrings, out prop) == true)
                    {
                        preview.Props.Add(prop);
                    }
                    continue;
                }

                if (LooksLikeActor(meta) == false && LooksLikeLight(meta) == false)
                {
                    continue;
                }

                var data = meta.GetChildNode<MetaObjectData>();
                var strings = metaStrings[meta];
                if (LooksLikeLight(meta) == true)
                {
                    CutsceneLightObject light;
                    if (TryReadCutsceneLight(meta, data == null ? null : data.Data, strings, out light) == true)
                    {
                        preview.Lights.Add(light);
                    }

                    continue;
                }

                preview.Actors.Add(new CutsceneActorObject
                {
                    LongName = meta.LongName,
                    ShortName = meta.ShortName,
                    TypeName = meta.TypeName,
                    ActorHash = ResolveMetaActorHash(meta),
                    DataLength = data == null || data.Data == null ? 0 : data.Data.Length,
                    CompositeName = ResolveMetaCompositeName(meta, strings, previewCompositeNames, metaObjects, metaStrings),
                    Position = data == null ? new Vec3() : ReadMetaTransformPosition(data.Data),
                    RotationX = data == null ? 0.0f : ReadSingleBigEndian(data.Data, 16),
                    RotationY = data == null ? 0.0f : ReadSingleBigEndian(data.Data, 20),
                    RotationZ = data == null ? 0.0f : ReadSingleBigEndian(data.Data, 24),
                    RotationW = data == null ? 1.0f : ReadSingleBigEndian(data.Data, 28),
                    Strings = strings.Take(16).ToArray(),
                });
            }

            AssignCutsceneLightGroups(preview);

            foreach (var camera in nodes.OfType<Camera>())
            {
                preview.Cameras.Add(new CutsceneCameraObject
                {
                    Name = camera.Name,
                    Fov = camera.FOV,
                    NearClip = camera.NearClip,
                    FarClip = camera.FarClip,
                    Position = new Vec3(camera.Position.X, camera.Position.Y, camera.Position.Z),
                    Look = new Vec3(camera.Look.X, camera.Look.Y, camera.Look.Z),
                    Up = new Vec3(camera.Up.X, camera.Up.Y, camera.Up.Z),
                });
            }

            preview.PreviewNodes.AddRange(nodes.OfType<CompositeDrawable>().Cast<BaseNode>());
            preview.PreviewNodes.AddRange(nodes.OfType<P2PolySkinComposite>().Cast<BaseNode>());
            // Disable stages for the time being
            preview.StagePreviewNodes.AddRange(FindStageGeometryNodes(nodes, FindReferencedStageGeometryNames(file)).Cast<BaseNode>());
            preview.PreviewNodes.AddRange(preview.StagePreviewNodes);
            if (preview.PreviewNodes.Count == 0)
            {
                preview.PreviewNodes.AddRange(nodes.OfType<PolySkin>().Cast<BaseNode>());
                preview.PreviewNodes.AddRange(nodes.OfType<Geometry>().Cast<BaseNode>());
                preview.PreviewNodes.AddRange(nodes.OfType<P2PolySkin>().Cast<BaseNode>());
            }
            preview.PreviewNodes = preview.PreviewNodes.Distinct().ToList();
            preview.StagePreviewNodes = preview.StagePreviewNodes.Distinct().ToList();

            preview.DurationSeconds = EstimateDurationSeconds(preview.TimelineEvents, nodes.OfType<Animation>(), preview.FrameRate);
            preview.TotalFrames = Math.Max(1, (int)Math.Ceiling(preview.DurationSeconds * Math.Max(1.0f, preview.FrameRate)));

            return preview;
        }

        private static float GetTimelineEndSeconds(IEnumerable<CutsceneTimelineEvent> events)
        {
            if (events == null)
            {
                return 0.0f;
            }

            var timelineEvents = events.ToList();
            var fmvEnd = timelineEvents.Where(e => string.Equals(e.Kind, "FMV", StringComparison.OrdinalIgnoreCase) == true)
                                       .Select(e => e.EndSeconds)
                                       .DefaultIfEmpty(0.0f)
                                       .Max();
            var eventEnd = timelineEvents.Select(e => e.EndSeconds).DefaultIfEmpty(0.0f).Max();
            return Math.Max(fmvEnd, eventEnd);
        }

        private static void ApplyLocalTakeTimelineOffsets(IList<CutsceneTimelineEvent> events)
        {
            if (events == null || events.Count == 0)
            {
                return;
            }

            string currentTakeGroup = null;
            var seenTakeGroups = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            float timelineOffset = 0.0f;
            float currentGroupEnd = 0.0f;

            foreach (var evt in events.OrderBy(e => e.TrackOffset))
            {
                if (evt == null || IsAnimationTimelineEvent(evt) == false)
                {
                    continue;
                }

                var takeGroup = GetTakeGroupKey(evt.AnimationName);
                if (string.IsNullOrEmpty(takeGroup) == true)
                {
                    continue;
                }

                if (currentTakeGroup == null)
                {
                    currentTakeGroup = takeGroup;
                    seenTakeGroups.Add(takeGroup);
                }
                else if (string.Equals(currentTakeGroup, takeGroup, StringComparison.OrdinalIgnoreCase) == false &&
                         seenTakeGroups.Contains(takeGroup) == false)
                {
                    timelineOffset += currentGroupEnd;
                    currentGroupEnd = 0.0f;
                    currentTakeGroup = takeGroup;
                    seenTakeGroups.Add(takeGroup);
                }

                var localEnd = evt.EndSeconds;
                evt.StartSeconds += timelineOffset;
                evt.EndSeconds += timelineOffset;
                currentGroupEnd = Math.Max(currentGroupEnd, localEnd);
            }
        }

        private static bool IsAnimationTimelineEvent(CutsceneTimelineEvent evt)
        {
            return evt != null &&
                   (string.Equals(evt.Kind, "PuppetAnimation", StringComparison.OrdinalIgnoreCase) == true ||
                    string.Equals(evt.Kind, "CameraAnimation", StringComparison.OrdinalIgnoreCase) == true ||
                    string.Equals(evt.Kind, "ScriptedCamera", StringComparison.OrdinalIgnoreCase) == true);
        }

        private static string GetTakeGroupKey(string animationName)
        {
            if (string.IsNullOrEmpty(animationName) == true)
            {
                return null;
            }

            var match = Regex.Match(animationName, @"(?:^|_)Take\s+(\d+)\s*$", RegexOptions.IgnoreCase);
            if (match.Success == false)
            {
                return null;
            }

            var take = match.Groups[1].Value;
            if (take.Length <= 1)
            {
                return take;
            }

            return take.Substring(0, take.Length - 1);
        }

        private static Dictionary<ulong, string> BuildAnimationHashLookup(IEnumerable<Animation> animations)
        {
            var result = new Dictionary<ulong, string>();
            foreach (var animation in animations ?? Enumerable.Empty<Animation>())
            {
                if (string.IsNullOrEmpty(animation.Name) == true)
                {
                    continue;
                }

                result[animation.Name.HashX65599()] = animation.Name;
            }

            return result;
        }

        private static Dictionary<ulong, string> BuildActorHashLookup(IEnumerable<BaseNode> nodes, IEnumerable<Animation> animations)
        {
            var result = new Dictionary<ulong, string>();
            foreach (var animation in animations ?? Enumerable.Empty<Animation>())
            {
                var actorName = GuessActorName(animation.Name);
                AddHashLookup(result, actorName);
            }

            foreach (var meta in (nodes ?? Enumerable.Empty<BaseNode>()).OfType<MetaObjectDefinition>())
            {
                AddHashLookup(result, meta.LongName);
                AddHashLookup(result, meta.ShortName);
                AddHashLookup(result, meta.TypeName);
                if (string.IsNullOrEmpty(meta.ShortName) == false &&
                    meta.ShortName.EndsWith("Spawner", StringComparison.OrdinalIgnoreCase) == true)
                {
                    AddHashLookup(result, meta.ShortName.Substring(0, meta.ShortName.Length - "Spawner".Length));
                }
            }

            return result;
        }

        private static void AddHashLookup(IDictionary<ulong, string> lookup, string name)
        {
            if (lookup == null || string.IsNullOrEmpty(name) == true)
            {
                return;
            }

            var hash = name.HashX65599();
            if (lookup.ContainsKey(hash) == false)
            {
                lookup.Add(hash, name);
            }
        }

        private static IEnumerable<CutsceneTimelineEvent> ExtractTimelineEvents(string fightName, byte[] data, IDictionary<ulong, string> animationNamesByHash, IDictionary<ulong, string> actorNamesByHash)
        {
            if (data == null || data.Length < 16)
            {
                yield break;
            }

            for (int offset = 0; offset + 16 <= data.Length; offset++)
            {
                ulong trackHash = ReadUInt64(data, offset);
                if (trackHash == CutsceneTimelineEvent.PuppetAnimationTrackHash)
                {
                    CutsceneTimelineEvent evt;
                    if (TryReadPuppetAnimationTrack(fightName, data, offset, animationNamesByHash, actorNamesByHash, out evt) == true)
                    {
                        yield return evt;
                    }
                }
                else if (trackHash == CutsceneTimelineEvent.CameraAnimationTrackHash)
                {
                    CutsceneTimelineEvent evt;
                    if (TryReadCameraAnimationTrack(fightName, data, offset, animationNamesByHash, out evt) == true)
                    {
                        yield return evt;
                    }
                }
                else if (trackHash == CutsceneTimelineEvent.ScriptedCameraTrackHash)
                {
                    CutsceneTimelineEvent evt;
                    if (TryReadScriptedCameraTrack(fightName, data, offset, animationNamesByHash, out evt) == true)
                    {
                        yield return evt;
                    }
                }
                else if (trackHash == CutsceneTimelineEvent.FmvTrackHash)
                {
                    CutsceneTimelineEvent evt;
                    if (TryReadFmvTrack(fightName, data, offset, out evt) == true)
                    {
                        yield return evt;
                    }
                }
                else if (trackHash == CutsceneTimelineEvent.CameraClippingPlanesTrackHash)
                {
                    CutsceneTimelineEvent evt;
                    if (TryReadCameraClippingPlanesTrack(fightName, data, offset, out evt) == true)
                    {
                        yield return evt;
                    }
                }
                else if (trackHash == CutsceneTimelineEvent.TeleportTrackHash)
                {
                    CutsceneTimelineEvent evt;
                    if (TryReadTeleportTrack(fightName, data, offset, actorNamesByHash, out evt) == true)
                    {
                        yield return evt;
                    }
                }
                else if (trackHash == CutsceneTimelineEvent.SetObjectLightGroupTrackHash)
                {
                    CutsceneTimelineEvent evt;
                    if (TryReadSetObjectLightGroupTrack(fightName, data, offset, actorNamesByHash, out evt) == true)
                    {
                        yield return evt;
                    }
                }
                else if (trackHash == CutsceneTimelineEvent.FadeTrackHash)
                {
                    CutsceneTimelineEvent evt;
                    if (TryReadFadeTrack(fightName, data, offset, out evt) == true)
                    {
                        yield return evt;
                    }
                }
                else if (trackHash == CutsceneTimelineEvent.AttachObjectTrackHash)
                {
                    CutsceneTimelineEvent evt;
                    if (TryReadAttachObjectTrack(fightName, data, offset, actorNamesByHash, out evt) == true)
                    {
                        yield return evt;
                    }
                }
                else if (trackHash == CutsceneTimelineEvent.FxLightPointTrackHash)
                {
                    CutsceneTimelineEvent evt;
                    if (TryReadFxLightPointTrack(fightName, data, offset, actorNamesByHash, out evt) == true)
                    {
                        yield return evt;
                    }
                }
                else if (trackHash == CutsceneTimelineEvent.LightTrackHash)
                {
                    CutsceneTimelineEvent evt;
                    if (TryReadLightTrack(fightName, data, offset, out evt) == true)
                    {
                        yield return evt;
                    }
                }
            }
        }

        private static bool TryReadPuppetAnimationTrack(string fightName, byte[] data, int offset, IDictionary<ulong, string> animationNamesByHash, IDictionary<ulong, string> actorNamesByHash, out CutsceneTimelineEvent evt)
        {
            evt = null;
            uint length;
            int payload;
            if (TryGetTrackPayload(data, offset, 60, out length, out payload) == false)
            {
                return false;
            }

            float start = ReadSingle(data, payload + 4);
            float end = ReadSingle(data, payload + 8);
            if (IsValidTimeRange(start, end) == false)
            {
                return false;
            }

            ulong actorHash = ReadUInt64(data, payload + 12);
            ulong animationHash = ReadUInt64(data, payload + 20);
            var animationName = ResolveAnimationName(animationHash, animationNamesByHash);
            evt = new CutsceneTimelineEvent
            {
                Kind = "PuppetAnimation",
                FightName = fightName,
                StartSeconds = start,
                EndSeconds = end,
                ActorHash = actorHash,
                AnimationHash = animationHash,
                AnimationName = animationName,
                ActorName = ResolveActorName(actorHash, actorNamesByHash) ?? GuessActorName(animationName),
                TrackOffset = offset,
                Speed = ReadSingle(data, payload + 28),
                StartFrame = ReadSingle(data, payload + 36),
                EndFrame = ReadSingle(data, payload + 40),
            };
            return true;
        }

        private static bool TryReadCameraAnimationTrack(string fightName, byte[] data, int offset, IDictionary<ulong, string> animationNamesByHash, out CutsceneTimelineEvent evt)
        {
            evt = null;
            uint length;
            int payload;
            if (TryGetTrackPayload(data, offset, 40, out length, out payload) == false)
            {
                return false;
            }

            float start = ReadSingle(data, payload + 4);
            float end = ReadSingle(data, payload + 8);
            if (IsValidTimeRange(start, end) == false)
            {
                return false;
            }

            ulong animationHash = ReadUInt64(data, payload + 12);
            evt = new CutsceneTimelineEvent
            {
                Kind = "CameraAnimation",
                FightName = fightName,
                StartSeconds = start,
                EndSeconds = end,
                AnimationHash = animationHash,
                AnimationName = ResolveAnimationName(animationHash, animationNamesByHash),
                ActorName = "Camera",
                TrackOffset = offset,
                StartFrame = ReadSingle(data, payload + 24),
                EndFrame = ReadSingle(data, payload + 28),
            };
            return true;
        }

        private static bool TryReadScriptedCameraTrack(string fightName, byte[] data, int offset, IDictionary<ulong, string> animationNamesByHash, out CutsceneTimelineEvent evt)
        {
            evt = null;
            uint length;
            int payload;
            if (TryGetTrackPayload(data, offset, 100, out length, out payload) == false)
            {
                return false;
            }

            float start = ReadSingle(data, payload + 4);
            float end = ReadSingle(data, payload + 8);
            if (IsValidTimeRange(start, end) == false)
            {
                return false;
            }

            ulong animationHash = ReadUInt64(data, payload + 52);
            evt = new CutsceneTimelineEvent
            {
                Kind = "ScriptedCamera",
                FightName = fightName,
                StartSeconds = start,
                EndSeconds = end,
                AnimationHash = animationHash,
                AnimationName = ResolveAnimationName(animationHash, animationNamesByHash),
                ActorName = "Camera",
                TrackOffset = offset,
                Speed = ReadSingle(data, payload + 76),
                StartFrame = ReadSingle(data, payload + 68),
                EndFrame = ReadSingle(data, payload + 72),
            };
            return true;
        }

        private static bool TryReadFmvTrack(string fightName, byte[] data, int offset, out CutsceneTimelineEvent evt)
        {
            evt = null;
            uint length;
            int payload;
            if (TryGetTrackPayload(data, offset, 32, out length, out payload) == false)
            {
                return false;
            }

            float start = ReadSingle(data, payload + 4);
            float end = ReadSingle(data, payload + 8);
            if (IsValidTimeRange(start, end) == false)
            {
                return false;
            }

            string fileName;
            int afterFileName;
            if (TryReadAlignedU32String(data, payload + 12, payload + (int)length, out fileName, out afterFileName) == false)
            {
                return false;
            }

            evt = new CutsceneTimelineEvent
            {
                Kind = "FMV",
                FightName = fightName,
                StartSeconds = start,
                EndSeconds = end,
                AnimationName = fileName,
                ActorName = "FMV",
                TrackOffset = offset,
            };

            if (afterFileName + 44 <= payload + (int)length)
            {
                evt.UseBlackFades = ReadUInt32(data, afterFileName + 12) != 0;
                evt.FadeInOnEnter = ReadUInt32(data, afterFileName + 16) != 0;
                evt.StayFadedOnExit = ReadUInt32(data, afterFileName + 20) != 0;
                evt.FadeUpOnExit = ReadUInt32(data, afterFileName + 24) != 0;
                evt.FadeDuration = ReadSingle(data, afterFileName + 36);
            }
            return true;
        }

        private static bool TryReadCameraClippingPlanesTrack(string fightName, byte[] data, int offset, out CutsceneTimelineEvent evt)
        {
            evt = null;
            uint length;
            int payload;
            if (TryGetTrackPayload(data, offset, 24, out length, out payload) == false)
            {
                return false;
            }

            float start = ReadSingle(data, payload + 4);
            float end = ReadSingle(data, payload + 8);
            if (IsValidTimeRange(start, end) == false)
            {
                return false;
            }

            evt = new CutsceneTimelineEvent
            {
                Kind = "CameraClippingPlanes",
                FightName = fightName,
                StartSeconds = start,
                EndSeconds = end,
                ActorName = "Camera",
                TrackOffset = offset,
                NearClip = ReadSingle(data, payload + 12),
                FarClip = ReadSingle(data, payload + 16),
                RestoreAtEnd = ReadUInt32(data, payload + 20) != 0,
            };
            return true;
        }

        private static bool TryReadTeleportTrack(string fightName, byte[] data, int offset, IDictionary<ulong, string> actorNamesByHash, out CutsceneTimelineEvent evt)
        {
            evt = null;
            uint length;
            int payload;
            if (TryGetTrackPayload(data, offset, 60, out length, out payload) == false)
            {
                return false;
            }

            float start = ReadSingle(data, payload + 4);
            if (start < 0.0f || start > 36000.0f)
            {
                return false;
            }

            ulong actorHash = ReadUInt64(data, payload + 12);
            evt = new CutsceneTimelineEvent
            {
                Kind = "Teleport",
                FightName = fightName,
                StartSeconds = start,
                EndSeconds = start,
                ActorHash = actorHash,
                ActorName = ResolveActorName(actorHash, actorNamesByHash),
                TrackOffset = offset,
                RestoreAtEnd = ReadUInt32(data, payload + 8) != 0,
                SpaceHash = ReadUInt64(data, payload + 20),
                RelatedHash = ReadUInt64(data, payload + 28),
                Offset = ReadVec3(data, payload + 36),
                Orientation = ReadVec3(data, payload + 48),
            };
            return true;
        }

        private static bool TryReadSetObjectLightGroupTrack(
            string fightName,
            byte[] data,
            int offset,
            IDictionary<ulong, string> actorNamesByHash,
            out CutsceneTimelineEvent evt)
        {
            evt = null;
            uint length;
            int payload;
            if (TryGetTrackPayload(data, offset, 20, out length, out payload) == false)
            {
                return false;
            }

            ulong actorHash = 0;
            float start;
            float end;
            ulong groupHash;

            if (length >= 32 && payload + 32 <= data.Length)
            {
                actorHash = ReadUInt64(data, payload + 4);
                start = ReadSingle(data, payload + 16);
                end = ReadSingle(data, payload + 20);
                groupHash = ReadUInt64(data, payload + 24);
                if (actorHash != 0 && groupHash != 0 && IsValidSetObjectLightGroupTime(start, end) == true)
                {
                    evt = new CutsceneTimelineEvent
                    {
                        Kind = "SetObjectLightGroup",
                        FightName = fightName,
                        StartSeconds = start,
                        EndSeconds = end < start ? start : end,
                        RawEndSeconds = end,
                        ActorHash = actorHash,
                        ActorName = ResolveActorName(actorHash, actorNamesByHash),
                        TrackOffset = offset,
                        LightGroupHash = groupHash,
                    };
                    return true;
                }
            }

            start = ReadSingle(data, payload + 4);
            end = ReadSingle(data, payload + 8);
            if (IsValidTimeRange(start, end) == false)
            {
                return false;
            }

            groupHash = ReadUInt64(data, payload + 12);
            evt = new CutsceneTimelineEvent
            {
                Kind = "SetObjectLightGroup",
                FightName = fightName,
                StartSeconds = start,
                EndSeconds = end < start ? start : end,
                RawEndSeconds = end,
                ActorHash = actorHash,
                ActorName = ResolveActorName(actorHash, actorNamesByHash) ?? "LightGroup",
                TrackOffset = offset,
                LightGroupHash = groupHash,
            };
            return true;
        }

        private static bool IsValidSetObjectLightGroupTime(float start, float end)
        {
            if (float.IsNaN(start) == true || float.IsInfinity(start) == true ||
                float.IsNaN(end) == true || float.IsInfinity(end) == true)
            {
                return false;
            }

            if (start < 0.0f || start > 36000.0f)
            {
                return false;
            }

            return end < 0.0f || end >= start && end <= 36000.0f;
        }

        private static bool TryReadFadeTrack(string fightName, byte[] data, int offset, out CutsceneTimelineEvent evt)
        {
            evt = null;
            uint length;
            int payload;
            if (TryGetTrackPayload(data, offset, 20, out length, out payload) == false)
            {
                return false;
            }

            float start = ReadSingle(data, payload + 4);
            float end = ReadSingle(data, payload + 8);
            if (IsValidTimeRange(start, end) == false)
            {
                return false;
            }

            evt = new CutsceneTimelineEvent
            {
                Kind = "Fade",
                FightName = fightName,
                StartSeconds = start,
                EndSeconds = end,
                ActorName = "Camera",
                TrackOffset = offset,
                FadeTypeHash = ReadUInt64(data, payload + 12),
            };
            return true;
        }

        private static bool TryReadAttachObjectTrack(string fightName, byte[] data, int offset, IDictionary<ulong, string> actorNamesByHash, out CutsceneTimelineEvent evt)
        {
            evt = null;
            uint length;
            int payload;
            if (TryGetTrackPayload(data, offset, 100, out length, out payload) == false)
            {
                return false;
            }

            float start = ReadSingle(data, payload + 4);
            if (start < 0.0f || start > 36000.0f)
            {
                return false;
            }

            ulong parentHash = ReadUInt64(data, payload + 12);
            ulong objectHash = ReadUInt64(data, payload + 40);
            evt = new CutsceneTimelineEvent
            {
                Kind = "AttachObject",
                FightName = fightName,
                StartSeconds = start,
                EndSeconds = start + Math.Max(0.0f, ReadSingle(data, payload + 8)),
                ActorHash = objectHash,
                ActorName = ResolveActorName(objectHash, actorNamesByHash),
                TrackOffset = offset,
                RelatedHash = parentHash,
                RelatedJointHash = ReadUInt64(data, payload + 20),
                Offset = ReadVec3(data, payload + 28),
                ChildJointHash = ReadUInt64(data, payload + 48),
                ChildOffset = ReadVec3(data, payload + 56),
                Orientation = ReadVec3(data, payload + 68),
                UsePhysics = ReadUInt32(data, payload + 80) != 0,
            };
            return true;
        }

        private static bool TryReadFxLightPointTrack(string fightName, byte[] data, int offset, IDictionary<ulong, string> actorNamesByHash, out CutsceneTimelineEvent evt)
        {
            evt = null;
            uint length;
            int payload;
            if (TryGetTrackPayload(data, offset, 176, out length, out payload) == false)
            {
                return false;
            }

            float start = ReadSingle(data, payload + 8);
            float end = ReadSingle(data, payload + 12);
            if (IsValidTimeRange(start, end) == false)
            {
                return false;
            }

            ulong parentHash = ReadUInt64(data, payload + 16);
            evt = new CutsceneTimelineEvent
            {
                Kind = "FXLightPoint",
                FightName = fightName,
                StartSeconds = start,
                EndSeconds = end,
                ActorHash = parentHash,
                ActorName = ResolveActorName(parentHash, actorNamesByHash) ?? "Light",
                TrackOffset = offset,
                Wait = ReadSingle(data, payload + 24),
                FadeIn = ReadSingle(data, payload + 28),
                Duration = ReadSingle(data, payload + 32),
                FadeOut = ReadSingle(data, payload + 36),
                LightIntensity = ReadSingle(data, payload + 56),
                LightRadius = ReadSingle(data, payload + 60),
                LightStartColor = ReadColor(data, payload + 104),
                LightEndColor = ReadColor(data, payload + 120),
                SpaceHash = ReadUInt64(data, payload + 136),
                Offset = ReadVec3(data, payload + 144),
                RelatedHash = ReadUInt64(data, payload + 156),
                RelatedJointHash = ReadUInt64(data, payload + 164),
                RestoreAtEnd = ReadUInt32(data, payload + 172) != 0,
            };
            return true;
        }

        private static bool TryReadLightTrack(string fightName, byte[] data, int offset, out CutsceneTimelineEvent evt)
        {
            evt = null;
            uint length;
            int payload;
            if (TryGetTrackPayload(data, offset, 92, out length, out payload) == false)
            {
                return false;
            }

            float start = ReadSingle(data, payload + 4);
            float end = ReadSingle(data, payload + 8);
            if (IsValidTimeRange(start, end) == false)
            {
                return false;
            }

            var range = ReadSingle(data, payload + 88);
            if (range <= 0.001f)
            {
                range = ReadSingle(data, payload + 64);
            }

            evt = new CutsceneTimelineEvent
            {
                Kind = "Light",
                FightName = fightName,
                StartSeconds = start,
                EndSeconds = end,
                ActorName = "Light",
                TrackOffset = offset,
                LightGroupHash = ReadUInt64(data, payload + 12),
                LightStartColor = ReadColor(data, payload + 24),
                LightIntensity = ReadSingle(data, payload + 44),
                LightRadius = range,
            };
            return true;
        }

        private static bool TryGetTrackPayload(byte[] data, int offset, uint minimumLength, out uint length, out int payload)
        {
            length = 0;
            payload = offset + 12;
            if (offset + 12 > data.Length)
            {
                return false;
            }

            length = ReadUInt32(data, offset + 8);
            if (length < minimumLength || length > 4096)
            {
                return false;
            }

            return payload + length <= data.Length;
        }

        private static bool TryReadAlignedU32String(byte[] data, int offset, int endOffset, out string value)
        {
            int nextOffset;
            return TryReadAlignedU32String(data, offset, endOffset, out value, out nextOffset);
        }

        private static bool TryReadAlignedU32String(byte[] data, int offset, int endOffset, out string value, out int nextOffset)
        {
            value = null;
            nextOffset = offset;
            if (offset + 4 > endOffset || endOffset > data.Length)
            {
                return false;
            }

            uint length = ReadUInt32(data, offset);
            int stringOffset = offset + 4;
            if (length > 4096 || stringOffset + length > endOffset)
            {
                return false;
            }

            value = Encoding.ASCII.GetString(data, stringOffset, (int)length).TrimEnd('\0');
            nextOffset = Align4(stringOffset + (int)length);
            return string.IsNullOrEmpty(value) == false;
        }

        private static int Align4(int value)
        {
            return (value + 3) & ~3;
        }

        private static bool IsValidTimeRange(float start, float end)
        {
            return float.IsNaN(start) == false &&
                   float.IsNaN(end) == false &&
                   start >= 0.0f &&
                   end >= start &&
                   end < 100000.0f;
        }

        private static string ResolveAnimationName(ulong animationHash, IDictionary<ulong, string> animationNamesByHash)
        {
            string name;
            if (animationNamesByHash != null && animationNamesByHash.TryGetValue(animationHash, out name) == true)
            {
                return name;
            }

            return animationHash.ToString("X16");
        }

        private static string ResolveActorName(ulong actorHash, IDictionary<ulong, string> actorNamesByHash)
        {
            string name;
            if (actorNamesByHash != null && actorNamesByHash.TryGetValue(actorHash, out name) == true)
            {
                return name;
            }

            return null;
        }

        private static ulong ResolveMetaActorHash(MetaObjectDefinition meta)
        {
            if (meta == null || string.IsNullOrEmpty(meta.ShortName) == true)
            {
                return 0;
            }

            if (string.Equals(meta.TypeName, "ActorSpawner", StringComparison.OrdinalIgnoreCase) == true &&
                meta.ShortName.EndsWith("Spawner", StringComparison.OrdinalIgnoreCase) == true)
            {
                return meta.ShortName.Substring(0, meta.ShortName.Length - "Spawner".Length).HashX65599();
            }

            return meta.ShortName.HashX65599();
        }

        private static string GuessActorName(string animationName)
        {
            if (string.IsNullOrEmpty(animationName) == true)
            {
                return null;
            }

            var name = animationName;
            if (name.StartsWith("PTRN_", StringComparison.OrdinalIgnoreCase) == true)
            {
                name = name.Substring(5);
            }

            var takeIndex = name.IndexOf("_Take", StringComparison.OrdinalIgnoreCase);
            if (takeIndex > 0)
            {
                return name.Substring(0, takeIndex);
            }

            return null;
        }

        private static string ResolveMetaCompositeName(
            MetaObjectDefinition meta,
            IEnumerable<string> strings,
            IEnumerable<string> compositeNames,
            IEnumerable<MetaObjectDefinition> metaObjects,
            IDictionary<MetaObjectDefinition, string[]> metaStrings)
        {
            if (compositeNames == null)
            {
                return null;
            }

            var names = compositeNames.Where(n => string.IsNullOrEmpty(n) == false).ToList();
            if (names.Count == 0)
            {
                return null;
            }

            var candidates = new List<string>();
            if (meta != null)
            {
                candidates.Add(meta.LongName);
                candidates.Add(meta.ShortName);
                candidates.Add(meta.TypeName);
            }

            if (strings != null)
            {
                candidates.AddRange(strings);
                foreach (var referencedMeta in ResolveReferencedMetaObjects(strings, metaObjects))
                {
                    candidates.Add(referencedMeta.LongName);
                    candidates.Add(referencedMeta.ShortName);
                    candidates.Add(referencedMeta.TypeName);
                    string[] referencedStrings;
                    if (metaStrings != null && metaStrings.TryGetValue(referencedMeta, out referencedStrings) == true)
                    {
                        candidates.AddRange(referencedStrings);
                    }
                }
            }

            foreach (var candidate in candidates.Where(c => string.IsNullOrEmpty(c) == false))
            {
                var exact = names.FirstOrDefault(n => string.Equals(n, candidate, StringComparison.OrdinalIgnoreCase) ||
                                                      string.Equals(n + "Template", candidate, StringComparison.OrdinalIgnoreCase));
                if (exact != null)
                {
                    return exact;
                }
            }

            foreach (var candidate in candidates.Where(c => string.IsNullOrEmpty(c) == false))
            {
                var normalizedCandidate = NormalizeName(StripTemplateSuffix(candidate));
                var match = names.FirstOrDefault(n => NormalizeName(n) == normalizedCandidate);
                if (match != null)
                {
                    return match;
                }
            }

            foreach (var candidate in candidates.Where(c => string.IsNullOrEmpty(c) == false))
            {
                var normalizedCandidate = NormalizeName(StripTemplateSuffix(candidate));
                var match = names.FirstOrDefault(n =>
                {
                    var normalizedName = NormalizeName(n);
                    return normalizedName.Contains(normalizedCandidate) || normalizedCandidate.Contains(normalizedName);
                });
                if (match != null)
                {
                    return match;
                }
            }

            return null;
        }

        private static IEnumerable<MetaObjectDefinition> ResolveReferencedMetaObjects(IEnumerable<string> strings, IEnumerable<MetaObjectDefinition> metaObjects)
        {
            if (strings == null || metaObjects == null)
            {
                yield break;
            }

            var metas = metaObjects.ToList();
            foreach (var value in strings.Where(s => string.IsNullOrEmpty(s) == false))
            {
                var normalized = NormalizeName(value);
                var normalizedNoTemplate = NormalizeName(StripTemplateSuffix(value));
                foreach (var meta in metas)
                {
                    if (MetaNameMatches(meta, normalized) == true ||
                        MetaNameMatches(meta, normalizedNoTemplate) == true)
                    {
                        yield return meta;
                    }
                }
            }
        }

        private static bool MetaNameMatches(MetaObjectDefinition meta, string normalizedName)
        {
            if (meta == null || string.IsNullOrEmpty(normalizedName) == true)
            {
                return false;
            }

            return NormalizeName(meta.LongName) == normalizedName ||
                   NormalizeName(meta.ShortName) == normalizedName ||
                   NormalizeName(StripTemplateSuffix(meta.LongName)) == normalizedName ||
                   NormalizeName(StripTemplateSuffix(meta.ShortName)) == normalizedName;
        }

        private static string StripTemplateSuffix(string value)
        {
            if (string.IsNullOrEmpty(value) == false &&
                value.EndsWith("Template", StringComparison.OrdinalIgnoreCase) == true)
            {
                return value.Substring(0, value.Length - "Template".Length);
            }

            return value;
        }

        private static string NormalizeName(string value)
        {
            if (string.IsNullOrEmpty(value) == true)
            {
                return string.Empty;
            }

            var builder = new StringBuilder(value.Length);
            foreach (var c in value)
            {
                if (char.IsLetterOrDigit(c) == true)
                {
                    builder.Append(char.ToLowerInvariant(c));
                }
            }

            return builder.ToString();
        }

        private static float EstimateDurationSeconds(IEnumerable<CutsceneTimelineEvent> events, IEnumerable<Animation> animations, float frameRate)
        {
            float seconds = 0.0f;
            var timelineEvents = events == null ? new CutsceneTimelineEvent[0] : events.ToArray();
            foreach (var evt in timelineEvents.Where(e => string.Equals(e.Kind, "FMV", StringComparison.OrdinalIgnoreCase) == true))
            {
                seconds = Math.Max(seconds, evt.EndSeconds);
            }

            foreach (var evt in seconds > 0.0f ? Enumerable.Empty<CutsceneTimelineEvent>() : timelineEvents)
            {
                seconds = Math.Max(seconds, evt.EndSeconds);
            }

            if (seconds <= 0.0f)
            {
                foreach (var animation in animations ?? Enumerable.Empty<Animation>())
                {
                    var rate = animation.FrameRate > 0.001f ? animation.FrameRate : frameRate;
                    seconds = Math.Max(seconds, animation.NumFrames / Math.Max(0.001f, rate));
                }
            }

            return Math.Max(0.0f, seconds);
        }

        private static uint ReadUInt32(byte[] data, int offset)
        {
            return BitConverter.ToUInt32(data, offset);
        }

        private static ulong ReadUInt64(byte[] data, int offset)
        {
            return BitConverter.ToUInt64(data, offset);
        }

        private static float ReadSingle(byte[] data, int offset)
        {
            return BitConverter.ToSingle(data, offset);
        }

        private static Vec3 ReadVec3(byte[] data, int offset)
        {
            return new Vec3(
                ReadSingle(data, offset + 0),
                ReadSingle(data, offset + 4),
                ReadSingle(data, offset + 8));
        }

        private static Vec3 ReadColor(byte[] data, int offset)
        {
            return new Vec3(
                ReadSingle(data, offset + 0),
                ReadSingle(data, offset + 4),
                ReadSingle(data, offset + 8));
        }

        public static IEnumerable<string> FindReferencedP3DFiles(Pure3DFile figFile, string figFileName)
        {
            if (figFile == null || string.IsNullOrEmpty(figFileName) == true)
            {
                return Enumerable.Empty<string>();
            }

            var folder = Path.GetDirectoryName(figFileName);
            if (string.IsNullOrEmpty(folder) == true || Directory.Exists(folder) == false)
            {
                return Enumerable.Empty<string>();
            }

            var dataStrings = Flatten(figFile.Nodes)
                .OfType<FightData>()
                .Where(d => d.Data != null && d.Data.Length > 0)
                .SelectMany(d => ExtractAsciiStrings(d.Data, 3))
                .ToArray();
            if (dataStrings.Length == 0)
            {
                return Enumerable.Empty<string>();
            }

            var referencedNames = FindReferencedP3DBaseNames(figFile);
            if (referencedNames.Count == 0)
            {
                return Enumerable.Empty<string>();
            }

            var figFullPath = Path.GetFullPath(figFileName);
            return FindReferencedP3DFiles(folder, figFullPath, referencedNames);
        }

        public static HashSet<string> FindReferencedP3DBaseNames(Pure3DFile file)
        {
            var referencedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (file == null)
            {
                return referencedNames;
            }

            var dataStrings = Flatten(file.Nodes)
                .OfType<FightData>()
                .Where(d => d.Data != null && d.Data.Length > 0)
                .SelectMany(d => ExtractAsciiStrings(d.Data, 3))
                .ToArray();
            foreach (var value in dataStrings)
            {
                foreach (var token in TokenizeReferenceString(value))
                {
                    if (token.EndsWith(".p3d", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        referencedNames.Add(Path.GetFileNameWithoutExtension(token));
                        continue;
                    }

                    referencedNames.Add(token);
                }
            }

            return referencedNames;
        }

        public static HashSet<string> FindReferencedCellBaseNames(Pure3DFile file)
        {
            var referencedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (file == null)
            {
                return referencedNames;
            }

            var strings = Flatten(file.Nodes)
                .SelectMany(n =>
                {
                    var fightData = n as FightData;
                    if (fightData != null && fightData.Data != null)
                    {
                        return ExtractAsciiStrings(fightData.Data, 3);
                    }

                    var metaData = n as MetaObjectData;
                    if (metaData != null && metaData.Data != null)
                    {
                        return ExtractMetaObjectStrings(metaData.Data);
                    }

                    return Enumerable.Empty<string>();
                });

            foreach (var value in strings)
            {
                foreach (Match match in Regex.Matches(value ?? string.Empty, @"\bCell_(\d+)", RegexOptions.IgnoreCase))
                {
                    referencedNames.Add("Cell_" + match.Groups[1].Value);
                }
            }

            return referencedNames;
        }

        private static HashSet<string> FindReferencedStageGeometryNames(Pure3DFile file)
        {
            var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (file == null)
            {
                return names;
            }

            var strings = Flatten(file.Nodes)
                .SelectMany(n =>
                {
                    var fightData = n as FightData;
                    if (fightData != null && fightData.Data != null)
                    {
                        return ExtractAsciiStrings(fightData.Data, 3);
                    }

                    var metaData = n as MetaObjectData;
                    if (metaData != null && metaData.Data != null)
                    {
                        return ExtractMetaObjectStrings(metaData.Data);
                    }

                    return Enumerable.Empty<string>();
                });

            foreach (var value in strings)
            {
                foreach (var token in TokenizeReferenceString(value))
                {
                    if (token.IndexOf("mergedDrawableRoot", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        names.Add(token);
                    }
                }
            }

            return names;
        }

        private static IEnumerable<Geometry> FindStageGeometryNodes(IEnumerable<BaseNode> nodes, ISet<string> referencedStageGeometryNames)
        {
            foreach (var geometry in (nodes ?? Enumerable.Empty<BaseNode>()).OfType<Geometry>())
            {
                if (geometry == null || string.IsNullOrEmpty(geometry.Name) == true)
                {
                    continue;
                }

                if (referencedStageGeometryNames != null && referencedStageGeometryNames.Contains(geometry.Name) == true)
                {
                    yield return geometry;
                    continue;
                }

                if (IsDefaultStageGeometryName(geometry.Name) == true)
                {
                    yield return geometry;
                }
            }
        }

        private static bool IsDefaultStageGeometryName(string name)
        {
            return string.Equals(name, "mergedDrawableRootCastShadow", StringComparison.OrdinalIgnoreCase) == true ||
                   string.Equals(name, "mergedDrawableRootNoShadow", StringComparison.OrdinalIgnoreCase) == true ||
                   string.Equals(name, "mergedDrawableRootReceiveShadow", StringComparison.OrdinalIgnoreCase) == true ||
                   string.Equals(name, "mergedDrawableRootNoCastShadow", StringComparison.OrdinalIgnoreCase) == true ||
                   string.Equals(name, "mergedDrawableRootNoReceiveShadow", StringComparison.OrdinalIgnoreCase) == true;
        }

        public static IEnumerable<string> FindReferencedP3DFiles(string folder, string excludedFileName, IEnumerable<string> referencedBaseNames)
        {
            if (string.IsNullOrEmpty(folder) == true || Directory.Exists(folder) == false || referencedBaseNames == null)
            {
                return Enumerable.Empty<string>();
            }

            var referencedNames = new HashSet<string>(referencedBaseNames.Where(n => string.IsNullOrEmpty(n) == false), StringComparer.OrdinalIgnoreCase);
            if (referencedNames.Count == 0)
            {
                return Enumerable.Empty<string>();
            }

            var excludedFullPath = string.IsNullOrEmpty(excludedFileName) == true ? null : Path.GetFullPath(excludedFileName);
            return Directory.GetFiles(folder, "*.p3d")
                .Where(f => string.Equals(Path.GetFullPath(f), excludedFullPath, StringComparison.OrdinalIgnoreCase) == false)
                .Where(f => IsReferencedP3D(Path.GetFileNameWithoutExtension(f), referencedNames))
                .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        public static IEnumerable<string> FindReferencedCellP3DFiles(string sourceFileName, IEnumerable<string> referencedCellBaseNames)
        {
            if (string.IsNullOrEmpty(sourceFileName) == true || referencedCellBaseNames == null)
            {
                return Enumerable.Empty<string>();
            }

            var referencedNames = referencedCellBaseNames.Where(n => string.IsNullOrEmpty(n) == false).ToArray();
            if (referencedNames.Length == 0)
            {
                return Enumerable.Empty<string>();
            }

            var roots = FindCandidateCellRoots(sourceFileName).ToArray();
            if (roots.Length == 0)
            {
                return Enumerable.Empty<string>();
            }

            var results = new List<string>();
            foreach (var root in roots)
            {
                foreach (var fileName in Directory.GetFiles(root, "*.p3d", SearchOption.AllDirectories))
                {
                    var baseName = Path.GetFileNameWithoutExtension(fileName);
                    foreach (var reference in referencedNames)
                    {
                        if (baseName.EndsWith(reference, StringComparison.OrdinalIgnoreCase) == true ||
                            baseName.IndexOf("_" + reference, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            results.Add(fileName);
                            break;
                        }
                    }
                }
            }

            return results.Distinct(StringComparer.OrdinalIgnoreCase)
                          .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
                          .ToArray();
        }

        private static IEnumerable<string> FindCandidateCellRoots(string sourceFileName)
        {
            var directory = Path.GetDirectoryName(Path.GetFullPath(sourceFileName));
            while (string.IsNullOrEmpty(directory) == false && Directory.Exists(directory) == true)
            {
                var cells = Path.Combine(directory, "cells");
                if (Directory.Exists(cells) == true)
                {
                    yield return cells;
                }

                foreach (var child in Directory.GetDirectories(directory, "cells", SearchOption.TopDirectoryOnly))
                {
                    yield return child;
                }

                directory = Directory.GetParent(directory) == null ? null : Directory.GetParent(directory).FullName;
            }
        }

        private static bool IsReferencedP3D(string baseName, IEnumerable<string> referencedBaseNames)
        {
            if (string.IsNullOrEmpty(baseName) == true)
            {
                return false;
            }

            foreach (var reference in referencedBaseNames)
            {
                if (string.IsNullOrEmpty(reference) == true)
                {
                    continue;
                }

                if (string.Equals(baseName, reference, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return true;
                }

                if (baseName.StartsWith(reference, StringComparison.OrdinalIgnoreCase) == true)
                {
                    var suffix = baseName.Substring(reference.Length);
                    if (IsAssociatedCutscenePackageSuffix(suffix) == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsAssociatedCutscenePackageSuffix(string suffix)
        {
            if (string.IsNullOrEmpty(suffix) == true)
            {
                return false;
            }

            if (string.Equals(suffix, "_tod", StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            for (int i = 0; i < suffix.Length; i++)
            {
                if (char.IsDigit(suffix[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        private static IEnumerable<string> TokenizeReferenceString(string value)
        {
            if (string.IsNullOrEmpty(value) == true)
            {
                yield break;
            }

            foreach (Match match in Regex.Matches(value, @"[A-Za-z0-9_.-]+"))
            {
                var token = match.Value.Trim('.');
                if (token.Length >= 3)
                {
                    yield return token;
                }
            }
        }

        private static bool TryReadCutsceneLight(
            MetaObjectDefinition meta,
            byte[] data,
            IEnumerable<string> strings,
            out CutsceneLightObject light)
        {
            light = null;
            if (meta == null)
            {
                return false;
            }

            var position = new Vec3();
            if (TryReadMetaMatrixAttributePosition(data, out position) == false && data != null)
            {
                position = ReadMetaTransformPosition(data);
            }

            var color = new Vec3(1.0f, 1.0f, 1.0f);
            float intensity = 1.0f;
            TryReadLightBehaviour(data, ref color, ref intensity);

            light = new CutsceneLightObject
            {
                LongName = meta.LongName,
                ShortName = meta.ShortName,
                TypeName = meta.TypeName,
                SourceHash = ResolveLightObjectHash(meta),
                Position = position,
                Color = color,
                Intensity = intensity,
                Range = 100.0f,
                DataLength = data == null ? 0 : data.Length,
                Strings = (strings ?? Enumerable.Empty<string>()).Take(16).ToArray(),
            };

            return true;
        }

        private static bool TryReadMetaMatrixAttributePosition(byte[] data, out Vec3 position)
        {
            position = new Vec3();
            if (data == null || data.Length < 64)
            {
                return false;
            }

            var marker = Encoding.ASCII.GetBytes("MatrixAttribute");
            int offset = IndexOf(data, marker);
            if (offset < 0)
            {
                return false;
            }

            offset += marker.Length;
            while (offset < data.Length && data[offset] == 0)
            {
                offset++;
            }

            if (offset < data.Length && data[offset] == 1)
            {
                offset++;
            }

            if (offset + 12 > data.Length)
            {
                return false;
            }

            position = new Vec3(
                ReadSingleBigEndian(data, offset + 0),
                ReadSingleBigEndian(data, offset + 4),
                ReadSingleBigEndian(data, offset + 8));
            return true;
        }

        private static bool TryReadLightBehaviour(byte[] data, ref Vec3 color, ref float intensity)
        {
            if (data == null || data.Length < 32)
            {
                return false;
            }

            var marker = Encoding.ASCII.GetBytes("engine::LightBehaviour");
            int offset = IndexOf(data, marker);
            if (offset < 0)
            {
                return false;
            }

            offset += marker.Length;
            while (offset < data.Length && data[offset] == 0)
            {
                offset++;
            }

            while (offset + 12 <= data.Length)
            {
                var x = ReadSingleBigEndian(data, offset);
                var y = ReadSingleBigEndian(data, offset + 4);
                var z = ReadSingleBigEndian(data, offset + 8);
                if (IsUsableLightScalar(x) == true &&
                    IsUsableLightScalar(y) == true &&
                    IsUsableLightScalar(z) == true)
                {
                    color = new Vec3(x, y, z);
                    if (offset + 16 <= data.Length)
                    {
                        var w = ReadSingleBigEndian(data, offset + 12);
                        if (IsUsableLightScalar(w) == true)
                        {
                            intensity = Math.Max(0.05f, w);
                        }
                    }

                    return true;
                }

                offset++;
            }

            return false;
        }

        private static bool IsUsableLightScalar(float value)
        {
            return float.IsNaN(value) == false &&
                   float.IsInfinity(value) == false &&
                   value >= 0.0f &&
                   value <= 32.0f;
        }

        private static int IndexOf(byte[] data, byte[] pattern)
        {
            if (data == null || pattern == null || pattern.Length == 0 || data.Length < pattern.Length)
            {
                return -1;
            }

            for (int i = 0; i <= data.Length - pattern.Length; i++)
            {
                bool matches = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (data[i + j] != pattern[j])
                    {
                        matches = false;
                        break;
                    }
                }

                if (matches == true)
                {
                    return i;
                }
            }

            return -1;
        }

        private static void AssignCutsceneLightGroups(CutscenePreview preview)
        {
            if (preview == null || preview.Lights == null || preview.Actors == null)
            {
                return;
            }

            foreach (var evt in preview.TimelineEvents
                .Where(e => string.Equals(e.Kind, "SetObjectLightGroup", StringComparison.OrdinalIgnoreCase) == true &&
                            e.ActorHash != 0 &&
                            e.LightGroupHash != 0))
            {
                var actor = preview.Actors.FirstOrDefault(a => a.ActorHash == evt.ActorHash);
                if (actor == null)
                {
                    continue;
                }

                actor.LightGroupHash = evt.LightGroupHash;
                actor.LightGroupName = ResolveKnownLightGroupName(evt.LightGroupHash, actor);
            }

            foreach (var light in preview.Lights)
            {
                if (light == null)
                {
                    continue;
                }

                var targetActor = ResolveTargetActorNameFromStrings(light.Strings);
                if (string.IsNullOrEmpty(targetActor) == false)
                {
                    var actor = preview.Actors.FirstOrDefault(a =>
                        string.Equals(a.ShortName, targetActor, StringComparison.OrdinalIgnoreCase) == true ||
                        string.Equals(a.LongName, targetActor, StringComparison.OrdinalIgnoreCase) == true ||
                        string.Equals(a.CompositeName, targetActor, StringComparison.OrdinalIgnoreCase) == true);
                    if (actor != null)
                    {
                        light.TargetActorName = string.IsNullOrEmpty(actor.ShortName) == false ? actor.ShortName : actor.LongName;
                        light.TargetActorHash = actor.ActorHash;
                        light.TargetCompositeName = actor.CompositeName;
                        light.LightGroupHash = actor.LightGroupHash;
                        light.LightGroupName = actor.LightGroupName;
                    }
                }

                var groupName = ResolveLightGroupNameFromStrings(light.Strings);
                if (string.IsNullOrEmpty(groupName) == false)
                {
                    light.LightGroupName = groupName;
                    light.LightGroupHash = groupName.HashX65599();
                }
            }
        }

        private static string ResolveKnownLightGroupName(ulong hash, CutsceneActorObject actor)
        {
            if (actor != null)
            {
                foreach (var name in new[] { actor.ShortName, actor.LongName, actor.CompositeName })
                {
                    if (string.IsNullOrEmpty(name) == true)
                    {
                        continue;
                    }

                    var candidate = StripTemplateSuffix(name) + "LightGroup";
                    if (candidate.HashX65599() == hash)
                    {
                        return candidate;
                    }
                }
            }

            return hash.ToString("X16");
        }

        private static string ResolveTargetActorNameFromStrings(IEnumerable<string> strings)
        {
            return (strings ?? Enumerable.Empty<string>())
                .FirstOrDefault(s => string.IsNullOrEmpty(s) == false &&
                                     (s.IndexOf("TargetActor", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                      s.IndexOf("TargetObject", StringComparison.OrdinalIgnoreCase) >= 0));
        }

        private static string ResolveLightGroupNameFromStrings(IEnumerable<string> strings)
        {
            return (strings ?? Enumerable.Empty<string>())
                .FirstOrDefault(s => string.IsNullOrEmpty(s) == false &&
                                     s.IndexOf("LightGroup", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static bool LooksLikeActor(MetaObjectDefinition meta)
        {
            var text = ((meta.LongName ?? "") + " " + (meta.ShortName ?? "") + " " + (meta.TypeName ?? "")).ToLowerInvariant();
            return text.Contains("actor") || text.Contains("spawner") || text.Contains("character") || text.Contains("ped");
        }

        private static bool LooksLikeLight(MetaObjectDefinition meta)
        {
            var text = ((meta.LongName ?? "") + " " + (meta.ShortName ?? "") + " " + (meta.TypeName ?? "")).ToLowerInvariant();
            return text.Contains("light");
        }

        private static bool LooksLikeProp(MetaObjectDefinition meta)
        {
            var text = ((meta.LongName ?? "") + " " + (meta.ShortName ?? "") + " " + (meta.TypeName ?? "")).ToLowerInvariant();
            return text.Contains("propspawner");
        }

        private static bool TryReadPropSpawner(MetaObjectDefinition meta, byte[] data, IEnumerable<string> strings, out CutscenePropObject prop)
        {
            prop = null;
            if (meta == null || data == null || data.Length < 32)
            {
                return false;
            }

            prop = new CutscenePropObject
            {
                LongName = meta.LongName,
                ShortName = meta.ShortName,
                TypeName = meta.TypeName,
                ActorName = ResolvePropObjectName(meta),
                ActorHash = ResolvePropObjectHash(meta),
                TemplateName = (strings ?? Enumerable.Empty<string>())
                    .FirstOrDefault(s => string.IsNullOrEmpty(s) == false && s.EndsWith("Template", StringComparison.OrdinalIgnoreCase)),
                Position = ReadMetaTransformPosition(data),
                RotationX = ReadSingleBigEndian(data, 16),
                RotationY = ReadSingleBigEndian(data, 20),
                RotationZ = ReadSingleBigEndian(data, 24),
                RotationW = ReadSingleBigEndian(data, 28),
                DataLength = data.Length,
                Strings = (strings ?? Enumerable.Empty<string>()).Take(16).ToArray(),
            };
            return true;
        }

        private static ulong ResolvePropObjectHash(MetaObjectDefinition meta)
        {
            var name = ResolvePropObjectName(meta);
            return string.IsNullOrEmpty(name) == true ? 0 : name.HashX65599();
        }

        private static ulong ResolveLightObjectHash(MetaObjectDefinition meta)
        {
            if (meta == null)
            {
                return 0;
            }

            if (string.IsNullOrEmpty(meta.ShortName) == false)
            {
                return meta.ShortName.HashX65599();
            }

            return string.IsNullOrEmpty(meta.LongName) == true ? 0 : meta.LongName.HashX65599();
        }

        private static string ResolvePropObjectName(MetaObjectDefinition meta)
        {
            if (meta == null || string.IsNullOrEmpty(meta.ShortName) == true)
            {
                return null;
            }

            return meta.ShortName.EndsWith("Spawner", StringComparison.OrdinalIgnoreCase) == true
                       ? meta.ShortName.Substring(0, meta.ShortName.Length - "Spawner".Length)
                       : meta.ShortName;
        }

        private static Vec3 ReadMetaTransformPosition(byte[] data)
        {
            return new Vec3(ReadSingleBigEndian(data, 4), ReadSingleBigEndian(data, 8), ReadSingleBigEndian(data, 12));
        }

        private static float ReadSingleBigEndian(byte[] data, int offset)
        {
            if (data == null || offset < 0 || offset + 4 > data.Length)
            {
                return 0.0f;
            }

            var bytes = new[] { data[offset + 3], data[offset + 2], data[offset + 1], data[offset + 0] };
            return BitConverter.ToSingle(bytes, 0);
        }

        private static IEnumerable<BaseNode> Flatten(IEnumerable<BaseNode> nodes)
        {
            if (nodes == null)
            {
                yield break;
            }

            foreach (var node in nodes)
            {
                yield return node;
                foreach (var child in Flatten(node.Children))
                {
                    yield return child;
                }
            }
        }

        private static IEnumerable<string> ExtractAsciiStrings(byte[] data, int minLength)
        {
            if (data == null || data.Length == 0)
            {
                yield break;
            }

            var builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                var b = data[i];
                if (b >= 32 && b <= 126)
                {
                    builder.Append((char)b);
                    continue;
                }

                if (builder.Length >= minLength)
                {
                    yield return builder.ToString();
                }
                builder.Length = 0;
            }

            if (builder.Length >= minLength)
            {
                yield return builder.ToString();
            }
        }

        private static IEnumerable<string> ExtractMetaObjectStrings(byte[] data)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var value in ExtractAsciiStrings(data, 4))
            {
                var clean = CleanMetaObjectString(value);
                if (string.IsNullOrEmpty(clean) == false && seen.Add(clean) == true)
                {
                    yield return clean;
                }
            }

            if (data == null)
            {
                yield break;
            }

            for (int offset = 0; offset < data.Length; offset++)
            {
                int length = data[offset];
                if (length < 3 || offset + 1 + length > data.Length)
                {
                    continue;
                }

                bool valid = true;
                for (int i = 0; i < length; i++)
                {
                    var b = data[offset + 1 + i];
                    if (b < 32 || b > 126)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid == false)
                {
                    continue;
                }

                var clean = CleanMetaObjectString(Encoding.ASCII.GetString(data, offset + 1, length));
                if (string.IsNullOrEmpty(clean) == false && seen.Add(clean) == true)
                {
                    yield return clean;
                }
            }
        }

        private static string CleanMetaObjectString(string value)
        {
            if (string.IsNullOrEmpty(value) == true)
            {
                return null;
            }

            var tokens = TokenizeReferenceString(value).ToArray();
            if (tokens.Length == 0)
            {
                return null;
            }

            return tokens.OrderByDescending(t => t.Length).FirstOrDefault();
        }
    }

    internal sealed class CutsceneFightTrack
    {
        public string Name { get; set; }
        public string Context { get; set; }
        public int DataLength { get; set; }

        [Browsable(false)]
        public string[] Strings { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? "Fight" : this.Name;
        }
    }

    internal sealed class CutsceneActorObject
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public string TypeName { get; set; }
        public string CompositeName { get; set; }
        public ulong ActorHash { get; set; }
        public ulong LightGroupHash { get; set; }
        public string LightGroupName { get; set; }
        public Vec3 Position { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float RotationW { get; set; }
        public int DataLength { get; set; }

        [Browsable(false)]
        public string[] Strings { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.LongName) ? this.TypeName : this.LongName;
        }
    }

    internal sealed class CutscenePropObject
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public string TypeName { get; set; }
        public string ActorName { get; set; }
        public ulong ActorHash { get; set; }
        public string TemplateName { get; set; }
        public Vec3 Position { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float RotationW { get; set; }
        public int DataLength { get; set; }

        [Browsable(false)]
        public string[] Strings { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.ShortName) ? this.TypeName : this.ShortName;
        }
    }

    internal sealed class CutsceneCameraObject
    {
        public string Name { get; set; }
        public float Fov { get; set; }
        public float NearClip { get; set; }
        public float FarClip { get; set; }
        public Vec3 Position { get; set; }
        public Vec3 Look { get; set; }
        public Vec3 Up { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? "Camera" : this.Name;
        }
    }

    internal sealed class CutsceneLightObject
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public string TypeName { get; set; }
        public ulong SourceHash { get; set; }
        public ulong LightGroupHash { get; set; }
        public string LightGroupName { get; set; }
        public string TargetActorName { get; set; }
        public ulong TargetActorHash { get; set; }
        public string TargetCompositeName { get; set; }
        public Vec3 Position { get; set; }
        public Vec3 Color { get; set; }
        public float Intensity { get; set; }
        public float Range { get; set; }
        public int DataLength { get; set; }

        [Browsable(false)]
        public string[] Strings { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.ShortName) ? this.TypeName : this.ShortName;
        }
    }

    internal sealed class CutsceneTimelineEvent
    {
        public const ulong PuppetAnimationTrackHash = 4673390682830472570UL;
        public const ulong CameraAnimationTrackHash = 8497939544312814465UL;
        public const ulong ScriptedCameraTrackHash = 15069011368341203315UL;
        public const ulong FmvTrackHash = 301230017567UL;
        public const ulong CameraClippingPlanesTrackHash = 3734088931403766258UL;
        public const ulong TeleportTrackHash = 17255156770509568857UL;
        public const ulong SetObjectLightGroupTrackHash = 547266019841551441UL;
        public const ulong FadeTrackHash = 28793743034745722UL;
        public const ulong AttachObjectTrackHash = 14657385042073036494UL;
        public const ulong FxLightPointTrackHash = 3121189187025458833UL;
        public const ulong LightTrackHash = 7701316379744913464UL;

        public string Kind { get; set; }
        public string FightName { get; set; }
        public string ActorName { get; set; }
        public float StartSeconds { get; set; }
        public float EndSeconds { get; set; }
        public float RawEndSeconds { get; set; }
        public ulong ActorHash { get; set; }
        public ulong AnimationHash { get; set; }
        public string AnimationName { get; set; }
        public float Speed { get; set; }
        public float StartFrame { get; set; }
        public float EndFrame { get; set; }
        public int TrackOffset { get; set; }
        public float NearClip { get; set; }
        public float FarClip { get; set; }
        public bool RestoreAtEnd { get; set; }
        public ulong RelatedHash { get; set; }
        public ulong RelatedJointHash { get; set; }
        public ulong ChildJointHash { get; set; }
        public ulong SpaceHash { get; set; }
        public ulong LightGroupHash { get; set; }
        public ulong FadeTypeHash { get; set; }
        public Vec3 Offset { get; set; }
        public Vec3 ChildOffset { get; set; }
        public Vec3 Orientation { get; set; }
        public bool UsePhysics { get; set; }
        public bool UseBlackFades { get; set; }
        public bool FadeInOnEnter { get; set; }
        public bool StayFadedOnExit { get; set; }
        public bool FadeUpOnExit { get; set; }
        public float Wait { get; set; }
        public float FadeIn { get; set; }
        public float Duration { get; set; }
        public float FadeOut { get; set; }
        public float FadeDuration { get; set; }
        public float LightIntensity { get; set; }
        public float LightRadius { get; set; }
        public Vec3 LightStartColor { get; set; }
        public Vec3 LightEndColor { get; set; }

        public override string ToString()
        {
            return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "{0} {1:0.###}-{2:0.###} {3}",
                this.Kind,
                this.StartSeconds,
                this.EndSeconds,
                this.AnimationName);
        }
    }
}
