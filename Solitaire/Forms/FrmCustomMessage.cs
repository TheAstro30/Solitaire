/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Classes.Helpers;
using Solitaire.Classes.UI;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    public sealed class FrmCustomMessage : FormEx
    {
        private readonly Label _lblText;
        private readonly Button _btnYes;

        public string MessageText { set { _lblText.Text = value; } }
        public string CaptionText { set { Text = value; } } 

        public FrmCustomMessage(CustomMessageBoxButtons buttons)
        {
            AcceptButton = _btnYes;
            ClientSize = new Size(365, 131);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = @"Message";

            var pnlIcon = new Panel
            {
                BackColor = Color.Transparent,
                Location = new Point(12, 12),
                Size = new Size(64, 64),
                BackgroundImage = Resources.aboutIcon.ToBitmap(),
                BackgroundImageLayout = ImageLayout.None
            };

            _lblText = new Label
            {
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(82, 12),
                Size = new Size(271, 64)
            };

            _btnYes = new Button
            {
                DialogResult = DialogResult.Yes,
                Location = new Point(197, 96),
                Size = new Size(75, 23),
                TabIndex = 0,
                Text = @"Yes",
                UseVisualStyleBackColor = true
            };

            var btnNo = new Button
            {
                DialogResult = DialogResult.No,
                Location = new Point(278, 96),
                Size = new Size(75, 23),
                TabIndex = 1,
                Text = @"No",
                UseVisualStyleBackColor = true
            };

            switch (buttons)
            {
                case CustomMessageBoxButtons.Ok:
                    btnNo.Text = @"Ok";
                    btnNo.DialogResult = DialogResult.OK;
                    _btnYes.Visible = false;
                    break;
            }

            Controls.AddRange(new Control[] {pnlIcon, _lblText, _btnYes, btnNo});
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult = DialogResult.No;
            }
            base.OnFormClosing(e);
        }
    }
}
