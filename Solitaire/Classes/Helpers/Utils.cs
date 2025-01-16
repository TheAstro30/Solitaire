﻿/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
            return ts.Hours > 0
                ? string.Format("{0}h {1}m {2}s", ts.Hours, ts.Minutes, ts.Seconds)
                : ts.Minutes > 0
                    ? string.Format("{0}m {1}s", ts.Minutes, ts.Seconds)
                    : string.Format("{0}s", ts.Seconds);
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

    /* Undo move helper */    public static class Undo
    {
        private static readonly Stack<GameData> Data = new Stack<GameData>();

        public static int Count { get { return Data.Count; } }

        public static void AddMove(GameData data)
        {
            /* Add a move to undo history */
            var d = new GameData(data);
            Data.Push(d);
        }

        public static void RemoveLastEntry()
        {
            if (Data.Count == 0)
            {
                return;
            }
            Data.Pop();
        }

        public static GameData UndoLastMove()
        {
            return Data.Count == 0 ? null : Data.Pop();
        }

        public static GameData GetRestartPoint()
        {
            /* Not the most elegant way to do it... */
            for (var i = Data.Count - 1; i >= 0; i--)
            {
                var d = Data.Pop();
                if (i == 0)
                {
                    return d;
                }
            }
            return null;
        }

        public static void Clear()
        {
            Data.Clear();
        }
    }
}