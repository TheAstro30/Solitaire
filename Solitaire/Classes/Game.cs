/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Solitaire.Classes
{
    internal enum HitTestType
    {
        None = 0,
        Deck = 1,
        Dealt = 2,
        HomeStack = 3,
        PlayStack = 4
    }

    public class Game : Form
    {
        private Size _cardSize;        
        private int _gameCenter;
        private Rectangle _deckRegion;

        public GameData CurrentGame { get; set; }

        public Game()
        {
            /* Double buffering */
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            NewGame();
        }

        #region Overrides
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (HitTest(e.Location))
                {
                    case HitTestType.Deck:
                        CurrentGame.Deal();
                        Invalidate();
                        break;

                    case HitTestType.Dealt:
                        var card = CurrentGame.DealtCards[CurrentGame.DealtCards.Count - 1];
                        System.Diagnostics.Debug.Print("Dealt pile: " + card.Suit + " " + card.Value);
                        break;

                    default:
                        System.Diagnostics.Debug.Print("Auto complete");
                        break;
                }
                System.Diagnostics.Debug.Print(HitTest(e.Location).ToString());
            }
            base.OnMouseDown(e);
        }

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
            /* Now, the fun part - drawing all the data... - start with deck */
            DrawDeck(e.Graphics);
            /* Draw dealt hand */
            DrawDealt(e.Graphics);
            /* Draw "foundation" slots */
            DrawHomeStacks(e.Graphics);
            /* Draw playing stacks */
            DrawPlayStacks(e.Graphics);
            base.OnPaint(e);
        }
        #endregion
        #endregion

        public void NewGame()
        {
            /* Setup game data */
            if (CurrentGame == null)
            {
                CurrentGame = new GameData(true);
            }
            CurrentGame.StartNewGame();
            _deckRegion = new Rectangle();
        }

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
                stackOffset += _cardSize.Width + 40;
            }
        }

        private void DrawPlayStacks(Graphics e)
        {
            var yOffset = _cardSize.Height + 70;
            var cardOffset = _cardSize.Height / 15;            
            var stackOffset = _gameCenter - ((_cardSize.Width + 40) * 3);
            foreach (var stack in CurrentGame.PlayingStacks)
            {
                var offset = 0;
                foreach (var card in stack.Cards)
                {
                    var img = card.IsHidden ? CurrentGame.CardBack : card.CardImage;
                    e.DrawImage(img, stackOffset, yOffset + offset, _cardSize.Width, _cardSize.Height);
                    card.Region = new Rectangle(stackOffset, yOffset + offset, _cardSize.Width, _cardSize.Height);
                    offset += cardOffset;
                }
                stackOffset += _cardSize.Width + 40;
            }
        }
        #endregion

        #region Private methods
        #region Hit-test
        /* I may want to change this as well */
        private HitTestType HitTest(Point location)
        {
            /* We need to work out what the mouse is over */
            if (location.X >= _deckRegion.X && location.X <= _deckRegion.X + _deckRegion.Width &&
                location.Y >= _deckRegion.Y && location.Y <= _deckRegion.Y + _deckRegion.Height)
            {
                return HitTestType.Deck;
            }
            /* Dealt pile */
            if (CurrentGame.DealtCards.Count > 0 && CurrentGame.DealtCards[CurrentGame.DealtCards.Count - 1].IsHitTest(location))
            {
                return HitTestType.Dealt;
            }
            /* Playing stacks - I may want to change this... this seems .. weird to do */
            foreach (var stack in CurrentGame.PlayingStacks)
            {
                foreach (var card in stack.Cards)
                {
                    if (card.IsHitTest(location))
                    {
                        return HitTestType.PlayStack;
                    }
                }
            }
            return HitTestType.None;
        }
        #endregion

        #region IsValidMove
        private bool IsValidMove(Card source, Card destination)
        {
            /* Place holder */
            return false;
        }
        #endregion
        #endregion
    }
}
