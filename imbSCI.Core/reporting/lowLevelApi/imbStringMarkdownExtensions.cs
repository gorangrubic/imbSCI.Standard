// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringMarkdownExtensions.cs" company="imbVeles" >
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
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.config;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    /// <summary>
    /// Markdown creation toolkit
    /// </summary>
    /// ingroup report_ll
    public static class imbStringMarkdownExtensions
    {
        //public static MarkdownPipeline pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseDiagrams().UseAutoLinks().Build();

        ///// <summary>
        ///// Converts markdown text into HTML page
        ///// </summary>
        ///// <param name="markdownText">The markdown text.</param>
        ///// <returns></returns>
        //public static String markdigMD2HTML(this String markdownText)
        //{
        //    MarkdownPipeline pipe2 = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

        //    //Markdig.MarkdownExtensions.UseDiagrams(pipeline);
        //    String output = Markdig.Markdown.ToHtml(markdownText, pipe2);

        //    return output;
        //}

        /// <summary>
        /// Gets the text size -- converts appendType
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static styleTextSizeEnum getTextSizeEnum(this appendType type)
        {
            // styleTextSizeEnum output = styleTextSizeEnum.regular;

            String typeName = type.ToString();
            if (typeName.StartsWith("heading", StringComparison.Ordinal))
            {
                String h_level_str = imbSciStringExtensions.removeStartsWith(typeName, "heading").Trim("_".ToCharArray());
                if (h_level_str.isNullOrEmpty())
                {
                    h_level_str = "1";
                }
                return (styleTextSizeEnum)Enum.Parse(typeof(styleTextSizeEnum), imbSciStringExtensions.add("h", h_level_str, ""));
            }

            switch (type)
            {
                case appendType.none:
                    break;

                case appendType.regular:
                    break;

                case appendType.bypass:
                    break;

                case appendType.italic:
                    break;

                case appendType.bold:
                    break;

                case appendType.footnote:
                case appendType.source:
                case appendType.sourceJS:
                case appendType.sourcePY:
                case appendType.sourceXML:
                case appendType.sourceCS:
                case appendType.monospace:
                    return styleTextSizeEnum.smaller;
                    break;

                case appendType.striketrough:
                    break;

                case appendType.heading:

                    break;

                case appendType.blockquote:

                    break;

                case appendType.subscript:
                    break;

                case appendType.superscript:
                    break;

                case appendType.marked:
                    break;

                case appendType.squareQuote:
                    break;

                case appendType.math:
                    break;

                case appendType.quotation:
                    break;

                case appendType.section:
                    break;

                case appendType.paragraph:
                    break;

                    break;

                    break;

                    break;

                case appendType.comment:
                    break;

                default:
                    return styleTextSizeEnum.regular;
            }
            return styleTextSizeEnum.regular;
        }

        /// <summary>
        /// Try to detect what ever the tag is known structure tag or source code
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static appendType detectMarkdownStructure(this String tag)
        {
            tag = tag.ToLower().Trim();

            if (htmlDefinitions.HTMLTags_semanticStructureTags.Contains(tag))
            {
                return appendType.section;
            }
            if (htmlDefinitions.HTMLTags_blockStructureTags.Contains(tag))
            {
                return appendType.paragraph;
            }
            if (htmlDefinitions.HTMLTags_textSemanticTags.Contains(tag))
            {
                return appendType.monospace;
            }

            if (htmlDefinitions.HTMLTags_highlightTags.Contains(tag))
            {
                return appendType.regular;
            }

            if (tag.StartsWith("code"))
            {
                String prefix = imbSciStringExtensions.removeStartsWith(tag, "code");

                switch (prefix)
                {
                    case "javascript":
                        return appendType.sourceJS;
                        break;

                    case "python":
                        return appendType.sourceJS;
                        break;

                    case "cs":
                        return appendType.sourceCS;
                        break;

                    case "xml":
                        return appendType.sourceXML;
                        break;

                    default:
                        return appendType.source;
                        break;
                }
            }

            return appendType.none;
        }

        /// <summary>
        /// Converting heading tags into proper markdown
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="content"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static appendType detectMarkdownTagHeading(this String tag)
        {
            String prefix = "";
            appendType output = appendType.none;
            tag = tag.ToLower();

            if (tag == "h") return appendType.heading;

            if ((tag.Length == 2) && tag.StartsWith("h"))
            {
                Int32 hLevel = (Int32)imbSciStringExtensions.removeStartsWith(tag, "h").imbToNumber(typeof(Int32));
                prefix = "heading_" + hLevel.ToString();

                Enum.TryParse<appendType>(prefix, out output);
            }
            return output;
        }

        /// <summary>
        /// Detects proper companion to HTML tag
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="content"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static appendType markdownTag(this String tag, String content, ITextRender sb)
        {
            String prefix = tag;

            appendType type = tag.detectMarkdownStructure();

            if (type != appendType.none)
            {
                sb.Append(content, type, true);
            }
            else
            {
                type = detectMarkdownTagHeading(tag);
            }

            if (type == appendType.none)
            {
                type = appendType.section;
            }

            return type;
        }

        ///// <summary>
        ///// Helper compatibility layer - transforms known HTML tags into Markdown companions. It controls open() and close() tags
        ///// </summary>
        ///// <param name="tag"></param>
        ///// <param name="content"></param>
        ///// <param name="sb"></param>
        ///// <param name="isClosing"></param>
        //public static void markdownTag(this String tag, String content, imbStringBuilderBase sb)
        //{
        //    String prefix = tag;

        //    appendType type = tag.detectMarkdownStructure();

        //    if (type != appendType.none)
        //    {
        //        sb.Append(content, type, true);
        //    } else
        //    {
        //        type = detectMarkdownTagHeading(tag);
        //    }

        //    if (type == appendType.none)
        //    {
        //        type = appendType.section;
        //    }

        //}

        /// <summary>
        /// Escapes reserved characters for markdown
        /// </summary>
        /// <param name="content">content send to wrap</param>
        /// <returns></returns>
        public static string markdownEscape(this String content)
        {
            content = content.Replace("\"", "\\\"");
            content = content.Replace(":", "\\:");
            content = content.Replace("|", "\\|");
            content = content.Replace("[", "\\[");
            content = content.Replace("]", "\\]");
            content = content.Replace("{", "\\{");
            content = content.Replace("}", "\\}");
            content = content.Replace("+", "\\+");
            content = content.Replace("#", "\\#");
            content = content.Replace("!", "\\!");
            content = content.Replace(".", "\\.");
            content = content.Replace("+", "\\+");
            content = content.Replace("-", "\\-");
            content = content.Replace("`", "\\`");
            return content;
        }

        /// <summary>
        /// Inverse call for markdownText extension
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content">Content</param>
        /// <returns>Markdown code</returns>
        public static String markdown(this appendType type, String content)
        {
            return content.markdownText(type);
        }

        public static string markdownText(this String content, appendType type)
        {
            String output = __markdownText(content, type);
            return output;
        }

        /// <summary>
        /// Deploying markdown wrapping syntax
        /// </summary>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string __markdownText(String content, appendType type)
        {
            //if (type != appendType.bypass) content = content.markdownEscape();

            switch (type)
            {
                case appendType.blockquote:
                    return imbSciStringExtensions.add("> ", content, "");
                    break;

                case appendType.bold:
                    return "**" + content.Trim() + "**";
                    break;

                case appendType.italic:
                    return "_" + content.Trim() + "_";
                    break;

                case appendType.monospace:
                    return "`" + content + "'";
                    break;

                case appendType.striketrough:
                    return "~~" + content + "~~";
                    break;

                case appendType.subscript:
                    return "~" + content + "~";
                    break;

                case appendType.superscript:
                    return "^" + content + "^";
                    break;

                case appendType.marked:
                    return "==" + content + "==";
                    break;

                case appendType.squareQuote:
                    return "`" + content + "`";
                    break;

                case appendType.heading:
                    return "# " + content + "";

                case appendType.heading_1:
                    return "## " + content + "";

                case appendType.heading_2:
                    return "### " + content + "";
                    break;

                case appendType.heading_3:
                    return "#### " + content + "";
                    break;

                case appendType.heading_4:
                    return "##### " + content + "";
                    break;

                case appendType.heading_5:
                    return "###### " + content + "";
                    break;

                case appendType.heading_6:
                    return "####### " + content + "";
                    break;

                case appendType.math:
                    return "$$".addLine(content).addLine("$$");

                case appendType.quotation:
                    return imbSciStringExtensions.add(imbSciStringExtensions.add("\"\"", content, ""), "\"\"", "");

                case appendType.source:
                    return "```".addLine(content).addLine("```");
                    break;

                case appendType.sourceJS:
                    return "```javascript".addLine(content).addLine("```");
                    break;

                case appendType.sourcePY:
                    return "```python".addLine(content).addLine("```");
                    break;

                case appendType.sourceCS:
                    return "```cs".addLine(content).addLine("```");
                    break;

                case appendType.sourceXML:
                    return "```xml".addLine(content).addLine("```");
                    break;

                case appendType.paragraph:
                    return Environment.NewLine.addLine(content);
                    break;

                case appendType.section:
                    //                    String prefix = Environment.NewLine;
                    //                    prefix += Environment.NewLine;

                    return " > ".addLine(content);
                    break;

                case appendType.regular:
                    return content.markdownEscape();
                    break;

                case appendType.none:
                    return content;
                    break;

                default:
                    return content.markdownEscape();
                    break;
            }
            //sb.AppendLine(linePrefix + tabInsert + content.ToUpper());
        }

        //  public static String getTableCellValue(this String content,)

        public static Tuple<String, String> getTableColumnHead(this printHorizontal position, string dcCaption)
        {
            String headingline = ""; //.add(dcCaption, "|");
            String underline = ""; //.add("-".Repeat(dcCaption.Length), "|");

            Int32 len = dcCaption.Length;
            if (len < 2) len = 2;

            switch (position)
            {
                case printHorizontal.left:
                    headingline = "|" + dcCaption + " ";
                    underline = "|:" + "-".Repeat(dcCaption.Length);
                    break;

                case printHorizontal.middle:
                    headingline = "|" + dcCaption + " ";
                    underline = "|:" + "-".Repeat(dcCaption.Length - 2) + ":";
                    break;

                case printHorizontal.right:
                    headingline = "|" + dcCaption + " ";
                    underline = "|" + "-".Repeat(dcCaption.Length - 1) + ":";
                    break;

                default:
                    break;
            }

            Tuple<String, String> output = new Tuple<string, string>(headingline, underline);
            return output;
        }

        public static String markdownFieldForColumn(this DataColumn dc, DataRow dr, Boolean skipEscape, dataValueFormatInfo dcf)
        {
            String content = "";
            Object value = dr[dc];

            content = value.toStringSafe("  ", dcf.valueFormat);

            if (value is Int32)
            {
                if ((((Int32)value) == Int32.MaxValue) || (((Int32)value) == Int32.MinValue)) content = "-";
            }

            if (!skipEscape)
            {
                content = content.markdownEscape();
            }

            // filter out table crushers
            content = content.Replace("|", " : ");
            content = content.Replace(Environment.NewLine, "");

            switch (dcf.importance)
            {
                case dataPointImportance.important:
                    content = "**" + content + "**";
                    break;

                case dataPointImportance.alarm:
                    content = "`**" + content + "**`";
                    break;

                case dataPointImportance.normal:

                    break;
            }

            //if (dc.GetEncodeMode() != toDosCharactersMode.none)
            //{
            //    content = content.toDosCharacters(dc.GetEncodeMode());
            //}

            Int32 columnWidth = dc.GetWidth();
            if (columnWidth > 0)
            {
                if (content.Length < columnWidth) content = content.PadRight(columnWidth);
            }

            String wrapTag = dc.GetWrapTag();
            if (!wrapTag.isNullOrEmpty()) content = "<span>" + content + "</span>";

            // content = content.add(content, "|");

            return content;
        }

        /// <summary>
        /// DataTable to markdown convertor
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string markdownTable(this DataTable table, Boolean prittyOutput = true)
        {
            String output = "";
            String underline = "|";
            String headingline = "|";
            String dcCaption = "";

            List<DataColumn> skipEscape = new List<DataColumn>();

            dataValueFormatDictionary dcfs = new dataValueFormatDictionary();

            String sep = "|";

            foreach (DataColumn dc in table.Columns)
            {
                dcCaption = dc.GetHeading();
                if (dcCaption.StartsWith("_"))
                {
                    skipEscape.Add(dc);
                    dcCaption = dcCaption.TrimStart('_');
                    dc.Caption = dcCaption;
                }

                dataValueFormatInfo dcf = new dataValueFormatInfo(dc);
                if (dcf.directAppend) skipEscape.Add(dc);
                dcfs.Add(dc.ColumnName, dcf);

                Tuple<String, String> adds = getTableColumnHead(dcf.position, dcCaption);

                headingline = headingline + adds.Item1;
                underline = underline + adds.Item2;
            }

            if (prittyOutput)
            {
                table = table.Copy();
                table.setColumnWidths(100);
            }

            underline = underline + "|";
            headingline = headingline + "|";

            output = output + headingline.Replace("||", "|") + Environment.NewLine;
            output = output + underline.Replace("||", "|") + Environment.NewLine;

            //output = output + Environment.NewLine;

            foreach (DataRow dr in table.Rows)
            {
                String rowline = "|";
                foreach (DataColumn dc in table.Columns)
                {
                    String content = dc.markdownFieldForColumn(dr, skipEscape.Contains(dc), dcfs[dc.ColumnName]);
                    content = content.Replace(Environment.NewLine, "");
                    /*

                    String content = "";
                    Object value = dr[dc];

                    content = value.toStringSafe("  ", dcf.valueFormat);

                    //if (!dcf.valueFormat.isNullOrEmpty())
                    //{
                    //    if (value is IFormattable)
                    //    {
                    //        IFormattable value_IFormattable = (IFormattable)value;
                    //        content = value_IFormattable.ToString(dcf.valueFormat, CultureInfo.CurrentCulture);
                    //    }

                    //} else
                    //{
                    //    content = value.toStringSafe("  ");
                    //}

                    if (value is Int32)
                    {
                        if ((((Int32)value) == Int32.MaxValue) || (((Int32)value) == Int32.MinValue)) content = "-";
                    }

                    if (!skipEscape.Contains(dc))
                    {
                        content = content.markdownEscape();
                    }

                    // filter out table crushers
                    content = content.Replace("|", " : ");
                    content = content.Replace(Environment.NewLine, "");

                    switch (dcf.importance)
                    {
                        case dataPointImportance.important:
                            content = "**" + content + "**";
                            break;

                        case dataPointImportance.alarm:
                            content = "`**" + content + "**`";
                            break;

                        case dataPointImportance.normal:

                            break;
                    }

                    if (dc.GetEncodeMode() != toDosCharactersMode.none)
                    {
                        content = content.toDosCharacters(dc.GetEncodeMode());
                    }

                    Int32 columnWidth = dc.GetWidth();
                    if (columnWidth > 0)
                    {
                        if (content.Length<columnWidth) content = content.PadRight(columnWidth);
                    }

                    String wrapTag = dc.GetWrapTag();
                    if (!wrapTag.isNullOrEmpty()) content = "<span>" + content + "</span>";
                    */
                    rowline = rowline.add(content, "|"); // imbSciStringExtensions.add(rowline, content, "|");
                }
                rowline = rowline + "|" + Environment.NewLine;
                output = output + rowline;
            }

            if (table.ExtraLinesCount() > 0)
            {
                output += Environment.NewLine;
                var lines = table.GetExtraDesc();
                foreach (String ln in lines)
                {
                    output = output + "> " + ln + Environment.NewLine;
                }
                output += Environment.NewLine;
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
        public static string markdownList(this IEnumerable list, Boolean isOrderedList = false, StringBuilder sb = null, Int32 level = 0)
        {
            if (sb == null) sb = new StringBuilder();

            Int32 i = 0;
            String levelPrefix = "  ".Repeat(level);
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
                    IEnumerable dce = dc as IEnumerable;
                    dce.markdownList(isOrderedList, sb, level + 1);
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
        public static string markdownLink(this String url, String name, String caption = "", appendLinkType linkType = appendLinkType.link)
        {
            url = url;

            if (url.Contains("..//"))
            {
                url = url.Replace("..//", "../");
            }

            name = name.markdownEscape();
            caption = caption.markdownEscape();
            switch (linkType)
            {
                case appendLinkType.image:
                    return String.Format("![{0}]:({1})", name, url);
                    break;

                case appendLinkType.referenceImage:
                    return String.Format("![{0}][{1}]", name, url);
                    break;

                case appendLinkType.link:
                    if (!imbSciStringExtensions.isNullOrEmptyString(caption))
                    {
                        return String.Format("[{0}]({1} \"{2}\")", name, url, caption);
                    }
                    else
                    {
                        return String.Format("[{0}]({1})", name, url);
                    }
                    break;

                case appendLinkType.reference:
                    return String.Format("[{0}]: {1}", name, url);
                    break;

                case appendLinkType.referenceLink:
                    return String.Format("[{0}][{1}]", name, url);
                    break;

                case appendLinkType.anchor:
                    return String.Format(" * [{0}](#{1})", name, url);
                    break;

                case appendLinkType.scriptPostLink:
                case appendLinkType.scriptLink:

                    return "<script src=\"" + url + "\" charset=\"utf-8\" ></script>";
                    break;

                case appendLinkType.styleLink:
                    return "<link rel=\"stylesheet\" type=\"text/css\" href=\"" + url + "\" >";
                    break;

                case appendLinkType.unknown:
                default:
                    return url;
                    break;
            }
        }
    }
}