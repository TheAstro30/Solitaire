﻿/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Xml.Serialization;
using Solitaire.Classes.Helpers;
using Solitaire.Classes.Settings.SettingsData;

namespace Solitaire.Classes.Settings
{
    /* TODO: Add statistics - longest game, shortest game (after games have been completed?), total number of games played, highest score */
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

        /* Stats and high scores */
        [XmlElement("stats")]
        public StatisticsData Statistics = new StatisticsData();

        [XmlElement("highScores")]
        public HighScoreData HighScores = new HighScoreData();

        /* Constructor */
        public Settings()
        {
            /* Set default settings */
            Size = new Size(720, 470);
        }
    }
}