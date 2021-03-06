// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataTableDataOperations.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.math;
    using imbSCI.Core.math.aggregation;
    using imbSCI.Core.reporting;
    using imbSCI.Data;
    using imbSCI.Data.collection.nested;
    using imbSCI.Data.enums.fields;
    using imbSCI.DataComplex.tables;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Operations over data contained in single or multiple datatables
    /// </summary>
    public static class dataTableDataOperations
    {
        /// <summary>
        /// Filenames for table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="proposal">The proposal.</param>
        /// <returns></returns>
        public static string FilenameForTable(this DataTable table, string proposal = null)
        {
            string fl = "";
            if (proposal.isNullOrEmpty())
            {
                if (table.TableName.isNullOrEmpty())
                {
                    fl = "dt_" + table.GetHashCode().ToString();
                }
                else
                {
                    fl = "dt_" + table.TableName.getFilename().Replace(" ", "");
                }
            }
            else
            {
                if (!proposal.StartsWith(DataTableForStatisticsExtension.PREFIX_CLEANDATATABLE))
                {
                    proposal = DataTableForStatisticsExtension.PREFIX_REPORTDATATABLE + proposal; //.getFilename().Replace(" ", "");
                }
                else
                {
                    proposal = proposal.getFilename().Replace(" ", "");
                }
                fl = proposal.getFilename().Replace(" ", "");
            }
            return fl;
        }

        /// <summary>
        /// Clears the data (sets the default value) from columns named in the argument
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnsToClear">The columns to clear.</param>
        public static void ClearData(this DataTable table, params string[] columnsToClear)
        {
            List<DataColumn> columns = new List<DataColumn>();

            foreach (string colName in columnsToClear)
            {
                if (table.Columns.Contains(colName))
                {
                    columns.Add(table.Columns[colName]);
                }
            }

            foreach (DataRow dr in table.Rows)
            {
                foreach (DataColumn dc in columns)
                {
                    dr[dc] = dc.DataType.GetDefaultValue();
                }
            }
        }

        /*
        public static String GetExpression(this IEnumerable<DataColumn> sourceColumns, dataPointAggregationType aggregation)
        {
            switch (aggregation)
            {
                case dataPointAggregationType.avg:
                    break;

                case dataPointAggregationType.count:
                    break;

                case dataPointAggregationType.entropy:
                    break;

                case dataPointAggregationType.max:
                    break;

                case dataPointAggregationType.min:
                    break;

                case dataPointAggregationType.range:
                    break;

                case dataPointAggregationType.sum:
                    break;

                case dataPointAggregationType.var:
                    break;

                case dataPointAggregationType.stdev:
                    break;

                case dataPointAggregationType.firstEntry:
                    break;

                case dataPointAggregationType.lastEntry:
                    break;
            }
        }*/

        /// <summary>
        /// Adds the table into dataset and performs auto rename if the table already exists there
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static DataSet AddTable(this DataSet dataset, DataTable table)
        {
            if (dataset != null)
            {
                int c = 0;
                string tn = table.TableName;
                string tnn = tn;
                while (dataset.Tables.Contains(tn))
                {
                    tn = tnn + c.ToString("D2");
                    c++;
                    if (c > 100)
                    {
                        tn = tnn.addTimeStamp();
                        break;
                    }
                }
                table.TableName = tn;
                dataset.Tables.Add(table);
            }
            return dataset;
        }

        public static List<templateFieldDataTable> GetMetaDataFieldsExistingInAllColumns(this DataTable table)
        {
            List<templateFieldDataTable> output = new List<templateFieldDataTable>();
            foreach (templateFieldDataTable field in Enum.GetValues(typeof(templateFieldDataTable)))
            {
                output.Add(field);
            }

            Dictionary<templateFieldDataTable, Object> values = new Dictionary<templateFieldDataTable, object>();

            List<templateFieldDataTable> haveDifferentValues = new List<templateFieldDataTable>();

            foreach (DataColumn dc in table.Columns)
            {
                List<templateFieldDataTable> newOutput = output.ToList();

                foreach (var field in output)
                {
                    if (!dc.ExtendedProperties.ContainsKey(field))
                    {
                        newOutput.Remove(field);
                    }
                    else
                    {
                        if (values.ContainsKey(field))
                        {
                            if (dc.ExtendedProperties[field] != values[field])
                            {
                                if (!haveDifferentValues.Contains(field)) haveDifferentValues.Add(field);
                            }
                        }
                        else
                        {
                            values.Add(field, dc.ExtendedProperties[field]);
                        }
                    }
                }
                output = newOutput;

            }


            List<templateFieldDataTable> final = new List<templateFieldDataTable>();

            foreach (var f in output)
            {
                if (haveDifferentValues.Contains(f)) final.Add(f);
            }


            return final;
        }



        /// <summary>
        /// Creates Legend for a data table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public static DataTable GetLegendDataTable(this DataTable table, List<templateFieldDataTable> columns = null)
        {
            if (columns == null)
            {
                columns = new List<templateFieldDataTable>();

                columns.AddUnique(templateFieldDataTable.col_caption);
                columns.AddUnique(templateFieldDataTable.col_desc);
                var cln = table.GetMetaDataFieldsExistingInAllColumns();

                foreach (var c in cln)
                {
                    columns.AddUnique(c);
                }


            }

            DataTable output = new DataTable();
            output.SetTitle(table.GetTitle() + " Legend");

            output.SetDescription("Description of " + table.GetTitle() + "columns");

            foreach (var c in columns)
            {
                DataColumn dc = output.Columns.Add(c.ToString(), typeof(String));

                String cn = c.ToString();
                switch (c)
                {

                    case templateFieldDataTable.col_caption:
                        dc.SetHeading("Name");
                        break;
                    case templateFieldDataTable.col_desc:
                        dc.SetHeading("Description");
                        break;
                    case templateFieldDataTable.col_letter:
                        dc.SetHeading("Letter");
                        break;
                    case templateFieldDataTable.col_unit:
                        dc.SetHeading("Unit");
                        break;
                    default:
                        dc.SetHeading(cn.removeStartsWith("col_").imbTitleCamelOperation(true));
                        break;
                }
            }

            /*
            DataRow hdr = output.NewRow();

            foreach (var c in columns)
            {
                String cn = c.ToString();
                switch (c)
                {

                    case templateFieldDataTable.col_caption:
                        hdr[cn] = "Name"; // source_dc.GetHeading();
                        break;
                    case templateFieldDataTable.col_desc:
                        hdr[cn] = "Description"; // source_dc.GetDesc();
                        break;
                    case templateFieldDataTable.col_letter:
                        hdr[cn] = "Code"; // source_dc.GetLetter();
                        break;
                    case templateFieldDataTable.col_unit:
                        hdr[cn] = "Unit"; // source_dc.GetUnit();
                        break;
                }
            }
            */
            //  output.Rows.Add(hdr);


            /*
            DataColumn column_name = output.Columns.Add(nameof(templateFieldDataTable.col_caption), typeof(String));

            DataColumn column_description = output.Columns.Add(nameof(templateFieldDataTable.col_desc), typeof(String));

            DataColumn column_unit = output.Columns.Add(nameof(templateFieldDataTable.col_unit), typeof(String));

            DataColumn column_letter = output.Columns.Add(nameof(templateFieldDataTable.col_letter), typeof(String));
            */

            foreach (DataColumn source_dc in table.Columns)
            {
                DataRow dr = output.NewRow();

                foreach (var c in columns)
                {
                    String cn = c.ToString();
                    switch (c)
                    {

                        case templateFieldDataTable.col_caption:
                            dr[cn] = source_dc.GetHeading();
                            break;
                        case templateFieldDataTable.col_desc:
                            dr[cn] = source_dc.GetDesc();
                            break;
                        case templateFieldDataTable.col_letter:
                            dr[cn] = source_dc.GetLetter();
                            break;
                        case templateFieldDataTable.col_unit:
                            dr[cn] = source_dc.GetUnit();
                            break;
                    }
                }

                output.Rows.Add(dr);

            }

            return output;

        }

        /// <summary>
        /// Gets a table version, without rows having default field value for all columns, except ones specified in <c>columnsToIgnore</c>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnsToIgnore">The columns to ignore.</param>
        /// <returns></returns>
        public static DataTable GetTableVersionWithoutEmptyRows(this DataTable table, List<String> columnsToIgnore, Boolean ignoreStringColumns = false)
        {
            DataTable output = table.GetClonedShema<DataTable>();
            Dictionary<String, Object> defaultValues = new Dictionary<String, Object>();
            if (columnsToIgnore == null) columnsToIgnore = new List<string>();
            foreach (DataColumn dc in table.Columns)
            {
                Type dcvt = dc.GetValueType();
                defaultValues.Add(dc.ColumnName, dcvt.GetDefaultValue());
                if (dcvt == typeof(String)) columnsToIgnore.Add(dc.ColumnName);

            }


            foreach (DataRow dr in table.Rows)
            {
                Dictionary<String, Object> rowValues = new Dictionary<String, Object>();
                Boolean copyRow = false;
                foreach (DataColumn dc in table.Columns)
                {
                    if (!columnsToIgnore.Contains(dc.ColumnName))
                    {
                        Object vl = dr[dc.ColumnName];
                        if (vl != defaultValues[dc.ColumnName])
                        {
                            copyRow = true;
                            break;

                        }
                    }
                    else
                    {

                    }

                }
                if (copyRow)
                {
                    var ndr = output.NewRow();

                    foreach (DataColumn dc in table.Columns)
                    {
                        ndr[dc.ColumnName] = dr[dc.ColumnName];
                    }


                    output.Rows.Add(ndr);
                }
            }
            return output;
        }


        public static DataTable GetTableVersionWithoutEmptyColumns(this DataTable table, List<String> columnsToAddAnyway, Boolean addStringColumnsAnyway = false)
        {

            List<DataColumn> ColumnsToAdd = new List<DataColumn>();
            if (columnsToAddAnyway == null) columnsToAddAnyway = new List<string>();
            //  foreach (DataColumn dc in table.Columns) ColumnsToRemove.Add(dc);


            foreach (DataColumn dc in table.Columns)
            {
                if (addStringColumnsAnyway)
                {
                    if (dc.GetValueType() == typeof(String))
                    {
                        ColumnsToAdd.Add(dc);
                        continue;
                    }
                }

                if (columnsToAddAnyway.Contains(dc.ColumnName))
                {
                    ColumnsToAdd.Add(dc);
                }
            }

            foreach (DataColumn dc in table.Columns)
            {
                if (ColumnsToAdd.Contains(dc)) continue;
                Object defaultValue = dc.GetValueType().GetDefaultValue();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow nr = table.Rows[i];
                    if (nr[dc] != defaultValue)
                    {
                        ColumnsToAdd.Add(dc);
                    }
                    break;
                }
            }


            DataTable output = table.GetSubColumnTable(-1, ColumnsToAdd.Select(x => x.ColumnName).ToArray());
            return output;
        }


        /// <summary>
        /// Gets table with limited number of rows and with selected column groups. Params alias for <see cref="GetSubColumnTable(DataTable, int, string[])"/>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="rowsLimit">The rows limit. -1 to disable row limit.</param>
        /// <returns></returns>
        public static DataTable GetSubColumnTableByParams(this DataTable table, Int32 rowsLimit, params String[] columnGroups)
        {
            return GetSubColumnTable(table, rowsLimit, columnGroups);
        }

        /// <summary>
        /// Gets table with limited number of rows and with selected column groups
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="rowsLimit">The rows limit. -1 to disable row limit.</param>
        /// <returns></returns>
        public static DataTable GetSubColumnTable(this DataTable table, Int32 rowsLimit, String[] columnGroups)
        {
            DataTable output = table.GetClonedShema<DataTable>(true, columnGroups);
            if (rowsLimit < 1) rowsLimit = table.Rows.Count;
            int rc = Math.Min(table.Rows.Count, rowsLimit);
            // IEnumerable<DataRow> rows = table.AsEnumerable().Take(rowsLimit);
            output.SetTitle(table.GetTitle().add("[" + rc.ToString() + "]", " "));
            output.SetDescription(table.GetDescription().add("Excerpt with [" + rc.ToString() + "] rows from of the original table with [" + table.Rows.Count + ".", " "));
            // output.SetAggregationAspect(dataPointAggregationAspect.subSetOfRows);

            int c = 0;
            for (int i = 0; i < rc; i++)
            {
                DataRow nr = output.NewRow();

                foreach (DataColumn dc in output.Columns)
                {
                    nr[dc] = table.Rows[i][dc.ColumnName];
                }

                output.Rows.Add(nr);
            }

            return output;
        }

        /// <summary>
        /// Gets the limited.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="rowsLimit">The rows limit.</param>
        /// <returns></returns>
        public static DataTable GetLimited(this DataTable table, int rowsLimit)
        {
            DataTable output = table.GetClonedShema<DataTable>();

            int rc = Math.Min(table.Rows.Count, rowsLimit);
            // IEnumerable<DataRow> rows = table.AsEnumerable().Take(rowsLimit);
            output.SetTitle(table.GetTitle().add("[" + rc.ToString() + "]", " "));
            output.SetDescription(table.GetDescription().add("Excerpt with [" + rc.ToString() + "] rows from of the original table with [" + table.Rows.Count + ".", " "));
            output.SetAggregationAspect(dataPointAggregationAspect.subSetOfRows);

            int c = 0;
            for (int i = 0; i < rc; i++)
            {
                DataRow nr = output.NewRow();

                foreach (DataColumn dc in output.Columns)
                {
                    nr[dc] = table.Rows[i][dc.ColumnName];
                }

                output.Rows.Add(nr);
            }

            return output;
        }

        /// <summary>
        /// Copies the rows from <c>source</c> into <c>table</c>
        /// </summary>
        /// <param name="table">The table to copy into.</param>
        /// <param name="source">The source data table.</param>
        /// <param name="rowsSkip">Rows to skip.</param>
        /// <param name="rowsLimit">Rows limit</param>
        public static void CopyRowsFrom(this DataTable table, DataTable source, int rowsSkip = 0, int rowsLimit = -1)
        {
            if (rowsLimit == -1) rowsLimit = source.Rows.Count;

            rowsLimit = Math.Min(rowsLimit, source.Rows.Count);

            //Int32 cc = Math.Min(table.Columns.Count, columnLimit);

            // IEnumerable<DataRow> rows = table.AsEnumerable().Take(rowsLimit);

            int c = 0;
            for (int i = 0; i < rowsLimit; i++)
            {
                if (i >= rowsSkip)
                {
                    DataRow sr = source.Rows[i];

                    if (sr.RowState != DataRowState.Deleted)
                    {
                        DataRow nr = table.NewRow();

                        foreach (DataColumn dc in source.Columns)
                        {
                            if (table.Columns.Contains(dc.ColumnName))
                            {
                                nr[dc.ColumnName] = sr[dc.ColumnName];
                            }
                        }

                        table.Rows.Add(nr);
                    }
                }
            }
        }

        /// <summary>
        /// Merges all table rows into single table with all rows
        /// </summary>
        /// <param name="tables">The tables.</param>
        /// <param name="tablename">The tablename.</param>
        /// <returns></returns>
        public static DataTable GetRowCollectionTable(this IEnumerable<DataTable> tables, string tablename = "summary")
        {
            DataTable shemaProvider = null;
            if (!tables.Any()) return new DataTable("empty");

            shemaProvider = tables.First();
            DataTable output = shemaProvider.GetClonedShema<DataTable>();
            output.TableName = tablename;
            foreach (DataTable dt in tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow o_dr = null;
                    o_dr = output.NewRow();
                    foreach (DataColumn dc in output.Columns)
                    {
                        if (dt.Columns.Contains(dc.ColumnName)) o_dr[dc.ColumnName] = dr[dc.ColumnName];
                    }
                    output.Rows.Add(o_dr);
                }
            }
            return output;
        }

        /// <summary>
        /// Gets the sum data set.
        /// </summary>
        /// <param name="datasets">The datasets.</param>
        /// <param name="dataSetName">Name of the data set.</param>
        /// <returns></returns>
        public static DataSet GetSumDataSet(this IEnumerable<DataSet> datasets, string dataSetName = "dataset")
        {
            DataSet dss = null;

            aceDictionarySet<string, DataTable> tableCumul = new aceDictionarySet<string, DataTable>();

            List<DataTable> fradt = new List<DataTable>();
            foreach (DataSet ds in datasets)
            {
                if (dss == null)
                {
                    dss = ds;
                }

                foreach (DataTable tb in ds.Tables)
                {
                    tableCumul.Add(tb.TableName, tb);
                }
            }

            DataSet fin = new DataSet(dataSetName);

            foreach (DataTable tb in dss.Tables)
            {
                fin.AddTable(GetAggregatedTable(tableCumul[tb.TableName], "SumOf_" + tb.TableName, dataPointAggregationAspect.overlapMultiTable)); //GetSumTable(dataSetName + "_" + tb.TableName + "_sum_" + dss.Tables.Count));
            }

            return fin;
        }

        /// <summary>
        /// Cleans the meta.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static DataTable CleanMeta(this DataTable table)
        {
            foreach (DataColumn dc in table.Columns)
            {
                dc.ExtendedProperties.Clear();
            }
            table.ExtendedProperties.Clear();
            return table;
        }

        /// <summary>
        /// Gets the aggregated table> where values are computed as agregation from multi source tables. 
        /// </summary>
        /// <param name="tables">The tables.</param>
        /// <param name="tablename">The tablename.</param>
        /// <param name="aspect">The aspect.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static DataTable GetAggregatedTable(this IEnumerable<DataTable> tables, string tablename = "summary", dataPointAggregationAspect aspect = dataPointAggregationAspect.overlapMultiTable, ILogBuilder logger = null)
        {
            if (tables == null)
            {
                if (logger != null) logger.log("GetAggregatedTable --> received no tables!!!");
                return new DataTable(tablename);
            }
            if (!tables.Any())
            {
                if (logger != null) logger.log("GetAggregatedTable --> received no tables!!!");
                return new DataTable(tablename);
            }

            DataTableAggregationDefinition definition = new DataTableAggregationDefinition(tables, dataPointAggregationAspect.overlapMultiTable);
            DataTable output = definition.getShema(tablename);

            for (int i = 0; i < definition.rowsMax; i++)
            {
                DataRow o_dr = output.Rows[i];
                foreach (var pair in definition.shemaColumns)
                {
                    double min = 0;
                    double max = 0;
                    List<double> i_data = new List<double>();

                    foreach (DataTable dt in tables)
                    {
                        if (i < dt.Rows.Count)
                        {
                            DataRow i_dr = dt.Rows[i];
                            i_data.Add(i_dr[pair.Key].imbConvertValueSafeTyped<double>());
                        }
                    }

                    foreach (var colDef in pair.Value)
                    {
                        double outValue = 0;

                        switch (colDef.aggregation)
                        {
                            case dataPointAggregationType.avg:
                                outValue = i_data.Average();
                                break;

                            case dataPointAggregationType.clear:
                                outValue = 0;
                                break;

                            case dataPointAggregationType.count:
                                outValue = i_data.Count();
                                break;

                            case dataPointAggregationType.max:
                                outValue = i_data.Max();
                                break;

                            case dataPointAggregationType.min:
                                outValue = i_data.Min();
                                break;

                            case dataPointAggregationType.range:
                                min = i_data.Min();
                                max = i_data.Max();
                                outValue = max - min;
                                break;

                            case dataPointAggregationType.sum:
                                outValue = i_data.Sum();
                                break;

                            case dataPointAggregationType.var:
                                outValue = i_data.GetVariance();
                                break;

                            case dataPointAggregationType.stdev:
                                outValue = i_data.GetStdDeviation();
                                break;

                            case dataPointAggregationType.firstEntry:
                                outValue = i_data.First();
                                break;

                            case dataPointAggregationType.lastEntry:
                                outValue = i_data.Last();
                                break;

                            case dataPointAggregationType.hidden:
                            case dataPointAggregationType.none:
                                break;
                        }

                        o_dr[colDef.columnName] = outValue.imbConvertValueSafe(colDef.columnValueType);
                    }

                    o_dr.AcceptChanges();
                    //output.Rows.Add(o_dr);
                }
            }

            return output;
        }

        public static DataTableAggregationDefinition GetAggregatedTableDescription(this IEnumerable<DataTable> tables, dataPointAggregationAspect aspect = dataPointAggregationAspect.overlapMultiTable)
        {
            return new DataTableAggregationDefinition(tables, aspect);
        }

        /// <summary>
        /// Creates summary table by summing all matching cells of <see cref="Int32"/>  and <see cref="Double"/>
        /// </summary>
        /// <param name="tables">The tables.</param>
        /// <returns></returns>
        public static DataTable GetSumTable(this IEnumerable<DataTable> tables, string tablename = "summary")
        {
            DataTable shemaProvider = null;
            if (!tables.Any()) return new DataTable("empty");

            shemaProvider = tables.First();
            DataTable output = shemaProvider.GetClonedShema<DataTable>();
            //output.TableName = tablename;
            //output.SetTitle(tablename);

            // DataTableAggregationDefinition definition = new DataTableAggregationDefinition(tables, dataPointAggregationAspect.overlapMultiTable);

            foreach (DataTable dt in tables)
            {
                int r = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow o_dr = null;
                    bool nrow = false;
                    if (output.Rows.Count <= r)
                    {
                        o_dr = output.NewRow();
                        foreach (DataColumn dc in output.Columns)
                        {
                            if (Enumerable.Contains(dc.Table.PrimaryKey, dc))
                            {
                                if (dc.DataType == typeof(int))
                                {
                                    o_dr[dc.ColumnName] = output.Rows.Count;
                                }
                                else if (dc.DataType == typeof(string))
                                {
                                    o_dr[dc.ColumnName] = output.Rows.Count.ToString("D5");
                                }
                                // ignored since it is primary key
                            }
                            else
                            {
                                o_dr[dc.ColumnName] = dc.GetValueType().GetDefaultValue();
                            }
                        }
                        nrow = true;
                    }
                    else
                    {
                        o_dr = output.Rows[r];
                    }

                    foreach (DataColumn dc in output.Columns)
                    {
                        if (Enumerable.Contains(dc.Table.PrimaryKey, dc))
                        {
                            // ignored since it is primary key
                        }
                        else
                        {
                            if (dt.Columns.Contains(dc.ColumnName))
                            {
                                object o_val = o_dr[dc.ColumnName];
                                object i_val = dr[dc.ColumnName];

                                if (o_val is DBNull)
                                {
                                    o_val = dc.GetValueType().GetDefaultValue();
                                }

                                if (i_val is DBNull)
                                {
                                    i_val = dc.GetValueType().GetDefaultValue();
                                }

                                if (dc.GetValueType() == typeof(int))
                                {
                                    int o_val_Int32 = Convert.ToInt32(o_val);
                                    int i_val_Int32 = Convert.ToInt32(i_val);
                                    o_val_Int32 = o_val_Int32 + i_val_Int32;
                                    o_val = o_val_Int32;
                                    o_dr[dc.ColumnName] = o_val_Int32;
                                }
                                else if (dc.GetValueType() == typeof(double))
                                {
                                    double o_val_Double = Convert.ToDouble(o_val);
                                    double i_val_Double = Convert.ToDouble(i_val);
                                    o_val_Double = o_val_Double + i_val_Double;
                                    o_val = o_val_Double;

                                    o_dr[dc.ColumnName] = o_val_Double;
                                }
                            }
                        }
                    }

                    if (nrow) output.Rows.Add(o_dr);
                    r++;
                }
            }

            return output;
        }
    }
}