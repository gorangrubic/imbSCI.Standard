// --------------------------------------------------------------------------------------------------------------------
// <copyright file="settingsEntriesForObjectExtensions.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Core.reporting.zone;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data
{
    /// <summary>
    /// Extensions for <see cref="settingsEntriesForObject"/>
    /// </summary>
    public static class settingsEntriesForObjectExtensions
    {


        public static String GetGUI(this PropertyInfo pi)
        {
            return pi.DeclaringType.Name + "." + pi.Name;
        }
        
        /// <summary>
        /// Static cached accessor for <see cref="settingsEntriesForObject"/>
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static settingsEntriesForObject GetSEO(this Type key) {
                if (SEOCache.ContainsKey(key)) return SEOCache[key];
                settingsEntriesForObject output = new settingsEntriesForObject(key);
                if (!SEOCache.ContainsKey(key))
                {
                    lock (_SEO_Get_lock) { if (!SEOCache.ContainsKey(key)) { SEOCache.Add(key, output); }
}
                }
            return output;
            }

            private static Object _SEO_Get_lock = new Object();
            private static Object _SEO_lock = new Object();
            private static Dictionary<Type, settingsEntriesForObject> _SEOCache;
            private static Dictionary<Type, settingsEntriesForObject> SEOCache
            {
                get
                {
                    if (_SEOCache == null)
                    {
                        lock (_SEO_Get_lock)
                        {

                            if (_SEOCache == null)
                            {
                                _SEOCache = new Dictionary<Type, settingsEntriesForObject>();
                                // add here if any additional initialization code is required
                            }
                        }
                    }
                    return _SEOCache;
                }
            }

        /// <summary>Gets C# code lines to be inserted before property declaration</summary>
        /// <param name="property">The property.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>List with C# code lines declaring attributes and XML documentation</returns>
        public static List<String> GetPropertyCodeInsertLines(this settingsPropertyEntry property, PropertyAppendFlags flags = PropertyAppendFlags.setAll)
        {
            List<String> inserts = new List<string>();

            if (flags.HasFlag(PropertyAppendFlags.setComponentModelAttributes))
            {
                inserts.Add($"[Category(\"{property.categoryName}\")]");
                inserts.Add($"[DisplayName(\"{property.displayName}\")]");
                inserts.Add($"[Description(\"{property.description}\")]");
            }

            if (flags.HasFlag(PropertyAppendFlags.setXmlSerializationAttributes))
            {

                if (property.IsXmlIgnore)
                {
                    inserts.Add($"[XmlIgnore]");
                }
            }

            if (flags.HasFlag(PropertyAppendFlags.setSCIReportingDefinitions))
            {
                if (!property.escapeValueString)
                {
                    inserts.Add($"[imb(imbAttributeName.reporting_escapeoff)]");
                }

                if (!property.letter.isNullOrEmpty())
                {
                    inserts.Add($"[imb(imbAttributeName.measure_letter, \"{property.letter}\")]");
                }

                if (!property.unit.isNullOrEmpty())
                {
                    inserts.Add($"[imb(imbAttributeName.measure_setUnit, \"{property.unit}\")]");
                }

                if (!property.format.isNullOrEmpty())
                {
                    inserts.Add($"[imb(imbAttributeName.reporting_valueformat, \"{property.format}\")]");
                }

                if (property.width != 0)
                {
                    inserts.Add($"[imb(imbAttributeName.reporting_columnWidth, {property.width})]");
                }

                if (property.isHiddenInReport)
                {
                    inserts.Add($"[imb(imbAttributeName.reporting_hide)]");
                }

                if (!property.color.isNullOrEmpty())
                {

                    inserts.Add($"[imb(imbAttributeName.basicColor, \"{property.color}\")]");

                }

                if (property.Alignment != textCursorZoneCorner.none)
                {
                    inserts.Add($"[imb(templateFieldDataTable.col_alignment, textCursorZoneCorner.{property.Alignment})]");
                }

            }



            if (flags.HasFlag(PropertyAppendFlags.setXmlDocumentation))
            {
                inserts.Add("/// <summary>");

                inserts.Add($"/// {property.description}");

                inserts.Add("/// </summary>");
            }

            return inserts;
        }


        /*
        public static DataTable SetVerticalDataTable(settingsMemberInfoGroupDictionary output, DataTable table)
        {
            if (table != null)
            {
                table = new DataTable();
                table.Columns.Add("Parametar");
            }

            
        }*/



        /// <summary>
        /// Adds columns and transfers formating and other meta information specified in the <see cref="settingsPropertyEntry"/> entries. 
        /// </summary>
        /// <param name="table">The table to write columns into. If not specified, it will create new with name of the group.</param>
        /// <remarks>The method will declare only <see cref="settingsPropertyEntry"/>s, not the entries of other type</remarks>
        /// <returns></returns>
        public static DataTable SetDataTable(this IEnumerable<settingsMemberInfoEntry> input, DataTable table)
        {
            //if (table == null) table = new DataTable();

            foreach (var p in input)
            {
                if (p is settingsPropertyEntry spe)
                {
                    if (!table.Columns.Contains(p.name))
                    {
                        var dtc = table.Columns.Add(p.name);
                        dtc.SetSPE(spe);
                    }
                }
            }

            return table;
        }

        /// <summary>
        /// Sets the data row.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="table">The table.</param>
        /// <param name="data">The data.</param>
        /// <param name="dr">The dr.</param>
        /// <param name="addRowBeforeEnd">if set to <c>true</c> [add row before end].</param>
        /// <returns></returns>
        public static DataRow SetDataRow(this IEnumerable<settingsMemberInfoEntry> input, DataTable table, Object data, DataRow dr, bool addRowBeforeEnd = true)
        {
            if (dr == null) dr = table.NewRow(); // new DataTable(name);

            var dict = GetDictionary(input, data);

            foreach (var p in dict)
            {

                if (dr.Table.Columns.Contains(p.Key))
                {
                    dr[p.Key] = p.Value;
                }

            }

            if (addRowBeforeEnd)
            {
                table.Rows.Add(dr);
            }

            return dr;
        }


        /// <summary>
        /// Gets the dictionary of values, key = prefix + PropertyInfo.Name
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="data">Data instance, to read value from</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        public static Dictionary<String, Object> GetDictionary(this IEnumerable<settingsMemberInfoEntry> input, Object data, String prefix = "")
        {
            Dictionary<String, Object> output = new Dictionary<string, Object>();

            foreach (settingsMemberInfoEntry entry in input)
            {
                if (entry is settingsPropertyEntry pe)
                {
                    Object v = pe.pi.GetValue(data, null);
                    output.Add(prefix + pe.name, v);
                }
            }
            return output;
        }


        public static Dictionary<String, T> GetDictionaryOf<T>(this IEnumerable<settingsMemberInfoEntry> input, Object data, String prefix = "")
        {
            Dictionary<String, T> output = new Dictionary<string, T>();
            Type type = typeof(T);
            foreach (settingsMemberInfoEntry entry in input)
            {
                if (entry is settingsPropertyEntry pe)
                {
                    if (pe.type == type)
                    {
                        T v = (T)pe.pi.GetValue(data, null);
                        output.Add(prefix + pe.name, v);
                    }
                }
            }
            return output;
        }


        /// <summary>
        /// Builds member info group dictionary
        /// </summary>
        /// <param name="objectInfo">The object information.</param>
        /// <returns></returns>
        public static settingsMemberInfoGroupDictionary GetMemberInfoGroupDictionary(this settingsEntriesForObject objectInfo)
        {
            List<String> groups = new List<string>();
            settingsMemberInfoGroupDictionary output = new settingsMemberInfoGroupDictionary();

            if (objectInfo.CategoryByPriority.Any())
            {
                groups = objectInfo.CategoryByPriority;
            }
            else
            {
                foreach (var pp in objectInfo.spes)
                {
                    if (!groups.Contains(pp.Value.categoryName))
                    {
                        groups.Add(pp.Value.categoryName);
                    }
                }
            }

            foreach (String g in groups)
            {
                output.Add(g, new settingsMemberInfoGroup(g));
            }

            foreach (var pp in objectInfo.spes)
            {
                if (!pp.Value.categoryName.isNullOrEmpty())
                {
                    if (groups.Contains(pp.Value.categoryName))
                    {
                        output[pp.Value.categoryName].Add(pp.Value);
                    }
                }
            }

            return output;
        }
    }
}