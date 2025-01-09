/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Xml.Serialization;

namespace Solitaire.Classes.Settings.SettingsData
{
    [Serializable]
    public sealed class StatisticsData
    {
        [XmlAttribute("totalGames")]
        public int TotalGamesPlayed { get; set; }

        [XmlAttribute("gamesWon")]
        public int GamesWon { get; set; }

        [XmlAttribute("gamesLost")]
        public int GamesLost { get; set; }

        [XmlAttribute("longest")]
        public int LongestGameTime { get; set; }

        [XmlAttribute("shortest")]
        public int ShortestGameTime { get; set; }

        [XmlAttribute("highestScore")]
        public int HighestScore { get; set; }
    }
}
