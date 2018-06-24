// --------------------------------------------------------------------------------------------------------------------
// <copyright file="rangeFinderForDataTable.cs" company="imbVeles" >
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
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.math.aggregation;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace imbSCI.Core.math.range.finder
{
    /// <summary>
    /// Performs aggregation over data in the table
    /// </summary>
    public class rangeFinderForDataTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="rangeFinderForDataTable"/> class.
        /// </summary>
        /// <param name="targetTable">The target table.</param>
        /// <param name="keyDataColumn">The key data column.</param>
        public rangeFinderForDataTable(DataTable targetTable, String keyDataColumn = "name")
        {
            prepareForTable(targetTable, keyDataColumn);
        }

        /// <summary>
        /// Gets or sets the key column.
        /// </summary>
        /// <value>
        /// The key column.
        /// </value>
        public DataColumn keyColumn { get; protected set; }

        /// <summary>
        /// Name of column that is UID
        /// </summary>
        /// <value>
        /// The name of the key column.
        /// </value>
        public String keyColumnName { get; protected set; } = "name";

        /// <summary>
        /// Columns to write the aggregate row name into
        /// </summary>
        /// <value>
        /// The columns to sign in.
        /// </value>
        public List<String> columnsToSignIn { get; protected set; } = new List<string>();

        /// <summary>
        /// The rows to skip from learning.
        /// </summary>
        /// <value>
        /// The rows to skip from learning.
        /// </value>
        public Int32 rowsToSkipFromLearning { get; protected set; } = 0;

        /// <summary>
        /// Prepares for the next aggregation block.
        /// </summary>
        /// <param name="targetTable">The target table.</param>
        /// <param name="keyDataColumn">The key data column.</param>
        public void prepareForNextAggregationBlock(DataTable targetTable, String _keyDataColumn = null)
        {
            rowsToSkipFromLearning = targetTable.Rows.Count;
            if (_keyDataColumn != null) keyColumnName = _keyDataColumn;
            if (keyColumn != null)
            {
                prepareForTable(targetTable, keyColumn.ColumnName);
            }
            else
            {
                prepareForTable(targetTable, _keyDataColumn);
            }
        }

        /// <summary>
        /// Prepares for the table shema -- later can be used with other data tables with the same shema
        /// </summary>
        /// <param name="targetTable">The target table.</param>
        /// <param name="keyDataColumn">The key (name/uid) column.</param>
        public void prepareForTable(DataTable targetTable, String _keyDataColumn = null)
        {
            finders.Clear();

            // rowsToSkipFromLearning = targetTable.Rows.Count;
            if (_keyDataColumn != null) keyColumnName = _keyDataColumn;
            foreach (DataColumn dc in targetTable.Columns)
            {
                Type vt = dc.GetValueType();
                if (dc.ColumnName.Equals(keyColumnName, StringComparison.CurrentCultureIgnoreCase))
                {
                    keyColumn = dc;
                    keyColumnName = dc.ColumnName;
                }

                if (vt.isNumber() || (vt == typeof(Boolean)))
                {
                    finders.Add(dc, new rangeFinderWithData(dc.ColumnName));
                }
            }
        }

        public List<rangeFinderWithData> GetRangerStartingWith(String columnNameStart)
        {
            var columns = finders.Keys.Where(x => x.ColumnName.StartsWith(columnNameStart));
            List<rangeFinderWithData> output = new List<rangeFinderWithData>();

            foreach (var cl in columns)
            {
                output.Add(finders[cl]);
            }
            return output;
        }

        public rangeFinderWithData this[String key]
        {
            get
            {
                DataColumn c = finders.Keys.First(x => x.ColumnName.Equals(key, StringComparison.InvariantCultureIgnoreCase));

                return finders[c];
            }
        }

        /// <summary>
        /// Adds the range rows into table.
        /// </summary>
        /// <param name="namePrefix">Row name prefix.</param>
        /// <param name="targetTable">The target table.</param>
        /// <param name="placeDataRowMarks">if set to <c>true</c> it will set styling conditioners to this table</param>
        /// <param name="rowsToAdd">The rows.</param>
        public void AddRangeRows(String namePrefix, DataTable targetTable, Boolean placeDataRowMarks, dataPointAggregationType rowsToAdd = dataPointAggregationType.sum | dataPointAggregationType.avg | dataPointAggregationType.count | dataPointAggregationType.min | dataPointAggregationType.max | dataPointAggregationType.range)
        {
            Int32 i = 0;
            foreach (DataRow dr in targetTable.Rows)
            {
                if (i < rowsToSkipFromLearning)
                {
                }
                else
                {
                    foreach (var pair in finders)
                    {
                        Object vl = dr[pair.Value.id];

                        if (vl is Int32)
                        {
                            pair.Value.Learn(Convert.ToDouble((Int32)vl));
                        }
                        else if (vl is Double)
                        {
                            pair.Value.Learn((Double)vl);
                        }
                        else if (vl is Boolean)
                        {
                            pair.Value.Learn(Convert.ToDouble((Boolean)vl));
                        }
                    }
                }
                i++;
            }

            List<String> rownamesHMax = new List<string>();
            List<String> rownamesHMin = new List<string>();
            List<String> rownamesH3 = new List<string>();

            foreach (dataPointAggregationType dt in rowsToAdd.getEnumListFromFlags())
            {
                DataRow dr = targetTable.NewRow();
                String name = namePrefix + " " + dt.ToString();

                if (keyColumn != null)
                {
                    dr[keyColumn.ColumnName] = name;
                }

                if (columnsToSignIn.Any())
                {
                    foreach (String cn in columnsToSignIn)
                    {
                        dr[cn] = name;
                    }
                }

                if (placeDataRowMarks && keyColumn != null)
                {
                    foreach (DataRow dd in targetTable.Rows)
                    {
                        foreach (var pair in finders)
                        {
                            rangeFinderWithData rf = pair.Value;

                            //Double vl = Convert.ToDouble(dd[rf.id].imbConvertValueSafeTyped<Double>());
                            Double vl = dd[rf.id].imbConvertValueSafeTyped<Double>();
                            switch (dt)
                            {
                                case dataPointAggregationType.max:
                                    if (vl == rf.Maximum) rownamesHMax.Add(dd[keyColumnName].toStringSafe());
                                    break;

                                case dataPointAggregationType.min:
                                    if (vl == rf.Minimum) rownamesHMin.Add(dd[keyColumnName].toStringSafe());

                                    break;
                            }
                        }
                    }
                }

                foreach (var pair in finders)
                {
                    rangeFinderWithData rf = pair.Value;

                    try
                    {
                        switch (dt)
                        {
                            case dataPointAggregationType.avg:
                                dr[rf.id] = rf.Average;
                                rownamesH3.Add(name);
                                targetTable.SetAdditionalInfoEntry("Prefix: " + dt.ToString(), "Arithmentic mean");
                                break;

                            case dataPointAggregationType.count:
                                dr[rf.id] = rf.Count;
                                targetTable.SetAdditionalInfoEntry("Prefix: " + dt.ToString(), "Number of rows");
                                break;

                            case dataPointAggregationType.max:
                                rownamesHMax.Add(name);
                                if (rf.Maximum > Double.MinValue) dr[rf.id] = rf.Maximum;
                                targetTable.SetAdditionalInfoEntry("Prefix: " + dt.ToString(), "Highest value");
                                break;

                            case dataPointAggregationType.min:
                                rownamesHMin.Add(name);
                                if (rf.Minimum < Double.MaxValue) dr[rf.id] = rf.Minimum;
                                targetTable.SetAdditionalInfoEntry("Prefix: " + dt.ToString(), "Smallest value");
                                break;

                            case dataPointAggregationType.range:
                                dr[rf.id] = rf.Range;
                                targetTable.SetAdditionalInfoEntry("Prefix: " + dt.ToString(), "Range of values");
                                break;

                            case dataPointAggregationType.sum:
                                dr[rf.id] = rf.Sum;
                                rownamesH3.Add(name);
                                targetTable.SetAdditionalInfoEntry("Prefix: " + dt.ToString(), "Sum");
                                break;

                            case dataPointAggregationType.entropy:
                                dr[rf.id] = rf.doubleEntries.GetEntropy(1E-06, true);
                                targetTable.SetAdditionalInfoEntry("Prefix: " + dt.ToString(), "Normalized Entropy");
                                rownamesH3.Add(name);
                                break;

                            case dataPointAggregationType.stdev:

                                dr[rf.id] = rf.doubleEntries.GetStdDeviation();
                                rownamesH3.Add(name);
                                targetTable.SetAdditionalInfoEntry("Prefix: " + dt.ToString(), "Standard Deviation");
                                break;

                            case dataPointAggregationType.var:
                                targetTable.SetAdditionalInfoEntry(dt.ToString(), "Variance");
                                dr[rf.id] = rf.doubleEntries.GetVariance();
                                rownamesH3.Add(name);
                                targetTable.SetAdditionalInfoEntry("Prefix: " + dt.ToString(), "Variance");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        targetTable.AddExtra("rangeFinder[" + rf.id + "] failed on [" + dt.ToString() + "] with exception: " + ex.Message);
                        targetTable.AddExtra("::: " + ex.StackTrace);
                    }
                    //if (!targetTable.GetAdditionalInfo().ContainsKey(dt)
                }

                targetTable.Rows.Add(dr);
            }

            targetTable.GetRowMetaSet().SetStyleForRowsWithValue<String>(DataRowInReportTypeEnum.dataHighlightA, keyColumnName, rownamesHMin);
            targetTable.GetRowMetaSet().SetStyleForRowsWithValue<String>(DataRowInReportTypeEnum.dataHighlightB, keyColumnName, rownamesHMax);
            targetTable.GetRowMetaSet().SetStyleForRowsWithValue<String>(DataRowInReportTypeEnum.dataHighlightC, keyColumnName, rownamesH3);
        }

        /// <summary>
        /// The finders
        /// </summary>
        protected Dictionary<DataColumn, rangeFinderWithData> finders = new Dictionary<DataColumn, rangeFinderWithData>();

        // protected Dictionary<String, DataColumn> columns = new Dictionary<String, DataColumn>();
    }
}