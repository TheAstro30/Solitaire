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
            //DeckBuilder.BuildDeck();
            /* Check that the graphics data files exist - include DeckBuilder.cs call here before this check if you're experiencing problems.
             * Refer to README.MD */
            if (!File.Exists(Utils.MainDir(@"\data\gfx\cards.dat")) ||
                !File.Exists(Utils.MainDir(@"\data\gfx\obj.dat")))
            {
                MessageBox.Show(@"Graphics files are missing, or corrupted. Please re-install Kanga's Solitaire.",
                    @"Load Error", MessageBoxButtons.OK);
                return;
            }
            /* Vee-one; rotate */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmGame());
        }
    }
}
