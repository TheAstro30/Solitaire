/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Solitaire.Classes.Data;
using Solitaire.Classes.Serialization;

namespace Solitaire.Classes.Helpers
{
    /* To be excluded from solution including deck-set.png & bg.png from resources
     * DON'T forget to re-include deck-set.png, bg.png & card-backs.png in Resources before attempting to execute
     * code below and remember to remove it! */
    public static class Images
    {
        private static readonly Size CardSize = new Size(146, 198); /* Hard programmed for now */

        private static readonly GraphicsObjectData ObjData = new GraphicsObjectData();

        public static void Build()
        {
            var parent = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
            if (parent == null)
            {
                /* This would only fail if the parent directory is the root directory, which, in most cases
                 * should never happen with any solution files and folders... */
                MessageBox.Show(@"Fatal error in Images.Build(). Path not found", @"Images.Build()");
                return;
            }

            var path = $@"{parent.FullName}\Images\";

            /* Build card sets */
            BuildCardSet($"{path}{@"card-set.png"}", "Default", "default.dat");
            BuildCardSet($"{path}{@"card-set2.png"}", "Classic Bicycle", "bicycle.dat");
            BuildCardSet($"{path}{@"card-set3.png"}", "Flat Large", "flat.dat");

            /* Build deck backs */
            BuildDeckBacks($"{path}{@"deck-backs.png"}");

            /* Build assets */
            BuildAssets($"{path}{@"assets.png"}");

            BuildBackgrounds(path);
            
            /* Serialize assets output */
            if (BinarySerialize<GraphicsObjectData>.Save(Utils.MainDir(@"\data\gfx\obj.dat"), ObjData))
            {
                System.Diagnostics.Debug.Print(">>>> Sucessfully wrote obj.dat!");
            }
        }

        private static void BuildCardSet(string fileName, string cardSetName, string outputFileName)
        {
            var data = new Cards {Name = cardSetName};

            using (var cards = (Bitmap)Image.FromFile(fileName))
            {
                for (var y = 0; y <= 3; y++)
                {
                    var startY = CardSize.Height * y;
                    for (var x = 0; x <= 12; x++)
                    {
                        /* Set each card image */
                        System.Diagnostics.Debug.Print(" > Set card Suit: {0} Value: {1}", y, x + 1);
                        var cardImage = new Bitmap(CardSize.Width, CardSize.Height);
                        var src = new Rectangle(x * CardSize.Width, startY, CardSize.Width, CardSize.Height);
                        GetImage(cardImage, src, cards);
                        var card = new CardData
                        {
                            Suit = (Suit)y,
                            Value = x + 1,
                            Image = cardImage
                        };
                        /* Push new image to the deck */
                        data.Images.Add(new KeyValuePair<Suit, int>(card.Suit, card.Value), card);
                    }
                }
            }
            /* Set preview image */
            data.PreviewImage = GenerateCardSetPreview(data);

            /* Serialize cards output */
            if (BinarySerialize<Cards>.Save(Utils.MainDir($@"\data\gfx\cards\{outputFileName}"), data))
            {
                System.Diagnostics.Debug.Print($" >>>> Sucessfully wrote {outputFileName}!");
            }
        }

        private static void BuildDeckBacks(string fileName)
        {
            using (var decks = (Bitmap)Image.FromFile(fileName))
            {
                for (var i = 0; i <= 7; i++)
                {
                    var cardImage = new Bitmap(CardSize.Width, CardSize.Height);
                    var src = new Rectangle(i * CardSize.Width, 0, CardSize.Width, CardSize.Height);
                    GetImage(cardImage, src, decks);
                    System.Diagnostics.Debug.Print(" >>> Adding deck back image " + (i + 1));
                    ObjData.CardBacks.Add(cardImage);
                }
            }
        }

        private static void BuildAssets(string fileName)
        {
            using (var assets = (Bitmap)Image.FromFile(fileName))
            {
                for (var asset = 0; asset <= 3; asset++)
                {
                    var cardImage = new Bitmap(CardSize.Width, CardSize.Height);
                    var src = new Rectangle(asset * CardSize.Width, 0, CardSize.Width, CardSize.Height);
                    GetImage(cardImage, src, assets);
                    switch (asset)
                    {
                        case 0:
                            /* Set empty stock image */
                            System.Diagnostics.Debug.Print(" >> Set empty stock");
                            ObjData.EmptyStock = cardImage;
                            break;

                        case 1:
                            System.Diagnostics.Debug.Print(">> Set no redeal");
                            ObjData.NoRedeal = cardImage;
                            break;

                        case 2:
                            /* Set empty foundation image */
                            System.Diagnostics.Debug.Print(">> Set empty foundation");
                            ObjData.EmptyFoundation = cardImage;
                            break;

                        case 3:
                            /* Set empty tableau image */
                            System.Diagnostics.Debug.Print(">> Set empty tableau");
                            ObjData.EmptyTableau = cardImage;
                            break;
                    }
                }
            }
        }

