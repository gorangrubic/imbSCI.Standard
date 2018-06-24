// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataTableDeliveryEnum.cs" company="imbVeles" >
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

    /// <summary>
    /// DataTable request rescription
    /// </summary>
    [Flags]
    public enum dataTableDeliveryEnum
    {
        none = 0,

        vertical = 1,

        horizontal = 2,

        /// <summary>
        /// The calculated: instruct provider to perform calculations before making table
        /// </summary>
        calculated = 4,

        /// <summary>
        /// The comparative: tells the provider to include more
        /// </summary>
        comparative = 8,

        singleRow = 16,
        navigation = 32,
        described = 64,
        collection = 128,

        dataTableVertical = vertical | singleRow | described,
        dataTableHorizontal = horizontal | collection,
        singleRowCalculatedSummary = singleRow | calculated | described | horizontal,
        verticalCalculatedSummary = singleRow | calculated | described | vertical,
        comparativeSummary = collection | comparative | vertical | calculated
    }
}