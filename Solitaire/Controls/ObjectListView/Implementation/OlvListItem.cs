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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Controls.ObjectListView.Rendering.Decoration;

namespace Solitaire.Controls.ObjectListView.Implementation
{
    public class OlvListItem : ListViewItem
    {
        private CheckState _checkState;
        private IList<IDecoration> _decorations;
        private object _imageSelector;

        public OlvListItem(object rowObject)
        {
            RowObject = rowObject;
        }

        public OlvListItem(object rowObject, string text, object image) : base(text, -1)
        {
            RowObject = rowObject;
            _imageSelector = image;
        }

        /* Properties */
        public new Rectangle Bounds
        {
            get
            {
                try
                {
                    return base.Bounds;
                }
                catch (ArgumentException)
                {
                    /* If the item is part of a collapsed group, Bounds will throw an exception */
                    return Rectangle.Empty;
                }
            }
        }

        public Rectangle? CellPadding { get; set; }
        public StringAlignment? CellVerticalAlignment { get; set; }

        public new bool Checked
        {
            get => base.Checked;
            set
            {
                if (Checked == value) { return; }
                if (value)
                {
                    ((Solitaire.Controls.ObjectListView.ObjectListView)ListView).CheckObject(RowObject);
                }
                else
                {
                    ((Solitaire.Controls.ObjectListView.ObjectListView)ListView).UncheckObject(RowObject);
                }
            }
        }

        public CheckState CheckState
        {
            get
            {
                switch (StateImageIndex)
                {
                    case 0:
                        return CheckState.Unchecked;

                    case 1:
                        return CheckState.Checked;

                    case 2:
                        return CheckState.Indeterminate;

                    default:
                        return CheckState.Unchecked;
                }
            }
            set
            {
                if (_checkState == value)
                {
                    return;
                }
                _checkState = value;
                /* THINK: I don't think we need this, since the Checked property just uses StateImageIndex, which we are
                 * about to set.
                 * We have to specifically set the state image */
                switch (value)
                {
                    case CheckState.Unchecked:
                        StateImageIndex = 0;
                        break;

                    case CheckState.Checked:
                        StateImageIndex = 1;
                        break;

                    case CheckState.Indeterminate:
                        StateImageIndex = 2;
                        break;
                }
            }
        }

        public bool HasDecoration => _decorations != null && _decorations.Count > 0;

        public IDecoration Decoration
        {
            get => HasDecoration ? Decorations[0] : null;
            set
            {
                Decorations.Clear();
                if (value != null)
                {
                    Decorations.Add(value);
                }
            }
        }

        public IList<IDecoration> Decorations => _decorations ?? (_decorations = new List<IDecoration>());

        public object ImageSelector
        {
            get => _imageSelector;
            set
            {
                _imageSelector = value;
                switch (value)
                {
                    case int i:
                        ImageIndex = i;
                        break;

                    case string s:
                        ImageKey = s;
                        break;

                    default:
                        ImageIndex = -1;
                        break;
                }
            }
        }

        public object RowObject { get; set; }

        /* Accessing */
        public virtual OlvListSubItem GetSubItem(int index)
        {
            return index >= 0 && index < SubItems.Count ? (OlvListSubItem)SubItems[index] : null;
        }

        public virtual Rectangle GetSubItemBounds(int subItemIndex)
        {
            if (subItemIndex == 0)
            {
                var r = Bounds;
                var sides = NativeMethods.GetScrolledColumnSides(ListView, subItemIndex);
                r.X = sides.X + 1;
                r.Width = sides.Y - sides.X;
                return r;
            }
            var subItem = GetSubItem(subItemIndex);
            return subItem?.Bounds ?? new Rectangle();
        }
    }
}