        private static void BuildBackgrounds(string path)
        {
            /* Build data in order, images first, then colors */
            ObjData.Backgrounds.AddRange(
                new[]
                {
                    new BackgroundImageData
                    {
                        Name = "Green felt",
                        Image = Image.FromFile($@"{path}\bg_green_felt.jpg"),
                        ImageLayout = BackgroundImageDataLayout.Stretch
                    },
                    new BackgroundImageData
                    {
                        Name = "Red felt",
                        Image = Image.FromFile($@"{path}\bg_red_felt.jpg"),
                        ImageLayout = BackgroundImageDataLayout.Stretch
                    },
                    new BackgroundImageData
                    {
                        Name = "Blue felt",
                        Image = Image.FromFile($@"{path}\bg_blue_felt.jpg"),
                        ImageLayout = BackgroundImageDataLayout.Stretch
                    },
                    new BackgroundImageData
                    {
                        Name = "City scape",
                        Image = Image.FromFile($@"{path}\bg_city_scape.jpg"),
                        ImageLayout = BackgroundImageDataLayout.Stretch
                    },
                    new BackgroundImageData
                    {
                        Name = "Mountain Range",
                        Image = Image.FromFile($@"{path}\bg_mountain_range.jpg"),
                        ImageLayout = BackgroundImageDataLayout.Stretch
                    },
                    new BackgroundImageData
                    {
                        Name = "Wooden Floor Tiled",
                        Image = Image.FromFile($@"{path}\bg_wood_floor.png"),
                        ImageLayout = BackgroundImageDataLayout.Tile
                    },
                    new BackgroundImageData
                    {
                        Name = "Carpet Tiled",
                        Image = Image.FromFile($@"{path}\bg_carpet_tile.jpg"),
                        ImageLayout = BackgroundImageDataLayout.Tile
                    },
                    /* Colors */
                    new BackgroundImageData
                    {
                        Name = "Solitaire Green",
                        ImageLayout = BackgroundImageDataLayout.Color,
                        BackgroundColor = new List<Color>(new[]
                        {
                            Color.DarkGreen
                        })
                    },
                    new BackgroundImageData
                    {
                        Name = "Solitaire Dark Green Gradient",
                        ImageLayout = BackgroundImageDataLayout.Color,
                        BackgroundColor = new List<Color>(new[]
                        {
                            Color.Black,
                            Color.DarkGreen
                        })
                    },
                    new BackgroundImageData
                    {
                        Name = "Solitaire Dark Red Gradient",
                        ImageLayout = BackgroundImageDataLayout.Color,
                        BackgroundColor = new List<Color>(new[]
                        {
                            Color.Black,
                            Color.DarkRed
                        })
                    },
                    new BackgroundImageData
                    {
                        Name = "Solitaire Dark Blue Gradient",
                        ImageLayout = BackgroundImageDataLayout.Color,
                        BackgroundColor = new List<Color>(new[]
                        {
                            Color.Black,
                            Color.DarkBlue
                        })
                    },
                    new BackgroundImageData
                    {
                        Name = "Sky Blue Gradient",
                        ImageLayout = BackgroundImageDataLayout.Color,
                        BackgroundColor = new List<Color>(new[]
                        {
                            Color.DeepSkyBlue,
                            Color.SkyBlue
                        })
                    },
                });
        }

        private static void GetImage(Image cardBmp, Rectangle srcRegion, Bitmap srcBitmap)
        {
            using (var gfx = Graphics.FromImage(cardBmp))
            {
                gfx.DrawImage(srcBitmap, new Rectangle(0, 0, cardBmp.Width, cardBmp.Height), srcRegion, GraphicsUnit.Pixel);
            }
        }

        private static Image GenerateCardSetPreview(Cards cards)
        {
            /* We build an image of various cards on top of each other */
            var b = cards.Images[new KeyValuePair<Suit, int>(Suit.Clubs, 13)].Image;
            /* Get card size and make it 0.5 times smaller */
            var s = b.Size;
            var width = (int)(s.Width / 1.5);
            var height = (int)(s.Height / 1.5);
            var bmp = new Bitmap(width + 60, height + 30);
            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(b, 0, 0, width, height);
                b = cards.Images[new KeyValuePair<Suit, int>(Suit.Hearts, 12)].Image;
                g.DrawImage(b, 20, 10, width, height);
                b = cards.Images[new KeyValuePair<Suit, int>(Suit.Diamonds, 11)].Image;
                g.DrawImage(b, 40, 20, width, height);
                b = cards.Images[new KeyValuePair<Suit, int>(Suit.Spades, 1)].Image;
                g.DrawImage(b, 60, 30, width, height);
            }

            return bmp;
        }
    }
}
