using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Gibbed.Prototype.FileFormats;
using Gibbed.Prototype.FileFormats.Pure3D;
using Gibbed.Prototype.FileFormats.Pure3D.BoneData;

namespace Gibbed.Prototype.Edit3D
{
    internal static class GltfAnimationImporter
    {
        public static int ImportInto(Animation target, string fileName, AnimationCompressionPreset preset)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            var document = Load(fileName);
            var animation = document.Animations.FirstOrDefault();
            if (animation == null || animation.Channels.Count == 0)
            {
                throw new InvalidDataException("The GLTF file does not contain animation channels.");
            }

            var existingBones = Flatten(new[] { target }).OfType<Bone>()
                .GroupBy(b => b.Name ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
            var imported = new Dictionary<string, ImportedBoneAnimation>(StringComparer.OrdinalIgnoreCase);
            float maxTime = 0.0f;

            foreach (var channel in animation.Channels)
            {
                if (channel.Sampler < 0 || channel.Sampler >= animation.Samplers.Count ||
                    channel.TargetNode < 0 || channel.TargetNode >= document.Nodes.Count)
                {
                    continue;
                }

                var nodeName = document.Nodes[channel.TargetNode].Name;
                if (string.IsNullOrEmpty(nodeName) == true)
                {
                    continue;
                }

                var sampler = animation.Samplers[channel.Sampler];
                var times = document.ReadAccessorFloats(sampler.Input);
                var values = document.ReadAccessorFloats(sampler.Output);
                if (times.Length == 0 || values.Length == 0)
                {
                    continue;
                }

                maxTime = Math.Max(maxTime, times.Max());
                ImportedBoneAnimation bone;
                if (imported.TryGetValue(nodeName, out bone) == false)
                {
                    bone = new ImportedBoneAnimation { Name = nodeName };
                    imported.Add(nodeName, bone);
                }

                if (string.Equals(channel.TargetPath, "rotation", StringComparison.OrdinalIgnoreCase) == true)
                {
                    bone.RotationTimes = times;
                    bone.Rotations = ToQuaternions(values);
                }
                else if (string.Equals(channel.TargetPath, "translation", StringComparison.OrdinalIgnoreCase) == true)
                {
                    bone.TranslationTimes = times;
                    bone.Translations = ToVec3s(values);
                }
            }

            if (imported.Count == 0)
            {
                throw new InvalidDataException("No GLTF animation channels matched named nodes.");
            }

            var frameRate = target.FrameRate > 0.001f ? target.FrameRate : 30.0f;
            var importedFrameCount = Math.Max(1, (int)Math.Ceiling(maxTime * frameRate) + 1);
            DecodeExternalAnimationFrames(target);
            target.FrameRate = frameRate;
            target.NumFrames = importedFrameCount;
            if (target.AnimationType == null || string.IsNullOrEmpty(target.AnimationType.ToString()) == true)
            {
                target.AnimationType = "PTRN";
            }

            var animationGroup = target.Children.OfType<AnimationGroup>().FirstOrDefault();
            if (animationGroup == null)
            {
                animationGroup = new AnimationGroup();
            }

            var nonAnimationDataChildren = target.Children.Where(c => !(c is Bone) && !(c is AnimationData)).ToList();
            if (nonAnimationDataChildren.Contains(animationGroup) == false)
            {
                nonAnimationDataChildren.Add(animationGroup);
            }

            var outputBones = preset == AnimationCompressionPreset.AlexPlayer
                                  ? BuildAlexPlayerBones(animationGroup, imported, document, frameRate, importedFrameCount, preset)
                                  : BuildSparseBones(target, existingBones, imported, frameRate, preset);

            animationGroup.NumGroups = (uint)outputBones.Count;
            animationGroup.Children.Clear();
            animationGroup.Children.AddRange(outputBones.Cast<BaseNode>());
            foreach (var bone in outputBones)
            {
                bone.ParentNode = animationGroup;
            }

            foreach (var animationHeader in nonAnimationDataChildren.OfType<U00121006>())
            {
                animationHeader.Unknown2 = (uint)outputBones.Count;
                if (preset == AnimationCompressionPreset.AlexPlayer)
                {
                    RebuildInlineAnimationHeader(animationHeader, outputBones);
                }
            }

            target.Children.Clear();
            target.Children.AddRange(nonAnimationDataChildren);
            foreach (var child in target.Children)
            {
                child.ParentNode = target;
            }

            return outputBones.Count;
        }

