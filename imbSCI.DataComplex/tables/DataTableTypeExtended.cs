// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTableTypeExtended.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
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

namespace imbSCI.DataComplex.tables
{
    using imbSCI.Core.extensions.text;

    /// <summary>
    /// Simple typed data table implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="imbSCI.DataComplex.tables.DataTableExtended" />
    public class DataTableTypeExtended<T> : DataTableExtended
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataTableTypeExtended() : base(typeof(T), typeof(T).Name.imbTitleCamelOperation(true), "Data table with [" + typeof(T).Name + "] records")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableTypeExtended{T}"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="primaryKeyName">Name of the primary key.</param>
        public DataTableTypeExtended(string title, String description = "") : base(typeof(T), title, description)
        {
        }

        /// <summary>
        /// Adds new row into the table
        /// </summary>
        /// <param name="input">The input.</param>
        public void AddRow(T input)
        {
            addRow(input);
        }
    }
}