/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.IO;
using Solitaire.Classes.Helpers;
using Solitaire.Classes.Serialization;

namespace Solitaire.Classes.Settings
{
    public static class SettingsManager
    {
        private const string FilePath = @"\KangaSoft\Solitaire\settings.xml";

        public static Settings Settings { get; set; }

        static SettingsManager()
        {
            Settings = new Settings();
        }

        public static void Load()
        {
            var file = AppPath.MainDir(FilePath, true);
            if (!File.Exists(file))
            {
                XmlSerialize<Settings>.Save(file, Settings);
                return;
            }
            var s = new Settings();
            if (XmlSerialize<Settings>.Load(file, ref s))
            {
                Settings = s;
            }
        }

        public static void Save()
        {
            XmlSerialize<Settings>.Save(AppPath.MainDir(FilePath, true), Settings);
        }
    }
}
