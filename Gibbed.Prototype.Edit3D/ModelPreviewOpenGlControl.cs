using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpGL;
using SharpGL.Shaders;
using SharpGL.Version;

namespace Gibbed.Prototype.Edit3D
{
    internal sealed class ModelPreviewOpenGlControl : OpenGLControl
    {
        private const int MaxGpuSkinningBones = 96;
        private const int GpuBoneIndicesAttribute = 6;
        private const int GpuBoneWeightsAttribute = 7;
        public event EventHandler RenderFrameStarting;
        public event EventHandler<CutsceneTimelineScrubEventArgs> CutsceneTimelineScrubRequested;
        public event EventHandler<VertexMorphScrubEventArgs> VertexMorphScrubRequested;

        private ModelPreviewScene _Scene;
        private ModelPreviewCamera _Camera;
        private readonly Dictionary<int, uint> _MaterialTextureIds = new Dictionary<int, uint>();
        private bool _TextureDirty = true;
        private bool _OpenGlInitialized;
        private Point _MousePosition;
        private int _HoveredBone = -1;
        private readonly double[] _ProjectionMatrix = new double[16];
        private readonly double[] _ModelViewMatrix = new double[16];
        private readonly int[] _Viewport = new int[4];
        private bool _ProjectionCacheValid;
        private bool _CutsceneTimelineScrubbing;
        private bool _CutsceneTimelineScrubberHovered;
        private RectangleF _CutsceneTimelineBounds;
        private RectangleF _CutsceneTimelineScrubberBounds;
        private bool _VertexMorphScrubbing;
        private bool _VertexMorphScrubberHovered;
        private RectangleF _VertexMorphScrubberBounds;
        private RectangleF _VertexMorphScrubberThumbBounds;
        private ModelPreviewRenderCache _RenderCache = new ModelPreviewRenderCache();
        private readonly ModelPreviewFrameProfiler _Profiler = new ModelPreviewFrameProfiler();
        private ShaderProgram _GpuSkinningProgram;
        private bool _GpuSkinningUnavailable;
        private int _GpuBoneIndicesLocation = -1;
        private int _GpuBoneWeightsLocation = -1;
        private bool _CurrentPartUsesTexture;
        private float _CurrentPartAlphaCutoff;
        private string _GpuSkinningStatus;
        private float[] _GpuPartBoneIndices;

        public string AnimationStatus { get; set; }
        public string RootMotionStatus { get; set; }
        public string MeshStatus { get; set; }
        public string CutsceneStatus { get; set; }
        public string CutsceneLightGroupStatus { get; set; }
        public float CutsceneFadeAlpha { get; set; }
        public List<CutsceneTimelineOverlayItem> CutsceneTimelineItems { get; private set; }
        public float CutsceneTimelineSeconds { get; set; }
        public float CutsceneTimelineDurationSeconds { get; set; }
        public bool VertexMorphScrubberVisible { get; set; }
        public float VertexMorphScrubberValue { get; set; }
        public string VertexMorphScrubberLabel { get; set; }

        public ModelPreviewOpenGlControl()
        {
            this.RenderContextType = RenderContextType.NativeWindow;
            this.OpenGLVersion = OpenGLVersion.OpenGL2_1;
            this.RenderTrigger = SharpGL.RenderTrigger.Manual;
            this.FrameRate = 60;
            this.DrawFPS = true;
            this.BackColor = Color.Black;
            this.Dock = DockStyle.Fill;
            this.TabStop = true;
            this.OpenGLInitialized += this.OnOpenGlInitialized;
            this.OpenGLDraw += this.OnOpenGlDraw;
            this.MouseMove += this.OnPreviewMouseMove;
            this.MouseDown += this.OnPreviewMouseDown;
            this.MouseUp += this.OnPreviewMouseUp;
            this.MouseLeave += this.OnPreviewMouseLeave;
            this.CutsceneTimelineItems = new List<CutsceneTimelineOverlayItem>();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (this._OpenGlInitialized == false && this.DesignMode == false)
            {
                this._OpenGlInitialized = true;
                ((ISupportInitialize)this).EndInit();
            }
        }

        public void SetPreview(ModelPreviewScene scene, ModelPreviewCamera camera)
        {
            if (!object.ReferenceEquals(this._Scene, scene))
            {
                this._TextureDirty = true;
                this._RenderCache.Clear();
                this._GpuSkinningUnavailable = false;
            }

            this._Scene = scene;
            this._Camera = camera;
            this.UpdateHoveredBone();
            this.Invalidate();
        }

        public void RetryGpuSkinning()
        {
            this._GpuSkinningUnavailable = false;
            if (this._Scene != null)
            {
                this._Scene.UseGpuSkinning = true;
            }
        }

        public void RenderPreview()
        {
            if (this.Visible == true && this.IsHandleCreated == true && this._OpenGlInitialized == true)
            {
                this.DoRender();
            }
        }

        public bool IsCutsceneTimelineAt(Point point)
        {
            return this.CutsceneTimelineDurationSeconds > 0.0f &&
                   this._CutsceneTimelineBounds.Width > 0.0f &&
                   this._CutsceneTimelineBounds.Contains(point);
        }

        public bool IsVertexMorphScrubberAt(Point point)
        {
            return this.VertexMorphScrubberVisible == true &&
                   this._VertexMorphScrubberBounds.Width > 0.0f &&
                   this._VertexMorphScrubberBounds.Contains(point);
        }

