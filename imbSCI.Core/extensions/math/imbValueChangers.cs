// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbValueChangers.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.math
{
    using imbSCI.Core.extensions.typeworks;
    using System;

    /// <summary>
    /// Special value manipulators - math
    /// </summary>
    public static class imbValueChangers
    {
        /// <summary>
        /// Checks the range - makes sure that input value is inside min and max (including exact min and max)
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="doLoop">if set to <c>true</c> [do loop].</param>
        /// <returns>Corrected value of input accorting to setum</returns>
        public static Int32 checkRange(this Int32 input, Int32 max, Int32 min = 0, Boolean doLoop = true)
        {
            Int32 output = input;

            if (max < min) max = min + 1;

            while (output < min)
            {
                if (doLoop)
                {
                    output = max + output;
                }
                else
                {
                    output = min;
                }
            }

            while (output > max)
            {
                if (doLoop)
                {
                    output = output - max;
                }
                else
                {
                    output = max;
                }
            }
            return output;
        }

        /// <summary>
        /// Moves the in range.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="step">The step.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="doLoop">if set to <c>true</c> [do loop].</param>
        /// <returns></returns>
        public static Int32 moveInRange(this Int32 input, Int32 step, Int32 max, Int32 min = 0, Boolean doLoop = true)
        {
            Int32 output = input + step;

            return checkRange(output, max, min, doLoop);
        }

        /// <summary>
        /// Changes the value as int32.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="step">The step.</param>
        /// <returns></returns>
        public static Int32 changeValueAsInt32(this object target, Int32 step)
        {
            Int32 output = (Int32)target.imbToNumber(typeof(Int32));
            output = output + step;
            return output;
        }

        /// <summary>
        /// Step je 0.01
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="step">The step.</param>
        /// <param name="stepResolution">The step resolution.</param>
        /// <returns></returns>
        public static Double changeValueDouble(this object target, Int32 step, Double stepResolution = 0.1)
        {
            Double output = (Double)target.imbToNumber(typeof(Double));
            output = output + (step * stepResolution);
            return output;
        }
    }
}