// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbEPPlusExtensions.cs" company="imbVeles" >
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

/// <summary>
/// EEPlus Table documents creation api
/// </summary>
/// \ingroup_disabled report_ll
namespace imbSCI.DataComplex.extensions.data
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using OfficeOpenXml;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Extensions for EPPlus
    /// </summary>
    /// \ingroup_disabled report_ll
    public static class imbEPPlusExtensions
    {
        /// <summary>
        /// Sets the data to header footer.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="dataMeta">The data meta.</param>
        public static void setDataToHeaderFooter(this ExcelWorksheet worksheet, PropertyCollection dataMeta)
        {
            // ExcelHeaderFooterText hf = new ExcelHeaderFooterText();

            worksheet.HeaderFooter.differentOddEven = false;
            worksheet.HeaderFooter.differentFirst = false;
            worksheet.HeaderFooter.AlignWithMargins = true;

            worksheet.HeaderFooter.FirstHeader.LeftAlignedText = dataMeta.getProperString(templateFieldBasic.page_title, templateFieldBasic.document_title, templateFieldBasic.document_title, templateFieldBasic.test_caption, templateFieldBasic.meta_softwareName);  // meta.header.name;
            worksheet.HeaderFooter.FirstHeader.RightAlignedText = dataMeta.getProperString(templateFieldBasic.test_runstamp, templateFieldBasic.sci_projectname, templateFieldBasic.sys_start); // meta.notation.softwareName;
            worksheet.HeaderFooter.FirstHeader.CenteredText = dataMeta.getProperString(templateFieldBasic.test_caption);

            worksheet.HeaderFooter.FirstFooter.LeftAlignedText = dataMeta.getStringLine(" - ", templateFieldBasic.meta_author, templateFieldBasic.meta_organization); // meta.notation.author.add(" - ").add(meta.notation.organization);
            worksheet.HeaderFooter.FirstFooter.RightAlignedText = dataMeta.getStringLine(" - ", templateFieldBasic.sys_time, templateFieldBasic.sci_projectname);// dataMeta[templateFieldBasic.sys_app].ToString().add(" - ").a(dataMeta[templateFieldBasic.meta_copyright].ToString());  // meta.notation.copyright;
            worksheet.HeaderFooter.FirstFooter.CenteredText = dataMeta.getStringLine(" - ", templateFieldBasic.meta_copyright, templateFieldBasic.meta_year);  //dataMeta[templateFieldBasic.sys_time].ToString().add(" - ").a(dataMeta[templateFieldBasic.sci_projectname].ToString());
        }

        /// <summary>
        /// Deploys meta information (properties and custom properties) into workbook.
        /// </summary>
        /// <param name="workbook">The workbook to apply data into</param>
        /// <param name="dataMeta">The data meta collection to use as data source</param>
        /// <param name="includeBasic">What fields from templateFieldBasic collection to add as CustomProperties of document</param>
        /// <param name="includeCP">What fields from templateFieldBasic collection to add as CustomProperties of document</param>
        /// <param name="extraFields">The extra fields to add as CustomProperties of the document</param>
        public static void setDataInfoStandard(this ExcelWorkbook workbook, PropertyCollection dataMeta)
        {
            workbook.Properties.Title = dataMeta.getProperString(templateFieldBasic.document_title, templateFieldBasic.page_title, templateFieldBasic.meta_softwareName, templateFieldBasic.self_title, templateFieldBasic.sci_projectname); // [];
            workbook.Properties.Subject = dataMeta.getProperString("Table sheet report", templateFieldBasic.document_desc, templateFieldBasic.meta_desc, templateFieldBasic.meta_subtitle, templateFieldBasic.sci_projectdesc); // meta.header.content.toCsvInLine();
            workbook.Properties.Author = dataMeta.getProperString("Goran Grubic", templateFieldBasic.meta_author, templateFieldBasic.sys_app);
            workbook.Properties.Company = dataMeta.getProperString("KOPLAS PRO doo", templateFieldBasic.meta_organization);
            workbook.Properties.Application = dataMeta.getProperString("imbVeles/ACE", templateFieldBasic.sys_app, templateFieldBasic.meta_organization);
            workbook.Properties.Keywords = dataMeta.getProperString("", templateFieldBasic.meta_keywords);
            workbook.Properties.Created = DateTime.Now;
            workbook.Properties.Comments = dataMeta.getProperString(templateFieldBasic.documentset_desc, templateFieldBasic.test_description, templateFieldBasic.sci_projectdesc, templateFieldBasic.self_desc, templateFieldBasic.sci_projectdesc);
        }

        /// <summary>
        /// Deploys custom meta information (custom properties) into workbook. If field not found in dataMeta it will not create custom property for it.
        /// </summary>
        /// <param name="workbook">The workbook to apply data into</param>
        /// <param name="dataMeta">The data meta collection to use as data source</param>
        /// <param name="extraFields">The extra fields to add as CustomProperties of the document</param>
        public static void setDataInfoCustomProperties(this ExcelWorkbook workbook, PropertyCollection dataMeta, params object[] extraFields)
        {
            object[] fields = extraFields.getFlatArray<object>();

            foreach (object key in fields)
            {
                object val = dataMeta.get(key);
                if (val != null) workbook.Properties.SetCustomPropertyValue(key.ToString(), val);
            }
        }

        /// <summary>
        /// Sets the value in cell on current cursor position
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        /// <param name="cur">The current.</param>
        /// <param name="value">The value.</param>
        /// <param name="doEnter">if set to <c>true</c> [do enter].</param>
        /// <param name="afterDirection">The after direction.</param>
        /// <returns>Returns <c>ExcelRange</c> pointing to cell affected</returns>
        public static ExcelRange SetValue(this ExcelWorksheet sheet, cursor cur, object value, bool doEnter = false, textCursorZoneCorner afterDirection = textCursorZoneCorner.Bottom)
        {
            ExcelRange cellAddress = sheet.Cells[cur.y + 1, cur.x + 1];
            cellAddress.Value = value;

            //sheet.SetValue(cellAddress, value);
            //sheet.SetValue(cur.y + 1, cur.x + 1, value);
            cur.moveInDirection(afterDirection);
            if (doEnter) cur.enter();
            return cellAddress;
        }

        public static ExcelRange GetCell(this ExcelWorksheet sheet, cursor cur)
        {
            ExcelRange cellAddress = sheet.Cells[cur.y + 1, cur.x + 1];
            return cellAddress;
        }

        public static ExcelRange GetPen(this ExcelWorksheet sheet, cursor cur)
        {
            ExcelRange cellAddress = sheet.Cells[cur.y + 1, cur.x + 1];

            var pencil = cur.pencilAbsolute;

            cellAddress = sheet.Cells[pencil.TopLeft.x, pencil.TopLeft.y, pencil.BottomRight.x, pencil.BottomRight.y];

            return cellAddress;
        }

        /// <summary>
        /// Set values into cells, respecting <c>direction</c>. Returns <c>ExcelRange</c> pointing to cells affected.
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        /// <param name="cur">The current.</param>
        /// <param name="value">The value.</param>
        /// <param name="doEnter">if set to <c>true</c> [do enter].</param>
        /// <param name="direction">The direction.</param>
        /// <returns>Returns <c>ExcelRange</c> pointing to cells affected</returns>
        public static ExcelRange SetValues(this ExcelWorksheet sheet, cursor cur, IEnumerable value, bool doEnter = false, textCursorZoneCorner direction = textCursorZoneCorner.Bottom)
        {
            int _y = cur.y + 1;
            int _x = cur.x + 1;
            //ExcelRange range = sheet.Cells[cur.y + 1, cur.x + 1];
            foreach (object vl in value)
            {
                ExcelRange cellAddress = sheet.Cells[cur.y + 1, cur.x + 1];
                cellAddress.Value = value;
                //sheet.SetValue(cur.y + 1, cur.x + 1, value);
                cur.moveInDirection(direction);
            }
            ExcelRange range = sheet.Cells[_y, _x, cur.y + 1, cur.x + 1];

            if (doEnter) cur.enter();
            return range;
        }

        public static void InsertRow(this ExcelWorksheet worksheet, cursor cur, int rows)
        {
            worksheet.InsertRow(cur.y + 1, rows);
        }

        public static void InsertArea(this ExcelWorksheet worksheet, cursor cur, selectRangeArea area)
        {
            worksheet.InsertColumn(cur.x + 1, area.width);
            worksheet.InsertRow(cur.y + 1, area.height);
        }

        /// <summary>
        /// Inserts DataTable content at current currsor position. Options: doInsertCaptions, doInsertRowID columnprefix (this is abanded)
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="cur">The current.</param>
        /// <param name="data">The data.</param>
        /// <param name="doInsertCaptions">if set to <c>true</c> [do insert captions].</param>
        /// <param name="doInsertRowId">if set to <c>true</c> [do insert row identifier].</param>
        /// <returns>Range of newly populated cells, including automatically created extra row and column</returns>
        public static ExcelRange SetDataTable(this ExcelWorksheet worksheet, cursor cur, DataTable data, cursorVariatorHeadFootFlags headFootFlags, cursorVariatorOddEvenFlags oddEvenFlags)
        {
            int twidth = data.Columns.Count;
            int theight = data.Rows.Count;

            bool doInsertCaptions = false;

            bool doInsertRowId = false;

            if (doInsertCaptions) theight++;

            if (doInsertRowId) twidth++;

            cur.setTempFrame(twidth, theight, textCursorZoneCorner.UpLeft, 0, 0); //.//.switchToZone(textCursorZone.innerZone, textCursorZoneCorner.UpLeft);
            cur.switchToZone(textCursorZone.innerZone);

            if (doInsertCaptions)
            {
                if (doInsertRowId)
                {
                    SetValue(worksheet, cur, "#", false, textCursorZoneCorner.Right);
                }
                foreach (DataColumn dc in data.Columns)
                {
                    SetValue(worksheet, cur, dc.Caption, false, textCursorZoneCorner.Right);
                }
                cur.enter();
            }

            foreach (DataRow rw in data.Rows)
            {
                if (doInsertRowId)
                {
                    SetValue(worksheet, cur, data.Rows.IndexOf(rw), false, textCursorZoneCorner.Right);
                }
                foreach (DataColumn dc in data.Columns)
                {
                    SetValue(worksheet, cur, rw[dc], false, textCursorZoneCorner.Right);
                }
                cur.enter();
            }

            var area = cur.selectZoneArea();

            cur.backToMainFrame();

            return area.getExcelRange(worksheet);
        }

        ///// <summary>
        ///// Builds plain (no format) workbook inside provided excel object and adds it into workbook set
        ///// </summary>
        ///// <param name="data">DataTable with rows and columns</param>
        ///// <param name="doInsertCaptions">if TRUE the first row will be DataColumn.Caption</param>
        ///// <param name="excel">ExcelPackage or ExcelWorkbook object. If null it will create new</param>
        ///// <returns>ExcelPackage - from parameter or newly instatiated</returns>
        //public static ExcelPackage buildPlainWorkbook(this IDocumentRender builder, DataTable data, PropertyCollection dataMeta, ExcelPackage excel = null)
        //{
        //    if (excel == null)
        //    {
        //        excel = new ExcelPackage();
        //    }

        //    //cursor cur = data.buildCursorAndZone(0, 1, 0, 0);

        //    String wkn = excel.Workbook.Worksheets.getUniqueName(data.TableName);

        //    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(wkn);

        //    worksheet.DefaultColWidth = 50.toExcelWidth();

        //    cur.switchToZone(textCursorZone.outterZone, textCursorZoneCorner.UpLeft);

        //    worksheet.SetValue(cur, meta.header.description, true);
        //    worksheet.SetValues(cur, meta.header.content, true, textCursorZoneCorner.Bottom);

        //    cur.setMarginHere(textCursorZoneCorner.Top);

        //    cur.switchToZone(textCursorZone.innerZone, textCursorZoneCorner.UpLeft);

        //    if (meta.doInsertCaptions)
        //    {
        //        foreach (DataColumn dc in data.Columns)
        //        {
        //            worksheet.SetValue(cur, dc.Caption, false, textCursorZoneCorner.Right);
        //        }
        //        cur.enter();
        //    }

        //    foreach (DataRow rw in data.Rows)
        //    {
        //        foreach (DataColumn dc in data.Columns)
        //        {
        //            worksheet.SetValue(cur, rw[dc], false, textCursorZoneCorner.Right);
        //        }
        //        cur.enter();
        //    }

        //    cur.switchToZone(textCursorZone.outterZone, textCursorZoneCorner.default_corner);

        //    worksheet.SetValues(cur, meta.footer.content, true);
        //    if (meta.keywords.content.Any())
        //    {
        //        cur.enter();
        //        worksheet.SetValue(cur, "Keywords:", false, textCursorZoneCorner.Right);
        //        worksheet.SetValues(cur, meta.keywords.content, true, textCursorZoneCorner.Right);
        //    }
        //    cur.enter();
        //    worksheet.SetValue(cur, meta.footer.bottomLine, true);

        //    return excel;
        //}

        //public static ExcelPackage createExcel(this String filename_base)
        //{
        //    ExcelPackage output = new ExcelPackage();

        //}

        public static ExcelPackage saveOutput(this ExcelPackage excel, DirectoryInfo di, string filename_base, getWritableFileMode mode = getWritableFileMode.overwrite, params reportOutputFormatName[] formats)
        {
            object output = "";

            reportOutputSupport support = reportOutputSupport.getFormatSupportFor(reportAPI.EEPlus, filename_base);

            var supported = support.checkSupport(true, formats);

            string filename = "";
            FileInfo fi = null;

            output = excel;

            foreach (reportOutputFormatName format in supported)
            {
                filename_base.add(support[format].toStringSafe(), ".");

                fi = support[format].toStringSafe().getWritableFile(mode);
                // File file = fi.OpenWrite();

                switch (format)
                {
                    case reportOutputFormatName.sheetCsv:
                        byte[] by = EpplusCsvConverter.ConvertToCsv(excel);
                        File.WriteAllBytes(fi.FullName, by);
                        break;

                    case reportOutputFormatName.sheetExcel:

                        excel.File = fi;
                        excel.Save();
                        break;

                    case reportOutputFormatName.sheetHtml:
                        //excel.Workbook.Worksheets[0]
                        break;

                    case reportOutputFormatName.sheetXML:

                        excel.Workbook.WorkbookXml.OuterXml.saveStringToFile(fi.FullName, mode);

                        // File.WriteAllText(fi.FullName, excel.Workbook.WorkbookXml.OuterXml);

                        break;

                    case reportOutputFormatName.sheetPDF:
                        break;
                }
            }
            return excel;
        }

        public static ExcelRange getExcelRange(this ExcelWorksheet worksheet, ExcelRow row, selectZone zone)
        {
            return worksheet.Cells[row.Row + zone.y, zone.x + 1, row.Row + zone.y + zone.height, zone.weight + zone.x + 1];
        }

        /// <summary>
        /// Gets the Escel range from worksheet.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="area">The area.</param>
        /// <returns></returns>
        public static ExcelRange getExcelRange(this selectRangeArea area, ExcelWorksheet worksheet)
        {
            area = area.normalizeRange();

            ExcelRange range = null;
            try
            {
                range = worksheet.Cells[area.TopLeft.y + 1, area.TopLeft.x + 1, area.BottomRight.y + 1, area.BottomRight.x + 1];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return range;
        }

        /// <summary>
        /// Gets the name of the unique.
        /// </summary>
        /// <param name="wks">The Worksheets collection to make name for</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string getUniqueName(this ExcelWorksheets wks, string input)
        {
            input = input.toWidthMaximum(8, " ");

            int i = 0;
            List<string> used = new List<string>();
            foreach (ExcelWorksheet wk in wks)
            {
                used.Add(wk.Name);
            }
            input = input.makeUniqueName(used);
            return input;
        }
    }
}