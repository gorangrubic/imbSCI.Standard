// --------------------------------------------------------------------------------------------------------------------
// <copyright file="circularSelector.cs" company="imbVeles" >
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
namespace imbSCI.Data.collection.special
{
    using imbSCI.Data.collection.graph;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;


   

    /// <summary>
    /// Circular and/or random item provider based on index loop.
    /// </summary>
    /// <typeparam name="T">If is Enum - automatic member fill in is performed on constructor call</typeparam>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    /// <seealso cref="System.Collections.IEnumerable" />
    public class circularSelector<T> : IEnumerable
    {

        public circularSelector(List<T> __items)
        {
            foreach (T it in __items)
            {
                items.Add(it);
            }
        }

        /// <summary>
        /// New instance with a number of instances within. the <see cref="circularSelector{T}"/> class.
        /// </summary>
        /// <param name="__items">The items.</param>
        public circularSelector(params T[] __items)
        {
            foreach (T it in __items)
            {
                items.Add(it);
            }
        }

        /// <summary>
        /// Moves the index into starting position
        /// </summary>
        public void reset()
        {
            index = 0;
        }

        /// <summary>
        /// If {T} is Enum it will automatically put all members into this collection.
        /// </summary>
        public circularSelector()
        {
            if (typeof(T).IsEnum)
            {
                var __items = Enum.GetValues(typeof(T));
                foreach (T it in __items)
                {
                    items.Add(it);
                }
            }
        }

        /// <summary>
        /// items Count
        /// </summary>
        /// <returns></returns>
        public Int32 Count()
        {
            return items.Count;
        }

        /// <summary>
        /// Gets the <see cref="Random"/> instance
        /// </summary>
        /// <value>
        /// The random.
        /// </value>
        protected Random rnd
        {
            get
            {
                Random outrnd = new Random();
                return outrnd;
            }
        }

        /// <summary>
        /// Gets random instance
        /// </summary>
        /// <returns></returns>
        public T random()
        {
            return next(rnd.Next(items.Count()));
        }

        /// <summary>
        /// Gets the alternative item relative to <c>from</c> item for <c>altSteps</c>
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="altSteps">The alt steps.</param>
        /// <returns></returns>
        public T getAlternative(T from, Int32 altSteps)
        {
            Int32 oldIndex = index;
            index = items.IndexOf(from);
            T output = next(altSteps);
            index = oldIndex;
            return output;
        }

        /// <summary>
        /// Gets next item according to circular order
        /// </summary>
        /// <param name="steps">The steps.</param>
        /// <returns></returns>
        public T next(Int32 steps = 1)
        {
            if (!items.Any()) return default(T);
            index = index + steps;
            if (index >= items.Count) index = 0;

            return items[index];
        }

        /// <summary>
        /// Adds the specified new item into circular collection
        /// </summary>
        /// <param name="newItem">The new item.</param>
        public void Add(T newItem)
        {
            items.Add(newItem);
        }

        /// <summary>
        /// Adds the specified items
        /// </summary>
        /// <param name="newItem">The new item.</param>
        public void Add(params T[] newItem)
        {
            foreach (T it in newItem)
            {
                items.Add(it);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// Current index
        /// </summary>
        protected Int32 index = 0;

        /// <summary>
        /// The items in the collection
        /// </summary>
        protected List<T> items = new List<T>();
    }
}