/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Windows.Forms;
using Solitaire.Forms;

namespace Solitaire
{
    static class Program
    {        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
            /* Yoshi Satoshi - if anyone is wondering about some of my comments on this source?
             * I'm just being silly. */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmGame());
        }
    }
}
