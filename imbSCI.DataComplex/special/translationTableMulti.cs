// --------------------------------------------------------------------------------------------------------------------
// <copyright file="translationTableMulti.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Data.data;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Translation table with multiple key capability
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="dataBindableBase" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{TKey, TValue}}" />
    [Serializable]
    public class translationTableMulti<TKey, TValue> : dataBindableBase, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public Int32 Count
        {
            get
            {
                return entries.Count;
            }
        }

        public virtual void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            foreach (var pair in source)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public virtual void Add(TKey key, TValue value)
        {
            KeyValuePair<TKey, TValue> t = new KeyValuePair<TKey, TValue>(key, value);
            //if (!entries.Any(x=>(x.Key.Equals(t.Key))&&(x.Value.Equals(t.Value))))
            //{
            entries.Add(t);
            //}
        }

        public Boolean ContainsKey(TKey key)
        {
            foreach (var pair in entries)
            {
                if (pair.Key.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean ContainsValue(TValue val)
        {
            foreach (var pair in entries)
            {
                if (pair.Value.Equals(val))
                {
                    return true;
                }
            }
            return false;
        }

        public List<TKey> GetKeys()
        {
            List<TKey> output = new List<TKey>();
            foreach (var pair in entries)
            {
                output.AddUnique(pair.Key);
            }
            return output;
        }

        public List<TValue> GetValues()
        {
            List<TValue> output = new List<TValue>();
            foreach (var pair in entries)
            {
                output.AddUnique(pair.Value);
            }
            return output;
        }

        public virtual List<TValue> GetByKey(TKey needle)
        {
            List<TValue> output = new List<TValue>();
            foreach (var pair in entries)
            {
                if (pair.Key.Equals(needle))
                {
                    output.AddUnique(pair.Value);
                }
            }
            return output;
        }

        public virtual TKey GetOfTypeByValue(TValue needle, Type t = null)
        {
            List<TKey> output = new List<TKey>();
            if (t == null) t = typeof(TKey);
            foreach (var pair in entries)
            {
                if (pair.Value.Equals(needle))
                {
                    output.AddUnique(pair.Key);
                }
            }

            foreach (var o in output)
            {
                if (o.GetType() == t)
                {
                    return o;
                }
            }

            return default(TKey);
        }

        public virtual TValue GetOfTypeByKey(TKey needle, Type t = null)
        {
            List<TValue> output = new List<TValue>();
            if (t == null) t = typeof(TValue);
            foreach (var pair in entries)
            {
                if (pair.Key.Equals(needle))
                {
                    output.AddUnique(pair.Value);
                }
            }

            foreach (var o in output)
            {
                if (o.GetType() == t)
                {
                    return o;
                }
            }

            return default(TValue);
        }

        public virtual List<TKey> GetByValue(TValue needle)
        {
            List<TKey> output = new List<TKey>();
            foreach (var pair in entries)
            {
                if (pair.Value.Equals(needle))
                {
                    output.AddUnique(pair.Key);
                }
            }
            return output;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)entries).GetEnumerator();
        }

        /// <summary> </summary>
        protected List<KeyValuePair<TKey, TValue>> entries { get; set; } = new List<KeyValuePair<TKey, TValue>>();
    }
}