// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringHTMLExtensions.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.reporting.render.config;
using imbSCI.Data;
using imbSCI.Data.enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace imbSCI.Core.reporting.lowLevelApi
{
    /// \ingroup_disabled report_ll
    public static class imbStringHTMLExtensions
    {



        /// <summary>
        /// DataTable to markdown convertor
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string htmlTable(this DataTable table, String table_tag_attribute = "", Boolean printExtraLines = true)
        {

            String headingline = "";
            String dcCaption = "";

            List<DataColumn> skipEscape = new List<DataColumn>();

            dataValueFormatDictionary dcfs = new dataValueFormatDictionary();



            StringBuilder sb = new StringBuilder();
            if (!table_tag_attribute.isNullOrEmpty())
            {
                sb.AppendLine("<table class=\"" + table_tag_attribute + "\" >");
            }
            else
            {
                sb.AppendLine("<table " + table_tag_attribute + ">");
            }


            sb.AppendLine("<tbody>");

            headingline = "<tr>";
            foreach (DataColumn dc in table.Columns)
            {
                dcCaption = dc.GetHeading();
                if (dcCaption.StartsWith("_"))
                {
                    skipEscape.Add(dc);
                    dcCaption = dcCaption.TrimStart('_');
                    dc.Caption = dcCaption;
                }

                //dataValueFormatInfo dcf = new dataValueFormatInfo(dc);
                //if (dcf.directAppend) skipEscape.Add(dc);
                //dcfs.Add(dc.ColumnName, dcf);

                headingline += "<td>" + dcCaption + "</td>";

            }
            headingline += "</tr>";

            sb.AppendLine(headingline);



            //output = output + Environment.NewLine;

            foreach (DataRow dr in table.Rows)
            {
                sb.Append("<tr>");

                foreach (DataColumn dc in table.Columns)
                {
                    sb.Append("<td>");

                    String format = dc.GetFormat();
                    if (format != "")
                    {

                    }

                    String content = dr[dc].toStringSafe("",format);

                    sb.Append(content);


                    sb.Append("</td>");
                }

                sb.Append("</tr>");
            }
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            if (printExtraLines)
            {
                if (table.ExtraLinesCount() > 0)
                {
                    sb.AppendLine("<div><p>");

                    var lines = table.GetExtraDesc();
                    foreach (String ln in lines)
                    {
                        sb.AppendLine(ln + "<br/>");
                    }
                    sb.AppendLine("<p></div>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates list from collection of [strings, IEnumerable or Objects]
        /// </summary>
        /// <param name="list">IEnumerable may contain strings, IEnumerable or Object.toStringSafe()</param>
        /// <param name="isOrderedList">Is numeric or button list</param>
        /// <param name="sb">String builder</param>
        /// <param name="level">What is current level of list</param>
        /// <returns>Well formed markdown list</returns>
        public static string HtmlList(IEnumerable list, Boolean isOrderedList = false, StringBuilder sb = null, Int32 level = 0)
        {
            if (sb == null) sb = new StringBuilder();

            Int32 i = 0;
            String levelPrefix = "";

            foreach (var dc in list)
            {
                i++;
                if (dc is String dcs)
                {

                    sb.AppendLine("<li>" + dcs + "</li>");


                }
                else if (dc is IEnumerable dce)
                {
                    if (isOrderedList)
                    {
                        sb.AppendLine("<ol>");
                        HtmlList(dce, isOrderedList, sb, level + 1);
                        sb.AppendLine("</ol>");
                    }
                    else
                    {
                        sb.AppendLine("<ul>");
                        HtmlList(dce, isOrderedList, sb, level + 1);
                        sb.AppendLine("</ul>");
                    }


                }
                else
                {
                    String dco = dc.toStringSafe("");
                    sb.AppendLine("<li>" + dco + "</li>");
                }
            }
            if (level == 0) return sb.ToString();
            return "";
        }

    }
}