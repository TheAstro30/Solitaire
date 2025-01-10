/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Windows.Forms;
using Solitaire.Forms;

namespace Solitaire.Classes.Helpers
{
    public static class CustomMessageBox
    {
        public static DialogResult Show(IWin32Window parent, string text, string caption)
        {
            using (var f = new FrmCustomMessage())
            {
                f.MessageText = text;
                f.CaptionText = caption;
                return f.ShowDialog(parent);
            }
        }
    }
}
