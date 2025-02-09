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

using System;
using System.Windows.Forms;

namespace Solitaire.Controls.ObjectListView.Implementation
{
    public enum HitTestLocation
    {
        Nothing = 0,
        Text = 1,
        Image = 2,
        CheckBox = 3,
        ExpandButton = 4,
        InCell = 5,
        UserDefined = 6,
        GroupExpander = 7,
        Group = 8
    }

    [Flags]
    public enum HitTestLocationEx
    {
        LvhtNowhere = 0x00000001,
        LvhtOnitemicon = 0x00000002,
        LvhtOnitemlabel = 0x00000004,
        LvhtOnitemstateicon = 0x00000008,
        LvhtOnitem = (LvhtOnitemicon | LvhtOnitemlabel | LvhtOnitemstateicon),

        LvhtAbove = 0x00000008,
        LvhtBelow = 0x00000010,
        LvhtToright = 0x00000020,
        LvhtToleft = 0x00000040,

        LvhtExGroupHeader = 0x10000000,
        LvhtExGroupFooter = 0x20000000,
        LvhtExGroupCollapse = 0x40000000,
        LvhtExGroupBackground = -2147483648, /* 0x80000000 */
        LvhtExGroupStateicon = 0x01000000,
        LvhtExGroupSubsetlink = 0x02000000,

        LvhtExGroup =
            (LvhtExGroupBackground | LvhtExGroupCollapse | LvhtExGroupFooter | LvhtExGroupHeader |
             LvhtExGroupStateicon | LvhtExGroupSubsetlink),

        LvhtExGroupMinusFooterAndBkgrd =
            (LvhtExGroupCollapse | LvhtExGroupHeader | LvhtExGroupStateicon | LvhtExGroupSubsetlink),
        LvhtExOncontents = 0x04000000, /* On item AND not on the background */
        LvhtExFooter = 0x08000000,
    }

    public class OlvListViewHitTestInfo
    {
        public HitTestLocation HitTestLocation;
        public HitTestLocationEx HitTestLocationEx;
        public OlvGroup Group;
        public object UserData;

        public OlvListViewHitTestInfo(OlvListItem olvListItem, OlvListSubItem subItem, int flags, OlvGroup group)
        {
            Item = olvListItem;
            SubItem = subItem;
            Location = ConvertNativeFlagsToDotNetLocation(olvListItem, flags);
            HitTestLocationEx = (HitTestLocationEx)flags;
            Group = group;

            switch (Location)
            {
                case ListViewHitTestLocations.StateImage:
                    HitTestLocation = HitTestLocation.CheckBox;
                    break;

                case ListViewHitTestLocations.Image:
                    HitTestLocation = HitTestLocation.Image;
                    break;

                case ListViewHitTestLocations.Label:
                    HitTestLocation = HitTestLocation.Text;
                    break;

                default:
                    if ((HitTestLocationEx & HitTestLocationEx.LvhtExGroupCollapse) == HitTestLocationEx.LvhtExGroupCollapse)
                    {
                        HitTestLocation = HitTestLocation.GroupExpander;
                    }
                    else if ((HitTestLocationEx & HitTestLocationEx.LvhtExGroupMinusFooterAndBkgrd) != 0)
                    {
                        HitTestLocation = HitTestLocation.Group;
                    }
                    else
                    {
                        HitTestLocation = HitTestLocation.Nothing;
                    }
                    break;
            }
        }

        private static ListViewHitTestLocations ConvertNativeFlagsToDotNetLocation(OlvListItem hitItem, int flags)
        {
            /* Untangle base .NET behaviour.
             * In Windows SDK, the value 8 can have two meanings here: LVHT_ONITEMSTATEICON or LVHT_ABOVE.
             * .NET changes these to be:
             * - LVHT_ABOVE becomes ListViewHitTestLocations.AboveClientArea (which is 0x100).
             * - LVHT_ONITEMSTATEICON becomes ListViewHitTestLocations.StateImage (which is 0x200).
             * So, if we see the 8 bit set in flags, we change that to either a state image hit
             * (if we hit an item) or to AboveClientAream if nothing was hit. */
            if ((8 & flags) == 8)
            {
                return (ListViewHitTestLocations)(0xf7 & flags | (hitItem == null ? 0x100 : 0x200));
            }
            /* Mask off the LVHT_EX_XXXX values since ListViewHitTestLocations doesn't have them */
            return (ListViewHitTestLocations)(flags & 0xffff);
        }

        /* Public read-only properties */
        public OlvListItem Item { get; internal set; }

        public OlvListSubItem SubItem { get; internal set; }

        public ListViewHitTestLocations Location { get; internal set; }

        public Solitaire.Controls.ObjectListView.ObjectListView ListView => (Solitaire.Controls.ObjectListView.ObjectListView) Item?.ListView;

        public object RowObject => Item?.RowObject;

        public int RowIndex => Item?.Index ?? -1;

        public int ColumnIndex => Item == null || SubItem == null ? -1 : Item.SubItems.IndexOf(SubItem);

        public OlvColumn Column
        {
            get
            {
                var index = ColumnIndex;
                return index < 0 ? null : ListView.GetColumn(index);
            }
        }

        public override string ToString()
        {
            return $"HitTestLocation: {HitTestLocation}, HitTestLocationEx: {HitTestLocationEx}, Item: {Item}, SubItem: {SubItem}, Location: {Location}, Group: {Group}";
        }
    }
}