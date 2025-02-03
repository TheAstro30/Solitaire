/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Solitaire.Classes.Data;
using Solitaire.Classes.Helpers;
using Solitaire.Classes.Helpers.Logic;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.Helpers.UI;
using Solitaire.Classes.Serialization;
using Solitaire.Classes.UI.Internal;
using Solitaire.Forms;

namespace Solitaire.Classes.UI
{
    public class Game : Form
    {
        #region Member variables/Events

        /* Main graphics objects */
        public Cards Cards = new Cards(); /* Main look-up table for card images */

        public Deck MasterDeck = new Deck();
        public GraphicsObjectData ObjectData = new GraphicsObjectData();

        public bool IsGameRunning => _timerGame.Enabled;

        public bool IsDeckReDealt { get; set; }

        private Rectangle _stockRegion;
        public Size CardSize { get; private set; }
        public int GameCenter { get; private set; }

        /* Timers */
        private readonly Timer _timerStart;
        private readonly Timer _timerGame;
        private readonly Timer _checkWin;
        private readonly Timer _timerFireWorks;

        private readonly GraphicsRenderer _gfx;

        /* Dragging variables */
        private Bitmap _dragBitmap;
        private bool _isFoundationDrag;

        public bool IsDragging { get; set; }
        public List<Card> DraggingCards { get; set; }
        public int DragStackIndex { get; set; }
        public Point DragLocation { get; set; }

        /* Loaded game score and time */
        private int _time;
        private int _score;

        private bool _firstRun;

        /* Fireworks variables */
        private const int MaxFireWorks = 10;
        private readonly FireWork[] _fireWorks = new FireWork[MaxFireWorks];
        private static readonly Random FireWorkPosition = new Random();
        private bool _cleared;

        /* Current game being played */
        public bool IsLoadedGame { get; private set; }
        public GameData CurrentGame { get; set; }

        /* Game is won */
        public bool GameCompleted { get; set; }

        /* Hints */
        private int _hintIndex;
        private readonly Timer _hintTimer;
        private bool _hintDestShown;

        /* Events raised back to form */
        public event Action<int> OnGameTimeChanged;
        public event Action<int, int> OnScoreChanged;

        #endregion

        #region Constructor

        public Game()
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            /* Settings - this class is called first, before FrmGame, so load settings here */
            SettingsManager.Load();
            _firstRun = SettingsManager.Settings.Statistics.TotalGamesPlayed == 0;
            /* Double buffering */
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            _timerGame = new Timer {Interval = 1000};
            _timerGame.Tick += OnGameTimer;

            _checkWin = new Timer {Interval = 100};
            _checkWin.Tick += OnCheckWin;

            /* On my machine uses about 7% CPU when updating; 16% at 1ms - no real speed difference */
            _timerFireWorks = new Timer {Interval = 10};
            _timerFireWorks.Tick += OnFireWorks;

            _hintTimer = new Timer {Interval = 500};
            _hintTimer.Tick += OnHintTimer;

            /* Setup game data, checking graphics data exists */
            CurrentGame = new GameData();

            var cd = new Cards();
            var cardFile = Utils.MainDir($@"\data\gfx\cards\{SettingsManager.Settings.Options.CardSet.FilePath}");
            if (!File.Exists(cardFile) || !BinarySerialize<Cards>.Load(cardFile, ref cd))
            {
                /* Complete error */
                MessageBox.Show(@"Graphics files are missing, or corrupted. Please re-install Kanga's Solitaire.",
                    @"Load Error", MessageBoxButtons.OK);
                Environment.Exit(0);
            }

            Cards = cd;

            var g = new GraphicsObjectData();
            if (!File.Exists(Utils.MainDir(@"\data\gfx\obj.dat")) ||
                !BinarySerialize<GraphicsObjectData>.Load(Utils.MainDir(@"\data\gfx\obj.dat"), ref g))
            {
                /* Complete error */
                MessageBox.Show(@"Graphics files are missing, or corrupted. Please re-install Kanga's Solitaire.",
                    @"Load Error", MessageBoxButtons.OK);
                Environment.Exit(0);
            }

