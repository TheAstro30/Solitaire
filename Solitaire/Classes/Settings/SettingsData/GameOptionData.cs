/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Xml.Serialization;

namespace Solitaire.Classes.Settings.SettingsData
{
    [Serializable]
    public sealed class ConfirmOptionData
    {
        [XmlAttribute("exit")]
        public bool OnExit { get; set; }

        [XmlAttribute("newload")]
        public bool OnNewLoad { get; set; }
    }

    [Serializable]
    public sealed class GameOptionData
    {
        [XmlAttribute("style")]
        public int AppearanceStyle { get; set; }

        [XmlAttribute("deckBack")]
        public int DeckBack { get; set; }

        [XmlAttribute("saveRecover")]
        public bool SaveRecover { get; set; }

        [XmlAttribute("showProgress")]
        public bool ShowProgress { get; set; }

        [XmlElement("sound")]
        public SoundData Sound = new SoundData();

        [XmlElement("confirm")]
        public ConfirmOptionData Confirm = new ConfirmOptionData();
    }
}
