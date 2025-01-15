/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.IO;
using Solitaire.Classes.Serialization;

namespace Solitaire.Classes.Helpers.Management
{
    public static class SettingsManager
    {
        /* It's worse than that, he's DEAD Jim! */
        private const string FilePath = @"\KangaSoft\Solitaire\settings.xml";

        public static Settings.Settings Settings { get; set; }

        static SettingsManager()
        {
            Settings = new Settings.Settings();
        }

        public static void Load()
        {
            var file = Utils.MainDir(FilePath, true);
            if (!File.Exists(file))
            {
                XmlSerialize<Settings.Settings>.Save(file, Settings);
                return;
            }
            var s = new Settings.Settings();
            if (XmlSerialize<Settings.Settings>.Load(file, ref s))
            {
                Settings = s;
            }
        }

        public static void Save()
        {
            XmlSerialize<Settings.Settings>.Save(Utils.MainDir(FilePath, true), Settings);
        }

        /* Game statistics */
        public static void UpdateStats(int seconds, int score)
        {
            if (Settings.Statistics.ShortestGameTime == 0 || seconds < Settings.Statistics.ShortestGameTime)
            {
                Settings.Statistics.ShortestGameTime = seconds;
            }
            if (Settings.Statistics.LongestGameTime == 0 || seconds > Settings.Statistics.LongestGameTime)
            {
                Settings.Statistics.LongestGameTime = seconds;
            }
            if (score > Settings.Statistics.HighestScore)
            {
                Settings.Statistics.HighestScore = score;
            }
        }
    }
}