        private static List<Bone> BuildSparseBones(
            Animation target,
            Dictionary<string, Bone> existingBones,
            Dictionary<string, ImportedBoneAnimation> imported,
            float frameRate,
            AnimationCompressionPreset preset)
        {
            var outputBones = new List<Bone>();
            foreach (var importedBone in imported.Values.OrderBy(b => GetExistingBoneOrder(target, b.Name)))
            {
                Bone oldBone;
                existingBones.TryGetValue(importedBone.Name, out oldBone);
                var bone = new Bone
                {
                    Version = oldBone == null ? 0 : oldBone.Version,
                    Name = importedBone.Name,
                    GroupId = oldBone == null ? 0 : oldBone.GroupId,
                };

                if (importedBone.Rotations != null && importedBone.Rotations.Length > 0)
                {
                    var rotation = CreateRotationChannel(importedBone, frameRate, preset);
                    if (rotation != null)
                    {
                        bone.Children.Add(rotation);
                    }
                }

                if (importedBone.Translations != null && importedBone.Translations.Length > 0)
                {
                    var translation = CreateTranslationChannel(importedBone, frameRate, preset);
                    if (translation != null)
                    {
                        bone.Children.Add(translation);
                    }
                }

                bone.NumChannels = (uint)bone.Children.Count;
                foreach (var child in bone.Children)
                {
                    RemoveAnimationDataReferences(child);
                    child.ParentNode = bone;
                }

                if (bone.NumChannels > 0)
                {
                    outputBones.Add(bone);
                }
            }

            return outputBones;
        }

