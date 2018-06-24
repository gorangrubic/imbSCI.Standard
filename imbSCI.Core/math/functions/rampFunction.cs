// --------------------------------------------------------------------------------------------------------------------
// <copyright file="rampFunction.cs" company="imbVeles" >
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

namespace imbSCI.Core.math.functions
{
    /// <summary>
    /// Ramp waveform function: starts with <see cref="functionBase.outputRange"/> minimum and ends with maximum value.
    /// </summary>
    /// <seealso cref="imbSCI.Core.math.functions.core.functionBase" />
    public class rampFunction : functionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="rampFunction"/> class.
        /// </summary>
        public rampFunction() : base(numberRangePresetEnum.zeroToOne)
        {
        }

        /// <summary>
        /// Gets or sets the alpha dimension scale: relevant only when <see cref="GetOutput(int)"/> -- integer input alpha is used
        /// </summary>
        /// <value>
        /// The alpha dimension.
        /// </value>
        public imbNumberScale alphaDimension { get; set; } = new imbNumberScale(numberRangePresetEnum.zeroToOne);

        /// <summary>
        /// Gets the output -- <c>alpha</c> should be 0-1, it is decimal phase position
        /// </summary>
        /// <param name="alpha">Decimal phase position</param>
        /// <returns></returns>
        public override double GetOutput(double alpha = 0)
        {
            return outputRange.getAbsoluteValue(alpha, true);
        }

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <param name="alpha">The alpha.</param>
        /// <returns></returns>
        public override double GetOutput(int alpha)
        {
            return outputRange.getAbsoluteValue(alphaDimension.getRatioValue(alpha), true);
        }
    }
}