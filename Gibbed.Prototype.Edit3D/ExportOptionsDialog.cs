using System;
using System.IO;
using System.Windows.Forms;

namespace Gibbed.Prototype.Edit3D
{
    internal sealed class ExportOptionsDialog : Form
    {
        private readonly TextBox _TexturePathTextBox;
        private readonly TextBox _ModelPathTextBox;
        private readonly TextBox _AnimationPathTextBox;
        private readonly ComboBox _TextureFormatComboBox;
        private readonly ComboBox _ModelFormatComboBox;
        private readonly CheckBox _RawRootTranslationCheckBox;
        private readonly ToolTip _ToolTip;

        public ExportOptionsDialog(bool texturesEnabled, bool modelsEnabled, bool animationsEnabled, string defaultPath)
        {
            this.Text = "Export Options";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.ClientSize = new System.Drawing.Size(560, 330);

            defaultPath = string.IsNullOrEmpty(defaultPath) ? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) : defaultPath;

            int y = 16;
            this._TexturePathTextBox = this.AddPathSection("Textures", "DDS", texturesEnabled, defaultPath, ref y);
            this._TextureFormatComboBox = (ComboBox)this.Controls[this.Controls.Count - 1];
            y += 8;
            this._ModelPathTextBox = this.AddPathSection("Geometry / PolySkin / Composite", "GLTF", modelsEnabled, defaultPath, ref y);
            this._ModelFormatComboBox = (ComboBox)this.Controls[this.Controls.Count - 1];
            y += 8;
            this._AnimationPathTextBox = this.AddPathSection("Animations", "GLTF", animationsEnabled, defaultPath, ref y);
            this._RawRootTranslationCheckBox = new CheckBox
            {
                Text = "Raw Root Translation [Hover for info]",
                Left = 24,
                Top = y,
                Width = 220,
                Enabled = animationsEnabled,
                Checked = false,
            };
            this.Controls.Add(this._RawRootTranslationCheckBox);
            this._ToolTip = new ToolTip();
            this._ToolTip.SetToolTip(
                this._RawRootTranslationCheckBox,
                "When checked, this will export with translation data read straight from the .P3D\r\nUnchecked will result in translation data being relative to the origin, like the preview shows.");

            var okButton = new Button
            {
                Text = "Export",
                DialogResult = DialogResult.OK,
                Left = this.ClientSize.Width - 192,
                Top = this.ClientSize.Height - 42,
                Width = 82,
            };
            var cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Left = this.ClientSize.Width - 102,
                Top = this.ClientSize.Height - 42,
                Width = 82,
            };
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }

        public string TexturePath { get { return this._TexturePathTextBox.Text; } }
        public string ModelPath { get { return this._ModelPathTextBox.Text; } }
        public string AnimationPath { get { return this._AnimationPathTextBox.Text; } }
        public string TextureFormat { get { return (string)this._TextureFormatComboBox.SelectedItem; } }
        public string ModelFormat { get { return (string)this._ModelFormatComboBox.SelectedItem; } }
        public bool RawRootTranslation { get { return this._RawRootTranslationCheckBox.Checked; } }

        private TextBox AddPathSection(string title, string format, bool enabled, string defaultPath, ref int y)
        {
            var label = new Label
            {
                Text = title,
                Left = 16,
                Top = y,
                Width = 300,
                Enabled = enabled,
            };
            this.Controls.Add(label);
            y += 22;

            var path = new TextBox
            {
                Left = 24,
                Top = y,
                Width = 430,
                Text = defaultPath,
                Enabled = enabled,
            };
            var browse = new Button
            {
                Text = "...",
                Left = 462,
                Top = y - 1,
                Width = 36,
                Enabled = enabled,
            };
            browse.Click += (sender, e) =>
            {
                var selected = SelectFolder(title, path.Text);
                if (string.IsNullOrEmpty(selected) == false)
                {
                    path.Text = selected;
                }
            };
            this.Controls.Add(path);
            this.Controls.Add(browse);
            y += 28;

            var combo = new ComboBox
            {
                Left = 24,
                Top = y,
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = enabled,
            };
            combo.Items.Add(format);
            combo.SelectedIndex = 0;
            this.Controls.Add(combo);
            y += 30;

            return path;
        }

        private static string SelectFolder(string title, string currentPath)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = title;
                dialog.CheckFileExists = false;
                dialog.CheckPathExists = true;
                dialog.ValidateNames = false;
                dialog.FileName = "Select Folder";
                if (string.IsNullOrEmpty(currentPath) == false && Directory.Exists(currentPath) == true)
                {
                    dialog.InitialDirectory = currentPath;
                }

                return dialog.ShowDialog() == DialogResult.OK ? Path.GetDirectoryName(dialog.FileName) : null;
            }
        }
    }
}
