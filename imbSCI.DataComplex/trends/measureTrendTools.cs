// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureTrendTools.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.text;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extensions for <see cref="measureTrend"/>s
    /// </summary>
    public static class measureTrendTools
    {
        /// <summary>
        ///  Finds total timespan by summing all inter-sample periods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sampleset"></param>
        /// <returns></returns>
        public static TimeSpan GetTimeSpanBySum<T>(this IEnumerable<T> sampleset) where T : IPerformanceTake
        {
            int seconds = (int)(sampleset.Sum(x => x.secondsSinceLastTake) / 60);
            return new TimeSpan(0, 0, seconds);
        }

        /// <summary>
        /// Gets the time span between sampling times of sampleset collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sampleset">The sampleset.</param>
        /// <returns></returns>
        public static TimeSpan GetTimeSpan<T>(this IEnumerable<T> sampleset) where T : IPerformanceTake
        {
            DateTime start = sampleset.Min<T, DateTime>(x => x.samplingTime);
            DateTime end = sampleset.Max<T, DateTime>(x => x.samplingTime);
            return end.Subtract(start);
        }

        /// <summary>
        /// Gets the trend from set of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sampleset">Set of objects to take measure from</param>
        /// <param name="selector">Expresion that takes the property value from an object in the <c>sampleset</c></param>
        /// <param name="trendTaker">The trend taker definition</param>
        /// <param name="span">The time span to recalculate mean values for.</param>
        /// <returns></returns>
        public static measureTrend GetTrend<T>(this IEnumerable<T> sampleset, Func<T, double> selector, measureTrendTaker trendTaker, TimeSpan span)
        {
            int sC = sampleset.Count();
            int sT = Math.Min(sC, trendTaker.MacroSampleSize);

            var sValues = (from num in sampleset select selector(num));

            //sampleset.Take(sT).GetTimeSpan

            measureTrend trend = new measureTrend(sValues, trendTaker, span);

            return trend;
        }

        /// <summary>
        /// Gets the trend interpretation as single line string
        /// </summary>
        /// <param name="trend">The trend.</param>
        /// <returns></returns>
        public static string GetTrendInline(this measureTrend trend)
        {
            string form = "{0} {1,10}:[_{2,15}_ {3,-14}] ({4,11}) _{5,8}_ ";

            List<string> st = new List<string>();
            st.Add(trend.SampleState.GetStateSymbols());
            st.Add(trend.name.toWidthMaximum(8, ""));

            st.Add(trend.MicroMean.ToString(trend.format));

            if (trend.sampledPeriod > 0)
            {
                form += trend.sampledPeriod.ToString("F2") + " min";
            }
            st.Add(trend.unit);

            if (trend.SampleState.HasFlag(measureTrendSampleState.macroMean))
            {
                st.Add(trend.MicroTrend.ToString("P2"));
            }
            else
            {
                st.Add("~~~~");
            }
            st.Add(trend.Direction.GetTrendDirectionSymbols());

            return string.Format(form, st.ToArray()).toWidthExact(75, " ");
        }

        /// <summary>
        /// Gets the trend direction symbolic interpretation
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public static string GetTrendDirectionSymbols(this measureTrendDirection direction)
        {
            // ▲▼▴▾○◦●■□
            string output = "";

            string left = "";
            string right = "";

            if (direction.HasFlag(measureTrendDirection.macroStable))
            {
                left += "-";
                right += "-";
            }

            if (direction.HasFlag(measureTrendDirection.microStable))
            {
                left += "-";
                right += "-";
            }

            if (direction.HasFlag(measureTrendDirection.macroDown)) left += "<-";
            if (direction.HasFlag(measureTrendDirection.microDown)) left += "<<";

            if (direction.HasFlag(measureTrendDirection.microUp)) right += ">>";
            if (direction.HasFlag(measureTrendDirection.macroUp)) right += "->";

            output = left + "" + right;

            return output;
        }

        public static string GetStateSymbols(this measureTrendSampleState direction)
        {
            if (direction.HasFlag(measureTrendSampleState.macroMean)) return "ok";
            if (direction.HasFlag(measureTrendSampleState.microMean)) return "--";
            if (direction.HasFlag(measureTrendSampleState.spearMean)) return "~~";
            if (direction.HasFlag(measureTrendSampleState.noEnough)) return "no";
            return "-";
        }
    }
}