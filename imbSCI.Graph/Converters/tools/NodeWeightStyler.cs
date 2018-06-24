// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeWeightStyler.cs" company="imbVeles" >
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
// Project: imbSCI.Graph
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using imbSCI.Core.math;
using System;
using System.Drawing;

namespace imbSCI.Graph.Converters.tools
{
    /// <summary>
    /// Node Weight styler
    /// </summary>
    public class NodeWeightStyler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeWeightStyler"/> class.
        /// </summary>
        /// <param name="_type">The type.</param>
        /// <param name="_color">The color.</param>
        public NodeWeightStyler(Int32 _type, Color _color)
        {
            type = _type;
            color = _color;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Int32 type { get; set; } = 0;

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color color { get; set; }

        /// <summary>
        /// Gets the thickness.
        /// </summary>
        /// <param name="weight">The weight.</param>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public Int32 GetThickness(Double weight, GraphStylerSettings s)
        {
            Double w = weight - min;
            w = w.GetRatio(range);

            w = s.lineMin + ((s.lineMax - s.lineMin) * s.lineMax);

            return Convert.ToInt32(w);
        }

        /// <summary>
        /// Gets the alpha.
        /// </summary>
        /// <param name="weight">The weight.</param>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public Byte GetAlpha(Double weight, GraphStylerSettings s)
        {
            Double w = weight - min;
            w = w.GetRatio(range);
            if (w > 1)
            {
                w = 1;
            }
            w = s.alphaMin + ((s.alphaMax - s.alphaMin) * w);
            Byte b = Convert.ToByte(w * Byte.MaxValue);
            return b;
        }

        /// <summary>
        /// Learns the specified weight.
        /// </summary>
        /// <param name="weight">The weight.</param>
        public void learn(Double weight)
        {
            min = Math.Min(weight, min);
            max = Math.Max(weight, max);
        }

        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <value>
        /// The range.
        /// </value>
        public Double range
        {
            get
            {
                return max - min;
            }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public Double min { get; set; } = Double.MaxValue;

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        public Double max { get; set; } = Double.MinValue;
    }
}