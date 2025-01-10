/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;

namespace Solitaire.Classes.Data
{
    [Serializable]
    public sealed class GraphicsObjectData
    {
        public Image Background { get; set; }

        public Image CardBack { get; set; }

        public Image EmptyDeck { get; set; }

        public Image HomeStack { get; set; }

        //todo add highlight box image data
    }
}
