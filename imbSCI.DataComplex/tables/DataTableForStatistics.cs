// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTableForStatistics.cs" company="imbVeles" >
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
    using imbSCI.Core.attributes;
    using imbSCI.Core.collection;
    using imbSCI.Core.config;
    using imbSCI.Core.data;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.table.core;
    using imbSCI.Core.extensions.table.style;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.math.aggregation;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using imbSCI.DataComplex.exceptions;
    using imbSCI.DataComplex.extensions.data;
    using imbSCI.DataComplex.extensions.data.modify;
    using imbSCI.DataComplex.extensions.data.operations;
    using imbSCI.DataComplex.extensions.data.schema;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// IDEJA SAMO
    /// </summary>
    /// <seealso cref="System.Data.DataTable" />
    public class DataTableForStatistics : DataTable, IDataTableForStatistics
    {
        //public DataTableForStatistics()
        //{
        //}

        public static bool AUTOSAVE_CleanDataTable
        {
            get
            {
                return imbSCICoreConfig.settings.DataTableReports_DoExportCleanData;
            }
        }

        public static bool AUTOSAVE_FieldsText
        {
            get
            {
                return imbSCICoreConfig.settings.DataTableReports_DoExportColMetaData;
            }
        }

        /// <summary>
        /// Row index position when data starts (after heading rows are created
        /// </summary>
        /// <value>
        /// The row start.
        /// </value>
        public int RowStart { get; set; } = 1;

        /// <summary>
        /// Rel address of the last aggregated row having maximum number of rows aggregated (i.e. number of common rows)
        /// </summary>
        /// <value>
        /// The row aggregation all.
        /// </value>
        public int RowsWithMaxAggregation { get; set; } = 1;

        /// <summary>
        /// Number of rows with data in this report datatable
        /// </summary>
        /// <value>
        /// The rows with data count.
        /// </value>
        public int RowsWithDataCount { get; set; } = 1;

        public DataTableCategorySets categories { get; set; }

        public Dictionary<DataColumn, DataColumnInReportTypeEnum> extraColumnStyles { get; set; } = new Dictionary<DataColumn, DataColumnInReportTypeEnum>();

        public Dictionary<DataRow, DataRowInReportTypeEnum> extraRowStyles { get; set; } = new Dictionary<DataRow, DataRowInReportTypeEnum>();

        /// <summary>
        /// Last file path that used for save or load
        /// </summary>
        public string lastFilePath { get; protected set; }

        //public String Save(folderNode folder, aceAuthorNotation notation=null, String filenamePrefix = "")
        //{
        //    if (filenamePrefix.isNullOrEmpty()) filenamePrefix = TableName;
        //    if (notation == null) notation = new aceAuthorNotation();
        //    String path = this.serializeDataTable(aceCommonTypes.enums.dataTableExportEnum.excel, filenamePrefix, folder, notation);
        //    lastFilePath = path;
        //    return path;
        //}

        public DataTableForStatistics()
        {
        }

        public DataTable ApplyFilters(DataTable output)
        {
            List<DataColumn> toRemove = new List<DataColumn>();

            foreach (DataColumn dc in output.Columns)
            {
                if (dc.ExtendedProperties.ContainsKey(imbAttributeName.reporting_hide))
                {
                    toRemove.Add(dc);
                }
                else
                {
                    var def = dc.GetAggregation();
                    if (def != null)
                    {
                        switch (def.multiTableType)
                        {
                            case dataPointAggregationType.hidden:
                                toRemove.Add(dc);
                                break;
                        }
                    }
                }
            }

            foreach (DataColumn dc in toRemove)
            {
                output.Columns.Remove(dc);
            }
            return output;
        }

        [XmlIgnore]
        public DataRowMetaDictionary metaRowInfo { get; set; }

        [XmlIgnore]
        public DataColumnMetaDictionary metaColumnInfo { get; set; }

        /// <summary>
        /// Renders the data table.
        /// </summary>
        /// <returns></returns>
        public DataTable RenderDataTable()
        {
            DataTable output = new DataTable(TableName);
            output = this.GetClonedShema<DataTable>();

            ApplyFilters(output);

            //output.SetRowMeta(new DataRowMetaDictionary());
            //output.SetDescription(this.GetDescription());

            //foreach (DataColumnInReportDefinition cd in metaColumnInfo[nameof(DataColumnInReportTypeEnum.infoOnLeft)])
            //{
            //    DataColumn dtc = output.Add(cd.infoSource);
            //    dtc.SetOrdinal(0);
            //}

            //foreach (DataColumnInReportDefinition cd in metaColumnInfo[nameof(DataColumnInReportTypeEnum.infoOnRight)])
            //{
            //    DataColumn dtc = output.Add(cd.infoSource);
            //    dtc.SetOrdinal(output.Columns.Count-1);
            //}

            //  extraColumnStyles.Add(output.AddRowNameColumn("Row name", false), DataColumnInReportTypeEnum.infoOnLeft);
            // extraColumnStyles.Add(output.AddRowDescriptionColumn("Description", true), DataColumnInReportTypeEnum.infoOnRight);

            // output.AddRowDescriptionColumn("Row info", true);
            // <--- ovde ubaciti da atributi klase odredjuju stra prikazuje
            extraRowStyles.Add(output.AddExtraRow(templateFieldDataTable.col_group, 200), DataRowInReportTypeEnum.mergedCategoryHeader);
            extraRowStyles.Add(output.AddExtraRow(templateFieldDataTable.col_caption, 200), DataRowInReportTypeEnum.columnCaption);
            extraRowStyles.Add(output.AddExtraRow(templateFieldDataTable.col_unit, 200), DataRowInReportTypeEnum.columnDescription);
            extraRowStyles.Add(output.AddExtraRow(templateFieldDataTable.col_letter, 200), DataRowInReportTypeEnum.columnDescription);
            // extraRowStyles.Add(output.AddExtraRow(templateFieldDataTable.col_desc, 300), DataRowInReportTypeEnum.columnDesc);

            RowStart = output.Rows.Count;

            output.CopyRowsFrom(this);

            RowsWithDataCount = output.Rows.Count - RowStart;

            PropertyCollectionExtended pce = this.GetAdditionalInfo();
            var val = pce[DataTableAggregationDefinition.ADDPROPS_ROWSCOMMON];
            if (val != null)
            {
                RowsWithMaxAggregation = val.imbToNumber<int>();
            }
            else
            {
                RowsWithMaxAggregation = RowsWithDataCount;
            }
            categories = new DataTableCategorySets(output);

            extraRowStyles.Add(output.AddLineRow(), DataRowInReportTypeEnum.mergedHorizontally);

            //foreach (DataColumn dc in output.Columns)
            //{
            //    dc.GetAggregation().rowRangeStart = ri;
            //    dc.GetAggregation().rowRangeEnd = ri;
            //}

            foreach (string ext in this.GetExtraDesc())
            {
                extraRowStyles.Add(output.AddStringLine(ext), DataRowInReportTypeEnum.mergedHorizontally);
            }

            extraRowStyles.Add(output.AddLineRow(), DataRowInReportTypeEnum.mergedHorizontally);

            return output;
        }

        [XmlIgnore]
        public dataTableStyleSet styleSet
        {
            get
            {
                return this.GetStyleSet();
            }
        }

        [XmlIgnore]
        public tableStyleRowSetter rowMetaSet
        {
            get
            {
                return this.GetRowMetaSet();
            }
        }

        [XmlIgnore]
        public tableStyleColumnSetter columnMetaSet
        {
            get
            {
                return this.GetColumnMetaSet();
            }
        }

        //public string fontName { get; set; } = "Cambria"; //"Times New Roman";
        //public System.Drawing.Color columnCaption = System.Drawing.Color.SteelBlue;
        //public System.Drawing.Color extraEven = System.Drawing.Color.LightSlateGray;
        //public System.Drawing.Color extraEvenOther = System.Drawing.Color.LightSteelBlue;
        //public System.Drawing.Color extraOdd = System.Drawing.Color.SlateGray;
        //public System.Drawing.Color dataOdd = System.Drawing.Color.WhiteSmoke;
        //public System.Drawing.Color dataEven = System.Drawing.Color.Snow;

        public void DefineStyles(ExcelWorksheet ws)
        {
            if (ws.Workbook.Styles.NamedStyles.Count() > 0) return;

            OfficeOpenXml.Style.XmlAccess.ExcelNamedStyleXml style = ws.Workbook.Styles.MakeStyle(styleSet, DataRowInReportTypeEnum.data);
            style = ws.Workbook.Styles.MakeStyle(styleSet, DataRowInReportTypeEnum.mergedHeaderTitle);//.CreateNamedStyle(nameof(DataRowInReportTypeEnum.mergedHeaderTitle));

            style = ws.Workbook.Styles.MakeStyle(styleSet, DataRowInReportTypeEnum.mergedHeaderInfo);//.CreateNamedStyle(nameof(DataRowInReportTypeEnum.mergedHeaderInfo));

            style = ws.Workbook.Styles.MakeStyle(styleSet, DataRowInReportTypeEnum.columnCaption);//.CreateNamedStyle(nameof(DataRowInReportTypeEnum.columnCaption));

            style = ws.Workbook.Styles.MakeStyle(styleSet, DataRowInReportTypeEnum.columnDescription);//.CreateNamedStyle(nameof(DataRowInReportTypeEnum.columnDescription));

            style = ws.Workbook.Styles.MakeStyle(styleSet, DataRowInReportTypeEnum.columnInformation);//.CreateNamedStyle(nameof(DataRowInReportTypeEnum.columnInformation));

            style = ws.Workbook.Styles.MakeStyle(styleSet, DataRowInReportTypeEnum.mergedCategoryHeader);//.CreateNamedStyle(nameof(DataRowInReportTypeEnum.mergedCategoryHeader));

            style = ws.Workbook.Styles.MakeStyle(styleSet, DataRowInReportTypeEnum.mergedFooterInfo);//.CreateNamedStyle(nameof(DataRowInReportTypeEnum.mergedFooterInfo));
        }

        /// <summary>
        /// Deploys the style.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="ws">The ws.</param>
        /// <param name="useMetaSetFromTheTable">if set to <c>true</c> it will use style set from the table specified.</param>
        public void DeployStyle(DataTable table, ExcelWorksheet ws, Boolean useMetaSetFromTheTable = false)
        {
            int rc = ws.Dimension.Rows;

            if (rc > ROW_LIMIT_TODISABLE_STYLING)
            {
            }
            else
            {
                for (int i = 0; i < rc; i++)
                {
                    var ex_row = ws.Row(i + 1);
                    var in_row = table.Rows[i];
                    if (i > ROW_LIMIT_FOR_STYLE) break;
                    tableStyleRowSetter metaSet = null;

                    if (useMetaSetFromTheTable) metaSet = table.GetRowMetaSet();
                    DeployStyleToRow(ex_row, in_row, ws, metaSet);
                }
            }

            foreach (DataColumn dc in table.Columns)
            {
                ws.Column(dc.Ordinal + 1).Width = dc.GetWidth();

                switch (dc.GetImportance())
                {
                    case dataPointImportance.alarm:
                        ws.Column(dc.Ordinal + 1).Style.Font.Color.SetColor(System.Drawing.Color.Red);
                        break;

                    case dataPointImportance.important:
                        ws.Column(dc.Ordinal + 1).Style.Font.Bold = true;
                        break;

                    case dataPointImportance.none:
                        break;

                    case dataPointImportance.normal:
                        break;
                }
            }
        }

        /// <summary>
        /// Deploys the style to row.
        /// </summary>
        /// <param name="ex_row">The ex row.</param>
        /// <param name="in_row">The in row.</param>
        /// <param name="ws">The ws.</param>
        /// <param name="metaSet">The meta set.</param>
        public void DeployStyleToRow(ExcelRow ex_row, DataRow in_row, ExcelWorksheet ws, tableStyleRowSetter metaSet = null)
        {
            DataRowInReportTypeEnum style = DataRowInReportTypeEnum.data;

            if (extraRowStyles.ContainsKey(in_row))
            {
                style = extraRowStyles[in_row];
            }

            bool isEven = ((in_row.Table.Rows.IndexOf(in_row) % 2) > 0);

            Boolean isLegend = in_row.Table.TableName == LEGENDTABLE_NAME;

            var baseStyle = styleSet.rowStyles[style];

            var rowsMetaSet = this.GetRowMetaSet();

            if (metaSet != null) rowsMetaSet = metaSet;

            var response = rowsMetaSet.evaluate(in_row, this, baseStyle);
            if (response != null) if (response.style != null) baseStyle = response.style;
            if (baseStyle == null) baseStyle = styleSet.rowStyles[style];
            foreach (string s in response.notes)
            {
                Console.WriteLine(s);
            }

            ex_row.SetStyle(baseStyle, isEven);

            switch (style)
            {
                case DataRowInReportTypeEnum.columnCaption:
                    if (!isLegend)
                    {
                        foreach (var cpair in categories)
                        {
                            foreach (selectZone zone in categories.categoryZones[cpair.Key])
                            {
                                var sl = ws.getExcelRange(ex_row, zone);
                                sl.Style.Fill.PatternType = ExcelFillStyle.Solid;

                                var col = categories.categoryColors[cpair.Key]; //.First().DefaultBackground(System.Drawing.Color.Gray);
                                sl.Style.Fill.BackgroundColor.SetColor(col);
                                sl.Style.Fill.BackgroundColor.Tint = new decimal(0.2);
                            }
                        }
                    }
                    break;

                case DataRowInReportTypeEnum.mergedHeaderTitle:
                    ws.Cells[ex_row.Row, 1, ex_row.Row, in_row.Table.Columns.Count].Merge = true;

                    break;

                case DataRowInReportTypeEnum.mergedHeaderInfo:

                    ws.Cells[ex_row.Row, 1, ex_row.Row, in_row.Table.Columns.Count].Merge = true;

                    break;

                case DataRowInReportTypeEnum.mergedFooterInfo:

                    ws.Cells[ex_row.Row, 1, ex_row.Row, in_row.Table.Columns.Count].Merge = true;

                    break;

                case DataRowInReportTypeEnum.mergedHorizontally:

                    selectZone z = new selectZone(0, 0, Columns.Count - 1, 0);
                    var s2l = ws.getExcelRange(ex_row, z);
                    s2l.Merge = true;
                    break;

                case DataRowInReportTypeEnum.mergedCategoryHeader:

                    double tn = 0.2;
                    int hlip = -1;
                    foreach (var cpair in categories)
                    {
                        if (hlip == -1)
                        {
                            tn = 0.3;
                        }
                        else
                        {
                            tn = 0.6;
                        }

                        foreach (selectZone zone in categories.categoryZones[cpair.Key])
                        {
                            var sl = ws.getExcelRange(ex_row, zone);
                            sl.Merge = true;
                            sl.Value = cpair.Key.ToUpper();
                            sl.Style.Fill.PatternType = ExcelFillStyle.Solid;

                            var col = categories.categoryColors[cpair.Key]; //.First().DefaultBackground(System.Drawing.Color.Gray);
                            sl.Style.Fill.BackgroundColor.SetColor(col);
                            sl.Style.Fill.BackgroundColor.Tint = new decimal(tn);
                        }

                        hlip = -hlip;
                    }
                    isEven = !isEven;

                    break;

                case DataRowInReportTypeEnum.data:

                    //ex_row.Style.Font.Size = 9;

                    foreach (DataColumn dc in in_row.Table.Columns)
                    {
                        string format = dc.GetFormatForExcel();
                        if (!format.isNullOrEmpty()) ws.Cells[ex_row.Row, dc.Ordinal + 1].Style.Numberformat.Format = format;

                        if (dc.GetValueType().isNumber())
                        {
                            ws.Cells[ex_row.Row, dc.Ordinal + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        else if (dc.GetValueType().IsEnum)
                        {
                            ws.Cells[ex_row.Row, dc.Ordinal + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        else if (dc.GetValueType().isBoolean())
                        {
                            ws.Cells[ex_row.Row, dc.Ordinal + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        else
                        {
                            ws.Cells[ex_row.Row, dc.Ordinal + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }
                    }

                    break;
            }

            if (!isLegend)
            {
                if (ex_row.Row == (RowStart + RowsWithMaxAggregation))
                {
                    ex_row.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    ex_row.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Red);
                }
            }
        }

        public const String LEGENDTABLE_NAME = "LEGEND";

        public DataTable RenderLegend()
        {
            DataTable legend = new DataTable(LEGENDTABLE_NAME);

            DataColumn columnName = legend.Add("Name").SetDefaultBackground(styleSet.extraEven).SetWidth(20);
            DataColumn columnDescription = legend.Add("Description").SetDefaultBackground(styleSet.dataOdd).SetWidth(120);
            DataColumn columnGroup = legend.Add("Group").SetDefaultBackground(styleSet.columnCaption).SetWidth(25);
            DataColumn columnLetter = legend.Add("Letter").SetDefaultBackground(styleSet.extraOdd).SetWidth(20);
            DataColumn columnUnit = legend.Add("Unit").SetDefaultBackground(styleSet.extraEven).SetWidth(10);

            extraRowStyles.Add(legend.AddRow("Table name", this.GetTitle()), DataRowInReportTypeEnum.columnDescription);
            extraRowStyles.Add(legend.AddRow("Table description", this.GetDescription()), DataRowInReportTypeEnum.columnDescription);
            extraRowStyles.Add(legend.AddRow("Aggregated aspect", this.GetAggregationAspect()), DataRowInReportTypeEnum.columnDescription);
            extraRowStyles.Add(legend.AddRow("Aggregated sources", this.GetAggregationOriginCount()), DataRowInReportTypeEnum.columnDescription);
            extraRowStyles.Add(legend.AddRow("Table class name", this.GetClassName()), DataRowInReportTypeEnum.columnDescription);
            legend.AddLineRow();
            //legend.AddRow("Table class name", this.);

            extraRowStyles.Add(legend.AddExtraRow(templateFieldDataTable.col_caption, 200), DataRowInReportTypeEnum.columnCaption);

            foreach (DataColumn dc in Columns)
            {
                var dr = legend.NewRow();

                dr[columnGroup] = dc.GetGroup();
                dr[columnName] = dc.GetHeading();
                dr[columnLetter] = dc.GetLetter();
                dr[columnUnit] = dc.GetUnit();
                dr[columnDescription] = dc.GetDesc();

                legend.Rows.Add(dr);
            }

            extraRowStyles.Add(legend.AddLineRow(), DataRowInReportTypeEnum.mergedHorizontally);

            var pce = this.GetAdditionalInfo();

            extraRowStyles.Add(legend.AddRow("Additional information"), DataRowInReportTypeEnum.mergedFooterInfo);

            extraRowStyles.Add(legend.AddRow("Property", "Value", "Info"), DataRowInReportTypeEnum.columnCaption);
            foreach (KeyValuePair<object, PropertyEntry> entryPair in pce.entries)
            {
                extraRowStyles.Add(legend.AddRow(entryPair.Key, entryPair.Value[PropertyEntryColumn.entry_value], entryPair.Value[PropertyEntryColumn.entry_description]), DataRowInReportTypeEnum.columnInformation);
            }

            foreach (string ext in this.GetExtraDesc())
            {
                extraRowStyles.Add(legend.AddStringLine(ext), DataRowInReportTypeEnum.mergedFooterInfo);
            }

            extraRowStyles.Add(legend.AddLineRow(), DataRowInReportTypeEnum.mergedHorizontally);

            extraRowStyles.Add(legend.AddStringLine(imbSCICoreConfig.settings.DataTableReports_SignatureLine), DataRowInReportTypeEnum.mergedHorizontally);
            if (!imbSCICoreConfig.settings.DataTableReports_SignatureLine.Contains("imbVeles"))
            {
                extraRowStyles.Add(legend.AddStringLine(SHORT_SIGNATURE), DataRowInReportTypeEnum.mergedHorizontally);
            }

            return legend;
        }

        public const string SHORT_SIGNATURE = "Generated with: imbVeles Framework (GNU GPLv3) - blog.veles.rs";

        public void SaveToLegendWorksheet(ExcelWorksheet ws)
        {
            DefineStyles(ws);
            DataTable legend = RenderLegend();
            ws.Cells["A1"].LoadFromDataTable(legend, false);
            DeployStyle(legend, ws, true);
        }

        public static int ROW_LIMIT_FOR_STYLE
        {
            get
            {
                return imbSCICoreConfig.settings.DataTableReports_RowsApplyStylingLimit;
            }
        }

        public static int ROW_LIMIT_TODISABLE_STYLING
        {
            get
            {
                return imbSCICoreConfig.settings.DataTableReports_RowsCountToDisableStyling;
            }
        }

        public void SaveToWorksheet(ExcelWorksheet ws)
        {
            DefineStyles(ws);
            DataTable source = RenderDataTable();

            ws.Cells["A1"].LoadFromDataTable(source, false);

            DeployStyle(source, ws);
        }

        /// <summary>
        /// Inserts the aggregation.
        /// </summary>
        /// <param name="ws">The ws.</param>
        /// <param name="dataTable">The data table.</param>
        public void InsertAggregation(ExcelWorksheet ws, DataTable dataTable)
        {
            int ri = 0;
            int re = 0;
            foreach (DataColumn dc in dataTable.Columns)
            {
                var aggregation = dc.GetAggregation();

                ri = Math.Max((int)aggregation.rowRangeStart, ri);

                re = Math.Max((int)aggregation.rowRangeEnd, re);
            }

            ExcelRow er = ws.Row(re + 2);

            foreach (DataColumn dc in dataTable.Columns)
            {
                if (dc.GetValueType() == typeof(string))
                {
                    ws.Cells[re + 2, dc.Ordinal].Value = "SUM()"; //ws.Cells[ri, dc.Ordinal, re, dc.Ordinal]
                    ws.Cells[re + 3, dc.Ordinal].Value = "AVG()";
                    ws.Cells[re + 4, dc.Ordinal].Value = "MAX()";
                    ws.Cells[re + 5, dc.Ordinal].Value = "MIN()";
                    ws.Cells[re + 6, dc.Ordinal].Value = "VAR()";
                    ws.Cells[re + 7, dc.Ordinal].Value = "STDEV()";
                }
                else
                {
                    ws.Cells[re + 2, dc.Ordinal].Formula = string.Format("SUM({0})", new ExcelAddress(ri, dc.Ordinal, re, dc.Ordinal).Address); //ws.Cells[ri, dc.Ordinal, re, dc.Ordinal]
                    ws.Cells[re + 3, dc.Ordinal].Formula = string.Format("AVG({0})", new ExcelAddress(ri, dc.Ordinal, re, dc.Ordinal).Address);
                    ws.Cells[re + 4, dc.Ordinal].Formula = string.Format("MAX({0})", new ExcelAddress(ri, dc.Ordinal, re, dc.Ordinal).Address);
                    ws.Cells[re + 5, dc.Ordinal].Formula = string.Format("MIN({0})", new ExcelAddress(ri, dc.Ordinal, re, dc.Ordinal).Address);
                    ws.Cells[re + 6, dc.Ordinal].Formula = string.Format("VAR({0})", new ExcelAddress(ri, dc.Ordinal, re, dc.Ordinal).Address);
                    ws.Cells[re + 7, dc.Ordinal].Formula = string.Format("STDEV({0})", new ExcelAddress(ri, dc.Ordinal, re, dc.Ordinal).Address);
                }
            }
        }

        //public static String GetFileName(this DataTa)

        /// <summary>
        /// Saves the specified folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="notation">The notation.</param>
        /// <param name="filenamePrefix">The filename prefix.</param>
        /// <returns></returns>
        /// <exception cref="dataException">Excell: " + ex.Message - Export to excell</exception>
        public string Save(folderNode folder, aceAuthorNotation notation = null, string filenamePrefix = "")
        {
            string msg = "tried to save a data table [" + this.GetTitle() + "][" + TableName + "] -> " + folder.path + " [" + DateTime.Now.ToLongTimeString() + "/" + DateTime.Now.ToLongDateString() + "]";
            string fl = "note_savefailed_" + GetHashCode() + ".txt";
            //FileInfo fi = msg.saveStringToFile(folder.pathFor(fl, getWritableFileMode.none), getWritableFileMode.autoRenameExistingToBack);
            FileInfo fileInfo = null;

            try
            {
                string output = "";
                string filename = "";

                if (DataTableForStatisticsExtension.tableReportCreation_useShortNames)
                {
                    if (filenamePrefix.isNullOrEmpty())
                    {
                        filename = "dt_".add(TableName, "_").getFilename();
                    }
                    else
                    {
                        filename = "dt_".add(filenamePrefix, "_").getFilename();
                    }
                }
                else
                {
                    filename = "dt_" + filenamePrefix.add(TableName).getFilename();
                }

                filename = filename.ensureEndsWith(".xlsx");
                filename = folder.pathFor(filename);

                fileInfo = filename.getWritableFile(getWritableFileMode.overwrite);

                if (DataTableForStatisticsExtension.tableReportCreation_insertFilePathToTableExtra)
                {
                    this.SetAdditionalInfoEntry("Folder", fileInfo.DirectoryName);
                    this.SetAdditionalInfoEntry("File", fileInfo.Name);
                    this.SetAdditionalInfoEntry("ID", filenamePrefix);
                }

                if (Rows.Count == 0) return fileInfo.FullName;

                //if (File.Exists(fileInfo.FullName)) File.Delete(fileInfo.FullName);
                try
                {
                    using (ExcelPackage pck = new ExcelPackage(fileInfo))
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add(this.GetTitle());
                        ExcelWorksheet wsL = pck.Workbook.Worksheets.Add("LEGEND");

                        pck.Workbook.Properties.Title = this.GetTitle();
                        pck.Workbook.Properties.Comments = notation.comment;
                        pck.Workbook.Properties.Category = "DataTable export";
                        pck.Workbook.Properties.Author = notation.author;
                        pck.Workbook.Properties.Company = notation.organization;
                        pck.Workbook.Properties.Application = notation.software;
                        //pck.Workbook.Properties.Keywords = meta.keywords.content.toCsvInLine();
                        pck.Workbook.Properties.Created = DateTime.Now;
                        pck.Workbook.Properties.Subject = this.GetDescription();

                        SaveToWorksheet(ws);

                        SaveToLegendWorksheet(wsL);

                        pck.Save();
                    }
                }
                catch (Exception ex)
                {
                    throw new dataException("Excell: " + ex.Message, ex, this, "Export to excell");

                    msg = msg + ex.LogException("Saving Excel file [" + fileInfo.FullName + "]", "DataTable");

                    msg.saveStringToFile(folder.pathFor(fl, getWritableFileMode.none), getWritableFileMode.autoRenameExistingToBack);
                }

                output = fileInfo.FullName;
                return output;
            }
            catch (Exception ex2)
            {
                msg = msg + ex2.LogException("Saving Excel file [" + fileInfo.FullName + "]", "DataTable");

                msg.saveStringToFile(folder.pathFor(fl, getWritableFileMode.none), getWritableFileMode.autoRenameExistingToBack);
            }

            //fi.Delete();
            if (fileInfo != null)
            {
                return fileInfo.FullName;
            }
            else
            {
                return "error";
            }
        }

        /// <summary>
        /// Applies the object table template.
        /// </summary>
        public void ApplyObjectTableTemplate()
        {
            if (metaRowInfo == null) metaRowInfo = new DataRowMetaDictionary();
            if (metaColumnInfo == null) metaColumnInfo = new DataColumnMetaDictionary();

            metaRowInfo.Add(DataRowInReportTypeEnum.mergedHeaderTitle, templateFieldDataTable.title);
            metaRowInfo.Add(DataRowInReportTypeEnum.mergedHeaderInfo, templateFieldDataTable.description);

            metaRowInfo.Add(DataRowInReportTypeEnum.mergedHorizontally, templateFieldDataTable.col_group);
            metaRowInfo.Add(DataRowInReportTypeEnum.columnCaption, templateFieldDataTable.col_caption);
            metaRowInfo.Add(DataRowInReportTypeEnum.columnDescription, templateFieldDataTable.col_letter);
            metaRowInfo.Add(DataRowInReportTypeEnum.columnDescription, templateFieldDataTable.col_unit);

            metaRowInfo.Add(DataRowInReportTypeEnum.columnFooterInfo, templateFieldDataTable.col_desc);

            metaRowInfo.Add(DataRowInReportTypeEnum.mergedFooterInfo, templateFieldDataTable.table_extraDesc);

            //   metaColumnInfo.Add(DataColumnInReportTypeEnum.infoOnLeft, DataColumnInfoSourceEnum.rowName);
            //  metaColumnInfo.Add(DataColumnInReportTypeEnum.infoOnRight, DataColumnInfoSourceEnum.rowDescription);

            // this.AddLineRow();

            //this.setColumnWidths(100);

            // this.AddLineRow();
            //   this.AddExtraRowInfo(templateFieldDataTable.col_group).SetName("Group").SetDesc("Group/Category of the property");
            //  this.AddExtraRowInfo(templateFieldDataTable.col_caption).SetName("Name").SetDesc("Descriptive name for the data point");
            //  output.AddExtraRowInfo(templateFieldDataTable.col_name).SetName("Code name").SetDesc("Code name used in the source code");
            // this.AddExtraRowInfo(templateFieldDataTable.col_letter).SetName("Notation").SetDesc("Letter-code notation used in the article");
            // this.AddExtraRowInfo(templateFieldDataTable.col_desc).SetName("Description").SetDesc("Description of the value columns below");

            //  this.AddLineRow();
            //  this.AddExtraLinesAsRows();

            //this.AddRow("Max. frequency").Set(1, max.ToString()).SetDesc("The highest number of occurences in the document");
            //this.AddRow("Max. cumulative weight").Set(1, maxWeight.ToString()).SetDesc("The highest cumulative weight in the document");
        }

        //public DataTableForStatistics():base()
        //{
        //}
    }
}