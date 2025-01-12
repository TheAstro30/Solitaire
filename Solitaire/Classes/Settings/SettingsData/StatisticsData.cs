/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Text;
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

        public override string ToString()
        {
            var winPercent = Math.Round(((float) GamesWon/GamesLost)*100, 1);
            return
                string.Format(
                    "Total games played: {0}\r\n\r\nGames won: {1} ({2}% win rate)\r\n\r\nGames lost: {3} ({4}% loss rate)\r\n\r\nShortest game time: {5}\r\n\r\nLongest game time: {6}\r\n\r\nHighest score: {7}",
                    TotalGamesPlayed, GamesWon, winPercent, GamesLost,
                    Math.Round(100 - winPercent, 1),
                    FormatTime(ShortestGameTime),
                    FormatTime(LongestGameTime),
                    HighestScore);
        }

        private static string FormatTime(int secs)
        {
            var ts = new TimeSpan(0, 0, 0, secs);
            return string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
        }
    }
}
