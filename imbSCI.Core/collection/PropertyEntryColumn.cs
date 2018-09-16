// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyEntryColumn.cs" company="imbVeles" >
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
namespace imbSCI.Core.collection
{
    using System;

    /// <summary>
    /// Column/data aspect of the PropertyEntry
    /// </summary>
    [Flags]
    public enum PropertyEntryColumn
    {
        none = 0,

        /// <summary>
        /// Original version of entry key
        /// </summary>
        entry_key = 8192,

        /// <summary>
        /// Display version of entry name
        /// </summary>
        entry_name = 1,

        /// <summary>
        /// Asociated entry description
        /// </summary>
        entry_description = 2,

        /// <summary>
        /// Display version of entry value - formatted but without unit
        /// </summary>
        entry_value = 4,

        /// <summary>
        /// Display version of entry value, formatted with unit
        /// </summary>
        entry_valueAndUnit = 8,

        /// <summary>
        /// Measure role letter
        /// </summary>
        role_letter = 16,

        /// <summary>
        /// Measure symbol
        /// </summary>
        role_symbol = 32,

        /// <summary>
        /// Measure role name
        /// </summary>
        role_name = 64,

        entry_unit = 128,

        entry_unitname = 256,

        entry_importance = 512,

        autocount_idcolumn = 1024,
        valueType = 2048,
        property_description = 8193,
    }
}