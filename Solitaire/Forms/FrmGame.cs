/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Solitaire.Classes.Helpers;
using Solitaire.Classes.UI;

namespace Solitaire.Forms
{
    public sealed class FrmGame : Game
    {
        /* Move all the passengers away from the deadly plane... */
        private readonly MenuStrip _menuBar;
        private readonly StatusStrip _statusBar;

        public FrmGame()
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ClientSize = new Size(720, 470);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                       
            MinimumSize = new Size(720, 470);
            StartPosition = FormStartPosition.Manual;

            _menuBar = new MenuStrip
            {
                Location = new Point(0, 0),
                Padding = new Padding(7, 2, 0, 2),
                Size = new Size(704, 24),
                TabIndex = 0
            };

            _statusBar = new StatusStrip
            {
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(0, 409),
                Padding = new Padding(1, 0, 16, 0),
                Size = new Size(704, 22),
                TabIndex = 1
            };

            Controls.AddRange(new Control[]
            {
                _menuBar,
                _statusBar
            });

            MainMenuStrip = _menuBar;            

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = string.Format("Kanga's Solitaire - {0}.{1}.{2} (Build: {3})", version.Major, version.Minor, version.Build, version.MinorRevision);
            
            /* Build menubar */
            var m = (ToolStripMenuItem)_menuBar.Items.Add("Game");
            m.DropDownItems.AddRange(
                new ToolStripItem[]
                {
                    MenuHelper.AddMenuItem("New game", "NEW", Keys.Control | Keys.N, true, OnMenuClick),
                    new ToolStripSeparator(), 
                    MenuHelper.AddMenuItem("Load saved game", "LOAD", Keys.Control | Keys.O, true, OnMenuClick),
                    MenuHelper.AddMenuItem("Save current game", "SAVE", Keys.Control | Keys.S, true, OnMenuClick),
                    new ToolStripSeparator(), 
                    MenuHelper.AddMenuItem("Undo last move", "UNDO", Keys.Control | Keys.Z ,true, OnMenuClick),
                    MenuHelper.AddMenuItem("Restart game", "RESTART", Keys.None,true, OnMenuClick),
                    new ToolStripSeparator(), 
                    MenuHelper.AddMenuItem("Auto complete...", "AUTO", Keys.Control | Keys.A ,true, OnMenuClick),
                    MenuHelper.AddMenuItem("Statistics", "STATS", Keys.None ,true, OnMenuClick),
                    new ToolStripSeparator(), 
                    MenuHelper.AddMenuItem("Exit", "EXIT", Keys.Alt | Keys.F4 ,true, OnMenuClick)
                });

            m = (ToolStripMenuItem) _menuBar.Items.Add("Help");
            m.DropDownItems.Add(MenuHelper.AddMenuItem("About", "ABOUT", OnMenuClick));

            /* Status bar */
            _statusBar.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripLabel("Elapsed time: 00:00") {AutoSize = false, Width = 120, TextAlign = ContentAlignment.MiddleLeft},
                new ToolStripSeparator(), 
                new ToolStripLabel("Score: 0")
            });

            OnGameTimeChanged += TimeChanged;
            OnScoreChanged += ScoreChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            /* Set window position and size */
            var loc = SettingsManager.Settings.Location;
            if (loc == Point.Empty)
            {
                /* Scale form to half the screen width/height */
                var screen = MonitorUtil.GetCurrentMonitor(this);
                var x = screen.Bounds.Width / 2;
                var y = screen.Bounds.Height / 2;
                Size = new Size(x, y);
                /* Set location to center screen */
                Location = new Point(x - (Size.Width / 2), y - (Size.Height / 2));
            }
            else
            {
                Location = loc;
                Size = SettingsManager.Settings.Size;
                if (SettingsManager.Settings.Maximized)
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
            if (CustomMessageBox.Show(this, "Are you sure you want to really quit?", "Quit Solitaire") == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            if (WindowState == FormWindowState.Normal)
            {
                SettingsManager.Settings.Location = Location;
                SettingsManager.Settings.Size = Size;
            }
            SettingsManager.Settings.Maximized = WindowState == FormWindowState.Maximized;            
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
                SettingsManager.Settings.Location = Location;
                SettingsManager.Settings.Size = Size;
            }
            base.OnResize(e);
        }

        private void TimeChanged(int seconds)
        {
            var ts = new TimeSpan(0, 0, 0, seconds);
            _statusBar.Items[0].Text = string.Format("Elapsed time: {0:00}:{1:00}", ts.Minutes, ts.Seconds);
        }

        private void ScoreChanged(int score)
        {
            _statusBar.Items[2].Text = string.Format("Score: {0}", score);
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
                    if (!GameCompleted && CustomMessageBox.Show(this, "Are you sure you want to quit the current game?", "Quit Current Game") == DialogResult.No)
                    {
                        return;
                    }
                    LoadSavedGame();
                    break;

                case "SAVE":
                    SaveCurrentGame();
                    break;

                case "UNDO":
                    UndoMove();
                    break;
                    
                case "RESTART":
                    if (!CurrentGame.CanRestart)
                    {
                        CustomMessageBox.Show(this, "Unable to restart a loaded game", "Error", CustomMessageBoxButtons.Ok);
                        return;
                    }
                    if (GameCompleted || CustomMessageBox.Show(this, "Are you sure you want to restart the current game?", "Restart Current Game") == DialogResult.No)
                    {
                        return;
                    }
                    RestartGame();
                    break;

                case "AUTO":
                    AutoComplete();
                    break;

                case "STATS":
                    using (var s = new FrmStatistics())
                    {
                        s.ShowDialog(this);
                    }
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