        private static List<Bone> BuildAlexPlayerBones(
            AnimationGroup animationGroup,
            Dictionary<string, ImportedBoneAnimation> imported,
            GltfDocument document,
            float frameRate,
            int importedFrameCount,
            AnimationCompressionPreset preset)
        {
            var sourceBones = animationGroup.Children.OfType<Bone>().ToList();
            var outputBones = new List<Bone>();
            foreach (var sourceBone in sourceBones)
            {
                ImportedBoneAnimation importedBone;
                imported.TryGetValue(sourceBone.Name ?? string.Empty, out importedBone);

                var bone = new Bone
                {
                    Version = sourceBone.Version,
                    Name = sourceBone.Name,
                    GroupId = sourceBone.GroupId,
                };

                foreach (var sourceChannel in sourceBone.Children.OfType<AnimationChannel>())
                {
                    var type = (sourceChannel.TranslationType ?? string.Empty).TrimEnd('\0');
                    BaseNode channel = null;
                    if (string.Equals(type, "ROT", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        var rotation = importedBone != null && importedBone.Rotations != null
                                           ? importedBone
                                           : CreateStaticRotation(sourceBone.Name, document, sourceChannel, importedFrameCount, frameRate);
                        channel = CreateRotationChannel(rotation, frameRate, preset, sourceChannel.GetType(), false);
                    }
                    else if (string.Equals(type, "TRAN", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        var translation = importedBone != null && importedBone.Translations != null
                                              ? importedBone
                                              : CreateFallbackTranslation(sourceBone.Name, document, sourceChannel, importedFrameCount, frameRate);
                        channel = CreateTranslationChannel(translation, frameRate, preset, sourceChannel.GetType(), false);
                    }

                    if (channel != null)
                    {
                        channel.ParentNode = bone;
                        RemoveAnimationDataReferences(channel);
                        bone.Children.Add(channel);
                    }
                }

                bone.NumChannels = (uint)bone.Children.Count;
                outputBones.Add(bone);
            }

            return outputBones;
        }

        private static BaseNode CreateRotationChannel(ImportedBoneAnimation bone, float frameRate, AnimationCompressionPreset preset)
        {
            return CreateRotationChannel(bone, frameRate, preset, null, true);
        }

        private static BaseNode CreateRotationChannel(ImportedBoneAnimation bone, float frameRate, AnimationCompressionPreset preset, Type preferredChannelType, bool skipStatic)
        {
            var frames = BuildFrameKeys(bone.RotationTimes, frameRate, GetKeyStride(preset));
            var values = BuildFrameValues(frames, bone.RotationTimes, bone.Rotations, frameRate, true);
            if (skipStatic == true && HasAnimatedValues(values, true) == false)
            {
                return null;
            }

            if (preset == AnimationCompressionPreset.Cutscenes)
            {
                return new BoneRotationData
                {
                    Version = 0,
                    Param = "ROT\0",
                    NumFrames = (uint)frames.Length,
                    Frames = frames,
                    Values = values.Select(v => new Quaternion { X = v.X, Y = v.Y, Z = v.Z, W = v.W }).ToArray(),
                };
            }

            AnimationChannel channel = CreateAnimationChannel(preferredChannelType) ??
                                       (preset == AnimationCompressionPreset.Pedestrians
                                            ? (AnimationChannel)new Quaternion3CompressedChannel()
                                            : new Quaternion6CompressedChannel());
            FillChannel(channel, "ROT\0", frames, values);
            return channel;
        }

        private static BaseNode CreateTranslationChannel(ImportedBoneAnimation bone, float frameRate, AnimationCompressionPreset preset)
        {
            return CreateTranslationChannel(bone, frameRate, preset, null, true);
        }

        private static BaseNode CreateTranslationChannel(ImportedBoneAnimation bone, float frameRate, AnimationCompressionPreset preset, Type preferredChannelType, bool skipStatic)
        {
            var frames = BuildFrameKeys(bone.TranslationTimes, frameRate, GetKeyStride(preset));
            var values = BuildFrameValues(frames, bone.TranslationTimes, bone.Translations, frameRate, false);
            if (skipStatic == true && HasAnimatedValues(values, false) == false)
            {
                return null;
            }

            AnimationChannel channel = CreateAnimationChannel(preferredChannelType) ??
                                       (preset == AnimationCompressionPreset.Cutscenes
                                            ? (AnimationChannel)new Vector3DOFChannel()
                                            : new Vector3DOFCompressedChannel());
            FillChannel(channel, "TRAN", frames, values);
            return channel;
        }

        private static AnimationChannel CreateAnimationChannel(Type type)
        {
            if (type == typeof(Quaternion6CompressedChannel))
            {
                return new Quaternion6CompressedChannel();
            }

            if (type == typeof(Quaternion3CompressedChannel))
            {
                return new Quaternion3CompressedChannel();
            }

            if (type == typeof(Vector3DOFCompressedChannel))
            {
                return new Vector3DOFCompressedChannel();
            }

            if (type == typeof(Vector2DOFCompressedChannel))
            {
                return new Vector2DOFCompressedChannel();
            }

            if (type == typeof(Vector1DOFCompressedChannel))
            {
                return new Vector1DOFCompressedChannel();
            }

            if (type == typeof(Vector3DOFChannel))
            {
                return new Vector3DOFChannel();
            }

            if (type == typeof(Vector2DOFChannel))
            {
                return new Vector2DOFChannel();
            }

            if (type == typeof(Vector1DOFChannel))
            {
                return new Vector1DOFChannel();
            }

            return null;
        }

        private static ImportedBoneAnimation CreateStaticRotation(string boneName, GltfDocument document, AnimationChannel sourceChannel, int frameCount, float frameRate)
        {
            Vector4 rotation;
            if (TryGetSourceInitialValue(sourceChannel, true, out rotation) == false)
            {
                rotation = document.GetNodeRotation(boneName);
            }

            return new ImportedBoneAnimation
            {
                Name = boneName,
                RotationTimes = CreateStaticTimes(frameCount, frameRate),
                Rotations = CreateStaticValues(rotation, frameCount),
            };
        }

        private static ImportedBoneAnimation CreateFallbackTranslation(string boneName, GltfDocument document, AnimationChannel sourceChannel, int frameCount, float frameRate)
        {
            if (ShouldPreserveSourceTranslation(boneName) == true && sourceChannel != null && sourceChannel.Frames != null && sourceChannel.Frames.Count > 0)
            {
                return CreateSampledSourceTranslation(boneName, sourceChannel, frameCount, frameRate);
            }

            return CreateStaticTranslation(boneName, document, sourceChannel, frameCount, frameRate);
        }

        private static ImportedBoneAnimation CreateStaticTranslation(string boneName, GltfDocument document, AnimationChannel sourceChannel, int frameCount, float frameRate)
        {
            Vector4 translation;
            if (TryGetSourceInitialValue(sourceChannel, false, out translation) == false)
            {
                translation = document.GetNodeTranslation(boneName);
            }

            return new ImportedBoneAnimation
            {
                Name = boneName,
                TranslationTimes = CreateStaticTimes(frameCount, frameRate),
                Translations = CreateStaticValues(translation, frameCount),
            };
        }

        private static ImportedBoneAnimation CreateSampledSourceTranslation(string boneName, AnimationChannel sourceChannel, int frameCount, float frameRate)
        {
            frameCount = Math.Max(1, frameCount);
            var times = new float[frameCount];
            var values = new Vector4[frameCount];
            var maxSourceFrame = sourceChannel.OrderedFrameKeys.Length == 0 ? 0 : sourceChannel.OrderedFrameKeys[sourceChannel.OrderedFrameKeys.Length - 1];
            for (int i = 0; i < frameCount; i++)
            {
                times[i] = i / frameRate;
                values[i] = sourceChannel.CalculateValue(Math.Min(i, maxSourceFrame));
            }

            return new ImportedBoneAnimation
            {
                Name = boneName,
                TranslationTimes = times,
                Translations = values,
            };
        }

        private static bool ShouldPreserveSourceTranslation(string boneName)
        {
            return string.Equals(boneName, "Motion_Root", StringComparison.OrdinalIgnoreCase) == true ||
                   string.Equals(boneName, "Character_Root", StringComparison.OrdinalIgnoreCase) == true ||
                   string.Equals(boneName, "L_Wrist_Grapple", StringComparison.OrdinalIgnoreCase) == true ||
                   string.Equals(boneName, "R_Wrist_Grapple", StringComparison.OrdinalIgnoreCase) == true ||
                   string.Equals(boneName, "leg_left", StringComparison.OrdinalIgnoreCase) == true ||
                   string.Equals(boneName, "leg_right", StringComparison.OrdinalIgnoreCase) == true ||
                   string.Equals(boneName, "arm_left", StringComparison.OrdinalIgnoreCase) == true ||
                   string.Equals(boneName, "arm_right", StringComparison.OrdinalIgnoreCase) == true;
        }

        private static bool TryGetSourceInitialValue(AnimationChannel sourceChannel, bool normalize, out Vector4 result)
        {
            result = null;
            if (sourceChannel == null || sourceChannel.Frames == null || sourceChannel.Frames.Count == 0)
            {
                return false;
            }

            var keys = sourceChannel.OrderedFrameKeys;
            if (keys == null || keys.Length == 0)
            {
                return false;
            }

            var value = sourceChannel.CalculateValue(keys[0]);
            result = normalize == true ? Normalize(value) : value;
            return true;
        }

        private static float[] CreateStaticTimes(int frameCount, float frameRate)
        {
            frameCount = Math.Max(1, frameCount);
            if (frameCount == 1)
            {
                return new[] { 0.0f };
            }

            return new[] { 0.0f, (frameCount - 1) / frameRate };
        }

        private static Vector4[] CreateStaticValues(Vector4 value, int frameCount)
        {
            if (frameCount <= 1)
            {
                return new[] { value };
            }

            return new[] { value, value };
        }

        private static int GetKeyStride(AnimationCompressionPreset preset)
        {
            switch (preset)
            {
                case AnimationCompressionPreset.Soldiers:
                    return 2;
                case AnimationCompressionPreset.Pedestrians:
                    return 4;
                default:
                    return 1;
            }
        }

        private static void FillChannel(AnimationChannel channel, string type, ushort[] frames, Vector4[] values)
        {
            channel.Version = 0;
            channel.TranslationType = type;
            channel.NumberOfFrames = (uint)frames.Length;
            channel.FrameOrder = frames;
            channel.Frames = new Dictionary<ushort, Vector4>();
            for (int i = 0; i < frames.Length; i++)
            {
                channel.Frames[frames[i]] = values[i];
            }

            var interpolation = new BoneInterpolation
            {
                Version = 0,
                Interpolate = uint.MaxValue,
            };
            interpolation.ParentNode = channel;
            channel.Children.Add(interpolation);
        }

        private static void RemoveAnimationDataReferences(BaseNode node)
        {
            if (node == null)
            {
                return;
            }

            foreach (var child in node.Children.OfType<AnimationDataReference>().ToList())
            {
                node.Children.Remove(child);
            }

            foreach (var child in node.Children.ToList())
            {
                RemoveAnimationDataReferences(child);
            }
        }

        private static void RebuildInlineAnimationHeader(U00121006 header, IEnumerable<Bone> bones)
        {
            if (header == null)
            {
                return;
            }

            var children = new List<BaseNode>();
            var marker = header.Children.FirstOrDefault(c => c.TypeId == 0x00121400);
            if (marker == null)
            {
                marker = new Unknown(0x00121400)
                {
                    Data = new byte[] { 0, 0, 0, 0, 4, 0, 0, 0 },
                };
            }

            marker.ParentNode = header;
            children.Add(marker);

            var channelsByType = bones.SelectMany(b => b.Children.OfType<AnimationChannel>())
                .GroupBy(c => c.TypeId)
                .OrderBy(g => g.Key);
            foreach (var group in channelsByType)
            {
                var data = new List<byte>();
                AppendU32(data, 0);
                AppendU32(data, group.Key);
                AppendU32(data, (uint)group.Count());
                foreach (var channel in group)
                {
                    AppendU16(data, (ushort)Math.Min(ushort.MaxValue, channel.NumberOfFrames));
                }

                children.Add(new Unknown(0x00121007)
                {
                    Data = data.ToArray(),
                    ParentNode = header,
                });
            }

            header.Children.Clear();
            header.Children.AddRange(children);
        }

        private static void DecodeExternalAnimationFrames(Animation animation)
        {
            if (animation == null)
            {
                return;
            }

            var channels = Flatten(new[] { animation }).OfType<AnimationChannel>().ToList();
            var externalAnimationData = UncompressAnimationDataBlocks(animation, channels);
            foreach (var channel in channels)
            {
                channel.ReadFrames(externalAnimationData);
            }
        }

        private static byte[] UncompressAnimationData(AnimationData animationData)
        {
            if (animationData == null || animationData.CompressedData == null || animationData.CompressedData.Length == 0)
            {
                return new byte[0];
            }

            if (string.Equals((animationData.Compression ?? string.Empty).TrimEnd('\0', ' '), "ZLIB", StringComparison.OrdinalIgnoreCase) == false)
            {
                return animationData.CompressedData;
            }

            using (var input = new MemoryStream(animationData.CompressedData))
            {
                if (input.Length >= 2)
                {
                    input.Position = 2;
                }

                using (var deflate = new DeflateStream(input, CompressionMode.Decompress))
                {
                    var output = new byte[animationData.UncompressedSize];
                    int offset = 0;
                    while (offset < output.Length)
                    {
                        var read = deflate.Read(output, offset, output.Length - offset);
                        if (read <= 0)
                        {
                            break;
                        }

                        offset += read;
                    }

                    return output;
                }
            }
        }

        private static List<byte[]> UncompressAnimationDataBlocks(Animation animation, IEnumerable<AnimationChannel> channels)
        {
            if (animation == null)
            {
                return new List<byte[]>();
            }

            var blocks = Flatten(new[] { animation })
                .OfType<AnimationData>()
                .Select(UncompressAnimationData)
                .ToList();
            return AnimationChannel.BuildAnimationDataBlocks(channels, blocks);
        }

        private static void AppendU16(List<byte> data, ushort value)
        {
            data.Add((byte)(value & 0xFF));
            data.Add((byte)((value >> 8) & 0xFF));
        }

        private static void AppendU32(List<byte> data, uint value)
        {
            data.Add((byte)(value & 0xFF));
            data.Add((byte)((value >> 8) & 0xFF));
            data.Add((byte)((value >> 16) & 0xFF));
            data.Add((byte)((value >> 24) & 0xFF));
        }

        private static bool HasAnimatedValues(Vector4[] values, bool rotation)
        {
            if (values == null || values.Length < 2)
            {
                return false;
            }

            var first = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                var value = values[i];
                if (rotation == true)
                {
                    if (Math.Abs(first.X - value.X) > 0.0001f ||
                        Math.Abs(first.Y - value.Y) > 0.0001f ||
                        Math.Abs(first.Z - value.Z) > 0.0001f ||
                        Math.Abs(first.W - value.W) > 0.0001f)
                    {
                        return true;
                    }
                }
                else if (Math.Abs(first.X - value.X) > 0.0001f ||
                         Math.Abs(first.Y - value.Y) > 0.0001f ||
                         Math.Abs(first.Z - value.Z) > 0.0001f)
                {
                    return true;
                }
            }

            return false;
        }

        private static ushort[] BuildFrameKeys(float[] times, float frameRate, int stride)
        {
            var keys = times.Select(t => ClampFrame(t * frameRate))
                .Distinct()
                .OrderBy(f => f)
                .Where((f, i) => stride <= 1 || i == 0 || i % stride == 0 || i == times.Length - 1)
                .ToList();
            if (keys.Count == 0)
            {
                keys.Add(0);
            }

            return keys.ToArray();
        }

        private static Vector4[] BuildFrameValues(ushort[] frames, float[] times, Vector4[] values, float frameRate, bool normalize)
        {
            var result = new Vector4[frames.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                result[i] = Sample(times, values, frames[i] / frameRate);
                if (normalize == true)
                {
                    result[i] = Normalize(result[i]);
                }
            }

            return result;
        }

        private static Vector4 Sample(float[] times, Vector4[] values, float time)
        {
            if (values.Length == 1 || time <= times[0])
            {
                return values[0];
            }

            if (time >= times[times.Length - 1])
            {
                return values[values.Length - 1];
            }

            for (int i = 0; i + 1 < times.Length; i++)
            {
                if (time < times[i] || time > times[i + 1])
                {
                    continue;
                }

                var span = Math.Max(0.000001f, times[i + 1] - times[i]);
                var t = (time - times[i]) / span;
                return new Vector4
                {
                    X = values[i].X + (values[i + 1].X - values[i].X) * t,
                    Y = values[i].Y + (values[i + 1].Y - values[i].Y) * t,
                    Z = values[i].Z + (values[i + 1].Z - values[i].Z) * t,
                    W = values[i].W + (values[i + 1].W - values[i].W) * t,
                };
            }

            return values[values.Length - 1];
        }

        private static ushort ClampFrame(float frame)
        {
            return (ushort)Math.Max(0, Math.Min(ushort.MaxValue, (int)Math.Round(frame)));
        }

        private static Vector4[] ToQuaternions(float[] values)
        {
            var count = values.Length / 4;
            var result = new Vector4[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = Normalize(new Vector4
                {
                    X = values[i * 4 + 0],
                    Y = values[i * 4 + 1],
                    Z = values[i * 4 + 2],
                    W = values[i * 4 + 3],
                });
            }

            return result;
        }

        private static Vector4[] ToVec3s(float[] values)
        {
            var count = values.Length / 3;
            var result = new Vector4[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = new Vector4
                {
                    X = values[i * 3 + 0],
                    Y = values[i * 3 + 1],
                    Z = values[i * 3 + 2],
                };
            }

            return result;
        }

        private static Vector4 Normalize(Vector4 value)
        {
            var length = (float)Math.Sqrt(value.W * value.W + value.X * value.X + value.Y * value.Y + value.Z * value.Z);
            if (length <= 0.000001f)
            {
                return new Vector4 { W = 1.0f };
            }

            value.W /= length;
            value.X /= length;
            value.Y /= length;
            value.Z /= length;
            return value;
        }

        private static int GetExistingBoneOrder(Animation animation, string name)
        {
            var bones = Flatten(new[] { animation }).OfType<Bone>().ToList();
            var index = bones.FindIndex(b => string.Equals(b.Name, name, StringComparison.OrdinalIgnoreCase));
            return index < 0 ? int.MaxValue : index;
        }

        private static IEnumerable<BaseNode> Flatten(IEnumerable<BaseNode> nodes)
        {
            foreach (var node in nodes ?? Enumerable.Empty<BaseNode>())
            {
                yield return node;
                foreach (var child in Flatten(node.Children))
                {
                    yield return child;
                }
            }
        }

        private static GltfDocument Load(string fileName)
        {
            byte[] binaryChunk;
            string json;
            if (string.Equals(Path.GetExtension(fileName), ".glb", StringComparison.OrdinalIgnoreCase) == true)
            {
                json = ReadGlb(fileName, out binaryChunk);
            }
            else
            {
                json = File.ReadAllText(fileName, Encoding.UTF8);
                binaryChunk = null;
            }

            var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            var root = (Dictionary<string, object>)serializer.DeserializeObject(json);
            return new GltfDocument(fileName, root, binaryChunk);
        }

        private static string ReadGlb(string fileName, out byte[] binaryChunk)
        {
            binaryChunk = null;
            using (var input = File.OpenRead(fileName))
            using (var reader = new BinaryReader(input))
            {
                if (reader.ReadUInt32() != 0x46546C67)
                {
                    throw new InvalidDataException("Invalid GLB header.");
                }

                reader.ReadUInt32();
                reader.ReadUInt32();
                string json = null;
                while (input.Position + 8 <= input.Length)
                {
                    var length = reader.ReadUInt32();
                    var type = reader.ReadUInt32();
                    var data = reader.ReadBytes((int)length);
                    if (type == 0x4E4F534A)
                    {
                        json = Encoding.UTF8.GetString(data).TrimEnd('\0', ' ', '\r', '\n', '\t');
                    }
                    else if (type == 0x004E4942)
                    {
                        binaryChunk = data;
                    }
                }

                if (json == null)
                {
                    throw new InvalidDataException("GLB does not contain a JSON chunk.");
                }

                return json;
            }
        }

        private sealed class ImportedBoneAnimation
        {
            public string Name;
            public float[] RotationTimes;
            public Vector4[] Rotations;
            public float[] TranslationTimes;
            public Vector4[] Translations;
        }

        private sealed class GltfDocument
        {
            private readonly string _FileName;
            private readonly List<byte[]> _Buffers = new List<byte[]>();
            private readonly List<Dictionary<string, object>> _BufferViews;
            private readonly List<Dictionary<string, object>> _Accessors;
            public readonly List<GltfNode> Nodes;
            public readonly List<GltfAnimation> Animations;

            public GltfDocument(string fileName, Dictionary<string, object> root, byte[] binaryChunk)
            {
                this._FileName = fileName;
                this._BufferViews = GetObjectList(root, "bufferViews");
                this._Accessors = GetObjectList(root, "accessors");
                this.Nodes = GetObjectList(root, "nodes").Select(GltfNode.From).ToList();
                this.Animations = GetObjectList(root, "animations").Select(GltfAnimation.From).ToList();

                var buffers = GetObjectList(root, "buffers");
                for (int i = 0; i < buffers.Count; i++)
                {
                    var uri = GetString(buffers[i], "uri");
                    if (string.IsNullOrEmpty(uri) == true)
                    {
                        this._Buffers.Add(binaryChunk ?? new byte[0]);
                    }
                    else if (uri.StartsWith("data:", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        var comma = uri.IndexOf(',');
                        this._Buffers.Add(Convert.FromBase64String(uri.Substring(comma + 1)));
                    }
                    else
                    {
                        this._Buffers.Add(File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(fileName) ?? string.Empty, uri)));
                    }
                }
            }

            public Vector4 GetNodeRotation(string name)
            {
                var node = this.Nodes.FirstOrDefault(n => string.Equals(n.Name, name, StringComparison.OrdinalIgnoreCase));
                return node == null ? new Vector4 { W = 1.0f } : node.Rotation;
            }

            public Vector4 GetNodeTranslation(string name)
            {
                var node = this.Nodes.FirstOrDefault(n => string.Equals(n.Name, name, StringComparison.OrdinalIgnoreCase));
                return node == null ? new Vector4() : node.Translation;
            }

            public float[] ReadAccessorFloats(int accessorIndex)
            {
                if (accessorIndex < 0 || accessorIndex >= this._Accessors.Count)
                {
                    return new float[0];
                }

                var accessor = this._Accessors[accessorIndex];
                var viewIndex = GetInt(accessor, "bufferView", -1);
                if (viewIndex < 0 || viewIndex >= this._BufferViews.Count)
                {
                    return new float[0];
                }

                var view = this._BufferViews[viewIndex];
                var bufferIndex = GetInt(view, "buffer", 0);
                if (bufferIndex < 0 || bufferIndex >= this._Buffers.Count)
                {
                    return new float[0];
                }

                var componentType = GetInt(accessor, "componentType", 5126);
                var count = GetInt(accessor, "count", 0);
                var width = GetTypeWidth(GetString(accessor, "type"));
                var offset = GetInt(view, "byteOffset", 0) + GetInt(accessor, "byteOffset", 0);
                var stride = GetInt(view, "byteStride", GetComponentSize(componentType) * width);
                var buffer = this._Buffers[bufferIndex];
                var result = new float[count * width];
                for (int i = 0; i < count; i++)
                {
                    var baseOffset = offset + i * stride;
                    for (int c = 0; c < width; c++)
                    {
                        result[i * width + c] = ReadComponent(buffer, baseOffset + c * GetComponentSize(componentType), componentType);
                    }
                }

                return result;
            }

            private static float ReadComponent(byte[] buffer, int offset, int componentType)
            {
                if (offset < 0 || offset >= buffer.Length)
                {
                    return 0.0f;
                }

                switch (componentType)
                {
                    case 5126: return BitConverter.ToSingle(buffer, offset);
                    case 5123: return BitConverter.ToUInt16(buffer, offset);
                    case 5125: return BitConverter.ToUInt32(buffer, offset);
                    case 5122: return BitConverter.ToInt16(buffer, offset);
                    case 5121: return buffer[offset];
                    case 5120: return (sbyte)buffer[offset];
                    default: throw new NotSupportedException("Unsupported GLTF accessor component type " + componentType.ToString(CultureInfo.InvariantCulture));
                }
            }

            private static int GetComponentSize(int componentType)
            {
                switch (componentType)
                {
                    case 5126:
                    case 5125:
                        return 4;
                    case 5123:
                    case 5122:
                        return 2;
                    default:
                        return 1;
                }
            }
        }

