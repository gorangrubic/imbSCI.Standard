// --------------------------------------------------------------------------------------------------------------------
// <copyright file="templateFieldStyle.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.style.setups
{
    /// <summary>
    /// Group of fields used for remove styler programming
    /// </summary>
    public enum templateFieldStyle
    {
        /// <summary>
        /// The style odd even flags
        /// </summary>
        style_oddEvenFlags,

        /// <summary>
        /// The style minor major flags
        /// </summary>
        style_minorMajorFlags,

        /// <summary>
        /// Width of append
        /// </summary>
        style_width,

        /// <summary>
        /// Height of append
        /// </summary>
        style_height,

        /// <summary>
        /// The style append table option flags
        /// </summary>
        style_appendTableOptionFlags,

        /// <summary>
        /// The style layout color role
        /// </summary>
        style_layoutColorRole,

        /// <summary>
        /// aceColorRole for default color
        /// </summary>
        style_defaultColorRole,

        /// <summary>
        /// To pass theme reference
        /// </summary>
        style_theme,

        /// <summary>
        /// The style container style
        /// </summary>
        style_containerStyle,

        /// <summary>
        /// The style head zone
        /// </summary>
        style_headZone,

        /// <summary>
        /// The style foot zone
        /// </summary>
        style_footZone,

        /// <summary>
        /// The style head extra zone
        /// </summary>
        style_headExtraZone,

        /// <summary>
        /// The style foot extra zone
        /// </summary>
        style_footExtraZone,

        /// <summary>
        /// The style left zone
        /// </summary>
        style_leftZone,

        /// <summary>
        /// The style right zone
        /// </summary>
        style_rightZone,

        /// <summary>
        /// To pass styleForRangeBase objects
        /// </summary>
        style_styler,

        style_appendRole,
        style_headFootFlags,
    }
}