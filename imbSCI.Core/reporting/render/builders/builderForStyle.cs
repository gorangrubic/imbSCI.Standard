// --------------------------------------------------------------------------------------------------------------------
// <copyright file="builderForStyle.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.render.builders
{
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.lowLevelApi;
    using imbSCI.Core.reporting.render.converters;
    using imbSCI.Core.reporting.render.core;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Basic builder for styles
    /// </summary>
    /// \ingroup_disabled report_int
    public class builderForStyle : imbStringBuilderBase, IStyleRender, ITextRender, IConverterRender //<HtmlTextWriterStyle, HtmlTextWriterTag>
    {
        public override converterBase converter
        {
            get
            {
                if (_converter == null) _converter = new converterForBootstrap3();
                return _converter;
            }
        }

        public override void prepareBuilder()
        {
            base.prepareBuilder();
            formats.defaultFormat = reportOutputFormatName.textCss;
        }

        /// <summary>
        /// Appends the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="type">The type.</param>
        /// <param name="breakLine">if set to <c>true</c> [break line].</param>
        public void Append(String content, HtmlTextWriterStyle type, Boolean breakLine = false)
        {
            switch (type)
            {
                case HtmlTextWriterStyle.FontSize:
                    _AppendLine(imbSciStringExtensions.ensureEndsWith(imbSciStringExtensions.add("font-size", ": ").a(content), ";"));
                    break;

                default:
                    _AppendLine(imbSciStringExtensions.ensureEndsWith(imbSciStringExtensions.add(type.ToString(), ": ").a(content), ";"));
                    break;
            }
        }

        /// <summary>
        /// Adds horizontal line
        /// </summary>
        public override void AppendHorizontalLine()
        {
            _AppendLine("/* ---------------- */");
        }

        /// <summary>
        /// Renders list and sublists
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isOrderedList"></param>
        public override void AppendList(IEnumerable<Object> content, Boolean isOrderedList = false)
        {
            foreach (Object con in content)
            {
                _AppendLine(imbSciStringExtensions.ensureEndsWith(con.ToString(), ";"));
            }
            //sb.AppendLine(linePrefix + tabInsert + content.ToUpper());
        }

        /// <summary>
        /// Appends the pair.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="breakLine">if set to <c>true</c> [break line].</param>
        /// <param name="between">The between.</param>
        public void AppendPair(HtmlTextWriterStyle key, object value, bool breakLine = true, string between = ":")
        {
            AppendPair(key.ToString(), imbSciStringExtensions.add(value.ToString(), ";"), true, ":");
        }

        //private void AppendPair(string v1, object p, bool v2, string v3)
        //{
        //    throw new NotImplementedException();
        //}

        //public override void open(String tag)
        //{
        //    String __op = String.Format(openTagFormat, tag);
        //    AppendLine(linePrefix + tabInsert + __op);
        //    openedTags.Push(tag);

        //}

        ///// <summary>
        ///// Dodaje HTML zatvaranje taga -- zatvara poslednji koji je otvoren
        ///// </summary>
        ///// <remarks>
        ///// Ako je prosledjeni tag none onda zatvara poslednji tag koji je otvoren.
        ///// </remarks>
        ///// <param name="tag"></param>
        //public override void close(String tag = "none")
        //{
        //    //tag = tag.checkForDefaultTag(reportOutputRoles.container);
        //    String __cl = String.Format(closeTagFormat, tag);

        //    if (tag == "none")
        //    {
        //        if (openedTags.Any())
        //        {
        //            tag = openedTags.Pop();
        //        }
        //        else
        //        {
        //            tag = "error";
        //        }
        //    }

        //    if (tag != "none")
        //    {
        //        AppendLine(linePrefix + tabInsert + __cl);
        //    }
        //}

        /// <summary>
        /// Appends inline or in new line
        /// </summary>
        /// <param name="content">String to add</param>
        /// <param name="type">Disabled</param>
        /// <param name="breakLine">On TRUE it will break into new line</param>
        public void Append(string content, appendType type = appendType.none, bool breakLine = false)
        {
            _AppendLine(imbStringCSSExtensions.renderCssBlockForTag(type, content));
            //_Append("/* ".a(content).Add(" */"));

            //Append(content.ensureEndsWith(";"),appendType.none, breakLine);
        }

        public object AppendComment(string content)
        {
            _AppendLine("/* ".a(content).Add(" */"));
            return getLastLine();
        }

        public object AppendHeading(string content, int level = 1)
        {
            _AppendLine("h" + level.ToString() + " {");
            _AppendLine(content);
            _AppendLine("}");

            return getLastLine();
        }

        /// <summary>
        /// HTML/XML adds <c></c>
        /// </summary>
        /// <param name="content">Initial content</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="title">Title of the section</param>
        /// <param name="footnote">Description under the section</param>
        /// <param name="paragraphs">Additional paragraphs to place inside</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        public object AppendFrame(string content, int width, int height, string title = "", string footnote = "", IEnumerable<string> paragraphs = null)
        {
            if (!imbSciStringExtensions.isNullOrEmptyString(footnote)) _AppendLine("/* frame style: " + title + " */");
            if (!imbSciStringExtensions.isNullOrEmptyString(footnote))
            {
                _AppendLine(".frame #" + title.imbCodeNameOperation() + " {");
            }
            else
            {
                _AppendLine(".frame {");
            }
            _AppendLine("   width=" + width + "px;");
            _AppendLine("   height=" + height + "px;");
            _AppendLine(content);
            if (paragraphs != null)
            {
                foreach (String str in paragraphs)
                {
                    _AppendLine(imbSciStringExtensions.ensureEndsWith(str.ensureStartsWith("   "), ";"));
                }
            }
            _AppendLine("}");
            if (!imbSciStringExtensions.isNullOrEmptyString(footnote)) _AppendLine("/* footnote: " + footnote + " */");

            return getLastLine();
        }

        /// <summary>
        /// Inserts line for each DictionaryEntry in data. Key is mapped to CSS property, Value is value, unit is sufix added. Special keys starting with: tag_, class_, id_ == > requred for css definition initiation
        /// </summary>
        /// <param name="data"> Key is mapped to CSS property - or ID, class, tag selector if in special format</param>
        /// <remarks>Implementation modified: between now goes to last position - its purpose is to represent unit of measurement (px, em...)</remarks>
        /// <returns>Complete CSS definition with { }</returns>
        public object AppendPairs(PropertyCollection data, bool noUse = false, string notUsed = "")
        {
            foreach (DictionaryEntry entry in data)
            {
                _Append(entry.renderCssOpeningEntry());
            }
            _AppendLine(" {");

            foreach (DictionaryEntry entry in data)
            {
                String tmp = entry.renderCssInnerEntry();
                if (!imbSciStringExtensions.isNullOrEmptyString(tmp)) _AppendLine(tmp);
            }
            _AppendLine("}");
            return getLastLine();
        }

        /// <summary>
        /// Creates new CSS definition for section . Optionally it may contain: additional paragraphs for content and footnote on bottom
        /// </summary>
        /// <param name="content">Main content of the section</param>
        /// <param name="title">Title of the section</param>
        /// <param name="footnote">Description under the section</param>
        /// <param name="paragraphs">Additional paragraphs to place inside</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        public object AppendSection(string content, string title, string footnote = null, IEnumerable<string> paragraphs = null)
        {
            if (!imbSciStringExtensions.isNullOrEmptyString(footnote)) _AppendLine("/* frame style: " + title + " */");
            if (!imbSciStringExtensions.isNullOrEmptyString(footnote))
            {
                _AppendLine(".section #" + title.imbCodeNameOperation() + " {");
            }
            else
            {
                _AppendLine(".section {");
            }

            _AppendLine(content);
            if (paragraphs != null)
            {
                foreach (String str in paragraphs)
                {
                    _AppendLine(imbSciStringExtensions.ensureEndsWith(str.ensureStartsWith("   "), ";"));
                }
            }
            _AppendLine("}");
            if (!imbSciStringExtensions.isNullOrEmptyString(footnote)) _AppendLine("/* footnote: " + footnote + " */");

            return getLastLine();
        }

        public FileInfo savePage(string name, reportOutputFormatName format = reportOutputFormatName.none)
        {
            throw new NotImplementedException();
        }

        public object addDocument(string name, bool scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameExistingOnOtherDate, reportOutputFormatName format = reportOutputFormatName.none)
        {
            throw new NotImplementedException();
        }

        public object addPage(string name, bool scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameThis, reportOutputFormatName format = reportOutputFormatName.none)
        {
            throw new NotImplementedException();
        }

        public override void AppendImage(string imageSrc, string imageAltText, string imageRef)
        {
            throw new NotImplementedException();
        }

        public override void AppendMath(string mathFormula, string mathFormat = "asciimath")
        {
            throw new NotImplementedException();
        }

        public override void AppendLabel(string content, bool isBreakLine = true, object comp_style = null)
        {
            throw new NotImplementedException();
        }

        public override void AppendPanel(string content, string comp_heading = "", string comp_description = "", object comp_style = null)
        {
            throw new NotImplementedException();
        }

        public bool Equals(builderForStyle other)
        {
            throw new NotImplementedException();
        }

        public void AppendLine()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        public builderForStyle()
        {
            openTagFormat = "{0} {";
            closeTagFormat = "} /* {0} */";
        }
    }
}