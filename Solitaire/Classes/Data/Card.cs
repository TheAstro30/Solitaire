/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;

namespace Solitaire.Classes.Data
{
    /* Suits of the deck */
    public enum Suit
    {
        Hearts = 0,
        Diamonds = 1,
        Spades = 2,
        Clubs = 3,
        None = 4
    }

    /* Serialized class of card images */
    [Serializable]
    public sealed class CardData
    {
        /* Spades, diamonds, hearts, clubs */
        public Suit Suit { get; set; }

        /* Ace - King (1 - 13) */
        public int Value { get; set; }

        public Image Image { get; set; }
    }

    /* Card class used during game play (no images; uses above data class) */
    [Serializable]
    public sealed class Card
    {
        /* Card can't be seen */
        public bool IsHidden { get; set; }
        
        /* Spades, diamonds, hearts, clubs */
        public Suit Suit { get; set; }
        
        /* Ace - King (1 - 13) */
        public int Value { get; set; }

        /* Rectangle to drawn region on the screen for dunny hit test */
        public Rectangle Region { get; set; }

        /* Constructors */
        public Card()
        {
            /* Empty by default */
        }

        public Card(Card card)
        {
            IsHidden = card.IsHidden;
            Suit = card.Suit;
            Value = card.Value;
        }

        public bool IsHitTest(Point location)
        {
            /* Is this hard at the mouse region on screen? */
            return location.X >= Region.X && location.X <=Region.X+ Region.Width && location.Y >= Region.Y && location.Y <= Region.Y + Region.Height;
        }
    }
}
