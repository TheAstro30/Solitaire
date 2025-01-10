/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Windows.Forms;
using Solitaire.Classes.UI;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    public partial class FrmCustomMessage : FormEx
    {
        public string MessageText { set { lblText.Text = value; } }
        public string CaptionText { set { Text = value; } }

        public FrmCustomMessage()
        {
            InitializeComponent();

            pnlIcon.BackgroundImage = Resources.aboutIcon.ToBitmap();
            pnlIcon.BackgroundImageLayout = ImageLayout.None;
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
