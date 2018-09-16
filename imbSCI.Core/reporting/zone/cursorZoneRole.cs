// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cursorZoneRole.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.zone
{
    /// <summary>
    /// Role that a zone may have inside another zone
    /// </summary>
    public enum cursorZoneRole
    {
        /// <summary>
        /// The parent zone for all zones
        /// </summary>
        master,

        /// <summary>
        /// The header - top most section with given height
        /// </summary>
        header,

        /// <summary>
        /// The footer - bottom most section with given height
        /// </summary>
        footer,

        /// <summary>
        /// The section - horizontal division of a zone where height is defined as part of parent zone
        /// </summary>
        section,

        /// <summary>
        /// The column - vertical division of a zone where width is defined as part of parent zone
        /// </summary>
        column,
    }
}