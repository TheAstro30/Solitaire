﻿/* Object List View
 * Copyright (C) 2006-2012 Phillip Piper
 * Refactored by Jason James Newland - 2014/January 2025; C# v7.0
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * If you wish to use this code in a closed source application, please contact phillip_piper@bigfoot.com.
 */

using System.Drawing;
using System.Drawing.Drawing2D;

namespace Solitaire.Controls.ObjectListView.Rendering.Decoration
{
    public class BorderDecoration : AbstractDecoration
    {
        public BorderDecoration() : this(new Pen(Color.FromArgb(64, Color.Blue), 1))
        {
            /* Empty */
        }

        public BorderDecoration(Pen borderPen)
        {
            BorderPen = borderPen;
        }

        public BorderDecoration(Pen borderPen, Brush fill)
        {
            BorderPen = borderPen;
            FillBrush = fill;
        }

        /* Properties */
        public Pen BorderPen { get; set; }

        public Size BoundsPadding { get; set; } = new Size(-1, 2);

        public float CornerRounding { get; set; } = 16.0f;

        public Brush FillBrush { get; set; } = new SolidBrush(Color.FromArgb(64, Color.Blue));

        public Color? FillGradientFrom { get; set; }
        public Color? FillGradientTo { get; set; }

        public LinearGradientMode FillGradientMode { get; set; } = LinearGradientMode.Vertical;

        /* IOverlay Members */
        public override void Draw(Solitaire.Controls.ObjectListView.ObjectListView olv, Graphics g, Rectangle r)
        {
            var bounds = CalculateBounds();
            if (!bounds.IsEmpty)
            {
                DrawFilledBorder(g, bounds);
            }
        }
        /* Subclass responsibility */
        protected virtual Rectangle CalculateBounds()
        {
            return Rectangle.Empty;
        }

        /* Implementation utlities */
        protected void DrawFilledBorder(Graphics g, Rectangle bounds)
        {
            bounds.Inflate(BoundsPadding);
            var path = GetRoundedRect(bounds, CornerRounding);
            if (FillGradientFrom != null && FillGradientTo != null)
            {
                FillBrush?.Dispose();
                FillBrush = new LinearGradientBrush(bounds, FillGradientFrom.Value, FillGradientTo.Value, FillGradientMode);
            }
            if (FillBrush != null)
            {
                g.FillPath(FillBrush, path);
            }
            if (BorderPen != null)
            {
                g.DrawPath(BorderPen, path);
            }
        }

        protected GraphicsPath GetRoundedRect(RectangleF rect, float diameter)
        {
            var path = new GraphicsPath();
            if (diameter <= 0.0f)
            {
                path.AddRectangle(rect);
            }
            else
            {
                var arc = new RectangleF(rect.X, rect.Y, diameter, diameter);
                path.AddArc(arc, 180, 90);
                arc.X = rect.Right - diameter;
                path.AddArc(arc, 270, 90);
                arc.Y = rect.Bottom - diameter;
                path.AddArc(arc, 0, 90);
                arc.X = rect.Left;
                path.AddArc(arc, 90, 90);
                path.CloseFigure();
            }
            return path;
        }
    }
}
