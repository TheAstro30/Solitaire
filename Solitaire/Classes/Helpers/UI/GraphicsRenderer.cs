/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using Solitaire.Classes.Data;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.Settings.SettingsData;
using Solitaire.Classes.UI;
using Solitaire.Properties;

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
            /* Now, the fun part - drawing all the data... */
            var bg = _gameCtl.ObjectData.Backgrounds[SettingsManager.Settings.Options.Background];
            if (bg != null)
            {
                /* Draw background */
                switch (bg.ImageLayout)
                {
                    /* I know we can just set the form's background property, however, stretching especially is slooooow */
                    case BackgroundImageDataLayout.Tile:
                        e.DrawImageTiled(bg.Image, _gameCtl.ClientSize);
                        break;

                    case BackgroundImageDataLayout.Stretch:
                        e.DrawImageStretched(bg.Image, _gameCtl.ClientSize);
                        break;

                    case BackgroundImageDataLayout.Color:
                        /* Not implemented yet */
                        break;
                }
            }
            else
            {
                /* This should never happen; but it's here incase we have a graphics error and don't end up with a black screen */
                e.DrawImageTiled(Resources.bg_default, _gameCtl.ClientSize);
            }
            /* Draw logo image in bottom right (with 50% opacity) */
            var img = Resources.logo;
            var rect = new Rectangle(_gameCtl.ClientSize.Width - img.Width - 10, _gameCtl.ClientSize.Height - img.Height - 20, img.Width, img.Height);
            e.DrawImageOpaque(img, rect, 0.5F);

            /* Draw deck */
            rect = DrawStock(e);
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
            if (SettingsManager.Settings.Options.ShowHighlight)
            {
                /* Draw a focus ring around destination card that the first dragging card is above */
                var rect = new Rectangle(_gameCtl.DragLocation.X - (_gameCtl.CardSize.Width/2),
                    _gameCtl.DragLocation.Y - 5,
                    _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);

                switch (HitTest.CompareDrop(_gameCtl, rect, out var d))
                {
                    case HitTestType.Foundation:
                        if (_gameCtl.DraggingCards.Count == 1)
                        {
                            rect = _gameCtl.CurrentGame.Foundation[d.StackIndex].Region;
                        }
                        break;

                    case HitTestType.Tableau:
                        if (d.CardIndex == -1 && _gameCtl.DraggingCards[0].Value == 13)
                        {
                            rect = _gameCtl.CurrentGame.Tableau[d.StackIndex].Region;
                        }
                        else if (d.CardIndex >= 0)
                        {
                            rect = _gameCtl.CurrentGame.Tableau[d.StackIndex].Cards[d.CardIndex].Region;
                        }
                        break;

                    default:
                        rect = Rectangle.Empty;
                        break;
                }
                if (rect != Rectangle.Empty)
                {
                    using (var p = new Pen(Color.Blue, 3))
                    {
                        e.DrawRoundedRectangle(p, rect, 5);
                    }
                }
            }
            /* Draw all dragging cards */
            var visibleOffset = _gameCtl.CardSize.Height/8;
            var offsetY = 0;
            var x = _gameCtl.CardSize.Width/2;
            foreach (var img in _gameCtl.DraggingCards.Select(card => _gameCtl.Cards.Images[new KeyValuePair<Suit, int>(card.Suit, card.Value)].Image))
            {
                e.DrawImage(img, _gameCtl.DragLocation.X - x, (_gameCtl.DragLocation.Y - 5) + offsetY,
                    _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);
                offsetY += visibleOffset;
            }
        }

        public void DrawFocusRing(Rectangle region, int cardCount)
        {
            /* Finally figured out how to draw the stupid thing when there's more than one card after 2 hours! I decided to go with drawing a
             * rounded rectangle directly, rather than fucking around with a graphic */
            var offset = _gameCtl.CardSize.Height/8;
            var rect = new Rectangle(region.X, region.Y, _gameCtl.CardSize.Width, _gameCtl.CardSize.Height + (offset*cardCount));
            using (var g = _gameCtl.CreateGraphics())
            {
                using (var p = new Pen(Color.Gold, 4))
                {
                    g.DrawRoundedRectangle(p, rect, 5);
                }
            }
        }

        /* Internal drawing methods */
        internal Rectangle DrawStock(Graphics e)
        {
            if (!_gameCtl.IsGameRunning)
            {
                return Rectangle.Empty;
            }
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
            
            var rect = Rectangle.Empty;

            var back = SettingsManager.Settings.Options.DeckBack;
            var stackOffset = _gameCtl.GameCenter - ((_gameCtl.CardSize.Width + 40) * 3);
            var xOffset = 0;
            var yOffset = 0;
            
            if (_gameCtl.CurrentGame.StockCards.Count == 0)
            {
                rect = new Rectangle(stackOffset, 40, _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);
                /* Check difficulty level */
                switch (SettingsManager.Settings.Options.Difficulty)
                {
                    case DifficultyLevel.Medium:
                        if (_gameCtl.CurrentGame.DeckRedeals == 3)
                        {
                            e.DrawImage(_gameCtl.ObjectData.NoRedeal, rect);
                            return rect;
                        }
                        break;

                    case DifficultyLevel.Hard:
                        e.DrawImage(_gameCtl.ObjectData.NoRedeal, rect);
                        return rect;
                }
                /* Stock is empty, draw redeal image and piss off */
                e.DrawImage(_gameCtl.ObjectData.EmptyStock, rect);
                return rect;
            }
            /* Draw deck as if it's a pile of cards */
            for (var i = 0; i <= stackSize; i++)
            {
                rect = new Rectangle(stackOffset + xOffset, 40 + yOffset, _gameCtl.CardSize.Width, _gameCtl.CardSize.Height);
                e.DrawImage(_gameCtl.ObjectData.CardBacks[back], rect);
                xOffset += 2;
                yOffset += 2;
            }
            /* Return the region where the last/top card is drawn - used for mouse hit test */
            return rect;
        }

        internal void DrawWaste(Graphics e)
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
            var draw3 = _gameCtl.CurrentGame.IsDrawThree;
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
                var img = _gameCtl.Cards.Images[new KeyValuePair<Suit, int>(c.Suit, c.Value)].Image;
                e.DrawImage(img, rect);
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

        internal void DrawFoundations(Graphics e)
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
                    var img = _gameCtl.Cards.Images[new KeyValuePair<Suit, int>(card.Suit, card.Value)].Image;
                    e.DrawImage(img, rect);
                    card.Region = rect;
                }
                /* Set hit test region */
                stack.Region = rect;
                center += _gameCtl.CardSize.Width + 40;
            }
        }

        internal void DrawTableaus(Graphics e)
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
                    var img = card.IsHidden
                        ? _gameCtl.ObjectData.CardBacks[back]
                        : _gameCtl.Cards.Images[new KeyValuePair<Suit, int>(card.Suit, card.Value)].Image;
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

    internal static class GraphicsExtensions
    {
        /* Drawing extensions */
        internal static void DrawImageTiled(this Graphics graphics, Image image, Size size)
        {
            using (var brush = new TextureBrush(image, WrapMode.Tile))
            {
                graphics.FillRectangle(brush, 0, 0, size.Width, size.Height);
            }
        }

        internal static void DrawImageStretched(this Graphics graphics, Image image, Size destSize)
        {
            graphics.DrawImage(image, 0, 0, destSize.Width, destSize.Height);
        }

        internal static void DrawImageOpaque(this Graphics graphics, Image image, Rectangle destRect, float opacity)
        {
            var colormatrix = new ColorMatrix { Matrix33 = opacity };
            using (var imgAttribute = new ImageAttributes())
            {
                imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imgAttribute);
            }
        }

        internal static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int cornerRadius)
        {
            using (var path = RoundedRect(bounds, cornerRadius))
            {
                graphics.DrawPath(pen, path);
            }
        }

        /* Private methods */
        internal static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var diameter = radius * 2;
            var size = new Size(diameter, diameter);
            var arc = new Rectangle(bounds.Location, size);
            var path = new GraphicsPath();
            /* Just return a rectangle if radius is 0 */
            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }
            /* Top left arc */
            path.AddArc(arc, 180, 90);
            /* Top right arc */
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            /* Bottom right arc */
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            /* Bottom left arc */
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            /* Close path and return result */
            path.CloseFigure();
            return path;
        }
    }
}
