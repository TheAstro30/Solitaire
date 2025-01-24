/* xsMedia - xsTrackBar
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using Solitaire.Controls.TrackBar.TrackBarBase;

namespace Solitaire.Controls.TrackBar
{
    [ToolboxBitmap(typeof(System.Windows.Forms.TrackBar))]
    public class TrackBarEx : System.Windows.Forms.TrackBar
    {
        private Rectangle _channelBounds;
        private Rectangle _thumbBounds;
        private int _thumbState;

        public event TrackBarDrawItemEventHandler DrawChannel;
        public event TrackBarDrawItemEventHandler DrawThumb;
        public event TrackBarDrawItemEventHandler DrawTicks;

        public TrackBarEx()
        {
            OwnerDrawParts = TrackBarOwnerDrawParts.None;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        /* Properties */
        [DefaultValue(typeof(TrackBarOwnerDrawParts), "None"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Editor(typeof(TrackDrawModeEditor), typeof(UITypeEditor)), Description("Gets/sets the trackbar parts that will be OwnerDrawn.")]
        public TrackBarOwnerDrawParts OwnerDrawParts { get; set; }

        private static bool VisualStylesEnabled => Application.RenderWithVisualStyles;

        /* Private control methods */
        private void DrawHorizontalTicks(Graphics g, Color color)
        {
            RectangleF ef3;
            var num = ((Maximum - Minimum) / TickFrequency) - 1;
            var pen = new Pen(color);
            var ef2 = new RectangleF(_channelBounds.Left + (_thumbBounds.Width / 2), _thumbBounds.Top - 5, 0f, 3f);
            var ef = new RectangleF((_channelBounds.Right - (_thumbBounds.Width / 2)) - 1, _thumbBounds.Top - 5, 0f, 3f);
            var x = (ef.Right - ef2.Left) / (num + 1);
            if (TickStyle != TickStyle.BottomRight)
            {
                g.DrawLine(pen, ef2.Left, ef2.Top, ef2.Right, ef2.Bottom);
                g.DrawLine(pen, ef.Left, ef.Top, ef.Right, ef.Bottom);
                ef3 = ef2;
                ef3.Height--;
                ef3.Offset(x, 1f);
                var num5 = num - 1;
                for (var i = 0; i <= num5; i++)
                {
                    g.DrawLine(pen, ef3.Left, ef3.Top, ef3.Right, ef3.Bottom);
                    ef3.Offset(x, 0f);
                }
            }
            ef2.Offset(0f, _thumbBounds.Height + 6);
            ef.Offset(0f, _thumbBounds.Height + 6);
            if (TickStyle != TickStyle.TopLeft)
            {
                g.DrawLine(pen, ef2.Left, ef2.Top, ef2.Right, ef2.Bottom);
                g.DrawLine(pen, ef.Left, ef.Top, ef.Right, ef.Bottom);
                ef3 = ef2;
                ef3.Height--;
                ef3.Offset(x, 0f);
                var num6 = num - 1;
                for (var j = 0; j <= num6; j++)
                {
                    g.DrawLine(pen, ef3.Left, ef3.Top, ef3.Right, ef3.Bottom);
                    ef3.Offset(x, 0f);
                }
            }
            pen.Dispose();
        }

        private void DrawPointerDown(Graphics g)
        {
            var pointArray = new[]
                                 {
                                     new Point(_thumbBounds.Left + (_thumbBounds.Width/2), _thumbBounds.Bottom - 1),
                                     new Point(_thumbBounds.Left,
                                               (_thumbBounds.Bottom - (_thumbBounds.Width/2)) - 1),
                                     _thumbBounds.Location, new Point(_thumbBounds.Right - 1, _thumbBounds.Top),
                                     new Point(_thumbBounds.Right - 1,
                                               (_thumbBounds.Bottom - (_thumbBounds.Width/2)) - 1),
                                     new Point(_thumbBounds.Left + (_thumbBounds.Width/2), _thumbBounds.Bottom - 1)
                                 };
            var path = new GraphicsPath();
            path.AddLines(pointArray);
            var region = new Region(path);
            g.Clip = region;
            if ((_thumbState == 3) || !Enabled)
            {
                ControlPaint.DrawButton(g, _thumbBounds, ButtonState.All);
            }
            else
            {
                g.Clear(SystemColors.Control);
            }
            g.ResetClip();
            region.Dispose();
            path.Dispose();
            var points = new[] {pointArray[0], pointArray[1], pointArray[2], pointArray[3]};
            g.DrawLines(SystemPens.ControlLightLight, points);
            points = new[] { pointArray[3], pointArray[4], pointArray[5] };
            g.DrawLines(SystemPens.ControlDarkDark, points);
            pointArray[0].Offset(0, -1);
            pointArray[1].Offset(1, 0);
            pointArray[2].Offset(1, 1);
            pointArray[3].Offset(-1, 1);
            pointArray[4].Offset(-1, 0);
            pointArray[5] = pointArray[0];
            points = new[] { pointArray[0], pointArray[1], pointArray[2], pointArray[3] };
            g.DrawLines(SystemPens.ControlLight, points);
            points = new[] { pointArray[3], pointArray[4], pointArray[5] };
            g.DrawLines(SystemPens.ControlDark, points);
        }

        private void DrawPointerLeft(Graphics g)
        {
            var pointArray = new[]
                                 {
                                     new Point(_thumbBounds.Left, _thumbBounds.Top + (_thumbBounds.Height/2)),
                                     new Point(_thumbBounds.Left + (_thumbBounds.Height/2), _thumbBounds.Top),
                                     new Point(_thumbBounds.Right - 1, _thumbBounds.Top),
                                     new Point(_thumbBounds.Right - 1, _thumbBounds.Bottom - 1),
                                     new Point(_thumbBounds.Left + (_thumbBounds.Height/2),
                                               _thumbBounds.Bottom - 1),
                                     new Point(_thumbBounds.Left, _thumbBounds.Top + (_thumbBounds.Height/2))
                                 };
            var path = new GraphicsPath();
            path.AddLines(pointArray);
            var region = new Region(path);
            g.Clip = region;
            if ((_thumbState == 3) || !Enabled)
            {
                ControlPaint.DrawButton(g, _thumbBounds, ButtonState.All);
            }
            else
            {
                g.Clear(SystemColors.Control);
            }
            g.ResetClip();
            region.Dispose();
            path.Dispose();
            var points = new[] { pointArray[0], pointArray[1], pointArray[2] };
            g.DrawLines(SystemPens.ControlLightLight, points);
            points = new[] { pointArray[2], pointArray[3], pointArray[4], pointArray[5] };
            g.DrawLines(SystemPens.ControlDarkDark, points);
            pointArray[0].Offset(1, 0);
            pointArray[1].Offset(0, 1);
            pointArray[2].Offset(-1, 1);
            pointArray[3].Offset(-1, -1);
            pointArray[4].Offset(0, -1);
            pointArray[5] = pointArray[0];
            points = new[] { pointArray[0], pointArray[1], pointArray[2] };
            g.DrawLines(SystemPens.ControlLight, points);
            points = new[] { pointArray[2], pointArray[3], pointArray[4], pointArray[5] };
            g.DrawLines(SystemPens.ControlDark, points);
        }

        private void DrawPointerRight(Graphics g)
        {
            var pointArray = new[]
                                 {
                                     new Point(_thumbBounds.Left, _thumbBounds.Bottom - 1),
                                     new Point(_thumbBounds.Left, _thumbBounds.Top),
                                     new Point((_thumbBounds.Right - (_thumbBounds.Height/2)) - 1,
                                               _thumbBounds.Top),
                                     new Point(_thumbBounds.Right - 1,
                                               _thumbBounds.Top + (_thumbBounds.Height/2)),
                                     new Point((_thumbBounds.Right - (_thumbBounds.Height/2)) - 1,
                                               _thumbBounds.Bottom - 1),
                                     new Point(_thumbBounds.Left, _thumbBounds.Bottom - 1)
                                 };
            var path = new GraphicsPath();
            path.AddLines(pointArray);
            var region = new Region(path);
            g.Clip = region;
            if ((_thumbState == 3) || !Enabled)
            {
                ControlPaint.DrawButton(g, _thumbBounds, ButtonState.All);
            }
            else
            {
                g.Clear(SystemColors.Control);
            }
            g.ResetClip();
            region.Dispose();
            path.Dispose();
            var points = new[] { pointArray[0], pointArray[1], pointArray[2], pointArray[3] };
            g.DrawLines(SystemPens.ControlLightLight, points);
            points = new[] { pointArray[3], pointArray[4], pointArray[5] };
            g.DrawLines(SystemPens.ControlDarkDark, points);
            pointArray[0].Offset(1, -1);
            pointArray[1].Offset(1, 1);
            pointArray[2].Offset(0, 1);
            pointArray[3].Offset(-1, 0);
            pointArray[4].Offset(0, -1);
            pointArray[5] = pointArray[0];
            points = new[] { pointArray[0], pointArray[1], pointArray[2], pointArray[3] };
            g.DrawLines(SystemPens.ControlLight, points);
            points = new[] { pointArray[3], pointArray[4], pointArray[5] };
            g.DrawLines(SystemPens.ControlDark, points);
        }

        private void DrawPointerUp(Graphics g)
        {
            var pointArray = new[]
                                 {
                                     new Point(_thumbBounds.Left, _thumbBounds.Bottom - 1),
                                     new Point(_thumbBounds.Left,
                                               _thumbBounds.Top + (_thumbBounds.Width/2)),
                                     new Point(_thumbBounds.Left + (_thumbBounds.Width/2),
                                               _thumbBounds.Top),
                                     new Point(_thumbBounds.Right - 1,
                                               _thumbBounds.Top + (_thumbBounds.Width/2)),
                                     new Point(_thumbBounds.Right - 1, _thumbBounds.Bottom - 1),
                                     new Point(_thumbBounds.Left, _thumbBounds.Bottom - 1)
                                 };
            var path = new GraphicsPath();
            path.AddLines(pointArray);
            var region = new Region(path);
            g.Clip = region;
            if ((_thumbState == 3) || !Enabled)
            {
                ControlPaint.DrawButton(g, _thumbBounds, ButtonState.All);
            }
            else
            {
                g.Clear(SystemColors.Control);
            }
            g.ResetClip();
            region.Dispose();
            path.Dispose();
            var points = new[] { pointArray[0], pointArray[1], pointArray[2] };
            g.DrawLines(SystemPens.ControlLightLight, points);
            points = new[] { pointArray[2], pointArray[3], pointArray[4], pointArray[5] };
            g.DrawLines(SystemPens.ControlDarkDark, points);
            pointArray[0].Offset(1, -1);
            pointArray[1].Offset(1, 0);
            pointArray[2].Offset(0, 1);
            pointArray[3].Offset(-1, 0);
            pointArray[4].Offset(-1, -1);
            pointArray[5] = pointArray[0];
            points = new[] { pointArray[0], pointArray[1], pointArray[2] };
            g.DrawLines(SystemPens.ControlLight, points);
            points = new[] { pointArray[2], pointArray[3], pointArray[4], pointArray[5] };
            g.DrawLines(SystemPens.ControlDark, points);
        }

        private void DrawVerticalTicks(Graphics g, Color color)
        {
            RectangleF ef3;
            var num = ((Maximum - Minimum) / TickFrequency) - 1;
            var pen = new Pen(color);
            var ef2 = new RectangleF(_thumbBounds.Left - 5, (_channelBounds.Bottom - (_thumbBounds.Height / 2)) - 1, 3f, 0f);
            var ef = new RectangleF(_thumbBounds.Left - 5, _channelBounds.Top + (_thumbBounds.Height / 2), 3f, 0f);
            var y = (ef.Bottom - ef2.Top) / (num + 1);
            if (TickStyle != TickStyle.BottomRight)
            {
                g.DrawLine(pen, ef2.Left, ef2.Top, ef2.Right, ef2.Bottom);
                g.DrawLine(pen, ef.Left, ef.Top, ef.Right, ef.Bottom);
                ef3 = ef2;
                ef3.Width--;
                ef3.Offset(1f, y);
                var num5 = num - 1;
                for (var i = 0; i <= num5; i++)
                {
                    g.DrawLine(pen, ef3.Left, ef3.Top, ef3.Right, ef3.Bottom);
                    ef3.Offset(0f, y);
                }
            }
            ef2.Offset(_thumbBounds.Width + 6, 0f);
            ef.Offset(_thumbBounds.Width + 6, 0f);
            if (TickStyle != TickStyle.TopLeft)
            {
                g.DrawLine(pen, ef2.Left, ef2.Top, ef2.Right, ef2.Bottom);
                g.DrawLine(pen, ef.Left, ef.Top, ef.Right, ef.Bottom);
                ef3 = ef2;
                ef3.Width--;
                ef3.Offset(0f, y);
                var num6 = num - 1;
                for (var j = 0; j <= num6; j++)
                {
                    g.DrawLine(pen, ef3.Left, ef3.Top, ef3.Right, ef3.Bottom);
                    ef3.Offset(0f, y);
                }
            }
            pen.Dispose();
        }

        protected virtual void OnDrawChannel(IntPtr hdc)
        {
            var graphics = Graphics.FromHdc(hdc);
            if (((OwnerDrawParts & TrackBarOwnerDrawParts.Channel) == TrackBarOwnerDrawParts.Channel) && !DesignMode)
            {
                var e = new TrackBarDrawItemEventArgs(graphics, _channelBounds, (TrackBarItemState)_thumbState);
                var drawChannelEvent = DrawChannel;
                drawChannelEvent?.Invoke(this, e);
            }
            else
            {
                if (_channelBounds.Equals(Rectangle.Empty))
                {
                    return;
                }
                if (VisualStylesEnabled)
                {
                    var hTheme = NativeMethods.OpenThemeData(Handle, "TRACKBAR");
                    if (!hTheme.Equals(IntPtr.Zero))
                    {
                        var pRect = new NativeMethods.Rect(_channelBounds);
                        var flag = NativeMethods.DrawThemeBackground(hTheme, hdc, 1, 1, ref pRect, ref pRect) == 0;
                        NativeMethods.CloseThemeData(hTheme);
                        if (flag)
                        {
                            return;
                        }
                    }
                }
                ControlPaint.DrawBorder3D(graphics, _channelBounds, Border3DStyle.Sunken);
            }
            graphics.Dispose();
        }

        protected virtual void OnDrawThumb(IntPtr hdc)
        {
            using (var graphics = Graphics.FromHdc(hdc))
            {
                graphics.Clip = new Region(_thumbBounds);
                if (((OwnerDrawParts & TrackBarOwnerDrawParts.Thumb) == TrackBarOwnerDrawParts.Thumb) && !DesignMode)
                {
                    var e = new TrackBarDrawItemEventArgs(graphics, _thumbBounds, (TrackBarItemState)_thumbState);
                    var drawThumbEvent = DrawThumb;
                    drawThumbEvent?.Invoke(this, e);
                }
                else
                {
                    var num = 0;
                    if (_thumbBounds.Equals(Rectangle.Empty))
                    {
                        return;
                    }
                    switch (TickStyle)
                    {
                        case TickStyle.None:
                        case TickStyle.BottomRight:
                            if (Orientation != Orientation.Horizontal)
                            {
                                num = 8;
                                break;
                            }
                            num = 4;
                            break;

                        case TickStyle.TopLeft:
                            if (Orientation != Orientation.Horizontal)
                            {
                                num = 7;
                                break;
                            }
                            num = 5;
                            break;

                        case TickStyle.Both:
                            if (Orientation != Orientation.Horizontal)
                            {
                                num = 6;
                                break;
                            }
                            num = 3;
                            break;
                    }
                    if (VisualStylesEnabled)
                    {
                        var hTheme = NativeMethods.OpenThemeData(Handle, "TRACKBAR");
                        if (!hTheme.Equals(IntPtr.Zero))
                        {
                            var pRect = new NativeMethods.Rect(_thumbBounds);
                            var flag =
                                NativeMethods.DrawThemeBackground(hTheme, hdc, num, _thumbState, ref pRect, ref pRect) == 0;
                            NativeMethods.CloseThemeData(hTheme);
                            if (flag)
                            {
                                graphics.ResetClip();
                                return;
                            }
                        }
                    }
                    switch (num)
                    {
                        case 4:
                            DrawPointerDown(graphics);
                            graphics.ResetClip();
                            return;

                        case 5:
                            DrawPointerUp(graphics);
                            graphics.ResetClip();
                            return;

                        case 7:
                            DrawPointerLeft(graphics);
                            graphics.ResetClip();
                            return;

                        case 8:
                            DrawPointerRight(graphics);
                            graphics.ResetClip();
                            return;
                    }
                    if ((_thumbState == 3) || !Enabled)
                    {
                        ControlPaint.DrawButton(graphics, _thumbBounds, ButtonState.All);
                    }
                    else
                    {
                        graphics.FillRectangle(SystemBrushes.Control, _thumbBounds);
                    }
                    ControlPaint.DrawBorder3D(graphics, _thumbBounds, Border3DStyle.Raised);
                }
                graphics.ResetClip();
            }
        }

        protected virtual void OnDrawTicks(IntPtr hdc)
        {
            using (var graphics = Graphics.FromHdc(hdc))
            {
                if (((OwnerDrawParts & TrackBarOwnerDrawParts.Ticks) == TrackBarOwnerDrawParts.Ticks) && !DesignMode)
                {
                    var rectangle = Orientation == Orientation.Horizontal
                                        ? new Rectangle(_channelBounds.Left + (_thumbBounds.Width/2),
                                                        _thumbBounds.Top - 6,
                                                        _channelBounds.Width - _thumbBounds.Width,
                                                        _thumbBounds.Height + 10)
                                        : new Rectangle(_thumbBounds.Left - (_thumbBounds.Height/2),
                                                        _channelBounds.Top + 6, _thumbBounds.Width + 10,
                                                        _channelBounds.Height - _thumbBounds.Height);
                    var e = new TrackBarDrawItemEventArgs(graphics, rectangle, (TrackBarItemState)_thumbState);
                    var drawTicksEvent = DrawTicks;
                    drawTicksEvent?.Invoke(this, e);
                }
                else
                {
                    if (TickStyle == TickStyle.None)
                    {
                        return;
                    }
                    if (_thumbBounds.Equals(Rectangle.Empty))
                    {
                        return;
                    }
                    var black = Color.Black;
                    if (VisualStylesEnabled)
                    {
                        var hTheme = NativeMethods.OpenThemeData(Handle, "TRACKBAR");
                        if (!hTheme.Equals(IntPtr.Zero))
                        {
                            var num = 0;
                            if (NativeMethods.GetThemeColor(hTheme, 9, _thumbState, 0xcc, ref num) == 0)
                            {
                                black = ColorTranslator.FromWin32(num);
                            }
                            NativeMethods.CloseThemeData(hTheme);
                        }
                    }
                    if (Orientation == Orientation.Horizontal)
                    {
                        DrawHorizontalTicks(graphics, black);
                    }
                    else
                    {
                        DrawVerticalTicks(graphics, black);
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button != MouseButtons.None || !_thumbBounds.Contains(e.X, e.Y))
            {
                return;
            }
            _thumbState = 2;
            Invalidate(new Region(_thumbBounds));
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            IntPtr ptr2;
            if (m.Msg == 20)
            {
                ptr2 = new IntPtr(1);
                m.Result = ptr2;
            }
            base.WndProc(ref m);
            if (m.Msg != 0x204e)
            {
                return;
            }
            var structure = (NativeMethods.Nmhdr)Marshal.PtrToStructure(m.LParam, typeof (NativeMethods.Nmhdr));
            if (structure.code != -12)
            {
                return;
            }
            Marshal.StructureToPtr(structure, m.LParam, false);
            var nmcustomdraw =
                (NativeMethods.NmCustomDraw)Marshal.PtrToStructure(m.LParam, typeof (NativeMethods.NmCustomDraw));
            switch (nmcustomdraw.dwDrawStage)
            {
                case NativeMethods.CustomDrawDrawStage.CddsPrepaint:
                    var graphics = Graphics.FromHdc(nmcustomdraw.hdc);
                    var e = new PaintEventArgs(graphics, Bounds);
                    e.Graphics.TranslateTransform(0 - Left, 0 - Top);
                    InvokePaintBackground(Parent, e);
                    InvokePaint(Parent, e);
                    var brush = new SolidBrush(BackColor);
                    e.Graphics.FillRectangle(brush, Bounds);
                    brush.Dispose();
                    e.Graphics.ResetTransform();
                    e.Dispose();
                    graphics.Dispose();
                    ptr2 = new IntPtr(0x30);
                    m.Result = ptr2;
                    break;

                case NativeMethods.CustomDrawDrawStage.CddsPostpaint:
                    OnDrawTicks(nmcustomdraw.hdc);
                    OnDrawChannel(nmcustomdraw.hdc);
                    OnDrawThumb(nmcustomdraw.hdc);
                    break;

                case NativeMethods.CustomDrawDrawStage.CddsItemprepaint:
                    switch (nmcustomdraw.dwItemSpec.ToInt32())
                    {
                        case 2:
                            _thumbBounds = nmcustomdraw.rc.ToRectangle();
                            if (Enabled)
                            {
                                _thumbState = nmcustomdraw.uItemState == NativeMethods.CustomDrawItemState.CdisSelected ? 3 : 1;
                            }
                            else
                            {
                                _thumbState = 5;
                            }
                            OnDrawThumb(nmcustomdraw.hdc);
                            break;

                        case 3:
                            _channelBounds = nmcustomdraw.rc.ToRectangle();
                            OnDrawChannel(nmcustomdraw.hdc);
                            break;
                        case 1:
                            OnDrawTicks(nmcustomdraw.hdc);
                            break;
                    }
                    ptr2 = new IntPtr(4);
                    m.Result = ptr2;
                    break;
            }
        }
    }
}

