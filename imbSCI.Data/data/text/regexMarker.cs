// --------------------------------------------------------------------------------------------------------------------
// <copyright file="regexMarker.cs" company="imbVeles" >
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
namespace imbSCI.Data.data.text
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class regexMarker<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="regexMarker{T}"/> class.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="__m">The m.</param>
        public regexMarker(String regex, T __m)
        {
            marker = __m;
            test = new Regex(regex);
        }

        /// <summary>
        /// Gets or sets the test.
        /// </summary>
        /// <value>
        /// The test.
        /// </value>
        public Regex test { get; set; }

        /// <summary>
        /// Gets or sets the marker.
        /// </summary>
        /// <value>
        /// The marker.
        /// </value>
        public T marker { get; set; }
    }
}