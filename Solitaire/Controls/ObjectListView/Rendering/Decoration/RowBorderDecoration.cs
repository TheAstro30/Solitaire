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

namespace Solitaire.Controls.ObjectListView.Rendering.Decoration
{
    public class RowBorderDecoration : BorderDecoration
    {
        public int LeftColumn { get; set; } = -1;

        public int RightColumn { get; set; } = -1;

        protected override Rectangle CalculateBounds()
        {
            var bounds = RowBounds;
            if (ListItem == null)
            {
                return bounds;
            }
            if (LeftColumn >= 0)
            {
                var leftCellBounds = ListItem.GetSubItemBounds(LeftColumn);
                if (!leftCellBounds.IsEmpty)
                {
                    bounds.Width = bounds.Right - leftCellBounds.Left;
                    bounds.X = leftCellBounds.Left;
                }
            }
            if (RightColumn >= 0)
            {
                var rightCellBounds = ListItem.GetSubItemBounds(RightColumn);
                if (!rightCellBounds.IsEmpty)
                {
                    bounds.Width = rightCellBounds.Right - bounds.Left;
                }
            }
            return bounds;
        }
    }
}
