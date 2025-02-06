/* Object List View
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
using Solitaire.Controls.ObjectListView.Implementation;
using Solitaire.Controls.ObjectListView.Rendering.Overlays;

namespace Solitaire.Controls.ObjectListView.Rendering.Decoration
{
    public interface IDecoration : IOverlay
    {
        OlvListItem ListItem { get; set; }
        OlvListSubItem SubItem { get; set; }
    }

    public class AbstractDecoration : IDecoration
    {
        public OlvListItem ListItem { get; set; }
        public OlvListSubItem SubItem { get; set; }

        public Rectangle RowBounds => ListItem?.Bounds ?? Rectangle.Empty;

        public Rectangle CellBounds
        {
            get
            {
                if (ListItem == null || SubItem == null)
                {
                    return Rectangle.Empty;
                }
                return ListItem.GetSubItemBounds(ListItem.SubItems.IndexOf(SubItem));
            }
        }

        public virtual void Draw(Solitaire.Controls.ObjectListView.ObjectListView olv, Graphics g, Rectangle r)
        {
            /* Not implemented */
        }
    }
}
