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

        public FrmInput()
        {
            /* I coded this dialog whilst drunk... */
            InitializeComponent();

            btnCancel.BackgroundImage = Resources.button_cancel;
            btnCancel.BackgroundImageLayout = ImageLayout.Tile;
            btnCancel.BackColor = Color.White;

            btnOk.BackgroundImage = Resources.button_ok;
            btnOk.BackgroundImageLayout = ImageLayout.Tile;
            btnOk.BackColor = Color.White;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK && txtInput.Text.Equals(string.Empty))
            {
                SystemSounds.Beep.Play();
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }
    }
}
