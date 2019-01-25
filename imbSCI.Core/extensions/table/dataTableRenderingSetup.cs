// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataTableRenderingSetup.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.table
{
    using imbSCI.Core.collection;
    using imbSCI.Core.data;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table.core;
    using imbSCI.Core.extensions.table.style;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.math.aggregation;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.lowLevelApi;
    using imbSCI.Core.reporting.render.config;
    using imbSCI.Data;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Extension methods for easy, typed and safe access to <see cref="DataTable.ExtendedProperties"/> collection
    /// </summary>
    public static class dataTableRenderingSetup
    {
        /// <summary>
        /// Sets the column width according to markdown rendering
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="rowCheckLimit">The row check limit.</param>
        /// <param name="cellMargin">Extra characters to add on content right</param>
        /// <returns></returns>
        public static DataTable setColumnWidths(this DataTable dt, Int32 rowCheckLimit = 100, Int32 cellMargin = 2)
        {
            Int32 prittyIndex = 0;
            foreach (DataRow dr in dt.Rows)
            {
                prittyIndex++;
                foreach (DataColumn dc in dt.Columns)
                {
                    dataValueFormatInfo dcf = new dataValueFormatInfo(dc);

                    String content = dc.markdownFieldForColumn(dr, dcf.directAppend, dcf);
                    dc.SetWidth(Math.Max(dc.GetWidth(), content.Length + cellMargin));
                }
                if (prittyIndex > rowCheckLimit) break;
            }
            return dt;
        }

        /// <summary>
        /// Orders the by category priority.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="categoryByPriority">The category by priority.</param>
        public static void OrderByCategoryPriority(this DataColumnCollection columns, List<string> categoryByPriority)
        {
            List<DataColumn> __columns = new List<DataColumn>();
            foreach (DataColumn dc in columns)
            {
                __columns.Add(dc);
            }
            __columns.OrderByCategoryPriority(categoryByPriority);
        }

        /// <summary>
        /// Orders the by category priority.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="categoryByPriority">The category by priority.</param>
        public static void OrderByCategoryPriority(this IEnumerable<DataColumn> columns, List<string> categoryByPriority)
        {
            Int32 i = 0;
            if (categoryByPriority == null) OrderByPriority(columns);
            if (!categoryByPriority.Any()) OrderByPriority(columns);
            List<string> ct = new List<string>();

            foreach (String cat in ct)
            {
                ct.AddUnique(cat.ToUpper());
            }

            foreach (String cat in ct)
            {
                var clm = columns.Where(x => x.GetGroup().ToUpper() == cat.ToUpper()).OrderBy<DataColumn, int>(x => x.GetPriority());
                foreach (DataColumn dc in clm)
                {
                    if (i >= dc.Table.Columns.Count) break;
                    // dc.SetPriority(i);
                    dc.SetOrdinal(i);
                    i++;
                }
            }
        }

        /// <summary>
        /// Orders the <see cref="DataColumn"/>by priority flag
        /// </summary>
        /// <param name="columns">The columns.</param>
        public static void OrderByPriority(this IEnumerable<DataColumn> columns)
        {
            Int32 i = 0;
            var clm = columns.OrderBy<DataColumn, int>(x => x.GetPriority());
            foreach (DataColumn dc in clm)
            {
                // dc.SetPriority(i);
                if (i >= dc.Table.Columns.Count) break;
                dc.SetOrdinal(i);
                i++;
            }
        }

        /// <summary>
        /// Categories the priority.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="default_categoryPriority">The default category priority.</param>
        /// <returns></returns>
        public static List<string> CategoryPriority(this DataTable dc, List<string> default_categoryPriority)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.categoryPriority))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.categoryPriority, default_categoryPriority);
            }
            return dc.ExtendedProperties[templateFieldDataTable.categoryPriority] as List<string>;
        }

        /// <summary>
        /// Gets the category priority.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        public static List<string> GetCategoryPriority(this DataTable dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.categoryPriority))
            {
                var output = new List<string>();

                Type t = dc.GetClassType();
                if (t != null)
                {
                    settingsEntriesForObject seo = new settingsEntriesForObject(t, false);
                    dc.SetCategoryPriority(seo.CategoryByPriority);
                }
                else
                {
                    List<string> cats = new List<string>();
                    foreach (DataColumn d in dc.Columns)
                    {
                        String cat = d.GetGroup();
                        if (cat != null)
                        {
                            cats.AddUnique(cat.ToUpper());
                        }
                    }
                    dc.SetCategoryPriority(cats);
                }
            }

            List<string> dcs = dc.ExtendedProperties[templateFieldDataTable.categoryPriority] as List<string>;
            List<string> dcout = new List<string>();
            dcs.ForEach(x => dcout.AddUnique(x));

            return dcs;
        }

        /// <summary>
        /// Sets the category priority.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="categoryPriority">The category priority.</param>
        /// <returns></returns>
        public static DataTable SetCategoryPriority(this DataTable dc, List<string> categoryPriority)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.categoryPriority, categoryPriority);
            return dc;
        }

        /// <summary>
        /// Defines type that is source of the table shema
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="default_shema_sourceinstance">The default shema sourceinstance.</param>
        /// <returns></returns>
        public static Type ClassType(this DataTable dc, Type default_shema_sourceinstance)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.shema_sourceinstance))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.shema_sourceinstance, default_shema_sourceinstance);
            }
            return dc.ExtendedProperties[templateFieldDataTable.shema_sourceinstance] as Type;
        }

        public static Type GetClassType(this DataTable dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.shema_sourceinstance))
            {
                return default(Type);
            }
            return dc.ExtendedProperties[templateFieldDataTable.shema_sourceinstance] as Type;
        }

        public static DataTable SetClassType(this DataTable dc, Type shema_sourceinstance)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.shema_sourceinstance, shema_sourceinstance);
            return dc;
        }

        /// <summary>
        /// Aggregation Aspect that used to create this table. If its <see cref="imbSCI.Core.math.aggregation.dataPointAggregationAspect.none" /> then this is a source table not derived one
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="default_data_aggregation_type">Default type of the data aggregation.</param>
        /// <returns></returns>
        public static dataPointAggregationAspect AggregationAspect(this DataTable dc, dataPointAggregationAspect default_data_aggregation_type)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.data_aggregation_type))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.data_aggregation_type, default_data_aggregation_type);
            }
            return (dataPointAggregationAspect)dc.ExtendedProperties[templateFieldDataTable.data_aggregation_type];
        }

        public static dataPointAggregationAspect GetAggregationAspect(this DataTable dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.data_aggregation_type))
            {
                dc.ExtendedProperties[templateFieldDataTable.data_aggregation_type] = new dataPointAggregationAspect();
            }
            return (dataPointAggregationAspect)dc.ExtendedProperties[templateFieldDataTable.data_aggregation_type];
        }

        public static DataTable SetAggregationAspect(this DataTable dc, dataPointAggregationAspect data_aggregation_type)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.data_aggregation_type, data_aggregation_type);
            return dc;
        }

        /// <summary>
        /// Aggregations the origin count: if 0 there were no aggregations as source
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="default_data_origin_count">The default data origin count.</param>
        /// <returns></returns>
        public static Int32 AggregationOriginCount(this DataTable dc, Int32 default_data_origin_count)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.data_origin_count))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.data_origin_count, default_data_origin_count);
            }
            return (Int32)dc.ExtendedProperties[templateFieldDataTable.data_origin_count];
        }

        public static Int32 GetAggregationOriginCount(this DataTable dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.data_origin_count))
            {
                return default(Int32);
            }
            return (Int32)dc.ExtendedProperties[templateFieldDataTable.data_origin_count];
        }

        public static DataTable SetAggregationOriginCount(this DataTable dc, Int32 data_origin_count)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.data_origin_count, data_origin_count);
            return dc;
        }

        /// <summary>
        /// Defines the shema for statistics -- horisontal
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="columns">The columns.</param>
        public static void DefineShemaForStatistics(this DataTable dc, PropertyEntryColumn columns = PropertyEntryColumn.entry_name | PropertyEntryColumn.entry_value | PropertyEntryColumn.role_letter | PropertyEntryColumn.entry_description)
        {
            if (columns.HasFlag(PropertyEntryColumn.entry_name)) dc.Columns.Add("dc_name").SetHeading("Measure");
            if (columns.HasFlag(PropertyEntryColumn.role_letter)) dc.Columns.Add("dc_letter").SetHeading(" - ");
            if (columns.HasFlag(PropertyEntryColumn.entry_value)) dc.Columns.Add("dc_value").SetHeading("Value");
            if (columns.HasFlag(PropertyEntryColumn.entry_description)) dc.Columns.Add("dc_description").SetHeading("Description");
        }

        public static void SetEncode(this DataTable dt, toDosCharactersMode mode)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                dc.SetEncodeMode(mode);
            }
        }

        /// <summary>
        /// Adds the new line in extra description set that will be directly appended below the table
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="formulae">The formulae.</param>
        /// <param name="format">The format.</param>
        /// <param name="insertDefinition">It will check measure data defined in the last added row</param>
        public static void AddExtraMath(this DataTable dc, String formulae, String format = "latex", Boolean insertDefinition = false)
        {
            if (insertDefinition)
            {
                String name = dc.Rows[dc.Rows.Count - 1]["dc_name"] as String;
                String letter = dc.Rows[dc.Rows.Count - 1]["dc_letter"] as String;
                //Object value = dc.Rows[dc.Rows.Count - 1]["dc_value"];
                dc.AddExtra("");
                dc.AddExtra("The _" + name.ToLower() + "_ (" + letter + ") is defined as:");
            }
            dc.AddExtra("```" + format);
            dc.AddExtra("");
            dc.AddExtra(formulae);
            dc.AddExtra("");
            dc.AddExtra("```");
            dc.AddExtra("");
            return;
        }

        /// <summary>
        /// Adds the new line in extra description set that will be directly appended below the table
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="newExtraLine">The new extra line.</param>
        public static void AddExtra(this DataTable dc, String newExtraLine)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.table_extraDesc))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.table_extraDesc, new List<string>());
            }
            else if (dc.ExtendedProperties[templateFieldDataTable.table_extraDesc] == null)
            {
                dc.ExtendedProperties[templateFieldDataTable.table_extraDesc] = new List<string>();
            }
            ((List<string>)dc.ExtendedProperties[templateFieldDataTable.table_extraDesc]).Add(newExtraLine);

            return;
        }

        /// <summary>
        /// Extras the lines count.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        public static Int32 ExtraLinesCount(this DataTable dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.table_extraDesc))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.table_extraDesc, new List<string>());
            }
            else if (dc.ExtendedProperties[templateFieldDataTable.table_extraDesc] == null)
            {
                dc.ExtendedProperties[templateFieldDataTable.table_extraDesc] = new List<string>();
            }
            return ((List<string>)dc.ExtendedProperties[templateFieldDataTable.table_extraDesc]).Count();
        }

        /// <summary>
        /// Gets the extra desc.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        public static List<string> GetExtraDesc(this DataTable dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.table_extraDesc))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.table_extraDesc, new List<string>());
            }
            else if (dc.ExtendedProperties[templateFieldDataTable.table_extraDesc] == null)
            {
                dc.ExtendedProperties[templateFieldDataTable.table_extraDesc] = new List<string>();
            }

            return ((List<string>)dc.ExtendedProperties[templateFieldDataTable.table_extraDesc]);
        }

        /// <summary>
        /// Sets the extra desc.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="table_extraDesc">The table extra desc.</param>
        /// <returns></returns>
        public static DataTable SetExtraDesc(this DataTable dc, List<string> table_extraDesc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.table_extraDesc))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.table_extraDesc, new List<string>());
            }
            else if (dc.ExtendedProperties[templateFieldDataTable.table_extraDesc] == null)
            {
                dc.ExtendedProperties[templateFieldDataTable.table_extraDesc] = new List<string>();
            }
            dc.ExtendedProperties.add(templateFieldDataTable.table_extraDesc, table_extraDesc);
            return dc;
        }

        public static PropertyCollectionExtended AdditionalInfo(this DataTable dc, PropertyCollectionExtended default_data_additional)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.data_additional))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.data_additional, default_data_additional);
            }
            return dc.ExtendedProperties[templateFieldDataTable.data_additional] as PropertyCollectionExtended;
        }

        public static PropertyCollectionExtended GetAdditionalInfo(this DataTable dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.data_additional))
            {
                return new PropertyCollectionExtended();
            }
            return dc.ExtendedProperties[templateFieldDataTable.data_additional] as PropertyCollectionExtended;
        }

        public static DataTable SetAdditionalInfoEntry(this DataTable dc, String data_name, Object data_value, String description = "")
        {
            var pce = dc.GetAdditionalInfo();
            pce[data_name] = data_value;
            if (!description.isNullOrEmpty()) pce.entries[PropertyEntryColumn.entry_description].EntryValue = description;

            dc.ExtendedProperties.add(templateFieldDataTable.data_additional, pce);
            return dc;
        }

        /// <summary>
        /// Sets the additional information entries by data object properties, being of <c>propertyTypes</c> type
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="data_name_refix">The data name refix.</param>
        /// <param name="data_value">The data value.</param>
        /// <param name="propertyTypes">The property types.</param>
        /// <returns></returns>
        public static DataTable SetAdditionalInfoEntries(this DataTable dc, String data_name_refix, Object data_value, params Type[] propertyTypes)
        {
            if (data_value == null) return dc;
            if (data_name_refix.isNullOrEmpty()) data_name_refix = "data";
            settingsEntriesForObject sEO = new settingsEntriesForObject(data_value, false, false);
            foreach (var spe in sEO.spes.Values)
            {
                if (propertyTypes.Contains(spe.type))
                {
                    if (spe.value != null)
                    {
                        dc.SetAdditionalInfoEntry(data_name_refix.add(spe.name, "_"), spe.value);
                    }
                }
                //if (containsQueries.Contains(propertyTypes, spe.type))
                //{
                    
                //}
            }
            return dc;
        }

        public static DataTable SetAdditionalInfo(this DataTable dc, PropertyCollectionExtended data_additional)
        {
            if (dc.ExtendedProperties.ContainsKey(templateFieldDataTable.data_additional))
            {
                PropertyCollectionExtended inf = GetAdditionalInfo(dc); // dc.ExtendedProperties[templateFieldDataTable.data_additional];
                if (inf != data_additional)
                {
                    inf.AddRange(data_additional);
                }
            }
            else
            {
                dc.ExtendedProperties.add(templateFieldDataTable.data_additional, data_additional);
            }

            return dc;
        }

        public static String Desc(this DataSet dc, String default_description)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.description))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.description, default_description);
            }
            return dc.ExtendedProperties[templateFieldDataTable.description].toStringSafe();
        }

        public static String GetDesc(this DataSet dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.description))
            {
                return default(String);
            }
            return dc.ExtendedProperties[templateFieldDataTable.description].toStringSafe();
        }

        public static DataSet SetDesc(this DataSet dc, String description)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.description, description);
            return dc;
        }

        public static String Title(this DataSet dc, String default_title)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.title))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.title, default_title);
            }

            return dc.ExtendedProperties[templateFieldDataTable.title].toStringSafe();
        }

        public static String GetTitle(this DataSet dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.title))
            {
                return dc.DataSetName;
            }
            return dc.ExtendedProperties[templateFieldDataTable.title].toStringSafe();
        }

        public static DataSet SetTitle(this DataSet dc, String title)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.title, title);
            dc.DataSetName = title.getFilename();
            return dc;
        }

        /// <summary>
        /// Filenames for table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="proposal">The proposal.</param>
        /// <returns></returns>
        public static String FilenameForDataset(this DataSet table, string proposal = null)
        {
            String fl = "";
            if (proposal.isNullOrEmpty())
            {
                if (table.DataSetName.isNullOrEmpty())
                {
                    if (table.DataSetName.isNullOrEmpty()) fl = "dt_" + table.GetHashCode().ToString();
                }
                else
                {
                    fl = "dt_" + table.DataSetName.getFilename().Replace(" ", "");
                }
            }
            else
            {
                fl = "dt_" + proposal.getFilename().Replace(" ", "");
            }
            return fl;
        }

        /// <summary>
        /// Titles the specified content.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="content">The content.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public static DataTable Title(this DataTable table, String content, existingDataMode policy = existingDataMode.overwriteExisting)
        {
            table.SetTitle(content, policy);
            return table;
        }

        /// <summary>
        /// Descriptions the specified content.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="content">The content.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public static DataTable Description(this DataTable table, String content, existingDataMode policy = existingDataMode.overwriteExisting)
        {
            table.SetDescription(content);
            return table;
        }

        public static String ClassName(this DataTable dc, String default_shema_classname)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.shema_classname))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.shema_classname, default_shema_classname);
            }
            return dc.ExtendedProperties[templateFieldDataTable.shema_classname].toStringSafe();
        }

        public static String GetClassName(this DataTable dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.shema_classname))
            {
                return default(String);
            }
            return dc.ExtendedProperties[templateFieldDataTable.shema_classname].toStringSafe();
        }

        /// <summary>
        /// Sets the name of the object type class associated with the shema
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="shema_classname">The shema classname.</param>
        /// <returns></returns>
        public static DataTable SetClassName(this DataTable dc, String shema_classname)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.shema_classname, shema_classname);
            return dc;
        }

        public static tableStyleRowSetter RowMetaSet(this DataTable dc, tableStyleRowSetter default_table_metarows)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.table_metarows))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.table_metarows, default_table_metarows);
            }
            return dc.ExtendedProperties[templateFieldDataTable.table_metarows] as tableStyleRowSetter;
        }

        public static tableStyleRowSetter GetRowMetaSet(this DataTable dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.table_metarows))
            {
                dc.ExtendedProperties[templateFieldDataTable.table_metarows] = new tableStyleRowSetter();
            }
            return dc.ExtendedProperties[templateFieldDataTable.table_metarows] as tableStyleRowSetter;
        }

        public static DataTable SetRowMetaSet(this DataTable dc, tableStyleRowSetter table_metarows)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.table_metarows, table_metarows);
            return dc;
        }

        public static tableStyleColumnSetter ColumnMetaSet(this DataTable dc, tableStyleColumnSetter default_table_metacolumns)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.table_metacolumns))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.table_metacolumns, default_table_metacolumns);
            }
            return dc.ExtendedProperties[templateFieldDataTable.table_metacolumns] as tableStyleColumnSetter;
        }

        public static tableStyleColumnSetter GetColumnMetaSet(this DataTable dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.table_metacolumns))
            {
                dc.ExtendedProperties[templateFieldDataTable.table_metacolumns] = new tableStyleColumnSetter();
            }
            return dc.ExtendedProperties[templateFieldDataTable.table_metacolumns] as tableStyleColumnSetter;
        }

        public static DataTable SetColumnMetaSet(this DataTable dc, tableStyleColumnSetter table_metacolumns)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.table_metacolumns, table_metacolumns);
            return dc;
        }

        public static dataTableStyleSet StyleSet(this DataTable dc, dataTableStyleSet default_table_styleset)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.table_styleset))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.table_styleset, default_table_styleset);
            }
            return dc.ExtendedProperties[templateFieldDataTable.table_styleset] as dataTableStyleSet;
        }

        /// <summary>
        /// Gets table style set
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        public static dataTableStyleSet GetStyleSet(this DataTable dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.table_styleset))
            {
                dc.ExtendedProperties[templateFieldDataTable.table_styleset] = new dataTableStyleSet();
            }
            return dc.ExtendedProperties[templateFieldDataTable.table_styleset] as dataTableStyleSet;
        }

        public static DataTable SetStyleSet(this DataTable dc, dataTableStyleSet table_styleset)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.table_styleset, table_styleset);
            return dc;
        }

        /// <summary>
        /// Sets the description into <see cref="DataTable.ExtendedProperties"/> under <see cref="imbSCI.Data.enums.fields.templateFieldDataTable.data_tabledesc"/>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="content">Description for the <c>table</c>.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public static DataTable SetDescription(this DataTable table, String content, existingDataMode policy = existingDataMode.overwriteExisting)
        {
            table.ExtendedProperties.Append(templateFieldDataTable.data_tabledesc, content, policy);
            return table;
        }

        /// <summary>
        /// Gets the description from <see cref="DataTable.ExtendedProperties"/> specified under <see cref="imbSCI.Data.enums.fields.templateFieldDataTable.data_tabledesc"/>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>Empty string if no entry found</returns>
        public static String GetDescription(this DataTable table, String descIfNotDefined = "")
        {
            return table.ExtendedProperties.getProperString(descIfNotDefined, templateFieldDataTable.data_tabledesc);
        }

        /// <summary>
        /// Sets the rendering purpose title for the <c>table</c> via <see cref="imbSCI.Data.enums.fields.templateFieldDataTable.data_tablename"/> entry in the <see cref="DataTable.ExtendedProperties"/>, without changing the <see cref="DataTable.TableName"/>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="content">The content.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public static DataTable SetTitle(this DataTable table, String content, existingDataMode policy = existingDataMode.overwriteExisting)
        {
            if (table.TableName.isNullOrEmpty())
            {
                table.TableName = content.getFilename();
            }

            if (table.TableName.isNullOrEmpty()) table.TableName = "Untitled";

            table.ExtendedProperties.Append(templateFieldDataTable.data_tablename, content, policy);
            return table;
        }

        public const String TABLE_DEFAULTNAME = "Untitled";

        /// <summary>
        /// Gets the title defined under <see cref="imbSCI.Data.enums.fields.templateFieldDataTable"/> or <see cref="DataTable.TableName"/> if no title entry was found inside the <see cref="DataTable.ExtendedProperties"/>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static String GetTitle(this DataTable table, String titleIfNotDefined = TABLE_DEFAULTNAME, Boolean permanent = false)
        {
            if (table.ExtendedProperties.ContainsKey(templateFieldDataTable.data_tablename))
            {
                return table.ExtendedProperties.getProperString(titleIfNotDefined, templateFieldDataTable.data_tablename);
            }

            if (titleIfNotDefined == TABLE_DEFAULTNAME)
            {
                if (!table.TableName.isNullOrEmpty())
                {
                    titleIfNotDefined = table.TableName;
                }
                else
                {
                    if (permanent)
                    {
                        table.TableName = titleIfNotDefined;
                    }
                }
            }
            return table.ExtendedProperties.getProperString(titleIfNotDefined, templateFieldDataTable.data_tablename);
        }

        /// <summary>
        /// Sets the color role
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="color">The color.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public static Boolean SetColor(this DataTable table, acePaletteRole color, existingDataMode policy = existingDataMode.overwriteExisting)
        {
            return table.ExtendedProperties.Append(templateFieldStyling.color_paletteRole, color, policy);
        }

        /// <summary>
        /// Gets the color role associated to <c>table</c> via <see cref="imbSCI.Data.enums.fields.templateFieldStyling.color_paletteRole" />
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>
        /// Color role
        /// </returns>
        public static acePaletteRole GetColor(this DataTable table)
        {
            return table.ExtendedProperties.getProperEnum<acePaletteRole>(acePaletteRole.colorDefault, templateFieldStyling.color_paletteRole);
        }
    }
}