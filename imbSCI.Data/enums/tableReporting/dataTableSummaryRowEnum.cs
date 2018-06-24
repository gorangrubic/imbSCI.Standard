// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataTableSummaryRowEnum.cs" company="imbVeles" >
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
namespace imbSCI.Data.enums.tableReporting
{
    using System;

    [Flags]
    public enum dataTableSummaryRowEnum
    {
        none = 0,
        sum = 1,
        mean = 2,
        max = 4,
        min = 8,
        range = 16,
        entropy = 32,
        variance = 64,
        diversity = 128,
        distinctValueCount = 256,

        /// <summary>
        /// The last value
        /// </summary>
        theLast = 512,

        /// <summary>
        /// The first value entry
        /// </summary>
        theFirst = 1024,
    }
}