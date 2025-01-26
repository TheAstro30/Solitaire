/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Classes.UI;
using Solitaire.Controls;

namespace Solitaire.Forms
{
    /* I can't stop farting...
     * Simple user input string box */
    public partial class FrmInput : FormEx
    {
        public string TitleText /* Form title text */
        {
            set => Text = value;
        }

        public string CaptionText /* Label text; "Choose name to save:" */
        {
            set => lblCaption.Text = value;
        }

        public string InputText => txtInput.Text; /* TextBox input text */

        public FrmInput(Game ctl)
        {
            /* I coded this dialog whilst drunk... */
            InitializeComponent();

            btnCancel.BackgroundImage = ctl.ObjectData.ButtonCancel;
            btnCancel.BackgroundImageLayout = ImageLayout.Tile;
            btnCancel.BackColor = Color.White;

            btnOk.BackgroundImage = ctl.ObjectData.ButtonOk;
            btnOk.BackgroundImageLayout = ImageLayout.Tile;
            btnOk.BackColor = Color.White;
        }
    }
}
