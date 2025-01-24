/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Solitaire.Classes.UI;
using Solitaire.Controls;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    /* Show info about the game - Captain James T. Kirk, of the Starship Enterprise... */
    public sealed class FrmAbout : FormEx
    {
        private readonly Button _btnOk;

        public FrmAbout(Game game)
        {
            /* I'm afraid, Captain, that's just illogical */
            AcceptButton = _btnOk;
            ClientSize = new Size(309, 173);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = @"About Kanga's Solitaire";

            var pnlIcon = new Panel
            {
                BackColor = Color.Transparent,
                Location = new Point(12, 12),
                Size = new Size(64, 64),
                BackgroundImageLayout = ImageLayout.None,
                BackgroundImage = Resources.aboutIcon.ToBitmap()
            };

            var lblTitle = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(82, 9),
                Size = new Size(159, 25),
                Text = @"Kanga's Solitaire"
            };

            var lblAuthor = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(84, 64),
                Size = new Size(185, 15),
                Text = @"Written by: Jason James Newland"
            };

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var lblVersion = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(84, 43),
                Size = new Size(49, 15),
                Text = $@"Version: {version.Major}.{version.Minor}.{version.Build} (Build: {version.MinorRevision})"
            };

            var lblCopyright = new Label
            {
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(84, 86),
                Name = "lblCopyright",
                Size = new Size(219, 33),
                Text = @"Copyright ©2025 KangaSoft Software. All Rights Reserved."
            };

            _btnOk = new Button
            {
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                DialogResult = DialogResult.OK,
                Location = new Point(187, 138),
                Size = new Size(110, 28),
                TabIndex = 0,
                Text = @"Ok",
                BackgroundImage = game.ObjectData.ButtonOk,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };

            Controls.AddRange(new Control[]
            {
                pnlIcon, lblTitle, lblVersion, lblAuthor, lblCopyright, _btnOk
            });
        }
    }
}
