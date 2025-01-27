/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.UI;
using Solitaire.Controls;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    public sealed class FrmStatistics : FormEx
    {
        private readonly Button _btnOk;

        public FrmStatistics()
        {
            AcceptButton = _btnOk;
            ClientSize = new Size(305, 263);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = @"Game Statistics";

            var pnlIcon = new Panel
            {
                BackColor = Color.Transparent,
                Location = new Point(12, 12),
                Size = new Size(64, 64),
                BackgroundImage = Resources.aboutIcon.ToBitmap(),
                BackgroundImageLayout = ImageLayout.None
            };

            var lblTitle = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(82, 12),
                Size = new Size(178, 25),
                Text = @"Life-time Statistics:"
            };

            var lblStats = new Label
            {
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(87, 37),
                Size = new Size(206, 173),
                TabIndex = 0,
                Text = SettingsManager.Settings.Statistics.ToString()
            };

            _btnOk = new Button
            {
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                DialogResult = DialogResult.OK,
                Location = new Point(183, 228),
                Size = new Size(110, 28),
                TabIndex = 0,
                Text = @"Ok",
                BackgroundImage = Resources.button_ok,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };

            Controls.AddRange(new Control[]
            {
                pnlIcon, lblTitle, lblStats, _btnOk
            });
        }
    }
}
