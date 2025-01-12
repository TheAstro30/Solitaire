/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Collections.Generic;
using System.Drawing;
using Solitaire.Classes.Data;
using Solitaire.Classes.UI;

namespace Solitaire.Classes.Helpers.UI
{
    public enum HitTestType
    {
        None = 0,
        Deck = 1,
        Dealt = 2,
        HomeStack = 3,
        PlayStack = 4
    }

    public struct HitTestData
    {
        public List<Card> Cards { get; set; }

        public int StackIndex { get; set; }

        public int CardIndex { get; set; }
    }

    public static class HitTest
    {
        public static HitTestType Compare(Game ctl, Point location, Rectangle deckRegion, out HitTestData data)
        {
            data = new HitTestData();
            
            /* We need to work out what the mouse is over */
            if (IsRegion(location, deckRegion))
            {
                return HitTestType.Deck;
            }
            /* Dealt pile */            
            if (ctl.CurrentGame.DealtCards.Count > 0 && ctl.CurrentGame.DealtCards[ctl.CurrentGame.DealtCards.Count - 1].IsHitTest(location))
            {
                return HitTestType.Dealt;
            }            
            var stackIndex = 0;
            /* Home stacks */
            foreach (var s in ctl.CurrentGame.HomeStacks)
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
            foreach (var stack in ctl.CurrentGame.PlayingStacks)
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

        private static bool IsRegion(Point src, Rectangle dest)
        {
            return src.X >= dest.X && src.X <= dest.X + dest.Width && src.Y >= dest.Y && src.Y <= dest.Y + dest.Height;
        }
    }
}
