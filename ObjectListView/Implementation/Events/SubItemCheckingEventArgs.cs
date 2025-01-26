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
using System.Windows.Forms;

namespace libolv.Implementation.Events
{
    public class SubItemCheckingEventArgs : CancellableEventArgs
    {
        public SubItemCheckingEventArgs(OlvColumn column, OlvListItem item, int subItemIndex, CheckState currentValue, CheckState newValue)
        {
            Column = column;
            ListViewItem = item;
            SubItemIndex = subItemIndex;
            CurrentValue = currentValue;
            NewValue = newValue;
        }

        public OlvColumn Column { get; }

        public object RowObject => ListViewItem.RowObject;

        public OlvListItem ListViewItem { get; }
        public CheckState CurrentValue { get; }
        public CheckState NewValue { get; set; }
        public int SubItemIndex { get; }
    }
}
