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
        public Deck GameDeck { get; set; }
        public List<Card> DealtCards { get; set; } /* Stock */

        public List<StackData> HomeStacks { get; set; } /* Foundation */
        public List<StackData> PlayingStacks { get; set; } /* Tableau */

        public int GameTime { get; set; }
        public int GameScore { get; set; }

        public GameData()
        {
            GameDeck = new Deck();
            DealtCards = new List<Card>();
            HomeStacks = new List<StackData>();
            PlayingStacks = new List<StackData>();
        }

        /* Copy constructor - reference deep copy */
        public GameData(GameData data)
        {
            /* This is the work-around to copying lists with reference types - if you just straight copied the list
             * using a copy constructor, without using "new" keyword; any modification on the list being copied is
             * reflected in the copy... could use IClonable/MemberwiseClone... but... */
            GameDeck = new Deck();
            foreach (var c in data.GameDeck)
            {
                GameDeck.Add(new Card(c));
            }
            DealtCards = new List<Card>();
            foreach (var c in data.DealtCards)
            {
                DealtCards.Add(new Card(c));
            }
            HomeStacks = new List<StackData>();
            foreach (var s in data.HomeStacks)
            {
                HomeStacks.Add(new StackData(s));
            }
            PlayingStacks = new List<StackData>();
            foreach (var s in data.PlayingStacks)
            {
                PlayingStacks.Add(new StackData(s));
            }
        }      
    }
}
