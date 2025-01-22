/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Solitaire.Classes.Data;
using Solitaire.Classes.Helpers.Management;
using Solitaire.Classes.Settings.SettingsData;
using Solitaire.Classes.UI;

namespace Solitaire.Classes.Helpers.Logic
{
    public class HintData
    {
        /* Source card region */
        public Rectangle SourceRegion { get; set; }

        /* Destination card region */
        public Rectangle DestinationRegion { get; set; }

        /* Number of cards of a partially completed stack */
        public int SourceCardCount { get; set; }
    }

    public static class GameLogic
    {
        private static readonly List<HintData> Hints = new List<HintData>();

        public static void BuildStacks(Game ctl)
        {
            /* Set home stacks */
            for (var i = 0; i <= 3; i++)
            {
                ctl.CurrentGame.Foundation.Add(new StackData());
            }
            /* Set up playable stacks */
            var stackSize = 1;
            for (var i = 0; i <= 6; i++)
            {
                var stack = new StackData();
                for (var y = 1; y <= stackSize; y++)
                {
                    var card = ctl.CurrentGame.StockCards[0];
                    card.IsHidden = y != stackSize;
                    stack.Cards.Add(card);
                    ctl.CurrentGame.StockCards.Remove(card);
                }
                ctl.CurrentGame.Tableau.Add(stack);
                stackSize++;
            }
        }

        public static bool Deal(Game ctl)
        {
            /* Deal cards from deck - may need to consult the NTSB */
            if (ctl.CurrentGame.StockCards.Count > 0)
            {
                Card card;
                if (ctl.CurrentGame.IsDrawThree)
                {
                    /* Need to draw 3 cards out of deck */
                    var count = 0;
                    for (var i = 0; i <= ctl.CurrentGame.StockCards.Count - 1; i++)
                    {
                        if (count == 3)
                        {
                            break;
                        }
                        count++;
                        card = ctl.CurrentGame.StockCards[0];
                        card.IsHidden = false;
                        ctl.CurrentGame.WasteCards.Add(card);
                        ctl.CurrentGame.StockCards.Remove(card);
                    }
                }
                else
                {
                    card = ctl.CurrentGame.StockCards[0];
                    card.IsHidden = false;
                    ctl.CurrentGame.WasteCards.Add(card);
                    ctl.CurrentGame.StockCards.Remove(card);
                }
                AudioManager.Play(SoundType.Deal);
            }
            else
            {
                if (ctl.CurrentGame.WasteCards.Count == 0)
                {
                    /* Nothing to do */
                    AudioManager.Play(SoundType.Empty);
                    return false;
                }
                /* Check difficulty */
                switch (SettingsManager.Settings.Options.Difficulty)
                {
                    case DifficultyLevel.Medium:
                        /* Limit redeals to 3 */
                        if (ctl.CurrentGame.DeckRedeals == 3)
                        {
                            AudioManager.Play(SoundType.Empty);
                            return false;
                        }
                        ctl.CurrentGame.DeckRedeals++;
                        break;

                    case DifficultyLevel.Hard:
                        /* 0 redeals */
                        AudioManager.Play(SoundType.Empty);
                        return false;
                }
                /* Copy cards from disposed back to normal deck */
                ctl.CurrentGame.StockCards.AddRange(ctl.CurrentGame.WasteCards);
                ctl.IsDeckReDealt = true;
                ctl.CurrentGame.WasteCards = new List<Card>();
                AudioManager.Play(SoundType.Shuffle);
            }
            return true;
        }

        public static bool IsValidMove(Card source, Card destination)
        {
            /* Source is card of lower value -> card of higher value - suits have to be opposite colors and values 1
             * less on the source than the destination: Valid opposite suit and number order of source is lower than destination */
            return IsOppositeSuit(source.Suit, destination.Suit) && source.Value == destination.Value - 1;
        }

        public static bool CheckWin(Game ctl)
        {
            return FoundationCount(ctl) == 52;
        }

        public static int FoundationCount(Game ctl)
        {
            return ctl.CurrentGame.Foundation.Sum(stack => stack.Cards.Count);
        }

        public static void ReturnCardsToSource(Game ctl, bool foundation)
        {
            Undo.RemoveLastEntry(); /* No point storing last entry if the card isn't being moved from its original spot */
            if (ctl.DragStackIndex == -1)
            {
                /* Return to disposed pile */
                ctl.CurrentGame.WasteCards.Add(ctl.DraggingCards[0]);
            }
            else
            {
                if (foundation)
                {
                    var stack = ctl.CurrentGame.Foundation[ctl.DragStackIndex];
                    var card = ctl.DraggingCards[0];
                    stack.Cards.Add(card);
                    stack.Suit = card.Suit;
                    return;
                }
                ctl.CurrentGame.Tableau[ctl.DragStackIndex].Cards.AddRange(ctl.DraggingCards);
            }
        }

        public static bool IsCompleted(Game ctl, Card card)
        {
            foreach (var stack in from stack in ctl.CurrentGame.Foundation where stack.Cards.Count > 0 let cd = stack.Cards[stack.Cards.Count - 1] where cd.Suit == card.Suit && cd.Value == card.Value - 1 select stack)
            {
                Undo.AddMove(ctl.CurrentGame);
                stack.Cards.Add(card);
                return true;
            }
            return false;
        }

        public static bool AddAceToFreeSlot(Game ctl, Card card)
        {
            foreach (var stack in ctl.CurrentGame.Foundation.Where(stack => stack.Cards.Count == 0).Where(stack => !card.IsHidden))
            {
                Undo.AddMove(ctl.CurrentGame);
                stack.Suit = card.Suit;
                stack.Cards.Add(card);
                return true;
            }
            return false;
        }

