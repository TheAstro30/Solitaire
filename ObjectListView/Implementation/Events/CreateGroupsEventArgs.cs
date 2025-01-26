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
using System;
using System.Collections.Generic;

namespace libolv.Implementation.Events
{
    public class CreateGroupsEventArgs : EventArgs
    {
        public CreateGroupsEventArgs(GroupingParameters parms)
        {
            Parameters = parms;
        }

        public GroupingParameters Parameters { get; }
        public IList<OlvGroup> Groups { get; set; }
        public bool Canceled { get; set; }
    }

    public class GroupTaskClickedEventArgs : EventArgs
    {
        public GroupTaskClickedEventArgs(OlvGroup group)
        {
            Group = group;
        }

        public OlvGroup Group { get; }
    }

    public class GroupExpandingCollapsingEventArgs : CancellableEventArgs
    {
        public GroupExpandingCollapsingEventArgs(OlvGroup group)
        {
            Group = group ?? throw new ArgumentNullException(nameof(group));
        }

        public OlvGroup Group { get; }

        public bool IsExpanding => Group.Collapsed;
    }

    public class GroupStateChangedEventArgs : EventArgs
    {
        public GroupStateChangedEventArgs(OlvGroup group, GroupState oldState, GroupState newState)
        {
            Group = group;
            OldState = oldState;
            NewState = newState;
        }

        public bool Collapsed =>
            ((OldState & GroupState.LvgsCollapsed) != GroupState.LvgsCollapsed) &&
            ((NewState & GroupState.LvgsCollapsed) == GroupState.LvgsCollapsed);

        public bool Focused =>
            ((OldState & GroupState.LvgsFocused) != GroupState.LvgsFocused) &&
            ((NewState & GroupState.LvgsFocused) == GroupState.LvgsFocused);

        public bool Selected =>
            ((OldState & GroupState.LvgsSelected) != GroupState.LvgsSelected) &&
            ((NewState & GroupState.LvgsSelected) == GroupState.LvgsSelected);

        public bool Uncollapsed =>
            ((OldState & GroupState.LvgsCollapsed) == GroupState.LvgsCollapsed) &&
            ((NewState & GroupState.LvgsCollapsed) != GroupState.LvgsCollapsed);

        public bool Unfocused =>
            ((OldState & GroupState.LvgsFocused) == GroupState.LvgsFocused) &&
            ((NewState & GroupState.LvgsFocused) != GroupState.LvgsFocused);

        public bool Unselected =>
            ((OldState & GroupState.LvgsSelected) == GroupState.LvgsSelected) &&
            ((NewState & GroupState.LvgsSelected) != GroupState.LvgsSelected);

        public OlvGroup Group { get; }
        public GroupState OldState { get; }
        public GroupState NewState { get; }
    }
}
