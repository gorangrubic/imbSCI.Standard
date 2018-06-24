// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceConcurrentBag.cs" company="imbVeles" >
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
    /// Concurrent Bag with implicit conversion
    /// </summary>
    /// <typeparam name="T">Item to store</typeparam>
    /// <seealso cref="System.Collections.Concurrent.ConcurrentBag{T}" />
    public class aceConcurrentBag<T> : ConcurrentBag<T>
    {
        public aceConcurrentBag()
        {
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="aceConcurrentBag{T}"/> to <see cref="List{T}"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator List<T>(aceConcurrentBag<T> input)
        {
            List<T> output = new List<T>();
            foreach (T item in input)
            {
                output.Add(item);
            }
            return output;
        }

        /// <summary>
        /// The add lock
        /// </summary>
        private Object AddLock = new Object();

        /// <summary>
        /// Adds the unique.
        /// </summary>
        /// <param name="item">The item.</param>
        public void AddUnique(T item)
        {
            if (item == null) return;
            lock (AddLock)
            {
                if (!this.Contains(item)) Add(item);
            }
        }
    }
}