// --------------------------------------------------------------------------------------------------------------------
// <copyright file="weightTableTermCompiled.cs" company="imbVeles" >
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
    using imbSCI.Core.attributes;
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// Compiled version of TF-IDF -- having no dynamic purpose. It serves in scenarios using precompiled TF-IDF data
    /// </summary>
    /// <seealso cref="IWeightTableTerm" />
    public class weightTableTermCompiled : IWeightTableTerm
    {
        public weightTableTermCompiled()
        {
        }

        /// <summary>  </summary>
        [Category("Term")]
        [DisplayName("Nominal form")]
        [imb(imbAttributeName.measure_letter, "Tn")]
        [imb(imbAttributeName.measure_setUnit, "-")]
        [Description("Nominal form of the term")] // [imb(imbAttributeName.reporting_escapeoff)]
        [imb(imbAttributeName.collectionPrimaryKey)]
        public string termName { get; set; } = "";

        [Category("Term")]
        [DisplayName("Inflected forms")]
        [imb(imbAttributeName.measure_letter, "Ti")]
        [imb(imbAttributeName.measure_setUnit, "-")]
        [Description("Inflected words or otherwise related terms in the same semantic cloud, as CSV")] // [imb(imbAttributeName.reporting_escapeoff)]
        public string termInflections { get; set; } = "";

        #region termInflectionList layer

        private object termInflectionListConstructionLock = new object();

        /// <summary>
        /// Builds the term inflection list.
        /// </summary>
        private void buildTermInflectionList()
        {
            if (!termInflections.isNullOrEmpty())
            {
                lock (termInflectionListConstructionLock)
                {
                    _termInflectionList = new ObservableCollection<string>();
                    _termInflectionList.AddRange(termInflections.SplitSmart(",", "", true));
                    _termInflectionList.CollectionChanged += _termInflectionList_CollectionChanged;
                }
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the _termInflectionList collection
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void _termInflectionList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            termInflections = termInflectionList.toCsvInLine();
        }

        private ObservableCollection<string> _termInflectionList;

        [XmlIgnore]
        /// <summary>Collection enabling access to the <see cref="termInflections"/>. The CSV version will be automatically updated once you commit any change</summary>
        public ObservableCollection<string> termInflectionList
        {
            get
            {
                if (_termInflectionList == null)
                {
                    buildTermInflectionList();
                }
                return _termInflectionList;
            }
            protected set
            {
                int __hash = _termInflectionList.GetHashCode();
                _termInflectionList = value;
                if (__hash != _termInflectionList.GetHashCode())
                {
                    _termInflectionList_CollectionChanged(null, null);
                }
            }
        }

        #endregion termInflectionList layer

        public void SetOtherForms(IEnumerable<string> instances)
        {
            foreach (String i in instances) termInflectionList.AddUnique(i);
            //termInflectionList.AddRangeUnique(instances);
        }

        /// <summary> Absolute frequency of the term </summary>
        [Category("Input")]
        [DisplayName("Absolute TF")]
        [imb(imbAttributeName.measure_letter, "TF_a")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [Description("Absolute frequency of the term")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public int freqAbs { get; set; } = default(int);

        /// <summary> Normalized frequency of the term - ratio with the maximum frequency </summary>
        [Category("Computed")]
        [DisplayName("Normalized TF")]
        [imb(imbAttributeName.measure_letter, "TF_n")]
        [imb(imbAttributeName.measure_setUnit, "%")]
        [Description("Normalized frequency of the term - ratio with the maximum frequency")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public double freqNorm { get; set; } = default(double);

        /// <summary> Document frequency - number of documents containing the term </summary>
        [Category("Input")]
        [DisplayName("Document frequency")]
        [imb(imbAttributeName.measure_letter, "DF")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [Description("Document frequency - number of documents containing the term")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public double df { get; set; } = 0;

        /// <summary> Inverse document frequency - logaritmicly normalized T_df </summary>
        [Category("Computed")]
        [DisplayName("Inverse Document Frequency")]
        [imb(imbAttributeName.measure_letter, "IDF")]
        [imb(imbAttributeName.measure_setUnit, "r")]
        [Description("Inverse document frequency - logaritmicly normalized T_df")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public double idf { get; set; } = default(double);

        /// <summary> Ratio </summary>
        [Category("Computed")]
        [DisplayName("TF-IDF")]
        [imb(imbAttributeName.measure_letter, "TF-IDF")]
        [imb(imbAttributeName.measure_setUnit, "r")]
        [Description("Term frequency Inverse document frequency - calculated as TF-IDF")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public double tf_idf { get; set; } = default(double);

        /// <summary> Ratio </summary>
        [Category("Special")]
        [DisplayName("Cumulative weight")]
        [imb(imbAttributeName.measure_letter, "CW")]
        [imb(imbAttributeName.measure_setUnit, "%")]
        [Description("Cumulative weight of semantically expanded term - i.e. semantic cloud weight sum")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public double cw { get; set; } = default(double);

        /// <summary> Ratio </summary>
        [Category("Special")]
        [DisplayName("Normalized cumulative weight")]
        [imb(imbAttributeName.measure_letter, "nCW")]
        [imb(imbAttributeName.measure_setUnit, "%")]
        [Description("Normalized against the max., cumulative weight of semantically expanded term - i.e. semantic cloud weight sum")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public double ncw { get; set; } = default(double);

        #region ----- IWeightTableTerm compatibility layer

        public void Define(string name, string nominalForm)
        {
            termName = name;
        }

        public List<string> GetAllForms(bool includingNominalForm = true)
        {
            var output = new List<string>();
            if (includingNominalForm) output.Add(nominalForm);
            output.AddRange(termInflectionList);
            return output;
        }

        /// <summary>
        /// Determines whether the specified <c>other</c> <see cref="IWeightTableTerm" /> is match with this one (meaning their frequencies are summed)
        /// </summary>
        /// <param name="other">The other term to compare with</param>
        /// <returns>
        ///   <c>true</c> if the specified other is match; otherwise, <c>false</c>.
        /// </returns>
        public bool isMatch(IWeightTableTerm other)
        {
            var allMyForms = GetAllForms();
            var allHisForms = other.GetAllForms();

            return allMyForms.ContainsAny(allHisForms);
        }

        [XmlIgnore]
        public string nominalForm
        {
            get
            {
                return termName;
            }
        }

        [XmlIgnore]
        public int AFreqPoints
        {
            get
            {
                return freqAbs;
            }

            set
            {
                freqAbs = value;
            }
        }

        [XmlIgnore]
        public double weight
        {
            get
            {
                return tf_idf;
            }
            set
            {
                tf_idf = value;
            }
        }

        /// <summary>
        /// Gets the count of inflected list + 1
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        [XmlIgnore]
        public int Count
        {
            get
            {
                return 1 + termInflectionList.Count();
            }
        }

        [XmlIgnore]
        public string name
        {
            get
            {
                return termName;
            }

            set
            {
                termName = value;
            }
        }

        #endregion ----- IWeightTableTerm compatibility layer
    }
}