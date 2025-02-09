﻿/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Solitaire.Classes.Data;
using Solitaire.Classes.Helpers;
using Solitaire.Classes.Helpers.Logic;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.Helpers.UI;
using Solitaire.Classes.Serialization;
using Solitaire.Classes.Settings.SettingsData;
using Solitaire.Classes.UI;
using Solitaire.Properties;

namespace Solitaire.Forms
{
    public sealed class FrmGame : Game
    {
        /* Move all the passengers away from the deadly plane... */
        private readonly ToolStripMenuItem _menuGame;
        private readonly ToolStripMenuItem _menuOptions;

        private readonly StatusStrip _statusBar;

        private readonly NotifyIcon _sysTray;
        private FormWindowState _originalWindowState;

        public FrmGame()
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ClientSize = new Size(720, 470);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BackColor = Color.Black;

            MinimumSize = new Size(720, 470);
            StartPosition = FormStartPosition.Manual;

            var menuBar = new MenuStrip {Location = new Point(0, 0), Padding = new Padding(7, 2, 0, 2)};

            var statusImages = new ImageList {ColorDepth = ColorDepth.Depth32Bit};
            statusImages.Images.AddRange(
                new Image[]
                {
                    Resources.newGame.ToBitmap(),
                    Resources.time.ToBitmap(),
                    Resources.moves.ToBitmap(),
                    Resources.scorePositive.ToBitmap(),
                    Resources.scoreNegative.ToBitmap(),
                    Resources.tip.ToBitmap()
                });

            _statusBar = new StatusStrip
            {
                Font = new Font("Segoe UI Semibold", 9.0F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(0, 409),
                Padding = new Padding(1, 0, 16, 0),
                RenderMode = ToolStripRenderMode.ManagerRenderMode,
                LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow,
                ImageList = statusImages
            };

            _sysTray = new NotifyIcon {Icon = Icon, Text = @"Kanga's Solitaire - Double-click to restore"};
            _sysTray.MouseDoubleClick += OnSysTrayDoubleClick;

            Controls.AddRange(new Control[] {menuBar, _statusBar});

            MainMenuStrip = menuBar;

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Text =
                $@"Kanga's Solitaire - {version.Major}.{version.Minor}.{version.Build} (Build: {version.MinorRevision})";

            /* Build menubar */
            _menuGame = (ToolStripMenuItem) menuBar.Items.Add("Game");
            _menuGame.DropDownOpening += OnMenuGameOpening;

            BuildMenuGame(_menuGame);
            menuBar.Items.Add(_menuGame);

            _menuOptions = (ToolStripMenuItem) menuBar.Items.Add("Options");
            _menuOptions.DropDownOpening += OnMenuOptionsOpening;

            BuildMenuOptions(_menuOptions);
            menuBar.Items.Add(_menuOptions);

            var m = (ToolStripMenuItem) menuBar.Items.Add("Help");
            m.DropDownItems.AddRange(new ToolStripItem[]
            {
                MenuHelper.AddMenuItem("Show help", "HELP", Keys.F1, OnMenuClick),
                new ToolStripSeparator(),
                MenuHelper.AddMenuItem("About", "ABOUT", Keys.None, true, false, Resources.about.ToBitmap(), OnMenuClick)
            });

            /* Status bar */
            _statusBar.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripStatusLabel("Game number: 0")
                {
                    AutoSize = false, Size = new Size(140, 16), TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = SystemColors.Control, ImageIndex = 0
                },
                new ToolStripStatusLabel("Elapsed time: 0s")
                {
                    AutoSize = false, Size = new Size(180, 16), TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = SystemColors.Control, ImageIndex = 1
                },
                new ToolStripStatusLabel("Total moves: 0")
                {
                    AutoSize = false, Size = new Size(120, 16), TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = SystemColors.Control, ImageIndex = 2
                },
                new ToolStripStatusLabel("Score: 0")
                {
                    AutoSize = false, Size = new Size(105, 16), TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = SystemColors.Control, ImageIndex = 3
                },
                new ToolStripStatusLabel("Welcome to Kanga's Solitaire. Are you ready for a challenge?")
                {
                    AutoSize = true, TextAlign = ContentAlignment.TopLeft, BackColor = SystemColors.Control,
                    ImageIndex = 5,
                    Spring = true, Visible = SettingsManager.Settings.Options.ShowTips
                },
                new ToolStripProgressBar
                {
                    Width = 120, Minimum = 0, Maximum = 52, Alignment = ToolStripItemAlignment.Right,
                    Visible = SettingsManager.Settings.Options.ShowProgress
                }
            });

