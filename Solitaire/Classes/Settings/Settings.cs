/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Xml.Serialization;
using Solitaire.Classes.Serialization;
using Solitaire.Classes.Settings.SettingsData;

namespace Solitaire.Classes.Settings
{
    [Serializable, XmlRoot("settings")]
    public sealed class Settings
    {
        [XmlAttribute("location")]
        public string LocationString
        {
            get { return XmlFormatting.WritePointFormat(Location); }
            set { Location = XmlFormatting.ParsePointFormat(value); }
        }

        [XmlAttribute("size")]
        public string SizeString
        {
            get { return XmlFormatting.WriteSizeFormat(Size); }
            set { Size = XmlFormatting.ParseSizeFormat(value); }
        }

        [XmlAttribute("max")]
        public bool Maximized { get; set; }

        [XmlIgnore]
        public Point Location { get; set; }

        [XmlIgnore]
        public Size Size { get; set; }

        /* Game options */
        [XmlElement("options")]
        public GameOptionData Options = new GameOptionData();

        /* Stats and high scores */
        [XmlElement("stats")]
        public StatisticsData Statistics = new StatisticsData();

        /* Constructor */
        public Settings()
        {
            /* Set default settings */
            Size = new Size(720, 470);
            Options.PlaySounds = true;
            Options.SaveRecover = true;
            Options.ShowProgress = true;
            Options.Confirm.OnExit = true;
            Options.Confirm.OnNewLoad = true;
        }
    }
}