            ObjectData = g;

            /* Build master deck - when using different card packs, this doesn't have to be done each time; as the Suit and Value data never changes,
             * only the card images */
            foreach (var card in cd.Images.Select(c => new Card
            {
                Suit = c.Value.Suit,
                Value = c.Value.Value
            }))
            {
                MasterDeck.Add(card);
            }

            _gfx = new GraphicsRenderer(this);
            DraggingCards = new List<Card>();

            /* Start a new game, or show dialog */
            _timerStart = new Timer {Interval = 500, Enabled = true};
            _timerStart.Tick += OnGameStart;
        }

        #endregion

        #region Public methods

        #region New game

        public void NewGame(bool ask = true)
        {
            if (SettingsManager.Settings.Options.Confirm.OnNewLoad && ask && !GameCompleted)
            {
                /* Ask user if they want to start a new game */
                if (CustomMessageBox.Show(this, "Are you sure you want to quit the current game?",
                    "Quit Current Game") == DialogResult.No)
                {
                    return;
                }
            }

            var draw3 = false;
            using (var ng = NewGameDialog.Show(this))
            {
                if (ng.DialogResult == DialogResult.Cancel)
                {
                    return;
                }

                switch (ng.NewGameDialogResult)
                {
                    case NewGameDialogResult.DrawThree:
                        draw3 = true;
                        break;

                    case NewGameDialogResult.LoadGame:
                        using (var load = new FrmSaveLoad(SaveLoadType.LoadGame))
                        {
                            if (load.ShowDialog(this) == DialogResult.OK && load.SelectedFile != null)
                            {
                                /* Serialize out current game */
                                if (LoadSavedGame(load.SelectedFile.FileName))
                                {
                                    return;
                                }

                                CustomMessageBox.Show(this, $"Unable to load game '{load.SelectedFile.FriendlyName}'.",
                                    "Error", CustomMessageBoxButtons.Ok, CustomMessageBoxIcon.Error);
                            }
                        }

                        NewGame(false);
                        return;
                }
            }

            if (!_firstRun)
            {
                SettingsManager.Settings.Statistics.TotalGamesPlayed++;
                if (!GameCompleted && IsGameRunning)
                {
                    SettingsManager.Settings.Statistics.GamesLost++;
                }
            }
            else
            {
                /* Game count is 0, so we don't increment until NewGame is called again on new game, win/loss */
                _firstRun = false;
            }

            Undo.Clear(); /* Clear undo history */
            GameLogic.ClearHints();
            GameCompleted = false;
            CurrentGame = new GameData {IsDrawThree = draw3};
            _stockRegion = new Rectangle();
            _timerFireWorks.Enabled = false;
            _timerGame.Enabled = true;
            /* Copy master deck to playing deck (this is important as cards are all over the place in the class) */
            IsLoadedGame = false;
            IsDeckReDealt = false;
            /* Shuffle the master deck - makes it more random */
            MasterDeck.Shuffle();
            CurrentGame.StockCards = new Deck(MasterDeck);
            GameLogic.BuildStacks(this);
            AudioManager.Play(SoundType.Shuffle);
            /* Set restart point (yes, it's duplicate data, but allows us to load a saved game and restart from beginning) */
            CurrentGame.RestartPoint = new GameData(CurrentGame);
            OnGameTimeChanged?.Invoke(CurrentGame.GameTime);
            OnScoreChanged?.Invoke(CurrentGame.GameScore, CurrentGame.Moves);
            Invalidate();
        }

        #endregion

        #region Load/save game

