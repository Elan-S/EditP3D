/* Copyright (c) 2012 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gibbed.IO;
using Gibbed.Prototype.FileFormats;
using Gibbed.Prototype.FileFormats.Pure3D;
using Gibbed.Prototype.FileFormats.Pure3D.BoneData;
using Gibbed.Prototype.FileFormats.Pure3D.Prototype2;

namespace Gibbed.Prototype.Edit3D
{
    public partial class Editor : Form
    {
        private const int CutsceneTimelineTicksPerSecond = 1000;

        public Editor()
        {
            TypeDescriptor.AddAttributes(
                typeof(byte[]),
                new EditorAttribute(typeof(ByteArrayPropertyEditor), typeof(System.Drawing.Design.UITypeEditor)));

            this.InitializeComponent();
            this.splitContainer2.FixedPanel = FixedPanel.Panel2;
            this.splitContainer2.Panel2MinSize = 160;
            this.splitContainer2.Panel1.AutoScroll = false;
            this._ModelPreviewOpenGl = new ModelPreviewOpenGlControl();
            this._ModelPreviewOpenGl.Visible = false;
            this._ModelPreviewOpenGl.MouseDown += this.OnPreviewMouseDown;
            this._ModelPreviewOpenGl.MouseMove += this.OnPreviewMouseMove;
            this._ModelPreviewOpenGl.MouseUp += this.OnPreviewMouseUp;
            this._ModelPreviewOpenGl.MouseWheel += this.OnPreviewMouseWheel;
            this._ModelPreviewOpenGl.Resize += this.OnPreviewResize;
            this._ModelPreviewOpenGl.RenderFrameStarting += this.OnPreviewRenderFrameStarting;
            this._ModelPreviewOpenGl.CutsceneTimelineScrubRequested += this.OnCutsceneTimelineScrubRequested;
            this._ModelPreviewOpenGl.VertexMorphScrubRequested += this.OnVertexMorphScrubRequested;
            this.splitContainer2.Panel1.Controls.Add(this._ModelPreviewOpenGl);
            this._ModelPreviewOpenGl.BringToFront();
            this.previewPicture.Bounds = this.splitContainer2.Panel1.ClientRectangle;
            this.splitContainer2.Panel1.Resize += this.OnPreviewPanelResize;
            this.splitContainer2.Resize += this.OnPreviewPanelResize;
            this.splitContainer1.Panel2.Resize += this.OnPreviewPanelResize;
            this.ClientSizeChanged += this.OnPreviewPanelResize;
            this._PreviewRenderTimer = new Timer();
            this._PreviewRenderTimer.Interval = 16;
            this._PreviewRenderTimer.Tick += this.OnPreviewRenderTimerTick;
            this._PreviewRenderTimer.Start();
            this._PreviewRenderContinuous = false;

            this._NodeContextMenu = new ContextMenuStrip();
            this._ExportAnimationGlbMenuItem = new ToolStripMenuItem("Export Animation as GLB");
            this._ExportAnimationGlbMenuItem.Click += this.OnAnimationGlbExport;
            this._NodeContextMenu.Items.Add(this._ExportAnimationGlbMenuItem);
            this._ExportModelGlbMenuItem = new ToolStripMenuItem("Export Model as GLB");
            this._ExportModelGlbMenuItem.Click += this.OnModelGlbExport;
            this._NodeContextMenu.Items.Add(this._ExportModelGlbMenuItem);
            this._ExportSelectedToFolderMenuItem = new ToolStripMenuItem("Export Selected to Folder");
            this._ExportSelectedToFolderMenuItem.Click += this.OnSelectedFolderExport;
            this._NodeContextMenu.Items.Add(this._ExportSelectedToFolderMenuItem);
            this._ExportNodeMenuItem = new ToolStripMenuItem("Export Node Data");
            this._ExportNodeMenuItem.Click += this.OnNodeExport;
            this._NodeContextMenu.Items.Add(this._ExportNodeMenuItem);
            this._EditLuaTrackMenuItem = new ToolStripMenuItem("Edit Lua Script");
            this._EditLuaTrackMenuItem.Click += this.OnLuaTrackEdit;
            this._NodeContextMenu.Items.Add(this._EditLuaTrackMenuItem);
            this._ExportAnimationGlbDialog = new SaveFileDialog
            {
                DefaultExt = "glb",
                Filter = "Binary glTF (*.glb)|*.glb|All Files (*.*)|*.*",
            };
            this.nodeView.NodeMouseClick += this.OnNodeMouseClick;
            this.nodeView.MouseDown += this.OnNodeMouseDown;
            this.nodeView.BeforeExpand += this.OnNodeBeforeExpand;
            this.propertyGrid.PropertyValueChanged += this.OnPropertyGridValueChanged;
            this.KeyUp += this.OnEditorKeyUp;

            this._OpenCutsceneDialog = new OpenFileDialog
            {
                DefaultExt = "p3d",
                Filter = "Cutscene FIG P3D (*_fig.p3d)|*_fig.p3d|Pure3D Files (*.p3d)|*.p3d|All Files (*.*)|*.*",
                Multiselect = false,
                Title = "Open Cutscene FIG",
            };
            this.CreateCutsceneTab();
        }

        public string LastFileName;
        public string[] LoadedFileNames;
        public Pure3DFile ActiveFile;
        private ModelPreviewScene _ModelPreviewScene;
        private ModelPreviewCamera _ModelPreviewCamera;
        private Point _PreviewMousePosition;
        private MouseButtons _PreviewMouseButton;
        private bool _PreviewMouseMoved;
        private bool _FreeCameraForward;
        private bool _FreeCameraBack;
        private bool _FreeCameraLeft;
        private bool _FreeCameraRight;
        private float _FreeCameraSpeedMultiplier = 1.0f;
        private float _FreeCameraTargetSpeedMultiplier = 1.0f;
        private Vec3 _FreeCameraVelocity;
        private DateTime _LastFreeCameraMoveTime = DateTime.UtcNow;
        private Timer _PreviewRenderTimer;
        private bool _PreviewRenderQueued;
        private bool _PreviewRenderFast;
        private bool _PreviewRenderContinuous;
        private bool _PreviewRendering;
        private ModelPreviewAnimation _ModelPreviewAnimation;
        private Stopwatch _ModelPreviewAnimationClock;
        private readonly List<TreeNode> _PreviewSelectedTreeNodes = new List<TreeNode>();
        private readonly List<FileFormats.Pure3D.BaseNode> _PreviewSelectedModelNodes = new List<FileFormats.Pure3D.BaseNode>();
        private readonly List<Animation> _PreviewSelectedAnimationNodes = new List<Animation>();
        private readonly Dictionary<FileFormats.Pure3D.BaseNode, string> _NodeSourceFiles = new Dictionary<FileFormats.Pure3D.BaseNode, string>();
        private readonly List<ModelPreviewAnimation> _ModelPreviewAnimationQueue = new List<ModelPreviewAnimation>();
        private int _ModelPreviewAnimationQueueIndex;
        private bool _PreviewRootMotionEnabled = true;
        private Vec3 _PreviewRootMotionOffset;
        private float? _PreviewAnimationPausedFrame;
        private bool _PreviewLoopSelection;
        private bool _IgnoreNextNodeSelect;
        private TreeNode _PreviewSelectionAnchorNode;
        private FileFormats.Pure3D.BaseNode _PreviewActiveModelNode;
        private ModelPreviewOpenGlControl _ModelPreviewOpenGl;
        private ContextMenuStrip _NodeContextMenu;
        private ToolStripMenuItem _ExportAnimationGlbMenuItem;
        private ToolStripMenuItem _ExportModelGlbMenuItem;
        private ToolStripMenuItem _ExportSelectedToFolderMenuItem;
        private ToolStripMenuItem _ExportNodeMenuItem;
        private ToolStripMenuItem _EditLuaTrackMenuItem;
        private SaveFileDialog _ExportAnimationGlbDialog;
        private OpenFileDialog _OpenCutsceneDialog;
        private TabControl _InspectorTabs;
        private TabPage _PropertiesTab;
        private TabPage _CutsceneTab;
        private ListView _CutsceneListView;
        private TrackBar _CutsceneTimelineTrackBar;
        private Button _CutscenePlayButton;
        private Button _CutsceneStopButton;
        private Label _CutsceneStatusLabel;
        private NumericUpDown _CutsceneStageOffsetX;
        private NumericUpDown _CutsceneStageOffsetY;
        private NumericUpDown _CutsceneStageOffsetZ;
        private NumericUpDown _CutsceneStageRotationX;
        private NumericUpDown _CutsceneStageRotationY;
        private NumericUpDown _CutsceneStageRotationZ;
        private CutscenePreview _CutscenePreview;
        private CutscenePreview _CutsceneTimelineOverlaySource;
        private Stopwatch _CutscenePreviewClock;
        private float _CutscenePlaybackStartSeconds;
        private bool _UpdatingCutsceneTrackBar;
        private bool _CutscenePlaying;
        private int _LastCutsceneEvaluatedFrame = -1;
        private bool _CutsceneEvaluationDirty = true;
        private List<CutscenePlaybackEvent> _CutscenePlaybackEvents = new List<CutscenePlaybackEvent>();

        private void CreateCutsceneTab()
        {
            this._InspectorTabs = new TabControl
            {
                Dock = DockStyle.Fill,
            };
            this._PropertiesTab = new TabPage("Properties");
            this._CutsceneTab = new TabPage("Cutscene");

            this.splitContainer2.Panel2.Controls.Remove(this.propertyGrid);
            this.propertyGrid.Dock = DockStyle.Fill;
            this._PropertiesTab.Controls.Add(this.propertyGrid);

            this._CutsceneListView = new ListView
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                HideSelection = false,
                View = View.Details,
            };
            this._CutsceneListView.Columns.Add("Type", 90);
            this._CutsceneListView.Columns.Add("Name", 210);
            this._CutsceneListView.Columns.Add("Detail", 360);

            this._CutsceneStatusLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Padding = new Padding(6, 4, 0, 0),
                Text = "Open a cutscene P3D ending with \"_fig\".",
            };
            this._CutsceneTimelineTrackBar = new TrackBar
            {
                Dock = DockStyle.Bottom,
                Height = 0,
                Minimum = 0,
                Maximum = 1,
                TickStyle = TickStyle.None,
                Enabled = false,
                Visible = false,
            };
            this._CutsceneTimelineTrackBar.Scroll += this.OnCutsceneTimelineScroll;
            var controls = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                FlowDirection = FlowDirection.LeftToRight,
            };
            this._CutscenePlayButton = new Button
            {
                Text = "Play",
                Width = 70,
                Enabled = false,
            };
            this._CutscenePlayButton.Click += this.OnCutscenePlay;
            this._CutsceneStopButton = new Button
            {
                Text = "Stop",
                Width = 70,
                Enabled = false,
            };
            this._CutsceneStopButton.Click += this.OnCutsceneStop;
            controls.Controls.Add(this._CutscenePlayButton);
            controls.Controls.Add(this._CutsceneStopButton);
            controls.Controls.Add(new Label { Text = "Stage Pos", AutoSize = true, Padding = new Padding(10, 6, 0, 0) });
            this._CutsceneStageOffsetX = CreateCutsceneStageNumberBox(-1000000.0m, 1000000.0m, 1.0m);
            this._CutsceneStageOffsetY = CreateCutsceneStageNumberBox(-1000000.0m, 1000000.0m, 1.0m);
            this._CutsceneStageOffsetZ = CreateCutsceneStageNumberBox(-1000000.0m, 1000000.0m, 1.0m);
            controls.Controls.Add(this._CutsceneStageOffsetX);
            controls.Controls.Add(this._CutsceneStageOffsetY);
            controls.Controls.Add(this._CutsceneStageOffsetZ);
            controls.Controls.Add(new Label { Text = "Rot", AutoSize = true, Padding = new Padding(10, 6, 0, 0) });
            this._CutsceneStageRotationX = CreateCutsceneStageNumberBox(-3600.0m, 3600.0m, 1.0m);
            this._CutsceneStageRotationY = CreateCutsceneStageNumberBox(-3600.0m, 3600.0m, 1.0m);
            this._CutsceneStageRotationZ = CreateCutsceneStageNumberBox(-3600.0m, 3600.0m, 1.0m);
            controls.Controls.Add(this._CutsceneStageRotationX);
            controls.Controls.Add(this._CutsceneStageRotationY);
            controls.Controls.Add(this._CutsceneStageRotationZ);

            this._CutsceneTab.Controls.Add(this._CutsceneListView);
            this._CutsceneTab.Controls.Add(this._CutsceneStatusLabel);
            this._CutsceneTab.Controls.Add(this._CutsceneTimelineTrackBar);
            this._CutsceneTab.Controls.Add(controls);
            this._InspectorTabs.TabPages.Add(this._PropertiesTab);
            this._InspectorTabs.TabPages.Add(this._CutsceneTab);
            this.splitContainer2.Panel2.Controls.Add(this._InspectorTabs);
            this._InspectorTabs.BringToFront();
        }

        private NumericUpDown CreateCutsceneStageNumberBox(decimal minimum, decimal maximum, decimal increment)
        {
            var control = new NumericUpDown
            {
                Width = 72,
                DecimalPlaces = 2,
                Minimum = minimum,
                Maximum = maximum,
                Increment = increment,
            };
            control.ValueChanged += this.OnCutsceneStageTransformChanged;
            return control;
        }

        private void OnFileOpen(object sender, EventArgs e)
        {
            if (this.openPure3DFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.LoadPure3DFiles(this.openPure3DFileDialog.FileNames);
        }

        private void OnFileAdd(object sender, EventArgs e)
        {
            if (this.openPure3DFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.AddPure3DFiles(this.openPure3DFileDialog.FileNames);
        }

        private void OnCutsceneOpen(object sender, EventArgs e)
        {
            if (this._OpenCutsceneDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var figFileName = Path.GetFullPath(this._OpenCutsceneDialog.FileName);
            if (figFileName.EndsWith("_fig.p3d", StringComparison.OrdinalIgnoreCase) == false)
            {
                MessageBox.Show(this, "Select a cutscene P3D ending with \"_fig.p3d\".", "Open Cutscene", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var files = GetCutscenePackageFiles(figFileName);
            this.LoadPure3DFiles(files);
            this._CutscenePreview = CutscenePreview.Build(this.ActiveFile, figFileName, this.LoadedFileNames);
            this.ShowCutscenePreview();
        }

        private static IEnumerable<string> GetCutscenePackageFiles(string figFileName)
        {
            var files = new List<string> { figFileName };
            var queued = new Queue<string>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var folder = Path.GetDirectoryName(figFileName);
            if (string.IsNullOrEmpty(folder) == true || Directory.Exists(folder) == false)
            {
                return files;
            }

            Pure3DFile figFile;
            try
            {
                figFile = LoadPure3DFile(figFileName);
            }
            catch
            {
                return files;
            }

            AddCutscenePackageReferences(folder, figFileName, figFile, files, queued, seen);

            while (queued.Count > 0)
            {
                var currentFileName = queued.Dequeue();
                Pure3DFile currentFile;
                try
                {
                    currentFile = LoadPure3DFile(currentFileName);
                }
                catch
                {
                    continue;
                }

                AddCutscenePackageReferences(folder, currentFileName, currentFile, files, queued, seen);
            }

            return files.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        }

        private static void AddCutscenePackageReferences(
            string folder,
            string sourceFileName,
            Pure3DFile sourceFile,
            ICollection<string> files,
            Queue<string> queued,
            ISet<string> seen)
        {
            var referencedNames = CutscenePreview.FindReferencedP3DBaseNames(sourceFile);
            foreach (var referencedFileName in CutscenePreview.FindReferencedP3DFiles(folder, sourceFileName, referencedNames))
            {
                if (files.Contains(referencedFileName, StringComparer.OrdinalIgnoreCase) == false)
                {
                    files.Add(referencedFileName);
                }

                if (seen.Add(referencedFileName) == true)
                {
                    queued.Enqueue(referencedFileName);
                }
            }

            var referencedCells = CutscenePreview.FindReferencedCellBaseNames(sourceFile);
            foreach (var referencedFileName in CutscenePreview.FindReferencedCellP3DFiles(sourceFileName, referencedCells))
            {
                if (files.Contains(referencedFileName, StringComparer.OrdinalIgnoreCase) == false)
                {
                    files.Add(referencedFileName);
                }

                if (seen.Add(referencedFileName) == true)
                {
                    queued.Enqueue(referencedFileName);
                }
            }
        }

        private void ShowCutscenePreview()
        {
            if (this._CutscenePreview == null)
            {
                return;
            }

            this.PopulateCutsceneTab(this._CutscenePreview);
            this.propertyGrid.SelectedObject = this._CutscenePreview;
            if (this._InspectorTabs != null)
            {
                this._InspectorTabs.SelectedTab = this._CutsceneTab;
            }

            this._PreviewActiveModelNode = null;
            this._ModelPreviewAnimation = null;
            this._ModelPreviewAnimationClock = null;
            this._ModelPreviewAnimationQueue.Clear();
            this._ModelPreviewAnimationQueueIndex = 0;
            this._PreviewRootMotionOffset = new Vec3();
            this._PreviewAnimationPausedFrame = null;
            this.StopCutscenePreview();

            if (this.ActiveFile != null && this._CutscenePreview.PreviewNodes.Count > 0)
            {
                this._ModelPreviewScene = ModelPreviewBuilder.CreateScene(
                    this.ActiveFile,
                    this._CutscenePreview.PreviewNodes,
                    this.LastFileName,
                    this._CutscenePreview.StagePreviewNodes);
                this._ModelPreviewScene.UseGpuSkinning = true;
                this._ModelPreviewCamera = ModelPreviewBuilder.CreateDefaultCamera(this._ModelPreviewScene);
                this._ModelPreviewCamera.ControlRadius = 10.0f;
                this.ApplyCutsceneStageDebugTransform();
                this.ApplyCutsceneCamera();
                this.ApplyCutsceneLights();
                this.CreateCutsceneActorInstances();
                this.CreateCutscenePropInstances();
                this.ApplyCutsceneActorTransforms();
                ModelPreviewBuilder.ApplyInstanceTransformsToBindPose(this._ModelPreviewScene);
                this.BuildCutscenePlaybackEvents();
                this._CutscenePlaybackStartSeconds = 0.0f;
                this._CutsceneEvaluationDirty = true;
                this.ApplyCutscenePreviewAtSeconds(0.0f);
                this.ShowModelPreview();
            }
            else
            {
                this._ModelPreviewScene = null;
                this._ModelPreviewCamera = null;
                this._CutscenePlaybackEvents.Clear();
                this.UpdateCutsceneTimelineOverlay(null, 0.0f);
                this._ModelPreviewOpenGl.SetPreview(null, null);
            }

            this.Text = "Edit3D - Cutscene - " + Path.GetFileName(this._CutscenePreview.FigFileName);
        }

        private void LeaveCutscenePreviewMode()
        {
            this.StopCutscenePreview();
            this._CutscenePreview = null;
            this._CutsceneTimelineOverlaySource = null;
            this._CutscenePlaybackEvents.Clear();
            this._CutscenePlaybackStartSeconds = 0.0f;
            this._LastCutsceneEvaluatedFrame = -1;
            this._CutsceneEvaluationDirty = true;
            this._PreviewRenderContinuous = false;
            if (this._ModelPreviewOpenGl != null)
            {
                this._ModelPreviewOpenGl.CutsceneStatus = null;
                this._ModelPreviewOpenGl.CutsceneFadeAlpha = 0.0f;
                this._ModelPreviewOpenGl.CutsceneLightGroupStatus = null;
                this.UpdateCutsceneTimelineOverlay(null, 0.0f);
            }
        }

        private void ApplyCutsceneCamera()
        {
            if (this._ModelPreviewCamera == null || this._CutscenePreview == null || this._CutscenePreview.Cameras.Count == 0)
            {
                return;
            }

            var camera = this._CutscenePreview.Cameras[0];
            this._ModelPreviewCamera.UseCutsceneCamera = true;
            this._ModelPreviewCamera.HasCutsceneCamera = true;
            this._ModelPreviewCamera.CutscenePosition = camera.Position;
            this._ModelPreviewCamera.CutsceneLook = camera.Look;
            this._ModelPreviewCamera.CutsceneUp = camera.Up;
            this._ModelPreviewCamera.CutsceneFov = ModelPreviewBuilder.NormalizeFovDegrees(camera.Fov);
            this._ModelPreviewCamera.CutsceneNearClip = camera.NearClip;
            this._ModelPreviewCamera.CutsceneFarClip = camera.FarClip;
        }

        private void ApplyCutsceneLights()
        {
            if (this._ModelPreviewScene == null || this._CutscenePreview == null)
            {
                return;
            }

            this._ModelPreviewScene.Lights.Clear();
            this._ModelPreviewScene.UseCinematicLighting = true;
            foreach (var light in this._CutscenePreview.Lights)
            {
                if (light == null)
                {
                    continue;
                }

                this._ModelPreviewScene.Lights.Add(new ModelPreviewLight
                {
                    Name = string.IsNullOrEmpty(light.ShortName) == true ? light.LongName : light.ShortName,
                    SourceHash = light.SourceHash,
                    RequiresTarget = true,
                    LightGroupHash = light.LightGroupHash,
                    LightGroupName = light.LightGroupName,
                    TargetActorName = string.IsNullOrEmpty(light.TargetActorName) == true ? light.TargetCompositeName : light.TargetActorName,
                    TargetActorHash = light.TargetActorHash,
                    Dynamic = false,
                    Position = light.Position,
                    Color = LengthSquared(light.Color) <= 0.000001f ? new Vec3(1.0f, 1.0f, 1.0f) : light.Color,
                    Intensity = light.Intensity <= 0.0001f ? 1.0f : light.Intensity,
                    Range = light.Range <= 0.001f ? 100.0f : light.Range,
                });
            }
        }

        private void CreateCutsceneActorInstances()
        {
            if (this._CutscenePreview == null || this._ModelPreviewScene == null || this._ModelPreviewScene.Instances == null)
            {
                return;
            }

            foreach (var actor in this._CutscenePreview.Actors)
            {
                if (actor == null || actor.ActorHash == 0 || string.IsNullOrEmpty(actor.CompositeName) == true)
                {
                    continue;
                }

                if (this._ModelPreviewScene.Instances.Any(i => i.ActorHash == actor.ActorHash) == true)
                {
                    continue;
                }

                var template = this.ResolveCutsceneTemplateInstance(actor.CompositeName);
                if (template == null)
                {
                    continue;
                }

                var name = string.IsNullOrEmpty(actor.ShortName) == false ? actor.ShortName : actor.CompositeName;
                var instance = template.ActorHash == 0
                    ? template
                    : ModelPreviewBuilder.DuplicateInstance(this._ModelPreviewScene, template, name);
                if (instance != null)
                {
                    instance.ActorHash = actor.ActorHash;
                    instance.LightGroupHash = actor.LightGroupHash;
                    instance.LightGroupName = actor.LightGroupName;
                }
            }
        }

        private ModelPreviewSceneInstance ResolveCutsceneTemplateInstance(string compositeName)
        {
            if (string.IsNullOrEmpty(compositeName) == true || this._ModelPreviewScene == null || this._ModelPreviewScene.Instances == null)
            {
                return null;
            }

            return this._ModelPreviewScene.Instances.FirstOrDefault(i => NamesMatch(i.Name, compositeName));
        }

        private void CreateCutscenePropInstances()
        {
            if (this._CutscenePreview == null || this._ModelPreviewScene == null || this._ModelPreviewScene.Instances == null)
            {
                return;
            }

            foreach (var prop in this._CutscenePreview.Props)
            {
                if (prop == null || prop.ActorHash == 0)
                {
                    continue;
                }

                if (this._ModelPreviewScene.Instances.Any(i => i.ActorHash == prop.ActorHash) == true)
                {
                    continue;
                }

                var template = this.ResolveCutscenePropTemplateInstance(prop);
                if (template == null)
                {
                    continue;
                }

                var name = string.IsNullOrEmpty(prop.ActorName) == false
                               ? prop.ActorName
                               : string.IsNullOrEmpty(prop.ShortName) == false
                                     ? prop.ShortName
                                     : template.Name;
                var instance = template.ActorHash == 0
                    ? template
                    : ModelPreviewBuilder.DuplicateInstance(this._ModelPreviewScene, template, name);
                if (instance != null)
                {
                    instance.ActorHash = prop.ActorHash;
                    instance.Name = name;
                }
            }
        }

        private ModelPreviewSceneInstance ResolveCutscenePropTemplateInstance(CutscenePropObject prop)
        {
            if (prop == null || this._ModelPreviewScene == null || this._ModelPreviewScene.Instances == null)
            {
                return null;
            }

            var candidates = new[]
            {
                StripTemplateSuffix(prop.TemplateName),
                prop.TemplateName,
                prop.ActorName,
                prop.ShortName,
                prop.LongName,
            }.Where(s => string.IsNullOrEmpty(s) == false)
             .Distinct(StringComparer.OrdinalIgnoreCase)
             .ToArray();

            foreach (var candidate in candidates)
            {
                var exact = this._ModelPreviewScene.Instances.FirstOrDefault(i => NamesMatch(i.Name, candidate));
                if (exact != null)
                {
                    return exact;
                }
            }

            foreach (var candidate in candidates.Select(NormalizeCutsceneName).Where(s => string.IsNullOrEmpty(s) == false))
            {
                var byName = this._ModelPreviewScene.Instances.FirstOrDefault(i =>
                    ContainsNormalized(i.Name, candidate) ||
                    ContainsNormalized(i.SkeletonName, candidate));
                if (byName != null)
                {
                    return byName;
                }
            }

            return null;
        }

        private void ApplyCutsceneActorTransforms()
        {
            if (this._CutscenePreview == null || this._ModelPreviewScene == null || this._ModelPreviewScene.Instances == null)
            {
                return;
            }

            foreach (var actor in this._CutscenePreview.Actors)
            {
                var instance = this.ResolveCutsceneInstance(actor);
                if (instance == null)
                {
                    continue;
                }

                instance.Position = actor.Position;
                instance.Rotation = new PreviewQuat
                {
                    X = actor.RotationX,
                    Y = actor.RotationY,
                    Z = actor.RotationZ,
                    W = Math.Abs(actor.RotationW) <= 0.000001f &&
                        Math.Abs(actor.RotationX) <= 0.000001f &&
                        Math.Abs(actor.RotationY) <= 0.000001f &&
                        Math.Abs(actor.RotationZ) <= 0.000001f
                            ? 1.0f
                            : actor.RotationW,
                };
            }
        }

        private void ApplyCutscenePropTransforms()
        {
            if (this._CutscenePreview == null || this._ModelPreviewScene == null || this._ModelPreviewScene.Instances == null)
            {
                return;
            }

            foreach (var prop in this._CutscenePreview.Props)
            {
                var instance = this.ResolveCutscenePropInstance(prop);
                if (instance == null)
                {
                    continue;
                }

                instance.Position = prop.Position;
                instance.Rotation = new PreviewQuat
                {
                    X = prop.RotationX,
                    Y = prop.RotationY,
                    Z = prop.RotationZ,
                    W = Math.Abs(prop.RotationW) <= 0.000001f &&
                        Math.Abs(prop.RotationX) <= 0.000001f &&
                        Math.Abs(prop.RotationY) <= 0.000001f &&
                        Math.Abs(prop.RotationZ) <= 0.000001f
                            ? 1.0f
                            : prop.RotationW,
                };
            }
        }

        private void OnCutsceneStageTransformChanged(object sender, EventArgs e)
        {
            this.ApplyCutsceneStageDebugTransform();
            this._CutsceneEvaluationDirty = true;
            if (this._CutscenePreview != null && this._ModelPreviewScene != null)
            {
                if (this._ModelPreviewScene.BindVertices != null && this._ModelPreviewScene.BindVertices.Count == this._ModelPreviewScene.Vertices.Count)
                {
                    this._ModelPreviewScene.Vertices = this._ModelPreviewScene.BindVertices.ToList();
                }

                this.ApplyCutscenePreviewAtSeconds(this._CutscenePlaybackStartSeconds);
            }
        }

        private void ApplyCutsceneStageDebugTransform()
        {
            if (this._ModelPreviewScene == null)
            {
                return;
            }

            this._ModelPreviewScene.StageDebugPositionOffset = new Vec3(
                ToSingle(this._CutsceneStageOffsetX),
                ToSingle(this._CutsceneStageOffsetY),
                ToSingle(this._CutsceneStageOffsetZ));
            this._ModelPreviewScene.StageDebugRotationDegrees = new Vec3(
                ToSingle(this._CutsceneStageRotationX),
                ToSingle(this._CutsceneStageRotationY),
                ToSingle(this._CutsceneStageRotationZ));
        }

        private static float ToSingle(NumericUpDown control)
        {
            return control == null ? 0.0f : (float)control.Value;
        }

        private void PopulateCutsceneTab(CutscenePreview cutscene)
        {
            this._CutsceneListView.Items.Clear();
            foreach (var fight in cutscene.FightTracks)
            {
                var detail = string.Format("context={0}; bytes={1}; strings={2}", fight.Context, fight.DataLength, string.Join(", ", fight.Strings ?? new string[0]));
                this._CutsceneListView.Items.Add(new ListViewItem(new[] { "Fight", fight.Name, detail }));
            }

            foreach (var fileName in cutscene.ReferencedP3DFileNames ?? new string[0])
            {
                this._CutsceneListView.Items.Add(new ListViewItem(new[] { "Referenced P3D", Path.GetFileName(fileName), fileName }));
            }

            foreach (var actor in cutscene.Actors)
            {
                var detail = string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "type={0}; short={1}; actorHash={2:X16}; composite={3}; pos=({4:0.###},{5:0.###},{6:0.###}); rot=({7:0.###},{8:0.###},{9:0.###},{10:0.###}); bytes={11}; strings={12}",
                    actor.TypeName,
                    actor.ShortName,
                    actor.ActorHash,
                    string.IsNullOrEmpty(actor.CompositeName) ? "unresolved" : actor.CompositeName,
                    actor.Position.X,
                    actor.Position.Y,
                    actor.Position.Z,
                    actor.RotationX,
                    actor.RotationY,
                    actor.RotationZ,
                    actor.RotationW,
                    actor.DataLength,
                    string.Join(", ", actor.Strings ?? new string[0]));
                this._CutsceneListView.Items.Add(new ListViewItem(new[] { "Meta", actor.LongName, detail }));
            }

            foreach (var prop in cutscene.Props)
            {
                var detail = string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "type={0}; actor={1}; actorHash={2:X16}; template={3}; pos=({4:0.###},{5:0.###},{6:0.###}); rot=({7:0.###},{8:0.###},{9:0.###},{10:0.###}); bytes={11}; strings={12}",
                    prop.TypeName,
                    string.IsNullOrEmpty(prop.ActorName) ? "unresolved" : prop.ActorName,
                    prop.ActorHash,
                    string.IsNullOrEmpty(prop.TemplateName) ? "unresolved" : prop.TemplateName,
                    prop.Position.X,
                    prop.Position.Y,
                    prop.Position.Z,
                    prop.RotationX,
                    prop.RotationY,
                    prop.RotationZ,
                    prop.RotationW,
                    prop.DataLength,
                    string.Join(", ", prop.Strings ?? new string[0]));
                this._CutsceneListView.Items.Add(new ListViewItem(new[] { "Prop", prop.LongName, detail }));
            }

            foreach (var camera in cutscene.Cameras)
            {
                var detail = string.Format(System.Globalization.CultureInfo.InvariantCulture, "fov={0:0.###}; near={1:0.#####}; far={2:0.###}; pos=({3:0.###},{4:0.###},{5:0.###})",
                    camera.Fov, camera.NearClip, camera.FarClip, camera.Position.X, camera.Position.Y, camera.Position.Z);
                this._CutsceneListView.Items.Add(new ListViewItem(new[] { "Camera", camera.Name, detail }));
            }

            foreach (var light in cutscene.Lights)
            {
                var detail = string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "type={0}; pos=({1:0.###},{2:0.###},{3:0.###}); bytes={4}; strings={5}",
                    light.TypeName,
                    light.Position.X,
                    light.Position.Y,
                    light.Position.Z,
                    light.DataLength,
                    string.Join(", ", light.Strings ?? new string[0]));
                this._CutsceneListView.Items.Add(new ListViewItem(new[] { "Light", light.LongName, detail }));
            }

            foreach (var evt in cutscene.TimelineEvents
                                        .OrderBy(e => e.StartSeconds)
                                        .ThenBy(e => e.EndSeconds)
                                        .ThenBy(e => e.Kind)
                                        .ThenBy(e => e.ActorName)
                                        .ThenBy(e => e.AnimationName))
            {
                var detail = string.Equals(evt.Kind, "SetObjectLightGroup", StringComparison.OrdinalIgnoreCase) == true
                    ? string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "actor={0}; actorHash={1:X16}; timeBegin={2:0.###}; timeEnd={3:0.###}; groupName={4:X16}; offset=0x{5:X}",
                        string.IsNullOrEmpty(evt.ActorName) ? "unknown" : evt.ActorName,
                        evt.ActorHash,
                        evt.StartSeconds,
                        evt.RawEndSeconds == 0.0f && evt.EndSeconds != 0.0f ? evt.EndSeconds : evt.RawEndSeconds,
                        evt.LightGroupHash,
                        evt.TrackOffset)
                    : string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "actor={0}; actorHash={1:X16}; anim={2}; {3:0.###}-{4:0.###}s; frames={5:0.###}-{6:0.###}; speed={7:0.###}; animHash={8:X16}; offset=0x{9:X}",
                        string.IsNullOrEmpty(evt.ActorName) ? "unknown" : evt.ActorName,
                        evt.ActorHash,
                        evt.AnimationName,
                        evt.StartSeconds,
                        evt.EndSeconds,
                        evt.StartFrame,
                        evt.EndFrame,
                        evt.Speed,
                        evt.AnimationHash,
                        evt.TrackOffset);
                this._CutsceneListView.Items.Add(new ListViewItem(new[] { "Timeline " + evt.Kind, evt.FightName, detail }));
            }

            this._CutsceneStatusLabel.Text = string.Format(
                "Loaded {0} file(s), {1} referenced by Fight data. Fights: {2}. Timeline: {3}. Meta actors/lights: {4}. Cameras: {5}. Preview nodes: {6}. Press C to toggle cutscene/orbit camera.",
                cutscene.LoadedFileNames == null ? 0 : cutscene.LoadedFileNames.Length,
                cutscene.ReferencedP3DFileNames == null ? 0 : cutscene.ReferencedP3DFileNames.Length,
                cutscene.FightTracks.Count,
                cutscene.TimelineEvents.Count,
                cutscene.Actors.Count + cutscene.Props.Count + cutscene.Lights.Count,
                cutscene.Cameras.Count,
                cutscene.PreviewNodes.Count);
            this._CutsceneTimelineTrackBar.Maximum = Math.Max(1, (int)Math.Ceiling(Math.Max(0.0f, cutscene.DurationSeconds) * CutsceneTimelineTicksPerSecond));
            this._CutsceneTimelineTrackBar.Value = 0;
            this._CutsceneTimelineTrackBar.Enabled = cutscene.DurationSeconds > 0.0f;
            this._CutscenePlayButton.Enabled = cutscene.DurationSeconds > 0.0f;
            this._CutsceneStopButton.Enabled = cutscene.DurationSeconds > 0.0f;
            this.UpdateCutsceneTimelineOverlay(cutscene, 0.0f);
        }

        private void UpdateCutsceneTimelineOverlay(CutscenePreview cutscene, float seconds)
        {
            if (this._ModelPreviewOpenGl == null)
            {
                return;
            }

            this._ModelPreviewOpenGl.CutsceneTimelineSeconds = Math.Max(0.0f, seconds);
            this._ModelPreviewOpenGl.CutsceneTimelineDurationSeconds = cutscene == null ? 0.0f : Math.Max(0.0f, cutscene.DurationSeconds);
            if (cutscene == null || cutscene.TimelineEvents == null)
            {
                this._CutsceneTimelineOverlaySource = null;
                this._ModelPreviewOpenGl.CutsceneTimelineItems.Clear();
                return;
            }

            if (object.ReferenceEquals(this._CutsceneTimelineOverlaySource, cutscene) == true &&
                this._ModelPreviewOpenGl.CutsceneTimelineItems.Count > 0)
            {
                return;
            }

            this._CutsceneTimelineOverlaySource = cutscene;
            this._ModelPreviewOpenGl.CutsceneTimelineItems.Clear();
            var laneKeys = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var evt in cutscene.TimelineEvents
                                       .OrderBy(e => e.StartSeconds)
                                       .ThenBy(e => e.EndSeconds)
                                       .ThenBy(e => e.Kind)
                                       .ThenBy(e => e.ActorName)
                                       .ThenBy(e => e.AnimationName))
            {
                if (evt.EndSeconds < evt.StartSeconds)
                {
                    continue;
                }

                var key = string.IsNullOrEmpty(evt.ActorName) ? evt.Kind : evt.ActorName;
                int lane;
                if (laneKeys.TryGetValue(key, out lane) == false)
                {
                    lane = laneKeys.Count;
                    laneKeys.Add(key, lane);
                }

                this._ModelPreviewOpenGl.CutsceneTimelineItems.Add(new CutsceneTimelineOverlayItem
                {
                    Kind = evt.Kind,
                    StartSeconds = evt.StartSeconds,
                    EndSeconds = evt.EndSeconds,
                    Lane = lane,
                    Label = this.FormatCutsceneTimelineLabel(evt),
                });
            }
        }

        private string FormatCutsceneTimelineLabel(CutsceneTimelineEvent evt)
        {
            if (evt == null)
            {
                return string.Empty;
            }

            var actor = string.IsNullOrEmpty(evt.ActorName) == true ? evt.Kind : evt.ActorName;
            var name = string.IsNullOrEmpty(evt.AnimationName) == true ? evt.Kind : evt.AnimationName;
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}: {1}", actor, name);
        }

        private void OnCutscenePlay(object sender, EventArgs e)
        {
            if (this._CutscenePreview == null || this._ModelPreviewScene == null)
            {
                return;
            }

            this._CutscenePlaying = true;
            this._CutscenePlaybackStartSeconds = this._CutsceneTimelineTrackBar.Enabled == true
                                                     ? this._CutsceneTimelineTrackBar.Value / (float)CutsceneTimelineTicksPerSecond
                                                     : 0.0f;
            this._CutscenePreviewClock = Stopwatch.StartNew();
            this._PreviewRenderContinuous = true;
            this._CutsceneEvaluationDirty = true;
            this.UpdateCutscenePreviewPlayback();
            this.RefreshModelPreview(true);
        }

        private void OnCutsceneStop(object sender, EventArgs e)
        {
            this.StopCutscenePreview();
            this.RefreshModelPreview(true);
        }

        private void StopCutscenePreview()
        {
            this._CutscenePlaying = false;
            this._CutscenePreviewClock = null;
            if (this._ModelPreviewOpenGl != null)
            {
                this._ModelPreviewOpenGl.CutsceneStatus = null;
            }
        }

        private void OnCutsceneTimelineScroll(object sender, EventArgs e)
        {
            if (this._UpdatingCutsceneTrackBar == true || this._CutscenePreview == null || this._ModelPreviewScene == null)
            {
                return;
            }

            this._CutscenePlaying = false;
            this._CutscenePreviewClock = null;
            this._CutscenePlaybackStartSeconds = this._CutsceneTimelineTrackBar.Value / (float)CutsceneTimelineTicksPerSecond;
            this._CutsceneEvaluationDirty = true;
            this.ApplyCutscenePreviewAtSeconds(this._CutscenePlaybackStartSeconds);
            this.RefreshModelPreview(true);
        }

        private void BuildCutscenePlaybackEvents()
        {
            this._CutscenePlaybackEvents.Clear();
            if (this._CutscenePreview == null || this.ActiveFile == null || this._ModelPreviewScene == null)
            {
                return;
            }

            var animations = Flatten(this.ActiveFile.Nodes)
                .OfType<Animation>()
                .Where(a => string.IsNullOrEmpty(a.Name) == false)
                .GroupBy(a => a.Name, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
            foreach (var evt in this._CutscenePreview.TimelineEvents)
            {
                Animation animationNode;
                if (string.IsNullOrEmpty(evt.AnimationName) == true ||
                    animations.TryGetValue(evt.AnimationName, out animationNode) == false)
                {
                    continue;
                }

                if (string.Equals(evt.Kind, "CameraAnimation", StringComparison.OrdinalIgnoreCase) == true ||
                    string.Equals(evt.Kind, "ScriptedCamera", StringComparison.OrdinalIgnoreCase) == true)
                {
                    var cameraAnimation = ModelPreviewBuilder.CreateCameraAnimation(animationNode);
                    if (cameraAnimation == null)
                    {
                        continue;
                    }

                    this._CutscenePlaybackEvents.Add(new CutscenePlaybackEvent
                    {
                        TimelineEvent = evt,
                        CameraAnimation = cameraAnimation,
                    });
                    continue;
                }

                if (string.Equals(evt.Kind, "PuppetAnimation", StringComparison.OrdinalIgnoreCase) == false)
                {
                    continue;
                }

                var instance = this.ResolveCutsceneInstance(evt);
                if (instance == null)
                {
                    continue;
                }

                var previewAnimation = ModelPreviewBuilder.CreateAnimation(this._ModelPreviewScene, animationNode, instance);
                if (previewAnimation == null)
                {
                    continue;
                }

                this._CutscenePlaybackEvents.Add(new CutscenePlaybackEvent
                {
                    TimelineEvent = evt,
                    Animation = previewAnimation,
                    Instance = instance,
                });
            }
        }

        private ModelPreviewSceneInstance ResolveCutsceneInstance(CutsceneTimelineEvent evt)
        {
            if (evt == null || this._ModelPreviewScene == null || this._ModelPreviewScene.Instances == null || this._ModelPreviewScene.Instances.Count == 0)
            {
                return null;
            }

            var actorName = evt.ActorName;
            if (evt.ActorHash != 0)
            {
                var propInstance = this._ModelPreviewScene.Instances.FirstOrDefault(i => i.ActorHash == evt.ActorHash);
                if (propInstance != null)
                {
                    return propInstance;
                }
            }

            var actor = this.ResolveCutsceneActor(evt);
            if (actor != null && actor.ActorHash != 0)
            {
                var byHash = this._ModelPreviewScene.Instances.FirstOrDefault(i => i.ActorHash == actor.ActorHash);
                if (byHash != null)
                {
                    return byHash;
                }
            }

            if (actor != null && string.IsNullOrEmpty(actor.CompositeName) == false)
            {
                var byComposite = this._ModelPreviewScene.Instances.FirstOrDefault(i => i.ActorHash == 0 && NamesMatch(i.Name, actor.CompositeName));
                if (byComposite != null)
                {
                    return byComposite;
                }
            }

            if (string.IsNullOrEmpty(actorName) == false)
            {
                var normalizedActor = NormalizeCutsceneName(actorName);
                var byName = this._ModelPreviewScene.Instances.FirstOrDefault(i =>
                    ContainsNormalized(i.Name, normalizedActor) ||
                    ContainsNormalized(i.SkeletonName, normalizedActor));
                if (byName != null)
                {
                    return byName;
                }
            }

            if (this._ModelPreviewScene.Instances.Count == 1)
            {
                return this._ModelPreviewScene.Instances[0];
            }

            return null;
        }

        private ModelPreviewSceneInstance ResolveCutsceneInstance(CutsceneActorObject actor)
        {
            if (actor == null || this._ModelPreviewScene == null || this._ModelPreviewScene.Instances == null || this._ModelPreviewScene.Instances.Count == 0)
            {
                return null;
            }

            if (string.IsNullOrEmpty(actor.CompositeName) == false)
            {
                var byHash = actor.ActorHash == 0
                    ? null
                    : this._ModelPreviewScene.Instances.FirstOrDefault(i => i.ActorHash == actor.ActorHash);
                if (byHash != null)
                {
                    return byHash;
                }

                var byComposite = this._ModelPreviewScene.Instances.FirstOrDefault(i => i.ActorHash == 0 && NamesMatch(i.Name, actor.CompositeName));
                if (byComposite != null)
                {
                    return byComposite;
                }
            }

            var actorName = actor.ShortName;
            if (string.IsNullOrEmpty(actorName) == false && actorName.EndsWith("Spawner", StringComparison.OrdinalIgnoreCase) == true)
            {
                actorName = actorName.Substring(0, actorName.Length - "Spawner".Length);
            }

            if (string.IsNullOrEmpty(actorName) == false)
            {
                var normalizedActor = NormalizeCutsceneName(actorName);
                var byName = this._ModelPreviewScene.Instances.FirstOrDefault(i =>
                    ContainsNormalized(i.Name, normalizedActor) ||
                    ContainsNormalized(i.SkeletonName, normalizedActor));
                if (byName != null)
                {
                    return byName;
                }
            }

            return null;
        }

        private ModelPreviewSceneInstance ResolveCutscenePropInstance(CutscenePropObject prop)
        {
            if (prop == null || this._ModelPreviewScene == null || this._ModelPreviewScene.Instances == null || this._ModelPreviewScene.Instances.Count == 0)
            {
                return null;
            }

            if (prop.ActorHash != 0)
            {
                var byHash = this._ModelPreviewScene.Instances.FirstOrDefault(i => i.ActorHash == prop.ActorHash);
                if (byHash != null)
                {
                    return byHash;
                }
            }

            var candidates = new[]
            {
                prop.ActorName,
                StripTemplateSuffix(prop.TemplateName),
                prop.TemplateName,
                prop.ShortName,
                prop.LongName,
            }.Where(s => string.IsNullOrEmpty(s) == false)
             .Select(NormalizeCutsceneName)
             .Where(s => string.IsNullOrEmpty(s) == false)
             .Distinct(StringComparer.OrdinalIgnoreCase)
             .ToArray();

            foreach (var candidate in candidates)
            {
                var byName = this._ModelPreviewScene.Instances.FirstOrDefault(i =>
                    ContainsNormalized(i.Name, candidate) ||
                    ContainsNormalized(i.SkeletonName, candidate));
                if (byName != null)
                {
                    return byName;
                }
            }

            return null;
        }

        private static string StripTemplateSuffix(string value)
        {
            return string.IsNullOrEmpty(value) == false &&
                   value.EndsWith("Template", StringComparison.OrdinalIgnoreCase) == true
                       ? value.Substring(0, value.Length - "Template".Length)
                       : value;
        }

        private CutsceneActorObject ResolveCutsceneActor(CutsceneTimelineEvent evt)
        {
            if (this._CutscenePreview == null || this._CutscenePreview.Actors == null || evt == null)
            {
                return null;
            }

            if (evt.ActorHash != 0)
            {
                var byHash = this._CutscenePreview.Actors.FirstOrDefault(a => a.ActorHash == evt.ActorHash);
                if (byHash != null)
                {
                    return byHash;
                }
            }

            var actorName = evt.ActorName;
            if (string.IsNullOrEmpty(actorName) == true)
            {
                return null;
            }

            var normalizedActor = NormalizeCutsceneName(actorName);
            return this._CutscenePreview.Actors.FirstOrDefault(a =>
                ContainsNormalized(a.LongName, normalizedActor) ||
                ContainsNormalized(a.ShortName, normalizedActor) ||
                ContainsNormalized(a.TypeName, normalizedActor) ||
                (a.Strings != null && a.Strings.Any(s => ContainsNormalized(s, normalizedActor))));
        }

        private static bool NamesMatch(string left, string right)
        {
            return string.Equals(left, right, StringComparison.OrdinalIgnoreCase) == true ||
                   NormalizeCutsceneName(left) == NormalizeCutsceneName(right);
        }

        private static bool ContainsNormalized(string value, string normalizedNeedle)
        {
            if (string.IsNullOrEmpty(value) == true || string.IsNullOrEmpty(normalizedNeedle) == true)
            {
                return false;
            }

            return NormalizeCutsceneName(value).Contains(normalizedNeedle);
        }

        private static string NormalizeCutsceneName(string value)
        {
            if (string.IsNullOrEmpty(value) == true)
            {
                return string.Empty;
            }

            var builder = new System.Text.StringBuilder(value.Length);
            foreach (var c in value)
            {
                if (char.IsLetterOrDigit(c) == true)
                {
                    builder.Append(char.ToLowerInvariant(c));
                }
            }

            return builder.ToString();
        }

        private void OnFileSave(object sender, EventArgs e)
        {
            //Do nothing for now

            if (this.ActiveFile == null ||
                this.LastFileName == null)
            {
                return;
            }

            using (var output = File.Create(this.LastFileName))
            {
                this.ActiveFile.Serialize(output);
            }

            /*
            this.UpdateNodeTree();
            this.SelectNothing();
            */
        }

        private void LoadPure3DFiles(IEnumerable<string> fileNames)
        {
            var names = fileNames == null
                            ? new List<string>()
                            : fileNames.Where(File.Exists)
                                       .Where(f => string.Equals(Path.GetExtension(f), ".p3d", StringComparison.OrdinalIgnoreCase))
                                       .Select(Path.GetFullPath)
                                       .Distinct(StringComparer.OrdinalIgnoreCase)
                                       .ToList();
            if (names.Count == 0)
            {
                return;
            }

            this.LeaveCutscenePreviewMode();

            Pure3DFile activeFile;
            this._NodeSourceFiles.Clear();
            if (names.Count == 1)
            {
                activeFile = LoadPure3DFile(names[0]);
                this.MapNodeSourceFile(activeFile.Nodes, names[0]);
                this.LastFileName = names[0];
            }
            else
            {
                activeFile = new Pure3DFile();
                foreach (var name in names)
                {
                    var file = LoadPure3DFile(name);
                    this.MapNodeSourceFile(file.Nodes, name);
                    if (activeFile.Nodes.Count == 0)
                    {
                        activeFile.Endian = file.Endian;
                    }

                    activeFile.Nodes.AddRange(file.Nodes);
                }

                this.LastFileName = null;
            }

            this.LoadedFileNames = names.ToArray();
            this.ActiveFile = activeFile;
            this.ResolveVertexBufferItemBoneNames(activeFile, names);
            this.saveFileButton.Enabled = this.LastFileName != null;
            this.exportAllAnimationsButton.Enabled = true;
            this.UpdateNodeTree();
            this.SelectNothing();
            this.Text = names.Count == 1
                            ? "Edit3D - " + Path.GetFileName(names[0])
                            : "Edit3D - " + names.Count + " files";
        }

        private void AddPure3DFiles(IEnumerable<string> fileNames)
        {
            if (this.ActiveFile == null)
            {
                this.LoadPure3DFiles(fileNames);
                return;
            }

            var loaded = this.LoadedFileNames == null
                             ? new List<string>()
                             : this.LoadedFileNames.Select(Path.GetFullPath).ToList();
            var names = fileNames == null
                            ? new List<string>()
                            : fileNames.Where(File.Exists)
                                       .Where(f => string.Equals(Path.GetExtension(f), ".p3d", StringComparison.OrdinalIgnoreCase))
                                       .Select(Path.GetFullPath)
                                       .Where(f => loaded.Contains(f, StringComparer.OrdinalIgnoreCase) == false)
                                       .Distinct(StringComparer.OrdinalIgnoreCase)
                                       .ToList();
            if (names.Count == 0)
            {
                return;
            }

            foreach (var name in names)
            {
                var file = LoadPure3DFile(name);
                this.MapNodeSourceFile(file.Nodes, name);
                this.ActiveFile.Nodes.AddRange(file.Nodes);
                loaded.Add(name);
            }

            this.LoadedFileNames = loaded.ToArray();
            this.LastFileName = null;
            this.ResolveVertexBufferItemBoneNames(this.ActiveFile, this.LoadedFileNames);
            this.saveFileButton.Enabled = false;
            this.exportAllAnimationsButton.Enabled = true;
            this.UpdateNodeTree();
            this.SelectNothing();
            this.Text = "Edit3D - " + this.LoadedFileNames.Length + " files";
        }

        private static Pure3DFile LoadPure3DFile(string fileName)
        {
            using (var input = File.OpenRead(fileName))
            {
                var pure3D = new Pure3DFile();
                pure3D.Deserialize(input);
                return pure3D;
            }
        }

        private void MapNodeSourceFile(IEnumerable<FileFormats.Pure3D.BaseNode> nodes, string fileName)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (var node in nodes)
            {
                this._NodeSourceFiles[node] = fileName;
                this.MapNodeSourceFile(node.Children, fileName);
            }
        }

        private void UpdateNode(FileFormats.Pure3D.BaseNode node, TreeNodeCollection parent)
        {
            this.UpdateNode(node, parent, null, false);
        }

        private bool UpdateNode(FileFormats.Pure3D.BaseNode node, TreeNodeCollection parent, string filter, bool ancestorMatched)
        {
            bool selfMatched = string.IsNullOrEmpty(filter) == true || ancestorMatched == true || NodeMatchesFilter(node, filter) == true;
            var treeNode = new TreeNode
            {
                Text = node.ToString(),
                Tag = node,
            };

            foreach (var child in node.Children)
            {
                this.UpdateNode(child, treeNode.Nodes, filter, selfMatched);
            }

            var vertexBuffer = node as VertexBuffer;
            if (vertexBuffer != null && selfMatched == true)
            {
                this.UpdateVertexBufferItems(vertexBuffer, treeNode.Nodes);
            }

            var fightData = node as FileFormats.Pure3D.FightData;
            if (fightData != null && selfMatched == true)
            {
                this.UpdateFightTrackItems(fightData, treeNode.Nodes);
            }

            if (selfMatched == true || treeNode.Nodes.Count > 0)
            {
                parent.Add(treeNode);
                return true;
            }

            return false;
        }

        private void UpdateVertexBufferItems(VertexBuffer vertexBuffer, TreeNodeCollection parent)
        {
            vertexBuffer.ResolveDescription();
            if (vertexBuffer.BufferItems == null ||
                vertexBuffer.Description == null ||
                vertexBuffer.Description.Descriptions == null ||
                vertexBuffer.Description.Descriptions.Length == 0)
            {
                return;
            }

            var verticesNode = new TreeNode
            {
                Text = "Vertices (" + vertexBuffer.VertexCount + ")",
                Tag = new VertexBufferItemList(vertexBuffer),
            };
            verticesNode.Nodes.Add(new TreeNode());
            parent.Add(verticesNode);
        }

        private void UpdateFightTrackItems(FileFormats.Pure3D.FightData fightData, TreeNodeCollection parent)
        {
            if (fightData == null || fightData.Data == null || fightData.Data.Length == 0)
            {
                return;
            }

            var tracksNode = new TreeNode
            {
                Text = "Fight Tracks",
                Tag = new FightTrackItemList(fightData),
            };
            tracksNode.Nodes.Add(new TreeNode());
            parent.Add(tracksNode);
        }

        private void OnNodeBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null)
            {
                return;
            }

            var itemList = e.Node.Tag as VertexBufferItemList;
            if (itemList != null)
            {
                PopulateVertexListNode(e.Node, itemList);
                return;
            }

            var vertexItems = e.Node.Tag as VertexBufferVertexItems;
            if (vertexItems != null)
            {
                PopulateVertexItemsNode(e.Node, vertexItems);
                return;
            }

            var fightTrackList = e.Node.Tag as FightTrackItemList;
            if (fightTrackList != null)
            {
                PopulateFightTrackListNode(e.Node, fightTrackList);
            }
        }

        private static void PopulateVertexListNode(TreeNode node, VertexBufferItemList itemList)
        {
            if (node.Nodes.Count != 1 || node.Nodes[0].Tag != null)
            {
                return;
            }

            node.Nodes.Clear();
            int vertexCount = (int)itemList.VertexBuffer.VertexCount;
            for (int vertex = 0; vertex < vertexCount; vertex++)
            {
                var vertexNode = new TreeNode
                {
                    Text = "Vertex " + vertex,
                    Tag = new VertexBufferVertexItems(itemList.VertexBuffer, vertex),
                };
                vertexNode.Nodes.Add(new TreeNode());
                node.Nodes.Add(vertexNode);
            }
        }

        private static void PopulateVertexItemsNode(TreeNode node, VertexBufferVertexItems vertexItems)
        {
            if (node.Nodes.Count != 1 || node.Nodes[0].Tag != null)
            {
                return;
            }

            node.Nodes.Clear();
            var vertexBuffer = vertexItems.VertexBuffer;
            vertexBuffer.ResolveDescription();
            if (vertexBuffer.BufferItems == null ||
                vertexBuffer.Description == null ||
                vertexBuffer.Description.Descriptions == null)
            {
                return;
            }

            int elementCount = vertexBuffer.Description.Descriptions.Length;
            for (int element = 0; element < elementCount; element++)
            {
                int index = vertexItems.VertexIndex * elementCount + element;
                if (index < 0 || index >= vertexBuffer.BufferItems.Length)
                {
                    continue;
                }

                var item = vertexBuffer.BufferItems[index];
                node.Nodes.Add(new TreeNode
                {
                    Text = item.ToString(),
                    Tag = item,
                });
            }
        }

        private static void PopulateFightTrackListNode(TreeNode node, FightTrackItemList itemList)
        {
            if (node.Nodes.Count != 1 || node.Nodes[0].Tag != null)
            {
                return;
            }

            node.Nodes.Clear();
            var tracks = ScanFightTracks(itemList.FightData);
            node.Text = "Fight Tracks (" + tracks.Count + ")";
            if (tracks.Count == 0)
            {
                node.Nodes.Add(new TreeNode
                {
                    Text = "No known fight tracks parsed",
                    Tag = itemList,
                });
                return;
            }

            foreach (var track in tracks)
            {
                node.Nodes.Add(new TreeNode
                {
                    Text = GetFightTrackDisplayName(track),
                    Tag = track,
                });
            }
        }

        private static List<FightTrackTreeItem> ScanFightTracks(FileFormats.Pure3D.FightData fightData)
        {
            var tracks = new List<FightTrackTreeItem>();
            if (fightData == null || fightData.Data == null || fightData.Data.Length < 16)
            {
                return tracks;
            }

            var data = fightData.Data;
            var usedOffsets = new HashSet<int>();
            for (int offset = 0; offset <= data.Length - 16; offset += 4)
            {
                ulong hash = BitConverter.ToUInt64(data, offset);
                if (Nixson.Prototype.Fight.Factory<Nixson.Prototype.Fight.BaseTrack, Nixson.Prototype.Fight.KnownTrackAttribute>.GetType(Nixson.Common.PrototypeGame.P1, hash) == null)
                {
                    continue;
                }

                using (var input = new MemoryStream(data, false))
                {
                    input.Position = offset + 8;
                    try
                    {
                        var track = Nixson.Prototype.Fight.BaseTrack.DeserializeBaseTrack(
                            Nixson.Common.PrototypeGame.P1,
                            input,
                            Endian.Little,
                            hash);
                        if (track != null && input.Position > offset + 12 && usedOffsets.Add(offset) == true)
                        {
                            tracks.Add(new FightTrackTreeItem(fightData, track, offset, (int)input.Position));
                        }
                    }
                    catch
                    {
                        // Fight files contain hashes for many object kinds. A known track hash is only accepted
                        // when the complete track payload deserializes cleanly at this offset.
                    }
                }
            }

            return tracks;
        }

        private static string GetFightTrackDisplayName(FightTrackTreeItem item)
        {
            return item == null ? "Track" : GetFightTrackDisplayName(item.Track);
        }

        private static string GetFightTrackDisplayName(Nixson.Prototype.Fight.BaseTrack track)
        {
            if (track == null)
            {
                return "Track";
            }

            var name = track.GetType().Name;
            if (name.EndsWith("Track", StringComparison.OrdinalIgnoreCase) == true && name.Length > 5)
            {
                name = name.Substring(0, name.Length - 5);
            }

            var begin = GetFightTrackProperty(track, "TimeBegin");
            var end = GetFightTrackProperty(track, "TimeEnd");
            var details = new List<string>();
            if (begin != null || end != null)
            {
                details.Add(string.Format("{0:0.###}-{1:0.###}", begin ?? 0.0f, end ?? 0.0f));
            }

            var actor = GetFightTrackProperty(track, "ActorName") ??
                        GetFightTrackProperty(track, "ActorHash") ??
                        GetFightTrackProperty(track, "ObjectName");
            if (actor != null)
            {
                details.Add("actor=" + FormatFightTrackValue(actor));
            }

            var animation = GetFightTrackProperty(track, "AnimationName") ??
                            GetFightTrackProperty(track, "AnimationHash") ??
                            GetFightTrackProperty(track, "AnimHash");
            if (animation != null)
            {
                details.Add("anim=" + FormatFightTrackValue(animation));
            }

            return details.Count == 0 ? name : name + " [" + string.Join("; ", details.ToArray()) + "]";
        }

        private static object GetFightTrackProperty(object value, string propertyName)
        {
            if (value == null)
            {
                return null;
            }

            var property = value.GetType().GetProperty(propertyName);
            return property == null ? null : property.GetValue(value, null);
        }

        private static string FormatFightTrackValue(object value)
        {
            if (value is ulong)
            {
                return ((ulong)value).ToString("X16");
            }

            if (value is uint)
            {
                return ((uint)value).ToString("X8");
            }

            return Convert.ToString(value);
        }

        private void UpdateNodeTree()
        {
            this.nodeView.BeginUpdate();
            this.nodeView.Nodes.Clear();
            this._PreviewSelectionAnchorNode = null;

            var root = new TreeNode
            {
                Text = "Root",
            };

            var categories = new Dictionary<string, TreeNode>();
            var filter = this.nodeFilterTextBox == null ? null : (this.nodeFilterTextBox.Text ?? string.Empty).Trim();
            foreach (var node in this.ActiveFile.Nodes)
            {
                var categoryName = GetNodeCategory(node);
                TreeNode categoryNode;
                if (categories.TryGetValue(categoryName, out categoryNode) == false)
                {
                    categoryNode = new TreeNode
                    {
                        Text = categoryName,
                    };
                    categories.Add(categoryName, categoryNode);
                    root.Nodes.Add(categoryNode);
                }

                this.UpdateNode(node, categoryNode.Nodes, filter, false);
            }

            foreach (var categoryNode in categories.Values.ToList())
            {
                if (categoryNode.Nodes.Count == 0)
                {
                    root.Nodes.Remove(categoryNode);
                    continue;
                }

                categoryNode.Text += " (" + categoryNode.Nodes.Count + ")";
            }

            this.nodeView.Nodes.Add(root);
            root.Expand();
            foreach (TreeNode categoryNode in root.Nodes)
            {
                categoryNode.Expand();
            }
            this.nodeView.EndUpdate();
        }

        private static bool NodeMatchesFilter(FileFormats.Pure3D.BaseNode node, string filter)
        {
            if (node == null || string.IsNullOrEmpty(filter) == true)
            {
                return true;
            }

            if ((node.ToString() ?? string.Empty).IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            var property = node.GetType().GetProperty("Name");
            if (property != null)
            {
                var value = property.GetValue(node, null) as string;
                if (string.IsNullOrEmpty(value) == false && value.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            property = node.GetType().GetProperty("PolySkinName");
            if (property != null)
            {
                var value = property.GetValue(node, null) as string;
                if (string.IsNullOrEmpty(value) == false && value.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnNodeFilterTextChanged(object sender, EventArgs e)
        {
            if (this.ActiveFile == null)
            {
                return;
            }

            this.UpdateNodeTree();
        }

        private static string GetNodeCategory(FileFormats.Pure3D.BaseNode node)
        {
            if (node is Texture || node is TextureDDS || node is TexturePNG || node is TextureData)
            {
                return "Textures";
            }

            if (node is NewShader || node.GetType().Name.IndexOf("Shader", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Shaders";
            }

            if (node is Animation || node.GetType().Name.IndexOf("Animation", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Animations";
            }

            if (node is Skeleton || node.GetType().Name.IndexOf("Skeleton", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Skeletons";
            }

            if (node is PolySkin)
            {
                return "PolySkins";
            }

            if (node is P2PolySkin)
            {
                return "Prototype 2 PolySkins";
            }

            if (node is P2Primitive)
            {
                return "Prototype 2 Primitives";
            }

            if (node is Geometry)
            {
                return "Geometry";
            }

            if (node is CompositeDrawable || node is P2PolySkinComposite)
            {
                return "Composite Drawables";
            }

            if (node is FightDefinition || node is FileFormats.Pure3D.FightData)
            {
                return "Fight Data";
            }         

            if (node.GetType().Name.IndexOf("Physics", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Physics";
            }

            return "Other";
        }

        private void SelectNothing()
        {
            this.propertyGrid.SelectedObject = null;
            this._ModelPreviewScene = null;
            this._ModelPreviewCamera = null;
            this._ModelPreviewAnimation = null;
            this._ModelPreviewAnimationClock = null;
            this._PreviewRenderContinuous = false;
            this.ClearPreviewMultiSelection();
            this._ModelPreviewAnimationQueue.Clear();
            this._ModelPreviewAnimationQueueIndex = 0;
            this._PreviewRootMotionOffset = new Vec3();
            this._PreviewAnimationPausedFrame = null;
            this._PreviewLoopSelection = false;
            this._PreviewActiveModelNode = null;
            this.UpdatePreviewOverlayText();
            this._ModelPreviewOpenGl.SetPreview(null, null);
            this._ModelPreviewOpenGl.Visible = false;
            this.previewPicture.Visible = true;
            this.previewPicture.Image = this.previewPicture.InitialImage;
            this.importNodeButton.Enabled = false;
            this.exportNodeButton.Enabled = false;
            this.exportAllAnimationsButton.Enabled = this.ActiveFile != null;
            this.saveFileButton.Enabled = this.LastFileName != null;
        }

        private void SelectNode(FileFormats.Pure3D.BaseNode node)
        {
            this.propertyGrid.SelectedObject = node;

            object preview;
            if (node is PolySkin && this.ActiveFile != null)
            {
                this._PreviewActiveModelNode = node;
                this._ModelPreviewScene = ModelPreviewBuilder.CreateScene(this.ActiveFile, (PolySkin)node, this.LastFileName);
                this.EnablePreviewGpuSkinning();
                this._ModelPreviewCamera = ModelPreviewBuilder.CreateDefaultCamera(this._ModelPreviewScene);
                this._ModelPreviewAnimation = null;
                this._ModelPreviewAnimationClock = null;
                this._ModelPreviewAnimationQueue.Clear();
                this._ModelPreviewAnimationQueueIndex = 0;
                this._PreviewRootMotionOffset = new Vec3();
                this._PreviewAnimationPausedFrame = null;
                this.StartVertexAnimationPreviewIfNeeded();
                preview = null;
                this.ShowModelPreview();
            }
            else if (node is Geometry && this.ActiveFile != null)
            {
                this._PreviewActiveModelNode = node;
                this._ModelPreviewScene = ModelPreviewBuilder.CreateScene(this.ActiveFile, (Geometry)node);
                this.EnablePreviewGpuSkinning();
                this._ModelPreviewCamera = ModelPreviewBuilder.CreateDefaultCamera(this._ModelPreviewScene);
                this._ModelPreviewAnimation = null;
                this._ModelPreviewAnimationClock = null;
                this._ModelPreviewAnimationQueue.Clear();
                this._ModelPreviewAnimationQueueIndex = 0;
                this._PreviewRootMotionOffset = new Vec3();
                this._PreviewAnimationPausedFrame = null;
                this.StartVertexAnimationPreviewIfNeeded();
                preview = null;
                this.ShowModelPreview();
            }
            else if (node is CompositeDrawable && this.ActiveFile != null)
            {
                this._PreviewActiveModelNode = node;
                this._ModelPreviewScene = ModelPreviewBuilder.CreateScene(this.ActiveFile, (CompositeDrawable)node, this.LastFileName);
                this.EnablePreviewGpuSkinning();
                this._ModelPreviewCamera = ModelPreviewBuilder.CreateDefaultCamera(this._ModelPreviewScene);
                this._ModelPreviewAnimation = null;
                this._ModelPreviewAnimationClock = null;
                this._ModelPreviewAnimationQueue.Clear();
                this._ModelPreviewAnimationQueueIndex = 0;
                this._PreviewRootMotionOffset = new Vec3();
                this._PreviewAnimationPausedFrame = null;
                this.StartVertexAnimationPreviewIfNeeded();
                preview = null;
                this.ShowModelPreview();
            }
            else if (node is P2PolySkinComposite && this.ActiveFile != null)
            {
                this._PreviewActiveModelNode = node;
                this._ModelPreviewScene = ModelPreviewBuilder.CreateScene(this.ActiveFile, (P2PolySkinComposite)node, this.LastFileName);
                this.EnablePreviewGpuSkinning();
                this._ModelPreviewCamera = ModelPreviewBuilder.CreateDefaultCamera(this._ModelPreviewScene);
                this._ModelPreviewCamera.ShowSkeleton = true;
                this._ModelPreviewAnimation = null;
                this._ModelPreviewAnimationClock = null;
                this._ModelPreviewAnimationQueue.Clear();
                this._ModelPreviewAnimationQueueIndex = 0;
                this._PreviewRootMotionOffset = new Vec3();
                this._PreviewAnimationPausedFrame = null;
                this.StartVertexAnimationPreviewIfNeeded();
                preview = null;
                this.ShowModelPreview();
            }
            else if (node is P2PolySkin && this.ActiveFile != null)
            {
                this._PreviewActiveModelNode = node;
                this._ModelPreviewScene = ModelPreviewBuilder.CreateScene(this.ActiveFile, (P2PolySkin)node, this.LastFileName);
                this.EnablePreviewGpuSkinning();
                this._ModelPreviewCamera = ModelPreviewBuilder.CreateDefaultCamera(this._ModelPreviewScene);
                this._ModelPreviewAnimation = null;
                this._ModelPreviewAnimationClock = null;
                this._ModelPreviewAnimationQueue.Clear();
                this._ModelPreviewAnimationQueueIndex = 0;
                this._PreviewRootMotionOffset = new Vec3();
                this._PreviewAnimationPausedFrame = null;
                this.StartVertexAnimationPreviewIfNeeded();
                preview = null;
                this.ShowModelPreview();
            }
            else if (node is P2Primitive && this.ActiveFile != null)
            {
                this._PreviewActiveModelNode = node;
                this._ModelPreviewScene = ModelPreviewBuilder.CreateScene(this.ActiveFile, (P2Primitive)node);
                this.EnablePreviewGpuSkinning();
                this._ModelPreviewCamera = ModelPreviewBuilder.CreateDefaultCamera(this._ModelPreviewScene);
                this._ModelPreviewAnimation = null;
                this._ModelPreviewAnimationClock = null;
                this._ModelPreviewAnimationQueue.Clear();
                this._ModelPreviewAnimationQueueIndex = 0;
                this._PreviewRootMotionOffset = new Vec3();
                this._PreviewAnimationPausedFrame = null;
                this.StartVertexAnimationPreviewIfNeeded();
                preview = null;
                this.ShowModelPreview();
            }
            else if (node is Animation && this._ModelPreviewScene != null)
            {
                var animation = ModelPreviewBuilder.CreateAnimation(this._ModelPreviewScene, (Animation)node);
                if (animation != null)
                {
                    this._ModelPreviewAnimation = animation;
                    this._ModelPreviewAnimationQueue.Clear();
                    this._ModelPreviewAnimationQueue.Add(animation);
                    this._ModelPreviewAnimationQueueIndex = 0;
                    this._PreviewRootMotionOffset = new Vec3();
                    this._PreviewAnimationPausedFrame = null;
                    this._ModelPreviewAnimationClock = Stopwatch.StartNew();
                    this.ConfigurePreviewAnimationPlayback();
                    this.UpdateModelPreviewAnimation();
                    this.RefreshModelPreview(true);
                }
                preview = null;
            }
            else if (this.IsTextureExportNode(node) == true)
            {
                this._ModelPreviewAnimation = null;
                this._ModelPreviewAnimationClock = null;
                this._ModelPreviewAnimationQueue.Clear();
                this._ModelPreviewAnimationQueueIndex = 0;
                this._PreviewRootMotionOffset = new Vec3();
                this._PreviewAnimationPausedFrame = null;
                this._PreviewRenderContinuous = false;
                this._PreviewActiveModelNode = null;

                preview = node.Preview();
                var image = preview as Image;
                if (image == null)
                {
                    var exportNode = this.ResolveTextureExportNode(node);
                    preview = exportNode == null ? null : exportNode.Preview();
                    image = preview as Image;
                }

                if (image != null)
                {
                    this._ModelPreviewScene = ModelPreviewBuilder.CreateTextureScene(this.GetNodeDisplayName(node, "texture"), image);
                    this._ModelPreviewCamera = ModelPreviewBuilder.CreateDefaultCamera(this._ModelPreviewScene);
                    this._ModelPreviewCamera.Yaw = 0.0f;
                    this._ModelPreviewCamera.Pitch = 0.0f;
                    this._ModelPreviewCamera.Zoom = 1.0f;
                    preview = null;
                    this.ShowModelPreview();
                }
                else
                {
                    this._ModelPreviewScene = null;
                    this._ModelPreviewCamera = null;
                    this._ModelPreviewOpenGl.Visible = false;
                    this.previewPicture.Visible = true;
                }
            }
            else
            {
                this._ModelPreviewAnimation = null;
                this._ModelPreviewAnimationClock = null;
                this._ModelPreviewAnimationQueue.Clear();
                this._ModelPreviewAnimationQueueIndex = 0;
                this._PreviewRootMotionOffset = new Vec3();
                this._PreviewAnimationPausedFrame = null;
                this._PreviewRenderContinuous = false;
                this._ModelPreviewOpenGl.Visible = false;
                this.previewPicture.Visible = true;
                preview = node.Preview();
                if (preview == null && this._ModelPreviewScene == null)
                {
                    this._ModelPreviewCamera = null;
                }
            }

            if (preview is Image)
            {
                this.previewPicture.Image = (Image)preview;
            }
            else if (this._ModelPreviewOpenGl.Visible == false)
            {
                this.previewPicture.Image = null;
            }

            this.importNodeButton.Enabled = node.Importable || node is Animation;
            this.exportNodeButton.Enabled = node.Exportable || this.IsModelExportNode(node);
        }

        private void ClearPreviewMultiSelection()
        {
            foreach (var treeNode in this._PreviewSelectedTreeNodes)
            {
                if (treeNode != null)
                {
                    treeNode.BackColor = Color.Empty;
                    treeNode.ForeColor = Color.Empty;
                }
            }

            this._PreviewSelectedTreeNodes.Clear();
            this._PreviewSelectedModelNodes.Clear();
            this._PreviewSelectedAnimationNodes.Clear();
            this._PreviewSelectionAnchorNode = null;
        }

        private void TogglePreviewMultiSelection(TreeNode treeNode, FileFormats.Pure3D.BaseNode node)
        {
            if (treeNode == null || node == null)
            {
                return;
            }

            bool selected = this._PreviewSelectedTreeNodes.Contains(treeNode);
            if (selected == true)
            {
                treeNode.BackColor = Color.Empty;
                treeNode.ForeColor = Color.Empty;
                this._PreviewSelectedTreeNodes.Remove(treeNode);
                this._PreviewSelectedModelNodes.Remove(node);

                var animationToRemove = node as Animation;
                if (animationToRemove != null)
                {
                    this._PreviewSelectedAnimationNodes.Remove(animationToRemove);
                }
            }
            else
            {
                this.AddPreviewSelection(treeNode, node);
            }

            this._PreviewSelectionAnchorNode = treeNode;
            this.propertyGrid.SelectedObject = node;
            this.importNodeButton.Enabled = node.Importable || node is Animation;
            this.exportNodeButton.Enabled = node.Exportable || this.IsModelExportNode(node);

            if (this._PreviewSelectedModelNodes.Count > 0)
            {
                this._PreviewActiveModelNode = this._PreviewSelectedModelNodes[this._PreviewSelectedModelNodes.Count - 1];
                this.RebuildPreviewSelectionScene();
            }
            else if (this._PreviewSelectedAnimationNodes.Count > 0)
            {
                this.RebuildPreviewAnimationQueue();
            }
            else if (this.IsModelExportNode(node) == true)
            {
                this._ModelPreviewAnimation = null;
                this._ModelPreviewAnimationClock = null;
                this._ModelPreviewAnimationQueue.Clear();
                this._ModelPreviewAnimationQueueIndex = 0;
                this._PreviewRootMotionOffset = new Vec3();
                this._PreviewRenderContinuous = false;
            }
        }

        private void AddPreviewSelection(TreeNode treeNode, FileFormats.Pure3D.BaseNode node)
        {
            if (treeNode == null || node == null || this._PreviewSelectedTreeNodes.Contains(treeNode) == true)
            {
                return;
            }

            treeNode.BackColor = Color.LightSteelBlue;
            treeNode.ForeColor = Color.Black;
            this._PreviewSelectedTreeNodes.Add(treeNode);

            if (this.IsModelExportNode(node) == true && this._PreviewSelectedModelNodes.Contains(node) == false)
            {
                this._PreviewSelectedModelNodes.Add(node);
            }

            var animation = node as Animation;
            if (animation != null && this._PreviewSelectedAnimationNodes.Contains(animation) == false)
            {
                this._PreviewSelectedAnimationNodes.Add(animation);
            }
        }

        private void RebuildPreviewFromMultiSelection(FileFormats.Pure3D.BaseNode selectedNode)
        {
            if (selectedNode != null)
            {
                this.propertyGrid.SelectedObject = selectedNode;
                this.importNodeButton.Enabled = selectedNode.Importable || selectedNode is Animation;
                this.exportNodeButton.Enabled = selectedNode.Exportable || this.IsModelExportNode(selectedNode);
            }

            if (this._PreviewSelectedModelNodes.Count > 0)
            {
                this.RebuildPreviewSelectionScene();
            }
            else if (this._PreviewSelectedAnimationNodes.Count > 0)
            {
                this.RebuildPreviewAnimationQueue();
            }
        }

        private void SelectPreviewRange(TreeNode targetNode, FileFormats.Pure3D.BaseNode targetNodeData, bool addToExisting)
        {
            if (targetNode == null || targetNodeData == null)
            {
                return;
            }

            var visibleNodes = new List<TreeNode>();
            this.CollectVisibleBaseNodeTreeNodes(this.nodeView.Nodes, visibleNodes);
            if (visibleNodes.Count == 0)
            {
                return;
            }

            var anchor = this._PreviewSelectionAnchorNode != null && visibleNodes.Contains(this._PreviewSelectionAnchorNode)
                             ? this._PreviewSelectionAnchorNode
                             : targetNode;
            int start = visibleNodes.IndexOf(anchor);
            int end = visibleNodes.IndexOf(targetNode);
            if (start < 0 || end < 0)
            {
                return;
            }

            if (addToExisting == false)
            {
                this.ClearPreviewMultiSelection();
            }

            if (start > end)
            {
                var swap = start;
                start = end;
                end = swap;
            }

            for (int i = start; i <= end; i++)
            {
                var node = visibleNodes[i].Tag as FileFormats.Pure3D.BaseNode;
                if (node != null)
                {
                    this.AddPreviewSelection(visibleNodes[i], node);
                }
            }

            this._PreviewSelectionAnchorNode = anchor;
            this.RebuildPreviewFromMultiSelection(targetNodeData);
        }

        private void CollectVisibleBaseNodeTreeNodes(TreeNodeCollection nodes, List<TreeNode> result)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag is FileFormats.Pure3D.BaseNode)
                {
                    result.Add(node);
                }

                if (node.IsExpanded == true && node.Nodes.Count > 0)
                {
                    this.CollectVisibleBaseNodeTreeNodes(node.Nodes, result);
                }
            }
        }

        private void RebuildPreviewSelectionScene()
        {
            if (this.ActiveFile == null || this._PreviewSelectedModelNodes.Count == 0)
            {
                return;
            }

            this._ModelPreviewScene = ModelPreviewBuilder.CreateScene(this.ActiveFile, this._PreviewSelectedModelNodes, this.LastFileName);
            this.EnablePreviewGpuSkinning();
            this._ModelPreviewCamera = ModelPreviewBuilder.CreateDefaultCamera(this._ModelPreviewScene);
            this._ModelPreviewAnimation = null;
            this._ModelPreviewAnimationClock = null;
            this._ModelPreviewAnimationQueue.Clear();
            this._ModelPreviewAnimationQueueIndex = 0;
            this._PreviewRootMotionOffset = new Vec3();
            this._PreviewAnimationPausedFrame = null;
            this.StartVertexAnimationPreviewIfNeeded();

            if (this._ModelPreviewScene != null)
            {
                this.ShowModelPreview();
            }

            this.RebuildPreviewAnimationQueue();
        }

        private void EnablePreviewGpuSkinning()
        {
            if (this._ModelPreviewScene != null)
            {
                this._ModelPreviewScene.UseGpuSkinning = true;
            }
        }

        private void RebuildPreviewAnimationQueue()
        {
            this._ModelPreviewAnimationQueue.Clear();
            this._ModelPreviewAnimationQueueIndex = 0;
            this._PreviewRootMotionOffset = new Vec3();
            this._ModelPreviewAnimation = null;
            this._ModelPreviewAnimationClock = null;
            this._PreviewAnimationPausedFrame = null;
            this._PreviewRenderContinuous = false;

            if (this._ModelPreviewScene == null || this._PreviewSelectedAnimationNodes.Count == 0)
            {
                this.StartVertexAnimationPreviewIfNeeded();
                this.RefreshModelPreview(true);
                return;
            }

            foreach (var animationNode in this._PreviewSelectedAnimationNodes)
            {
                var animation = ModelPreviewBuilder.CreateAnimation(this._ModelPreviewScene, animationNode);
                if (animation != null)
                {
                    this._ModelPreviewAnimationQueue.Add(animation);
                }
            }

            if (this._ModelPreviewAnimationQueue.Count == 0)
            {
                this.RefreshModelPreview(true);
                return;
            }

            this._ModelPreviewAnimation = this._ModelPreviewAnimationQueue[0];
            this._ModelPreviewAnimationClock = Stopwatch.StartNew();
            this.ConfigurePreviewAnimationPlayback();

            this.UpdateModelPreviewAnimation();
            this.RefreshModelPreview(true);
        }

        private void ShowModelPreview()
        {
            this.previewPicture.Image = null;
            this.previewPicture.Visible = false;
            this._ModelPreviewOpenGl.Visible = true;
            this._ModelPreviewOpenGl.Bounds = this.splitContainer2.Panel1.ClientRectangle;
            this.UpdatePreviewOverlayText();
            this._ModelPreviewOpenGl.SetPreview(this._ModelPreviewScene, this._ModelPreviewCamera);
            this._ModelPreviewOpenGl.RenderPreview();
        }

        private void RefreshModelPreview()
        {
            this.RefreshModelPreview(false);
        }

        private void RefreshModelPreview(bool fast)
        {
            if (this._ModelPreviewScene == null || this._ModelPreviewCamera == null)
            {
                return;
            }

            this.UpdatePreviewOverlayText();
            this._ModelPreviewOpenGl.SetPreview(this._ModelPreviewScene, this._ModelPreviewCamera);
            this._PreviewRenderFast = this._PreviewRenderQueued ? this._PreviewRenderFast && fast : fast;
            this._PreviewRenderQueued = true;
        }

        private void RefreshModelPreviewNow(bool fast)
        {
            if (this._ModelPreviewScene == null || this._ModelPreviewCamera == null)
            {
                return;
            }

            this.UpdatePreviewOverlayText();
            this._ModelPreviewOpenGl.SetPreview(this._ModelPreviewScene, this._ModelPreviewCamera);
            this._ModelPreviewOpenGl.RenderPreview();
        }

        private void OnPreviewRenderTimerTick(object sender, EventArgs e)
        {
            if (this._PreviewRendering == true)
            {
                return;
            }

            if (this._PreviewRenderContinuous == true)
            {
                this.RefreshModelPreview(true);
            }

            if (this.UpdateFreeCameraMovement() == true)
            {
                this.RefreshModelPreview(true);
            }

            if (this._PreviewRenderQueued == false)
            {
                return;
            }

            var fast = this._PreviewRenderFast;
            this._PreviewRenderQueued = false;
            this._PreviewRenderFast = false;

            this._PreviewRendering = true;
            try
            {
                this.RefreshModelPreviewNow(fast);
            }
            finally
            {
                this._PreviewRendering = false;
            }
        }

        private void OnPreviewRenderFrameStarting(object sender, EventArgs e)
        {
            if (this._PreviewRenderContinuous == false)
            {
                return;
            }

            if (this._CutscenePlaying == true)
            {
                this.UpdateCutscenePreviewPlayback();
            }
            else
            {
                this.UpdateModelPreviewAnimation();
            }
        }

        private void StartVertexAnimationPreviewIfNeeded()
        {
            if (this._ModelPreviewScene != null &&
                this._ModelPreviewScene.VertexAnimations != null &&
                this._ModelPreviewScene.VertexAnimations.Count > 0)
            {
                this._ModelPreviewAnimationClock = null;
                this._PreviewRenderContinuous = false;
            }
            else
            {
                this._PreviewRenderContinuous = false;
            }
        }

        private bool IsVertexMorphPreviewAnimation(ModelPreviewAnimation animation)
        {
            return animation != null &&
                   animation.UseVertexAnimation == true &&
                   animation.VertexMorphChannels != null &&
                   animation.VertexMorphChannels.Count > 0;
        }

        private void ConfigurePreviewAnimationPlayback()
        {
            if (this.IsVertexMorphPreviewAnimation(this._ModelPreviewAnimation) == true)
            {
                this._PreviewAnimationPausedFrame = 0.0f;
                this._PreviewRenderContinuous = false;
            }
            else
            {
                this._PreviewAnimationPausedFrame = null;
                this._PreviewRenderContinuous = true;
            }
        }

        private void OnVertexMorphScrubRequested(object sender, VertexMorphScrubEventArgs e)
        {
            if (this._ModelPreviewScene == null ||
                this._ModelPreviewAnimation == null ||
                this.IsVertexMorphPreviewAnimation(this._ModelPreviewAnimation) == false)
            {
                return;
            }

            float frame = Math.Max(0.0f, Math.Min(1.0f, e.Value));
            this._PreviewAnimationPausedFrame = frame;
            this._PreviewRenderContinuous = false;
            this._ModelPreviewAnimationClock = Stopwatch.StartNew();
            this.ApplyCurrentPreviewAnimationFrame(frame);
            this.RefreshModelPreview(true);
        }

        private void ApplyCurrentPreviewAnimationFrame(float frame)
        {
            if (this._ModelPreviewScene == null || this._ModelPreviewAnimation == null)
            {
                return;
            }

            Vec3 rootDelta;
            ModelPreviewBuilder.ApplyAnimation(
                this._ModelPreviewScene,
                this._ModelPreviewAnimation,
                this.GetPreviewAnimationSeconds(frame),
                this._PreviewRootMotionEnabled,
                this._PreviewRootMotionEnabled ? this._PreviewRootMotionOffset : new Vec3(),
                out rootDelta);
            if (this._ModelPreviewAnimation.UseVertexAnimation == true)
            {
                ModelPreviewBuilder.ApplyVertexAnimation(
                    this._ModelPreviewScene,
                    this._ModelPreviewAnimation,
                    this.GetPreviewAnimationSeconds(frame));
            }
        }

        private void UpdateModelPreviewAnimation()
        {
            if (this._ModelPreviewScene == null || this._ModelPreviewAnimationClock == null)
            {
                return;
            }

            if (this._ModelPreviewAnimation == null)
            {
                if (this._ModelPreviewScene.VertexAnimations != null && this._ModelPreviewScene.VertexAnimations.Count > 0)
                {
                    ModelPreviewBuilder.ResetAnimation(this._ModelPreviewScene);
                }

                return;
            }

            if (this._PreviewAnimationPausedFrame.HasValue == true)
            {
                this.ApplyCurrentPreviewAnimationFrame(this._PreviewAnimationPausedFrame.Value);
                return;
            }

            Vec3 rootDelta;
            var active = ModelPreviewBuilder.ApplyAnimation(
                this._ModelPreviewScene,
                this._ModelPreviewAnimation,
                (float)this._ModelPreviewAnimationClock.Elapsed.TotalSeconds,
                this._PreviewRootMotionEnabled,
                this._PreviewRootMotionEnabled ? this._PreviewRootMotionOffset : new Vec3(),
                out rootDelta);
            if (this._ModelPreviewAnimation.UseVertexAnimation == true)
            {
                ModelPreviewBuilder.ApplyVertexAnimation(
                    this._ModelPreviewScene,
                    this._ModelPreviewAnimation,
                    (float)this._ModelPreviewAnimationClock.Elapsed.TotalSeconds);
            }
            if (active == false)
            {
                if (this._ModelPreviewAnimationQueueIndex + 1 < this._ModelPreviewAnimationQueue.Count)
                {
                    if (this._PreviewRootMotionEnabled == true)
                    {
                        this._PreviewRootMotionOffset += rootDelta;
                    }

                    this._ModelPreviewAnimationQueueIndex++;
                    this._ModelPreviewAnimation = this._ModelPreviewAnimationQueue[this._ModelPreviewAnimationQueueIndex];
                    this._ModelPreviewAnimationClock = Stopwatch.StartNew();
                    this._PreviewRenderContinuous = true;
                }
                else
                {
                    if (this._PreviewLoopSelection == true && this._ModelPreviewAnimationQueue.Count > 0)
                    {
                        if (this._PreviewRootMotionEnabled == true)
                        {
                            this._PreviewRootMotionOffset += rootDelta;
                        }

                        this._ModelPreviewAnimationQueueIndex = 0;
                        this._ModelPreviewAnimation = this._ModelPreviewAnimationQueue[0];
                        this._ModelPreviewAnimationClock = Stopwatch.StartNew();
                        this._PreviewRenderContinuous = true;
                    }
                    else
                    {
                        this._PreviewRenderContinuous = false;
                    }
                }
            }
        }

        private void UpdateCutscenePreviewPlayback()
        {
            if (this._CutscenePreview == null || this._ModelPreviewScene == null || this._CutscenePreviewClock == null)
            {
                return;
            }

            float durationSeconds = this.GetCutsceneDurationSeconds();
            float seconds = this._CutscenePlaybackStartSeconds + (float)this._CutscenePreviewClock.Elapsed.TotalSeconds;
            if (durationSeconds > 0.0f && seconds >= durationSeconds)
            {
                seconds = durationSeconds;
                this._CutscenePlaying = false;
                this._PreviewRenderContinuous = false;
            }

            this.ApplyCutscenePreviewAtSeconds(seconds);
        }

        private float GetCutsceneDurationSeconds()
        {
            if (this._CutscenePreview == null)
            {
                return 0.0f;
            }

            if (this._CutscenePreview.DurationSeconds > 0.0f)
            {
                return this._CutscenePreview.DurationSeconds;
            }

            var frameRate = this._CutscenePreview.FrameRate <= 0.001f ? 30.0f : this._CutscenePreview.FrameRate;
            return Math.Max(0.0f, this._CutscenePreview.TotalFrames / frameRate);
        }

        private void ApplyCutscenePreviewAtSeconds(float seconds)
        {
            if (this._CutscenePreview == null || this._ModelPreviewScene == null)
            {
                return;
            }

            float frameRate = this._CutscenePreview.FrameRate <= 0.001f ? 30.0f : this._CutscenePreview.FrameRate;
            float durationSeconds = this.GetCutsceneDurationSeconds();
            int totalFrames = Math.Max(1, this._CutscenePreview.TotalFrames);
            if (durationSeconds > 0.0f)
            {
                seconds = Math.Max(0.0f, Math.Min(durationSeconds, seconds));
            }
            else
            {
                seconds = Math.Max(0.0f, seconds);
            }

            int frame = Math.Max(0, Math.Min(totalFrames - 1, (int)Math.Floor(seconds * frameRate)));
            if (this._CutsceneEvaluationDirty == false && this._LastCutsceneEvaluatedFrame == frame)
            {
                return;
            }
            this._LastCutsceneEvaluatedFrame = frame;
            this._CutsceneEvaluationDirty = false;

            this.UpdateCutsceneTimelineOverlay(this._CutscenePreview, seconds);
            this.ApplyCutsceneActorTransforms();
            this.ApplyCutscenePropTransforms();

            var activeEvents = this._CutscenePlaybackEvents
                .Where(e => IsCutsceneTimelineEventActive(e.TimelineEvent, seconds, durationSeconds))
                .OrderBy(e => e.Instance == null ? 1 : 0)
                .ThenBy(e => e.TimelineEvent.ActorName)
                .ThenBy(e => e.TimelineEvent.StartSeconds)
                .ToList();
            var poseEvents = this.GetCutscenePoseEvents(seconds);

            if (this._CutscenePreview.Cameras.Count > 0)
            {
                var baseCamera = this._CutscenePreview.Cameras[0];
                this._ModelPreviewCamera.HasCutsceneCamera = true;
                this._ModelPreviewCamera.CutscenePosition = baseCamera.Position;
                this._ModelPreviewCamera.CutsceneLook = baseCamera.Look;
                this._ModelPreviewCamera.CutsceneUp = baseCamera.Up;
                this._ModelPreviewCamera.CutsceneFov = ModelPreviewBuilder.NormalizeFovDegrees(baseCamera.Fov);
                this._ModelPreviewCamera.CutsceneNearClip = baseCamera.NearClip;
                this._ModelPreviewCamera.CutsceneFarClip = baseCamera.FarClip;
            }

            this.ApplyCutsceneControlTracks(seconds, durationSeconds);

            ModelPreviewBuilder.BeginCutsceneAnimationFrame(this._ModelPreviewScene);
            foreach (var active in poseEvents)
            {
                var localSeconds = Math.Max(0.0f, Math.Min(active.TimelineEvent.EndSeconds, seconds) - active.TimelineEvent.StartSeconds);
                var localFrame = this.GetCutsceneEventAnimationFrame(active.TimelineEvent, active.Animation, localSeconds);
                Vec3 rootDelta;
                ModelPreviewBuilder.ApplyCutsceneAnimationFrameToCurrentPose(
                    this._ModelPreviewScene,
                    active.Animation,
                    localFrame,
                    this._PreviewRootMotionEnabled,
                    this._PreviewRootMotionEnabled ? this._PreviewRootMotionOffset : new Vec3(),
                    this._CutscenePreview == null,
                    out rootDelta);
                if (active.Animation.UseVertexAnimation == true)
                {
                    ModelPreviewBuilder.ApplyVertexAnimation(this._ModelPreviewScene, active.Animation, localSeconds);
                }
            }

            foreach (var active in activeEvents.Where(e => e.CameraAnimation != null))
            {
                var localSeconds = Math.Max(0.0f, seconds - active.TimelineEvent.StartSeconds);
                var localFrame = this.GetCutsceneEventCameraFrame(active.TimelineEvent, active.CameraAnimation, localSeconds);
                ModelPreviewBuilder.ApplyCameraAnimationFrame(this._ModelPreviewCamera, active.CameraAnimation, localFrame);
            }
            ModelPreviewBuilder.FinishCutsceneAnimationFrame(
                this._ModelPreviewScene,
                poseEvents.Select(e => e.Instance).Where(i => i != null));

            if (this._CutsceneTimelineTrackBar.Enabled == true)
            {
                this._UpdatingCutsceneTrackBar = true;
                var tick = (int)Math.Round(seconds * CutsceneTimelineTicksPerSecond);
                this._CutsceneTimelineTrackBar.Value = Math.Max(this._CutsceneTimelineTrackBar.Minimum, Math.Min(this._CutsceneTimelineTrackBar.Maximum, tick));
                this._UpdatingCutsceneTrackBar = false;
            }

            if (this._ModelPreviewOpenGl != null)
            {
                var status = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Cutscene {0:0.###}s/{1:0.###}s (frame {2}/{3})", seconds, durationSeconds, frame + 1, totalFrames);
                var active = activeEvents.Where(e => e.Instance != null || e.CameraAnimation != null).Take(3).ToList();
                if (active.Count > 0)
                {
                    status += " - " + string.Join(", ", active.Select(e => FormatCutsceneActiveEvent(e)).ToArray());
                }

                this._ModelPreviewOpenGl.CutsceneStatus = status;
            }
        }

        private List<CutscenePlaybackEvent> GetCutscenePoseEvents(float seconds)
        {
            var latestByInstance = new Dictionary<ModelPreviewSceneInstance, CutscenePlaybackEvent>();
            foreach (var playbackEvent in this._CutscenePlaybackEvents)
            {
                if (playbackEvent.Animation == null ||
                    playbackEvent.Instance == null ||
                    playbackEvent.TimelineEvent.StartSeconds > seconds)
                {
                    continue;
                }

                CutscenePlaybackEvent current;
                if (latestByInstance.TryGetValue(playbackEvent.Instance, out current) == false ||
                    IsCutscenePoseEventLater(playbackEvent, current) == true)
                {
                    latestByInstance[playbackEvent.Instance] = playbackEvent;
                }
            }

            return latestByInstance.Values
                .OrderBy(e => e.Instance == null ? 1 : 0)
                .ThenBy(e => e.TimelineEvent.ActorName)
                .ThenBy(e => e.TimelineEvent.StartSeconds)
                .ThenBy(e => e.TimelineEvent.EndSeconds)
                .ThenBy(e => e.TimelineEvent.AnimationName)
                .ToList();
        }

        private static bool IsCutscenePoseEventLater(CutscenePlaybackEvent candidate, CutscenePlaybackEvent current)
        {
            if (candidate.TimelineEvent.StartSeconds > current.TimelineEvent.StartSeconds)
            {
                return true;
            }

            if (candidate.TimelineEvent.StartSeconds < current.TimelineEvent.StartSeconds)
            {
                return false;
            }

            if (candidate.TimelineEvent.EndSeconds > current.TimelineEvent.EndSeconds)
            {
                return true;
            }

            if (candidate.TimelineEvent.EndSeconds < current.TimelineEvent.EndSeconds)
            {
                return false;
            }

            return string.Compare(
                candidate.TimelineEvent.AnimationName,
                current.TimelineEvent.AnimationName,
                StringComparison.OrdinalIgnoreCase) > 0;
        }

        private void ApplyCutsceneControlTracks(float seconds, float durationSeconds)
        {
            if (this._CutscenePreview == null || this._ModelPreviewScene == null)
            {
                return;
            }

            var activeClipping = this._CutscenePreview.TimelineEvents
                .Where(e => string.Equals(e.Kind, "CameraClippingPlanes", StringComparison.OrdinalIgnoreCase) == true &&
                            IsCutsceneTimelineEventActive(e, seconds, durationSeconds) == true)
                .OrderBy(e => e.StartSeconds)
                .LastOrDefault();
            if (activeClipping != null && this._ModelPreviewCamera != null)
            {
                if (activeClipping.NearClip > 0.000001f)
                {
                    this._ModelPreviewCamera.CutsceneNearClip = activeClipping.NearClip;
                }
                if (activeClipping.FarClip > this._ModelPreviewCamera.CutsceneNearClip)
                {
                    this._ModelPreviewCamera.CutsceneFarClip = activeClipping.FarClip;
                }
            }

            foreach (var teleport in this._CutscenePreview.TimelineEvents
                .Where(e => string.Equals(e.Kind, "Teleport", StringComparison.OrdinalIgnoreCase) == true &&
                            e.StartSeconds <= seconds)
                .OrderBy(e => e.StartSeconds))
            {
                var instance = this.ResolveCutsceneInstanceByHash(teleport.ActorHash);
                if (instance == null)
                {
                    continue;
                }

                instance.Position = teleport.Offset;
                instance.Rotation = EulerRadiansToQuaternion(teleport.Orientation);
            }

            foreach (var attach in this._CutscenePreview.TimelineEvents
                .Where(e => string.Equals(e.Kind, "AttachObject", StringComparison.OrdinalIgnoreCase) == true &&
                            e.StartSeconds <= seconds)
                .OrderBy(e => e.StartSeconds))
            {
                var child = this.ResolveCutsceneInstanceByHash(attach.ActorHash);
                var parent = this.ResolveCutsceneInstanceByHash(attach.RelatedHash);
                if (child == null || parent == null)
                {
                    continue;
                }

                child.Position = parent.Position + attach.Offset - attach.ChildOffset;
                child.Rotation = EulerRadiansToQuaternion(attach.Orientation);
            }

            if (this._ModelPreviewOpenGl != null)
            {
                var activeFade = this._CutscenePreview.TimelineEvents
                    .Where(e => string.Equals(e.Kind, "Fade", StringComparison.OrdinalIgnoreCase) == true &&
                                IsCutsceneTimelineEventActive(e, seconds, durationSeconds) == true)
                    .OrderBy(e => e.StartSeconds)
                    .LastOrDefault();
                var activeFmv = this._CutscenePreview.TimelineEvents
                    .Where(e => string.Equals(e.Kind, "FMV", StringComparison.OrdinalIgnoreCase) == true &&
                                e.UseBlackFades == true &&
                                IsCutsceneTimelineEventActive(e, seconds, durationSeconds) == true)
                    .OrderBy(e => e.StartSeconds)
                    .LastOrDefault();
                this._ModelPreviewOpenGl.CutsceneFadeAlpha = this._ModelPreviewCamera != null &&
                                                             this._ModelPreviewCamera.UseCutsceneCamera == true
                                                                 ? Math.Max(GetFadeAlpha(activeFade, seconds), GetFmvFadeAlpha(activeFmv, seconds))
                                                                 : 0.0f;

                var activeLightGroup = this._CutscenePreview.TimelineEvents
                    .Where(e => string.Equals(e.Kind, "SetObjectLightGroup", StringComparison.OrdinalIgnoreCase) == true &&
                                IsCutsceneTimelineEventActive(e, seconds, durationSeconds) == true)
                    .OrderBy(e => e.StartSeconds)
                    .LastOrDefault();
                this._ModelPreviewOpenGl.CutsceneLightGroupStatus = activeLightGroup == null
                                                                        ? null
                                                                        : string.Format(System.Globalization.CultureInfo.InvariantCulture, "LightGroup {0:X16}", activeLightGroup.LightGroupHash);
            }

            this.ApplyCutsceneTrackLights(seconds, durationSeconds);
        }

        private void ApplyCutsceneTrackLights(float seconds, float durationSeconds)
        {
            if (this._ModelPreviewScene == null || this._ModelPreviewScene.Lights == null || this._CutscenePreview == null)
            {
                return;
            }

            this.ApplyCutsceneLights();
            foreach (var evt in this._CutscenePreview.TimelineEvents
                .Where(e => string.Equals(e.Kind, "Light", StringComparison.OrdinalIgnoreCase) == true &&
                            IsCutsceneTimelineEventActive(e, seconds, durationSeconds) == true)
                .OrderBy(e => e.StartSeconds))
            {
                var metaLight = this.ResolveCutsceneLight(evt.LightGroupHash);
                if (metaLight == null)
                {
                    continue;
                }

                var color = LengthSquared(evt.LightStartColor) <= 0.000001f ? new Vec3(1.0f, 1.0f, 1.0f) : evt.LightStartColor;
                var previewLight = this._ModelPreviewScene.Lights.FirstOrDefault(l => l != null &&
                    l.Dynamic == false &&
                    (l.SourceHash == evt.LightGroupHash ||
                     l.SourceHash == metaLight.SourceHash ||
                     string.Equals(l.Name, metaLight.ShortName, StringComparison.OrdinalIgnoreCase) == true ||
                     string.Equals(l.Name, metaLight.LongName, StringComparison.OrdinalIgnoreCase) == true));
                if (previewLight == null)
                {
                    previewLight = new ModelPreviewLight
                    {
                        Name = string.IsNullOrEmpty(metaLight.ShortName) ? metaLight.LongName : metaLight.ShortName,
                        SourceHash = metaLight.SourceHash,
                        RequiresTarget = true,
                        LightGroupHash = metaLight.LightGroupHash,
                        LightGroupName = metaLight.LightGroupName,
                        TargetActorName = string.IsNullOrEmpty(metaLight.TargetActorName) == true ? metaLight.TargetCompositeName : metaLight.TargetActorName,
                        TargetActorHash = metaLight.TargetActorHash,
                        Dynamic = true,
                    };
                    this._ModelPreviewScene.Lights.Add(previewLight);
                }

                previewLight.Position = metaLight.Position;
                previewLight.Color = color;
                previewLight.Intensity = evt.LightIntensity <= 0.0001f ? 1.0f : evt.LightIntensity;
                previewLight.Range = evt.LightRadius <= 0.001f ? 100.0f : evt.LightRadius;
            }

            foreach (var evt in this._CutscenePreview.TimelineEvents
                .Where(e => string.Equals(e.Kind, "FXLightPoint", StringComparison.OrdinalIgnoreCase) == true &&
                            IsCutsceneTimelineEventActive(e, seconds, durationSeconds) == true)
                .OrderBy(e => e.StartSeconds))
            {
                var basePosition = new Vec3();
                var parent = this.ResolveCutsceneInstanceByHash(evt.ActorHash);
                if (parent != null)
                {
                    basePosition = parent.Position;
                }

                float lightSeconds = Math.Max(0.0f, seconds - evt.StartSeconds - Math.Max(0.0f, evt.Wait));
                float scalar = GetCutsceneLightIntensityScalar(evt, lightSeconds);
                if (scalar <= 0.0001f)
                {
                    continue;
                }

                float colorT = evt.Duration > 0.0001f ? Math.Max(0.0f, Math.Min(1.0f, lightSeconds / evt.Duration)) : 0.0f;
                var color = evt.LightStartColor + (evt.LightEndColor - evt.LightStartColor) * colorT;
                if (LengthSquared(color) <= 0.000001f)
                {
                    color = new Vec3(1.0f, 1.0f, 1.0f);
                }

                this._ModelPreviewScene.Lights.Add(new ModelPreviewLight
                {
                    Name = string.Format(System.Globalization.CultureInfo.InvariantCulture, "FXLightPoint 0x{0:X}", evt.TrackOffset),
                    Dynamic = true,
                    Position = basePosition + evt.Offset,
                    Color = color,
                    Intensity = Math.Max(0.0f, evt.LightIntensity) * scalar,
                    Range = Math.Max(0.001f, evt.LightRadius),
                });
            }
        }

        private CutsceneLightObject ResolveCutsceneLight(ulong hash)
        {
            if (hash == 0 || this._CutscenePreview == null || this._CutscenePreview.Lights == null)
            {
                return null;
            }

            return this._CutscenePreview.Lights.FirstOrDefault(l =>
                string.Equals(l.LongName, hash.ToString("X16"), StringComparison.OrdinalIgnoreCase) == true ||
                string.Equals(l.ShortName, hash.ToString("X16"), StringComparison.OrdinalIgnoreCase) == true ||
                (string.IsNullOrEmpty(l.LongName) == false && l.LongName.HashX65599() == hash) ||
                (string.IsNullOrEmpty(l.ShortName) == false && l.ShortName.HashX65599() == hash) ||
                (string.IsNullOrEmpty(l.TypeName) == false && l.TypeName.HashX65599() == hash));
        }

        private static float GetCutsceneLightIntensityScalar(CutsceneTimelineEvent evt, float localSeconds)
        {
            if (evt == null || localSeconds < 0.0f)
            {
                return 0.0f;
            }

            float fadeIn = Math.Max(0.0f, evt.FadeIn);
            float duration = Math.Max(0.0f, evt.Duration);
            float fadeOut = Math.Max(0.0f, evt.FadeOut);
            float total = fadeIn + duration + fadeOut;
            if (total <= 0.0001f)
            {
                return 1.0f;
            }

            if (localSeconds <= fadeIn && fadeIn > 0.0001f)
            {
                return Math.Max(0.0f, Math.Min(1.0f, localSeconds / fadeIn));
            }

            if (localSeconds <= fadeIn + duration)
            {
                return 1.0f;
            }

            if (fadeOut > 0.0001f)
            {
                return Math.Max(0.0f, Math.Min(1.0f, 1.0f - ((localSeconds - fadeIn - duration) / fadeOut)));
            }

            return localSeconds <= total ? 1.0f : 0.0f;
        }

        private ModelPreviewSceneInstance ResolveCutsceneInstanceByHash(ulong hash)
        {
            if (hash == 0 || this._ModelPreviewScene == null || this._ModelPreviewScene.Instances == null)
            {
                return null;
            }

            return this._ModelPreviewScene.Instances.FirstOrDefault(i => i.ActorHash == hash);
        }

        private static float GetFadeAlpha(CutsceneTimelineEvent evt, float seconds)
        {
            if (evt == null)
            {
                return 0.0f;
            }

            if (evt.EndSeconds <= evt.StartSeconds)
            {
                return 1.0f;
            }

            var t = Math.Max(0.0f, Math.Min(1.0f, (seconds - evt.StartSeconds) / (evt.EndSeconds - evt.StartSeconds)));
            return IsFadeInType(evt.FadeTypeHash) == true ? 1.0f - t : t;
        }

        private static bool IsFadeInType(ulong fadeTypeHash)
        {
            return fadeTypeHash == "FadeIn".HashX65599() ||
                   fadeTypeHash == "FadeDown".HashX65599() ||
                   fadeTypeHash == "In".HashX65599();
        }

        private static float GetFmvFadeAlpha(CutsceneTimelineEvent evt, float seconds)
        {
            if (evt == null)
            {
                return 0.0f;
            }

            float fadeTime = evt.FadeDuration > 0.0001f ? evt.FadeDuration : 0.35f;
            float alpha = 0.0f;
            if (evt.FadeInOnEnter == true)
            {
                alpha = Math.Max(alpha, 1.0f - Math.Max(0.0f, Math.Min(1.0f, (seconds - evt.StartSeconds) / fadeTime)));
            }

            if (evt.FadeUpOnExit == true)
            {
                alpha = Math.Max(alpha, Math.Max(0.0f, Math.Min(1.0f, (seconds - (evt.EndSeconds - fadeTime)) / fadeTime)));
            }

            if (evt.StayFadedOnExit == true && seconds >= evt.EndSeconds - fadeTime)
            {
                alpha = 1.0f;
            }

            return alpha;
        }

        private static PreviewQuat EulerRadiansToQuaternion(Vec3 euler)
        {
            float cx = (float)Math.Cos(euler.X * 0.5f);
            float sx = (float)Math.Sin(euler.X * 0.5f);
            float cy = (float)Math.Cos(euler.Y * 0.5f);
            float sy = (float)Math.Sin(euler.Y * 0.5f);
            float cz = (float)Math.Cos(euler.Z * 0.5f);
            float sz = (float)Math.Sin(euler.Z * 0.5f);

            return new PreviewQuat
            {
                W = cx * cy * cz + sx * sy * sz,
                X = sx * cy * cz - cx * sy * sz,
                Y = cx * sy * cz + sx * cy * sz,
                Z = cx * cy * sz - sx * sy * cz,
            };
        }

        private void OnCutsceneTimelineScrubRequested(object sender, CutsceneTimelineScrubEventArgs e)
        {
            if (this._CutscenePreview == null || this._ModelPreviewScene == null)
            {
                return;
            }

            this._CutscenePlaying = false;
            this._CutscenePreviewClock = null;
            this._CutscenePlaybackStartSeconds = Math.Max(0.0f, Math.Min(this.GetCutsceneDurationSeconds(), e.Seconds));
            this._CutsceneEvaluationDirty = true;
            this.ApplyCutscenePreviewAtSeconds(this._CutscenePlaybackStartSeconds);
            this.RefreshModelPreview(true);
        }

        private static string FormatCutsceneActiveEvent(CutscenePlaybackEvent active)
        {
            if (active == null || active.TimelineEvent == null)
            {
                return string.Empty;
            }

            if (active.CameraAnimation != null)
            {
                return string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Camera:{0}",
                    active.TimelineEvent.AnimationName);
            }

            var actor = string.IsNullOrEmpty(active.TimelineEvent.ActorName) ? "unknown" : active.TimelineEvent.ActorName;
            var instance = active.Instance == null || string.IsNullOrEmpty(active.Instance.Name) == true ? "unbound" : active.Instance.Name;
            return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "{0}:{1}->{2}",
                actor,
                active.TimelineEvent.AnimationName,
                instance);
        }

        private static bool IsCutsceneTimelineEventActive(CutsceneTimelineEvent evt, float seconds, float durationSeconds)
        {
            if (evt == null || seconds < evt.StartSeconds)
            {
                return false;
            }

            if (seconds < evt.EndSeconds)
            {
                return true;
            }

            return durationSeconds > 0.0f &&
                   Math.Abs(seconds - durationSeconds) <= 0.0001f &&
                   Math.Abs(evt.EndSeconds - durationSeconds) <= 0.0001f;
        }

        private float GetCutsceneEventAnimationFrame(CutsceneTimelineEvent evt, ModelPreviewAnimation animation, float localSeconds)
        {
            if (evt == null || animation == null)
            {
                return 0.0f;
            }

            var speed = Math.Abs(evt.Speed) > 0.0001f ? evt.Speed : 1.0f;
            var sourceStartFrame = Math.Max(0.0f, animation.StartFrame);
            var frame = sourceStartFrame + localSeconds * animation.FrameRate * speed;
            if (evt.EndFrame > evt.StartFrame)
            {
                frame = Math.Min(sourceStartFrame + (evt.EndFrame - evt.StartFrame), frame);
            }
            else if (animation.NumFrames > 0.0f)
            {
                frame = Math.Min(sourceStartFrame + animation.NumFrames - 1.0f, frame);
            }

            return Math.Max(0.0f, frame);
        }

        private float GetCutsceneEventCameraFrame(CutsceneTimelineEvent evt, ModelPreviewCameraAnimation animation, float localSeconds)
        {
            if (evt == null || animation == null)
            {
                return 0.0f;
            }

            var speed = Math.Abs(evt.Speed) > 0.0001f ? evt.Speed : 1.0f;
            var sourceStartFrame = Math.Max(0.0f, animation.StartFrame);
            var frame = sourceStartFrame + localSeconds * animation.FrameRate * speed;
            if (evt.EndFrame > evt.StartFrame)
            {
                frame = Math.Min(sourceStartFrame + (evt.EndFrame - evt.StartFrame), frame);
            }
            else if (animation.NumFrames > 0.0f)
            {
                frame = Math.Min(sourceStartFrame + animation.NumFrames - 1.0f, frame);
            }

            return Math.Max(0.0f, frame);
        }

        private float GetCurrentPreviewAnimationFrame()
        {
            if (this._ModelPreviewAnimation == null)
            {
                return 0.0f;
            }

            if (this._PreviewAnimationPausedFrame.HasValue == true)
            {
                return this._PreviewAnimationPausedFrame.Value;
            }

            if (this._ModelPreviewAnimationClock == null)
            {
                return 0.0f;
            }

            float frameRate = this._ModelPreviewAnimation.FrameRate <= 0.0f ? 30.0f : this._ModelPreviewAnimation.FrameRate;
            float frame = (float)this._ModelPreviewAnimationClock.Elapsed.TotalSeconds * frameRate;
            float totalFrames = Math.Max(1.0f, this._ModelPreviewAnimation.NumFrames);
            if (this._ModelPreviewAnimation.Cyclic == true || this._PreviewLoopSelection == true)
            {
                frame %= totalFrames;
            }
            else
            {
                frame = Math.Min(totalFrames - 1.0f, frame);
            }

            return Math.Max(0.0f, frame);
        }

        private float GetCurrentVertexMorphScrubValue()
        {
            if (this._ModelPreviewAnimation == null || this.IsVertexMorphPreviewAnimation(this._ModelPreviewAnimation) == false)
            {
                return 0.0f;
            }

            return Math.Max(0.0f, Math.Min(1.0f, this.GetCurrentPreviewAnimationFrame()));
        }

        private float GetPreviewAnimationSeconds(float frame)
        {
            if (this._ModelPreviewAnimation == null)
            {
                return 0.0f;
            }

            float frameRate = this._ModelPreviewAnimation.FrameRate <= 0.0f ? 30.0f : this._ModelPreviewAnimation.FrameRate;
            return frame / frameRate;
        }

        private void StepPreviewAnimation(int direction)
        {
            if (this._ModelPreviewScene == null || this._ModelPreviewAnimation == null)
            {
                return;
            }

            float totalFrames = Math.Max(1.0f, this._ModelPreviewAnimation.NumFrames);
            float frame = (float)Math.Round(this.GetCurrentPreviewAnimationFrame()) + direction;
            if (frame < 0.0f)
            {
                frame = this._PreviewLoopSelection == true || this._ModelPreviewAnimation.Cyclic == true ? totalFrames - 1.0f : 0.0f;
            }
            else if (frame >= totalFrames)
            {
                frame = this._PreviewLoopSelection == true || this._ModelPreviewAnimation.Cyclic == true ? 0.0f : totalFrames - 1.0f;
            }

            this._PreviewAnimationPausedFrame = frame;
            this._PreviewRenderContinuous = false;
            this.ApplyCurrentPreviewAnimationFrame(frame);
            this.RefreshModelPreview(true);
        }

        private void UpdatePreviewOverlayText()
        {
            if (this._ModelPreviewOpenGl == null)
            {
                return;
            }

            this._ModelPreviewOpenGl.AnimationStatus = this.GetPreviewAnimationStatusText();
            bool showVertexMorphScrubber = this._ModelPreviewAnimation != null &&
                                           this.IsVertexMorphPreviewAnimation(this._ModelPreviewAnimation) == true;
            this._ModelPreviewOpenGl.VertexMorphScrubberVisible = showVertexMorphScrubber;
            this._ModelPreviewOpenGl.VertexMorphScrubberValue = showVertexMorphScrubber == true
                                                                   ? this.GetCurrentVertexMorphScrubValue()
                                                                   : 0.0f;
            this._ModelPreviewOpenGl.VertexMorphScrubberLabel = showVertexMorphScrubber == true
                                                                   ? "Vertex morph"
                                                                   : null;
            var statusParts = new List<string>();
            if (this._PreviewRootMotionEnabled == false)
            {
                statusParts.Add("Root Motion: disabled");
            }
            if (this._ModelPreviewCamera != null && this._ModelPreviewCamera.FreeCamera == true)
            {
                statusParts.Add(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Freecam speed: {0:0.0}x", this._FreeCameraTargetSpeedMultiplier));
            }
            if (this._ModelPreviewCamera != null && this._ModelPreviewCamera.LightingEnabled == false)
            {
                statusParts.Add("Lighting: disabled");
            }
            this._ModelPreviewOpenGl.RootMotionStatus = statusParts.Count == 0 ? null : string.Join(" | ", statusParts.ToArray());
            this._ModelPreviewOpenGl.MeshStatus = this.GetPreviewMeshStatusText();
        }

        private string GetPreviewAnimationStatusText()
        {
            if (this._ModelPreviewAnimation == null)
            {
                return null;
            }

            int currentFrame = (int)Math.Round(this.GetCurrentPreviewAnimationFrame()) + 1;
            int totalFrames = Math.Max(1, (int)Math.Ceiling(this._ModelPreviewAnimation.NumFrames));
            currentFrame = Math.Max(1, Math.Min(totalFrames, currentFrame));
            string name = string.IsNullOrEmpty(this._ModelPreviewAnimation.Name) ? "Animation" : this._ModelPreviewAnimation.Name;
            if (this.IsVertexMorphPreviewAnimation(this._ModelPreviewAnimation) == true)
            {
                return string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "{0} - vertex morph {1:0.###}",
                    name,
                    this.GetCurrentVertexMorphScrubValue());
            }

            string loop = this._PreviewLoopSelection == true ? " loop" : "";
            return string.Format("{0} - {1}/{2}{3}", name, currentFrame, totalFrames, loop);
        }

        private string GetPreviewMeshStatusText()
        {
            var node = this._PreviewActiveModelNode;
            if (node == null)
            {
                return null;
            }

            string meshName = this.GetNodeDisplayName(node, "mesh");
            string fileName;
            if (this._NodeSourceFiles.TryGetValue(node, out fileName) == false || string.IsNullOrEmpty(fileName) == true)
            {
                fileName = this.LastFileName;
            }

            string source = string.IsNullOrEmpty(fileName) ? "unknown p3d" : Path.GetFileName(fileName);
            return meshName + " - " + source;
        }

        private void RestartPreviewAnimation()
        {
            if (this._ModelPreviewAnimation == null)
            {
                return;
            }

            this._ModelPreviewAnimationQueueIndex = 0;
            if (this._ModelPreviewAnimationQueue.Count > 0)
            {
                this._ModelPreviewAnimation = this._ModelPreviewAnimationQueue[0];
            }

            this._PreviewRootMotionOffset = new Vec3();
            this._PreviewAnimationPausedFrame = null;
            this._ModelPreviewAnimationClock = Stopwatch.StartNew();
            this.ConfigurePreviewAnimationPlayback();
            this.UpdateModelPreviewAnimation();
            this.RefreshModelPreview(true);
        }

        private bool SelectPreviewCategory(TreeNode categoryNode)
        {
            if (categoryNode == null || this.ActiveFile == null)
            {
                return false;
            }

            var childNodes = categoryNode.Nodes.Cast<TreeNode>()
                                         .Where(n => n.Tag is FileFormats.Pure3D.BaseNode)
                                         .ToList();
            if (childNodes.Count == 0)
            {
                return false;
            }

            var baseNodes = childNodes.Select(n => (FileFormats.Pure3D.BaseNode)n.Tag).ToList();
            bool textureCategory = baseNodes.All(n => this.IsTextureExportNode(n));
            bool modelCategory = baseNodes.All(n => this.IsModelExportNode(n));
            bool animationCategory = baseNodes.All(n => n is Animation);
            if (textureCategory == false)
            {
                Type firstType = baseNodes[0].GetType();
                if (baseNodes.Any(n => n.GetType() != firstType) == true)
                {
                    return false;
                }
            }

            if (textureCategory == false && modelCategory == false && animationCategory == false)
            {
                return false;
            }

            this.ClearPreviewMultiSelection();
            foreach (var treeNode in childNodes)
            {
                var node = (FileFormats.Pure3D.BaseNode)treeNode.Tag;
                treeNode.BackColor = Color.LightSteelBlue;
                treeNode.ForeColor = Color.Black;
                this._PreviewSelectedTreeNodes.Add(treeNode);
                if (modelCategory == true)
                {
                    this._PreviewSelectedModelNodes.Add(node);
                }
                else if (animationCategory == true)
                {
                    this._PreviewSelectedAnimationNodes.Add((Animation)node);
                }
            }

            this.propertyGrid.SelectedObject = baseNodes.Count == 1 ? (object)baseNodes[0] : categoryNode.Text;
            this.importNodeButton.Enabled = false;
            this.exportNodeButton.Enabled = false;

            if (textureCategory == true)
            {
                this._PreviewActiveModelNode = null;
                this._ModelPreviewAnimation = null;
                this._ModelPreviewAnimationClock = null;
                this._ModelPreviewAnimationQueue.Clear();
                this._ModelPreviewAnimationQueueIndex = 0;
                this._PreviewRenderContinuous = false;
            }
            else if (modelCategory == true)
            {
                this._PreviewActiveModelNode = this._PreviewSelectedModelNodes.Count == 0 ? null : this._PreviewSelectedModelNodes[this._PreviewSelectedModelNodes.Count - 1];
                this.RebuildPreviewSelectionScene();
            }
            else
            {
                this.RebuildPreviewAnimationQueue();
            }

            return true;
        }

        private void SelectRelativeAnimationNode(int direction)
        {
            if (this.nodeView.Nodes.Count == 0)
            {
                return;
            }

            var animationNodes = new List<TreeNode>();
            this.CollectAnimationTreeNodes(this.nodeView.Nodes, animationNodes);
            if (animationNodes.Count == 0)
            {
                return;
            }

            int index = this.nodeView.SelectedNode == null ? -1 : animationNodes.IndexOf(this.nodeView.SelectedNode);
            if (index < 0)
            {
                index = direction >= 0 ? -1 : 0;
            }

            int next = (index + direction + animationNodes.Count) % animationNodes.Count;
            this.nodeView.SelectedNode = animationNodes[next];
            animationNodes[next].EnsureVisible();
            this.nodeView.Focus();
        }

        private void SelectRelativeMeshNode(int direction)
        {
            if (this.nodeView.Nodes.Count == 0)
            {
                return;
            }

            var meshNodes = new List<TreeNode>();
            this.CollectMeshTreeNodes(this.nodeView.Nodes, meshNodes);
            if (meshNodes.Count == 0)
            {
                return;
            }

            int index = this.nodeView.SelectedNode == null ? -1 : meshNodes.IndexOf(this.nodeView.SelectedNode);
            if (index < 0)
            {
                index = direction >= 0 ? -1 : 0;
            }

            int next = (index + direction + meshNodes.Count) % meshNodes.Count;
            this.nodeView.SelectedNode = meshNodes[next];
            meshNodes[next].EnsureVisible();
            this.nodeView.Focus();
        }

        private void CollectAnimationTreeNodes(TreeNodeCollection nodes, List<TreeNode> animationNodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag is Animation)
                {
                    animationNodes.Add(node);
                }

                if (node.Nodes.Count > 0)
                {
                    this.CollectAnimationTreeNodes(node.Nodes, animationNodes);
                }
            }
        }

        private void CollectMeshTreeNodes(TreeNodeCollection nodes, List<TreeNode> meshNodes)
        {
            foreach (TreeNode node in nodes)
            {
                var baseNode = node.Tag as FileFormats.Pure3D.BaseNode;
                if (baseNode != null && this.IsModelExportNode(baseNode) == true)
                {
                    meshNodes.Add(node);
                }

                if (node.Nodes.Count > 0)
                {
                    this.CollectMeshTreeNodes(node.Nodes, meshNodes);
                }
            }
        }

        private void OnPreviewMouseDown(object sender, MouseEventArgs e)
        {
            if (this._ModelPreviewScene == null)
            {
                return;
            }

            if (this._ModelPreviewOpenGl != null && this._ModelPreviewOpenGl.IsCutsceneTimelineAt(e.Location) == true)
            {
                this._PreviewMouseButton = MouseButtons.None;
                return;
            }

            if (this._ModelPreviewOpenGl != null && this._ModelPreviewOpenGl.IsVertexMorphScrubberAt(e.Location) == true)
            {
                this._PreviewMouseButton = MouseButtons.None;
                return;
            }

            var control = sender as Control;
            (control ?? this.previewPicture).Focus();
            this._PreviewMousePosition = e.Location;
            this._PreviewMouseButton = e.Button;
            this._PreviewMouseMoved = false;
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this._ModelPreviewScene == null || this._ModelPreviewCamera == null || this._PreviewMouseButton == MouseButtons.None)
            {
                return;
            }

            int dx = e.X - this._PreviewMousePosition.X;
            int dy = e.Y - this._PreviewMousePosition.Y;
            if (Math.Abs(dx) + Math.Abs(dy) > 2)
            {
                this._PreviewMouseMoved = true;
            }

            if (this._PreviewMouseButton == MouseButtons.Left)
            {
                this._ModelPreviewCamera.Yaw += dx * 0.01f;
                this._ModelPreviewCamera.Pitch = Math.Max(-1.45f, Math.Min(1.45f, this._ModelPreviewCamera.Pitch + dy * 0.01f));
            }
            else if (this._PreviewMouseButton == MouseButtons.Right)
            {
                if (this._ModelPreviewCamera.FreeCamera == true)
                {
                    this._ModelPreviewCamera.Yaw += dx * 0.01f;
                    this._ModelPreviewCamera.Pitch = Math.Max(-1.45f, Math.Min(1.45f, this._ModelPreviewCamera.Pitch + dy * 0.01f));
                }
                else
                {
                    this._ModelPreviewCamera.Zoom = Math.Max(0.05f, Math.Min(30.0f, this._ModelPreviewCamera.Zoom * (float)Math.Pow(1.01, -dy)));
                }
            }
            else if (this._PreviewMouseButton == MouseButtons.Middle)
            {
                this._ModelPreviewCamera.PanX += dx;
                this._ModelPreviewCamera.PanY += dy;
            }

            this._PreviewMousePosition = e.Location;
            this.RefreshModelPreview(true);
        }

        private void OnPreviewMouseUp(object sender, MouseEventArgs e)
        {
            if (this._ModelPreviewScene != null && this._ModelPreviewCamera != null && e.Button == MouseButtons.Middle && this._PreviewMouseMoved == false)
            {
                this._ModelPreviewCamera.Origin = this._ModelPreviewScene.Center;
                this._ModelPreviewCamera.PanX = 0;
                this._ModelPreviewCamera.PanY = 0;
            }

            this._PreviewMouseButton = MouseButtons.None;
            this.RefreshModelPreview();
        }

        private void OnPreviewMouseWheel(object sender, MouseEventArgs e)
        {
            if (this._ModelPreviewScene == null || this._ModelPreviewCamera == null)
            {
                return;
            }

            if (this._ModelPreviewCamera.FreeCamera == true)
            {
                var steps = e.Delta / 120.0f;
                this._FreeCameraTargetSpeedMultiplier = Math.Max(0.1f, Math.Min(5.0f, this._FreeCameraTargetSpeedMultiplier + steps * 0.1f));
                this.UpdatePreviewOverlayText();
                this.RefreshModelPreview(true);
                return;
            }

            this._ModelPreviewCamera.Zoom = Math.Max(0.05f, Math.Min(30.0f, this._ModelPreviewCamera.Zoom * (e.Delta > 0 ? 1.1f : 0.9f)));
            this.RefreshModelPreview();
        }

        private void OnPreviewResize(object sender, EventArgs e)
        {
            this.RefreshModelPreview();
        }

        private void OnPreviewPanelResize(object sender, EventArgs e)
        {
            this.previewPicture.Bounds = this.splitContainer2.Panel1.ClientRectangle;
            this._ModelPreviewOpenGl.Bounds = this.splitContainer2.Panel1.ClientRectangle;
            this.RefreshModelPreview();
        }

        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemOpenBrackets)
            {
                this.SelectRelativeAnimationNode(-1);
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.OemCloseBrackets)
            {
                this.SelectRelativeAnimationNode(1);
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.PageUp)
            {
                this.SelectRelativeMeshNode(-1);
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.PageDown)
            {
                this.SelectRelativeMeshNode(1);
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.X && e.Control == true)
            {
                this.ExportSelectedObjectsToFolder();
                e.Handled = true;
                return;
            }

            if (this._ModelPreviewScene == null || this._ModelPreviewCamera == null)
            {
                return;
            }

            if (this._ModelPreviewCamera.FreeCamera == true && this._PreviewMouseButton == MouseButtons.Right)
            {
                if (e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D)
                {
                    this.SetFreeCameraKey(e.KeyCode, true);
                    e.Handled = true;
                    return;
                }
            }

            if (e.KeyCode == Keys.F)
            {
                this._ModelPreviewCamera.Origin = this._ModelPreviewScene.Average;
                this._ModelPreviewCamera.PanX = 0;
                this._ModelPreviewCamera.PanY = 0;
                this.RefreshModelPreview();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.H)
            {
                this._ModelPreviewCamera.ShowControls = !this._ModelPreviewCamera.ShowControls;
                this.RefreshModelPreview();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.S)
            {
                this._ModelPreviewCamera.ShowSkeleton = !this._ModelPreviewCamera.ShowSkeleton;
                this.RefreshModelPreview();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.N)
            {
                this._ModelPreviewCamera.ShowBoneNames = !this._ModelPreviewCamera.ShowBoneNames;
                this._ModelPreviewCamera.ShowSkeleton = true;
                this.RefreshModelPreview();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.I)
            {
                this._ModelPreviewCamera.ShowInfluences = !this._ModelPreviewCamera.ShowInfluences;
                this.RefreshModelPreview();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.L)
            {
                this._ModelPreviewCamera.ShowLocators = !this._ModelPreviewCamera.ShowLocators;
                this.RefreshModelPreview();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.T)
            {
                if (e.Control == true)
                {
                    this._ModelPreviewCamera.LightingEnabled = !this._ModelPreviewCamera.LightingEnabled;
                }
                else
                {
                    this._ModelPreviewCamera.TextureMode = (ModelPreviewTextureMode)(((int)this._ModelPreviewCamera.TextureMode + 1) % 3);
                }
                this.RefreshModelPreview();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.M)
            {
                this._ModelPreviewCamera.ShowMaterialPreview = !this._ModelPreviewCamera.ShowMaterialPreview;
                this.RefreshModelPreview();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.G && e.Control == true)
            {
                this.TogglePreviewGpuSkinning();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.W)
            {
                this._ModelPreviewCamera.WireMode = (ModelPreviewWireMode)(((int)this._ModelPreviewCamera.WireMode + 1) % 3);
                this.RefreshModelPreview();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.V)
            {
                this.ToggleFreeCamera();
                this.RefreshModelPreview(true);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.C && this._CutscenePreview != null)
            {
                if (this._ModelPreviewCamera.UseCutsceneCamera == true)
                {
                    this.SwitchFromCutsceneCameraToOrbit();
                }
                else
                {
                    this._ModelPreviewCamera.FreeCamera = false;
                    this.ClearFreeCameraKeys();
                    this._ModelPreviewCamera.UseCutsceneCamera = true;
                }
                this.RefreshModelPreview();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.X)
            {
                this._PreviewLoopSelection = !this._PreviewLoopSelection;
                this.RefreshModelPreview(true);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Oemcomma)
            {
                this.StepPreviewAnimation(-1);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.OemPeriod)
            {
                this.StepPreviewAnimation(1);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.R && e.Control == true)
            {
                this._PreviewRootMotionEnabled = !this._PreviewRootMotionEnabled;
                this._PreviewRootMotionOffset = new Vec3();
                this._CutsceneEvaluationDirty = true;
                if (this._ModelPreviewAnimation != null)
                {
                    this._ModelPreviewAnimationClock = Stopwatch.StartNew();
                    this._PreviewAnimationPausedFrame = null;
                    this.ConfigurePreviewAnimationPlayback();
                    this.UpdateModelPreviewAnimation();
                }
                this.RefreshModelPreview(true);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.A)
            {
                if (this._ModelPreviewAnimation != null)
                {
                    this.RestartPreviewAnimation();
                    e.Handled = true;
                }
            }
        }

        private void TogglePreviewGpuSkinning()
        {
            if (this._ModelPreviewScene == null)
            {
                return;
            }

            this._ModelPreviewScene.UseGpuSkinning = !this._ModelPreviewScene.UseGpuSkinning;
            if (this._ModelPreviewScene.UseGpuSkinning == true && this._ModelPreviewOpenGl != null)
            {
                this._ModelPreviewOpenGl.RetryGpuSkinning();
            }
            if (this._ModelPreviewAnimation != null)
            {
                this.UpdateModelPreviewAnimation();
            }
            else if (this._CutscenePreview != null)
            {
                this._CutsceneEvaluationDirty = true;
                this.ApplyCutscenePreviewAtSeconds(this._CutscenePlaybackStartSeconds);
            }
            else
            {
                ModelPreviewBuilder.ResetAnimation(this._ModelPreviewScene);
            }

            this.RefreshModelPreview(true);
        }

        private void OnEditorKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D)
            {
                this.SetFreeCameraKey(e.KeyCode, false);
            }
        }

        private void SetFreeCameraKey(Keys key, bool pressed)
        {
            if (key == Keys.W) this._FreeCameraForward = pressed;
            else if (key == Keys.S) this._FreeCameraBack = pressed;
            else if (key == Keys.A) this._FreeCameraLeft = pressed;
            else if (key == Keys.D) this._FreeCameraRight = pressed;
        }

        private bool UpdateFreeCameraMovement()
        {
            var now = DateTime.UtcNow;
            var elapsed = (float)(now - this._LastFreeCameraMoveTime).TotalSeconds;
            this._LastFreeCameraMoveTime = now;
            if (this._ModelPreviewCamera == null ||
                this._ModelPreviewCamera.FreeCamera == false)
            {
                this._FreeCameraVelocity = new Vec3();
                return false;
            }

            if (this._PreviewMouseButton != MouseButtons.Right)
            {
                this._FreeCameraVelocity = new Vec3();
                return false;
            }

            var dt = Math.Max(0.001f, Math.Min(0.05f, elapsed));
            var speedAlpha = 1.0f - (float)Math.Exp(-10.0f * dt);
            this._FreeCameraSpeedMultiplier += (this._FreeCameraTargetSpeedMultiplier - this._FreeCameraSpeedMultiplier) * speedAlpha;

            var move = new Vec3();
            var forward = GetCameraForward(this._ModelPreviewCamera);
            var right = GetCameraRight(this._ModelPreviewCamera);
            if (this._FreeCameraForward == true) move += forward;
            if (this._FreeCameraBack == true) move -= forward;
            if (this._FreeCameraRight == true) move += right;
            if (this._FreeCameraLeft == true) move -= right;

            var speed = Math.Max(0.1f, this.GetPreviewControlRadius() * 0.75f);
            var targetVelocity = LengthSquared(move) <= 0.000001f
                                     ? new Vec3()
                                     : Normalize(move) * (speed * this._FreeCameraSpeedMultiplier);
            var velocityAlpha = 1.0f - (float)Math.Exp(-16.0f * dt);
            this._FreeCameraVelocity += (targetVelocity - this._FreeCameraVelocity) * velocityAlpha;
            if (LengthSquared(this._FreeCameraVelocity) <= 0.000001f)
            {
                return Math.Abs(this._FreeCameraSpeedMultiplier - this._FreeCameraTargetSpeedMultiplier) > 0.001f;
            }

            this._ModelPreviewCamera.FreePosition += this._FreeCameraVelocity * dt;
            return true;
        }

        private void ToggleFreeCamera()
        {
            if (this._ModelPreviewCamera == null)
            {
                return;
            }

            if (this._ModelPreviewCamera.FreeCamera == false)
            {
                var currentEye = GetCurrentCameraEye(this._ModelPreviewCamera, this._ModelPreviewScene);
                if (this._ModelPreviewCamera.UseCutsceneCamera == true && LengthSquared(this._ModelPreviewCamera.CutsceneLook) > 0.000001f)
                {
                    SetCameraAnglesFromDirection(this._ModelPreviewCamera, this._ModelPreviewCamera.CutsceneLook);
                }

                this._ModelPreviewCamera.UseCutsceneCamera = false;
                this._ModelPreviewCamera.FreeCamera = true;
                this._ModelPreviewCamera.FreePosition = currentEye;
                this._FreeCameraVelocity = new Vec3();
                this._LastFreeCameraMoveTime = DateTime.UtcNow;
                return;
            }

            var forward = GetCameraForward(this._ModelPreviewCamera);
            var focusDistance = 2.0f;
            this._ModelPreviewCamera.Origin = this._ModelPreviewCamera.FreePosition + forward * focusDistance;
            this._ModelPreviewCamera.PanX = 0;
            this._ModelPreviewCamera.PanY = 0;
            var controlRadius = this.GetPreviewControlRadius();
            if (controlRadius > 0.001f)
            {
                this._ModelPreviewCamera.Zoom = Math.Max(0.05f, Math.Min(30.0f, controlRadius * 3.0f / focusDistance));
            }
            this._ModelPreviewCamera.FreeCamera = false;
            this._FreeCameraVelocity = new Vec3();
            this.ClearFreeCameraKeys();
        }

        private void SwitchFromCutsceneCameraToOrbit()
        {
            if (this._ModelPreviewCamera == null)
            {
                return;
            }

            var eye = this._ModelPreviewCamera.CutscenePosition;
            var forward = LengthSquared(this._ModelPreviewCamera.CutsceneLook) > 0.000001f
                              ? Normalize(this._ModelPreviewCamera.CutsceneLook)
                              : GetCameraForward(this._ModelPreviewCamera);
            SetCameraAnglesFromDirection(this._ModelPreviewCamera, forward);

            var focusDistance = 2.0f;
            this._ModelPreviewCamera.Origin = eye + forward * focusDistance;
            this._ModelPreviewCamera.PanX = 0;
            this._ModelPreviewCamera.PanY = 0;
            var controlRadius = this.GetPreviewControlRadius();
            if (controlRadius > 0.001f)
            {
                this._ModelPreviewCamera.Zoom = Math.Max(0.05f, Math.Min(30.0f, controlRadius * 3.0f / focusDistance));
            }

            this._ModelPreviewCamera.UseCutsceneCamera = false;
            this._ModelPreviewCamera.FreeCamera = false;
            this._FreeCameraVelocity = new Vec3();
            this.ClearFreeCameraKeys();
        }

        private void ClearFreeCameraKeys()
        {
            this._FreeCameraForward = false;
            this._FreeCameraBack = false;
            this._FreeCameraLeft = false;
            this._FreeCameraRight = false;
        }

        private static Vec3 GetCurrentCameraEye(ModelPreviewCamera camera, ModelPreviewScene scene)
        {
            if (camera == null)
            {
                return new Vec3();
            }

            if (camera.UseCutsceneCamera == true)
            {
                return camera.CutscenePosition;
            }

            var origin = camera.Origin;
            var radius = Math.Max(0.001f, camera.ControlRadius > 0.001f ? camera.ControlRadius : scene == null ? 1.0f : scene.Radius);
            var distance = radius * 3.0f / Math.Max(0.05f, camera.Zoom);
            return origin - GetCameraForward(camera) * distance;
        }

        private float GetPreviewControlRadius()
        {
            if (this._ModelPreviewCamera != null && this._ModelPreviewCamera.ControlRadius > 0.001f)
            {
                return this._ModelPreviewCamera.ControlRadius;
            }

            return this._ModelPreviewScene == null ? 1.0f : this._ModelPreviewScene.Radius;
        }

        private static Vec3 GetCameraForward(ModelPreviewCamera camera)
        {
            float cy = (float)Math.Cos(camera.Yaw);
            float sy = (float)Math.Sin(camera.Yaw);
            float cp = (float)Math.Cos(camera.Pitch);
            float sp = (float)Math.Sin(camera.Pitch);
            return Normalize(new Vec3(sy * cp, -sp, cy * cp));
        }

        private static Vec3 GetCameraRight(ModelPreviewCamera camera)
        {
            float cy = (float)Math.Cos(camera.Yaw);
            float sy = (float)Math.Sin(camera.Yaw);
            return Normalize(new Vec3(cy, 0.0f, -sy));
        }

        private static void SetCameraAnglesFromDirection(ModelPreviewCamera camera, Vec3 direction)
        {
            if (camera == null)
            {
                return;
            }

            var forward = Normalize(direction);
            camera.Pitch = Math.Max(-1.45f, Math.Min(1.45f, (float)Math.Asin(-forward.Y)));
            camera.Yaw = (float)Math.Atan2(forward.X, forward.Z);
        }

        private static float LengthSquared(Vec3 value)
        {
            return value.X * value.X + value.Y * value.Y + value.Z * value.Z;
        }

        private static Vec3 Normalize(Vec3 value)
        {
            var length = (float)Math.Sqrt(LengthSquared(value));
            return length <= 0.000001f ? new Vec3() : value / length;
        }

        private void OnSelectNode(object sender, TreeViewEventArgs e)
        {
            if (this._IgnoreNextNodeSelect == true)
            {
                this._IgnoreNextNodeSelect = false;
                return;
            }

            if (this._CutscenePreview != null)
            {
                if (e.Node != null && e.Node.Tag != null)
                {
                    var cutsceneFightTrackItem = e.Node.Tag as FightTrackTreeItem;
                    this.propertyGrid.SelectedObject = cutsceneFightTrackItem == null ? e.Node.Tag : cutsceneFightTrackItem.Track;
                    this.importNodeButton.Enabled = false;
                    this.exportNodeButton.Enabled = e.Node.Tag is FileFormats.Pure3D.BaseNode &&
                                                    ((FileFormats.Pure3D.BaseNode)e.Node.Tag).Exportable;
                }
                else if (e.Node != null)
                {
                    this.propertyGrid.SelectedObject = e.Node.Text;
                    this.importNodeButton.Enabled = false;
                    this.exportNodeButton.Enabled = false;
                }

                return;
            }

            if (e.Node == null || e.Node.Tag == null)
            {
                if (this.SelectPreviewCategory(e.Node) == true)
                {
                    return;
                }

                this.SelectNothing();
                return;
            }

            if (e.Node.Tag is FileFormats.Pure3D.BaseNode)
            {
                var node = (FileFormats.Pure3D.BaseNode)e.Node.Tag;
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    this.SelectPreviewRange(e.Node, node, (Control.ModifierKeys & Keys.Control) == Keys.Control);
                    return;
                }

                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    this._IgnoreNextNodeSelect = false;
                    this.TogglePreviewMultiSelection(e.Node, node);
                    return;
                }

                this.ClearPreviewMultiSelection();
                this._PreviewSelectionAnchorNode = e.Node;
                this.SelectNode(node);
                return;
            }

            var fightTrackItem = e.Node.Tag as FightTrackTreeItem;
            if (fightTrackItem != null)
            {
                this.propertyGrid.SelectedObject = fightTrackItem.Track;
                this.importNodeButton.Enabled = false;
                this.exportNodeButton.Enabled = false;
                return;
            }

            this.propertyGrid.SelectedObject = e.Node.Tag;
            this.importNodeButton.Enabled = false;
            this.exportNodeButton.Enabled = false;
        }

        private void OnPropertyGridValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (this.nodeView.SelectedNode == null)
            {
                return;
            }

            var fightTrackItem = this.nodeView.SelectedNode.Tag as FightTrackTreeItem;
            if (fightTrackItem == null)
            {
                return;
            }

            this.ReplaceFightTrackBytes(fightTrackItem);
            this.nodeView.SelectedNode.Text = GetFightTrackDisplayName(fightTrackItem);
        }

        private void OnNodeMouseDown(object sender, MouseEventArgs e)
        {
            if (this._CutscenePreview != null)
            {
                return;
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            var hit = this.nodeView.HitTest(e.Location);
            if (hit == null || hit.Node == null || !(hit.Node.Tag is FileFormats.Pure3D.BaseNode))
            {
                return;
            }

            var modifiers = Control.ModifierKeys;
            if ((modifiers & Keys.Shift) == Keys.Shift)
            {
                this._IgnoreNextNodeSelect = this.nodeView.SelectedNode != hit.Node;
                this.SelectPreviewRange(
                    hit.Node,
                    (FileFormats.Pure3D.BaseNode)hit.Node.Tag,
                    (modifiers & Keys.Control) == Keys.Control);
                this.BeginInvoke((Action)(() => this._IgnoreNextNodeSelect = false));
                return;
            }

            if ((modifiers & Keys.Control) == Keys.Control)
            {
                this._IgnoreNextNodeSelect = this.nodeView.SelectedNode != hit.Node;
                this.TogglePreviewMultiSelection(hit.Node, (FileFormats.Pure3D.BaseNode)hit.Node.Tag);
                this.BeginInvoke((Action)(() => this._IgnoreNextNodeSelect = false));
            }
        }

        private void OnEditorDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (fileNames != null && fileNames.Any(f => string.Equals(Path.GetExtension(f), ".p3d", StringComparison.OrdinalIgnoreCase)))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }

            e.Effect = DragDropEffects.None;
        }

        private void OnEditorDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null || e.Data.GetDataPresent(DataFormats.FileDrop) == false)
            {
                return;
            }

            var fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (fileNames == null)
            {
                return;
            }

            this.LoadPure3DFiles(fileNames);
        }

        private void OnNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.Node == null)
            {
                return;
            }

            this._ExportAnimationGlbMenuItem.Visible = false;
            this._ExportModelGlbMenuItem.Visible = false;
            this._ExportSelectedToFolderMenuItem.Visible = false;
            this._ExportNodeMenuItem.Visible = false;
            this._EditLuaTrackMenuItem.Visible = false;

            var fightTrackItem = e.Node.Tag as FightTrackTreeItem;
            if (fightTrackItem != null)
            {
                this.nodeView.SelectedNode = e.Node;
                this.propertyGrid.SelectedObject = fightTrackItem.Track;
                this._EditLuaTrackMenuItem.Visible = this.IsEditableLuaTrack(fightTrackItem);
                if (this._EditLuaTrackMenuItem.Visible == true)
                {
                    this._NodeContextMenu.Show(this.nodeView, e.Location);
                }

                return;
            }

            if (!(e.Node.Tag is FileFormats.Pure3D.BaseNode))
            {
                this.nodeView.SelectedNode = e.Node;
                if (this.SelectPreviewCategory(e.Node) == true)
                {
                    this._ExportSelectedToFolderMenuItem.Visible =
                        this._PreviewSelectedModelNodes.Count > 0 ||
                        this._PreviewSelectedAnimationNodes.Count > 0 ||
                        this._PreviewSelectedTreeNodes.Any(n => n.Tag is FileFormats.Pure3D.BaseNode && this.IsTextureExportNode((FileFormats.Pure3D.BaseNode)n.Tag) == true);
                    if (this._ExportSelectedToFolderMenuItem.Visible == true)
                    {
                        this._NodeContextMenu.Show(this.nodeView, e.Location);
                    }
                }

                return;
            }

            var node = (FileFormats.Pure3D.BaseNode)e.Node.Tag;
            var modifiers = Control.ModifierKeys;
            if (this._CutscenePreview != null)
            {
                this.nodeView.SelectedNode = e.Node;
                this.propertyGrid.SelectedObject = node;
            }
            else if ((modifiers & Keys.Shift) == Keys.Shift)
            {
                this.SelectPreviewRange(e.Node, node, (modifiers & Keys.Control) == Keys.Control);
            }
            else if ((modifiers & Keys.Control) == Keys.Control)
            {
                this.TogglePreviewMultiSelection(e.Node, node);
            }
            else
            {
                this.nodeView.SelectedNode = e.Node;
                this.ClearPreviewMultiSelection();
                this._PreviewSelectionAnchorNode = e.Node;
                this.SelectNode(node);
            }

            this._ExportAnimationGlbMenuItem.Visible = node is Animation;
            this._ExportModelGlbMenuItem.Visible = this.IsModelExportNode(node);
            this._ExportSelectedToFolderMenuItem.Visible =
                this.IsModelExportNode(node) == true ||
                this.IsTextureExportNode(node) == true ||
                node is Animation ||
                this._PreviewSelectedModelNodes.Count > 0 ||
                this._PreviewSelectedAnimationNodes.Count > 0 ||
                this._PreviewSelectedTreeNodes.Any(n => n.Tag is FileFormats.Pure3D.BaseNode && this.IsTextureExportNode((FileFormats.Pure3D.BaseNode)n.Tag) == true);
            this._ExportNodeMenuItem.Visible = node.Exportable && !(node is Animation) && this.IsModelExportNode(node) == false;

            if (this._ExportAnimationGlbMenuItem.Visible == true ||
                this._ExportModelGlbMenuItem.Visible == true ||
                this._ExportSelectedToFolderMenuItem.Visible == true ||
                this._ExportNodeMenuItem.Visible == true ||
                this._EditLuaTrackMenuItem.Visible == true)
            {
                this._NodeContextMenu.Show(this.nodeView, e.Location);
            }
        }

        private void OnModelGlbExport(object sender, EventArgs e)
        {
            if (this.nodeView.SelectedNode == null || !(this.nodeView.SelectedNode.Tag is FileFormats.Pure3D.BaseNode))
            {
                return;
            }

            this.ExportModelNode((FileFormats.Pure3D.BaseNode)this.nodeView.SelectedNode.Tag);
        }

        private void OnSelectedFolderExport(object sender, EventArgs e)
        {
            this.ExportSelectedObjectsToFolder();
        }

        private void OnLuaTrackEdit(object sender, EventArgs e)
        {
            if (this.nodeView.SelectedNode == null)
            {
                return;
            }

            var item = this.nodeView.SelectedNode.Tag as FightTrackTreeItem;
            if (this.IsEditableLuaTrack(item) == false)
            {
                return;
            }

            var luaCode = GetLuaCode(item.Track);
            bool preserveTrailingNull = luaCode.Length > 0 && luaCode[luaCode.Length - 1] == 0;
            var text = DecodeLuaText(luaCode);
            using (var dialog = new LuaTrackEditDialog(text))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var newCode = EncodeLuaText(dialog.ScriptText, preserveTrailingNull);
                SetLuaCode(item.Track, newCode);
                this.ReplaceFightTrackBytes(item);
                this.nodeView.SelectedNode.Text = GetFightTrackDisplayName(item);
                this.propertyGrid.Refresh();
            }
        }

        private bool IsEditableLuaTrack(FightTrackTreeItem item)
        {
            if (item == null || item.Track == null || item.FightData == null || item.FightData.Data == null)
            {
                return false;
            }

            var typeName = item.Track.GetType().Name;
            if (string.Equals(typeName, "LuaTrack", StringComparison.OrdinalIgnoreCase) == false)
            {
                return false;
            }

            var compiledProperty = item.Track.GetType().GetProperty("IsLuaCompiled");
            return compiledProperty == null || (bool)compiledProperty.GetValue(item.Track, null) == false;
        }

        private static byte[] GetLuaCode(Nixson.Prototype.Fight.BaseTrack track)
        {
            if (track == null)
            {
                return new byte[0];
            }

            var property = track.GetType().GetProperty("LuaCode");
            return property == null ? new byte[0] : (property.GetValue(track, null) as byte[]) ?? new byte[0];
        }

        private static void SetLuaCode(Nixson.Prototype.Fight.BaseTrack track, byte[] code)
        {
            var property = track == null ? null : track.GetType().GetProperty("LuaCode");
            if (property != null && property.CanWrite == true)
            {
                property.SetValue(track, code ?? new byte[0], null);
            }
        }

        private static string DecodeLuaText(byte[] code)
        {
            if (code == null || code.Length == 0)
            {
                return string.Empty;
            }

            int length = code.Length;
            while (length > 0 && code[length - 1] == 0)
            {
                length--;
            }

            return Encoding.UTF8.GetString(code, 0, length);
        }

        private static byte[] EncodeLuaText(string text, bool trailingNull)
        {
            var data = Encoding.UTF8.GetBytes(text ?? string.Empty);
            if (trailingNull == false)
            {
                return data;
            }

            var withNull = new byte[data.Length + 1];
            Array.Copy(data, withNull, data.Length);
            return withNull;
        }

        private void ReplaceFightTrackBytes(FightTrackTreeItem item)
        {
            if (item == null || item.FightData == null || item.FightData.Data == null || item.Track == null)
            {
                return;
            }

            var data = item.FightData.Data;
            if (item.SourceOffset < 0 || item.SourceEnd < item.SourceOffset || item.SourceEnd > data.Length)
            {
                throw new InvalidOperationException("Fight track source range is invalid.");
            }

            byte[] replacement;
            using (var output = new MemoryStream())
            {
                Nixson.Prototype.Fight.BaseTrack.SerializeBaseTrack(
                    Nixson.Common.PrototypeGame.P1,
                    output,
                    Endian.Little,
                    item.Track);
                replacement = output.ToArray();
            }

            int oldLength = item.SourceEnd - item.SourceOffset;
            var next = new byte[data.Length - oldLength + replacement.Length];
            Array.Copy(data, 0, next, 0, item.SourceOffset);
            Array.Copy(replacement, 0, next, item.SourceOffset, replacement.Length);
            Array.Copy(data, item.SourceEnd, next, item.SourceOffset + replacement.Length, data.Length - item.SourceEnd);
            item.FightData.Data = next;

            int delta = replacement.Length - oldLength;
            int oldEnd = item.SourceEnd;
            item.SourceEnd = item.SourceOffset + replacement.Length;
            if (delta != 0 && this.nodeView.SelectedNode != null && this.nodeView.SelectedNode.Parent != null)
            {
                foreach (TreeNode node in this.nodeView.SelectedNode.Parent.Nodes)
                {
                    var other = node.Tag as FightTrackTreeItem;
                    if (other != null && other != item && other.FightData == item.FightData && other.SourceOffset >= oldEnd)
                    {
                        other.SourceOffset += delta;
                        other.SourceEnd += delta;
                    }
                }
            }
        }

        private void OnAnimationGlbExport(object sender, EventArgs e)
        {
            if (this.nodeView.SelectedNode == null || !(this.nodeView.SelectedNode.Tag is Animation))
            {
                return;
            }

            if (this._ModelPreviewScene == null || this._ModelPreviewScene.BindBones == null || this._ModelPreviewScene.BindBones.Count == 0)
            {
                MessageBox.Show(this, "Select a PolySkin first so the exporter has the target skeleton and mesh.", "Export GLB", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var animation = (Animation)this.nodeView.SelectedNode.Tag;
            this._ExportAnimationGlbDialog.FileName = SanitizeFileName(string.IsNullOrEmpty(animation.Name) ? "animation" : animation.Name) + ".glb";
            if (this._ExportAnimationGlbDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (var output = File.Create(this._ExportAnimationGlbDialog.FileName))
                {
                    ModelPreviewBuilder.ExportAnimationGlb(output, this._ModelPreviewScene, animation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Export GLB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnAllAnimationsGlbExport(object sender, EventArgs e)
        {
            if (this.ActiveFile == null)
            {
                return;
            }

            if (this._ModelPreviewScene == null || this._ModelPreviewScene.BindBones == null || this._ModelPreviewScene.BindBones.Count == 0)
            {
                MessageBox.Show(this, "Select a PolySkin first so the exporter has the target skeleton and mesh.", "Export GLB", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var animations = Flatten(this.ActiveFile.Nodes).OfType<Animation>().ToList();
            if (animations.Count == 0)
            {
                MessageBox.Show(this, "No animation chunks are loaded.", "Export GLB", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ExportOptionsDialog options;
            if (this.ShowExportOptions(false, false, true, "Export All Animations", out options) == false)
            {
                return;
            }

            var folder = options.AnimationPath;
            if (string.IsNullOrEmpty(folder) == true)
            {
                return;
            }

            Directory.CreateDirectory(folder);
            int exported = 0;
            int skipped = 0;
            var failures = new List<string>();
            var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var animation in animations)
            {
                var fileName = MakeUniqueFileName(
                    usedNames,
                    SanitizeFileName(string.IsNullOrEmpty(animation.Name) ? "animation" : animation.Name),
                    ".glb");
                var path = Path.Combine(folder, fileName);

                try
                {
                    using (var output = File.Create(path))
                    {
                        ModelPreviewBuilder.ExportAnimationGlb(output, this._ModelPreviewScene, animation, options.RawRootTranslation);
                    }

                    exported++;
                }
                catch (InvalidOperationException ex)
                {
                    skipped++;
                    failures.Add(fileName + ": " + ex.Message);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch (Exception ex)
                {
                    skipped++;
                    failures.Add(fileName + ": " + ex.Message);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }

            var message = string.Format("Exported {0} animation(s). Skipped {1}.", exported, skipped);
            if (failures.Count > 0)
            {
                var visibleFailures = failures.Take(8).ToArray();
                message += Environment.NewLine + Environment.NewLine + string.Join(Environment.NewLine, visibleFailures);
                if (failures.Count > visibleFailures.Length)
                {
                    message += Environment.NewLine + "...";
                }
            }

            MessageBox.Show(this, message, "Export GLB", MessageBoxButtons.OK, skipped == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private static string SanitizeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }

            return string.IsNullOrEmpty(name) ? "animation" : name;
        }

        private static string MakeUniqueFileName(HashSet<string> usedNames, string baseName, string extension)
        {
            var candidate = baseName + extension;
            int index = 2;
            while (usedNames.Add(candidate) == false)
            {
                candidate = baseName + "_" + index + extension;
                index++;
            }

            return candidate;
        }

        private void ResolveVertexBufferItemBoneNames(Pure3DFile file, IEnumerable<string> sourceFileNames)
        {
            if (file == null)
            {
                return;
            }

            var nodes = Flatten(file.Nodes).ToList();
            var skeletonNodes = nodes.OfType<Skeleton>().ToList();
            foreach (var startup in FindStartupFiles(sourceFileNames))
            {
                skeletonNodes.AddRange(Flatten(startup.Nodes).OfType<Skeleton>());
            }

            var skeletons = skeletonNodes
                                 .Where(s => string.IsNullOrEmpty(s.Name) == false)
                                 .GroupBy(s => s.Name, StringComparer.OrdinalIgnoreCase)
                                 .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

            foreach (var primitiveGroup in nodes.OfType<U00010020_PrimitiveGroup>())
            {
                var polySkin = GetParentNode<PolySkin>(primitiveGroup);
                if (polySkin == null || string.IsNullOrEmpty(polySkin.SkeletonName))
                {
                    continue;
                }

                Skeleton skeleton;
                if (skeletons.TryGetValue(polySkin.SkeletonName, out skeleton) == false)
                {
                    continue;
                }

                var jointNames = skeleton.Children.OfType<SkeletonJoint>()
                                         .Take((int)skeleton.NumJoints)
                                         .Select(j => j.Name)
                                         .ToArray();
                var matrixPalette = ReadMatrixPalette(primitiveGroup.Children.OfType<PrimitiveMatrix>().FirstOrDefault());
                var vertexBuffers = primitiveGroup.Children.OfType<VertexBuffer>().ToList();
                foreach (var vertexBuffer in vertexBuffers)
                {
                    vertexBuffer.ResolveDescription();
                }

                var weightBuffer = FindVertexBuffer(vertexBuffers, VertexDescriptionType.Weight, 2);
                var groupBuffer = FindVertexBuffer(vertexBuffers, VertexDescriptionType.Group, 2);
                if (weightBuffer == null || groupBuffer == null)
                {
                    continue;
                }

                int count = (int)Math.Min(primitiveGroup.NumVertices, Math.Min(weightBuffer.VertexCount, groupBuffer.VertexCount));
                for (int vertex = 0; vertex < count; vertex++)
                {
                    var weightItem = weightBuffer.GetItem(vertex, VertexDescriptionType.Weight);
                    var group = groupBuffer.GetBytes(vertex, VertexDescriptionType.Group);
                    if (weightItem == null || group == null)
                    {
                        continue;
                    }

                    var boneNames = new string[Math.Min(3, group.Length)];
                    for (int i = 0; i < boneNames.Length; i++)
                    {
                        int boneIndex = ResolvePaletteBone(matrixPalette, group[i]);
                        if (boneIndex >= 0 && boneIndex < jointNames.Length)
                        {
                            boneNames[i] = jointNames[boneIndex];
                        }
                    }

                    weightItem.SetBoneNames(boneNames);
                }
            }
        }

        private static IEnumerable<Pure3DFile> FindStartupFiles(IEnumerable<string> sourceFileNames)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (sourceFileNames == null)
            {
                yield break;
            }

            foreach (var sourceFileName in sourceFileNames)
            {
                var startup = LoadStartupFile(sourceFileName, seen);
                if (startup != null)
                {
                    yield return startup;
                }
            }
        }

        private static Pure3DFile LoadStartupFile(string sourceFileName, HashSet<string> seen)
        {
            if (string.IsNullOrEmpty(sourceFileName) == true)
            {
                return null;
            }

            var directory = Path.GetDirectoryName(Path.GetFullPath(sourceFileName));
            while (string.IsNullOrEmpty(directory) == false)
            {
                var candidate = Path.Combine(directory, "startup.p3d");
                if (File.Exists(candidate) == true)
                {
                    var fullName = Path.GetFullPath(candidate);
                    if (seen.Add(fullName) == false)
                    {
                        return null;
                    }

                    using (var input = File.OpenRead(fullName))
                    {
                        var file = new Pure3DFile();
                        file.Deserialize(input);
                        return file;
                    }
                }

                var parent = Directory.GetParent(directory);
                directory = parent == null ? null : parent.FullName;
            }

            return null;
        }

        private static T GetParentNode<T>(FileFormats.Pure3D.BaseNode node) where T : FileFormats.Pure3D.BaseNode
        {
            var current = node == null ? null : node.ParentNode;
            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }

                current = current.ParentNode;
            }

            return null;
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

        private static IEnumerable<FileFormats.Pure3D.BaseNode> Flatten(IEnumerable<FileFormats.Pure3D.BaseNode> nodes)
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

        private bool IsModelExportNode(FileFormats.Pure3D.BaseNode node)
        {
            return node is PolySkin || node is Geometry || node is CompositeDrawable || node is P2PolySkinComposite || node is P2PolySkin || node is P2Primitive;
        }

        private bool IsTextureExportNode(FileFormats.Pure3D.BaseNode node)
        {
            return node is Texture || node is TextureDDS || node is TexturePNG || node is TextureData;
        }

        private ModelPreviewScene CreateModelExportScene(FileFormats.Pure3D.BaseNode node)
        {
            if (this.ActiveFile == null)
            {
                throw new InvalidOperationException("No P3D file is loaded.");
            }

            if (node is PolySkin)
            {
                return ModelPreviewBuilder.CreateScene(this.ActiveFile, (PolySkin)node, this.LastFileName);
            }

            if (node is Geometry)
            {
                return ModelPreviewBuilder.CreateScene(this.ActiveFile, (Geometry)node);
            }

            if (node is CompositeDrawable)
            {
                return ModelPreviewBuilder.CreateScene(this.ActiveFile, (CompositeDrawable)node, this.LastFileName);
            }

            if (node is P2PolySkinComposite)
            {
                return ModelPreviewBuilder.CreateScene(this.ActiveFile, (P2PolySkinComposite)node, this.LastFileName);
            }

            if (node is P2PolySkin)
            {
                return ModelPreviewBuilder.CreateScene(this.ActiveFile, (P2PolySkin)node, this.LastFileName);
            }

            if (node is P2Primitive)
            {
                return ModelPreviewBuilder.CreateScene(this.ActiveFile, (P2Primitive)node);
            }

            throw new InvalidOperationException("The selected node is not a model.");
        }

        private void ExportModelNode(FileFormats.Pure3D.BaseNode node)
        {
            if (this.IsModelExportNode(node) == false)
            {
                return;
            }

            this._ExportAnimationGlbDialog.FileName = SanitizeFileName(this.GetNodeDisplayName(node, "model")) + ".glb";
            this._ExportAnimationGlbDialog.DefaultExt = "glb";
            this._ExportAnimationGlbDialog.Filter = "Binary glTF (*.glb)|*.glb|All Files (*.*)|*.*";
            if (this._ExportAnimationGlbDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                var scene = this.CreateModelExportScene(node);
                using (var output = File.Create(this._ExportAnimationGlbDialog.FileName))
                {
                    ModelPreviewBuilder.ExportModelGlb(output, scene);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Export GLB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportSelectedObjectsToFolder()
        {
            if (this.ActiveFile == null)
            {
                return;
            }

            var modelNodes = this.GetSelectedModelNodes();
            var textureNodes = this.GetSelectedTextureNodes();
            var animationNodes = this.GetSelectedAnimationNodes();
            if (modelNodes.Count == 0 && textureNodes.Count == 0 && animationNodes.Count == 0)
            {
                return;
            }

            if (animationNodes.Count > 0 &&
                (this._ModelPreviewScene == null || this._ModelPreviewScene.BindBones == null || this._ModelPreviewScene.BindBones.Count == 0))
            {
                MessageBox.Show(this, "Select a PolySkin first so the animation exporter has the target skeleton and mesh.", "Export Selected Objects", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ExportOptionsDialog options;
            if (this.ShowExportOptions(textureNodes.Count > 0 || modelNodes.Count > 0, modelNodes.Count > 0, animationNodes.Count > 0, "Export Selected Objects", out options) == false)
            {
                return;
            }

            try
            {
                if (textureNodes.Count > 0 || modelNodes.Count > 0)
                {
                    Directory.CreateDirectory(options.TexturePath);
                }

                if (modelNodes.Count > 0)
                {
                    Directory.CreateDirectory(options.ModelPath);
                }

                if (animationNodes.Count > 0)
                {
                    Directory.CreateDirectory(options.AnimationPath);
                }

                var duplicateNames = new HashSet<string>(
                    modelNodes.GroupBy(n => this.GetNodeDisplayName(n, "model"), StringComparer.OrdinalIgnoreCase)
                              .Where(g => g.Count() > 1)
                              .Select(g => g.Key),
                    StringComparer.OrdinalIgnoreCase);
                var exportedTextures = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var modelNode in modelNodes)
                {
                    var scene = this.CreateModelExportScene(modelNode);
                    var modelName = this.GetNodeDisplayName(modelNode, "model");
                    if (duplicateNames.Contains(modelName) == true)
                    {
                        var source = this.GetNodeSourceFileStem(modelNode);
                        if (string.IsNullOrEmpty(source) == false)
                        {
                            modelName += "_" + source;
                        }
                    }

                    var path = this.GetExportPath(options.ModelPath, SanitizeFileName(modelName), ".glb");
                    using (var output = File.Create(path))
                    {
                        ModelPreviewBuilder.ExportModelGlb(output, scene);
                    }

                    this.ExportReferencedTextures(scene, options.TexturePath, exportedTextures);
                }

                foreach (var textureNode in textureNodes)
                {
                    this.ExportTextureNode(textureNode, options.TexturePath, exportedTextures);
                }

                var duplicateAnimationNames = new HashSet<string>(
                    animationNodes.GroupBy(n => this.GetNodeDisplayName(n, "animation"), StringComparer.OrdinalIgnoreCase)
                                  .Where(g => g.Count() > 1)
                                  .Select(g => g.Key),
                    StringComparer.OrdinalIgnoreCase);
                foreach (var animationNode in animationNodes)
                {
                    var animationName = this.GetNodeDisplayName(animationNode, "animation");
                    if (duplicateAnimationNames.Contains(animationName) == true)
                    {
                        var source = this.GetNodeSourceFileStem(animationNode);
                        if (string.IsNullOrEmpty(source) == false)
                        {
                            animationName += "_" + source;
                        }
                    }

                    var path = this.GetExportPath(options.AnimationPath, SanitizeFileName(animationName), ".glb");
                    using (var output = File.Create(path))
                    {
                        ModelPreviewBuilder.ExportAnimationGlb(output, this._ModelPreviewScene, animationNode, options.RawRootTranslation);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Export Selected Objects", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<Animation> GetSelectedAnimationNodes()
        {
            var nodes = this._PreviewSelectedAnimationNodes.Distinct().ToList();
            if (nodes.Count == 0 &&
                this.nodeView.SelectedNode != null &&
                this.nodeView.SelectedNode.Tag is Animation)
            {
                nodes.Add((Animation)this.nodeView.SelectedNode.Tag);
            }

            return nodes;
        }

        private List<FileFormats.Pure3D.BaseNode> GetSelectedModelNodes()
        {
            var nodes = this._PreviewSelectedModelNodes.Distinct().ToList();
            if (nodes.Count == 0 &&
                this.nodeView.SelectedNode != null &&
                this.nodeView.SelectedNode.Tag is FileFormats.Pure3D.BaseNode &&
                this.IsModelExportNode((FileFormats.Pure3D.BaseNode)this.nodeView.SelectedNode.Tag) == true)
            {
                nodes.Add((FileFormats.Pure3D.BaseNode)this.nodeView.SelectedNode.Tag);
            }

            return nodes;
        }

        private List<FileFormats.Pure3D.BaseNode> GetSelectedTextureNodes()
        {
            var nodes = this._PreviewSelectedTreeNodes.Select(n => n.Tag as FileFormats.Pure3D.BaseNode)
                                                      .Where(n => n != null && this.IsTextureExportNode(n) == true)
                                                      .Distinct()
                                                      .ToList();
            if (nodes.Count == 0 &&
                this.nodeView.SelectedNode != null &&
                this.nodeView.SelectedNode.Tag is FileFormats.Pure3D.BaseNode &&
                this.IsTextureExportNode((FileFormats.Pure3D.BaseNode)this.nodeView.SelectedNode.Tag) == true)
            {
                nodes.Add((FileFormats.Pure3D.BaseNode)this.nodeView.SelectedNode.Tag);
            }

            return nodes;
        }

        private string SelectExportFolder(string title)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = title;
                dialog.CheckFileExists = false;
                dialog.CheckPathExists = true;
                dialog.ValidateNames = false;
                dialog.FileName = "Select Folder";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    return Path.GetDirectoryName(dialog.FileName);
                }
            }

            return null;
        }

        private bool ShowExportOptions(bool texturesEnabled, bool modelsEnabled, bool animationsEnabled, string title, out ExportOptionsDialog options)
        {
            var defaultPath = string.IsNullOrEmpty(this.LastFileName) == false
                                  ? Path.GetDirectoryName(Path.GetFullPath(this.LastFileName))
                                  : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            options = new ExportOptionsDialog(texturesEnabled, modelsEnabled, animationsEnabled, defaultPath)
            {
                Text = title,
            };

            if (options.ShowDialog(this) != DialogResult.OK)
            {
                options.Dispose();
                options = null;
                return false;
            }

            if (texturesEnabled == true && string.IsNullOrEmpty(options.TexturePath) == true ||
                modelsEnabled == true && string.IsNullOrEmpty(options.ModelPath) == true ||
                animationsEnabled == true && string.IsNullOrEmpty(options.AnimationPath) == true)
            {
                MessageBox.Show(this, "Choose an export path for each enabled export type.", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                options.Dispose();
                options = null;
                return false;
            }

            return true;
        }

        private string GetNodeSourceFileStem(FileFormats.Pure3D.BaseNode node)
        {
            string source;
            return this._NodeSourceFiles.TryGetValue(node, out source) == true && string.IsNullOrEmpty(source) == false
                       ? SanitizeFileName(Path.GetFileNameWithoutExtension(source))
                       : null;
        }

        private string GetUniquePath(string folder, string name, string extension)
        {
            var path = Path.Combine(folder, name + extension);
            int index = 2;
            while (File.Exists(path) == true)
            {
                path = Path.Combine(folder, name + "_" + index.ToString(System.Globalization.CultureInfo.InvariantCulture) + extension);
                index++;
            }

            return path;
        }

        private string GetExportPath(string folder, string name, string extension)
        {
            return Path.Combine(folder, name + extension);
        }

        private void ExportReferencedTextures(ModelPreviewScene scene, string folder, HashSet<string> exported)
        {
            if (scene == null || scene.Materials == null)
            {
                return;
            }

            foreach (var material in scene.Materials)
            {
                if (material == null)
                {
                    continue;
                }

                this.ExportReferencedTexture(material.TextureName, folder, exported);
                this.ExportReferencedTexture(material.NormalTextureName, folder, exported);
                this.ExportReferencedTexture(material.SpecularTextureName, folder, exported);
            }
        }

        private void ExportReferencedTexture(string textureName, string folder, HashSet<string> exported)
        {
            if (string.IsNullOrEmpty(textureName) == true || string.IsNullOrEmpty(folder) == true)
            {
                return;
            }

            var texture = Flatten(this.ActiveFile.Nodes).OfType<Texture>()
                                                        .FirstOrDefault(t => string.Equals(t.Name, textureName, StringComparison.OrdinalIgnoreCase));
            if (texture != null)
            {
                this.ExportTextureNode(texture, folder, exported);
            }
        }

        private void ExportTextureNode(FileFormats.Pure3D.BaseNode node, string folder, HashSet<string> exported)
        {
            if (node == null || folder == null || exported == null)
            {
                return;
            }

            var exportNode = this.ResolveTextureExportNode(node);
            if (exportNode == null || exportNode.Exportable == false)
            {
                return;
            }

            var name = this.GetNodeDisplayName(exportNode, this.GetNodeDisplayName(node, "texture"));
            var extension = exportNode is TexturePNG ? ".png" : ".dds";
            var key = name + extension;
            if (exported.Add(key) == false)
            {
                return;
            }

            var path = this.GetExportPath(folder, SanitizeFileName(Path.GetFileNameWithoutExtension(name)), extension);
            using (var output = File.Create(path))
            {
                exportNode.Export(output);
            }
        }

        private FileFormats.Pure3D.BaseNode ResolveTextureExportNode(FileFormats.Pure3D.BaseNode node)
        {
            var texture = node as Texture;
            if (texture != null)
            {
                var dds = texture.Children.OfType<TextureDDS>().FirstOrDefault();
                if (dds != null)
                {
                    return dds;
                }

                var png = texture.Children.OfType<TexturePNG>().FirstOrDefault();
                return png;
            }

            var data = node as TextureData;
            if (data != null)
            {
                return data.ParentNode is TextureDDS || data.ParentNode is TexturePNG ? data.ParentNode : data;
            }

            return node is TextureDDS || node is TexturePNG ? node : null;
        }

        private void ConfigureNodeExportDialog(FileFormats.Pure3D.BaseNode node)
        {
            string name = this.GetNodeDisplayName(node, "node");
            string extension = ".bin";
            string filter = "All Files (*.*)|*.*";

            if (node is Texture || node is TextureDDS || node is TexturePNG || node is TextureData)
            {
                var textureDds = node as TextureDDS;
                var texturePng = node as TexturePNG;
                var texture = node as Texture;
                var textureData = node as TextureData;

                if (texture != null)
                {
                    var dds = texture.Children.OfType<TextureDDS>().FirstOrDefault();
                    var png = texture.Children.OfType<TexturePNG>().FirstOrDefault();
                    if (dds != null)
                    {
                        textureDds = dds;
                    }
                    else if (png != null)
                    {
                        texturePng = png;
                    }
                }
                else if (textureData != null)
                {
                    textureDds = textureData.ParentNode as TextureDDS;
                    texturePng = textureData.ParentNode as TexturePNG;
                }

                if (textureDds != null)
                {
                    name = string.IsNullOrEmpty(textureDds.Name) ? name : textureDds.Name;
                    extension = ".dds";
                    filter = "DDS Texture (*.dds)|*.dds|All Files (*.*)|*.*";
                }
                else if (texturePng != null)
                {
                    name = string.IsNullOrEmpty(texturePng.Name) ? name : texturePng.Name;
                    extension = ".png";
                    filter = "PNG Texture (*.png)|*.png|All Files (*.*)|*.*";
                }
            }

            this.exportNodeFileDialog.DefaultExt = extension.TrimStart('.');
            this.exportNodeFileDialog.Filter = filter;
            this.exportNodeFileDialog.FileName = SanitizeFileName(Path.GetFileNameWithoutExtension(name)) + extension;
        }

        private string GetNodeDisplayName(FileFormats.Pure3D.BaseNode node, string fallback)
        {
            if (node is PolySkin && string.IsNullOrEmpty(((PolySkin)node).Name) == false)
            {
                return ((PolySkin)node).Name;
            }

            if (node is Geometry && string.IsNullOrEmpty(((Geometry)node).Name) == false)
            {
                return ((Geometry)node).Name;
            }

            if (node is CompositeDrawable && string.IsNullOrEmpty(((CompositeDrawable)node).Name) == false)
            {
                return ((CompositeDrawable)node).Name;
            }

            if (node is P2PolySkinComposite && string.IsNullOrEmpty(((P2PolySkinComposite)node).Name) == false)
            {
                return ((P2PolySkinComposite)node).Name;
            }

            if (node is Texture && string.IsNullOrEmpty(((Texture)node).Name) == false)
            {
                return ((Texture)node).Name;
            }

            if (node is TextureDDS && string.IsNullOrEmpty(((TextureDDS)node).Name) == false)
            {
                return ((TextureDDS)node).Name;
            }

            if (node is TexturePNG && string.IsNullOrEmpty(((TexturePNG)node).Name) == false)
            {
                return ((TexturePNG)node).Name;
            }

            if (node is Animation && string.IsNullOrEmpty(((Animation)node).Name) == false)
            {
                return ((Animation)node).Name;
            }

            return fallback;
        }

        private void OnNodeExport(object sender, EventArgs e)
        {
            if (this.propertyGrid.SelectedObject == null ||
                !(this.propertyGrid.SelectedObject is FileFormats.Pure3D.BaseNode))
            {
                return;
            }

            var node = (FileFormats.Pure3D.BaseNode)this.propertyGrid.SelectedObject;
            if (this.IsModelExportNode(node) == true)
            {
                this.ExportModelNode(node);
                return;
            }

            if (node.Exportable == false)
            {
                throw new InvalidOperationException();
            }

            this.ConfigureNodeExportDialog(node);
            if (this.exportNodeFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (var output = this.exportNodeFileDialog.OpenFile())
            {
                node.Export(output);
            }
        }

        private void OnNodeImport(object sender, EventArgs e)
        {
            if (this.propertyGrid.SelectedObject == null ||
                !(this.propertyGrid.SelectedObject is FileFormats.Pure3D.BaseNode))
            {
                return;
            }

            var node = (FileFormats.Pure3D.BaseNode)this.propertyGrid.SelectedObject;
            var animation = node as Animation;
            if (animation != null)
            {
                this.ImportAnimationFromGltf(animation);
                return;
            }

            if (node.Importable == false)
            {
                throw new InvalidOperationException();
            }

            if (this.importNodeFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (var input = this.importNodeFileDialog.OpenFile())
            {
                node.Import(input);
            }
        }

        private void ImportAnimationFromGltf(Animation animation)
        {
            using (var dialog = new OpenFileDialog
            {
                DefaultExt = "gltf",
                Filter = "glTF Animation (*.gltf;*.glb)|*.gltf;*.glb|All Files (*.*)|*.*",
                Multiselect = false,
                Title = "Import Animation GLTF",
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                using (var options = new AnimationImportOptionsDialog())
                {
                    if (options.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    try
                    {
                        var count = GltfAnimationImporter.ImportInto(animation, dialog.FileName, options.SelectedPreset);
                        this.propertyGrid.Refresh();
                        if (this.nodeView.SelectedNode != null && object.ReferenceEquals(this.nodeView.SelectedNode.Tag, animation))
                        {
                            this.nodeView.SelectedNode.Text = animation.ToString();
                        }

                        if (this._ModelPreviewScene != null)
                        {
                            this._ModelPreviewAnimation = ModelPreviewBuilder.CreateAnimation(this._ModelPreviewScene, animation);
                            this._PreviewAnimationPausedFrame = 0.0f;
                        }

                        MessageBox.Show(
                            this,
                            string.Format(System.Globalization.CultureInfo.InvariantCulture, "Imported animation data for {0} bone(s).", count),
                            "Import Animation",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "Import Animation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void OnSelectedNodeRawSave(object sender, EventArgs e)
        {
            if (this.ActiveFile == null || string.IsNullOrEmpty(this.LastFileName) == true)
            {
                MessageBox.Show(this, "Open a single P3D file before saving a selected chunk.", "Save Node", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.nodeView.SelectedNode == null || !(this.nodeView.SelectedNode.Tag is FileFormats.Pure3D.BaseNode))
            {
                MessageBox.Show(this, "Select a chunk node first.", "Save Node", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selected = (FileFormats.Pure3D.BaseNode)this.nodeView.SelectedNode.Tag;
            try
            {
                var animation = selected as Animation;
                if (animation != null)
                {
                    NormalizeAnimationBoneParenting(animation);
                }

                SaveSelectedNodeRawPreserving(this.ActiveFile, this.LastFileName, selected);
                MessageBox.Show(this, "Selected chunk saved. Other chunks were copied from the original file bytes.", "Save Node", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Save Node", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void NormalizeAnimationBoneParenting(Animation animation)
        {
            if (animation == null)
            {
                return;
            }

            var animationGroup = animation.Children.OfType<AnimationGroup>().FirstOrDefault();
            if (animationGroup == null)
            {
                animationGroup = new AnimationGroup();
                animation.Children.Add(animationGroup);
            }

            var directBones = animation.Children.OfType<Bone>().ToList();
            foreach (var bone in directBones)
            {
                animation.Children.Remove(bone);
            }

            if (directBones.Count > 0)
            {
                animationGroup.Children.Clear();
                animationGroup.Children.AddRange(directBones.Cast<FileFormats.Pure3D.BaseNode>());
            }

            animationGroup.NumGroups = (uint)animationGroup.Children.OfType<Bone>().Count();
            animationGroup.ParentNode = animation;
            foreach (var bone in animationGroup.Children.OfType<Bone>())
            {
                bone.ParentNode = animationGroup;
            }

            foreach (var animationHeader in animation.Children.OfType<U00121006>())
            {
                animationHeader.Unknown2 = animationGroup.NumGroups;
                if (animation.Children.OfType<AnimationData>().Any() == false)
                {
                    RebuildInlineAnimationHeader(animationHeader, animationGroup.Children.OfType<Bone>());
                }
            }

            NormalizeInlineAnimationChannels(animationGroup, animation.Children.OfType<AnimationData>().Any());
        }

        private static void NormalizeInlineAnimationChannels(FileFormats.Pure3D.BaseNode node, bool hasExternalAnimationData)
        {
            if (node == null)
            {
                return;
            }

            foreach (var child in node.Children.ToList())
            {
                var channel = child as AnimationChannel;
                if (channel != null)
                {
                    if (hasExternalAnimationData == false)
                    {
                        foreach (var reference in channel.Children.OfType<AnimationDataReference>().ToList())
                        {
                            channel.Children.Remove(reference);
                        }
                    }

                    if (channel.Children.OfType<BoneInterpolation>().Any() == false)
                    {
                        var interpolation = new BoneInterpolation
                        {
                            Version = 0,
                            Interpolate = uint.MaxValue,
                            ParentNode = channel,
                        };
                        channel.Children.Insert(0, interpolation);
                    }
                }

                NormalizeInlineAnimationChannels(child, hasExternalAnimationData);
            }
        }

        private static void SaveSelectedNodeRawPreserving(Pure3DFile file, string fileName, FileFormats.Pure3D.BaseNode selected)
        {
            if (file == null || selected == null)
            {
                throw new ArgumentNullException();
            }

            var source = File.ReadAllBytes(fileName);
            var selectedPath = new HashSet<FileFormats.Pure3D.BaseNode>();
            for (var node = selected; node != null; node = node.ParentNode)
            {
                selectedPath.Add(node);
            }

            using (var output = new MemoryStream())
            {
                WriteU32(output, Pure3DFile.Signature);
                WriteU32(output, 12);
                WriteU32(output, 0);
                foreach (var node in file.Nodes)
                {
                    WriteRawPreservingNode(output, source, node, selected, selectedPath);
                }

                var totalSize = output.Length;
                output.Position = 8;
                WriteU32(output, (uint)totalSize);
                File.WriteAllBytes(fileName, output.ToArray());
            }
        }

        private static void WriteRawPreservingNode(
            Stream output,
            byte[] source,
            FileFormats.Pure3D.BaseNode node,
            FileFormats.Pure3D.BaseNode selected,
            ISet<FileFormats.Pure3D.BaseNode> selectedPath)
        {
            if (object.ReferenceEquals(node, selected) == true)
            {
                WriteSerializedNode(output, source, node);
                return;
            }

            if (selectedPath.Contains(node) == false)
            {
                output.Write(source, (int)node.StartPosition, (int)node.TotalSize);
                return;
            }

            using (var payload = new MemoryStream())
            using (var children = new MemoryStream())
            {
                var payloadOffset = (int)node.StartPosition + 12;
                var payloadLength = (int)node.HeaderSize - 12;
                if (payloadLength > 0)
                {
                    payload.Write(source, payloadOffset, payloadLength);
                }

                foreach (var child in node.Children)
                {
                    WriteRawPreservingNode(children, source, child, selected, selectedPath);
                }

                WriteU32(output, node.TypeId);
                WriteU32(output, (uint)(12 + payload.Length));
                WriteU32(output, (uint)(12 + payload.Length + children.Length));
                payload.Position = 0;
                payload.CopyTo(output);
                children.Position = 0;
                children.CopyTo(output);
            }
        }

        private static void WriteSerializedNode(Stream output, byte[] source, FileFormats.Pure3D.BaseNode node)
        {
            if (node is FileFormats.Pure3D.Unknown && CanCopyOriginalNode(source, node) == true)
            {
                output.Write(source, (int)node.StartPosition, (int)node.TotalSize);
                return;
            }

            using (var payload = new MemoryStream())
            using (var children = new MemoryStream())
            {
                var unknown = node as FileFormats.Pure3D.Unknown;
                if (unknown != null)
                {
                    if (unknown.Data != null && unknown.Data.Length > 0)
                    {
                        payload.Write(unknown.Data, 0, unknown.Data.Length);
                    }
                }
                else
                {
                    node.Serialize(payload);
                }

                foreach (var child in node.Children)
                {
                    WriteSerializedNode(children, source, child);
                }

                WriteU32(output, node.TypeId);
                WriteU32(output, (uint)(12 + payload.Length));
                WriteU32(output, (uint)(12 + payload.Length + children.Length));
                payload.Position = 0;
                payload.CopyTo(output);
                children.Position = 0;
                children.CopyTo(output);
            }
        }

        private static void RebuildInlineAnimationHeader(U00121006 header, IEnumerable<Bone> bones)
        {
            if (header == null)
            {
                return;
            }

            var children = new List<FileFormats.Pure3D.BaseNode>();
            var marker = header.Children.FirstOrDefault(c => c.TypeId == 0x00121400);
            if (marker == null)
            {
                marker = new FileFormats.Pure3D.Unknown(0x00121400)
                {
                    Data = new byte[] { 0, 0, 0, 0, 4, 0, 0, 0 },
                };
            }

            marker.ParentNode = header;
            children.Add(marker);

            foreach (var group in bones.SelectMany(b => b.Children.OfType<AnimationChannel>())
                                      .GroupBy(c => c.TypeId)
                                      .OrderBy(g => g.Key))
            {
                var data = new List<byte>();
                AppendU32(data, 0);
                AppendU32(data, group.Key);
                AppendU32(data, (uint)group.Count());
                foreach (var channel in group)
                {
                    AppendU16(data, (ushort)Math.Min(ushort.MaxValue, channel.NumberOfFrames));
                }

                children.Add(new FileFormats.Pure3D.Unknown(0x00121007)
                {
                    Data = data.ToArray(),
                    ParentNode = header,
                });
            }

            header.Children.Clear();
            header.Children.AddRange(children);
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

        private static bool CanCopyOriginalNode(byte[] source, FileFormats.Pure3D.BaseNode node)
        {
            if (source == null || node == null || node.TotalSize == 0)
            {
                return false;
            }

            return node.StartPosition <= int.MaxValue &&
                   node.TotalSize <= int.MaxValue &&
                   node.StartPosition + node.TotalSize <= source.Length;
        }

        private static void WriteU32(Stream output, uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            output.Write(bytes, 0, bytes.Length);
        }

        private sealed class VertexBufferItemList
        {
            public VertexBufferItemList(VertexBuffer vertexBuffer)
            {
                this.VertexBuffer = vertexBuffer;
            }

            public VertexBuffer VertexBuffer { get; private set; }

            public override string ToString()
            {
                return "Vertices (" + this.VertexBuffer.VertexCount + ")";
            }
        }

        private sealed class VertexBufferVertexItems
        {
            public VertexBufferVertexItems(VertexBuffer vertexBuffer, int vertexIndex)
            {
                this.VertexBuffer = vertexBuffer;
                this.VertexIndex = vertexIndex;
            }

            public VertexBuffer VertexBuffer { get; private set; }
            public int VertexIndex { get; private set; }

            public override string ToString()
            {
                return "Vertex " + this.VertexIndex;
            }
        }

        private sealed class FightTrackItemList
        {
            public FightTrackItemList(FileFormats.Pure3D.FightData fightData)
            {
                this.FightData = fightData;
            }

            public FileFormats.Pure3D.FightData FightData { get; private set; }

            public override string ToString()
            {
                return "Fight Tracks";
            }
        }

        private sealed class FightTrackTreeItem
        {
            public FightTrackTreeItem(FileFormats.Pure3D.FightData fightData, Nixson.Prototype.Fight.BaseTrack track, int sourceOffset, int sourceEnd)
            {
                this.FightData = fightData;
                this.Track = track;
                this.SourceOffset = sourceOffset;
                this.SourceEnd = sourceEnd;
            }

            public FileFormats.Pure3D.FightData FightData { get; private set; }
            public Nixson.Prototype.Fight.BaseTrack Track { get; private set; }
            public int SourceOffset { get; set; }
            public int SourceEnd { get; set; }

            public override string ToString()
            {
                return this.Track == null ? "Fight Track" : this.Track.ToString();
            }
        }

        private sealed class LuaTrackEditDialog : Form
        {
            private readonly TextBox _TextBox;

            public LuaTrackEditDialog(string text)
            {
                this.Text = "Edit Lua Script";
                this.StartPosition = FormStartPosition.CenterParent;
                this.Width = 900;
                this.Height = 650;
                this.MinimizeBox = false;
                this.ShowIcon = false;

                this._TextBox = new TextBox
                {
                    AcceptsReturn = true,
                    AcceptsTab = true,
                    Dock = DockStyle.Fill,
                    Font = new Font(FontFamily.GenericMonospace, 9.0f),
                    Multiline = true,
                    ScrollBars = ScrollBars.Both,
                    Text = text ?? string.Empty,
                    WordWrap = false,
                };

                var buttons = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom,
                    FlowDirection = FlowDirection.RightToLeft,
                    Height = 36,
                    Padding = new Padding(4),
                };

                var okButton = new Button
                {
                    DialogResult = DialogResult.OK,
                    Text = "OK",
                    Width = 80,
                };
                var cancelButton = new Button
                {
                    DialogResult = DialogResult.Cancel,
                    Text = "Cancel",
                    Width = 80,
                };
                buttons.Controls.Add(okButton);
                buttons.Controls.Add(cancelButton);

                this.AcceptButton = okButton;
                this.CancelButton = cancelButton;
                this.Controls.Add(this._TextBox);
                this.Controls.Add(buttons);
            }

            public string ScriptText
            {
                get { return this._TextBox.Text; }
            }
        }

        private sealed class CutscenePlaybackEvent
        {
            public CutsceneTimelineEvent TimelineEvent;
            public ModelPreviewAnimation Animation;
            public ModelPreviewCameraAnimation CameraAnimation;
            public ModelPreviewSceneInstance Instance;
        }
    }
}
