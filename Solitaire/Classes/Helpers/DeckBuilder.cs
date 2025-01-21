/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */

using System.Collections.Generic;
using System.Drawing;
using Solitaire.Classes.Data;
using Solitaire.Classes.Serialization;
using Solitaire.Properties;

namespace Solitaire.Classes.Helpers
{
    /* To be excluded from solution including deck-set.png & bg.png from resources
     * DON'T forget to re-include deck-set.png, bg.png & card-backs.png in Resources before attempting to execute
     * code below and remember to remove it! */
    public static class DeckBuilder
    {
        private static readonly Size CardSize = new Size(146, 198); /* Hard programmed for now */

        private static readonly List<CardData> Deck = new List<CardData>(); 

        private static readonly GraphicsObjectData ObjData = new GraphicsObjectData();

        public static void BuildDeck()
        {
            ObjData.Background = Resources.bg;
            Bitmap cardImage;
            Rectangle src;
            for (var y = 0; y <= 3; y++)
            {
                var startY = CardSize.Height * y;
                for (var x = 0; x <= 12; x++)
                {
                    /* Set each card image */
                    System.Diagnostics.Debug.Print("> Set card Suit: {0} Value: {1}", y, x + 1);
                    cardImage = new Bitmap(CardSize.Width, CardSize.Height);
                    src = new Rectangle(x*CardSize.Width, startY, CardSize.Width, CardSize.Height);
                    GetImage(cardImage, src, Resources.card_set);
                    var card = new CardData
                    {
                        Suit = (Suit) y,
                        Value = x + 1,
                        Image = cardImage
                    };
                    /* Push new image to the deck */
                    Deck.Add(card);
                }
            }
            /* Build assets */
            for (var asset = 0; asset <= 2; asset++)
            {
                cardImage = new Bitmap(CardSize.Width, CardSize.Height);
                src = new Rectangle(asset * CardSize.Width, 0, CardSize.Width, CardSize.Height);
                GetImage(cardImage, src, Resources.assets);
                switch (asset)
                {
                    case 0:
                        /* Set empty stock image */
                        System.Diagnostics.Debug.Print(">> Set empty stock");
                        ObjData.EmptyStock = cardImage;
                        break;

                    case 1:
                        /* Set empty foundation image */
                        System.Diagnostics.Debug.Print(">> Set empty foundation");
                        ObjData.EmptyFoundation = cardImage;
                        break;

                    case 2:
                        /* Set empty tableau image */
                        System.Diagnostics.Debug.Print(">> Set empty taleau");
                        ObjData.EmptyTableau = cardImage;
                        break;
                }
            }
            /* Build the deck backs list */
            for (var i = 0; i <= 6; i++)
            {
                cardImage = new Bitmap(CardSize.Width, CardSize.Height);
                src = new Rectangle(i * CardSize.Width, 0, CardSize.Width, CardSize.Height);
                GetImage(cardImage, src, Resources.deck_backs);
                System.Diagnostics.Debug.Print(">>> Adding deck back image " + (i + 1));
                ObjData.CardBacks.Add(cardImage);
            }
            /* Other images */
            ObjData.ButtonOk = Resources.button_ok;
            ObjData.ButtonCancel = Resources.button_cancel;
            ObjData.NewGameBackground = Resources.new_game_bg;
            ObjData.Logo = Resources.logo;
            /* Serialize the output */
            if (BinarySerialize<List<CardData>>.Save(Utils.MainDir(@"\data\gfx\cards.dat"), Deck))
            {
                System.Diagnostics.Debug.Print(">>>> Sucessfully wrote cards.dat!");
            }
            if (BinarySerialize<GraphicsObjectData>.Save(Utils.MainDir(@"\data\gfx\obj.dat"), ObjData))
            {
                System.Diagnostics.Debug.Print(">>>> Sucessfully wrote obj.dat!");
            }
        }

        private static void GetImage(Image cardBmp, Rectangle srcRegion, Bitmap srcBitmap)
        {
            using (var gfx = Graphics.FromImage(cardBmp))
            {
                gfx.DrawImage(srcBitmap, new Rectangle(0, 0, cardBmp.Width, cardBmp.Height), srcRegion, GraphicsUnit.Pixel);
            }
        }
    }
}
