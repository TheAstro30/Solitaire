/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Solitaire.Classes;
using Solitaire.Classes.Helpers;

namespace Solitaire.Forms
{
    public partial class FrmGame : Game
    {
        /* Move all the passengers away from the deadly plane... */
        public FrmGame()
        {
            InitializeComponent();

            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = string.Format("Kanga's Solitaire - {0}.{1}.{2} (Build: {3})", version.Major, version.Minor, version.Build, version.MinorRevision);

            /* Build menubar */
            var m = (ToolStripMenuItem)menuBar.Items.Add("Game");
            m.DropDownItems.AddRange(
                new ToolStripItem[]
                {
                    MenuHelper.AddMenuItem("New game", "NEW", Keys.Control | Keys.N, true, OnMenuClick),
                    new ToolStripSeparator(), 
                    MenuHelper.AddMenuItem("Load saved game", "LOAD", Keys.Control | Keys.O, true, OnMenuClick),
                    MenuHelper.AddMenuItem("Save current game", "SAVE", Keys.Control | Keys.S, true, OnMenuClick),
                    new ToolStripSeparator(), 
                    MenuHelper.AddMenuItem("Exit", "EXIT", Keys.Alt | Keys.F4 ,true, OnMenuClick)
                });

            m = (ToolStripMenuItem) menuBar.Items.Add("Help");
            m.DropDownItems.Add(MenuHelper.AddMenuItem("About", "ABOUT", OnMenuClick));
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            /* Ask the user if they really want to quit - yes, I know, kind of annoying */
            if (MessageBox.Show(this, @"Are you sure you want to really quit?", @"Quit Solitaire", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            /* Through the cockpit window, we can now piss off :) */
            base.OnFormClosing(e);
        }

        /* Menu click callback */
        private void OnMenuClick(object sender, EventArgs e)
        {
            var o = (ToolStripItem) sender;
            switch (o.Tag.ToString())
            {
                case "NEW":
                    NewGame();
                    Invalidate();
                    break;

                case "LOAD":
                    if (
                        MessageBox.Show(this, @"Are you sure you want to quit the current game?", @"Quit Current Game",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                    var d = new GameData(false);
                    if (BinarySerialize<GameData>.Load(AppPath.MainDir(@"\KangaSoft\Solitaire\saved.dat", true), ref d))
                    {
                        /* Load a saved game */
                        CurrentGame = d;
                        Invalidate();
                    }
                    break;

                case "SAVE":
                    /* Save current game */
                    BinarySerialize<GameData>.Save(AppPath.MainDir(@"\KangaSoft\Solitaire\saved.dat", true), CurrentGame);
                    break;

                case "EXIT":
                    Close();
                    break;

                case "ABOUT":
                    using (var f = new FrmAbout())
                    {
                        f.ShowDialog(this);
                    }
                    break;
            }
        }
    }
}
