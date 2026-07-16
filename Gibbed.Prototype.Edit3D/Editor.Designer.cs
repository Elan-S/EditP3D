namespace Gibbed.Prototype.Edit3D
{
    partial class Editor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing && (this._PreviewRenderTimer != null))
            {
                this._PreviewRenderTimer.Dispose();
            }
            if (disposing && (this._NodeContextMenu != null))
            {
                this._NodeContextMenu.Dispose();
            }
            if (disposing && (this._ExportAnimationGlbDialog != null))
            {
                this._ExportAnimationGlbDialog.Dispose();
            }
            if (disposing && (this._ExportAllAnimationsDialog != null))
            {
                this._ExportAllAnimationsDialog.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.nodeView = new System.Windows.Forms.TreeView();
            this.nodeFilterTextBox = new System.Windows.Forms.TextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.nodeToolStrip = new System.Windows.Forms.ToolStrip();
            this.editorToolStrip = new System.Windows.Forms.ToolStrip();
            this.openPure3DFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.exportNodeFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.importNodeFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._ExportAllAnimationsDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.exportNodeButton = new System.Windows.Forms.ToolStripButton();
            this.importNodeButton = new System.Windows.Forms.ToolStripButton();
            this.newFileButton = new System.Windows.Forms.ToolStripButton();
            this.openFileButton = new System.Windows.Forms.ToolStripButton();
            this.addFileButton = new System.Windows.Forms.ToolStripButton();
            this.saveButtonSeparatorLeft = new System.Windows.Forms.ToolStripSeparator();
            this.saveFileButton = new System.Windows.Forms.ToolStripButton();
            this._SaveSelectedNodeButton = new System.Windows.Forms.ToolStripButton();
            this.saveButtonSeparatorRight = new System.Windows.Forms.ToolStripSeparator();
            this.exportAllAnimationsButton = new System.Windows.Forms.ToolStripButton();
            this._OpenCutsceneButton = new System.Windows.Forms.ToolStripButton();
            this.exportHelpLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.nodeToolStrip.SuspendLayout();
            this.editorToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.nodeView);
            this.splitContainer1.Panel1.Controls.Add(this.nodeFilterTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(784, 539);
            this.splitContainer1.SplitterDistance = 261;
            this.splitContainer1.TabIndex = 0;
            // 
            // nodeView
            // 
            this.nodeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeView.Location = new System.Drawing.Point(0, 20);
            this.nodeView.Name = "nodeView";
            this.nodeView.Size = new System.Drawing.Size(261, 519);
            this.nodeView.TabIndex = 0;
            this.nodeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnSelectNode);
            // 
            // nodeFilterTextBox
            // 
            this.nodeFilterTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.nodeFilterTextBox.Location = new System.Drawing.Point(0, 0);
            this.nodeFilterTextBox.Name = "nodeFilterTextBox";
            this.nodeFilterTextBox.Size = new System.Drawing.Size(261, 20);
            this.nodeFilterTextBox.TabIndex = 1;
            this.nodeFilterTextBox.TextChanged += new System.EventHandler(this.OnNodeFilterTextChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.previewPicture);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer2.Panel2.Controls.Add(this.nodeToolStrip);
            this.splitContainer2.Size = new System.Drawing.Size(519, 539);
            this.splitContainer2.SplitterDistance = 300;
            this.splitContainer2.TabIndex = 0;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 25);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(519, 210);
            this.propertyGrid.TabIndex = 0;
            // 
            // nodeToolStrip
            // 
            this.nodeToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportNodeButton,
            this.importNodeButton});
            this.nodeToolStrip.Location = new System.Drawing.Point(0, 0);
            this.nodeToolStrip.Name = "nodeToolStrip";
            this.nodeToolStrip.Size = new System.Drawing.Size(519, 25);
            this.nodeToolStrip.TabIndex = 1;
            this.nodeToolStrip.Text = "toolStrip2";
            // 
            // editorToolStrip
            // 
            this.editorToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileButton,
            this.openFileButton,
            this.addFileButton,
            this.saveButtonSeparatorLeft,
            this.saveFileButton,
            this._SaveSelectedNodeButton,
            this.saveButtonSeparatorRight,
            this.exportAllAnimationsButton,
            this._OpenCutsceneButton});
            this.editorToolStrip.Location = new System.Drawing.Point(0, 0);
            this.editorToolStrip.Name = "editorToolStrip";
            this.editorToolStrip.Size = new System.Drawing.Size(784, 25);
            this.editorToolStrip.TabIndex = 1;
            this.editorToolStrip.Text = "toolStrip1";
            // 
            // openPure3DFileDialog
            // 
            this.openPure3DFileDialog.DefaultExt = "p3d";
            this.openPure3DFileDialog.Filter = "Pure3D Files (*.p3d)|*.p3d|All Files (*.*)|*.*";
            this.openPure3DFileDialog.Multiselect = true;
            // 
            // exportNodeFileDialog
            // 
            this.exportNodeFileDialog.Filter = "All Files (*.*)|*.*";
            // 
            // importNodeFileDialog
            // 
            this.importNodeFileDialog.Filter = "All Files (*.*)|*.*";
            // 
            // previewPicture
            // 
            this.previewPicture.BackColor = System.Drawing.Color.Black;
            this.previewPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewPicture.Location = new System.Drawing.Point(0, 0);
            this.previewPicture.Name = "previewPicture";
            this.previewPicture.Size = new System.Drawing.Size(519, 300);
            this.previewPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.previewPicture.TabIndex = 0;
            this.previewPicture.TabStop = false;
            this.previewPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnPreviewMouseDown);
            this.previewPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnPreviewMouseMove);
            this.previewPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnPreviewMouseUp);
            this.previewPicture.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.OnPreviewMouseWheel);
            this.previewPicture.Resize += new System.EventHandler(this.OnPreviewResize);
            // 
            // exportNodeButton
            // 
            this.exportNodeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exportNodeButton.Image = global::Gibbed.Prototype.Edit3D.Properties.Resources.usb_stick_blue;
            this.exportNodeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportNodeButton.Name = "exportNodeButton";
            this.exportNodeButton.Size = new System.Drawing.Size(23, 22);
            this.exportNodeButton.Text = "Export Node Data";
            this.exportNodeButton.Click += new System.EventHandler(this.OnNodeExport);
            // 
            // importNodeButton
            // 
            this.importNodeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.importNodeButton.Image = global::Gibbed.Prototype.Edit3D.Properties.Resources.usb_stick_orange;
            this.importNodeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importNodeButton.Name = "importNodeButton";
            this.importNodeButton.Size = new System.Drawing.Size(23, 22);
            this.importNodeButton.Text = "Import Node Data";
            this.importNodeButton.Click += new System.EventHandler(this.OnNodeImport);
            // 
            // newFileButton
            // 
            this.newFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newFileButton.Enabled = false;
            this.newFileButton.Image = global::Gibbed.Prototype.Edit3D.Properties.Resources.document_new;
            this.newFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newFileButton.Name = "newFileButton";
            this.newFileButton.Size = new System.Drawing.Size(23, 22);
            this.newFileButton.Text = "New Pure3D File";
            // 
            // openFileButton
            // 
            this.openFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openFileButton.Image = global::Gibbed.Prototype.Edit3D.Properties.Resources.folder;
            this.openFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(23, 22);
            this.openFileButton.Text = "Open Pure3D File";
            this.openFileButton.Click += new System.EventHandler(this.OnFileOpen);
            // 
            // addFileButton
            // 
            this.addFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addFileButton.Image = global::Gibbed.Prototype.Edit3D.Properties.Resources.folderadd;
            this.addFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addFileButton.Name = "addFileButton";
            this.addFileButton.Size = new System.Drawing.Size(23, 22);
            this.addFileButton.Text = "Add Pure3D File(s) to Current Selection";
            this.addFileButton.Click += new System.EventHandler(this.OnFileAdd);
            // 
            // saveButtonSeparatorLeft
            // 
            this.saveButtonSeparatorLeft.Name = "saveButtonSeparatorLeft";
            this.saveButtonSeparatorLeft.Size = new System.Drawing.Size(6, 25);
            // 
            // saveFileButton
            // 
            this.saveFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveFileButton.Image = global::Gibbed.Prototype.Edit3D.Properties.Resources.floppy;
            this.saveFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveFileButton.Name = "saveFileButton";
            this.saveFileButton.Size = new System.Drawing.Size(23, 22);
            this.saveFileButton.Text = "Save Pure3D File";
            this.saveFileButton.Click += new System.EventHandler(this.OnFileSave);
            // 
            // _SaveSelectedNodeButton
            // 
            this._SaveSelectedNodeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._SaveSelectedNodeButton.Image = global::Gibbed.Prototype.Edit3D.Properties.Resources.saveNode;
            this._SaveSelectedNodeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._SaveSelectedNodeButton.Name = "_SaveSelectedNodeButton";
            this._SaveSelectedNodeButton.Size = new System.Drawing.Size(23, 22);
            this._SaveSelectedNodeButton.Text = "Save Node";
            this._SaveSelectedNodeButton.ToolTipText = "Save only the currently selected chunk, preserving all other chunk bytes";
            this._SaveSelectedNodeButton.Click += new System.EventHandler(this.OnSelectedNodeRawSave);
            // 
            // saveButtonSeparatorRight
            // 
            this.saveButtonSeparatorRight.Name = "saveButtonSeparatorRight";
            this.saveButtonSeparatorRight.Size = new System.Drawing.Size(6, 25);
            // 
            // exportAllAnimationsButton
            // 
            this.exportAllAnimationsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exportAllAnimationsButton.Enabled = false;
            this.exportAllAnimationsButton.Image = global::Gibbed.Prototype.Edit3D.Properties.Resources.exportanim;
            this.exportAllAnimationsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportAllAnimationsButton.Name = "exportAllAnimationsButton";
            this.exportAllAnimationsButton.Size = new System.Drawing.Size(23, 22);
            this.exportAllAnimationsButton.Text = "Export All Animations as GLB";
            this.exportAllAnimationsButton.Click += new System.EventHandler(this.OnAllAnimationsGlbExport);
            // 
            // _OpenCutsceneButton
            // 
            this._OpenCutsceneButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._OpenCutsceneButton.Image = global::Gibbed.Prototype.Edit3D.Properties.Resources.cutscene;
            this._OpenCutsceneButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._OpenCutsceneButton.Name = "_OpenCutsceneButton";
            this._OpenCutsceneButton.Size = new System.Drawing.Size(23, 22);
            this._OpenCutsceneButton.Text = "Cutscene Viewer";
            this._OpenCutsceneButton.ToolTipText = "Cutscene Viewer: Open a cutscene P3D ending with \"_fig\"";
            this._OpenCutsceneButton.Click += new System.EventHandler(this.OnCutsceneOpen);
            // 
            // Editor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 564);
            // 
            // exportHelpLabel
            // 
            this.exportHelpLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exportHelpLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.exportHelpLabel.Location = new System.Drawing.Point(0, 542);
            this.exportHelpLabel.Name = "exportHelpLabel";
            this.exportHelpLabel.Padding = new System.Windows.Forms.Padding(6, 2, 0, 0);
            this.exportHelpLabel.Size = new System.Drawing.Size(784, 22);
            this.exportHelpLabel.TabIndex = 2;
            this.exportHelpLabel.Text = "Tip: select nodes or a category, then press Ctrl+X to export with options.";
            this.exportHelpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.exportHelpLabel);
            this.Controls.Add(this.editorToolStrip);
            this.KeyPreview = true;
            this.Name = "Editor";
            this.Text = "Edit3D";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnEditorDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnEditorDragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEditorKeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.nodeToolStrip.ResumeLayout(false);
            this.nodeToolStrip.PerformLayout();
            this.editorToolStrip.ResumeLayout(false);
            this.editorToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView nodeView;
        private System.Windows.Forms.TextBox nodeFilterTextBox;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ToolStrip nodeToolStrip;
        private System.Windows.Forms.ToolStrip editorToolStrip;
        private System.Windows.Forms.ToolStripButton newFileButton;
        private System.Windows.Forms.ToolStripButton openFileButton;
        private System.Windows.Forms.ToolStripButton addFileButton;
        private System.Windows.Forms.ToolStripSeparator saveButtonSeparatorLeft;
        private System.Windows.Forms.ToolStripButton saveFileButton;
        private System.Windows.Forms.ToolStripButton _SaveSelectedNodeButton;
        private System.Windows.Forms.ToolStripSeparator saveButtonSeparatorRight;
        private System.Windows.Forms.ToolStripButton exportAllAnimationsButton;
        private System.Windows.Forms.ToolStripButton _OpenCutsceneButton;
        private System.Windows.Forms.ToolStripButton exportNodeButton;
        private System.Windows.Forms.ToolStripButton importNodeButton;
        private System.Windows.Forms.PictureBox previewPicture;
        private System.Windows.Forms.OpenFileDialog openPure3DFileDialog;
        private System.Windows.Forms.SaveFileDialog exportNodeFileDialog;
        private System.Windows.Forms.OpenFileDialog importNodeFileDialog;
        private System.Windows.Forms.FolderBrowserDialog _ExportAllAnimationsDialog;
        private System.Windows.Forms.Label exportHelpLabel;
    }
}

