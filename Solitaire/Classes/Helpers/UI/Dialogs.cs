/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Windows.Forms;
using Solitaire.Forms;

namespace Solitaire.Classes.Helpers.UI
{
    /* Lucy?! You got some 'splainin' to do! */
    public enum CustomMessageBoxButtons
    {
        Ok = 0,
        YesNo = 1
    }

    public enum CustomMessageBoxIcon
    {
        Warning = 0,
        Error = 1,
        Information = 2
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
            return Show(parent, text, caption, buttons, CustomMessageBoxIcon.Warning);
        }

        public static DialogResult Show(IWin32Window parent, string text, string caption, CustomMessageBoxIcon icon)
        {
            return Show(parent, text, caption, CustomMessageBoxButtons.YesNo, icon);
        }

        public static DialogResult Show(IWin32Window parent, string text, string caption, CustomMessageBoxButtons buttons, CustomMessageBoxIcon icon)
        {
            using (var f = new FrmCustomMessage(buttons, icon))
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
            using (var ng = new FrmNew())
            {
                ng.ShowDialog(owner);
                return ng;
            }
        }
    }
}
