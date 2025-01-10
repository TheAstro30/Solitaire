/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Drawing;
using Solitaire.Classes.Data;
using Solitaire.Classes.Serialization;
using Solitaire.Properties;

namespace Solitaire.Classes.Helpers
{
    /* To be excluded from solution including deck-set.png & bg.png from resources
     * DON'T forget to re-include deck-set.png & bg.png in Resources before attempting to execute
     * code below and remember to remove it! */
    public static class DeckBuilder
    {
        private static readonly Deck Deck = new Deck();

        private static readonly GraphicsObjectData ObjData = new GraphicsObjectData();

        public static void BuildDeck()
        {
            var cardSize = new Size(120, 184); /* Hard programmed for now */

            ObjData.Background = Resources.bg;

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
                                System.Diagnostics.Debug.Print("set back");
                                ObjData.CardBack = cardImage;
                                break;

                            case 1:
                                /* Set home stack image */
                                System.Diagnostics.Debug.Print("set homestack");
                                ObjData.HomeStack = cardImage;
                                break;

                            case 2:
                                /* Set empty deck image */
                                ObjData.EmptyDeck = cardImage;
                                break;
                        }
                    }
                    else
                    {
                        /* Normal card */
                        var card = new Card(false, (Suit)y, x + 1, cardImage);
                        /* Push new image to the deck */
                        Deck.Add(card);
                    }
                }
            }
            /* Serialize the output */
            if (BinarySerialize<Deck>.Save(AppPath.MainDir(@"\data\gfx\cards.dat"), Deck))
            {
                System.Diagnostics.Debug.Print("Sucessfully wrote cards.dat!");
            }
            if (BinarySerialize<GraphicsObjectData>.Save(AppPath.MainDir(@"\data\gfx\obj.dat"), ObjData))
            {
                System.Diagnostics.Debug.Print("Sucessfully wrote obj.dat!");
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
