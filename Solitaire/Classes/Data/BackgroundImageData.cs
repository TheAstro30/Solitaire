/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Drawing;
using libolv.Implementation;

namespace Solitaire.Classes.Data
{
    public enum BackgroundImageDataLayout
    {
        Tile = 0,
        Stretch = 1,
        Color = 2
    }

    /* This class allows us to have multiple background images to choose from, including color gradients */
    [Serializable]
    public sealed class BackgroundImageData
    {
        public string Name { get; set; }

        [OlvIgnore]
        public Image Image { get; set; }

        [OlvIgnore]
        public BackgroundImageDataLayout ImageLayout { get; set; }

        public List<Color> BackgroundColor = new List<Color>();

        public override string ToString()
        {
            return Name;
        }
    }
}
