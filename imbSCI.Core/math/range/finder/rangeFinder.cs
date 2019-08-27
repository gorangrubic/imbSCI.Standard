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
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Xml.Serialization;

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
    [Serializable]
    public class rangeFinder:IEquatable<rangeFinder>
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

        public rangeFinder GetDifference(rangeFinder other)
        {
            rangeFinder output = new rangeFinder();
            if (other.IsLearned && IsLearned)
            {
                if (!id.isNullOrEmpty() && !other.id.isNullOrEmpty())
                {
                    output.id = id.add(other.id, "-");
                }
                output.Maximum = Maximum - other.Maximum;
                output.Minimum = Minimum - other.Minimum;
                output.Count = Count - other.Count;
                output.Sum = Sum  - other.Sum;
            }
            return output;
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
        public virtual void Learn(IEnumerable<Double> input)
        {
            foreach (Double i in input)
            {
                Learn(i);
            }
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
        [XmlIgnore]
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
        /// Returns value gradinet from <see cref="Minimum"/> to <see cref="Maximum"/> in <c>steps</c>, returned in zig-zag order. i.e. for 10 steps, min 0, max 1: it will return> 0, 0.9, 0.1, 0.8, 0.2 ... 
        /// </summary>
        /// <param name="steps">The steps.</param>
        /// <returns></returns>
        public List<Double> GetValueRangeZigZagSteps(Int32 steps)
        {
            List<Double> output = new List<double>();
            Int32 halfSteps = steps / 2;

            List<Double> rs = new List<double>();
            for (int i = 0; i < halfSteps; i++)
            {

                Double r_l = (i*2).GetRatio(steps);
                Double r_h = (steps-(i*2)).GetRatio(steps);
                rs.AddUnique(r_l);
                rs.AddUnique(r_h);
                
            }

            foreach (Double r in rs)
            {
                output.Add(GetValueForRangePosition(r));
            }

            return output;
        }

        /// <summary>
        /// Returns value gradinet from <see cref="Minimum"/> to <see cref="Maximum"/>, in given steps
        /// </summary>
        /// <param name="steps">The steps.</param>
        /// <returns></returns>
        public List<Double> GetValueRangeSteps(Int32 steps)
        {
            List<Double> output = new List<double>();
            for (int i = 0; i <= steps; i++)
            {
                Double r = i.GetRatio(steps);
                output.Add(GetValueForRangePosition(r));
            }
            return output;
        }

        /// <summary>
        /// For specified position in range, returns value
        /// </summary>
        /// <param name="ratio">The ratio.</param>
        /// <returns></returns>
        public Double GetValueForRangePosition(Double ratio)
        {
            while (ratio > 1)
            {
                ratio = ratio - 1;
            }
            Double output = Minimum;
            output += Range * ratio;
            return output;
        }


        public void Report(ITextRender output, String rangeName, String prefix)
        {
            if (rangeName == "") rangeName = id;
            
            output.AppendLine("Range [" + rangeName + "]");
            output.nextTabLevel();
            foreach (var pair in GetDictionary(prefix))
            {
                output.AppendPair(pair.Key, pair.Value.ToString("F3"));
            }
            output.prevTabLevel();
        }

        /// <summary>
        /// Gets list of dictionary fields for a <see cref="rangeFinder"/> with given <c>prefix</c>
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        public static List<String> GetRangeFinderDictionaryFields(String prefix = "")
        {
            List<String> output = new List<string>();
            
            output.Add(prefix + nameof(Minimum));
            output.Add(prefix + nameof(Maximum));
            output.Add(prefix + nameof(Range));
            output.Add(prefix + nameof(Average));
            output.Add(prefix + nameof(Sum));
            output.Add(prefix + nameof(Count));

            return output;
        }

        /// <summary>
        /// Returns dictionary with range descriptive statistics (min, max, range, sum, count)
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, Double> GetDictionary(String prefix = "")
        {
            Dictionary<String, Double> output = new Dictionary<string, double>();
            if (prefix.isNullOrEmpty()) prefix = id;

            output.Add(prefix + nameof(Minimum), Minimum);
            output.Add(prefix + nameof(Maximum), Maximum);
            output.Add(prefix + nameof(Range), Range);
            output.Add(prefix + nameof(Average), Average);
            output.Add(prefix + nameof(Sum), Sum);
            output.Add(prefix + nameof(Count), Count);

            return output;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(rangeFinder other)
        {
            if (Minimum == other.Minimum && Maximum == other.Maximum) return true;
            return false;
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
        [XmlIgnore]
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
        [XmlIgnore]
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