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

namespace Solitaire.Controls.ObjectListView.Implementation.Events
{
    public class HyperlinkEventArgs : EventArgs
    {
        /* TODO: Unified with CellEventArgs */

        public Solitaire.Controls.ObjectListView.ObjectListView ListView { get; internal set; }
        public object Model { get; internal set; }

        public int RowIndex { get; internal set; } = -1;

        public int ColumnIndex { get; internal set; } = -1;

        public OlvColumn Column { get; internal set; }
        public OlvListItem Item { get; internal set; }
        public OlvListSubItem SubItem { get; internal set; }
        public string Url { get; internal set; }
        public bool Handled { get; set; }
    }

    public class IsHyperlinkEventArgs : EventArgs
    {
        public Solitaire.Controls.ObjectListView.ObjectListView ListView { get; internal set; }
        public object Model { get; internal set; }
        public OlvColumn Column { get; internal set; }
        public string Text { get; internal set; }
        public string Url { get; internal set; }
    }

    public class HyperlinkClickedEventArgs : CellEventArgs
    {
        public string Url { get; set; }
    }
}
