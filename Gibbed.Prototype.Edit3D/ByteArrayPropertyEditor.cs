using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Gibbed.Prototype.Edit3D
{
    internal class ByteArrayPropertyEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var editorService = provider == null ? null : provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (editorService == null)
            {
                return value;
            }

            var data = value as byte[];
            if (data == null)
            {
                data = new byte[0];
            }

            using (var dialog = new ByteArrayEditDialog(data))
            {
                if (editorService.ShowDialog(dialog) != DialogResult.OK)
                {
                    return value;
                }

                return dialog.Data;
            }
        }

        private sealed class ByteArrayEditDialog : Form
        {
            private readonly TextBox _TextBox;
            private readonly RadioButton _HexButton;
            private readonly RadioButton _AnsiButton;
            private byte[] _Data;
            private ViewMode _Mode;

            public ByteArrayEditDialog(byte[] data)
            {
                this._Data = data == null ? new byte[0] : (byte[])data.Clone();
                this._Mode = ViewMode.Hex;

                this.Text = "Edit Byte Array";
                this.StartPosition = FormStartPosition.CenterParent;
                this.MinimizeBox = false;
                this.MaximizeBox = true;
                this.Width = 760;
                this.Height = 520;

                var modePanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Top,
                    Height = 32,
                    FlowDirection = FlowDirection.LeftToRight,
                    Padding = new Padding(8, 6, 0, 0),
                };

                this._HexButton = new RadioButton
                {
                    Text = "Hex",
                    Checked = true,
                    AutoSize = true,
                };
                this._HexButton.CheckedChanged += this.OnModeChanged;

                this._AnsiButton = new RadioButton
                {
                    Text = "ANSI",
                    AutoSize = true,
                };
                this._AnsiButton.CheckedChanged += this.OnModeChanged;

                modePanel.Controls.Add(this._HexButton);
                modePanel.Controls.Add(this._AnsiButton);

                this._TextBox = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ScrollBars = ScrollBars.Both,
                    WordWrap = false,
                    AcceptsReturn = true,
                    AcceptsTab = true,
                    Font = new System.Drawing.Font("Consolas", 9.0f),
                };

                var buttonPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom,
                    Height = 42,
                    FlowDirection = FlowDirection.RightToLeft,
                    Padding = new Padding(0, 6, 8, 0),
                };

                var okButton = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Width = 84,
                };
                okButton.Click += this.OnOk;

                var cancelButton = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Width = 84,
                };

                buttonPanel.Controls.Add(okButton);
                buttonPanel.Controls.Add(cancelButton);

                this.AcceptButton = okButton;
                this.CancelButton = cancelButton;
                this.Controls.Add(this._TextBox);
                this.Controls.Add(buttonPanel);
                this.Controls.Add(modePanel);

                this.UpdateText();
            }

            public byte[] Data
            {
                get { return (byte[])this._Data.Clone(); }
            }

            private void OnModeChanged(object sender, EventArgs e)
            {
                var newMode = this._AnsiButton.Checked == true ? ViewMode.Ansi : ViewMode.Hex;
                if (newMode == this._Mode)
                {
                    return;
                }

                if (this.TryCommitText() == false)
                {
                    if (this._Mode == ViewMode.Hex)
                    {
                        this._HexButton.Checked = true;
                    }
                    else
                    {
                        this._AnsiButton.Checked = true;
                    }

                    return;
                }

                this._Mode = newMode;
                this.UpdateText();
            }

            private void OnOk(object sender, EventArgs e)
            {
                if (this.TryCommitText() == false)
                {
                    this.DialogResult = DialogResult.None;
                }
            }

            private void UpdateText()
            {
                this._TextBox.Text = this._Mode == ViewMode.Ansi
                    ? Encoding.Default.GetString(this._Data)
                    : FormatHex(this._Data);
            }

            private bool TryCommitText()
            {
                try
                {
                    this._Data = this._Mode == ViewMode.Ansi
                        ? Encoding.Default.GetBytes(this._TextBox.Text)
                        : ParseHex(this._TextBox.Text);
                    return true;
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(this, ex.Message, "Invalid Byte Array", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            private static string FormatHex(byte[] data)
            {
                if (data == null || data.Length == 0)
                {
                    return string.Empty;
                }

                var builder = new StringBuilder(data.Length * 3);
                for (int i = 0; i < data.Length; i++)
                {
                    if (i > 0)
                    {
                        builder.Append(i % 16 == 0 ? Environment.NewLine : " ");
                    }

                    builder.Append(data[i].ToString("X2", CultureInfo.InvariantCulture));
                }

                return builder.ToString();
            }

            private static byte[] ParseHex(string text)
            {
                if (string.IsNullOrWhiteSpace(text) == true)
                {
                    return new byte[0];
                }

                var digits = text.Where(Uri.IsHexDigit).ToArray();
                if (digits.Length % 2 != 0)
                {
                    throw new FormatException("Hex mode requires an even number of hexadecimal digits.");
                }

                var data = new byte[digits.Length / 2];
                for (int i = 0; i < data.Length; i++)
                {
                    var value = new string(digits, i * 2, 2);
                    data[i] = byte.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                }

                return data;
            }

            private enum ViewMode
            {
                Hex,
                Ansi,
            }
        }
    }
}
