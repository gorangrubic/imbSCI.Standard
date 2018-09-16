// --------------------------------------------------------------------------------------------------------------------
// <copyright file="sineFunction.cs" company="imbVeles" >
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
    /// Function provides sine and cosine values.
    /// </summary>
    /// <seealso cref="imbSCI.Core.math.functions.core.functionBase" />
    public class sineFunction : functionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="sineFunction"/> class.
        /// </summary>
        public sineFunction() : base(numberRangePresetEnum.zeroToOne) { }

        /// <summary>
        /// If true, it will produce cosine value instead of sine
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use cosine]; otherwise, <c>false</c>.
        /// </value>
        public Boolean useCosine { get; set; } = false;

        /// <summary>
        /// Phase offset in decrees
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public Int32 offset { get; set; } = 0;

        /// <summary>
        /// Producing output from decimal phase position (0 is 0 degree, 0.5 is 180 degree... 1.0 is 360)
        /// </summary>
        /// <param name="alpha">The alpha: number of full cycles. e.g. 0.25 is 90 degree</param>
        /// <returns></returns>
        public override double GetOutput(double alpha = 0)
        {
            Int32 ang = Convert.ToInt32(alpha * 360) + offset;
            double angle = Math.PI * ang / 180.0;

            if (useCosine) return outputRange.getAbsoluteValue(Math.Cos(angle));
            return outputRange.getAbsoluteValue(Math.Sin(angle));
        }

        /// <summary>
        /// Produces output from degree input.
        /// </summary>
        /// <param name="alpha">Degrees</param>
        /// <returns></returns>
        public override double GetOutput(int alpha)
        {
            Int32 ang = alpha + offset;
            double angle = Math.PI * ang / 180.0;

            if (useCosine) return outputRange.getAbsoluteValue(Math.Cos(angle));
            return outputRange.getAbsoluteValue(Math.Sin(angle));
        }
    }
}