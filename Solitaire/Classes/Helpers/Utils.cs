﻿/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using Solitaire.Classes.Data;

namespace Solitaire.Classes.Helpers
{
    public static class Utils
    {
        private static readonly string BaseFolder = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string FormatTime(int seconds)
        {
            var ts = new TimeSpan(0, 0, 0, seconds);
            return ts.Hours > 0 ? $"{ts.Hours}h {ts.Minutes}m {ts.Seconds}s" :
                ts.Minutes > 0 ? $"{ts.Minutes}m {ts.Seconds}s" : $"{ts.Seconds}s";
        }

        /* Main application folder */
        public static string MainDir(string path)
        {
            /* Default is not to return application data roaming profile path */
            return MainDir(path, false);
        }

        public static string MainDir(string path, bool forceApplicationDataPath)
        {
            var folder = forceApplicationDataPath ? UserFolder : BaseFolder;
            if (string.IsNullOrEmpty(path))
            {
                /* Failed */
                return folder;
            }
            if (!forceApplicationDataPath && path.ToLower().Contains(folder.ToLower()))
            {
                /* Remove app path */
                return @"\" + path.Replace(folder, null);
            }
            var currentPath = path.Substring(0, 1) == @"\" ? (folder + path).Replace(@"\\", @"\") : path.Replace(@"\\", @"\");
            var pathOnly = Path.GetDirectoryName(currentPath);
            if (forceApplicationDataPath && !string.IsNullOrEmpty(pathOnly) && pathOnly.ToLower() != folder.ToLower() && !Directory.Exists(pathOnly))
            {
                /* If the directory doesn't exists, create it */
                Directory.CreateDirectory(pathOnly);
            }
            return currentPath;
        }

        /* Quick util for getting the current monitor the application is running on */
        public static Screen GetCurrentMonitor(Form wnd)
        {
            foreach (var s in Screen.AllScreens.Where(s => s.Bounds.Contains(wnd.Bounds)))
            {
                return s;
            }
            return Screen.PrimaryScreen;
        }

        /* Quick method for deleting data files */
        public static void DeleteFile(string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    /* Delete current recovery file */
                    File.Delete(file);
                }
            }
            catch (Exception)
            {
                /* Silently ignore any file IO exceptions */
                Debug.Assert(true);
            }
        }

        public static string GetDescriptionFromEnumValue(Enum value)
        {
            var attribute = value.GetType()
                                .GetField(value.ToString())
                                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }

    /* Menu building helper */
    public static class MenuHelper
    {
        public static ToolStripMenuItem AddMenuItem(string text)
        {
            return AddMenuItem(text, null, Keys.None, true, false, null, null);
        }

        public static ToolStripMenuItem AddMenuItem(string text, Image image)
        {
            return AddMenuItem(text, null, Keys.None, true, false, image, null);
        }

        public static ToolStripMenuItem AddMenuItem(string text, string key, EventHandler callBack)
        {
            return AddMenuItem(text, key, Keys.None, true, false, null, callBack);
        }

        public static ToolStripMenuItem AddMenuItem(string text, string key, Keys shortCutKeys, EventHandler callBack)
        {
            return AddMenuItem(text, key, shortCutKeys, true, false, null, callBack);
        }

        public static ToolStripMenuItem AddMenuItem(string text, string key, Keys shortCutKeys, bool enabled, EventHandler callBack)
        {
            return AddMenuItem(text, key, shortCutKeys, enabled, false, null, callBack);
        }

        public static ToolStripMenuItem AddMenuItem(string text, string key, Keys shortCutKeys, bool enabled, bool bChecked, Image image, EventHandler callBack)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            var m = new ToolStripMenuItem(text, image, callBack)
            {
                ShortcutKeys = shortCutKeys,
                Tag = key,
                Enabled = enabled,
                Checked = bChecked
            };
            return m;
        }
    }

    /* Undo move helper */
    public static class Undo
    {
        /* This class was a memory hog as it copies ALL card data including the images;
         * I re-wrote the Card.cs class and separated the card images to a new class CardData.
         * This GREATLY reduces the memory footprint (I was over 500MB!!), but we still want to
         * keep the amount we store conservitive. 1000 should be enough! */
        private static readonly List<GameData> UndoData = new List<GameData>();

        private static readonly List<GameData> RedoData = new List<GameData>();

        public static int UndoCount => UndoData.Count;

        public static int RedoCount => RedoData.Count;

        public static void AddMove(GameData data)
        {
            /* Clear redo history */
            RedoData.Clear();
            /* Add a move to undo history */
            var d = new GameData(data) {RestartPoint = new GameData(data.RestartPoint)}; /* Forgot to renew restart point */
            UndoData.Add(d);
        }

        public static void RemoveLastEntry()
        {
            /* A card was moved nowhere, and returned to where it came from - no point keeping the last move
             * recorded on MouseDown of Game.cs */
            if (UndoData.Count == 0)
            {
                return;
            }
            UndoData.RemoveAt(UndoData.Count - 1);
        }

        public static GameData UndoLastMove(GameData data)
        {
            /* Get last entry, remove it from undo list and return result */
            if (UndoData.Count == 0)
            {
                return null;
            }
            var d = UndoData[UndoData.Count - 1];
            /* Push this to RedoData */
            RedoData.Add(new GameData(data));
            UndoData.Remove(d);
            return d;
        }

        public static GameData RedoMove()
        {
            if (RedoCount == 0)
            {
                return null;
            }
            var d = RedoData[RedoData.Count - 1];
            /* Push back to undo */
            UndoData.Add(d);
            RedoData.Remove(d);
            return d;
        }

        public static void Clear()
        {
            /* Obvious */
            UndoData.Clear();
            RedoData.Clear();
        }
    }
}
