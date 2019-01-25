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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace imbSCI.Core.math.range.finder
{

    /// <summary>
    /// Typed version of <see cref="rangeFinderForProperty"/> 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="imbSCI.Core.math.range.finder.rangeFinderForProperty" />
    public class rangeFinderForProperty<T> : rangeFinderForProperty where T : class
    {
        public rangeFinderForProperty(PropertyInfo _property) : base(_property)
        {

        }

        public rangeFinderForProperty(String propertyName)
        {
            //hostType = typeof(T);
            property = hostType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            Deploy(property);
            //id = propertyName;
        }


        /// <summary>
        /// Performs one learning call
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        public virtual void Learn(T dataObject)
        {
            base.Learn(dataObject);
        }

    }

    /// <summary>
    /// Range finder set to a property
    /// </summary>
    /// <seealso cref="imbSCI.Core.math.range.finder.rangeFinderForProperty" />
    public class rangeFinderForProperty : rangeFinder
    {
        internal PropertyInfo property { get; set; }
        internal Type hostType { get; set; }

        internal settingsMemberInfoGroup infoGroup { get; set; }

        //public Boolean IsEnabled { get; set; } = true;

        protected rangeFinderForProperty()
        {

        }

        /// <summary>
        /// Gets the settings information group.
        /// </summary>
        /// <returns></returns>
        public settingsMemberInfoGroup GetSettingsInfoGroup()
        {

            settingsMemberInfoGroup output = new settingsMemberInfoGroup(id);

            settingsPropertyEntry entry = new settingsPropertyEntry(property);

            settingsEntriesForObject seo = new settingsEntriesForObject(this);

            foreach (var seo_pair in seo.spes)
            {
                if (seo_pair.Key == nameof(id)) continue;

                settingsPropertyEntry seo_entry = new settingsPropertyEntry(property);
                seo_entry.name = id + "_" + seo_pair.Value.name;
                seo_entry.displayName = seo_pair.Value.displayName + " of " + seo_entry.displayName;
                seo_entry.description = seo_pair.Value.description + " -> " + seo_entry.description;
                seo_entry.unit = seo_entry.unit.or(seo_pair.Value.unit);
                seo_entry.letter = seo_entry.letter.or(seo_pair.Value.letter);
                seo_entry.format = seo_entry.format.or(seo_pair.Value.format);
                if (seo_pair.Value.name == nameof(rangeFinder.Count))
                {
                    seo_entry.format = "D";
                }


                seo_entry.type = seo_pair.Value.type;
                seo_entry.categoryName = entry.displayName;
                seo_entry.color = entry.color;




                output.Add(seo_entry);

            }

            return output;
        }

        /// <summary>
        /// Declares data columns in the data table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public DataTable SetDataTable(DataTable table)
        {

            if (table == null) throw new ArgumentNullException(nameof(table));
            //if (table == null) table = new DataTable(property.Name);

            return infoGroup.SetDataTable(table);
        }


        /// <summary>
        /// Sets values to the datarow
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">dr</exception>
        public DataRow SetDataRow(DataRow dr)
        {

            //dr = infoG //table.NewRow(); // new DataTable(name);
            if (dr == null) throw new ArgumentNullException(nameof(dr));

            if (IsLearned)
            {

                var dict = GetDictionary(id + "_");

                foreach (var p in dict)
                {
                    if (dr.Table.Columns.Contains(p.Key))
                    {
                        dr[p.Key] = p.Value;
                    }
                    else
                    {

                    }

                }

            }
            //if (addRowBeforeEnd)
            //{
            //    table.Rows.Add(dr);
            //}

            return dr;
        }


        protected void Deploy(PropertyInfo _property)
        {
            property = _property;
            hostType = property.DeclaringType;
            id = property.Name;

            infoGroup = GetSettingsInfoGroup();
        }

        public rangeFinderForProperty(PropertyInfo _property)
        {
            Deploy(_property);
        }



        /// <summary>
        /// Learns property value from the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        public virtual void Learn(Object dataObject)
        {
            if (dataObject == null) return;

            if (dataObject.GetType() == hostType)
            {
                Object val = property.GetValue(dataObject, null);
                if (val == null) return;

                base.Learn(Convert.ToDouble(val));
            }
            else if (dataObject.GetType() == typeof(Double))
            {
                base.Learn((Double)dataObject);
            }
            else
            {
                throw new ArgumentException("Learn failed for [" + id + "] as dataObject is of type [" + dataObject.GetType().Name + "] and not [" + hostType.Name + "]");
            }
        }

    }
}