// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceInt2DStackedMatrixConcurrent.cs" company="imbVeles" >
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
namespace imbSCI.Data.collection.math
{
    using System;

#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.List{System.Collections.Generic.List{System.Int32}}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    /// 2D matrix of integer values, where Y is not preset but contains stacked values. Thread-safe version
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{System.Collections.Generic.List{System.Int32}}" />
    public class aceInt2DStackedMatrixConcurrent : ConcurrentList<ConcurrentList<Int32>>
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.List{System.Collections.Generic.List{System.Int32}}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    {
        /// <summary>
        /// Constructor for serialization, do not use directly
        /// </summary>
        public aceInt2DStackedMatrixConcurrent()
        {
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="xStart">The x start.</param>
        /// <param name="xEnd">The x end.</param>
        /// <param name="value">The value.</param>
        /// <param name="uniqueByY">if set to <c>true</c> [unique by y].</param>
        /// <param name="includingXEnd">if set to <c>true</c> [including x end].</param>
        public void AddRange(Int32 xStart, Int32 xEnd, Int32 value, Boolean uniqueByY, Boolean includingXEnd = false)
        {
            if (includingXEnd) xEnd++;
            for (int i = xStart; i < xEnd; i++)
            {
                var y = GetByX(i);
                if (!uniqueByY || y.Contains(value))
                {
                    y.Add(value);
                }
            }
        }

        /// <summary>
        /// Removes the value or all values from start to end.
        /// </summary>
        /// <param name="xStart">The x start.</param>
        /// <param name="xEnd">The x end.</param>
        /// <param name="value">Value to remove, or -1 to remove all</param>
        /// <param name="includingXEnd">if set to <c>true</c> [including x end].</param>
        public void RemoveRange(Int32 xStart, Int32 xEnd, Int32 value, Boolean includingXEnd = false)
        {
            if (includingXEnd) xEnd++;
            for (int i = xStart; i < xEnd; i++)
            {
                var y = GetByX(i);
                if (value == -1)
                {
                    y.Clear();
                }
                else
                {
                    y.Remove(value);
                }
            }
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'List{T}' that could not be resolved
        /// <summary>
        /// Gets the Y stack by X. It creates automatically new List instance if X was empty. Use this better than <see cref="List{T}"/> indexer.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public ConcurrentList<Int32> GetByX(Int32 x)
#pragma warning restore CS1574 // XML comment has cref attribute 'List{T}' that could not be resolved
        {
            while (x >= this.Count)
            {
                this.Add(new ConcurrentList<int>());
            }

            return this[x];
        }

        /// <summary>
        /// Sets the width.
        /// </summary>
        /// <param name="x">The x.</param>
        protected void SetWidth(Int32 x)
        {
            for (int i = 0; i < x; i++)
            {
                this.Add(new ConcurrentList<int>());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="aceInt2DStackedMatrix"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        public aceInt2DStackedMatrixConcurrent(Int32 x)
        {
            SetWidth(x);
        }
    }
}