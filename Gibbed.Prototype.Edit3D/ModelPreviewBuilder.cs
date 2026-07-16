using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using System.Linq;
using System.Text;
using Gibbed.IO;
using Gibbed.Prototype.FileFormats;
using Gibbed.Prototype.FileFormats.Pure3D;
using Gibbed.Prototype.FileFormats.Pure3D.Prototype2;

namespace Gibbed.Prototype.Edit3D
{
    internal sealed class ModelPreviewCamera
    {
        public float Yaw = -0.55f;
        public float Pitch = 0.1f;
        public float Zoom = 1.0f;
        public float PanX;
        public float PanY;
        public Vec3 Origin;
        public Vec3 FreePosition;
        public bool FreeCamera;
        public bool ShowSkeleton;
        public bool ShowBoneNames;
        public bool ShowInfluences;
        public bool ShowLocators;
        public bool ShowControls;
        public bool ShowMaterialPreview;
        public bool UseCutsceneCamera;
        public Vec3 CutscenePosition;
        public Vec3 CutsceneLook;
        public Vec3 CutsceneUp;
        public float CutsceneFov;
        public float CutsceneNearClip;
        public float CutsceneFarClip;
        public bool HasCutsceneCamera;
        public float ControlRadius;
        public bool LightingEnabled = true;
        public ModelPreviewTextureMode TextureMode;
        public ModelPreviewWireMode WireMode;
    }

    internal enum ModelPreviewTextureMode
    {
        Default,
        NoAlpha,
        None,
    }

    internal enum ModelPreviewWireMode
    {
        Off,
        Overlay,
        Wireframe,
    }

    internal sealed class ModelPreviewScene
    {
        public string Name;
        public List<ModelPreviewVertex> Vertices = new List<ModelPreviewVertex>();
        public List<ModelPreviewVertex> BindVertices = new List<ModelPreviewVertex>();
        public List<ModelPreviewVertex> VertexAnimationBindVertices = new List<ModelPreviewVertex>();
        public List<ModelPreviewVertexAnimation> VertexAnimations = new List<ModelPreviewVertexAnimation>();
        public List<ModelPreviewVertexMorph> VertexMorphs = new List<ModelPreviewVertexMorph>();
        public Dictionary<string, ModelPreviewExpression> Expressions = new Dictionary<string, ModelPreviewExpression>(StringComparer.OrdinalIgnoreCase);
        public List<int> Indices = new List<int>();
        public List<ModelPreviewPart> Parts = new List<ModelPreviewPart>();
        public List<ModelPreviewMaterial> Materials = new List<ModelPreviewMaterial>();
        public List<ModelPreviewLight> Lights = new List<ModelPreviewLight>();
        public bool UseCinematicLighting;
        public Bitmap Texture;
        public string TextureName;
        public bool Unlit;
        public List<ModelPreviewBone> Bones = new List<ModelPreviewBone>();
        public List<ModelPreviewBone> BindBones = new List<ModelPreviewBone>();
        public List<ModelPreviewLocator> Locators = new List<ModelPreviewLocator>();
        public List<ModelPreviewSceneInstance> Instances = new List<ModelPreviewSceneInstance>();
        public bool UseGpuSkinning = true;
        public Vec3 StageDebugPositionOffset;
        public Vec3 StageDebugRotationDegrees;
        public Vec3 Center;
        public Vec3 Average;
        public float Radius = 1.0f;
    }

    internal sealed class ModelPreviewSceneInstance
    {
        public string Name;
        public string SkeletonName;
        public ulong ActorHash;
        public ulong LightGroupHash;
        public string LightGroupName;
        public int VertexStart;
        public int VertexCount;
        public int PartStart;
        public int PartCount;
        public int BoneStart;
        public int BoneCount;
        public Vec3 Position;
        public PreviewQuat Rotation;
        public bool IsStage;
    }

    internal sealed class ModelPreviewLocator
    {
        public string Name;
        public Vec3 Position;
    }

    internal struct ModelPreviewVertex
    {
        public Vec3 Position;
        public Vec3 Normal;
        public float U;
        public float V;
        public int Bone0;
        public int Bone1;
        public int Bone2;
        public int Bone3;
        public float Weight0;
        public float Weight1;
        public float Weight2;
        public float Weight3;
        public bool UseRigidBoneOrigin;
    }

    internal struct ModelPreviewPart
    {
        public string Name;
        public string ObjectName;
        public string ShaderName;
        public int MaterialIndex;
        public int VertexStart;
        public int VertexCount;
        public int IndexStart;
        public int IndexCount;
        public bool Hidden;
    }

    internal sealed class ModelPreviewMaterial
    {
        public string ShaderName;
        public string ShaderTemplateName;
        public string TextureName;
        public string NormalTextureName;
        public string SpecularTextureName;
        public Bitmap Texture;
        public Bitmap NormalTexture;
        public Bitmap SpecularTexture;
        public float SpecularMultiply = 0.0f;
        public float SpecularPower = 8.0f;
        public bool HasSpecParams;
        public bool HasAlpha;
    }

    internal sealed class ModelPreviewLight
    {
        public string Name;
        public ulong SourceHash;
        public bool RequiresTarget;
        public ulong LightGroupHash;
        public string LightGroupName;
        public string TargetActorName;
        public ulong TargetActorHash;
        public bool Dynamic;
        public Vec3 Position;
        public Vec3 Color = new Vec3(1.0f, 1.0f, 1.0f);
        public float Intensity = 1.0f;
        public float Range = 100.0f;
    }

    internal sealed class ModelPreviewVertexAnimation
    {
        public int VertexStart;
        public int VertexCount;
        public List<ModelPreviewVertexAnimationTarget> Targets = new List<ModelPreviewVertexAnimationTarget>();
    }

    internal sealed class ModelPreviewVertexAnimationTarget
    {
        public int SourceIndex;
        public Vec3?[] Positions;
        public Vec3?[] Normals;
    }

    internal sealed class ModelPreviewVertexMorph
    {
        public string InitialShapeName;
        public string DamagedShapeName;
        public int VertexStart;
        public int VertexCount;
        public Vec3[] Positions;
        public Vec3[] Normals;
    }

    internal sealed class ModelPreviewExpression
    {
        public string Name;
        public List<ModelPreviewExpressionTarget> Targets = new List<ModelPreviewExpressionTarget>();
    }

    internal sealed class ModelPreviewExpressionTarget
    {
        public float Value;
        public int TargetIndex;
    }

    internal struct ModelPreviewBone
    {
        public string Name;
        public int GroupId;
        public int Parent;
        public Vec3 LocalPosition;
        public Vec3 LocalAxisX;
        public Vec3 LocalAxisY;
        public Vec3 LocalAxisZ;
        public Vec3 Position;
        public Vec3 AxisX;
        public Vec3 AxisY;
        public Vec3 AxisZ;
    }

    internal sealed class ModelPreviewAnimation
    {
        public string Name;
        public float NumFrames;
        public float FrameRate;
        public bool Cyclic;
        public float StartFrame;
        public int BoneStart;
        public int BoneCount;
        public Vec3 InstancePosition;
        public PreviewQuat InstanceRotation;
        public bool UseVertexAnimation;
        public bool UseVisibilityAnimation;
        public List<ModelPreviewAnimationChannel> Channels = new List<ModelPreviewAnimationChannel>();
        public List<ModelPreviewVertexAnimationChannel> VertexChannels = new List<ModelPreviewVertexAnimationChannel>();
        public List<ModelPreviewVertexMorphChannel> VertexMorphChannels = new List<ModelPreviewVertexMorphChannel>();
        public List<ModelPreviewVisibilityChannel> VisibilityChannels = new List<ModelPreviewVisibilityChannel>();
    }

    internal sealed class ModelPreviewCameraAnimation
    {
        public string Name;
        public float NumFrames;
        public float FrameRate;
        public float StartFrame;
        public ushort[] PositionFrames;
        public Vec3[] Positions;
        public ushort[] LookFrames;
        public Vec3[] Looks;
        public ushort[] UpFrames;
        public Vec3[] Ups;
        public ushort[] FovFrames;
        public float[] Fovs;
        public ushort[] NearClipFrames;
        public float[] NearClips;
        public ushort[] FarClipFrames;
        public float[] FarClips;
    }

    internal sealed class ModelPreviewAnimationChannel
    {
        public int BoneIndex;
        public ushort[] RotationFrames;
        public PreviewQuat[] Rotations;
        public ushort[] TranslationFrames;
        public Vec3[] Translations;
    }

    internal sealed class ModelPreviewVertexAnimationChannel
    {
        public string Name;
        public int TargetIndex;
        public List<ModelPreviewExpressionTarget> ExpressionTargets;
        public ushort[] Frames;
        public float[] Weights;
    }

    internal sealed class ModelPreviewVisibilityChannel
    {
        public string ObjectName;
        public ushort[] Frames;
        public bool InitialVisible;
    }

    internal sealed class ModelPreviewVertexMorphChannel
    {
        public string InitialShapeName;
        public ushort[] Frames;
        public float[] Weights;
    }

    internal struct PreviewQuat
    {
        public float W;
        public float X;
        public float Y;
        public float Z;
    }

    internal struct Vec3
    {
        public float X;
        public float Y;
        public float Z;

