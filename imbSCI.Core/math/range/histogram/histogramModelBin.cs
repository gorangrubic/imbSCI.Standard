// --------------------------------------------------------------------------------------------------------------------
// <copyright file="histogramModelBin.cs" company="imbVeles" >
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

namespace imbSCI.Core.math.range.histogram
{
    /// <summary>
    ///
    /// </summary>
    public class histogramModelBin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="histogramModelBin"/> class.
        /// </summary>
        /// <param name="_lavel">The lavel.</param>
        /// <param name="_start">The start.</param>
        /// <param name="_end">The end.</param>
        /// <param name="_binPlace">The bin place.</param>
        public histogramModelBin(String _lavel, Double _start, Double _end, Int32 _binPlace)
        {
            Label = _lavel;
            start = _start;
            end = _end;
            binPlace = _binPlace;
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public String Label { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public Double start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public Double end { get; set; }

        /// <summary>
        /// Gets or sets the bin place.
        /// </summary>
        /// <value>
        /// The bin place.
        /// </value>
        public Int32 binPlace { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public List<Double> values { get; set; } = new List<double>();

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Boolean Add(Double value)
        {
            if (value > start && value <= end)
            {
                values.Add(value);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public Int32 Count
        {
            get
            {
                return values.Count;
            }
        }
    }
}