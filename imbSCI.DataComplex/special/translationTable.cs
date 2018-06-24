// --------------------------------------------------------------------------------------------------------------------
// <copyright file="translationTable.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.special
{
    using imbSCI.Data.data;
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="imbSCI.Data.data.dataBindableBase" />
    [Serializable]
    public class translationTable<TKey, TValue> : dataBindableBase
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                return byKeys.Count;
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public virtual void Add(TKey key, TValue value)
        {
            if (!_byKeys.ContainsKey(key))
            {
                byKeys.TryAdd(key, value);
                //return;
            }
            if (!_byValues.ContainsKey(value))
            {
                byValues.TryAdd(value, key);
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="notfound">The notfound.</param>
        /// <returns></returns>
        public virtual TValue getValue(TKey key, TValue notfound)
        {
            if (byKeys.ContainsKey(key))
            {
                return byKeys[key];
            }
            else
            {
                return notfound;
            }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="notfound">The notfound.</param>
        /// <returns></returns>
        public virtual TKey getKey(TValue value, TKey notfound)
        {
            if (byValues.ContainsKey(value))
            {
                return byValues[value];
            }
            else
            {
                return notfound;
            }
        }

        #region -----------  byValues  -------  [recnik kljuceva prema objektima]

        private ConcurrentDictionary<TValue, TKey> _byValues = new ConcurrentDictionary<TValue, TKey>();

        /// <summary>
        /// recnik kljuceva prema objektima
        /// </summary>
        // [XmlIgnore]
        [Category("translationTable")]
        [DisplayName("byValues")]
        [Description("recnik kljuceva prema objektima")]
        public ConcurrentDictionary<TValue, TKey> byValues
        {
            get
            {
                return _byValues;
            }
            set
            {
                // Boolean chg = (_byValues != value);
                _byValues = value;
                OnPropertyChanged("byValues");
                // if (chg) {}
            }
        }

        #endregion -----------  byValues  -------  [recnik kljuceva prema objektima]

        #region -----------  byKeys  -------  [recnik objekata prema kljucevima]

        private ConcurrentDictionary<TKey, TValue> _byKeys = new ConcurrentDictionary<TKey, TValue>();

        /// <summary>
        /// recnik objekata prema kljucevima
        /// </summary>
        // [XmlIgnore]
        [Category("translationTable")]
        [DisplayName("byKeys")]
        [Description("recnik objekata prema kljucevima")]
        public ConcurrentDictionary<TKey, TValue> byKeys
        {
            get
            {
                return _byKeys;
            }
            set
            {
                // Boolean chg = (_byKeys != value);
                _byKeys = value;
                OnPropertyChanged("byKeys");
                // if (chg) {}
            }
        }

        #endregion -----------  byKeys  -------  [recnik objekata prema kljucevima]
    }
}