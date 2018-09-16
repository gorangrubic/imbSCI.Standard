// --------------------------------------------------------------------------------------------------------------------
// <copyright file="sequenceFunction.cs" company="imbVeles" >
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
using System.Collections.Generic;

namespace imbSCI.Core.math.functions
{
    /// <summary>
    /// Repeats sequence of pre-set output values.
    /// </summary>
    /// <seealso cref="imbSCI.Core.math.functions.core.functionBase" />
    public class sequenceFunction : functionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="sequenceFunction"/> class. Constructor for XmlSerialization, better use: <see cref="sequenceFunction"/>
        /// </summary>
        public sequenceFunction() : base(numberRangePresetEnum.zeroToOne) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="sequenceFunction"/> class.
        /// </summary>
        /// <param name="firstStep">The first step value, the step at index = 0</param>
        public sequenceFunction(Double firstStep) : base(numberRangePresetEnum.zeroToOne)
        {
            AddStep(firstStep);
        }

        /// <summary>
        /// Adds new value into step sequence
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddStep(Double value)
        {
            steps.Add(value);
            //   alphaDimension.minValue = 0;
            //  alphaDimension.maxValue = steps.Count;
        }

        /// <summary>
        /// The steps in the sequence
        /// </summary>
        public List<Double> steps = new List<double>();

        /// <summary>
        /// Computes sequence step from decimal phase ratio. e.g. alpha=0.5 for sequence of 10 steps will return value for step 5. <see cref="steps"/>
        /// </summary>
        /// <param name="alpha">Decimal phase of the sequence, e.g. 0.25 gets 2nd step of sequence having 8 steps.</param>
        /// <remarks>
        /// <para>Negative <c>alpha</c> will return n-th step from the end (reversed query)</para>
        /// <para>Input is recomputed: alpha % 1. e.g. for <c>alpha</c> 1.2, the same output is returned like for <c>alpha</c> = 0.2. </para>
        /// </remarks>
        /// <seealso cref="steps"/>
        /// <seealso cref="AddStep(double)"/>
        /// <returns>Value of step n, where n is: |steps| * (alpha % 1)</returns>
        public override double GetOutput(double alpha = 0)
        {
            alpha = alpha % 1;
            if (alpha < 0) alpha = 1 - alpha;

            Int32 i = Convert.ToInt32(steps.Count * alpha);

            return outputRange.getAbsoluteValue(steps[i]);
        }

        /// <summary>
        /// Returns step value at specified index, <c>alpha</c>. See <see cref="GetOutput(double)"/> for other remarks.
        /// </summary>
        /// <param name="alpha">The index of step to be returned</param>
        /// <returns></returns>
        /// <seealso cref="GetOutput(double)"/>
        public override double GetOutput(int alpha)
        {
            alpha = alpha % steps.Count;
            if (alpha < 0) alpha = steps.Count - alpha;

            return outputRange.getAbsoluteValue(steps[alpha]);
        }
    }
}