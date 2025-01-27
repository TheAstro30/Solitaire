/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Classes.Helpers.UI;
using Solitaire.Classes.UI;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    public sealed partial class FrmNew : Form
    {
        public NewGameDialogResult NewGameDialogResult { get; private set; }

        public FrmNew()
        {
            InitializeComponent();

            var image = Resources.new_game_bg;

            ClientSize = image.Size;

            BackgroundImage = image;
            BackgroundImageLayout = ImageLayout.None;

            btnOne.BackgroundImage = Resources.button_ok;
            btnOne.BackgroundImageLayout = ImageLayout.Tile;
            btnOne.ForeColor = Color.White;

            btnThree.BackgroundImage = Resources.button_ok;
            btnThree.BackgroundImageLayout = ImageLayout.Tile;
            btnThree.ForeColor = Color.White;

            btnLoad.BackgroundImage = Resources.button_ok;
            btnLoad.BackgroundImageLayout = ImageLayout.Tile;
            btnLoad.ForeColor = Color.White;

            btnCancel.BackgroundImage = Resources.button_cancel;
            btnCancel.BackgroundImageLayout = ImageLayout.Tile;
            btnCancel.ForeColor = Color.White;

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
