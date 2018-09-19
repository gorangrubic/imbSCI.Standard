// --------------------------------------------------------------------------------------------------------------------
// <copyright file="instanceCountCollection.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.math;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using imbSCI.Data.enums.fields;
    using imbSCI.DataComplex.extensions.data;
    using imbSCI.DataComplex.extensions.data.operations;
    using imbSCI.DataComplex.extensions.data.schema;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Collection that counts how many same instances were "added"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
    /// <seealso cref="System.Collections.IDictionary" />
    public class instanceCountCollection<T> : changeBindableBase, IEnumerable<T>, IComparable, ICollection
    {
        public string ToStringFormatted(string format = "{0}[{1}]", string separator = " ")
        {
            string output = "";
            foreach (T item in this)
            {
                output = imbSciStringExtensions.add(output, string.Format(format, item.toStringSafe(), this[item]), separator);
            }
            return output;
        }

        public bool isKeyZero(T key)
        {
            return (this[key] == 0);
        }

        ///// <summary>
        ///// Builds the data table with statistics
        ///// </summary>
        ///// <param name="tableName">Name of the table.</param>
        ///// <returns></returns>
        //public DataTable buildDataTableWithEntries(String tableName)
        //{
        //    instanceCountPipeLine<T> ot = this.getFirstSafe() as instanceCountPipeLine<T>;

        //    DataTable output = ot.self.buildDataTable(tableName,
        //        buildDataTableOptions.doInsertAutocountColumn | buildDataTableOptions.doOnlyWithDisplayName | buildDataTableOptions.doInsertItemTitleColumn, globalMeasureUnitDictionary.stats);

        //    foreach (T oti in this)
        //    {
        //        ot = this[oti];
        //        ot.self.

        //        output.AddDataTableRow(ot, buildDataTableOptions.doCreate);
        //    }
        //    return output;
        //}

        public DataTable getDataTableVertical()
        {
            reCalculate(preCalculateTasks.all);
            DataTable dt = this.BuildDataShema(new string[] { nameof(avgFreq), nameof(minFreq), nameof(maxFreq), nameof(totalScore), nameof(entropyFreq), nameof(varianceFreq), nameof(diversityRatio), nameof(medianFreq) }, PropertyEntryColumn.autocount_idcolumn | PropertyEntryColumn.entry_description | PropertyEntryColumn.role_letter | PropertyEntryColumn.role_symbol);

            dt.AddObject(this);

            //this.BuildDataTableHor(PropertyEntryColumn.entry_description | PropertyEntryColumn.entry_name | PropertyEntryColumn.role_letter | PropertyEntryColumn.role_symbol, nameof(name), nameof(description), nameof(index), nameof(avgValue), nameof(minValue), nameof(maxValue), nameof(sumOfValues), nameof(distinctValues), nameof(avgFreq), nameof(minFreq), nameof(maxFreq), nameof(entropyFreq), nameof(varianceFreq), nameof(medianFreq));
            return dt.GetInversedDataTable("avgFreq");
        }

        public DataTable getDataTableHorizontal(numericSampleStatisticsList list)
        {
            reCalculate(preCalculateTasks.all);
            return list.BuildDataTableHor(PropertyEntryColumn.entry_description | PropertyEntryColumn.role_letter, new string[] { nameof(totalScore), nameof(Count), nameof(avgFreq), nameof(minFreq), nameof(maxFreq), nameof(entropyFreq), nameof(varianceFreq), nameof(medianFreq) });
        }

        /// <summary>
        /// Builds the data table summary row.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public DataTable buildDataTableSummaryRow(string tableName)
        {
            reCalculate(preCalculateTasks.all);
            return this.buildDataTable(tableName, true, true, false, globalMeasureUnitDictionary.stats, new string[] { nameof(Count), nameof(TotalScore), nameof(minFreq), nameof(maxFreq), nameof(avgFreq), nameof(entropyFreq), nameof(diversityRatio) });
        }

        /// <summary>
        /// Builds the vertical table with all data
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public DataTable buildDataTableSummaryTable(string tableName)
        {
            reCalculate(preCalculateTasks.all);
            PropertyCollectionExtended pce = this.buildPCE(false, null);

            return pce.buildDataTableVertical("Counter summary results", "Descriptive statistics");
        }

        //  _tableName
        /// <summary>
        /// Builds the data table with statistics
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public DataTable buildSampleFequencyTableSorted(object _tname, int rowCountToExplore, string instanceTitle = "Token")
        {
            string tableName = "";
            if (_tname is Enum)
            {
                tableName = globalMeasureUnitDictionary.GetTableName(_tname as Enum);
            }

            List<T> rank = getSorted();

            DataTable output = new DataTable(tableName);
            DataColumn dc = output.Columns.Add("#", typeof(int));
            dc.AutoIncrement = true;
            dc.AutoIncrementSeed = 1;

            dc = output.Columns.Add(instanceTitle, typeof(string));

            var cou = output.Columns.Add("Count", typeof(int)).set(templateFieldDataTable.col_caption, "Count").set(templateFieldDataTable.col_desc, "Number of distinct instances discovered in the sample");
            var tsc = output.Columns.Add("Score", typeof(string)).set(templateFieldDataTable.col_desc, "Number of instances observed");

            rowCountToExplore = Math.Min(rowCountToExplore, rank.Count());

            for (int i = 0; i < rowCountToExplore; i++)
            {
                T oti = rank[i];
                DataRow dr = output.NewRow();
                dr[dc] = oti.toStringSafe();

                dr[(DataColumn)cou] = Count;
                dr[(DataColumn)tsc] = TotalScore;

                output.Rows.Add(dr);
            }

            return output;
        }

        [Flags]
        public enum preCalculateTasks
        {
            none = 0,
            minMaxRangeTotal = 1,
            avgDiversity = 2,
            entropyCoVarDev = 4,
            all = minMaxRangeTotal | avgDiversity | entropyCoVarDev
        }

        private bool blockReCall = false;

        private void doMinMaxSum()
        {
            int output = 0;
            minFreq = int.MaxValue;
            maxFreq = int.MinValue;

            foreach (KeyValuePair<T, int> pair in items)
            {
                minFreq = Math.Min(minFreq, pair.Value);
                maxFreq = Math.Max(maxFreq, pair.Value);
                output += pair.Value;
            }

            if (minFreq == int.MaxValue) minFreq = 0;
            if (maxFreq == int.MinValue) maxFreq = 0;
            totalScore = output;
            range = maxFreq - minFreq;
        }

        /// <summary>
        /// Recalculates frequency statistics
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        public virtual void reCalculate(preCalculateTasks tasks = preCalculateTasks.all)
        {
            if (blockReCall) return;
            blockReCall = true;

            double count = Convert.ToDouble(Count);

            doMinMaxSum();

            Accept();

            //List<T> rank = getSorted();

            double _sumScore = Convert.ToDouble(totalScore);

            var valArray = items.Values.getDoubleValues().ToArray();

            //if (tasks.HasFlag(preCalculateTasks.avgDiversity))
            //{
            avgFreq = _sumScore / count;
            _diversityAntiValue = _sumScore / (avgFreq * avgFreq);
            _diversityAntiRatio = (avgFreq * avgFreq) / _sumScore;
            _diversityRatio = 1 - _diversityAntiRatio;
            //}

            //if (tasks.HasFlag(preCalculateTasks.entropyCoVarDev))
            //{
            //    //Measures.Entropy()

            var RFreqArray = GetRFreqArray();
            entropyFreq = RFreqArray.GetEntropy();

            varianceFreq = RFreqArray.GetVariance(false);
            //Accord.Statistics.Measures.Variance(valArray, true);
            standardDeviation = Math.Sqrt(varianceFreq); //RFreqArray.GetStdDeviation(false); // Accord.Statistics.Measures.StandardDeviation(valArray, avgFreq, true);
            //}

            Accept();
            blockReCall = false;
        }

        private double _diversityRatio = default(double); // = new Double();

        /// <summary>
        /// Description of $property$
        /// </summary>
        [Category("instanceCountCollection")]
        [DisplayName("diversityRatio")]
        [Description("Description of $property$")]
        public double diversityRatio
        {
            get
            {
                return _diversityRatio;
            }
            protected set
            {
                _diversityRatio = value;
                OnPropertyChanged("diversityRatio");
            }
        }

        private double _diversityAntiRatio = default(double); // = new Double();

        /// <summary>
        /// Description of $property$
        /// </summary>
        [Category("instanceCountCollection")]
        [DisplayName("diversityAntiRatio")]
        [Description("Description of $property$")]
        public double diversityAntiRatio
        {
            get
            {
                return _diversityAntiRatio;
            }
            protected set
            {
                _diversityAntiRatio = value;
                OnPropertyChanged("diversityAntiRatio");
            }
        }

        private double _diversityAntiValue = default(double); // = new Double();

        /// <summary>
        /// Description of $property$
        /// </summary>
        [Category("instanceCountCollection")]
        [DisplayName("Diversity Anti-Value")]
        [Description("Description of $property$")]
        public double diversityAntiValue
        {
            get
            {
                return _diversityAntiValue;
            }
            protected set
            {
                _diversityAntiValue = value;
                OnPropertyChanged("diversityAntiValue");
            }
        }

        /// <summary>
        /// Description of $property$
        /// </summary>
        [Category("instanceCountCollection")]
        [DisplayName("Range")]
        [Description("Rang of the frequencies in the sample")]
        public int range { get; protected set; } = default(int);

        /// <summary>
        /// The highest frequency
        /// </summary>
        [Category("instanceCountCollection")]
        [DisplayName("Max freq")]
        [Description("The highest frequency")]
        public int maxFreq { get; protected set; } = int.MinValue;

        /// <summary>
        /// The lowest frequency
        /// </summary>
        [Category("instanceCountCollection")]
        [DisplayName("Min freq")]
        [Description("The lowest frequency")]
        public int minFreq { get; protected set; } = int.MaxValue;

        private int totalScore = -1;

        /// <summary>
        /// Gets the total score - sum of all frequencies. It is total count of all instance in the sample.
        /// </summary>
        /// <value>
        /// The total number of instances observed
        /// </value>
        [DisplayName("Total score")]
        public int TotalScore
        {
            get
            {
                if (totalScore == -1)
                {
                    int output = 0;
                    foreach (int pair in items.Values)
                    {
                        output += pair;
                    }
                    totalScore = output;
                }
                return totalScore;
            }
        }

        /// <summary>
        /// Description of $property$
        /// </summary>
        [Category("instanceCountCollection")]
        [DisplayName("avgFreq")]
        [Description("Description of $property$")]
        public double avgFreq { get; protected set; } = default(double);

        /// <summary>
        /// The median of frequency
        /// </summary>
        [Category("instanceCountCollection")]
        [DisplayName("medianFreq")]
        [Description("The median of frequency")]
        public double medianFreq { get; protected set; } = default(double);

        /// <summary>
        /// Entropy of the sample
        /// </summary>
        [Category("instanceCountCollection")]
        [DisplayName("entropyFreq")]
        [Description("Entropy of the sample")]
        public double entropyFreq { get; protected set; } = default(double);

        /// <summary>
        /// Variance of the frequencies
        /// </summary>
        [Category("instanceCountCollection")]
        [DisplayName("varianceFreq")]
        [Description("Variance of the frequencies")]
        public double varianceFreq { get; protected set; } = default(double);

        /// <summary>
        /// Standard deviation in the fequencies
        /// </summary>
        [Category("instanceCountCollection")]
        [DisplayName("standardDeviation")]
        [Description("Standard deviation in the fequencies")]
        public double standardDeviation { get; protected set; } = default(double);

        ///// <summary>
        ///// Average frequency. <see cref="TotalScore"/> / <see cref="Count"/>
        ///// </summary>
        ///// <value>
        ///// The average frequency
        ///// </value>
        ////[DisplayName("Avg. freq.")]
        //public Double AverageFrequency
        //{
        //    get
        //    {
        //        return Convert.ToDouble(TotalScore) / Convert.ToDouble(items.Count);
        //    }
        //}

        ///// <summary>
        ///// Measure of collection insignificance [lower fequencies] with [higher Key count] => higher value
        ///// </summary>
        ///// <value>
        ///// Lower values suggest lower insignificance
        ///// </value>
        ////[DisplayName("DiversityAntiValue")]
        //public Double DiversityAntiValue
        //{
        //    get
        //    {
        //        Double avg = AverageFrequency;
        //        return Convert.ToDouble(TotalScore) / Convert.ToDouble(avg * avg);

        //    }
        //}

        ///// <summary>
        ///// Measure of collection insignificance [lower fequencies] with [higher Key count] => higher value
        ///// </summary>
        ///// <value>
        ///// Values near 0 suggest high significance, values near 1 mean low significance. It it always between 0 and 1 but never 0 and 1 exactly.
        ///// </value>
        ////[DisplayName("Diversity anti ratio")]
        //public Double DiversityAntiRatio
        //{
        //    get
        //    {
        //        return Convert.ToDouble(AverageFrequency * AverageFrequency) / Convert.ToDouble(TotalScore);
        //    }

        //}

        ///// <summary>
        ///// Measure of collection significance [lower fequencies] with [higher Key count] => higher value
        ///// </summary>
        ///// <value>
        ///// Values near 0 suggest low significance, values near 1 mean higher significance. It it always between 0 and 1 but never 0 and 1 exactly.
        ///// </value>
        //[DisplayName("Diversity ratio")]
        //public Double DiversityRatio
        //{
        //    get
        //    {
        //        return 1 - DiversityAntiRatio;
        //    }

        //}

        /// <summary>
        /// Default compare mode used when compared against another instance
        /// </summary>
        public instanceCountCollectionFormulae compareModeDefault { get; set; } = instanceCountCollectionFormulae.keyCount;

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="compareMode">The compare mode.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public int CompareTo(instanceCountCollectionFormulae compareMode, IInstanceCountCollection obj)
        {
            switch (compareMode)
            {
                case instanceCountCollectionFormulae.avgFrequency:
                    return avgFreq.CompareTo(obj.avgFreq);
                    break;

                default:
                case instanceCountCollectionFormulae.keyCount:
                    return Count.CompareTo(obj.Count);
                    break;

                case instanceCountCollectionFormulae.totalScore:
                    return TotalScore.CompareTo(obj.Count);
                    break;

                case instanceCountCollectionFormulae.significance:
                    return diversityRatio.CompareTo(obj.diversityRatio);
                    break;
            }
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return CompareTo(compareModeDefault, obj as IInstanceCountCollection);
        }

        //  public DataTable buildDataTable()

        /// <summary>
        /// Adds all instances and their existing score
        /// </summary>
        /// <param name="source">The source.</param>
        public virtual void AddInstanceRange(instanceCountCollection<T> source)
        {
            foreach (var pair in source.items)
            {
                AddInstance(pair.Key, pair.Value);
            }
        }

        public virtual void AddInstanceRange(IEnumerable<T> source)
        {
            foreach (T pair in source)
            {
                AddInstance(pair, "Add instance enumerable");
            }
        }

        /// <summary>
        /// Reduces this scores by specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void Reduce(instanceCountCollection<T> source)
        {
            foreach (var pair in source.items)
            {
                Reduce(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Reduces the freq to all items for the number specified
        /// </summary>
        /// <param name="reduction">The reduction.</param>
        public instanceCountCollection<T> ReduceFreqAll(int reduction = 1)
        {
            foreach (var pair in items)
            {
                items[pair.Key] = pair.Value - reduction;
            }
            return this;
        }

        /// <summary>
        /// Removes the entries having frequency under specified value
        /// </summary>
        /// <param name="removeUnder">The remove under.</param>
        public instanceCountCollection<T> RemoveUnderFreg(int removeUnder = 1)
        {
            List<T> toRemove = new List<T>();
            foreach (var pair in items)
            {
                if (items[pair.Key] < removeUnder)
                {
                    toRemove.Add(pair.Key);
                }
            }
            foreach (T key in toRemove)
            {
                items.Remove(key);
            }
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<T, int> items { get; private set; } = new Dictionary<T, int>();

        /// <summary>
        /// Gets an <see cref="T:System.Collections.ICollection" /> object containing the keys of the <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        public ICollection Keys
        {
            get
            {
                return items.Keys;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.ICollection" /> object containing the values in the <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return items.Values;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IDictionary" /> object is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IDictionary" /> object has a fixed size.
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        [DisplayName("Distinct values")]
        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public object SyncRoot
        {
            get
            {
                return ((ICollection)items).SyncRoot;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return ((ICollection)items).IsSynchronized;
            }
        }

        private object getRFreqLock = new object();

        private object GetRFreqArrayLock = new object();

        /// <summary>
        /// Returns Array of relative frequencies
        /// </summary>
        /// <returns></returns>
        public virtual double[] GetRFreqArray()
        {
            lock (GetRFreqArrayLock)
            {
                if (HasChanges)
                {
                    doMinMaxSum();
                    Accept();
                }

                double[] output = new double[Count];
                int i = 0;
                double mx = ((double)maxFreq);
                foreach (var pair in items)
                {
                    output[i] = ((double)pair.Value) / mx;
                }
                return output;
            }
        }

        /// <summary>
        /// Returns relative frequency calculated as follows: absolute frequency divided by maximum frequency: rF = aF / maxF
        /// </summary>
        /// <value>
        /// The <see cref="Double"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual double GetRFreq(T key)
        {
            if (!items.ContainsKey(key)) return 0;
            lock (getRFreqLock)
            {
                int aF = this[(object)key];

                if (HasChanges)
                {
                    doMinMaxSum();
                    Accept();
                }

                if (maxFreq > 0)
                {
                    return ((double)aF) / ((double)maxFreq);
                }
                else
                {
                    if (aF > 0) return 1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual int this[object key]
        {
            get
            {
                if (key == null) return 0;

                if (!items.ContainsKey((T)key))
                {
                    items.Add((T)key, 0);
                }
                return items[(T)key];
            }

            set
            {
                if (key == null) return;
                if (!items.ContainsKey((T)key))
                {
                    items.Add((T)key, 0);
                }
                items[(T)key] = (int)value;
            }
        }

        //private static int CompareKeyValuePairs(KeyValuePair<T, Int32> a, KeyValuePair<T, Int32> b)
        //{
        //    return a.Value.CompareTo(b.Value);
        //}

        /// <summary>
        /// Gets the sorted list with all instances
        /// </summary>
        /// <returns></returns>
        public List<T> getSorted()
        {
            List<T> output = new List<T>();
            var sorted = items.OrderByDescending(pair => pair.Value).ToList();
            for (int i = 0; i < sorted.Count(); i++)
            {
                output.Add(sorted[i].Key);
            }
            return output;

            //List<T> sout = new List<T>();
            //List<T> sout = items.toSortedList<T>();

            //List<KeyValuePair<T, Int32>> pairs = new List<KeyValuePair<T, int>>();

            //foreach (KeyValuePair<T, Int32> pair in items)
            //{
            //    pairs.Add(pair);
            //}

            //pairs.Sort(CompareKeyValuePairs);

            //foreach (KeyValuePair<T, Int32> k in pairs)
            //{
            //    sout.Add(k.Key);
            //}

            //return sout;
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <param name="key">The <see cref="T:System.Object" /> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to use as the value of the element to add.</param>
        public virtual void AddInstance(object key, object value)
        {
            if (key == null) return;
            if (!items.ContainsKey((T)key))
            {
                items.Add((T)key, 0);
            }
            items[(T)key] += (int)value;
            InvokeChanged();
        }

        public virtual void Reduce(T key, int value)
        {
            if (items.ContainsKey((T)key))
            {
                items[key] -= value;
                if (items[key] < 1) items.Remove(key);
            }
        }

        public virtual void DivideFrequency(T key, int division)
        {
            if (division == 0)
            {
                throw new ArgumentOutOfRangeException("DivideFrequency by zero! " + "DivideFrequency by zero!");
            }
            if (items.ContainsKey((T)key))
            {
                if (items[key] > 0)
                {
                    items[key] = items[key] / division;
                }
                if (items[key] < 1) items.Remove(key);
            }
            InvokeChanged();
        }

        public virtual void DivideAllFrequencies(int division)
        {
            if (division == 0)
            {
                throw new ArgumentOutOfRangeException("DivideAllFrequencies by zero! " + "DivideAllFrequencies by zero!");
            }
            foreach (var pair in this)
            {
                DivideFrequency(pair, division);
            }
            InvokeChanged();
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void AddInstance(T item, string comment = "")
        {
            if (item == null)
            {
                return;
                //if (Count == 0)
                //{
                //   String msg = "[" + comment + "] --> instanceCountCollection<" + typeof(T).Name + "> received [null] item but it has no entries so far -- Add() call";
                //   aceLog.log(msg, this);

                //    if (comment == "Web page caption @ spiderPage")
                //    {
                //    } else
                //    {
                //    }
                //}
            }

            if (items.ContainsKey(item))
            {
                items[item]++;
            }
            else
            {
                items.Add(item, 1);
            }
            InvokeChanged();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            reCalculate(preCalculateTasks.all);
            return items.Keys.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            reCalculate(preCalculateTasks.all);
            return items.GetEnumerator();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IDictionary" /> object contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.IDictionary" /> object.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.IDictionary" /> contains an element with the key; otherwise, false.
        /// </returns>
        public bool Contains(object key) => items.ContainsKey((T)key);

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        public virtual void Clear()
        {
            items.Clear();
            Accept();
        }

        ///// <summary>
        ///// Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> object for the <see cref="T:System.Collections.IDictionary" /> object.
        ///// </summary>
        ///// <returns>
        ///// An <see cref="T:System.Collections.IDictionaryEnumerator" /> object for the <see cref="T:System.Collections.IDictionary" /> object.
        ///// </returns>
        //IDictionaryEnumerator IDictionary.GetEnumerator() => items.GetEnumerator();

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        public void Remove(object key)
        {
            OnPropertyChanged("Count");

            items.Remove((T)key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
}