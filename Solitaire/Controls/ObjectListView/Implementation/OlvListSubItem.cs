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

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Controls.ObjectListView.Rendering.Decoration;
using Solitaire.Controls.ObjectListView.Rendering.Renderers;

namespace Solitaire.Controls.ObjectListView.Implementation
{
    [Browsable(false)]
    public class OlvListSubItem : ListViewItem.ListViewSubItem
    {
        internal ImageRenderer.AnimationState AnimationState;

        private IList<IDecoration> _decorations;

        public OlvListSubItem()
        {
            /* Empty */
        }

        public OlvListSubItem(object modelValue, string text, object image)
        {
            ModelValue = modelValue;
            Text = text;
            ImageSelector = image;
        }

        /* Properties */
        public Rectangle? CellPadding { get; set; }
        public StringAlignment? CellVerticalAlignment { get; set; }
        public object ModelValue { get; }

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

        public object ImageSelector { get; set; }
        public string Url { get; set; }        
    }
}