        private void OnOpenGlInitialized(object sender, EventArgs e)
        {
            var gl = this.OpenGL;
            gl.ClearColor(0.075f, 0.085f, 0.095f, 1.0f);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_NORMALIZE);
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_AMBIENT_AND_DIFFUSE);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
        }

        private void OnOpenGlDraw(object sender, SharpGL.RenderEventArgs args)
        {
            this._Profiler.BeginFrame();
            var renderFrameStarting = this.RenderFrameStarting;
            if (renderFrameStarting != null)
            {
                this._Profiler.BeginSection(ModelPreviewProfileSection.Animation);
                renderFrameStarting(this, EventArgs.Empty);
                this._Profiler.EndSection(ModelPreviewProfileSection.Animation);
            }

            var gl = this.OpenGL;
            int width = Math.Max(1, this.Width);
            int height = Math.Max(1, this.Height);

            this._Profiler.BeginSection(ModelPreviewProfileSection.Clear);
            this.ResetOpenGlRenderState(gl);
            gl.Viewport(0, 0, width, height);
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            this._Profiler.EndSection(ModelPreviewProfileSection.Clear);

            if (this._Scene == null || this._Camera == null)
            {
                gl.Flush();
                this._Profiler.EndFrame();
                return;
            }

            this._Profiler.BeginSection(ModelPreviewProfileSection.Textures);
            this.UploadMaterialTexturesIfNeeded(gl);
            this._Profiler.EndSection(ModelPreviewProfileSection.Textures);
            this._Profiler.BeginSection(ModelPreviewProfileSection.Camera);
            this.SetupProjection(gl, width, height);
            this.SetupCamera(gl);
            this.CaptureProjection(gl);
            this.UpdateHoveredBone();
            this.SetupLights(gl);
            this._Profiler.EndSection(ModelPreviewProfileSection.Camera);

            if (this._Scene.Unlit == false && this._Camera.UseCutsceneCamera == false)
            {
                this._Profiler.BeginSection(ModelPreviewProfileSection.Grid);
                this.DrawGrid(gl);
                this._Profiler.EndSection(ModelPreviewProfileSection.Grid);
            }
            this._Profiler.BeginSection(ModelPreviewProfileSection.Model);
            this.ResetOpenGlRenderState(gl);
            this.DrawModel(gl);
            this._Profiler.EndSection(ModelPreviewProfileSection.Model);
            if (this._Camera.ShowSkeleton == true)
            {
                this._Profiler.BeginSection(ModelPreviewProfileSection.Skeleton);
                gl.Disable(OpenGL.GL_DEPTH_TEST);
                this.DrawSkeleton(gl);
                gl.Enable(OpenGL.GL_DEPTH_TEST);
                this._Profiler.EndSection(ModelPreviewProfileSection.Skeleton);
            }
            if (this._Camera.ShowLocators == true && this._Scene.Locators != null && this._Scene.Locators.Count > 0)
            {
                gl.Disable(OpenGL.GL_DEPTH_TEST);
                this.DrawLocators(gl);
                gl.Enable(OpenGL.GL_DEPTH_TEST);
            }
            if (this._Scene.Lights != null && this._Scene.Lights.Count > 0)
            {
                this._Profiler.BeginSection(ModelPreviewProfileSection.Lights);
                gl.Disable(OpenGL.GL_DEPTH_TEST);
                this.DrawLightGizmos(gl);
                gl.Enable(OpenGL.GL_DEPTH_TEST);
                this._Profiler.EndSection(ModelPreviewProfileSection.Lights);
            }
            if (this._Camera.UseCutsceneCamera == false && this._Camera.HasCutsceneCamera == true)
            {
                this._Profiler.BeginSection(ModelPreviewProfileSection.Frustum);
                gl.Disable(OpenGL.GL_DEPTH_TEST);
                this.DrawCutsceneCameraFrustum(gl, width, height);
                gl.Enable(OpenGL.GL_DEPTH_TEST);
                this._Profiler.EndSection(ModelPreviewProfileSection.Frustum);
            }

            this._Profiler.BeginSection(ModelPreviewProfileSection.Overlay);
            this.DrawOverlay(gl, width, height);
            this._Profiler.EndSection(ModelPreviewProfileSection.Overlay);
            this._Profiler.BeginSection(ModelPreviewProfileSection.Flush);
            gl.Flush();
            this._Profiler.EndSection(ModelPreviewProfileSection.Flush);
            this._Profiler.EndFrame();
        }

        private void UploadMaterialTexturesIfNeeded(OpenGL gl)
        {
            if (this._TextureDirty == false)
            {
                return;
            }

            this._TextureDirty = false;
            foreach (var textureId in this._MaterialTextureIds.Values)
            {
                if (textureId != 0)
                {
                    gl.DeleteTextures(1, new[] { textureId });
                }
            }

            this._MaterialTextureIds.Clear();
            if (this._Scene == null || this._Scene.Materials == null)
            {
                return;
            }

            for (int i = 0; i < this._Scene.Materials.Count; i++)
            {
                var material = this._Scene.Materials[i];
                if (material == null || material.Texture == null)
                {
                    continue;
                }

                var textureIds = new uint[1];
                gl.GenTextures(1, textureIds);
                var textureId = textureIds[0];
                this._MaterialTextureIds[i] = textureId;
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, textureId);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, (int)OpenGL.GL_LINEAR);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, (int)OpenGL.GL_LINEAR);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, (int)OpenGL.GL_REPEAT);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, (int)OpenGL.GL_REPEAT);

                var rgba = GetRgbaPixels(material.Texture);
                gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGBA, material.Texture.Width, material.Texture.Height, 0, OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, rgba);
            }
        }

        private void ResetOpenGlRenderState(OpenGL gl)
        {
            if (gl == null)
            {
                return;
            }

            try
            {
                gl.UseProgram(0);
            }
            catch
            {
            }

            this.DisableClientArrays(gl);
            gl.Disable(OpenGL.GL_CULL_FACE);
            gl.FrontFace(OpenGL.GL_CCW);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.DepthMask(1);
            gl.Enable(OpenGL.GL_NORMALIZE);
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_AMBIENT_AND_DIFFUSE);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Disable(OpenGL.GL_ALPHA_TEST);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        private static byte[] GetRgbaPixels(Bitmap bitmap)
        {
            var rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var data = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                int stride = Math.Abs(data.Stride);
                var bgra = new byte[stride * bitmap.Height];
                Marshal.Copy(data.Scan0, bgra, 0, bgra.Length);
                var rgba = new byte[bitmap.Width * bitmap.Height * 4];
                for (int y = 0; y < bitmap.Height; y++)
                {
                    int sourceRow = y * data.Stride;
                    if (data.Stride < 0)
                    {
                        sourceRow = (bitmap.Height - 1 - y) * stride;
                    }

                    int targetRow = y * bitmap.Width * 4;
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int source = sourceRow + x * 4;
                        int target = targetRow + x * 4;
                        rgba[target + 0] = bgra[source + 2];
                        rgba[target + 1] = bgra[source + 1];
                        rgba[target + 2] = bgra[source + 0];
                        rgba[target + 3] = bgra[source + 3];
                    }
                }

                return rgba;
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
        }

        private void SetupProjection(OpenGL gl, int width, int height)
        {
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            double radius = Math.Max(0.001, this.GetCameraControlRadius());
            double aspect = (double)width / Math.Max(1, height);
            double fov = this._Camera.UseCutsceneCamera == true && this._Camera.CutsceneFov > 0.001f
                             ? ConvertHorizontalFovToVertical(this._Camera.CutsceneFov, aspect)
                             : 45.0;
            double near = Math.Max(0.001, radius * 0.01);
            double far = radius * 100.0;
            if (this._Camera.HasCutsceneCamera == true)
            {
                near = this._Camera.CutsceneNearClip > 0.000001f ? this._Camera.CutsceneNearClip : 0.01;
                far = this._Camera.CutsceneFarClip > near ? this._Camera.CutsceneFarClip : 100000.0;
            }
            gl.Perspective(fov, aspect, near, far);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
        }

        private static double ConvertHorizontalFovToVertical(double horizontalDegrees, double aspect)
        {
            if (double.IsNaN(horizontalDegrees) == true || horizontalDegrees <= 0.001)
            {
                return 45.0;
            }

            aspect = Math.Max(0.001, aspect);
            double horizontalRadians = horizontalDegrees * Math.PI / 180.0;
            double verticalRadians = 2.0 * Math.Atan(Math.Tan(horizontalRadians * 0.5) / aspect);
            return verticalRadians * 180.0 / Math.PI;
        }

        private void SetupCamera(OpenGL gl)
        {
            if (this._Camera.UseCutsceneCamera == true)
            {
                var cutsceneEye = ToDisplayPosition(this._Camera.CutscenePosition);
                var cutsceneTarget = ToDisplayPosition(this._Camera.CutscenePosition + this._Camera.CutsceneLook);
                var cutsceneUp = ToDisplayDirection(LengthSquared(this._Camera.CutsceneUp) > 0.000001f ? this._Camera.CutsceneUp : new Vec3(0, 1, 0));
                gl.LookAt(cutsceneEye.X, cutsceneEye.Y, cutsceneEye.Z, cutsceneTarget.X, cutsceneTarget.Y, cutsceneTarget.Z, cutsceneUp.X, cutsceneUp.Y, cutsceneUp.Z);
                return;
            }

            if (this._Camera.FreeCamera == true)
            {
                float cyFree = (float)Math.Cos(this._Camera.Yaw);
                float syFree = (float)Math.Sin(this._Camera.Yaw);
                float cpFree = (float)Math.Cos(this._Camera.Pitch);
                float spFree = (float)Math.Sin(this._Camera.Pitch);
                var freeForward = Normalize(new Vec3(syFree * cpFree, -spFree, cyFree * cpFree));
                var freeRight = Normalize(new Vec3(cyFree, 0.0f, -syFree));
                var freeUp = Normalize(Cross(freeForward, freeRight));
                var freeEye = ToDisplayPosition(this._Camera.FreePosition);
                var freeTarget = ToDisplayPosition(this._Camera.FreePosition + freeForward);
                var displayUp = ToDisplayDirection(freeUp);
                gl.LookAt(freeEye.X, freeEye.Y, freeEye.Z, freeTarget.X, freeTarget.Y, freeTarget.Z, displayUp.X, displayUp.Y, displayUp.Z);
                return;
            }

            var origin = ToDisplayPosition(this._Camera.Origin);
            float radius = Math.Max(0.001f, this.GetCameraControlRadius());
            float distance = radius * 3.0f / Math.Max(0.05f, this._Camera.Zoom);
            float cy = (float)Math.Cos(this._Camera.Yaw);
            float sy = (float)Math.Sin(this._Camera.Yaw);
            float cp = (float)Math.Cos(this._Camera.Pitch);
            float sp = (float)Math.Sin(this._Camera.Pitch);
            var forward = Normalize(new Vec3(sy * cp, -sp, cy * cp));
            var right = Normalize(new Vec3(cy, 0.0f, -sy));
            var up = Normalize(Cross(forward, right));
            float panScale = radius * 2.0f / Math.Max(1, Math.Min(this.Width, this.Height));
            origin += right * (-this._Camera.PanX * panScale);
            origin += up * (this._Camera.PanY * panScale);
            var eye = origin - forward * distance;
            gl.LookAt(eye.X, eye.Y, eye.Z, origin.X, origin.Y, origin.Z, up.X, up.Y, up.Z);
        }

        private void CaptureProjection(OpenGL gl)
        {
            try
            {
                gl.GetDouble(OpenGL.GL_PROJECTION_MATRIX, this._ProjectionMatrix);
                gl.GetDouble(OpenGL.GL_MODELVIEW_MATRIX, this._ModelViewMatrix);
                gl.GetInteger(OpenGL.GL_VIEWPORT, this._Viewport);
                this._ProjectionCacheValid = true;
            }
            catch
            {
                this._ProjectionCacheValid = false;
            }
        }

        private void SetupLights(OpenGL gl)
        {
            this.DisableAllLights(gl);
            if (this._Camera == null || this._Camera.LightingEnabled == false)
            {
                gl.Disable(OpenGL.GL_LIGHTING);
                return;
            }

            gl.Enable(OpenGL.GL_LIGHTING);
            if (this._Scene != null && this._Scene.UseCinematicLighting == true)
            {
                this.SetupCinematicLights(gl, null);
                return;
            }

            this.SetupStudioLights(gl);
        }

        private void DisableAllLights(OpenGL gl)
        {
            gl.Disable(OpenGL.GL_LIGHT0);
            gl.Disable(OpenGL.GL_LIGHT1);
            gl.Disable(OpenGL.GL_LIGHT2);
            gl.Disable(OpenGL.GL_LIGHT3);
            gl.Disable(OpenGL.GL_LIGHT4);
            gl.Disable(OpenGL.GL_LIGHT5);
            gl.Disable(OpenGL.GL_LIGHT6);
            gl.Disable(OpenGL.GL_LIGHT7);
        }

        private void SetupCinematicLights(OpenGL gl, ModelPreviewPart? part)
        {
            this.DisableAllLights(gl);
            if (this._Camera == null || this._Camera.LightingEnabled == false)
            {
                gl.Disable(OpenGL.GL_LIGHTING);
                return;
            }

            if (this._Scene == null || this._Scene.Lights == null || this._Scene.Lights.Count == 0)
            {
                gl.Disable(OpenGL.GL_LIGHTING);
                return;
            }

            var lights = this._Scene.Lights
                .Where(l => l != null &&
                            (part.HasValue == true
                                 ? this.LightAffectsPart(l, part.Value)
                                 : LightAffectsPart(l, part)) == true)
                .Take(8)
                .ToList();
            if (lights.Count == 0)
            {
                gl.Disable(OpenGL.GL_LIGHTING);
                return;
            }

            gl.Enable(OpenGL.GL_LIGHTING);
            for (int i = 0; i < lights.Count; i++)
            {
                var light = lights[i];
                uint glLight = (uint)(OpenGL.GL_LIGHT0 + i);
                var position = ToDisplayPosition(light.Position);
                float intensity = Math.Max(0.0f, light.Intensity);
                gl.Enable(glLight);
                gl.Light(glLight, OpenGL.GL_POSITION, new[] { position.X, position.Y, position.Z, 1.0f });
                gl.Light(glLight, OpenGL.GL_DIFFUSE, new[] { light.Color.X * intensity, light.Color.Y * intensity, light.Color.Z * intensity, 1.0f });
                gl.Light(glLight, OpenGL.GL_AMBIENT, new[] { 0.02f, 0.02f, 0.02f, 1.0f });
                gl.Light(glLight, OpenGL.GL_CONSTANT_ATTENUATION, 1.0f);
                gl.Light(glLight, OpenGL.GL_LINEAR_ATTENUATION, light.Range > 0.001f ? 1.0f / light.Range : 0.0f);
            }
        }

        private static bool LightAffectsPart(ModelPreviewLight light, ModelPreviewPart? part)
        {
            if (light == null)
            {
                return false;
            }

            bool scoped = light.TargetActorHash != 0 ||
                          string.IsNullOrEmpty(light.TargetActorName) == false ||
                          light.LightGroupHash != 0 ||
                          string.IsNullOrEmpty(light.LightGroupName) == false;
            if (scoped == false)
            {
                return light.RequiresTarget == false;
            }

            if (part.HasValue == false)
            {
                return false;
            }

            var value = part.Value;
            if (string.IsNullOrEmpty(light.TargetActorName) == false &&
                (NameMatches(value.ObjectName, light.TargetActorName) == true ||
                 NameMatches(value.Name, light.TargetActorName) == true))
            {
                return true;
            }

            return false;
        }

        private bool LightAffectsPart(ModelPreviewLight light, ModelPreviewPart part)
        {
            if (LightAffectsPart(light, (ModelPreviewPart?)part) == true)
            {
                return true;
            }

            if (light == null || this._Scene == null || this._Scene.Instances == null)
            {
                return false;
            }

            var instance = this.ResolvePartInstance(part);
            if (instance == null)
            {
                return false;
            }

            if (light.TargetActorHash != 0 && instance.ActorHash == light.TargetActorHash)
            {
                return true;
            }

            if (light.LightGroupHash != 0 && instance.LightGroupHash == light.LightGroupHash)
            {
                return true;
            }

            if (string.IsNullOrEmpty(light.LightGroupName) == false &&
                string.Equals(instance.LightGroupName, light.LightGroupName, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            return string.IsNullOrEmpty(light.TargetActorName) == false &&
                   (NameMatches(instance.Name, light.TargetActorName) == true ||
                    NameMatches(part.ObjectName, light.TargetActorName) == true);
        }

        private ModelPreviewSceneInstance ResolvePartInstance(ModelPreviewPart part)
        {
            if (this._Scene == null || this._Scene.Instances == null)
            {
                return null;
            }

            int partStart = part.VertexStart;
            int partEnd = part.VertexCount <= 0 ? partStart : partStart + part.VertexCount;
            return this._Scene.Instances.FirstOrDefault(i =>
                i != null &&
                ((partStart >= i.VertexStart && partStart < i.VertexStart + Math.Max(0, i.VertexCount)) ||
                 (partEnd > i.VertexStart && partEnd <= i.VertexStart + Math.Max(0, i.VertexCount)) ||
                 NameMatches(i.Name, part.ObjectName) == true));
        }

        private static bool NameMatches(string actual, string expected)
        {
            if (string.IsNullOrEmpty(actual) == true || string.IsNullOrEmpty(expected) == true)
            {
                return false;
            }

            return string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase) == true ||
                   actual.IndexOf(expected, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   expected.IndexOf(actual, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool NameStartsWith(string actual, string expectedPrefix)
        {
            if (string.IsNullOrEmpty(actual) == true || string.IsNullOrEmpty(expectedPrefix) == true)
            {
                return false;
            }

            return actual.StartsWith(expectedPrefix, StringComparison.OrdinalIgnoreCase) == true;
        }

        private void SetupStudioLights(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_LIGHT0);
            gl.Enable(OpenGL.GL_LIGHT1);
            gl.Enable(OpenGL.GL_LIGHT2);
            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, new[] { 0.22f, 0.23f, 0.25f, 1.0f });

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, new[] { -0.85f, 0.35f, 0.35f, 0.0f });
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, new[] { 0.50f, 0.56f, 0.68f, 1.0f });
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, new[] { 0.08f, 0.09f, 0.10f, 1.0f });
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, new[] { 0.18f, 0.20f, 0.24f, 1.0f });

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, new[] { 0.25f, 0.45f, -1.0f, 0.0f });
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, new[] { 0.44f, 0.52f, 0.70f, 1.0f });
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, new[] { 0.0f, 0.0f, 0.0f, 1.0f });
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, new[] { 0.45f, 0.52f, 0.70f, 1.0f });

            gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_POSITION, new[] { 0.45f, 0.95f, 0.70f, 0.0f });
            gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_DIFFUSE, new[] { 1.10f, 1.04f, 0.92f, 1.0f });
            gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_AMBIENT, new[] { 0.0f, 0.0f, 0.0f, 1.0f });
            gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_SPECULAR, new[] { 0.85f, 0.80f, 0.70f, 1.0f });
        }

        private void DrawGrid(OpenGL gl)
        {
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.LineWidth(1.0f);
            float radius = Math.Max(1.0f, this.GetCameraControlRadius());
            float step = NiceGridStep(radius);
            int lines = 16;
            gl.Begin(OpenGL.GL_LINES);
            for (int i = -lines; i <= lines; i++)
            {
                float v = i * step;
                float alpha = i == 0 ? 0.75f : 0.25f;
                gl.Color(0.35f, 0.38f, 0.40f, alpha);
                gl.Vertex(-lines * step, 0.0f, v);
                gl.Vertex(lines * step, 0.0f, v);
                gl.Vertex(v, 0.0f, -lines * step);
                gl.Vertex(v, 0.0f, lines * step);
            }

            gl.Color(0.85f, 0.15f, 0.15f);
            gl.Vertex(0.0f, 0.01f, 0.0f);
            gl.Vertex(step * 2.0f, 0.01f, 0.0f);
            gl.Color(0.15f, 0.85f, 0.15f);
            gl.Vertex(0.0f, 0.01f, 0.0f);
            gl.Vertex(0.0f, step * 2.0f, 0.0f);
            gl.Color(0.15f, 0.45f, 1.0f);
            gl.Vertex(0.0f, 0.01f, 0.0f);
            gl.Vertex(0.0f, 0.01f, step * 2.0f);
            gl.End();
        }

        private static float NiceGridStep(float radius)
        {
            var raw = radius / 8.0f;
            var power = (float)Math.Pow(10.0, Math.Floor(Math.Log10(raw)));
            var normalized = raw / power;
            if (normalized >= 5.0f)
            {
                return power * 5.0f;
            }
            if (normalized >= 2.0f)
            {
                return power * 2.0f;
            }
            return power;
        }

        private void DrawModel(OpenGL gl)
        {
            if (this._Scene.Vertices.Count == 0 || this._Scene.Indices.Count == 0)
            {
                return;
            }

            this._GpuSkinningStatus = null;
            bool influencePreview = this._Camera.ShowInfluences == true;
            bool materialPreview = influencePreview == false && this._Camera.ShowMaterialPreview == true && this._Scene.Unlit == false;
            bool drawFilled = this._Camera.WireMode != ModelPreviewWireMode.Wireframe;
            bool drawWire = this._Camera.WireMode != ModelPreviewWireMode.Off;

            var parts = this._Scene.Parts.Count > 0
                            ? this._Scene.Parts
                            : new List<ModelPreviewPart>
                              {
                                  new ModelPreviewPart
                                  {
                                      IndexStart = 0,
                                      IndexCount = this._Scene.Indices.Count,
                                  },
                              };
            this._Profiler.ResetDrawCounters(parts.Count, this._Scene.Vertices.Count, this._Scene.Indices.Count);
            this._Profiler.BeginSection(ModelPreviewProfileSection.Cache);
            this.UpdateRenderCache(parts);
            this._Profiler.EndSection(ModelPreviewProfileSection.Cache);
            if (drawFilled == true)
            {
                this.DrawFilledParts(gl, parts, false, influencePreview, materialPreview);
                if (influencePreview == false && materialPreview == false && this._Camera.TextureMode == ModelPreviewTextureMode.Default)
                {
                    this.DrawFilledParts(gl, parts, true, influencePreview, materialPreview);
                }
                this.DisableClientArrays(gl);
            }

            if (drawWire == true)
            {
                this.DrawWireParts(gl, parts);
            }

            gl.Enable(OpenGL.GL_BLEND);
            gl.Disable(OpenGL.GL_ALPHA_TEST);
            gl.DepthMask(1);
        }

        private void DrawWireParts(OpenGL gl, IList<ModelPreviewPart> parts)
        {
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.Disable(OpenGL.GL_BLEND);
            gl.Disable(OpenGL.GL_ALPHA_TEST);
            gl.LineWidth(this._Camera.WireMode == ModelPreviewWireMode.Wireframe ? 1.25f : 1.0f);
            gl.Color(0.02f, 0.02f, 0.02f, 1.0f);

            bool overlay = this._Camera.WireMode == ModelPreviewWireMode.Overlay;
            if (overlay == true)
            {
                gl.Disable(OpenGL.GL_DEPTH_TEST);
            }

            this._CurrentPartUsesTexture = false;
            this._CurrentPartAlphaCutoff = 0.0f;
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
            try
            {
                foreach (var part in parts)
                {
                    if (part.Hidden == true)
                    {
                        continue;
                    }

                    if (this.IsPartVisible(part, width: this.Width, height: this.Height) == false)
                    {
                        continue;
                    }

                    this.DrawPartWithClientArrays(gl, part, null, false);
                }
            }
            finally
            {
                gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                if (overlay == true)
                {
                    gl.Enable(OpenGL.GL_DEPTH_TEST);
                }
            }
        }

        private void DrawFilledParts(OpenGL gl, IList<ModelPreviewPart> parts, bool translucentPass, bool influencePreview, bool materialPreview)
        {
            if (this._Scene.Unlit == true || this._Camera.LightingEnabled == false)
            {
                gl.Disable(OpenGL.GL_LIGHTING);
            }
            else
            {
                gl.Enable(OpenGL.GL_LIGHTING);
            }

            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            if (translucentPass == true)
            {
                gl.Enable(OpenGL.GL_BLEND);
                gl.Enable(OpenGL.GL_ALPHA_TEST);
                gl.AlphaFunc(OpenGL.GL_GREATER, 0.18f);
                gl.DepthMask(0);
            }
            else
            {
                gl.Disable(OpenGL.GL_ALPHA_TEST);
                gl.DepthMask(1);
            }

            if (translucentPass == true)
            {
                this.DrawSortedTranslucentParts(gl, parts);
                gl.DepthMask(1);
                gl.Disable(OpenGL.GL_ALPHA_TEST);
                return;
            }

            var drawParts = parts.Where(p => this.IsPartAlphaBlended(p) == false).ToList();
            foreach (var part in drawParts)
            {
                if (part.Hidden == true)
                {
                    this._Profiler.HiddenParts++;
                    continue;
                }

                if (this.IsPartVisible(part, width: this.Width, height: this.Height) == false)
                {
                    this._Profiler.CulledParts++;
                    continue;
                }

                if (this._Scene.UseCinematicLighting == true &&
                    this._Scene.Unlit == false &&
                    this._Camera.LightingEnabled == true)
                {
                    this.SetupCinematicLights(gl, part);
                }

                this.ApplyPartMaterial(gl, part, influencePreview, materialPreview, translucentPass);
                this.DrawPartWithClientArrays(gl, part);
            }
        }

        private void DrawSortedTranslucentParts(OpenGL gl, IList<ModelPreviewPart> parts)
        {
            foreach (var part in parts.Where(this.IsPartAlphaBlended).OrderBy(p => this.GetPartViewDepth(p)))
            {
                if (part.Hidden == true)
                {
                    continue;
                }

                if (this.IsPartVisible(part, width: this.Width, height: this.Height) == false)
                {
                    continue;
                }

                int partIndex = this.ResolvePartIndex(part);
                if (partIndex < 0 ||
                    this._RenderCache.PartIndices == null ||
                    partIndex >= this._RenderCache.PartIndices.Length ||
                    this._RenderCache.PartIndices[partIndex] == null)
                {
                    continue;
                }

                if (this._Scene.UseCinematicLighting == true &&
                    this._Scene.Unlit == false &&
                    this._Camera.LightingEnabled == true)
                {
                    this.SetupCinematicLights(gl, part);
                }

                var sortedIndices = this.BuildSortedTranslucentPartIndices(partIndex);
                this.ApplyPartMaterial(gl, part, false, false, true);
                this.DrawPartWithClientArrays(gl, part, sortedIndices);
            }
        }

        private uint[] BuildSortedTranslucentPartIndices(int partIndex)
        {
            var indices = this._RenderCache.PartIndices[partIndex];
            if (indices == null || indices.Length < 6)
            {
                return indices;
            }

            int triangleCount = indices.Length / 3;
            if (this._RenderCache.AlphaSortTriangles == null || this._RenderCache.AlphaSortTriangles.Length <= partIndex)
            {
                this._RenderCache.AlphaSortTriangles = new ModelPreviewAlphaTriangle[this._RenderCache.PartCount][];
            }
            if (this._RenderCache.AlphaSortTriangles[partIndex] == null ||
                this._RenderCache.AlphaSortTriangles[partIndex].Length != triangleCount)
            {
                this._RenderCache.AlphaSortTriangles[partIndex] = new ModelPreviewAlphaTriangle[triangleCount];
            }
            var triangles = this._RenderCache.AlphaSortTriangles[partIndex];
            for (int triangleIndex = 0; triangleIndex < triangleCount; triangleIndex++)
            {
                int offset = triangleIndex * 3;
                uint i0 = indices[offset + 0];
                uint i1 = indices[offset + 1];
                uint i2 = indices[offset + 2];
                triangles[triangleIndex] = new ModelPreviewAlphaTriangle
                {
                    I0 = i0,
                    I1 = i1,
                    I2 = i2,
                    Depth = (this.GetVertexViewDepth(i0) + this.GetVertexViewDepth(i1) + this.GetVertexViewDepth(i2)) / 3.0,
                    OriginalIndex = triangleIndex,
                };
            }

            double depthEpsilon = Math.Max(0.0001, this.GetCameraControlRadius() * 0.00035);
            Array.Sort(triangles, (a, b) => CompareAlphaTriangles(a, b, depthEpsilon));
            if (this._RenderCache.SortedAlphaPartIndices == null || this._RenderCache.SortedAlphaPartIndices.Length <= partIndex)
            {
                this._RenderCache.SortedAlphaPartIndices = new uint[this._RenderCache.PartCount][];
            }
            if (this._RenderCache.SortedAlphaPartIndices[partIndex] == null ||
                this._RenderCache.SortedAlphaPartIndices[partIndex].Length != triangleCount * 3)
            {
                this._RenderCache.SortedAlphaPartIndices[partIndex] = new uint[triangleCount * 3];
            }
            var sorted = this._RenderCache.SortedAlphaPartIndices[partIndex];
            for (int triangleIndex = 0; triangleIndex < triangles.Length; triangleIndex++)
            {
                int offset = triangleIndex * 3;
                sorted[offset + 0] = triangles[triangleIndex].I0;
                sorted[offset + 1] = triangles[triangleIndex].I1;
                sorted[offset + 2] = triangles[triangleIndex].I2;
            }

            return sorted;
        }

        private static int CompareAlphaTriangles(ModelPreviewAlphaTriangle a, ModelPreviewAlphaTriangle b, double depthEpsilon)
        {
            double delta = a.Depth - b.Depth;
            if (Math.Abs(delta) > depthEpsilon)
            {
                return delta < 0.0 ? -1 : 1;
            }

            return a.OriginalIndex.CompareTo(b.OriginalIndex);
        }

        private void ApplyPartMaterial(OpenGL gl, ModelPreviewPart part, bool influencePreview, bool materialPreview, bool translucentPass)
        {
            this._CurrentPartAlphaCutoff = translucentPass == true ? 0.18f : 0.0f;
            if (influencePreview == true)
            {
                this._CurrentPartUsesTexture = false;
                gl.Disable(OpenGL.GL_TEXTURE_2D);
                gl.Disable(OpenGL.GL_BLEND);
                gl.Color(1.0f, 1.0f, 1.0f, 1.0f);
                return;
            }

            if (materialPreview == true)
            {
                var color = GetMaterialDebugColor(part.MaterialIndex, part.ShaderName);
                this._CurrentPartUsesTexture = false;
                gl.Disable(OpenGL.GL_TEXTURE_2D);
                gl.Disable(OpenGL.GL_BLEND);
                gl.Disable(OpenGL.GL_LIGHTING);
                gl.Color(color.R, color.G, color.B, 1.0f);
                return;
            }

            this.ApplyMaterialLighting(gl, part.MaterialIndex);
            uint textureId;
            if (this._Camera.TextureMode != ModelPreviewTextureMode.None &&
                this._MaterialTextureIds.TryGetValue(part.MaterialIndex, out textureId) == true && textureId != 0)
            {
                this._CurrentPartUsesTexture = true;
                gl.Enable(OpenGL.GL_TEXTURE_2D);
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, textureId);
                if (translucentPass == true)
                {
                    gl.Enable(OpenGL.GL_BLEND);
                }
                else
                {
                    gl.Disable(OpenGL.GL_BLEND);
                }
                gl.Color(1.0f, 1.0f, 1.0f, 1.0f);
                return;
            }

            this._CurrentPartUsesTexture = false;
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.Disable(OpenGL.GL_BLEND);
            gl.Color(0.78f, 0.78f, 0.74f, 1.0f);
        }

        private bool IsPartAlphaBlended(ModelPreviewPart part)
        {
            if (this._Camera == null ||
                this._Camera.TextureMode != ModelPreviewTextureMode.Default ||
                this._Scene == null ||
                this._Scene.Materials == null ||
                part.MaterialIndex < 0 ||
                part.MaterialIndex >= this._Scene.Materials.Count)
            {
                return false;
            }

            var material = this._Scene.Materials[part.MaterialIndex];
            return material != null && material.HasAlpha == true;
        }

        private double GetPartViewDepth(ModelPreviewPart part)
        {
            int partIndex = this.ResolvePartIndex(part);
            if (partIndex < 0 || this._RenderCache.Bounds == null || partIndex >= this._RenderCache.Bounds.Length)
            {
                return 0.0;
            }

            var bounds = this._RenderCache.Bounds[partIndex];
            if (bounds.Valid == false)
            {
                return 0.0;
            }

            double x, y, z, w;
            TransformPoint(this._ModelViewMatrix, bounds.Center, out x, out y, out z, out w);
            return z;
        }

        private double GetVertexViewDepth(uint vertexIndex)
        {
            if (this._RenderCache.Positions == null || vertexIndex >= this._RenderCache.VertexCount)
            {
                return 0.0;
            }

            int offset = (int)vertexIndex * 3;
            double x, y, z, w;
            TransformPoint(
                this._ModelViewMatrix,
                this._RenderCache.Positions[offset + 0],
                this._RenderCache.Positions[offset + 1],
                this._RenderCache.Positions[offset + 2],
                1.0,
                out x,
                out y,
                out z,
                out w);
            return z;
        }

        private void UpdateRenderCache(IList<ModelPreviewPart> parts)
        {
            if (this._Scene == null)
            {
                this._RenderCache.Clear();
                return;
            }

            bool rebuildStatic = this._RenderCache.Scene != this._Scene ||
                                 this._RenderCache.IndexCount != this._Scene.Indices.Count ||
                                 this._RenderCache.PartCount != parts.Count;
            if (rebuildStatic == true)
            {
                this._RenderCache.Clear();
                this._RenderCache.Scene = this._Scene;
                this._RenderCache.IndexCount = this._Scene.Indices.Count;
                this._RenderCache.PartCount = parts.Count;
                this._RenderCache.PartIndices = new uint[parts.Count][];
                for (int partIndex = 0; partIndex < parts.Count; partIndex++)
                {
                    var part = parts[partIndex];
                    int start = Math.Max(0, part.IndexStart);
                    int count = Math.Max(0, Math.Min(part.IndexCount, this._Scene.Indices.Count - start));
                    var indices = new uint[count];
                    for (int i = 0; i < count; i++)
                    {
                        indices[i] = (uint)Math.Max(0, this._Scene.Indices[start + i]);
                    }
                    this._RenderCache.PartIndices[partIndex] = indices;
                }
            }

            int vertexCount = this._Scene.Vertices.Count;
            if (this._RenderCache.Positions == null || this._RenderCache.VertexCount != vertexCount)
            {
                this._RenderCache.Positions = new float[vertexCount * 3];
                this._RenderCache.Normals = new float[vertexCount * 3];
                this._RenderCache.OriginalPositions = new float[vertexCount * 3];
                this._RenderCache.OriginalNormals = new float[vertexCount * 3];
                this._RenderCache.Uvs = new float[vertexCount * 2];
                this._RenderCache.InfluenceColors = new float[vertexCount * 4];
                this._RenderCache.BoneIndices = new float[vertexCount * 4];
                this._RenderCache.BoneWeights = new float[vertexCount * 4];
            }

            this._RenderCache.VertexCount = vertexCount;
            for (int i = 0; i < vertexCount; i++)
            {
                var vertex = this.GetRenderVertex(i);
                var position = ToDisplayPosition(vertex.Position);
                var normal = ToDisplayDirection(vertex.Normal);
                int p = i * 3;
                this._RenderCache.Positions[p + 0] = position.X;
                this._RenderCache.Positions[p + 1] = position.Y;
                this._RenderCache.Positions[p + 2] = position.Z;
                this._RenderCache.Normals[p + 0] = normal.X;
                this._RenderCache.Normals[p + 1] = normal.Y;
                this._RenderCache.Normals[p + 2] = normal.Z;
                this._RenderCache.OriginalPositions[p + 0] = vertex.Position.X;
                this._RenderCache.OriginalPositions[p + 1] = vertex.Position.Y;
                this._RenderCache.OriginalPositions[p + 2] = vertex.Position.Z;
                this._RenderCache.OriginalNormals[p + 0] = vertex.Normal.X;
                this._RenderCache.OriginalNormals[p + 1] = vertex.Normal.Y;
                this._RenderCache.OriginalNormals[p + 2] = vertex.Normal.Z;
                int uv = i * 2;
                this._RenderCache.Uvs[uv + 0] = vertex.U;
                this._RenderCache.Uvs[uv + 1] = vertex.V;
                var influenceColor = this.GetInfluenceColor(vertex);
                int color = i * 4;
                this._RenderCache.InfluenceColors[color + 0] = influenceColor.X;
                this._RenderCache.InfluenceColors[color + 1] = influenceColor.Y;
                this._RenderCache.InfluenceColors[color + 2] = influenceColor.Z;
                this._RenderCache.InfluenceColors[color + 3] = 1.0f;
                int skin = i * 4;
                var bindVertex = this._Scene.BindVertices != null && i < this._Scene.BindVertices.Count ? this._Scene.BindVertices[i] : vertex;
                this._RenderCache.BoneIndices[skin + 0] = bindVertex.Bone0;
                this._RenderCache.BoneIndices[skin + 1] = bindVertex.Bone1;
                this._RenderCache.BoneIndices[skin + 2] = bindVertex.Bone2;
                this._RenderCache.BoneIndices[skin + 3] = bindVertex.Bone3;
                this._RenderCache.BoneWeights[skin + 0] = bindVertex.Weight0;
                this._RenderCache.BoneWeights[skin + 1] = bindVertex.Weight1;
                this._RenderCache.BoneWeights[skin + 2] = bindVertex.Weight2;
                this._RenderCache.BoneWeights[skin + 3] = bindVertex.Weight3;
            }

            if (this._RenderCache.Bounds == null || this._RenderCache.Bounds.Length != parts.Count)
            {
                this._RenderCache.Bounds = new ModelPreviewBounds[parts.Count];
                this._RenderCache.SortedAlphaPartIndices = new uint[parts.Count][];
                this._RenderCache.AlphaSortTriangles = new ModelPreviewAlphaTriangle[parts.Count][];
            }
            for (int i = 0; i < parts.Count; i++)
            {
                this._RenderCache.Bounds[i] = this.CalculatePartBounds(parts[i]);
            }
        }

        private ModelPreviewVertex GetRenderVertex(int index)
        {
            if (this._Scene != null &&
                this._Scene.UseGpuSkinning == true &&
                this._Scene.BindVertices != null &&
                index >= 0 &&
                index < this._Scene.BindVertices.Count &&
                HasVertexWeights(this._Scene.BindVertices[index]) == true)
            {
                return this._Scene.BindVertices[index];
            }

            return this._Scene.Vertices[index];
        }

        private void DrawPartWithClientArrays(OpenGL gl, ModelPreviewPart part, uint[] overrideIndices = null, bool countPart = true)
        {
            if (this._RenderCache.Positions == null || this._RenderCache.Normals == null || this._RenderCache.Uvs == null)
            {
                return;
            }

            int partIndex = this.ResolvePartIndex(part);
            if (partIndex < 0 || this._RenderCache.PartIndices == null || partIndex >= this._RenderCache.PartIndices.Length)
            {
                return;
            }

            var indices = overrideIndices ?? this._RenderCache.PartIndices[partIndex];
            if (indices == null || indices.Length == 0)
            {
                return;
            }

            if (countPart == true)
            {
                this._Profiler.DrawnParts++;
            }
            this._Profiler.DrawnTriangles += indices.Length / 3;
            var gpuSkinning = this.TryBeginGpuSkinning(gl, part);
            gl.EnableClientState(OpenGL.GL_VERTEX_ARRAY);
            gl.EnableClientState(OpenGL.GL_NORMAL_ARRAY);
            gl.EnableClientState(OpenGL.GL_TEXTURE_COORD_ARRAY);
            if (this._Camera.ShowInfluences == true && this._RenderCache.InfluenceColors != null)
            {
                gl.EnableClientState(OpenGL.GL_COLOR_ARRAY);
                gl.ColorPointer(4, OpenGL.GL_FLOAT, 0, this._RenderCache.InfluenceColors);
            }
            gl.VertexPointer(3, 0, gpuSkinning == true ? this._RenderCache.OriginalPositions : this._RenderCache.Positions);
            gl.NormalPointer(OpenGL.GL_FLOAT, 0, gpuSkinning == true ? this._RenderCache.OriginalNormals : this._RenderCache.Normals);
            gl.TexCoordPointer(2, OpenGL.GL_FLOAT, 0, this._RenderCache.Uvs);
            GCHandle boneIndicesHandle = new GCHandle();
            GCHandle boneWeightsHandle = new GCHandle();
            try
            {
                if (gpuSkinning == true)
                {
                    boneIndicesHandle = GCHandle.Alloc(this._GpuPartBoneIndices, GCHandleType.Pinned);
                    boneWeightsHandle = GCHandle.Alloc(this._RenderCache.BoneWeights, GCHandleType.Pinned);
                    gl.EnableVertexAttribArray((uint)this._GpuBoneIndicesLocation);
                    gl.EnableVertexAttribArray((uint)this._GpuBoneWeightsLocation);
                    gl.VertexAttribPointer((uint)this._GpuBoneIndicesLocation, 4, OpenGL.GL_FLOAT, false, 0, boneIndicesHandle.AddrOfPinnedObject());
                    gl.VertexAttribPointer((uint)this._GpuBoneWeightsLocation, 4, OpenGL.GL_FLOAT, false, 0, boneWeightsHandle.AddrOfPinnedObject());
                }

                gl.DrawElements(OpenGL.GL_TRIANGLES, indices.Length, indices);
            }
            finally
            {
                if (gpuSkinning == true)
                {
                    gl.DisableVertexAttribArray((uint)this._GpuBoneIndicesLocation);
                    gl.DisableVertexAttribArray((uint)this._GpuBoneWeightsLocation);
                    this.EndGpuSkinning(gl);
                }
                if (boneIndicesHandle.IsAllocated == true)
                {
                    boneIndicesHandle.Free();
                }
                if (boneWeightsHandle.IsAllocated == true)
                {
                    boneWeightsHandle.Free();
                }
                if (this._Camera.ShowInfluences == true)
                {
                    gl.DisableClientState(OpenGL.GL_COLOR_ARRAY);
                }
            }
        }

        private Vec3 GetInfluenceColor(ModelPreviewVertex vertex)
        {
            var color = new Vec3();
            float total = 0.0f;
            AddInfluenceColor(vertex.Bone0, vertex.Weight0, ref color, ref total);
            AddInfluenceColor(vertex.Bone1, vertex.Weight1, ref color, ref total);
            AddInfluenceColor(vertex.Bone2, vertex.Weight2, ref color, ref total);
            AddInfluenceColor(vertex.Bone3, vertex.Weight3, ref color, ref total);
            if (total <= 0.000001f)
            {
                return new Vec3(0.55f, 0.55f, 0.55f);
            }

            return color * (1.0f / total);
        }

        private static void AddInfluenceColor(int boneIndex, float weight, ref Vec3 color, ref float total)
        {
            if (boneIndex < 0 || weight <= 0.0001f)
            {
                return;
            }

            var boneColor = GetBoneDebugColor(boneIndex);
            color += boneColor * weight;
            total += weight;
        }

        private static Vec3 GetBoneDebugColor(int boneIndex)
        {
            unchecked
            {
                uint hash = (uint)(boneIndex + 1) * 2654435761u;
                hash ^= hash >> 16;
                var hue = (hash & 0xFFFF) / 65535.0f;
                var saturation = 0.72f + (((hash >> 16) & 0xFF) / 255.0f) * 0.25f;
                var value = 0.85f + (((hash >> 24) & 0x7F) / 127.0f) * 0.15f;
                return HsvToInfluenceRgb(hue, saturation, value);
            }
        }

        private static Vec3 HsvToInfluenceRgb(float hue, float saturation, float value)
        {
            var h = (hue - (float)Math.Floor(hue)) * 6.0f;
            var sector = (int)Math.Floor(h);
            var f = h - sector;
            var p = value * (1.0f - saturation);
            var q = value * (1.0f - saturation * f);
            var t = value * (1.0f - saturation * (1.0f - f));

            switch (sector % 6)
            {
                case 0: return new Vec3(value, t, p);
                case 1: return new Vec3(q, value, p);
                case 2: return new Vec3(p, value, t);
                case 3: return new Vec3(p, q, value);
                case 4: return new Vec3(t, p, value);
                default: return new Vec3(value, p, q);
            }
        }

        private void DisableClientArrays(OpenGL gl)
        {
            gl.DisableClientState(OpenGL.GL_VERTEX_ARRAY);
            gl.DisableClientState(OpenGL.GL_NORMAL_ARRAY);
            gl.DisableClientState(OpenGL.GL_TEXTURE_COORD_ARRAY);
            gl.DisableClientState(OpenGL.GL_COLOR_ARRAY);
        }

        private int ResolvePartIndex(ModelPreviewPart part)
        {
            if (this._Scene == null || this._Scene.Parts == null)
            {
                return 0;
            }

            for (int i = 0; i < this._Scene.Parts.Count; i++)
            {
                if (this._Scene.Parts[i].IndexStart == part.IndexStart &&
                    this._Scene.Parts[i].IndexCount == part.IndexCount &&
                    this._Scene.Parts[i].VertexStart == part.VertexStart &&
                    this._Scene.Parts[i].VertexCount == part.VertexCount)
                {
                    return i;
                }
            }

            return this._Scene.Parts.Count == 0 ? 0 : -1;
        }

        private bool TryBeginGpuSkinning(OpenGL gl, ModelPreviewPart part)
        {
            if (this._Scene == null ||
                this._Scene.UseGpuSkinning == false ||
                this._GpuSkinningUnavailable == true ||
                this._Scene.BindBones == null ||
                this._Scene.Bones == null ||
                this._Scene.BindBones.Count == 0 ||
                this._Scene.Bones.Count == 0 ||
                this._RenderCache.BoneIndices == null ||
                this._RenderCache.BoneWeights == null ||
                PartHasGpuSkinnableWeights(part) == false)
            {
                return false;
            }

            var palette = this.BuildBonePalette(part);
            if (palette == null)
            {
                if (string.IsNullOrEmpty(this._GpuSkinningStatus) == true)
                {
                    this.SetGpuSkinningStatus(part, "palette build failed");
                }
                return false;
            }

            if (this.EnsureGpuSkinningProgram(gl) == false)
            {
                this.SetGpuSkinningStatus(part, "shader unavailable");
                this.DisableGpuSkinningFallback();
                return false;
            }

            this._GpuSkinningProgram.Bind(gl);
            gl.UniformMatrix4(this._GpuSkinningProgram.GetUniformLocation(gl, "uBones[0]"), palette.Length / 16, false, palette);
            gl.Uniform1(this._GpuSkinningProgram.GetUniformLocation(gl, "uLighting"), this._Scene.Unlit == false && this._Camera.LightingEnabled == true ? 1 : 0);
            gl.Uniform1(this._GpuSkinningProgram.GetUniformLocation(gl, "uUseTexture"), this._CurrentPartUsesTexture == true ? 1 : 0);
            gl.Uniform1(this._GpuSkinningProgram.GetUniformLocation(gl, "uAlphaCutoff"), this._CurrentPartAlphaCutoff);
            gl.Uniform1(this._GpuSkinningProgram.GetUniformLocation(gl, "uTexture"), 0);
            return true;
        }

        private void SetGpuSkinningStatus(ModelPreviewPart part, string reason)
        {
            var name = string.IsNullOrEmpty(part.Name) == true ? part.ObjectName : part.Name;
            this._GpuSkinningStatus = "GPU skin skip: " + (string.IsNullOrEmpty(name) ? "part" : name) + " (" + reason + ")";
        }

        private void DisableGpuSkinningFallback()
        {
            if (this._GpuSkinningUnavailable == true && this._Scene != null)
            {
                this._Scene.UseGpuSkinning = false;
            }
        }

        private void EndGpuSkinning(OpenGL gl)
        {
            if (this._GpuSkinningProgram != null)
            {
                this._GpuSkinningProgram.Unbind(gl);
            }
        }

        private bool EnsureGpuSkinningProgram(OpenGL gl)
        {
            if (this._GpuSkinningProgram != null)
            {
                return true;
            }

            try
            {
                this._GpuSkinningProgram = new ShaderProgram();
                this._GpuSkinningProgram.Create(gl, GpuSkinningVertexShader, GpuSkinningFragmentShader, new Dictionary<uint, string>
                {
                    { GpuBoneIndicesAttribute, "aBoneIndices" },
                    { GpuBoneWeightsAttribute, "aBoneWeights" },
                });
                this._GpuBoneIndicesLocation = this._GpuSkinningProgram.GetAttributeLocation(gl, "aBoneIndices");
                this._GpuBoneWeightsLocation = this._GpuSkinningProgram.GetAttributeLocation(gl, "aBoneWeights");
                if (this._GpuBoneIndicesLocation < 0 || this._GpuBoneWeightsLocation < 0)
                {
                    this._GpuSkinningUnavailable = true;
                    return false;
                }

                return true;
            }
            catch
            {
                this._GpuSkinningUnavailable = true;
                this._GpuSkinningProgram = null;
                return false;
            }
        }

        private ModelPreviewSceneInstance ResolveGpuSkinningInstance(ModelPreviewPart part)
        {
            if (this._Scene == null ||
                this._Scene.BindBones == null ||
                this._Scene.BindBones.Count == 0 ||
                this._Scene.BindVertices == null ||
                part.VertexStart < 0 ||
                part.VertexStart >= this._Scene.BindVertices.Count)
            {
                return null;
            }

            if (this._Scene.Instances != null && this._Scene.Instances.Count > 0)
            {
                int partIndex = this.ResolvePartIndex(part);
                if (partIndex >= 0)
                {
                    var byPart = this._Scene.Instances.FirstOrDefault(i =>
                        i != null &&
                        i.PartCount > 0 &&
                        partIndex >= i.PartStart &&
                        partIndex < i.PartStart + i.PartCount);
                    if (byPart != null)
                    {
                        return byPart;
                    }
                }

                int partStart = Math.Max(0, part.VertexStart);
                int partEnd = Math.Min(this._Scene.BindVertices.Count, partStart + Math.Max(0, part.VertexCount));
                return this._Scene.Instances.FirstOrDefault(i =>
                    i != null &&
                    i.VertexCount > 0 &&
                    partStart >= i.VertexStart &&
                    partEnd <= i.VertexStart + i.VertexCount);
            }

            return new ModelPreviewSceneInstance
            {
                Name = this._Scene.Name,
                VertexStart = 0,
                VertexCount = this._Scene.BindVertices.Count,
                PartStart = 0,
                PartCount = this._Scene.Parts == null ? 0 : this._Scene.Parts.Count,
                BoneStart = 0,
                BoneCount = this._Scene.BindBones.Count,
            };
        }

        private bool PartHasGpuSkinnableWeights(ModelPreviewPart part)
        {
            if (this._Scene == null || this._Scene.BindVertices == null)
            {
                return false;
            }

            int start = Math.Max(0, part.VertexStart);
            int end = Math.Min(this._Scene.BindVertices.Count, start + Math.Max(0, part.VertexCount));
            bool hasWeights = false;
            for (int i = start; i < end; i++)
            {
                if (HasVertexWeights(this._Scene.BindVertices[i]) == true)
                {
                    hasWeights = true;
                }
            }

            return hasWeights;
        }

        private float[] BuildBonePalette(ModelPreviewPart part)
        {
            if (this._Scene == null ||
                this._Scene.BindBones == null ||
                this._Scene.Bones == null ||
                this._RenderCache.BoneIndices == null)
            {
                return null;
            }

            int partIndex = this.ResolvePartIndex(part);
            if (partIndex < 0 ||
                this._RenderCache.PartIndices == null ||
                partIndex >= this._RenderCache.PartIndices.Length ||
                this._RenderCache.PartIndices[partIndex] == null)
            {
                return null;
            }

            int vertexCount = this._Scene.BindVertices == null ? 0 : this._Scene.BindVertices.Count;
            if (vertexCount <= 0)
            {
                return null;
            }

            int skinCount = vertexCount * 4;
            if (this._GpuPartBoneIndices == null || this._GpuPartBoneIndices.Length != skinCount)
            {
                this._GpuPartBoneIndices = new float[skinCount];
            }

            var remap = new Dictionary<GpuBonePaletteKey, int>();
            var globalBones = new List<GpuBonePaletteKey>();
            var indices = this._RenderCache.PartIndices[partIndex];
            foreach (var rawIndex in indices)
            {
                if (rawIndex >= vertexCount)
                {
                    continue;
                }

                int vertexIndex = (int)rawIndex;
                int skin = vertexIndex * 4;
                bool rigidBoneOrigin = this._Scene.BindVertices[vertexIndex].UseRigidBoneOrigin == true;
                this.RemapGpuBone(skin + 0, remap, globalBones, rigidBoneOrigin);
                this.RemapGpuBone(skin + 1, remap, globalBones, false);
                this.RemapGpuBone(skin + 2, remap, globalBones, false);
                this.RemapGpuBone(skin + 3, remap, globalBones, false);
            }

            if (globalBones.Count <= 0)
            {
                return null;
            }
            if (globalBones.Count > MaxGpuSkinningBones)
            {
                this.SetGpuSkinningStatus(part, "part palette too large");
                return null;
            }

            var palette = new float[globalBones.Count * 16];
            for (int i = 0; i < globalBones.Count; i++)
            {
                var key = globalBones[i];
                int boneIndex = key.BoneIndex;
                var bind = this._Scene.BindBones[boneIndex];
                var pose = this._Scene.Bones[boneIndex];
                if (key.UseRigidBoneOrigin == true)
                {
                    WriteBoneMatrix(palette, i * 16, pose);
                }
                else
                {
                    WriteSkinMatrix(palette, i * 16, bind, pose);
                }
            }
            return palette;
        }

        private void RemapGpuBone(int skinIndex, Dictionary<GpuBonePaletteKey, int> remap, List<GpuBonePaletteKey> globalBones, bool useRigidBoneOrigin)
        {
            int boneIndex = (int)this._RenderCache.BoneIndices[skinIndex];
            if (boneIndex < 0 ||
                boneIndex >= this._Scene.BindBones.Count ||
                boneIndex >= this._Scene.Bones.Count ||
                this._RenderCache.BoneWeights[skinIndex] <= 0.0001f)
            {
                this._GpuPartBoneIndices[skinIndex] = 0.0f;
                return;
            }

            var key = new GpuBonePaletteKey(boneIndex, useRigidBoneOrigin);
            int localIndex;
            if (remap.TryGetValue(key, out localIndex) == false)
            {
                localIndex = globalBones.Count;
                remap.Add(key, localIndex);
                globalBones.Add(key);
            }

            this._GpuPartBoneIndices[skinIndex] = localIndex;
        }

        private bool PartHasWeights(ModelPreviewPart part)
        {
            if (this._Scene == null || this._Scene.BindVertices == null)
            {
                return false;
            }

            int start = Math.Max(0, part.VertexStart);
            int end = Math.Min(this._Scene.BindVertices.Count, start + Math.Max(0, part.VertexCount));
            for (int i = start; i < end; i++)
            {
                if (HasVertexWeights(this._Scene.BindVertices[i]) == true)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasVertexWeights(ModelPreviewVertex vertex)
        {
            return vertex.Weight0 > 0.0001f ||
                   vertex.Weight1 > 0.0001f ||
                   vertex.Weight2 > 0.0001f ||
                   vertex.Weight3 > 0.0001f;
        }

        private static void WriteSkinMatrix(float[] values, int offset, ModelPreviewBone bind, ModelPreviewBone pose)
        {
            float r00 = pose.AxisX.X * bind.AxisX.X + pose.AxisY.X * bind.AxisY.X + pose.AxisZ.X * bind.AxisZ.X;
            float r01 = pose.AxisX.X * bind.AxisX.Y + pose.AxisY.X * bind.AxisY.Y + pose.AxisZ.X * bind.AxisZ.Y;
            float r02 = pose.AxisX.X * bind.AxisX.Z + pose.AxisY.X * bind.AxisY.Z + pose.AxisZ.X * bind.AxisZ.Z;
            float r10 = pose.AxisX.Y * bind.AxisX.X + pose.AxisY.Y * bind.AxisY.X + pose.AxisZ.Y * bind.AxisZ.X;
            float r11 = pose.AxisX.Y * bind.AxisX.Y + pose.AxisY.Y * bind.AxisY.Y + pose.AxisZ.Y * bind.AxisZ.Y;
            float r12 = pose.AxisX.Y * bind.AxisX.Z + pose.AxisY.Y * bind.AxisY.Z + pose.AxisZ.Y * bind.AxisZ.Z;
            float r20 = pose.AxisX.Z * bind.AxisX.X + pose.AxisY.Z * bind.AxisY.X + pose.AxisZ.Z * bind.AxisZ.X;
            float r21 = pose.AxisX.Z * bind.AxisX.Y + pose.AxisY.Z * bind.AxisY.Y + pose.AxisZ.Z * bind.AxisZ.Y;
            float r22 = pose.AxisX.Z * bind.AxisX.Z + pose.AxisY.Z * bind.AxisY.Z + pose.AxisZ.Z * bind.AxisZ.Z;
            float tx = pose.Position.X - (r00 * bind.Position.X + r01 * bind.Position.Y + r02 * bind.Position.Z);
            float ty = pose.Position.Y - (r10 * bind.Position.X + r11 * bind.Position.Y + r12 * bind.Position.Z);
            float tz = pose.Position.Z - (r20 * bind.Position.X + r21 * bind.Position.Y + r22 * bind.Position.Z);

            values[offset + 0] = r00;
            values[offset + 1] = r10;
            values[offset + 2] = r20;
            values[offset + 3] = 0.0f;
            values[offset + 4] = r01;
            values[offset + 5] = r11;
            values[offset + 6] = r21;
            values[offset + 7] = 0.0f;
            values[offset + 8] = r02;
            values[offset + 9] = r12;
            values[offset + 10] = r22;
            values[offset + 11] = 0.0f;
            values[offset + 12] = tx;
            values[offset + 13] = ty;
            values[offset + 14] = tz;
            values[offset + 15] = 1.0f;
        }

        private static void WriteBoneMatrix(float[] values, int offset, ModelPreviewBone bone)
        {
            values[offset + 0] = bone.AxisX.X;
            values[offset + 1] = bone.AxisX.Y;
            values[offset + 2] = bone.AxisX.Z;
            values[offset + 3] = 0.0f;
            values[offset + 4] = bone.AxisY.X;
            values[offset + 5] = bone.AxisY.Y;
            values[offset + 6] = bone.AxisY.Z;
            values[offset + 7] = 0.0f;
            values[offset + 8] = bone.AxisZ.X;
            values[offset + 9] = bone.AxisZ.Y;
            values[offset + 10] = bone.AxisZ.Z;
            values[offset + 11] = 0.0f;
            values[offset + 12] = bone.Position.X;
            values[offset + 13] = bone.Position.Y;
            values[offset + 14] = bone.Position.Z;
            values[offset + 15] = 1.0f;
        }

        private ModelPreviewBounds CalculatePartBounds(ModelPreviewPart part)
        {
            var bounds = new ModelPreviewBounds();
            if (this._Scene == null || this._Scene.Vertices == null || this._Scene.Vertices.Count == 0)
            {
                return bounds;
            }

            int partIndex = this.ResolvePartIndex(part);
            uint[] indices = partIndex >= 0 && this._RenderCache.PartIndices != null && partIndex < this._RenderCache.PartIndices.Length
                                 ? this._RenderCache.PartIndices[partIndex]
                                 : null;
            if (indices != null && indices.Length > 0)
            {
                Vec3 sum = new Vec3();
                int count = 0;
                foreach (var index in indices)
                {
                    if (index >= this._Scene.Vertices.Count)
                    {
                        continue;
                    }
                    sum += this._Scene.Vertices[(int)index].Position;
                    count++;
                }
                if (count == 0)
                {
                    return bounds;
                }

                bounds.Center = sum / count;
                bounds.Valid = true;
                foreach (var index in indices)
                {
                    if (index >= this._Scene.Vertices.Count)
                    {
                        continue;
                    }
                    var delta = this._Scene.Vertices[(int)index].Position - bounds.Center;
                    bounds.Radius = Math.Max(bounds.Radius, (float)Math.Sqrt(LengthSquared(delta)));
                }
                return bounds;
            }

            int start = Math.Max(0, part.VertexStart);
            int end = Math.Min(this._Scene.Vertices.Count, start + Math.Max(0, part.VertexCount));
            if (end <= start)
            {
                return bounds;
            }

            Vec3 vertexSum = new Vec3();
            for (int i = start; i < end; i++)
            {
                vertexSum += this._Scene.Vertices[i].Position;
            }

            bounds.Center = vertexSum / (end - start);
            bounds.Valid = true;
            for (int i = start; i < end; i++)
            {
                var delta = this._Scene.Vertices[i].Position - bounds.Center;
                bounds.Radius = Math.Max(bounds.Radius, (float)Math.Sqrt(LengthSquared(delta)));
            }
            return bounds;
        }

        private bool IsPartVisible(ModelPreviewPart part, int width, int height)
        {
            if (this._Camera == null || this._Scene == null || this._Camera.UseCutsceneCamera == false)
            {
                return true;
            }

            if (this._Scene.UseGpuSkinning == true && this.PartHasWeights(part) == true)
            {
                return true;
            }

            int partIndex = this.ResolvePartIndex(part);
            if (partIndex < 0 || this._RenderCache.Bounds == null || partIndex >= this._RenderCache.Bounds.Length)
            {
                return true;
            }

            var bounds = this._RenderCache.Bounds[partIndex];
            if (bounds.Valid == false)
            {
                return true;
            }

            var look = Normalize(this._Camera.CutsceneLook);
            if (LengthSquared(look) <= 0.000001f)
            {
                return true;
            }

            var up = LengthSquared(this._Camera.CutsceneUp) > 0.000001f
                         ? Normalize(this._Camera.CutsceneUp)
                         : new Vec3(0.0f, 1.0f, 0.0f);
            var right = Normalize(Cross(look, up));
            if (LengthSquared(right) <= 0.000001f)
            {
                return true;
            }
            up = Normalize(Cross(right, look));

            var toCenter = bounds.Center - this._Camera.CutscenePosition;
            float depth = Dot(toCenter, look);
            float near = this._Camera.CutsceneNearClip > 0.000001f ? this._Camera.CutsceneNearClip : 0.01f;
            float far = this._Camera.CutsceneFarClip > near ? this._Camera.CutsceneFarClip : 100000.0f;
            if (depth + bounds.Radius < near || depth - bounds.Radius > far)
            {
                return false;
            }

            if (depth <= 0.0001f)
            {
                return bounds.Radius > -depth;
            }

            float aspect = Math.Max(0.001f, (float)width / Math.Max(1, height));
            float verticalFov = (float)ConvertHorizontalFovToVertical(
                this._Camera.CutsceneFov > 0.001f ? this._Camera.CutsceneFov : 45.0f,
                aspect);
            float halfVertical = (float)Math.Tan((verticalFov * Math.PI / 180.0) * 0.5) * depth;
            float halfHorizontal = halfVertical * aspect;
            float horizontalDistance = Math.Abs(Dot(toCenter, right));
            float verticalDistance = Math.Abs(Dot(toCenter, up));
            return horizontalDistance <= halfHorizontal + bounds.Radius &&
                   verticalDistance <= halfVertical + bounds.Radius;
        }

        private void ApplyMaterialLighting(OpenGL gl, int materialIndex)
        {
            var material = this._Scene.Materials != null && materialIndex >= 0 && materialIndex < this._Scene.Materials.Count
                               ? this._Scene.Materials[materialIndex]
                               : null;
            float specularMultiply = material == null ? 0.0f : Math.Max(0.0f, material.SpecularMultiply);
            float shininess = material == null ? 8.0f : Math.Max(1.0f, Math.Min(128.0f, material.SpecularPower));
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_SPECULAR, new[]
            {
                specularMultiply,
                specularMultiply,
                specularMultiply,
                1.0f,
            });
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_SHININESS, shininess);
        }

        private void DrawVertex(OpenGL gl, int index)
        {
            if (index < 0 || index >= this._Scene.Vertices.Count)
            {
                return;
            }

            var vertex = this._Scene.Vertices[index];
            var normal = ToDisplayDirection(vertex.Normal);
            var position = ToDisplayPosition(vertex.Position);
            gl.Normal(normal.X, normal.Y, normal.Z);
            gl.TexCoord(vertex.U, vertex.V);
            gl.Vertex(position.X, position.Y, position.Z);
        }

        private void DrawSkeleton(OpenGL gl)
        {
            if (this._Scene.Bones == null || this._Scene.Bones.Count == 0)
            {
                return;
            }

            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.LineWidth(3.0f);
            gl.Color(1.0f, 1.0f, 1.0f, 1.0f);
            gl.Begin(OpenGL.GL_LINES);
            for (int i = 0; i < this._Scene.Bones.Count; i++)
            {
                var bone = this._Scene.Bones[i];
                if (bone.Parent < 0 || bone.Parent >= this._Scene.Bones.Count)
                {
                    continue;
                }

                var parent = ToDisplayPosition(this._Scene.Bones[bone.Parent].Position);
                var position = ToDisplayPosition(bone.Position);
                if (this._Camera.ShowInfluences == true)
                {
                    var color = GetBoneDebugColor(i);
                    gl.Color(color.X, color.Y, color.Z, 1.0f);
                }
                gl.Vertex(parent.X, parent.Y, parent.Z);
                gl.Vertex(position.X, position.Y, position.Z);
            }
            gl.End();

            float axisLength = Math.Max(0.001f, this.GetCameraControlRadius()) * 0.035f;
            gl.LineWidth(2.0f);
            gl.Begin(OpenGL.GL_LINES);
            foreach (var bone in this._Scene.Bones)
            {
                DrawAxis(gl, bone.Position, bone.AxisX, axisLength, 1.0f, 0.0f, 0.0f);
                DrawAxis(gl, bone.Position, bone.AxisY, axisLength, 0.0f, 1.0f, 0.0f);
                DrawAxis(gl, bone.Position, bone.AxisZ, axisLength, 0.0f, 0.45f, 1.0f);
            }
            gl.End();

            gl.PointSize(16.0f);
            gl.Begin(OpenGL.GL_POINTS);
            for (int i = 0; i < this._Scene.Bones.Count; i++)
            {
                if (this._Camera.ShowInfluences == true)
                {
                    var color = GetBoneDebugColor(i);
                    float hoverBoost = i == this._HoveredBone ? 1.25f : 1.0f;
                    gl.Color(
                        Math.Min(1.0f, color.X * hoverBoost),
                        Math.Min(1.0f, color.Y * hoverBoost),
                        Math.Min(1.0f, color.Z * hoverBoost),
                        1.0f);
                }
                else
                {
                    gl.Color(i == this._HoveredBone ? 1.0f : 1.0f, i == this._HoveredBone ? 0.9f : 1.0f, i == this._HoveredBone ? 0.15f : 1.0f, 1.0f);
                }
                var p = ToDisplayPosition(this._Scene.Bones[i].Position);
                gl.Vertex(p.X, p.Y, p.Z);
            }
            gl.End();
        }

        private void DrawLocators(OpenGL gl)
        {
            if (this._Scene.Locators == null || this._Scene.Locators.Count == 0)
            {
                return;
            }

            float markerSize = Math.Max(0.02f, this.GetCameraControlRadius() * 0.015f);
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.LineWidth(2.0f);
            gl.Color(0.15f, 0.85f, 1.0f, 1.0f);
            gl.Begin(OpenGL.GL_LINES);
            foreach (var locator in this._Scene.Locators)
            {
                var p = ToDisplayPosition(locator.Position);
                gl.Vertex(p.X - markerSize, p.Y, p.Z);
                gl.Vertex(p.X + markerSize, p.Y, p.Z);
                gl.Vertex(p.X, p.Y - markerSize, p.Z);
                gl.Vertex(p.X, p.Y + markerSize, p.Z);
                gl.Vertex(p.X, p.Y, p.Z - markerSize);
                gl.Vertex(p.X, p.Y, p.Z + markerSize);
            }
            gl.End();

            gl.PointSize(10.0f);
            gl.Begin(OpenGL.GL_POINTS);
            foreach (var locator in this._Scene.Locators)
            {
                var p = ToDisplayPosition(locator.Position);
                gl.Vertex(p.X, p.Y, p.Z);
            }
            gl.End();
        }

        private static void DrawAxis(OpenGL gl, Vec3 start, Vec3 axis, float length, float red, float green, float blue)
        {
            start = ToDisplayPosition(start);
            axis = ToDisplayDirection(axis);
            axis = LengthSquared(axis) > 0.0001f ? Normalize(axis) : new Vec3();
            var end = start + axis * length;
            gl.Color(red, green, blue, 1.0f);
            gl.Vertex(start.X, start.Y, start.Z);
            gl.Vertex(end.X, end.Y, end.Z);
        }

        private void DrawCutsceneCameraFrustum(OpenGL gl, int width, int height)
        {
            var look = LengthSquared(this._Camera.CutsceneLook) > 0.000001f
                           ? Normalize(this._Camera.CutsceneLook)
                           : new Vec3(0.0f, 0.0f, 1.0f);
            var up = LengthSquared(this._Camera.CutsceneUp) > 0.000001f
                         ? Normalize(this._Camera.CutsceneUp)
                         : new Vec3(0.0f, 1.0f, 0.0f);
            var right = Cross(look, up);
            if (LengthSquared(right) <= 0.000001f)
            {
                right = new Vec3(1.0f, 0.0f, 0.0f);
            }
            right = Normalize(right);
            up = Normalize(Cross(right, look));

            float radius = Math.Max(1.0f, this.GetCameraControlRadius());
            float distance = Math.Max(radius * 0.25f, 1.0f);
            float aspect = Math.Max(0.001f, (float)width / Math.Max(1, height));
            float fovDegrees = this._Camera.CutsceneFov > 0.001f
                                   ? (float)ConvertHorizontalFovToVertical(this._Camera.CutsceneFov, aspect)
                                   : 45.0f;
            float halfHeight = (float)Math.Tan((fovDegrees * Math.PI / 180.0) * 0.5) * distance;
            float halfWidth = halfHeight * aspect;

            var eye = this._Camera.CutscenePosition;
            var center = eye + look * distance;
            var topLeft = center + up * halfHeight - right * halfWidth;
            var topRight = center + up * halfHeight + right * halfWidth;
            var bottomLeft = center - up * halfHeight - right * halfWidth;
            var bottomRight = center - up * halfHeight + right * halfWidth;

            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.Disable(OpenGL.GL_BLEND);
            gl.LineWidth(2.0f);
            gl.Color(1.0f, 0.0f, 1.0f, 1.0f);
            gl.Begin(OpenGL.GL_LINES);
            DrawLine(gl, eye, topLeft);
            DrawLine(gl, eye, topRight);
            DrawLine(gl, eye, bottomLeft);
            DrawLine(gl, eye, bottomRight);
            DrawLine(gl, topLeft, topRight);
            DrawLine(gl, topRight, bottomRight);
            DrawLine(gl, bottomRight, bottomLeft);
            DrawLine(gl, bottomLeft, topLeft);
            DrawLine(gl, eye, center);
            gl.End();
        }

        private static void DrawLine(OpenGL gl, Vec3 a, Vec3 b)
        {
            a = ToDisplayPosition(a);
            b = ToDisplayPosition(b);
            gl.Vertex(a.X, a.Y, a.Z);
            gl.Vertex(b.X, b.Y, b.Z);
        }

        private void DrawLightGizmos(OpenGL gl)
        {
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.Enable(OpenGL.GL_BLEND);
            gl.LineWidth(1.5f);

            foreach (var light in this._Scene.Lights)
            {
                if (light == null)
                {
                    continue;
                }

                var color = light.Color;
                if (LengthSquared(color) <= 0.000001f)
                {
                    color = new Vec3(1.0f, 1.0f, 1.0f);
                }

                float intensity = Math.Max(0.15f, Math.Min(1.0f, light.Intensity));
                gl.Color(color.X, color.Y, color.Z, 0.85f);
                var position = ToDisplayPosition(light.Position);
                float marker = Math.Max(0.025f, this.GetCameraControlRadius() * 0.012f);
                gl.Begin(OpenGL.GL_LINES);
                gl.Vertex(position.X - marker, position.Y, position.Z);
                gl.Vertex(position.X + marker, position.Y, position.Z);
                gl.Vertex(position.X, position.Y - marker, position.Z);
                gl.Vertex(position.X, position.Y + marker, position.Z);
                gl.Vertex(position.X, position.Y, position.Z - marker);
                gl.Vertex(position.X, position.Y, position.Z + marker);
                gl.End();

                float radius = Math.Max(0.001f, light.Range);
                gl.Color(color.X * intensity, color.Y * intensity, color.Z * intensity, 0.28f);
                this.DrawLightRadiusCircle(gl, light.Position, radius, 0);
                this.DrawLightRadiusCircle(gl, light.Position, radius, 1);
                this.DrawLightRadiusCircle(gl, light.Position, radius, 2);
            }
        }

        private void DrawLightRadiusCircle(OpenGL gl, Vec3 center, float radius, int plane)
        {
            const int segments = 64;
            var displayCenter = ToDisplayPosition(center);
            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int i = 0; i < segments; i++)
            {
                float angle = (float)(i * Math.PI * 2.0 / segments);
                float c = (float)Math.Cos(angle) * radius;
                float s = (float)Math.Sin(angle) * radius;
                if (plane == 0)
                {
                    gl.Vertex(displayCenter.X + c, displayCenter.Y, displayCenter.Z + s);
                }
                else if (plane == 1)
                {
                    gl.Vertex(displayCenter.X + c, displayCenter.Y + s, displayCenter.Z);
                }
                else
                {
                    gl.Vertex(displayCenter.X, displayCenter.Y + c, displayCenter.Z + s);
                }
            }
            gl.End();
        }

        private void DrawOverlay(OpenGL gl, int width, int height)
        {
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);

            this.DrawCutsceneFade(gl, width, height);
            this.DrawCutsceneTimeline(gl, width, height);
            this.DrawVertexMorphScrubber(gl, width, height);

            var lines = this.BuildControlOverlayLines();
            int y = height - 20;
            if (string.IsNullOrEmpty(this.MeshStatus) == false)
            {
                gl.DrawText(12, y, 0.9f, 0.95f, 1.0f, "Consolas", 10.0f, this.MeshStatus);
                y -= 14;
            }

            if (string.IsNullOrEmpty(this.CutsceneStatus) == false)
            {
                int textWidth = this.CutsceneStatus.Length * 8;
                gl.DrawText(Math.Max(12, (width - textWidth) / 2), height - 20, 1.0f, 1.0f, 1.0f, "Consolas", 11.0f, this.CutsceneStatus);
            }

            if (string.IsNullOrEmpty(this.CutsceneLightGroupStatus) == false)
            {
                gl.DrawText(12, 50, 1.0f, 0.82f, 0.35f, "Consolas", 10.0f, this.CutsceneLightGroupStatus);
            }

            foreach (var line in lines)
            {
                gl.DrawText(12, y, 1.0f, 1.0f, 1.0f, "Consolas", 10.0f, line);
                y -= 14;
            }

            if (string.IsNullOrEmpty(this.AnimationStatus) == false)
            {
                gl.DrawText(12, 34, 1.0f, 1.0f, 1.0f, "Consolas", 10.0f, this.AnimationStatus);
            }

            if (string.IsNullOrEmpty(this.RootMotionStatus) == false)
            {
                gl.DrawText(12, 18, 1.0f, 0.85f, 0.35f, "Consolas", 10.0f, this.RootMotionStatus);
            }

            if (this._Scene != null && this._Scene.UseGpuSkinning == false)
            {
                gl.DrawText(12, 66, 1.0f, 0.62f, 0.35f, "Consolas", 10.0f, "Skinning: CPU");
            }

            if (this._Camera.ShowMaterialPreview == true && this._Scene.Materials != null && this._Scene.Materials.Count > 0)
            {
                var materialIndices = this._Scene.Parts
                                          .Where(p => p.Hidden == false && p.MaterialIndex >= 0 && p.MaterialIndex < this._Scene.Materials.Count)
                                          .Select(p => p.MaterialIndex)
                                          .Distinct()
                                          .OrderBy(i => i)
                                          .ToList();
                int legendY = (height / 2) + (materialIndices.Count * 7);
                foreach (var materialIndex in materialIndices)
                {
                    var material = this._Scene.Materials[materialIndex];
                    var name = material == null || string.IsNullOrEmpty(material.ShaderName) == true
                                   ? "material " + materialIndex.ToString(System.Globalization.CultureInfo.InvariantCulture)
                                   : material.ShaderName;
                    var colorName = name;
                    var templateName = material == null || string.IsNullOrEmpty(material.ShaderTemplateName) == true
                                           ? "Unknown"
                                           : material.ShaderTemplateName;
                    name = name + " (" + templateName + ")";
                    if (material != null)
                    {
                        name += string.Format(
                            System.Globalization.CultureInfo.InvariantCulture,
                            " | D:{0} N:{1} S:{2} spec:{3:0.###},{4:0.###}",
                            string.IsNullOrEmpty(material.TextureName) ? "none" : material.TextureName,
                            string.IsNullOrEmpty(material.NormalTextureName) ? "none" : material.NormalTextureName,
                            string.IsNullOrEmpty(material.SpecularTextureName) ? "none" : material.SpecularTextureName,
                            material.HasSpecParams ? material.SpecularMultiply : 0.0f,
                            material.HasSpecParams ? material.SpecularPower : 0.0f);
                    }
                    var color = GetMaterialDebugColor(materialIndex, colorName);
                    gl.DrawText(12, legendY, color.R, color.G, color.B, "Consolas", 10.0f, name);
                    legendY -= 14;
                }
            }

            if (this._Scene.Bones != null && this._Scene.Bones.Count > 0 &&
                (this._Camera.ShowSkeleton == true || this._HoveredBone >= 0))
            {
                var labels = new List<Tuple<string, PointF, int>>();
                if (this._Camera.ShowBoneNames == true)
                {
                    for (int i = 0; i < this._Scene.Bones.Count; i++)
                    {
                        var bone = this._Scene.Bones[i];
                        PointF point;
                        if (this.TryProjectPoint(bone.Position, out point) == true)
                        {
                            labels.Add(Tuple.Create(bone.Name, point, i));
                        }
                    }
                }
                else if (this._HoveredBone >= 0 && this._HoveredBone < this._Scene.Bones.Count)
                {
                    var bone = this._Scene.Bones[this._HoveredBone];
                    PointF point;
                    if (this.TryProjectPoint(bone.Position, out point) == true)
                    {
                        labels.Add(Tuple.Create(bone.Name, point, this._HoveredBone));
                    }
                }

                foreach (var label in labels)
                {
                    float red = 1.0f;
                    float green = 1.0f;
                    float blue = 1.0f;
                    if (this._Camera.ShowInfluences == true)
                    {
                        var color = GetBoneDebugColor(label.Item3);
                        red = color.X;
                        green = color.Y;
                        blue = color.Z;
                    }

                    gl.DrawText((int)label.Item2.X + 10, height - (int)label.Item2.Y + 10, red, green, blue, "Consolas", 10.0f, label.Item1);
                }
            }

            if (this._Camera.ShowLocators == true && this._Scene.Locators != null && this._Scene.Locators.Count > 0)
            {
                foreach (var locator in this._Scene.Locators)
                {
                    PointF point;
                    if (this.TryProjectPoint(locator.Position, out point) == false)
                    {
                        continue;
                    }

                    var name = string.IsNullOrEmpty(locator.Name) == true ? "Locator" : locator.Name;
                    var text = string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "{0} ({1:0.###}, {2:0.###}, {3:0.###})",
                        name,
                        locator.Position.X,
                        locator.Position.Y,
                        locator.Position.Z);
                    gl.DrawText((int)point.X + 10, height - (int)point.Y + 10, 0.15f, 0.85f, 1.0f, "Consolas", 10.0f, text);
                }
            }

            this.DrawProfilerOverlay(gl, width, height);
        }

        private void DrawProfilerOverlay(OpenGL gl, int width, int height)
        {
            if (this._Profiler.FrameCount < 2)
            {
                return;
            }

            int x = Math.Max(12, width - 330);
            int y = height - 20;
            foreach (var line in this._Profiler.GetOverlayLines())
            {
                gl.DrawText(x, y, 1.0f, 0.42f, 0.28f, "Consolas", 9.0f, line);
                y -= 13;
            }
        }

        private void DrawCutsceneFade(OpenGL gl, int width, int height)
        {
            float alpha = Math.Max(0.0f, Math.Min(1.0f, this.CutsceneFadeAlpha));
            if (alpha <= 0.0001f)
            {
                return;
            }

            this.PushScreenProjection(gl, width, height);
            gl.Disable(OpenGL.GL_DEPTH_TEST);
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Color(0.0f, 0.0f, 0.0f, alpha);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Vertex(0.0f, 0.0f);
            gl.Vertex(width, 0.0f);
            gl.Vertex(width, height);
            gl.Vertex(0.0f, height);
            gl.End();
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            this.PopScreenProjection(gl);
        }

        private void DrawCutsceneTimeline(OpenGL gl, int width, int height)
        {
            if (this.CutsceneTimelineDurationSeconds <= 0.0f ||
                this.CutsceneTimelineItems == null ||
                this.CutsceneTimelineItems.Count == 0)
            {
                this._CutsceneTimelineBounds = RectangleF.Empty;
                this._CutsceneTimelineScrubberBounds = RectangleF.Empty;
                return;
            }

            const float marginX = 22.0f;
            const float bottomMargin = 68.0f;
            const float laneHeight = 18.0f;
            const float laneGap = 3.0f;
            const int maxLanes = 4;

            int laneCount = Math.Max(1, Math.Min(maxLanes, this.CutsceneTimelineItems.Select(i => i.Lane).DefaultIfEmpty(0).Max() + 1));
            float timelineWidth = Math.Max(1.0f, width - marginX * 2.0f);
            float centerY = bottomMargin + 12.0f;
            float boxBottom = centerY + 8.0f;
            float timelineHeight = 28.0f + laneCount * (laneHeight + laneGap);
            float left = marginX;
            float right = width - marginX;
            float top = bottomMargin + timelineHeight;
            float bottom = bottomMargin - 12.0f;
            this._CutsceneTimelineBounds = new RectangleF(left, height - top, timelineWidth, top - bottom);

            this.PushScreenProjection(gl, width, height);
            gl.Disable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_BLEND);

            gl.LineWidth(2.0f);
            gl.Color(0.82f, 0.84f, 0.86f, 0.75f);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(left, bottom);
            gl.Vertex(left, top);
            gl.Vertex(right, bottom);
            gl.Vertex(right, top);
            gl.End();

            gl.LineWidth(1.0f);
            gl.Color(0.55f, 0.57f, 0.60f, 0.75f);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(left, centerY);
            gl.Vertex(right, centerY);
            gl.End();

            foreach (var item in this.CutsceneTimelineItems)
            {
                var start = Math.Max(0.0f, Math.Min(this.CutsceneTimelineDurationSeconds, item.StartSeconds));
                var end = Math.Max(start, Math.Min(this.CutsceneTimelineDurationSeconds, item.EndSeconds));
                float x0 = left + (start / this.CutsceneTimelineDurationSeconds) * timelineWidth;
                float x1 = left + (end / this.CutsceneTimelineDurationSeconds) * timelineWidth;
                if (x1 - x0 < 3.0f)
                {
                    x1 = Math.Min(right, x0 + 3.0f);
                }

                float laneBottom = boxBottom + item.Lane % laneCount * (laneHeight + laneGap);
                bool active = IsCutsceneTimelineItemActive(item, this.CutsceneTimelineSeconds, this.CutsceneTimelineDurationSeconds);
                if (active == true)
                {
                    gl.Color(0.05f, 0.55f, 0.18f, 0.85f);
                }
                else
                {
                    gl.Color(0.58f, 0.08f, 0.08f, 0.78f);
                }

                gl.Begin(OpenGL.GL_QUADS);
                gl.Vertex(x0, laneBottom);
                gl.Vertex(x1, laneBottom);
                gl.Vertex(x1, laneBottom + laneHeight);
                gl.Vertex(x0, laneBottom + laneHeight);
                gl.End();

                gl.Color(0.95f, 0.95f, 0.95f, 0.75f);
                gl.Begin(OpenGL.GL_LINE_LOOP);
                gl.Vertex(x0, laneBottom);
                gl.Vertex(x1, laneBottom);
                gl.Vertex(x1, laneBottom + laneHeight);
                gl.Vertex(x0, laneBottom + laneHeight);
                gl.End();

                var label = item.Label ?? string.Empty;
                int maxChars = Math.Max(1, (int)((x1 - x0 - 6.0f) / 7.0f));
                if (label.Length > maxChars)
                {
                    label = maxChars <= 3 ? label.Substring(0, maxChars) : label.Substring(0, maxChars - 3) + "...";
                }

                if (x1 - x0 >= 14.0f && label.Length > 0)
                {
                    gl.DrawText((int)x0 + 3, (int)laneBottom + 4, 1.0f, 1.0f, 1.0f, "Consolas", 8.0f, label);
                }
            }

            float scrubSeconds = Math.Max(0.0f, Math.Min(this.CutsceneTimelineDurationSeconds, this.CutsceneTimelineSeconds));
            float scrubX = left + (scrubSeconds / this.CutsceneTimelineDurationSeconds) * timelineWidth;
            float scrubHalfHeight = (top - bottom) * 0.375f;
            gl.LineWidth(2.0f);
            gl.Color(1.0f, 1.0f, 1.0f, 0.95f);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(scrubX, centerY - scrubHalfHeight);
            gl.Vertex(scrubX, centerY + scrubHalfHeight);
            gl.End();

            float triangleTop = top + 12.0f;
            float triangleBottom = top + 2.0f;
            float triangleHalfWidth = 7.0f;
            this._CutsceneTimelineScrubberBounds = new RectangleF(scrubX - 10.0f, height - triangleTop - 4.0f, 20.0f, triangleTop - triangleBottom + 10.0f);
            if (this._CutsceneTimelineScrubberHovered == true || this._CutsceneTimelineScrubbing == true)
            {
                gl.Color(0.95f, 0.38f, 0.02f, 0.95f);
                gl.Begin(OpenGL.GL_TRIANGLES);
                gl.Vertex(scrubX - triangleHalfWidth, triangleTop);
                gl.Vertex(scrubX + triangleHalfWidth, triangleTop);
                gl.Vertex(scrubX, triangleBottom);
                gl.End();
            }

            gl.Color(0.70f, 0.70f, 0.70f, 0.95f);
            gl.Begin(OpenGL.GL_LINE_LOOP);
            gl.Vertex(scrubX - triangleHalfWidth, triangleTop);
            gl.Vertex(scrubX + triangleHalfWidth, triangleTop);
            gl.Vertex(scrubX, triangleBottom);
            gl.End();

            gl.LineWidth(1.0f);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            this.PopScreenProjection(gl);
        }

        private void DrawVertexMorphScrubber(OpenGL gl, int width, int height)
        {
            if (this.VertexMorphScrubberVisible == false)
            {
                this._VertexMorphScrubberBounds = RectangleF.Empty;
                this._VertexMorphScrubberThumbBounds = RectangleF.Empty;
                return;
            }

            const float marginX = 22.0f;
            const float bottomMargin = 42.0f;
            float left = marginX;
            float right = Math.Max(left + 1.0f, width - marginX);
            float centerY = bottomMargin;
            float trackWidth = right - left;
            float value = Math.Max(0.0f, Math.Min(1.0f, this.VertexMorphScrubberValue));
            float thumbX = left + value * trackWidth;
            this._VertexMorphScrubberBounds = new RectangleF(left, height - (centerY + 18.0f), trackWidth, 36.0f);
            this._VertexMorphScrubberThumbBounds = new RectangleF(thumbX - 6.0f, height - (centerY + 8.0f), 12.0f, 16.0f);

            this.PushScreenProjection(gl, width, height);
            gl.Disable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_BLEND);

            gl.LineWidth(2.0f);
            gl.Color(0.22f, 0.24f, 0.27f, 0.82f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Vertex(left, centerY - 3.0f);
            gl.Vertex(right, centerY - 3.0f);
            gl.Vertex(right, centerY + 3.0f);
            gl.Vertex(left, centerY + 3.0f);
            gl.End();

            gl.Color(0.18f, 0.68f, 1.0f, 0.9f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Vertex(left, centerY - 3.0f);
            gl.Vertex(thumbX, centerY - 3.0f);
            gl.Vertex(thumbX, centerY + 3.0f);
            gl.Vertex(left, centerY + 3.0f);
            gl.End();

            gl.Color(
                this._VertexMorphScrubberHovered || this._VertexMorphScrubbing ? 1.0f : 0.82f,
                this._VertexMorphScrubberHovered || this._VertexMorphScrubbing ? 1.0f : 0.86f,
                this._VertexMorphScrubberHovered || this._VertexMorphScrubbing ? 1.0f : 0.92f,
                0.96f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Vertex(thumbX - 6.0f, centerY - 8.0f);
            gl.Vertex(thumbX + 6.0f, centerY - 8.0f);
            gl.Vertex(thumbX + 6.0f, centerY + 8.0f);
            gl.Vertex(thumbX - 6.0f, centerY + 8.0f);
            gl.End();

            this.PopScreenProjection(gl);

            string label = string.IsNullOrEmpty(this.VertexMorphScrubberLabel)
                               ? "Vertex morph"
                               : this.VertexMorphScrubberLabel;
            label = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} {1:0.###}", label, value);
            gl.DrawText((int)left, height - (int)(centerY + 26.0f), 0.8f, 0.9f, 1.0f, "Consolas", 10.0f, label);
        }

        private void PushScreenProjection(OpenGL gl, int width, int height)
        {
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PushMatrix();
            gl.LoadIdentity();
            gl.Ortho(0, width, 0, height, -1, 1);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.PushMatrix();
            gl.LoadIdentity();
        }

        private void PopScreenProjection(OpenGL gl)
        {
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.PopMatrix();
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PopMatrix();
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        private IEnumerable<string> BuildControlOverlayLines()
        {
            if (this._Camera == null || this._Camera.ShowControls == false)
            {
                return new[] { "Press H to show controls" };
            }

            return new[]
            {
                "Controls",
                "View: Left drag orbit | Right drag dolly | Middle drag pan | Middle click center | F frame vertices",
                "Freecam: V detach/return | Hold right click + WASD move | Mouse wheel speed",
                "Display: S skeleton | N bone names | L locators | I influences | T textures | Ctrl+T lighting | Ctrl+G CPU skinning | M material preview | W wire mode | C cutscene camera | H hide controls",
                "Animation: A restart | < previous frame | > next frame | X loop selection | Ctrl+R root motion",
                "Node Tree: Ctrl+Click multi-select | [ previous animation | ] next animation",
            };
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            this._MousePosition = e.Location;
            this.UpdateCutsceneTimelineMouse(e.Location);
            if (this._CutsceneTimelineScrubbing == true)
            {
                this.RequestCutsceneTimelineScrub(e.Location);
            }

            this.UpdateVertexMorphScrubberMouse(e.Location);
            if (this._VertexMorphScrubbing == true)
            {
                this.RequestVertexMorphScrub(e.Location);
            }

            this.UpdateHoveredBone();
        }

        private void OnPreviewMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.IsVertexMorphScrubberAt(e.Location) == true)
            {
                this._VertexMorphScrubbing = true;
                this.UpdateVertexMorphScrubberMouse(e.Location);
                this.RequestVertexMorphScrub(e.Location);
                return;
            }

            if (e.Button != MouseButtons.Left || this.IsCutsceneTimelineAt(e.Location) == false)
            {
                return;
            }

            this._CutsceneTimelineScrubbing = true;
            this.UpdateCutsceneTimelineMouse(e.Location);
            this.RequestCutsceneTimelineScrub(e.Location);
        }

        private void OnPreviewMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this._CutsceneTimelineScrubbing = false;
                this._VertexMorphScrubbing = false;
                this.UpdateCutsceneTimelineMouse(e.Location);
                this.UpdateVertexMorphScrubberMouse(e.Location);
            }
        }

        private void OnPreviewMouseLeave(object sender, EventArgs e)
        {
            this._HoveredBone = -1;
            this._CutsceneTimelineScrubberHovered = false;
            this._CutsceneTimelineScrubbing = false;
            this._VertexMorphScrubberHovered = false;
            this._VertexMorphScrubbing = false;
            this.Invalidate();
        }

        private void UpdateCutsceneTimelineMouse(Point point)
        {
            bool hovered = this._CutsceneTimelineScrubberBounds.Contains(point);
            if (hovered != this._CutsceneTimelineScrubberHovered)
            {
                this._CutsceneTimelineScrubberHovered = hovered;
                this.Invalidate();
            }
        }

        private void RequestCutsceneTimelineScrub(Point point)
        {
            if (this.CutsceneTimelineDurationSeconds <= 0.0f || this._CutsceneTimelineBounds.Width <= 0.0f)
            {
                return;
            }

            var t = Math.Max(0.0f, Math.Min(1.0f, (point.X - this._CutsceneTimelineBounds.Left) / this._CutsceneTimelineBounds.Width));
            var handler = this.CutsceneTimelineScrubRequested;
            if (handler != null)
            {
                handler(this, new CutsceneTimelineScrubEventArgs(t * this.CutsceneTimelineDurationSeconds));
            }
        }

        private void UpdateVertexMorphScrubberMouse(Point point)
        {
            bool hovered = this._VertexMorphScrubberBounds.Contains(point) ||
                           this._VertexMorphScrubberThumbBounds.Contains(point);
            if (hovered != this._VertexMorphScrubberHovered)
            {
                this._VertexMorphScrubberHovered = hovered;
                this.Invalidate();
            }
        }

        private void RequestVertexMorphScrub(Point point)
        {
            if (this.VertexMorphScrubberVisible == false || this._VertexMorphScrubberBounds.Width <= 0.0f)
            {
                return;
            }

            var t = Math.Max(0.0f, Math.Min(1.0f, (point.X - this._VertexMorphScrubberBounds.Left) / this._VertexMorphScrubberBounds.Width));
            var handler = this.VertexMorphScrubRequested;
            if (handler != null)
            {
                handler(this, new VertexMorphScrubEventArgs(t));
            }
        }

        private static bool IsCutsceneTimelineItemActive(CutsceneTimelineOverlayItem item, float seconds, float durationSeconds)
        {
            if (item == null || seconds < item.StartSeconds)
            {
                return false;
            }

            if (seconds < item.EndSeconds)
            {
                return true;
            }

            return durationSeconds > 0.0f &&
                   Math.Abs(seconds - durationSeconds) <= 0.0001f &&
                   Math.Abs(item.EndSeconds - durationSeconds) <= 0.0001f;
        }

        private void UpdateHoveredBone()
        {
            int hovered = -1;
            float hoveredDistance = 12.0f * 12.0f;
            if (this._Scene != null && this._Camera != null && this._Scene.Bones != null && this._ProjectionCacheValid == true)
            {
                for (int i = 0; i < this._Scene.Bones.Count; i++)
                {
                    PointF point;
                    if (this.TryProjectPoint(this._Scene.Bones[i].Position, out point) == false)
                    {
                        continue;
                    }

                    float dx = point.X - this._MousePosition.X;
                    float dy = point.Y - this._MousePosition.Y;
                    float distance = dx * dx + dy * dy;
                    if (distance < hoveredDistance)
                    {
                        hoveredDistance = distance;
                        hovered = i;
                    }
                }
            }

            if (hovered != this._HoveredBone)
            {
                this._HoveredBone = hovered;
                this.Invalidate();
            }
        }

        private bool TryProjectPoint(Vec3 point, out PointF screen)
        {
            screen = new PointF();
            if (this._ProjectionCacheValid == false)
            {
                return false;
            }

            var display = ToDisplayPosition(point);
            double clipX;
            double clipY;
            double clipZ;
            double clipW;
            TransformPoint(this._ModelViewMatrix, display, out clipX, out clipY, out clipZ, out clipW);
            TransformPoint(this._ProjectionMatrix, clipX, clipY, clipZ, clipW, out clipX, out clipY, out clipZ, out clipW);
            if (Math.Abs(clipW) <= 0.000001)
            {
                return false;
            }

            double ndcX = clipX / clipW;
            double ndcY = clipY / clipW;
            double ndcZ = clipZ / clipW;
            if (ndcZ < -1.0 || ndcZ > 1.0)
            {
                return false;
            }

            screen = new PointF(
                (float)(this._Viewport[0] + ((ndcX + 1.0) * 0.5) * this._Viewport[2]),
                (float)(this._Viewport[1] + ((1.0 - ndcY) * 0.5) * this._Viewport[3]));
            return true;
        }

        private static Vec3 ToDisplayPosition(Vec3 value)
        {
            return new Vec3(-value.X, value.Y, value.Z);
        }

        private static Vec3 ToDisplayDirection(Vec3 value)
        {
            return new Vec3(-value.X, value.Y, value.Z);
        }

        private float GetCameraControlRadius()
        {
            if (this._Camera != null && this._Camera.ControlRadius > 0.001f)
            {
                return this._Camera.ControlRadius;
            }

            return this._Scene == null ? 1.0f : this._Scene.Radius;
        }

        private static MaterialDebugColor GetMaterialDebugColor(int materialIndex, string name)
        {
            unchecked
            {
                uint hash = 2166136261u;
                var key = string.IsNullOrEmpty(name) ? materialIndex.ToString(System.Globalization.CultureInfo.InvariantCulture) : name;
                foreach (var ch in key)
                {
                    hash ^= ch;
                    hash *= 16777619u;
                }

                float hue = (hash % 360u) / 360.0f;
                return FromHsv(hue, 0.78f, 0.95f);
            }
        }

        private static MaterialDebugColor FromHsv(float hue, float saturation, float value)
        {
            float h = hue * 6.0f;
            int sector = (int)Math.Floor(h);
            float f = h - sector;
            float p = value * (1.0f - saturation);
            float q = value * (1.0f - saturation * f);
            float t = value * (1.0f - saturation * (1.0f - f));

            switch (sector % 6)
            {
                case 0:
                    return new MaterialDebugColor(value, t, p);
                case 1:
                    return new MaterialDebugColor(q, value, p);
                case 2:
                    return new MaterialDebugColor(p, value, t);
                case 3:
                    return new MaterialDebugColor(p, q, value);
                case 4:
                    return new MaterialDebugColor(t, p, value);
                default:
                    return new MaterialDebugColor(value, p, q);
            }
        }

        private static void TransformPoint(double[] matrix, Vec3 point, out double x, out double y, out double z, out double w)
        {
            TransformPoint(matrix, point.X, point.Y, point.Z, 1.0, out x, out y, out z, out w);
        }

        private static void TransformPoint(double[] matrix, double inputX, double inputY, double inputZ, double inputW, out double x, out double y, out double z, out double w)
        {
            x = matrix[0] * inputX + matrix[4] * inputY + matrix[8] * inputZ + matrix[12] * inputW;
            y = matrix[1] * inputX + matrix[5] * inputY + matrix[9] * inputZ + matrix[13] * inputW;
            z = matrix[2] * inputX + matrix[6] * inputY + matrix[10] * inputZ + matrix[14] * inputW;
            w = matrix[3] * inputX + matrix[7] * inputY + matrix[11] * inputZ + matrix[15] * inputW;
        }

        private static Vec3 Normalize(Vec3 value)
        {
            float length = (float)Math.Sqrt(LengthSquared(value));
            return length <= 0.000001f ? new Vec3() : value * (1.0f / length);
        }

        private static float LengthSquared(Vec3 value)
        {
            return value.X * value.X + value.Y * value.Y + value.Z * value.Z;
        }

        private static float Dot(Vec3 a, Vec3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        private static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
        }

        private const string GpuSkinningVertexShader = @"
