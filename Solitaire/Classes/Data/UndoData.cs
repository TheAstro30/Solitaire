/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Collections.Generic;

namespace Solitaire.Classes.Data
{
    public class UndoData
    {
        /* Data class for storing copies of current cards of current game */
        public Deck GameDeck { get; set; }

        public List<Card> DealtCards { get; set; }

        public List<StackData> HomeStacks { get; set; }

        public List<StackData> PlayingStacks { get; set; }

        public UndoData()
        {
            GameDeck = new Deck();
            DealtCards = new List<Card>();
            HomeStacks = new List<StackData>();
            PlayingStacks = new List<StackData>();
        }
    }
}
