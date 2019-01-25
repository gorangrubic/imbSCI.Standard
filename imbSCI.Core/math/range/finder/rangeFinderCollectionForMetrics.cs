// --------------------------------------------------------------------------------------------------------------------
// <copyright file="rangeFinder.cs" company="imbVeles" >
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
using imbSCI.Core.attributes;
using imbSCI.Core.data;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace imbSCI.Core.math.range.finder
{



    /// <summary>
    /// rangeFinders for numeric properties of the object {T}
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class rangeFinderCollectionForMetrics<T> : IObjectWithName where T : class
    {
        public String name { get; set; } = "";

        protected Dictionary<PropertyInfo, rangeFinderForProperty<T>> metricProperties { get; set; } = new Dictionary<PropertyInfo, rangeFinderForProperty<T>>();

        protected Dictionary<PropertyInfo, settingsPropertyEntryWithContext> textProperties { get; set; } = new Dictionary<PropertyInfo, settingsPropertyEntryWithContext>();

        protected Dictionary<PropertyInfo, String> textPropertyValues { get; set; } = new Dictionary<PropertyInfo, string>();

        //  protected settingsMemberInfoGroupDictionary memberInfo { get; set; } = new settingsMemberInfoGroupDictionary();

        //protected settingsEntriesForObject hostTypeInfo { get; set; }


        internal Type hostType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="rangeFinderCollectionForMetrics{T}"/> class.
        /// </summary>
        /// <param name="learnMetaDataFrom">The learn meta data from.</param>
        public rangeFinderCollectionForMetrics(rangeFinderCollectionForMetrics<T> learnMetaDataFrom)
        {
            hostType = learnMetaDataFrom.hostType;

            foreach (var pair in learnMetaDataFrom.metricProperties) metricProperties.Add(pair.Key, new rangeFinderForProperty<T>(pair.Key));
            textProperties = learnMetaDataFrom.textProperties;
            textPropertyValues = new Dictionary<PropertyInfo, string>();

        }


        public rangeFinderCollectionForMetrics()
        {
            Deploy(typeof(T));
        }

        protected Boolean IsTypeAcceptable(PropertyInfo pi)
        {
            //if (pi.PropertyType == typeof(String)) return true;
            if (pi.PropertyType.isNumber()) return true;
            return false;
        }

        /// <summary>
        /// Deploys the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        public void Deploy(Type type)
        {
            hostType = type;
            var hostTypeInfo = new settingsEntriesForObject(type, false);

            foreach (var spe in hostTypeInfo.spes)
            {
                if (IsTypeAcceptable(spe.Value.pi))
                {
                    metricProperties.Add(spe.Value.pi, new rangeFinderForProperty<T>(spe.Value.pi));
                }
                else if (spe.Value.pi.PropertyType == typeof(String))
                {
                    textProperties.Add(spe.Value.pi, spe.Value);
                    textPropertyValues.Add(spe.Value.pi, "");
                }
                else
                {

                }
            }

        }


        /// <summary>
        /// Gets the dictionary with all values
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, Object> GetDictionary()
        {

            Dictionary<String, Object> output = new Dictionary<string, object>();

            foreach (var pair in textProperties)
            {
                output.Add(pair.Key.Name, textPropertyValues[pair.Key]);


            }

            foreach (var pair in metricProperties)
            {
                var dict = pair.Value.GetDictionary(pair.Value.id + "_");
                foreach (var dp in dict)
                {
                    output.Add(dp.Key, dp.Value);
                }
            }

            return output;
        }

        /// <summary>
        /// Adds columns and transfers formating and other meta information specified in the <see cref="settingsPropertyEntry"/> entries. 
        /// </summary>
        /// <param name="table">The table to write columns into. If not specified, it will create new with name of the group.</param>
        /// <remarks>The method will declare only <see cref="settingsPropertyEntry"/>s, not the entries of other type</remarks>
        /// <returns></returns>
        public virtual DataTable SetDataTable(DataTable table = null)
        {
            if (table == null) table = new DataTable(hostType.Name.getCleanTypeFullName() + "_range");


            if (!table.Columns.Contains(nameof(name)))
            {
                var dtc = table.Columns.Add(nameof(name));
            }


            foreach (var pair in textProperties)
            {
                if (!table.Columns.Contains(pair.Key.Name))
                {
                    var dtc = table.Columns.Add(pair.Key.Name);
                    dtc.SetSPE(pair.Value);
                }
            }

            foreach (var pair in metricProperties)
            {
                pair.Value.SetDataTable(table);
            }

            return table;
        }

        /// <summary>
        /// Sets values to the datarow
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">dr</exception>
        public virtual DataRow SetDataRow(DataRow dr)
        {

            //dr = infoG //table.NewRow(); // new DataTable(name);
            if (dr == null) throw new ArgumentNullException(nameof(dr));

            if (dr.Table.Columns.Contains(nameof(name)))
            {
                dr[nameof(name)] = name;

            }


            foreach (var pair in textProperties)
            {
                if (dr.Table.Columns.Contains(pair.Key.Name))
                {
                    dr[pair.Key.Name] = textPropertyValues[pair.Key];
                    //var dtc = dr.Table.Columns.Add(pair.Key.Name);
                    //dtc.SetSPE(pair.Value);
                }
            }

            foreach (var pair in metricProperties)
            {
                pair.Value.SetDataRow(dr);
            }




            return dr;
        }

        public virtual void Learn(IEnumerable<T> dataObject)
        {
            foreach (var pair in dataObject)
            {
                Learn(pair);
            }
        }

        /// <summary>
        /// Performs one learning call
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        public virtual void Learn(T dataObject)
        {

            foreach (var pair in metricProperties)
            {
                pair.Value.Learn(dataObject);
            }

            foreach (var pair in textProperties)
            {
                if (!textPropertyValues.ContainsKey(pair.Key))
                {
                    String v = pair.Key.GetValue(dataObject, null) as String;
                    if (!v.isNullOrEmpty())
                    {
                        textPropertyValues.Add(pair.Key, v);
                    }

                }

            }

        }

    }
}