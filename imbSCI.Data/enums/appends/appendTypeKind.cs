// --------------------------------------------------------------------------------------------------------------------
// <copyright file="appendTypeKind.cs" company="imbVeles" >
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
namespace imbSCI.Data.enums.appends
{
    public enum appendTypeKind
    {
        /// <summary>
        /// The none - not for execution
        /// </summary>
        none,

        /// <summary>
        /// The simple: simple string appends and headings
        /// Without prefix
        /// </summary>
        simple,

        ///// <summary>
        ///// The data: DataTable, Pairs, Lists
        ///// </summary>
        //data,
        ///// <summary>
        ///// The structure: add page, add document, add...
        ///// </summary>
        //structure,
        /// <summary>
        /// The style: generate style, variations... <c>s_</c> prefix
        /// </summary>
        style,

        /// <summary>
        /// Starting with <c>c_</c>
        /// The complex: section, line...
        /// The data: DataTable, Pairs, Lists
        /// </summary>
        complex,

        ///// <summary>
        ///// The file
        ///// </summary>
        //file,

        /// <summary>
        /// starting with <c>i_</c> and <c>x_</c>The special and  The structure: add page, add document, add...
        /// </summary>
        special,

        /// <summary>
        /// The other
        /// </summary>
        other,
    }
}