#version 120
#define MAX_BONES 96
attribute vec4 aBoneIndices;
attribute vec4 aBoneWeights;
uniform mat4 uBones[MAX_BONES];
varying vec2 vTexCoord;
varying vec4 vColor;
varying vec3 vNormal;

void main()
{
    vec4 weights = max(aBoneWeights, vec4(0.0));
    float totalWeight = weights.x + weights.y + weights.z + weights.w;
    vec4 position = gl_Vertex;
    vec3 normal = gl_Normal;
    if (totalWeight > 0.0001)
    {
        vec4 skinnedPosition = vec4(0.0);
        vec3 skinnedNormal = vec3(0.0);
        int bone0 = int(aBoneIndices.x);
        int bone1 = int(aBoneIndices.y);
        int bone2 = int(aBoneIndices.z);
        int bone3 = int(aBoneIndices.w);
        if (weights.x > 0.0001 && bone0 >= 0 && bone0 < MAX_BONES)
        {
            skinnedPosition += (uBones[bone0] * gl_Vertex) * weights.x;
            skinnedNormal += (mat3(uBones[bone0]) * gl_Normal) * weights.x;
        }
        if (weights.y > 0.0001 && bone1 >= 0 && bone1 < MAX_BONES)
        {
            skinnedPosition += (uBones[bone1] * gl_Vertex) * weights.y;
            skinnedNormal += (mat3(uBones[bone1]) * gl_Normal) * weights.y;
        }
        if (weights.z > 0.0001 && bone2 >= 0 && bone2 < MAX_BONES)
        {
            skinnedPosition += (uBones[bone2] * gl_Vertex) * weights.z;
            skinnedNormal += (mat3(uBones[bone2]) * gl_Normal) * weights.z;
        }
        if (weights.w > 0.0001 && bone3 >= 0 && bone3 < MAX_BONES)
        {
            skinnedPosition += (uBones[bone3] * gl_Vertex) * weights.w;
            skinnedNormal += (mat3(uBones[bone3]) * gl_Normal) * weights.w;
        }
        position = skinnedPosition / totalWeight;
        normal = normalize(skinnedNormal);
    }

    position.x = -position.x;
    normal.x = -normal.x;
    gl_Position = gl_ModelViewProjectionMatrix * position;
    vNormal = normalize(gl_NormalMatrix * normal);
    vTexCoord = gl_MultiTexCoord0.st;
    vColor = gl_Color;
}
";

        private const string GpuSkinningFragmentShader = @"