        public static List<HintData> GetHint(Game ctl)
        {
            /* "Caching" the results, we don't wan't to continuous loop all the time for multiple matches. Use ClearHints() on resize, game load, new game and OnMouseUp */
            if (Hints.Count > 0)
            {
                return Hints;
            }
            HintData data;
            Card waste = null;
            int index;
            /* The following lines of code aren't pretty, by any means... there is probably a better way to do this */
            if (ctl.CurrentGame.WasteCards.Count != 0)
            {
                waste = ctl.CurrentGame.WasteCards[ctl.CurrentGame.WasteCards.Count - 1];
                /* Can the waste card be moved to a foundation? */
                index = IsHintFoundation(ctl, waste);
                if (index != -1)
                {
                    data = new HintData
                    {
                        SourceRegion = waste.Region,
                        DestinationRegion = ctl.CurrentGame.Foundation[index].Region
                    };
                    Hints.Add(data);
                }              
            }
            /* Iterate each tableau - I also need to figure out how I'm going to include partially completed stacks */
            for (var srcIndex = 0; srcIndex <= 6; srcIndex++)
            {
                if (ctl.CurrentGame.Tableau[srcIndex].Cards.Count == 0)
                {
                    /* Nothing to compare */
                    if (waste != null && waste.Value == 13)
                    {
                        data = new HintData
                        {
                            SourceRegion = waste.Region,
                            DestinationRegion = ctl.CurrentGame.Tableau[srcIndex].Region
                        };
                        Hints.Add(data);
                    }
                    continue;
                }
                var srcCard = ctl.CurrentGame.Tableau[srcIndex].Cards[ctl.CurrentGame.Tableau[srcIndex].Cards.Count - 1];
                if (srcCard.IsHidden)
                {
                    /* Nothing to compare, but should suggest user to turn it over */
                    data = new HintData
                    {
                        SourceRegion = srcCard.Region
                    };
                    Hints.Add(data);
                    continue;
                }
                /* Can the waste card be put on top of this card? */
                if (waste != null && IsValidMove(waste, srcCard))
                {
                    data = new HintData
                    {
                        SourceRegion = waste.Region,
                        DestinationRegion = srcCard.Region
                    };
                    Hints.Add(data);
                }
                /* Can the srcCard be moved to a foundation? */
                index = IsHintFoundation(ctl, srcCard);
                if (index != -1)
                {
                    data = new HintData
                    {
                        SourceRegion = srcCard.Region,
                        DestinationRegion = ctl.CurrentGame.Foundation[index].Region
                    };
                    Hints.Add(data);
                }
                /* Need to check for a partially completed stack */
                var stackCount = 0;
                Card stackStartCard = null;
                foreach (var c in ctl.CurrentGame.Tableau[srcIndex].Cards.Where(c => !c.IsHidden))
                {
                    stackCount++;
                    if (stackStartCard == null)
                    {
                        stackStartCard = c;
                    }
                }
                /* Match source card and stack starting card to a destination tableau */
                for (var destIndex = 0; destIndex <= 6; destIndex++)
                {
                    if (destIndex == srcIndex)
                    {
                        continue;
                    }
                    if (ctl.CurrentGame.Tableau[destIndex].Cards.Count == 0)
                    {
                        /* Is it a king from the source? */
                        if (srcCard.Value == 13)
                        {
                            data = new HintData
                            {
                                SourceRegion = srcCard.Region,
                                DestinationRegion = ctl.CurrentGame.Tableau[destIndex].Region
                            };
                            Hints.Add(data);
                        }
                        /* If we have mulitple cards as part of a stack and the first card of that stack is a king, then it's a valid move */
                        if (stackStartCard != null && stackStartCard.Value == 13)
                        {
                            data = new HintData
                            {
                                SourceRegion = stackStartCard.Region,
                                DestinationRegion = ctl.CurrentGame.Tableau[destIndex].Region,
                                SourceCardCount = stackCount - 1
                            };
                            Hints.Add(data);
                        }
                        continue;
                    }
                    var destCard = ctl.CurrentGame.Tableau[destIndex].Cards[ctl.CurrentGame.Tableau[destIndex].Cards.Count - 1];
                    /* Check if the start card of a partially completed stack can be moved */
                    if (!destCard.IsHidden && stackStartCard != null && IsValidMove(stackStartCard, destCard))
                    {
                        data = new HintData
                        {
                            SourceRegion = stackStartCard.Region,
                            DestinationRegion = destCard.Region,
                            SourceCardCount = stackCount - 1
                        };
                        Hints.Add(data);
                        continue;
                    }
                    /* Assuming we're only looking at a single card instead of a partial stack, check that the source valid with destination */
                    if (destCard.IsHidden || !IsValidMove(srcCard, destCard))
                    {
                        continue;
                    }

                    data = new HintData
                    {
                        SourceRegion = srcCard.Region,
                        DestinationRegion = destCard.Region
                    };
                    Hints.Add(data);
                }
            }
            return Hints;
        }

        public static void ClearHints()
        {
            Hints.Clear();
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

        private static int IsHintFoundation(Game ctl, Card srcCard)
        {
            var index = 0;
            foreach (var foundation in ctl.CurrentGame.Foundation)
            {
                if (foundation.Cards.Count == 0)
                {
                    if (srcCard.Value == 1)
                    {
                        /* It's an ace and the slot is empty */
                        return index;
                    }
                }
                else if (foundation.Suit == srcCard.Suit &&
                    foundation.Cards[foundation.Cards.Count - 1].Value == srcCard.Value - 1)
                {
                    /* Valid */
                    return index;
                }
                index++;
            }
            return -1;
        }
    }
}
