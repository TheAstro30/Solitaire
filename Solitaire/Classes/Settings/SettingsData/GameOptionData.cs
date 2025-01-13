/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Xml.Serialization;

namespace Solitaire.Classes.Settings.SettingsData
{
    [Serializable]
    public sealed class GameOptionData
    {
        [XmlAttribute("deckBack")]
        public int DeckBack { get; set; }

        [XmlAttribute("drawThree")]
        public bool DrawThree { get; set; }

        [XmlAttribute("sound")]
        public bool PlaySounds { get; set; }
    }
}
