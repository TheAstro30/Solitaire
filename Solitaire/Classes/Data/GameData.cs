/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;

namespace Solitaire.Classes.Data
{
    [Serializable]
    public sealed class GameData
    {
        /* Game-play objects */
        public Deck StockCards { get; set; } /* Stock cards (deck) */
        public List<Card> WasteCards { get; set; } /* Waste cards (dealt) */

        public List<StackData> Foundation { get; set; } /* Foundation (aces to kings) */
        public List<StackData> Tableau { get; set; } /* Tableau (each playing stack out of 7) */

        public int GameTime { get; set; }
        public int GameScore { get; set; }
        public int Moves { get; set; }

        public GameData()
        {
            StockCards = new Deck();
            WasteCards = new List<Card>();
            Foundation = new List<StackData>();
            Tableau = new List<StackData>();
        }

        /* Copy constructor - reference deep copy */
        public GameData(GameData data)
        {
            /* This is the work-around to copying lists with reference types - if you just straight copied the list
             * using a copy constructor, without using "new" keyword; any modification on the list being copied is
             * reflected in the copy... could use IClonable/MemberwiseClone... but... */
            StockCards = new Deck();
            foreach (var c in data.StockCards)
            {
                StockCards.Add(new Card(c));
            }
            WasteCards = new List<Card>();
            foreach (var c in data.WasteCards)
            {
                WasteCards.Add(new Card(c));
            }
            Foundation = new List<StackData>();
            foreach (var s in data.Foundation)
            {
                Foundation.Add(new StackData(s));
            }
            Tableau = new List<StackData>();
            foreach (var s in data.Tableau)
            {
                Tableau.Add(new StackData(s));
            }
            Moves = data.Moves;
        }      
    }
}
