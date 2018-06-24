// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeWeightStylerCategories.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.data;
using imbSCI.Core.math;
using imbSCI.Core.style.color;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace imbSCI.Graph.Converters.tools
{
    /// <summary>
    /// Managing styles for different categories
    /// </summary>
    public class NodeWeightStylerCategories
    {
        public NodeWeightStylerCategories(ColorGradient _gradient, GraphStylerSettings _settings = null)
        {
            if (_settings != null) settings = _settings;

            gradient = _gradient;
        }

        public ColorGradient gradient { get; set; }

        public ColorGradientDictionary gradientDictionary { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public GraphStylerSettings settings { get; set; } = new GraphStylerSettings();

        protected Dictionary<Int32, NodeWeightStyler> stylers = new Dictionary<int, NodeWeightStyler>();

        protected List<Int32> typeIDs { get; set; } = new List<int>();

        /// <summary>
        /// Learns the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="weight">The weight.</param>
        public void learn(Int32 typeGuid, Double weight)
        {
            typeIDs.AddUnique(typeGuid);

            Int32 type = typeIDs.IndexOf(typeGuid);

            if (!stylers.ContainsKey(type))
            {
                stylers.Add(type, new NodeWeightStyler(type, settings.colorWheel.next()));
            }

            stylers[type].learn(weight);
        }

        /// <summary>
        /// Gets the border thickness.
        /// </summary>
        /// <param name="weight">The weight.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public Int32 GetBorderThickness(Double weight, Int32 typeGuid, Boolean inverse = false)
        {
            Int32 type = typeIDs.IndexOf(typeGuid);
            if (inverse)
            {
                return Math.Abs(settings.lineMax - stylers[type].GetThickness(weight, settings));
            }
            return Math.Abs(stylers[type].GetThickness(weight, settings));
        }

        /// <summary>
        /// Gets the color of the hexadecimal.
        /// </summary>
        /// <param name="weight">The weight.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public String GetHexColor(Double weight, Int32 typeGuid, Boolean inverse = false)
        {
            Int32 type = typeIDs.IndexOf(typeGuid);

            if (gradientDictionary == null)
            {
                Int32 m = stylers.Keys.Max();
                gradientDictionary = gradient.GetHexColorDictionary(m + 1);
                foreach (var pair in stylers)
                {
                    pair.Value.color = ColorWorks.GetColor(gradientDictionary.GetColor(pair.Key.GetRatio(m)));
                }
            }

            Byte a = stylers[type].GetAlpha(weight, settings);

            if (inverse) a = (byte)(Byte.MaxValue - a);

            Color c = Color.FromArgb(a, stylers[type].color);

            return c.ColorToHex(); //.toHexColor();
        }
    }
}