        private sealed class GltfNode
        {
            public string Name;
            public Vector4 Rotation;
            public Vector4 Translation;

            public static GltfNode From(Dictionary<string, object> data)
            {
                return new GltfNode
                {
                    Name = GetString(data, "name"),
                    Rotation = GetVector(data, "rotation", new Vector4 { W = 1.0f }),
                    Translation = GetVector(data, "translation", new Vector4()),
                };
            }
        }

        private sealed class GltfAnimation
        {
            public readonly List<GltfSampler> Samplers = new List<GltfSampler>();
            public readonly List<GltfChannel> Channels = new List<GltfChannel>();

            public static GltfAnimation From(Dictionary<string, object> data)
            {
                var animation = new GltfAnimation();
                animation.Samplers.AddRange(GetObjectList(data, "samplers").Select(GltfSampler.From));
                animation.Channels.AddRange(GetObjectList(data, "channels").Select(GltfChannel.From));
                return animation;
            }
        }

        private sealed class GltfSampler
        {
            public int Input;
            public int Output;

            public static GltfSampler From(Dictionary<string, object> data)
            {
                return new GltfSampler { Input = GetInt(data, "input", -1), Output = GetInt(data, "output", -1) };
            }
        }

        private sealed class GltfChannel
        {
            public int Sampler;
            public int TargetNode;
            public string TargetPath;

