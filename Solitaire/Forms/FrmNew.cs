/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Classes.Helpers.UI;
using Solitaire.Classes.UI;

namespace Solitaire.Forms
{
    public sealed partial class FrmNew : Form
    {
        public NewGameDialogResult NewGameDialogResult { get; private set; }

        public FrmNew(Game game)
        {
            InitializeComponent();

            var image = game.ObjectData.NewGameBackground;

            ClientSize = image.Size;

            BackgroundImage = image;
            BackgroundImageLayout = ImageLayout.None;

            btnOne.BackgroundImage = game.ObjectData.ButtonOk;
            btnOne.BackgroundImageLayout = ImageLayout.Tile;
            btnOne.ForeColor = Color.White;

            btnThree.BackgroundImage = game.ObjectData.ButtonOk;
            btnThree.BackgroundImageLayout = ImageLayout.Tile;
            btnThree.ForeColor = Color.White;

            btnLoad.BackgroundImage = game.ObjectData.ButtonOk;
            btnLoad.BackgroundImageLayout = ImageLayout.Tile;
            btnLoad.ForeColor = Color.White;

            btnCancel.BackgroundImage = game.ObjectData.ButtonCancel;
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
