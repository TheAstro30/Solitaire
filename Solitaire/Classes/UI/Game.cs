/* Solitaire
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

namespace Solitaire.Classes.UI
{
    internal enum HitTestType
    {
        None = 0,
        Deck = 1,
        Dealt = 2,
        HomeStack = 3,
        PlayStack = 4
    }

    internal struct HitTestData
    {
        public List<Card> Cards { get; set; } 

        public int StackIndex { get; set; }

        public int CardIndex { get; set; }
    }

    public class Game : Form
    {
        private Size _cardSize;        
        private int _gameCenter;
        private Rectangle _deckRegion;

        /* Timer */
        private Timer _timerGame;
        private int _gameTime;

        /* Dragging variables */
        private bool _isDragging;
        private bool _isHomeDrag;
        private List<Card> _draggingCards = new List<Card>();
        private int _dragStackIndex;
        private Point _dragLocation;        
        private Bitmap _dragBitmap; /* TODO implement this */

        private readonly Timer _checkWin;
        private readonly Timer _timerFireWorks;

        private const int MaxFireWorks = 10;
        private readonly FireWork[] _fireWork = new FireWork[MaxFireWorks];
        private static readonly Random FireWorkPosition = new Random();

        public GameData CurrentGame { get; set; }

        public event Action<int> OnGameTimerChanged;

        public Game()
        {
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

            NewGame();
        }

        #region Overrides
        #region Mouse

        protected override void OnMouseDown(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    _dragLocation = e.Location;
                    HitTestData data;
                    Card card;
                    switch (HitTest(e.Location, out data))
                    {
                        case HitTestType.Deck:
                            CurrentGame.Deal();
                            Invalidate();
                            break;

                        case HitTestType.Dealt:
                            /* Pick up first card on pile, begin drag */
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
                                    if (data.CardIndex == data.Cards.Count - 1)
                                    {
                                        /* Turn card over */
                                        data.Cards[data.CardIndex].IsHidden = false;
                                        Invalidate();
                                        AudioManager.Play(SoundType.Deal);
                                        return;
                                    }
                                    /* Invalid drag */
                                    return;
                                }
                                if (data.CardIndex < data.Cards.Count - 1)
                                {
                                    /* Valid cards on top of this card */
                                    for (var i = data.CardIndex; i <= data.Cards.Count - 1; i++)
                                    {
                                        card = data.Cards[i];
                                        if (i < data.Cards.Count - 1)
                                        {
                                            if (!IsValidMove(data.Cards[i + 1], card))
                                            {
                                                return;
                                            }
                                        }
                                        /* Add card to list */
                                        _draggingCards.Add(card);
                                    }
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
            if (_isDragging)
            {
                /* Hit test, validate move, drop */
                if (_draggingCards.Count == 0)
                {
                    return;
                }
                HitTestData data;
                switch (HitTest(e.Location, out data))
                {
                    case HitTestType.HomeStack:
                        /* Dropping a card on a home stack? Cards must go from ace to king and be the same suit */
                        if (_draggingCards.Count > 1)
                        {
                            ReturnCardsToSource(_isHomeDrag);
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
                            }                                
                            else if (stack.Cards.Count > 0 && card.Suit == stack.Suit && card.Value == stack.Cards[stack.Cards.Count - 1].Value + 1)
                            {
                                /* Valid drop */
                                stack.Cards.Add(card);
                            }
                            else
                            {
                                /* Invalid */
                                ReturnCardsToSource(_isHomeDrag);
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
                            if (IsValidMove(_draggingCards[0], data.Cards[data.CardIndex]))
                            {
                                data.Cards.AddRange(_draggingCards);
                            }
                            else
                            {
                                /* Put it back where it came from */
                                ReturnCardsToSource(_isHomeDrag);
                            }
                        }
                        else
                        {
                            /* Put it back where it came from */
                            ReturnCardsToSource(_isHomeDrag);
                        }
                        break;

                    default:
                        /* Put it back where it came from */
                        ReturnCardsToSource(_isHomeDrag);
                        break;
                }
                _isDragging = false;
                _isHomeDrag = false;
                _draggingCards = new List<Card>();
                Invalidate();
                AudioManager.Play(SoundType.Drop);
            }
            base.OnMouseUp(e);
        }
        #endregion

        #region Resize override
        protected override void OnResize(EventArgs e)
        {
            /* Calculate what the size of the images should be based on clientsize */
            var img = CurrentGame.CardBack;
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
            if (!_timerFireWorks.Enabled)
            {
                /* Now, the fun part - drawing all the data... - start with deck */
                DrawDeck(e.Graphics);
                /* Draw dealt hand */
                DrawDealt(e.Graphics);
                /* Draw "foundation" slots */
                DrawHomeStacks(e.Graphics);
                /* Draw playing stacks */
                DrawPlayStacks(e.Graphics);
                /* Draw dragging cards */
                if (_isDragging)
                {
                    /* I really should refactor this paint method to take a bitmap of the screen (with cards removed from where they're
                 * being dragged), and just draw out the bitmap and the drag. This would probably be faster and less "laggy"
                 * as we're not doing the above 4 methods all the time as the mouse moves */
                    DrawDrag(e.Graphics);
                }
                else
                {
                    _checkWin.Enabled = true;
                }
            }
            else
            {
                e.Graphics.Clear(Color.Black);
                foreach (var fw in _fireWork)
                {
                    if (fw != null)
                    {
                        fw.Paint(e.Graphics);
                    }
                }
            }
            base.OnPaint(e);
        }
        #endregion
        #endregion

        #region Public methods
        public void NewGame(bool ask = true)
        {
            /* Setup game data */
            if (CurrentGame == null)
            {
                CurrentGame = new GameData(true);
            }
            else if (ask)
            {
                /* Ask user if they want to start a new game */
                if (MessageBox.Show(this, @"Are you sure you want to quit the current game?", @"Quit Current Game",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                SettingsManager.Settings.Statistics.GamesLost++;
            }
            SettingsManager.Settings.Statistics.TotalGamesPlayed++;
            _timerFireWorks.Enabled = false;
            _gameTime = 0;
            _timerGame.Enabled = true;
            CurrentGame.StartNewGame();
            _deckRegion = new Rectangle();
            AudioManager.Play(SoundType.Shuffle);
            Invalidate();
        }

        public void AutoComplete()
        {
            /* Iterate through any drawn cards and play stacks moving them to each ace home stack */
            var complete = true;
            while (complete)
            {
                /* Deck card */
                var movedCards = 0;
                Card card;
                if (CurrentGame.DealtCards.Count > 0)
                {
                    card = CurrentGame.DealtCards[CurrentGame.DealtCards.Count - 1];
                    /* Is it an ace ? - Find free home slot; if not see if it can be completed */
                    if (card.Value == 1 && AddAceToFreeSlot(card) || IsCompleted(card))
                    {
                        CurrentGame.DealtCards.Remove(card);
                        movedCards++;
                    }
                }
                /* Each stack */
                foreach (var stack in CurrentGame.PlayingStacks.Where(stack => stack.Cards.Count > 0))
                {
                    card = stack.Cards[stack.Cards.Count - 1];
                    /* Is it an ace ? - Find free home slot; if not see if it can be completed */
                    if ((card.IsHidden || !IsCompleted(card)) && (card.Value != 1 || !AddAceToFreeSlot(card)))
                    {
                        continue;
                    }
                    stack.Cards.Remove(card);
                    movedCards++;
                }
                complete = movedCards != 0;
            }
            Invalidate();
        }
        #endregion

        #region Drawing methods
        /* Private drawing methods */
        private void DrawDeck(Graphics e)
        {
            var stackSize = 4;
            if (CurrentGame.GameDeck.Count < 8)
            {
                stackSize = 3;
            }
            if (CurrentGame.GameDeck.Count <= 4)
            {
                stackSize = 2;
            }
            if (CurrentGame.GameDeck.Count <= 1)
            {
                stackSize = 0;
            }

            var stackOffset = _gameCenter - ((_cardSize.Width + 40) * 3);
            var xOffset = 0;
            var yOffset = 0;

            _deckRegion = new Rectangle(stackOffset, 40, _cardSize.Width, _cardSize.Height);

            if (CurrentGame.GameDeck.Count == 0)
            {
                /* Deck is empty, draw empty deck image and piss off */
                e.DrawImage(CurrentGame.EmptyDeck, stackOffset + xOffset, 40 + yOffset, _cardSize.Width, _cardSize.Height);
                return;
            }
            /* Draw deck as if it's a pile of cards */
            for (var i = 0; i <= stackSize; i++)
            {
                e.DrawImage(CurrentGame.CardBack, stackOffset + xOffset, 40 + yOffset, _cardSize.Width, _cardSize.Height);
                xOffset += 2;
                yOffset += 2;
            }
        }

        private void DrawDealt(Graphics e)
        {
            /* For now, I'm just going to draw the last drawn */
            if (CurrentGame.DealtCards.Count == 0)
            {
                /* Do nothing */
                return;
            }
            var card = CurrentGame.DealtCards[CurrentGame.DealtCards.Count - 1];
            var stackOffset = _gameCenter - ((_cardSize.Width + 40) * 2);
            e.DrawImage(card.CardImage, stackOffset , 40 , _cardSize.Width, _cardSize.Height);
            card.Region = new Rectangle(stackOffset, 40, _cardSize.Width, _cardSize.Height);
        }

        private void DrawHomeStacks(Graphics e)
        {
            var stackOffset = _gameCenter;
            foreach (var stack in CurrentGame.HomeStacks)
            {
                e.DrawImage(stack.StackImage, stackOffset, 40, _cardSize.Width, _cardSize.Height);                
                /* Draw last card */
                if (stack.Cards.Count > 0)
                {
                    var card = stack.Cards[stack.Cards.Count - 1];
                    e.DrawImage(card.CardImage, stackOffset, 40, _cardSize.Width, _cardSize.Height);
                    card.Region = new Rectangle(stackOffset, 40, _cardSize.Width, _cardSize.Height);
                }
                stack.Region = new Rectangle(stackOffset, 40, _cardSize.Width, _cardSize.Height);
                stackOffset += _cardSize.Width + 40;
            }
        }

        private void DrawPlayStacks(Graphics e)
        {
            var yOffset = _cardSize.Height + 70;
            var invisibleOffset = _cardSize.Height / 15;
            var visibleOffset = _cardSize.Height/8;
            var stackOffset = _gameCenter - ((_cardSize.Width + 40) * 3);
            foreach (var stack in CurrentGame.PlayingStacks)
            {
                var offset = 0;
                foreach (var card in stack.Cards)
                {
                    var img = card.IsHidden ? CurrentGame.CardBack : card.CardImage;
                    e.DrawImage(img, stackOffset, yOffset + offset, _cardSize.Width, _cardSize.Height);
                    card.Region = new Rectangle(stackOffset, yOffset + offset, _cardSize.Width, _cardSize.Height);
                    offset += card.IsHidden ? invisibleOffset : visibleOffset;
                }
                stack.Region = new Rectangle(stackOffset, yOffset, _cardSize.Width, _cardSize.Height);
                stackOffset += _cardSize.Width + 40;
            }
        }

        private void DrawDrag(Graphics e)
        {
            if (_draggingCards.Count == 0)
            {
                return;
            }
            var visibleOffset = _cardSize.Height / 8;
            var offsetY = 0;
            var x = _cardSize.Width/2;
            foreach (var card in _draggingCards)
            {
                e.DrawImage(card.CardImage, _dragLocation.X - x, (_dragLocation.Y - 5) + offsetY, _cardSize.Width, _cardSize.Height);
                offsetY += visibleOffset;
            }
        }
        #endregion

        #region Timers
        private void OnGameTimer(object sender, EventArgs e)
        {
            if (OnGameTimerChanged != null)
            {
                OnGameTimerChanged(_gameTime);
            }
            _gameTime++;
        }

        private void OnCheckWin(object sender, EventArgs e)
        {
            _checkWin.Enabled = false;            
            if (!CheckWin())
            {
                return;
            }
            SettingsManager.Settings.Statistics.GamesWon++;
            _timerGame.Enabled = false;
            _timerFireWorks.Enabled = true;
            /* Game is won. It's kind of easier to do this check here from the paint method, as it's called most of the time */
            AudioManager.Play(SoundType.Win);
            if (MessageBox.Show(this, @"You win! Would you like to deal again?", @"Congratulations",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                NewGame(false);
            }
        }

        private void OnFireWorks(object sender, EventArgs e)
        {
            for (var i = 0; i < MaxFireWorks; ++i)
            {
                if (_fireWork[i] != null && !_fireWork[i].Update())
                {
                    _fireWork[i] = null;
                }
            }

            if (FireWorkPosition.Next(10) == 0)
            {
                for (var i = 0; i < MaxFireWorks; ++i)
                {
                    if (_fireWork[i] != null)
                    {
                        continue;
                    }
                    _fireWork[i] = new FireWork(ClientRectangle.Width, ClientRectangle.Height);
                    break;
                }
            }
            Invalidate();
        }
        #endregion

        #region Private methods
        #region Hit-test
        private HitTestType HitTest(Point location, out HitTestData data)
        {
            data = new HitTestData();
            /* We need to work out what the mouse is over */
            if (IsRegion(location, _deckRegion))
            {
                return HitTestType.Deck;
            }
            /* Dealt pile */
            if (CurrentGame.DealtCards.Count > 0 && CurrentGame.DealtCards[CurrentGame.DealtCards.Count - 1].IsHitTest(location))
            {
                return HitTestType.Dealt;
            }            
            var stackIndex = 0;
            /* Home stacks */
            foreach (var s in CurrentGame.HomeStacks)
            {
                data.StackIndex = stackIndex;
                if (s.Cards.Count > 0 && IsRegion(location, s.Cards[s.Cards.Count - 1].Region))
                {
                    /* Get top card */
                    data.CardIndex = s.Cards.Count - 1;
                    return HitTestType.HomeStack;
                }
                if (IsRegion(location, s.Region))
                {
                    return HitTestType.HomeStack;
                }
                stackIndex++;
            }
            /* Playing stacks */
            stackIndex = 0;
            foreach (var stack in CurrentGame.PlayingStacks)
            {
                for (var i = stack.Cards.Count - 1; i >= 0; i--)
                {
                    if (!stack.Cards[i].IsHitTest(location))
                    {
                        continue;
                    }
                    data.Cards = stack.Cards; /* Which list? */
                    data.StackIndex = stackIndex; /* Index of the stack (1 - 7) */
                    data.CardIndex = i; /* Index of card in list that was hit */
                    return HitTestType.PlayStack;
                }
                if (IsRegion(location, stack.Region))
                {
                    data.StackIndex = stackIndex;
                    return HitTestType.PlayStack;
                }                
                stackIndex++;
            }
            return HitTestType.None;
        }
        #endregion

        #region IsValidMove
        private static bool IsValidMove(Card source, Card destination)
        {
            /* Source is card of lower value -> card of higher value - suits have to be opposite colors and values 1
             * less on the source than the destination: Valid opposite suit and number order of source is lower than destination */
            return IsOppositeSuit(source.Suit, destination.Suit) && source.Value == destination.Value - 1;
        }

        private static bool IsOppositeSuit(Suit srcSuit, Suit destSuit)
        {
            /* Check src and dest suits are OPPOSITE (clubs : hearts | diamonds : spades, etc.) */
            switch (srcSuit)
            {
                case Suit.Clubs:
                case Suit.Spades:
                    return destSuit == Suit.Diamonds || destSuit == Suit.Hearts;

                case Suit.Diamonds:
                case Suit.Hearts:
                    return destSuit == Suit.Clubs || destSuit == Suit.Spades;
            }
            return false;
        }
        #endregion

        #region Misc
        private static bool IsRegion(Point src, Rectangle dest)
        {
            return src.X >= dest.X && src.X <= dest.X + dest.Width && src.Y >= dest.Y && src.Y <= dest.Y + dest.Height;
        }

        private void ReturnCardsToSource(bool homeStack)
        {
            if (_dragStackIndex == -1)
            {
                /* Return to disposed pile */
                CurrentGame.DealtCards.Add(_draggingCards[0]);
            }
            else
            {
                if (homeStack)
                {
                    var stack = CurrentGame.HomeStacks[_dragStackIndex];
                    var card = _draggingCards[0];
                    stack.Cards.Add(card);
                    stack.Suit = card.Suit;
                    return;
                }
                CurrentGame.PlayingStacks[_dragStackIndex].Cards.AddRange(_draggingCards);
            }     
        }

        /* Auto complete private methods */
        private bool IsCompleted(Card card)
        {
            foreach (var stack in from stack in CurrentGame.HomeStacks where stack.Cards.Count > 0 let cd = stack.Cards[stack.Cards.Count - 1] where cd.Suit == card.Suit && cd.Value == card.Value - 1 select stack)
            {
                stack.Cards.Add(card);
                return true;
            }
            return false;
        }

        private bool AddAceToFreeSlot(Card card)
        {
            foreach (var stack in CurrentGame.HomeStacks.Where(stack => stack.Cards.Count == 0).Where(stack => !card.IsHidden))
            {
                stack.Cards.Add(card);
                return true;
            }
            return false;
        }

        /* Check if game is won */
        private bool CheckWin()
        {
            return CurrentGame.HomeStacks.Sum(stack => stack.Cards.Count) == 52;
        }
        #endregion
        #endregion
    }
}