            public static GltfChannel From(Dictionary<string, object> data)
            {
                var target = GetObject(data, "target");
                return new GltfChannel
                {
                    Sampler = GetInt(data, "sampler", -1),
                    TargetNode = GetInt(target, "node", -1),
                    TargetPath = GetString(target, "path"),
                };
            }
        }

        private static List<Dictionary<string, object>> GetObjectList(Dictionary<string, object> data, string key)
        {
            object value;
            if (data == null || data.TryGetValue(key, out value) == false || !(value is object[]))
            {
                return new List<Dictionary<string, object>>();
            }

            return ((object[])value).OfType<Dictionary<string, object>>().ToList();
        }

        private static Dictionary<string, object> GetObject(Dictionary<string, object> data, string key)
        {
            object value;
            return data != null && data.TryGetValue(key, out value) == true
                       ? value as Dictionary<string, object>
                       : null;
        }

        private static string GetString(Dictionary<string, object> data, string key)
        {
            object value;
            return data != null && data.TryGetValue(key, out value) == true && value != null
                       ? value.ToString()
                       : null;
        }

        private static int GetInt(Dictionary<string, object> data, string key, int fallback)
        {
            object value;
            if (data == null || data.TryGetValue(key, out value) == false || value == null)
            {
                return fallback;
            }

            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        private static Vector4 GetVector(Dictionary<string, object> data, string key, Vector4 fallback)
        {
            object value;
            if (data == null || data.TryGetValue(key, out value) == false || value == null)
            {
                return fallback;
            }

            var values = value as object[];
            if (values == null || values.Length == 0)
            {
                return fallback;
            }

            var result = fallback;
            if (values.Length > 0)
            {
                result.X = Convert.ToSingle(values[0], CultureInfo.InvariantCulture);
            }

            if (values.Length > 1)
            {
                result.Y = Convert.ToSingle(values[1], CultureInfo.InvariantCulture);
            }

            if (values.Length > 2)
            {
                result.Z = Convert.ToSingle(values[2], CultureInfo.InvariantCulture);
            }

            if (values.Length > 3)
            {
                result.W = Convert.ToSingle(values[3], CultureInfo.InvariantCulture);
            }

            return values.Length > 3 ? Normalize(result) : result;
        }

        private static int GetTypeWidth(string type)
        {
            switch ((type ?? string.Empty).ToUpperInvariant())
            {
                case "SCALAR": return 1;
                case "VEC2": return 2;
                case "VEC3": return 3;
                case "VEC4": return 4;
                default: throw new NotSupportedException("Unsupported GLTF accessor type " + type);
            }
        }
    }
}
