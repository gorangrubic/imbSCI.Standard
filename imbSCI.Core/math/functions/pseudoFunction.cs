// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pseudoFunction.cs" company="imbVeles" >
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
using imbSCI.Core.math.functions.core;
using imbSCI.Core.math.range;
using System;

namespace imbSCI.Core.math.functions
{
    /// <summary>
    /// Returns predefined fixed value
    /// </summary>
    /// <seealso cref="imbSCI.Core.math.functions.core.functionBase" />
    public class pseudoFunction : functionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="pseudoFunction"/> class.
        /// </summary>
        public pseudoFunction() : base(numberRangePresetEnum.zeroToOne)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="pseudoFunction"/> class.
        /// </summary>
        /// <param name="outputValue">The output value.</param>
        public pseudoFunction(Double outputValue) : base(numberRangePresetEnum.zeroToOne)
        {
            output = outputValue;
        }

        public Double output { get; set; } = 0;

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <param name="alpha">The alpha.</param>
        /// <returns></returns>
        public override double GetOutput(double alpha = 0)
        {
            return output;
        }

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <param name="alpha">The alpha.</param>
        /// <returns></returns>
        public override double GetOutput(int alpha)
        {
            return output;
        }
    }
}