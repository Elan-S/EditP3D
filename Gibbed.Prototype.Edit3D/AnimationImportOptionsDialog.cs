using System;
using System.Windows.Forms;

namespace Gibbed.Prototype.Edit3D
{
    internal enum AnimationCompressionPreset
    {
        AlexPlayer = 1,
        Soldiers = 2,
        LargeInfected = 3,
        Pedestrians = 4,
        Cutscenes = 5,
    }

    internal sealed class AnimationImportOptionsDialog : Form
    {
        private readonly ComboBox _CompressionComboBox;

        public AnimationImportOptionsDialog()
        {
            this.Text = "Animation Import Compression";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ClientSize = new System.Drawing.Size(420, 118);

            var label = new Label
            {
                Left = 12,
                Top = 12,
                Width = 396,
                Text = "Choose the compression profile for the imported animation:",
            };
            this.Controls.Add(label);

            this._CompressionComboBox = new ComboBox
            {
                Left = 12,
                Top = 38,
                Width = 396,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            this._CompressionComboBox.Items.Add(new CompressionItem(AnimationCompressionPreset.AlexPlayer, "Compression 1: Alex/Player"));
            this._CompressionComboBox.Items.Add(new CompressionItem(AnimationCompressionPreset.Soldiers, "Compression 2: Soldiers"));
            this._CompressionComboBox.Items.Add(new CompressionItem(AnimationCompressionPreset.LargeInfected, "Compression 3: Large Infected"));
            this._CompressionComboBox.Items.Add(new CompressionItem(AnimationCompressionPreset.Pedestrians, "Compression 4: Pedestrians"));
            this._CompressionComboBox.Items.Add(new CompressionItem(AnimationCompressionPreset.Cutscenes, "Compression 5: Cutscenes"));
            this._CompressionComboBox.SelectedIndex = 0;
            this.Controls.Add(this._CompressionComboBox);

            var ok = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Left = 252,
                Top = 78,
                Width = 75,
            };
            var cancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Left = 333,
                Top = 78,
                Width = 75,
            };
            this.Controls.Add(ok);
            this.Controls.Add(cancel);
            this.AcceptButton = ok;
            this.CancelButton = cancel;
        }

        public AnimationCompressionPreset SelectedPreset
        {
            get
            {
                var item = this._CompressionComboBox.SelectedItem as CompressionItem;
                return item == null ? AnimationCompressionPreset.AlexPlayer : item.Preset;
            }
        }

        private sealed class CompressionItem
        {
            public readonly AnimationCompressionPreset Preset;
            private readonly string _Text;

            public CompressionItem(AnimationCompressionPreset preset, string text)
            {
                this.Preset = preset;
                this._Text = text;
            }

            public override string ToString()
            {
                return this._Text;
            }
        }
    }
}
