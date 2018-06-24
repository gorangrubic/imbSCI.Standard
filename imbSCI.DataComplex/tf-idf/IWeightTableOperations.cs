// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWeightTableOperations.cs" company="imbVeles" >
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
    using imbSCI.Core.math.aggregation;
    using System.Collections.Generic;
    using System.Data;

    public interface IWeightTableOperations
    {
        /// <summary>
        /// Sets the term weight = as nominal fequency
        /// </summary>
        void SetWeightTo_NominalFrequency();

        /// <summary>
        /// Sets the weight of each term as proportion between absolute frequency and total sum of all frequencies
        /// </summary>
        void SetWeightTo_FrequencyRatio();

        /// <summary>
        /// Sets the weights according to current TF_IDF of a term
        /// </summary>
        void SetWeightTo_TF_IDF();

        /// <summary>
        /// Removes the terms under specified weight.
        /// </summary>
        /// <param name="limit">The limit.</param>
        void RemoveUnderWeight(double limit = 0.1);

        /// <summary>
        /// Removes the terms with zero weidht.
        /// </summary>
        void RemoveZeroWeidht();

        /// <summary>
        /// Loads term definitions from the specified DataTable, interpreting <c>termName_column</c> and <c>termAFreq_column</c>. Leave * to use export default column names.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="termName_column">The term name column.</param>
        /// <param name="termAFreq_column">The term a freq column.</param>
        /// <returns></returns>
        int AddExternalDataTable(DataTable table, string termName_column = "*", string termAFreq_column = "*");

        /// <summary>
        /// Copies matching terms from external document> adds new if have to, rise the abs frequency if exists
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="CopyFrequencies">if set to <c>true</c> [copy frequencies].</param>
        void AddExternalDocument(IWeightTable source, bool CopyFrequencies);

        List<IWeightTableTerm> GetCrossSection(IWeightTable secondTable, bool thisAgainstSecond = false);

        /// <summary>
        /// Queries table for specified terms and return aggregated score. The score source is specified by <see cref="termTableColumns.tf_idf"/> (only numeric columns are supported).
        /// </summary>
        /// <param name="queryTerms">Terms to test against the table, terms found are used in calculation.</param>
        /// <param name="scoreToUse">What numeric property of matched term to use for aggregation.</param>
        /// <param name="aggregation">The aggregation type</param>
        /// <returns>Any score information from the query terms is ignored.</returns>
        double GetScoreForMatch(IEnumerable<IWeightTableTerm> queryTerms, termTableColumns scoreToUse = termTableColumns.tf_idf, dataPointAggregationType aggregation = dataPointAggregationType.sum);

        /// <summary>
        /// Queries table for specified terms and return aggregated score. The score source is specified by <see cref="termTableColumns.tf_idf"/> (only numeric columns are supported).
        /// </summary>
        /// <param name="queryTerms">Terms to test against the table, terms found are used in calculation.</param>
        /// <param name="scoreToUse">What numeric property of matched term to use for aggregation.</param>
        /// <param name="aggregation">The aggregation type</param>
        /// <returns>Any score information from the query terms is ignored.</returns>
        double GetScoreForMatch(IEnumerable<string> queryTerms, termTableColumns scoreToUse = termTableColumns.tf_idf, dataPointAggregationType aggregation = dataPointAggregationType.sum);

        /// <summary>
        /// Returns the matching term entries
        /// </summary>
        /// <param name="queryTerms">The query terms.</param>
        /// <returns></returns>
        List<IWeightTableTerm> GetMatches(IEnumerable<IWeightTableTerm> queryTerms);

        /// <summary>
        /// Returns the matching term entries
        /// </summary>
        /// <param name="queryTerms">The query terms.</param>
        /// <returns></returns>
        List<IWeightTableTerm> GetMatches(IEnumerable<string> queryTerms);
    }
}