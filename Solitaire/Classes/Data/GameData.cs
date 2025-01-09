/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Drawing;
using Solitaire.Classes.Helpers;
using Solitaire.Properties;

namespace Solitaire.Classes.Data
{
    [Serializable]
    public sealed class GameData
    {
        public Image CardBack { get; private set; }
        public Image EmptyDeck { get; private set; }

        public Deck GameDeck { get; private set; }
        public List<Card> DealtCards { get; private set; }

        public List<StackData> HomeStacks { get; private set; }       
        public List<StackData> PlayingStacks { get; private set; }

        private readonly Deck _masterDeck = new Deck();

        public GameData(bool initNewDeck)
        {
            if (!initNewDeck)
            {
                /* We don't want to create a new deck (which lags a bit) when deserializing from a saved file
                 * StartNewGame() should be called within the game for new games, and only call new GameData(true) 
                 * on first load of the exe */
                return;
            }
            HomeStacks = new List<StackData>();
            BuildDeck();
            StartNewGame();
        }

        public void StartNewGame()
        {
            /* Shuffle the deck */
            _masterDeck.Shuffle();
            /* Copy master deck */
            GameDeck = new Deck(_masterDeck);            
            DealtCards = new List<Card>();
            foreach (var stack in HomeStacks)
            {
                stack.Cards = new List<Card>();
            }
            PlayingStacks = new List<StackData>();
            BuildStacks();
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
                DealtCards = new List<Card>();
                AudioManager.Play(SoundType.Shuffle);
            }
        }

        /* Deck building */
        public void BuildDeck()
        {
            var cardSize = new Size(120, 184); /* Hard programmed for now */

            for (var y = 0; y <= 3; y++)
            {
                var startY = cardSize.Height * y;
                for (var x = 0; x <= 13; x++)
                {
                    var cardImage = new Bitmap(cardSize.Width, cardSize.Height);
                    var src = new Rectangle(x * cardSize.Width, startY, cardSize.Width, cardSize.Height);
                    GetImage(cardImage, src);
                    cardImage.MakeTransparent(Color.FromArgb(1, 1, 1));
                    /* Card 14 - which doesn't exist (there's only 13 per suit), set card back and home stack images */
                    if (x == 13)
                    {
                        switch (y)
                        {
                            case 0:
                                /* Set card back */
                                CardBack = cardImage;
                                break;

                            case 1:
                                /* Set home stack image */
                                for (var i = 0; i <= 3; i++)
                                {
                                    var stack = new StackData
                                    {
                                        Suit = (Suit)i,
                                        StackImage = cardImage
                                    };
                                    HomeStacks.Add(stack);
                                }
                                break;

                            case 2:
                                /* Set empty deck image */
                                EmptyDeck = cardImage;
                                break;
                        }
                    }
                    else
                    {
                        /* Normal card */
                        var card = new Card(false, (Suit)y, x + 1, cardImage);
                        /* Push new image to the deck */
                        _masterDeck.Add(card);
                    }
                }
            }
        }

        private void BuildStacks()
        {
            /* Temporarily build a stack with one card in it */
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

        private static void GetImage(Image cardBmp, Rectangle srcRegion)
        {
            using (var gfx = Graphics.FromImage(cardBmp))
            {
                gfx.DrawImage(Resources.card_set, new Rectangle(0, 0, cardBmp.Width, cardBmp.Height), srcRegion, GraphicsUnit.Pixel);
            }
        }
    }
}
