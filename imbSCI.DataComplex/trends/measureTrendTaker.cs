// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureTrendTaker.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.trends
{
    using imbSCI.Core.attributes;
    using System.ComponentModel;

    /// <summary>
    /// Sample taker for trend estimation
    /// </summary>
    public class measureTrendTaker
    {
        /// <summary>
        /// True if the measured value has time based measurement unit, e.g. kilobytes per minut
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is time average; otherwise, <c>false</c>.
        /// </value>
        public bool IsTimeAverage { get; set; } = false;

        /// <summary>
        /// True if the measured value represents a cumulative measure, e.g. bytes downloaded so far
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is cumulative; otherwise, <c>false</c>.
        /// </value>
        public bool IsCumulative { get; set; } = false;

        /// <summary>
        /// Constructor for serialization and delayed, manual, initialization. It is better to use constructor with arguments"/>
        /// </summary>
        public measureTrendTaker()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="measureTrendTaker"/> class.
        /// </summary>
        /// <param name="__name">The name.</param>
        /// <param name="__unit">The unit.</param>
        /// <param name="__macroSampleSize">Size of the macro sample.</param>
        /// <param name="__microSampleSize">Size of the micro sample.</param>
        /// <param name="__spearSampleSize">Size of the spear sample.</param>
        /// <param name="__zeroMargin">The zero margin.</param>
        public measureTrendTaker(string __name, string __unit, int __macroSampleSize, int __microSampleSize = -1, int __spearSampleSize = -1, double __zeroMargin = -1)
        {
            name = __name;
            unit = __unit;
            MacroSampleSize = __macroSampleSize;
            if (__microSampleSize == -1) __microSampleSize = __macroSampleSize / 2;
            if (__spearSampleSize == -1) __spearSampleSize = __microSampleSize / 2;
            if (__zeroMargin == -1) __zeroMargin = 0.05;
            MicroSampleSize = __microSampleSize;
            SpearSampleSize = __spearSampleSize;
            ZeroMargin = __zeroMargin;
        }

        /// <summary>
        /// Gets the state of the sample.
        /// </summary>
        /// <param name="sampleSize">Size of the sample.</param>
        /// <returns></returns>
        public measureTrendSampleState GetSampleState(int sampleSize)
        {
            if (sampleSize >= MacroSampleSize) return measureTrendSampleState.macroMean | measureTrendSampleState.microMean | measureTrendSampleState.spearMean;
            if (sampleSize >= MicroSampleSize) return measureTrendSampleState.microMean | measureTrendSampleState.spearMean;
            if (sampleSize >= SpearSampleSize) return measureTrendSampleState.spearMean;
            return measureTrendSampleState.noEnough;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name { get; set; } = "Trend";

        /// <summary>
        /// Format (Standard Numeric Format, like F4, P2...) to be used for value representation
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public string format { get; set; } = "F4";

        /// <summary>
        /// Letter or symbol representing the measurement unit
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public string unit { get; set; } = "n";

        /// <summary> Number of samples to take for macro mean </summary>
        [Category("Count")]
        [DisplayName("MacroSampleSize")]
        [imb(imbAttributeName.measure_letter, "")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [Description("Number of samples to take for macro mean")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public int MacroSampleSize { get; set; } = 8;

        /// <summary> Number of samples to take for micro mean </summary>
        [Category("Count")]
        [DisplayName("MicroSampleSize")]
        [imb(imbAttributeName.measure_letter, "")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [Description("Number of samples to take for micro mean")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public int MicroSampleSize { get; set; } = 4;

        /// <summary> Number of samples to take for spear mean </summary>
        [Category("Count")]
        [DisplayName("SpearSampleSize")]
        [imb(imbAttributeName.measure_letter, "")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [Description("Number of samples to take for spear mean")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public int SpearSampleSize { get; set; } = 2;

        /// <summary> Zero-centered margin (noise gate) to ignore, in percentage</summary>
        [Category("Ratio")]
        [DisplayName("ZeroMargin")]
        [imb(imbAttributeName.measure_letter, "+/- d")]
        [imb(imbAttributeName.measure_setUnit, "%")]
        [imb(imbAttributeName.reporting_valueformat, "P2")]
        [Description("Zero-centered margin (noise gate) to ignore")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public double ZeroMargin { get; set; } = 0.05;
    }
}