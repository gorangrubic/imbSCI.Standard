// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphStylerSettings.cs" company="imbVeles" >
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
using imbSCI.Core.style.color;
using imbSCI.Data.collection.special;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;

namespace imbSCI.Graph.Converters.tools
{
    public enum ColorTransformationMode
    {
        none = 0,
        alphaMinMax = 1,
        colorCircle = 2,
        colorSet = 4,

        /// <summary>
        /// Uses the color gradient
        /// </summary>
        colorGradient = 8,

        /// <summary>
        /// Takes Category's <see cref="Category.ExplicitColor"/> as base color (ColorA) of <see cref="ColorGradient"/> and applies rules of the gradient
        /// </summary>
        explicitColorViaGradient = 16
    }

    /// <summary>
    /// Graph styler
    /// </summary>
    public class GraphStylerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphStylerSettings"/> class.
        /// </summary>
        public GraphStylerSettings()
        {
        }

        public DGML.enums.GraphLayoutEnum GraphLayout { get; set; } = DGML.enums.GraphLayoutEnum.Sugiyama;
        public DGML.enums.GraphDirectionEnum GraphDirection { get; set; } = DGML.enums.GraphDirectionEnum.Sugiyama;

        public Boolean doAddLinkWeightInTheLabel { get; set; } = false;

        public Boolean doLinkDirectionFromLowerTypeToHigher { get; set; } = true;
        public Boolean doAddNodeTypeToLabel { get; set; } = true;

        public String NodeWeightFormat { get; set; } = "F5";
        public String LinkWeightFormat { get; set; } = "F2";

        public ColorGradient NodeGradient { get; set; } = ColorGradient.ColorCircleCW;
        public ColorGradient LinkGradient { get; set; } = ColorGradient.RedBlueAtoBPreset;

        public Int32 gradientResolution { get; set; } = 10;

        /// <summary>
        /// The colors
        /// </summary>
        public List<Color> colorsForCircularSelector = new List<Color>();

        /// <summary>
        /// Gets or sets the alpha maximum.
        /// </summary>
        /// <value>
        /// The alpha maximum.
        /// </value>
        public Double alphaMax { get; set; } = 1;

        /// <summary>
        /// Gets or sets the alpha minimum.
        /// </summary>
        /// <value>
        /// The alpha minimum.
        /// </value>
        public Double alphaMin { get; set; } = 0.5;

        /// <summary>
        /// Gets or sets the line minimum.
        /// </summary>
        /// <value>
        /// The line minimum.
        /// </value>
        public Int32 lineMin { get; set; } = 1;

        /// <summary>
        /// Gets or sets the line maximum.
        /// </summary>
        /// <value>
        /// The line maximum.
        /// </value>
        public Int32 lineMax { get; set; } = 3;

        //public List<Color> colorsForCircularSelector { get; set; } = new List<Color>();

        private circularSelector<Color> _colorWheel = null;

        [XmlIgnore]
        public circularSelector<Color> colorWheel
        {
            get
            {
                if (_colorWheel == null)
                {
                    _colorWheel = new circularSelector<Color>();
                    var cls = new List<Color>();
                    cls.AddRange(colorsForCircularSelector);
                    if (!cls.Any())
                    {
                        cls.AddRange(new Color[] {  Color.Orange, Color.Teal, Color.Gray, Color.SeaGreen, Color.SteelBlue, Color.OrangeRed, Color.DarkOrange,
    Color.SlateBlue});
                    }
                    cls.ForEach(x => _colorWheel.Add(x));
                }
                return _colorWheel;
            }
        }
    }
}