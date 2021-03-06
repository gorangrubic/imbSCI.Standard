// --------------------------------------------------------------------------------------------------------------------
// <copyright file="classificationReportExtensions.cs" company="imbVeles" >
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
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace imbSCI.Core.math.classificationMetrics
{
    /// <summary>
    /// Extension methods for <see cref="IClassificationReport"/>
    /// </summary>
    public static class classificationReportExtensions
    {

        public static Double GetReportValue(this classificationReport report, String valueToUseKey)
        {
            Double val = 0;


            switch (valueToUseKey)
            {
                case classificationReportStyleDefinition.VALUE_F1:
                    val = report.F1measure;
                    break;
                case classificationReportStyleDefinition.VALUE_FS:
                    val = report.GetSelectedFeatureAverage();
                    break;
            }

            return val;
        }

        public static Double GetSelectedFeatureAverage(this classificationReport report)
        {
            return report.data.GetMeanValue("SelectedFeatures");
        }

        public static Dictionary<String, List<classificationReportExpanded>> GetSpaceLayers(this IEnumerable<classificationReportExpanded> reports, classificationReportStyleDefinition style, Dictionary<String, List<classificationReportExpanded>> output = null)
        {
            if (output == null) output = new Dictionary<string, List<classificationReportExpanded>>();
            style.Prepare();

            foreach (classificationReportExpanded rep in reports)
            {
                foreach (var pair in style.layerNeedleByNameCompiled)
                {
                    if (pair.Key.IsMatch(rep.Name))
                    {
                        Match mc = pair.Key.Match(rep.Name);
                        String nm = rep.Name;

                        var splits = pair.Key.Split(nm);
                        nm = "";

                        foreach (String sp in splits)
                        {
                            nm += sp;
                        }

                        rep.layer = pair.Value;

                    }


                }

                if (!output.ContainsKey(rep.layer)) output.Add(rep.layer, new List<classificationReportExpanded>());
                output[rep.layer].Add(rep);
            }


            return output;

        }



        /// <summary>
        /// Sets or Adds the values from specified <c>metrics</c> object. 
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="metrics">The metrics.</param>
        /// <param name="method">The method of ratios computation (F1, Precision, Recall)</param>
        public static void AddValues(this IClassificationReport a, classificationEvalMetricSet metrics, classificationMetricComputation method)
        {
            a.Precision += metrics.GetPrecision(method);
            a.Recall += metrics.GetRecall(method);
            a.F1measure += metrics.GetF1(method);

            foreach (var p in metrics)
            {
                a.Correct += p.Value.correct;
                a.Wrong += p.Value.wrong;
                a.Targets += p.Value.correct + p.Value.wrong;
            }

            a.method = method;
        }

        /// <summary>
        /// Adds the values from specified <c>source</c>
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="source">The source.</param>
        public static void AddValues(this IClassificationReport a, IClassificationReport source)
        {
            a.Precision += source.Precision;
            a.Recall += source.Recall;
            a.F1measure += source.F1measure;
            a.Correct += source.Correct;
            a.Targets += source.Targets;
            a.Wrong += source.Wrong;

            if (a is classificationReport cReport)
            {
                if (source is classificationReport sReport)
                {
                    cReport.data.Merge(sReport.data);
                }
            }
        }

        /// <summary>
        /// Divides the values stored in the report
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="divisor">The divisor.</param>
        /// <param name="OnlyRatios">if set to <c>true</c> [only ratios].</param>
        public static void DivideValues(this IClassificationReport a, Double divisor, Boolean OnlyRatios = true)
        {
            a.Precision = a.Precision.GetRatio(divisor);
            a.Recall = a.Recall.GetRatio(divisor);
            a.F1measure = a.F1measure.GetRatio(divisor);
            if (!OnlyRatios)
            {
                a.Correct = a.Correct.GetRatio(divisor);
                a.Targets = a.Targets.GetRatio(divisor);
                a.Wrong = a.Wrong.GetRatio(divisor);
            }
        }

        //public static DocumentSetCaseCollectionReport GetAverage(IEnumerable<DocumentSetCaseCollectionReport> input)
        //{
        //    DocumentSetCaseCollectionReport output = null;
        //    DocumentSetCaseCollectionReport first = null;
        //    Int32 c = 0;
        //    foreach (var i in input)
        //    {
        //        if (first == null)
        //        {
        //            first = i;
        //            output = new DocumentSetCaseCollectionReport(first.Classifier + "_" + "")
        //            output.Classifier = first.Classifier;
        //            output.kFoldCase = output.Classifier + " (mean)";
        //        }
        //        output.AddValues(i);
        //        c++;
        //    }
        //    output.DivideValues(c);

        //    return output;
        //}
    }
}