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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Solitaire.Controls.ObjectListView.Filtering.Filters;
using Solitaire.Controls.ObjectListView.Implementation.Adapters;
using Solitaire.Controls.ObjectListView.Implementation.Comparers;

namespace Solitaire.Controls.ObjectListView
{
    public class FastObjectListDataSource : AbstractVirtualListDataSource
    {
        private IModelFilter _modelFilter;
        private IListFilter _listFilter;

        private readonly Dictionary<object, int> _objectsToIndexMap = new Dictionary<object, int>();

        public FastObjectListDataSource(VirtualObjectListView listView) : base(listView)
        {
            /* Empty */
        }

        /* IVirtualListDataSource Members */
        public override object GetNthObject(int n)
        {
            return n >= 0 && n < FilteredObjectList.Count ? FilteredObjectList[n] : null;
        }

        public override int GetObjectCount()
        {
            return FilteredObjectList.Count;
        }

        public override int GetObjectIndex(object model)
        {
            return model != null && _objectsToIndexMap.TryGetValue(model, out var index) ? index : -1;
        }

        public override int SearchText(string text, int first, int last, OlvColumn column)
        {
            if (first <= last)
            {
                for (var i = first; i <= last; i++)
                {
                    var data = column.GetStringValue(ListView.GetNthItemInDisplayOrder(i).RowObject);
                    if (data.StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return i;
                    }
                }
            }
            else
            {
                for (var i = first; i >= last; i--)
                {
                    var data = column.GetStringValue(ListView.GetNthItemInDisplayOrder(i).RowObject);
                    if (data.StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public override void Sort(OlvColumn column, SortOrder sortOrder)
        {
            if (sortOrder != SortOrder.None)
            {
                var comparer = new ModelObjectComparer(column, sortOrder,
                                                       ListView.SecondarySortColumn,
                                                       ListView.SecondarySortOrder);
                ObjectList.Sort(comparer);
                FilteredObjectList.Sort(comparer);
            }
            RebuildIndexMap();
        }

        public override void AddObjects(ICollection modelObjects)
        {
            foreach (var modelObject in modelObjects.Cast<object>().Where(modelObject => modelObject != null))
            {
                ObjectList.Add(modelObject);
            }
            FilterObjects();
            RebuildIndexMap();
        }

        public override void RemoveObjects(ICollection modelObjects)
        {
            var indicesToRemove = new List<int>();
            foreach (var modelObject in modelObjects)
            {
                var i = GetObjectIndex(modelObject);
                if (i >= 0)
                {
                    indicesToRemove.Add(i);
                }
                /* Remove the objects from the unfiltered list */
                ObjectList.Remove(modelObject);
            }
            /* Sort the indices from highest to lowest so that we
             * remove latter ones before earlier ones. In this way, the
             * indices of the rows doesn't change after the deletes. */
            indicesToRemove.Sort();
            indicesToRemove.Reverse();
            foreach (var i in indicesToRemove)
            {
                ListView.SelectedIndices.Remove(i);
            }
            FilterObjects();
            RebuildIndexMap();
        }

        public override void SetObjects(IEnumerable collection)
        {
            var newObjects = Solitaire.Controls.ObjectListView.ObjectListView.EnumerableToArray(collection, true);
            ObjectList = newObjects;
            FilterObjects();
            RebuildIndexMap();
        }

        /* IFilterableDataSource Members */
        public override void ApplyFilters(IModelFilter iModelFilter, IListFilter iListFilter)
        {
            _modelFilter = iModelFilter;
            _listFilter = iListFilter;
            SetObjects(ObjectList);
        }

        /* Implementation */
        public ArrayList ObjectList { get; private set; } = new ArrayList();

        public ArrayList FilteredObjectList { get; private set; } = new ArrayList();

        protected void RebuildIndexMap()
        {
            _objectsToIndexMap.Clear();
            for (var i = 0; i < FilteredObjectList.Count; i++)
            {
                _objectsToIndexMap[FilteredObjectList[i]] = i;
            }
        }

        protected void FilterObjects()
        {
            if (!ListView.UseFiltering || (_modelFilter == null && _listFilter == null))
            {
                FilteredObjectList = new ArrayList(ObjectList);
                return;
            }
            var objects = (_listFilter == null)
                              ? ObjectList
                              : _listFilter.Filter(ObjectList);
            /* Apply the object filter if there is one */
            if (_modelFilter == null)
            {
                FilteredObjectList = Solitaire.Controls.ObjectListView.ObjectListView.EnumerableToArray(objects, false);
            }
            else
            {
                FilteredObjectList = new ArrayList();
                foreach (var model in objects.Cast<object>().Where(model => _modelFilter.Filter(model)))
                {
                    FilteredObjectList.Add(model);
                }
            }
        }
    }
}
