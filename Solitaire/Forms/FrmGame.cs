/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Solitaire.Classes.Data;
using Solitaire.Classes.Helpers;
using Solitaire.Classes.Serialization;
using Solitaire.Classes.Settings;
using Solitaire.Classes.UI;

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

            /* Settings */
            SettingsManager.Load();
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
                    MenuHelper.AddMenuItem("Auto complete...", "AUTO", Keys.Control | Keys.A ,true, OnMenuClick),
                    new ToolStripSeparator(), 
                    MenuHelper.AddMenuItem("Exit", "EXIT", Keys.Alt | Keys.F4 ,true, OnMenuClick)
                });

            m = (ToolStripMenuItem) menuBar.Items.Add("Help");
            m.DropDownItems.Add(MenuHelper.AddMenuItem("About", "ABOUT", OnMenuClick));
        }

        //to remove
        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            /* Set window position and size */
            var loc = SettingsManager.Settings.Window.Location;
            if (loc == Point.Empty)
            {
                /* Scale form to half the screen width/height */
                var screen = Monitor.GetCurrentMonitor(this);
                var x = screen.Bounds.Width / 2;
                var y = screen.Bounds.Height / 2;
                Size = new Size(x, y);
                /* Set location to center screen */
                Location = new Point(x - (Size.Width / 2), y - (Size.Height / 2));
            }
            else
            {
                Location = loc;
                Size = SettingsManager.Settings.Window.Size;
                if (SettingsManager.Settings.Window.Maximized)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }
            OnResize(e);
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            /* Ask the user if they really want to quit - yes, I know, kind of annoying */
            if (MessageBox.Show(this, @"Are you sure you want to really quit?", @"Quit Solitaire", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            if (WindowState == FormWindowState.Normal)
            {
                SettingsManager.Settings.Window.Location = Location;
                SettingsManager.Settings.Window.Size = Size;
            }
            SettingsManager.Settings.Window.Maximized = WindowState == FormWindowState.Maximized;
            /* Dump settings */
            SettingsManager.Save();
            /* Through the cockpit window, we can now piss off :) */
            AudioManager.Dispose();
            base.OnFormClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (!Visible)
            {
                return;
            }
            if (WindowState == FormWindowState.Normal)
            {
                SettingsManager.Settings.Window.Location = Location;
                SettingsManager.Settings.Window.Size = Size;
            }
            base.OnResize(e);
        }

        /* Menu click callback */
        private void OnMenuClick(object sender, EventArgs e)
        {
            var o = (ToolStripItem) sender;
            switch (o.Tag.ToString())
            {
                case "NEW":
                    NewGame();
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

                case "AUTO":
                    AutoComplete();
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
