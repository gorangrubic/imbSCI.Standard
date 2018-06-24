// --------------------------------------------------------------------------------------------------------------------
// <copyright file="settingsEntriesTools.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting
{
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    public static class settingsEntriesTools
    {
        /// <summary>
        /// Gets the user manual saved:
        /// </summary>
        /// <param name="dataObject">The data object to describe</param>
        /// <param name="path">The path.</param>
        /// <param name="directInfo">Custom description line to be inserted after description</param>
        /// <param name="skipUnDescribed">if set to <c>true</c> [skip un described].</param>
        /// <param name="showValue">if set to <c>true</c> [show value].</param>
        public static void GetUserManualSaved(this Object dataObject, String path, String directInfo = "", Boolean skipUnDescribed = true, Boolean showValue = true)
        {
            builderForMarkdown mdb = new builderForMarkdown();

            dataObject.GetUserManual(mdb, directInfo, skipUnDescribed, showValue);

            FileInfo fi = path.getWritableFile();

            String output = mdb.ToString(false);
            output.saveStringToFile(fi.FullName);
        }

        /// <summary>
        /// Gets the user manual for table saved.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="path">The path.</param>
        /// <param name="directInfo">The direct information.</param>
        /// <param name="skipUnDescribed">if set to <c>true</c> [skip un described].</param>
        /// <param name="showValue">if set to <c>true</c> [show value].</param>
        public static void GetUserManualForTableSaved(this DataTable dataObject, String path, String directInfo = "", Boolean skipUnDescribed = true, Boolean showValue = true)
        {
            builderForMarkdown mdb = new builderForMarkdown();

            dataObject.GetUserManualForTable(mdb, directInfo, skipUnDescribed);

            FileInfo fi = path.getWritableFile();

            String output = mdb.ToString(false);
            output.saveStringToFile(fi.FullName);
        }

        public static void AppendIf(this ITextRender output, String content, String prefix = "")
        {
            if (content.isNullOrEmpty()) return;
            output.AppendLine(prefix + content);
        }

        /// <summary>
        /// Gets the user manual for table.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="output">The output.</param>
        /// <param name="directInfo">The direct information.</param>
        /// <param name="skipUnDescribed">if set to <c>true</c> [skip un described].</param>
        public static void GetUserManualForTable(this DataTable dataObject, ITextRender output, String directInfo = "", Boolean skipUnDescribed = true)
        {
            output.AppendHeading("DataTable: " + dataObject.GetTitle(), 1);

            AppendIf(output, directInfo, "Comment: ");
            AppendIf(output, dataObject.GetDescription(), "Table description: ");

            output.AppendLine("This is automatically generated description of columns of DataTable [" + dataObject.TableName + "]");
            output.AppendHorizontalLine();

            AppendIf(output, dataObject.GetClassName(), "Table shema source: ");
            //output.AppendIf(dataObject.Rows.Count, "Table shema source: ");

            foreach (DataColumn dc in dataObject.Columns)
            {
                //output.AppendHeading("DataColumn -> [" + dc.ColumnName + "]", 2);

                var spe = dc.ExtendedProperties[templateFieldDataTable.col_spe] as settingsPropertyEntry;
                if (spe == null)
                {
                    spe = new settingsPropertyEntry(dc);
                    //spe.displayName = dc.Caption;
                    //spe.name = dc.ColumnName;
                    //spe.description = dc.GetDesc();
                    //spe.type = dc.DataType;
                    //spe.categoryName = dc.GetGroup();
                    //spe.format = dc.GetFormat();
                    //spe.letter = dc.GetLetter();
                    //spe.aggregation = dc.GetAggregation();
                    ////spe.index = dc.Ordinal;
                    //spe.importance = dc.GetImportance();
                    //spe.expression = dc.Expression;
                    //spe.unit = dc.GetUnit();
                    //spe.priority = dc.Ordinal;
                    //spe.width = dc.GetWidth();
                }

                GetUserManualSPE(spe, output);

                // dc.GetSPE().GetUserManual(output);
            }

            output.AppendHorizontalLine();

            var addInfo = dataObject.GetAdditionalInfo();
            output.AppendPairs(addInfo, false, " => ");

            var extraLines = dataObject.GetExtraDesc();

            if (Enumerable.Any<string>(extraLines))
            {
                output.AppendHorizontalLine();

                output.AppendList(extraLines, true);
            }

            //dataObject.GetDescription();
        }

        /// <summary>
        /// Generates content for <see cref="settingsPropertyEntry"/>
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <param name="output">The output.</param>
        public static void GetUserManualSPE(this settingsPropertyEntry spec, ITextRender output)
        {
            output.AppendHeading(imbSciStringExtensions.add(spec.categoryName, spec.displayName, ": "), 3);
            output.AppendPair("Property name: ", spec.name);
            output.AppendPair("Type: ", spec.type.Name);

            if (!spec.description.isNullOrEmpty())
            {
                output.AppendParagraph(spec.description);
            }

            if (!spec.letter.isNullOrEmpty()) output.AppendPair("Annotation: ", spec.letter);
            if (!spec.unit.isNullOrEmpty()) output.AppendPair("Unit: ", spec.unit);
            if (spec.aggregation != null) output.AppendPair("Aggregation: ", spec.aggregation.multiTableType.ToString());

            //if (showValue)
            //{
            //    if (spec.value != null)
            //    {
            //        output.AppendPair("Value: ", spec.value.toStringSafe(spec.format));
            //    }
            //}

            if (spec.type.IsEnum)
            {
                output.AppendPair("Possible values: ", spec.type.GetEnumNames().toCsvInLine());
            }
            else if (spec.type == typeof(Int32))
            {
            }

            if (!spec.info_helpTitle.isNullOrEmpty())
            {
                output.AppendLabel(spec.info_helpTitle);
                output.AppendQuote(spec.info_helpTips);
            }

            if (!spec.info_link.isNullOrEmpty())
            {
                output.AppendLink(spec.info_link, "More", "More");
            }

            output.AppendHorizontalLine();
        }

        /// <summary>
        /// Generates property manual
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="output">The output.</param>
        public static void GetUserManual(this Object dataObject, ITextRender output, String directInfo = "", Boolean skipUnDescribed = true, Boolean showValue = true)
        {
            settingsEntriesForObject seo = new settingsEntriesForObject(dataObject, false, false);

            String heading = "";
            if (!seo.Category.isNullOrEmpty())
            {
                heading += seo.Category + ": ";
            }

            output.AppendHeading(heading + seo.DisplayName, 2);

            List<String> description = new List<string>();

            if (!seo.Description.isNullOrEmpty())
            {
                output.AppendSection(seo.Description, "Class: " + dataObject.GetType().Name, dataObject.GetType().Namespace, seo.additionalInfo);
            }

            if (!directInfo.isNullOrEmpty())
            {
                output.AppendComment(directInfo);
            }

            var list = seo.spes.Values.ToList().OrderBy(x => x.categoryName);

            foreach (settingsPropertyEntryWithContext spec in list)
            {
                if (spec.description.isNullOrEmpty() && skipUnDescribed)
                {
                }
                else
                {
                    output.AppendHeading(imbSciStringExtensions.add(spec.categoryName, spec.displayName, ": "), 3);
                    output.AppendPair("Property name: ", spec.pi.Name);
                    output.AppendPair("Type: ", spec.pi.PropertyType.Name);

                    if (!spec.description.isNullOrEmpty())
                    {
                        output.AppendParagraph(spec.description);
                    }

                    if (!spec.letter.isNullOrEmpty()) output.AppendPair("Annotation: ", spec.letter);
                    if (!spec.unit.isNullOrEmpty()) output.AppendPair("Unit: ", spec.unit);
                    if (spec.aggregation != null) output.AppendPair("Aggregation: ", spec.aggregation.multiTableType.ToString());

                    if (showValue)
                    {
                        if (spec.value != null)
                        {
                            output.AppendPair("Value: ", spec.value.toStringSafe(spec.format));
                        }
                    }

                    if (spec.type.IsEnum)
                    {
                        output.AppendPair("Possible values: ", spec.type.GetEnumNames().toCsvInLine());
                    }
                    else if (spec.type == typeof(Int32))
                    {
                    }

                    if (!spec.info_helpTitle.isNullOrEmpty())
                    {
                        output.AppendLabel(spec.info_helpTitle);
                        output.AppendQuote(spec.info_helpTips);
                    }

                    if (!spec.info_link.isNullOrEmpty())
                    {
                        output.AppendLink(spec.info_link, "More", "More");
                    }

                    output.AppendHorizontalLine();
                }
            }

            if (!seo.info_helpTitle.isNullOrEmpty())
            {
                output.AppendLabel(seo.info_helpTitle);
                output.AppendQuote(seo.info_helpTips);
            }

            if (!seo.info_link.isNullOrEmpty())
            {
                output.AppendLink(seo.info_link, "More", "More");
            }
        }
    }
}