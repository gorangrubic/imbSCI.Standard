// --------------------------------------------------------------------------------------------------------------------
// <copyright file="resourceDictionaryBase.cs" company="imbVeles" >
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
namespace imbSCI.Core.collection
{
    using imbSCI.Data.data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Simple generic dictionary used by: <see cref="aceCommonTypes.collection.reportOutputRepository"/>, <see cref="PropertyCollectionDictionary"/>/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    public abstract class resourceDictionaryBase<T> : dataBindableBase, IEnumerable
    {
        /// <summary>
        /// The items
        /// </summary>
        protected Dictionary<String, T> items = new Dictionary<string, T>();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <returns></returns>
        public Int32 Count() => items.Count();

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear() => items.Clear();

        /// <summary>
        /// Gets the <see cref="T"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T this[Enum key] => this[key.ToString()];

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The value.</param>
        public void Add(String key, T val) => items.Add(key, val);

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <returns></returns>
        protected abstract T getDefault();

        /// <summary>
        /// Gets or sets the <see cref="PropertyCollection"/> with the specified key. Automatically creates entry for new key
        /// </summary>
        /// <value>
        /// The <see cref="PropertyCollection"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T this[String key]
        {
            get
            {
                if (!items.ContainsKey(key)) items.Add(key, getDefault());
                return items[key];
            }
            set
            {
                if (!items.ContainsKey(key))
                {
                    items[key] = value;
                }
                else
                {
                    items.Add(key, value);
                }
            }
        }
    }
}