#version 120
uniform sampler2D uTexture;
uniform int uLighting;
uniform int uUseTexture;
uniform float uAlphaCutoff;
varying vec2 vTexCoord;
varying vec4 vColor;
varying vec3 vNormal;

void main()
{
    vec4 color = vColor;
    if (uUseTexture != 0)
    {
        color *= texture2D(uTexture, vTexCoord);
    }
    if (uAlphaCutoff > 0.0 && color.a <= uAlphaCutoff)
    {
        discard;
    }
    if (uLighting != 0)
    {
        vec3 normal = normalize(vNormal);
        vec3 fillDirection = normalize(gl_NormalMatrix * normalize(vec3(0.85, -0.35, -0.35)));
        vec3 rimDirection = normalize(gl_NormalMatrix * normalize(vec3(-0.25, -0.45, 1.0)));
        vec3 keyDirection = normalize(gl_NormalMatrix * normalize(vec3(-0.45, -0.95, -0.70)));
        float fill = clamp(dot(normal, fillDirection) * 0.5 + 0.5, 0.0, 1.0);
        float rim = clamp(dot(normal, rimDirection) * 0.5 + 0.5, 0.0, 1.0);
        float key = clamp(dot(normal, keyDirection) * 0.5 + 0.5, 0.0, 1.0);
        vec3 light = vec3(0.34, 0.35, 0.37) +
                     fill * vec3(0.28, 0.32, 0.40) +
                     rim * vec3(0.24, 0.30, 0.42) +
                     key * vec3(0.62, 0.58, 0.50);
        color.rgb *= min(light, vec3(1.45));
    }
    gl_FragColor = color;
}
";
    }

    internal struct MaterialDebugColor
    {
        public readonly float R;
        public readonly float G;
        public readonly float B;

        public MaterialDebugColor(float r, float g, float b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }
    }

    internal struct ModelPreviewAlphaTriangle
    {
        public uint I0;
        public uint I1;
        public uint I2;
        public double Depth;
        public int OriginalIndex;
    }

    internal struct GpuBonePaletteKey : IEquatable<GpuBonePaletteKey>
    {
        public readonly int BoneIndex;
        public readonly bool UseRigidBoneOrigin;

        public GpuBonePaletteKey(int boneIndex, bool useRigidBoneOrigin)
        {
            this.BoneIndex = boneIndex;
            this.UseRigidBoneOrigin = useRigidBoneOrigin;
        }

        public bool Equals(GpuBonePaletteKey other)
        {
            return this.BoneIndex == other.BoneIndex &&
                   this.UseRigidBoneOrigin == other.UseRigidBoneOrigin;
        }

        public override bool Equals(object obj)
        {
            return obj is GpuBonePaletteKey && this.Equals((GpuBonePaletteKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.BoneIndex * 397) ^ (this.UseRigidBoneOrigin == true ? 1 : 0);
            }
        }
    }

    internal sealed class ModelPreviewRenderCache
    {
        public ModelPreviewScene Scene;
        public int VertexCount;
        public int IndexCount;
        public int PartCount;
        public float[] Positions;
        public float[] Normals;
        public float[] OriginalPositions;
        public float[] OriginalNormals;
        public float[] Uvs;
        public float[] InfluenceColors;
        public uint[][] PartIndices;
        public ModelPreviewBounds[] Bounds;
        public float[] BoneIndices;
        public float[] BoneWeights;
        public uint[][] SortedAlphaPartIndices;
        public ModelPreviewAlphaTriangle[][] AlphaSortTriangles;

        public void Clear()
        {
            this.Scene = null;
            this.VertexCount = 0;
            this.IndexCount = 0;
            this.PartCount = 0;
            this.Positions = null;
            this.Normals = null;
            this.OriginalPositions = null;
            this.OriginalNormals = null;
            this.Uvs = null;
            this.InfluenceColors = null;
            this.PartIndices = null;
            this.Bounds = null;
            this.BoneIndices = null;
            this.BoneWeights = null;
            this.SortedAlphaPartIndices = null;
            this.AlphaSortTriangles = null;
        }
    }

    internal enum ModelPreviewProfileSection
    {
        Animation,
        Clear,
        Textures,
        Camera,
        Grid,
        Model,
        Cache,
        Skeleton,
        Lights,
        Frustum,
        Overlay,
        Flush,
        Count,
    }

    internal sealed class ModelPreviewFrameProfiler
    {
        private readonly double[] _FrameSectionMs = new double[(int)ModelPreviewProfileSection.Count];
        private readonly double[] _AverageSectionMs = new double[(int)ModelPreviewProfileSection.Count];
        private long _FrameStart;
        private long _SectionStart;
        private ModelPreviewProfileSection _CurrentSection;
        private bool _InSection;
        private double _AverageFrameMs;

        public int FrameCount { get; private set; }
        public int PartCount { get; private set; }
        public int VertexCount { get; private set; }
        public int IndexCount { get; private set; }
        public int DrawnParts { get; set; }
        public int HiddenParts { get; set; }
        public int CulledParts { get; set; }
        public int DrawnTriangles { get; set; }

        public void BeginFrame()
        {
            if (this._InSection == true)
            {
                this.EndSection(this._CurrentSection);
            }

            Array.Clear(this._FrameSectionMs, 0, this._FrameSectionMs.Length);
            this._FrameStart = Stopwatch.GetTimestamp();
        }

        public void BeginSection(ModelPreviewProfileSection section)
        {
            if (this._InSection == true)
            {
                this.EndSection(this._CurrentSection);
            }

            this._CurrentSection = section;
            this._SectionStart = Stopwatch.GetTimestamp();
            this._InSection = true;
        }

        public void EndSection(ModelPreviewProfileSection section)
        {
            if (this._InSection == false)
            {
                return;
            }

            var elapsed = TicksToMilliseconds(Stopwatch.GetTimestamp() - this._SectionStart);
            this._FrameSectionMs[(int)section] += elapsed;
            this._InSection = false;
        }

        public void ResetDrawCounters(int partCount, int vertexCount, int indexCount)
        {
            this.PartCount = partCount;
            this.VertexCount = vertexCount;
            this.IndexCount = indexCount;
            this.DrawnParts = 0;
            this.HiddenParts = 0;
            this.CulledParts = 0;
            this.DrawnTriangles = 0;
        }

        public void EndFrame()
        {
            if (this._InSection == true)
            {
                this.EndSection(this._CurrentSection);
            }

            var frameMs = TicksToMilliseconds(Stopwatch.GetTimestamp() - this._FrameStart);
            double weight = this.FrameCount < 10 ? 1.0 / (this.FrameCount + 1) : 0.08;
            this._AverageFrameMs = this._AverageFrameMs <= 0.0
                                       ? frameMs
                                       : this._AverageFrameMs + (frameMs - this._AverageFrameMs) * weight;
            for (int i = 0; i < this._FrameSectionMs.Length; i++)
            {
                this._AverageSectionMs[i] = this._AverageSectionMs[i] <= 0.0
                                                ? this._FrameSectionMs[i]
                                                : this._AverageSectionMs[i] + (this._FrameSectionMs[i] - this._AverageSectionMs[i]) * weight;
            }
            this.FrameCount++;
        }

        public IEnumerable<string> GetOverlayLines()
        {
            double fps = this._AverageFrameMs <= 0.0001 ? 0.0 : 1000.0 / this._AverageFrameMs;
            yield return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "Profile: {0:0.00} ms ({1:0.0} fps)",
                this._AverageFrameMs,
                fps);
            yield return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "Draw: {0}/{1} parts, {2} culled, {3:n0} tris",
                this.DrawnParts,
                this.PartCount,
                this.CulledParts,
                this.DrawnTriangles);
            yield return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "Scene: {0:n0} verts, {1:n0} indices",
                this.VertexCount,
                this.IndexCount);

            foreach (var line in this.GetSectionLines())
            {
                yield return line;
            }
        }

        private IEnumerable<string> GetSectionLines()
        {
            yield return this.FormatSection("anim", ModelPreviewProfileSection.Animation);
            yield return this.FormatSection("cache", ModelPreviewProfileSection.Cache);
            yield return this.FormatSection("model", ModelPreviewProfileSection.Model);
            yield return this.FormatSection("flush", ModelPreviewProfileSection.Flush);
            yield return this.FormatSection("overlay", ModelPreviewProfileSection.Overlay);
            yield return this.FormatSection("camera", ModelPreviewProfileSection.Camera);
            yield return this.FormatSection("grid/skel/light", ModelPreviewProfileSection.Grid, ModelPreviewProfileSection.Skeleton, ModelPreviewProfileSection.Lights, ModelPreviewProfileSection.Frustum);
            yield return this.FormatSection("textures", ModelPreviewProfileSection.Textures);
        }

        private string FormatSection(string name, params ModelPreviewProfileSection[] sections)
        {
            double ms = 0.0;
            foreach (var section in sections)
            {
                ms += this._AverageSectionMs[(int)section];
            }
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}: {1:0.00} ms", name, ms);
        }

        private static double TicksToMilliseconds(long ticks)
        {
            return ticks * 1000.0 / Stopwatch.Frequency;
        }
    }

    internal struct ModelPreviewBounds
    {
        public bool Valid;
        public Vec3 Center;
        public float Radius;
    }

    internal sealed class CutsceneTimelineOverlayItem
    {
        public string Label { get; set; }
        public string Kind { get; set; }
        public float StartSeconds { get; set; }
        public float EndSeconds { get; set; }
        public int Lane { get; set; }
    }

    internal sealed class CutsceneTimelineScrubEventArgs : EventArgs
    {
        public CutsceneTimelineScrubEventArgs(float seconds)
        {
            this.Seconds = seconds;
        }

        public float Seconds { get; private set; }
    }

    internal sealed class VertexMorphScrubEventArgs : EventArgs
    {
        public VertexMorphScrubEventArgs(float value)
        {
            this.Value = Math.Max(0.0f, Math.Min(1.0f, value));
        }

        public float Value { get; private set; }
    }
}