        public Vec3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3 operator *(Vec3 a, float b)
        {
            return new Vec3(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vec3 operator /(Vec3 a, float b)
        {
            return new Vec3(a.X / b, a.Y / b, a.Z / b);
        }
    }

    internal static class ModelPreviewBuilder
    {
        private static readonly Dictionary<string, Pure3DFile> ExternalFileCache = new Dictionary<string, Pure3DFile>(StringComparer.OrdinalIgnoreCase);
        public static ModelPreviewCamera CreateDefaultCamera(ModelPreviewScene scene)
        {
            return new ModelPreviewCamera
            {
                Origin = scene.Center,
                ControlRadius = scene == null ? 1.0f : scene.Radius,
            };
        }

        public static ModelPreviewScene CreateTextureScene(string name, Image image)
        {
            if (image == null)
            {
                return null;
            }

            var bitmap = image as Bitmap;
            if (bitmap == null)
            {
                bitmap = new Bitmap(image);
            }
            else
            {
                bitmap = new Bitmap(bitmap);
            }

            float width = Math.Max(1.0f, bitmap.Width);
            float height = Math.Max(1.0f, bitmap.Height);
            float aspect = width / height;
            float halfWidth = aspect >= 1.0f ? aspect * 0.5f : 0.5f;
            float halfHeight = aspect >= 1.0f ? 0.5f : 0.5f / aspect;
            var scene = new ModelPreviewScene
            {
                Name = string.IsNullOrEmpty(name) ? "texture" : name,
                Texture = bitmap,
                TextureName = string.IsNullOrEmpty(name) ? "texture" : name,
                Unlit = true,
            };
            scene.Materials.Add(new ModelPreviewMaterial
            {
                ShaderName = "texture_preview",
                TextureName = scene.TextureName,
                Texture = bitmap,
            });
            scene.Vertices.Add(new ModelPreviewVertex { Position = new Vec3(-halfWidth, -halfHeight, 0.0f), Normal = new Vec3(0, 0, 1), U = 0, V = 1 });
            scene.Vertices.Add(new ModelPreviewVertex { Position = new Vec3(halfWidth, -halfHeight, 0.0f), Normal = new Vec3(0, 0, 1), U = 1, V = 1 });
            scene.Vertices.Add(new ModelPreviewVertex { Position = new Vec3(halfWidth, halfHeight, 0.0f), Normal = new Vec3(0, 0, 1), U = 1, V = 0 });
            scene.Vertices.Add(new ModelPreviewVertex { Position = new Vec3(-halfWidth, halfHeight, 0.0f), Normal = new Vec3(0, 0, 1), U = 0, V = 0 });
            scene.Indices.AddRange(new[] { 0, 1, 2, 0, 2, 3 });
            scene.Parts.Add(new ModelPreviewPart
            {
                Name = scene.Name,
                ObjectName = scene.Name,
                ShaderName = "texture_preview",
                MaterialIndex = 0,
                VertexStart = 0,
                VertexCount = 4,
                IndexStart = 0,
                IndexCount = 6,
            });
            scene.BindVertices = scene.Vertices.ToList();
            scene.VertexAnimationBindVertices = scene.BindVertices.ToList();
            FitScene(scene);
            return scene;
        }

        public static ModelPreviewScene CreateScene(Pure3DFile file, PolySkin polySkin)
        {
            return CreateScene(file, polySkin, null);
        }

        public static ModelPreviewScene CreateScene(Pure3DFile file, PolySkin polySkin, string sourceFileName)
        {
            var scene = new ModelPreviewScene
            {
                Name = polySkin.Name,
            };
            var shaderNames = new List<string>();

            foreach (var primitiveGroup in polySkin.Children.OfType<U00010020_PrimitiveGroup>())
            {
                shaderNames.Add(primitiveGroup.ShaderName);
                AppendPrimitiveGroup(primitiveGroup, scene, polySkin.Name);
                AppendVertexAnimation(polySkin, scene, scene.Parts.Count == 0 ? 0 : scene.Parts[scene.Parts.Count - 1].VertexStart, scene.Parts.Count == 0 ? 0 : scene.Parts[scene.Parts.Count - 1].VertexCount);
            }

            scene.BindVertices = scene.Vertices.ToList();
            scene.VertexAnimationBindVertices = scene.BindVertices.ToList();

            ResolveExpressions(file, scene);
            RegisterVrtxShapeMorphs(file, scene);
            ResolveMaterials(file, scene);
            FitScene(scene);
            scene.Bones = BuildBones(FindSkeletonFiles(file, sourceFileName), polySkin);
            scene.BindBones = CloneBones(scene.Bones);
            return WithLocators(file, scene);
        }

        public static ModelPreviewScene CreateScene(Pure3DFile file, Geometry geometry)
        {
            var scene = new ModelPreviewScene
            {
                Name = geometry.Name,
            };
            var shaderNames = new List<string>();

            AppendGeometry(geometry, scene, shaderNames);

            scene.BindVertices = scene.Vertices.ToList();
            scene.VertexAnimationBindVertices = scene.BindVertices.ToList();
            ResolveExpressions(file, scene);
            RegisterVrtxShapeMorphs(file, scene);
            ResolveMaterials(file, scene);
            FitScene(scene);
            return WithLocators(file, scene);
        }

        public static ModelPreviewScene CreateScene(Pure3DFile file, P2Primitive primitive)
        {
            var scene = new ModelPreviewScene
            {
                Name = primitive.Name,
            };

            AppendP2Primitive(primitive, primitive, primitive, scene, primitive.Name, null);
            ResolveMaterials(file, scene);
            FitScene(scene);
            return WithLocators(file, scene);
        }

        public static ModelPreviewScene CreateScene(Pure3DFile file, P2PolySkin polySkin, string sourceFileName)
        {
            var scene = new ModelPreviewScene
            {
                Name = polySkin.Name,
            };

            var flattened = file == null ? new List<BaseNode>() : Flatten(file.Nodes).ToList();
            var composite = polySkin.GetParentNode<CompositeDrawable>();
            var p2Composite = polySkin.GetParentNode<P2PolySkinComposite>();
            var skeletonFiles = FindSkeletonFiles(file, sourceFileName).ToList();
            var p2BoneRemap = p2Composite != null
                                  ? BuildP2BoneRemap(skeletonFiles, p2Composite.SkeletonName)
                                  : null;
            var metadata = polySkin.Children.OfType<P2PolySkinMetadata>().FirstOrDefault();
            if (metadata != null)
            {
                var indexPrimitive = flattened.OfType<P2Primitive>()
                                              .FirstOrDefault(p => string.Equals(p.Name, metadata.IndicesName, StringComparison.OrdinalIgnoreCase));
                var vertexPrimitive = flattened.OfType<P2Primitive>()
                                               .FirstOrDefault(p => string.Equals(p.Name, metadata.VerticesName, StringComparison.OrdinalIgnoreCase));
                var skinPrimitive = flattened.OfType<P2Primitive>()
                                             .FirstOrDefault(p => string.Equals(p.Name, metadata.SkinName, StringComparison.OrdinalIgnoreCase)) ??
                                    vertexPrimitive;
                AppendP2Primitive(skinPrimitive, vertexPrimitive, indexPrimitive, scene, polySkin.Name, metadata.ShaderName, p2BoneRemap);
            }

            ResolveMaterials(file, scene);
            scene.Bones = composite != null
                              ? BuildBones(skeletonFiles, composite.SkeletonName, false)
                              : p2Composite != null
                                    ? BuildBones(skeletonFiles, p2Composite.SkeletonName, false)
                                    : new List<ModelPreviewBone>();
            scene.BindBones = CloneBones(scene.Bones);
            scene.BindVertices = scene.Vertices.ToList();
            scene.VertexAnimationBindVertices = scene.BindVertices.ToList();
            FitScene(scene);
            return WithLocators(file, scene);
        }

        public static ModelPreviewScene CreateScene(Pure3DFile file, P2PolySkinComposite composite, string sourceFileName)
        {
            var scene = new ModelPreviewScene
            {
                Name = composite.Name,
            };
            var shaderNames = new List<string>();
            var nodes = file == null ? new List<BaseNode>() : Flatten(file.Nodes).ToList();
            var skeletonFiles = FindSkeletonFiles(file, sourceFileName).ToList();
            var p2BoneRemap = BuildP2BoneRemap(skeletonFiles, composite.SkeletonName);

            foreach (var p2PolySkin in composite.Children.OfType<P2PolySkin>())
            {
                AppendP2PolySkin(p2PolySkin, nodes, scene, shaderNames, p2BoneRemap);
            }

            ResolveMaterials(file, scene);
            RegisterVrtxShapeMorphs(file, scene);
            scene.Bones = BuildBones(skeletonFiles, composite.SkeletonName, false);
            scene.BindBones = CloneBones(scene.Bones);
            scene.BindVertices = scene.Vertices.ToList();
            scene.VertexAnimationBindVertices = scene.BindVertices.ToList();
            FitScene(scene);
            return WithLocators(file, scene);
        }

        public static ModelPreviewScene CreateScene(Pure3DFile file, CompositeDrawable compositeDrawable, string sourceFileName)
        {
            var scene = new ModelPreviewScene
            {
                Name = compositeDrawable.Name,
            };
            var shaderNames = new List<string>();
            var nodes = Flatten(file.Nodes).ToList();
            var skeletonFiles = FindSkeletonFiles(file, sourceFileName).ToList();
            var p2BoneRemap = BuildP2BoneRemap(skeletonFiles, compositeDrawable.SkeletonName);

            foreach (var reference in compositeDrawable.Children.OfType<CompositeDrawablePolySkinReference>())
            {
                var polySkin = nodes.OfType<PolySkin>()
                                    .FirstOrDefault(p => string.Equals(p.Name, reference.PolySkinName, StringComparison.OrdinalIgnoreCase));
                if (polySkin != null)
                {
                    AppendPolySkin(polySkin, scene, shaderNames);
                    continue;
                }

                var geometry = nodes.OfType<Geometry>()
                                    .FirstOrDefault(g => string.Equals(g.Name, reference.PolySkinName, StringComparison.OrdinalIgnoreCase));
                if (geometry != null)
                {
                    AppendGeometry(geometry, scene, shaderNames, GetCompositeReferenceBoneIndex(reference));
                }
            }

            foreach (var p2PolySkin in compositeDrawable.Children.OfType<P2PolySkin>())
            {
                var metadata = p2PolySkin.Children.OfType<P2PolySkinMetadata>().FirstOrDefault();
                if (metadata == null)
                {
                    continue;
                }

                var indexPrimitive = nodes.OfType<P2Primitive>()
                                          .FirstOrDefault(p => string.Equals(p.Name, metadata.IndicesName, StringComparison.OrdinalIgnoreCase));
                var vertexPrimitive = nodes.OfType<P2Primitive>()
                                           .FirstOrDefault(p => string.Equals(p.Name, metadata.VerticesName, StringComparison.OrdinalIgnoreCase));
                var skinPrimitive = nodes.OfType<P2Primitive>()
                                         .FirstOrDefault(p => string.Equals(p.Name, metadata.SkinName, StringComparison.OrdinalIgnoreCase)) ??
                                    vertexPrimitive;
                AppendP2Primitive(skinPrimitive, vertexPrimitive, indexPrimitive, scene, p2PolySkin.Name, metadata.ShaderName, p2BoneRemap);
            }

            ResolveMaterials(file, scene);
            ResolveExpressions(file, scene);
            RegisterVrtxShapeMorphs(file, scene);
            scene.Bones = BuildBones(skeletonFiles, compositeDrawable.SkeletonName, false);
            scene.BindBones = CloneBones(scene.Bones);
            scene.BindVertices = scene.Vertices.ToList();
            scene.VertexAnimationBindVertices = scene.BindVertices.ToList();
            ApplyRigidGeometryBindPose(scene);
            FitScene(scene);
            return WithLocators(file, scene);
        }

        public static ModelPreviewScene CreateScene(Pure3DFile file, IEnumerable<BaseNode> previewNodes, string sourceFileName)
        {
            return CreateScene(file, previewNodes, sourceFileName, null);
        }

        public static ModelPreviewScene CreateScene(Pure3DFile file, IEnumerable<BaseNode> previewNodes, string sourceFileName, IEnumerable<BaseNode> stageNodes)
        {
            var nodesToPreview = previewNodes == null ? new List<BaseNode>() : previewNodes.Where(IsPreviewNode).ToList();
            if (nodesToPreview.Count == 0)
            {
                return null;
            }

            var stageNodeSet = stageNodes == null
                                   ? new HashSet<BaseNode>()
                                   : new HashSet<BaseNode>(stageNodes.Where(IsPreviewNode));

            if (nodesToPreview.Count == 1)
            {
                var only = nodesToPreview[0];
                var isStageOnly = stageNodeSet.Contains(only);
                ModelPreviewScene singleScene = null;
                if (only is PolySkin)
                {
                    singleScene = CreateScene(file, (PolySkin)only, sourceFileName);
                }
                else if (only is Geometry)
                {
                    singleScene = CreateScene(file, (Geometry)only);
                }
                else if (only is CompositeDrawable)
                {
                    singleScene = CreateScene(file, (CompositeDrawable)only, sourceFileName);
                }
                else if (only is P2PolySkinComposite)
                {
                    singleScene = CreateScene(file, (P2PolySkinComposite)only, sourceFileName);
                }
                else if (only is P2PolySkin)
                {
                    singleScene = CreateScene(file, (P2PolySkin)only, sourceFileName);
                }
                else if (only is P2Primitive)
                {
                    singleScene = CreateScene(file, (P2Primitive)only);
                }

                if (singleScene != null && isStageOnly == true)
                {
                    singleScene.Instances.Add(new ModelPreviewSceneInstance
                    {
                        Name = GetNodeName(only),
                        SkeletonName = GetNodeSkeletonName(only),
                        VertexStart = 0,
                        VertexCount = singleScene.Vertices.Count,
                        PartStart = 0,
                        PartCount = singleScene.Parts.Count,
                        BoneStart = 0,
                        BoneCount = singleScene.Bones.Count,
                        Rotation = new PreviewQuat { W = 1.0f },
                        IsStage = true,
                    });
                }

                return singleScene;
            }

            if (nodesToPreview.Any(n => n is CompositeDrawable || n is P2PolySkinComposite || n is PolySkin || n is P2PolySkin) == true)
            {
                var mergedScene = CreateMergedInstanceScene(file, nodesToPreview, sourceFileName, stageNodeSet);
                if (mergedScene != null)
                {
                    return mergedScene;
                }
            }

            var scene = new ModelPreviewScene
            {
                Name = "Preview Selection",
            };
            var shaderNames = new List<string>();
            string skeletonName = null;
            bool exactP2SkeletonRequired = false;
            var flattened = file == null ? new List<BaseNode>() : Flatten(file.Nodes).ToList();
            var compositeReferenceLookup = BuildCompositeReferenceLookup(nodesToPreview.OfType<Geometry>(), flattened, ref skeletonName);

            foreach (var node in nodesToPreview)
            {
                var polySkin = node as PolySkin;
                if (polySkin != null)
                {
                    if (string.IsNullOrEmpty(skeletonName))
                    {
                        skeletonName = polySkin.SkeletonName;
                    }

                    AppendPolySkin(polySkin, scene, shaderNames);
                    continue;
                }

                var geometry = node as Geometry;
                if (geometry != null)
                {
                    CompositeDrawablePolySkinReference reference;
                    AppendGeometry(
                        geometry,
                        scene,
                        shaderNames,
                        compositeReferenceLookup.TryGetValue(geometry.Name ?? string.Empty, out reference) == true
                            ? GetCompositeReferenceBoneIndex(reference)
                            : -1);
                    continue;
                }

                var compositeDrawable = node as CompositeDrawable;
                if (compositeDrawable != null)
                {
                    if (string.IsNullOrEmpty(skeletonName))
                    {
                        skeletonName = compositeDrawable.SkeletonName;
                    }

                    foreach (var reference in compositeDrawable.Children.OfType<CompositeDrawablePolySkinReference>())
                    {
                        var referencedPolySkin = flattened.OfType<PolySkin>()
                                                          .FirstOrDefault(p => string.Equals(p.Name, reference.PolySkinName, StringComparison.OrdinalIgnoreCase));
                        if (referencedPolySkin != null)
                        {
                            AppendPolySkin(referencedPolySkin, scene, shaderNames);
                            continue;
                        }

                        var referencedGeometry = flattened.OfType<Geometry>()
                                                          .FirstOrDefault(g => string.Equals(g.Name, reference.PolySkinName, StringComparison.OrdinalIgnoreCase));
                        if (referencedGeometry != null)
                        {
                            AppendGeometry(referencedGeometry, scene, shaderNames, GetCompositeReferenceBoneIndex(reference));
                        }
                    }
                }

                var p2PolySkin = node as P2PolySkin;
                if (p2PolySkin != null)
                {
                    exactP2SkeletonRequired = true;
                    var p2Composite = p2PolySkin.GetParentNode<CompositeDrawable>();
                    if (p2Composite != null && string.IsNullOrEmpty(skeletonName))
                    {
                        skeletonName = p2Composite.SkeletonName;
                    }
                    var p2PolySkinComposite = p2PolySkin.GetParentNode<P2PolySkinComposite>();
                    if (p2PolySkinComposite != null && string.IsNullOrEmpty(skeletonName))
                    {
                        skeletonName = p2PolySkinComposite.SkeletonName;
                    }
                    var p2ParentSkeletonName = p2PolySkinComposite != null
                                                   ? p2PolySkinComposite.SkeletonName
                                                   : p2Composite != null ? p2Composite.SkeletonName : null;
                    AppendP2PolySkin(
                        p2PolySkin,
                        flattened,
                        scene,
                        shaderNames,
                        BuildP2BoneRemap(FindSkeletonFiles(file, sourceFileName), p2ParentSkeletonName));
                    continue;
                }

                var p2CompositeNode = node as P2PolySkinComposite;
                if (p2CompositeNode != null)
                {
                    exactP2SkeletonRequired = true;
                    if (string.IsNullOrEmpty(skeletonName))
                    {
                        skeletonName = p2CompositeNode.SkeletonName;
                    }

                    foreach (var p2PolySkinChild in p2CompositeNode.Children.OfType<P2PolySkin>())
                    {
                        AppendP2PolySkin(
                            p2PolySkinChild,
                            flattened,
                            scene,
                            shaderNames,
                            BuildP2BoneRemap(FindSkeletonFiles(file, sourceFileName), p2CompositeNode.SkeletonName));
                    }
                    continue;
                }

                var p2Primitive = node as P2Primitive;
                if (p2Primitive != null)
                {
                    AppendP2Primitive(p2Primitive, p2Primitive, p2Primitive, scene, p2Primitive.Name, null);
                }
            }

            ResolveMaterials(file, scene);
            ResolveExpressions(file, scene);
            RegisterVrtxShapeMorphs(file, scene);
            scene.Bones = string.IsNullOrEmpty(skeletonName)
                              ? new List<ModelPreviewBone>()
                              : BuildBones(FindSkeletonFiles(file, sourceFileName), skeletonName, exactP2SkeletonRequired == false);
            scene.BindBones = CloneBones(scene.Bones);
            scene.BindVertices = scene.Vertices.ToList();
            scene.VertexAnimationBindVertices = scene.BindVertices.ToList();
            ApplyRigidGeometryBindPose(scene);
            FitScene(scene);
            return WithLocators(file, scene);
        }

        private static ModelPreviewScene CreateMergedInstanceScene(Pure3DFile file, IEnumerable<BaseNode> previewNodes, string sourceFileName)
        {
            return CreateMergedInstanceScene(file, previewNodes, sourceFileName, null);
        }

        private static ModelPreviewScene CreateMergedInstanceScene(Pure3DFile file, IEnumerable<BaseNode> previewNodes, string sourceFileName, ISet<BaseNode> stageNodes)
        {
            var scene = new ModelPreviewScene
            {
                Name = "Preview Selection",
            };
            var sharedSkeletons = new List<Tuple<int, int>>();

            foreach (var node in previewNodes)
            {
                var instanceScene = CreateSinglePreviewScene(file, node, sourceFileName);
                if (instanceScene == null)
                {
                    continue;
                }

                var skeletonName = GetNodeSkeletonName(node);
                bool isStage = stageNodes != null && stageNodes.Contains(node);
                int? sharedBoneStart = null;
                if (isStage == false &&
                    instanceScene.Bones != null &&
                    instanceScene.Bones.Count > 0)
                {
                    var shared = sharedSkeletons.FirstOrDefault(s => s.Item2 == instanceScene.Bones.Count);
                    if (shared != null)
                    {
                        sharedBoneStart = shared.Item1;
                    }
                }

                var instance = MergePreviewSceneInstance(scene, instanceScene, GetNodeName(node), skeletonName, isStage, sharedBoneStart);
                if (isStage == false &&
                    sharedBoneStart.HasValue == false &&
                    instance != null &&
                    instance.BoneCount > 0)
                {
                    sharedSkeletons.Add(new Tuple<int, int>(instance.BoneStart, instance.BoneCount));
                }
            }

            if (scene.Parts.Count == 0 && scene.Vertices.Count == 0)
            {
                return null;
            }

            if (scene.BindVertices.Count != scene.Vertices.Count)
            {
                scene.BindVertices = scene.Vertices.ToList();
            }

            if (scene.VertexAnimationBindVertices.Count != scene.Vertices.Count)
            {
                scene.VertexAnimationBindVertices = scene.BindVertices.ToList();
            }
            FitScene(scene);
            return WithLocators(file, scene);
        }

        private static ModelPreviewScene CreateSinglePreviewScene(Pure3DFile file, BaseNode node, string sourceFileName)
        {
            if (node is PolySkin)
            {
                return CreateScene(file, (PolySkin)node, sourceFileName);
            }

            if (node is Geometry)
            {
                return CreateScene(file, (Geometry)node);
            }

            if (node is CompositeDrawable)
            {
                return CreateScene(file, (CompositeDrawable)node, sourceFileName);
            }

            if (node is P2PolySkinComposite)
            {
                return CreateScene(file, (P2PolySkinComposite)node, sourceFileName);
            }

            if (node is P2PolySkin)
            {
                return CreateScene(file, (P2PolySkin)node, sourceFileName);
            }

            if (node is P2Primitive)
            {
                return CreateScene(file, (P2Primitive)node);
            }

            return null;
        }

        private static ModelPreviewSceneInstance MergePreviewSceneInstance(ModelPreviewScene target, ModelPreviewScene source, string instanceName, string skeletonName)
        {
            return MergePreviewSceneInstance(target, source, instanceName, skeletonName, false, null);
        }

        private static ModelPreviewSceneInstance MergePreviewSceneInstance(ModelPreviewScene target, ModelPreviewScene source, string instanceName, string skeletonName, bool isStage)
        {
            return MergePreviewSceneInstance(target, source, instanceName, skeletonName, isStage, null);
        }

        private static ModelPreviewSceneInstance MergePreviewSceneInstance(ModelPreviewScene target, ModelPreviewScene source, string instanceName, string skeletonName, bool isStage, int? sharedBoneStart)
        {
            int vertexOffset = target.Vertices.Count;
            int indexOffset = target.Indices.Count;
            int materialOffset = target.Materials.Count;
            int boneOffset = sharedBoneStart.HasValue == true ? sharedBoneStart.Value : target.Bones.Count;
            int partOffset = target.Parts.Count;
            int vertexAnimationOffset = target.VertexAnimations.Count;
            bool reuseBones = sharedBoneStart.HasValue == true;

            target.Materials.AddRange(source.Materials ?? new List<ModelPreviewMaterial>());
            foreach (var vertex in source.Vertices ?? new List<ModelPreviewVertex>())
            {
                target.Vertices.Add(OffsetVertexBones(vertex, boneOffset));
            }

            foreach (var vertex in source.BindVertices ?? source.Vertices ?? new List<ModelPreviewVertex>())
            {
                target.BindVertices.Add(OffsetVertexBones(vertex, boneOffset));
            }

            foreach (var vertex in source.VertexAnimationBindVertices ?? source.BindVertices ?? source.Vertices ?? new List<ModelPreviewVertex>())
            {
                target.VertexAnimationBindVertices.Add(OffsetVertexBones(vertex, boneOffset));
            }

            foreach (var index in source.Indices ?? new List<int>())
            {
                target.Indices.Add(index + vertexOffset);
            }

            foreach (var part in source.Parts ?? new List<ModelPreviewPart>())
            {
                var mergedPart = part;
                mergedPart.VertexStart += vertexOffset;
                mergedPart.IndexStart += indexOffset;
                if (mergedPart.MaterialIndex >= 0)
                {
                    mergedPart.MaterialIndex += materialOffset;
                }

                target.Parts.Add(mergedPart);
            }

            if (reuseBones == false)
            {
                foreach (var bone in source.Bones ?? new List<ModelPreviewBone>())
                {
                    target.Bones.Add(OffsetBone(bone, boneOffset));
                }

                foreach (var bone in source.BindBones ?? new List<ModelPreviewBone>())
                {
                    target.BindBones.Add(OffsetBone(bone, boneOffset));
                }
            }

            foreach (var vertexAnimation in source.VertexAnimations ?? new List<ModelPreviewVertexAnimation>())
            {
                var mergedAnimation = new ModelPreviewVertexAnimation
                {
                    VertexStart = vertexAnimation.VertexStart + vertexOffset,
                    VertexCount = vertexAnimation.VertexCount,
                    Targets = vertexAnimation.Targets,
                };
                target.VertexAnimations.Add(mergedAnimation);
            }

            foreach (var morph in source.VertexMorphs ?? new List<ModelPreviewVertexMorph>())
            {
                target.VertexMorphs.Add(new ModelPreviewVertexMorph
                {
                    InitialShapeName = morph.InitialShapeName,
                    DamagedShapeName = morph.DamagedShapeName,
                    VertexStart = morph.VertexStart + vertexOffset,
                    VertexCount = morph.VertexCount,
                    Positions = morph.Positions,
                    Normals = morph.Normals,
                });
            }

            foreach (var expression in source.Expressions ?? new Dictionary<string, ModelPreviewExpression>(StringComparer.OrdinalIgnoreCase))
            {
                var mergedExpression = new ModelPreviewExpression
                {
                    Name = expression.Value.Name,
                    Targets = expression.Value.Targets
                        .Select(t => new ModelPreviewExpressionTarget { Value = t.Value, TargetIndex = t.TargetIndex + vertexAnimationOffset })
                        .ToList(),
                };
                target.Expressions[BuildInstanceExpressionName(instanceName, expression.Key)] = mergedExpression;
            }

            var instance = new ModelPreviewSceneInstance
            {
                Name = instanceName,
                SkeletonName = skeletonName,
                VertexStart = vertexOffset,
                VertexCount = target.Vertices.Count - vertexOffset,
                PartStart = partOffset,
                PartCount = target.Parts.Count - partOffset,
                BoneStart = boneOffset,
                BoneCount = reuseBones == true
                                ? (source.Bones == null ? 0 : source.Bones.Count)
                                : target.Bones.Count - boneOffset,
                Rotation = new PreviewQuat { W = 1.0f },
                IsStage = isStage,
            };
            target.Instances.Add(instance);
            return instance;
        }

        public static ModelPreviewSceneInstance DuplicateInstance(ModelPreviewScene scene, ModelPreviewSceneInstance source, string instanceName)
        {
            if (scene == null || source == null)
            {
                return null;
            }

            int vertexOffset = scene.Vertices.Count;
            int indexOffset = scene.Indices.Count;
            int boneOffset = scene.Bones.Count;
            int partOffset = scene.Parts.Count;

            int sourceVertexStart = Math.Max(0, source.VertexStart);
            int sourceVertexEnd = Math.Min(scene.Vertices.Count, sourceVertexStart + Math.Max(0, source.VertexCount));
            int sourceBoneStart = Math.Max(0, source.BoneStart);
            int sourceBoneEnd = Math.Min(scene.Bones.Count, sourceBoneStart + Math.Max(0, source.BoneCount));

            for (int i = sourceVertexStart; i < sourceVertexEnd; i++)
            {
                scene.Vertices.Add(RemapInstanceVertex(scene.Vertices[i], sourceBoneStart, sourceBoneEnd, boneOffset));
            }

            for (int i = sourceVertexStart; i < sourceVertexEnd && i < scene.BindVertices.Count; i++)
            {
                scene.BindVertices.Add(RemapInstanceVertex(scene.BindVertices[i], sourceBoneStart, sourceBoneEnd, boneOffset));
            }

            for (int i = sourceVertexStart; i < sourceVertexEnd && i < scene.VertexAnimationBindVertices.Count; i++)
            {
                scene.VertexAnimationBindVertices.Add(RemapInstanceVertex(scene.VertexAnimationBindVertices[i], sourceBoneStart, sourceBoneEnd, boneOffset));
            }

            for (int i = sourceBoneStart; i < sourceBoneEnd; i++)
            {
                scene.Bones.Add(OffsetBone(scene.Bones[i], boneOffset - sourceBoneStart));
            }

            for (int i = sourceBoneStart; i < sourceBoneEnd && i < scene.BindBones.Count; i++)
            {
                scene.BindBones.Add(OffsetBone(scene.BindBones[i], boneOffset - sourceBoneStart));
            }

            int sourcePartStart = Math.Max(0, source.PartStart);
            int sourcePartEnd = Math.Min(scene.Parts.Count, sourcePartStart + Math.Max(0, source.PartCount));
            for (int i = sourcePartStart; i < sourcePartEnd; i++)
            {
                var sourcePart = scene.Parts[i];
                var copiedPart = sourcePart;
                copiedPart.VertexStart = vertexOffset + Math.Max(0, sourcePart.VertexStart - sourceVertexStart);
                copiedPart.IndexStart = scene.Indices.Count;
                for (int j = 0; j < sourcePart.IndexCount && sourcePart.IndexStart + j < scene.Indices.Count; j++)
                {
                    scene.Indices.Add(vertexOffset + scene.Indices[sourcePart.IndexStart + j] - sourceVertexStart);
                }

                scene.Parts.Add(copiedPart);
            }

            var instance = new ModelPreviewSceneInstance
            {
                Name = instanceName,
                SkeletonName = source.SkeletonName,
                VertexStart = vertexOffset,
                VertexCount = sourceVertexEnd - sourceVertexStart,
                PartStart = partOffset,
                PartCount = scene.Parts.Count - partOffset,
                BoneStart = boneOffset,
                BoneCount = sourceBoneEnd - sourceBoneStart,
                Rotation = new PreviewQuat { W = 1.0f },
                IsStage = source.IsStage,
            };
            scene.Instances.Add(instance);
            FitScene(scene);
            return instance;
        }

        private static ModelPreviewVertex RemapInstanceVertex(ModelPreviewVertex vertex, int sourceBoneStart, int sourceBoneEnd, int boneOffset)
        {
            vertex.Bone0 = RemapInstanceBone(vertex.Bone0, sourceBoneStart, sourceBoneEnd, boneOffset);
            vertex.Bone1 = RemapInstanceBone(vertex.Bone1, sourceBoneStart, sourceBoneEnd, boneOffset);
            vertex.Bone2 = RemapInstanceBone(vertex.Bone2, sourceBoneStart, sourceBoneEnd, boneOffset);
            vertex.Bone3 = RemapInstanceBone(vertex.Bone3, sourceBoneStart, sourceBoneEnd, boneOffset);
            return vertex;
        }

        private static int RemapInstanceBone(int bone, int sourceBoneStart, int sourceBoneEnd, int boneOffset)
        {
            return bone >= sourceBoneStart && bone < sourceBoneEnd
                       ? boneOffset + bone - sourceBoneStart
                       : bone;
        }

        private static ModelPreviewVertex OffsetVertexBones(ModelPreviewVertex vertex, int boneOffset)
        {
            if (boneOffset <= 0)
            {
                return vertex;
            }

            if (vertex.Bone0 >= 0) vertex.Bone0 += boneOffset;
            if (vertex.Bone1 >= 0) vertex.Bone1 += boneOffset;
            if (vertex.Bone2 >= 0) vertex.Bone2 += boneOffset;
            if (vertex.Bone3 >= 0) vertex.Bone3 += boneOffset;
            return vertex;
        }

        private static ModelPreviewBone OffsetBone(ModelPreviewBone bone, int boneOffset)
        {
            if (bone.Parent >= 0)
            {
                bone.Parent += boneOffset;
            }

            return bone;
        }

        private static string GetNodeName(BaseNode node)
        {
            if (node is PolySkin) return ((PolySkin)node).Name;
            if (node is Geometry) return ((Geometry)node).Name;
            if (node is CompositeDrawable) return ((CompositeDrawable)node).Name;
            if (node is P2PolySkinComposite) return ((P2PolySkinComposite)node).Name;
            if (node is P2PolySkin) return ((P2PolySkin)node).Name;
            if (node is P2Primitive) return ((P2Primitive)node).Name;
            return node == null ? null : node.ToString();
        }

        private static string GetNodeSkeletonName(BaseNode node)
        {
            if (node is PolySkin) return ((PolySkin)node).SkeletonName;
            if (node is CompositeDrawable) return ((CompositeDrawable)node).SkeletonName;
            if (node is P2PolySkinComposite) return ((P2PolySkinComposite)node).SkeletonName;
            var p2PolySkin = node as P2PolySkin;
            if (p2PolySkin != null)
            {
                var composite = p2PolySkin.GetParentNode<P2PolySkinComposite>();
                if (composite != null)
                {
                    return composite.SkeletonName;
                }
            }

            return null;
        }

        private static string BuildInstanceExpressionName(string instanceName, string expressionName)
        {
            if (string.IsNullOrEmpty(instanceName) == true)
            {
                return expressionName;
            }

            return instanceName + "::" + expressionName;
        }

        private static bool IsPreviewNode(BaseNode node)
        {
            return node is PolySkin || node is Geometry || node is CompositeDrawable || node is P2PolySkinComposite || node is P2PolySkin || node is P2Primitive;
        }

        private static Dictionary<string, CompositeDrawablePolySkinReference> BuildCompositeReferenceLookup(
            IEnumerable<Geometry> geometries,
            IEnumerable<BaseNode> flattened,
            ref string skeletonName)
        {
            var result = new Dictionary<string, CompositeDrawablePolySkinReference>(StringComparer.OrdinalIgnoreCase);
            var geometryNames = geometries == null
                                    ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                                    : new HashSet<string>(
                                        geometries.Where(g => string.IsNullOrEmpty(g.Name) == false).Select(g => g.Name),
                                        StringComparer.OrdinalIgnoreCase);
            if (geometryNames.Count == 0 || flattened == null)
            {
                return result;
            }

            foreach (var compositeDrawable in flattened.OfType<CompositeDrawable>())
            {
                var references = compositeDrawable.Children.OfType<CompositeDrawablePolySkinReference>()
                                                 .Where(r => string.IsNullOrEmpty(r.PolySkinName) == false)
                                                 .Where(r => geometryNames.Contains(r.PolySkinName))
                                                 .ToList();
                if (references.Count == 0)
                {
                    continue;
                }

                foreach (var reference in references)
                {
                    if (result.ContainsKey(reference.PolySkinName) == false)
                    {
                        result.Add(reference.PolySkinName, reference);
                    }
                }

                if (string.IsNullOrEmpty(skeletonName) == true)
                {
                    skeletonName = compositeDrawable.SkeletonName;
                }
            }

            return result;
        }

        public static ModelPreviewAnimation CreateAnimation(ModelPreviewScene scene, Animation animation)
        {
            return CreateAnimation(scene, animation, null);
        }

        public static ModelPreviewCameraAnimation CreateCameraAnimation(Animation animation)
        {
            if (animation == null ||
                string.Equals(animation.AnimationType.ToString().TrimEnd('\0', ' '), "CAM", StringComparison.OrdinalIgnoreCase) == false)
            {
                return null;
            }

            var animationBones = Flatten(new[] { animation }).OfType<Bone>().ToList();
            var externalAnimationData = UncompressAnimationDataBlocks(animation, animationBones.SelectMany(b => b.Children.OfType<AnimationChannel>()));
            foreach (var bone in animationBones)
            {
                foreach (var channel in bone.Children.OfType<AnimationChannel>())
                {
                    channel.ReadFrames(externalAnimationData);
                }
            }

            var channels = animationBones.SelectMany(b => b.Children.OfType<AnimationChannel>()).ToList();
            var preview = new ModelPreviewCameraAnimation
            {
                Name = animation.Name,
                NumFrames = Math.Max(1.0f, animation.NumFrames),
                FrameRate = animation.FrameRate > 0.001f ? animation.FrameRate : 30.0f,
            };

            ReadCameraVectorChannel(channels, "TRAN", out preview.PositionFrames, out preview.Positions);
            ReadCameraVectorChannel(channels, "LOOK", out preview.LookFrames, out preview.Looks);
            ReadCameraVectorChannel(channels, "UP", out preview.UpFrames, out preview.Ups);
            ReadCameraFloatChannel(channels, "FOV", out preview.FovFrames, out preview.Fovs);
            ReadCameraFloatChannel(channels, "NCLP", out preview.NearClipFrames, out preview.NearClips);
            ReadCameraFloatChannel(channels, "FCLP", out preview.FarClipFrames, out preview.FarClips);
            preview.StartFrame = GetCameraAnimationStartFrame(preview);

            return preview.PositionFrames != null ||
                   preview.LookFrames != null ||
                   preview.UpFrames != null ||
                   preview.FovFrames != null ||
                   preview.NearClipFrames != null ||
                   preview.FarClipFrames != null
                       ? preview
                       : null;
        }

        public static ModelPreviewAnimation CreateAnimation(ModelPreviewScene scene, Animation animation, ModelPreviewSceneInstance instance)
        {
            if (scene == null || animation == null)
            {
                return null;
            }

            var animationType = animation.AnimationType.ToString().TrimEnd('\0', ' ');
            bool isVisibilityAnimation = string.Equals(animationType, "PVIS", StringComparison.OrdinalIgnoreCase);
            bool isVrtxAnimation = string.Equals(animationType, "VRTX", StringComparison.OrdinalIgnoreCase);
            if (isVisibilityAnimation == false &&
                isVrtxAnimation == false &&
                (scene.BindBones == null || scene.BindBones.Count == 0))
            {
                return null;
            }

            int boneStart = instance == null ? 0 : Math.Max(0, instance.BoneStart);
            int boneEnd = scene.BindBones == null
                              ? 0
                              : instance == null
                                    ? scene.BindBones.Count
                                    : Math.Min(scene.BindBones.Count, boneStart + Math.Max(0, instance.BoneCount));
            var boneLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (scene.BindBones != null)
            {
                for (int i = boneStart; i < boneEnd; i++)
                {
                    if (string.IsNullOrEmpty(scene.BindBones[i].Name) == false && boneLookup.ContainsKey(scene.BindBones[i].Name) == false)
                    {
                        boneLookup.Add(scene.BindBones[i].Name, i);
                    }
                }
            }

            var previewAnimation = new ModelPreviewAnimation
            {
                Name = animation.Name,
                NumFrames = Math.Max(1.0f, animation.NumFrames),
                FrameRate = animation.FrameRate > 0.001f ? animation.FrameRate : 30.0f,
                Cyclic = animation.Cyclic != 0,
                BoneStart = boneStart,
                BoneCount = Math.Max(0, boneEnd - boneStart),
                InstancePosition = instance == null ? new Vec3() : instance.Position,
                InstanceRotation = instance == null || IsIdentity(instance.Rotation) == true ? new PreviewQuat { W = 1.0f } : instance.Rotation,
            };
            var animationBones = Flatten(new[] { animation }).OfType<Bone>().ToList();
            var externalAnimationData = UncompressAnimationDataBlocks(animation, animationBones.SelectMany(b => b.Children.OfType<AnimationChannel>()));

            foreach (var bone in animationBones)
            {
                foreach (var channel in bone.Children.OfType<AnimationChannel>())
                {
                    channel.ReadFrames(externalAnimationData);
                }
            }

            if (isVrtxAnimation == true)
            {
                AddVrtxMorphChannels(scene, animation, animationBones, previewAnimation);
                AddVrtxVisibilityChannels(scene, animation, animationBones, previewAnimation);
            }

            if (isVisibilityAnimation == true)
            {
                foreach (var bone in animationBones)
                {
                    var visibilityChannel = bone.Children.OfType<VisibilityChannel>().FirstOrDefault();
                    if (visibilityChannel == null ||
                        string.IsNullOrEmpty(bone.Name) == true ||
                        MatchesPreviewPart(scene, bone.Name) == false)
                    {
                        continue;
                    }

                    previewAnimation.VisibilityChannels.Add(new ModelPreviewVisibilityChannel
                    {
                        ObjectName = bone.Name,
                        Frames = visibilityChannel.OrderedFrameKeys,
                        InitialVisible = visibilityChannel.InitialVisibility != 0,
                    });
                }
            }

            previewAnimation.StartFrame = ShouldUseTakeStartFrame(animation.Name) == true
                                              ? GetAnimationStartFrame(animationBones, previewAnimation.NumFrames)
                                              : 0.0f;

            foreach (var bone in animationBones)
            {
                int boneIndex;
                if (string.IsNullOrEmpty(bone.Name) == true || boneLookup.TryGetValue(bone.Name, out boneIndex) == false)
                {
                    continue;
                }

                ushort[] rotationFrames = null;
                PreviewQuat[] rotations = null;
                ushort[] translationFrames = null;
                Vec3[] translations = null;

                var rotationChannel = bone.Children.OfType<AnimationChannel>()
                                                   .Where(c => string.Equals((c.TranslationType ?? string.Empty).TrimEnd('\0', ' '), "ROT", StringComparison.OrdinalIgnoreCase) &&
                                                               c.Frames != null &&
                                                               c.Frames.Count > 0)
                                                   .OrderByDescending(c => c is Quaternion6CompressedChannel ? 6 :
                                                                           c is Quaternion3CompressedChannel ? 3 : 1)
                                                   .FirstOrDefault();
                if (rotationChannel != null)
                {
                    var sourceFrames = rotationChannel.OrderedFrameKeys;
                    rotationFrames = sourceFrames;
                    rotations = sourceFrames.Select(f => ToPreviewQuat(rotationChannel.CalculateValue(f))).ToArray();
                }

                var plainRotation = bone.Children.OfType<BoneRotationData>().FirstOrDefault();
                if (rotations == null && plainRotation != null && plainRotation.Frames != null && plainRotation.Values != null && plainRotation.Values.Length > 0)
                {
                    rotationFrames = plainRotation.Frames;
                    rotations = plainRotation.Values.Select(ToPreviewQuat).ToArray();
                }

                var translation = bone.Children.OfType<BoneTranslationData>().FirstOrDefault();
                if (translation != null && translation.Frames != null && translation.Values != null && translation.Values.Length > 0)
                {
                    translationFrames = translation.Frames;
                    translations = translation.Values.Select(ToPreviewPosition).ToArray();
                }
                else
                {
                    var compressedTranslation = bone.Children.OfType<AnimationChannel>()
                                                            .Where(c => string.Equals((c.TranslationType ?? string.Empty).TrimEnd('\0', ' '), "TRAN", StringComparison.OrdinalIgnoreCase) &&
                                                                        c.Frames != null &&
                                                                        c.Frames.Count > 0)
                                                            .OrderByDescending(c => c is Vector3DOFChannel || c is Vector3DOFCompressedChannel ? 3 :
                                                                                    c is Vector2DOFChannel || c is Vector2DOFCompressedChannel ? 2 : 1)
                                                            .FirstOrDefault();
                    if (compressedTranslation != null)
                    {
                        var sourceFrames = compressedTranslation.OrderedFrameKeys;
                        translationFrames = sourceFrames;
                        translations = sourceFrames.Select(f => ToPreviewPosition(compressedTranslation.CalculateValue(f))).ToArray();
                    }
                }

                if ((rotationFrames == null || rotations == null || rotationFrames.Length == 0 || rotations.Length == 0) &&
                    (translationFrames == null || translations == null || translationFrames.Length == 0 || translations.Length == 0))
                {
                    continue;
                }

                previewAnimation.Channels.Add(new ModelPreviewAnimationChannel
                {
                    BoneIndex = boneIndex,
                    RotationFrames = rotationFrames,
                    Rotations = rotations,
                    TranslationFrames = translationFrames,
                    Translations = translations,
                });
            }

            if (scene.VertexAnimations != null && scene.VertexAnimations.Count > 0 &&
                string.Equals(animation.AnimationType.ToString().TrimEnd('\0', ' '), "EXP", StringComparison.OrdinalIgnoreCase) == true)
            {
                foreach (var bone in animationBones)
                {
                    var stepChannel = bone.Children.OfType<AnimationChannel>()
                                          .FirstOrDefault(c => string.Equals((c.TranslationType ?? string.Empty).TrimEnd('\0', ' '), "STE", StringComparison.OrdinalIgnoreCase) &&
                                                               c.Frames != null &&
                                                               c.Frames.Count > 0);
                    if (stepChannel == null)
                    {
                        continue;
                    }

                    var sourceFrames = stepChannel.OrderedFrameKeys;
                    if (sourceFrames == null || sourceFrames.Length == 0)
                    {
                        continue;
                    }

                    ModelPreviewExpression expression = null;
                    if (string.IsNullOrEmpty(bone.Name) == false && scene.Expressions != null)
                    {
                        scene.Expressions.TryGetValue(bone.Name, out expression);
                    }

                    previewAnimation.VertexChannels.Add(new ModelPreviewVertexAnimationChannel
                    {
                        Name = bone.Name,
                        TargetIndex = bone.GroupId > int.MaxValue ? -1 : (int)bone.GroupId,
                        ExpressionTargets = expression == null ? null : expression.Targets,
                        Frames = sourceFrames,
                        Weights = sourceFrames.Select(f => stepChannel.CalculateValue(f).X).ToArray(),
                    });
                }
            }

            if (previewAnimation.Channels.Count == 0)
            {
                if (isVisibilityAnimation == true && previewAnimation.VisibilityChannels.Count > 0)
                {
                    previewAnimation.UseVisibilityAnimation = true;
                    return previewAnimation;
                }

                if (string.Equals(animationType, "EXP", StringComparison.OrdinalIgnoreCase) == true &&
                    scene.VertexAnimations != null &&
                    scene.VertexAnimations.Count > 0 &&
                    previewAnimation.VertexChannels.Count > 0)
                {
                    previewAnimation.UseVertexAnimation = true;
                    return previewAnimation;
                }

                if (isVrtxAnimation == true &&
                    scene.VertexMorphs != null &&
                    scene.VertexMorphs.Count > 0 &&
                    previewAnimation.VertexMorphChannels.Count > 0)
                {
                    previewAnimation.UseVertexAnimation = true;
                    return previewAnimation;
                }

                if (isVrtxAnimation == true &&
                    previewAnimation.VisibilityChannels.Count > 0)
                {
                    previewAnimation.UseVisibilityAnimation = true;
                    return previewAnimation;
                }

                return null;
            }

            if (scene.VertexAnimations != null && scene.VertexAnimations.Count > 0 &&
                string.Equals(animationType, "EXP", StringComparison.OrdinalIgnoreCase) == true &&
                previewAnimation.VertexChannels.Count > 0)
            {
                previewAnimation.UseVertexAnimation = true;
            }

            if (isVisibilityAnimation == true && previewAnimation.VisibilityChannels.Count > 0)
            {
                previewAnimation.UseVisibilityAnimation = true;
            }

            if (isVrtxAnimation == true && previewAnimation.VertexMorphChannels.Count > 0)
            {
                previewAnimation.UseVertexAnimation = true;
            }

            if (isVrtxAnimation == true && previewAnimation.VisibilityChannels.Count > 0)
            {
                previewAnimation.UseVisibilityAnimation = true;
            }

            return previewAnimation;
        }

        private static float GetAnimationStartFrame(IEnumerable<Bone> bones, float numFrames)
        {
            ushort minFrame = ushort.MaxValue;
            bool hasFrames = false;

            foreach (var bone in bones)
            {
                foreach (var channel in bone.Children.OfType<AnimationChannel>())
                {
                    AddMinFrame(channel.OrderedFrameKeys, ref minFrame, ref hasFrames);
                }

                var rotation = bone.Children.OfType<BoneRotationData>().FirstOrDefault();
                if (rotation != null)
                {
                    AddMinFrame(rotation.Frames, ref minFrame, ref hasFrames);
                }

                var translation = bone.Children.OfType<BoneTranslationData>().FirstOrDefault();
                if (translation != null)
                {
                    AddMinFrame(translation.Frames, ref minFrame, ref hasFrames);
                }
            }

            if (hasFrames == false || minFrame == 0)
            {
                return 0.0f;
            }

            return minFrame;
        }

        private static float GetCameraAnimationStartFrame(ModelPreviewCameraAnimation animation)
        {
            ushort minFrame = ushort.MaxValue;
            bool hasFrames = false;
            AddMinFrame(animation.PositionFrames, ref minFrame, ref hasFrames);
            AddMinFrame(animation.LookFrames, ref minFrame, ref hasFrames);
            AddMinFrame(animation.UpFrames, ref minFrame, ref hasFrames);
            AddMinFrame(animation.FovFrames, ref minFrame, ref hasFrames);
            AddMinFrame(animation.NearClipFrames, ref minFrame, ref hasFrames);
            AddMinFrame(animation.FarClipFrames, ref minFrame, ref hasFrames);
            return hasFrames == false || minFrame == 0 ? 0.0f : minFrame;
        }

        private static bool ShouldUseTakeStartFrame(string animationName)
        {
            if (string.IsNullOrEmpty(animationName) == true)
            {
                return false;
            }

            var name = animationName.TrimEnd('\0', ' ');
            return name.StartsWith("PTRN_", StringComparison.OrdinalIgnoreCase) == true &&
                   name.IndexOf("_Take ", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool MatchesPreviewPart(ModelPreviewScene scene, string objectName)
        {
            if (scene == null || scene.Parts == null || string.IsNullOrEmpty(objectName) == true)
            {
                return false;
            }

            foreach (var part in scene.Parts)
            {
                if (PartMatchesObject(part, objectName) == true)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool PartMatchesObject(ModelPreviewPart part, string objectName)
        {
            if (string.IsNullOrEmpty(objectName) == true)
            {
                return false;
            }

            if (string.Equals(part.ObjectName, objectName, StringComparison.OrdinalIgnoreCase) == true ||
                string.Equals(part.Name, objectName, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            return string.IsNullOrEmpty(part.Name) == false &&
                   part.Name.StartsWith(objectName + "_", StringComparison.OrdinalIgnoreCase) == true;
        }

        private static void AddVrtxMorphChannels(
            ModelPreviewScene scene,
            Animation animation,
            List<Bone> animationBones,
            ModelPreviewAnimation previewAnimation)
        {
            if (scene == null || scene.VertexMorphs == null || scene.VertexMorphs.Count == 0 || animation == null || previewAnimation == null)
            {
                return;
            }

            var shapeNames = new List<string>();
            var animationShapeName = GetVrtxShapeName(animation.Name);
            if (string.IsNullOrEmpty(animationShapeName) == false)
            {
                shapeNames.Add(animationShapeName);
            }

            foreach (var bone in animationBones)
            {
                if (string.IsNullOrEmpty(bone.Name) == false &&
                    shapeNames.Contains(bone.Name, StringComparer.OrdinalIgnoreCase) == false)
                {
                    shapeNames.Add(bone.Name);
                }
            }

            foreach (var shapeName in shapeNames)
            {
                var morph = scene.VertexMorphs.FirstOrDefault(m => MatchesShapeName(m.InitialShapeName, shapeName));
                if (morph == null)
                {
                    continue;
                }

                if (previewAnimation.VertexMorphChannels.Any(c => MatchesShapeName(c.InitialShapeName, morph.InitialShapeName)) == true)
                {
                    continue;
                }

                ushort[] frames;
                float[] weights;
                var channel = animationBones.SelectMany(b => b.Children.OfType<VertexMorphChannel>()).FirstOrDefault();
                if (TryGetVrtxChannelKeys(channel, previewAnimation.NumFrames, out frames, out weights) == false)
                {
                    frames = new[] { (ushort)0, (ushort)Math.Max(1, Math.Min(ushort.MaxValue, (int)Math.Ceiling(previewAnimation.NumFrames) - 1)) };
                    weights = new[] { 0.0f, 1.0f };
                }

                previewAnimation.VertexMorphChannels.Add(new ModelPreviewVertexMorphChannel
                {
                    InitialShapeName = morph.InitialShapeName,
                    Frames = frames,
                    Weights = weights,
                });
            }
        }

        private static void AddVrtxVisibilityChannels(
            ModelPreviewScene scene,
            Animation animation,
            List<Bone> animationBones,
            ModelPreviewAnimation previewAnimation)
        {
            if (scene == null || scene.Parts == null || animation == null || previewAnimation == null)
            {
                return;
            }

            var shapeName = GetVrtxShapeName(animation.Name);
            if (string.IsNullOrEmpty(shapeName) == true ||
                shapeName.EndsWith("InitialShape", StringComparison.OrdinalIgnoreCase) == false)
            {
                return;
            }

            var initialPart = scene.Parts.FirstOrDefault(p => MatchesShapeName(p.ObjectName, shapeName));
            if (string.IsNullOrEmpty(initialPart.ObjectName) == true)
            {
                return;
            }

            var damagedObjectName = initialPart.ObjectName.Substring(0, initialPart.ObjectName.Length - "InitialShape".Length) + "DamagedShape";
            if (scene.Parts.Any(p => PartMatchesObject(p, damagedObjectName)) == false)
            {
                return;
            }

            var channel = animationBones.SelectMany(b => b.Children.OfType<VertexMorphChannel>()).FirstOrDefault();
            ushort[] frames;
            if (TryGetVrtxVisibilityFrames(channel, out frames) == false)
            {
                frames = new[] { (ushort)1 };
            }

            previewAnimation.VisibilityChannels.Add(new ModelPreviewVisibilityChannel
            {
                ObjectName = initialPart.ObjectName,
                Frames = frames,
                InitialVisible = true,
            });
            previewAnimation.VisibilityChannels.Add(new ModelPreviewVisibilityChannel
            {
                ObjectName = damagedObjectName,
                Frames = frames,
                InitialVisible = false,
            });
        }

        public static void ApplyCameraAnimationFrame(ModelPreviewCamera camera, ModelPreviewCameraAnimation animation, float frame)
        {
            if (camera == null || animation == null)
            {
                return;
            }

            if (animation.PositionFrames != null && animation.Positions != null && animation.PositionFrames.Length > 0 && animation.Positions.Length > 0)
            {
                camera.CutscenePosition = SampleVector(animation.PositionFrames, animation.Positions, frame);
                camera.HasCutsceneCamera = true;
            }

            if (animation.LookFrames != null && animation.Looks != null && animation.LookFrames.Length > 0 && animation.Looks.Length > 0)
            {
                camera.CutsceneLook = SampleVector(animation.LookFrames, animation.Looks, frame);
                camera.HasCutsceneCamera = true;
            }

            if (animation.UpFrames != null && animation.Ups != null && animation.UpFrames.Length > 0 && animation.Ups.Length > 0)
            {
                camera.CutsceneUp = SampleVector(animation.UpFrames, animation.Ups, frame);
                camera.HasCutsceneCamera = true;
            }

            if (animation.FovFrames != null && animation.Fovs != null && animation.FovFrames.Length > 0 && animation.Fovs.Length > 0)
            {
                camera.CutsceneFov = NormalizeFovDegrees(SampleFloat(animation.FovFrames, animation.Fovs, frame));
                camera.HasCutsceneCamera = true;
            }

            if (animation.NearClipFrames != null && animation.NearClips != null && animation.NearClipFrames.Length > 0 && animation.NearClips.Length > 0)
            {
                camera.CutsceneNearClip = SampleFloat(animation.NearClipFrames, animation.NearClips, frame);
                camera.HasCutsceneCamera = true;
            }

            if (animation.FarClipFrames != null && animation.FarClips != null && animation.FarClipFrames.Length > 0 && animation.FarClips.Length > 0)
            {
                camera.CutsceneFarClip = SampleFloat(animation.FarClipFrames, animation.FarClips, frame);
                camera.HasCutsceneCamera = true;
            }
        }

        public static float NormalizeFovDegrees(float value)
        {
            if (float.IsNaN(value) == true || value <= 0.001f)
            {
                return 45.0f;
            }

            return value <= Math.PI * 2.0f
                       ? value * (180.0f / (float)Math.PI)
                       : value;
        }

        private static void ReadCameraVectorChannel(IEnumerable<AnimationChannel> channels, string type, out ushort[] frames, out Vec3[] values)
        {
            frames = null;
            values = null;
            var channel = SelectCameraChannel(channels, type);
            if (channel == null)
            {
                return;
            }

            frames = channel.OrderedFrameKeys;
            if (frames == null || frames.Length == 0)
            {
                frames = null;
                return;
            }

            values = frames.Select(f => ToPreviewPosition(channel.CalculateValue(f))).ToArray();
        }

        private static void ReadCameraFloatChannel(IEnumerable<AnimationChannel> channels, string type, out ushort[] frames, out float[] values)
        {
            frames = null;
            values = null;
            var channel = SelectCameraChannel(channels, type);
            if (channel == null)
            {
                return;
            }

            frames = channel.OrderedFrameKeys;
            if (frames == null || frames.Length == 0)
            {
                frames = null;
                return;
            }

            values = frames.Select(f => channel.CalculateValue(f).X).ToArray();
        }

        private static AnimationChannel SelectCameraChannel(IEnumerable<AnimationChannel> channels, string type)
        {
            if (channels == null)
            {
                return null;
            }

            return channels.Where(c => string.Equals((c.TranslationType ?? string.Empty).TrimEnd('\0', ' '), type, StringComparison.OrdinalIgnoreCase) &&
                                       c.Frames != null &&
                                       c.Frames.Count > 0)
                           .OrderByDescending(c => c is Vector3DOFChannel || c is Vector3DOFCompressedChannel ? 3 :
                                                   c is Vector2DOFChannel || c is Vector2DOFCompressedChannel ? 2 : 1)
                           .FirstOrDefault();
        }

        private static Vec3 SampleVector(ushort[] frames, Vec3[] values, float frame)
        {
            if (frames == null || values == null || frames.Length == 0 || values.Length == 0)
            {
                return new Vec3();
            }

            int count = Math.Min(frames.Length, values.Length);
            if (frame <= frames[0])
            {
                return values[0];
            }

            if (frame >= frames[count - 1])
            {
                return values[count - 1];
            }

            for (int i = 0; i + 1 < count; i++)
            {
                if (frame < frames[i] || frame > frames[i + 1])
                {
                    continue;
                }

                float length = Math.Max(0.000001f, frames[i + 1] - frames[i]);
                float t = (frame - frames[i]) / length;
                return values[i] + (values[i + 1] - values[i]) * t;
            }

            return values[count - 1];
        }

        private static float SampleFloat(ushort[] frames, float[] values, float frame)
        {
            if (frames == null || values == null || frames.Length == 0 || values.Length == 0)
            {
                return 0.0f;
            }

            int count = Math.Min(frames.Length, values.Length);
            if (frame <= frames[0])
            {
                return values[0];
            }

            if (frame >= frames[count - 1])
            {
                return values[count - 1];
            }

            for (int i = 0; i + 1 < count; i++)
            {
                if (frame < frames[i] || frame > frames[i + 1])
                {
                    continue;
                }

                float length = Math.Max(0.000001f, frames[i + 1] - frames[i]);
                float t = (frame - frames[i]) / length;
                return values[i] + (values[i + 1] - values[i]) * t;
            }

            return values[count - 1];
        }

        private static bool TryGetVrtxChannelKeys(VertexMorphChannel channel, float animationFrames, out ushort[] frames, out float[] weights)
        {
            frames = null;
            weights = null;
            if (channel == null || channel.Keys == null || channel.Keys.Length < 2)
            {
                return false;
            }

            var keys = channel.Keys.OrderBy(k => k.Frame).ToArray();
            if (keys.Select(k => k.Frame).Distinct().Count() < 2)
            {
                return false;
            }

            var max = Math.Max(1, keys.Max(k => (int)k.Value));
            frames = keys.Select(k => k.Frame).ToArray();
            weights = keys.Select(k => Math.Max(0.0f, Math.Min(1.0f, k.Value / (float)max))).ToArray();
            return true;
        }

        private static bool TryGetVrtxVisibilityFrames(VertexMorphChannel channel, out ushort[] frames)
        {
            frames = null;
            if (channel == null || channel.Keys == null || channel.Keys.Length < 2)
            {
                return false;
            }

            var keys = channel.Keys.OrderBy(k => k.Frame).ToArray();
            var toggles = new List<ushort>();
            bool visible = keys[0].Value == 0;
            for (int i = 1; i < keys.Length; i++)
            {
                bool nextVisible = keys[i].Value == 0;
                if (nextVisible == visible)
                {
                    continue;
                }

                toggles.Add(keys[i].Frame);
                visible = nextVisible;
            }

            if (toggles.Count == 0)
            {
                return false;
            }

            frames = toggles.ToArray();
            return true;
        }

        private static string GetVrtxShapeName(string animationName)
        {
            if (string.IsNullOrEmpty(animationName) == true)
            {
                return null;
            }

            var name = animationName.TrimEnd('\0', ' ');
            return name.StartsWith("VRTX_", StringComparison.OrdinalIgnoreCase) == true
                       ? name.Substring(5)
                       : name;
        }

        private static bool MatchesShapeName(string left, string right)
        {
            if (string.IsNullOrEmpty(left) == true || string.IsNullOrEmpty(right) == true)
            {
                return false;
            }

            left = left.TrimEnd('\0', ' ');
            right = right.TrimEnd('\0', ' ');
            if (string.Equals(left, right, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            return left.EndsWith("_" + right, StringComparison.OrdinalIgnoreCase) == true ||
                   right.EndsWith("_" + left, StringComparison.OrdinalIgnoreCase) == true;
        }

        private static void AddMinFrame(ushort[] frames, ref ushort minFrame, ref bool hasFrames)
        {
            if (frames == null || frames.Length == 0)
            {
                return;
            }

            foreach (var frame in frames)
            {
                minFrame = Math.Min(minFrame, frame);
                hasFrames = true;
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

        public static bool ApplyAnimation(ModelPreviewScene scene, ModelPreviewAnimation animation, float seconds)
        {
            Vec3 rootDelta;
            return ApplyAnimation(scene, animation, seconds, true, new Vec3(), out rootDelta);
        }

        public static bool ApplyAnimation(ModelPreviewScene scene, ModelPreviewAnimation animation, float seconds, bool rootMotion, Vec3 rootOffset, out Vec3 rootDelta)
        {
            if (scene == null || animation == null)
            {
                rootDelta = new Vec3();
                return false;
            }

            float frame = seconds * animation.FrameRate;
            bool active = true;
            if (animation.Cyclic == true)
            {
                frame = frame % animation.NumFrames;
                if (frame < 0.0f)
                {
                    frame += animation.NumFrames;
                }
            }
            else if (frame >= animation.NumFrames)
            {
                frame = animation.NumFrames - 1.0f;
                active = false;
            }

            frame += animation.StartFrame;
            if (animation.UseVisibilityAnimation == true)
            {
                ApplyVisibilityAnimation(scene, animation, frame);
            }

            ResetVertexAnimationBindVertices(scene, scene.BindBones == null || scene.BindBones.Count == 0);

            if (scene.BindBones == null || scene.BindBones.Count == 0)
            {
                rootDelta = new Vec3();
                return active;
            }

            scene.Bones = BuildAnimationPose(scene, animation, frame, rootMotion, rootOffset, true);
            rootDelta = GetRootDelta(scene);
            if (rootMotion == true)
            {
                rootDelta -= rootOffset;
            }
            if (scene.UseGpuSkinning == false)
            {
                SkinVertices(scene);
            }
            else
            {
                SkinGpuFallbackVertices(scene);
            }
            ApplyInstanceTransformsToRigidVertices(scene);
            return active;
        }

        public static void BeginCutsceneAnimationFrame(ModelPreviewScene scene)
        {
            if (scene == null)
            {
                return;
            }

            SetAllPartsHidden(scene, false);
            if (scene.BindBones != null)
            {
                scene.Bones = CloneBones(scene.BindBones);
            }

            ResetVertexAnimationBindVertices(scene, false);
        }

        public static bool ApplyAnimationToCurrentPose(ModelPreviewScene scene, ModelPreviewAnimation animation, float seconds, bool rootMotion, Vec3 rootOffset, out Vec3 rootDelta)
        {
            if (scene == null || animation == null)
            {
                rootDelta = new Vec3();
                return false;
            }

            float frame = seconds * animation.FrameRate;
            bool active = true;
            if (animation.Cyclic == true)
            {
                frame = frame % animation.NumFrames;
                if (frame < 0.0f)
                {
                    frame += animation.NumFrames;
                }
            }
            else if (frame >= animation.NumFrames)
            {
                frame = animation.NumFrames - 1.0f;
                active = false;
            }

            frame += animation.StartFrame;
            return ApplyAnimationFrameToCurrentPose(scene, animation, frame, rootMotion, rootOffset, out rootDelta) && active;
        }

        public static bool ApplyAnimationFrameToCurrentPose(ModelPreviewScene scene, ModelPreviewAnimation animation, float frame, bool rootMotion, Vec3 rootOffset, out Vec3 rootDelta)
        {
            return ApplyAnimationFrameToCurrentPose(scene, animation, frame, rootMotion, rootOffset, true, out rootDelta);
        }

        public static bool ApplyAnimationFrameToCurrentPose(ModelPreviewScene scene, ModelPreviewAnimation animation, float frame, bool rootMotion, Vec3 rootOffset, bool normalizeRootMotion, out Vec3 rootDelta)
        {
            if (scene == null || animation == null)
            {
                rootDelta = new Vec3();
                return false;
            }

            if (animation.UseVisibilityAnimation == true)
            {
                ApplyVisibilityAnimation(scene, animation, frame);
            }

            if (scene.BindBones == null || scene.BindBones.Count == 0 || scene.Bones == null || scene.Bones.Count == 0)
            {
                rootDelta = new Vec3();
                return true;
            }

            ApplyAnimationChannelsToPose(scene, scene.Bones, animation, frame, rootMotion, rootOffset, normalizeRootMotion);
            ComposeBones(scene.Bones);
            rootDelta = GetRootDelta(scene, animation);
            if (rootMotion == true)
            {
                rootDelta -= rootOffset;
            }

            return true;
        }

        public static bool ApplyCutsceneAnimationFrameToCurrentPose(ModelPreviewScene scene, ModelPreviewAnimation animation, float frame, bool rootMotion, Vec3 rootOffset, bool normalizeRootMotion, out Vec3 rootDelta)
        {
            if (scene == null || animation == null)
            {
                rootDelta = new Vec3();
                return false;
            }

            if (animation.UseVisibilityAnimation == true)
            {
                ApplyVisibilityAnimation(scene, animation, frame);
            }

            if (scene.BindBones == null || scene.BindBones.Count == 0 || scene.Bones == null || scene.Bones.Count == 0)
            {
                rootDelta = new Vec3();
                return true;
            }

            ApplyAnimationChannelsToPose(scene, scene.Bones, animation, frame, rootMotion, rootOffset, normalizeRootMotion);
            rootDelta = new Vec3();
            return true;
        }

        public static void FinishCutsceneAnimationFrame(ModelPreviewScene scene)
        {
            FinishCutsceneAnimationFrame(scene, null);
        }

        public static void FinishCutsceneAnimationFrame(ModelPreviewScene scene, IEnumerable<ModelPreviewSceneInstance> skinnedInstances)
        {
            if (scene == null)
            {
                return;
            }

            ApplyInstanceTransformsToPose(scene);
            if (scene.Bones != null && scene.Bones.Count > 0)
            {
                ComposeBones(scene.Bones);
            }

            if (scene.UseGpuSkinning == false)
            {
                SkinVertices(scene, skinnedInstances);
            }
            else
            {
                SkinGpuFallbackVertices(scene, skinnedInstances);
            }
            ApplyInstanceTransformsToRigidVertices(scene);
        }

        public static void ApplyInstanceTransformsToBindPose(ModelPreviewScene scene)
        {
            if (scene == null)
            {
                return;
            }

            if (scene.BindBones != null)
            {
                scene.Bones = CloneBones(scene.BindBones);
                ApplyInstanceTransformsToPose(scene);
                ComposeBones(scene.Bones);
            }

            if (scene.UseGpuSkinning == false)
            {
                SkinVertices(scene);
            }
            else
            {
                SkinGpuFallbackVertices(scene);
            }
        }

        private static void ApplyInstanceTransformsToPose(ModelPreviewScene scene)
        {
            if (scene == null || scene.Instances == null || scene.Instances.Count == 0 || scene.Bones == null || scene.Bones.Count == 0)
            {
                return;
            }

            foreach (var instance in scene.Instances)
            {
                if (instance == null)
                {
                    continue;
                }

                var instancePosition = GetEffectiveInstancePosition(scene, instance);
                var instanceRotation = GetEffectiveInstanceRotation(scene, instance);
                if (LengthSquared(instancePosition) <= 0.000001f && IsIdentity(instanceRotation) == true)
                {
                    continue;
                }

                int start = Math.Max(0, instance.BoneStart);
                int end = instance.BoneCount <= 0 ? scene.Bones.Count : Math.Min(scene.Bones.Count, start + instance.BoneCount);
                var rotation = IsIdentity(instanceRotation) == true ? new PreviewQuat { W = 1.0f } : Normalize(instanceRotation);
                for (int i = start; i < end; i++)
                {
                    if (scene.Bones[i].Parent >= start && scene.Bones[i].Parent < end)
                    {
                        continue;
                    }

                    var bone = scene.Bones[i];
                    bone.LocalPosition = Rotate(rotation, bone.LocalPosition) + instancePosition;
                    bone.LocalAxisX = Normalize(Rotate(rotation, bone.LocalAxisX));
                    bone.LocalAxisY = Normalize(Rotate(rotation, bone.LocalAxisY));
                    bone.LocalAxisZ = Normalize(Rotate(rotation, bone.LocalAxisZ));
                    scene.Bones[i] = bone;
                    break;
                }
            }
        }

        private static void ApplyInstanceTransformsToRigidVertices(ModelPreviewScene scene)
        {
            if (scene == null || scene.Instances == null || scene.Instances.Count == 0 || scene.Vertices == null || scene.Vertices.Count == 0)
            {
                return;
            }

            foreach (var instance in scene.Instances)
            {
                if (instance == null)
                {
                    continue;
                }

                var instancePosition = GetEffectiveInstancePosition(scene, instance);
                var instanceRotation = GetEffectiveInstanceRotation(scene, instance);
                if (instance.BoneCount > 0 ||
                    (LengthSquared(instancePosition) <= 0.000001f && IsIdentity(instanceRotation) == true))
                {
                    continue;
                }

                int start = Math.Max(0, instance.VertexStart);
                int end = Math.Min(scene.Vertices.Count, start + Math.Max(0, instance.VertexCount));
                var rotation = IsIdentity(instanceRotation) == true ? new PreviewQuat { W = 1.0f } : Normalize(instanceRotation);
                for (int i = start; i < end; i++)
                {
                    var vertex = scene.BindVertices != null && i < scene.BindVertices.Count
                                     ? scene.BindVertices[i]
                                     : scene.Vertices[i];
                    vertex.Position = Rotate(rotation, vertex.Position) + instancePosition;
                    vertex.Normal = Normalize(Rotate(rotation, vertex.Normal));
                    scene.Vertices[i] = vertex;
                }
            }
        }

        private static Vec3 GetEffectiveInstancePosition(ModelPreviewScene scene, ModelPreviewSceneInstance instance)
        {
            if (scene == null || instance == null || instance.IsStage == false)
            {
                return instance == null ? new Vec3() : instance.Position;
            }

            var stageRotation = CreateEulerDegreesRotation(scene.StageDebugRotationDegrees);
            return Rotate(stageRotation, instance.Position) + scene.StageDebugPositionOffset;
        }

        private static PreviewQuat GetEffectiveInstanceRotation(ModelPreviewScene scene, ModelPreviewSceneInstance instance)
        {
            if (scene == null || instance == null || instance.IsStage == false)
            {
                return instance == null ? new PreviewQuat { W = 1.0f } : instance.Rotation;
            }

            return Multiply(CreateEulerDegreesRotation(scene.StageDebugRotationDegrees), instance.Rotation);
        }

        private static void ApplyVisibilityAnimation(ModelPreviewScene scene, ModelPreviewAnimation animation, float frame)
        {
            if (scene == null || scene.Parts == null || animation == null || animation.VisibilityChannels == null)
            {
                return;
            }

            SetAllPartsHidden(scene, false);
            foreach (var channel in animation.VisibilityChannels)
            {
                bool visible = SampleVisibility(channel, frame);
                for (int i = 0; i < scene.Parts.Count; i++)
                {
                    var part = scene.Parts[i];
                    if (PartMatchesObject(part, channel.ObjectName) == false)
                    {
                        continue;
                    }

                    part.Hidden = !visible;
                    scene.Parts[i] = part;
                }
            }
        }

        private static bool SampleVisibility(ModelPreviewVisibilityChannel channel, float frame)
        {
            bool visible = channel == null ? true : channel.InitialVisible;
            if (channel == null || channel.Frames == null)
            {
                return visible;
            }

            foreach (var key in channel.Frames)
            {
                if (frame < key)
                {
                    break;
                }

                visible = !visible;
            }

            return visible;
        }

        private static void SetAllPartsHidden(ModelPreviewScene scene, bool hidden)
        {
            if (scene == null || scene.Parts == null)
            {
                return;
            }

            for (int i = 0; i < scene.Parts.Count; i++)
            {
                var part = scene.Parts[i];
                part.Hidden = hidden;
                scene.Parts[i] = part;
            }
        }

        public static bool ApplyVertexAnimation(ModelPreviewScene scene, ModelPreviewAnimation animation, float seconds)
        {
            if (scene == null || animation == null)
            {
                return false;
            }

            bool hasExpressionChannels = animation.VertexChannels != null &&
                                         animation.VertexChannels.Count > 0 &&
                                         scene.VertexAnimations != null &&
                                         scene.VertexAnimations.Count > 0;
            bool hasMorphChannels = animation.VertexMorphChannels != null &&
                                    animation.VertexMorphChannels.Count > 0 &&
                                    scene.VertexMorphs != null &&
                                    scene.VertexMorphs.Count > 0;
            if (hasExpressionChannels == false && hasMorphChannels == false)
            {
                return false;
            }

            var source = scene.VertexAnimationBindVertices != null && scene.VertexAnimationBindVertices.Count == scene.BindVertices.Count
                             ? scene.VertexAnimationBindVertices
                             : scene.BindVertices;
            if (source == null || source.Count == 0)
            {
                return false;
            }

            var bindVertices = source.ToList();
            float frame = Math.Max(0.0f, seconds * animation.FrameRate) + animation.StartFrame;
            if (hasMorphChannels == true)
            {
                foreach (var channel in animation.VertexMorphChannels)
                {
                    float weight = Math.Max(0.0f, Math.Min(1.0f, SampleVertexAnimationWeight(channel, frame)));
                    if (weight <= 0.0001f)
                    {
                        continue;
                    }

                    foreach (var morph in scene.VertexMorphs.Where(m => MatchesShapeName(m.InitialShapeName, channel.InitialShapeName)))
                    {
                        ApplyVertexMorphTarget(bindVertices, morph, weight);
                    }
                }
            }

            if (hasExpressionChannels == true)
            {
                foreach (var vertexAnimation in scene.VertexAnimations)
                {
                    if (vertexAnimation.Targets == null || vertexAnimation.Targets.Count == 0)
                    {
                        continue;
                    }

                    foreach (var channel in animation.VertexChannels)
                    {
                        float weight = SampleVertexAnimationWeight(channel, frame);
                        if (Math.Abs(weight) <= 0.0001f)
                        {
                            continue;
                        }

                        var targetWeights = ResolveVertexAnimationTargetWeights(channel, weight);
                        foreach (var targetWeight in targetWeights)
                        {
                            var target = FindVertexAnimationTarget(vertexAnimation, targetWeight.TargetIndex);
                            if (target == null)
                            {
                                continue;
                            }

                            ApplyVertexAnimationTarget(
                                bindVertices,
                                vertexAnimation,
                                target,
                                Math.Max(0.0f, Math.Min(1.0f, targetWeight.Value)));
                        }
                    }
                }
            }

            scene.BindVertices = bindVertices;
            if (scene.Bones != null && scene.Bones.Count > 0 && scene.BindBones != null && scene.BindBones.Count > 0)
            {
                if (scene.UseGpuSkinning == false)
                {
                    SkinVertices(scene);
                }
                else
                {
                    scene.Vertices = bindVertices.ToList();
                }
            }
            else
            {
                scene.Vertices = bindVertices.ToList();
            }

            return true;
        }

        private static IEnumerable<ModelPreviewExpressionTarget> ResolveVertexAnimationTargetWeights(ModelPreviewVertexAnimationChannel channel, float value)
        {
            if (channel == null)
            {
                yield break;
            }

            if (channel.ExpressionTargets == null || channel.ExpressionTargets.Count == 0)
            {
                yield return new ModelPreviewExpressionTarget
                {
                    TargetIndex = channel.TargetIndex,
                    Value = value,
                };
                yield break;
            }

            var targets = channel.ExpressionTargets
                                 .Where(t => t != null)
                                 .OrderBy(t => t.Value)
                                 .ToList();
            if (targets.Count == 0)
            {
                yield break;
            }

            if (value <= 0.0f)
            {
                yield break;
            }

            if (targets.Count == 1)
            {
                yield return new ModelPreviewExpressionTarget
                {
                    TargetIndex = targets[0].TargetIndex,
                    Value = targets[0].Value <= 0.0001f ? value : value / targets[0].Value,
                };
                yield break;
            }

            if (value <= targets[0].Value)
            {
                yield return new ModelPreviewExpressionTarget
                {
                    TargetIndex = targets[0].TargetIndex,
                    Value = targets[0].Value <= 0.0001f ? 1.0f : value / targets[0].Value,
                };
                yield break;
            }

            for (int i = 1; i < targets.Count; i++)
            {
                if (value > targets[i].Value)
                {
                    continue;
                }

                float span = Math.Max(0.0001f, targets[i].Value - targets[i - 1].Value);
                float t = (value - targets[i - 1].Value) / span;
                yield return new ModelPreviewExpressionTarget
                {
                    TargetIndex = targets[i - 1].TargetIndex,
                    Value = 1.0f - t,
                };
                yield return new ModelPreviewExpressionTarget
                {
                    TargetIndex = targets[i].TargetIndex,
                    Value = t,
                };
                yield break;
            }

            yield return new ModelPreviewExpressionTarget
            {
                TargetIndex = targets[targets.Count - 1].TargetIndex,
                Value = 1.0f,
            };
        }

        private static ModelPreviewVertexAnimationTarget FindVertexAnimationTarget(ModelPreviewVertexAnimation animation, int targetIndex)
        {
            if (animation == null || animation.Targets == null || targetIndex < 0)
            {
                return null;
            }

            return animation.Targets.FirstOrDefault(t => t.SourceIndex == targetIndex);
        }

        private static float SampleVertexAnimationWeight(ModelPreviewVertexAnimationChannel channel, float frame)
        {
            if (channel == null || channel.Frames == null || channel.Weights == null || channel.Frames.Length == 0 || channel.Weights.Length == 0)
            {
                return 0.0f;
            }

            if (channel.Frames.Length == 1 || channel.Weights.Length == 1)
            {
                return channel.Weights[0];
            }

            int next = 0;
            while (next + 1 < channel.Frames.Length && channel.Frames[next + 1] <= frame)
            {
                next++;
            }

            if (next + 1 >= channel.Frames.Length)
            {
                return channel.Weights[Math.Min(next, channel.Weights.Length - 1)];
            }

            float span = Math.Max(0.0001f, channel.Frames[next + 1] - channel.Frames[next]);
            float t = (frame - channel.Frames[next]) / span;
            return channel.Weights[next] + (channel.Weights[next + 1] - channel.Weights[next]) * t;
        }

        private static float SampleVertexAnimationWeight(ModelPreviewVertexMorphChannel channel, float frame)
        {
            if (channel == null || channel.Frames == null || channel.Weights == null || channel.Frames.Length == 0 || channel.Weights.Length == 0)
            {
                return 0.0f;
            }

            if (channel.Frames.Length == 1 || channel.Weights.Length == 1)
            {
                return channel.Weights[0];
            }

            int next = 0;
            while (next + 1 < channel.Frames.Length && channel.Frames[next + 1] <= frame)
            {
                next++;
            }

            if (next + 1 >= channel.Frames.Length)
            {
                return channel.Weights[Math.Min(next, channel.Weights.Length - 1)];
            }

            float span = Math.Max(0.0001f, channel.Frames[next + 1] - channel.Frames[next]);
            float t = (frame - channel.Frames[next]) / span;
            return channel.Weights[next] + (channel.Weights[next + 1] - channel.Weights[next]) * t;
        }

        private static void ApplyVertexAnimationTarget(List<ModelPreviewVertex> vertices, ModelPreviewVertexAnimation animation, ModelPreviewVertexAnimationTarget target, float weight)
        {
            if (vertices == null || animation == null || target == null || weight <= 0.0f)
            {
                return;
            }

            int count = Math.Min(animation.VertexCount, Math.Min(target.Positions.Length, target.Normals.Length));
            for (int i = 0; i < count; i++)
            {
                int vertexIndex = animation.VertexStart + i;
                if (vertexIndex < 0 || vertexIndex >= vertices.Count)
                {
                    continue;
                }

                var vertex = vertices[vertexIndex];
                if (target.Positions[i].HasValue == true)
                {
                    vertex.Position = Lerp(vertex.Position, target.Positions[i].Value, weight);
                }

                if (target.Normals[i].HasValue == true)
                {
                    vertex.Normal = Normalize(Lerp(vertex.Normal, target.Normals[i].Value, weight));
                }

                vertices[vertexIndex] = vertex;
            }
        }

        private static void ApplyVertexMorphTarget(List<ModelPreviewVertex> vertices, ModelPreviewVertexMorph morph, float weight)
        {
            if (vertices == null || morph == null || morph.Positions == null || weight <= 0.0f)
            {
                return;
            }

            int count = Math.Min(morph.VertexCount, morph.Positions.Length);
            for (int i = 0; i < count; i++)
            {
                int vertexIndex = morph.VertexStart + i;
                if (vertexIndex < 0 || vertexIndex >= vertices.Count)
                {
                    continue;
                }

                var vertex = vertices[vertexIndex];
                vertex.Position = Lerp(vertex.Position, morph.Positions[i], weight);
                if (morph.Normals != null && i < morph.Normals.Length)
                {
                    vertex.Normal = Normalize(Lerp(vertex.Normal, morph.Normals[i], weight));
                }

                vertices[vertexIndex] = vertex;
            }
        }

        private static Vec3 GetRootDelta(ModelPreviewScene scene)
        {
            return GetRootDelta(scene, null);
        }

        private static Vec3 GetRootDelta(ModelPreviewScene scene, ModelPreviewAnimation animation)
        {
            if (scene == null || scene.Bones == null || scene.BindBones == null)
            {
                return new Vec3();
            }

            int start = animation == null ? 0 : Math.Max(0, animation.BoneStart);
            int end = animation == null || animation.BoneCount <= 0
                          ? Math.Min(scene.Bones.Count, scene.BindBones.Count)
                          : Math.Min(Math.Min(scene.Bones.Count, scene.BindBones.Count), start + animation.BoneCount);
            for (int i = start; i < end; i++)
            {
                if (scene.Bones[i].Parent < 0)
                {
                    return scene.Bones[i].Position - scene.BindBones[i].Position;
                }
            }

            return new Vec3();
        }

        private static bool HasRigidGeometryVertices(ModelPreviewScene scene)
        {
            return scene != null &&
                   scene.BindVertices != null &&
                   scene.BindVertices.Any(v => v.UseRigidBoneOrigin == true);
        }

        private static void ApplyRigidGeometryBindPose(ModelPreviewScene scene)
        {
            if (HasRigidGeometryVertices(scene) == false ||
                scene.BindBones == null ||
                scene.BindBones.Count == 0)
            {
                return;
            }

            scene.Bones = CloneBones(scene.BindBones);
            if (scene.UseGpuSkinning == false)
            {
                SkinVertices(scene);
            }
        }

        private static List<ModelPreviewVertex> BuildExportVertices(ModelPreviewScene scene)
        {
            var source = scene != null && scene.BindVertices != null && scene.BindVertices.Count > 0
                             ? scene.BindVertices
                             : scene == null ? null : scene.Vertices;
            if (source == null)
            {
                return null;
            }

            var vertices = source.ToList();
            if (HasRigidGeometryVertices(scene) == false ||
                scene.BindBones == null ||
                scene.BindBones.Count == 0)
            {
                return vertices;
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                if (vertex.UseRigidBoneOrigin == false ||
                    vertex.Bone0 < 0 ||
                    vertex.Bone0 >= scene.BindBones.Count ||
                    vertex.Weight0 <= 0.0001f)
                {
                    continue;
                }

                var bone = scene.BindBones[vertex.Bone0];
                vertex.Position = TransformSkinPoint(vertex.Position, bone);
                vertex.Normal = Normalize(TransformSkinDirection(vertex.Normal, bone));
                vertices[i] = vertex;
            }

            return vertices;
        }

        public static void ExportAnimationGlb(Stream output, ModelPreviewScene scene, Animation animation)
        {
            ExportAnimationGlb(output, scene, animation, true);
        }

        public static void ExportAnimationGlb(Stream output, ModelPreviewScene scene, Animation animation, bool rawRootTranslation)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            if (scene == null || scene.BindBones == null || scene.BindBones.Count == 0)
            {
                throw new InvalidOperationException("No preview skeleton is loaded. Select a PolySkin before exporting an animation.");
            }

            if (scene.BindVertices == null || scene.BindVertices.Count == 0 || scene.Indices == null || scene.Indices.Count == 0)
            {
                throw new InvalidOperationException("No preview mesh is loaded. Select a PolySkin before exporting an animation.");
            }

            var previewAnimation = CreateAnimation(scene, animation);
            if (previewAnimation == null)
            {
                throw new InvalidOperationException("The selected animation does not contain channels for the current preview skeleton.");
            }

            var binary = new MemoryStream();
            var bufferViews = new List<string>();
            var accessors = new List<string>();

            var exportVertices = BuildExportVertices(scene);
            int positionAccessor = AddVec3Accessor(binary, bufferViews, accessors, exportVertices.Select(v => v.Position).ToArray(), true);
            int normalAccessor = AddVec3Accessor(binary, bufferViews, accessors, exportVertices.Select(v => Normalize(v.Normal)).ToArray(), false);
            int uvAccessor = AddVec2Accessor(binary, bufferViews, accessors, exportVertices.Select(v => new[] { v.U, v.V }).ToArray());
            int jointsAccessor = AddJointsAccessor(binary, bufferViews, accessors, exportVertices, scene.BindBones.Count);
            int weightsAccessor = AddWeightsAccessor(binary, bufferViews, accessors, exportVertices);
            int indicesAccessor = AddIndexAccessor(binary, bufferViews, accessors, scene.Indices.ToArray());
            int inverseBindAccessor = AddMat4Accessor(binary, bufferViews, accessors, scene.BindBones.Select(GetInverseBindMatrix).ToArray());

            var samplers = new List<string>();
            var channels = new List<string>();
            int frameCount = Math.Max(1, (int)Math.Ceiling(previewAnimation.NumFrames));
            int timeAccessor = AddAnimationTimeAccessor(binary, bufferViews, accessors, frameCount, previewAnimation.FrameRate);

            for (int boneIndex = 0; boneIndex < scene.BindBones.Count; boneIndex++)
            {
                var translations = new Vec3[frameCount];
                var rotations = new PreviewQuat[frameCount];
                for (int frame = 0; frame < frameCount; frame++)
                {
                    var pose = BuildAnimationPose(
                        scene,
                        previewAnimation,
                        frame + previewAnimation.StartFrame,
                        true,
                        new Vec3(),
                        rawRootTranslation == false);
                    translations[frame] = pose[boneIndex].LocalPosition;
                    rotations[frame] = ToQuaternion(pose[boneIndex].LocalAxisX, pose[boneIndex].LocalAxisY, pose[boneIndex].LocalAxisZ);
                }

                int translationOutput = AddVec3Accessor(binary, bufferViews, accessors, translations, false);
                int translationSampler = samplers.Count;
                samplers.Add(string.Format(CultureInfo.InvariantCulture, "{{\"input\":{0},\"output\":{1},\"interpolation\":\"LINEAR\"}}", timeAccessor, translationOutput));
                channels.Add(string.Format(CultureInfo.InvariantCulture, "{{\"sampler\":{0},\"target\":{{\"node\":{1},\"path\":\"translation\"}}}}", translationSampler, boneIndex));

                int rotationOutput = AddQuatAccessor(binary, bufferViews, accessors, rotations);
                int rotationSampler = samplers.Count;
                samplers.Add(string.Format(CultureInfo.InvariantCulture, "{{\"input\":{0},\"output\":{1},\"interpolation\":\"LINEAR\"}}", timeAccessor, rotationOutput));
                channels.Add(string.Format(CultureInfo.InvariantCulture, "{{\"sampler\":{0},\"target\":{{\"node\":{1},\"path\":\"rotation\"}}}}", rotationSampler, boneIndex));
            }

            var json = BuildGlbJson(
                scene,
                animation,
                binary.Length,
                bufferViews,
                accessors,
                positionAccessor,
                normalAccessor,
                uvAccessor,
                jointsAccessor,
                weightsAccessor,
                indicesAccessor,
                inverseBindAccessor,
                samplers,
                channels);
            WriteGlb(output, json, binary.ToArray());
        }

        public static void ExportModelGlb(Stream output, ModelPreviewScene scene)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            if (scene == null)
            {
                throw new InvalidOperationException("No preview model is loaded.");
            }

            var vertices = BuildExportVertices(scene);
            if (vertices == null || vertices.Count == 0 || scene.Indices == null || scene.Indices.Count == 0)
            {
                throw new InvalidOperationException("The selected node does not contain previewable mesh data.");
            }

            var binary = new MemoryStream();
            var bufferViews = new List<string>();
            var accessors = new List<string>();
            var exportParts = new List<GlbExportPart>();
            var parts = scene.Parts != null && scene.Parts.Count > 0
                            ? scene.Parts.Where(p => p.VertexCount > 0 && p.IndexCount > 0).ToList()
                            : new List<ModelPreviewPart>
                            {
                                new ModelPreviewPart
                                {
                                    Name = string.IsNullOrEmpty(scene.Name) ? "mesh" : scene.Name,
                                    VertexStart = 0,
                                    VertexCount = vertices.Count,
                                    IndexStart = 0,
                                    IndexCount = scene.Indices.Count,
                                },
                            };

            bool skinned = scene.BindBones != null && scene.BindBones.Count > 0;
            var materialLookup = BuildMaterialLookup(parts);
            int inverseBindAccessor = -1;
            var materialTextureImageBufferViews = AddMaterialTextureImageBufferViews(binary, bufferViews, scene.Materials, materialLookup);
            foreach (var part in parts)
            {
                var partVertices = vertices.Skip(part.VertexStart).Take(part.VertexCount).ToList();
                var partIndices = scene.Indices.Skip(part.IndexStart)
                                               .Take(part.IndexCount)
                                               .Select(i => i - part.VertexStart)
                                               .ToArray();

                var exportPart = new GlbExportPart
                {
                    Name = part.Name,
                    Material = GetMaterialIndex(materialLookup, part.ShaderName),
                    PositionAccessor = AddVec3Accessor(binary, bufferViews, accessors, partVertices.Select(v => v.Position).ToArray(), true),
                    NormalAccessor = AddVec3Accessor(binary, bufferViews, accessors, partVertices.Select(v => Normalize(v.Normal)).ToArray(), false),
                    UvAccessor = AddVec2Accessor(binary, bufferViews, accessors, partVertices.Select(v => new[] { v.U, v.V }).ToArray()),
                    IndicesAccessor = AddIndexAccessor(binary, bufferViews, accessors, partIndices),
                    JointsAccessor = -1,
                    WeightsAccessor = -1,
                };

                if (skinned == true)
                {
                    exportPart.JointsAccessor = AddJointsAccessor(binary, bufferViews, accessors, partVertices, scene.BindBones.Count);
                    exportPart.WeightsAccessor = AddWeightsAccessor(binary, bufferViews, accessors, partVertices);
                }

                exportParts.Add(exportPart);
            }

            if (skinned == true)
            {
                inverseBindAccessor = AddMat4Accessor(binary, bufferViews, accessors, scene.BindBones.Select(GetInverseBindMatrix).ToArray());
            }

            var json = BuildModelGlbJson(
                scene,
                binary.Length,
                bufferViews,
                accessors,
                exportParts,
                materialLookup,
                inverseBindAccessor,
                materialTextureImageBufferViews);
            WriteGlb(output, json, binary.ToArray());
        }

        private struct GlbExportPart
        {
            public string Name;
            public int Material;
            public int PositionAccessor;
            public int NormalAccessor;
            public int UvAccessor;
            public int JointsAccessor;
            public int WeightsAccessor;
            public int IndicesAccessor;
        }

        private static List<ModelPreviewBone> BuildAnimationPose(ModelPreviewScene scene, ModelPreviewAnimation animation, float frame)
        {
            return BuildAnimationPose(scene, animation, frame, true, new Vec3(), false);
        }

        private static List<ModelPreviewBone> BuildAnimationPose(ModelPreviewScene scene, ModelPreviewAnimation animation, float frame, bool rootMotion, Vec3 rootOffset, bool normalizeRootMotion)
        {
            var pose = CloneBones(scene.BindBones);
            ApplyAnimationChannelsToPose(scene, pose, animation, frame, rootMotion, rootOffset, normalizeRootMotion);
            ComposeBones(pose);
            return pose;
        }

        private static void ApplyAnimationChannelsToPose(ModelPreviewScene scene, List<ModelPreviewBone> pose, ModelPreviewAnimation animation, float frame, bool rootMotion, Vec3 rootOffset, bool normalizeRootMotion)
        {
            if (scene == null || pose == null || animation == null || animation.Channels == null)
            {
                return;
            }

            foreach (var channel in animation.Channels)
            {
                if (channel.BoneIndex < 0 || channel.BoneIndex >= pose.Count)
                {
                    continue;
                }

                var bone = pose[channel.BoneIndex];
                if (channel.Rotations != null && channel.RotationFrames != null && channel.Rotations.Length > 0 && channel.RotationFrames.Length > 0)
                {
                    var rotation = SampleRotation(channel, frame);
                    bone.LocalAxisX = RotateLocalBasis(rotation, new Vec3(1, 0, 0), bone);
                    bone.LocalAxisY = RotateLocalBasis(rotation, new Vec3(0, 1, 0), bone);
                    bone.LocalAxisZ = RotateLocalBasis(rotation, new Vec3(0, 0, 1), bone);
                }

                if (channel.Translations != null && channel.TranslationFrames != null && channel.Translations.Length > 0 && channel.TranslationFrames.Length > 0)
                {
                    if (rootMotion == true || bone.Parent >= 0)
                    {
                        var translation = SampleTranslation(channel, frame);
                        if (normalizeRootMotion == true && bone.Parent < 0)
                        {
                            translation = scene.BindBones[channel.BoneIndex].LocalPosition + translation - channel.Translations[0];
                        }

                        bone.LocalPosition = translation;
                    }
                }

                pose[channel.BoneIndex] = bone;
            }

            if (rootMotion == true && LengthSquared(rootOffset) > 0.000001f)
            {
                int start = Math.Max(0, animation.BoneStart);
                int end = animation.BoneCount <= 0 ? pose.Count : Math.Min(pose.Count, start + animation.BoneCount);
                for (int i = start; i < end; i++)
                {
                    if (pose[i].Parent >= 0)
                    {
                        continue;
                    }

                    var bone = pose[i];
                    bone.LocalPosition += rootOffset;
                    pose[i] = bone;
                    break;
                }
            }

        }

        public static void ResetAnimation(ModelPreviewScene scene)
        {
            if (scene == null)
            {
                return;
            }

            SetAllPartsHidden(scene, false);
            ResetVertexAnimationBindVertices(scene, true);
            if (scene.BindBones == null)
            {
                return;
            }

            scene.Bones = CloneBones(scene.BindBones);
            scene.Vertices = scene.BindVertices.ToList();
            if (scene.UseGpuSkinning == false)
            {
                ApplyRigidGeometryBindPose(scene);
            }
        }

        private static void ResetVertexAnimationBindVertices(ModelPreviewScene scene, bool updateVertices)
        {
            if (scene == null ||
                scene.VertexAnimationBindVertices == null ||
                scene.VertexAnimationBindVertices.Count == 0)
            {
                return;
            }

            scene.BindVertices = scene.VertexAnimationBindVertices.ToList();
            if (updateVertices == true)
            {
                scene.Vertices = scene.BindVertices.ToList();
            }
        }

        private static void AppendP2PolySkin(
            P2PolySkin p2PolySkin,
            IEnumerable<BaseNode> flattened,
            ModelPreviewScene scene,
            List<string> shaderNames,
            Dictionary<int, int> p2BoneRemap)
        {
            var metadata = p2PolySkin == null ? null : p2PolySkin.Children.OfType<P2PolySkinMetadata>().FirstOrDefault();
            if (metadata == null)
            {
                return;
            }

            var nodes = flattened == null ? new List<BaseNode>() : flattened.ToList();
            var indexPrimitive = nodes.OfType<P2Primitive>()
                                      .FirstOrDefault(p => string.Equals(p.Name, metadata.IndicesName, StringComparison.OrdinalIgnoreCase));
            var vertexPrimitive = nodes.OfType<P2Primitive>()
                                       .FirstOrDefault(p => string.Equals(p.Name, metadata.VerticesName, StringComparison.OrdinalIgnoreCase));
            var skinPrimitive = nodes.OfType<P2Primitive>()
                                     .FirstOrDefault(p => string.Equals(p.Name, metadata.SkinName, StringComparison.OrdinalIgnoreCase)) ??
                                vertexPrimitive;

            if (shaderNames != null)
            {
                shaderNames.Add(metadata.ShaderName);
            }

            AppendP2Primitive(skinPrimitive, vertexPrimitive, indexPrimitive, scene, p2PolySkin.Name, metadata.ShaderName, p2BoneRemap);
        }

        private static void AppendP2Primitive(
            P2Primitive skinPrimitive,
            P2Primitive vertexPrimitive,
            P2Primitive indexPrimitive,
            ModelPreviewScene scene,
            string ownerName,
            string shaderName)
        {
            AppendP2Primitive(skinPrimitive, vertexPrimitive, indexPrimitive, scene, ownerName, shaderName, null);
        }

        private static void AppendP2Primitive(
            P2Primitive skinPrimitive,
            P2Primitive vertexPrimitive,
            P2Primitive indexPrimitive,
            ModelPreviewScene scene,
            string ownerName,
            string shaderName,
            Dictionary<int, int> p2BoneRemap)
        {
            if (scene == null)
            {
                return;
            }

            var positionItems = GetP2Items(skinPrimitive, VertexDescriptionType.Position);
            if (positionItems.Length == 0 && !ReferenceEquals(skinPrimitive, vertexPrimitive))
            {
                positionItems = GetP2Items(vertexPrimitive, VertexDescriptionType.Position);
            }

            if (positionItems.Length == 0)
            {
                return;
            }

            var normalItems = GetP2Items(skinPrimitive, VertexDescriptionType.Normal);
            if (normalItems.Length == 0 && !ReferenceEquals(skinPrimitive, vertexPrimitive))
            {
                normalItems = GetP2Items(vertexPrimitive, VertexDescriptionType.Normal);
            }

            var uvItems = GetP2Items(vertexPrimitive, VertexDescriptionType.Uv0);
            if (uvItems.Length == 0)
            {
                uvItems = GetP2Items(skinPrimitive, VertexDescriptionType.Uv0);
            }

            var weightItems = GetP2Items(skinPrimitive, VertexDescriptionType.Weight);
            var groupItems = GetP2Items(skinPrimitive, VertexDescriptionType.Group);
            var vertices = scene.Vertices;
            var indices = scene.Indices;
            int baseVertex = vertices.Count;
            int baseIndex = indices.Count;
            int vertexCount = positionItems.Length;

            for (int i = 0; i < vertexCount; i++)
            {
                var position = ReadP2Vector3(positionItems, i);
                var normal = ReadP2Vector3(normalItems, i);
                var vertex = new ModelPreviewVertex
                {
                    Bone0 = -1,
                    Bone1 = -1,
                    Bone2 = -1,
                    Bone3 = -1,
                    Position = ToPreviewPosition(position),
                    Normal = normal != null
                                 ? ToPreviewDirection(new Vec3(-normal.X, -normal.Y, -normal.Z))
                                 : new Vec3(0, 0, 1),
                };

                var uv = ReadP2Vector2(uvItems, i);
                if (uv != null)
                {
                    vertex.U = uv.X;
                    vertex.V = uv.Y;
                }

                var weight = ReadP2Weights(weightItems, i);
                var group = ReadP2BoneIndices(groupItems, i);
                if (weight != null && group != null && group.Length >= 4)
                {
                    vertex.Bone0 = ResolveP2BoneIndex(group[0], p2BoneRemap);
                    vertex.Bone1 = ResolveP2BoneIndex(group[1], p2BoneRemap);
                    vertex.Bone2 = ResolveP2BoneIndex(group[2], p2BoneRemap);
                    vertex.Bone3 = ResolveP2BoneIndex(group[3], p2BoneRemap);
                    vertex.Weight0 = vertex.Bone0 >= 0 ? weight.X : 0.0f;
                    vertex.Weight1 = vertex.Bone1 >= 0 ? weight.Y : 0.0f;
                    vertex.Weight2 = vertex.Bone2 >= 0 ? weight.Z : 0.0f;
                    vertex.Weight3 = vertex.Bone3 >= 0 ? weight.W : 0.0f;
                    NormalizeP2Weights(ref vertex);
                }

                vertices.Add(vertex);
            }

            var indexItems = GetP2Items(indexPrimitive, VertexDescriptionType.Group);
            if (indexItems.Length == 0)
            {
                indexItems = GetP2Items(indexPrimitive, VertexDescriptionType.Unknown);
            }

            if (indexItems.Length >= 3)
            {
                for (int i = 0; i + 2 < indexItems.Length; i += 3)
                {
                    var a = ReadP2Index(indexItems[i]);
                    var b = ReadP2Index(indexItems[i + 1]);
                    var c = ReadP2Index(indexItems[i + 2]);
                    if (a >= 0 && b >= 0 && c >= 0 && a < vertexCount && b < vertexCount && c < vertexCount)
                    {
                        indices.Add(baseVertex + a);
                        indices.Add(baseVertex + b);
                        indices.Add(baseVertex + c);
                    }
                }
            }
            else if (ReferenceEquals(indexPrimitive, skinPrimitive) == true || indexPrimitive == null)
            {
                for (int i = 0; i + 2 < vertexCount; i += 3)
                {
                    indices.Add(baseVertex + i);
                    indices.Add(baseVertex + i + 1);
                    indices.Add(baseVertex + i + 2);
                }
            }

            scene.Parts.Add(new ModelPreviewPart
            {
                Name = string.IsNullOrEmpty(ownerName) ? "p2_primitive" : ownerName,
                ObjectName = ownerName,
                ShaderName = shaderName,
                MaterialIndex = -1,
                VertexStart = baseVertex,
                VertexCount = vertices.Count - baseVertex,
                IndexStart = baseIndex,
                IndexCount = indices.Count - baseIndex,
            });
        }

        private static P2BufferItem[] GetP2Items(P2Primitive primitive, VertexDescriptionType type)
        {
            if (primitive == null)
            {
                return new P2BufferItem[0];
            }

            var buffer = primitive.Children.OfType<P2Buffer>().FirstOrDefault();
            if (buffer == null || buffer.Items == null)
            {
                return new P2BufferItem[0];
            }

            if (type == VertexDescriptionType.Unknown)
            {
                return buffer.Items.Where(i => i != null).ToArray();
            }

            return buffer.Items.Where(i => i != null && i.BufferType == type).ToArray();
        }

        private static Vector2 ReadP2Vector2(P2BufferItem[] items, int index)
        {
            if (items == null || index < 0 || index >= items.Length ||
                items[index] == null || items[index].Data == null || items[index].Data.Length < 8)
            {
                return null;
            }

            return items[index].GetVector2();
        }

        private static Vector3 ReadP2Vector3(P2BufferItem[] items, int index)
        {
            if (items == null || index < 0 || index >= items.Length ||
                items[index] == null || items[index].Data == null || items[index].Data.Length < 12)
            {
                return null;
            }

            return items[index].GetVector3();
        }

        private static Vector4 ReadP2Weights(P2BufferItem[] items, int index)
        {
            if (items == null || index < 0 || index >= items.Length ||
                items[index] == null)
            {
                return null;
            }

            return items[index].GetWeights();
        }

        private static int[] ReadP2BoneIndices(P2BufferItem[] items, int index)
        {
            if (items == null || index < 0 || index >= items.Length || items[index] == null)
            {
                return null;
            }

            var indices = items[index].GetBoneIndices();
            return indices.Length > 0 ? indices : null;
        }

        private static int ResolveP2BoneIndex(int boneIndex, Dictionary<int, int> remap)
        {
            if (remap == null)
            {
                return boneIndex;
            }

            int mapped;
            return remap.TryGetValue(boneIndex, out mapped) == true ? mapped : -1;
        }

        private static void NormalizeP2Weights(ref ModelPreviewVertex vertex)
        {
            vertex.Weight0 = Math.Max(0.0f, vertex.Weight0);
            vertex.Weight1 = Math.Max(0.0f, vertex.Weight1);
            vertex.Weight2 = Math.Max(0.0f, vertex.Weight2);
            vertex.Weight3 = Math.Max(0.0f, vertex.Weight3);

            var total = vertex.Weight0 + vertex.Weight1 + vertex.Weight2 + vertex.Weight3;
            if (total <= 0.000001f)
            {
                return;
            }

            var scale = 1.0f / total;
            vertex.Weight0 *= scale;
            vertex.Weight1 *= scale;
            vertex.Weight2 *= scale;
            vertex.Weight3 *= scale;
        }

        private static int ReadP2Index(P2BufferItem item)
        {
            if (item == null || item.Data == null)
            {
                return -1;
            }

            if (item.Data.Length >= 2)
            {
                return BitConverter.ToUInt16(item.Data, 0);
            }

            return item.Data.Length == 1 ? item.Data[0] : -1;
        }

        private static void AppendPrimitiveGroup(U00010020_PrimitiveGroup primitiveGroup, ModelPreviewScene scene, string ownerName)
        {
            AppendPrimitiveGroup(primitiveGroup, scene, ownerName, -1);
        }

        private static void AppendPrimitiveGroup(U00010020_PrimitiveGroup primitiveGroup, ModelPreviewScene scene, string ownerName, int rigidBoneIndex)
        {
            var vertexBuffers = primitiveGroup.Children.OfType<VertexBuffer>().ToList();
            foreach (var vertexBuffer in vertexBuffers)
            {
                vertexBuffer.ResolveDescription();
            }

            var positionBuffer = FindVertexBuffer(vertexBuffers, VertexDescriptionType.Position, 2);
            var normalBuffer = FindVertexBuffer(vertexBuffers, VertexDescriptionType.Normal, 2);
            var weightBuffer = FindVertexBuffer(vertexBuffers, VertexDescriptionType.Weight, 2);
            var groupBuffer = FindVertexBuffer(vertexBuffers, VertexDescriptionType.Group, 2);
            var uvBuffer = FindVertexBuffer(vertexBuffers, VertexDescriptionType.Uv0, 1);
            var indexBuffer = primitiveGroup.Children.OfType<IndexBuffer>().FirstOrDefault();
            var matrixPalette = ReadMatrixPalette(primitiveGroup.Children.OfType<PrimitiveMatrix>().FirstOrDefault());

            if (positionBuffer == null || indexBuffer == null || primitiveGroup.NumVertices == 0)
            {
                AppendLegacyPrimitiveGroup(primitiveGroup, scene, ownerName, rigidBoneIndex, matrixPalette);
                return;
            }

            var vertices = scene.Vertices;
            var indices = scene.Indices;
            var baseVertex = vertices.Count;
            var baseIndex = indices.Count;

            for (int i = 0; i < primitiveGroup.NumVertices; i++)
            {
                var position = positionBuffer.GetVector3(i, VertexDescriptionType.Position);
                var normal = normalBuffer == null ? null : normalBuffer.GetVector3(i, VertexDescriptionType.Normal);
                var uv = uvBuffer == null ? null : uvBuffer.GetUv(i, VertexDescriptionType.Uv0);
                var weight = weightBuffer == null ? null : weightBuffer.GetVector3(i, VertexDescriptionType.Weight);
                var group = groupBuffer == null ? null : groupBuffer.GetBytes(i, VertexDescriptionType.Group);

                var vertex = new ModelPreviewVertex
                {
                    Bone0 = -1,
                    Bone1 = -1,
                    Bone2 = -1,
                    Bone3 = -1,
                    Position = ToPreviewPosition(position),
                    Normal = normal != null
                                 ? ToPreviewDirection(new Vec3(-normal.X, -normal.Y, -normal.Z))
                                 : new Vec3(0, 0, 1),
                };

                if (uv != null)
                {
                    vertex.U = uv.U;
                    vertex.V = uv.V;
                }

                if (weight != null && group != null && group.Length >= 3)
                {
                    AssignSkinWeights(
                        ref vertex,
                        matrixPalette,
                        weight.X,
                        weight.Y,
                        weight.Z,
                        group[0],
                        group[1],
                        group[2],
                        group.Length >= 4 ? group[3] : (byte)0,
                        group.Length >= 4);
                }
                else if (rigidBoneIndex >= 0)
                {
                    vertex.Weight0 = 1.0f;
                    vertex.Weight1 = 0.0f;
                    vertex.Weight2 = 0.0f;
                    vertex.Weight3 = 0.0f;
                    vertex.Bone0 = rigidBoneIndex;
                    vertex.Bone1 = -1;
                    vertex.Bone2 = -1;
                    vertex.Bone3 = -1;
                    vertex.UseRigidBoneOrigin = true;
                }

                vertices.Add(vertex);
            }

            foreach (var face in indexBuffer.Faces)
            {
                indices.Add(baseVertex + face.Point1);
                indices.Add(baseVertex + face.Point2);
                indices.Add(baseVertex + face.Point3);
            }

            scene.Parts.Add(new ModelPreviewPart
            {
                Name = MakePartName(ownerName, primitiveGroup, scene.Parts.Count),
                ObjectName = ownerName,
                ShaderName = primitiveGroup.ShaderName,
                MaterialIndex = -1,
                VertexStart = baseVertex,
                VertexCount = vertices.Count - baseVertex,
                IndexStart = baseIndex,
                IndexCount = indices.Count - baseIndex,
            });
        }

        private static void AppendLegacyPrimitiveGroup(
            U00010020_PrimitiveGroup primitiveGroup,
            ModelPreviewScene scene,
            string ownerName,
            int rigidBoneIndex,
            List<int> matrixPalette)
        {
            var positions = primitiveGroup.Children.OfType<LegacyPositionStream>().Select(n => n.Values).FirstOrDefault() ?? new Vector3[0];
            var normals = primitiveGroup.Children.OfType<LegacyNormalStream>().Select(n => n.Values).FirstOrDefault() ?? new Vector3[0];
            var uvs = primitiveGroup.Children.OfType<LegacyUvStream>().Select(n => n.Values).FirstOrDefault() ?? new Vector2[0];
            var groups = primitiveGroup.Children.OfType<LegacyGroupStream>().Select(n => n.Groups).FirstOrDefault() ?? new LegacyGroupValue[0];
            var weights = primitiveGroup.Children.OfType<LegacyWeightStream>().Select(n => n.Values).FirstOrDefault() ?? new Vector3[0];
            var indices = primitiveGroup.Children.OfType<LegacyIndexStream>().Select(n => n.Indices).FirstOrDefault() ?? new uint[0];

            if (positions.Length == 0 || indices.Length == 0)
            {
                return;
            }

            var vertices = scene.Vertices;
            var sceneIndices = scene.Indices;
            var baseVertex = vertices.Count;
            var baseIndex = sceneIndices.Count;

            for (int i = 0; i < positions.Length; i++)
            {
                var normal = i < normals.Length ? normals[i] : null;
                var vertex = new ModelPreviewVertex
                {
                    Bone0 = -1,
                    Bone1 = -1,
                    Bone2 = -1,
                    Bone3 = -1,
                    Position = ToPreviewPosition(positions[i]),
                    Normal = normal != null
                                 ? ToPreviewDirection(new Vec3(-normal.X, -normal.Y, -normal.Z))
                                 : new Vec3(0, 0, 1),
                };

                if (i < uvs.Length)
                {
                    vertex.U = uvs[i].X;
                    vertex.V = uvs[i].Y;
                }

                if (i < weights.Length && i < groups.Length)
                {
                    AssignLegacySkinWeights(
                        ref vertex,
                        matrixPalette,
                        weights[i],
                        groups[i][0],
                        groups[i][1],
                        groups[i][2],
                        groups[i][3]);
                }
                else if (rigidBoneIndex >= 0)
                {
                    vertex.Weight0 = 1.0f;
                    vertex.Bone0 = rigidBoneIndex;
                    vertex.UseRigidBoneOrigin = true;
                }

                vertices.Add(vertex);
            }

            foreach (var index in indices)
            {
                if (index < positions.Length)
                {
                    sceneIndices.Add(baseVertex + (int)index);
                }
            }

            scene.Parts.Add(new ModelPreviewPart
            {
                Name = MakePartName(ownerName, primitiveGroup, scene.Parts.Count),
                ObjectName = ownerName,
                ShaderName = primitiveGroup.ShaderName,
                MaterialIndex = -1,
                VertexStart = baseVertex,
                VertexCount = vertices.Count - baseVertex,
                IndexStart = baseIndex,
                IndexCount = sceneIndices.Count - baseIndex,
            });
        }

        private static void AppendPolySkin(PolySkin polySkin, ModelPreviewScene scene, List<string> shaderNames)
        {
            foreach (var primitiveGroup in polySkin.Children.OfType<U00010020_PrimitiveGroup>())
            {
                shaderNames.Add(primitiveGroup.ShaderName);
                AppendPrimitiveGroup(primitiveGroup, scene, polySkin.Name);
                AppendVertexAnimation(polySkin, scene, scene.Parts.Count == 0 ? 0 : scene.Parts[scene.Parts.Count - 1].VertexStart, scene.Parts.Count == 0 ? 0 : scene.Parts[scene.Parts.Count - 1].VertexCount);
            }
        }

        private static void AppendGeometry(Geometry geometry, ModelPreviewScene scene, List<string> shaderNames)
        {
            AppendGeometry(geometry, scene, shaderNames, -1);
        }

        private static void AppendGeometry(Geometry geometry, ModelPreviewScene scene, List<string> shaderNames, int rigidBoneIndex)
        {
            foreach (var primitiveGroup in geometry.Children.OfType<U00010020_PrimitiveGroup>())
            {
                shaderNames.Add(primitiveGroup.ShaderName);
                AppendPrimitiveGroup(primitiveGroup, scene, geometry.Name, rigidBoneIndex);
            }
        }

        private static void RegisterVrtxShapeMorphs(Pure3DFile file, ModelPreviewScene scene)
        {
            if (file == null || scene == null || scene.Parts == null || scene.Parts.Count == 0)
            {
                return;
            }

            var nodes = Flatten(file.Nodes).ToList();
            var geometries = nodes.OfType<Geometry>()
                                  .Where(g => string.IsNullOrEmpty(g.Name) == false)
                                  .ToLookup(g => g.Name, StringComparer.OrdinalIgnoreCase);
            if (geometries.Count == 0)
            {
                return;
            }

            var initialShapeNames = scene.Parts
                                         .Where(p => string.IsNullOrEmpty(p.ObjectName) == false)
                                         .Select(p => p.ObjectName)
                                         .Distinct(StringComparer.OrdinalIgnoreCase)
                                         .Where(n => n.EndsWith("InitialShape", StringComparison.OrdinalIgnoreCase))
                                         .ToList();

            foreach (var initialShapeName in initialShapeNames)
            {
                if (scene.VertexMorphs.Any(m => MatchesShapeName(m.InitialShapeName, initialShapeName)) == true)
                {
                    continue;
                }

                var damagedShapeName = initialShapeName.Substring(0, initialShapeName.Length - "InitialShape".Length) + "DamagedShape";
                int vertexStart;
                int vertexCount;
                if (TryGetObjectVertexSpan(scene, initialShapeName, out vertexStart, out vertexCount) == false)
                {
                    continue;
                }

                ModelPreviewScene targetScene = null;
                foreach (var damagedGeometry in geometries[damagedShapeName])
                {
                    var candidateScene = new ModelPreviewScene();
                    var shaderNames = new List<string>();
                    AppendGeometry(damagedGeometry, candidateScene, shaderNames);
                    if (candidateScene.Vertices.Count == vertexCount)
                    {
                        targetScene = candidateScene;
                        break;
                    }
                }

                if (targetScene == null)
                {
                    continue;
                }

                scene.VertexMorphs.Add(new ModelPreviewVertexMorph
                {
                    InitialShapeName = initialShapeName,
                    DamagedShapeName = damagedShapeName,
                    VertexStart = vertexStart,
                    VertexCount = vertexCount,
                    Positions = targetScene.Vertices.Select(v => v.Position).ToArray(),
                    Normals = targetScene.Vertices.Select(v => v.Normal).ToArray(),
                });
            }

            RegisterEmbeddedVrtxOffsetMorphs(geometries, scene);
        }

        private static void RegisterEmbeddedVrtxOffsetMorphs(ILookup<string, Geometry> geometries, ModelPreviewScene scene)
        {
            if (geometries == null || scene == null || scene.Parts == null)
            {
                return;
            }

            var objectNames = scene.Parts
                                   .Where(p => string.IsNullOrEmpty(p.ObjectName) == false)
                                   .Select(p => p.ObjectName)
                                   .Distinct(StringComparer.OrdinalIgnoreCase)
                                   .ToList();

            foreach (var objectName in objectNames)
            {
                if (scene.VertexMorphs.Any(m => MatchesShapeName(m.InitialShapeName, objectName)) == true)
                {
                    continue;
                }

                int vertexStart;
                int vertexCount;
                if (TryGetObjectVertexSpan(scene, objectName, out vertexStart, out vertexCount) == false)
                {
                    continue;
                }

                foreach (var geometry in geometries[objectName])
                {
                    Vec3[] positions;
                    Vec3[] normals;
                    if (TryBuildEmbeddedVrtxOffsetMorph(geometry, vertexCount, out positions, out normals) == false)
                    {
                        continue;
                    }

                    scene.VertexMorphs.Add(new ModelPreviewVertexMorph
                    {
                        InitialShapeName = objectName,
                        DamagedShapeName = objectName + "_VrtxOffset",
                        VertexStart = vertexStart,
                        VertexCount = vertexCount,
                        Positions = positions,
                        Normals = normals,
                    });
                    break;
                }
            }
        }

        private static bool TryBuildEmbeddedVrtxOffsetMorph(
            Geometry geometry,
            int expectedVertexCount,
            out Vec3[] positions,
            out Vec3[] normals)
        {
            positions = null;
            normals = null;
            if (geometry == null || expectedVertexCount <= 0)
            {
                return false;
            }

            var targetPositions = new List<Vec3>(expectedVertexCount);
            var targetNormals = new List<Vec3>(expectedVertexCount);
            bool hasOffsets = false;

            foreach (var primitiveGroup in geometry.Children.OfType<U00010020_PrimitiveGroup>())
            {
                var vertexBuffers = primitiveGroup.Children.OfType<VertexBuffer>().ToList();
                foreach (var vertexBuffer in vertexBuffers)
                {
                    vertexBuffer.ResolveDescription();
                }

                var positionBuffer = FindVertexBuffer(vertexBuffers, VertexDescriptionType.Position, 2);
                if (positionBuffer == null || primitiveGroup.NumVertices == 0)
                {
                    return false;
                }

                var normalBuffer = FindVertexBuffer(vertexBuffers, VertexDescriptionType.Normal, 2);
                var positionOffsetBuffer = FindVertexBuffer(vertexBuffers, VertexDescriptionType.VertexMorphPositionOffset, 2);
                var normalOffsetBuffer = FindVertexBuffer(vertexBuffers, VertexDescriptionType.VertexMorphNormalOffset, 2);

                for (int i = 0; i < primitiveGroup.NumVertices; i++)
                {
                    var position = positionBuffer.GetVector3(i, VertexDescriptionType.Position);
                    if (position == null)
                    {
                        return false;
                    }

                    var positionOffset = positionOffsetBuffer == null
                                             ? null
                                             : positionOffsetBuffer.GetVector3(i, VertexDescriptionType.VertexMorphPositionOffset);
                    if (positionOffset != null)
                    {
                        hasOffsets |= Math.Abs(positionOffset.X) > 0.000001f ||
                                      Math.Abs(positionOffset.Y) > 0.000001f ||
                                      Math.Abs(positionOffset.Z) > 0.000001f;
                    }

                    targetPositions.Add(ToPreviewPosition(new Vector3
                    {
                        X = position.X + (positionOffset == null ? 0.0f : positionOffset.X),
                        Y = position.Y + (positionOffset == null ? 0.0f : positionOffset.Y),
                        Z = position.Z + (positionOffset == null ? 0.0f : positionOffset.Z),
                    }));

                    var normal = normalBuffer == null ? null : normalBuffer.GetVector3(i, VertexDescriptionType.Normal);
                    var normalOffset = normalOffsetBuffer == null
                                           ? null
                                           : normalOffsetBuffer.GetVector3(i, VertexDescriptionType.VertexMorphNormalOffset);
                    if (normalOffset != null)
                    {
                        hasOffsets |= Math.Abs(normalOffset.X) > 0.000001f ||
                                      Math.Abs(normalOffset.Y) > 0.000001f ||
                                      Math.Abs(normalOffset.Z) > 0.000001f;
                    }

                    if (normal != null)
                    {
                        targetNormals.Add(Normalize(ToPreviewDirection(new Vec3(
                            -(normal.X + (normalOffset == null ? 0.0f : normalOffset.X)),
                            -(normal.Y + (normalOffset == null ? 0.0f : normalOffset.Y)),
                            -(normal.Z + (normalOffset == null ? 0.0f : normalOffset.Z))))));
                    }
                    else
                    {
                        targetNormals.Add(new Vec3(0, 0, 1));
                    }
                }
            }

            if (hasOffsets == false || targetPositions.Count != expectedVertexCount)
            {
                return false;
            }

            positions = targetPositions.ToArray();
            normals = targetNormals.Count == targetPositions.Count ? targetNormals.ToArray() : null;
            return true;
        }

        private static bool TryGetObjectVertexSpan(ModelPreviewScene scene, string objectName, out int vertexStart, out int vertexCount)
        {
            vertexStart = 0;
            vertexCount = 0;
            if (scene == null || scene.Parts == null || string.IsNullOrEmpty(objectName) == true)
            {
                return false;
            }

            var parts = scene.Parts.Where(p => PartMatchesObject(p, objectName))
                                   .OrderBy(p => p.VertexStart)
                                   .ToList();
            if (parts.Count == 0)
            {
                return false;
            }

            vertexStart = parts[0].VertexStart;
            int end = vertexStart;
            foreach (var part in parts)
            {
                if (part.VertexStart != end)
                {
                    return false;
                }

                end = part.VertexStart + part.VertexCount;
            }

            vertexCount = end - vertexStart;
            return vertexCount > 0;
        }

        private static void AppendVertexAnimation(PolySkin polySkin, ModelPreviewScene scene, int vertexStart, int vertexCount)
        {
            if (polySkin == null || scene == null || vertexCount <= 0)
            {
                return;
            }

            var animationNode = polySkin.Children.OfType<VertexAnimationSet>().FirstOrDefault();
            if (animationNode == null)
            {
                return;
            }

            var animation = new ModelPreviewVertexAnimation
            {
                VertexStart = vertexStart,
                VertexCount = vertexCount,
            };

            foreach (var targetNode in animationNode.Children.OfType<VertexAnimationTarget>())
            {
                var target = new ModelPreviewVertexAnimationTarget
                {
                    SourceIndex = targetNode.Index > int.MaxValue ? animation.Targets.Count : (int)targetNode.Index,
                    Positions = new Vec3?[vertexCount],
                    Normals = new Vec3?[vertexCount],
                };

                foreach (var channel in targetNode.Children.OfType<VertexAnimationChannel>())
                {
                    ReadVertexAnimationChannel(channel, target, vertexCount);
                }

                if (target.Positions.Count(v => v.HasValue) == vertexCount)
                {
                    animation.Targets.Add(target);
                }
            }

            if (animation.Targets.Count > 0)
            {
                scene.VertexAnimations.Add(animation);
            }
        }

        private static void ReadVertexAnimationChannel(VertexAnimationChannel channel, ModelPreviewVertexAnimationTarget target, int vertexCount)
        {
            if (channel == null || channel.Values == null || target == null)
            {
                return;
            }

            var type = (channel.ChannelType ?? string.Empty).TrimEnd('\0', ' ');
            bool positions = string.Equals(type, "POS", StringComparison.OrdinalIgnoreCase);
            bool normals = string.Equals(type, "NRML", StringComparison.OrdinalIgnoreCase);
            if (positions == false && normals == false || channel.Values.Length != vertexCount)
            {
                return;
            }

            foreach (var channelValue in channel.Values)
            {
                if (channelValue.VertexIndex >= vertexCount || channelValue.Value == null)
                {
                    continue;
                }

                if (positions == true)
                {
                    target.Positions[channelValue.VertexIndex] = ToPreviewPosition(channelValue.Value);
                }
                else
                {
                    target.Normals[channelValue.VertexIndex] = ToPreviewDirection(new Vec3(-channelValue.Value.X, -channelValue.Value.Y, -channelValue.Value.Z));
                }
            }
        }

        private static int GetCompositeReferenceBoneIndex(CompositeDrawablePolySkinReference reference)
        {
            if (reference == null || reference.Unknown5 > int.MaxValue)
            {
                return -1;
            }

            return (int)reference.Unknown5;
        }

        private static string MakePartName(string ownerName, U00010020_PrimitiveGroup primitiveGroup, int index)
        {
            var owner = string.IsNullOrEmpty(ownerName) ? "mesh" : ownerName;
            var shader = primitiveGroup == null || string.IsNullOrEmpty(primitiveGroup.ShaderName) ? null : primitiveGroup.ShaderName;
            return shader == null
                       ? string.Format(CultureInfo.InvariantCulture, "{0}_{1}", owner, index)
                       : string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", owner, index, shader);
        }

        private static VertexBuffer FindVertexBuffer(IEnumerable<VertexBuffer> buffers, VertexDescriptionType type, uint preferredParam)
        {
            return buffers.FirstOrDefault(v => v.Param == preferredParam && v.HasElement(type)) ??
                   buffers.FirstOrDefault(v => v.HasElement(type));
        }

        private static List<int> ReadMatrixPalette(PrimitiveMatrix node)
        {
            var palette = new List<int>();
            if (node == null || node.Indices == null)
            {
                return palette;
            }

            for (int i = 0; i < node.Indices.Length; i++)
            {
                palette.Add((int)node.Indices[i]);
            }
            return palette;
        }

        private static int ResolvePaletteBone(List<int> palette, byte index)
        {
            return index < palette.Count ? palette[index] : index;
        }

        private static void AssignSkinWeights(
            ref ModelPreviewVertex vertex,
            List<int> matrixPalette,
            float weight0,
            float weight1,
            float weight2,
            byte group0,
            byte group1,
            byte group2,
            byte group3,
            bool hasFourthGroup)
        {
            vertex.Weight0 = weight0;
            vertex.Weight1 = weight1;
            vertex.Weight2 = weight2;
            vertex.Weight3 = hasFourthGroup == true
                                 ? Math.Max(0.0f, 1.0f - (weight0 + weight1 + weight2))
                                 : 0.0f;
            vertex.Bone0 = ResolvePaletteBone(matrixPalette, group0);
            vertex.Bone1 = ResolvePaletteBone(matrixPalette, group1);
            vertex.Bone2 = ResolvePaletteBone(matrixPalette, group2);
            vertex.Bone3 = hasFourthGroup == true && vertex.Weight3 > 0.0001f
                               ? ResolvePaletteBone(matrixPalette, group3)
                               : -1;
        }

        private static void AssignLegacySkinWeights(
            ref ModelPreviewVertex vertex,
            List<int> matrixPalette,
            Vector3 weights,
            byte group0,
            byte group1,
            byte group2,
            byte group3)
        {
            vertex.Weight0 = Math.Max(0.0f, weights.X);
            vertex.Weight1 = Math.Max(0.0f, weights.Y);
            vertex.Weight2 = Math.Max(0.0f, weights.Z);
            vertex.Weight3 = Math.Max(0.0f, 1.0f - (weights.X + weights.Y + weights.Z));
            vertex.Bone0 = vertex.Weight0 > 0.0001f ? ResolvePaletteBone(matrixPalette, group3) : -1;
            vertex.Bone1 = vertex.Weight1 > 0.0001f ? ResolvePaletteBone(matrixPalette, group2) : -1;
            vertex.Bone2 = vertex.Weight2 > 0.0001f ? ResolvePaletteBone(matrixPalette, group1) : -1;
            vertex.Bone3 = vertex.Weight3 > 0.0001f ? ResolvePaletteBone(matrixPalette, group0) : -1;
        }

        private static void FitScene(ModelPreviewScene scene)
        {
            var points = scene.Vertices.Select(v => v.Position).ToList();
            if (scene.Bones != null && scene.Bones.Count > 0)
            {
                points.AddRange(scene.Bones.Select(b => b.Position));
            }

            if (points.Count == 0)
            {
                return;
            }

            float minX = points.Min(v => v.X), maxX = points.Max(v => v.X);
            float minY = points.Min(v => v.Y), maxY = points.Max(v => v.Y);
            float minZ = points.Min(v => v.Z), maxZ = points.Max(v => v.Z);
            scene.Center = new Vec3((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, (minZ + maxZ) * 0.5f);

            var average = new Vec3();
            foreach (var point in points)
            {
                average += point;
            }
            scene.Average = average * (1.0f / points.Count);

            float radius = 0.001f;
            foreach (var point in points)
            {
                var d = point - scene.Center;
                radius = Math.Max(radius, (float)Math.Sqrt(d.X * d.X + d.Y * d.Y + d.Z * d.Z));
            }
            scene.Radius = radius;
        }

        private static List<ModelPreviewBone> BuildBones(IEnumerable<Pure3DFile> files, PolySkin polySkin)
        {
            return BuildBones(files, polySkin.SkeletonName);
        }

        private static List<ModelPreviewBone> BuildBones(IEnumerable<Pure3DFile> files, string skeletonName)
        {
            return BuildBones(files, skeletonName, true);
        }

        private static List<ModelPreviewBone> BuildBones(IEnumerable<Pure3DFile> files, string skeletonName, bool allowP2Fallback)
        {
            var skeleton = FindSkeleton(files, skeletonName);
            if (skeleton != null)
            {
                return BuildBonesFromSkeleton(skeleton);
            }

            var p2Skeleton = FindP2Skeleton(files, skeletonName, allowP2Fallback);
            if (p2Skeleton != null)
            {
                return BuildBonesFromP2Skeleton(p2Skeleton);
            }

            return new List<ModelPreviewBone>();
        }

        private static IEnumerable<Pure3DFile> FindSkeletonFiles(Pure3DFile activeFile, string sourceFileName)
        {
            yield return activeFile;

            var startup = LoadStartupFile(sourceFileName);
            if (startup != null && ReferenceEquals(startup, activeFile) == false)
            {
                yield return startup;
            }

            var shared = LoadSharedFile(sourceFileName);
            if (shared != null && ReferenceEquals(shared, activeFile) == false && ReferenceEquals(shared, startup) == false)
            {
                yield return shared;
            }
        }

        private static Pure3DFile LoadStartupFile(string sourceFileName)
        {
            return LoadExternalFile(sourceFileName, "startup.p3d");
        }

        private static Pure3DFile LoadSharedFile(string sourceFileName)
        {
            return LoadExternalFile(sourceFileName, "Shared.p3d");
        }

        private static Pure3DFile LoadExternalFile(string sourceFileName, string fileName)
        {
            if (string.IsNullOrEmpty(sourceFileName) == true)
            {
                return null;
            }

            var directory = Path.GetDirectoryName(Path.GetFullPath(sourceFileName));
            while (string.IsNullOrEmpty(directory) == false)
            {
                var candidate = Path.Combine(directory, fileName);
                if (File.Exists(candidate) == true)
                {
                    var fullName = Path.GetFullPath(candidate);
                    Pure3DFile cached;
                    if (ExternalFileCache.TryGetValue(fullName, out cached) == true)
                    {
                        return cached;
                    }

                    using (var input = File.OpenRead(fullName))
                    {
                        var file = new Pure3DFile();
                        file.Deserialize(input);
                        ExternalFileCache[fullName] = file;
                        return file;
                    }
                }

                directory = Directory.GetParent(directory) == null ? null : Directory.GetParent(directory).FullName;
            }

            return null;
        }

        private static Skeleton FindSkeleton(IEnumerable<Pure3DFile> files, string skeletonName)
        {
            if (string.IsNullOrEmpty(skeletonName) == true)
            {
                return null;
            }

            foreach (var file in files)
            {
                var skeleton = Flatten(file.Nodes).OfType<Skeleton>()
                                                 .FirstOrDefault(s => string.Equals(s.Name, skeletonName, StringComparison.OrdinalIgnoreCase));
                if (skeleton != null)
                {
                    return skeleton;
                }
            }

            return null;
        }

        private static P2Skeleton FindP2Skeleton(IEnumerable<Pure3DFile> files, string skeletonName)
        {
            return FindP2Skeleton(files, skeletonName, true);
        }

        private static P2Skeleton FindP2Skeleton(IEnumerable<Pure3DFile> files, string skeletonName, bool allowFallback)
        {
            P2Skeleton fallback = null;
            foreach (var file in files)
            {
                if (file == null)
                {
                    continue;
                }

                foreach (var skeleton in Flatten(file.Nodes).OfType<P2Skeleton>())
                {
                    if (fallback == null || skeleton.NumJoints > fallback.NumJoints)
                    {
                        fallback = skeleton;
                    }

                    if (string.IsNullOrEmpty(skeletonName) == false &&
                        string.Equals(skeleton.Name, skeletonName, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        return skeleton;
                    }
                }
            }

            return allowFallback == true ? fallback : null;
        }

        private static Dictionary<int, int> BuildP2BoneRemap(IEnumerable<Pure3DFile> files, string skeletonName)
        {
            var skeleton = FindP2Skeleton(files, skeletonName, false);
            if (skeleton == null)
            {
                return null;
            }

            var rigData = skeleton.Children.OfType<P2SkeletonRigData>().FirstOrDefault();
            if (rigData == null)
            {
                return null;
            }

            var skinBoneIndices = rigData.GetSkinBoneIndices();
            var count = (int)Math.Min(skeleton.NumJoints, (uint)skinBoneIndices.Length);
            var remap = new Dictionary<int, int>();
            for (int i = 0; i < count; i++)
            {
                var skinIndex = skinBoneIndices[i];
                if (skinIndex == 0xFFFF || remap.ContainsKey(skinIndex) == true)
                {
                    continue;
                }

                remap.Add(skinIndex, i);
            }

            if (skinBoneIndices.Length > count && count > 0)
            {
                var rootSkinIndex = skinBoneIndices[count];
                if (rootSkinIndex != 0xFFFF && remap.ContainsKey(rootSkinIndex) == false)
                {
                    remap.Add(rootSkinIndex, 0);
                }
            }

            return remap.Count > 0 ? remap : null;
        }

        private static List<ModelPreviewBone> BuildBonesFromSkeleton(Skeleton skeleton)
        {
            var bones = new List<ModelPreviewBone>();
            var joints = skeleton.Children.OfType<SkeletonJoint>().Take((int)skeleton.NumJoints).ToList();
            for (int i = 0; i < joints.Count; i++)
            {
                var joint = joints[i];
                var bone = new ModelPreviewBone
                {
                    Name = joint.Name,
                    GroupId = i,
                    Parent = -1,
                    LocalPosition = ToPreviewPosition(joint.Position),
                    LocalAxisX = Normalize(ToPreviewDirection(joint.AxisX)),
                    LocalAxisY = Normalize(ToPreviewDirection(joint.AxisY)),
                    LocalAxisZ = Normalize(ToPreviewDirection(joint.AxisZ)),
                };

                if (joint.ParentIndex >= 0 && joint.ParentIndex < bones.Count && joint.ParentIndex != i)
                {
                    bone.Parent = joint.ParentIndex;
                }

                bones.Add(bone);
            }

            ComposeBones(bones);
            return bones;
        }

        private static List<ModelPreviewBone> BuildBonesFromP2Skeleton(P2Skeleton skeleton)
        {
            var bones = new List<ModelPreviewBone>();
            var joints = skeleton.Children.OfType<P2SkeletonJoint>().Take((int)skeleton.NumJoints).ToList();
            for (int i = 0; i < joints.Count; i++)
            {
                var parent = FindP2ParentIndex(joints, i);
                bones.Add(new ModelPreviewBone
                {
                    Name = joints[i].Name,
                    GroupId = i,
                    Parent = parent,
                    LocalPosition = GetP2PreviewJointOffset(joints[i].Name, parent < 0),
                    LocalAxisX = new Vec3(1, 0, 0),
                    LocalAxisY = new Vec3(0, 1, 0),
                    LocalAxisZ = new Vec3(0, 0, 1),
                });
            }

            ComposeBones(bones);
            return bones;
        }

        private static int FindP2ParentIndex(IList<P2SkeletonJoint> joints, int index)
        {
            if (joints == null || index <= 0 || index >= joints.Count)
            {
                return -1;
            }

            var name = NormalizeBoneName(joints[index].Name);
            string[] candidates;
            if (name == "pelvis")
            {
                return -1;
            }
            else if (name.StartsWith("hip_"))
            {
                candidates = new[] { "pelvis" };
            }
            else if (name.StartsWith("knee_"))
            {
                candidates = new[] { ReplaceBoneBase(name, "hip") };
            }
            else if (name.StartsWith("ankle_"))
            {
                candidates = new[] { ReplaceBoneBase(name, "knee") };
            }
            else if (name.StartsWith("foot_"))
            {
                candidates = new[] { ReplaceBoneBase(name, "ankle") };
            }
            else if (name.StartsWith("toe_"))
            {
                candidates = new[] { ReplaceBoneBase(name, "foot") };
            }
            else if (name == "spine_1")
            {
                candidates = new[] { "pelvis" };
            }
            else if (name == "spine_2")
            {
                candidates = new[] { "spine_1", "pelvis" };
            }
            else if (name == "spine_3")
            {
                candidates = new[] { "spine_2", "spine_1", "pelvis" };
            }
            else if (name == "neck")
            {
                candidates = new[] { "spine_3", "spine_2", "pelvis" };
            }
            else if (name == "head")
            {
                candidates = new[] { "neck", "spine_3", "spine_2" };
            }
            else if (name.StartsWith("shoulder_"))
            {
                candidates = new[] { "spine_3", "spine_2", "neck", "pelvis" };
            }
            else if (name.StartsWith("elbow_"))
            {
                candidates = new[] { ReplaceBoneBase(name, "shoulder") };
            }
            else if (name.StartsWith("wrist_"))
            {
                candidates = new[] { ReplaceBoneBase(name, "elbow") };
            }
            else if (name.StartsWith("hand_"))
            {
                candidates = new[] { ReplaceBoneBase(name, "wrist"), ReplaceBoneBase(name, "elbow") };
            }
            else
            {
                candidates = new[] { "pelvis" };
            }

            foreach (var candidate in candidates)
            {
                var parent = FindPriorBone(joints, index, candidate);
                if (parent >= 0)
                {
                    return parent;
                }
            }

            return -1;
        }

        private static int FindPriorBone(IList<P2SkeletonJoint> joints, int index, string name)
        {
            for (int i = index - 1; i >= 0; i--)
            {
                if (NormalizeBoneName(joints[i].Name) == name)
                {
                    return i;
                }
            }

            return -1;
        }

        private static string NormalizeBoneName(string name)
        {
            return (name ?? string.Empty).TrimEnd('\0', ' ').ToLowerInvariant();
        }

        private static string ReplaceBoneBase(string name, string boneBase)
        {
            var suffix = string.Empty;
            var underscore = name == null ? -1 : name.IndexOf('_');
            if (underscore >= 0)
            {
                suffix = name.Substring(underscore);
            }

            return boneBase + suffix;
        }

        private static Vec3 GetP2PreviewJointOffset(string jointName, bool root)
        {
            if (root == true)
            {
                return new Vec3(0, 0, 0);
            }

            var name = NormalizeBoneName(jointName);
            var side = name.EndsWith("_l") ? -1.0f : name.EndsWith("_r") ? 1.0f : 0.0f;
            if (name.StartsWith("hip_")) return new Vec3(side * 0.25f, 0, -0.15f);
            if (name.StartsWith("knee_")) return new Vec3(side * 0.05f, 0, -0.65f);
            if (name.StartsWith("ankle_")) return new Vec3(0, 0, -0.55f);
            if (name.StartsWith("foot_")) return new Vec3(0, 0.18f, -0.08f);
            if (name.StartsWith("toe_")) return new Vec3(0, 0.18f, 0);
            if (name == "spine_1") return new Vec3(0, 0, 0.32f);
            if (name == "spine_2") return new Vec3(0, 0, 0.38f);
            if (name == "spine_3") return new Vec3(0, 0, 0.34f);
            if (name == "neck") return new Vec3(0, 0, 0.22f);
            if (name == "head") return new Vec3(0, 0, 0.26f);
            if (name.StartsWith("shoulder_")) return new Vec3(side * 0.32f, 0, 0.12f);
            if (name.StartsWith("elbow_")) return new Vec3(side * 0.45f, 0, -0.05f);
            if (name.StartsWith("wrist_")) return new Vec3(side * 0.38f, 0, 0);
            if (name.StartsWith("hand_")) return new Vec3(side * 0.16f, 0, 0);
            return new Vec3(0, 0, 0.2f);
        }

        private static Vec3 TransformDirection(Vec3 value, ModelPreviewBone parent)
        {
            return parent.AxisX * value.X + parent.AxisY * value.Y + parent.AxisZ * value.Z;
        }

        private static void ComposeBones(List<ModelPreviewBone> bones)
        {
            for (int i = 0; i < bones.Count; i++)
            {
                var bone = bones[i];
                if (bone.Parent >= 0 && bone.Parent < i)
                {
                    var parent = bones[bone.Parent];
                    bone.Position = parent.Position + TransformDirection(bone.LocalPosition, parent);
                    bone.AxisX = Normalize(TransformDirection(bone.LocalAxisX, parent));
                    bone.AxisY = Normalize(TransformDirection(bone.LocalAxisY, parent));
                    bone.AxisZ = Normalize(TransformDirection(bone.LocalAxisZ, parent));
                }
                else
                {
                    bone.Position = bone.LocalPosition;
                    bone.AxisX = Normalize(bone.LocalAxisX);
                    bone.AxisY = Normalize(bone.LocalAxisY);
                    bone.AxisZ = Normalize(bone.LocalAxisZ);
                }

                bones[i] = bone;
            }
        }

        private static void SkinVertices(ModelPreviewScene scene)
        {
            SkinVertices(scene, null);
        }

        public static void SkinGpuFallbackVertices(ModelPreviewScene scene)
        {
            SkinGpuFallbackVertices(scene, null);
        }

        public static void SkinGpuFallbackVertices(ModelPreviewScene scene, IEnumerable<ModelPreviewSceneInstance> skinnedInstances)
        {
            if (scene == null ||
                scene.BindVertices == null ||
                scene.BindVertices.Count == 0 ||
                scene.BindBones == null ||
                scene.Bones == null)
            {
                return;
            }

            if (scene.Vertices == null || scene.Vertices.Count != scene.BindVertices.Count)
            {
                scene.Vertices = scene.BindVertices.ToList();
            }

            var ranges = GetGpuFallbackVertexRanges(scene, skinnedInstances);
            foreach (var range in ranges)
            {
                int start = Math.Max(0, range.Item1);
                int end = Math.Min(scene.BindVertices.Count, Math.Max(start, range.Item2));
                for (int i = start; i < end; i++)
                {
                    SkinVertex(scene, i);
                }
            }
        }

        private static void SkinVertices(ModelPreviewScene scene, IEnumerable<ModelPreviewSceneInstance> skinnedInstances)
        {
            if (scene.BindVertices == null || scene.BindVertices.Count == 0 || scene.BindBones == null || scene.Bones == null)
            {
                return;
            }

            if (scene.Vertices == null || scene.Vertices.Count != scene.BindVertices.Count)
            {
                scene.Vertices = scene.BindVertices.ToList();
            }

            var ranges = GetSkinVertexRanges(scene, skinnedInstances);
            if (ranges.Count == 0)
            {
                ranges.Add(new Tuple<int, int>(0, scene.BindVertices.Count));
            }

            foreach (var range in ranges)
            {
                int start = Math.Max(0, range.Item1);
                int end = Math.Min(scene.BindVertices.Count, Math.Max(start, range.Item2));
                for (int i = start; i < end; i++)
                {
                    SkinVertex(scene, i);
                }
            }
        }

        private static List<Tuple<int, int>> GetSkinVertexRanges(ModelPreviewScene scene, IEnumerable<ModelPreviewSceneInstance> skinnedInstances)
        {
            var ranges = new List<Tuple<int, int>>();
            if (scene == null || skinnedInstances == null)
            {
                return ranges;
            }

            foreach (var instance in skinnedInstances.Distinct())
            {
                if (instance == null || instance.VertexCount <= 0)
                {
                    continue;
                }

                int start = Math.Max(0, instance.VertexStart);
                int end = Math.Min(scene.BindVertices.Count, start + instance.VertexCount);
                if (end > start)
                {
                    ranges.Add(new Tuple<int, int>(start, end));
                }
            }

            return ranges;
        }

        public static List<Tuple<int, int>> GetGpuFallbackVertexRanges(ModelPreviewScene scene, IEnumerable<ModelPreviewSceneInstance> skinnedInstances = null)
        {
            var ranges = new List<Tuple<int, int>>();
            if (scene == null || scene.BindVertices == null || scene.BindVertices.Count == 0)
            {
                return ranges;
            }

            var allowedRanges = GetSkinVertexRanges(scene, skinnedInstances);
            bool hasAllowedRanges = allowedRanges.Count > 0;
            if (scene.Parts != null && scene.Parts.Count > 0)
            {
                foreach (var part in scene.Parts)
                {
                    int start = Math.Max(0, part.VertexStart);
                    int end = Math.Min(scene.BindVertices.Count, start + Math.Max(0, part.VertexCount));
                    if (end <= start || HasRigidBoneOrigin(scene, start, end) == false)
                    {
                        continue;
                    }

                    if (hasAllowedRanges == true && RangesOverlap(start, end, allowedRanges) == false)
                    {
                        continue;
                    }

                    ranges.Add(new Tuple<int, int>(start, end));
                }
            }
            else
            {
                for (int i = 0; i < scene.BindVertices.Count; i++)
                {
                    if (scene.BindVertices[i].UseRigidBoneOrigin == true &&
                        (hasAllowedRanges == false || RangesOverlap(i, i + 1, allowedRanges) == true))
                    {
                        ranges.Add(new Tuple<int, int>(i, i + 1));
                    }
                }
            }

            return ranges;
        }

        private static bool HasRigidBoneOrigin(ModelPreviewScene scene, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                if (scene.BindVertices[i].UseRigidBoneOrigin == true)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool RangesOverlap(int start, int end, List<Tuple<int, int>> ranges)
        {
            foreach (var range in ranges)
            {
                if (start < range.Item2 && end > range.Item1)
                {
                    return true;
                }
            }

            return false;
        }

        private static void SkinVertex(ModelPreviewScene scene, int index)
        {
            if (index < 0 || index >= scene.BindVertices.Count || index >= scene.Vertices.Count)
            {
                return;
            }

            var bindVertex = scene.BindVertices[index];
            var skinnedPosition = new Vec3();
            var skinnedNormal = new Vec3();
            float totalWeight = 0.0f;

            AddSkinnedVertex(scene, bindVertex.Bone0, bindVertex.Weight0, bindVertex.Position, bindVertex.Normal, bindVertex.UseRigidBoneOrigin, ref skinnedPosition, ref skinnedNormal, ref totalWeight);
            AddSkinnedVertex(scene, bindVertex.Bone1, bindVertex.Weight1, bindVertex.Position, bindVertex.Normal, false, ref skinnedPosition, ref skinnedNormal, ref totalWeight);
            AddSkinnedVertex(scene, bindVertex.Bone2, bindVertex.Weight2, bindVertex.Position, bindVertex.Normal, false, ref skinnedPosition, ref skinnedNormal, ref totalWeight);
            AddSkinnedVertex(scene, bindVertex.Bone3, bindVertex.Weight3, bindVertex.Position, bindVertex.Normal, false, ref skinnedPosition, ref skinnedNormal, ref totalWeight);

            var vertex = bindVertex;
            if (totalWeight > 0.0001f)
            {
                vertex.Position = skinnedPosition * (1.0f / totalWeight);
                vertex.Normal = Normalize(skinnedNormal);
            }

            scene.Vertices[index] = vertex;
        }

        private static void AddSkinnedVertex(ModelPreviewScene scene, int boneIndex, float weight, Vec3 position, Vec3 normal, bool useRigidBoneOrigin, ref Vec3 skinnedPosition, ref Vec3 skinnedNormal, ref float totalWeight)
        {
            if (weight <= 0.0001f || boneIndex < 0 || boneIndex >= scene.BindBones.Count || boneIndex >= scene.Bones.Count)
            {
                return;
            }

            var bindBone = scene.BindBones[boneIndex];
            var poseBone = scene.Bones[boneIndex];
            var localPosition = useRigidBoneOrigin == true ? position : InverseSkinPoint(position, bindBone);
            var localNormal = useRigidBoneOrigin == true ? normal : InverseSkinDirection(normal, bindBone);
            skinnedPosition += TransformSkinPoint(localPosition, poseBone) * weight;
            skinnedNormal += TransformSkinDirection(localNormal, poseBone) * weight;
            totalWeight += weight;
        }

        private static Vec3 TransformSkinPoint(Vec3 value, ModelPreviewBone bone)
        {
            return bone.Position + TransformSkinDirection(value, bone);
        }

        private static Vec3 TransformSkinDirection(Vec3 value, ModelPreviewBone bone)
        {
            return TransformDirection(value, bone);
        }

        private static Vec3 InverseSkinPoint(Vec3 value, ModelPreviewBone bone)
        {
            return InverseSkinDirection(value - bone.Position, bone);
        }

        private static Vec3 InverseSkinDirection(Vec3 value, ModelPreviewBone bone)
        {
            return new Vec3(Dot(value, bone.AxisX), Dot(value, bone.AxisY), Dot(value, bone.AxisZ));
        }

        private static Vec3 TransformPoint(Vec3 value, ModelPreviewBone bone)
        {
            return bone.Position + TransformDirection(value, bone);
        }

        private static Vec3 InverseTransformPoint(Vec3 value, ModelPreviewBone bone)
        {
            return InverseTransformDirection(value - bone.Position, bone);
        }

        private static Vec3 InverseTransformDirection(Vec3 value, ModelPreviewBone bone)
        {
            return new Vec3(Dot(value, bone.AxisX), Dot(value, bone.AxisY), Dot(value, bone.AxisZ));
        }

        private static int AddVec3Accessor(MemoryStream binary, List<string> bufferViews, List<string> accessors, Vec3[] values, bool includeBounds)
        {
            Align(binary, 4);
            int offset = (int)binary.Position;
            var writer = new BinaryWriter(binary);
            {
                foreach (var value in values)
                {
                    writer.Write(value.X);
                    writer.Write(value.Y);
                    writer.Write(value.Z);
                }
            }

            int length = values.Length * 12;
            int view = AddBufferView(bufferViews, offset, length, 34962);
            var json = string.Format(CultureInfo.InvariantCulture, "{{\"bufferView\":{0},\"componentType\":5126,\"count\":{1},\"type\":\"VEC3\"", view, values.Length);
            if (includeBounds == true && values.Length > 0)
            {
                json += string.Format(CultureInfo.InvariantCulture,
                    ",\"min\":[{0:0.######},{1:0.######},{2:0.######}],\"max\":[{3:0.######},{4:0.######},{5:0.######}]",
                    values.Min(v => v.X), values.Min(v => v.Y), values.Min(v => v.Z),
                    values.Max(v => v.X), values.Max(v => v.Y), values.Max(v => v.Z));
            }
            json += "}";
            return AddAccessor(accessors, json);
        }

        private static int AddVec2Accessor(MemoryStream binary, List<string> bufferViews, List<string> accessors, float[][] values)
        {
            Align(binary, 4);
            int offset = (int)binary.Position;
            var writer = new BinaryWriter(binary);
            {
                foreach (var value in values)
                {
                    writer.Write(value[0]);
                    writer.Write(value[1]);
                }
            }

            int view = AddBufferView(bufferViews, offset, values.Length * 8, 34962);
            return AddAccessor(accessors, string.Format(CultureInfo.InvariantCulture, "{{\"bufferView\":{0},\"componentType\":5126,\"count\":{1},\"type\":\"VEC2\"}}", view, values.Length));
        }

        private static int AddQuatAccessor(MemoryStream binary, List<string> bufferViews, List<string> accessors, PreviewQuat[] values)
        {
            Align(binary, 4);
            int offset = (int)binary.Position;
            var writer = new BinaryWriter(binary);
            {
                foreach (var value in values)
                {
                    writer.Write(value.X);
                    writer.Write(value.Y);
                    writer.Write(value.Z);
                    writer.Write(value.W);
                }
            }

            int view = AddBufferView(bufferViews, offset, values.Length * 16, 34962);
            return AddAccessor(accessors, string.Format(CultureInfo.InvariantCulture, "{{\"bufferView\":{0},\"componentType\":5126,\"count\":{1},\"type\":\"VEC4\"}}", view, values.Length));
        }

        private static int AddWeightsAccessor(MemoryStream binary, List<string> bufferViews, List<string> accessors, List<ModelPreviewVertex> vertices)
        {
            Align(binary, 4);
            int offset = (int)binary.Position;
            var writer = new BinaryWriter(binary);
            {
                foreach (var vertex in vertices)
                {
                    float w0 = Math.Max(0.0f, vertex.Weight0);
                    float w1 = Math.Max(0.0f, vertex.Weight1);
                    float w2 = Math.Max(0.0f, vertex.Weight2);
                    float w3 = Math.Max(0.0f, vertex.Weight3);
                    float total = w0 + w1 + w2 + w3;
                    if (total <= 0.0001f)
                    {
                        w0 = 1.0f;
                        w1 = w2 = w3 = 0.0f;
                    }
                    else
                    {
                        w0 /= total;
                        w1 /= total;
                        w2 /= total;
                        w3 /= total;
                    }

                    writer.Write(w0);
                    writer.Write(w1);
                    writer.Write(w2);
                    writer.Write(w3);
                }
            }

            int view = AddBufferView(bufferViews, offset, vertices.Count * 16, 34962);
            return AddAccessor(accessors, string.Format(CultureInfo.InvariantCulture, "{{\"bufferView\":{0},\"componentType\":5126,\"count\":{1},\"type\":\"VEC4\"}}", view, vertices.Count));
        }

        private static int AddJointsAccessor(MemoryStream binary, List<string> bufferViews, List<string> accessors, List<ModelPreviewVertex> vertices, int boneCount)
        {
            Align(binary, 2);
            int offset = (int)binary.Position;
            var writer = new BinaryWriter(binary);
            {
                foreach (var vertex in vertices)
                {
                    writer.Write((ushort)ClampJoint(vertex.Bone0, boneCount));
                    writer.Write((ushort)ClampJoint(vertex.Bone1, boneCount));
                    writer.Write((ushort)ClampJoint(vertex.Bone2, boneCount));
                    writer.Write((ushort)ClampJoint(vertex.Bone3, boneCount));
                }
            }

            int view = AddBufferView(bufferViews, offset, vertices.Count * 8, 34962);
            return AddAccessor(accessors, string.Format(CultureInfo.InvariantCulture, "{{\"bufferView\":{0},\"componentType\":5123,\"count\":{1},\"type\":\"VEC4\"}}", view, vertices.Count));
        }

        private static int AddU16ScalarAccessor(MemoryStream binary, List<string> bufferViews, List<string> accessors, ushort[] values, bool elementArray)
        {
            Align(binary, 2);
            int offset = (int)binary.Position;
            var writer = new BinaryWriter(binary);
            {
                foreach (var value in values)
                {
                    writer.Write(value);
                }
            }

            int view = AddBufferView(bufferViews, offset, values.Length * 2, elementArray ? 34963 : 0);
            string bounds = values.Length > 0
                ? string.Format(CultureInfo.InvariantCulture, ",\"min\":[{0}],\"max\":[{1}]", values.Min(v => v), values.Max(v => v))
                : "";
            return AddAccessor(accessors, string.Format(CultureInfo.InvariantCulture, "{{\"bufferView\":{0},\"componentType\":5123,\"count\":{1},\"type\":\"SCALAR\"{2}}}", view, values.Length, bounds));
        }

        private static int AddIndexAccessor(MemoryStream binary, List<string> bufferViews, List<string> accessors, int[] values)
        {
            if (values == null || values.Length == 0 || values.Max() <= ushort.MaxValue)
            {
                var u16 = values == null
                              ? new ushort[0]
                              : values.Select(v => (ushort)Math.Max(0, v)).ToArray();
                return AddU16ScalarAccessor(binary, bufferViews, accessors, u16, true);
            }

            Align(binary, 4);
            int offset = (int)binary.Position;
            var writer = new BinaryWriter(binary);
            {
                foreach (var value in values)
                {
                    writer.Write((uint)Math.Max(0, value));
                }
            }

            int view = AddBufferView(bufferViews, offset, values.Length * 4, 34963);
            string bounds = values.Length > 0
                ? string.Format(CultureInfo.InvariantCulture, ",\"min\":[{0}],\"max\":[{1}]", values.Min(v => Math.Max(0, v)), values.Max(v => Math.Max(0, v)))
                : "";
            return AddAccessor(accessors, string.Format(CultureInfo.InvariantCulture, "{{\"bufferView\":{0},\"componentType\":5125,\"count\":{1},\"type\":\"SCALAR\"{2}}}", view, values.Length, bounds));
        }

        private static int AddAnimationTimeAccessor(MemoryStream binary, List<string> bufferViews, List<string> accessors, int frameCount, float frameRate)
        {
            Align(binary, 4);
            int offset = (int)binary.Position;
            var writer = new BinaryWriter(binary);
            {
                for (int i = 0; i < frameCount; i++)
                {
                    writer.Write(i / Math.Max(0.001f, frameRate));
                }
            }

            int view = AddBufferView(bufferViews, offset, frameCount * 4, 0);
            float max = (frameCount - 1) / Math.Max(0.001f, frameRate);
            return AddAccessor(accessors, string.Format(CultureInfo.InvariantCulture, "{{\"bufferView\":{0},\"componentType\":5126,\"count\":{1},\"type\":\"SCALAR\",\"min\":[0],\"max\":[{2:0.######}]}}", view, frameCount, max));
        }

        private static int AddMat4Accessor(MemoryStream binary, List<string> bufferViews, List<string> accessors, float[][] values)
        {
            Align(binary, 4);
            int offset = (int)binary.Position;
            var writer = new BinaryWriter(binary);
            {
                foreach (var matrix in values)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        writer.Write(matrix[i]);
                    }
                }
            }

            int view = AddBufferView(bufferViews, offset, values.Length * 64, 0);
            return AddAccessor(accessors, string.Format(CultureInfo.InvariantCulture, "{{\"bufferView\":{0},\"componentType\":5126,\"count\":{1},\"type\":\"MAT4\"}}", view, values.Length));
        }

        private static int AddBufferView(List<string> bufferViews, int offset, int length, int target)
        {
            string targetText = target == 0 ? "" : string.Format(CultureInfo.InvariantCulture, ",\"target\":{0}", target);
            bufferViews.Add(string.Format(CultureInfo.InvariantCulture, "{{\"buffer\":0,\"byteOffset\":{0},\"byteLength\":{1}{2}}}", offset, length, targetText));
            return bufferViews.Count - 1;
        }

        private static int AddTextureImageBufferView(MemoryStream binary, List<string> bufferViews, Bitmap texture)
        {
            if (texture == null)
            {
                return -1;
            }

            using (var image = new MemoryStream())
            {
                texture.Save(image, ImageFormat.Png);
                var bytes = image.ToArray();
                Align(binary, 4);
                int offset = (int)binary.Position;
                binary.Write(bytes, 0, bytes.Length);
                return AddBufferView(bufferViews, offset, bytes.Length, 0);
            }
        }

        private static int[] AddMaterialTextureImageBufferViews(
            MemoryStream binary,
            List<string> bufferViews,
            List<ModelPreviewMaterial> materials,
            Dictionary<string, int> materialLookup)
        {
            var imageBufferViews = Enumerable.Repeat(-1, Math.Max(1, materialLookup.Count)).ToArray();
            if (materials == null || materials.Count == 0 || materialLookup == null)
            {
                return imageBufferViews;
            }

            foreach (var material in materials)
            {
                if (material == null || material.Texture == null)
                {
                    continue;
                }

                int materialIndex;
                if (materialLookup.TryGetValue(GetMaterialName(material.ShaderName), out materialIndex) == false ||
                    materialIndex < 0 ||
                    materialIndex >= imageBufferViews.Length)
                {
                    continue;
                }

                imageBufferViews[materialIndex] = AddTextureImageBufferView(binary, bufferViews, material.Texture);
            }

            return imageBufferViews;
        }

        private static int AddAccessor(List<string> accessors, string json)
        {
            accessors.Add(json);
            return accessors.Count - 1;
        }

        private static Dictionary<string, int> BuildMaterialLookup(IEnumerable<ModelPreviewPart> parts)
        {
            var lookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var part in parts)
            {
                var name = GetMaterialName(part.ShaderName);
                if (lookup.ContainsKey(name) == false)
                {
                    lookup.Add(name, lookup.Count);
                }
            }

            if (lookup.Count == 0)
            {
                lookup.Add("preview_material", 0);
            }

            return lookup;
        }

        private static int GetMaterialIndex(Dictionary<string, int> lookup, string shaderName)
        {
            int index;
            return lookup.TryGetValue(GetMaterialName(shaderName), out index) == true ? index : 0;
        }

        private static string GetMaterialName(string shaderName)
        {
            return string.IsNullOrEmpty(shaderName) ? "preview_material" : shaderName;
        }

        private static string BuildGlbJson(
            ModelPreviewScene scene,
            Animation animation,
            long binaryLength,
            List<string> bufferViews,
            List<string> accessors,
            int positionAccessor,
            int normalAccessor,
            int uvAccessor,
            int jointsAccessor,
            int weightsAccessor,
            int indicesAccessor,
            int inverseBindAccessor,
            List<string> samplers,
            List<string> channels)
        {
            var json = new StringBuilder();
            int meshNode = scene.BindBones.Count;
            json.Append("{\"asset\":{\"version\":\"2.0\",\"generator\":\"Gibbed.Prototype.Edit3D\"},");
            json.AppendFormat(CultureInfo.InvariantCulture, "\"buffers\":[{{\"byteLength\":{0}}}],", binaryLength);
            json.Append("\"bufferViews\":[");
            json.Append(string.Join(",", bufferViews.ToArray()));
            json.Append("],\"accessors\":[");
            json.Append(string.Join(",", accessors.ToArray()));
            json.Append("],");
            json.Append("\"materials\":[{\"name\":\"preview_material\",\"pbrMetallicRoughness\":{\"baseColorFactor\":[0.75,0.75,0.72,1],\"metallicFactor\":0,\"roughnessFactor\":0.7}}],");
            json.AppendFormat(CultureInfo.InvariantCulture,
                "\"meshes\":[{{\"name\":{0},\"primitives\":[{{\"attributes\":{{\"POSITION\":{1},\"NORMAL\":{2},\"TEXCOORD_0\":{3},\"JOINTS_0\":{4},\"WEIGHTS_0\":{5}}},\"indices\":{6},\"material\":0}}]}}],",
                JsonString(scene.Name), positionAccessor, normalAccessor, uvAccessor, jointsAccessor, weightsAccessor, indicesAccessor);

            json.Append("\"nodes\":[");
            for (int i = 0; i < scene.BindBones.Count; i++)
            {
                if (i > 0)
                {
                    json.Append(",");
                }

                var bone = scene.BindBones[i];
                var rotation = ToQuaternion(bone.LocalAxisX, bone.LocalAxisY, bone.LocalAxisZ);
                json.Append("{");
                json.AppendFormat(CultureInfo.InvariantCulture, "\"name\":{0},\"translation\":[{1:0.######},{2:0.######},{3:0.######}],\"rotation\":[{4:0.########},{5:0.########},{6:0.########},{7:0.########}]",
                    JsonString(string.IsNullOrEmpty(bone.Name) ? "bone_" + i.ToString(CultureInfo.InvariantCulture) : bone.Name),
                    bone.LocalPosition.X, bone.LocalPosition.Y, bone.LocalPosition.Z,
                    rotation.X, rotation.Y, rotation.Z, rotation.W);
                var children = Enumerable.Range(0, scene.BindBones.Count).Where(j => scene.BindBones[j].Parent == i).ToList();
                if (children.Count > 0)
                {
                    json.Append(",\"children\":[");
                    json.Append(string.Join(",", children.Select(c => c.ToString(CultureInfo.InvariantCulture)).ToArray()));
                    json.Append("]");
                }
                json.Append("}");
            }

            json.AppendFormat(CultureInfo.InvariantCulture, ",{{\"name\":{0},\"mesh\":0,\"skin\":0}}", JsonString((scene.Name ?? "mesh") + "_mesh"));
            var rootNodes = Enumerable.Range(0, scene.BindBones.Count).Where(i => scene.BindBones[i].Parent < 0).Select(i => i.ToString(CultureInfo.InvariantCulture)).ToList();
            rootNodes.Add(meshNode.ToString(CultureInfo.InvariantCulture));
            int axisRootNode = scene.BindBones.Count + 1;
            json.AppendFormat(CultureInfo.InvariantCulture, ",{{\"scale\":[-1,1,1],\"children\":[{0}]}}", string.Join(",", rootNodes.ToArray()));
            json.Append("],");
            json.Append("\"skins\":[{\"inverseBindMatrices\":");
            json.Append(inverseBindAccessor.ToString(CultureInfo.InvariantCulture));
            json.Append(",\"joints\":[");
            json.Append(string.Join(",", Enumerable.Range(0, scene.BindBones.Count).Select(i => i.ToString(CultureInfo.InvariantCulture)).ToArray()));
            json.Append("]}],");
            json.Append("\"animations\":[{\"name\":");
            json.Append(JsonString(string.IsNullOrEmpty(animation.Name) ? "animation" : animation.Name));
            json.Append(",\"samplers\":[");
            json.Append(string.Join(",", samplers.ToArray()));
            json.Append("],\"channels\":[");
            json.Append(string.Join(",", channels.ToArray()));
            json.Append("]}],");
            json.Append("\"scenes\":[{\"nodes\":[");
            json.Append(axisRootNode.ToString(CultureInfo.InvariantCulture));
            json.Append("]}],\"scene\":0}");
            return json.ToString();
        }

        private static string BuildModelGlbJson(
            ModelPreviewScene scene,
            long binaryLength,
            List<string> bufferViews,
            List<string> accessors,
            List<GlbExportPart> parts,
            Dictionary<string, int> materialLookup,
            int inverseBindAccessor,
            int[] materialTextureImageBufferViews)
        {
            bool skinned = scene.BindBones != null && scene.BindBones.Count > 0;
            var json = new StringBuilder();
            json.Append("{\"asset\":{\"version\":\"2.0\",\"generator\":\"Gibbed.Prototype.Edit3D\"},");
            json.AppendFormat(CultureInfo.InvariantCulture, "\"buffers\":[{{\"byteLength\":{0}}}],", binaryLength);
            json.Append("\"bufferViews\":[");
            json.Append(string.Join(",", bufferViews.ToArray()));
            json.Append("],\"accessors\":[");
            json.Append(string.Join(",", accessors.ToArray()));
            json.Append("],");
            var materialNames = materialLookup.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToArray();
            var materialTextures = materialTextureImageBufferViews ?? new int[0];
            var exportedTextureLookup = new Dictionary<int, int>();
            for (int i = 0; i < materialNames.Length; i++)
            {
                int bufferView = i < materialTextures.Length ? materialTextures[i] : -1;
                if (bufferView >= 0 && exportedTextureLookup.ContainsKey(bufferView) == false)
                {
                    exportedTextureLookup.Add(bufferView, exportedTextureLookup.Count);
                }
            }

            if (exportedTextureLookup.Count > 0)
            {
                var imageEntries = exportedTextureLookup.OrderBy(kv => kv.Value)
                                                        .Select(kv => string.Format(CultureInfo.InvariantCulture, "{{\"mimeType\":\"image/png\",\"bufferView\":{0}}}", kv.Key))
                                                        .ToArray();
                json.Append("\"images\":[");
                json.Append(string.Join(",", imageEntries));
                json.Append("],\"textures\":[");
                json.Append(string.Join(",", Enumerable.Range(0, imageEntries.Length).Select(i => string.Format(CultureInfo.InvariantCulture, "{{\"source\":{0}}}", i)).ToArray()));
                json.Append("],");
            }

            json.Append("\"materials\":[");
            for (int i = 0; i < materialNames.Length; i++)
            {
                if (i > 0)
                {
                    json.Append(",");
                }

                json.Append("{\"name\":");
                json.Append(JsonString(materialNames[i]));
                int textureIndex;
                int materialBufferView = i < materialTextures.Length ? materialTextures[i] : -1;
                if (materialBufferView >= 0 && exportedTextureLookup.TryGetValue(materialBufferView, out textureIndex) == true)
                {
                    json.AppendFormat(CultureInfo.InvariantCulture, ",\"pbrMetallicRoughness\":{{\"baseColorTexture\":{{\"index\":{0}}},\"metallicFactor\":0,\"roughnessFactor\":0.7}}", textureIndex);
                }
                else
                {
                    json.Append(",\"pbrMetallicRoughness\":{\"baseColorFactor\":[0.75,0.75,0.72,1],\"metallicFactor\":0,\"roughnessFactor\":0.7}");
                }
                json.Append("}");
            }
            json.Append("],");

            json.Append("\"meshes\":[");
            for (int i = 0; i < parts.Count; i++)
            {
                if (i > 0)
                {
                    json.Append(",");
                }

                var part = parts[i];
                json.AppendFormat(CultureInfo.InvariantCulture,
                    "{{\"name\":{0},\"primitives\":[{{\"attributes\":{{\"POSITION\":{1},\"NORMAL\":{2},\"TEXCOORD_0\":{3}",
                    JsonString(string.IsNullOrEmpty(part.Name) ? (scene.Name ?? "mesh") + "_" + i.ToString(CultureInfo.InvariantCulture) : part.Name),
                    part.PositionAccessor,
                    part.NormalAccessor,
                    part.UvAccessor);
                if (skinned == true)
                {
                    json.AppendFormat(CultureInfo.InvariantCulture, ",\"JOINTS_0\":{0},\"WEIGHTS_0\":{1}", part.JointsAccessor, part.WeightsAccessor);
                }
                json.AppendFormat(CultureInfo.InvariantCulture, "}},\"indices\":{0},\"material\":{1}}}]}}", part.IndicesAccessor, part.Material);
            }
            json.Append("],");

            json.Append("\"nodes\":[");
            if (skinned == true)
            {
                for (int i = 0; i < scene.BindBones.Count; i++)
                {
                    if (i > 0)
                    {
                        json.Append(",");
                    }

                    var bone = scene.BindBones[i];
                    var rotation = ToQuaternion(bone.LocalAxisX, bone.LocalAxisY, bone.LocalAxisZ);
                    json.Append("{");
                    json.AppendFormat(CultureInfo.InvariantCulture, "\"name\":{0},\"translation\":[{1:0.######},{2:0.######},{3:0.######}],\"rotation\":[{4:0.########},{5:0.########},{6:0.########},{7:0.########}]",
                        JsonString(string.IsNullOrEmpty(bone.Name) ? "bone_" + i.ToString(CultureInfo.InvariantCulture) : bone.Name),
                        bone.LocalPosition.X, bone.LocalPosition.Y, bone.LocalPosition.Z,
                        rotation.X, rotation.Y, rotation.Z, rotation.W);
                    var children = Enumerable.Range(0, scene.BindBones.Count).Where(j => scene.BindBones[j].Parent == i).ToList();
                    if (children.Count > 0)
                    {
                        json.Append(",\"children\":[");
                        json.Append(string.Join(",", children.Select(c => c.ToString(CultureInfo.InvariantCulture)).ToArray()));
                        json.Append("]");
                    }
                    json.Append("}");
                }

                for (int i = 0; i < parts.Count; i++)
                {
                    json.AppendFormat(CultureInfo.InvariantCulture, ",{{\"name\":{0},\"mesh\":{1},\"skin\":0}}",
                        JsonString(string.IsNullOrEmpty(parts[i].Name) ? (scene.Name ?? "mesh") + "_" + i.ToString(CultureInfo.InvariantCulture) : parts[i].Name),
                        i);
                }
                var rootNodes = Enumerable.Range(0, scene.BindBones.Count).Where(i => scene.BindBones[i].Parent < 0).Select(i => i.ToString(CultureInfo.InvariantCulture)).ToList();
                rootNodes.AddRange(Enumerable.Range(scene.BindBones.Count, parts.Count).Select(i => i.ToString(CultureInfo.InvariantCulture)));
                int axisRootNode = scene.BindBones.Count + parts.Count;
                json.AppendFormat(CultureInfo.InvariantCulture, ",{{\"scale\":[-1,1,1],\"children\":[{0}]}}", string.Join(",", rootNodes.ToArray()));
                json.Append("],");
                json.Append("\"skins\":[{\"inverseBindMatrices\":");
                json.Append(inverseBindAccessor.ToString(CultureInfo.InvariantCulture));
                json.Append(",\"joints\":[");
                json.Append(string.Join(",", Enumerable.Range(0, scene.BindBones.Count).Select(i => i.ToString(CultureInfo.InvariantCulture)).ToArray()));
                json.Append("]}],");
                json.Append("\"scenes\":[{\"nodes\":[");
                json.Append(axisRootNode.ToString(CultureInfo.InvariantCulture));
                json.Append("]}],\"scene\":0}");
            }
            else
            {
                for (int i = 0; i < parts.Count; i++)
                {
                    if (i > 0)
                    {
                        json.Append(",");
                    }

                    json.AppendFormat(CultureInfo.InvariantCulture, "{{\"name\":{0},\"mesh\":{1}}}",
                        JsonString(string.IsNullOrEmpty(parts[i].Name) ? (scene.Name ?? "mesh") + "_" + i.ToString(CultureInfo.InvariantCulture) : parts[i].Name),
                        i);
                }
                var rootNodes = Enumerable.Range(0, parts.Count).Select(i => i.ToString(CultureInfo.InvariantCulture)).ToList();
                int axisRootNode = parts.Count;
                json.AppendFormat(CultureInfo.InvariantCulture, ",{{\"scale\":[-1,1,1],\"children\":[{0}]}}", string.Join(",", rootNodes.ToArray()));
                json.Append("],\"scenes\":[{\"nodes\":[");
                json.Append(axisRootNode.ToString(CultureInfo.InvariantCulture));
                json.Append("]}],\"scene\":0}");
            }

            return json.ToString();
        }

        private static void WriteGlb(Stream output, string json, byte[] binary)
        {
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            int jsonLength = AlignLength(jsonBytes.Length, 4);
            int binLength = AlignLength(binary.Length, 4);
            var writer = new BinaryWriter(output);
            {
                writer.Write(0x46546C67);
                writer.Write(2);
                writer.Write(12 + 8 + jsonLength + 8 + binLength);
                writer.Write(jsonLength);
                writer.Write(0x4E4F534A);
                writer.Write(jsonBytes);
                for (int i = jsonBytes.Length; i < jsonLength; i++)
                {
                    writer.Write((byte)0x20);
                }
                writer.Write(binLength);
                writer.Write(0x004E4942);
                writer.Write(binary);
                for (int i = binary.Length; i < binLength; i++)
                {
                    writer.Write((byte)0);
                }
            }
        }

        private static float[] GetInverseBindMatrix(ModelPreviewBone bone)
        {
            return new[]
            {
                bone.AxisX.X, bone.AxisY.X, bone.AxisZ.X, 0.0f,
                bone.AxisX.Y, bone.AxisY.Y, bone.AxisZ.Y, 0.0f,
                bone.AxisX.Z, bone.AxisY.Z, bone.AxisZ.Z, 0.0f,
                -Dot(bone.Position, bone.AxisX), -Dot(bone.Position, bone.AxisY), -Dot(bone.Position, bone.AxisZ), 1.0f,
            };
        }

        private static PreviewQuat ToQuaternion(Vec3 axisX, Vec3 axisY, Vec3 axisZ)
        {
            float m00 = axisX.X, m01 = axisY.X, m02 = axisZ.X;
            float m10 = axisX.Y, m11 = axisY.Y, m12 = axisZ.Y;
            float m20 = axisX.Z, m21 = axisY.Z, m22 = axisZ.Z;
            float trace = m00 + m11 + m22;
            PreviewQuat q;
            if (trace > 0.0f)
            {
                float s = (float)Math.Sqrt(trace + 1.0f) * 2.0f;
                q = new PreviewQuat
                {
                    W = 0.25f * s,
                    X = (m21 - m12) / s,
                    Y = (m02 - m20) / s,
                    Z = (m10 - m01) / s,
                };
            }
            else if (m00 > m11 && m00 > m22)
            {
                float s = (float)Math.Sqrt(1.0f + m00 - m11 - m22) * 2.0f;
                q = new PreviewQuat
                {
                    W = (m21 - m12) / s,
                    X = 0.25f * s,
                    Y = (m01 + m10) / s,
                    Z = (m02 + m20) / s,
                };
            }
            else if (m11 > m22)
            {
                float s = (float)Math.Sqrt(1.0f + m11 - m00 - m22) * 2.0f;
                q = new PreviewQuat
                {
                    W = (m02 - m20) / s,
                    X = (m01 + m10) / s,
                    Y = 0.25f * s,
                    Z = (m12 + m21) / s,
                };
            }
            else
            {
                float s = (float)Math.Sqrt(1.0f + m22 - m00 - m11) * 2.0f;
                q = new PreviewQuat
                {
                    W = (m10 - m01) / s,
                    X = (m02 + m20) / s,
                    Y = (m12 + m21) / s,
                    Z = 0.25f * s,
                };
            }

            return Normalize(q);
        }

        private static string JsonString(string value)
        {
            if (value == null)
            {
                value = "";
            }

            return "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n") + "\"";
        }

        private static int ClampJoint(int value, int boneCount)
        {
            return value >= 0 && value < boneCount ? value : 0;
        }

        private static void Align(MemoryStream stream, int alignment)
        {
            int aligned = AlignLength((int)stream.Position, alignment);
            while (stream.Position < aligned)
            {
                stream.WriteByte(0);
            }
        }

        private static int AlignLength(int value, int alignment)
        {
            return (value + alignment - 1) & ~(alignment - 1);
        }

        private static List<ModelPreviewBone> CloneBones(List<ModelPreviewBone> bones)
        {
            return bones == null ? new List<ModelPreviewBone>() : bones.ToList();
        }

        private static PreviewQuat ToPreviewQuat(Quaternion value)
        {
            return Normalize(new PreviewQuat
            {
                W = value.W,
                X = value.X,
                Y = value.Y,
                Z = value.Z,
            });
        }

        private static PreviewQuat ToPreviewQuat(Vector4 value)
        {
            return Normalize(new PreviewQuat
            {
                W = value.W,
                X = value.X,
                Y = value.Y,
                Z = value.Z,
            });
        }

        private static Vec3 ToVec3(Vector3 value)
        {
            return new Vec3(value.X, value.Y, value.Z);
        }

        private static Vec3 ToVec3(Vector4 value)
        {
            return new Vec3(value.X, value.Y, value.Z);
        }

        private static Vec3 ToPreviewPosition(Vector3 value)
        {
            return new Vec3(value.X, value.Y, value.Z);
        }

        private static Vec3 ToPreviewPosition(Vector4 value)
        {
            return new Vec3(value.X, value.Y, value.Z);
        }

        private static Vec3 ToPreviewDirection(Vector3 value)
        {
            return new Vec3(value.X, value.Y, value.Z);
        }

        private static Vec3 ToPreviewDirection(Vec3 value)
        {
            return new Vec3(value.X, value.Y, value.Z);
        }

        private static PreviewQuat SampleRotation(ModelPreviewAnimationChannel channel, float frame)
        {
            if (channel.RotationFrames.Length == 1 || channel.Rotations.Length == 1)
            {
                return channel.Rotations[0];
            }

            int next = 0;
            while (next + 1 < channel.RotationFrames.Length && channel.RotationFrames[next + 1] <= frame)
            {
                next++;
            }

            int a = next;
            int b = Math.Min(next + 1, channel.RotationFrames.Length - 1);
            float span = Math.Max(0.001f, channel.RotationFrames[b] - channel.RotationFrames[a]);
            float t = b == a ? 0.0f : Clamp01((frame - channel.RotationFrames[a]) / span);
            return Slerp(channel.Rotations[a], channel.Rotations[b], t);
        }

        private static Vec3 SampleTranslation(ModelPreviewAnimationChannel channel, float frame)
        {
            if (channel.TranslationFrames.Length == 1 || channel.Translations.Length == 1)
            {
                return channel.Translations[0];
            }

            int next = 0;
            while (next + 1 < channel.TranslationFrames.Length && channel.TranslationFrames[next + 1] <= frame)
            {
                next++;
            }

            int a = next;
            int b = Math.Min(next + 1, channel.TranslationFrames.Length - 1);
            float span = Math.Max(0.001f, channel.TranslationFrames[b] - channel.TranslationFrames[a]);
            float t = b == a ? 0.0f : Clamp01((frame - channel.TranslationFrames[a]) / span);
            return Lerp(channel.Translations[a], channel.Translations[b], t);
        }

        private static PreviewQuat Slerp(PreviewQuat a, PreviewQuat b, float t)
        {
            float dot = a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
            if (dot < 0.0f)
            {
                b.W = -b.W;
                b.X = -b.X;
                b.Y = -b.Y;
                b.Z = -b.Z;
                dot = -dot;
            }

            if (dot > 0.9995f)
            {
                return Normalize(new PreviewQuat
                {
                    W = a.W + (b.W - a.W) * t,
                    X = a.X + (b.X - a.X) * t,
                    Y = a.Y + (b.Y - a.Y) * t,
                    Z = a.Z + (b.Z - a.Z) * t,
                });
            }

            float theta0 = (float)Math.Acos(Clamp01(dot));
            float theta = theta0 * t;
            float sinTheta = (float)Math.Sin(theta);
            float sinTheta0 = (float)Math.Sin(theta0);
            float s0 = (float)Math.Cos(theta) - dot * sinTheta / sinTheta0;
            float s1 = sinTheta / sinTheta0;
            return new PreviewQuat
            {
                W = a.W * s0 + b.W * s1,
                X = a.X * s0 + b.X * s1,
                Y = a.Y * s0 + b.Y * s1,
                Z = a.Z * s0 + b.Z * s1,
            };
        }

        private static Vec3 Rotate(PreviewQuat q, Vec3 value)
        {
            var u = new Vec3(q.X, q.Y, q.Z);
            var uv = Cross(u, value);
            var uuv = Cross(u, uv);
            return value + uv * (2.0f * q.W) + uuv * 2.0f;
        }

        private static PreviewQuat CreateEulerDegreesRotation(Vec3 degrees)
        {
            if (LengthSquared(degrees) <= 0.000001f)
            {
                return new PreviewQuat { W = 1.0f };
            }

            var qx = CreateAxisAngle(new Vec3(1.0f, 0.0f, 0.0f), ToRadians(degrees.X));
            var qy = CreateAxisAngle(new Vec3(0.0f, 1.0f, 0.0f), ToRadians(degrees.Y));
            var qz = CreateAxisAngle(new Vec3(0.0f, 0.0f, 1.0f), ToRadians(degrees.Z));
            return Normalize(Multiply(qz, Multiply(qy, qx)));
        }

        private static PreviewQuat CreateAxisAngle(Vec3 axis, float radians)
        {
            var half = radians * 0.5f;
            var sin = (float)Math.Sin(half);
            return Normalize(new PreviewQuat
            {
                W = (float)Math.Cos(half),
                X = axis.X * sin,
                Y = axis.Y * sin,
                Z = axis.Z * sin,
            });
        }

        private static PreviewQuat Multiply(PreviewQuat a, PreviewQuat b)
        {
            if (IsIdentity(a) == true)
            {
                return b;
            }

            if (IsIdentity(b) == true)
            {
                return a;
            }

            return Normalize(new PreviewQuat
            {
                W = a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                X = a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                Y = a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X,
                Z = a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W,
            });
        }

        private static float ToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180.0f;
        }

        private static Vec3 RotateLocalBasis(PreviewQuat rotation, Vec3 axis, ModelPreviewBone bindBone)
        {
            return Normalize(Rotate(rotation, axis));
        }

        private static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
        }

        private static Vec3 Lerp(Vec3 a, Vec3 b, float t)
        {
            return new Vec3(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t);
        }

        private static float Dot(Vec3 a, Vec3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        private static PreviewQuat Normalize(PreviewQuat value)
        {
            float length = (float)Math.Sqrt(value.W * value.W + value.X * value.X + value.Y * value.Y + value.Z * value.Z);
            if (length <= 0.0001f)
            {
                return new PreviewQuat { W = 1.0f };
            }

            return new PreviewQuat
            {
                W = value.W / length,
                X = value.X / length,
                Y = value.Y / length,
                Z = value.Z / length,
            };
        }

        private static bool IsIdentity(PreviewQuat value)
        {
            return Math.Abs(value.X) <= 0.000001f &&
                   Math.Abs(value.Y) <= 0.000001f &&
                   Math.Abs(value.Z) <= 0.000001f &&
                   (Math.Abs(value.W) <= 0.000001f || Math.Abs(Math.Abs(value.W) - 1.0f) <= 0.000001f);
        }

        private static float Clamp01(float value)
        {
            return Math.Max(0.0f, Math.Min(1.0f, value));
        }

        private static void FindDiffuseTexture(Pure3DFile file, IEnumerable<string> shaderNames, out Bitmap bitmap, out string textureName)
        {
            var nodes = Flatten(file.Nodes).ToList();
            var shaderNameSet = new HashSet<string>(shaderNames.Where(n => string.IsNullOrEmpty(n) == false), StringComparer.OrdinalIgnoreCase);
            var shader = nodes.OfType<NewShader>().FirstOrDefault(s => shaderNameSet.Contains(s.Name));
            var diffuseName = shader == null
                                  ? null
                                  : shader.Children.OfType<U00011016>().Where(v => string.Equals(v.Name, "color", StringComparison.OrdinalIgnoreCase))
                                          .Select(v => v.Value).FirstOrDefault();
            textureName = diffuseName;
            var texture = nodes.OfType<Texture>().FirstOrDefault(t => string.Equals(t.Name, diffuseName, StringComparison.OrdinalIgnoreCase));
            var dds = texture == null ? null : texture.Children.OfType<TextureDDS>().FirstOrDefault();
            var data = dds == null ? null : dds.Children.OfType<TextureData>().FirstOrDefault();
            bitmap = data == null ? null : DecodeDds(data.Data);
        }

        private static void ResolveExpressions(Pure3DFile file, ModelPreviewScene scene)
        {
            if (scene == null)
            {
                return;
            }

            scene.Expressions.Clear();
            if (file == null)
            {
                return;
            }

            foreach (var expression in Flatten(file.Nodes).OfType<Expression>())
            {
                if (string.IsNullOrEmpty(expression.Name) == true ||
                    expression.Unknown3 == null ||
                    expression.Unknown4 == null)
                {
                    continue;
                }

                int count = Math.Min(expression.Unknown3.Length, expression.Unknown4.Length);
                var previewExpression = new ModelPreviewExpression
                {
                    Name = expression.Name,
                };

                for (int i = 0; i < count; i++)
                {
                    if (expression.Unknown4[i] > int.MaxValue)
                    {
                        continue;
                    }

                    previewExpression.Targets.Add(new ModelPreviewExpressionTarget
                    {
                        Value = expression.Unknown3[i],
                        TargetIndex = (int)expression.Unknown4[i],
                    });
                }

                if (previewExpression.Targets.Count > 0 &&
                    scene.Expressions.ContainsKey(previewExpression.Name) == false)
                {
                    scene.Expressions.Add(previewExpression.Name, previewExpression);
                }
            }
        }

        private static void ResolveMaterials(Pure3DFile file, ModelPreviewScene scene)
        {
            scene.Materials.Clear();
            scene.Texture = null;
            scene.TextureName = null;

            if (file == null || scene == null || scene.Parts == null || scene.Parts.Count == 0)
            {
                return;
            }

            var nodes = Flatten(file.Nodes).ToList();
            var materialLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < scene.Parts.Count; i++)
            {
                var part = scene.Parts[i];
                var shaderName = string.IsNullOrEmpty(part.ShaderName) ? string.Empty : part.ShaderName;
                int materialIndex;
                if (materialLookup.TryGetValue(shaderName, out materialIndex) == false)
                {
                    string textureName;
                    string normalTextureName;
                    string specularTextureName;
                    Bitmap texture;
                    Bitmap normalTexture;
                    Bitmap specularTexture;
                    string shaderTemplateName;
                    float specularMultiply;
                    float specularPower;
                    bool hasSpecParams;
                    ResolveShaderMaterial(
                        nodes,
                        shaderName,
                        out texture,
                        out textureName,
                        out normalTexture,
                        out normalTextureName,
                        out specularTexture,
                        out specularTextureName,
                        out specularMultiply,
                        out specularPower,
                        out hasSpecParams,
                        out shaderTemplateName);
                    materialIndex = scene.Materials.Count;
                    scene.Materials.Add(new ModelPreviewMaterial
                    {
                        ShaderName = shaderName,
                        ShaderTemplateName = shaderTemplateName,
                        TextureName = textureName,
                        NormalTextureName = normalTextureName,
                        SpecularTextureName = specularTextureName,
                        Texture = texture,
                        NormalTexture = normalTexture,
                        SpecularTexture = specularTexture,
                        SpecularMultiply = specularMultiply,
                        SpecularPower = specularPower,
                        HasSpecParams = hasSpecParams,
                        HasAlpha = HasTextureAlpha(texture),
                    });
                    materialLookup.Add(shaderName, materialIndex);

                    if (scene.Texture == null && texture != null)
                    {
                        scene.Texture = texture;
                        scene.TextureName = textureName;
                    }
                }

                part.MaterialIndex = materialIndex;
                scene.Parts[i] = part;
            }
        }

        private static void ResolveShaderMaterial(
            IEnumerable<BaseNode> nodes,
            string shaderName,
            out Bitmap diffuseBitmap,
            out string diffuseTextureName,
            out Bitmap normalBitmap,
            out string normalTextureName,
            out Bitmap specularBitmap,
            out string specularTextureName,
            out float specularMultiply,
            out float specularPower,
            out bool hasSpecParams,
            out string shaderTemplateName)
        {
            var shader = string.IsNullOrEmpty(shaderName)
                             ? null
                             : nodes.OfType<NewShader>().FirstOrDefault(s => string.Equals(s.Name, shaderName, StringComparison.OrdinalIgnoreCase));
            shaderTemplateName = shader == null || string.IsNullOrEmpty(shader.ShaderTemplateName) == true
                                     ? "Unknown"
                                     : shader.ShaderTemplateName;
            diffuseTextureName = shader == null ? null : GetShaderTextureName(shader, "color");
            if (string.IsNullOrEmpty(diffuseTextureName) == true && shader != null)
            {
                diffuseTextureName = GetShaderTextureName(shader, "camo");
            }

            normalTextureName = shader == null ? null : GetShaderTextureName(shader, "normal");
            specularTextureName = shader == null ? null : GetShaderTextureName(shader, "specular");
            diffuseBitmap = DecodeTexture(nodes, diffuseTextureName);
            normalBitmap = DecodeTexture(nodes, normalTextureName);
            specularBitmap = DecodeTexture(nodes, specularTextureName);

            specularMultiply = 0.0f;
            specularPower = 8.0f;
            hasSpecParams = false;
            var specParams = shader == null
                                 ? null
                                 : shader.Children.OfType<U00011018>()
                                         .FirstOrDefault(v => string.Equals(v.Name, "specParams", StringComparison.OrdinalIgnoreCase));
            if (specParams != null && specParams.Value != null)
            {
                specularMultiply = Math.Max(0.0f, specParams.Value.X);
                specularPower = Math.Max(1.0f, specParams.Value.Y);
                hasSpecParams = true;
            }
        }

        private static string GetShaderTextureName(NewShader shader, string propertyName)
        {
            if (shader == null || string.IsNullOrEmpty(propertyName) == true)
            {
                return null;
            }

            return shader.Children.OfType<U00011016>()
                         .Where(v => string.Equals(v.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                         .Select(v => v.Value)
                         .FirstOrDefault();
        }

        private static Bitmap DecodeTexture(IEnumerable<BaseNode> nodes, string textureName)
        {
            var texture = string.IsNullOrEmpty(textureName)
                              ? null
                              : nodes.OfType<Texture>().FirstOrDefault(t => string.Equals(t.Name, textureName, StringComparison.OrdinalIgnoreCase));
            var dds = texture == null ? null : texture.Children.OfType<TextureDDS>().FirstOrDefault();
            var data = dds == null ? null : dds.Children.OfType<TextureData>().FirstOrDefault();
            return data == null ? null : DecodeDds(data.Data);
        }

        private static bool HasTextureAlpha(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return false;
            }

            int total = bitmap.Width * bitmap.Height;
            int transparent = 0;
            int translucent = 0;
            int minimumAlpha = 255;
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int alpha = bitmap.GetPixel(x, y).A;
                    minimumAlpha = Math.Min(minimumAlpha, alpha);
                    if (alpha < 32)
                    {
                        transparent++;
                    }
                    else if (alpha < 240)
                    {
                        translucent++;
                    }
                }
            }

            int meaningfulAlpha = transparent + translucent;
            if (minimumAlpha >= 240 || meaningfulAlpha < 16)
            {
                return false;
            }

            float alphaCoverage = meaningfulAlpha / (float)Math.Max(1, total);
            float transparentCoverage = transparent / (float)Math.Max(1, total);
            return transparentCoverage >= 0.01f || alphaCoverage >= 0.04f;
        }

        private static Bitmap DecodeDds(byte[] data)
        {
            if (data == null || data.Length < 128 || data[0] != 'D' || data[1] != 'D' || data[2] != 'S')
            {
                return null;
            }

            int height = BitConverter.ToInt32(data, 12);
            int width = BitConverter.ToInt32(data, 16);
            uint fourCc = BitConverter.ToUInt32(data, 84);
            if ((fourCc != 0x31545844 && fourCc != 0x33545844 && fourCc != 0x35545844) || width <= 0 || height <= 0)
            {
                return null;
            }

            var bitmap = new Bitmap(width, height);
            int offset = 128;
            for (int by = 0; by < height; by += 4)
            {
                for (int bx = 0; bx < width; bx += 4)
                {
                    int blockSize = fourCc == 0x31545844 ? 8 : 16;
                    if (offset + blockSize > data.Length)
                    {
                        return bitmap;
                    }

                    var alpha = DecodeDxtAlpha(data, offset, fourCc);
                    int colorOffset = fourCc == 0x31545844 ? offset : offset + 8;
                    var colors = DecodeDxtColors(BitConverter.ToUInt16(data, colorOffset), BitConverter.ToUInt16(data, colorOffset + 2), fourCc == 0x31545844);
                    uint bits = BitConverter.ToUInt32(data, colorOffset + 4);
                    offset += blockSize;

                    for (int py = 0; py < 4; py++)
                    {
                        for (int px = 0; px < 4; px++)
                        {
                            int code = (int)((bits >> (2 * (py * 4 + px))) & 3);
                            if (bx + px < width && by + py < height)
                            {
                                int alphaIndex = py * 4 + px;
                                bitmap.SetPixel(bx + px, by + py, Color.FromArgb(Math.Min(alpha[alphaIndex], colors[code].A), colors[code]));
                            }
                        }
                    }
                }
            }

            return bitmap;
        }

        private static Color[] DecodeDxtColors(ushort c0, ushort c1, bool allowTransparent)
        {
            Color a = Rgb565(c0), b = Rgb565(c1);
            if (allowTransparent == true && c0 <= c1)
            {
                return new[]
                {
                    a,
                    b,
                    Color.FromArgb((a.R + b.R) / 2, (a.G + b.G) / 2, (a.B + b.B) / 2),
                    Color.FromArgb(0, 0, 0, 0),
                };
            }

            return new[]
            {
                a,
                b,
                Color.FromArgb((2 * a.R + b.R) / 3, (2 * a.G + b.G) / 3, (2 * a.B + b.B) / 3),
                Color.FromArgb((a.R + 2 * b.R) / 3, (a.G + 2 * b.G) / 3, (a.B + 2 * b.B) / 3),
            };
        }

        private static int[] DecodeDxtAlpha(byte[] data, int offset, uint fourCc)
        {
            var alpha = Enumerable.Repeat(255, 16).ToArray();
            if (fourCc == 0x33545844)
            {
                ulong bits = BitConverter.ToUInt64(data, offset);
                for (int i = 0; i < 16; i++)
                {
                    alpha[i] = (int)(((bits >> (i * 4)) & 0xF) * 17);
                }
            }
            else if (fourCc == 0x35545844)
            {
                int a0 = data[offset];
                int a1 = data[offset + 1];
                var table = new int[8];
                table[0] = a0;
                table[1] = a1;
                if (a0 > a1)
                {
                    table[2] = (6 * a0 + a1) / 7;
                    table[3] = (5 * a0 + 2 * a1) / 7;
                    table[4] = (4 * a0 + 3 * a1) / 7;
                    table[5] = (3 * a0 + 4 * a1) / 7;
                    table[6] = (2 * a0 + 5 * a1) / 7;
                    table[7] = (a0 + 6 * a1) / 7;
                }
                else
                {
                    table[2] = (4 * a0 + a1) / 5;
                    table[3] = (3 * a0 + 2 * a1) / 5;
                    table[4] = (2 * a0 + 3 * a1) / 5;
                    table[5] = (a0 + 4 * a1) / 5;
                    table[6] = 0;
                    table[7] = 255;
                }

                ulong bits = 0;
                for (int i = 0; i < 6; i++)
                {
                    bits |= ((ulong)data[offset + 2 + i]) << (8 * i);
                }

                for (int i = 0; i < 16; i++)
                {
                    alpha[i] = table[(int)((bits >> (i * 3)) & 7)];
                }
            }

            return alpha;
        }

        private static Color Rgb565(ushort value)
        {
            int r = ((value >> 11) & 31) * 255 / 31;
            int g = ((value >> 5) & 63) * 255 / 63;
            int b = (value & 31) * 255 / 31;
            return Color.FromArgb(r, g, b);
        }

        private static IEnumerable<BaseNode> Flatten(IEnumerable<BaseNode> nodes)
        {
            foreach (var node in nodes)
            {
                yield return node;
                foreach (var child in Flatten(node.Children))
                {
                    yield return child;
                }
            }
        }

        private static ModelPreviewScene WithLocators(Pure3DFile file, ModelPreviewScene scene)
        {
            RegisterLocators(file, scene);
            return scene;
        }

        private static void RegisterLocators(Pure3DFile file, ModelPreviewScene scene)
        {
            if (scene == null)
            {
                return;
            }

            scene.Locators = new List<ModelPreviewLocator>();
            if (file == null || file.Nodes == null)
            {
                return;
            }

            foreach (var locator in Flatten(file.Nodes).OfType<Locator>())
            {
                if (locator.Position == null)
                {
                    continue;
                }

                scene.Locators.Add(new ModelPreviewLocator
                {
                    Name = string.IsNullOrEmpty(locator.Name) == true ? "Locator" : locator.Name,
                    Position = ToPreviewPosition(locator.Position),
                });
            }
        }

        private static float LengthSquared(Vec3 value)
        {
            return value.X * value.X + value.Y * value.Y + value.Z * value.Z;
        }

        private static Vec3 Normalize(Vec3 value)
        {
            float length = (float)Math.Sqrt(LengthSquared(value));
            return length > 0.0001f ? value * (1.0f / length) : value;
        }

        private static int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}
