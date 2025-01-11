/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using Solitaire.Classes.Helpers;

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

        //public bool CanRestart { get; set; }

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
       
        public void StartNewGame()
        {
            /* Shuffle the deck */  
            GameDeck.Shuffle();            
            DealtCards = new List<Card>();
            HomeStacks = new List<StackData>();
            PlayingStacks = new List<StackData>();
            BuildStacks();
            GameScore = 0;
            GameTime = 0;
        }

        public void Deal()
        {
            /* Deal cards from deck - may need to consult the NTSB */
            if (GameDeck.Count > 0)
            {
                var card = GameDeck[0];
                card.IsHidden = false;
                DealtCards.Add(card);
                GameDeck.Remove(card);
                AudioManager.Play(SoundType.Deal);
            }
            else
            {
                if (DealtCards.Count == 0)
                {
                    /* Nothing to do */
                    AudioManager.Play(SoundType.Empty);
                    return;
                }
                /* Copy cards from disposed back to normal deck */
                GameDeck.AddRange(DealtCards);
                GameDeck.IsDeckReshuffled = true;
                DealtCards = new List<Card>();
                AudioManager.Play(SoundType.Shuffle);
            }
        }

        /* Private methods */
        private void BuildStacks()
        {
            /* Set home stacks */
            for (var i = 0; i <= 3; i++)
            {
                HomeStacks.Add(new StackData());
            }
            /* Set up playable stacks */
            var stackSize = 8;
            for (var i = 0; i <= 6; i++)
            {
                var stack = new StackData();
                for (var y = 1; y <= stackSize; y++)
                {
                    var card = GameDeck[0];
                    card.IsHidden = y != stackSize;
                    stack.Cards.Add(card);
                    GameDeck.Remove(card);                    
                }
                PlayingStacks.Add(stack);
                stackSize--;
            }
        }
    }
}
