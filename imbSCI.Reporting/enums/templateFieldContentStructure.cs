// --------------------------------------------------------------------------------------------------------------------
// <copyright file="templateFieldContentStructure.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.enums
{
    /// <summary>
    /// Fields of documentSet structure report
    /// </summary>
    public enum templateFieldContentStructure
    {
        /// <summary>
        /// document count - total number of documents in set
        /// </summary>
        str_documentCount,

        /// <summary>
        ///  page count - total number of pages
        /// </summary>
        str_pageCount,

        /// <summary>
        /// block count - total number of blocks
        /// </summary>
        str_blockCount,

        /// <summary>
        /// script count - number of script instructions
        /// </summary>
        str_scriptCount,

        /// <summary>
        ///  render name - name of rendering engine
        /// </summary>
        str_renderName,

        /// <summary>
        /// Code Name of theme
        /// </summary>
        str_themeCode,
    }
}