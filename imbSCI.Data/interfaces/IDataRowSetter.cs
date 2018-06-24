// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataRowSetter.cs" company="imbVeles" >
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
using System;
using System.Data;

namespace imbSCI.Data.interfaces
{
    public interface IDataRowSetter
    {
        /// <summary>
        /// Builds the data table shema without any row
        /// </summary>
        /// <param name="onlyValues">if set to <c>true</c> it will create columns only for properties</param>
        /// <returns></returns>
        DataTable buildDataTableShema(Boolean onlyValues);

        /// <summary>
        /// Sets the data row -- non reflection row data set. Only if table has column with same name and only values are copied
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        DataRow setDataRow(DataTable target);

        /// <summary>
        /// Builds the data table vertical.
        /// </summary>
        /// <returns></returns>
        DataTable buildDataTableVertical();
    }
}