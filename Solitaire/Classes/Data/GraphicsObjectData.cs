/* Solitaire
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
        public Image Background { get; set; }

        public Image EmptyStock { get; set; }

        public Image EmptyFoundation { get; set; }

        public Image EmptyTableau { get; set; }

        public List<Image> CardBacks { get; set; }

        public Image ButtonOk { get; set; }

        public Image ButtonCancel { get; set; }

        public Image NewGameBackground { get; set; }

        public Image Logo { get; set; }

        public GraphicsObjectData()
        {
            CardBacks = new List<Image>();
        }
    }
}
