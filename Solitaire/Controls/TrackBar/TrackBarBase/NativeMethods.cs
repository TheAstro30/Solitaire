/* xsMedia - xsTrackBar
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Solitaire.Controls.TrackBar.TrackBarBase
{
    internal static class NativeMethods
    {
        public const int NmCustomdraw = -12;
        public const int NmFirst = 0;
        public const int SOk = 0;
        public const int TmtColor = 0xcc;
        public const int WmErasebkgnd = 20;
        public const int WmNotify = 0x4e;
        public const int WmReflect = 0x2000;
        public const int WmUser = 0x400;
        
        public enum CustomDrawDrawStage
        {
            CddsItem = 0x10000,
            CddsItemposterase = 0x10004,
            CddsItempostpaint = 0x10002,
            CddsItempreerase = 0x10003,
            CddsItemprepaint = 0x10001,
            CddsPosterase = 4,
            CddsPostpaint = 2,
            CddsPreerase = 3,
            CddsPrepaint = 1,
            CddsSubitem = 0x20000
        }

        public enum CustomDrawItemState
        {
            CdisChecked = 8,
            CdisDefault = 0x20,
            CdisDisabled = 4,
            CdisFocus = 0x10,
            CdisGrayed = 2,
            CdisHot = 0x40,
            CdisIndeterminate = 0x100,
            CdisMarked = 0x80,
            CdisSelected = 1,
            CdisShowkeyboardcues = 0x200
        }

        public enum CustomDrawReturnFlags
        {
            CdrfDodefault = 0,
            CdrfNewfont = 2,
            CdrfNotifyitemdraw = 0x20,
            CdrfNotifyposterase = 0x40,
            CdrfNotifypostpaint = 0x10,
            CdrfNotifysubitemdraw = 0x20,
            CdrfSkipdefault = 4
        }

        public enum TrackBarCustomDrawPart
        {
            TbcdChannel = 3,
            TbcdThumb = 2,
            TbcdTics = 1
        }

        public enum TrackBarParts
        {
            TkpThumb = 3,
            TkpThumbbottom = 4,
            TkpThumbleft = 7,
            TkpThumbright = 8,
            TkpThumbtop = 5,
            TkpThumbvert = 6,
            TkpTics = 9,
            TkpTicsvert = 10,
            TkpTrack = 1,
            TkpTrackvert = 2
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DllVersionInfo
        {
            public int cbSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NmCustomDraw
        {
            public Nmhdr hdr;
            public CustomDrawDrawStage dwDrawStage;
            public IntPtr hdc;
            public Rect rc;
            public IntPtr dwItemSpec;
            public CustomDrawItemState uItemState;
            public IntPtr lItemlParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Nmhdr
        {
            public IntPtr HWND;
            public IntPtr idFrom;
            public int code;

            public override string ToString()
            {
                return string.Format(CultureInfo.InvariantCulture, "Hwnd: {0}, ControlID: {1}, Code: {2}", HWND, idFrom, code);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public Rect(Rectangle rect)
            {
                this = new Rect
                {
                    Left = rect.Left,
                    Top = rect.Top,
                    Right = rect.Right,
                    Bottom = rect.Bottom
                };
            }

            public override string ToString()
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}, {3}", Left, Top, Right, Bottom);
            }

            public Rectangle ToRectangle()
            {
                return Rectangle.FromLTRB(Left, Top, Right, Bottom);
            }
        }

        /* API */
        [DllImport("UxTheme.dll")]
        public static extern int CloseThemeData(IntPtr hTheme);

        [DllImport("UxTheme.dll")]
        public static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref Rect pRect, ref Rect pClipRect);

        [DllImport("UxTheme.dll")]
        public static extern int GetThemeColor(IntPtr hTheme, int iPartId, int iStateId, int iPropId, ref int pColor);
        
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("UxTheme.dll")]
        public static extern bool IsAppThemed();
        
        [DllImport("UxTheme.dll", CharSet=CharSet.Unicode)]
        public static extern IntPtr OpenThemeData(IntPtr hwnd, string pszClassList);
    }
}

