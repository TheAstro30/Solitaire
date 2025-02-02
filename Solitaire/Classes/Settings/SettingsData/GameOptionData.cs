/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Solitaire.Classes.Settings.SettingsData
{
    [Serializable]
    public enum DifficultyLevel
    {
        [Description("Easy")] /* Unlimited redeals */
        Easy = 0,

        [Description("Medium")] /* Limit redeals to 3 */
        Medium = 1,

        [Description("Hard")] /* Limit redeals to 0 */
        Hard = 2
    }

    [Serializable]
    public sealed class ConfirmOptionData
    {
        [XmlAttribute("exit")]
        public bool OnExit { get; set; }

        [XmlAttribute("newLoad")]
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

        [XmlAttribute("autoTurn")]
        public bool AutoTurn { get; set; }

        [XmlAttribute("showProgress")]
        public bool ShowProgress { get; set; }

        [XmlAttribute("showHighlight")]
        public bool ShowHighlight { get; set; }

        [XmlAttribute("showTips")]
        public bool ShowTips { get; set; }

        [XmlAttribute("difficulty")]
        public DifficultyLevel Difficulty { get; set; }

        [XmlElement("cardSet")]
        public CardSetData CardSet = new CardSetData();

        [XmlElement("sound")]
        public SoundData Sound = new SoundData();

        [XmlElement("confirm")]
        public ConfirmOptionData Confirm = new ConfirmOptionData();
    }
}
