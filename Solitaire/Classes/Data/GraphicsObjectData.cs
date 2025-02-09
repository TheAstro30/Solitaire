﻿/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Solitaire.Classes.Data
{
    [Serializable]
    public sealed class GraphicsObjectData
    {
        public List<BackgroundImageData> Backgrounds { get; set; }

        public Image EmptyStock { get; set; }

        public Image NoRedeal { get; set; }

        public Image EmptyFoundation { get; set; }

        public Image EmptyTableau { get; set; }

        public List<Image> CardBacks { get; set; }

        public GraphicsObjectData()
        {
            Backgrounds = new List<BackgroundImageData>();
            CardBacks = new List<Image>();
        }
    }
}
