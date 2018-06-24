// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyDataStructureTools.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.extensions.data
{
    using imbSCI.Core.collection;
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.extensions.data.modify;
    using imbSCI.DataComplex.extensions.data.operations;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    //using aceCommonType.data;

    public static class PropertyDataStructureTools
    {
        public const string getColumnValue_Default = "--";

        /// <summary>
        /// Builds the data table hor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="extraColumn">The extra column.</param>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public static DataTable BuildDataTableHor<T>(this T items, PropertyEntryColumn extraColumn, params string[] columns) where T : IEnumerable, IObjectWithNameAndDescription
        {
            if (items is IObjectWithNameAndDescription)
            {
                IObjectWithNameAndDescription items_IObjectWithNameAndDescription = (IObjectWithNameAndDescription)items;

                return items.BuildDataTableHorizontal(items_IObjectWithNameAndDescription.name, items_IObjectWithNameAndDescription.description, extraColumn, columns);
            }

            return items.BuildDataTableHorizontal("Instance list", "", extraColumn, columns);
        }

        public const string KEYCOLUMN = "Entry";
        public const string IDCOLUMN = "_i_";

        /// <summary>
        /// Builds the data table hor dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemKeys">The item keys.</param>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public static DataTable BuildDataTableHorDictionary<TKey, T>(this IDictionary<TKey, T> itemKeys, string valuePath, string KeyCaption, PropertyEntryColumn extraColumn, string[] columns)
        {
            object source = null;
            TKey firstKey;
            foreach (var pair in itemKeys)
            {
                firstKey = pair.Key;
                source = pair.Value.imbGetPropertySafe(valuePath);
                break;
            }

            var seo = new settingsEntriesForObject(source);
            DataTable dt = new DataTable(typeof(T).Name);

            DataColumn dcKey = dt.Columns.Add(KEYCOLUMN);
            dcKey.Caption = KeyCaption;

            source.BuildDataShema(columns, extraColumn, seo, dt);

            List<Enum>[] output = convertPEColumn(extraColumn);

            if (extraColumn != PropertyEntryColumn.none)
            {
                foreach (PropertyEntryColumn extra in output[0])
                {
                    dt.AddExtraRow(extra);
                }
            }

            int c = 0;
            foreach (var pair in itemKeys)
            {
                object it = pair.Value.imbGetPropertySafe(valuePath);
                string key = pair.Key.toStringSafe();

                DataRow dr = dt.SetDataRow(it, extraColumn, seo);
                dr[dcKey] = key;
                c++;
            }

            if (extraColumn != PropertyEntryColumn.none)
            {
                foreach (PropertyEntryColumn extra in output[1])
                {
                    dt.AddExtraRow(extra);
                }
            }

            //dt.TableName = title;
            //dt.ExtendedProperties[templateFieldDataTable.data_tabledesc] = description;
            //dt.ExtendedProperties[templateFieldDataTable.data_tablename] = title;
            dt.ExtendedProperties[templateFieldDataTable.data_tablescount] = c;

            return dt;
        }

        public static DataTable BuildDataTableHor<T>(this T items, params string[] columns) where T : IEnumerable, IObjectWithNameAndDescription
        {
            PropertyEntryColumn extraColumn = PropertyEntryColumn.none;

            if (items is IObjectWithNameAndDescription)
            {
                IObjectWithNameAndDescription items_IObjectWithNameAndDescription = (IObjectWithNameAndDescription)items;

                return items.BuildDataTableHorizontal(items_IObjectWithNameAndDescription.name, items_IObjectWithNameAndDescription.description, extraColumn, columns);
            }

            return items.BuildDataTableHorizontal("Instance list", "", extraColumn, columns);
        }

        /// <summary>
        /// Builds the data table using reflection and selected columns. Supports formating instruction, expression and other advanced atributes
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public static DataTable BuildDataTableHorizontal(this IEnumerable items, string title, string description, PropertyEntryColumn extraColumn, string[] columns)
        {
            object source = null;
            foreach (object it in items)
            {
                source = it;
                break;
            }
            if (source == null) return null;

            var seo = new settingsEntriesForObject(source);
            DataTable dt = source.BuildDataShema(columns, extraColumn, seo);

            List<Enum>[] output = convertPEColumn(extraColumn);

            if (extraColumn != PropertyEntryColumn.none)
            {
                foreach (PropertyEntryColumn extra in output[0])
                {
                    dt.AddExtraRow(extra);
                }
            }

            int c = 0;
            foreach (object it in items)
            {
                dt.SetDataRow(it, extraColumn, seo);
                c++;
            }

            if (extraColumn != PropertyEntryColumn.none)
            {
                foreach (PropertyEntryColumn extra in output[1])
                {
                    dt.AddExtraRow(extra);
                }
            }

            dt.TableName = title;
            dt.ExtendedProperties[templateFieldDataTable.data_tabledesc] = description;
            dt.ExtendedProperties[templateFieldDataTable.data_tablename] = title;
            dt.ExtendedProperties[templateFieldDataTable.data_tablescount] = c;

            return dt;
        }

        /// <summary>
        /// Converts the pe column.
        /// </summary>
        /// <param name="extras">The extras.</param>
        /// <returns>[0] extra rows before main paty, [1] extra rows after</returns>
        public static List<Enum>[] convertPEColumn(this PropertyEntryColumn extras)
        {
            List<PropertyEntryColumn> extraList = extras.getEnumListFromFlags<PropertyEntryColumn>();
            List<Enum>[] output = { new List<Enum>(), new List<Enum>() };

            foreach (PropertyEntryColumn pec in extraList)
            {
                switch (pec)
                {
                    case PropertyEntryColumn.none:
                        break;

                    case PropertyEntryColumn.role_name:
                    case PropertyEntryColumn.entry_unit:
                    case PropertyEntryColumn.entry_importance:
                    case PropertyEntryColumn.entry_description:
                    case PropertyEntryColumn.property_description:
                        output[1].Add(pec);
                        break;

                    default:
                    case PropertyEntryColumn.entry_key:
                    case PropertyEntryColumn.entry_name:
                    case PropertyEntryColumn.role_letter:
                    case PropertyEntryColumn.role_symbol:
                        output[0].Add(pec);
                        break;
                }
            }

            return output;
        }

        /// <summary>
        /// Builds the data table.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public static DataTable BuildDataShema(this object source, string[] columns, PropertyEntryColumn extraColumn, settingsEntriesForObject seo = null, DataTable dt = null)
        {
            if (seo == null) seo = new settingsEntriesForObject(source);

            if (dt == null) dt = new DataTable(source.GetType().Name);
            dt.ExtendedProperties[templateFieldDataTable.shema_sourceinstance] = source;
            dt.ExtendedProperties[templateFieldDataTable.shema_classname] = source.GetType().Name;

            dt.ExtendedProperties[templateFieldDataTable.shema_dictionary] = source.buildPCE(true, null);

            if (extraColumn.HasFlag(PropertyEntryColumn.autocount_idcolumn))
            {
                dt.Columns.Add(IDCOLUMN).AutoIncrement = true;
            }

            foreach (string col in columns)
            {
                dt.AddToShema(col, source, seo);
            }

            return dt;
        }

        private static List<string> _columnsToSkip;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<string> columnsToSkip
        {
            get
            {
                if (_columnsToSkip == null)
                {
                    _columnsToSkip = new List<string>();
                    _columnsToSkip.Add(KEYCOLUMN);
                    _columnsToSkip.Add(IDCOLUMN);
                }
                return _columnsToSkip;
            }
        }

        public static void NormalizeColumnNames(this DataTable dt)
        {
            foreach (DataColumn cl in dt.Columns)
            {
                cl.ColumnName = cl.ColumnName.Replace(" ", "_");
            }
        }

        public static void NormalizeTableAndColumnNames(this DataSet ds)
        {
            foreach (DataTable ct in ds.Tables)
            {
                foreach (DataColumn cl in ct.Columns)
                {
                    cl.ColumnName = cl.ColumnName.Replace(" ", "_");
                }
            }
        }

        public static DataRow SetDataRow(this DataTable dt, object source, PropertyEntryColumn extraColumn, settingsEntriesForObject seo = null)
        {
            if (seo == null) seo = new settingsEntriesForObject(source);

            DataRow dr = dt.NewRow();
            foreach (DataColumn col in dt.Columns)
            {
                if (columnsToSkip.Contains(col.ColumnName))
                {
                    continue;
                }
                object val = null; //  source.GetPropertyValue(col.ColumnName);
                throw new NotImplementedException();

                if (val is IFormattable)
                {
                    IFormattable valf = (IFormattable)val;
                    if (col.ExtendedProperties.ContainsKey(templateFieldDataTable.col_format))
                    {
                        string format = col.ExtendedProperties[templateFieldDataTable.col_format].toStringSafe();
                        if (col.AutoIncrement)
                        {
                        }
                        else
                        {
                            val = valf.ToString(format, null);
                        }
                    }
                }

                if (col.ExtendedProperties.ContainsKey(templateFieldDataTable.col_expression))
                {
                    col.Expression = col.ExtendedProperties[templateFieldDataTable.col_expression].toStringSafe();
                }

                dr[col] = val;
            }
            dt.Rows.Add(dr);
            return dr;
        }

        public static DataTable StartShema(this IDataRowSetter source)
        {
            DataTable dt = new DataTable(source.GetType().Name);
            dt.ExtendedProperties[templateFieldDataTable.shema_sourceinstance] = source;
            dt.ExtendedProperties[templateFieldDataTable.shema_classname] = source.GetType().Name;

            return dt;
        }

        /// <summary>
        /// Adds column using object <c>source</c> reflection information
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="source">The source.</param>
        /// <param name="seo">The seo.</param>
        /// <returns></returns>
        public static DataColumn AddToShema(this DataTable dt, string propertyName, object source = null, settingsEntriesForObject seo = null)
        {
            if (source == null) source = dt.ExtendedProperties[templateFieldDataTable.shema_sourceinstance] as IDataRowSetter;
            var dc = dt.Columns.Add(propertyName, typeof(string));
            if (seo == null) seo = new settingsEntriesForObject(source);
            if (!seo.spes.ContainsKey(propertyName))
            {
                dc.Caption = propertyName;
            }
            else
            {
                settingsMemberInfoEntry smi = seo.spes[propertyName];
                dc.Caption = smi.displayName;

                // smi.exportPropertyCollection(dc.ExtendedProperties);
            }

            return dc;
        }

        public static DataColumn set(this DataColumn dc, object key, object value)
        {
            dc.ExtendedProperties[key] = value;
            return dc;
        }

        ///// <summary>
        ///// Builds the category property list via measures.
        ///// </summary>
        ///// <param name="source">The source.</param>
        ///// <param name="name">The name.</param>
        ///// <param name="description">The description.</param>
        ///// <returns></returns>
        //public static PropertyCollectionExtendedList buildCategoryPropertyListViaMeasures(this Type source, String name, String description)
        //{
        //    String __name = source.getAttributeValueOrDefault<DisplayNameAttribute>(name, false);
        //    String __description = source.getAttributeValueOrDefault<DisplayNameAttribute>(description, false);
        //    measureSetExternal setExternal = new measureSetExternal(source, __name, __description);

        //    return setExternal.export();
        //}

        /// <summary>
        /// Builds vertical data table - aka: comparative table
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public static DataTable buildComparativeDataTable(this IEnumerable<PropertyCollectionExtended> items, string itemNameProperty, string name, string description, PropertyEntryDictionary columns = null)
        {
            DataTable output = items.buildDataTableHorizontal(name, description, columns);

            output = output.GetInversedDataTable(itemNameProperty);

            return output;
        }

        /// <summary>
        /// Builds the data table - from all entries inside the collection
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <returns>DataTable ready for output</returns>
        public static DataTable buildDataTableHorizontal(this IEnumerable<PropertyCollectionExtended> items, string name, string description, PropertyEntryDictionary columns = null)
        {
            DataTable output = new DataTable();

            if (columns == null)
            {
                // columns = buildConsolidatedColumnDictionary(PropertyEntryColumn.entry_name | PropertyEntryColumn.entry_value | PropertyEntryColumn.entry_description);
                columns = items.buildConsolidatedColumnDictionary(PropertyEntryColumn.entry_name | PropertyEntryColumn.entry_value | PropertyEntryColumn.entry_description);
            }

            output.TableName = name;
            output.ExtendedProperties[templateFieldDataTable.data_tablename] = name;
            output.ExtendedProperties[templateFieldDataTable.data_tabledesc] = description;
            output.ExtendedProperties[templateFieldDataTable.data_rowcounttotal] = items.Count();

            output.ExtendedProperties[templateFieldDataTable.shema_sourceinstance] = columns.GetType().Name;

            //columns.Values.ToList()

            foreach (KeyValuePair<object, PropertyEntry> pair in columns)
            {
                DataColumn dc = output.Columns.Add();
                dc.ColumnName = pair.Value.keyName;

                dc.Caption = pair.Value[PropertyEntryColumn.entry_name].toStringSafe();
                dc.ColumnMapping = MappingType.Element;
                dc.ExtendedProperties.AddRange(pair.Value);
            }

            string autocount_format = items.Count().getToStringDFormat(2);

            int c = 1;
            if (items == null)
            {
                return output;
            }
            else
            {
            }
            foreach (PropertyCollectionExtended pce in items)
            {
                DataRow dr = output.NewRow();
                object vl = "--";

                foreach (KeyValuePair<object, PropertyEntry> pair in columns)
                {
                    dr[(string)pair.Value.keyName] = getColumnValue(pair.Value, pce, c, autocount_format);
                }

                output.Rows.Add(dr);
                c++;
            }

            return output;
        }

        /// <summary>
        /// Adds the table extended rows.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="columns">The columns.</param>
        /// <exception cref="System.ArgumentNullException">columns - Column shema never supplied -- and the table has no shema attached on templateFieldDataTable.shema_dictionary</exception>
        public static void addTableExtendedRows(this DataTable table, IEnumerable<PropertyCollection> rows, PropertyEntryDictionary columns = null)
        {
            if (columns == null)
            {
                if (table.ExtendedProperties.ContainsKey(templateFieldDataTable.shema_dictionary))
                {
                    columns = (PropertyEntryDictionary)table.ExtendedProperties[templateFieldDataTable.shema_dictionary];
                }
            }

            //String autocount_format = "D3";

            //if (table.ExtendedProperties.ContainsKey(templateFieldDataTable.count_format))
            //{
            //    autocount_format = (String)table.ExtendedProperties[templateFieldDataTable.count_format];
            //}
            //autocount_format = rows.Count().getToStringDFormat(3);

            if (columns == null) throw new ArgumentNullException("columns", "Column shema never supplied -- and the table has no shema attached on templateFieldDataTable.shema_dictionary");
            if (rows == null)
            {
                return;
            }
            int c = 1;
            foreach (PropertyCollectionExtended pce in rows)
            {
                DataRow dr = table.NewRow();
                object vl = getColumnValue_Default;

                foreach (KeyValuePair<object, PropertyEntry> pair in columns)
                {
                    dr[(string)pair.Value.keyName] = getColumnValue(pair.Value, pce, c, "D4");
                }

                table.Rows.Add(dr);
                c++;
            }
        }

        /// <summary>
        /// Builds vertical datatable
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="__title">The title.</param>
        /// <param name="__description">The description.</param>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        public static DataTable buildDataTableVertical(this PropertyEntryDictionary columns, string __title, string __description = "", IEnumerable<PropertyCollection> rows = null)
        {
            var table = new DataTable();
            table.TableName = __title;

            //if (rows != null)
            //{
            //    autocount_format = rows.Count().getToStringDFormat(3);
            //}

            table.ExtendedProperties[templateFieldDataTable.data_tablename] = __title;
            table.ExtendedProperties[templateFieldDataTable.data_tabledesc] = __description;

            table.ExtendedProperties[templateFieldDataTable.shema_dictionary] = columns;

            //columns.Add(PropertyEntryColumn.autocount_idcolumn, new PropertyEntry(PropertyEntryColumn.autocount_idcolumn, 1, "ID", "Row number"));

            foreach (KeyValuePair<object, PropertyEntry> pair in columns)
            {
                DataColumn dc = table.Columns.Add();
                dc.ColumnName = pair.Value.keyName;
                //if (PropertyEntryColumn.autocount_idcolumn.ToString() == pair.Key.ToString())
                //{
                //    dc.AutoIncrementSeed = 1;
                //    dc.AutoIncrement = true;
                //    dc.AutoIncrementStep = 1;
                //    dc.DataType = typeof(Int32);
                //}

                dc.Caption = pair.Value[PropertyEntryColumn.entry_name].toStringSafe();
                dc.ColumnMapping = MappingType.Element;
                dc.ExtendedProperties.AddRange(pair.Value);
            }

            table.addTableExtendedRows(rows, columns);
            return table;
        }

        /// <summary>
        /// Builds the data table.
        /// </summary>
        /// <param name="shema">The shema.</param>
        /// <param name="__title">The title.</param>
        /// <param name="__description">The description.</param>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        public static DataTable buildDataTableVertical(this PropertyCollectionExtended shema, string __title, string __description = "", IEnumerable<PropertyCollection> rows = null)
        {
            PropertyEntryDictionary columns = buildColumnDictionary(shema, PropertyEntryColumn.entry_name | PropertyEntryColumn.entry_value | PropertyEntryColumn.entry_description);
            return columns.buildDataTableVertical(__title, __description, rows);
        }

        /// <summary>
        /// Gets the right column value
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="pce">The pce.</param>
        /// <param name="c">Row count - required if <see cref="PropertyEntryColumn.autocount_idcolumn"/> is the <c>column</c>.</param>
        /// <param name="autocount_format">The autocount format.</param>
        /// <returns></returns>
        public static object getColumnValue(this PropertyEntry column, object instance, int c, string autocount_format)
        {
            settingsEntriesForObject sef = new settingsEntriesForObject(instance, false);

            object vl = getColumnValue_Default;
            object key = column[PropertyEntryColumn.entry_key];

            if (key is PropertyEntryColumn)
            {
                PropertyEntryColumn pec = (PropertyEntryColumn)key;
                switch (pec)
                {
                    case PropertyEntryColumn.entry_name:
                        vl = sef.spes[column.keyName].displayName;
                        break;

                    case PropertyEntryColumn.entry_description:
                        vl = sef.spes[column.keyName].description;
                        break;

                    case PropertyEntryColumn.autocount_idcolumn:

                        vl = c.ToString(autocount_format);
                        break;

                    default:

                        //vl = column.getColumn(pec, [key]);
                        break;
                }
            }
            else
            {
                vl = instance.GetType().GetProperty(column.keyName).GetValue(instance, null);
                // vl = instance.GetPropertyValue(column.keyName);

                if (sef.spes[column.keyName].type == typeof(int) || sef.spes[column.keyName].type == typeof(double))
                {
                    vl = string.Format(sef.spes[column.keyName].format, vl);
                    //vl = String.Format(, )
                }
                else
                {
                }
            }
            return vl;
        }

        /// <summary>
        /// Gets the right column value
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="pce">The pce.</param>
        /// <param name="c">Row count - required if <see cref="imbSCI.Core.collection.PropertyEntryColumn.autocount_idcolumn"/> is the <c>column</c>.</param>
        /// <param name="autocount_format">The autocount format.</param>
        /// <returns></returns>
        public static object getColumnValue(this PropertyEntry column, PropertyCollectionExtended pce, int c, string autocount_format)
        {
            object vl = getColumnValue_Default;
            object key = column[PropertyEntryColumn.entry_key];

            if (key is PropertyEntryColumn)
            {
                PropertyEntryColumn pec = (PropertyEntryColumn)key;
                switch (pec)
                {
                    case PropertyEntryColumn.entry_name:
                        vl = pce.name;
                        break;

                    case PropertyEntryColumn.entry_description:
                        vl = pce.description;
                        break;

                    case PropertyEntryColumn.autocount_idcolumn:

                        vl = c.ToString(autocount_format);
                        break;

                    default:
                        vl = column.getColumn(pec, pce[key]);
                        break;
                }
            }
            else
            {
                vl = pce[key];
            }
            return vl;
        }

        /// <summary>
        /// Builds the consolidated column dictionary scanning all <see cref="imbSCI.Core.collection.PropertyCollectionExtended"/> items.
        /// </summary>
        /// <remarks>
        /// <para>If <c>__allowedColumns</c> are not specified it will include any newly column found inside collection</para>
        /// </remarks>
        /// <param name="obligatoryColumns">The obligatory columns.</param>
        /// <param name="__allowedColumns">The allowed columns.</param>
        /// <returns>Set of columns ready to be used for DataTable creation and for similar tasks</returns>
        public static PropertyEntryDictionary buildConsolidatedColumnDictionary(this IEnumerable<PropertyCollectionExtended> items, PropertyEntryColumn obligatoryColumns, object[] __allowedColumns = null)
        {
            List<PropertyEntryColumn> columnList = obligatoryColumns.getEnumListFromFlags<PropertyEntryColumn>();

            List<object> allowedColumns = __allowedColumns.getFlatList<object>();

            PropertyEntryDictionary output = new PropertyEntryDictionary();

            foreach (PropertyCollectionExtended pair in items)
            {
                foreach (object key in pair.Keys)
                {
                    if (!output.ContainsKey(key))
                    {
                        bool allowed = !allowedColumns.Any();
                        if (allowedColumns.Contains(key))
                        {
                            allowed = true;
                        }
                        if (allowed)
                        {
                            output.Add(key, pair.entries[key]);
                        }
                    }
                }
            }

            foreach (PropertyEntryColumn c in columnList)
            {
                if (!output.ContainsKey(c))
                {
                    PropertyEntry pe = new PropertyEntry(c, "", c.toColumnCaption(false), c.getDescriptionForKey());
                    output.Add(c, pe);
                }
            }

            //pel.Sort((x, y) => x.priority.CompareTo(y.priority));

            return output;
        }

        /// <summary>
        /// Builds the consolidated column dictionary scanning all <see cref="imbSCI.Core.collection.PropertyCollectionExtended"/> items.
        /// </summary>
        /// <remarks>
        /// <para>If <c>__allowedColumns</c> are not specified it will include any newly column found inside collection</para>
        /// </remarks>
        /// <param name="obligatoryColumns">The obligatory columns.</param>
        /// <param name="__allowedColumns">The allowed columns.</param>
        /// <returns>Set of columns ready to be used for DataTable creation and for similar tasks</returns>
        public static PropertyEntryDictionary buildColumnDictionary(this PropertyCollectionExtended source, PropertyEntryColumn obligatoryColumns, params object[] __allowedColumns)
        {
            List<PropertyEntryColumn> columnList = obligatoryColumns.getEnumListFromFlags<PropertyEntryColumn>();

            List<object> allowedColumns = __allowedColumns.getFlatList<object>();

            PropertyEntryDictionary output = new PropertyEntryDictionary();

            foreach (object key in source.Keys)
            {
                if (!output.ContainsKey(key))
                {
                    bool allowed = !allowedColumns.Any();
                    if (allowedColumns.Contains(key))
                    {
                        allowed = true;
                    }
                    if (allowed)
                    {
                        output.Add(key, source.entries[key]);
                    }
                }
            }

            foreach (PropertyEntryColumn c in columnList)
            {
                if (!output.ContainsKey(c))
                {
                    PropertyEntry pe = new PropertyEntry(c, "", c.toColumnCaption(false), c.getDescriptionForKey());
                    output.Add(c, pe);
                }
            }

            //pel.Sort((x, y) => x.priority.CompareTo(y.priority));

            return output;
        }
    }
}