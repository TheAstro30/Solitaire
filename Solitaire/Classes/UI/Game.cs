﻿/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Solitaire.Classes.Data;
using Solitaire.Classes.Helpers;
using Solitaire.Classes.Helpers.Logic;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.Helpers.System;
using Solitaire.Classes.Helpers.UI;
using Solitaire.Classes.Serialization;
using Solitaire.Classes.UI.Internal;

namespace Solitaire.Classes.UI
{
    public class Game : Form
    {
        #region Member variables/Events
        /* Main graphics objects */
        public Deck MasterDeck = new Deck();
        public GraphicsObjectData ObjectData = new GraphicsObjectData();

        private Size _cardSize;        
        private int _gameCenter;
        private Rectangle _deckRegion;

        /* Timers */
        private readonly Timer _timerGame;
        private readonly Timer _checkWin;
        private readonly Timer _timerFireWorks;

        /* Dragging variables */
        private bool _isDragging;
        private bool _isHomeDrag;
        private List<Card> _draggingCards = new List<Card>();
        private int _dragStackIndex;
        private Point _dragLocation;
        private Bitmap _dragBitmap;

        /* Loaded game score and time */
        private int _time;
        private int _score;

        /* Fireworks variables */
        private const int MaxFireWorks = 10;
        private readonly FireWork[] _fireWorks = new FireWork[MaxFireWorks];
        private static readonly Random FireWorkPosition = new Random();

        /* Current game being played */
        public bool IsLoadedGame { get; private set; }
        public GameData CurrentGame { get; set; }
        
        /* Game is won */
        public bool GameCompleted { get; set; }

        /* Events raised back to form */
        public event Action<int> OnGameTimeChanged;
        public event Action<int> OnScoreChanged;
        #endregion

        #region Constructor
        public Game()
        {
            if (DesignMode)
            {
                return;
            }
            /* Settings - this class is called first, before FrmGame, so load settings here */
            SettingsManager.Load();
            /* Double buffering */
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            _timerGame = new Timer
            {
                Interval = 1000
            };
            _timerGame.Tick += OnGameTimer;

            _checkWin = new Timer
            {
                Interval = 100
            };
            _checkWin.Tick += OnCheckWin;

            _timerFireWorks = new Timer
            {
                Interval = 10
            };
            _timerFireWorks.Tick += OnFireWorks;

            /* Setup game data */
            CurrentGame = new GameData();
            var d = new Deck();
            if (!BinarySerialize<Deck>.Load(AppPath.MainDir(@"\data\gfx\cards.dat"), ref d))
            {
                /* Complete error */
                return;
            }
            var g = new GraphicsObjectData();
            if (!BinarySerialize<GraphicsObjectData>.Load(AppPath.MainDir(@"\data\gfx\obj.dat"), ref g))
            {
                /* Complete error */
                return;
            }
            ObjectData = g;
            MasterDeck = new Deck(d);
        }
        #endregion

        #region Public methods
        #region New game
        public bool NewGame(bool ask = true)
        {
            if (ask && !GameCompleted)
            {
                /* Ask user if they want to start a new game */
                if (CustomMessageBox.Show(this, "Are you sure you want to quit the current game?", "Quit Current Game") == DialogResult.No)
                {
                    return false;
                }
                SettingsManager.Settings.Statistics.GamesLost++;
            }
            SettingsManager.Settings.Statistics.TotalGamesPlayed++;
            Undo.Clear(); /* Clear undo history */
            GameCompleted = false;
            CurrentGame = new GameData();
            _timerFireWorks.Enabled = false;
            _timerGame.Enabled = true;
            /* Copy master deck to playing deck (this is important as cards are all over the place in the class) */
            IsLoadedGame = false;
            CurrentGame.GameDeck = new Deck(MasterDeck);
            /* Shuffle the deck */
            CurrentGame.GameDeck.Shuffle();
            GameLogic.BuildStacks(this);

            _deckRegion = new Rectangle();
            AudioManager.Play(SoundType.Shuffle);
            if (OnGameTimeChanged != null)
            {
                OnGameTimeChanged(CurrentGame.GameTime);
            }
            if (OnScoreChanged != null)
            {
                OnScoreChanged(CurrentGame.GameScore);
            }
            Invalidate();
            return true;
        }
        #endregion

