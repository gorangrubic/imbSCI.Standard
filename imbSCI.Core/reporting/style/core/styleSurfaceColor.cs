// --------------------------------------------------------------------------------------------------------------------
// <copyright file="styleSurfaceColor.cs" company="imbVeles" >
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
using System;

namespace imbSCI.Core.reporting.style.core
{
    using imbSCI.Core.reporting.style.enums;
    using System.Drawing;

    /// <summary>
    ///
    /// </summary>
    public class styleSurfaceColor
    {
        /// <summary>
        /// Gets or sets the type of the fill.
        /// </summary>
        /// <value>
        /// The type of the fill.
        /// </value>
        public styleFillType FillType { get; set; } = styleFillType.Solid;

        /// <summary>
        /// Gets or sets the tint.
        /// </summary>
        /// <value>
        /// The tint.
        /// </value>
        public Double Tint { get; set; } = 1;

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color { get; set; } = Color.Gray;

        public styleSurfaceColor()
        {
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public styleSurfaceColor Clone()
        {
            styleSurfaceColor output = new styleSurfaceColor();

            output.FillType = FillType;
            output.Tint = Tint;
            output.Color = Color;

            return output;
        }

        //style.Style.Font.Name = styleSet.fontName;

        //    style.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    style.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);
        //    style.Style.Fill.BackgroundColor.Tint = new decimal (0.8);
        //    style.Style.Font.Bold = false;
        //    style.Style.Font.Size = 10;
    }
}