/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Xml.Serialization;
using Solitaire.Classes.Helpers;

namespace Solitaire.Classes.Settings
{
    [Serializable, XmlRoot("settings")]
    public sealed class Settings
    {
        [Serializable]
        public sealed class WindowData
        {
            [XmlAttribute("location")]
            public string LocationString
            {
                get { return XmlFormatting.WritePointFormat(Location); }
                set { Location = XmlFormatting.ParsePointFormat(value); }
            }

            [XmlAttribute("size")]
            public string SizeString
            {
                get { return XmlFormatting.WriteSizeFormat(Size); }
                set { Size = XmlFormatting.ParseSizeFormat(value); }
            }

            [XmlAttribute("max")]
            public bool Maximized { get; set; }

            [XmlIgnore]
            public Point Location { get; set; }

            [XmlIgnore]
            public Size Size { get; set; }
        }

        [XmlElement("window")]
        public WindowData Window { get; set; }

        public Settings()
        {
            /* Set default settings */
            Window = new WindowData
            {
                Size = new Size(720, 470)
            };
        }
    }
}
