/* xsMedia - xsTrackBar
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;

namespace Solitaire.Controls.TrackBar.TrackBarBase
{
    public delegate void TrackBarDrawItemEventHandler(object sender, TrackBarDrawItemEventArgs e);

    public class TrackBarDrawItemEventArgs : EventArgs
    {
        public TrackBarDrawItemEventArgs(Graphics graphics, Rectangle bounds, TrackBarItemState state)
        {
            Graphics = graphics;
            Bounds = bounds;
            State = state;
        }

        public Rectangle Bounds { get; }

        public Graphics Graphics { get; }

        public TrackBarItemState State { get; }
    }
}

