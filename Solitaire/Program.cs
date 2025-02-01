/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.IO;
using System.Windows.Forms;
using Solitaire.Classes.Helpers;
using Solitaire.Forms;

namespace Solitaire
{
    /* Yoshi Satoshi - if anyone is wondering about some of my comments on this source?
     * I'm just being silly. */
    internal static class Program
    {        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        private static void Main()
        {
            //Images.Build(); /* Only call this to rebuild the gfx folder's graphics files */
            /* Check that saved games folder exists in the appdata folder */
            var saved = Utils.MainDir(@"\KangaSoft\Solitaire\saved", true);
            try
            {
                if (!Directory.Exists(saved))
                {
                    Directory.CreateDirectory(saved);
                }
            }
            catch 
            {
                /* Ignore */
            }
            /* Vee-one; rotate */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmGame());
        }
    }
}
