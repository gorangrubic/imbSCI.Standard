// --------------------------------------------------------------------------------------------------------------------
// <copyright file="weightTableSet.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data.data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Weight term table set
    /// </summary>
    /// <typeparam name="TWeightTable">The type of the weight table.</typeparam>
    /// <seealso cref="imbSCI.Data.data.changeBindableBase" />
    /// <seealso cref="IWeightTableSet" />
    public class weightTableSet<TWeightTableTerm, TWeightTable> : changeBindableBase, IWeightTableSet
        where TWeightTableTerm : IWeightTableTerm
        where TWeightTable : IWeightTable, new()
    {
        /// <summary>
        /// Count all terms in all documents
        /// </summary>
        /// <param name="expanded">if set to <c>true</c> [expanded].</param>
        /// <returns></returns>
        public int CountAllDocuments(bool expanded = false)
        {
            return AggregateDocument.Count(expanded);
        }

        public void Add(object item)
        {
            if (item is IWeightTable)
            {
                IWeightTable item_IWeightTable = (IWeightTable)item;

                documents.Add(item_IWeightTable.name, item_IWeightTable);
            }
            else if (item is IWeightTableTerm)
            {
                IWeightTableTerm item_IWeightTableTerm = (IWeightTableTerm)item;

                //item_IWeightTableTerm.
            }
        }

        /// <summary>
        /// Gets the <see cref="IWeightTable"/> with the specified document name.
        /// </summary>
        /// <value>
        /// The <see cref="IWeightTable"/>.
        /// </value>
        /// <param name="documentName">Name of the document.</param>
        /// <returns></returns>
        public IWeightTable this[string documentName]
        {
            get
            {
                if (documents.ContainsKey(documentName)) return documents[documentName];
                return null;
            }
        }

        private string _name = "FrequencyTableSet";

        /// <summary> </summary>
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string description { get; set; } = "";

        private weightTableTermInSetCounter<TWeightTableTerm, TWeightTable> _counter = new weightTableTermInSetCounter<TWeightTableTerm, TWeightTable>();

        /// <summary> </summary>
        public weightTableTermInSetCounter<TWeightTableTerm, TWeightTable> counter
        {
            get
            {
                return _counter;
            }
            protected set
            {
                _counter = value;
                OnPropertyChanged("counter");
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, IWeightTable> documents { get; set; } = new Dictionary<string, IWeightTable>();

        private TWeightTable _aggregateDocument;

        public IWeightTable AggregateDocument
        {
            get
            {
                if (_aggregateDocument == null)
                {
                    _aggregateDocument = new TWeightTable(); // (this, "Summary");
                    _aggregateDocument.name = "Summary";

                    _aggregateDocument.parent = this;
                }

                return _aggregateDocument;
            }
        }

        /// <summary>
        /// Adds the specified document and processes all terms contained
        /// </summary>
        /// <param name="document">The document.</param>
        public IWeightTable Add(IWeightTable document)
        {
            string newName = document.name;
            newName = newName.makeUniqueName(documents.ContainsKey, "D4", 10000);

            TWeightTable newDoc = (TWeightTable)AddTable(newName);

            if (document is weightTableCompiled)
            {
                weightTableCompiled cTable = (weightTableCompiled)document;

                foreach (weightTableTermCompiled cTerm in cTable.GetList())
                {
                    newDoc.Add(cTerm);
                }
            }
            else
            {
                foreach (IWeightTableTerm iTerm in document)
                {
                    newDoc.Add(iTerm, iTerm.AFreqPoints);
                }
            }

            return newDoc;
        }

        /// <summary>
        /// Non semantic matching
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="term">The term.</param>
        /// <param name="callTableLevelAdd">Add supplied table, usully you don't want that</param>
        public void Add(IWeightTable table, IWeightTableTerm term, bool callTableLevelAdd = false) //, Int32 DFPoints = -1)
        {
            if (callTableLevelAdd) table.Add(term);

            if (table != AggregateDocument)
            {
                var t = AggregateDocument.Add(term);

                counter.AddVote(table, t);
            }
            else
            {
            }
        }

        ///// <summary>
        ///// Gets the cross section of matched terms
        ///// </summary>
        ///// <param name="secondTable">The second table.</param>
        ///// <param name="thisAgainstSecond">if set to <c>true</c> [this against second].</param>
        ///// <returns></returns>
        //public List<IWeightTableTerm> GetCrossSection(IWeightTable secondTable, Boolean thisAgainstSecond = false)
        //{
        //    List<IWeightTableTerm> matched = new List<IWeightTableTerm>();

        //    foreach (IWeightTable doc in this)
        //    {
        //        matched.AddRange(doc.GetCrossSection(secondTable, thisAgainstSecond));
        //    }

        //    return matched;
        //}

        public IWeightTable AddTable(string documentName = "", IEnumerable<IWeightTableTerm> terms = null)
        {
            TWeightTable output = new TWeightTable();
            if (documentName.isNullOrEmpty()) documentName = typeof(TWeightTable).Name.Replace(" ", "_");

            output.parent = this;
            output.name = documentName;
            int i = 1;
            while (documents.ContainsKey(output.name))
            {
                output.name = documentName + i.ToString("D5");
                i++;
            }

            documents.Add(output.name, output);

            if (terms != null)
            {
                foreach (IWeightTableTerm t in terms)
                {
                    output.Add(t, t.AFreqPoints);
                }
            }

            return output;
        }

        private DataSet _dataSet;

        /// <summary> </summary>
        public DataSet dataSet
        {
            get
            {
                if (_dataSet == null) _dataSet = new DataSet(name);
                return _dataSet;
            }
            protected set
            {
                _dataSet = value;
                OnPropertyChanged("dataSet");
            }
        }

        /// <summary>
        /// Gets the aggregate table.
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetAggregateDataTable()
        {
            var output = AggregateDocument.GetDataTable("TFIDF_" + name, dataSet);
            output.SetDescription("Aggregate TF-IDF table of terms contained in [" + documents.Count() + "] document vectors");
            return output;
        }

        public double GetBDFreq(string term)
        {
            var t = AggregateDocument.GetMatchTermByName(term);
            if (t == null) return 0;
            return Enumerable.Count<KeyValuePair<IWeightTable, int>>(counter[t]);//  termADF[t].Count();
        }

        public double GetBDFreq(IWeightTableTerm term)
        {
            var t = AggregateDocument.GetMatchTerm(term);
            if (t == null) return 0;
            return Enumerable.Count<KeyValuePair<IWeightTable, int>>(counter[t]);
        }

        public virtual DataSet GetDataSet(bool includeAggregateTable = false)
        {
            DataSet ds = dataSet;
            dataSet.SetTitle(name);
            dataSet.SetDesc(description);

            if (includeAggregateTable) GetAggregateDataTable();
            int c = 0;
            foreach (var tb in documents.Values)
            {
                c++;
                if (tb.name.isNullOrEmpty()) tb.name = c.ToString("D4");
                tb.GetDataTable(tb.name + c.ToString("D4"), ds);
            }

            return ds;
        }

        /// <summary>
        /// Gets the table for the specified document
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(string document)
        {
            return documents[document].GetDataTable(document, dataSet);
        }

        public IEnumerator<IWeightTable> GetEnumerator()
        {
            return documents.Values.GetEnumerator();
        }

        public double GetIDF(string term)
        {
            var t = AggregateDocument.GetMatchTermByName(term);
            if (t == null) return 0;

            return GetIDF(t);
        }

        public double GetIDF(IWeightTableTerm term)
        {
            double bd = GetBDFreq((string)term.name);
            if (bd == 0) return 0;
            double idf = Math.Log(documents.Count() / bd);
            return idf;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return documents.Values.GetEnumerator();
        }

        public void updateMaxValues()
        {
            foreach (var t in documents.Values) t.updateMaxValues();
        }

        /*
        public void SetWeightTo_NominalFrequency()
        {
            documents.Values.ForEach(x => x.SetWeightTo_NominalFrequency());
        }

        public void SetWeightTo_FrequencyRatio()
        {
            documents.Values.ForEach(x => x.SetWeightTo_NominalFrequency());
        }

        public void SetWeightTo_TF_IDF()
        {
            documents.Values.ForEach(x => x.SetWeightTo_TF_IDF());
        }*/
    }
}