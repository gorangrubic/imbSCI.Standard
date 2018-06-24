// --------------------------------------------------------------------------------------------------------------------
// <copyright file="selectRangeAreaDictionary.cs" company="imbVeles" >
//
// Copyright (C) 2018 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

/// <summary>
///
/// </summary>
namespace imbSCI.Core.reporting.zone
{
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Memory of saved ranges
    /// </summary>
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithChildSelector" />
    public class selectRangeAreaDictionary : IObjectWithChildSelector
    {
        /// <summary>
        /// Gets a entry <c>n</c>additions <c>ago</c> from back/end, counting only ones that are <c>isClosed</c>=true.
        /// </summary>
        /// <param name="ago">The ago. If 1 it will return the last closed entry. 0 or less will throw exception</param>
        /// <returns>Last selectRangeAreaNamed that is closed</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">ago - Must be between 0 and infinity. If it is above Count() - the first is returned</exception>
        public selectRangeAreaNamed getLastClosed(Int32 ago = 1)
        {
            return getLast(false, false, ago);
        }

        /// <summary>
        /// Gets the last or <c>N-th</c> that must be unclosed.
        /// </summary>
        /// <param name="ago">The ago.</param>
        /// <returns></returns>
        public selectRangeAreaNamed getLastUnclosed(Int32 ago = 1)
        {
            return getLast(true, false, ago);
        }

        /// <summary>
        /// Gets the last  or <c>N-th</c> - without isClosed criteria.
        /// </summary>
        /// <param name="ago">The ago.</param>
        /// <returns></returns>
        public selectRangeAreaNamed getLastAny(Int32 ago = 1)
        {
            return getLast(false, false, ago);
        }

        /// <summary>
        /// Gets the last.
        /// </summary>
        /// <param name="mustBeUnclosed">if set to <c>true</c> [must be unclosed].</param>
        /// <param name="allowUnclosed">if set to <c>true</c> [allow unclosed].</param>
        /// <param name="ago">The ago.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">ago - Must be between 0 and infinity. If it is above Count() - the first is returned</exception>
        public selectRangeAreaNamed getLast(Boolean mustBeUnclosed = false, Boolean allowUnclosed = true, Int32 ago = 1)
        {
            if (ago < 1) throw new ArgumentOutOfRangeException("ago", "Must be between 0 and infinity. If it is above Count() - the first is returned");
            Int32 i = Count() - ago;
            if (i < 0) i = 0;

            selectRangeAreaNamed found = null;

            for (int a = Count(); a > 0; a--)
            {
                String pt = history[a - 1];
                if (mustBeUnclosed)
                {
                    if (!this[pt].isClosed || allowUnclosed)
                    {
                        found = this[pt];
                    }
                }
                else
                {
                    if (this[pt].isClosed || allowUnclosed)
                    {
                        found = this[pt];
                    }
                }
                if ((a > ago) && (found != null))
                {
                    return found;
                }
            }
            return found;
        }

        protected List<String> history = new List<string>();

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public selectRangeAreaNamed Add(selectRangeAreaNamed item)
        {
            items.Add(item.path, item);
            history.Add(item.path);
            return item;
        }

        /// <summary>
        /// Adds the specified path.
        /// </summary>
        /// <param name="__path">The path.</param>
        /// <param name="item">The item.</param>
        /// <param name="doClose">if set to <c>true</c> [do close].</param>
        /// <returns></returns>
        public selectRangeAreaNamed Add(String __path, selectRangeArea item, Boolean doClose = false)
        {
            selectRangeAreaNamed tmp = new selectRangeAreaNamed(__path, item.x, item.y);

            if (doClose) tmp.setEnd(item.BottomRight.x, item.BottomRight.y);

            //tmp.resize(item.width, item.height);

            items.Add(tmp.path, tmp);
            history.Add(tmp.path);
            return tmp;
        }

        /// <summary>
        /// Adds the specified path.
        /// </summary>
        /// <param name="__path">The path.</param>
        /// <param name="x_st">The x st.</param>
        /// <param name="y_st">The y st.</param>
        /// <returns></returns>
        public selectRangeAreaNamed Add(String __path, Int32 x_st, Int32 y_st)
        {
            selectRangeAreaNamed tmp = new selectRangeAreaNamed(__path, x_st, y_st);
            if (items.ContainsKey(tmp.path))
            {
                items[tmp.path] = tmp;
                history.Add(tmp.path);
            }
            else
            {
                items.Add(tmp.path, tmp);
                history.Add(tmp.path);
            }

            return tmp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="selectRangeAreaDictionary"/> class.
        /// </summary>
        /// <param name="__name">Name of this dictionary</param>
        public selectRangeAreaDictionary(String __name = "rangeDictionary")
        {
            name = __name;
        }

        private Dictionary<String, selectRangeAreaNamed> _items = new Dictionary<string, selectRangeAreaNamed>();

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified path.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public selectRangeAreaNamed this[string path]
        {
            get
            {
                if (!items.ContainsKey(path))
                {
                    items.Add(path, new selectRangeAreaNamed(path, 0, 0));
                }
                return items[path];
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public selectRangeAreaNamed this[int key]
        {
            get
            {
                Int32 c = 0;
                foreach (selectRangeAreaNamed item in items.Values)
                {
                    if (c == key)
                    {
                        return item;
                    }
                    c++;
                }
                return null;
            }
        }

        private String _name = "";

        /// <summary>
        /// Dictionary name
        /// </summary>
        public string name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Members
        /// </summary>
        protected Dictionary<String, selectRangeAreaNamed> items
        {
            get { return _items; }
            set { _items = value; }
        }

        string IObjectWithName.name
        {
            get { return this.name; }
            set
            {
                this.name = value;
            }
        }

        object IObjectWithChildSelector.this[string childName]
        {
            get
            {
                return this[childName];
            }
        }

        object IObjectWithChildSelector.this[int key]
        {
            get
            {
                return this[key];
            }
        }

        /// <summary>
        /// Number of child items
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return items.Count();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        /// <summary>
        /// Index of supplied child - in his collection
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public int indexOf(IObjectWithChildSelector child)
        {
            if (!items.ContainsValue(child as selectRangeAreaNamed)) return -1;

            Int32 c = 0;
            foreach (selectRangeAreaNamed item in items.Values)
            {
                if (item == child) return c;
                c++;
            }
            return -1;
        }

        IEnumerator IObjectWithChildSelector.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        int IObjectWithChildSelector.indexOf(IObjectWithChildSelector child)
        {
            throw new NotImplementedException();
        }

        int IObjectWithChildSelector.Count()
        {
            throw new NotImplementedException();
        }
    }
}