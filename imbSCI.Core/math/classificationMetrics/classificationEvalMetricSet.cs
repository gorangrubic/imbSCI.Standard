// --------------------------------------------------------------------------------------------------------------------
// <copyright file="classificationEvalMetricSet.cs" company="imbVeles" >
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
using imbSCI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.math.classificationMetrics
{
    /// <summary>
    /// Classification performance records dictionary, keeping records for each category/label separatly
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.String, imbSCI.Core.math.classificationMetrics.classificationEval}}" />
    public class classificationEvalMetricSet : IEnumerable<KeyValuePair<String, classificationEval>>
    {
        /// <summary>
        /// Optional name for the classification metric set
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name { get; set; } = "";

        public classificationEvalMetricSet()
        {
        }

        /// <summary>
        /// Constructor with name assigment
        /// </summary>
        /// <param name="_name">The name.</param>
        public classificationEvalMetricSet(String _name)
        {
            name = _name;
        }


        /// <summary>
        /// Constructs eval metrics with categories specified <c>labels</c>
        /// </summary>
        /// <param name="_name">The name.</param>
        /// <param name="labels">The labels.</param>
        public classificationEvalMetricSet(String _name, IEnumerable<String> labels)
        {
            name = _name;
            foreach (String lbl in labels)
            {
                var m = this[lbl];
            }
        }

        /// <summary>
        /// Adds the record in the confusion matrix
        /// </summary>
        /// <param name="testResult">Class label - What was the prediction?</param>
        /// <param name="truth">Class label - What the truth table said?</param>
        public void AddRecord(String testResult, String truth)
        {
            if (testResult == truth)
            {
                this[truth].correct++;
            }
            else
            {
                this[truth].wrong++;
            }

            if (testResult == truth)
            {
                this[testResult].truePositives++;
                foreach (String label in items.Keys)
                {
                    if (label != testResult)
                    {
                        this[label].trueNegatives++;
                    }
                }

            }
            else
            {
                this[testResult].falsePositives++;
                this[truth].falseNegatives++;

                foreach (String label in items.Keys)
                {
                    if ((label != testResult) && (label != truth))
                    {
                        this[label].trueNegatives++;
                    }
                }

            }



            //foreach (String label in items.Keys)
            //{
            //    if (testResult == truth)
            //    {
            //        if (label == truth)
            //        {
            //            this[label].truePositives++;
            //        }
            //        else
            //        {
            //            this[label].falsePositives++;
            //        }
            //    }
            //    else
            //    {
            //        if (label == truth)
            //        {
            //            this[label].falseNegatives++;
            //        }
            //        else
            //        {
            //            this[label].trueNegatives++;
            //        }
            //    }
            //}

        }


        protected Dictionary<String, classificationEval> items = new Dictionary<string, classificationEval>();

        /// <summary>
        /// Gets a <see cref="classificationEval"/> for the specified category name, if not known so far - creates new <see cref="classificationEval"/> for it
        /// </summary>
        /// <value>
        /// The <see cref="classificationEval"/>.
        /// </value>
        /// <param name="categoryName">The name of category.</param>
        /// <returns></returns>
        public classificationEval this[String categoryName]
        {
            get
            {
                if (!items.ContainsKey(categoryName)) items.Add(categoryName, new classificationEval(name.add(categoryName, "_")));
                return items[categoryName];
            }
        }

        /// <summary>
        /// Gets all entries.
        /// </summary>
        /// <returns></returns>
        public List<classificationEval> GetAllEntries()
        {
            return items.Values.ToList();
        }

        /// <summary>
        /// Gets all category names
        /// </summary>
        /// <returns></returns>
        public List<String> GetAllCategories()
        {
            return items.Keys.ToList();
        }

        /// <summary>
        /// Gets the Error measure, computed in respect to the <c>method</c> specified
        /// </summary>
        /// <param name="method">The method of Error measure computation.</param>
        /// <seealso cref="classificationEval.GetError"/>
        /// <returns></returns>
        public Double GetError(classificationMetricComputation method = classificationMetricComputation.microAveraging)
        {
            Double output = 0;
            switch (method)
            {
                case classificationMetricComputation.microAveraging:
                    return GetSummary().GetError();
                    break;

                case classificationMetricComputation.macroAveraging:
                    foreach (var pair in items)
                    {
                        output += this[pair.Key].GetError();
                    }
                    return output.GetRatio(items.Count);
                    break;
            }

            return output;
        }

        /// <summary>
        /// Gets the Accuracy measure, computed in respect to the <c>method</c> specified
        /// </summary>
        /// <param name="method">The method of Accuracy measure computation.</param>
        /// <seealso cref="classificationEval.GetAccuracy"/>
        /// <returns></returns>
        public Double GetAccuracy(classificationMetricComputation method = classificationMetricComputation.microAveraging)
        {
            Double output = 0;
            switch (method)
            {
                case classificationMetricComputation.microAveraging:
                    return GetSummary().GetAccuracy();
                    break;

                case classificationMetricComputation.macroAveraging:
                    foreach (var pair in items)
                    {
                        output += this[pair.Key].GetAccuracy();
                    }
                    return output.GetRatio(items.Count);
                    break;
            }

            return output;
        }

        /// <summary>
        /// Gets the Precission measure, computed in respect to the <c>method</c> specified
        /// </summary>
        /// <param name="method">The method of Precission measure computation.</param>
        /// <seealso cref="classificationEval.GetPrecision"/>
        /// <returns></returns>
        public Double GetPrecision(classificationMetricComputation method = classificationMetricComputation.microAveraging)
        {
            Double output = 0;
            switch (method)
            {
                case classificationMetricComputation.microAveraging:
                    return GetSummary().GetPrecision();
                    break;

                case classificationMetricComputation.macroAveraging:
                    foreach (var pair in items)
                    {
                        output += this[pair.Key].GetPrecision();
                    }
                    return output.GetRatio(items.Count);
                    break;
            }

            return output;
        }

        /// <summary>
        /// Gets the Recall measure, computed in respect to the <c>method</c> specified
        /// </summary>
        /// <param name="method">The method of Recall measure computation.</param>
        /// <seealso cref="classificationEval.GetRecall"/>
        /// <returns></returns>
        public Double GetRecall(classificationMetricComputation method = classificationMetricComputation.microAveraging)
        {
            Double output = 0;
            switch (method)
            {
                case classificationMetricComputation.microAveraging:
                    return GetSummary().GetRecall();
                    break;

                case classificationMetricComputation.macroAveraging:
                    foreach (var pair in items)
                    {
                        output += this[pair.Key].GetRecall();
                    }
                    return output.GetRatio(items.Count);
                    break;
            }
            return output;
        }

        /// <summary>
        /// Gets the F1 measure, computed in respect to the <c>method</c> specified
        /// </summary>
        /// <param name="method">The method of F1 measure computation.</param>
        /// <seealso cref="classificationEval.GetF1"/>
        /// <returns></returns>
        public Double GetF1(classificationMetricComputation method = classificationMetricComputation.microAveraging)
        {
            Double output = 0;
            switch (method)
            {
                case classificationMetricComputation.microAveraging:
                    return GetSummary().GetF1();
                    break;

                case classificationMetricComputation.macroAveraging:
                    foreach (var pair in items)
                    {
                        output += this[pair.Key].GetF1();
                    }
                    return output.GetRatio(items.Count);
                    break;
            }
            return output;
        }

        /// <summary>
        /// Gets the summary metrics, for all categories. Optionally, a descriptive name can be specified.
        /// </summary>
        /// <param name="structName">Name of the metric structure.</param>
        /// <returns></returns>
        public classificationEval GetSummary(String structName = "summary")
        {
            classificationEval sum = new classificationEval(name.add(structName, "_"));
            foreach (var pair in items)
            {
                sum = sum + pair.Value;
            }

            return sum;
        }

        public IEnumerator<KeyValuePair<string, classificationEval>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, classificationEval>>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, classificationEval>>)items).GetEnumerator();
        }
    }
}