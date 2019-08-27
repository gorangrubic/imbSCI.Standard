// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWeightTable.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting;
    using imbSCI.Data.interfaces;
    using System.Collections.Generic;
    using System.Data;

    //public enum weightTableScoreType

    /// <summary>
    /// Table of terms and frequencies on the document level
    /// </summary>
    public interface IWeightTable : IEnumerable<IWeightTableTerm>, IObjectWithName
    {
        IWeightTableTerm GetMatchByString(string term);

        /// <summary>
        /// Number of terms defined in the document. If <c>expandedCount</c> is TRUE it will also count all instances defined within one semantic term
        /// </summary>
        /// <param name="expandedCount">if set to <c>true</c> [expanded count].</param>
        /// <returns></returns>
        int Count(bool expandedCount);

        /// <summary>
        /// Name of the collection
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        // String name { get; set; }

        // weightTableMatchCollection<TWeightTableTerm, TSecondTableTerm> GetMatchAgainst<TWeightTableTerm,TSecondTableTerm>(weightTable<TSecondTableTerm> secondTable) where TSecondTableTerm : IWeightTableTerm where TWeightTableTerm : IWeightTableTerm;

        /// <summary>
        /// Gets the maximum frequency in the collection
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        int max { get; }

        /// <summary>
        /// Gets the sum of all frequencies
        /// </summary>
        /// <value>
        /// The sum.
        /// </value>
        int sum { get; }

        /// <summary>
        /// Gets the maximum weight
        /// </summary>
        /// <value>
        /// The maximum c weight.
        /// </value>
        double maxWeight { get; }

        /// <summary>
        /// Gets the sum weight. (no update check)
        /// </summary>
        /// <value>
        /// The sum weight.
        /// </value>
        double sumWeight { get; }

        /// <summary>
        /// Updates the maximum AFreq and CWeight - if chagnes occured since last call.
        /// </summary>
        void updateMaxValues();

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        IWeightTableSet parent { get; set; }

        /// <summary>
        /// Implementation for serialization
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        void Add(object term);

        /// <summary>
        /// Adds the specified term - or updates existing
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="AFreqPoints">Absolute frequency to set. Leave -1 for default</param>
        /// <param name="DFPoints">The Document Frequency to set. Leave -1 for default</param>
        /// <returns></returns>
        IWeightTableTerm Add(IWeightTableTerm term, int AFreqPoints = -1);

        /// <summary>
        /// Adds the specified term given as string
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="AFreqPoints">a freq points.</param>
        /// <param name="DFPoints">The df points.</param>
        /// <returns></returns>
        IWeightTableTerm Add(string term, int AFreqPoints = -1);

        /// <summary>
        /// Adds the specified terms into table
        /// </summary>
        /// <param name="terms">The terms.</param>
        List<IWeightTableTerm> Add(IEnumerable<IWeightTableTerm> terms);

        weightTableCompiled GetCompiledTable(ILogBuilder loger = null);

        ///// <summary>
        ///// Imports terms from
        ///// </summary>
        ///// <param name="document">The document.</param>
        //void Add(IWeightTableDocument document);

        IWeightTableTerm GetMatchTerm(IWeightTableTerm term, bool termOnNotFound = false);

        /// <summary>
        /// Gets the match term by the name .
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="termOnNotFound">if set to <c>true</c> [term on not found].</param>
        /// <returns></returns>
        IWeightTableTerm GetMatchTermByName(IWeightTableTerm term, bool termOnNotFound = false);

        /// <summary>
        /// Gets  the match term by the name.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        IWeightTableTerm GetMatchTermByName(string term);

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <param name="documentName">Name of the document.</param>
        /// <returns></returns>
        DataTable GetDataTable(string documentName = "", DataSet ds = null, bool addExtra = true);

        /// <summary>
        /// Exports clean version of data table - more compatibile for later load
        /// </summary>
        /// <param name="tableName">Name of the table document -- if empty it will use document name</param>
        /// <param name="ds">The DataSet to include into</param>
        /// <param name="onlyTermAndFreq">Exporting only term and frequency</param>
        /// <returns>DataTable with TF_IDF information</returns>
        DataTable GetDataTableClean(string tableName = "", DataSet ds = null, bool onlyTermAndFreq = false);

        /// <summary>
        /// Gets the normalized frequency of the specified term
        /// </summary>
        /// <param name="term">The term to get frequency for</param>
        /// <returns>Double ratio number with value from 0 to 1</returns>
        double GetNFreq(IWeightTableTerm term);

        /// <summary>
        /// Gets the cumulative weight for the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        double GetNFreq(string term);

        /// <summary>
        /// Gets the cumulative weight (sum of all weights)
        /// </summary>
        /// <returns></returns>
        double GetCWeight();

        /// <summary>
        /// Gets the weight for the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        double GetWeight(IWeightTableTerm term);

        /// <summary>
        /// Gets the weight for the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        double GetWeight(string term);

        /// <summary>
        /// Gets the normalized weight for the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        double GetNWeight(IWeightTableTerm term);

        /// <summary>
        /// Gets the normalized weight for the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        double GetNWeight(string term);

        /// <summary>
        /// Gets the absolute frequency of the specified term
        /// </summary>
        /// <param name="term">The term to get frequency for</param>
        /// <returns></returns>
        int GetAFreq(IWeightTableTerm term);

        /// <summary>
        /// Gets the absolute frequency for the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        int GetAFreq(string term);

        /// <summary>
        /// Determines whether the specified term is contained within the document
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns>
        ///   <c>true</c> if the specified term is match; otherwise, <c>false</c>.
        /// </returns>
        bool isMatch(IWeightTableTerm term);

        /// <summary>
        /// Heuristic method - comparing only the term name against the contained collection
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns>
        ///   <c>true</c> if [is match by name] [the specified term]; otherwise, <c>false</c>.
        /// </returns>
        bool isMatchByName(IWeightTableTerm term);

        /// <summary>
        /// Determines what kind of match this term might be to this table
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        weightTableMatchResultEnum isMatchBySemantics(IWeightTableTerm term);

        bool containsByName(string termName);

        /// <summary>
        /// Gets the binary document frequency of the specified term, i.e.: number of documents containing the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        double GetBDFreq(IWeightTableTerm term);

        double GetBDFreq(string term);

        /// <summary>
        /// Gets the tf idf (term frequency - inverse document frequency
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        double GetTF_IDF(IWeightTableTerm term);

        double GetTF_IDF(string term);

        /// <summary>
        /// Gets the idf - inverse document frequency
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        double GetIDF(IWeightTableTerm term);

        /// <summary>
        /// Gets the idf.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        double GetIDF(string term);

        /// <summary>
        /// Returns list of all terms in nominal form or by name
        /// </summary>
        /// <returns></returns>
        List<string> GetAllTermString();

        List<IWeightTableTerm> GetAllTerms();

        /// <summary>
        /// Gets the cross section of matched terms
        /// </summary>
        /// <param name="secondTable">The second table.</param>
        /// <param name="thisAgainstSecond">if set to <c>true</c> [this against second].</param>
        /// <returns></returns>
        List<IWeightTableTerm> GetCrossSection(IWeightTable secondTable, bool thisAgainstSecond = false);

#pragma warning disable CS1574 // XML comment has cref attribute 'dataException' that could not be resolved
        /// <summary>
        /// Loads term definitions from the specified DataTable, interpreting <c>termName_column</c> and <c>termAFreq_column</c>. Leave * to use export default column names.
        /// </summary>
        /// <param name="table">The source data table</param>
        /// <param name="termName_column">The term name column.</param>
        /// <param name="termAFreq_column">The term a freq column.</param>
        /// <param name="termDF_column">The column with DocumentFrequency - optional</param>
        /// <returns></returns>
        /// <exception cref="dataException">Column for TermName not found! - null - AddExternalDataTable() failed - TermName column not found in the input table
        /// or
        /// Column for TermAFreq not found! - null - AddExternalDataTable() failed - TermAFreq column not found in the input table</exception>
        int AddExternalDataTable(DataTable table, string termName_column = "*", string termAFreq_column = "*");
#pragma warning restore CS1574 // XML comment has cref attribute 'dataException' that could not be resolved

        /// <summary>
        /// Copies matching terms from external document&gt; adds new if have to, rise the abs frequency if exists
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="CopyFrequencies">if set to <c>true</c> [copy frequencies].</param>
        void AddExternalDocument(IWeightTable source, bool CopyFrequencies);

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

        bool RemoveTerm(string name);

        /// <summary>
        /// Removes the terms under specified weight.
        /// </summary>
        /// <param name="limit">The limit.</param>
        void RemoveUnderWeight(double limit = 0.1);

        /// <summary>
        /// Removes the terms with zero weidht.
        /// </summary>
        void RemoveZeroWeidht();
    }
}