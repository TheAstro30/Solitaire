/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Windows.Forms;
using Solitaire.Classes.UI;
using Solitaire.Forms;

namespace Solitaire.Classes.Helpers.UI
{
    public enum CustomMessageBoxButtons
    {
        Ok = 0,
        YesNo = 1
    }

    public enum NewGameDialogResult
    {
        None = 0,
        DrawOne = 1,
        DrawThree = 2,
        LoadGame = 3
    }

    /* Simple custom messagebox class */
    public static class CustomMessageBox
    {
        public static DialogResult Show(IWin32Window parent, string text, string caption)
        {
            return Show(parent, text, caption, CustomMessageBoxButtons.YesNo);
        }

        public static DialogResult Show(IWin32Window parent, string text, string caption, CustomMessageBoxButtons buttons)
        {
            using (var f = new FrmCustomMessage((Game)parent, buttons))
            {
                f.MessageText = text;
                f.CaptionText = caption;
                return f.ShowDialog(parent);
            }
        }
    }

    /* New game dialog */
    public static class NewGameDialog
    {
        public static FrmNew Show(IWin32Window owner)
        {
            using (var ng = new FrmNew((Game)owner))
            {
                ng.ShowDialog(owner);
                return ng;
            }
        }
    }
}
