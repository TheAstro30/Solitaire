/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Collections.Generic;
using System.Linq;
using Solitaire.Classes.Data;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.UI;

namespace Solitaire.Classes.Helpers.Logic
{
    public static class GameLogic
    {
        public static void BuildStacks(Game ctl)
        {
            /* Set home stacks */
            for (var i = 0; i <= 3; i++)
            {
                ctl.CurrentGame.HomeStacks.Add(new StackData());
            }
            /* Set up playable stacks */
            var stackSize = 8;
            for (var i = 0; i <= 6; i++)
            {
                var stack = new StackData();
                for (var y = 1; y <= stackSize; y++)
                {
                    var card = ctl.CurrentGame.GameDeck[0];
                    card.IsHidden = y != stackSize;
                    stack.Cards.Add(card);
                    ctl.CurrentGame.GameDeck.Remove(card);
                }
                ctl.CurrentGame.PlayingStacks.Add(stack);
                stackSize--;
            }
        }

        public static void Deal(Game ctl)
        {
            /* Deal cards from deck - may need to consult the NTSB */
            if (ctl.CurrentGame.GameDeck.Count > 0)
            {
                Card card;
                if (SettingsManager.Settings.Options.DrawThree)
                {
                    /* Need to draw 3 cards out of deck */
                    var count = 0;
                    for (var i = 0; i <= ctl.CurrentGame.GameDeck.Count - 1; i++)
                    {
                        if (count == 3)
                        {
                            break;
                        }
                        count++;
                        card = ctl.CurrentGame.GameDeck[0];
                        card.IsHidden = false;
                        ctl.CurrentGame.DealtCards.Add(card);
                        ctl.CurrentGame.GameDeck.Remove(card);
                    }
                }
                else
                {
                    card = ctl.CurrentGame.GameDeck[0];
                    card.IsHidden = false;
                    ctl.CurrentGame.DealtCards.Add(card);
                    ctl.CurrentGame.GameDeck.Remove(card);
                }
                AudioManager.Play(SoundType.Deal);
            }
            else
            {
                if (ctl.CurrentGame.DealtCards.Count == 0)
                {
                    /* Nothing to do */
                    AudioManager.Play(SoundType.Empty);
                    return;
                }
                /* Copy cards from disposed back to normal deck */
                ctl.CurrentGame.GameDeck.AddRange(ctl.CurrentGame.DealtCards);
                ctl.CurrentGame.GameDeck.IsDeckReshuffled = true;
                ctl.CurrentGame.DealtCards = new List<Card>();
                AudioManager.Play(SoundType.Shuffle);
            }
        }

        public static bool IsValidMove(Card source, Card destination)
        {
            /* Source is card of lower value -> card of higher value - suits have to be opposite colors and values 1
             * less on the source than the destination: Valid opposite suit and number order of source is lower than destination */
            return IsOppositeSuit(source.Suit, destination.Suit) && source.Value == destination.Value - 1;
        }

        public static bool CheckWin(Game ctl)
        {
            return ctl.CurrentGame.HomeStacks.Sum(stack => stack.Cards.Count) == 52;
        }

        public static void ReturnCardsToSource(Game ctl, bool homeStack, int dragStackIndex, List<Card> dragCards)
        {
            Undo.RemoveLastEntry(); /* No point storing last entry if the card isn't being moved from its original spot */
            if (dragStackIndex == -1)
            {
                /* Return to disposed pile */
                ctl.CurrentGame.DealtCards.Add(dragCards[0]);
            }
            else
            {
                if (homeStack)
                {
                    var stack = ctl.CurrentGame.HomeStacks[dragStackIndex];
                    var card = dragCards[0];
                    stack.Cards.Add(card);
                    stack.Suit = card.Suit;
                    return;
                }
                ctl.CurrentGame.PlayingStacks[dragStackIndex].Cards.AddRange(dragCards);
            }
        }

        public static bool IsCompleted(Game ctl, Card card)
        {
            foreach (var stack in from stack in ctl.CurrentGame.HomeStacks where stack.Cards.Count > 0 let cd = stack.Cards[stack.Cards.Count - 1] where cd.Suit == card.Suit && cd.Value == card.Value - 1 select stack)
            {
                Undo.AddMove(ctl.CurrentGame);
                stack.Cards.Add(card);
                return true;
            }
            return false;
        }

        public static bool AddAceToFreeSlot(Game ctl, Card card)
        {
            foreach (var stack in ctl.CurrentGame.HomeStacks.Where(stack => stack.Cards.Count == 0).Where(stack => !card.IsHidden))
            {
                Undo.AddMove(ctl.CurrentGame);
                stack.Cards.Add(card);
                return true;
            }
            return false;
        }

        /* Private methods */
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
    }
}
