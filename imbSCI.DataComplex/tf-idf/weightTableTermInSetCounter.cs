// --------------------------------------------------------------------------------------------------------------------
// <copyright file="weightTableTermInSetCounter.cs" company="imbVeles" >
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
    using imbSCI.Data.collection.nested;
    using System.Collections.Generic;
    using System.Linq;

#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'aceCommonTypes.collection.nested.aceDictionary2D{aceCommonTypes.collection.tf_idf.IWeightTableTerm, aceCommonTypes.collection.tf_idf.IWeightTable, System.Int32}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    /// Counter - helper class keeping record of the term ocurrences
    /// </summary>
    /// <typeparam name="TWeightTableTerm">The type of the weight table term.</typeparam>
    /// <typeparam name="TWeightTable">The type of the weight table.</typeparam>
    /// <seealso cref="aceCommonTypes.collection.nested.aceDictionary2D{aceCommonTypes.collection.tf_idf.IWeightTableTerm, aceCommonTypes.collection.tf_idf.IWeightTable, System.Int32}" />
    public class weightTableTermInSetCounter<TWeightTableTerm, TWeightTable> : aceDictionary2D<IWeightTableTerm, IWeightTable, int>
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'aceCommonTypes.collection.nested.aceDictionary2D{aceCommonTypes.collection.tf_idf.IWeightTableTerm, aceCommonTypes.collection.tf_idf.IWeightTable, System.Int32}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
        where TWeightTable : IWeightTable
        where TWeightTableTerm : IWeightTableTerm
    {
        public int AddVote(IWeightTable targetTable, IWeightTableTerm term)
        {
            this[term, targetTable] = this[term, targetTable] + term.AFreqPoints;
            return this[term, targetTable];
        }

        /// <summary>
        /// Gets the Binary Document Frequency, i.e. number of <see cref="IWeightTable"/>s containing the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public int GetBDF(IWeightTableTerm term)
        {
            return Enumerable.Count<KeyValuePair<IWeightTable, int>>(this[term]);
        }

        /// <summary>
        /// Gets Apsolute frequency accross all document (summary)
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public int GetAFreq(IWeightTableTerm term)
        {
            int A = 0;
            foreach (var t in this[term]) A += t.Value;
            //this[term]
            //this[term].ForEach(x => A += x.Value);
            return A;
        }

        /// <summary>
        /// Gets all <see cref="IWeightTable"/> containing matching terms
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public List<IWeightTable> GetTablesWithTerm(IWeightTableTerm term)
        {
            List<IWeightTable> output = new List<IWeightTable>();
            foreach (IWeightTable table in this[term].Keys)
            {
                output.Add(table);
            }
            return output;
        }
    }
}