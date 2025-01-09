/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Solitaire.Classes.Settings.SettingsData
{
    [Serializable]
    public class ScoreData
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("time")]
        public int Time { get; set; }

        [XmlAttribute("score")]
        public int Score { get; set; }
    }

    [Serializable]
    public sealed class HighScoreData
    {
        [XmlElement("data")]
        public List<ScoreData> Scores = new List<ScoreData>();
    }
}
