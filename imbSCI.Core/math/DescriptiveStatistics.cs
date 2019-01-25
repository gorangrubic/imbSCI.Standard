// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DescriptiveStatistics.cs" company="imbVeles" >
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
using imbSCI.Core.reporting.render;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.math
{

    /// <summary>
    /// 
    /// </summary>
    public class DescriptiveStatistics
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptiveStatistics"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="isSample">if set to <c>true</c> [is sample].</param>
        public DescriptiveStatistics(IEnumerable<double> data, Boolean isSample)
        {

            Mean = data.Average();
            Maximum = Double.MinValue;
            Minimum = Double.MaxValue;


            Double above = 0;
            Int32 c = 0;
            foreach (Double rF in data)
            {
                Double d = rF - Mean;
                above += (d * d);
                c++;

                Maximum = Math.Max(Maximum, rF);
                Minimum = Math.Min(Minimum, rF);
            }

            if (c == 1)
            {
                Variance = 0;
            }

            if (above == 0)
            {
                Variance = 0;
            }

            if (!isSample)
            {
                Variance = above / c;
            }
            else
            {
                Variance = above / (c - 1);
            }

            StandardDeviation = Math.Sqrt(Variance);


        }

        public void Describe(ITextRender textRender)
        {
            textRender.AppendPair(nameof(Count), Count);
            textRender.AppendPair(nameof(Mean), Mean);
            textRender.AppendPair(nameof(Variance), Variance);
            textRender.AppendPair(nameof(StandardDeviation), StandardDeviation);
            textRender.AppendPair(nameof(Maximum), Maximum);
            textRender.AppendPair(nameof(Minimum), Minimum);
        }


        public long Count { get; }

        public double Mean { get; }


        public double Variance { get; }


        public double StandardDeviation { get; }


        public double Maximum { get; }

        public double Minimum { get; }
    }

}