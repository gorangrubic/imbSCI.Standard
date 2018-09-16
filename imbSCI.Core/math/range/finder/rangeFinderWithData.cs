// --------------------------------------------------------------------------------------------------------------------
// <copyright file="rangeFinderWithData.cs" company="imbVeles" >
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
using System;
using System.Collections.Generic;

namespace imbSCI.Core.math.range.finder
{
    /// <summary>
    /// Version of <see cref="rangeFinder"/> that stores all values sent trough <see cref="doubleEntries"/>
    /// </summary>
    /// <seealso cref="imbWBI.Core.math.rangeFinder" />
    public class rangeFinderWithData : rangeFinder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="rangeFinderWithData"/> class.
        /// </summary>
        public rangeFinderWithData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="rangeFinderWithData"/> class.
        /// </summary>
        /// <param name="_id">The identifier.</param>
        public rangeFinderWithData(String _id) : base(_id)
        {
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public override void Reset()
        {
            doubleEntries.Clear();
            base.Reset();
        }

        /// <summary>
        /// Updates the instance with data
        /// </summary>
        /// <param name="input">The input.</param>
        public override void Learn(double input)
        {
            doubleEntries.Add(input);
            base.Learn(input);
        }

        /// <summary>
        /// The double entries
        /// </summary>
        public List<Double> doubleEntries { get; set; } = new List<double>();
    }
}