        public bool LoadSavedGame(string fileName, bool saveRecover = false)
        {
            /* Load a saved game */
            var d = new GameData();
            var file = $@"\KangaSoft\Solitaire\{(saveRecover ? "recovery.dat" : $@"saved\{fileName}")}";
            if (!BinarySerialize<GameData>.Load(Utils.MainDir(file, true), ref d))
            {
                return false;
            }

            Undo.Clear(); /* Clear undo history */
            GameLogic.ClearHints();
            CurrentGame = d;
            IsLoadedGame = true;

            /* Set time and score (used for restart) */
            _time = CurrentGame.GameTime;
            _score = CurrentGame.GameScore;
            _firstRun = false;
            if (!GameCompleted && !saveRecover)
            {
                SettingsManager.Settings.Statistics.TotalGamesPlayed++;
                SettingsManager.Settings.Statistics.GamesLost++;
            }

            AudioManager.Play(SoundType.Shuffle);
            OnGameTimeChanged?.Invoke(CurrentGame.GameTime);
            OnScoreChanged?.Invoke(CurrentGame.GameScore, CurrentGame.Moves);
            _timerFireWorks.Enabled = false;
            _timerGame.Enabled = true;

            GameCompleted =
                false; /* Forgot to change this until now, other menu items like save, etc, are greyed out */

            Invalidate();
            return true;
        }

        public bool SaveCurrentGame(string fileName, bool saveRecover = false)
        {
            /* Save current game - if it's a completed game, don't save it */
            var file = Utils.MainDir($@"\KangaSoft\Solitaire\{(saveRecover ? "recovery.dat" : $@"saved\{fileName}")}",
                true);
            if (!GameCompleted)
            {
                return BinarySerialize<GameData>.Save(file, CurrentGame);
            }

            if (saveRecover)
            {
                Utils.DeleteFile(file);
            }

            return false;
        }

        #endregion

        #region Undo/Redo

        public void UndoMove()
        {
            var d = Undo.UndoLastMove(CurrentGame);
            if (d == null)
            {
                return;
            }

            var t = CurrentGame.GameTime;
            var s = CurrentGame.GameScore;
            CurrentGame = new GameData(d)
            {
                GameTime = t, GameScore = s, RestartPoint = new GameData(d.RestartPoint)
            }; /* Forgot to renew restart point */
            AudioManager.Play(SoundType.Drop);
            Invalidate();
        }

        public void RedoMove()
        {
            var d = Undo.RedoMove();
            if (d == null)
            {
                return;
            }

            var t = CurrentGame.GameTime;
            var s = CurrentGame.GameScore;
            CurrentGame = new GameData(d) {GameTime = t, GameScore = s, RestartPoint = new GameData(d.RestartPoint)};
            AudioManager.Play(SoundType.Drop);
            Invalidate();
        }

        #endregion

        #region Restart

        public void RestartGame(bool keepTimeAndScore)
        {
            var d = CurrentGame.RestartPoint;
            if (d != null)
            {
                /* If it's a loaded game, we don't want to reset time or score */
                CurrentGame = keepTimeAndScore
                    ? new GameData(d) {GameTime = _time, GameScore = _score, DeckRedeals = 0}
                    : new GameData(d);
                /* Make sure to set the Restart point */
                CurrentGame.RestartPoint = new GameData(CurrentGame);
            }

            /* Reset time and score */
            OnGameTimeChanged?.Invoke(CurrentGame.GameTime);
            OnScoreChanged?.Invoke(CurrentGame.GameScore, CurrentGame.Moves);
            AudioManager.Play(SoundType.Shuffle);
            Invalidate();
        }

        #endregion

        #region Auto complete

