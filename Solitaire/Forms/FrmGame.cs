/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Solitaire.Classes.Helpers;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.Helpers.System;
using Solitaire.Classes.UI;

namespace Solitaire.Forms
{
    public sealed class FrmGame : Game
    {
        /* Move all the passengers away from the deadly plane... */
        private readonly ToolStripMenuItem _menuGame;
        private readonly StatusStrip _statusBar;

        private readonly NotifyIcon _sysTray;
        private FormWindowState _originalWindowState;

        public FrmGame()
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ClientSize = new Size(720, 470);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                       
            MinimumSize = new Size(720, 470);
            StartPosition = FormStartPosition.Manual;

            var menuBar = new MenuStrip
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

            _sysTray = new NotifyIcon {Icon = Icon, Text = @"Kanga's Solitaire - Double-click to restore"};
            _sysTray.MouseDoubleClick += OnSysTrayDoubleClick;

            Controls.AddRange(new Control[]
            {
                menuBar,
                _statusBar
            });

            MainMenuStrip = menuBar;            

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = string.Format("Kanga's Solitaire - {0}.{1}.{2} (Build: {3})", version.Major, version.Minor, version.Build, version.MinorRevision);
            
            /* Build menubar */
            _menuGame = (ToolStripMenuItem)menuBar.Items.Add("Game");
            _menuGame.DropDownOpening += OnMenuOpening;

            BuildMenu(_menuGame);
            menuBar.Items.Add(_menuGame);

            var m = (ToolStripMenuItem) menuBar.Items.Add("Help");
            m.DropDownItems.Add(MenuHelper.AddMenuItem("About", "ABOUT", OnMenuClick));

            /* Status bar */
            _statusBar.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripLabel("Game number: 0") {AutoSize = false, Width = 150, TextAlign = ContentAlignment.MiddleLeft},
                new ToolStripSeparator(),
                new ToolStripLabel("Elapsed time: 00:00") {AutoSize = false, Width = 150, TextAlign = ContentAlignment.MiddleLeft},
                new ToolStripSeparator(),
                new ToolStripLabel("Score: 0") {AutoSize = false, Width = 120, TextAlign = ContentAlignment.MiddleLeft}
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
                /* Big fucking white snow flakes... */
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    /* "Boss" key */
                    _originalWindowState = WindowState;
                    WindowState = FormWindowState.Minimized;
                    Hide();
                    _sysTray.Visible = true;
                    break;

                case Keys.Control | Keys.Z:
                    /* This gets around the issue of undo being disabled in the menu, and not re-enabled until menu is opened again -
                     * What I don't want to do is clear the menu list and re-add every single call of Ctrl+Z... */
                    UndoMove();
                    break;
            }
            base.OnKeyDown(e);
        }

        private void OnSysTrayDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Show();
                WindowState = _originalWindowState;
                _sysTray.Visible = false;
            }
        }

        private void TimeChanged(int seconds)
        {
            _statusBar.Items[0].Text = string.Format("Game number: {0}", SettingsManager.Settings.Statistics.TotalGamesPlayed);
            var ts = new TimeSpan(0, 0, 0, seconds);
            _statusBar.Items[2].Text = string.Format("Elapsed time: {0:00}:{1:00}", ts.Minutes, ts.Seconds);
        }

        private void ScoreChanged(int score)
        {
            _statusBar.Items[4].Text = string.Format("Score: {0}", score);
        }

        /* Menu click callback */
        private void OnMenuOpening(object sender, EventArgs e)
        {
            BuildMenu(_menuGame);
        }

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
                    if (!LoadSavedGame())
                    {
                        CustomMessageBox.Show(this, "No game was loaded.\r\n\r\nSave a game to be recalled later first.", "Error", CustomMessageBoxButtons.Ok);
                    }
                    break;

                case "SAVE":
                    if (SaveCurrentGame())
                    {
                        CustomMessageBox.Show(this, "Current game was saved.", "Game Saved", CustomMessageBoxButtons.Ok);
                    }
                    break;

                case "DRAW3":
                    /* Deciding to either allow current game to be continued to play, or ask to restart using draw 3 */
                    SettingsManager.Settings.Options.DrawThree = !SettingsManager.Settings.Options.DrawThree;
                    Invalidate();
                    break;

                case "SOUND":
                    SettingsManager.Settings.Options.PlaySounds = !SettingsManager.Settings.Options.PlaySounds;
                    break;

                case "UNDO":
                    /* This gets around the issue of undo being disabled in the menu, and not re-enabled until menu is opened again -
                     * What I don't want to do is clear the menu list and re-add every single call of Ctrl+Z... */
                    OnKeyDown(new KeyEventArgs(Keys.Control | Keys.Z));
                    break;
                    
                case "RESTART":                    
                    if (IsLoadedGame)
                    {
                        if (
                            CustomMessageBox.Show(this,
                                "Restarting a loaded game will cause it to restart from the saved point.\r\n\r\nDo you want to restart this game?",
                                "Restart Current Game") == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else if (CustomMessageBox.Show(this, "Are you sure you want to restart the current game?", "Restart Current Game") == DialogResult.No)
                    {
                        return;
                    }
                    RestartGame(IsLoadedGame);
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

        /* Private menu building method */

        private void BuildMenu(ToolStripMenuItem m)
        {
            m.DropDownItems.Clear();
            m.DropDownItems.AddRange(
                new ToolStripItem[]
                {
                    MenuHelper.AddMenuItem("New game", "NEW", Keys.Control | Keys.N, true, OnMenuClick),
                    new ToolStripSeparator(),
                    MenuHelper.AddMenuItem("Load saved game", "LOAD", Keys.Control | Keys.O, true, OnMenuClick),
                    MenuHelper.AddMenuItem("Save current game", "SAVE", Keys.Control | Keys.S, !GameCompleted,
                        OnMenuClick),
                    new ToolStripSeparator(),
                });
            var o = MenuHelper.AddMenuItem("Options");
            o.DropDownItems.AddRange(new ToolStripItem[]
            {
                MenuHelper.AddMenuItem("Draw three", "DRAW3", Keys.None, true, SettingsManager.Settings.Options.DrawThree, null, OnMenuClick),
                MenuHelper.AddMenuItem("Play sound effects", "SOUND", Keys.None, true, SettingsManager.Settings.Options.PlaySounds, null, OnMenuClick)
            });
            m.DropDownItems.AddRange(
                new ToolStripItem[]
                {
                    o,
                    new ToolStripSeparator(),
                    MenuHelper.AddMenuItem("Undo last move", "UNDO", Keys.Control | Keys.Z,
                        Undo.Count > 0 && !GameCompleted, OnMenuClick),
                    MenuHelper.AddMenuItem("Restart game", "RESTART", Keys.None, !GameCompleted, OnMenuClick),
                    new ToolStripSeparator(),
                    MenuHelper.AddMenuItem("Auto complete...", "AUTO", Keys.Control | Keys.A, !GameCompleted,
                        OnMenuClick),
                    MenuHelper.AddMenuItem("Statistics", "STATS", Keys.None, true, OnMenuClick),
                    new ToolStripSeparator(),
                    MenuHelper.AddMenuItem("Exit", "EXIT", Keys.Alt | Keys.F4, true, OnMenuClick)
                });
        }
    }
}
