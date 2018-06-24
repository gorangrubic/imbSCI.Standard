// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BibTexEntryTag.cs" company="imbVeles" >
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
// Project: imbSCI.BibTex
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
using System.Linq;
using System.Collections.Generic;

namespace imbSCI.BibTex
{

    /// <summary>
    /// KeyValue entry, a property of a <see cref="BibTexEntryBase"/>
    /// </summary>
    public class BibTexEntryTag
    {
        /// <summary>
        /// Initializes a new blank instance of the <see cref="BibTexEntryTag"/> class.
        /// </summary>
        public BibTexEntryTag() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BibTexEntryTag"/> class.
        /// </summary>
        /// <param name="_key">The key.</param>
        /// <param name="_value">The value.</param>
        public BibTexEntryTag(String _key, String _value)
        {
            Key = _key;
            Value = _value;
        }

        /// <summary>
        /// Property name
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public String Key { get; set; }

        /// <summary>
        /// Clean, unicode version of the value, associated with the <see cref="Key"/>
        /// </summary>
        /// <value>
        /// The value - as written/read in the BibTex file
        /// </value>
        public String Value { get; set; }

        /// <summary>
        /// Source version of the value. Compared to the <see cref="Value"/>, it contains LaTeX symbol tags instead of Unicode equivalents
        /// </summary>
        /// <value>
        /// The source - content of the tag
        /// </value>
        public String source { get; set; }
    }

}