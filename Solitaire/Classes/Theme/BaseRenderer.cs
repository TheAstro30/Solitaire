﻿/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Windows.Forms;
using Solitaire.Classes.Theme.ColorTables;

namespace Solitaire.Classes.Theme
{
    public class BaseRenderer : ToolStripProfessionalRenderer
    {
        private static readonly CustomizableColorTable CustomColorTable = new CustomizableColorTable();

        public BaseRenderer() : base(CustomColorTable)
        {
            /* Empty by default */
        }

        public void RefreshToolStrips()
        {
            var old = ToolStripManager.Renderer;
            if (old == null || ToolStripManager.RenderMode != ToolStripManagerRenderMode.Custom)
            {
                return;
            }
            ToolStripManager.RenderMode = ToolStripManagerRenderMode.Professional;
            ToolStripManager.Renderer = old;
        }
     
        public new CustomizableColorTable ColorTable => CustomColorTable;

        #region Rendering Improvements (includes fixes for bugs occured when Windows Classic theme is on).
        /*
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            // Do not draw disabled item background.
            if (e.Item.Enabled)
            {
                bool isMenuDropDown = e.Item.Owner is MenuStrip;
                if (isMenuDropDown && e.Item.Pressed)
                {
                    base.OnRenderMenuItemBackground(e);
                }
                else if (e.Item.Selected)
                {
                    // Rect of item's content area.
                    Rectangle contentRect = e.Item.ContentRectangle;

                    // Fix item rect.
                    Rectangle itemRect = isMenuDropDown
                                             ? new Rectangle(
                                                   contentRect.X + 2, contentRect.Y - 2,
                                                   contentRect.Width - 5, contentRect.Height + 3)
                                             : new Rectangle(
                                                   contentRect.X, contentRect.Y - 1,
                                                   contentRect.Width, contentRect.Height + 1);

                    // Border pen and fill brush.
                    Color pen = ColorTable.MenuItemBorder;
                    Color brushBegin;
                    Color brushEnd;

                    if (isMenuDropDown)
                    {
                        brushBegin = ColorTable.MenuItemSelectedGradientBegin;
                        brushEnd = ColorTable.MenuItemSelectedGradientEnd;
                    }
                    else
                    {
                        brushBegin = ColorTable.MenuItemSelected;
                        brushEnd = Color.Empty;
                    }

                    DrawRectangle(e.Graphics, itemRect, brushBegin, brushEnd, pen, true);
                }
            }
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripButton button = e.Item as ToolStripButton;
            if (button != null && button.Enabled)
            {
                if (button.Selected || button.Checked)
                {
                    // Rect of item's content area.
                    Rectangle contentRect = new Rectangle(0, 0, button.Width - 1, button.Height - 1);

                    Color pen;
                    Color brushBegin;
                    Color brushMiddle;
                    Color brushEnd;

                    if (button.Checked)
                    {
                        pen = ColorTable.ButtonPressedBorder;
                        brushBegin = ColorTable.ButtonCheckedGradientBegin;
                        brushMiddle = ColorTable.ButtonCheckedGradientMiddle;
                        brushEnd = ColorTable.ButtonCheckedGradientEnd;
                    }
                    else if (button.Pressed)
                    {
                        pen = ColorTable.ButtonPressedBorder;
                        brushBegin = ColorTable.ButtonPressedGradientBegin;
                        brushMiddle = ColorTable.ButtonPressedGradientMiddle;
                        brushEnd = ColorTable.ButtonPressedGradientEnd;
                    }
                    else
                    {
                        pen = ColorTable.ButtonSelectedBorder;
                        brushBegin = ColorTable.ButtonSelectedGradientBegin;
                        brushMiddle = ColorTable.ButtonSelectedGradientMiddle;
                        brushEnd = ColorTable.ButtonSelectedGradientEnd;
                    }

                    DrawRectangle(e.Graphics, contentRect, 
                        brushBegin, brushMiddle, brushEnd, pen, false);
                }
            }
            else
            {
                base.OnRenderButtonBackground(e);
            }
        }

        private static void DrawRectangle(Graphics graphics, Rectangle rect, Color brushBegin, 
            Color brushMiddle, Color brushEnd, Color penColor, bool glass)
        {
            RectangleF firstHalf = new RectangleF(
                rect.X, rect.Y, 
                rect.Width, (float)rect.Height / 2);

            RectangleF secondHalf = new RectangleF(
                rect.X, rect.Y + (float)rect.Height / 2, 
                rect.Width, (float)rect.Height / 2);

            if (brushMiddle.IsEmpty && brushEnd.IsEmpty)
            {
                graphics.FillRectangle(new SolidBrush(brushBegin), rect);
            }
            if (brushMiddle.IsEmpty)
            {
                Brush gradient = new LinearGradientBrush(rect, brushBegin,
                    brushEnd, LinearGradientMode.Vertical);

                graphics.FillRectangle(gradient, rect);
            }
            else
            {
                Brush first = new LinearGradientBrush(
                    firstHalf, brushBegin, brushMiddle, LinearGradientMode.Vertical);
                Brush second = new LinearGradientBrush(
                    secondHalf, brushMiddle, brushEnd, LinearGradientMode.Vertical);

                graphics.FillRectangle(first, firstHalf);
                graphics.FillRectangle(second, secondHalf);
            }

            if (glass)
            {
                Brush glassBrush = new SolidBrush(Color.FromArgb(120, Color.White));
                graphics.FillRectangle(glassBrush, firstHalf);
            }

            if (penColor.A > 0)
            {
                graphics.DrawRectangle(new Pen(penColor), rect);
            }
        }

        private static void DrawRectangle(Graphics graphics, Rectangle rect, Color brushBegin,
            Color brushEnd, Color penColor, bool glass)
        {
            DrawRectangle(graphics, rect, brushBegin, Color.Empty, brushEnd, penColor, glass);
        }

        private static void DrawRectangle(Graphics graphics, Rectangle rect, Color brush, 
            Color penColor, bool glass)
        {
            DrawRectangle(graphics, rect, brush, Color.Empty, Color.Empty, penColor, glass);
        }

        private static void FillRoundRectangle(Graphics graphics, Brush brush, Rectangle rect, int radius)
        {
            float fx = Convert.ToSingle(rect.X);
            float fy = Convert.ToSingle(rect.Y);
            float fwidth = Convert.ToSingle(rect.Width);
            float fheight = Convert.ToSingle(rect.Height);
            float fradius = Convert.ToSingle(radius);
            FillRoundRectangle(graphics, brush, fx, fy, fwidth, fheight, fradius);
        }

        private static void FillRoundRectangle(Graphics graphics, Brush brush, float x, float y, float width, float height, float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = GetRoundedRect(rectangle, radius);
            graphics.FillPath(brush, path);
        }

        private static void DrawRoundRectangle(Graphics graphics, Pen pen, Rectangle rect, int radius)
        {
            float fx = Convert.ToSingle(rect.X);
            float fy = Convert.ToSingle(rect.Y);
            float fwidth = Convert.ToSingle(rect.Width);
            float fheight = Convert.ToSingle(rect.Height);
            float fradius = Convert.ToSingle(radius);
            DrawRoundRectangle(graphics, pen, fx, fy, fwidth, fheight, fradius);
        }

        private static void DrawRoundRectangle(Graphics graphics, Pen pen, float x, float y, float width, float height, float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = GetRoundedRect(rectangle, radius);
            graphics.DrawPath(pen, path);
        }

        private static GraphicsPath GetRoundedRect(RectangleF baseRect, float radius)
        {
            // if corner radius is less than or equal to zero, 
            // return the original rectangle 

            if (radius <= 0)
            {
                GraphicsPath mPath = new GraphicsPath();
                mPath.AddRectangle(baseRect);
                mPath.CloseFigure();
                return mPath;
            }

            // if the corner radius is greater than or equal to 
            // half the width, or height (whichever is shorter) 
            // then return a capsule instead of a lozenge 

            if (radius >= (Math.Min(baseRect.Width, baseRect.Height)) / 2.0)
                return GetCapsule(baseRect);

            // create the arc for the rectangle sides and declare 
            // a graphics path object for the drawing 

            float diameter = radius * 2.0F;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            GraphicsPath path = new GraphicsPath();

            // top left arc 
            path.AddArc(arc, 180, 90);

            // top right arc 
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc 
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private static GraphicsPath GetCapsule(RectangleF baseRect)
        {
            RectangleF arc;
            GraphicsPath path = new GraphicsPath();

            try
            {
                float diameter;
                if (baseRect.Width > baseRect.Height)
                {
                    // return horizontal capsule 
                    diameter = baseRect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    // return vertical capsule 
                    diameter = baseRect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else
                {
                    // return circle 
                    path.AddEllipse(baseRect);
                }
            }
            catch
            {
                path.AddEllipse(baseRect);
            }
            finally
            {
                path.CloseFigure();
            }

            return path;
        } 
        */
        #endregion
    }
}
