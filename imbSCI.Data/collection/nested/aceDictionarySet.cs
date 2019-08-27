// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceDictionarySet.cs" company="imbVeles" >
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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;


    /// <summary>
    /// Based on aceConcurrentBag
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Concurrent.ConcurrentDictionary{TEnum, aceCommonTypes.collection.nested.aceConcurrentBag{T}}" />
    public class aceDictionarySet<TEnum, T> : ConcurrentDictionary<TEnum, aceConcurrentBag<T>>
    {
        public aceDictionarySet() : base()
        {
            Type tenum = typeof(TEnum);

            if (tenum.IsEnum)
            {
                foreach (Object it in Enum.GetValues(tenum))
                {
                    this.TryAdd((TEnum)it, new aceConcurrentBag<T>());
                    //  Add(, new aceConcurrentBag<T>());
                }
            }
        }

        public void Add(ILookup<TEnum, T> source)
        {
            foreach ( IGrouping<TEnum, T> l in source)
            {
                Add(l.Key, l);
            }
        }

        public void Add(TEnum key, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Add(key, item);
            }
        }

        public void AddUnique(TEnum key, T item)
        {
            if (!ContainsKey(key))
            {
                this.TryAdd(key, new aceConcurrentBag<T>());
            }

            base[key].AddUnique(item);
        }

        public void Add(TEnum key, T link)
        {
            if (!ContainsKey(key))
            {
                this.TryAdd(key, new aceConcurrentBag<T>());
            }

            base[key].Add(link);
        }

        public aceConcurrentBag<T> this[TEnum key]
        {
            get
            {
                if (!ContainsKey(key))
                {
                    this.TryAdd(key, new aceConcurrentBag<T>());
                }
                return base[key];
            }
        }
    }
}