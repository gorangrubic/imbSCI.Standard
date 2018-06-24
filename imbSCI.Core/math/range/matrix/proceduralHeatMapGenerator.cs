// --------------------------------------------------------------------------------------------------------------------
// <copyright file="proceduralHeatMapGenerator.cs" company="imbVeles" >
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

//using Accord.Imaging.Filters;

using imbSCI.Core.math.functions;
using System;

namespace imbSCI.Core.math.range.matrix
{
    /// <summary>
    /// Generator for <see cref="HeatMapModel"/> using X and Y function arrays to set value
    /// </summary>
    public class proceduralHeatMapGenerator
    {
        /// <summary>
        /// Generator for horizontal sine function waves
        /// </summary>
        /// <returns></returns>
        public static proceduralHeatMapGenerator PresetSineWave()
        {
            proceduralHeatMapGenerator output = new proceduralHeatMapGenerator();

            sineFunction fn = new sineFunction();

            output.xAxisFunction.Add(new sineFunction(), 1);
            output.yAxisFunction.Add(new pseudoFunction(0), 1);

            return output;
        }

        /// <summary>
        /// Generates sine function waves in both horizontal and vertical directions
        /// </summary>
        /// <param name="xVsYRatio">Weight of the horizontal sine function, complementary to the vertical sine function. </param>
        /// <returns></returns>
        public static proceduralHeatMapGenerator PresetDoubleSineWave(Double xVsYRatio = 0.8)
        {
            proceduralHeatMapGenerator output = new proceduralHeatMapGenerator();

            sineFunction fn = new sineFunction();

            output.xAxisFunction.Add(new sineFunction(), xVsYRatio);
            output.yAxisFunction.Add(new sineFunction(), 1 - xVsYRatio);

            return output;
        }

        /// <summary>
        /// Gets or sets the x axis function.
        /// </summary>
        /// <value>
        /// The x axis function.
        /// </value>
        public functionArray xAxisFunction { get; set; } = new functionArray();

        /// <summary>
        /// Gets or sets the y axis function.
        /// </summary>
        /// <value>
        /// The y axis function.
        /// </value>
        public functionArray yAxisFunction { get; set; } = new functionArray();

        /// <summary>
        /// Renders HeatMapModel of specified size
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public HeatMapModel MakeHeatMap(Int32 width, Int32 height, Int32 xPeriod = 20, Int32 yPeriod = 20)
        {
            xAxisFunction.outputRange = new imbNumberScale(numberRangePresetEnum.zeroToOne);
            yAxisFunction.outputRange = new imbNumberScale(numberRangePresetEnum.zeroToOne);

            HeatMapModel map = HeatMapModel.Create(width, height, "D3");

            map.AllocateSize(width, height);

            for (Int32 y = 0; y < height; y++)
            {
                Double yValue = yAxisFunction.GetOutput(y.GetRatio(yPeriod));
                for (Int32 x = 0; x < width; x++)
                {
                    map[x, y] = xAxisFunction.GetOutput(x.GetRatio(xPeriod)) + yValue;  //grayImage.GetIntesity(x, y, bs);
                }
            }

            return map;
        }
    }
}