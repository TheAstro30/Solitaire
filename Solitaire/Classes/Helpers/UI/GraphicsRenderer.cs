/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Solitaire.Classes.Data;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.UI;

namespace Solitaire.Classes.Helpers.UI
{
    public static class GraphicsRenderer
    {
        public static Rectangle Draw(Game ctl, Graphics e, Size cardSize, bool dragging, int dragStackIndex, int screenCenter)
        {
            /* Now, the fun part - drawing all the data... draw background tiled */
            using (var brush = new TextureBrush(ctl.ObjectData.Background, WrapMode.Tile))
            {
                /* Yes, we can set the forms background image property... */
                e.FillRectangle(brush, 0, 0, ctl.ClientSize.Width, ctl.ClientSize.Height);
            }
            /* Draw deck */
            var rect = DrawDeck(ctl, e, cardSize, screenCenter);
            /* Draw dealt hand */
            DrawDealt(ctl, e, cardSize, dragging, dragStackIndex, screenCenter);
            /* Draw "foundation" slots */
            DrawHomeStacks(ctl, e, cardSize, screenCenter);
            /* Draw playing stacks */
            DrawPlayStacks(ctl, e, cardSize, screenCenter);
            return rect;
        }

        public static void DrawDrag(Game ctl, Graphics e, Size cardSize, List<Card> dragCards, Point location)
        {
            if (dragCards.Count == 0)
            {
                return;
            }
            var visibleOffset = cardSize.Height / 8;
            var offsetY = 0;
            var x = cardSize.Width / 2;
            foreach (var card in dragCards)
            {
                e.DrawImage(card.CardImage, location.X - x, (location.Y - 5) + offsetY, cardSize.Width, cardSize.Height);
                offsetY += visibleOffset;
            }
        }

        private static Rectangle DrawDeck(Game ctl, Graphics e, Size cardSize, int screenCenter)
        {
            var stackSize = 4;
            if (ctl.CurrentGame.GameDeck.Count < 8)
            {
                stackSize = 3;
            }
            if (ctl.CurrentGame.GameDeck.Count <= 4)
            {
                stackSize = 2;
            }
            if (ctl.CurrentGame.GameDeck.Count <= 1)
            {
                stackSize = 0;
            }

            var stackOffset = screenCenter - ((cardSize.Width + 40) * 3);
            var xOffset = 0;
            var yOffset = 0;

            if (ctl.CurrentGame.GameDeck.Count == 0)
            {
                /* Deck is empty, draw empty deck image and piss off */
                e.DrawImage(ctl.ObjectData.EmptyDeck, stackOffset + xOffset, 40 + yOffset, cardSize.Width, cardSize.Height);
                return Rectangle.Empty;
            }
            /* Draw deck as if it's a pile of cards */
            for (var i = 0; i <= stackSize; i++)
            {
                e.DrawImage(ctl.ObjectData.CardBack, stackOffset + xOffset, 40 + yOffset, cardSize.Width, cardSize.Height);
                xOffset += 2;
                yOffset += 2;
            }

            return new Rectangle(stackOffset + xOffset, 40 + yOffset, cardSize.Width, cardSize.Height);
        }

        private static void DrawDealt(Game ctl, Graphics e, Size cardSize, bool dragging, int dragStackIndex, int screenCenter)
        {
            if (ctl.CurrentGame.DealtCards.Count == 0)
            {
                /* Do nothing */
                return;
            }
            var stackOffset = screenCenter - ((cardSize.Width + 40) * 2);
            var xOffset = 0;
            var yOffset = 0;
            var index = 0;
            var draw3 = SettingsManager.Settings.Options.DrawThree;
            var draw3Offset = cardSize.Width/5;
            var draw3Start = ctl.CurrentGame.DealtCards.Count - 3;
            if (draw3Start < 0)
            {
                draw3Start = 0;
            }
            foreach (var c in ctl.CurrentGame.DealtCards)
            {
                if (draw3 && index == draw3Start && ctl.CurrentGame.DealtCards.Count > 3 && dragging && dragStackIndex == -1)
                {
                    index++;
                    continue;
                }
                var rect = new Rectangle(stackOffset + xOffset, 40 + yOffset, cardSize.Width, cardSize.Height);
                e.DrawImage(c.CardImage, rect);
                c.Region = rect;
                /* Draw as a pile of cards */
                index++;
                if (draw3 && index > draw3Start)
                {
                    xOffset += draw3Offset;
                }
                if (index != 1 && index != 5 && index != 9 && index != 13)
                {
                    continue;
                }
                xOffset += 2;
                yOffset += 2;
            }
        }

        private static void DrawHomeStacks(Game ctl, Graphics e, Size cardSize, int screenCenter)
        {
            foreach (var stack in ctl.CurrentGame.HomeStacks)
            {
                var rect = new Rectangle(screenCenter, 40, cardSize.Width, cardSize.Height);
                e.DrawImage(ctl.ObjectData.HomeStack, rect);
                /* Draw last card */
                if (stack.Cards.Count > 0)
                {
                    var card = stack.Cards[stack.Cards.Count - 1];
                    e.DrawImage(card.CardImage, rect);
                    card.Region = rect;
                }
                stack.Region = rect;
                screenCenter += cardSize.Width + 40;
            }
        }

        private static void DrawPlayStacks(Game ctl, Graphics e, Size cardSize, int screenCenter)
        {
            var yOffset = cardSize.Height + 70;
            var invisibleOffset = cardSize.Height / 15;
            var visibleOffset = cardSize.Height / 8;
            var stackOffset = screenCenter - ((cardSize.Width + 40) * 3);
            foreach (var stack in ctl.CurrentGame.PlayingStacks)
            {
                var offset = 0;
                foreach (var card in stack.Cards)
                {
                    var img = card.IsHidden ? ctl.ObjectData.CardBack : card.CardImage;
                    var rect = new Rectangle(stackOffset, yOffset + offset, cardSize.Width, cardSize.Height);
                    e.DrawImage(img, rect);
                    card.Region = rect;
                    offset += card.IsHidden ? invisibleOffset : visibleOffset;
                }
                stack.Region = new Rectangle(stackOffset, yOffset, cardSize.Width, cardSize.Height);
                stackOffset += cardSize.Width + 40;
            }
        }
    }
}
