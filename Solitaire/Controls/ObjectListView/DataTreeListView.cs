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
using System.ComponentModel;
using System.Drawing.Design;
using Solitaire.Controls.ObjectListView.Implementation.Adapters;

namespace Solitaire.Controls.ObjectListView
{
    public class DataTreeListView : TreeListView
    {
        private TreeDataSourceAdapter _adapter;

        /* Public Properties */
        [Category("Data"), TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design")]
        public virtual object DataSource
        {
            get => Adapter.DataSource;
            set => Adapter.DataSource = value;
        }

        [Category("Data"), Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design", typeof (UITypeEditor)), DefaultValue("")]
        public virtual string DataMember
        {
            get => Adapter.DataMember;
            set => Adapter.DataMember = value;
        }

        [Category("Data"), Description("The name of the property/column that holds the key of a row"), DefaultValue(null)]
        public virtual string KeyAspectName
        {
            get => Adapter.KeyAspectName;
            set => Adapter.KeyAspectName = value;
        }

        [Category("Data"), Description("The name of the property/column that holds the key of the parent of a row"), DefaultValue(null)]
        public virtual string ParentKeyAspectName
        {
            get => Adapter.ParentKeyAspectName;
            set => Adapter.ParentKeyAspectName = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object RootKeyValue
        {
            get => Adapter.RootKeyValue;
            set => Adapter.RootKeyValue = value;
        }

        [Category("Data"), Description("The parent id value that identifies a row as a root object"), DefaultValue(null)]
        public virtual string RootKeyValueString
        {
            get => Convert.ToString(Adapter.RootKeyValue);
            set => Adapter.RootKeyValue = value;
        }

        [Category("Data"), Description("Should the keys columns (id and parent id) be shown to the user?"), DefaultValue(true)]
        public virtual bool ShowKeyColumns
        {
            get => Adapter.ShowKeyColumns;
            set => Adapter.ShowKeyColumns = value;
        }

        /* Implementation properties */
        protected TreeDataSourceAdapter Adapter
        {
            get => _adapter ?? (_adapter = new TreeDataSourceAdapter(this));
            set => _adapter = value;
        }
    }
}
