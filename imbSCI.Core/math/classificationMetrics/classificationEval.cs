// --------------------------------------------------------------------------------------------------------------------
// <copyright file="classificationEval.cs" company="imbVeles" >
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

namespace imbSCI.Core.math.classificationMetrics
{
    /// <summary>
    /// Entry for <see cref="classificationEvalMetricSet"/>
    /// </summary>
    public class classificationEval
    {
        public classificationEval()
        {
        }

        /// <summary>
        /// Sets the name for metric struct
        /// </summary>
        /// <param name="_name">The name.</param>
        public classificationEval(String _name)
        {
            name = _name;
        }


        //public void EvaluateByID(Int32 testResult, Int32 truthTable)
        //{


        //}

        /// <summary>
        /// Optional name assigned to this metric structure
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name { get; set; } = "";

        /// <summary>
        /// Gets or sets the positives.
        /// </summary>
        /// <value>
        /// The positives.
        /// </value>
        public Int32 positives { get; set; }

        /// <summary>
        /// T_p - prediction was [true], and truth table said [true]
        /// </summary>
        /// <value>
        /// The true positives.
        /// </value>
        public Int32 truePositives { get; set; }

        /// <summary>
        /// F_p - prediction was [true], but truth table said [false]
        /// </summary>
        /// <value>
        /// The false positives.
        /// </value>
        public Int32 falsePositives { get; set; }

        /// <summary>
        /// F_n - prediction was [false], but the truth table said [true]
        /// </summary>
        /// <value>
        /// The false negatives.
        /// </value>
        public Int32 falseNegatives { get; set; }

        /// <summary>
        /// T_n - prediction was [false], and the truth table said [false]
        /// </summary>
        /// <value>
        /// The true negatives.
        /// </value>
        public Int32 trueNegatives { get; set; }

        /// <summary>
        /// Gets or sets the correct: when used for simple precision computation
        /// </summary>
        /// <value>
        /// The correct.
        /// </value>
        public Int32 correct { get; set; }

        /// <summary>
        /// Gets or sets the wrong: when used for simple precission computation
        /// </summary>
        /// <value>
        /// The wrong.
        /// </value>
        public Int32 wrong { get; set; }

        public static classificationEval operator +(classificationEval a, classificationEval b)
        {
            a.truePositives += b.truePositives;
            a.falseNegatives += b.falseNegatives;
            a.falsePositives += b.falsePositives;
            a.trueNegatives += b.trueNegatives;

            a.correct += b.correct;
            a.wrong += b.wrong;

            return a;
        }

        public static classificationEval operator -(classificationEval a, classificationEval b)
        {
            a.truePositives -= b.truePositives;
            a.falseNegatives -= b.falseNegatives;
            a.falsePositives -= b.falsePositives;
            a.trueNegatives -= b.trueNegatives;

            a.correct -= b.correct;
            a.wrong -= b.wrong;

            return a;
        }

        /// <summary>
        /// Gets the Error measure: ( Fp + Fn ) / ( Tp + Tn + Fp + Fn)
        /// </summary>
        /// <returns></returns>
        public Double GetError()
        {
            return (falsePositives + falseNegatives).GetRatio(truePositives + trueNegatives + falsePositives + falseNegatives);
        }

        /// <summary>
        /// Gets the Accuracy measure: ( Tp + Tn ) / ( Tp + Tn + Fp + Fn)
        /// </summary>
        /// <returns></returns>
        public Double GetAccuracy()
        {
            return (truePositives + trueNegatives).GetRatio(truePositives + trueNegatives + falsePositives + falseNegatives);
        }

        /// <summary>
        /// Gets the F1 measure: harmonic mean of <see cref="GetPrecision"/> and <see cref="GetRecall"/>
        /// </summary>
        /// <returns></returns>
        public Double GetF1()
        {
            Double p = GetPrecision();
            Double r = GetRecall();
            Double f1 = 2 * ((p * r).GetRatio(p + r));
            return f1;
        }

        /// <summary>
        /// Gets the precision: Tp / (Tp + Fp)
        /// </summary>
        /// <returns></returns>
        public Double GetPrecision()
        {
            return truePositives.GetRatio(truePositives + falsePositives); // GetRatio(tmp.positives);
        }

        /// <summary>
        /// Gets the recall: Tp / (Tp + Fn)
        /// </summary>
        /// <returns></returns>
        public Double GetRecall()
        {
            var items = (truePositives + falseNegatives);
            return truePositives.GetRatio(items);
        }

        /// <summary>
        /// Gets the targets.
        /// </summary>
        /// <value>
        /// The targets.
        /// </value>
        public Int32 targets
        {
            get
            {
                return correct + wrong;
            }
        }
    }
}