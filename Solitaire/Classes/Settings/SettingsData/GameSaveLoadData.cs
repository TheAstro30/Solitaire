/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
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

        public override string ToString()
        {
            return $@"{FriendlyName}\r\n({DateTime})";
        }
    }

    [Serializable]
    public class GameSaveLoadData
    {
        [XmlElement("data")]
        public List<SaveLoadData> Data = new List<SaveLoadData>();
    }
}
