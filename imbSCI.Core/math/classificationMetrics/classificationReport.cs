// --------------------------------------------------------------------------------------------------------------------
// <copyright file="classificationReport.cs" company="imbVeles" >
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
using System;
using System.ComponentModel;

namespace imbSCI.Core.math.classificationMetrics
{
    /// <summary>
    /// Base class for classification reporting
    /// </summary>
    /// <seealso cref="imbSCI.Core.math.classificationMetrics.IClassificationReport" />
    public class classificationReport : IClassificationReport
    {
        public classificationReport()
        {
        }

        public classificationReport(String caseName)
        {
            Name = caseName;
        }

        private classificationEvalMetricSet metrics;

        /// <summary>
        /// Sets (if specified <c>_metrics</c> is not null) and Gets (earlier or just set) metrics object. Makes no change to report content, just stores the metrics temporarrly
        /// </summary>
        /// <param name="_metrics">The metrics.</param>
        /// <returns></returns>
        public classificationEvalMetricSet GetSetMetrics(classificationEvalMetricSet _metrics = null)
        {
            if (_metrics != null) metrics = _metrics;
            return metrics;
        }

        /// <summary> the title attached to this k-fold evaluation case instance </summary>
        [Category("Labels")]
        [DisplayName("Name")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("The label / title, attached to this case / data row / report instance")]
        [imb(imbAttributeName.reporting_columnWidth, "50")]
        public String Name { get; set; } = default(String);

        /// <summary> Name of post classifier </summary>
        [Category("Labels")]
        [DisplayName("Classifier")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Classification algorithm code-name")] // [imb(imbAttributeName.reporting_escapeoff)]
        public String Classifier { get; set; } = default(String);

        /// <summary> Correct classifications </summary>
        [Category("Basic measures")]
        [DisplayName("Correct")]
        [imb(imbAttributeName.measure_letter, "E_c")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [Description("Correct classifications - True positives")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Double Correct { get; set; } = 0;

        /// <summary> Wrong </summary>
        [Category("Basic measures")]
        [DisplayName("Wrong")]
        [imb(imbAttributeName.measure_letter, "E_w")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [Description("Wrong classification count - False positives")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Double Wrong { get; set; } = default(Int32);

        /// <summary> Number of web sites designated for model evaluation </summary>
        [Category("Basic measures")]
        [DisplayName("Targets")]
        [imb(imbAttributeName.measure_letter, "W_n")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [Description("Number of cases evaluated by the model, in test phase")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Double Targets { get; set; } = default(Int32);

        /// <summary> Ratio </summary>
        [Category("Effectiveness")]
        [DisplayName("Precision")]
        [imb(imbAttributeName.measure_letter, "P")]
        [imb(imbAttributeName.measure_setUnit, "%")]
        [imb(imbAttributeName.reporting_valueformat, "F5")]
        [Description("Rate of correctly classified cases in all evaluated")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public Double Precision { get; set; } = default(Double);

        /// <summary> Ratio </summary>
        [Category("Effectiveness")]
        [DisplayName("Recall")]
        [imb(imbAttributeName.measure_letter, "R")]
        [imb(imbAttributeName.measure_setUnit, "%")]
        [imb(imbAttributeName.reporting_valueformat, "F5")]
        [Description("Rate of correctly classified web sites in total number of web sites of the class")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public Double Recall { get; set; } = default(Double);

        /// <summary> F1 measure - harmonic mean of precision and recall </summary>
        [Category("Effectiveness")]
        [DisplayName("F1measure")]
        [imb(imbAttributeName.measure_letter, "F1")]
        [imb(imbAttributeName.measure_setUnit, "%")]
        [imb(imbAttributeName.reporting_valueformat, "F5")]
        [Description("F1 measure - harmonic mean of precision and recall")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public Double F1measure { get; set; } = default(Double);

        /// <summary> Optional information or comment on the data in the row </summary>
        [Category("Meta")]
        [DisplayName("Comment")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Optional information or comment on the data in the row")] // [imb(imbAttributeName.reporting_escapeoff)]
        public String Comment { get; set; } = default(String);

        /// <summary> Basic enumeration of the report entry, i.e. data row </summary>
        [Category("Meta")]
        [DisplayName("Flags")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Basic enumeration of the report entry, i.e. data row")] // [imb(imbAttributeName.reporting_escapeoff)]
        public classificationReportRowFlags EntryFlags { get; set; } = classificationReportRowFlags.none;

        /// <summary> Number of cases that were aggregated in this row </summary>
        [Category("Meta")]
        [DisplayName("Count")]
        [imb(imbAttributeName.measure_letter, "")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [Description("Number of cases that were aggregated in this row")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 Count { get; set; }
    }
}