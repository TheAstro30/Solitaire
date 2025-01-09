/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.IO;
using Solitaire.Classes.Serialization;
using Solitaire.Classes.Settings.SettingsData;

namespace Solitaire.Classes.Helpers
{
    public static class SettingsManager
    {
        private const string FilePath = @"\KangaSoft\Solitaire\settings.xml";

        public static Settings.Settings Settings { get; set; }

        static SettingsManager()
        {
            Settings = new Settings.Settings();
        }

        public static void Load()
        {
            var file = AppPath.MainDir(FilePath, true);
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
            XmlSerialize<Settings.Settings>.Save(AppPath.MainDir(FilePath, true), Settings);
        }

        /* High scores */
        public static void AddHighScore(string name, int time, int score)
        {
            /* The idea of this is to be at the top of the list, faster than the last - hold only 10 */
            var s = new ScoreData
            {
                Name = name,
                Time = time,
                Score = score
            };
            Settings.HighScores.Scores.Add(s);
            if (Settings.HighScores.Scores.Count - 1 > 9)
            {
                Settings.HighScores.Scores.RemoveAt(Settings.HighScores.Scores.Count - 1);
            }
        }

        public static bool CheckHighScore(int time)
        {
            foreach (var s in Settings.HighScores.Scores)
            {
                if (s.Time < time)
                {
                    return true; /* ? desired operation ? */
                }
            }
            return false;
        }
    }
}
