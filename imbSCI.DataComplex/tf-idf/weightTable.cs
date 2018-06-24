// --------------------------------------------------------------------------------------------------------------------
// <copyright file="weightTable.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.math;
    using imbSCI.Core.reporting;
    using imbSCI.Data.data;
    using imbSCI.Data.enums.fields;
    using imbSCI.DataComplex.exceptions;
    using imbSCI.DataComplex.extensions.data.modify;
    using imbSCI.DataComplex.extensions.data.operations;
    using imbSCI.DataComplex.extensions.data.schema;
    using imbSCI.DataComplex.tables.extensions;
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Universal weight table (SVM / TF-IDF model)
    /// </summary>
    /// <seealso cref="imbSCI.Data.data.changeBindableBase" />
    /// <seealso cref="IWeightTable" />
    public abstract class weightTable<TWeightTableTerm> : changeBindableBase, IWeightTable
        where TWeightTableTerm : IWeightTableTerm, new()
    {
        /// <summary>
        /// Gets the match against.
        /// </summary>
        /// <typeparam name="TSecondTableTerm">The type of the second table term.</typeparam>
        /// <param name="secondTable">The second table.</param>
        /// <returns></returns>
        public weightTableMatchCollection<TWeightTableTerm, TSecondTableTerm> GetMatchAgainst<TSecondTableTerm>(weightTable<TSecondTableTerm> secondTable)
            where TSecondTableTerm : IWeightTableTerm, new()

        {
            weightTableMatchCollection<TWeightTableTerm, TSecondTableTerm> output = new weightTableMatchCollection<TWeightTableTerm, TSecondTableTerm>(this, secondTable);

            foreach (TWeightTableTerm term in this.ToList())
            {
                TSecondTableTerm match = (TSecondTableTerm)secondTable.GetMatchTerm(term);
                if (match != null)
                {
                    output.Add(match, term);
                }
            }
            return output;
        }

        /// <summary>
        /// Gets the cross section of matched terms
        /// </summary>
        /// <param name="secondTable">The second table.</param>
        /// <param name="thisAgainstSecond">if set to <c>true</c> [this against second].</param>
        /// <returns></returns>
        public List<IWeightTableTerm> GetCrossSection(IWeightTable secondTable, bool thisAgainstSecond = false)
        {
            List<IWeightTableTerm> matched = new List<IWeightTableTerm>();

            if (thisAgainstSecond)
            {
                foreach (IWeightTableTerm term in this.ToList())
                {
                    var match = GetMatchTerm(term);
                    match = secondTable.GetMatchTerm(term);
                    if (match != null)
                    {
                        matched.Add(match);
                    }
                }
            }
            else
            {
                foreach (IWeightTableTerm term in secondTable.ToList())
                {
                    var match = GetMatchTerm(term);
                    if (match != null)
                    {
                        matched.Add(match);
                    }
                }
            }
            /*

            */
            return matched;
        }

        public weightTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="weightTable"/> class.
        /// </summary>
        /// <param name="__parent">The parent.</param>
        public weightTable(IWeightTableSet __parent, string __name)
        {
            parent = __parent;
            name = __name;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string name { get; set; } = "tf_idf";

        public IWeightTableSet parent { get; set; }

        /// <summary>
        /// Gets or sets the terms.
        /// </summary>
        /// <value>
        /// The terms.
        /// </value>
        public ConcurrentDictionary<string, IWeightTableTerm> terms { get; protected set; } = new ConcurrentDictionary<string, IWeightTableTerm>();

        /// <summary>
        ///
        /// </summary>
        protected ConcurrentDictionary<string, int> termsAFreq { get; set; } = new ConcurrentDictionary<string, int>();

        /// <summary>
        /// Builds the frequency table shema
        /// </summary>
        /// <param name="documentName">Name of the document.</param>
        /// <param name="tableDataSet">The table data set.</param>
        /// <returns></returns>
        /// <exception cref="aceCommonTypes.core.exceptions.dataException">Table name selection failed into stackoverflow - null - Table autoname failed [" + i.ToString() + "]</exception>
        protected DataTable buildFrequencyTable(string documentName, DataSet tableDataSet = null)
        {
            if (documentName.isNullOrEmpty()) documentName = name;

            if (tableDataSet == null) tableDataSet = new DataSet(documentName + "_set");

            DataTable output = new DataTable(documentName);
            string proposal = documentName.makeUniqueName(tableDataSet.Tables.Contains);

            output.TableName = proposal;

            /*
            Int32 i = 0;
            while (tableDataSet.Tables.Contains(output.TableName))
            {
                output.TableName = proposal + i.ToString("D" + 4);
                i++;
                if (i > 10000) throw new dataException("Table name selection failed into stackoverflow", null, output, "Table autoname failed [" + i.ToString() + "]");
            }
            */
            output = buildTableShema(output);

            //output.SetEncode(extensions.text.toDosCharactersMode.toCleanAndXChars);

            tableDataSet.AddTable(output);

            return output;
        }

        /// <summary>
        /// If TRUE allows only one add per table
        /// </summary>
        /// <value>
        /// <c>true</c> if [term single add allowed]; otherwise, <c>false</c>.
        /// </value>
        public abstract bool termSingleAddAllowed { get; }

        //{
        //    get
        //    {
        //        return false;

        //    }
        //}

        public abstract DataTable buildTableShema(DataTable output);

        //{
        //output.Add(termTableColumns.termName, "Nominal form of the term", "T_n", typeof(String), dataPointImportance.normal);
        //    output.Add(termTableColumns.freqAbs, "Absolute frequency - number of occurences", "T_af", typeof(Int32), dataPointImportance.normal, "Abs. freq.");
        //    output.Add(termTableColumns.freqNorm, "Normalized frequency - abs. frequency divided by the maximum", "T_nf", typeof(Double), dataPointImportance.important, "#0.00000");
        //    output.Add(termTableColumns.df, "Document frequency - number of documents containing the term", "T_df", typeof(Int32), dataPointImportance.normal);
        //    output.Add(termTableColumns.idf, "Inverse document frequency - logaritmicly normalized T_df", "T_idf", typeof(Double), dataPointImportance.normal, "#0.00000");
        //    output.Add(termTableColumns.tf_idf, "Term frequency Inverse document frequency - calculated as TF-IDF", "T_tf-idf", typeof(Double), dataPointImportance.important, "#0.00000");
        //    output.Add(termTableColumns.cw, "Cumulative weight of term", "T_cw", typeof(Double), dataPointImportance.normal, "#0.00000");
        //    output.Add(termTableColumns.ncw, "Normalized cumulative weight of term", "T_ncw", typeof(Double), dataPointImportance.important, "#0.00000");
        //    return output;
        //}

        public abstract DataRow buildTableRow(DataRow dr, TWeightTableTerm t);

        //{
        //    dr.SetData(termTableColumns.termName, t.name);
        //    dr.SetData(termTableColumns.freqAbs, termsAFreq[t.name]);
        //    dr.SetData(termTableColumns.freqNorm, GetNFreq(t.name));
        //    dr.SetData(termTableColumns.idf, GetIDF(t.name));
        //    dr.SetData(termTableColumns.tf_idf, GetTF_IDF(t.name));
        //    dr.SetData(termTableColumns.df, GetBDFreq(t.name));
        //    dr.SetData(termTableColumns.cw, GetWeight(t.name));
        //    dr.SetData(termTableColumns.ncw, GetNWeight(t.name));
        //    return dr;
        //}

        //public DataSet GetDataTableWithInfo

        public virtual DataTable GetDataTable(string documentName = "", DataSet ds = null, bool addExtra = true)
        {
            if (documentName.isNullOrEmpty()) documentName = name;
            DataTable output = buildFrequencyTable(documentName, ds);

            if (addExtra)
            {
                output.AddRowNameColumn("Row name", true);
                output.AddRowDescriptionColumn("Row info", true);

                output.AddLineRow();
            }
            foreach (TWeightTableTerm t in terms.Values)
            {
                DataRow dr = output.NewRow();
                dr = buildTableRow(dr, t);
                output.Rows.Add(dr);
            }

            output.setColumnWidths(100);

            if (addExtra)
            {
                output.AddLineRow();
                output.AddExtraRowInfo(templateFieldDataTable.col_caption).SetName("Name").SetDesc("Descriptive name for the data point");
                //  output.AddExtraRowInfo(templateFieldDataTable.col_name).SetName("Code name").SetDesc("Code name used in the source code");
                output.AddExtraRowInfo(templateFieldDataTable.col_letter).SetName("Notation").SetDesc("Letter-code notation used in the article");
                output.AddExtraRowInfo(templateFieldDataTable.col_desc).SetName("Description").SetDesc("Description of the value columns below");

                output.AddRow("Max. frequency").Set(1, max.ToString()).SetDesc("The highest number of occurences in the document");
                output.AddRow("Max. cumulative weight").Set(1, maxWeight.ToString()).SetDesc("The highest cumulative weight in the document");
            }
            return output;
        }

        /// <summary>
        /// Generates a compiled version of TF-IDF table. <see cref="weightTableCompiled"/>
        /// </summary>
        /// <param name="loger">The loger - for diagnostics</param>
        /// <returns></returns>
        public weightTableCompiled GetCompiledTable(ILogBuilder loger = null)
        {
            weightTableCompiled output = new weightTableCompiled(name);

            int ti = 0;
            int ts = 10;
            int c = 0;
            int tc = Count();
            int input_c = 0;
            int output_c = 0;
            double io_r = 0;

            updateMaxValues();

            foreach (IWeightTableTerm t in terms.Values)
            {
                double tp = ti.GetRatio(tc);

                weightTableTermCompiled cterm = GetCompiledTerm(t); //output.Add(t, GetAFreq(t.nominalForm)) as weightTableTermCompiled;

                output.AddOrUpdate(cterm);

                if (c > 10)
                {
                    c = 0;
                    io_r = input_c.GetRatio(output_c);
                    if (loger != null) loger.AppendLine("TF-IDF [" + name + "] table compiled [" + tp.ToString("P2") + "]");
                }
            }

            output.updateMaxValues();

            return output;
        }

        protected weightTableTermCompiled GetCompiledTerm(IWeightTableTerm term)
        {
            weightTableTermCompiled cterm = new weightTableTermCompiled();
            cterm.termName = term.nominalForm;
            cterm.termInflections = term.GetAllForms(false).toCsvInLine();
            cterm.df = GetBDFreq((string)term.name);
            cterm.idf = GetIDF((string)term.name);
            cterm.freqAbs = termsAFreq[term.name];
            cterm.freqNorm = ((double)cterm.freqAbs / (double)max);
            cterm.tf_idf = cterm.idf * cterm.freqNorm;
            cterm.cw = cterm.weight;
            cterm.ncw = cterm.weight / maxWeight;
            return cterm;
        }

        /// <summary>
        /// Gets the data table clean.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="onlyTermAndFreq">if set to <c>true</c> [only term and freq].</param>
        /// <returns></returns>
        public DataTable GetDataTableClean(string tableName = "", DataSet ds = null, bool onlyTermAndFreq = false)
        {
            if (tableName.isNullOrEmpty()) tableName = name;

            DataTable output = new DataTable();
            output.SetTitle(tableName);

            output.Add(termTableColumns.termName, "Nominal form of the term", "Tn", typeof(string), dataPointImportance.normal);
            output.Add(termTableColumns.termInflections, "Inflected words or otherwise related terms in the same semantic cloud, as CSV", "Ti", typeof(string), dataPointImportance.normal);
            output.Add(termTableColumns.freqAbs, "Absolute frequency - number of occurences", "T_af", typeof(int), dataPointImportance.normal, "Abs. freq.");

            if (!onlyTermAndFreq)
            {
                output.Add(termTableColumns.freqNorm, "Normalized frequency - abs. frequency divided by the maximum", "T_nf", typeof(double), dataPointImportance.important, "#0.00000");
                output.Add(termTableColumns.df, "Document frequency - number of documents containing the term", "T_df", typeof(int), dataPointImportance.normal);
                output.Add(termTableColumns.idf, "Inverse document frequency - logaritmicly normalized T_df", "T_idf", typeof(double), dataPointImportance.normal, "#0.00000");
                output.Add(termTableColumns.tf_idf, "Term frequency Inverse document frequency - calculated as TF-IDF", "T_tf-idf", typeof(double), dataPointImportance.important, "#0.00000");
                output.Add(termTableColumns.cw, "Cumulative weight of term", "T_cw", typeof(double), dataPointImportance.normal, "#0.00000");
                output.Add(termTableColumns.ncw, "Normalized cumulative weight of term", "T_ncw", typeof(double), dataPointImportance.important, "#0.00000");
            }

            foreach (IWeightTableTerm t in terms.Values)
            {
                DataRow dr = output.NewRow();

                dr[nameof(termTableColumns.termName)] = t.name;
                List<string> _all = t.GetAllForms(false);

                dr[nameof(termTableColumns.termInflections)] = _all.toCsvInLine();
                dr[nameof(termTableColumns.freqAbs)] = GetAFreq(t.nominalForm);

                if (!onlyTermAndFreq)
                {
                    dr[nameof(termTableColumns.freqNorm)] = GetNFreq(t.nominalForm);
                    dr[nameof(termTableColumns.df)] = GetBDFreq(t.nominalForm);
                    dr[nameof(termTableColumns.idf)] = GetIDF(t.nominalForm);
                    dr[nameof(termTableColumns.tf_idf)] = GetTF_IDF(t.nominalForm);
                    dr[nameof(termTableColumns.cw)] = GetWeight(t.nominalForm);
                    dr[nameof(termTableColumns.ncw)] = GetNWeight(t.nominalForm);
                }

                output.Rows.Add(dr);
            }

            if (ds != null) ds.AddTable(output);

            return output;
        }

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
        public int AddExternalDataTable(DataTable table, string termName_column = "*", string termAFreq_column = "*")
        {
            int c = Count();
            if (table == null) return 0;
            if (table.Rows.Count == 0) return 0;

            if (termAFreq_column == "*") termAFreq_column = nameof(termTableColumns.freqAbs);
            if (termName_column == "*") termName_column = nameof(termTableColumns.termName);
            //if (termDF_column == "*") termDF_column = nameof(termTableColumns.df);

            DataColumn dc_AFreq = null;
            DataColumn dc_Name = null;
            // DataColumn dc_DF = null;

            foreach (DataColumn dc in table.Columns)
            {
                if (dc.ColumnName == termAFreq_column)
                {
                    dc_AFreq = dc;
                }
                else if (dc.ColumnName == termName_column)
                {
                    dc_Name = dc;
                }

                //} else if (dc.ColumnName == termDF_column)
                //{
                //    dc_DF = dc;
                //}
            }

            if (dc_Name == null) throw new dataException("Column for TermName not found!", null, this, "AddExternalDataTable() failed - TermName column not found in the input table");
            if (dc_AFreq == null) throw new dataException("Column for TermAFreq not found!", null, this, "AddExternalDataTable() failed - TermAFreq column not found in the input table");

            foreach (DataRow dr in table.Rows)
            {
                string t_name = dr[dc_Name] as string;
                int t_afreq = (int)dr[dc_AFreq];
                // if (dc_DF == null)
                // {
                Add(t_name, t_afreq);
                // } else
                // {
                //  Add(t_name, t_afreq, (Int32) dr[dc_DF]);
                // }
            }

            return Count() - c;
        }

        /// <summary>
        /// Adds the specified term string.
        /// </summary>
        /// <param name="term_str">The term string.</param>
        /// <param name="AFreqPoints">a freq points to be added. Leave -1 for default (1)</param>
        public IWeightTableTerm Add(string term_str, int AFreqPoints = -1)
        {
            IWeightTableTerm term = GetMatchTermByName(term_str);

            if (term == null)
            {
                TWeightTableTerm tmp_termp = new TWeightTableTerm();
                tmp_termp.name = term_str;

                tmp_termp.Define(term_str, term_str);
                return Add(tmp_termp, AFreqPoints);
            }

            Add(term, AFreqPoints);
            return term;
        }

        /// <summary>
        /// Copies matching terms from external document&gt; adds new if have to, rise the abs frequency if exists
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="CopyFrequencies">if set to <c>true</c> [copy frequencies].</param>
        public void AddExternalDocument(IWeightTable source, bool CopyFrequencies)
        {
            int c = Count();
            List<string> new_terms = source.GetAllTermString();
            foreach (var nt in source.GetAllTerms())
            {
                if (CopyFrequencies)
                {
                    Add(source.GetMatchTermByName(nt), source.GetAFreq(nt));
                }
                else
                {
                    Add(source.GetMatchTermByName(nt));
                }
            }

            int a = Count() - c;
        }

        /// <summary>
        /// Implementation for serialization
        /// </summary>
        /// <param name="term">The term.</param>
        public void Add(object term)
        {
            if (term is IWeightTableTerm)
            {
                IWeightTableTerm term_IWeightTableTerm = (IWeightTableTerm)term;
                Add(term_IWeightTableTerm, -1);
            }
        }

        /// <summary>
        /// Adds the specified terms into table
        /// </summary>
        /// <param name="terms">The terms.</param>
        /// <returns></returns>
        public virtual List<IWeightTableTerm> Add(IEnumerable<IWeightTableTerm> terms)
        {
            List<IWeightTableTerm> output = new List<IWeightTableTerm>();
            foreach (IWeightTableTerm t in terms)
            {
                output.Add(Add(t));
            }
            return output;
        }

        public virtual IWeightTableTerm Add(weightTableTermCompiled term)
        {
            if (term == null)
            {
                return null;
            }
            var t = GetMatchTerm(term);
            if (t == null)
            {
                t = new TWeightTableTerm();
                t.name = term.name;
                t.SetOtherForms(term.GetAllForms(false));

                terms.TryAdd(term.name, t);

                termsAFreq.TryAdd(term.name, term.AFreqPoints);
            }
            else
            {
                //  if (DFPoints == -1) DFPoints = 0;
                // if (DFPoints > 1) DFPoints--;
                if (!termSingleAddAllowed)
                {
                    //term.weight += term.weight;
                    //if (AFreqPoints == -1) AFreqPoints = 1;

                    termsAFreq[t.name] = termsAFreq[t.name] + term.AFreqPoints;
                }
            }

            if (parent != null)
            {
                parent.Add(this, t, false);
            }

            InvokeChanged();
            return t;
        }

        /// <summary>
        /// Adds the specified term - or updates existing
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="AFreqPoints">a freq points.</param>
        /// <returns></returns>
        public virtual IWeightTableTerm Add(IWeightTableTerm term, int AFreqPoints = -1)
        {
            if (term == null)
            {
                return null;
            }
            var t = GetMatchTerm(term);
            if (t == null)
            {
                // if (DFPoints == -1) DFPoints = 1;
                terms.TryAdd(term.name, term);
                if (AFreqPoints == -1) AFreqPoints = term.AFreqPoints;
                termsAFreq.TryAdd(term.name, AFreqPoints);
                t = term;
            }
            else
            {
                //  if (DFPoints == -1) DFPoints = 0;
                // if (DFPoints > 1) DFPoints--;
                if (!termSingleAddAllowed)
                {
                    t.weight += term.weight;
                    if (AFreqPoints == -1) AFreqPoints = 1;

                    termsAFreq[t.name] = termsAFreq[t.name] + AFreqPoints;
                }
            }

            if (parent != null)
            {
                parent.Add(this, term, false);
            }

            InvokeChanged();
            return t;
        }

        /// <summary>
        /// Determines whether [contains by name] [the specified term name].
        /// </summary>
        /// <param name="termName">Name of the term.</param>
        /// <returns>
        ///   <c>true</c> if [contains by name] [the specified term name]; otherwise, <c>false</c>.
        /// </returns>
        public bool containsByName(string termName)
        {
            return (terms.ContainsKey(termName));
        }

        /// <summary>
        /// Gets the absolute frequency of the specified term
        /// </summary>
        /// <param name="term">The term to get frequency for</param>
        /// <returns></returns>
        public virtual int GetAFreq(IWeightTableTerm term)
        {
            IWeightTableTerm t = GetMatchTerm(term);
            if (t != null)
            {
                return termsAFreq[t.name];
            }

            return 0;
        }

        public IEnumerator<IWeightTableTerm> GetEnumerator()
        {
            List<IWeightTableTerm> output = new List<IWeightTableTerm>();
            foreach (var t in terms.Values) output.Add(t);
            //terms.Values.ForEach(x => output.Add(x));
            //output.AddRange(terms.Values.toList());
            return output.GetEnumerator();
        }

        private int _max = int.MinValue;

        /// <summary> </summary>
        public int max
        {
            get
            {
                return _max;
            }
            protected set
            {
                _max = value;
                OnPropertyChanged("max");
            }
        }

        private int _sum = 0;

        /// <summary> </summary>
        public int sum
        {
            get
            {
                return _sum;
            }
            protected set
            {
                _sum = value;
                OnPropertyChanged("sum");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public double maxWeight { get; protected set; } = 0;

        /// <summary>
        /// Updates the maximum AFreq and CWeight - if chagnes occured since last call.
        /// </summary>
        public void updateMaxValues()
        {
            _sum = 0;
            _max = int.MinValue;
            maxWeight = 0;
            _sumWeights = 0;

            foreach (var pair in terms.toList())
            {
                IWeightTableTerm t = pair.Value;
                _sum = _sum + termsAFreq[t.name];
                _max = Math.Max(_max, termsAFreq[t.name]);
                maxWeight = Math.Max(maxWeight, t.weight);
                _sumWeights = _sumWeights + t.weight;
            }

            Accept();
        }

        /// <summary>
        /// Gets the normalized frequency of the specified term
        /// </summary>
        /// <param name="term">The term to get frequency for</param>
        /// <returns>
        /// Double ratio number with value from 0 to 1
        /// </returns>
        public virtual double GetNFreq(IWeightTableTerm term)
        {
            int abs = GetAFreq(term);

            if (HasChanges) updateMaxValues();

            return ((double)abs) / ((double)max);
        }

        /// <summary>
        /// Determines what kind of match this term might be to this table
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public weightTableMatchResultEnum isMatchBySemantics(IWeightTableTerm term)
        {
            weightTableMatchResultEnum output = weightTableMatchResultEnum.none;

            if (terms.ContainsKey(term.nominalForm)) return weightTableMatchResultEnum.hostTermName_and_needleTermName;

            foreach (var t in this)
            {
                if (t.isMatch(term))
                {
                    // <--------------------- nije potpuna implementacija --- jer mozda bude i hostTermInstance_and_needleTermInstance
                    return weightTableMatchResultEnum.hostTermInstance_and_needleTermName;
                }
            }

            var allForms = term.GetAllForms();
            foreach (string form in allForms)
            {
                if (terms.ContainsKey(form)) return weightTableMatchResultEnum.hostTermName_and_needleTermInstance;
            }

            return output;
        }

        /// <summary>
        /// Determines whether the specified term is contained within the document
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns>
        ///   <c>true</c> if the specified term is match; otherwise, <c>false</c>.
        /// </returns>
        public bool isMatch(IWeightTableTerm term)
        {
            foreach (var t in this)
            {
                if (t.isMatch(term))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Heuristic method - comparing only the term name against the contained collection
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns>
        ///   <c>true</c> if [is match by name] [the specified term]; otherwise, <c>false</c>.
        /// </returns>
        public bool isMatchByName(IWeightTableTerm term)
        {
            return (terms.ContainsKey(term.name));
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return terms.Values.toList().GetEnumerator();
        }

        private object GetMatchTermLock = new object();

        /// <summary>
        /// Gets the match term.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="termOnNotFound">if set to <c>true</c> [term on not found].</param>
        /// <returns></returns>
        public IWeightTableTerm GetMatchTerm(IWeightTableTerm term, bool termOnNotFound = false)
        {
            IWeightTableTerm mt = null;
            /*
            List<String> keys = terms.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                var t = terms[keys[i]];
                if (t.isMatch(term))
                {
                    return t;
                }
            }*/

            foreach (var tpair in terms)
            {
                var t = tpair.Value;
                if (t.isMatch(term))
                {
                    mt = t;
                    return mt;
                }
            }

            if (termOnNotFound) mt = term;
            return mt;
        }

        /// <summary>
        /// Gets the match term by the name .
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="termOnNotFound">if set to <c>true</c> if will return the same term supplied</param>
        /// <returns></returns>
        public IWeightTableTerm GetMatchTermByName(IWeightTableTerm term, bool termOnNotFound = false)
        {
            if (terms.ContainsKey(term.name)) return terms[term.name];
            if (termOnNotFound) return term;
            return null;
        }

        /// <summary>
        /// Gets  the match term by the name.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public IWeightTableTerm GetMatchTermByName(string term)
        {
            if (terms.ContainsKey(term)) return terms[term];

            return null;
        }

        /// <summary>
        /// Gets the match by string. The specified <c>term</c> must be lowercase, without spaces, interpunction etc. It tries with direct match (LemmaForm), then if fails tries with full search
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public IWeightTableTerm GetMatchByString(string term)
        {
            TWeightTableTerm tr = (TWeightTableTerm)GetMatchTermByName(term);
            if (tr != null) return tr;

            foreach (TWeightTableTerm t in terms.Values)
            {
                if (t.GetAllForms().Any(x => x.Equals(term, StringComparison.CurrentCultureIgnoreCase)))
                {
                    return t;
                }
            }
            return default(TWeightTableTerm);
        }

        /// <summary>
        /// Gets the normalized frequency by term name
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public double GetNFreq(string term)
        {
            int abs = GetAFreq(term);
            if (abs == 0) return 0;
            if (HasChanges) updateMaxValues();

            return ((double)abs) / ((double)max);
        }

        /// <summary>
        /// Gets the cumulative weight for the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public virtual double GetWeight(IWeightTableTerm term)
        {
            IWeightTableTerm t = GetMatchTerm(term);
            if (t == null) return 0;
            return t.weight;
        }

        /// <summary>
        /// Gets the cumulative weight for the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public virtual double GetWeight(string term)
        {
            IWeightTableTerm t = GetMatchTermByName(term);
            if (t == null) return 0;
            return t.weight;
        }

        /// <summary>
        /// Gets the normalized cumulative weight for the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public virtual double GetNWeight(IWeightTableTerm term)
        {
            IWeightTableTerm t = GetMatchTerm(term);
            if (t == null) return 0;
            if (HasChanges) updateMaxValues();
            if (t.weight == 0) return 0;
            if (maxWeight == 0) return 0;
            return t.weight / maxWeight;
        }

        /// <summary>
        /// Gets the normalized cumulative weight for the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public virtual double GetNWeight(string term)
        {
            IWeightTableTerm t = GetMatchTermByName(term);
            if (t == null) return 0;
            if (HasChanges) updateMaxValues();
            if (t.weight == 0) return 0;
            if (maxWeight == 0) return 0;
            return t.weight / maxWeight;
        }

        /// <summary>
        /// Gets the absolute frequency for the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public virtual int GetAFreq(string term)
        {
            IWeightTableTerm t = GetMatchTermByName(term);
            if (t != null)
            {
                return termsAFreq[t.name];
            }

            return 0;
        }

        /// <summary>
        /// Gets the binary document frequency of the specified term, i.e.: number of documents containing the term
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public virtual double GetBDFreq(IWeightTableTerm term)
        {
            if (parent != null)
            {
                return parent.GetBDFreq(term);
            }
            return 1;
        }

        /// <summary>
        /// Gets the tf idf (term frequency - inverse document frequency
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public virtual double GetTF_IDF(IWeightTableTerm term)
        {
            return GetIDF(term) * GetNFreq(term);
        }

        /// <summary>
        /// Gets the idf - inverse document frequency
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public virtual double GetIDF(IWeightTableTerm term)
        {
            if (parent != null)
            {
                return parent.GetIDF(term);
            }
            return 1;
        }

        /// <summary>
        /// Gets the bd freq.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public virtual double GetBDFreq(string term)
        {
            if (parent != null)
            {
                return parent.GetBDFreq(term);
            }
            return 1;
        }

        /// <summary>
        /// Gets the tf idf.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public virtual double GetTF_IDF(string term)
        {
            var idf = GetIDF(term);
            var tnf = GetNFreq(term);

            return idf * tnf;
        }

        /// <summary>
        /// Gets the idf.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public virtual double GetIDF(string term)
        {
            if (parent != null)
            {
                return parent.GetIDF(term);
            }
            return 1;
        }

        /// <summary>
        /// Sets the term weight = as nominal fequency
        /// </summary>
        public void SetWeightTo_NominalFrequency()
        {
            foreach (var t in terms.Values)
            {
                t.weight = GetNFreq((string)t.name);
            }

            InvokeChanged();
        }

        /// <summary>
        /// Sets the weight of each term as proportion between absolute frequency and total sum of all frequencies
        /// </summary>
        public void SetWeightTo_FrequencyRatio()
        {
            foreach (var t in terms.Values)
            {
                t.weight = ((double)GetNFreq((string)t.name) / ((double)sum));
            }
            InvokeChanged();
        }

        /// <summary>
        /// Sets the weights according to current TF_IDF of a term
        /// </summary>
        public void SetWeightTo_TF_IDF()
        {
            foreach (var t in terms.Values)
            {
                t.weight = GetTF_IDF((string)t.name);
            }
            InvokeChanged();
        }

        public bool RemoveTerm(string name)
        {
            IWeightTableTerm removed = null;
            int remScore = 0;
            terms.TryRemove(name, out removed);
            InvokeChanged();
            return termsAFreq.TryRemove(name, out remScore);
        }

        /// <summary>
        /// Removes the terms under specified weight.
        /// </summary>
        /// <param name="limit">The limit.</param>
        public void RemoveUnderWeight(double limit = 0.1)
        {
            foreach (var t in terms.Values)
            {
                if (t.weight < limit)
                {
                    RemoveTerm(t.name);
                }
                t.weight = GetTF_IDF((string)t.name);
            }
            InvokeChanged();
        }

        /// <summary>
        /// Removes the terms with zero weidht.
        /// </summary>
        public void RemoveZeroWeidht()
        {
            foreach (var t in terms.Values)
            {
                if (t.weight == 0)
                {
                    RemoveTerm(t.name);
                }
                t.weight = GetTF_IDF((string)t.name);
            }
            InvokeChanged();
        }

        /// <summary>
        /// The sum all weights
        /// </summary>
        private double _sumWeights = new double();

        /// <summary> </summary>
        public double sumWeight
        {
            get
            {
                return _sumWeights;
            }
            protected set
            {
                _sumWeights = value;
                OnPropertyChanged("sumWeights");
            }
        }

        /// <summary>
        /// Gets the cumulative weight (sum of all weights)
        /// </summary>
        /// <returns></returns>
        public double GetCWeight()
        {
            if (HasChanges) updateMaxValues();
            return sumWeight;
        }

        /// <summary>
        /// Returns list of all terms in nominal form or by name - term keys
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllTermString()
        {
            return terms.Keys.ToList();
        }

        public List<IWeightTableTerm> GetAllTerms()
        {
            return terms.Values.ToList();
        }

        public int Count()
        {
            return terms.Count;
        }

        public int Count(bool expandedCount)
        {
            if (expandedCount)
            {
                int c = 0;
                foreach (IWeightTableTerm t in terms.Values)
                {
                    c = c + t.Count;
                }
            }
            else
            {
            }
            return terms.Count;
        }
    }
}