            ToolStripManager.Renderer = ThemeManager.Renderer;
            ThemeManager.SetTheme(SettingsManager.Settings.Options.AppearanceStyle);

            var tips = new TipProvider();
            tips.OnTipChange += TipChanged;

            OnGameTimeChanged += TimeChanged;
            OnScoreChanged += ScoreChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            /* Set window position and size */
            var loc = SettingsManager.Settings.Location;
            if (loc == Point.Empty)
            {
                /* Scale form to less than the screen width/height */
                var screen = Utils.GetCurrentMonitor(this);
                var x = screen.Bounds.Width - 100;
                var y = screen.Bounds.Height - 100;
                Size = new Size(x, y);
                /* Set location to center screen */
                Location = new Point((screen.Bounds.Width/2) - (Size.Width/2), (screen.Bounds.Height/2) - (Size.Height/2));
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
            if (SettingsManager.Settings.Options.Confirm.OnExit)
            {
                if (CustomMessageBox.Show(this, "Are you sure you want to really quit?", "Quit Solitaire") == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            if (WindowState == FormWindowState.Normal)
            {
                SettingsManager.Settings.Location = Location;
                SettingsManager.Settings.Size = Size;
            }
            SettingsManager.Settings.Maximized = WindowState == FormWindowState.Maximized;
            if (SettingsManager.Settings.Options.SaveRecover && IsGameRunning)
            {
                SaveCurrentGame(string.Empty, true);
            }
            /* Through the cockpit window, we can now piss off :) */
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
                    AudioManager.PauseMusic();
                    break;

                case Keys.Control | Keys.Z:
                    /* This gets around the issue of undo being disabled in the menu, and not re-enabled until menu is opened again -
                     * What I don't want to do is clear the menu list and re-add every single call of Ctrl+Z... */
                    UndoMove();
                    break;

                case Keys.Control | Keys.Y:
                    /* This gets around the issue of undo being disabled in the menu, and not re-enabled until menu is opened again -
                     * What I don't want to do is clear the menu list and re-add every single call of Ctrl+Z... */
                    RedoMove();
                    break;

                case Keys.Control | Keys.H:
                    /* This is also disabled unless the menu is opened */                    
                    Hint();
                    break;
            }
            base.OnKeyDown(e);
        }

        private void OnSysTrayDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            Show();
            WindowState = _originalWindowState;
            _sysTray.Visible = false;
            AudioManager.ResumeMusic();
        }

        private void TimeChanged(int seconds)
        {
            if (!Visible)
            {
                return;
            }
            _statusBar.Items[0].Text = $@"Game number: {SettingsManager.Settings.Statistics.TotalGamesPlayed + 1}";
            _statusBar.Items[1].Text = $@"Elapsed time: {Utils.FormatTime(seconds)}";
        }        

        private void ScoreChanged(int score, int moves)
        {
            if (!Visible)
            {
                return;
            }
            _statusBar.Items[2].Text = $@"Total moves: {moves}";
            _statusBar.Items[3].Text = $@"Score: {score}";
            _statusBar.Items[3].ImageIndex = score < 0 ? 4 : 3;
            ((ToolStripProgressBar)_statusBar.Items[5]).Value = GameLogic.FoundationCount(this);
        }

        private void TipChanged(string tip)
        {
            if (!Visible)
            {
                return;
            }
            _statusBar.Items[4].Text = tip;
        }

        /* Menu click callback */
        private void OnMenuGameOpening(object sender, EventArgs e)
        {
            BuildMenuGame(_menuGame);
        }

        private void OnMenuOptionsOpening(object sender, EventArgs e)
        {
            BuildMenuOptions(_menuOptions);
        }

