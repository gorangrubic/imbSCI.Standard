// --------------------------------------------------------------------------------------------------------------------
// <copyright file="builderForMarkdown.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.lowLevelApi;
    using imbSCI.Core.reporting.render.converters;
    using imbSCI.Core.reporting.render.core;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Base class for all MarkdoenBuilder writters or renderrers
    /// </summary>
    /// \ingroup_disabled report_int
    public class builderForMarkdown : imbStringBuilderBase, ITextRender, IConverterRender //<appendType, HtmlTextWriterTag>
    {
        public override converterBase converter
        {
            get
            {
                if (_converter == null) _converter = new converterForBootstrap3();
                return _converter;
            }
        }

        /// <summary>
        /// Appends the image tag/call.
        /// </summary>
        /// <param name="imageSrc">Source url/path of the image</param>
        /// <param name="imageAltText">The image alt text.</param>
        /// <param name="imageRef">The image reference ID used internally</param>
        public override void AppendImage(string imageSrc, string imageAltText, string imageRef)
        {
            String tmp = "![" + imageRef + "](" + imageSrc + " \"" + imageAltText + "\")";
            AppendDirect(tmp);
        }

        /// <summary>
        /// Inserts <c>mathFormula</c> block
        /// </summary>
        /// <param name="mathFormula">The math formula: LaTeX, KaTex, asciimath...</param>
        /// <param name="mathFormat">The math format used to describe the formula</param>
        public override void AppendMath(string mathFormula, string mathFormat = "asciimath")
        {
            String tmp = "```" + mathFormat + Environment.NewLine;
            tmp += mathFormula + Environment.NewLine;
            tmp += "```";
            AppendDirect(tmp);
        }

        /// <summary>
        /// Appends the content with label decoration
        /// </summary>
        /// <param name="content">The content to show inside label</param>
        /// <param name="isBreakLine">if set to <c>true</c> if will break line after this append</param>
        /// <param name="comp_style">Special style tag, class, definition.</param>
        public override void AppendLabel(string content, bool isBreakLine = true, object comp_style = null)
        {
            String tmpst = "label-default";
            if (comp_style != null)
            {
                tmpst = comp_style.ToString();
                tmpst = imbSciStringExtensions.removeStartsWith(tmpst, "style_");
                tmpst = tmpst.ensureStartsWith("label-");
            }
            String tmp = "<span class=\"label " + tmpst + "\">" + content + "</span>";

            _Append(tmp, isBreakLine);
        }

        /// <summary>
        /// Creates panel with <c>content</c> and (optionally) with <c>comp_heading</c> and <c>comp_description</c> as footer.
        /// </summary>
        /// <param name="content">String content to place inside the panel</param>
        /// <param name="comp_heading">The heading for the panel. If blank panel will have no heading</param>
        /// <param name="comp_description">Description to be placed at bottom of the panel</param>
        /// <param name="comp_style">Special style tag, class, definition.</param>
        public override void AppendPanel(string content, string comp_heading = "", string comp_description = "", object comp_style = null)
        {
            String tmpst = "panel-default";
            if (comp_style != null)
            {
                tmpst = comp_style.ToString();
                tmpst = imbSciStringExtensions.removeStartsWith(tmpst, "style_");
                tmpst = tmpst.ensureStartsWith("panel-");
            }

            String tmp = "<div class=\"panel " + tmpst + "\">" + Environment.NewLine;

            if (!comp_heading.isNullOrEmpty())
            {
                tmp = tmp + "<div class=\"panel-heading\">" + comp_heading + "</div>" + Environment.NewLine;
            }
            tmp = tmp + "<div class=\"panel-body\">" + content + "</div>" + Environment.NewLine;
            if (!comp_description.isNullOrEmpty())
            {
                tmp = tmp + "<div class=\"panel-footer\">" + comp_description + "</div>" + Environment.NewLine;
            }
            tmp = tmp + "</div>" + Environment.NewLine;

            Append(tmp, appendType.bypass);
        }

        public builderForMarkdown() : base(reportAPI.textBuilder)
        {
            // prepareBuilder();
        }

        public override void prepareBuilder()
        {
            // base.prepareBuilder();
            tabLevel = 0;

            settings.api = reportAPI.textBuilder;
            settings.cursorBehaviour.cursorMode = textCursorMode.scroll;
            settings.formats = reportOutputSupport.getFormatSupportFor(reportAPI.imbMarkdown, "output");
            formats.defaultFormat = reportOutputFormatName.textMdFile;
            _zone = new cursorZone();
            _zone.setPresetSpatialSettings(cursorZoneSpatialPreset.console);
            _zone.setZoneStructure(cursorZoneLayoutPreset.oneFullPage);
            _c = new cursor(_zone, textCursorMode.scroll, textCursorZone.innerZone, this.GetType().Name);
        }

        public override string newLineInsert
        {
            get
            {
                if (tabLevel > 0)
                {
                    return Environment.NewLine;// + Environment.NewLine;
                }
                else
                {
                    return Environment.NewLine + Environment.NewLine;
                }
                //return Environment.NewLine;// + Environment.NewLine;
            }
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public IList content
        {
            get
            {
                return contentElements;
            }
        }

        /// <summary>
        /// Appends the table row.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="columns">The columns.</param>
        public void AppendTableRow(acePaletteVariationRole role, params String[] columns)
        {
            String output = "";

            foreach (String col in columns)
            {
                output = imbSciStringExtensions.add(output, col, "|");
            }

            output = output.newLine();

            switch (role)
            {
                case acePaletteVariationRole.header:
                case acePaletteVariationRole.heading:
                    foreach (String col in columns)
                    {
                        output = imbSciStringExtensions.add(output, "-".Repeat(col.Length), "|");
                    }
                    break;

                default:
                    break;
            }

            Append(output);
        }

        /// <summary>
        /// Renders key-> value pair
        /// </summary>
        /// <param name="key">Property name or collection key</param>
        /// <param name="value">ToString value</param>
        /// <param name="breakLine">should break line </param>
        public override void AppendPair(String key, Object value, Boolean breakLine = true, String between = " = ")
        {
            base._Append((imbStringMarkdownExtensions.markdown(appendType.bold, key.toStringSafe())) + (between) + imbStringMarkdownExtensions.markdown(appendType.italic, value.toStringSafe()), breakLine);
        }

        public override object AppendHeading(string content, int level = 1)
        {
            appendType hd = (appendType)appendType.heading + level;

            Boolean hadTab = (tabLevel > 0);
            prevTabLevel();

            Append(content, hd, true);
            AppendLine();

            if (hadTab) nextTabLevel();

            //_AppendLine(content.markdownText(hd));
            return "";
        }

        /// <summary>
        /// Appends inline or new line content
        /// </summary>
        /// <param name="content">String content to be wrapped into container</param>
        /// <param name="type">Container type - for text it is always none</param>
        /// <param name="breakLine">Inline (FALSE) or new line (TRUE)</param>
        public override void Append(String content, appendType type = appendType.none, Boolean breakLine = false)
        {
            base._Append(content.markdownText(type), breakLine);
        }

        /// <summary>
        /// Renders IEnumerable that may contain other IEnumerables
        /// </summary>
        /// <param name="content">Collection with objects and/or subcollections</param>
        /// <param name="isOrderedList">On TRUE it will be ordered list with number, FALSE will create button list</param>
        public override void AppendList(IEnumerable<Object> content, Boolean isOrderedList = false)
        {
            base._AppendLine(content.markdownList(isOrderedList));
        }

        /// <summary>
        /// Renders link, image or reference
        /// </summary>
        /// <param name="url">url or reference id</param>
        /// <param name="name">Name of link</param>
        /// <param name="caption">Title - popup content</param>
        /// <param name="linkType">Select if output is link, image or reference</param>
        public void AppendLink(String url, String name, String caption = "", appendLinkType linkType = appendLinkType.link)
        {
            _AppendLine(url.markdownLink(name, caption, linkType));
        }

        /// <summary>
        /// Renders DataTable
        /// </summary>
        /// <param name="table"></param>
        /// <param name="doThrowException"></param>
        public override void AppendTable(DataTable table, bool doThrowException = true)
        {
            if (table.validateTable())
            {
                try
                {
                    String tableContent = table.markdownTable();

                    _AppendLine(tableContent);
                }
                catch (Exception ex)
                {
                    if (doThrowException) new ArgumentException("AppendTable(" + table.TableName + ") failed: data table is failed on [table.validateTable()] test", nameof(table), ex);
                }
            }
            else
            {
                //if (doThrowException) new aceGeneralException("AppendTable(" + table.TableName + ") failed: data table is failed on [table.validateTable()] test", null, table, "DataTable append fail");
                AppendLine("AppendTable failed");
                return;
            }
        }

        /// <summary>
        /// Opens the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        /// \ingroup_disabled renderapi_append
        public override tagBlock open(String tag, String title = "", String description = "")
        {
            // appendType type = tag.markdownTag("", this);

            tagBlock tb = openedTags.Add(tag, title, description); //.Push(type.ToString());

            if (converter.HasContainerByName(tag))
            {
                AppendDirect(converter.GetContainerOpen(tag, "default", "md"));
                AppendLine();
            }
            else
            {
                AppendLine();
            }

            if (!imbSciStringExtensions.isNullOrEmptyString(title)) AppendHeading(tb.getTitle(false), openedTags.headingLevel);

            // nextTabLevel();

            //AppendLine("");
            //            tag.markdownTag(tag, this);
            //  base.open(tag);

            return tb;
        }

        /// <summary>
        /// Dodaje HTML zatvaranje taga -- zatvara poslednji koji je otvoren
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <remarks>
        /// Ako je prosledjeni tag none onda zatvara poslednji tag koji je otvoren.
        /// </remarks>
        /// \ingroup_disabled renderapi_append
        public override tagBlock close(string tag = "none")
        {
            AppendLine();
            var typeOut = openedTags.Pop();
            if (typeOut != null)
            {
                String description = typeOut.description;

                if (converter.HasContainerByName(typeOut.tag))
                {
                    AppendDirect(converter.GetContainerClose(typeOut.tag));
                    AppendLine();
                }
                else
                {
                    AppendLine();
                }

                if (!imbSciStringExtensions.isNullOrEmptyString(description)) AppendLine(description.toPrefixedMultiline(" > ", true));
            }
            //prevTabLevel();
            AppendLine();

            /// base.close(tag);
            return typeOut;
        }

        public override void closeAll()
        {
            String tag = "none"; // htmlTagName.none;

            Int32 c = openedTags.Count;

            for (int i = 0; i < c; i++)
            {
                close();
            }

            base.closeAll();
        }

        /// <summary>
        /// HTML/XML adds <c>q</c> tag, Table aplies <c>smallText</c> style
        /// </summary>
        /// <param name="content">Text content of the quote</param>
        /// <param name="codetypename"></param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        /// \ingroup_disabled renderapi_append
        public override object AppendCode(String content, String codetypename)
        {
            //Append(content, appendType.source, true);
            _AppendLine("```" + codetypename);
            _AppendLine(content);
            _AppendLine("```");

            return "";
        }

        /// <summary>
        /// Frame puts the content with \> prefix. Multiline content is supported <c></c>
        /// </summary>
        /// <param name="content">Initial content</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="title">Title of the section</param>
        /// <param name="footnote">Description under the section</param>
        /// <param name="paragraphs">Additional paragraphs to place inside</param>
        /// <returns>OuterXML/String or proper DOM object of container</returns>
        public object AppendFrame(String content, Int32 width, Int32 height, String title = "", String footnote = "", IEnumerable<String> paragraphs = null)
        {
            String prefix = "";

            AppendHorizontalLine();

            if (!imbSciStringExtensions.isNullOrEmptyString(title))
            {
                _AppendLine(title.markdownText(appendType.heading_3).markdownText(appendType.section));
                prefix = " > ";
            }

            _AppendLine(prefix + content.markdownText(appendType.section));

            if (paragraphs != null)
            {
                foreach (String ln in paragraphs)
                {
                    _AppendLine(prefix + ln.markdownText(appendType.section));
                }
            }
            if (!imbSciStringExtensions.isNullOrEmptyString(footnote)) _AppendLine(prefix + footnote.markdownText(appendType.italic));

            AppendHorizontalLine();
            return "";
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Creates new section with title and content. Optionally it may contain: additional paragraphs for content and footnote on bottom
        /// </summary>
        /// <param name="content">Main content of the section</param>
        /// <param name="title">Title of the section</param>
        /// <param name="footnote">Description under the section</param>
        /// <param name="paragraphs">Additional paragraphs to place inside</param>
        /// <returns>OuterXML/String or proper DOM object of container</returns>
        public object AppendSection(string content, string title, string footnote = null, IEnumerable<string> paragraphs = null)
        {
            String prefix = "";
            if (!imbSciStringExtensions.isNullOrEmptyString(title))
            {
                _AppendLine(title.markdownText(appendType.heading_3).markdownText(appendType.section));
                prefix = " > ";
            }

            _AppendLine(prefix + content.markdownText(appendType.section));

            if (paragraphs != null)
            {
                foreach (String ln in paragraphs)
                {
                    _AppendLine(prefix + ln.markdownText(appendType.section));
                }
            }
            if (!imbSciStringExtensions.isNullOrEmptyString(footnote)) _AppendLine(prefix + footnote.markdownText(appendType.italic));
            return "";
        }

        public FileInfo savePage(string name, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return saveDocument(name, getWritableFileMode.autoRenameExistingOnOtherDate, format);
        }

        public object addDocument(string name, bool scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameExistingOnOtherDate, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return saveDocument(name, mode, format);
        }

        public object addPage(string name, bool scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameThis, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return saveDocument(name, mode, format);
        }

        public override string ContentToString(bool doFlush = false, reportOutputFormatName format = reportOutputFormatName.none)
        {
            String markdown = base.ContentToString(doFlush);
            String str_output = markdown;

            //if (format == reportOutputFormatName.htmlViaMD)
            //{
            //    str_output = str_output.markdigMD2HTML();
            //}
            return str_output;
        }

        public override PropertyCollection getContentBlocks(bool includeMain, reportOutputFormatName format = reportOutputFormatName.none)
        {
            PropertyCollection pc = sbControler.getDataset(includeMain);

            PropertyCollection pc_out = new PropertyCollection();
            foreach (Object key in pc.Keys)
            {
                String str_block = pc[key].toStringSafe();
                //if (format == reportOutputFormatName.htmlViaMD)
                //{
                //    str_block = str_block.markdigMD2HTML();
                //}
                pc_out.Add(key, str_block);

                //  String str_key = key.ToString() + "_md";

                // pc_out.Add(str_key, pc[key].toStringSafe());
            }
            return pc_out;
        }

        public override FileInfo saveDocument(string name, getWritableFileMode mode, reportOutputFormatName format = reportOutputFormatName.none)
        {
            if (format == reportOutputFormatName.none) format = reportOutputFormatName.textMdFile;

            String str_output = ContentToString(false, format);

            String filename = name;

            if (!Path.HasExtension(name))
            {
                filename = formats.getFilename(name, format);
            }
            FileInfo fi = filename.getWritableFile(getWritableFileMode.newOrExisting);

            return str_output.saveStringToFile(fi.FullName, getWritableFileMode.overwrite, System.Text.Encoding.UTF8);

            //if (format == reportOutputFormatName.htmlViaMD)
            //{
            //    FileInfo fi = filename.getWritableFile(getWritableFileMode.newOrExisting);
            //    getLastLine();

            //    //String markdown =  //contentElements.ToDelimitedString(Environment.NewLine);
            //    String html = markdown.markdigMD2HTML();

            //    saveBase.saveToFile(fi.FullName, html);
            //    return fi;

            //}
            //else
            //{
            //    return base.saveDocument(name, mode, format);
            //}
        }

        public void AppendLine()
        {
            base.AppendLine();
        }
    }
}