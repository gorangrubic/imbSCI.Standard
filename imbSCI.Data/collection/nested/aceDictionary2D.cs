// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceDictionary2D.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data.collection.nested
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;


    /// <summary>
    /// 2D dictionary with automatic dimension dictionary management.
    /// </summary>
    /// <typeparam name="TD1Key">The type of the d1 key - the key of the first dimension.</typeparam>
    /// <typeparam name="TD2Key">The type of the d2 key - the key of the second dimension.</typeparam>
    /// <typeparam name="TValue">The type of the value - the value stored in 2D matrix</typeparam>
    public class aceDictionary2D<TD1Key, TD2Key, TValue> : IEnumerable<KeyValuePair<TD1Key, ConcurrentDictionary<TD2Key, TValue>>>
    {
        /// <summary>
        /// Determines whether the specified key1 contains key.
        /// </summary>
        /// <param name="key1">The key1.</param>
        /// <returns>
        ///   <c>true</c> if the specified key1 contains key; otherwise, <c>false</c>.
        /// </returns>
        public Boolean ContainsKey(TD1Key key1)
        {
            return items.ContainsKey(key1);
        }

        /// <summary>
        /// Gets the width: number of columns (second dimension)
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        ///
        public Int32 width
        {
            get
            {
                return items.Count;
            }
        }

        /// <summary>
        /// If contains at least 1 x 1 data entries
        /// </summary>
        /// <returns></returns>
        public Boolean Any()
        {
            if (items.Any())
            {
                return items.First().Value.Any();
            }
            return false;
        }

        /// <summary>
        /// Gets the height: number of rows (second dimension)
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public Int32 height
        {
            get
            {
                if (!Any()) return 0;
                return items.First().Value.Count;
            }
        }

        /// <summary>
        /// Gets the count of the first dimension entries
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public Int32 Count
        {
            get
            {
                return items.Count;
            }
        }

        /// <summary>
        /// Determines whether contains key in second dimension of Key2, returns false if even key1 is not contained
        /// </summary>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <returns>
        ///   <c>true</c> if [contains key d2] [the specified key1]; otherwise, <c>false</c>.
        /// </returns>
        public Boolean ContainsKey2InKey1(TD1Key key1, TD2Key key2)
        {
            if (!ContainsKey(key1)) return false;
            return items[key1].ContainsKey(key2);
        }

        /// <summary>
        /// Determines whether [contains key2 any key1] [the specified key2].
        /// </summary>
        /// <param name="key2">The key2.</param>
        /// <returns>
        ///   <c>true</c> if [contains key2 any key1] [the specified key2]; otherwise, <c>false</c>.
        /// </returns>
        public Boolean ContainsKey2AnyKey1(TD2Key key2)
        {
            foreach (var column in items)
            {
                if (column.Value.ContainsKey(key2)) return true;
            }
            return false;
        }

        /// <summary>
        /// Access to stored 2D value. Returns <see cref="TValue"/> default value if not defined within the matrix
        /// </summary>
        /// <value>
        /// The <see cref="TValue"/>.
        /// </value>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <returns></returns>
        public TValue this[TD1Key key1, TD2Key key2]
        {
            get
            {
                return getForKeys(key1, key2);
            }
            set
            {
                setForKeys(key1, key2, value);
            }
        }

        /// <summary>
        /// Get2nds the keys.
        /// </summary>
        /// <param name="key1">The key1.</param>
        /// <returns></returns>
        public List<TD2Key> Get2ndKeys(TD1Key key1)
        {
            return items[key1].Keys.ToList();
        }

        /// <summary>
        /// Get1sts the keys.
        /// </summary>
        /// <returns></returns>
        public List<TD1Key> Get1stKeys()
        {
            return items.Keys.ToList();
        }

        /// <summary>
        /// Accessing whole nested dimension. Get will return dictionary with reference, Set will not replace existing dimension but update if with all <see cref="KeyValuePair{TKey, TValue}"/> pairs.
        /// </summary>
        /// <value>
        /// The <see cref="Dictionary{TD2Key, TValue}"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public ConcurrentDictionary<TD2Key, TValue> this[TD1Key key]
        {
            get
            {
                return getOrMakeForKey1(key);
            }
            set
            {
                if (getOrMakeForKey1(key) == value) return;

                foreach (var pair in value)
                {
                    setForKeys(key, pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// Gets the or make for key1.
        /// </summary>
        /// <param name="key1">The key1.</param>
        /// <returns></returns>
        private ConcurrentDictionary<TD2Key, TValue> getOrMakeForKey1(TD1Key key1)
        {
            ConcurrentDictionary<TD2Key, TValue> output = null;
            while (!items.TryGetValue(key1, out output))
            {
                if (!items.ContainsKey(key1))
                {
                    items.TryAdd(key1, new ConcurrentDictionary<TD2Key, TValue>());
                }
                Thread.SpinWait(1);
            }
            return output;
        }

        /// <summary>
        /// Gets for keys.
        /// </summary>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <returns></returns>
        private TValue getForKeys(TD1Key key1, TD2Key key2)
        {
            var forKey1 = getOrMakeForKey1(key1);

            TValue output = default(TValue);

            while (!forKey1.TryGetValue(key2, out output))
            {
                if (!forKey1.ContainsKey(key2))
                {
                    forKey1.TryAdd(key2, default(TValue));
                }
                Thread.SpinWait(1);
            }

            return output;
        }

        /// <summary>
        /// Sets for keys.
        /// </summary>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <param name="item">The item.</param>
        private void setForKeys(TD1Key key1, TD2Key key2, TValue item)
        {
            var forKey1 = getOrMakeForKey1(key1);
            if (!forKey1.ContainsKey(key2))
            {
                forKey1.TryAdd(key2, item);
            }
            else
            {
                forKey1[key2] = item;
            }
        }

        public IEnumerator<KeyValuePair<TD1Key, ConcurrentDictionary<TD2Key, TValue>>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TD1Key, ConcurrentDictionary<TD2Key, TValue>>>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TD1Key, ConcurrentDictionary<TD2Key, TValue>>>)items).GetEnumerator();
        }

        private ConcurrentDictionary<TD1Key, ConcurrentDictionary<TD2Key, TValue>> _items = new ConcurrentDictionary<TD1Key, ConcurrentDictionary<TD2Key, TValue>>();

        /// <summary>
        ///
        /// </summary>
        protected ConcurrentDictionary<TD1Key, ConcurrentDictionary<TD2Key, TValue>> items
        {
            get
            {
                //if (_items == null)_items = new Dictionary<TD1Key, Dictionary<TD2Key, TValue>>();
                return _items;
            }
            set { _items = value; }
        }
    }
}