        #region Load/save game
        public bool LoadSavedGame()
        {
            /* Load a saved game */
            var d = new GameData();
            if (!BinarySerialize<GameData>.Load(AppPath.MainDir(@"\KangaSoft\Solitaire\saved.dat", true), ref d))
            {
                return false;
            }
            Undo.Clear(); /* Clear undo history */
            CurrentGame = d;
            IsLoadedGame = true;

            /* Set time and score (used for restart) */
            _time = CurrentGame.GameTime;
            _score = CurrentGame.GameScore;

            SettingsManager.Settings.Statistics.TotalGamesPlayed++;
            if (!GameCompleted)
            {
                SettingsManager.Settings.Statistics.GamesLost++;
            }
            AudioManager.Play(SoundType.Shuffle);
            if (OnGameTimeChanged != null)
            {
                OnGameTimeChanged(CurrentGame.GameTime);
            }
            if (OnScoreChanged != null)
            {
                OnScoreChanged(CurrentGame.GameScore);
            }
            _timerFireWorks.Enabled = false;
            _timerGame.Enabled = true;
            Invalidate();
            return true;
        }

        public bool SaveCurrentGame()
        {
            /* Save current game - if it's a completed game, don't save it */
            return !GameCompleted && BinarySerialize<GameData>.Save(AppPath.MainDir(@"\KangaSoft\Solitaire\saved.dat", true), CurrentGame);
        }
        #endregion

        #region Undo
        public void UndoMove()
        {
            var d = Undo.UndoLastMove();
            if (d == null)
            {
                return;
            }
            var t = CurrentGame.GameTime;
            var s = CurrentGame.GameScore;
            CurrentGame = new GameData(d) { GameTime = t, GameScore = s };
            AudioManager.Play(SoundType.Drop);
            /* Penalize by 2 points on the score for using undo */
            CurrentGame.GameScore -= 2;
            if (OnScoreChanged != null)
            {
                OnScoreChanged(CurrentGame.GameScore);
            }
            Invalidate();
        }
        #endregion

        #region Restart
        public void RestartGame(bool keepTimeAndScore)
        {
            var d = Undo.GetRestartPoint();
            if (d != null)
            {
                /* If it's a loaded game, we don't want to reset time or score */
                CurrentGame = keepTimeAndScore ? new GameData(d) {GameTime = _time, GameScore = _score} : new GameData(d);
            }
            /* Reset time and score */
            if (OnGameTimeChanged != null)
            {
                OnGameTimeChanged(CurrentGame.GameTime);
            }
            if (OnScoreChanged != null)
            {
                OnScoreChanged(CurrentGame.GameScore);
            }
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
                /* Deck card */
                var movedCards = 0;
                Card card;
                if (CurrentGame.DealtCards.Count > 0)
                {
                    card = CurrentGame.DealtCards[CurrentGame.DealtCards.Count - 1];
                    /* Is it an ace ? - Find free home slot; if not see if it can be completed */
                    if (card.Value == 1 && GameLogic.AddAceToFreeSlot(this, card) || GameLogic.IsCompleted(this, card))
                    {
                        success = true;
                        CurrentGame.DealtCards.Remove(card);
                        movedCards++;
                        CurrentGame.GameScore += 10;
                    }
                }
                /* Each stack */
                foreach (var stack in CurrentGame.PlayingStacks.Where(stack => stack.Cards.Count > 0))
                {
                    card = stack.Cards[stack.Cards.Count - 1];
                    /* Is it an ace ? - Find free home slot; if not see if it can be completed */
                    if ((card.IsHidden || !GameLogic.IsCompleted(this, card)) && (card.Value != 1 || !GameLogic.AddAceToFreeSlot(this, card)))
                    {
                        continue;
                    }
                    success = true;
                    stack.Cards.Remove(card);
                    movedCards++;
                    CurrentGame.GameScore += 10;
                }
                complete = movedCards != 0;
            }
            if (!success)
            {
                return;
            }
            Invalidate();
            AudioManager.Play(SoundType.Complete);
            if (OnScoreChanged != null)
            {
                OnScoreChanged(CurrentGame.GameScore);
            }
        }
        #endregion
        #endregion

