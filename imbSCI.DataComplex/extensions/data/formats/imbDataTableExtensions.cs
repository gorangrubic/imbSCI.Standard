// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbDataTableExtensions.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.extensions.data.formats
{
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.files;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.lowLevelApi;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.enums.reporting;
    using imbSCI.Data.enums.tableReporting;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.exceptions;

    // using Newtonsoft.Json;
    using imbSCI.DataComplex.extensions.data.operations;
    using imbSCI.DataComplex.extensions.data.schema;
    using imbSCI.DataComplex.tables;
    using OfficeOpenXml;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Extensions for DataTable creation and data manipulation
    /// </summary>
    /// \ingroup_disabled ace_ext_datastructs
    public static class imbDataTableExtensions
    {
        /// <summary>
        /// Adds data from the <c>list</c> into new or existing column with specified <c>columnName</c>. Updates existing rows or creates new rows if required
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="list">The list.</param>
        /// <param name="columnName">Name of the column - it will automatically give name if not specified</param>
        /// <param name="skipRows">Number of rows to skip before start with data insertation</param>
        /// <param name="description">The description of the column</param>
        /// <param name="letter">The letter or code associated with the column</param>
        /// <param name="displayName">The display name for the column</param>
        /// <returns>Number of newly created rows</returns>
        public static int AddListToColumn(this DataTable table, IEnumerable list, string columnName = "", int skipRows = 0, string description = "", string letter = "", string displayName = "")
        {
            Type objType = null;
            Type columnType = typeof(string);
            bool hasAny = false;
            bool typeIsVariant = false;
            DataColumn column = null;
            int count = 0;

            if (columnName.isNullOrEmpty())
            {
                columnName = "Column" + table.Columns.Count.ToString();
            }
            if (displayName.isNullOrEmpty())
            {
                displayName = column.ColumnName.imbTitleCamelOperation(true);
            }

            List<object> o_list = new List<object>();
            foreach (object obj in list)
            {
                hasAny = true;
                if (objType == null)
                {
                    objType = obj.GetType();
                }

                if (typeIsVariant == false)
                {
                    if (obj.GetType() != objType)
                    {
                        typeIsVariant = true;
                    }
                }
                count++;
                o_list.Add(obj);
            }

            if (!hasAny) return 0;

            if (!typeIsVariant)
            {
                columnType = objType;
            }

            if (table.Columns.Contains(columnName))
            {
                column = table.Columns[columnName];
            }
            else
            {
                column = table.Add(columnName, description, letter, columnType);
            }

            column.SetHeading(displayName);
            column.SetDesc(description);
            column.SetLetter(letter);

            int existingRows = Math.Min(table.Rows.Count - skipRows, count);

            int newRows = count - existingRows;

            for (int i = 0; i < existingRows; i++)
            {
                table.Rows[i + skipRows][column] = o_list[i];
            }

            for (int i = 0; i < newRows; i++)
            {
                DataRow dr = table.NewRow();

                dr[column] = o_list[existingRows + i + skipRows];

                table.Rows.Add(dr);
            }
            return newRows;
        }

        public static dataTableExportEnum checkFormatByFilename(this dataTableExportEnum format, string filename)
        {
            if ((format == dataTableExportEnum.none) || (46 == (int)format))
            {
                if (filename.EndsWith("json", StringComparison.Ordinal)) format = dataTableExportEnum.json;
                if (filename.EndsWith("csv", StringComparison.Ordinal)) format = dataTableExportEnum.csv;
                if (filename.EndsWith("excell", StringComparison.Ordinal)) format = dataTableExportEnum.excel;
                if (filename.EndsWith("excel", StringComparison.Ordinal)) format = dataTableExportEnum.excel;
                if (filename.EndsWith("xls", StringComparison.Ordinal)) format = dataTableExportEnum.excel;
                if (filename.EndsWith("xlsx", StringComparison.Ordinal)) format = dataTableExportEnum.excel;
                if (filename.EndsWith("md", StringComparison.Ordinal)) format = dataTableExportEnum.markdown;
                if (filename.EndsWith("xml", StringComparison.Ordinal)) format = dataTableExportEnum.xml;
            }
            return format;
        }

        /// <summary>
        /// Gets the format by filename extension.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
        public static dataTableExportEnum getExportFormatByExtension(this string filepath)
        {
            string filename = Path.GetFileName(filepath);
            dataTableExportEnum format = dataTableExportEnum.csv;

            if (filename.EndsWith("json", StringComparison.Ordinal)) format = dataTableExportEnum.json;
            if (filename.EndsWith("csv", StringComparison.Ordinal)) format = dataTableExportEnum.csv;
            if (filename.EndsWith("excell", StringComparison.Ordinal)) format = dataTableExportEnum.excel;
            if (filename.EndsWith("excel", StringComparison.Ordinal)) format = dataTableExportEnum.excel;
            if (filename.EndsWith("xls", StringComparison.Ordinal)) format = dataTableExportEnum.excel;
            if (filename.EndsWith("xlsx", StringComparison.Ordinal)) format = dataTableExportEnum.excel;
            if (filename.EndsWith("md", StringComparison.Ordinal)) format = dataTableExportEnum.markdown;
            if (filename.EndsWith("xml", StringComparison.Ordinal)) format = dataTableExportEnum.xml;

            return format;
        }

        public static List<DataColumn> toList(this DataColumnCollection columns)
        {
            List<DataColumn> cols = new List<DataColumn>();
            foreach (DataColumn dc in columns)
            {
                cols.Add(dc);
            }
            return cols;
        }

        /// <summary>
        /// Deserializes a excel file to the DataSet.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="target">The target.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public static DataSet deserializeExcelFileToDataSet(this string path, DataSet target, ILogBuilder log, dataTableIOFlags IOFlags = dataTableIOFlags.defaultFlags)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            if (target == null) target = new DataSet(name);
            FileInfo fi = new FileInfo(path);

            using (ExcelPackage pck = new ExcelPackage(fi))
            {
                DataSet dts = pck.ToDataSet(false);
                dts.NormalizeTableAndColumnNames();
                int c = 0;
                int r = 0;
                foreach (DataTable dt in dts.Tables)
                {
                    string nm = Path.GetFileNameWithoutExtension(fi.Name);
                    if (c > 0)
                    {
                        nm = nm + c.ToString("D3");
                    }
                    dt.TableName = nm;
                    target.Tables.Add(dt.Copy());
                    r = r + dt.Rows.Count;
                    if (log != null) log.log("> Imported DataTable [" + nm + "] with [" + dt.Rows.Count + "] rows");
                    c++;
                }
                if (log != null) log.log("Total [" + r + "] rows imported from [" + fi.Name + "] in [" + target.Tables.Count + "] data tables");
            }

            return target;
        }

        /// <summary>
        /// Deserializes the folder excel files to data set.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="target">The target.</param>
        /// <param name="log">The log.</param>
        /// <param name="filenamePattern">The filename pattern.</param>
        /// <returns></returns>
        public static DataSet deserializeFolderExcelFilesToDataSet(this DirectoryInfo directory, DataSet target, ILogBuilder log, string filenamePattern = "*.xlsx")
        {
            if (target == null) target = new DataSet(directory.Name);
            foreach (FileInfo fi in directory.GetFiles(filenamePattern))
            {
                if (log != null) log.log("Importing external DataTable from [" + fi.Name + "]");
                using (ExcelPackage pck = new ExcelPackage(fi))
                {
                    DataSet dts = pck.ToDataSet(false);
                    dts.NormalizeTableAndColumnNames();

                    int c = 0;
                    int r = 0;
                    foreach (DataTable dt in dts.Tables)
                    {
                        string nm = Path.GetFileNameWithoutExtension(fi.Name);
                        if (c > 0)
                        {
                            nm = nm + c.ToString("D3");
                        }
                        dt.TableName = nm;
                        target.Tables.Add(dt.Copy());
                        r = r + dt.Rows.Count;
                        if (log != null) log.log("> Imported DataTable [" + nm + "] with [" + dt.Rows.Count + "] rows");
                        c++;
                    }
                    if (log != null) log.log("Total [" + r + "] rows imported from [" + fi.Name + "] in [" + target.Tables.Count + "] data tables");
                }
            }
            return target;
        }

        /// <summary>
        /// Extracting Input Output flags from array of resources
        /// </summary>
        /// <param name="resources">The resources.</param>
        /// <returns>Detected flags</returns>
        private static dataTableIOFlags GetIOFlags(this object[] resources)
        {
            dataTableIOFlags IOFlags = dataTableIOFlags.defaultFlags;

            if (resources != null)
            {
                foreach (var r in resources)
                {
                    if (r != null)
                    {
                        if (r is dataTableIOFlags rflag)
                        {
                            IOFlags = rflag;
                            break;
                        }
                    }
                }
            }

            return IOFlags;
        }

        /// <summary>
        /// Deserializes the data table.
        /// </summary>
        /// <param name="filename">The filename or filepath.</param>
        /// <param name="format">The format to read from</param>
        /// <param name="directory">The directory (if filename was supplied and not full filepath)</param>
        /// <param name="table">The table.</param>
        /// <param name="resources">Supports: IObjectWithNameAndDescription</param>
        /// <returns></returns>
        /// <exception cref="dataException">
        /// File path not accessable: " + filepath - null - null - deserializeDataTable
        /// or
        /// Deserialization error - imbDataTableExtensions->deserialize
        /// </exception>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable deserializeDataTable(this string filename, dataTableExportEnum format, folderNode directory = null, DataTable table = null, params object[] resources)
        {
            dataTableIOFlags iOFlags = resources.GetIOFlags();

            string output = "";
            string filepath = "";
            FileInfo fileInfo = null;
            if (Path.IsPathRooted(filename))
            {
                filepath = filename;
            }
            else
            {
                if (directory == null) directory = new DirectoryInfo(Directory.GetCurrentDirectory());
                filepath = directory.pathFor(filename, getWritableFileMode.existing, table.GetDescription());
            }

            if (!File.Exists(filepath))
            {
                throw new dataException("File path not accessable: " + filepath, null, null, "deserializeDataTable");
            }

            fileInfo = new FileInfo(filepath);

            if (table == null) table = new DataTable();

            IObjectWithNameAndDescription ownerWithName = resources.getFirstOfType<IObjectWithNameAndDescription>(false, null, true);
            if (ownerWithName == null)
            {
                table.TableName = Path.GetFileNameWithoutExtension(filepath);
            }
            else
            {
                table.TableName = ownerWithName.name;

                table.ExtendedProperties.Add(templateFieldDataTable.data_tabledesc, ownerWithName.description);
            }
            switch (format)
            {
                case dataTableExportEnum.csv:
                    // <---------------- postoji vec u ekstenzijama CSV resenje
                    StreamReader sr = new StreamReader(fileInfo.FullName);

                    throw new NotImplementedException();

                    //var csvr = new CsvReader(sr);

                    //    csvr.ReadHeader();

                    //    csvr.Configuration.WillThrowOnMissingField = false;

                    //    foreach (string column in csvr.FieldHeaders)
                    //    {
                    //        var DataColumn = table.Columns.Add(column.Replace("__", "_"));

                    //    }

                    //while (csvr.Read())
                    //{
                    //    var row = table.NewRow();
                    //    foreach (DataColumn column in table.Columns)
                    //    {
                    //        string vl = csvr.GetField(column.DataType, column.ColumnName).toStringSafe();
                    //        vl = vl.Replace(".", "");
                    //        vl = vl.Replace(",", ".");
                    //        row[column.ColumnName] = vl;
                    //    }
                    //    table.Rows.Add(row);
                    //}

                    /*
                    IEnumerable<DataRow> rows = csvr.GetRecords<DataRow>();
                    foreach (DataRow dr in rows)
                    {
                        DataRow tdr = table.NewRow();
                        foreach (DataColumn dc in table.Columns)
                        {
                            tdr[dc] = dr[dc.ColumnName];
                        }
                        table.Rows.Add(tdr);
                    }*/

                    break;

                case dataTableExportEnum.excel:
                    try
                    {
                        using (ExcelPackage pck = new ExcelPackage(fileInfo))
                        {
                            var dts = pck.ToDataSet(iOFlags.HasFlag(dataTableIOFlags.firstRowColumnNames));
                            table = dts.Tables.imbFirstSafe() as DataTable;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new dataException("Deserialization error", ex, table, "imbDataTableExtensions->deserialize");
                    }
                    /*

                        // <--- OVO POSTOJI DOLE U KOMANDI toDataSet
                        using (ExcelPackage pck = new ExcelPackage(fileInfo))
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets[0];
                        //ws.GetValue(0,)
                        Int32 c = 1;
                        Int32 r = 1;
                        Boolean headersOk = false;
                        List<String> headers = new List<string>();
                        String head = ws.GetValue(r, c).toStringSafe();

                        for (int i = 1; i < ws.Dimension.Columns; i++)
                        {
                            DataColumn dc = new DataColumn(c.toOrdinalLetter(false));
                            head = ws.GetValue(r, c).toStringSafe();
                            if (head.isWord())
                            {
                                headers.Add(head);
                            }

                            dc.ExtendedProperties.Add(templateFieldDataTable.col_id, i);
                            dc.ExtendedProperties.Add(templateFieldDataTable.col_name, i.toOrdinalLetter());
                            table.Columns.Add(dc);
                            c++;
                        }

                        if (headers.Count() >= ws.Dimension.Columns) headersOk = true;

                        if (headersOk)
                        {
                            c = 0;
                            foreach (DataColumn dc in table.Columns)
                            {
                                dc.Caption = headers[c];
                                c++;
                            }
                            r++;
                        }

                        for (int i = r; i < ws.Dimension.Rows; i++)
                        {
                            DataRow dr = table.NewRow();
                            foreach (DataColumn dc in table.Columns)
                            {
                                dr[dc] = ws.GetValue(i, dc.Ordinal);
                            }
                            table.Rows.Add(dr);
                            r++;
                        }
                        }
                        */
                    output = fileInfo.FullName;
                    break;

                case dataTableExportEnum.json:

                    string json = openBase.openFileToString(filepath, true);
                    table = objectSerialization.DeserializeJson<DataTable>(json);

                    break;

                case dataTableExportEnum.markdown:
                    throw new NotImplementedException();
                    break;

                case dataTableExportEnum.xml:
                    string xml = openBase.openFileToString(filepath, true);
                    table.ReadXml(xml);
                    break;

                default:
                    break;
            }

            table.NormalizeColumnNames();
            return table;
        }

        public static string serializeDataSet(this DataSet source, string filename, folderNode directory, dataTableExportEnum format, params object[] resources)
        {
            dataTableIOFlags IOFlags = resources.GetIOFlags();

            string output = filename;
            FileInfo fileInfo = null;

            format = checkFormatByFilename(format, filename);

            aceAuthorNotation authorNotation = resources.getFirstOfType<aceAuthorNotation>(false, false, true);
            if (authorNotation == null) authorNotation = new aceAuthorNotation();

            if (directory == null) directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            string cleanfilename = source.FilenameForDataset(filename);
            filename = directory.pathFor(cleanfilename, getWritableFileMode.overwrite, "Exported DataSet [" + source.DataSetName + "] with [" + source.Tables.Count + "] tables. " + source.GetDesc());

            DirectoryInfo dix = null;
            switch (format)
            {
                case dataTableExportEnum.csv:
                    dix = directory.Add(cleanfilename, source.DataSetName, "Folder with CSV exports of DataSet [" + source.DataSetName + "] with [" + source.Tables.Count + "] tables. " + source.GetDesc());
                    foreach (DataTable table in source.Tables)
                    {
                        table.serializeDataTable(format, table.FilenameForTable(), dix);
                    }
                    output = dix.FullName;
                    break;

                case dataTableExportEnum.excel:
                    //fileInfo = new FileInfo(filename.ensureEndsWith(".xlsx"));

                    filename = imbSciStringExtensions.ensureEndsWith(filename, ".xlsx");
                    fileInfo = filename.getWritableFile(getWritableFileMode.overwrite);
                    //if (File.Exists(fileInfo.FullName)) File.Delete(fileInfo.FullName);
                    try
                    {
                        using (ExcelPackage pck = new ExcelPackage(fileInfo))
                        {
                            pck.Workbook.Properties.Title = source.GetTitle();
                            pck.Workbook.Properties.Comments = authorNotation.comment;
                            pck.Workbook.Properties.Category = "DataTable export";
                            pck.Workbook.Properties.Author = authorNotation.author;
                            pck.Workbook.Properties.Company = authorNotation.organization;
                            pck.Workbook.Properties.Application = authorNotation.software;
                            //pck.Workbook.Properties.Keywords = meta.keywords.content.toCsvInLine();
                            pck.Workbook.Properties.Created = DateTime.Now;
                            pck.Workbook.Properties.Subject = source.GetDesc();
                            int c = 0;
                            foreach (DataTable table in source.Tables)
                            {
                                string title = table.GetTitle();

                                if (title.Length > 20)
                                {
                                    title = title.toWidthMaximum(15, "");
                                }

                                title = title + c.ToString("D3");
                                c++;
                                while (pck.Workbook.Worksheets.Any(x => x.Name == title))
                                {
                                    title = title + c.ToString("D3");
                                    c++;
                                }

                                if (title == dataTableRenderingSetup.TABLE_DEFAULTNAME)
                                {
                                    title = "Table" + source.Tables.Count.ToString("D3");
                                }

                                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(title);

                                DataTableForStatistics dt_stat = table as DataTableForStatistics;
                                if (table is DataTableForStatistics)
                                {
                                    dt_stat = table as DataTableForStatistics;
                                }
                                else
                                {
                                    dt_stat = table.GetReportTableVersion(true);
                                }
                                table.SetTitle(title);
                                table.TableName = title;
                                //DataTableForStatistics dt_stat = table as DataTableForStatistics;

                                dt_stat.RenderToWorksheet(ws);

                                /*

                                ws.Cells["A1"].LoadFromDataTable(table, true);

                                ExcelRow row = ws.Row(1); //.Height = 100;
                                row.Height = 100;
                                row.Style.WrapText = true;

                                row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);

                                Int32 rc = ws.Dimension.Rows - 1;

                                for (int i = 0; i < rc; i++)
                                {
                                    var ex_row = ws.Row(i + 1);
                                    var in_row = table.Rows[i];
                                    if (dt_stat != null)
                                    {
                                        if (dt_stat.extraRows.Contains(in_row))
                                        {
                                            if (dt_stat.extraRows.IndexOf(in_row) % 2 > 0)
                                            {
                                                ex_row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                ex_row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                            }
                                            else
                                            {
                                                ex_row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                ex_row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);
                                            }
                                        }
                                    }

                                    ex_row.Style.WrapText = true;
                                    // row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);
                                }
                                */
                            }

                            pck.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new dataException("Excell: " + ex.Message, ex, source, "Export to excell");
                    }

                    output = fileInfo.FullName;
                    break;

                case dataTableExportEnum.json:
                    dix = directory.Add(cleanfilename, source.DataSetName, "Folder with JSON exports of DataSet [" + source.DataSetName + "] with [" + source.Tables.Count + "] tables. " + source.GetDesc());

                    output = objectSerialization.SerializeJson<DataSet>(source);

                    fileInfo = output.saveStringToFile(imbSciStringExtensions.ensureEndsWith(filename, ".json")); //.FullName;
                    output = fileInfo.FullName;
                    break;

                case dataTableExportEnum.markdown:
                    dix = directory.Add(cleanfilename, source.DataSetName, "Folder with Markdown exports of DataSet [" + source.DataSetName + "] with [" + source.Tables.Count + "] tables. " + source.GetDesc());
                    foreach (DataTable table in source.Tables)
                    {
                        table.serializeDataTable(format, table.FilenameForTable(), dix);
                    }
                    output = dix.FullName;
                    break;

                case dataTableExportEnum.xml:
                    dix = directory.Add(cleanfilename, source.DataSetName, "Folder with  XML exports of DataSet [" + source.DataSetName + "] with [" + source.Tables.Count + "] tables. " + source.GetDesc());
                    foreach (DataTable table in source.Tables)
                    {
                        table.serializeDataTable(format, table.FilenameForTable(), dix);
                    }
                    output = dix.FullName;
                    break;

                default:
                    break;
            }

            return output;
        }

        /// <summary>
        /// Serializes the data table into choosed format and returns file path
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="format">The format.</param>
        /// <param name="filename">The filename, without extension.</param>
        /// <param name="directory">The directory to save into.</param>
        /// <returns></returns>
        public static string serializeDataTable(this DataTable source, dataTableExportEnum format, string filename, folderNode directory, params object[] resources)
        {
            dataTableIOFlags IOFlags = resources.GetIOFlags();

            if (source == null) return "";
            if (source.Columns.Count == 0)
            {
                throw new dataException("Source table [0 columns]: ", null, source, "Export to excell : table is not applicable");
            }
            if (source.Rows.Count == 0) return "";

            format = checkFormatByFilename(format, filename);

            string output = filename;
            FileInfo fileInfo = null;

            aceAuthorNotation authorNotation = resources.getFirstOfType<aceAuthorNotation>(false, false, true);
            if (authorNotation == null) authorNotation = new aceAuthorNotation();

            if (directory == null) directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            string cleanfilename = source.FilenameForTable(filename);
            filename = directory.pathFor(cleanfilename, getWritableFileMode.overwrite, "Exported DataTable [" + source.GetTitle() + "][" + source.GetDescription() + "]. ");

            switch (format)
            {
                case dataTableExportEnum.csv:
                    output = source.toCSV(true);
                    output = output.saveStringToFile(imbSciStringExtensions.ensureEndsWith(filename, ".csv")).FullName;
                    break;

                case dataTableExportEnum.excel:
                    //fileInfo = new FileInfo(filename.ensureEndsWith(".xlsx"));

                    filename = imbSciStringExtensions.ensureEndsWith(filename, ".xlsx");
                    fileInfo = filename.getWritableFile(getWritableFileMode.overwrite);
                    //if (File.Exists(fileInfo.FullName)) File.Delete(fileInfo.FullName);
                    try
                    {
                        using (ExcelPackage pck = new ExcelPackage(fileInfo))
                        {
                            DataTableForStatistics dt_stat = null;
                            if (source is DataTableForStatistics)
                            {
                                dt_stat = source as DataTableForStatistics;
                            }
                            else
                            {
                                dt_stat = source.GetReportTableVersion(true);
                            }

                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(source.GetTitle());
                            dt_stat.RenderToWorksheet(ws);

                            /*
                            pck.Workbook.Properties.Title = source.GetTitle();
                            pck.Workbook.Properties.Comments = authorNotation.comment;
                            pck.Workbook.Properties.Category = "DataTable export";
                            pck.Workbook.Properties.Author = authorNotation.author;
                            pck.Workbook.Properties.Company = authorNotation.organization;
                            pck.Workbook.Properties.Application = authorNotation.software;
                            //pck.Workbook.Properties.Keywords = meta.keywords.content.toCsvInLine();
                            pck.Workbook.Properties.Created = DateTime.Now;
                            pck.Workbook.Properties.Subject = source.GetDescription();

                            ws.Cells["A1"].LoadFromDataTable(source, true);
                            ExcelRow row = ws.Row(1); //.Height = 100;
                            row.Height = 100;
                            row.Style.WrapText = true;

                            row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);

                            Int32 rc = ws.Dimension.Rows - 1;

                            for (int i = 0; i < rc; i++)
                            {
                                var ex_row = ws.Row(i+1);
                                var in_row = source.Rows[i];
                                if (dt_stat != null)
                                {
                                    if (dt_stat.extraRows.Contains(in_row))
                                    {
                                        if (dt_stat.extraRows.IndexOf(in_row)%2 > 0)
                                        {
                                            ex_row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            ex_row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                        } else
                                        {
                                            ex_row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            ex_row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);
                                        }
                                    }
                                }

                                ex_row.Style.WrapText = true;
                               // row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);
                            }
                            */

                            pck.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new dataException("Excell: " + ex.Message, ex, source, "Export to excell");
                    }

                    output = fileInfo.FullName;
                    break;

                case dataTableExportEnum.json:
                    output = objectSerialization.SerializeJson(source);
                    // JsonConvert.SerializeObject(source, Newtonsoft.Json.Formatting.Indented);
                    fileInfo = output.saveStringToFile(imbSciStringExtensions.ensureEndsWith(filename, ".json")); //.FullName;
                    output = fileInfo.FullName;
                    break;

                case dataTableExportEnum.markdown:
                    output = source.markdownTable();
                    fileInfo = output.saveStringToFile(imbSciStringExtensions.ensureEndsWith(filename, ".md"));
                    output = fileInfo.FullName;
                    break;

                case dataTableExportEnum.xml:
                    filename = imbSciStringExtensions.ensureEndsWith(filename, ".xml");
                    source.WriteXml(filename, false);
                    output = openBase.openFileToString(filename, true, false);
                    break;

                default:
                    break;
            }

            return output;
        }

        /// <summary>
        /// Gets the total rows count.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        public static int GetTotalRowsCount(this DataSet host)
        {
            int score = 0;
            foreach (DataTable dt in host.Tables)
            {
                score += dt.Rows.Count;
            }
            return score;
        }

        /// <summary>
        /// Constructs the data table and populates with random content of the table with.
        /// </summary>
        /// <param name="host">Host actually has no role in this extension. It-s just there to make this function easier to find</param>
        /// <param name="columns">The columns.</param>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        public static DataTable ConstructTableWithRandomContent(this object host, int columns, int rows)
        {
            DataTable table = new DataTable();
            table.TableName = "demoTable";

            for (int i = 0; i < columns; i++)
            {
                var dc = table.Columns.Add();
                dc.Caption = imbStringGenerators.getRandomString(6);
                dc.ColumnName = imbStringGenerators.getRandomString(6);
            }
            Random rnd = new Random();
            for (int i = 0; i < rows; i++)
            {
                var rw = table.NewRow();
                for (int c = 0; c < columns; c++)
                {
                    rw[c] = rnd.Next(100);
                }
                table.Rows.Add(rw);
            }
            return table;
        }

        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public static PropertyCollection AppendDataFields(this DataTable table, PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();
            //table.buildPropertyCollection(false, false, "data", data);

            data[templateFieldDataTable.data_columncount] = table.Columns.Count;
            data[templateFieldDataTable.data_rowcounttotal] = table.Rows.Count;
            data.add(templateFieldDataTable.data_tablename, table.TableName, true);

            // data[templateFieldDataTable.data_id] = id;
            // data[templateFieldDataTable.data_url] = url;
            return data;
        }

        /// <summary>
        /// Extracts a DataSet from the ExcelPackage.
        /// </summary>
        /// <param name="package">The Excel package.</param>
        /// <param name="firstRowContainsHeader">if set to <c>true</c> [first row contains header].</param>
        /// <returns></returns>
        public static DataSet ToDataSet(this ExcelPackage package, bool firstRowContainsHeader = false)
        {
            var headerRow = firstRowContainsHeader ? 1 : 0;
            return ToDataSet(package, headerRow);
        }

        /// <summary>
        /// Extracts a DataSet from the ExcelPackage.
        /// </summary>
        /// <param name="package">The Excel package.</param>
        /// <param name="headerRow">The header row. Use 0 if there is no header row. Value must be 0 or greater.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">headerRow must be 0 or greater.</exception>
        public static DataSet ToDataSet(this ExcelPackage package, int headerRow = 0)
        {
            if (headerRow < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(headerRow), headerRow, "Must be 0 or greater.");
            }

            var result = new DataSet();

            foreach (var sheet in package.Workbook.Worksheets)
            {
                var table = new DataTable { TableName = sheet.Name };

                int sheetStartRow = 1;
                if (headerRow > 0)
                {
                    sheetStartRow = headerRow;
                }
                var columns = from firstRowCell in sheet.Cells[sheetStartRow, 1, sheetStartRow, sheet.Dimension.End.Column]
                              select new DataColumn(headerRow > 0 ? firstRowCell.Text : $"Column {firstRowCell.Start.Column}");

                table.Columns.AddRange(columns.ToArray());
                if (table.Columns.Count == 0) break;

                var startRow = headerRow > 0 ? sheetStartRow + 1 : sheetStartRow;

                for (var rowIndex = startRow; rowIndex <= sheet.Dimension.End.Row; rowIndex++)
                {
                    var inputRow = sheet.Cells[rowIndex, 1, rowIndex, sheet.Dimension.End.Column];
                    var row = table.Rows.Add();
                    foreach (var cell in inputRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }

                result.Tables.Add(table);
            }

            return result;
        }

        /// <summary>
        /// Selects columns from data table - if empty select all columns
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fieldsOrCategoriesToShow"></param>
        /// <returns></returns>
        public static List<DataColumn> selectColumns(this DataTable data, string[] fieldsOrCategoriesToShow)
        {
            List<DataColumn> columns = new List<DataColumn>();

            foreach (DataColumn dc in data.Columns)
            {
                if (fieldsOrCategoriesToShow.Any())
                {
                    List<string> tokens = dc.ExtendedProperties[name_tokens] as List<string>;
                    if (tokens != null)
                    {
                        if (tokens.Any(x => Enumerable.Contains(fieldsOrCategoriesToShow, x)))
                        {
                            columns.Add(dc);
                        }
                    }
                }
                else
                {
                    columns.Add(dc);
                }
            }

            return columns;
        }

        /// <summary>
        /// Lista tokena za filtriranje
        /// </summary>
        public static string name_tokens = "tokens";

        /// <summary>
        /// Opis iz propertija
        /// </summary>
        public static string name_description = "description";

        /// <summary>
        /// Opis iz propertija
        /// </summary>
        public static string name_category = "kategorija";

        /// <summary>
        /// Referenca prema propertiju iz kojeg se puni polje
        /// </summary>
        public static string name_property = "property";

        /// <summary>
        /// Referenca prema objektu iz koga je napravljen red
        /// </summary>
        public static string name_object = "object";

        public static string name_display = "display";

        ///// <summary>
        ///// Rebuilds the data table from a number of rows
        ///// </summary>
        ///// <param name="rows">The rows.</param>
        ///// <param name="__dataTable">The data table.</param>
        ///// <returns></returns>
        //public static DataTable rebuildDataTable(this IEnumerable<DataRow> rows, String __dataTable)
        //{
        //    if (!rows.Any()) return null;

        //    DataTable output = rows.First().Table.Clone();

        //    foreach (DataRow dr in rows)
        //    {
        //        DataRow nr = output.NewRow();
        //        foreach (DataColumn dc in output.Columns)
        //        {
        //            nr[dc] = dr[dc.ColumnName];

        //        }
        //        output.Rows.Add(nr);
        //    }
        //    return output;
        //}

        /// <summary>
        /// Renames the type of the columns data table to.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="type">The type.</param>
        public static void renameColumnsDataTableToType(this DataTable table, Type type)
        {
            DataTable tmp = type.buildDataTable("temp", false, false, false, null);
            int c = 0;
            table.shiftColumnNames();

            foreach (DataColumn dc in table.Columns)
            {
                dc.ColumnName = tmp.Columns[c].ColumnName;
                dc.Caption = tmp.Columns[c].Caption;
                dc.ExtendedProperties.copyInto(tmp.ExtendedProperties);
                dc.Expression = tmp.Columns[c].Expression;
                c++;
            }
        }

        /// <summary>
        /// Shifts the column names: adds prefix in front of each column name
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="prefix">The prefix.</param>
        public static void shiftColumnNames(this DataTable table, string prefix = "Old_")
        {
            int c = 0;
            foreach (DataColumn dc in table.Columns)
            {
                dc.ColumnName = prefix + dc.ColumnName;
                dc.Caption = prefix + dc.Caption;
                c++;
            }
        }
    }
}