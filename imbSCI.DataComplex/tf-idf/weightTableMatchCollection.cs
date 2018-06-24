// --------------------------------------------------------------------------------------------------------------------
// <copyright file="weightTableMatchCollection.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;

    public class weightTableMatchCollection<TWeightTableTerm, TSecondTableTerm> : List<weightTableMatch<TWeightTableTerm, TSecondTableTerm>>
        where TWeightTableTerm : IWeightTableTerm
        where TSecondTableTerm : IWeightTableTerm
    {
        public weightTableMatchCollection(IWeightTable __first, IWeightTable __second)
        {
            first = __first;
            second = __second;
        }

        public string ToString()
        {
            string output = "";
            foreach (var mch in this)
            {
                output = output.add(mch.ToString(), " | ");
            }
            return output;
        }

        /// <summary>
        ///
        /// </summary>
        public IWeightTable first { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IWeightTable second { get; set; }

        public void Add(TSecondTableTerm second, TWeightTableTerm match)
        {
            //if (!ContainsKey(second)) this.Add(new weightTableMatch<TWeightTableTerm, TSecondTableTerm>(second));

            Add(new weightTableMatch<TWeightTableTerm, TSecondTableTerm>(second, match));
        }

        /// <summary>
        /// Gets the semantic similarity.
        /// </summary>
        /// <returns></returns>
        public double GetSemanticSimilarity()
        {
            double output = 0;

            double up = 0;
            double downSumA = 0;
            double downSumB = 0;

            foreach (weightTableMatch<TWeightTableTerm, TSecondTableTerm> pair in this)
            {
                var q_i = first.GetNFreq((string)pair.match.name);
                var d_i = second.GetTF_IDF((string)pair.key.name); //< -- obrni
                var sim = pair.subKey.weight;
                up += q_i * d_i * sim;

                //downSumA++;
            }

            downSumA = first.GetCWeight(); // +  second.GetCWeight();

            if (up != 0)
            {
                output = up / downSumA;
            }
            return output;
        }

        public double GetVSMCosineSimilarity()
        {
            double output = 0;

            double up = 0;
            double downSumA = 0;
            double downSumB = 0;

            foreach (weightTableMatch<TWeightTableTerm, TSecondTableTerm> pair in this)
            {
                up += first.GetTF_IDF(pair.key) * second.GetTF_IDF(pair.match);
                downSumA += Math.Pow(first.GetTF_IDF((string)pair.key.name), 2);
                downSumB += Math.Pow(second.GetTF_IDF((string)pair.match.name), 2);
            }

            double down = Math.Sqrt(downSumA * downSumB);

            output = up / down;
            return output;
        }
    }
}