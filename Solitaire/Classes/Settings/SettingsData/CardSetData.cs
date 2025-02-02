/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Xml.Serialization;

namespace Solitaire.Classes.Settings.SettingsData
{
    [Serializable]
    public class CardSetData
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("path")]
        public string FilePath { get; set; }
    }
}
