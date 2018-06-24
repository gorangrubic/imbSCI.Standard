// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTableBuilders.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.extensions.data.schema
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.collection;
    using imbSCI.Core.data;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.reporting;
    using imbSCI.Data;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.exceptions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    public static class DataTableBuilders
    {
        [Flags]
        public enum buildDataTableOptions
        {
            none = 0,
            doCreate = 1,
            doInsertItem = 2,
            doOnlyWithDisplayName = 4,
            doInherited = 8,
            doInsertAutocountColumn = 16,
            doInsertItemTitleColumn = 32,

            doInsertLetterColumn = 64,
            doInsertUnitColumn = 128,

            doConsolidateColumns = 256
        }

        /// <summary>
        /// Builds the vertical table with all data
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static DataTable buildDataTableVerticalSummaryTable(this object host, globalMeasureUnitDictionary.globalTableEnum tableName)
        {
            PropertyCollectionExtended pce = host.buildPCE(false, null);

            return pce.buildDataTableVertical(tableName.GetTableName(), tableName.GetTableDescription());
        }

        /// <summary>
        /// Builds the horizontal data table
        /// </summary>
        /// <param name="firstItem">The first item.</param>
        /// <param name="__dataTable">The data table.</param>
        /// <param name="doInsertItem">if set to <c>true</c> [do insert item].</param>
        /// <param name="doOnlyWithDisplayName">if set to <c>true</c> [do only with display name].</param>
        /// <param name="doInherited">if set to <c>true</c> [do inherited].</param>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="fieldsOrCategoriesToShow">The fields or categories to show.</param>
        /// <returns></returns>
        public static DataTable buildDataTable(this object firstItem, string __dataTable, bool doInsertItem, bool doOnlyWithDisplayName, bool doInherited, PropertyCollectionExtended dictionary, params string[] fieldsOrCategoriesToShow)
        {
            buildDataTableOptions flags = buildDataTableOptions.doCreate;
            if (doInherited) flags |= buildDataTableOptions.doInherited;
            if (doOnlyWithDisplayName) flags |= buildDataTableOptions.doOnlyWithDisplayName;
            if (doInsertItem) flags |= buildDataTableOptions.doInsertItem;

            return buildDataTable(firstItem, __dataTable, flags, dictionary, fieldsOrCategoriesToShow);
        }

        /// <summary>
        /// Adds the data table row.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="item">The item.</param>
        /// <param name="options">The options.</param>
        public static void AddDataTableRow(this DataTable table, object item, buildDataTableOptions options)
        {
            DataRow dr = table.NewRow();

            foreach (DataColumn dc in table.Columns)
            {
                object val = dc.DataType.GetDefaultValue();
                if (!dc.AutoIncrement)
                {
                    if (dc.ColumnName == nameof(globalMeasureUnitDictionary.globalMeasureEnum.itemTitle))
                    {
                        if (item is IObjectWithName)
                        {
                            IObjectWithName item_IObjectWithName = (IObjectWithName)item;
                            val = item_IObjectWithName;
                        }
                        else if (item is string)
                        {
                            val = item;
                        }
                        else if (item is KeyValuePair<string, int>)
                        {
                            var pair = (KeyValuePair<string, int>)item;
                            val = pair.Key;
                        }
                        else
                        {
                            val = item.GetType().Name;
                        }
                    }

                    var pi = item.GetType().GetProperty(dc.ColumnName);

                    val = pi.GetValue(dc.ColumnName, null); // item.GetPropertyValue(dc.ColumnName);

                    PropertyEntry pe = dc.ExtendedProperties.getProperObject<PropertyEntry>(templateFieldDataTable.col_pe);
                    if (pe != null)
                    {
                        val = PropertyDataStructureTools.getColumnValue(pe, item, table.Rows.Count, "D3");
                    }
                    //String format = dc.ExtendedProperties.get(templateFieldDataTable.col_format, "").toStringSafe();
                    //if (format != "") val = val.
                    dr[dc] = val;
                }
            }
            table.Rows.Add(dr);
        }

        /// <summary>
        /// 2017:: Builds horizontal data table with columns mapping properties of the type.
        /// </summary>
        /// <remarks>
        /// <para>If fieldsCategoriesToShow[] parameters array is empty - filters are not applied</para>
        /// </remarks>
        /// <param name="firstItem">Type or Instance to be used for column estraction.</param>
        /// <param name="__dataTable">Name of data table. If empty or null = type name is used.</param>
        /// <param name="doInsertItem">If TRUE and first parametar was Instance - it will be transfered as the first row in the DataTable</param>
        /// <param name="doOnlyWithDisplayName">Only properties with DisplayName attribute. Value of attribute will be mapped to Caption of column</param>
        /// <param name="doInherited">Should inherited properties be included? FALSE to get only properties declared at class of the object</param>
        /// <param name="fieldsOrCategoriesToShow">Category or property name to include in DataTable. If its empty it will ignore this criteria. Entries ending with _ are prefix definisions</param>
        /// <returns>DataTable object with proper DataColumn shema made from Primitive, Enum and ToStrings</returns>
        public static DataTable buildDataTable(this object firstItem, string __dataTable, buildDataTableOptions options, PropertyCollectionExtended dictionary, string[] fieldsOrCategoriesToShow = null, ILogBuilder logger = null)
        {
            bool doInsertItem = options.HasFlag(buildDataTableOptions.doInsertItem);
            bool doOnlyWithDisplayName = options.HasFlag(buildDataTableOptions.doOnlyWithDisplayName);
            bool doInherited = options.HasFlag(buildDataTableOptions.doInherited);
            bool doInsertAutocountColumn = options.HasFlag(buildDataTableOptions.doInsertAutocountColumn);

            DataTable output = new DataTable(__dataTable);

            if (firstItem == null)
            {
                throw new dataException("firstItem is null -- can't buildDataTable!");
                return output;
            }

            if (dictionary != null) dictionary = globalMeasureUnitDictionary.units;

            BindingFlags flags = BindingFlags.Instance;
            string prefix = "";

            List<string> filters = fieldsOrCategoriesToShow.getFlatList<string>();
            if (filters.Any(x => x.EndsWith("_")))
            {
                prefix = filters.First(x => x.EndsWith("_"));
                filters.Remove(prefix);
            }

            if (!doInherited)
            {
                flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
            }
            else
            {
                flags = BindingFlags.Public | BindingFlags.Instance;
            }

            Type itemType = firstItem as Type;
            if (itemType == null) itemType = firstItem.GetType();

            if (imbSciStringExtensions.isNullOrEmptyString(__dataTable))
            {
                __dataTable = itemType.Name;
            }

            output.TableName = __dataTable;

            List<DataColumn> columnList = new List<DataColumn>();

            if (options.HasFlag(buildDataTableOptions.doInsertAutocountColumn))
            {
                DataColumn dc = new DataColumn("#", typeof(int));
                dc.AutoIncrement = true;
                dc.AutoIncrementSeed = 1;
                columnList.Add(dc);
            }

            if (options.HasFlag(buildDataTableOptions.doInsertItemTitleColumn))
            {
                DataColumn dc = new DataColumn(globalMeasureUnitDictionary.globalMeasureEnum.itemTitle.ToString(), typeof(string));

                columnList.Add(dc);
            }

            PropertyInfo[] propList = itemType.GetProperties(flags);

            foreach (PropertyInfo pi in propList)
            {
                PropertyCollection extraData = new PropertyCollection();
                imbAttributeCollection imb = imbAttributeTools.getImbAttributeDictionary(pi);

                object[] atts = pi.GetCustomAttributes(true);

                List<string> tokenList = new List<string>();
                tokenList.Add(pi.Name);
                tokenList.Add(pi.Name.ToLower());
                bool pass = false;

                settingsMemberInfoEntry me = new settingsMemberInfoEntry(pi, dictionary);
                tokenList.AddSeveral(me.displayName, me.categoryName);

                if (doOnlyWithDisplayName)
                {
                    pass = (tokenList.Count() > 2);
                }
                else
                {
                    pass = true;
                }

                if (pass)
                {
                    if (filters.Any())
                    {
                        pass = filters.Any(x => tokenList.Contains(x, StringComparer.CurrentCultureIgnoreCase));
                    }
                }

                if (pass)
                {
                    if (pi.PropertyType.isSimpleInputEnough() || pi.PropertyType.isEnum())
                    {
                    }
                    else
                    {
                        //Type IInterface = pi.PropertyType.GetInterface(nameof(IGetToSetFromString));
                        //if (IInterface == null)
                        //{
                        //    pass = false;
                        //}
                        //else
                        //{
                        //    pass = true;
                        //}
                    }
                }
                if (pass)
                {
                    string fieldName = prefix.add(pi.Name, "_");
                    string format = imb.getMessage(imbAttributeName.reporting_valueformat, "");

                    DataColumn dc = new DataColumn(fieldName, pi.PropertyType, "", MappingType.Attribute);

                    if (tokenList.Count > 2)
                    {
                        dc.Caption = tokenList[2];
                    }
                    else
                    {
                        dc.Caption = pi.Name.imbTitleCamelOperation(true);
                    }
                    dc.DataType = pi.PropertyType;

                    // extraData[name_tokens] = tokenList;

                    // extraData[templateFieldInfo] = firstItem;

                    extraData[imbAttributeName.reporting_valueformat] = format; //*
                    extraData[imbAttributeName.measure_displayGroup] = imb.getProperString(me.categoryName, imbAttributeName.measure_displayGroup, imbAttributeName.menuGroupPath); //*
                    extraData[imbAttributeName.measure_important] = imb.getMessage(imbAttributeName.measure_important, dataPointImportance.normal); //*
                    extraData[templateFieldDataTable.col_type] = pi.PropertyType;
                    extraData[templateFieldDataTable.col_directAppend] = imb.ContainsKey(imbAttributeName.reporting_escapeoff);
                    extraData[templateFieldDataTable.col_attributes] = imb;
                    extraData[templateFieldDataTable.col_propertyInfo] = pi;
                    extraData[templateFieldDataTable.col_desc] = imb.getStringLine(Environment.NewLine, imbAttributeName.menuHelp, imbAttributeName.help, imbAttributeName.helpDescription,
                        imbAttributeName.helpPurpose);
                    extraData[templateFieldDataTable.col_format] = format; //*

                    dc.ExtendedProperties.copyInto(extraData);

                    if (me.pe != null)
                    {
                        dc.ExtendedProperties.copyInto(me.pe);
                        dc.ExtendedProperties[templateFieldDataTable.col_pe] = me.pe;
                        dc.ColumnName = me.pe[PropertyEntryColumn.entry_key].toStringSafe();
                    }
                    if (output.Columns.Contains(dc.ColumnName))
                    {
                        if (logger != null) logger.log("ColumnName [" + dc.ColumnName + "] already defined in table [" + output.TableName + "]");
                    }
                    else
                    {
                        output.Columns.Add(dc);
                    }
                }
            }

            //
            if (doInsertItem)
            {
                if (firstItem is Type)
                {
                    // -- it was type
                }
                else
                {
                    output.AddObject(firstItem);
                }
            }

            output.ExtendedProperties[templateFieldDataTable.shema_sourcename] = "";
            output.ExtendedProperties[templateFieldDataTable.shema_sourceinstance] = "runtime type";
            output.ExtendedProperties[templateFieldDataTable.shema_classname] = firstItem.GetType().Name;

            return output;
        }

        /// <summary>
        /// Inserts new DataRow into table, based on input object and existing DataColumn shema of the table
        /// </summary>
        /// <param name="table">DataTable with proper DataColumn shema</param>
        /// <param name="input">Object to extract data from</param>
        public static void AddObject(this DataTable table, object input)
        {
            List<object> rowData = new List<object>();
            DataRow dr = table.NewRow();
            foreach (DataColumn dc in table.Columns)
            {
                dr[dc] = input.imbGetPropertySafe(dc.ColumnName, dc.DataType.GetDefaultValue());
            }
            table.Rows.Add(dr);
        }

        /// <summary>
        /// 2017::Builds data table out of collection. Supported properties: primitives, enums and IGetFromToString interface types
        /// </summary>
        /// <remarks>
        /// <para>If fieldsCategoriesToShow[] parameters array is empty - filters are not applied</para>
        /// </remarks>
        /// <param name="firstItem">Type or Instance to be used for column estraction.</param>
        /// <param name="__dataTable">Name of data table. If empty or null = type name is used.</param>
        /// <param name="doOnlyWithDisplayName">Only properties with DisplayName attribute. Value of attribute will be mapped to Caption of column</param>
        /// <param name="doInherited">Should inherited properties be included? FALSE to get only properties declared at class of the object</param>
        /// <param name="fieldsOrCategoriesToShow">Category or property name to include in DataTable. If its empty it will ignore this criteria</param>
        /// <returns>DataTable object with proper DataColumn shema made from Primitive, Enum and ToStrings</returns>
        public static DataTable buildDataTable(this IEnumerable items, string __dataTable, bool doOnlyWithDisplayName, bool doInherited, PropertyCollectionExtended dictionary, params string[] fieldsOrCategoriesToShow)
        {
            DataTable output = null; //new DataTable(__dataTable);

            foreach (object item in items)
            {
                if (output == null)
                {
                    output = item.buildDataTable(__dataTable, false, doOnlyWithDisplayName, doInherited, dictionary, fieldsOrCategoriesToShow);
                }

                output.AddObject(item);
            }

            return output;
        }
    }
}