        private void OnMenuClick(object sender, EventArgs e)
        {
            var o = (ToolStripItem) sender;
            switch (o.Tag.ToString())
            {
                case "NEW":
                    NewGame(IsGameRunning);
                    break;

                case "BACKGROUND":
                    using (var back = new FrmBackground(this))
                    {
                        if (back.ShowDialog(this) == DialogResult.OK && back.SelectedBackground != null)
                        {
                            var index = ObjectData.Backgrounds.IndexOf(back.SelectedBackground);
                            if (index == -1)
                            {
                                index = 0;
                            }
                            SettingsManager.Settings.Options.Background = index;
                            Invalidate();
                        }
                    }
                    break;

                case "DECK":
                    using (var back = new FrmDeckBack(this, SettingsManager.Settings.Options.DeckBack))
                    {
                        if (back.ShowDialog(this) == DialogResult.OK && back.SelectedImage != -1)
                        {
                            SettingsManager.Settings.Options.DeckBack = back.SelectedImage;
                            Invalidate();
                        }
                    }                    
                    break;

                case "CARDS":
                    using (var cards = new FrmCards())
                    {
                        if (cards.ShowDialog(this) == DialogResult.OK && cards.Cards != null)
                        {
                            if (SettingsManager.Settings.Options.CardSet.Name.Equals(cards.Cards.Name,
                                    StringComparison.InvariantCultureIgnoreCase) ||
                                !BinarySerialize<Cards>.Load(
                                    Utils.MainDir($@"\data\gfx\cards\{cards.Cards.FilePath}", false), ref Cards))
                            {
                                return;
                            }
                            SettingsManager.Settings.Options.CardSet = cards.Cards;
                            Invalidate();
                        }
                    }
                    break;

                case "EASY":
                    CurrentGame.DeckRedeals = 0;
                    SettingsManager.Settings.Options.Difficulty = DifficultyLevel.Easy;
                    Invalidate();
                    break;

                case "MEDIUM":
                    CurrentGame.DeckRedeals = 0;
                    SettingsManager.Settings.Options.Difficulty = DifficultyLevel.Medium;
                    Invalidate();
                    break;

                case "HARD":
                    CurrentGame.DeckRedeals = 0;
                    SettingsManager.Settings.Options.Difficulty = DifficultyLevel.Hard;
                    Invalidate();
                    break;

                case "SOUND":
                    SettingsManager.Settings.Options.Sound.EnableEffects = !SettingsManager.Settings.Options.Sound.EnableEffects;
                    break;

                case "MUSIC":
                    var enable = !SettingsManager.Settings.Options.Sound.EnableMusic;
                    SettingsManager.Settings.Options.Sound.EnableMusic = enable;
                    if (enable)
                    {
                        AudioManager.PlayMusic(true);
                    }
                    else
                    {
                        AudioManager.StopMusic();
                    }
                    break;

                case "OPTIONS":
                    using (var opt = new FrmOptions())
                    {
                        opt.ShowDialog(this);
                    }
                    /* Apply statusbar progress change */
                    _statusBar.Items[4].Visible = SettingsManager.Settings.Options.ShowTips;
                    _statusBar.Items[5].Visible = SettingsManager.Settings.Options.ShowProgress;
                    break;

                case "SAVE":
                    using (var save = new FrmSaveLoad(SaveLoadType.SaveGame))
                    {
                        if (save.ShowDialog(this) == DialogResult.OK && save.SelectedFile != null)
                        {
                            /* Serialize out current game */
                            if (SaveCurrentGame(save.SelectedFile.FileName))
                            {
                                SettingsManager.Settings.SavedGames.Add(save.SelectedFile);
                                CustomMessageBox.Show(this, $"Current game was saved as '{save.SelectedFile.FriendlyName}'.", "Game Saved", CustomMessageBoxButtons.Ok, CustomMessageBoxIcon.Information);
                            }
                        }
                    }
                    break;

                case "UNDO":
                    /* This gets around the issue of undo being disabled in the menu, and not re-enabled until menu is opened again -
                     * What I don't want to do is clear the menu list and re-add every single call of Ctrl+Z... */
                    OnKeyDown(new KeyEventArgs(Keys.Control | Keys.Z));
                    break;

                case "REDO":
                    /* This gets around the issue of undo being disabled in the menu, and not re-enabled until menu is opened again -
                     * What I don't want to do is clear the menu list and re-add every single call of Ctrl+Z... */
                    OnKeyDown(new KeyEventArgs(Keys.Control | Keys.Y));
                    break;

                case "RESTART":
                    if (CustomMessageBox.Show(this, "Are you sure you want to restart the current game?", "Restart Current Game") == DialogResult.No)
                    {
                        return;
                    }
                    RestartGame(IsLoadedGame);
                    break;

                case "HINT":
                    OnKeyDown(new KeyEventArgs(Keys.Control | Keys.H));
                    break;

                case "AUTO":
                    AutoComplete();
                    break;

                case "STATS":
                    using (var stats = new FrmStatistics())
                    {
                        stats.ShowDialog(this);
                    }
                    break;

                case "EXIT":
                    Close();
                    break;

                case "HELP":
                    Help.ShowHelp(this, @"Kanga's Solitaire.chm");
                    break;

                case "ABOUT":
                    using (var about = new FrmAbout())
                    {
                        about.ShowDialog(this);
                    }
                    break;
            }
        }

        private static void OnMenuOptionsStyleClick(object sender, EventArgs e)
        {
            var o = (ToolStripItem) sender;
            if (!int.TryParse(o.Tag.ToString(), out var index))
            {
                return;
            }
            ThemeManager.SetTheme(index);
            SettingsManager.Settings.Options.AppearanceStyle = index;
        }

