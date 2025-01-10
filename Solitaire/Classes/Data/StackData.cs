/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Solitaire.Classes.Data
{
    [Serializable]
    public class StackData
    {
        /* This is the 7 rows of playable cards - and ace slots */
        public Suit Suit { get; set; }

        public Rectangle Region { get; set; }
        public List<Card> Cards { get; set; }

        public StackData()
        {
            Region = new Rectangle();
            Cards = new List<Card>();
        }

        public StackData(StackData data)
        {
            Region = new Rectangle();
            Cards = new List<Card>();

            Suit = data.Suit;

            foreach (var c in data.Cards)
            {
                Cards.Add(new Card(c));
            }
        }
    }
}
