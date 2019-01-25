// --------------------------------------------------------------------------------------------------------------------
// <copyright file="rangeFinder.cs" company="imbVeles" >
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
using imbSCI.Core.attributes;
using imbSCI.Core.data;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace imbSCI.Core.math.range.finder
{

    //public interface IRangeFinder
    //{
    //    void Reset();
    //    String id { get; set; }

    //}


    /// <summary>
    /// Math utility class for: min-max-range computations
    /// </summary>
    public class rangeFinder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="rangeFinder"/> class.
        /// </summary>
        public rangeFinder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="rangeFinder"/> class.
        /// </summary>
        /// <param name="_id">The identifier.</param>
        public rangeFinder(String _id)
        {
            id = _id;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public String id { get; set; }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public virtual void Reset()
        {
            Maximum = Double.MinValue;
            Minimum = Double.MaxValue;

            Count = 0;
            Sum = 0;
        }

        /// <summary>
        /// Updates the instance with data
        /// </summary>
        /// <param name="input">The input.</param>
        public virtual void Learn(Double input)
        {
            if (input != Double.NaN)
            {
                Sum = Sum + input;
                Maximum = Math.Max(input, Maximum);
                Minimum = Math.Min(input, Minimum);
                Count++;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is learned.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is learned; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsLearned
        {
            get
            {
                if (Maximum == Double.MinValue) return false;
                if (Minimum == Double.MaxValue) return false;
                return true;
            }
        }

        /// <summary>
        /// Returns 0 (min) to 1 (max) position of the <c>input</c>, within <see cref="Minimum"/> - <see cref="Maximum"/> range
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public Double GetPositionInRange(Double input)
        {
            input = input - Minimum;
            if (input > Range) return 1;
            if (input < 0) return 0;
            return input.GetRatio(Range);
        }

        /// <summary>
        /// Returns dictionary with values
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, Double> GetDictionary(String prefix = "")
        {
            Dictionary<String, Double> output = new Dictionary<string, double>();

            output.Add(prefix + nameof(Minimum), Minimum);
            output.Add(prefix + nameof(Maximum), Maximum);
            output.Add(prefix + nameof(Range), Range);
            output.Add(prefix + nameof(Average), Average);
            output.Add(prefix + nameof(Sum), Sum);
            output.Add(prefix + nameof(Count), Count);

            return output;
        }

        /// <summary> Ratio </summary>
        [Category("Ratio")]
        [DisplayName("Minimum")]
        [imb(imbAttributeName.measure_letter, "Min")]
        [imb(imbAttributeName.measure_setUnit, "u")]
        [Description("Lowest value in the range")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public Double Minimum { get; set; } = Double.MaxValue;

        /// <summary> Ratio </summary>
        [Category("Ratio")]
        [DisplayName("Maximum")]
        [imb(imbAttributeName.measure_letter, "Max")]
        [imb(imbAttributeName.measure_setUnit, "u")]
        [Description("Highest value in the range")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public Double Maximum { get; set; } = Double.MinValue;

        /// <summary> Ratio </summary>
        [Category("Ratio")]
        [DisplayName("Range")]
        [imb(imbAttributeName.measure_letter, "R")]
        [imb(imbAttributeName.measure_setUnit, "u")]
        [Description("Maximum value, minus minimum value")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public Double Range
        {
            get
            {
                return Maximum - Minimum;
            }
        }

        /// <summary> Count </summary>
        [Category("Count")]
        [DisplayName("Count")]
        [imb(imbAttributeName.measure_letter, "|r|")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [imb(imbAttributeName.reporting_valueformat, "D2")]
        [Description("Count - size of the set - number of readings")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 Count { get; set; } = 0;

        /// <summary> Average </summary>
        [Category("Ratio")]
        [DisplayName("Mean")]
        [imb(imbAttributeName.measure_letter, "Avg")]
        [imb(imbAttributeName.measure_setUnit, "u")]
        [Description("Aritmetic mean of the measured set")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public Double Average
        {
            get
            {
                return Sum.GetRatio(Count);
            }
        }

        /// <summary> Summary of all values </summary>
        [Category("Ratio")]
        [DisplayName("Sum")]
        [imb(imbAttributeName.measure_letter, "S")]
        [imb(imbAttributeName.measure_setUnit, "u")]
        [Description("Sum of all values in the set")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public Double Sum { get; set; } = 0;
    }
}