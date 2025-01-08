/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;

namespace Solitaire.Classes
{
    public enum Suit
    {
        Hearts = 0,
        Diamonds = 1,
        Clubs = 2,
        Spades = 3
    }

    public sealed class Card
    {
        /* Is the card the card back or a normal card */
        public bool IsCardBack { get; set; }

        /* Card can't be seen */
        public bool IsHidden { get; set; }
        
        /* Spades, diamonds, hearts, clubs */
        public Suit Suit { get; set; }
        
        /* Ace - King (1 - 13) */
        public int Value { get; set; }

        /* Card face or back */
        public Image CardImage { get; set; }

        public Rectangle Region { get; set; }

        /* Constructors */
        public Card()
        {
            Region = new Rectangle();
        }

        public Card(bool isBack, Suit suit, int value, Image image)
        {            
            IsCardBack = isBack;
            Suit = suit;
            Value = value;
            CardImage = image;
            Region = new Rectangle(); /* I don't think it's necessary to copy this value, as painting resets it anyway */
        }

        public Card(Card card)
        {
            IsCardBack = card.IsCardBack;
            IsHidden = card.IsHidden;
            Suit = card.Suit;
            Value = card.Value;
            CardImage = new Bitmap(card.CardImage);
            Region = new Rectangle(); /* I don't think it's necessary to copy this value, as painting resets it anyway */
        }

        public bool IsHitTest(Point location)
        {
            /* Is this hard at the mouse region on screen? */
            return location.X >= Region.X && location.X <=Region.X+ Region.Width && location.Y >= Region.Y && location.Y <= Region.Y + Region.Height;
        }
    }
}
