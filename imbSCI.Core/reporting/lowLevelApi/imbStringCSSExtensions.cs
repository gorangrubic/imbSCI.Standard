// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringCSSExtensions.cs" company="imbVeles" >
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

    /// \ingroup_disabled report_ll
    public static class imbStringCSSExtensions
    {
        /// <summary>
        /// Renders the CSS block for tag.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static string renderCssBlockForTag(this appendType type, String content)
        {
            String output = "";
            output.addLine(imbSciStringExtensions.add(toHtmlTagName(type), "{", " "));
            output.addLine(content);
            output.addLine("}");

            return output;
        }

        /// <summary>
        /// Returns html tag name for appendType
        /// </summary>
        /// <param name="type"></param>
        /// <returns>only textual part of html tag</returns>
        public static string toHtmlTagName(this appendType type)
        {
            //if (type != appendType.bypass) content = content.markdownEscape();

            switch (type)
            {
                case appendType.blockquote:
                    return "blockquote";
                    break;

                case appendType.bold:
                    return "b";
                    break;

                case appendType.italic:
                    return "i";
                    break;

                case appendType.monospace:
                    return "kbd";
                    break;

                case appendType.striketrough:
                    return "s";
                    break;

                case appendType.subscript:
                    return "sub";
                    break;

                case appendType.superscript:
                    return "sup";
                    break;

                case appendType.marked:
                    return "mark";
                    break;

                case appendType.squareQuote:
                    return "em";
                    break;

                case appendType.heading:
                    return "h1";

                case appendType.heading_1:
                    return "h1";

                case appendType.heading_2:
                    return "h2";
                    break;

                case appendType.heading_3:
                    return "h3";
                    break;

                case appendType.heading_4:
                    return "h4";
                    break;

                case appendType.heading_5:
                    return "h5";
                    break;

                case appendType.heading_6:
                    return "h6";
                    break;

                case appendType.math:
                    return "summary";

                case appendType.quotation:
                    return "q";

                case appendType.source:
                    return "code";
                    break;

                case appendType.sourceJS:
                    return "script";
                    break;

                case appendType.sourcePY:
                    return "code";
                    break;

                case appendType.sourceCS:
                    return "code";
                    break;

                case appendType.sourceXML:
                    return "code"; // "```xml".addLine(content).addLine("```");
                    break;

                case appendType.paragraph:
                    return "p";
                    break;

                case appendType.section:
                    //                    String prefix = Environment.NewLine;
                    //                    prefix += Environment.NewLine;

                    return "section";
                    break;

                case appendType.regular:
                    return "span";
                    break;

                default:
                    return "p";
                    break;
            }
            //sb.AppendLine(linePrefix + tabInsert + content.ToUpper());
        }

        /// <summary>
        /// Returns opening line -- using values from special keys (tag_, class_, id_)
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>CSS formated property string</returns>
        public static String renderCssOpeningEntry(this DictionaryEntry entry)
        {
            String key = entry.Key.ToString();

            if (key.StartsWith("tag_"))
            {
                return entry.Value.ToString() + " ";
            }
            else if (key.StartsWith("class_"))
            {
                return "." + entry.Value.ToString() + " ";
            }
            else if (key.StartsWith("id_"))
            {
                return "#" + entry.Value.ToString() + " ";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns key:value result in case key is not special one (tag_, class_, id_)
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>CSS formated property string</returns>
        public static String renderCssInnerEntry(this DictionaryEntry entry)
        {
            String key = entry.Key.ToString();

            if (key.StartsWith("tag_"))
            {
                return "";
                //return entry.Value.ToString() + " ";
            }
            else if (key.StartsWith("class_"))
            {
                return "";
                // return "." + entry.Value.ToString() + " ";
            }
            else if (key.StartsWith("id_"))
            {
                return "";
                //return "#" + entry.Value.ToString() + " ";
            }
            else
            {
                return key + ": " + entry.Value.ToString() + ";";
            }
        }
    }
}