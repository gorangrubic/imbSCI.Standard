// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringTextExtensions.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.lowLevelApi
{
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;
    using imbSCI.Data.enums.appends;
    using System;
    using System.Collections;
    using System.Data;
    using System.Text;

    /// <summary>
    /// Markdown creation toolkit
    /// </summary>
    /// \ingroup_disabled report_ll
    public static class imbStringTextExtensions
    {
        /// <summary>
        /// Inverse call for markdownText extension
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content">Content</param>
        /// <returns>Markdown code</returns>
        public static String plainText(this appendType type, String content)
        {
            return content.plainText(type);
        }

        /// <summary>
        /// Deploying markdown wrapping syntax
        /// </summary>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string plainText(this String content, appendType type)
        {
            if (type != appendType.bypass) content = content.markdownEscape();

            switch (type)
            {
                case appendType.marked:
                    return "" + content + "[*]";
                    break;

                case appendType.squareQuote:
                    return "<<" + content + ">>";
                    break;

                case appendType.heading:
                case appendType.heading_1:
                case appendType.heading_2:
                    return content.ToUpper();
                    break;

                case appendType.quotation:
                    return imbSciStringExtensions.add(imbSciStringExtensions.add("\"", content, ""), "\"", "");

                case appendType.sourceXML:
                case appendType.sourceCS:
                case appendType.sourcePY:
                case appendType.sourceJS:
                case appendType.source:
                    return imbSciStringExtensions.add(imbSciStringExtensions.add("[code]", content, ""), "[/code]", "");

                case appendType.paragraph:
                    return Environment.NewLine.addLine(content);
                    break;

                case appendType.section:
                    String prefix = Environment.NewLine;
                    prefix += Environment.NewLine;

                    //return " > ".addLine(content);
                    break;

                case appendType.regular:
                    return content;
                    break;

                default:
                    return content;
                    break;
            }

            return content;
            //sb.AppendLine(linePrefix + tabInsert + content.ToUpper());
        }

        public static String nl = Environment.NewLine;
        public static String tb = "\t";

        /// <summary>
        /// DataTable to markdown convertor
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string textTable(this DataTable table, Int32 columnTabWidth = 4)
        {
            String output = "";
            String tab = tb.Repeat(columnTabWidth);

            foreach (DataColumn dc in table.Columns)
            {
                output = imbSciStringExtensions.add(output, imbSciStringExtensions.add(dc.Caption, tab, ""), "|");
            }
            output = imbSciStringExtensions.add(output, nl, "");
            output = imbSciStringExtensions.add(output, nl, "");
            foreach (DataRow dr in table.Rows)
            {
                foreach (DataColumn dc in table.Columns)
                {
                    String content = dr[dc].toStringSafe("");
                    output = imbSciStringExtensions.add(imbSciStringExtensions.add(output, content + tab, "|"), Environment.NewLine, "");
                }
            }

            return output;
        }

        /// <summary>
        /// Creates list from collection of [strings, IEnumerable or Objects]
        /// </summary>
        /// <param name="list">IEnumerable may contain strings, IEnumerable or Object.toStringSafe()</param>
        /// <param name="isOrderedList">Is numeric or button list</param>
        /// <param name="sb">String builder</param>
        /// <param name="level">What is current level of list</param>
        /// <returns>Well formed markdown list</returns>
        public static string textList(this IEnumerable list, Boolean isOrderedList = false, StringBuilder sb = null, Int32 level = 0)
        {
            if (sb == null) sb = new StringBuilder();

            Int32 i = 0;
            String levelPrefix = tb.Repeat(level);
            foreach (var dc in list)
            {
                i++;
                if (dc is String)
                {
                    String dcs = dc as String;
                    dcs = dcs.markdownEscape();
                    if (isOrderedList)
                    {
                        sb.AppendLine(levelPrefix + i.ToString() + ". " + dcs);
                    }
                    else
                    {
                        sb.AppendLine(levelPrefix + "* " + dcs);
                    }
                }
                else if (dc is IEnumerable)
                {
                    if (dc != list)
                    {
                        IEnumerable dce = dc as IEnumerable;
                        dce.textList(isOrderedList, sb, level + 1);
                    }
                }
                else
                {
                    String dco = dc.toStringSafe("");
                    dco = dco.markdownEscape();
                    if (isOrderedList)
                    {
                        sb.AppendLine(levelPrefix + i.ToString() + ". " + dco);
                    }
                    else
                    {
                        sb.AppendLine(levelPrefix + "* " + dco);
                    }
                }
            }
            if (level == 0) return sb.ToString();
            return "";
        }

        /// <summary>
        /// Creates markdown link, image, reference or referencedLink
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="caption"></param>
        /// <param name="linkType"></param>
        /// <returns></returns>
        public static string textLink(this String url, String name, String caption = "", appendLinkType linkType = appendLinkType.link)
        {
            //  url = url.markdownEscape();
            //name = name.markdownEscape();
            //caption = caption.markdownEscape();
            switch (linkType)
            {
                case appendLinkType.image:
                    return String.Format("Image [{0}]: {1}", name, url);
                    break;

                case appendLinkType.link:
                    if (!imbSciStringExtensions.isNullOrEmptyString(caption))
                    {
                        return String.Format("Link [{0}] = ({1} \"{2}\")", name, url, caption);
                    }
                    else
                    {
                        return String.Format("Link [{0}] = ({1})", name, url);
                    }
                    break;

                case appendLinkType.reference:
                    return String.Format("Ref [{0}]: {1}", name, url);
                    break;

                case appendLinkType.referenceLink:
                    return String.Format("Ref [{0}][{1}]", name, url);
                    break;

                default:
                    return url;
                    break;
            }
        }
    }
}