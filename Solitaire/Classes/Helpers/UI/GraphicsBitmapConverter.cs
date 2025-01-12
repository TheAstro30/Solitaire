/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Solitaire.Classes.Helpers.UI
{
    public static class GraphicsBitmapConverter
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        private const uint SrcCopy = 0x00CC0020;
        
        /* Method to copy the contents of a hDC, such as a form to a bitmap */
        public static Bitmap GraphicsToBitmap(Graphics g, Rectangle bounds)
        {
            var bmp = new Bitmap(bounds.Width, bounds.Height);
            using (var bmpGrf = Graphics.FromImage(bmp))
            {
                var hdc1 = g.GetHdc();
                var hdc2 = bmpGrf.GetHdc();
                BitBlt(hdc2, 0, 0, bmp.Width, bmp.Height, hdc1, 0, 0, SrcCopy);
                /* Dispose */
                g.ReleaseHdc(hdc1);
                bmpGrf.ReleaseHdc(hdc2);
            }
            return bmp;
        }
    }
}
