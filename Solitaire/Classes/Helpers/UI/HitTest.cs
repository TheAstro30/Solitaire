/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Collections.Generic;
using System.Drawing;
using Microsoft.SqlServer.Server;
using Solitaire.Classes.Data;
using Solitaire.Classes.Helpers.Logic;
using Solitaire.Classes.UI;

namespace Solitaire.Classes.Helpers.UI
{
    public enum HitTestType
    {
        None = 0,
        Stock = 1, /* Deck */
        Waste = 2, /* Dealt pile */
        Foundation = 3, /* Ace to king slot */
        Tableau = 4 /* One of the 7 playing stacks */
    }

    public struct HitTestData
    {
        public List<Card> Cards { get; set; }

        public int StackIndex { get; set; }

        public int CardIndex { get; set; }
    }

    public static class HitTest
    {
        /* This class splits code from Game.cs up into a more managable chunk (it was originally on Game.cs itself) */
        internal static HitTestType CompareClick(Game ctl, Point location, Rectangle stockRegion, out HitTestData data)
        {
            /* This is mainly responsible for MouseDown detection */
            data = new HitTestData();            
            /* We need to work out what the mouse is over */
            if (IsRegion(location, stockRegion))
            {
                return HitTestType.Stock;
            }
            /* Waste pile */            
            if (ctl.CurrentGame.WasteCards.Count > 0 && ctl.CurrentGame.WasteCards[ctl.CurrentGame.WasteCards.Count - 1].IsHitTest(location))
            {
                return HitTestType.Waste;
            }            
            var stackIndex = 0;
            /* Home stacks */
            foreach (var s in ctl.CurrentGame.Foundation)
            {
                data.StackIndex = stackIndex;
                if (s.Cards.Count > 0 && IsRegion(location, s.Cards[s.Cards.Count - 1].Region))
                {
                    /* Get top card */
                    data.CardIndex = s.Cards.Count - 1;
                    return HitTestType.Foundation;
                }
                if (IsRegion(location, s.Region))
                {
                    return HitTestType.Foundation;
                }
                stackIndex++;
            }
            /* Playing stacks */            
            stackIndex = 0;
            foreach (var stack in ctl.CurrentGame.Tableau)
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
                    return HitTestType.Tableau;
                }
                if (IsRegion(location, stack.Region))
                {
                    data.StackIndex = stackIndex;
                    return HitTestType.Tableau;
                }                
                stackIndex++;
            }
            return HitTestType.None;
        }

        internal static HitTestType CompareDrop(Game ctl, Rectangle src, out HitTestData data)
        {
            /* Detect if the dragged card(s) are within a droppable region and is a valid move */
            var srcCard = ctl.DraggingCards[0];           
            data = new HitTestData();
            var stackIndex = 0;
            /* Home stacks */
            foreach (var s in ctl.CurrentGame.Foundation)
            {
                /* If card count of stack is 0, keep that in mind */
                data.StackIndex = stackIndex;
                if (s.Cards.Count > 0 && src.IntersectsWith(s.Cards[s.Cards.Count - 1].Region))
                {                    
                    if (srcCard.Suit == s.Suit && srcCard.Value == s.Cards[s.Cards.Count - 1].Value + 1)
                    {
                        /* Get top card */
                        data.CardIndex = s.Cards.Count - 1;
                        return HitTestType.Foundation;
                    }
                }
                if (src.IntersectsWith(s.Region))
                {
                    /* Check it's an ace */
                    if (ctl.DraggingCards.Count > 1)
                    {
                        /* Cannot drop more than one card at a time on this slot */
                        return HitTestType.None;
                    }
                    if (s.Cards.Count == 0 && srcCard.Value == 1)
                    {
                        /* It's an ace and the slot is empty */
                        return HitTestType.Foundation;
                    }
                }
                stackIndex++;
            }
            /* Playing stacks */
            stackIndex = 0;
            foreach (var stack in ctl.CurrentGame.Tableau)
            {
                if (stack.Cards.Count == 0 && src.IntersectsWith(stack.Region) && srcCard.Value == 13)
                {
                    /* Empty slot and value is a king */
                    data.CardIndex = -1;
                    data.StackIndex = stackIndex;
                    return HitTestType.Tableau;
                }
                /* Validate dragging card(s) can be placed on top of this card */
                var index = stack.Cards.Count - 1;
                if (index < 0)
                {
                    stackIndex++;
                    continue;
                }
                var card = stack.Cards[index];
                if (src.IntersectsWith(card.Region) && GameLogic.IsValidMove(srcCard, card))
                {
                    data.CardIndex = index;
                    data.StackIndex = stackIndex; /* Index of the stack (1 - 7) */
                    return HitTestType.Tableau;
                }
                stackIndex++;
            }
            return HitTestType.None;
        }

        /* Private helper method */
        private static bool IsRegion(Point src, Rectangle dest)
        {
            return src.X >= dest.X && src.X <= dest.X + dest.Width && src.Y >= dest.Y && src.Y <= dest.Y + dest.Height;
        }
    }
}