        #region Overrides
        #region Form
        protected override void OnLoad(EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }
            NewGame(false);
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            /* Update stats */
            if (!GameCompleted)
            {
                SettingsManager.Settings.Statistics.GamesLost++;
            }
            /* Dump settings */
            SettingsManager.Save();
            base.OnFormClosing(e);
        }
        #endregion

        #region Mouse
        protected override void OnMouseDown(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    _dragLocation = e.Location;
                    HitTestData data;
                    Card card;
                    switch (HitTest.Compare(this, e.Location, _deckRegion, out data))
                    {
                        case HitTestType.Deck:
                            Undo.AddMove(CurrentGame);
                            GameLogic.Deal(this);
                            if (CurrentGame.GameDeck.IsDeckReshuffled)
                            {
                                CurrentGame.GameDeck.IsDeckReshuffled = false;
                                CurrentGame.GameScore -= 15;
                                if (OnScoreChanged != null)
                                {
                                    OnScoreChanged(CurrentGame.GameScore);
                                }
                            }
                            Invalidate();
                            break;

                        case HitTestType.Dealt:
                            /* Pick up first card on pile, begin drag */
                            Undo.AddMove(CurrentGame);
                            _dragStackIndex = -1;
                            card = CurrentGame.DealtCards[CurrentGame.DealtCards.Count - 1];
                            card.IsHidden = false;
                            _draggingCards.Add(card);
                            CurrentGame.DealtCards.Remove(card);
                            _isDragging = true;
                            Invalidate();
                            break;

                        case HitTestType.HomeStack:
                            /* Pick up first card on stack, begin drag */
                            var stack = CurrentGame.HomeStacks[data.StackIndex];
                            if (stack.Cards.Count == 0)
                            {
                                return;
                            }
                            card = stack.Cards[stack.Cards.Count - 1];
                            _dragStackIndex = data.StackIndex;
                            _draggingCards.Add(card);
                            stack.Cards.Remove(card);
                            if (stack.Cards.Count == 0)
                            {
                                stack.Suit = Suit.None;
                            }
                            _isHomeDrag = true;
                            _isDragging = true;
                            Invalidate();
                            break;

                        case HitTestType.PlayStack:
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
                                    if (OnScoreChanged != null)
                                    {
                                        OnScoreChanged(CurrentGame.GameScore);
                                    }
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
                                        _draggingCards.Add(card);
                                    }
                                    Undo.AddMove(CurrentGame);
                                    _dragStackIndex = data.StackIndex;
                                    /* Remove dragging cards from stack - I can't do it in above loop as it modifies the list it's comparing */
                                    foreach (var c in _draggingCards)
                                    {
                                        CurrentGame.PlayingStacks[_dragStackIndex].Cards.Remove(c);
                                    }
                                }
                                else
                                {
                                    /* Single card */
                                    Undo.AddMove(CurrentGame);
                                    _dragStackIndex = data.StackIndex;
                                    card = data.Cards[data.CardIndex];
                                    _draggingCards.Add(card);
                                    CurrentGame.PlayingStacks[data.StackIndex].Cards.Remove(card);
                                }
                                _isDragging = true;
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
            if (!_isDragging)
            {
                return;
            }
            _dragLocation = e.Location;
            Invalidate();
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            /* TODO: I need to figure out how to check if a card is sitting on "top" of a valid drop, not just relying on x and y mouse location... */
            if (_isDragging)
            {
                /* Hit test, validate move, drop */
                if (_draggingCards.Count == 0)
                {
                    return;
                }
                HitTestData data;
                /* I need to think of a way of modifying the hittest to include the area of the dragging card... */
                switch (HitTest.Compare(this, e.Location, _deckRegion, out data))
                {
                    case HitTestType.HomeStack:
                        /* Dropping a card on a home stack? Cards must go from ace to king and be the same suit */
                        if (_draggingCards.Count > 1)
                        {
                            GameLogic.ReturnCardsToSource(this, _isHomeDrag, _dragStackIndex, _draggingCards);
                        }
                        else
                        {                            
                            var stack = CurrentGame.HomeStacks[data.StackIndex];
                            var card = _draggingCards[0];
                            if (stack.Cards.Count == 0 && card.Value == 1)
                            {
                                /* It's an ace, drop it here */
                                stack.Suit = card.Suit;
                                stack.Cards.Add(card);
                                CurrentGame.GameScore += 10;
                                if (OnScoreChanged != null)
                                {
                                    OnScoreChanged(CurrentGame.GameScore);
                                }
                            }                                
                            else if (stack.Cards.Count > 0 && card.Suit == stack.Suit && card.Value == stack.Cards[stack.Cards.Count - 1].Value + 1)
                            {
                                /* Valid drop */
                                stack.Cards.Add(card);
                                CurrentGame.GameScore += 10;
                                if (OnScoreChanged != null)
                                {
                                    OnScoreChanged(CurrentGame.GameScore);
                                }
                            }
                            else
                            {
                                /* Invalid */
                                GameLogic.ReturnCardsToSource(this, _isHomeDrag, _dragStackIndex, _draggingCards);
                            }
                        }
                        break;

                    case HitTestType.PlayStack:
                        /* Is this slot empty and the card being dragged is a King? */
                        if (data.Cards == null && _draggingCards[0].Value == 13)
                        {
                            CurrentGame.PlayingStacks[data.StackIndex].Cards.AddRange(_draggingCards);
                            /* Make sure no cards are hidden */
                            foreach (var card in CurrentGame.PlayingStacks[data.StackIndex].Cards)
                            {
                                card.IsHidden = false;
                            }
                        }
                        else if (data.Cards != null)
                        {
                            /* Validate dragging cards can be placed on top of this card */
                            if (GameLogic.IsValidMove(_draggingCards[0], data.Cards[data.CardIndex]))
                            {
                                data.Cards.AddRange(_draggingCards);
                                CurrentGame.GameScore += _isHomeDrag ? -15 : 5;
                                if (OnScoreChanged != null)
                                {
                                    OnScoreChanged(CurrentGame.GameScore);
                                }
                            }
                            else
                            {
                                /* Put it back where it came from */
                                GameLogic.ReturnCardsToSource(this, _isHomeDrag, _dragStackIndex, _draggingCards);
                            }
                        }
                        else
                        {
                            /* Put it back where it came from */
                            GameLogic.ReturnCardsToSource(this, _isHomeDrag, _dragStackIndex, _draggingCards);
                        }
                        break;

                    default:
                        /* Put it back where it came from */
                        GameLogic.ReturnCardsToSource(this, _isHomeDrag, _dragStackIndex, _draggingCards);
                        break;
                }
                _isDragging = false;
                _isHomeDrag = false;
                _draggingCards = new List<Card>();
                if (_dragBitmap != null)
                {
                    _dragBitmap.Dispose();
                    _dragBitmap = null;
                }
                Invalidate();
                AudioManager.Play(SoundType.Drop);
            }
            base.OnMouseUp(e);
        }
        #endregion

        #region Resize override
        protected override void OnResize(EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }
            /* Calculate what the size of the images should be based on clientsize */
            var img = ObjectData.CardBack;
            var ratioX = (double)ClientSize.Width / img.Width;
            var ratioY = (double)ClientSize.Height / img.Height;
            /* Use whichever multiplier is smaller */
            var ratio = ratioX < ratioY ? ratioX : ratioY;
            /* Now we can get the new height and width - only to the actual card size */
            var newWidth = Convert.ToInt32(img.Width * ratio) / 5;
            var newHeight = Convert.ToInt32(img.Height * ratio) / 5;
            _cardSize = new Size(newWidth > img.Width ? img.Width : newWidth, newHeight > img.Height ? img.Height : newHeight);
            /* This is used for centering drawing of images on X axis and for mouse hit test */
            _gameCenter = (ClientSize.Width/2) - (_cardSize.Width/2);
            Invalidate();
            base.OnResize(e);
        }
        #endregion

        #region Paint override
        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                /* Steve Jobs was here */
                return;
            }
            if (!_timerFireWorks.Enabled)
            {
                if (_dragBitmap == null)
                {
                    var rect = GraphicsRenderer.Draw(this, e.Graphics, _cardSize, _isDragging, _dragStackIndex, _gameCenter);
                    if (rect != Rectangle.Empty)
                    {
                        _deckRegion = rect;
                    }
                }
                /* Draw dragging cards */
                if (_isDragging)
                {
                    if (_dragBitmap == null)
                    {
                        /* Clone the screen - this just speeds up drawing dragging cards as we don't have to do the above loops in the drawing methods */
                        _dragBitmap = new Bitmap(ClientSize.Width, ClientSize.Height);
                        using (var g = Graphics.FromImage(_dragBitmap))
                        {
                            g.DrawImage(
                                GraphicsBitmapConverter.GraphicsToBitmap(e.Graphics,
                                    new Rectangle(0, 0, ClientSize.Width, ClientSize.Height)), 0, 0, ClientSize.Width,
                                ClientSize.Height);

                        }
                    }
                    e.Graphics.DrawImage(_dragBitmap, 0, 0, ClientSize.Width, ClientSize.Height);
                    GraphicsRenderer.DrawDrag(this, e.Graphics, _cardSize, _draggingCards, _dragLocation);
                }
                else
                {
                    _checkWin.Enabled = true;
                }
            }
            else
            {
                /* Draw fireworks winning sequence */
                e.Graphics.Clear(Color.Black);
                foreach (var fw in _fireWorks.Where(fw => fw != null))
                {
                    fw.Paint(e.Graphics);
                }
            }
            base.OnPaint(e);
        }
        #endregion
        #endregion

        #region Timers
        private void OnGameTimer(object sender, EventArgs e)
        {
            if (OnGameTimeChanged != null)
            {
                OnGameTimeChanged(CurrentGame.GameTime);
            }
            CurrentGame.GameTime++;
            if (CurrentGame.GameTime % 10 != 0)
            {
                return;
            }
            CurrentGame.GameScore -= 5;
            if (OnScoreChanged != null)
            {
                OnScoreChanged(CurrentGame.GameScore);
            }
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
                CurrentGame.GameScore += 120;
            }
            else if (CurrentGame.GameTime <= 300)
            {
                CurrentGame.GameScore += 60;
            }
            if (OnGameTimeChanged != null)
            {
                OnGameTimeChanged(CurrentGame.GameTime);
            }
            if (OnScoreChanged != null)
            {
                OnScoreChanged(CurrentGame.GameScore);
            }
            SettingsManager.UpdateStats(CurrentGame.GameTime, CurrentGame.GameScore);
            AudioManager.Play(SoundType.Win);
            if (CustomMessageBox.Show(this, "Congratulations! You win!\r\n\r\nWould you like to deal again?", "Congratulations!") == DialogResult.Yes)
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
        #endregion
    }
}
