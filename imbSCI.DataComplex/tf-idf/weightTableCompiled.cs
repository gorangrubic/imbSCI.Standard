// --------------------------------------------------------------------------------------------------------------------
// <copyright file="weightTableCompiled.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.reporting;
    using imbSCI.DataComplex.tables;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

#pragma warning disable CS1574 // XML comment has cref attribute 'IWeightTable' that could not be resolved
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'aceCommonTypes.data.tables.objectTable{aceCommonTypes.collection.tf_idf.weightTableTermCompiled}'
    /// <summary>
    /// Precompiled version of a <see cref="weightTable{TWeightTableTerm}"/>
    /// </summary>
    /// <seealso cref="aceCommonTypes.data.tables.objectTable{aceCommonTypes.collection.tf_idf.weightTableTermCompiled}" />
    /// <seealso cref="aceCommonTypes.collection.tf_idf.IWeightTable" />
    public class weightTableCompiled : objectTable<weightTableTermCompiled>, IWeightTable
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'aceCommonTypes.data.tables.objectTable{aceCommonTypes.collection.tf_idf.weightTableTermCompiled}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1574 // XML comment has cref attribute 'IWeightTable' that could not be resolved
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="weightTableCompiled"/> class.
        /// </summary>
        /// <param name="__tableName">Name of the table.</param>
        public weightTableCompiled(string __tableName) : base(nameof(weightTableTermCompiled.termName), __tableName)
        {
            LoadAllToCache();
            if (table.Rows.Count > 0) ReadOnlyMode = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="weightTableCompiled"/> class.
        /// </summary>
        /// <param name="__filePath">The file path.</param>
        /// <param name="autoLoad">if set to <c>true</c> [automatic load].</param>
        /// <param name="__tableName">Name of the table.</param>
        public weightTableCompiled(string __filePath, bool autoLoad, string __tableName = "") : base(__filePath, autoLoad, nameof(weightTableTermCompiled.termName), __tableName)
        {
            LoadAllToCache();
            if (table.Rows.Count > 0) ReadOnlyMode = true;
        }

        protected ConcurrentDictionary<string, weightTableTermCompiled> cache = new ConcurrentDictionary<string, weightTableTermCompiled>();

        private object AddToCacheLock = new object();

        protected void addToCache(weightTableTermCompiled cterm)
        {
            if (cterm == null) return;

            if (!cache.ContainsKey(cterm.nominalForm))
            {
                //lock (AddToCacheLock)
                //{
                List<string> allForms = cterm.GetAllForms();
                foreach (string f in allForms)
                {
                    if (!cache.ContainsKey(cterm.nominalForm)) cache.TryAdd(f, cterm);
                }
                //}
            }
        }

        protected bool allInCache { get; set; }

        protected void LoadAllToCache()
        {
            var trms = GetList();
            foreach (weightTableTermCompiled cterm in trms)
            {
                List<string> allForms = cterm.GetAllForms();
                foreach (string f in allForms)
                {
                    if (!cache.ContainsKey(cterm.nominalForm)) cache.TryAdd(f, cterm);
                }
            }
            allInCache = true;
        }

        /// <summary>
        /// Gets the instance if exists
        /// </summary>
        /// <param name="termName">Name of the term.</param>
        /// <param name="matchForInflections">if set to <c>true</c> [match for inflections].</param>
        /// <returns></returns>
        public weightTableTermCompiled GetIfExists(string termName, bool matchForInflections = false)
        {
            if (cache.ContainsKey(termName))
            {
                return cache[termName];
            }
            else
            {
                if (allInCache) return null;
            }

            weightTableTermCompiled cterm = null;

            if (!matchForInflections)
            {
                if (ContainsKey(termName))
                {
                    cterm = GetOrCreate(termName);
                    addToCache(cterm);
                    return cterm;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //weightTableTermCompiled found = null;

                if (ContainsKey(termName))
                {
                    cterm = GetOrCreate(termName);
                    addToCache(cterm);
                    return cterm;
                }

                cterm = GetFirstWhere(nameof(weightTableTermCompiled.termInflections) + " LIKE '%" + termName + "%'");
                addToCache(cterm);
                return cterm;
            }
        }

        /// <summary>
        /// Gets the maximum frequency in the collection
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public int max { get; protected set; } = 0;

        public double maxWeight { get; protected set; } = 0;

        public IWeightTableSet parent { get; set; }

        public int sum { get; protected set; } = 0;

        public double sumWeight { get; protected set; } = 0;

        private weightTableTermCompiled CompileTerm(string term, int afreq = -1, int __df = -1)
        {
            if (afreq == -1) afreq = 1;
            if (__df == -1) __df = 1;
            weightTableTermCompiled termCompiled = null;
            termCompiled = new weightTableTermCompiled();
            termCompiled.freqAbs = 1;
            termCompiled.df = __df;
            termCompiled.termName = term.Trim();

            return termCompiled;
        }

        private weightTableTermCompiled CompileTerm(IWeightTableTerm term)
        {
            weightTableTermCompiled termCompiled = null;
            if (term is weightTableTermCompiled)
            {
                termCompiled = term as weightTableTermCompiled;
            }
            else
            {
                termCompiled = new weightTableTermCompiled();
                termCompiled.freqAbs = term.AFreqPoints;
                termCompiled.weight = term.weight;
                termCompiled.termName = term.name;
                List<string> __forms = term.GetAllForms(false);
                termCompiled.termInflections = __forms.toCsvInLine();

                //termCompiled.termInflectionList.AddRange(__forms, true);
            }
            return termCompiled;
        }

        public List<IWeightTableTerm> Add(IEnumerable<IWeightTableTerm> terms)
        {
            List<IWeightTableTerm> converted = new List<IWeightTableTerm>();
            foreach (IWeightTableTerm term in terms)
            {
                var cterm = CompileTerm(term);

                if (AddOrUpdate(cterm))
                {
                    converted.Add(cterm);
                }
            }
            return converted;
        }

        public void Add(object term)
        {
            if (term == null) return;

            if (term is string)
            {
                AddOrUpdate(CompileTerm((string)term));
            }
            else if (term is IWeightTableTerm)
            {
                AddOrUpdate(CompileTerm(term as IWeightTableTerm));
            }
            else
            {
                throw new ArgumentException("Specified term is non compatibile object type");
            }
        }

        public IWeightTableTerm Add(string term, int AFreqPoints = -1)
        {
            IWeightTableTerm cterm = CompileTerm(term, AFreqPoints);
            if (AddOrUpdate(cterm))
            {
                return cterm;
            }
            else
            {
                return null;
            }
        }

        public IWeightTableTerm Add(IWeightTableTerm term, int AFreqPoints = -1)
        {
            weightTableTermCompiled cterm = CompileTerm(term);
            if (AFreqPoints > -1) cterm.AFreqPoints = AFreqPoints;

            if (AddOrUpdate(cterm))
            {
                return cterm;
            }
            else
            {
                return null;
            }
        }

        public bool containsByName(string termName)
        {
            return ContainsKey(termName);
        }

        public int GetAFreq(string term)
        {
            weightTableTermCompiled mterm = GetIfExists(term, true) as weightTableTermCompiled;
            if (mterm == null) return 0;

            //var cterm = GetOrCreate(term);
            return mterm.freqAbs;
        }

        public const int TERMNOTFOUND_FREQ = -1;
        public const double TERMNOTFOUND_WEIGHT = -1;

        public int GetAFreq(IWeightTableTerm term)
        {
            if (term is weightTableTermCompiled)
            {
                return ((weightTableTermCompiled)term).freqAbs;
            }
            var cterm = GetIfExists(term.nominalForm);
            if (cterm == null) return TERMNOTFOUND_FREQ;
            return cterm.freqAbs;
        }

        public List<IWeightTableTerm> GetAllTerms()
        {
            List<IWeightTableTerm> output = new List<IWeightTableTerm>();
            output.AddRange(GetList());
            return output;
        }

        public List<string> GetAllTermString()
        {
            return Enumerable.ToList<string>(keyCache);
        }

        public double GetBDFreq(string term)
        {
            var cterm = GetIfExists(term, true);
            if (cterm == null) return TERMNOTFOUND_WEIGHT;
            return cterm.df;
        }

        public double GetBDFreq(IWeightTableTerm term)
        {
            var cterm = GetIfExists(term.nominalForm);
            if (cterm == null) return TERMNOTFOUND_WEIGHT;
            return cterm.df;
        }

        public double GetCWeight()
        {
            var rows = table.Select("Sum(" + nameof(weightTableTermCompiled.cw) + ")");
            if (!Enumerable.Any<DataRow>(rows)) return 0;
            DataRow dr = Enumerable.First<DataRow>(rows);

            var vl = dr[nameof(weightTableTermCompiled.cw)];

            return vl.imbConvertValueSafeTyped<Double>();
        }

        public DataTable GetDataTable(string documentName = "", DataSet ds = null, bool addExtra = true)
        {
            return GetDataTable(ds);
        }

        public DataTable GetDataTableClean(string tableName = "", DataSet ds = null, bool onlyTermAndFreq = false)
        {
            return GetDataTable(ds);
        }

        public double GetIDF(string term)
        {
            var cterm = GetIfExists(term, true);
            if (cterm == null) return TERMNOTFOUND_WEIGHT;
            return cterm.idf;
        }

        public double GetIDF(IWeightTableTerm term)
        {
            var cterm = GetIfExists(term.nominalForm);
            if (cterm == null) return TERMNOTFOUND_WEIGHT;
            return cterm.idf;
        }

        public IWeightTableTerm GetMatchByString(string term)
        {
            return GetIfExists(term, true);
        }

        /// <summary>
        /// Determines what kind of match this term might be to this table
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public weightTableMatchResultEnum isMatchBySemantics(IWeightTableTerm term)
        {
            weightTableMatchResultEnum output = weightTableMatchResultEnum.none;

            if (ContainsKey(term.nominalForm)) return weightTableMatchResultEnum.hostTermName_and_needleTermName;

            var cterm = GetIfExists(term.nominalForm, true);
            if (cterm != null) return weightTableMatchResultEnum.hostTermInstance_and_needleTermName;

            var allForms = term.GetAllForms();
            foreach (string form in allForms)
            {
                if (ContainsKey(form)) return weightTableMatchResultEnum.hostTermName_and_needleTermInstance;
            }

            foreach (string form in allForms)
            {
                cterm = GetIfExists(form, true);
                if (cterm != null) return weightTableMatchResultEnum.hostTermInstance_and_needleTermName;
            }

            return output;
        }

        public IWeightTableTerm GetMatchTerm(IWeightTableTerm term, bool termOnNotFound = false)
        {
            return GetIfExists(term.name, true);
        }

        public IWeightTableTerm GetMatchTermByName(string term)
        {
            return GetIfExists(term);
        }

        public IWeightTableTerm GetMatchTermByName(IWeightTableTerm term, bool termOnNotFound = false)
        {
            return GetIfExists(term.name);
        }

        public double GetNFreq(string term)
        {
            var cterm = GetIfExists(term);
            if (cterm == null) return TERMNOTFOUND_WEIGHT;
            return cterm.freqNorm;
        }

        public double GetNFreq(IWeightTableTerm term)
        {
            var cterm = GetIfExists(term.nominalForm);
            if (cterm == null) return TERMNOTFOUND_WEIGHT;
            return cterm.freqNorm;
        }

        public double GetNWeight(string term)
        {
            var cterm = GetIfExists(term);
            if (cterm == null) return TERMNOTFOUND_WEIGHT;
            return cterm.weight;
        }

        public double GetNWeight(IWeightTableTerm term)
        {
            var cterm = GetIfExists(term.nominalForm);
            if (cterm == null) return TERMNOTFOUND_WEIGHT;
            return cterm.weight;
        }

        public double GetTF_IDF(string term)
        {
            var cterm = GetIfExists(term, true);
            if (cterm == null) return TERMNOTFOUND_WEIGHT;
            return cterm.tf_idf;
        }

        public double GetTF_IDF(IWeightTableTerm term)
        {
            var cterm = GetIfExists(term.nominalForm);
            if (cterm == null) return TERMNOTFOUND_WEIGHT;
            return cterm.tf_idf;
        }

        public double GetWeight(string term)
        {
            var cterm = GetIfExists(term);
            if (cterm == null) return TERMNOTFOUND_WEIGHT;
            return cterm.weight;
        }

        public double GetWeight(IWeightTableTerm term)
        {
            var cterm = GetIfExists(term.nominalForm);
            if (cterm == null) return TERMNOTFOUND_WEIGHT;
            return cterm.weight;
        }

        public bool isMatch(IWeightTableTerm term)
        {
            weightTableMatchResultEnum mres = isMatchBySemantics(term);
            return mres.HasFlag(weightTableMatchResultEnum.isMatch);
        }

        public bool isMatchByName(IWeightTableTerm term)
        {
            return (Enumerable.Contains<string>(keyCache, term.name));
        }

        /// <summary>
        /// Updates the maximum AFreq and CWeight - if chagnes occured since last call.
        /// </summary>
        public void updateMaxValues()
        {
            sum = 0;
            max = int.MinValue;
            maxWeight = 0;
            sumWeight = 0;

            foreach (var term in GetList())
            {
                sum = sum + term.freqAbs;
                max = Math.Max(max, (int)term.freqAbs);
                maxWeight = Math.Max(maxWeight, term.weight);
                sumWeight = sumWeight + term.weight;
            }

            Accept();
        }

        int IWeightTable.Count(bool expandedCount)
        {
            if (expandedCount)
            {
                int c = 0;
                foreach (DataRow dr in table.Rows)
                {
                    string inflections = dr[nameof(weightTableTermCompiled.termInflections)].toStringSafe();
                    c += inflections.Count(x => x == ',') + 2;
                }

                return c;
            }
            else
            {
                return Count;
            }
        }

        IEnumerator<IWeightTableTerm> IEnumerable<IWeightTableTerm>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public weightTableCompiled GetCompiledTable(ILogBuilder loger = null)
        {
            if (loger != null) loger.log("--- this table was already compiled --- [" + name + "] from [" + info.FullName + "]");
            return this;
        }

        public List<IWeightTableTerm> GetCrossSection(IWeightTable secondTable, bool thisAgainstSecond = false)
        {
            throw new NotImplementedException();
        }

        public int AddExternalDataTable(DataTable table, string termName_column = "*", string termAFreq_column = "*")
        {
            throw new NotImplementedException();
        }

        public void AddExternalDocument(IWeightTable source, bool CopyFrequencies)
        {
            throw new NotImplementedException();
        }

        public void SetWeightTo_NominalFrequency()
        {
            throw new NotImplementedException();
        }

        public void SetWeightTo_FrequencyRatio()
        {
            throw new NotImplementedException();
        }

        public void SetWeightTo_TF_IDF()
        {
            throw new NotImplementedException();
        }

        public bool RemoveTerm(string name)
        {
            throw new NotImplementedException();
        }

        public void RemoveUnderWeight(double limit = 0.1)
        {
            throw new NotImplementedException();
        }

        public void RemoveZeroWeidht()
        {
            throw new NotImplementedException();
        }
    }
}