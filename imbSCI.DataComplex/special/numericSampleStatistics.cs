// --------------------------------------------------------------------------------------------------------------------
// <copyright file="numericSampleStatistics.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.special
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.math;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Data;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.exceptions;
    using imbSCI.DataComplex.extensions.data;
    using imbSCI.DataComplex.extensions.data.operations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'aceCommonTypes.collection.special.instanceCountCollection{System.Int32}'
    /// <summary>
    /// Instance frequency and value stats
    /// </summary>
    /// <seealso cref="aceCommonTypes.collection.special.instanceCountCollection{System.Int32}" />
    public class numericSampleStatistics : instanceCountCollection<int>, IObjectWithNameAndDescription
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'aceCommonTypes.collection.special.instanceCountCollection{System.Int32}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    {
        /*
        public DataTable GetStatsTable(dataTableDeliveryEnum format, dataTableSummaryRowEnum whatToSummarize = dataTableSummaryRowEnum.none)
        {
            if (format.HasFlag(dataTableDeliveryEnum.horizontal | dataTableDeliveryEnum.collection))
            {
                return getDataTableHorizontal();
            }

            if (format.HasFlag(dataTableDeliveryEnum.comparative))
            {
                return getDataTableComparative();
            }

            if (format.HasFlag(dataTableDeliveryEnum.singleRow | dataTableDeliveryEnum.vertical))
            {
                return Current().getDataTableVertical();
            }

            if (format.HasFlag(dataTableDeliveryEnum.verticalCalculatedSummary))
            {
                numericSampleStatistics summary = GetSummary(dataTableSummaryRowEnum.entropy)
                return Current().getDataTableVertical();
            }
        }
        */

        /// <summary>TRUE if  the collection is locked to preserve input-type consistency</summary>
        public bool isLocked { get; protected set; } = false;

        private bool _kLevelApplied = false;

        /// <summary>TRUE if the <c>kLevel</c> was applied to (an) input value. </summary>
        public bool kLevelApplied
        {
            get
            {
                return _kLevelApplied;
            }
            set
            {
                if (!value) if (Count > 0) throw new dataException("Can't deactivate the k-Level once this instance received it's first measurement/entry. Call Clear() to reset the instance, it will deactivate k-Level too.", null, this, "Already have [" + Count + "] measurements");
                _kLevelApplied = value;
                OnPropertyChanged("kLevelApplied");
            }
        }

        private int _kLevel = 100; // = new Int32();

        /// <summary>
        /// Max K-level / factor applied to convert Double/Float value inputs to Int32, before accepted into this collection.
        /// </summary>
        [Category("numericSampleStatistics")]
        [DisplayName("kLevel")]
        [Description("Max K-level / factor applied to convert Double/Float value inputs to Int32, before accepted into this collection. ")]
        public int kLevel
        {
            get
            {
                return _kLevel;
            }
            set
            {
                if (Count > 0) throw new dataException("Can't change k-Level once this instance received it's first measurement/entry. Call Clear() to reset the instance.", null, this, "Already have [" + Count + "] measurements");
                _kLevel = value;
                OnPropertyChanged("kLevel");
            }
        }

        /// <summary>
        /// Name for this instance collection, used for Table feneration
        /// </summary>
        public string name { get; set; } = "Statistics";

        private string _description = "Agregates and descriptive statistics over _n_ entities in sample.";

        /// <summary>
        /// Human-readable description of object instance
        /// </summary>
        public string description
        {
            get
            {
                if (kLevelApplied)
                {
                    return imbSciStringExtensions.add(_description, "**k-Level** factor " + kLevel + " applied to _float_ inputs, effective resolution: " + (1 / kLevel).ToString("0.0" + "#".Repeat(kLevel.ToString().Length)) + ".", " ");
                }

                return _description;
            }
            set { _description = value; }
        }

        /// <summary>
        /// Gets the data table vertical.
        /// </summary>
        /// <returns></returns>
        public DataTable getDataTableVertical()
        {
            DataTable dt = null; // this.buildPropertyCollection (PropertyEntryColumn.entry_description | PropertyEntryColumn.entry_name | PropertyEntryColumn.role_letter | PropertyEntryColumn.role_symbol, nameof(name), nameof(description), nameof(index), nameof(avgValue), nameof(minValue), nameof(maxValue), nameof(sumOfValues), nameof(distinctValues), nameof(avgFreq), nameof(minFreq), nameof(maxFreq), nameof(entropyFreq), nameof(varianceFreq), nameof(medianFreq));
            return dt.GetInversedDataTable("name");
        }

        /// <summary>
        /// Gets the data table comparative.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public DataTable getDataTableComparative(numericSampleStatisticsList list)
        {
            DataTable dt = list.BuildDataTableHor(PropertyEntryColumn.entry_name | PropertyEntryColumn.role_letter | PropertyEntryColumn.role_symbol, nameof(name), nameof(index), nameof(avgValue), nameof(minValue), nameof(maxValue), nameof(sumOfValues), nameof(distinctValues), nameof(avgFreq), nameof(minFreq), nameof(maxFreq), nameof(entropyFreq), nameof(varianceFreq), nameof(medianFreq));
            return dt.GetInversedDataTable("name");
        }

        /// <summary>
        /// Gets the data table horizontal.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public DataTable getDataTableHorizontal(numericSampleStatisticsList list)
        {
            return list.BuildDataTableHor(PropertyEntryColumn.entry_description | PropertyEntryColumn.role_letter, nameof(name), nameof(index), nameof(avgValue), nameof(minValue), nameof(maxValue), nameof(distinctValues), nameof(avgFreq), nameof(minFreq), nameof(maxFreq), nameof(entropyFreq), nameof(varianceFreq), nameof(medianFreq));
        }

        public numericSampleStatistics(int __index = 0)
        {
            index = __index;
        }

        private int _index = 1;

        /// <summary> </summary>
        [DisplayName("Index")]
        [Description("Position in collection")]
        public int index
        {
            get
            {
                return _index;
            }
            protected set
            {
                _index = value;
                OnPropertyChanged("index");
            }
        }

        public void lockTheCollection()
        {
            isLocked = true;
        }

        public override void Clear()
        {
            base.Clear();
            _minValue = int.MaxValue;
            minFreq = int.MaxValue;
            _maxValue = int.MinValue;
            maxFreq = int.MinValue;
            avgValue = 0;
            avgFreq = 0;
            sumOfValues = 0;
            kLevelApplied = false;
            isLocked = false;

            Accept();
        }

        /// <summary>
        /// Adds the specified <c>Double</c> value by applying <see cref="kLevel"/> factor
        /// </summary>
        /// <param name="valueInSample">The value in sample.</param>
        /// <param name="originHash">The origin hash.</param>
        public void Add(double valueInSample, string originHash = "")
        {
            kLevelApplied = true;
            valueInSample = valueInSample * kLevel;
            Add(Convert.ToInt32(valueInSample), originHash);
            OnPropertyChanged("Count");
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'dataException' that could not be resolved
        /// <summary>
        /// Adds the specified value in sample.
        /// </summary>
        /// <param name="valueInSample">The value in sample.</param>
        /// <param name="originHash">The origin hash.</param>
        /// <exception cref="aceCommonTypes.core.exceptions.dataException">The origin already have value into this sample statistics - null - Bad application - origin exists</exception>
        public void Add(int valueInSample, string originHash = "")
#pragma warning restore CS1574 // XML comment has cref attribute 'dataException' that could not be resolved
        {
            sumOfValues += valueInSample;
            _maxValue = Math.Max(_maxValue, valueInSample);
            _minValue = Math.Min(_minValue, valueInSample);

            if (!originHash.isNullOrEmptyString())
            {
                if (originHashVsValue.ContainsKey(originHash))
                {
                    throw new dataException("The origin already have value into this sample statistics", null, this, "Bad application - origin exists");
                }
                else
                {
                    originHashVsValue.Add(originHash, valueInSample);
                }
            }
            AddInstance(valueInSample, "Add() @ numericSampleStatistics");
            OnPropertyChanged("Count");
        }

        private bool blockReCall = false;

        /// <summary>
        /// Recalculates frequency statistics
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        public override void reCalculate(preCalculateTasks tasks = preCalculateTasks.all)
        {
            if (blockReCall) return;
            blockReCall = true;
            Accept();
            _sumOfValues = 0;
            _maxValue = int.MinValue;
            _minValue = int.MaxValue;
            foreach (var pair in items)
            {
                _sumOfValues = _sumOfValues + pair.Key;
                _maxValue = Math.Max(_maxValue, pair.Key);
                _minValue = Math.Min(_minValue, pair.Key);
            }

            base.reCalculate(tasks);
            _avgValue = (((double)_sumOfValues) / ((double)Count));
            var valArray = items.Keys.getDoubleValues().ToArray();
            //  entropyFreq = Accord.Statistics.Measures.Entropy(valArray);
            varianceFreq = valArray.GetVariance(); // Accord.Statistics.Measures.Variance(valArray, false);
            standardDeviation = valArray.GetStdDeviation(); //Accord.Statistics.Measures.StandardDeviation(valArray, _avgValue, true);

            Accept();
            blockReCall = false;
        }

        /// <summary>
        /// Gets sampled value <see cref="Int32"/> for the origin object identified via hash ID
        /// </summary>
        /// <value>
        /// The <see cref="Int32"/>.
        /// </value>
        /// <param name="originHash">The origin hash.</param>
        /// <returns></returns>
        public int this[string originHash]
        {
            get
            {
                return originHashVsValue[originHash];
            }
        }

        private int _sumOfValues = 0; //= default(Int32); // = new Int32();

        /// <summary>
        /// Direct sum of values
        /// </summary>
        [Category("numericSampleStatistics")]
        [DisplayName("sumOfValues")]
        [Description("Direct sum of values")]
        public int sumOfValues
        {
            get
            {
                if (HasChanges)
                {
                    reCalculate(preCalculateTasks.all);
                }
                return _sumOfValues;
            }
            set
            {
                _sumOfValues = value;
                OnPropertyChanged("sumOfValues");
            }
        }

        private Dictionary<string, int> _originHashVsValue = new Dictionary<string, int>();

        /// <summary>UID as key, sampled value as Value</summary>
        public Dictionary<string, int> originHashVsValue
        {
            get
            {
                return _originHashVsValue;
            }
            protected set
            {
                _originHashVsValue = value;
                OnPropertyChanged("originHashVsValue");
            }
        }

        private int _minValue = int.MaxValue; // = new Int32();

        /// <summary>
        /// The lowest value in the sample
        /// </summary>
        [Category("numericSampleStatistics")]
        [DisplayName("minValue")]
        [Description("The lowest value in the sample")]
        public int minValue
        {
            get
            {
                if (HasChanges)
                {
                    reCalculate(preCalculateTasks.all);
                }
                return _minValue;
            }
            set
            {
                _minValue = value;
                OnPropertyChanged("minValue");
            }
        }

        private int _maxValue = int.MinValue; // = new Int32();

        /// <summary>
        /// The highest value in the sample
        /// </summary>
        [Category("numericSampleStatistics")]
        [DisplayName("maxValue")]
        [Description("The highest value in the sample")]
        public int maxValue
        {
            get
            {
                if (HasChanges)
                {
                    reCalculate(preCalculateTasks.all);
                }
                return _maxValue;
            }
            set
            {
                _maxValue = value;
                OnPropertyChanged("maxValue");
            }
        }

        private double _avgValue = default(double); // = new Double();

        /// <summary>
        /// Arithmetic mean of sampled values
        /// </summary>
        [Category("numericSampleStatistics")]
        [DisplayName("avgValue")]
        [Description("Arithmetic mean of sampled values")]
        public double avgValue
        {
            get
            {
                if (HasChanges)
                {
                    reCalculate(preCalculateTasks.all);
                }
                return _avgValue;
            }
            set
            {
                _avgValue = value;
                OnPropertyChanged("avgValue");
            }
        }

        /// <summary>
        /// Count of distinct values in the sample
        /// </summary>
        [Category("numericSampleStatistics")]
        [DisplayName("distinctValues")]
        [Description("Count of distinct values in the sample")]
        public int distinctValues
        {
            get
            {
                return Count;
            }
        }
    }
}