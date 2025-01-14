/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Solitaire.Classes.Helpers
{
    public static class MyExtensions
    {
        private static readonly Random Rng = new Random();

        /* List extension */
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /* Drawing extension */
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int cornerRadius)
        {
            using (var path = RoundedRect(bounds, cornerRadius))
            {
                graphics.DrawPath(pen, path);
            }
        }

        /* Private methods */
        internal static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var diameter = radius * 2;
            var size = new Size(diameter, diameter);
            var arc = new Rectangle(bounds.Location, size);
            var path = new GraphicsPath();
            /* Just return a rectangle if radius is 0 */
            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }
            /* Top left arc */  
            path.AddArc(arc, 180, 90);
            /* Top right arc */  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            /* Bottom right arc */  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            /* Bottom left arc */ 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            /* Close path and return result */
            path.CloseFigure();
            return path;
        }
    }
}
