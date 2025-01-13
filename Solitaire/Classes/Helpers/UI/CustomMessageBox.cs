/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Windows.Forms;
using Solitaire.Forms;

namespace Solitaire.Classes.Helpers.UI
{
    /* Simple custom messagebox class */
    public enum CustomMessageBoxButtons
    {
        Ok = 0,
        YesNo = 1
    }
    
    public static class CustomMessageBox
    {
        public static DialogResult Show(IWin32Window parent, string text, string caption)
        {
            return Show(parent, text, caption, CustomMessageBoxButtons.YesNo);
        }

        public static DialogResult Show(IWin32Window parent, string text, string caption, CustomMessageBoxButtons buttons)
        {
            using (var f = new FrmCustomMessage(buttons))
            {
                f.MessageText = text;
                f.CaptionText = caption;
                return f.ShowDialog(parent);
            }
        }
    }
}
