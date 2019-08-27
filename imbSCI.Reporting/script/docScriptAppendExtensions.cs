// --------------------------------------------------------------------------------------------------------------------
// <copyright file="docScriptAppendExtensions.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.script
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.render.converters;
    using imbSCI.Data;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.reporting;
    using imbSCI.DataComplex.extensions;
    using imbSCI.DataComplex.extensions.data;
    using imbSCI.DataComplex.extensions.data.operations;
    using imbSCI.DataComplex.extensions.data.schema;
    using imbSCI.DataComplex.extensions.text;
    using imbSCI.Reporting.exceptions;
    using imbSCI.Reporting.interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public static class docScriptAppendExtensions
    {
        public static bool doAllowExcelAttachments = true;
        public static bool doAllowJSONAttachments = true;

        /// <summary>
        /// Titles to filename.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        public static string titleToFilename(string title)
        {
            return title.getCleanFilepath().getCleanPropertyName().Replace(" ", "").ToLower();
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'aceReportException' that could not be resolved
        /// <summary>
        /// Appends the macro simple table.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="source">The source.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        /// <exception cref="imbSCI.Cores.core.exceptions.aceReportException">Can't append table with 0 columns, 0 rows and without title - null - Macro: DataTable block failed</exception>
        public static DataTable AppendMacroSimpleTable(this ITextAppendContentExtended script, DataTable source, bool throwException = true, macroOptions options = macroOptions.common)
#pragma warning restore CS1574 // XML comment has cref attribute 'aceReportException' that could not be resolved
        {
            if (!source.validateTable())
            {
                if (throwException) throw new aceReportException("Can't append table with 0 columns, 0 rows and without title. Macro: DataTable block failed");
                return null;
            }

            if (options.HasFlag(macroOptions.wrapp))
            {
                script.open(nameof(bootstrap_containers.panel), source.GetTitle(), source.GetDescription());
            }
            else
            {
                script.AppendHeading(source.GetTitle(), 3);
                script.AppendLine(source.GetDescription());
            }
            script.AppendTable(source);

            if (options.HasFlag(macroOptions.wrapp))
            {
                script.close();
            }

            if (options.HasFlag(macroOptions.legend))
            {
                AppendMacroLegend(script, source);
            }

            DataTable output = source;

            if (output.validateTable() && options.HasFlag(macroOptions.download))
            {
                //script.AppendTable(output, true);
                //script.AppendParagraph(output.GetDescription());

                script.open(nameof(bootstrap_containers.buttongroup), "", "");

                if (doAllowExcelAttachments) script.Attachment(source, dataTableExportEnum.excel, "Download Excel", bootstrap_color.primary, bootstrap_size.xs);

                script.close();
            }
            else
            {
                script.AppendLabel("Empty data table", true, bootstrap_style.style_warning);
            }

            return source;
        }

        /// <summary>
        /// Appends the macro legend.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="table">The table.</param>
        public static void AppendMacroLegend(this ITextAppendContentExtended script, DataTable table)
        {
            var name = "Legend";
            var description = "Description of columns shown on table `" + table.GetTitle() + "`";
            script.AppendHeading(name, 4);

            script.AppendLine("");

            foreach (DataColumn dc in table.Columns)
            {
                string item_name = dc.GetHeading().Trim('_');
                string item_desc = dc.GetDesc();
                if (!item_desc.isNullOrEmpty())
                {
                    script.Append(" +   ", appendType.direct, false); //, appendType.bypass, false);
                    dataPointImportance importance = dc.GetImportance();

                    string item_letter = dc.GetLetter();

                    script.Append(item_name, appendType.bold);
                    if (!item_letter.isNullOrEmpty()) script.Append(" ( " + item_letter + " ) ");

                    script.AppendLine("     _" + dc.GetDesc() + "_");
                }
            }

            script.AppendLine(description);
        }

        /// <summary>
        ///
        /// </summary>
        public static int defaultRowLimit { get; set; } = 10;

        /// <summary>
        /// Shows first <c>rowLimit</c> and provides exported versions
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="source">The source.</param>
        /// <param name="title">The title.</param>
        /// <param name="rowLimit">The row limit. -1 to disable row limit, 0 to use default</param>
        /// <returns></returns>
        public static DataTable AppendMacroDataTableBlock(this ITextAppendContentExtended script, DataTable source, string title = "", int rowLimit = -1, macroOptions options = macroOptions.common)
        {
            if (rowLimit == -1) rowLimit = int.MaxValue;
            if (rowLimit == 0) rowLimit = defaultRowLimit;
            int rc = Math.Min(source.Rows.Count, rowLimit);
            string description = "";
            if (source.Rows.Count < rowLimit)
            {
                description = "DataTable: `" + source.GetTitle() + "` with (" + source.Rows.Count + ") rows.";
            }
            else
            {
                description = "Excerpt from datatable: `" + source.GetTitle() + "` limited to (" + rc + ") rows. Total row count:" + source.Rows.Count + "). For complete content open exported file.";
            }

            if (options.HasFlag(macroOptions.wrapp))
            {
                script.open("well", source.GetTitle(title), "");
            }
            else
            {
                script.AppendHeading(source.GetTitle(), 3);
            }

            DataTable output = source.GetLimited(rc);

            if (output.validateTable())
            {
                script.AppendTable(output, true);
                //script.AppendParagraph(output.GetDescription());

                /*

                    */
            }
            else
            {
                script.AppendLabel("Empty data table", true, bootstrap_style.style_warning);
            }

            script.AppendLine(source.GetDescription());
            script.AppendLine(description);
            if (options.HasFlag(macroOptions.wrapp))
            {
                script.close();
            }

            if (options.HasFlag(macroOptions.legend))
            {
                AppendMacroLegend(script, source);
            }

            if (options.HasFlag(macroOptions.download))
            {
                script.open(nameof(bootstrap_containers.buttongroup), "", "");

                script.Attachment(source, dataTableExportEnum.csv, "CSV", bootstrap_color.sucess, bootstrap_size.sm);
                if (doAllowExcelAttachments) script.Attachment(source, dataTableExportEnum.excel, "Excel", bootstrap_color.primary, bootstrap_size.sm);
                if (doAllowJSONAttachments) script.Attachment(source, dataTableExportEnum.json, "JSON", bootstrap_color.info, bootstrap_size.sm);

                script.close();
            }

            script.AppendHorizontalLine();

            return output;
        }

        /// <summary>
        /// Appends the macro download.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="source">The source.</param>
        public static void AppendMacroDownload(this ITextAppendContentExtended script, DataTable source)
        {
            script.open(nameof(bootstrap_containers.buttongroup), "", "");

            script.Attachment(source, dataTableExportEnum.csv, "CSV", bootstrap_color.sucess, bootstrap_size.sm);
            script.Attachment(source, dataTableExportEnum.excel, "Excel", bootstrap_color.primary, bootstrap_size.sm);
            script.Attachment(source, dataTableExportEnum.json, "JSON", bootstrap_color.info, bootstrap_size.sm);

            script.close();
        }

        /// <summary>
        /// Shows first <c>rowLimit</c> and provides exported versions
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="source">The source.</param>
        /// <param name="title">The title.</param>
        /// <param name="rowLimit">The row limit.</param>
        /// <returns></returns>
        public static void AppendMacroRecordLogFileBlock(this ITextAppendContentExtended script, ILogBuilder loger, string title, int lastLines = 20, macroOptions options = macroOptions.common)
        {
            string description = "";

            List<string> lines = loger.ContentToString().breakLines(true);
            int ln = Math.Min(lines.Count(), lastLines);
            if (ln == 0) return;

            lines.Reverse();

            //if (lines.Count < ln)
            //{
            //    description = "Log content from `" + loger.outputPath + "` with (" + ln + ") lines.";
            //}
            //else
            //{
            //    description = "Excerpt from log `" + loger.outputPath + "` limited to last (" + ln + ") lines. Total log count:" +  lines.Count + "). For complete content open one of files below.";
            //}

            if (options.HasFlag(macroOptions.wrapp))
            {
                script.open(nameof(bootstrap_containers.well), title, description);
            }
            else
            {
                script.AppendHeading(title, 4);
            }

            script.AppendLine("Log contains [" + lines.Count() + "] lines - the last [" + ln + "] is included below.");

            DataTable dr = lines.buildDataTable(imbStringCollectionExtensions.buildDataTableOptions.addCounterColumn | imbStringCollectionExtensions.buildDataTableOptions.extractExceptions, "Log");

            if (dr.validateTable())
            {
                DataTable output = dr.GetLimited(ln);

                output.SetDescription(description);

                script.AppendTable(output);

                script.AppendHorizontalLine();

                script.open(nameof(bootstrap_containers.buttontoolbar), "Format options", "");

                script.open(nameof(bootstrap_containers.buttongroup), "Data", "");

                script.Attachment(dr, dataTableExportEnum.csv, "CSV", bootstrap_color.info, bootstrap_size.sm);
                script.Attachment(dr, dataTableExportEnum.excel, "Excel", bootstrap_color.primary, bootstrap_size.sm);
                script.Attachment(dr, dataTableExportEnum.json, "JSON", bootstrap_color.info, bootstrap_size.sm);

                script.close();

                script.open(nameof(bootstrap_containers.buttongroup), "Text", "");

                script.Attachment(loger.ContentToString(), title.getFilename(".md"), "Markdown", bootstrap_color.gray, bootstrap_size.sm);

                script.close();

                script.close();
            }
            else
            {
                script.AppendLabel("The log is empty", true, bootstrap_color.warning);
            }

            if (options.HasFlag(macroOptions.wrapp))
            {
                script.close();
            }
            else
            {
                script.AppendLine(description);
            }
        }
    }
}