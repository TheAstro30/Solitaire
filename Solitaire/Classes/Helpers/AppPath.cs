/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.IO;

namespace Solitaire.Classes.Helpers
{
    public static class AppPath
    {
        /* These variables are used especially on closing out with DVD - AccessViolation otherwise */
        private static readonly string BaseFolder = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

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
    }
}
