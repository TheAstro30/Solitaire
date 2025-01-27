/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Classes.Helpers.UI;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    public sealed class FrmNew : Form
    {
        public NewGameDialogResult NewGameDialogResult { get; private set; }

        public FrmNew()
        {
            var image = Resources.new_game_bg;

            ClientSize = image.Size;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = @"New Game";

            BackgroundImage = image;
            BackgroundImageLayout = ImageLayout.None;

            var btnOne = new Button
            {
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(59, 72),
                Size = new Size(226, 28),
                TabIndex = 0,
                Tag = "ONE",
                Text = @"Solitaire By Ones",
                UseVisualStyleBackColor = false,
                BackgroundImage = Resources.button_ok,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };

            var btnThree = new Button
            {
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(59, 106),
                Size = new Size(226, 28),
                TabIndex = 1,
                Tag = "THREE",
                Text = @"Solitaire By Threes",
                UseVisualStyleBackColor = false,
                BackgroundImage = Resources.button_ok,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };

            var btnLoad = new Button
            {
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(59, 140),
                Size = new Size(226, 28),
                TabIndex = 2,
                Tag = "LOAD",
                Text = @"Load Saved Game",
                UseVisualStyleBackColor = false,
                BackgroundImage = Resources.button_ok,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };

            var btnCancel = new Button
            {
                DialogResult = DialogResult.Cancel,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(59, 178),
                Name = "btnCancel",
                Size = new Size(226, 28),
                TabIndex = 3,
                Text = @"Cancel",
                UseVisualStyleBackColor = false,
                BackgroundImage = Resources.button_cancel,
                BackgroundImageLayout = ImageLayout.Tile,
                ForeColor = Color.White
            };

            Controls.AddRange(new Control[]
            {
                btnOne, btnThree, btnLoad, btnCancel
            });

            btnOne.Click += OnButtonClick;
            btnThree.Click += OnButtonClick;
            btnLoad.Click += OnButtonClick;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            var o = (Button) sender;
            switch (o.Tag.ToString())
            {
                case "ONE":
                    NewGameDialogResult = NewGameDialogResult.DrawOne;
                    break;

                case "THREE":
                    NewGameDialogResult = NewGameDialogResult.DrawThree;
                    break;

                case "LOAD":
                    NewGameDialogResult = NewGameDialogResult.LoadGame;
                    break;
            }
            DialogResult = DialogResult.OK;
        }
    }
}
