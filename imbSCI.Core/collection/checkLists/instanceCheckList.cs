// --------------------------------------------------------------------------------------------------------------------
// <copyright file="instanceCheckList.cs" company="imbVeles" >
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
namespace imbSCI.Core.collection.checkLists
{
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Instance check list
    /// </summary>
    /// <typeparam name="T">Any type of value or object</typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{T, System.Boolean}}" />
    public class instanceCheckList<T> : IEnumerable<KeyValuePair<T, bool>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="instanceCheckList{T}"/> class.
        /// </summary>
        /// <param name="allowedValues">The allowed values.</param>
        public instanceCheckList(IEnumerable<T> allowedValues)
        {
            AddAllowValues(allowedValues);
        }

        /// <summary>
        /// Creates empty check list
        /// </summary>
        public instanceCheckList()
        {
        }

        /// <summary>
        /// Gets the checked instances.
        /// </summary>
        /// <param name="inverse">if set to <c>true</c> [inverse].</param>
        /// <returns></returns>
        public List<T> GetCheckedInstances(Boolean inverse = false)
        {
            List<T> output = new List<T>();

            foreach (var pair in items)
            {
                if (inverse)
                {
                    if (!pair.Value) output.Add(pair.Key);
                }
                else
                {
                    if (pair.Value) output.Add(pair.Key);
                }
            }

            return output;
        }

        /// <summary>
        /// Gets the percentage score: ratio between: score and count
        /// </summary>
        /// <returns></returns>
        public Double GetPercentageScore()
        {
            if (items.Count == 0) return 0.0F;
            Double output = Convert.ToDouble(GetScore());

            return output / items.Count;
        }

        /// <summary>
        /// Gets the score: number of <c>true</c> values
        /// </summary>
        /// <returns></returns>
        public Int32 GetScore()
        {
            Int32 output = 0;
            foreach (T vl in items.Keys)
            {
                if (items[vl]) output++;
            }
            return output;
        }

        /// <summary>
        /// Creates dictionary where items from the list are indexed by value of <c>name</c> property.
        /// </summary>
        /// <remarks>
        /// If the items don't have <c>name</c> property, ToString() is called.
        /// To avoid duplicate names, it will add number of existing names for the second and all other duplicates.
        /// </remarks>
        /// <param name="getChecked">if set to <c>true</c>, the result will contain only checked items</param>
        /// <param name="getUnchecked">if set to <c>true</c>, the result will contain only not checked items</param>
        /// <returns></returns>
        public Dictionary<String, T> GetItemsByNames(Boolean getChecked = true, Boolean getUnchecked = false)
        {
            Dictionary<String, T> output = new Dictionary<String, T>();

            foreach (var pair in items)
            {
                if (pair.Value && !getChecked) continue;
                if (!pair.Value && !getUnchecked) continue;

                String name = GetItemName(pair.Key);
                String nameProposal = name;
                Int32 c = 0;
                while (output.ContainsKey(nameProposal))
                {
                    nameProposal = name + "_" + c.ToString();
                    c++;
                }

                output.Add(nameProposal, pair.Key);
            }

            return output;
        }

        protected String GetItemName(T item)
        {
            String name = "";
            if (item is Data.interfaces.IObjectWithName itemWithName)
            {
                name = itemWithName.name;
            }
            else
            {
                name = item.imbGetPropertySafe("name", "").toStringSafe();
            }

            if (name.isNullOrEmpty())
            {
                name = item.ToString();
            }

            return name;
        }

        /// <summary>
        /// Gets the item names.
        /// </summary>
        /// <param name="getChecked">if set to <c>true</c> [get checked].</param>
        /// <param name="getUnchecked">if set to <c>true</c> [get unchecked].</param>
        /// <returns></returns>
        public List<String> GetItemNames(Boolean getChecked = true, Boolean getUnchecked = false)
        {
            var dict = GetItemsByNames(getChecked, getUnchecked);
            return dict.Keys.ToList();
        }

        /// <summary>
        /// Adds the specified instance and sets its state
        /// </summary>
        /// <param name="vl">The instance to be added into list</param>
        /// <param name="checkState">if set to <c>true</c> [check state].</param>
        public void Add(T vl, Boolean checkState = true)
        {
            Set(vl, checkState, checkListLogic.OR, true);
        }

        /// <summary>
        /// Sets the value to <c>checkState</c> for specified key
        /// </summary>
        /// <param name="vl">The vl.</param>
        /// <param name="checkState">if set to <c>true</c> [check state].</param>
        /// <param name="logic">The logic.</param>
        /// <param name="expandAllow">if set to <c>true</c> it will add the <c>vl</c> in the list, if it is not already inside</param>
        /// <returns></returns>
        public Boolean Set(T vl, Boolean checkState, checkListLogic logic = checkListLogic.OR, Boolean expandAllow = false)
        {
            if (!items.ContainsKey(vl))
            {
                if (expandAllow) items.Add(vl, checkState);
            }
            else
            {
                switch (logic)
                {
                    case checkListLogic.AND:
                        items[vl] = items[vl] && checkState;
                        break;

                    case checkListLogic.NOR:
                        items[vl] = !(items[vl] || checkState);
                        break;

                    case checkListLogic.NOT:
                        items[vl] = items[vl] && !checkState;
                        break;

                    case checkListLogic.OR:
                        items[vl] = items[vl] && checkState;
                        break;
                }
            }
            return items[vl];
        }

        /// <summary>
        /// Sets the values specified according to <c>check state</c>, only instances that are in the list are set - it will not add any new instance
        /// </summary>
        /// <param name="testValues">The test values.</param>
        /// <param name="checkState">if set to <c>true</c> [check state].</param>
        /// <param name="logic">The logic.</param>
        public void Set(IEnumerable<T> testValues, Boolean checkState, checkListLogic logic = checkListLogic.AND)
        {
            foreach (T vl in testValues)
            {
                if (items.ContainsKey(vl))
                {
                    Set(vl, checkState, logic);
                }
            }
        }

        /// <summary>
        /// Returns cross section of this list and <c>testValues</c>, returning only instances in this list, that have TRUE value in the list.
        /// </summary>
        /// <param name="testValues">The test values.</param>
        /// <param name="logic">The logic.</param>
        /// <returns></returns>
        public List<T> Check(IEnumerable<T> testValues)
        {
            List<T> output = new List<T>();

            foreach (T vl in testValues)
            {
                if (items.ContainsKey(vl))
                {
                    if (items[vl]) output.Add(vl);
                }
            }
            return output;
        }

        /// <summary>
        /// Adds instances that can be checked
        /// </summary>
        /// <param name="allowedValues">The allowed values.</param>
        /// <returns></returns>
        public Int32 AddAllowValues(IEnumerable<T> allowedValues)
        {
            Int32 c = 0;

            foreach (T av in allowedValues)
            {
                if (!items.ContainsKey(av))
                {
                    c++;
                    items.Add(av, false);
                }
            }
            return c;
        }

        public IEnumerator<KeyValuePair<T, bool>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<T, bool>>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<T, bool>>)items).GetEnumerator();
        }

        private Dictionary<T, Boolean> _items = new Dictionary<T, Boolean>();

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<T, Boolean> items
        {
            get
            {
                //if (_items == null)_items = new Dictionary<T, Boolean>();
                return _items;
            }
            set { _items = value; }
        }
    }
}