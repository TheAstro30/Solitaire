/* xsMedia - xsTrackBar
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;

namespace Solitaire.Controls.TrackBar.TrackBarBase
{
    public enum TrackBarItemState
    {
        Active = 3,
        Disabled = 5,
        Hot = 2,
        Normal = 1
    }

    [Flags]
    public enum TrackBarOwnerDrawParts
    {
        Channel = 4,
        None = 0,
        Thumb = 2,
        Ticks = 1
    }
}
