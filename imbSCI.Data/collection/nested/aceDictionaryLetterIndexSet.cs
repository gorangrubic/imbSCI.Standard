// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceDictionaryLetterIndexSet.cs" company="imbVeles" >
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
using System;

namespace imbSCI.Data.collection.nested
{
    using System.Collections.Concurrent;
    using System.Text;

    /// <summary>
    /// Dictionary with first letter indexing, usefull for large collections
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class aceDictionaryLetterIndexSet<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="aceDictionaryLetterIndexSet{T}"/> class.
        /// </summary>
        public aceDictionaryLetterIndexSet()
        {
        }

        private Object getByKeyLock = new Object();

#pragma warning disable CS1574 // XML comment has cref attribute 'List{T}' that could not be resolved
#pragma warning disable CS1574 // XML comment has cref attribute 'List{T}' that could not be resolved
        /// <summary>
        /// Gets the <see cref="List{T}"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="List{T}"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual T this[String key]
#pragma warning restore CS1574 // XML comment has cref attribute 'List{T}' that could not be resolved
#pragma warning restore CS1574 // XML comment has cref attribute 'List{T}' that could not be resolved
        {
            get
            {
                lock (getByKeyLock)
                {
                    if (key.isNullOrEmpty()) return default(T);
                    key = key.ToLower();
                    String iKey = IndexPart(key);
                    checkTheKey(key, iKey);

                    if (items[iKey].ContainsKey(key))
                    {
                        return items[iKey][key];
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
        }

        private Object AddByKeyLock = new Object();

        /// <summary>
        /// Adds the item
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="allowReplace">if set to <c>true</c> [check for duplicates].</param>
        public virtual void Add(String key, T item, Boolean allowReplace = false)
        {
            lock (AddByKeyLock)
            {
                if (key.isNullOrEmpty()) return;

                key = key.ToLower();
                String iKey = IndexPart(key);
                checkTheKey(key, iKey);

                if (items[iKey].ContainsKey(key))
                {
                    items[iKey][key] = item;
                }
                else
                {
                    items[iKey].TryAdd(key, item);
                }
            }
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public virtual Boolean ContainsKey(String key)
        {
            if (key.isNullOrEmpty()) return false;
            key = key.ToLower();
            String iKey = IndexPart(key);
            checkTheKey(key, iKey);
            return items[iKey].ContainsKey(key);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public ConcurrentDictionary<String, T> Get(String key)
        {
            key = key.ToLower();
            String iKey = IndexPart(key);
            checkTheKey(key, iKey);

            return items[iKey];
        }

        protected void checkTheKey(String key, String iKey = "")
        {
            if (iKey == "")
            {
                iKey = IndexPart(key);
            }
            if (iKey.isNullOrEmpty()) return;
            if (!items.ContainsKey(iKey))
            {
                items.TryAdd(iKey, new ConcurrentDictionary<string, T>());
            }
            //items[iKey].Add(key, T);
        }

        protected String IndexPart(String key)
        {
            if (key.isNullOrEmpty()) return "";
            return key[0].ToString();
        }

        /// <summary>
        /// Counts indexed first letters
        /// </summary>
        /// <returns></returns>
        public Int32 Count()
        {
            return items.Count;
        }

        /// <summary>
        /// Counts all instances saved in the collection
        /// </summary>
        /// <returns></returns>
        public Int32 CountAll()
        {
            Int32 output = 0;
            foreach (var pair in items)
            {
                output += pair.Value.Count;
            }
            return output;
        }

        /// <summary>
        /// Gets the signature.
        /// </summary>
        /// <returns></returns>
        public String GetSignature()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var pair in items)
            {
                sb.AppendLine(String.Format("{0,-10} : {1,20}", pair.Key, pair.Value.Count));
            }

            return sb.ToString();
        }

        private ConcurrentDictionary<String, ConcurrentDictionary<String, T>> _items = new ConcurrentDictionary<String, ConcurrentDictionary<String, T>>();

        /// <summary> </summary>
        protected ConcurrentDictionary<String, ConcurrentDictionary<String, T>> items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }
    }
}