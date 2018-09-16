// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileTextInMemoryBlockUnit.cs" company="imbVeles" >
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

namespace imbSCI.Core.files.search
{
    /// <summary>
    /// Internal use. Collection of content lines, separated into blocks.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{System.String}" />
    public class fileTextInMemoryBlockUnit : List<String>
    {
        private Int32 _lineStart;

        /// <summary>
        /// Index of the line (in original content) when this block starts
        /// </summary>
        public Int32 lineStart
        {
            get { return _lineStart; }
            set { _lineStart = value; }
        }

        /// <summary>
        /// Initializes empty block unit, that starts at <c>__ln</c>
        /// </summary>
        /// <param name="__ln">The ln.</param>
        public fileTextInMemoryBlockUnit(Int32 __ln)
        {
            lineStart = __ln;
        }
    }
}