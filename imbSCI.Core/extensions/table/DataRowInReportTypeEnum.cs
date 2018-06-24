// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRowInReportTypeEnum.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.table
{
    public enum DataRowInReportTypeEnum
    {
        none,
        mergedHeaderTitle,
        mergedHeaderInfo,
        columnCaption,
        columnDescription,
        columnFooterInfo,
        mergedFooterInfo,

        /// <summary>
        /// The merged horizontally: cells with the same value are merged horizontally
        /// </summary>
        mergedHorizontally,

        info,
        data,
        dataAggregate,
        columnInformation,
        mergedCategoryHeader,

        /// <summary>
        /// The data highlight a: OrangeRed by default style
        /// </summary>
        dataHighlightA,

        /// <summary>
        /// The data highlight b: CadetBlue by default style
        /// </summary>
        dataHighlightB,

        /// <summary>
        /// The data highlight c: SteelBlue by default style
        /// </summary>
        dataHighlightC,

        removedLight,
        removedStrong,

        /// <summary>
        /// The group01: lightblue
        /// </summary>
        group01,

        /// <summary>
        /// The group02: bisque
        /// </summary>
        group02,

        /// <summary>
        /// The group03: lightcyan
        /// </summary>
        group03,

        /// <summary>
        /// The group04: light golded rod
        /// </summary>
        group04,

        /// <summary>
        /// The group05: light pink
        /// </summary>
        group05
    }
}