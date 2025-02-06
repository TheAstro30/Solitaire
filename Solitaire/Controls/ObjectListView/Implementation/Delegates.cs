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
using System.Drawing;
using System.Windows.Forms;
using Solitaire.Controls.ObjectListView.Rendering.Styles;

namespace Solitaire.Controls.ObjectListView.Implementation
{
    public delegate object AspectGetterDelegate(object rowObject);
    public delegate void AspectPutterDelegate(object rowObject, object newValue);
    public delegate string AspectToStringConverterDelegate(object value);
    public delegate string CellToolTipGetterDelegate(OlvColumn column, object modelObject);
    public delegate CheckState CheckStateGetterDelegate(object rowObject);
    public delegate bool BooleanCheckStateGetterDelegate(object rowObject);
    public delegate CheckState CheckStatePutterDelegate(object rowObject, CheckState newValue);
    public delegate bool BooleanCheckStatePutterDelegate(object rowObject, bool newValue);
    public delegate void ColumnRightClickEventHandler(object sender, ColumnClickEventArgs e);
    public delegate bool HeaderDrawingDelegate(Graphics g, Rectangle r, int columnIndex, OlvColumn column, bool isPressed, HeaderStateStyle stateStyle);
    public delegate void GroupFormatterDelegate(OlvGroup group, GroupingParameters parms);
    public delegate object GroupKeyGetterDelegate(object rowObject);
    public delegate string GroupKeyToTitleConverterDelegate(object groupKey);
    public delegate string HeaderToolTipGetterDelegate(OlvColumn column);
    public delegate object ImageGetterDelegate(object rowObject);
    public delegate bool RenderDelegate(EventArgs e, Graphics g, Rectangle r, object rowObject);
    public delegate object RowGetterDelegate(int rowIndex);
    public delegate void RowFormatterDelegate(OlvListItem olvItem);
    public delegate void SortDelegate(OlvColumn column, SortOrder sortOrder);
}
