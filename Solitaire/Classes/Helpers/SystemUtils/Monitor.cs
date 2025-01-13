/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Linq;
using System.Windows.Forms;

namespace Solitaire.Classes.Helpers.SystemUtils
{
    public static class Monitor
    {
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
}
