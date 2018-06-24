// --------------------------------------------------------------------------------------------------------------------
// <copyright file="collectionAggregation.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.extensions.data.operations
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.data;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.math;
    using imbSCI.Core.math.aggregation;
    using imbSCI.Data;
    using imbSCI.Data.collection.nested;
    using imbSCI.DataComplex.extensions.data.schema;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    public static class collectionAggregation
    {
        public static DataTable GetParallelAggregates<T>(this List<IEnumerable<T>> sources, string column_snap, string column_prefix, dataPointAggregationType column_sideAggregates, params string[] column_toInclude)
        {
            settingsEntriesForObject sEO = new settingsEntriesForObject(typeof(T));
            settingsPropertyEntry sPE_snap = sEO.spes[column_snap];
            settingsPropertyEntry sPE_prefix = sEO.spes[column_prefix];

            List<settingsPropertyEntry> sPE_toInclude = new List<settingsPropertyEntry>();
            foreach (string toInclude in column_toInclude)
            {
                sPE_toInclude.Add(sEO.spes[toInclude]);
            }

            List<dataPointAggregationType> side_aggregates = column_sideAggregates.getEnumListFromFlags<dataPointAggregationType>();

            Dictionary<dataPointAggregationType, settingsPropertyEntry> sPE_sideAggregates = new Dictionary<dataPointAggregationType, settingsPropertyEntry>();

            Dictionary<settingsPropertyEntry, dataPointAggregationType> sPE_sideAggregatesContra = new Dictionary<settingsPropertyEntry, dataPointAggregationType>();

            foreach (settingsPropertyEntry toInclude in sPE_toInclude)
            {
                foreach (dataPointAggregationType sideType in side_aggregates)
                {
                    settingsPropertyEntry sPE = new settingsPropertyEntry(toInclude.pi);
                    sPE.type = typeof(double);
                    sPE.name = sPE.name + "_" + sideType.ToString();
                    sPE_sideAggregates.Add(sideType, sPE);
                    sPE_sideAggregatesContra.Add(sPE, sideType);
                }
            }

            // <---------------------------- preparing data

            Dictionary<string, IEnumerable<T>> dataByPrefix = new Dictionary<string, IEnumerable<T>>();
            int c = 0;
            foreach (IEnumerable<T> s in sources)
            {
                T firstItem = s.FirstOrDefault<T>();
                if (firstItem != null)
                {
                    string prefix = firstItem.imbGetPropertySafe(sPE_prefix.pi).toStringSafe(c.ToString("D3"));
                    dataByPrefix.Add(prefix, s);
                }
                c++;
            }

            // <----- DataColumn Index

            aceDictionarySet<string, DataColumn> columnsByPrefix = new aceDictionarySet<string, DataColumn>();

            aceDictionarySet<string, DataColumn> columnsSideAggregationByPrefix = new aceDictionarySet<string, DataColumn>();

            // <------------------------- building Shema
            DataTable output = new DataTable();
            output.TableName = "ParallelAggregate_by_" + column_snap;

            DataColumn col_recordID = output.Add("ID", "Row ordinal number", "ID", typeof(int), dataPointImportance.normal, "D3").SetUnit("#");
            settingsPropertyEntry sPE_recID = col_recordID.GetSPE();

            DataColumn col_snap = output.Add(sPE_snap);

            aceDictionary2D<settingsPropertyEntry, dataPointAggregationType, DataColumn> columnsByAggregationType = new aceDictionary2D<settingsPropertyEntry, dataPointAggregationType, DataColumn>();
            aceDictionarySet<settingsPropertyEntry, DataColumn> columnsBySource = new aceDictionarySet<settingsPropertyEntry, DataColumn>();

            foreach (settingsPropertyEntry toInclude in sPE_toInclude)
            {
                foreach (var pair in dataByPrefix)
                {
                    DataColumn nColumn = output.Add(toInclude);
                    nColumn.ColumnName = pair.Key + "_" + nColumn.ColumnName;
                    nColumn.SetGroup(pair.Key);

                    columnsByPrefix.Add(pair.Key, nColumn);

                    columnsBySource.Add(toInclude, nColumn);
                }

                foreach (var pair2 in sPE_sideAggregatesContra)
                {
                    DataColumn nColumn2 = output.Add(toInclude);
                    nColumn2.SetGroup("Aggregate");

                    //                    columnsSideAggregationByPrefix.Add(pair.Key, nColumn);
                }
            }

            // <----------------------------------------------------------- collecting rows

            aceDictionary2D<string, settingsPropertyEntry, object> dataRowBySnapValue = new aceDictionary2D<string, settingsPropertyEntry, object>();

            int riMax = 0;
            foreach (string prefix in dataByPrefix.Keys)
            {
                IEnumerable<T> s = dataByPrefix[prefix];
                int ri = 0;
                foreach (T si in s)
                {
                    ri++;
                    string snapValue = si.imbGetPropertySafe(sPE_snap.pi).toStringSafe();

                    dataRowBySnapValue[snapValue, sPE_snap] = snapValue;
                    dataRowBySnapValue[snapValue, sPE_recID] = ri;

                    foreach (settingsPropertyEntry toInclude in sPE_toInclude)
                    {
                        foreach (var pair in columnsByPrefix[prefix])
                        {
                            var spe = dataColumnRenderingSetup.GetSPE(pair);

                            dataRowBySnapValue[snapValue, spe] = si.imbGetPropertySafe(spe.pi);
                        }
                    }
                    riMax = Math.Max(ri, riMax);
                }
            }

            foreach (string prefix in dataByPrefix.Keys)
            {
            }

            //List<Double> data = new List<Double>();
            //foreach (var pair2 in columnsSideAggregationByPrefix[prefix])
            //{
            //    var spe2 = pair.GetSPE();

            //    dataRowBySnapValue[snapValue, spe2] = si.imbGetPropertySafe(spe2.pi);
            //}

            return output;
        }

        /// <summary>
        /// Gets aggregated version of the objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static collectionAggregationResult<T> GetAggregates<T>(this IEnumerable<T> source, dataPointAggregationType type = dataPointAggregationType.avg, bool stringKeepLastEntry = true) where T : class, new()
        {
            //if (type == dataPointAggregationType.none)
            //{
            //}

            var aggList = type.getEnumListFromFlags<dataPointAggregationType>();

            collectionAggregationResult<T> output = new collectionAggregationResult<T>();
            output.aspect = dataPointAggregationAspect.subSetOfRows;

            aceDictionary2D<dataPointAggregationType, PropertyInfo, double> outputData = new aceDictionary2D<dataPointAggregationType, PropertyInfo, double>();

            aceDictionary2D<dataPointAggregationType, PropertyInfo, List<double>> datatCollections = new aceDictionary2D<dataPointAggregationType, PropertyInfo, List<double>>();

            Type t = typeof(T);

            List<PropertyInfo> nominalList = new List<PropertyInfo>();
            List<PropertyInfo> piList = new List<PropertyInfo>();

            Dictionary<PropertyInfo, settingsPropertyEntry> sPEDict = new Dictionary<PropertyInfo, settingsPropertyEntry>();

            foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.GetProperty))
            {
                settingsPropertyEntry sPE = new settingsPropertyEntry(pi);

                bool ok = true;

                if (!pi.CanWrite) ok = false;

                if (ok && pi.PropertyType == typeof(string))
                {
                    nominalList.Add(pi);
                    ok = false;
                }
                else if (ok && pi.PropertyType == typeof(Enum))
                {
                    ok = false;
                }

                if (ok && sPE.aggregation[dataPointAggregationAspect.subSetOfRows].HasFlag(dataPointAggregationType.hidden))
                {
                    ok = false;
                }
                if (ok && sPE.attributes.ContainsKey(imbAttributeName.reporting_hide))
                {
                    ok = false;
                }

                if (ok)
                {
                    sPEDict.Add(pi, sPE);
                    piList.Add(pi);
                }
            }

            if (aggList.Contains(dataPointAggregationType.avg)) aggList.AddUnique(dataPointAggregationType.sum);

            if (aggList.Contains(dataPointAggregationType.range))
            {
                aggList.AddUnique(dataPointAggregationType.min);
                aggList.AddUnique(dataPointAggregationType.max);
            }

            foreach (dataPointAggregationType aggType in aggList)
            {
                output.Add(aggType, new T());

                switch (aggType)
                {
                    case dataPointAggregationType.var:
                    case dataPointAggregationType.stdev:
                    case dataPointAggregationType.entropy:
                        foreach (PropertyInfo pi in piList)
                        {
                            datatCollections[aggType, pi] = new List<double>(); //.Add(item.imbGetPropertySafe<Double>(pi));
                        }
                        break;
                }

                // outputData.Add(aggType, 0);
            }

            int count = 0;

            // <------------ first pass
            foreach (T item in source)
            {
                if (output.firstItem == null) output.firstItem = item;
                output.lastItem = item;
                foreach (dataPointAggregationType aggType in aggList)
                {
                    foreach (PropertyInfo pi in piList)
                    {
                        double vl = outputData[aggType, pi];

                        switch (aggType)
                        {
                            case dataPointAggregationType.sum:
                                vl = vl + item.imbGetPropertySafe<double>(pi);
                                break;

                            case dataPointAggregationType.min:
                                vl = Math.Min(item.imbGetPropertySafe<double>(pi), vl);
                                break;

                            case dataPointAggregationType.max:
                                vl = Math.Max(item.imbGetPropertySafe<double>(pi), vl);
                                break;

                            case dataPointAggregationType.var:
                            case dataPointAggregationType.stdev:
                            case dataPointAggregationType.entropy:
                                datatCollections[aggType, pi].Add(item.imbGetPropertySafe<double>(pi));
                                break;
                        }
                        outputData[aggType, pi] = vl;
                    }
                }

                count++;
            }

            foreach (dataPointAggregationType aggType in aggList)
            {
                foreach (PropertyInfo pi in piList)
                {
                    switch (aggType)
                    {
                        case dataPointAggregationType.count:
                            outputData[aggType, pi] = count;
                            break;

                        case dataPointAggregationType.avg:
                            outputData[aggType, pi] = outputData[dataPointAggregationType.sum, pi] / (double)count;
                            break;

                        case dataPointAggregationType.range:
                            outputData[aggType, pi] = outputData[dataPointAggregationType.max, pi] - outputData[dataPointAggregationType.min, pi];
                            break;

                        case dataPointAggregationType.firstEntry:
                            outputData[aggType, pi] = output.firstItem.imbGetPropertySafe<double>(pi);
                            break;

                        case dataPointAggregationType.lastEntry:
                            outputData[aggType, pi] = output.lastItem.imbGetPropertySafe<double>(pi);
                            break;

                        case dataPointAggregationType.var:
                            outputData[aggType, pi] = datatCollections[aggType, pi].GetVariance();
                            break;

                        case dataPointAggregationType.stdev:
                            outputData[aggType, pi] = datatCollections[aggType, pi].GetStdDeviation();
                            break;

                        case dataPointAggregationType.entropy:
                            outputData[aggType, pi] = datatCollections[aggType, pi].GetEntropy();
                            break;
                    }
                }
            }

            foreach (dataPointAggregationType aggType in aggList)
            {
                foreach (PropertyInfo pi in piList)
                {
                    output[aggType].imbSetPropertyConvertSafe(pi, outputData[aggType, pi]);
                }

                if (stringKeepLastEntry)
                {
                    foreach (PropertyInfo pi in nominalList)
                    {
                        output[aggType].imbSetPropertyConvertSafe(pi, output.lastItem.imbGetPropertySafe(pi));
                    }
                }
            }
            output.Count = count;
            return output;
        }
    }
}