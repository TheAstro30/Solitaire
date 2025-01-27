/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using libolv.Implementation;

namespace Solitaire.Classes.Settings.SettingsData
{
    [Serializable]
    public class SaveLoadData
    {
        [OlvIgnore, XmlAttribute("name")]
        public string FriendlyName { get; set; }

        [OlvIgnore, XmlAttribute("dateTime")]
        public string DateTime { get; set; }

        [XmlIgnore]
        public string FileName => $"{FriendlyName.Replace(" ", "_")}-{DateTime.Replace(" ", "_").Replace(",", "-")}.dat";

        public override string ToString()
        {
            return $@"{FriendlyName} - ({DateTime})";
        }
    }

    [Serializable]
    public class GameSaveLoadData
    {
        [XmlElement("data")]
        public List<SaveLoadData> Data = new List<SaveLoadData>();

        public void Add(SaveLoadData game)
        {
            /* Use this method directly, rather than Data.Add, as this checks for duplicates */
            if (Data.Any(g => g.FriendlyName.ToLower().Equals(game.FriendlyName.ToLower())))
            {
                return;
            }
            Data.Add(game);
        }
    }
}