        /* Private menu building methods */
        private void BuildMenuGame(ToolStripDropDownItem m)
        {
            m.DropDownItems.Clear();

            m.DropDownItems.AddRange(
                new ToolStripItem[]
                {
                    MenuHelper.AddMenuItem("New game", "NEW", Keys.Control | Keys.N, true, false, Resources.newGame.ToBitmap(), OnMenuClick),
                    new ToolStripSeparator(),
                    MenuHelper.AddMenuItem("Save current game", "SAVE", Keys.Control | Keys.S, !GameCompleted && IsGameRunning, false, Resources.save.ToBitmap(), OnMenuClick),
                    MenuHelper.AddMenuItem("Restart game", "RESTART", Keys.None, !GameCompleted && IsGameRunning, false, Resources.restart.ToBitmap(), OnMenuClick),
                    new ToolStripSeparator(),
                    MenuHelper.AddMenuItem("Undo last move", "UNDO", Keys.Control | Keys.Z, Undo.UndoCount > 0 && !GameCompleted, false, Resources.undo.ToBitmap(), OnMenuClick),
                    MenuHelper.AddMenuItem("Redo move", "REDO", Keys.Control | Keys.Y, Undo.RedoCount > 0 && !GameCompleted, false, Resources.redo.ToBitmap(), OnMenuClick),
                    new ToolStripSeparator(),
                    MenuHelper.AddMenuItem("Hint...", "HINT", Keys.Control | Keys.H, IsGameRunning, false, Resources.hint.ToBitmap(), OnMenuClick),
                    MenuHelper.AddMenuItem("Auto complete...", "AUTO", Keys.Control | Keys.A, !GameCompleted && IsGameRunning, false, Resources.auto.ToBitmap(), OnMenuClick),
                    new ToolStripSeparator(),
                    MenuHelper.AddMenuItem("Statistics", "STATS", Keys.None, true, false, Resources.stats.ToBitmap(), OnMenuClick),
                    new ToolStripSeparator(),
                    MenuHelper.AddMenuItem("Exit", "EXIT", Keys.Alt | Keys.F4, true, false, Resources.close.ToBitmap(), OnMenuClick)
                });
        }

        private void BuildMenuOptions(ToolStripDropDownItem m)
        {
            m.DropDownItems.Clear();
            var style = MenuHelper.AddMenuItem("Appearance", Resources.appearance.ToBitmap());
            var index = 0;
            var themeIndex = SettingsManager.Settings.Options.AppearanceStyle;
            foreach (var d in ThemeManager.Presets.Select(preset => MenuHelper.AddMenuItem(preset.Name, index.ToString(CultureInfo.InvariantCulture), Keys.None, true, index == themeIndex, null, OnMenuOptionsStyleClick)))
            {
                style.DropDownItems.Add(d);
                index++;
            }
            var diff = MenuHelper.AddMenuItem("Difficulty", Resources.difficulty.ToBitmap());
            index = 0;
            foreach (var d in (DifficultyLevel[]) Enum.GetValues(typeof (DifficultyLevel)))
            {
                diff.DropDownItems.Add(MenuHelper.AddMenuItem(d.ToString(), d.ToString().ToUpper(), Keys.None, true,
                    (DifficultyLevel) index == SettingsManager.Settings.Options.Difficulty, null, OnMenuClick));
                index++;
            }
            m.DropDownItems.AddRange(new ToolStripItem[]
            {
                style, new ToolStripSeparator(), diff, new ToolStripSeparator(),
                MenuHelper.AddMenuItem("Enable sound effects", "SOUND", Keys.Alt | Keys.S, true, SettingsManager.Settings.Options.Sound.EnableEffects, null, OnMenuClick),
                MenuHelper.AddMenuItem("Enable music", "MUSIC", Keys.Alt | Keys.M, true, SettingsManager.Settings.Options.Sound.EnableMusic, null, OnMenuClick),
                new ToolStripSeparator(),
                MenuHelper.AddMenuItem("Choose background","BACKGROUND", Keys.Control | Keys.B, true, false, Resources.picture.ToBitmap(), OnMenuClick),
                MenuHelper.AddMenuItem("Choose card set", "CARDS", Keys.Control | Keys.C, true, false, Resources.cardSet.ToBitmap(), OnMenuClick),
                MenuHelper.AddMenuItem("Choose deck image","DECK", Keys.Control | Keys.D, true, false, Resources.deckBack.ToBitmap(), OnMenuClick),
                new ToolStripSeparator(), 
                MenuHelper.AddMenuItem("Game options", "OPTIONS", Keys.Control | Keys.O, true, false, Resources.options.ToBitmap(), OnMenuClick)
            });
        }
    }
}
