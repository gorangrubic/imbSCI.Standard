// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTableForStatisticsExtension.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.tables
{
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.table.style;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.files;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.style.shot;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.reporting;
    using imbSCI.DataComplex.converters;
    using imbSCI.DataComplex.extensions.data.formats;
    using imbSCI.DataComplex.extensions.data.operations;
    using imbSCI.DataComplex.extensions.data.schema;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using OfficeOpenXml.Style.XmlAccess;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public static class DataTableForStatisticsExtension
    {


        /// <summary>
        /// Gets the report and save.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="notation">The notation.</param>
        /// <param name="fileprefix">The fileprefix.</param>
        /// <returns></returns>
        public static List<DataTableForStatistics> GetReportAndSave(this Dictionary<String, DataTable> input, folderNode folder, aceAuthorNotation notation, String fileprefix = "", DataTableConverterASCII converter = null)
        {
            List<DataTableForStatistics> output = new List<DataTableForStatistics>();
            foreach (var pair in input)
            {
                String p_m = folder.pathFor(fileprefix + pair.Key);
                output.Add(pair.Value.GetReportAndSave(folder, notation, fileprefix + pair.Key));

                if (converter != null)
                {


                    converter.ConvertToFile(pair.Value, folder, pair.Value.TableName);
                }
            }

            return output;

        }

        /// <summary>
        /// Builds the data table splits.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <param name="numberOfSplits">The number of splits.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public static Dictionary<String, DataTable> BuildDataTableSplits<T>(this IEnumerable<T> input, Int32 numberOfSplits, String name = "", String description = "") where T : class
        {

            DataTableTypeExtended<T> table = new DataTableTypeExtended<T>(name, description);


            Dictionary<String, DataTable> output = new Dictionary<String, DataTable>();

            Int32 c = 0;
            Int32 block = input.Count() / numberOfSplits;

            Int32 b = 0;

            foreach (T metrics in input)
            {

                table.AddRow(metrics);

                c++;
                if (c > block)
                {
                    String n = table.TableName;

                    if (output.ContainsKey(n))
                    {
                        n = n + b.ToString("D3");
                        table.TableName = n;
                        table.SetTitle(n);
                    }
                    else
                    {

                    }

                    output.Add(n, table);

                    table = new DataTableTypeExtended<T>(name, description);
                    table.TableName = name + "_" + output.Count.ToString();
                    table.SetDescription(description + ".Block [" + output.Count.ToString() + "]");
                    c = 0;
                    b++;
                }
            }

            return output;

        }





        public static ExcelNamedStyleXml MakeStyle(this ExcelStyles styles, dataTableStyleSet styleSet, DataRowInReportTypeEnum data)
        {
            var output = styles.CreateNamedStyle(data.ToString());
            output.Style.SetStyle(styleSet.rowStyles[data]);
            return output;
        }

        /// <summary>
        /// Sets the style.
        /// </summary>
        /// <param name="ExcelStyle">The excel style.</param>
        /// <param name="styleEntry">The style entry.</param>
        public static void SetStyle(this ExcelStyle Style, dataTableStyleEntry styleEntry, Boolean isEven = false)
        {
            Style.Font.SetStyle(styleEntry.Text);
            //Style.TextRotation = styleEntry.Text.ro2

            if (isEven)
            {
                Style.Fill.SetStyle(styleEntry.Background);
            }
            else
            {
                Style.Fill.SetStyle(styleEntry.BackgroundAlt);
            }
            Style.SetStyle(styleEntry.Cell);
        }

        /// <summary>
        /// Sets the style.
        /// </summary>
        /// <param name="Fill">The fill.</param>
        /// <param name="styleEntry">The style entry.</param>
        public static void SetStyle(this ExcelFill Fill, styleSurfaceColor styleEntry)
        {
            if (styleEntry != null)
            {
                Fill.PatternType = (ExcelFillStyle)styleEntry.FillType;
                Fill.BackgroundColor.SetColor(styleEntry.Color);
                Fill.BackgroundColor.Tint = new decimal(styleEntry.Tint);
            }
        }

        public static void SetStyle(this ExcelRow row, dataTableStyleEntry style, Boolean isEven = false)
        {
            if (style != null)
            {
                if (style?.Cell?.minSize?.height == null)
                {
                    return;
                }
                row.Height = style.Cell.minSize.height;
                row.StyleName = style.key.ToString();
                row.Style.SetStyle(style, isEven);
            }
        }

        public static void SetStyle(this ExcelStyle Style, styleFourSide side)
        {
            if (side != null)
            {
                Style.SetStyle(side.top);
                Style.SetStyle(side.bottom);
                Style.SetStyle(side.left);
                Style.SetStyle(side.right);
            }
        }

        /// <summary>
        /// Sets the style.
        /// </summary>
        /// <param name="Style">The style.</param>
        /// <param name="side">The side.</param>
        public static void SetStyle(this ExcelStyle Style, styleSide side)
        {
            ExcelBorderItem bri = null;
            switch (side.direction)
            {
                case styleSideDirection.bottom:
                    bri = Style.Border.Bottom;
                    break;

                case styleSideDirection.left:
                    bri = Style.Border.Left;
                    break;

                case styleSideDirection.right:
                    bri = Style.Border.Right;
                    break;

                case styleSideDirection.top:
                    bri = Style.Border.Top;
                    break;
            }

            if (side.type != styleBorderType.unknown)
            {
                bri.Style = (ExcelBorderStyle)side.type.ToInt32();
                if (bri.Style != ExcelBorderStyle.None) bri.Color.SetColor(side.borderColorStatic);
            }
        }

        /// <summary>
        /// Sets the style.
        /// </summary>
        /// <param name="Style">The style.</param>
        /// <param name="styleEntry">The style entry.</param>
        public static void SetStyle(this ExcelStyle Style, styleContainerShot styleEntry)
        {
            Style.WrapText = styleEntry.doWrapText;
            Style.SetStyle(styleEntry.sizeAndBorder);

            switch (styleEntry.aligment)
            {
                case Core.reporting.zone.textCursorZoneCorner.Bottom:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    break;

                case textCursorZoneCorner.center:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    break;

                case textCursorZoneCorner.default_corner:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    break;

                case textCursorZoneCorner.DownLeft:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    break;

                case textCursorZoneCorner.DownRight:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    break;

                case textCursorZoneCorner.Left:
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    break;

                case textCursorZoneCorner.none:
                    break;

                case textCursorZoneCorner.Right:
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    break;

                case textCursorZoneCorner.Top:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    break;

                case textCursorZoneCorner.UpLeft:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    break;

                case textCursorZoneCorner.UpRight:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    break;

                default:
                    break;
            }

            Style.ShrinkToFit = styleEntry.doSizedownContent;
        }

        public static void SetStyle(this ExcelFont Font, styleTextFontSingle styleEntry)
        {
            Font.Name = styleEntry.FontName.ToString();
            Font.Color.SetColor(styleEntry.Color);

            Font.Bold = styleEntry.Style.HasFlag(styleTextTypeEnum.bold);
            Font.Italic = styleEntry.Style.HasFlag(styleTextTypeEnum.italic);
            Font.Strike = styleEntry.Style.HasFlag(styleTextTypeEnum.striketrough);
            Font.UnderLine = styleEntry.Style.HasFlag(styleTextTypeEnum.underline);
            Font.Size = styleEntry.FontSize;
        }

        //public static DataTable RenderLegend(this DataTable host)
        //{
        //    DataTable legend = new DataTable("LEGEND");

        //    DataColumn columnGroup = legend.Add("Group").SetDefaultBackground(host.columnCaption).SetWidth(25);
        //    DataColumn columnName = legend.Add("Name").SetDefaultBackground(extraEven).SetWidth(40);
        //    DataColumn columnLetter = legend.Add("Letter").SetDefaultBackground(extraOdd).SetWidth(25);
        //    DataColumn columnUnit = legend.Add("Unit").SetDefaultBackground(extraEven).SetWidth(25);
        //    DataColumn columnDescription = legend.Add("Description").SetDefaultBackground(dataOdd).SetWidth(180);

        //    extraRowStyles.Add(legend.AddRow("Table name", host.GetTitle()), DataRowInReportTypeEnum.columnDescription);
        //    extraRowStyles.Add(legend.AddRow("Table description", host.GetDescription()), DataRowInReportTypeEnum.columnDescription);
        //    extraRowStyles.Add(legend.AddRow("Aggregated aspect", host.GetAggregationAspect()), DataRowInReportTypeEnum.columnDescription);
        //    extraRowStyles.Add(legend.AddRow("Aggregated sources", host.GetAggregationOriginCount()), DataRowInReportTypeEnum.columnDescription);
        //    extraRowStyles.Add(legend.AddRow("Table class name", host.GetClassName()), DataRowInReportTypeEnum.columnDescription);
        //    legend.AddLineRow();
        //    //legend.AddRow("Table class name", this.);

        //    extraRowStyles.Add(legend.AddExtraRow(templateFieldDataTable.col_caption, 200), DataRowInReportTypeEnum.columnCaption);

        //    foreach (DataColumn dc in Columns)
        //    {
        //        var dr = legend.NewRow();

        //        dr[columnGroup] = dc.GetGroup();
        //        dr[columnName] = dc.GetHeading();
        //        dr[columnLetter] = dc.GetLetter();
        //        dr[columnUnit] = dc.GetUnit();
        //        dr[columnDescription] = dc.GetDesc();

        //        legend.Rows.Add(dr);
        //    }

        //    extraRowStyles.Add(legend.AddLineRow(), DataRowInReportTypeEnum.mergedHorizontally);

        //    foreach (string ext in this.GetExtraDesc())
        //    {
        //        extraRowStyles.Add(legend.AddStringLine(ext), DataRowInReportTypeEnum.mergedHorizontally);
        //    }

        //    extraRowStyles.Add(legend.AddLineRow(), DataRowInReportTypeEnum.mergedHorizontally);

        //    var pce = this.GetAdditionalInfo();

        //    extraRowStyles.Add(legend.AddRow("Additional information"), DataRowInReportTypeEnum.mergedFooterInfo);

        //    extraRowStyles.Add(legend.AddRow("Property", "Value", "Info"), DataRowInReportTypeEnum.columnCaption);
        //    foreach (KeyValuePair<object, PropertyEntry> entryPair in pce.entries)
        //    {
        //        extraRowStyles.Add(legend.AddRow(entryPair.Key, entryPair.Value[PropertyEntryColumn.entry_value], entryPair.Value[PropertyEntryColumn.entry_description]), DataRowInReportTypeEnum.columnInformation);
        //    }

        //    return legend;
        //}

        /// <summary>
        /// Checks if data type is allowed for the DataTable
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool checkIfDataTypeIsAllowed(this Type type)
        {
            if (type == typeof(bool)) return true;
            if (type == typeof(byte)) return true;
            if (type == typeof(byte[])) return true;
            if (type == typeof(char)) return true;
            if (type == typeof(DateTime)) return true;
            if (type == typeof(decimal)) return true;
            if (type == typeof(double)) return true;
            if (type == typeof(Guid)) return true;
            if (type == typeof(short)) return true;
            if (type == typeof(int)) return true;
            if (type == typeof(long)) return true;
            if (type == typeof(sbyte)) return true;
            if (type == typeof(float)) return true;
            if (type == typeof(string)) return true;
            if (type == typeof(TimeSpan)) return true;
            if (type == typeof(ushort)) return true;
            if (type == typeof(uint)) return true;
            if (type == typeof(ulong)) return true;

            return false;
        }

        /// <summary>
        /// Renders pivoted table
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="rowsSkip">The rows skip.</param>
        /// <returns></returns>
        public static DataTable RenderPivoted(this DataTable source, int rows = 2, int rowsSkip = 1)
        {
            DataTable legend = new DataTable(source.TableName);

            DataColumn columnGroup = legend.Add("Group").SetWidth(20).SetValueType(typeof(string));
            DataColumn columnName = legend.Add("Name").SetWidth(30).SetValueType(typeof(string));
            DataColumn columnLetter = legend.Add("Letter").SetWidth(10).SetValueType(typeof(string));

            List<DataColumn> columnValues = new List<DataColumn>();
            for (int i = rowsSkip; i < rows; i++)
            {
                string dcn = "Data" + i.ToString("D2");
                var dc = legend.Add(dcn);
                dc.SetWidth(30);

                columnValues.Add(dc); // new DataColumn(dcn));
            }

            DataColumn columnUnit = legend.Add("Unit").SetWidth(15).SetValueType(typeof(string));
            DataColumn columnDescription = legend.Add("Description").SetWidth(180).SetValueType(typeof(string));

            foreach (DataColumn dc in source.Columns)
            {
                var dr = legend.NewRow();

                dr[columnGroup] = dc.GetGroup();
                dr[columnName] = dc.GetHeading();
                dr[columnLetter] = dc.GetLetter();

                for (int i = 0; i < columnValues.Count; i++)
                {
                    DataRow dro = source.Rows[i];
                    dr[columnValues[i]] = dro[dc];
                }

                dr[columnUnit] = dc.GetUnit();
                dr[columnDescription] = dc.GetDesc();

                legend.Rows.Add(dr);
            }

            return legend;
        }

        /// <summary>
        /// Gets the report and save.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="notation">The notation.</param>
        /// <param name="filenamePrefix">The filename prefix.</param>
        /// <param name="disablePrimaryKey">if set to <c>true</c> [disable primary key].</param>
        /// <param name="allowAsyncCall">if set to <c>true</c> [allow asynchronous call].</param>
        /// <returns></returns>
        public static DataSetForStatistics GetReportAndSave(this DataSet source, folderNode folder, aceAuthorNotation notation = null, string filenamePrefix = "", bool disablePrimaryKey = true, Boolean allowAsyncCall = false)
        {
            DataSetForStatistics output = GetReportVersion(source, disablePrimaryKey);

            output.Save(folder, notation, filenamePrefix);

            return output;
        }

        public static String GetColumnSignatures(this DataTable table)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn dc in table.Columns)
            {
                sb.Append(dc.ColumnName);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the report version.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="disablePrimaryKey">if set to <c>true</c> [disable primary key].</param>
        /// <returns></returns>
        public static DataSetForStatistics GetReportVersion(this DataSet source, bool disablePrimaryKey = true)
        {
            DataSetForStatistics output = new DataSetForStatistics();
            output.DataSetName = source.DataSetName;


            List<String> columnSignatures = new List<string>();

            foreach (DataTable dt in source.Tables)
            {
                String sng = dt.GetColumnSignatures();

                var reportV = dt.GetReportTableVersion(disablePrimaryKey);

                output.AddTable(reportV);
                if (!columnSignatures.Contains(sng))
                {
                    output.AddTable(reportV.RenderLegend());
                    columnSignatures.Add(sng);
                }



            }

            return output;
        }

        public static DataTableForStatistics GetReportTableVersion(this DataTable source, bool disablePrimaryKey = true)
        {
            DataTableForStatistics output = source.GetClonedShema<DataTableForStatistics>(disablePrimaryKey);

            //output.AddRowNameColumn("Row name", true);
            // output.AddRowDescriptionColumn("Description", true);
            //// output.AddRowDescriptionColumn("Row info", true);
            // // <--- ovde ubaciti da atributi klase odredjuju stra prikazuje
            // output.AddExtraRow(templateFieldDataTable.col_group, 200);
            // output.AddExtraRow(PropertyEntryColumn.entry_unit, 200);
            // output.AddExtraRow(templateFieldDataTable.col_letter, 200);
            // output.AddExtraRow(templateFieldDataTable.col_desc, 200);

            //   output.SetDefaults();
            output.CopyRowsFrom(source);

            output.ApplyObjectTableTemplate();

            return output;
        }

        public static DataTable SaveXML(this DataTable source, folderNode folder, string filenamePrefix = "", bool clearMeta = true, bool checkContent = true, ILogBuilder logger = null)
        {
            if (source.TableName.isNullOrEmpty()) source.TableName = filenamePrefix + source.GetHashCode().ToString() + ".xml";

            string path = folder.pathFor(filenamePrefix + source.GetHashCode().ToString() + ".xml", getWritableFileMode.autoRenameExistingOnOtherDate);

            if (clearMeta) source = source.CleanMeta();

            if (checkContent)
            {
                foreach (DataRow row in source.Rows)
                {
                    foreach (DataColumn dc in source.Columns)
                    {
                        var t = row[dc]?.GetType();
                        if (t.IsClass)
                        {
                            if (logger != null) logger.log(" === unallowed content detected in the table to save " + path);
                            row[dc] = dc.DataType.GetDefaultValue();
                        }
                    }
                }
            }

            bool notOkToSave = false;
            foreach (DataColumn dc in source.Columns)
            {
                bool okType = dc.DataType.checkIfDataTypeIsAllowed();
                if (!okType)
                {
                    notOkToSave = true;
                    break;
                }
            }

            if (!notOkToSave)
            {
                objectSerialization.saveObjectToXML(source, path);
            }
            else
            {
                if (logger != null) logger.log("Can't save XML for table: " + source.TableName + " to " + path);
            }

            return source;
        }

        public static DataSet Save(this DataSet source, folderNode folder, aceAuthorNotation notation = null, string filenamePrefix = "")
        {

            string path = source.serializeDataSet(filenamePrefix, folder, dataTableExportEnum.excel, notation); //.serializeDataTable(dataTableExportEnum.excel, filenamePrefix, folder, notation);

            //source.serializeDataTable(enums.dataTableExportEnum.excel, filenamePrefix + "_source", folder, notation);

            return source;
        }


        public static DataTable Save(this DataTable source, folderNode folder, aceAuthorNotation notation = null, string filenamePrefix = "")
        {
            if (source is DataTable)
            {
                //  source.SetDefaults();
            }

            string path = source.serializeDataTable(dataTableExportEnum.excel, filenamePrefix, folder, notation);

            //source.serializeDataTable(enums.dataTableExportEnum.excel, filenamePrefix + "_source", folder, notation);

            return source;
        }

        public static bool tableReportCreation_useShortNames { get; set; }
        public static bool tableReportCreation_insertFilePathToTableExtra { get; set; } = true;

        public const string PREFIX_CLEANDATATABLE = "dc_";
        public const string PREFIX_REPORTDATATABLE = "dr_";
        public const string PREFIX_COLUMNINFO = "ci_";

        public const string EXTRAFOLDER = "data";

        /// <summary>
        /// Creates report table version for the <c>source</c> and saves the report on specified <c>folder</c>
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="notation">The notation.</param>
        /// <param name="filenamePrefix">The filename prefix.</param>
        /// <param name="disablePrimaryKey">if set to <c>true</c> [disable primary key].</param>
        /// <param name="allowAsyncCall">if set to <c>true</c> [allow asynchronous call].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Folder is null! at GetReportAndSave() for [" + source.TableName + "] at filename [" + filenamePrefix + "]</exception>
        public static DataTableForStatistics GetReportAndSave(this DataTable source, folderNode folder, aceAuthorNotation notation = null, string filenamePrefix = "", bool disablePrimaryKey = true, Boolean allowAsyncCall = false)
        {
            if (notation == null) notation = new aceAuthorNotation();

            if (allowAsyncCall)
            {
                if (imbSCI.Core.config.imbSCICoreConfig.settings.DataTableReports_AsyncExportCalls)
                {
                    DataTableForStatisticsExportJob job = new DataTableForStatisticsExportJob(source, folder, notation, filenamePrefix, disablePrimaryKey);
                    Thread t = new Thread(job.Do);
                    t.Start();
                    return null;
                    // Task.Factory
                }
            }

            // if (source == null) return new DataTableForStatistics();

            if (folder == null)
            {
                throw new ArgumentNullException("Folder is null! at GetReportAndSave() for [" + source.TableName + "] at filename [" + filenamePrefix + "]");
            }

            if (source.Columns.Count > 0)
            {
                folderNode dataFolder = null;
                if (DataTableForStatistics.AUTOSAVE_CleanDataTable || DataTableForStatistics.AUTOSAVE_FieldsText || imbSCI.Core.config.imbSCICoreConfig.settings.DataTableReports_DoExportXMLData)
                {
                    dataFolder = folder.Add(EXTRAFOLDER, "Excel report meta data", "Folder containing clean data export (single header row, CSV format) for easier use by other software platforms and/or column meta descriptions - additional information - in separate txt file for each Excel report created.");
                }

                if (imbSCI.Core.config.imbSCICoreConfig.settings.DataTableReports_DoExportXMLData)
                {
                    try
                    {
                        String xmlCode = objectSerialization.ObjectToXML(source);
                        xmlCode.saveStringToFile(dataFolder.pathFor(source.TableName.getFilename(".xml"), getWritableFileMode.overwrite, "XML Serialized DataTable [" + source.GetTitle() + "]", true));
                    }
                    catch (Exception ex)
                    {
                        source.SetAdditionalInfoEntry("XML data", "Serialization failed: " + ex.Message);
                    }
                }

                if (DataTableForStatistics.AUTOSAVE_CleanDataTable)
                {
                    string cld = source.serializeDataTable(dataTableExportEnum.csv, PREFIX_CLEANDATATABLE + filenamePrefix.getFilename() + ".csv", dataFolder, notation);
                    source.SetAdditionalInfoEntry("Clean data", cld);
                }

                if (DataTableForStatistics.AUTOSAVE_FieldsText)
                {
                    string cli = dataFolder.pathFor(PREFIX_COLUMNINFO + filenamePrefix.getFilename() + ".txt");
                    source.GetUserManualForTableSaved(cli);
                    source.SetAdditionalInfoEntry("Column info", cli);
                }

                if (tableReportCreation_insertFilePathToTableExtra)
                {
                }
            }

            DataTableForStatistics output = null;

            if (source is DataTableForStatistics)
            {
                output = source as DataTableForStatistics;
            }
            else
            {
                output = source.GetReportTableVersion(disablePrimaryKey);
                // output.SetDefaults();

                //source.serializeDataTable(enums.dataTableExportEnum.excel, filenamePrefix + "_source", folder, notation);
            }

            output.Save(folder, notation, filenamePrefix);

            return output;
        }

        public static void SetDefaults(this DataTable source)
        {
            foreach (DataColumn dc in source.Columns)
            {
                //if (dc.GetWidth()== dataColumnRenderingSetup.DEFAULT_WIDTH)
                //{
                //    if (dc.GetValueType() == typeof(String)) dc.SetWidth(40);
                //    if (dc.GetValueType() == typeof(Int32)) dc.SetWidth(10);
                //}
                if (dc.GetFormat() == "")
                {
                    if (dc.GetValueType() == typeof(double))
                    {
                        if (dc.GetUnit().Contains("%"))
                        {
                            dc.SetFormat("P2");
                        }
                        else
                        {
                            dc.SetFormat("F5");
                        }
                    }
                }
            }
        }

        //public static String
    }
}