        public void AutoComplete()
        {
            /* Iterate through any drawn cards and play stacks moving them to each ace home stack */
            var complete = true;
            var success = false;
            while (complete)
            {
                /* Stock card */
                var movedCards = 0;
                Card card;
                if (CurrentGame.WasteCards.Count > 0)
                {
                    card = CurrentGame.WasteCards[CurrentGame.WasteCards.Count - 1];
                    /* Is it an ace ? - Find free home slot; if not see if it can be completed */
                    if (card.Value == 1 && GameLogic.AddAceToFreeSlot(this, card) || GameLogic.IsCompleted(this, card))
                    {
                        success = true;
                        CurrentGame.WasteCards.Remove(card);
                        CurrentGame.Moves++;
                        movedCards++;
                        CurrentGame.GameScore += 10;
                    }
                }

                /* Each stack */
                for (var stack = 0; stack <= 6; stack++)
                {
                    var s = CurrentGame.Tableau[stack];
                    if (s.Cards.Count == 0)
                    {
                        continue;
                    }

                    card = s.Cards[s.Cards.Count - 1];
                    /* Is it an ace ? - Find free home slot; if not see if it can be completed */
                    if (card.IsHidden)
                    {
                        AutoTurnCard(stack);
                        continue;
                    }

                    if (!GameLogic.IsCompleted(this, card) &&
                        (card.Value != 1 || !GameLogic.AddAceToFreeSlot(this, card)))
                    {
                        continue;
                    }

                    success = true;
                    s.Cards.Remove(card);
                    movedCards++;
                    CurrentGame.Moves++;
                    CurrentGame.GameScore += 10;
                    AutoTurnCard(stack);
                }

                complete = movedCards != 0;
            }

            if (!success)
            {
                return;
            }

            Invalidate();
            AudioManager.Play(SoundType.Complete);
            OnScoreChanged?.Invoke(CurrentGame.GameScore, CurrentGame.Moves);
        }

        #endregion

        #region Show hint

        public void Hint()
        {
            var h = GameLogic.GetHint(this);
            if (h.Count > 0)
            {
                if (_hintIndex > h.Count - 1)
                {
                    _hintIndex = 0;
                }

                _gfx.DrawFocusRing(h[_hintIndex].SourceRegion, h[_hintIndex].SourceCardCount);
                _hintTimer.Tag = h[_hintIndex].DestinationRegion;
                _hintTimer.Enabled = true;
                _hintIndex++;
            }
            else
            {
                /* Tell user to draw */
                _hintIndex = 0;
                if (CurrentGame.StockCards.Count == 0)
                {
                    AudioManager.Play(SoundType.NoHint);
                    return;
                }

                _hintTimer.Tag = null;
                _hintTimer.Enabled = true;
                _gfx.DrawFocusRing(_stockRegion, 0);
            }
        }

        #endregion

        #endregion

        #region Overrides

        #region Form

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            /* Update stats */
            if (!GameCompleted && !SettingsManager.Settings.Options.SaveRecover)
            {
                SettingsManager.Settings.Statistics.GamesLost++;
            }

