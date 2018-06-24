// --------------------------------------------------------------------------------------------------------------------
// <copyright file="weightTableSetOperations.cs" company="imbVeles" >
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
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.math.aggregation;
    using imbSCI.DataComplex.exceptions;
    using imbSCI.DataComplex.extensions.data.schema;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public static class weightTableSetOperations
    {
        /// <summary>
        /// Queries table for specified terms and return aggregated score. The score source is specified by <see cref="termTableColumns.tf_idf"/> (only numeric columns are supported).
        /// </summary>
        /// <param name="queryTerms">Terms to test against the table, terms found are used in calculation.</param>
        /// <param name="scoreToUse">What numeric property of matched term to use for aggregation.</param>
        /// <param name="aggregation">The aggregation type</param>
        /// <returns>Any score information from the query terms is ignored.</returns>
        public static double GetScoreForMatch(this IWeightTable table, IEnumerable<IWeightTableTerm> queryTerms, termTableColumns scoreToUse = termTableColumns.tf_idf, dataPointAggregationType aggregation = dataPointAggregationType.sum)
        {
            List<IWeightTableTerm> output = new List<IWeightTableTerm>();
            output = table.GetMatches(queryTerms);
            return output.GetScoreAggregate(table, scoreToUse, aggregation);
        }

        /// <summary>
        /// Queries table for specified terms and return aggregated score. The score source is specified by <see cref="termTableColumns.tf_idf"/> (only numeric columns are supported).
        /// </summary>
        /// <param name="queryTerms">Terms to test against the table, terms found are used in calculation.</param>
        /// <param name="scoreToUse">What numeric property of matched term to use for aggregation.</param>
        /// <param name="aggregation">The aggregation type</param>
        /// <returns>Any score information from the query terms is ignored.</returns>
        public static double GetScoreForMatch(this IWeightTable table, IEnumerable<string> queryTerms, termTableColumns scoreToUse = termTableColumns.tf_idf, dataPointAggregationType aggregation = dataPointAggregationType.sum)
        {
            List<IWeightTableTerm> output = new List<IWeightTableTerm>();

            output = table.GetMatches(queryTerms);
            return output.GetScoreAggregate(table, scoreToUse, aggregation);
        }

        /// <summary>
        /// Returns the matching term entries
        /// </summary>
        /// <param name="queryTerms">The query terms.</param>
        /// <returns></returns>
        public static List<IWeightTableTerm> GetMatches(this IWeightTable table, IEnumerable<IWeightTableTerm> queryTerms)
        {
            List<IWeightTableTerm> output = new List<IWeightTableTerm>();
            List<string> expandedQuery = new List<string>();

            foreach (IWeightTableTerm qt in queryTerms)
            {
                expandedQuery.AddUnique(qt.GetAllForms());
            }

            //queryTerms.ForEach(x => expandedQuery.AddRangeUnique(x.GetAllForms()));
            return table.GetMatches(expandedQuery);
        }

        /// <summary>
        /// Returns the matching term entries
        /// </summary>
        /// <param name="queryTerms">The query terms.</param>
        /// <returns></returns>
        public static List<IWeightTableTerm> GetMatches(this IWeightTable table, IEnumerable<string> queryTerms)
        {
            List<IWeightTableTerm> output = new List<IWeightTableTerm>();
            foreach (string term in queryTerms)
            {
                var mc = table.GetMatchByString(term);
                if (mc != null)
                {
                    collectionExtensions.AddUnique(output, mc);
                }
            }
            return output;
        }

        public static double GetScoreAggregate(this IEnumerable<IWeightTableTerm> terms, IWeightTable table, termTableColumns scoreToUse = termTableColumns.tf_idf, dataPointAggregationType aggregation = dataPointAggregationType.sum)
        {
            List<double> output = new List<double>();

            foreach (IWeightTableTerm term in terms)
            {
                switch (scoreToUse)
                {
                    case termTableColumns.cw:
                        output.Add(table.GetWeight(term));
                        break;

                    case termTableColumns.df:
                        output.Add(table.GetBDFreq(term));
                        break;

                    case termTableColumns.freqAbs:
                        output.Add(table.GetAFreq(term));
                        break;

                    case termTableColumns.freqNorm:
                        output.Add(table.GetNFreq(term));
                        break;

                    case termTableColumns.idf:
                        output.Add(table.GetIDF(term));
                        break;

                    case termTableColumns.ncw:
                        output.Add(table.GetNWeight(term));
                        break;

                    case termTableColumns.none:
                        break;

                    case termTableColumns.words:
                    case termTableColumns.normalizedSemanticDistance:
                    case termTableColumns.semanticDistance:
                    case termTableColumns.termLemma:
                    case termTableColumns.termName:
                        throw new NotImplementedException();
                        break;

                    case termTableColumns.tf_idf:
                        output.Add(table.GetTF_IDF(term));
                        break;
                }
            }

            switch (aggregation)
            {
                case dataPointAggregationType.avg:
                    return output.Average();
                    break;

                case dataPointAggregationType.count:
                    return output.Count();
                    break;

                case dataPointAggregationType.max:
                    return output.Max();
                    break;

                case dataPointAggregationType.min:
                    return output.Min();
                    break;

                case dataPointAggregationType.range:
                    return output.Max() - output.Min();
                    break;

                case dataPointAggregationType.sum:
                    return output.Sum();
                    break;

                default:
                    throw new dataException("Operation not supported [" + aggregation.toString() + "]", null, table, "Aggregation operation not supported");
                    return 0;
                    break;
            }

            return 0;
        }

        public static List<weightTableGenericTerm> GetList(this IEnumerable<string> tokens)
        {
            List<weightTableGenericTerm> output = new List<weightTableGenericTerm>();
            foreach (string tkn in tokens)
            {
                weightTableGenericTerm term = new weightTableGenericTerm(tkn, 1);

                output.Add(term);
            }

            return output;
        }

        /// <summary>
        /// Builds the frequency table shema
        /// </summary>
        /// <param name="documentName">Name of the document.</param>
        /// <param name="tableDataSet">The table data set.</param>
        /// <returns></returns>
        /// <exception cref="aceCommonTypes.core.exceptions.dataException">Table name selection failed into stackoverflow - null - Table autoname failed [" + i.ToString() + "]</exception>
        public static DataTable buildFrequencyTable(string documentName, DataSet tableDataSet = null)
        {
            if (documentName.isNullOrEmpty()) documentName = "TermTable";
            if (tableDataSet == null) tableDataSet = new DataSet(documentName + "_set");

            DataTable output = new DataTable(documentName);
            string proposal = documentName;

            int i = 0;
            while (tableDataSet.Tables.Contains(output.TableName))
            {
                output.TableName = proposal + i.ToString("D" + 4);
                if (i > 10000) throw new dataException("Table name selection failed into stackoverflow", null, output, "Table autoname failed [" + i.ToString() + "]");
            }

            output.Add(termTableColumns.termName, "Nominal form of the term", "T_n", typeof(string), dataPointImportance.normal);
            output.Add(termTableColumns.freqAbs, "Absolute frequency - number of occurences", "T_af", typeof(int), dataPointImportance.normal, "D", "Abs. freq.");
            output.Add(termTableColumns.freqNorm, "Normalized frequency - abs. frequency divided by the maximum", "T_nf", typeof(double), dataPointImportance.important, "#0.00000");
            output.Add(termTableColumns.df, "Document frequency - number of documents containing the term", "T_df", typeof(int), dataPointImportance.normal);
            output.Add(termTableColumns.idf, "Inverse document frequency - logaritmicly normalized T_df", "T_idf", typeof(double), dataPointImportance.normal, "#0.00000");
            output.Add(termTableColumns.tf_idf, "Term frequency Inverse document frequency - calculated as TF-IDF", "T_tf-idf", typeof(double), dataPointImportance.important, "#0.00000");
            output.Add(termTableColumns.cw, "Cumulative weight of term", "T_cw", typeof(double), dataPointImportance.normal, "#0.00000");
            output.Add(termTableColumns.ncw, "Normalized cumulative weight of term", "T_ncw", typeof(double), dataPointImportance.important, "#0.00000");

            tableDataSet.Tables.Add(output);

            return output;
        }
    }
}