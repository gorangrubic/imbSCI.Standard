// --------------------------------------------------------------------------------------------------------------------
// <copyright file="acePaletteVariationRole.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.colors
{
    /// <summary>
    /// Role per variation value
    /// </summary>
    public enum acePaletteVariationRole
    {
        /// <summary>
        /// The normal content> text etc
        /// </summary>
        normal = 0,

        /// <summary>
        /// Odd tr element
        /// </summary>
        odd = 1,

        /// <summary>
        /// Even tr element
        /// </summary>
        ///         even = 2,
        /// <summary>
        /// The heading tags
        /// </summary>
        even = 2,

        important = 3,

        /// <summary>
        /// The heading tags
        /// </summary>
        heading = 4,

        /// <summary>
        /// The th elements
        /// </summary>
        header = 5,

        /// <summary>
        /// Must stay the last enumeration --
        /// </summary>
        none = 6,
    }
}