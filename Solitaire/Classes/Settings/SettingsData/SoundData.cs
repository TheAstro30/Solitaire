/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Xml.Serialization;

namespace Solitaire.Classes.Settings.SettingsData
{
    [Serializable]
    public sealed class SoundData
    {
        [XmlAttribute("effects")]
        public bool EnableEffects { get; set; }

        [XmlAttribute("effectsVolume")]
        public int EffectsVolume { get; set; }

        [XmlAttribute("music")]
        public bool EnableMusic { get; set; }

        [XmlAttribute("musicVolume")]
        public int MusicVolume { get; set; }
    }
}
