/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;
using System.Drawing.Drawing2D;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.UI;

namespace Solitaire.Classes.Helpers.UI
{
    public class GraphicsRenderer
    {
        /* This class splits code from Game.cs up into a more managable chunk (it was originally on Game.cs itself) */
        private readonly Game _gameCtl;

        public GraphicsRenderer(Game gameCtl)
        {
            _gameCtl = gameCtl;
        }

        public Rectangle Draw(Graphics e)
        {
            /* Now, the fun part - drawing all the data... draw background tiled */
            using (var brush = new TextureBrush(_gameCtl.ObjectData.Background, WrapMode.Tile))
            {
                /* Yes, we can set the forms background image property... */
                e.FillRectangle(brush, 0, 0, _gameCtl.ClientSize.Width, _gameCtl.ClientSize.Height);
            }
            /* Draw deck */
            var rect = DrawStock(e);
            /* Draw dealt hand */
            DrawWaste(e);
            /* Draw "foundation" slots */
            DrawFoundations(e);
            /* Draw playing stacks */
            DrawTableaus(e);
            return rect;
        }

        public void DrawDrag(Graphics e)
        {
            if (_gameCtl.DraggingCards.Count == 0)
            {
                /* Nothing to do */
                return;
            }
            var visibleOffset = _gameCtl.CardSize.Height / 8;
            var offsetY = 0;
            var x = _gameCtl.CardSize.Width / 2;
            foreach (var card in _gameCtl.DraggingCards)
            {
                e.DrawImage(card.CardImage, _gameCtl.DragLocation.X - x, (_gameCtl.DragLocation.Y - 5) + offsetY, _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);
                offsetY += visibleOffset;
            }
        }

        private Rectangle DrawStock(Graphics e)
        {
            var stackSize = 4;
            if (_gameCtl.CurrentGame.StockCards.Count < 8)
            {
                stackSize = 3;
            }
            if (_gameCtl.CurrentGame.StockCards.Count <= 4)
            {
                stackSize = 2;
            }
            if (_gameCtl.CurrentGame.StockCards.Count <= 1)
            {
                stackSize = 0;
            }

            var back = SettingsManager.Settings.Options.DeckBack;
            var stackOffset = _gameCtl.GameCenter - ((_gameCtl.CardSize.Width + 40) * 3);
            var xOffset = 0;
            var yOffset = 0;
            
            if (_gameCtl.CurrentGame.StockCards.Count == 0)
            {
                /* Stock is empty, draw empty deck image and piss off */
                e.DrawImage(_gameCtl.ObjectData.EmptyStock, stackOffset + xOffset, 40 + yOffset, _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);
                return Rectangle.Empty;
            }
            /* Draw deck as if it's a pile of cards */
            for (var i = 0; i <= stackSize; i++)
            {
                e.DrawImage(_gameCtl.ObjectData.CardBacks[back], stackOffset + xOffset, 40 + yOffset, _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);
                xOffset += 2;
                yOffset += 2;
            }
            /* Return the region where the last/top card is drawn- used for mouse hit test */
            return new Rectangle(stackOffset + xOffset, 40 + yOffset, _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);
        }

        private void DrawWaste(Graphics e)
        {
            if (_gameCtl.CurrentGame.WasteCards.Count == 0)
            {
                /* Do nothing */
                return;
            }
            /* Where to start drawing */
            var stackOffset = _gameCtl.GameCenter - ((_gameCtl.CardSize.Width + 40) * 2);
            /* X/Y offset for appearance of a "stack" of cards */
            var xOffset = 0;
            var yOffset = 0;
            var index = 0;
            var draw3 = SettingsManager.Settings.Options.DrawThree;
            var draw3Offset = _gameCtl.CardSize.Width / 5;
            /* Point in the deck thats 3 cards less than the top card index */
            var draw3Start = _gameCtl.CurrentGame.WasteCards.Count - 3;
            if (draw3Start < 0)
            {
                draw3Start = 0;
            }
            foreach (var c in _gameCtl.CurrentGame.WasteCards)
            {
                if (draw3 && index == draw3Start && _gameCtl.CurrentGame.WasteCards.Count > 3 && _gameCtl.IsDragging && _gameCtl.DragStackIndex == -1)
                {
                    /* If we're in draw three mode, we only want to draw the last two cards before the picked up card */
                    index++;
                    continue;
                }
                /* Set card region where it is drawn on screen */
                var rect = new Rectangle(stackOffset + xOffset, 40 + yOffset, _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);
                e.DrawImage(c.CardImage, rect);
                c.Region = rect;     
                /* Increase our draw three mode index */
                index++;
                if (draw3 && index > draw3Start)
                {
                    /* In draw three mode, we want to see 3 cards drawn with an X offset greater than the one before */
                    xOffset += draw3Offset;
                }
                if (index != 1 && index != 5 && index != 9 && index != 13)
                {
                    continue;
                }
                /* Draw as a pile of cards by increasing offsets (or at least give the appearance it's a pile; looks a bit strange in draw three mode, but I actually like it) */
                xOffset += 2;
                yOffset += 2;
            }
        }

        private void DrawFoundations(Graphics e)
        {
            var center = _gameCtl.GameCenter;
            foreach (var stack in _gameCtl.CurrentGame.Foundation)
            {               
                var rect = new Rectangle(center, 40, _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);
                e.DrawImage(_gameCtl.ObjectData.EmptyFoundation, rect);
                /* Draw last card */
                if (stack.Cards.Count > 0)
                {
                    var card = stack.Cards[stack.Cards.Count - 1];
                    e.DrawImage(card.CardImage, rect);
                    card.Region = rect;
                }
                /* Set hit test region */
                stack.Region = rect;
                center += _gameCtl.CardSize.Width + 40;
            }
        }

        private void DrawTableaus(Graphics e)
        {
            var back = SettingsManager.Settings.Options.DeckBack;
            var yOffset = _gameCtl.CardSize.Height + 70;
            var invisibleOffset = _gameCtl.CardSize.Height / 15;
            var visibleOffset = _gameCtl.CardSize.Height / 8;
            var stackOffset = _gameCtl.GameCenter - ((_gameCtl.CardSize.Width + 40) * 3);
            foreach (var stack in _gameCtl.CurrentGame.Tableau)
            {
                var offset = 0;
                /* Draw empty tableau */
                e.DrawImage(_gameCtl.ObjectData.EmptyTableau, stackOffset, yOffset, _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);
                /* Draw each card in the tableau */
                foreach (var card in stack.Cards)
                {
                    var img = card.IsHidden ? _gameCtl.ObjectData.CardBacks[back] : card.CardImage;
                    var rect = new Rectangle(stackOffset, yOffset + offset, _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);
                    e.DrawImage(img, rect);
                    card.Region = rect;
                    offset += card.IsHidden ? invisibleOffset : visibleOffset;
                }
                /* Set hit test region */
                stack.Region = new Rectangle(stackOffset, yOffset, _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);
                stackOffset += _gameCtl.CardSize.Width + 40;
            }
        }
    }
}
