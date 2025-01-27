/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using Solitaire.Controls;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    /* I can't stop farting...
     * Simple user input string box */
    public sealed class FrmInput : FormEx
    {
        private readonly Label _lblCaption;
        private readonly TextBox _txtInput;

        public string TitleText /* Form title text */
        {
            set => Text = value;
        }

        public string CaptionText /* Label text; "Choose name to save:" */
        {
            set => _lblCaption.Text = value;
        }

        public string InputText => _txtInput.Text; /* TextBox input text */

        public FrmInput()
        {
            /* I coded this dialog whilst drunk... */
            ClientSize = new Size(341, 111);
            Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;

            _lblCaption = new Label
            {
                AutoSize = true, BackColor = Color.Transparent, Location = new Point(9, 9), Size = new Size(48, 15)
            };

            _txtInput = new TextBox
            {
                Location = new Point(12, 31), MaxLength = 32, Size = new Size(317, 23), TabIndex = 0
            };

            var btnCancel = new Button
            {
                DialogResult = DialogResult.Cancel,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(219, 77),
                Size = new Size(110, 28),
                TabIndex = 1,
                Text = @"Cancel",
                UseVisualStyleBackColor = false,
                BackgroundImage = Resources.button_cancel,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };

            var btnOk = new Button
            {
                DialogResult = DialogResult.OK,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(103, 77),
                Size = new Size(110, 28),
                TabIndex = 2,
                Text = @"Ok",
                UseVisualStyleBackColor = false,
                BackgroundImage = Resources.button_ok,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };

            Controls.AddRange(new Control[]
            {
                _lblCaption, _txtInput, btnCancel, btnOk
            });

            AcceptButton = btnOk;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK && _txtInput.Text.Equals(string.Empty))
            {
                SystemSounds.Beep.Play();
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }
    }
}