            /* Dump settings */
            SettingsManager.Save();
            /* Stop all running timers */
            _timerFireWorks.Enabled = false;
            _timerGame.Enabled = false;
            base.OnFormClosing(e);
        }

        #endregion

        #region Mouse

        protected override void OnMouseDown(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    DragLocation = e.Location;
                    Card card;
                    switch (HitTest.CompareClick(this, e.Location, _stockRegion, out var data))
                    {
                        case HitTestType.Stock:
                            Undo.AddMove(CurrentGame);
                            if (!GameLogic.Deal(this))
                            {
                                base.OnMouseDown(e);
                                return;
                            }

                            if (IsDeckReDealt)
                            {
                                IsDeckReDealt = false;
                                CurrentGame.GameScore -= 5;
                                OnScoreChanged?.Invoke(CurrentGame.GameScore, CurrentGame.Moves);
                            }

                            Invalidate();
                            break;

                        case HitTestType.Waste:
                            /* Pick up first card on pile, begin drag (but first check it can be moved to foundation) */
                            Undo.AddMove(CurrentGame);
                            DragStackIndex = -1;
                            card = CurrentGame.WasteCards[CurrentGame.WasteCards.Count - 1];
                            card.IsHidden = false;
                            DraggingCards.Add(card);
                            CurrentGame.WasteCards.Remove(card);
                            IsDragging = true;
                            Invalidate();
                            break;

                        case HitTestType.Foundation:
                            /* Pick up first card on stack, begin drag */
                            var stack = CurrentGame.Foundation[data.StackIndex];
                            if (stack.Cards.Count == 0)
                            {
                                return;
                            }

                            card = stack.Cards[stack.Cards.Count - 1];
                            DragStackIndex = data.StackIndex;
                            DraggingCards.Add(card);
                            stack.Cards.Remove(card);
                            if (stack.Cards.Count == 0)
                            {
                                stack.Suit = Suit.None;
                            }

                            _isFoundationDrag = true;
                            IsDragging = true;
                            Invalidate();
                            break;

                        case HitTestType.Tableau:
                            /* Begin stack drag - validate clicked area can be dragged */
                            if (data.Cards != null)
                            {
                                if (data.Cards[data.CardIndex].IsHidden)
                                {
                                    if (data.CardIndex != data.Cards.Count - 1)
                                    {
                                        /* Invalid drag */
                                        return;
                                    }

                                    /* Turn card over */
                                    Undo.AddMove(CurrentGame);
                                    data.Cards[data.CardIndex].IsHidden = false;
                                    Invalidate();
                                    AudioManager.Play(SoundType.Deal);
                                    CurrentGame.GameScore += 5;
                                    OnScoreChanged?.Invoke(CurrentGame.GameScore, CurrentGame.Moves);
                                    return;
                                }

                                if (data.CardIndex < data.Cards.Count - 1)
                                {
                                    /* Valid cards on top of this card */
                                    for (var i = data.CardIndex; i <= data.Cards.Count - 1; i++)
                                    {
                                        card = data.Cards[i];
                                        if (i < data.Cards.Count - 1 && !GameLogic.IsValidMove(data.Cards[i + 1], card))
                                        {
                                            return;
                                        }

                                        /* Add card to list */
                                        DraggingCards.Add(card);
                                    }

                                    Undo.AddMove(CurrentGame);
                                    DragStackIndex = data.StackIndex;
                                    /* Remove dragging cards from stack - I can't do it in above loop as it modifies the list it's comparing */
                                    foreach (var c in DraggingCards)
                                    {
                                        CurrentGame.Tableau[DragStackIndex].Cards.Remove(c);
                                    }
                                }
                                else
                                {
                                    /* Single card */
                                    Undo.AddMove(CurrentGame);
                                    DragStackIndex = data.StackIndex;
                                    card = data.Cards[data.CardIndex];
                                    DraggingCards.Add(card);
                                    CurrentGame.Tableau[data.StackIndex].Cards.Remove(card);
                                }

                                IsDragging = true;
                                Invalidate();
                            }

                            break;
                    }

                    break;

                case MouseButtons.Right:
                    AutoComplete();
                    break;
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!IsDragging)
            {
                return;
            }

            DragLocation = e.Location;
            Invalidate();
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (IsDragging)
            {
                /* Hit test, validate move, drop */
                if (DraggingCards.Count == 0)
                {
                    return;
                }

                /* This rectangle intersect code does work better than using the HitTest.CompareClick method */
                var src = new Rectangle(e.Location.X - (CardSize.Width / 2), e.Location.Y + 5, CardSize.Width,
                    CardSize.Height);
                switch (HitTest.CompareDrop(this, src, out var data))
                {
                    case HitTestType.Foundation:
                        /* Dropping a card on a home stack? Cards must go from ace to king and be the same suit */
                        var stack = CurrentGame.Foundation[data.StackIndex];
                        var c = DraggingCards[0];
                        if (stack.Cards.Count == 0 && c.Value == 1)
                        {
                            /* It's an ace, drop it here */
                            stack.Suit = c.Suit;
                        }

                        stack.Cards.Add(c);
                        CurrentGame.GameScore += 10;
                        CurrentGame.Moves++;
                        AutoTurnCard();
                        break;

                    case HitTestType.Tableau:
                        if (data.CardIndex == -1 && DraggingCards[0].Value == 13)
                        {
                            /* Slot is vacant and first card being dragged is a king */
                            CurrentGame.Tableau[data.StackIndex].Cards.AddRange(DraggingCards);
                            /* Make sure no cards are hidden */
                            foreach (var card in CurrentGame.Tableau[data.StackIndex].Cards)
                            {
                                card.IsHidden = false;
                            }

                            CurrentGame.GameScore += 5;
                            CurrentGame.Moves++;
                            AutoTurnCard();
                        }
                        else if (data.CardIndex >= 0)
                        {
                            /* Cards already present in this stack and is valid drop */
                            CurrentGame.Tableau[data.StackIndex].Cards.AddRange(DraggingCards);
                            CurrentGame.GameScore += _isFoundationDrag ? -15 : 5;
                            CurrentGame.Moves++;
                            AutoTurnCard();
                        }
                        else
                        {
                            /* Put it back where it came from */
                            GameLogic.ReturnCardsToSource(this, _isFoundationDrag);
                        }

                        break;

                    default:
                        /* Put it back where it came from */
                        GameLogic.ReturnCardsToSource(this, _isFoundationDrag);
                        break;
                }

                IsDragging = false;
                _isFoundationDrag = false;
                DraggingCards = new List<Card>();
                if (_dragBitmap != null)
                {
                    _dragBitmap.Dispose();
                    _dragBitmap = null;
                }

                Invalidate();
                AudioManager.Play(SoundType.Drop);
                OnScoreChanged?.Invoke(CurrentGame.GameScore, CurrentGame.Moves);
            }

            GameLogic.ClearHints();
            base.OnMouseUp(e);
        }

        #endregion

        #region Resize override

        protected override void OnResize(EventArgs e)
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            /* Calculate what the size of the images should be based on clientsize */
            var img = ObjectData.CardBacks[0];
            var ratioX = (double) ClientSize.Width / img.Width;
            var ratioY = (double) ClientSize.Height / img.Height;
            /* Use whichever multiplier is smaller */
            var ratio = ratioX < ratioY ? ratioX : ratioY;
            /* Now we can get the new height and width - only to the actual card size */
            var newWidth = Convert.ToInt32(img.Width * ratio) / 5;
            var newHeight = Convert.ToInt32(img.Height * ratio) / 5;
            CardSize = new Size(newWidth > img.Width ? img.Width : newWidth,
                newHeight > img.Height ? img.Height : newHeight);
            /* This is used for centering drawing of images on X axis and for mouse hit test */
            GameCenter = (ClientSize.Width / 2) - (CardSize.Width / 2);
            GameLogic.ClearHints();
            Invalidate();
            base.OnResize(e);
        }

        #endregion

        #region Paint override

        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime || !Visible)
            {
                /* Steve Jobs was here */
                return;
            }
            if (!_timerFireWorks.Enabled)
            {
                _cleared = false;
                if (_dragBitmap == null)
                {
                    var rect = _gfx.Draw(e.Graphics);
                    /* It shouldn't be empty... */
                    if (rect != Rectangle.Empty)
                    {
                        _stockRegion = rect;
                    }
                }
                /* Draw dragging cards */
                if (IsDragging)
                {
                    if (_dragBitmap == null)
                    {
                        /* Clone the screen - this just speeds up drawing dragging cards as we don't have to do the above loops in the GraphicsRenderer drawing methods */
                        _dragBitmap = new Bitmap(ClientSize.Width, ClientSize.Height);
                        using (var g = Graphics.FromImage(_dragBitmap))
                        {
                            _gfx.Draw(g);
                        }
                    }
                    e.Graphics.DrawImage(_dragBitmap, 0, 0, ClientSize.Width, ClientSize.Height);
                    _gfx.DrawDrag(e.Graphics);
                }
                else
                {
                    _checkWin.Enabled = true;
                }
            }
            else
            {
                /* Draw fireworks winning sequence */
                if (!_cleared)
                {
                    /* Clear screen ONCE - set form backcolor to black, and the next foreach loop draws much fast with less CPU usage;
                     * .Clear() is actually a CPU hog */
                    _cleared = true;
                    e.Graphics.Clear(Color.Black);
                }

                /* On my machine (which the specs aren't that great), I get about 6-7% CPU usage with this drawing loop, which is acceptable */
                foreach (var fw in _fireWorks.Where(fw => fw != null))
                {
                    fw.Paint(e.Graphics);
                }
            }
            base.OnPaint(e);
        }

        #endregion
        #endregion

        #region Private methods
        #region Timers
        private void OnGameStart(object sender, EventArgs e)
        {
            _timerStart.Enabled = false;
            if (SettingsManager.Settings.Options.SaveRecover && LoadSavedGame(string.Empty, true))
            {
                return;
            }
            NewGame(false);
        }

        private void OnGameTimer(object sender, EventArgs e)
        {
            OnGameTimeChanged?.Invoke(CurrentGame.GameTime);
            CurrentGame.GameTime++;
            if (CurrentGame.GameTime % 10 != 0)
            {
                return;
            }
            CurrentGame.GameScore -= 5;
            OnScoreChanged?.Invoke(CurrentGame.GameScore, CurrentGame.Moves);
        }

        private void OnCheckWin(object sender, EventArgs e)
        {
            _checkWin.Enabled = false;            
            if (!GameLogic.CheckWin(this))
            {
                return;
            }
            _timerGame.Enabled = false;
            _timerFireWorks.Enabled = true;
            /* Game is won. It's kind of easier to do this check here from the paint method, as it's called most of the time */
            GameCompleted = true;
            SettingsManager.Settings.Statistics.GamesWon++;
            /* Award time bonuses */
            if (CurrentGame.GameTime <= 120)
            {
                CurrentGame.GameScore += 1000 * 3;
            }
            else if (CurrentGame.GameTime <= 300)
            {
                CurrentGame.GameScore += 200 * 3;
            }

            OnGameTimeChanged?.Invoke(CurrentGame.GameTime);
            OnScoreChanged?.Invoke(CurrentGame.GameScore, CurrentGame.Moves);
            SettingsManager.UpdateStats(CurrentGame.GameTime, CurrentGame.GameScore);
            AudioManager.Play(SoundType.Win);
            /* Remove recovery file, if it exists */
            Utils.DeleteFile(Utils.MainDir(@"\KangaSoft\Solitaire\recovery.dat", true));
            if (CustomMessageBox.Show(this, "Congratulations! You win!\r\n\r\nWould you like to deal again?", "Congratulations!", CustomMessageBoxIcon.Information) == DialogResult.Yes)
            {
                NewGame(false);
            }
        }

        private void OnFireWorks(object sender, EventArgs e)
        {
            for (var i = 0; i < MaxFireWorks; ++i)
            {
                if (_fireWorks[i] != null && !_fireWorks[i].Update())
                {
                    _fireWorks[i] = null;
                }
            }

            if (FireWorkPosition.Next(10) == 0)
            {
                for (var i = 0; i < MaxFireWorks; ++i)
                {
                    if (_fireWorks[i] != null)
                    {
                        continue;
                    }
                    _fireWorks[i] = new FireWork(ClientRectangle.Width, ClientRectangle.Height);
                    break;
                }
            }
            Invalidate();
        }

        private void OnHintTimer(object sender, EventArgs e)
        {        
            if (_hintDestShown)
            {
                Invalidate();
                _hintDestShown = false;                
                _hintTimer.Enabled = false;
                return;
            }
            _hintDestShown = true;
            var o = (Timer)sender;
            if (o.Tag == null)
            {
                return;
            }
            var tag = (Rectangle)o.Tag;
            if (tag == Rectangle.Empty)
            {
                /* Do nothing */
                return;
            }
            /* Draw hint destination highlight */
            _gfx.DrawFocusRing(tag, 0);
        }
        #endregion

        #region Auto turn card
        private void AutoTurnCard(int stackIndex = -1)
        {
            /* Automatically turn over hidden cards on tableau stacks and award player points */
            var index = stackIndex != -1 ? stackIndex : DragStackIndex;
            if (index == -1 || !SettingsManager.Settings.Options.AutoTurn)
            {
                return;
            }
            var stack = CurrentGame.Tableau[index].Cards;
            if (stack.Count == 0 || !stack[stack.Count - 1].IsHidden)
            {
                return;
            }
            Undo.AddMove(CurrentGame);
            stack[stack.Count - 1].IsHidden = false;
            CurrentGame.GameScore += 5;
            OnScoreChanged?.Invoke(CurrentGame.GameScore, CurrentGame.Moves);
        }
        #endregion
        #endregion
    }
}
