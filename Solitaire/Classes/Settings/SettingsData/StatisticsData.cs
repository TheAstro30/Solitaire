/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Xml.Serialization;
using Solitaire.Classes.Helpers;

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

        public override string ToString()
        {
            //var games = TotalGamesPlayed - 1;
            var winPercent = TotalGamesPlayed != 0 ? Math.Round(((float)GamesWon / TotalGamesPlayed) * 100, 1) : 0;
            var lossPercent = TotalGamesPlayed != 0 ? Math.Round(((float)GamesLost / TotalGamesPlayed) * 100, 1) : 0;
            return
                $"Total games played: {TotalGamesPlayed}\r\n\r\nGames won: {GamesWon} ({winPercent}% win rate)\r\n\r\nGames lost: {GamesLost} ({lossPercent}% loss rate)\r\n\r\nShortest game time: {Utils.FormatTime(ShortestGameTime)}\r\n\r\nLongest game time: {Utils.FormatTime(LongestGameTime)}\r\n\r\nHighest score: {HighestScore}";
        }
    }
}
