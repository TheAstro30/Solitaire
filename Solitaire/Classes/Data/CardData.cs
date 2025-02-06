/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Drawing;
using Solitaire.Controls.ObjectListView.Implementation;

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

    /* Image class for cards - allows expansion to use different sets */
    [Serializable]
    public sealed class Cards
    {
        public string Name { get; set; }

        [OlvIgnore]
        public Dictionary<KeyValuePair<Suit, int>, CardData> Images { get; private set; }

        [OlvIgnore]
        public Image PreviewImage { get; set; }

        public Cards()
        {
            Images = new Dictionary<KeyValuePair<Suit, int>, CardData>();
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /* Card class used during game play (no images; uses above CardData.cs class) */
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
            return location.X >= Region.X && location.X <= Region.X + Region.Width && location.Y >= Region.Y && location.Y <= Region.Y + Region.Height;
        }
    }
}
