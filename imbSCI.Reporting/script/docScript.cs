// --------------------------------------------------------------------------------------------------------------------
// <copyright file="docScript.cs" company="imbVeles" >
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

/// <summary>
///
/// </summary>
namespace imbSCI.Reporting.script
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Core.reporting.render.converters;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.style.shot;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Core.style.color;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.enums.reporting;
    using imbSCI.DataComplex.data;
    using imbSCI.DataComplex.extensions;
    using imbSCI.DataComplex.extensions.data;
    using imbSCI.DataComplex.extensions.data.operations;
    using imbSCI.Graph.Diagram;
    using imbSCI.Graph.Diagram.builders;
    using imbSCI.Graph.Diagram.core;
    using imbSCI.Graph.Diagram.enums;
    using imbSCI.Graph.Diagram.output;
    using imbSCI.Reporting.charts.core;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.exceptions;
    using imbSCI.Reporting.interfaces;
    using imbSCI.Reporting.meta.delivery;
    using imbSCI.Reporting.script.exeAppenders;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using d = docScriptArguments;

    /// <summary>
    /// Script that genererates output document. It may be used multiple times to generate different outputs
    /// </summary>
    /// <remarks>
    /// Script containes ordinal instructions with content of data applied template.
    /// </remarks>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.Cores.reporting.script.docScriptInstruction}" />
    public class docScript : List<docScriptInstruction>, ITextAppendContent, ITextAppendContentExtended, IEquatable<docScript>
    {
        /// <summary>
        /// Appends the executable kernel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IExeAppend AppendExe<T>() where T : IExeAppend, new()
        {
            IExeAppend app = new T();
            add(appendType.exe).arg(d.dsa_value, app);
            return app;
        }

        /// <summary>
        /// Appends the executable kernel
        /// </summary>
        /// <param name="exeRunner">The executable runner.</param>
        public IExeAppend AppendExe(IExeAppend exeRunner)
        {
            add(appendType.exe).arg(d.dsa_value, exeRunner);
            return exeRunner;
        }

        /// <summary>
        /// Appends the button.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="url">Apsolutni path od roota sajta do lokacije cilja.</param>
        /// <param name="color">The color.</param>
        /// <param name="size">The size.</param>
        public void AppendButton(string caption, string url, bootstrap_color color = bootstrap_color.basic, bootstrap_size size = bootstrap_size.md)
        {
            if (!url.StartsWith("http"))
            {
                url = url.ensureStartsWith("".t(templateFieldBasic.root_relpath));
            }
            object[] props = { caption, url, color, size };
            add(appendType.button).arg(d.dsa_title, caption).arg(d.dsa_url, url).arg(d.dsa_styleTarget, color).arg(d.dsa_stylerSettings, size).arg(d.dsa_value, props);
        }

        public void AppendChart(chartTypeEnum chartType, chartFeatures features, DataTable data, chartSizeEnum size, chartTypeEnum typesForSeries = chartTypeEnum.none)
        {
            List<object> parameters = new List<object>();
            parameters.AddMultiple(chartType, features, data, size);

            add(appendType.i_chart).arg(d.dsa_value, parameters).arg(d.dsa_format, typesForSeries);
        }

        public void Attachment(FileInfo file, string caption, bootstrap_color color, bootstrap_size size)
        {
            add(appendType.attachment).arg(d.dsa_title, caption).arg(d.dsa_path, file.FullName).arg(d.dsa_url, file.FullName).arg(d.dsa_styleTarget, color).arg(d.dsa_stylerSettings, size);
        }

        public void Attachment(string content, string filepath, string caption, bootstrap_color color, bootstrap_size size)
        {
            add(appendType.attachment).arg(d.dsa_title, caption).arg(d.dsa_content, content).arg(d.dsa_path, filepath).arg(d.dsa_url, filepath).arg(d.dsa_styleTarget, color).arg(d.dsa_stylerSettings, size);
        }

        public void Attachment(DataTable content, dataTableExportEnum format, string caption, bootstrap_color color, bootstrap_size size)
        {
            string fn = content.FilenameForTable(caption); // (caption, existingDataMode.overwriteExistingIfEmptyOrNull);
            add(appendType.attachment).arg(d.dsa_format, format).arg(d.dsa_content, content).arg(d.dsa_styleTarget, color).arg(d.dsa_stylerSettings, size).arg(d.dsa_title, caption).arg(d.dsa_url, fn);
        }

        public void AppendDiagram(object diagramModel, diagramOutputEngineEnum outputFormat)
        {
            add(appendType.image).arg(d.dsa_value, diagramModel).arg(d.dsa_format, outputFormat);
        }

        public void AppendFileTemplated(string sourcepath, string templateUnitFilenameNeedle, string outputpath, bool isDataTemplate, bool isLocalSource)
        {
            add(appendType.file).arg(d.dsa_path, sourcepath).arg(d.dsa_name, outputpath).arg(d.dsa_styleTarget, templateUnitFilenameNeedle).arg(d.dsa_on, isDataTemplate).arg(d.dsa_relative, isLocalSource);
            //  this.add(appendType.file).arg(d.dsa_key, datakey).arg(d.dsa_path, sourcepath).arg(d.dsa_on, isLocalSource).arg(d.dsa_styleTarget, templateUnitFilenameNeedle);
        }

        /// <summary>
        /// Direct content injection, bypassing all internal transformations by class implementing <see cref="T:imbSCI.Cores.reporting.render.ITextRender" />
        /// </summary>
        /// <param name="content">The content.</param>
        public void AppendDirect(string content)
        {
            add(appendType.direct).arg(d.dsa_contentLine, content);
        }

        /// <summary>
        /// Saves <c>content</c> to specified path. Path is local to context scope
        /// </summary>
        /// <param name="outputpath">The filepath, including filename and extension</param>
        /// <param name="content">Any string content</param>
        public void AppendToFile(string outputpath, string content)
        {
            add(appendType.toFile).arg(d.dsa_contentLine, content).arg(d.dsa_path, outputpath);
        }

        /// <summary>
        /// Loads content from <c>sourcepath</c> into renderer [if <c>datakey</c> is <see cref="F:imbSCI.Cores.reporting.render.fields.templateFieldSubcontent.none" /> or into data field if specified.
        /// </summary>
        /// <param name="sourcepath">The sourcepath.</param>
        /// <param name="datakey">The datakey.</param>
        /// <param name="isLocalSource">if set to <c>true</c><c>sourcepath</c> is interpreted as local to context</param>
        public void AppendFromFile(string sourcepath, templateFieldSubcontent datakey = templateFieldSubcontent.none, bool isLocalSource = false)
        {
            add(appendType.fromFile).arg(d.dsa_key, datakey).arg(d.dsa_path, sourcepath).arg(d.dsa_on, isLocalSource);
        }

        /// <summary>
        /// File from <c>sourcepath</c> is copied to <c>outputpath</c> or used as data template if <c>isDataTeplate</c> is true
        /// </summary>
        /// <param name="sourcepath">The sourcepath - within application directory</param>
        /// <param name="outputpath">The outputpath - local to context</param>
        /// <param name="isDataTemplate">if set to <c>true</c> the <c>soucepath</c> content will be processed as data template before saving output to <c>outputpath</c></param>
        public void AppendFile(string sourcepath, string outputpath, bool isDataTemplate = false)
        {
            add(appendType.file).arg(d.dsa_path, sourcepath).arg(d.dsa_name, outputpath).arg(d.dsa_on, isDataTemplate);
        }

        /// <summary>
        /// Appends the image tag/call.
        /// </summary>
        /// <param name="imageSrc">Source url/path of the image</param>
        /// <param name="imageAltText">The image alt text.</param>
        /// <param name="imageRef">The image reference ID used internally</param>
        public void AppendImage(string imageSrc, string imageAltText, string imageRef)
        {
            add(appendType.image).arg(d.dsa_path, imageSrc).arg(d.dsa_contentLine, imageAltText).arg(d.dsa_name, imageRef);
        }

        /// <summary>
        /// Inserts <c>mathFormula</c> block
        /// </summary>
        /// <param name="mathFormula">The math formula: LaTeX, KaTex, asciimath...</param>
        /// <param name="mathFormat">The math format used to describe the formula</param>
        public void AppendMath(string mathFormula, string mathFormat = "asciimath")
        {
            add(appendType.math).arg(d.dsa_contentLine, mathFormula).arg(d.dsa_format, mathFormat);
        }

        /// <summary>
        /// Appends the content with label decoration
        /// </summary>
        /// <param name="content">The content to show inside label</param>
        /// <param name="isBreakLine">if set to <c>true</c> if will break line after this append</param>
        /// <param name="comp_style">Special style tag, class, definition.</param>
        public void AppendLabel(string content, bool isBreakLine = true, object comp_style = null)
        {
            add(appendType.label).arg(d.dsa_contentLine, content).arg(d.dsa_styleTarget, comp_style).arg(d.dsa_on, isBreakLine).isHorizontal = !isBreakLine;
        }

        /// <summary>
        /// Creates panel with <c>content</c> and (optionally) with <c>comp_heading</c> and <c>comp_description</c> as footer.
        /// </summary>
        /// <param name="content">String content to place inside the panel</param>
        /// <param name="comp_heading">The heading for the panel. If blank panel will have no heading</param>
        /// <param name="comp_description">Description to be placed at bottom of the panel</param>
        /// <param name="comp_style">Special style tag, class, definition.</param>
        public void AppendPanel(string content, string comp_heading = "", string comp_description = "", object comp_style = null)
        {
            add(appendType.panel).arg(d.dsa_contentLine, content).arg(d.dsa_title, comp_heading).arg(d.dsa_description, comp_description).arg(d.dsa_styleTarget, comp_style);
        }

        /// <summary>
        /// Appends the function.
        /// </summary>
        /// <param name="functionCode">The function code.</param>
        /// <param name="breakLine">if set to <c>true</c> [break line].</param>
        /// <returns></returns>
        public object AppendFunction(string functionCode, bool breakLine = false)
        {
            return append(appendType.i_function, functionCode).isHorizontal = !breakLine;
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
            return add(appendType.frame).arg(d.dsa_contentLine, content).arg(d.dsa_title, title).arg(d.dsa_description, footnote).arg(d.dsa_w, width).arg(d.dsa_h, height).arg(d.dsa_content, paragraphs);
        }

        /// <summary>
        /// Appends string with template placeholder tag {{{ }}} / creates field to call custom property --&gt; for document builder: introduces custom parameter and field
        /// </summary>
        /// <param name="fieldName">String, enum what ever</param>
        /// <param name="breakLine">on TRUE it is new line call, on FALSE its inline call</param>
        public void AppendPlaceholder(object fieldName, bool breakLine = false)
        {
            add(appendType.placeholder).arg(d.dsa_name, fieldName).arg(d.dsa_on, breakLine).isHorizontal = !breakLine;
        }

        /// <summary>
        /// Horizontal line divider.
        /// </summary>
        /// <remarks>
        /// It respect active full width and/or background color
        /// </remarks>
        public void AppendHorizontalLine()
        {
            c_line();
        }

        /// <summary>
        /// On HTML/XML builder adds invisible comment tag, on Table builder it adds comment to the current cell, on Document builder it adds pop-up comment on aplicable way
        /// </summary>
        /// <param name="content">Text content for the paragraph</param>
        /// <returns></returns>
        public object AppendComment(string content)
        {
            return append(appendType.comment, content);
        }

        /// <summary>
        /// HTML/XML builder adds H tag with proper level sufix, on Table it applies style and for H1 and H2
        /// </summary>
        /// <param name="content">Text</param>
        /// <param name="level">from 1 to 6</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        public object AppendHeading(string content, int level = 1)
        {
            return heading(content, level, false);
        }

        /// <summary>
        /// HTML/XML adds <c>q</c> tag, Table aplies <c>smallText</c> style
        /// </summary>
        /// <param name="content">Text content of the quote</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        public object AppendQuote(string content)
        {
            return append(appendType.quotation, content);
        }

        /// <summary>
        /// HTML/XML adds <c>q</c> tag, Table aplies <c>smallText</c> style
        /// </summary>
        /// <param name="content">Text content of the quote</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        public object AppendCite(string content)
        {
            return append(appendType.comment, content);
        }

        /// <summary>
        /// HTML/XML adds <c>q</c> tag, Table aplies <c>smallText</c> style
        /// </summary>
        /// <param name="content">Text content of the quote</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        public object AppendCode(string content)
        {
            return code("", "", content.breakLines(), "");
        }

        /// <summary>
        /// HTML/XML adds <c>q</c> tag, Table aplies <c>smallText</c> style
        /// </summary>
        /// <param name="content">Text content of the quote</param>
        /// <param name="codetypename">The codetypename: i.e. html</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        public object AppendCode(string content, string codetypename)
        {
            return code("", "", content.breakLines(), codetypename);
        }

        /// <summary>
        /// Appends collection of pairs.
        /// </summary>
        /// <param name="data">Data to use as pair source</param>
        /// <param name="isHorizontal">Should output be horizontal</param>
        /// <param name="between">Content to place between. If empty it will skip middle column</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        public object AppendPairs(PropertyCollection data, bool isHorizontal = false, string between = "")
        {
            return pairs("", "", data, between, 3, isHorizontal);
        }

        /// <summary>
        /// Appends content wrapped into paragraph tag. Table builders will merge whole line if "fullWidth" is TRUE.
        /// </summary>
        /// <param name="content">Text content for the paragraph</param>
        /// <param name="fullWidth">if TRUE it will maximize width</param>
        /// <returns>
        /// OuterXML/String or proper DOM object of container
        /// </returns>
        public object AppendParagraph(string content, bool fullWidth = false)
        {
            return append(appendType.paragraph, content);
        }

        /// <summary>
        /// Creates new section with title and content. Optionally it may contain: additional paragraphs for content and footnote on bottom
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
            return section(title, footnote, paragraphs).arg(d.dsa_contentLine, content);
        }

        /// <summary>
        /// Renders key-&gt; value pair
        /// </summary>
        /// <param name="key">Property name or collection key</param>
        /// <param name="value">ToString value</param>
        /// <param name="breakLine">should break line</param>
        /// <param name="between"></param>
        public void AppendPair(string key, object value, bool breakLine = true, string between = " = ")
        {
            c_pair(key, between, value, !breakLine);
        }

        /// <summary>
        /// Appends inline or new line content.
        /// </summary>
        /// <param name="content">String content to be wrapped into container</param>
        /// <param name="type">Container type - for text it is always none</param>
        /// <param name="breakLine">Inline (FALSE) or new line (TRUE)</param>
        /// <remarks>
        /// It is supported by: Source, Document and Table builders
        /// </remarks>
        public void Append(string content, appendType type = appendType.none, bool breakLine = false)
        {
            append(type, content).isHorizontal = !breakLine;
        }

        /// <summary>
        /// Appends the line.
        /// </summary>
        /// <param name="content">The content.</param>
        public void AppendLine(string content)
        {
            AppendLine(content, appendRole.paragraph);
        }

        public docScriptInstruction AppendLine(string content, appendRole __role)
        {
            var tmp = add(appendType.paragraph).arg(d.dsa_isHorizontal, false).arg(d.dsa_contentLine, content).arg(d.dsa_role, __role);
            return tmp;
        }

        /// <summary>
        /// Renders IEnumerable that may contain other IEnumerables
        /// </summary>
        /// <param name="content">Collection with objects and/or subcollections</param>
        /// <param name="isOrderedList">On TRUE it will be ordered list with number, FALSE will create button list</param>
        /// <remarks>
        /// In Document builders isOrderedList has isHorizontal role
        /// </remarks>
        public void AppendList(IEnumerable<object> content, bool isOrderedList = false)
        {
            add(appendType.list).arg(d.dsa_content, content).arg(d.dsa_on, isOrderedList).isHorizontal = false;
        }

        /// <summary>
        /// Renders link, image or reference
        /// </summary>
        /// <param name="url">url or reference id</param>
        /// <param name="name">Name of link</param>
        /// <param name="caption">Title - popup content</param>
        /// <param name="linkType">Select if output is link, image or reference</param>
        public void AppendLink(string url, string name, string caption = "", appendLinkType linkType = appendLinkType.link)
        {
            c_link(name, url, caption, "", linkType);
        }

        /// <summary>
        /// Renders DataTable
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="doThrowException">if set to <c>true</c> it will throw an exception on <see cref="validateTable()"/> return false.</param>
        public void AppendTable(DataTable table, bool doThrowException = true)
        {
            if (table.validateTable())
            {
                c_table(table, table.TableName, table.GetDescription());
            }
            else
            {
                if (doThrowException) new aceReportException("AppendTable(" + table.TableName + ") failed: data table is failed on [table.validateTable()] test", null, table, "DataTable append fail");
                AppendLine("AppendTable failed");
                return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public docScriptFlags flags { get; set; } = docScriptFlags.none;

        /// <summary>
        /// Save docScript in <c>textFormat</c> and open it with <c>openWith</c> external tool
        /// </summary>
        /// <param name="textFormat">The text format.</param>
        /// <param name="filepath">The filepath.</param>
        /// <param name="openWith">The open with.</param>
        /// <returns>Filepath where script was saved</returns>
        public string ToFile(docScriptInstructionTextFormatEnum textFormat, string filepath, externalTool openWith = externalTool.none)
        {
            string scriptCode = ToString(textFormat);
            string path = filepath.ensureEndsWith(".txt");// ;
            if (File.Exists(path)) File.Delete(path);
            saveBase.saveToFile(path, scriptCode);
            //  openWith.run(path);
            return path;
        }

        /// <summary>
        /// To the string.
        /// </summary>
        /// <param name="textFormat">The text format.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string ToString(docScriptInstructionTextFormatEnum textFormat)
        {
            builderForText sb = new builderForText();
            int c = 0;
            switch (textFormat)
            {
                case docScriptInstructionTextFormatEnum.none:
                case docScriptInstructionTextFormatEnum.unknown:
                    return "";
                    break;

                case docScriptInstructionTextFormatEnum.meta:

                    foreach (docScriptInstruction ins in this)
                    {
                        c = IndexOf(ins);
                        if (ins.type == appendType.x_scopeIn)
                        {
                            string classSufix = "";
                            string path = ins[d.dsa_path].toStringSafe();
                            IMetaContentNested scopeObject = ins[d.dsa_value] as IMetaContentNested;
                            if (scopeObject != null)
                            {
                                if (scopeObject.isThisRoot)
                                {
                                    path = ":root:" + path;
                                }
                                //path = scopeObject.
                                classSufix = ":" + ins[d.dsa_value].GetType().Name;
                                reportElementLevel el = scopeObject.elementLevel;
                                classSufix += " (" + el.toStringSafe() + ")";
                            }
                            sb.AppendLine("// ->[" + path + "]" + classSufix);
                            sb.nextTabLevel();
                        }
                        else if (ins.type == appendType.x_scopeOut)
                        {
                            sb.prevTabLevel();
                            sb.AppendLine("// <-- ");
                        }
                        else
                        {
                            sb.AppendLine(c.ToString("D5") + " " + ins.ToString(textFormat));
                        }
                    }
                    break;

                case docScriptInstructionTextFormatEnum.cs_compose:
                    sb.AppendLine("public void compose(docScript script) {");
                    sb.AppendLine("if (script == null) script = new docScript();");
                    sb.AppendLine("");
                    foreach (docScriptInstruction ins in this)
                    {
                        if (ins.type == appendType.x_scopeIn) sb.nextTabLevel();
                        sb.AppendLine(ins.ToString(textFormat));
                        if (ins.type == appendType.x_scopeOut) sb.prevTabLevel();
                    }
                    sb.AppendLine("}");
                    break;

                case docScriptInstructionTextFormatEnum.xml:

                    sb.AppendLine("<docScript>");
                    foreach (docScriptInstruction ins in this)
                    {
                        sb.AppendLine(ins.ToString(textFormat));
                    }
                    sb.AppendLine("</docScript>");
                    break;

                case docScriptInstructionTextFormatEnum.json:
                    sb.AppendLine("docScript {");
                    foreach (docScriptInstruction ins in this)
                    {
                        sb.AppendLine(ins.ToString(textFormat));
                    }
                    sb.AppendLine("}");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return sb.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="docScript"/> class.
        /// </summary>
        /// <param name="__name">The name.</param>
        public docScript(string __name)
        {
            name = __name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="docScript"/> class.
        /// </summary>
        /// <param name="__context">The context.</param>
        public docScript(IRenderExecutionContext __context)
        {
            context = __context;
            name = __context?.name;
            //name = __name;
        }

        /// <summary>
        /// Inserts set of instructions
        /// </summary>
        /// <param name="source">The source.</param>
        public void insertSub(IList<docScriptInstruction> source, IMetaContentNested parent = null)
        {
            // appendType.c_open
            //   appendType.c_close;
            int scopeIns = 0;
            int scopeOuts = 0;
            int openIns = 0;
            int openOuts = 0;

            IMetaContentNested lastScopeIn = parent;
            IMetaContentNested lastScopeOut = null;

            foreach (docScriptInstruction ins in source)
            {
                bool accept = true;
                switch (ins.type)
                {
                    case appendType.c_open:

                        openIns++;

                        break;

                    case appendType.c_close:
                        openOuts++;
                        if (openOuts > openIns)
                        {
                            aceReportException axe = new aceReportException(this, "Can't close container it will brake the structure - o[" + openIns + "] / c[" + openOuts + "]", aceReportExceptionType.constructMetaModelError, null);
                            axe.add("The last scope in: " + lastScopeIn.path + "").add("Its name: " + lastScopeIn.name + "").add("Its type: " + lastScopeIn.GetType().Name + "");
                            //  aceLog.log(axe.Message);
                            accept = false;
                        }
                        break;

                    case appendType.x_scopeIn:
                        lastScopeIn = ins[d.dsa_value] as IMetaContentNested; //, scope
                        scopeIns++;
                        break;

                    case appendType.x_scopeOut:
                        lastScopeOut = ins[d.dsa_value] as IMetaContentNested;
                        scopeOuts++;
                        break;

                    default:

                        break;
                }
                if (accept) Add(ins);
            }
            if (scopeIns != scopeOuts)
            {
                throw new aceReportException(this, "ScopeIn and ScopeOut are not same in[" + scopeIns + "] / out[" + scopeOuts + "]", aceReportExceptionType.constructMetaModelError)
                                .add("The last scope in: " + lastScopeIn.path + "")
                                .add("Its name: " + lastScopeIn.name + "")
                                .add("Its type: " + lastScopeIn.GetType().Name + "");
            }

            if (openIns != openOuts)
            {
                aceReportException axe = new aceReportException(this, "openIns and openOuts are not same o[" + openIns + "] / c[" + openOuts + "]", aceReportExceptionType.constructMetaModelError);
                axe.add("The last scope in: " + lastScopeIn.path + "")
                .add("Its name: " + lastScopeIn.name + "")
                .add("Its type: " + lastScopeIn.GetType().Name + "");

                throw axe;
            }
        }

        /// <summary>
        /// Sets dimension of column or row
        /// </summary>
        /// <param name="id">From cursor position relative id to target column/row</param>
        /// <param name="units">Width/height to set</param>
        /// <param name="setRow">TRUE it will set row, FALSE it will set column</param>
        /// <param name="setAutofit">if set to <c>true</c> [set autofit].</param>
        /// <param name="corner">The corner - to set alignment.</param>
        /// <returns></returns>
        public docScriptInstruction s_width(int id, int units, bool setRow = false, bool setAutofit = false, textCursorZoneCorner corner = textCursorZoneCorner.none)
        {
            return add(appendType.s_width).arg(d.dsa_key, id).arg(d.dsa_value, units).arg(d.dsa_isHorizontal, !setRow).arg(d.dsa_autostyling, setAutofit).arg(d.dsa_cursorCorner, corner);
        }

        /// <summary>
        /// Section of specific column width has: title, content and footer.
        /// </summary>
        /// <param name="title">The heading - first row</param>
        /// <param name="footer">The footer lowest row in section structure</param>
        /// <param name="content">The content multiline content to render between heading and footer</param>
        /// <param name="width">The width in columns - for -1 it will use complete zone width.</param>
        /// <returns>
        /// Instruction set
        /// </returns>
        public docScriptInstruction section(string title, string footer, IEnumerable<string> content, int width = -1)
        {
            return add(appendType.c_section).arg(d.dsa_title, title).arg(d.dsa_footer, footer).arg(d.dsa_content, content).arg(d.dsa_w, width);
        }

        public docScriptInstruction c_table(DataTable table, string title, string description)
        {
            if (table.Rows.Count == 0)
            {
            }
            return add(appendType.c_table).arg(d.dsa_title, title).arg(d.dsa_footer, description).arg(d.dsa_dataTable, table);
        }

        public docScriptInstruction code(string title, string footer, IEnumerable<string> content, string codetypename, int width = -1)
        {
            return add(appendType.source).arg(d.dsa_title, title).arg(d.dsa_footer, footer).arg(d.dsa_content, content).arg(d.dsa_w, width).arg(d.dsa_class_attribute, codetypename).arg(d.dsa_name, title.getFilename());
        }

        public docScriptInstruction c_pair(string key, string between, object value, bool isHorizontal = false)
        {
            return add(appendType.c_pair).arg(d.dsa_key, key).arg(d.dsa_value, value).arg(d.dsa_separator, between).arg(d.dsa_isHorizontal, isHorizontal);
        }

        public docScriptInstruction c_line()
        {
            return add(appendType.c_line);
        }

        public docScriptInstruction c_link(string name, string url, string title, string desc, appendLinkType linkType)
        {
            return add(appendType.c_link).arg(d.dsa_name, name).arg(d.dsa_url, url).arg(d.dsa_title, title).arg(d.dsa_linkType, linkType).arg(d.dsa_description, desc);
        }

        public docScriptInstruction open(string name, string title, string desc)
        {
            return add(appendType.c_open).arg(d.dsa_name, name).arg(d.dsa_title, title).arg(d.dsa_description, desc);
        }

        public docScriptInstruction close()
        {
            return add(appendType.c_close);
        }

        /// <summary>
        /// Create paris table with title, footer and middle cell
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="footer">The footer.</param>
        /// <param name="pairs">The pairs.</param>
        /// <param name="between">The between.</param>
        /// <param name="width">The width.</param>
        /// <param name="isHorizontal">if set to <c>true</c> [is horizontal].</param>
        /// <returns></returns>
        public docScriptInstruction pairs(string title, string footer, PropertyCollection pairs, string between = "=", int width = 3, bool isHorizontal = false)
        {
            return add(appendType.c_data).arg(d.dsa_title, title).arg(d.dsa_footer, footer).arg(d.dsa_dataPairs, pairs).arg(d.dsa_isHorizontal, isHorizontal).arg(d.dsa_separator, between).arg(d.dsa_w, width);
        }

        /// <summary>
        /// Lists the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="footer">The footer.</param>
        /// <param name="items">The items.</param>
        /// <param name="width">The width.</param>
        /// <param name="isHorizontal">if set to <c>true</c> [is horizontal].</param>
        /// <returns></returns>
        public docScriptInstruction list(string title, string footer, IEnumerable items, int width = 3, bool isHorizontal = false)
        {
            return add(appendType.c_data).arg(d.dsa_title, title).arg(d.dsa_footer, footer).arg(d.dsa_dataList, items).arg(d.dsa_isHorizontal, isHorizontal).arg(d.dsa_w, width);
        }

        /// <summary>
        /// Turns off active styling - current was set with <see cref="styleApplicationFlag.setAsActive"/> call
        /// </summary>
        /// <returns></returns>
        public docScriptInstruction s_style()
        {
            return add(appendType.s_style);
        }

        public docScriptInstruction s_settings(appendType styleForType, styleApplicationFlag autostyleFlags, appendRole styleForRole)
        {
            return add(appendType.s_settings).arg(d.dsa_styleTarget, styleForRole).arg(d.dsa_innerAppend, styleForType).arg(d.dsa_stylerSettings, autostyleFlags);
        }

        public docScriptInstruction s_settings(Color background, Color foreground)
        {
            return add(appendType.s_settings).arg(d.dsa_forecolor, foreground.ColorToHex()).arg(d.dsa_background, background.ColorToHex());
        }

        /// <summary>
        /// Going to set background into proper color
        /// </summary>
        /// <param name="palette">The palette.</param>
        /// <param name="variation">The variation.</param>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        public docScriptInstruction s_settings(aceColorPalette palette, int variation, acePaletteShotResEnum resource)
        {
            return add(appendType.s_settings).arg(d.dsa_stylerSettings, palette).arg(d.dsa_variationRole, variation).arg(d.dsa_styleTarget, resource);
        }

        /// <summary>
        /// Direct styling shot pass
        /// </summary>
        /// <param name="shot">The shot to be applied</param>
        /// <param name="target">The target of application</param>
        /// <returns></returns>
        public docScriptInstruction s_style(IStyleInstruction shot, styleShotTargetEnum target = styleShotTargetEnum.unknown)
        {
            return add(appendType.s_style).arg(d.dsa_stylerSettings, shot).arg(d.dsa_styleTarget, target);
        }

        //
        /// <summary>
        /// Sets styling and/or turns on active styling.
        /// </summary>
        /// <param name="app">flags describing way of style application</param>
        /// <param name="target">designate target of style application</param>
        /// <returns>Instruction that was just set</returns>
        public docScriptInstruction s_style(styleApplicationFlag app, styleShotTargetEnum target = styleShotTargetEnum.unknown)
        {
            return add(appendType.s_style).arg(d.dsa_autostyling, app).arg(d.dsa_styleTarget, target);
        }

        public docScriptInstruction x_scopeOut(IMetaContentNested scope)
        {
            deliveryInstance del = scope.context as deliveryInstance;     //       scope.context.composeOperationEnd(context, scope, this);
            if (del != null)
            {
                del.composeOperationEnd(context, scope, this);
            }
            return add(appendType.x_scopeOut);
        }

        /// <summary>
        /// Starts subcontent session
        /// </summary>
        /// <param name="subcontent">The subcontent.</param>
        /// <returns></returns>
        public docScriptInstruction openSub(templateFieldSubcontent subcontent)
        {
            return add(appendType.x_subcontent).arg(d.dsa_key, subcontent).arg(d.dsa_innerAppend, appendType.c_open);
        }

        /// <summary>
        /// Closes the current subcontent session
        /// </summary>
        /// <param name="subcontent">The subcontent.</param>
        /// <returns></returns>
        public docScriptInstruction closeSub()
        {
            return add(appendType.x_subcontent).arg(d.dsa_innerAppend, appendType.c_close);
        }

        /// <summary>
        /// Close export session and restore primary IDocumentRenderer
        /// </summary>
        /// <param name="filenamebase">Filename and session ID in the same time.</param>
        /// <param name="fileformat">The fileformat to use to save exported content</param>
        /// <returns></returns>
        public docScriptInstruction x_exportEnd(string filenamebase, reportOutputFormatName fileformat)
        {
            return add(appendType.x_export).arg(d.dsa_path, filenamebase).arg(d.dsa_format, fileformat).arg(d.dsa_innerAppend, appendType.c_close);
        }

        public docScriptInstruction x_scopeIn(IMetaContentNested scope)
        {
            deliveryInstance del = scope.context as deliveryInstance;
            if (del != null)
            {
                del.composeOperationStart(scope.context, scope, this);
            }
            else
            {
            }
            //scope.context.composeOperationStart(scope.context, scope, this);

            return add(appendType.x_scopeIn).arg(d.dsa_path, scope.path).arg(d.dsa_value, scope);
        }

        public docScriptInstruction x_directory(directoryOperation op, string path, bool scopeToNew)
        {
            return add(appendType.x_directory).arg(d.dsa_dirOperation, op).arg(d.dsa_path, path).arg(d.dsa_scopeToNew, scopeToNew);
        }

        public docScriptInstruction i_dataSource(PropertyCollection data, existingDataMode existMode = existingDataMode.overwriteExisting)
        {
            return add(appendType.i_dataSource).arg(d.dsa_dataSource, data).arg(d.dsa_itemExistsMode, existMode);
        }

        public docScriptInstruction x_data()
        {
            return add(appendType.x_data);
        }

        public docScriptInstruction i_meta(string key, object value)
        {
            return add(appendType.i_meta).arg(d.dsa_key, key).arg(d.dsa_value, value);
        }

        public docScriptInstruction i_log(string title, params string[] contentlines)
        {
            return add(appendType.i_log).arg(d.dsa_contentLine, title).arg(d.dsa_content, contentlines);
        }

        public docScriptInstruction x_moveToCorner(textCursorZoneCorner direction)
        {
            return add(appendType.x_move).arg(d.dsa_cursorCorner, direction).arg(d.dsa_zoneFrame, false).arg(d.dsa_relative, false);
        }

        /// <summary>
        /// Moves the cursor
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="steps">The steps.</param>
        /// <returns></returns>
        public docScriptInstruction x_move(textCursorZoneCorner direction, int steps, bool moveZone = false)
        {
            return add(appendType.x_move).arg(d.dsa_cursorCorner, direction).arg(d.dsa_value, steps).arg(d.dsa_zoneFrame, moveZone).arg(d.dsa_relative, true);
        }

        public docScriptInstruction x_move(int x, int y, bool relative = false, bool moveZone = false)
        {
            return add(appendType.x_move).arg(d.dsa_x, x).arg(d.dsa_y, y).arg(d.dsa_relative, relative).arg(d.dsa_zoneFrame, moveZone);
        }

        public docScriptInstruction x_move(selectRange vector, bool relative = false, bool moveZone = false)
        {
            return add(appendType.x_move).arg(d.dsa_vector, vector).arg(d.dsa_relative, relative).arg(d.dsa_zoneFrame, moveZone);
        }

        public docScriptInstruction i_dataInDocument(PropertyCollection data, bool appendAsPairs, params templateFieldBasic[] fieldsToUse)
        {
            var tmp = add(appendType.i_dataInDocument).arg(d.dsa_dataSource, data).arg(d.dsa_dataField, fieldsToUse);
            if (appendAsPairs) tmp.arg(d.dsa_value, 1);
            return tmp;
        }

        public docScriptInstruction x_save(appendType innerAppend, string path, reportOutputFormatName format)
        {
            var tmp = add(appendType.x_save).arg(d.dsa_path, path).arg(d.dsa_innerAppend, innerAppend).arg(d.dsa_format, format);
            return tmp;
        }

        /// <summary>
        /// Load content into page/document. Inner append is performed for each line
        /// </summary>
        /// <param name="innerAppend">Way loaded content should be appended: <see cref="imbSCI.Cores.enums.appendType.i_document"/>, <see cref="imbSCI.Cores.enums.appendType.i_page"/>, and any other append for each line>:<see cref="imbSCI.Cores.enums.appendType"/></param>
        /// <param name="path">The path.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public docScriptInstruction i_load(appendType innerAppend, string path, reportOutputFormatName format)
        {
            var tmp = add(appendType.i_load).arg(d.dsa_path, path).arg(d.dsa_innerAppend, innerAppend).arg(d.dsa_format, format);
            return tmp;
        }

        public docScriptInstruction i_external(appendType innerAppend, string path, bool doRecompile)
        {
            var tmp = add(appendType.i_external).arg(d.dsa_path, path).arg(d.dsa_innerAppend, innerAppend).arg(d.dsa_recompile, doRecompile);
            return tmp;
        }

        public docScriptInstruction AppendPair(string key, Enum value, string between, bool isHorizontal)
        {
            var tmp = add(appendType.c_pair).arg(d.dsa_key, key).arg(d.dsa_dataField, value).arg(d.dsa_separator, between).arg(d.dsa_isHorizontal, isHorizontal);
            return tmp;
        }

        public docScriptInstruction AppendPair(string key, object value, string between, bool isHorizontal)
        {
            var tmp = add(appendType.c_pair).arg(d.dsa_key, key).arg(d.dsa_value, value).arg(d.dsa_separator, between).arg(d.dsa_isHorizontal, isHorizontal);
            return tmp;
        }

        public docScriptInstruction heading(string content, int level, bool underline = false)
        {
            appendType ht = appendType.heading + level;
            var tmp = add(ht).arg(d.dsa_contentLine, content).arg(d.dsa_separator, underline);
            return tmp;
        }

        public docScriptInstruction AppendLine(appendType __type, string content, appendRole __role = appendRole.none)
        {
            var tmp = add(__type).arg(d.dsa_isHorizontal, false).arg(d.dsa_contentLine, content).arg(d.dsa_role, __role);
            return tmp;
        }

        public docScriptInstruction AppendLine()
        {
            // var tmp = this.add(appendType.x_move).arg(d.dsa_cursorCorner, textCursorZoneCorner.Bottom).arg(d.dsa_value, 1).arg(d.dsa_relative, true).arg(d.dsa_zoneFrame, false);
            return add(appendType.c_line);
        }

        public docScriptInstruction append(appendType __type, string content)
        {
            var tmp = add(__type).arg(d.dsa_isHorizontal, true).arg(d.dsa_contentLine, content);
            return tmp;
        }

        public docScriptInstruction add(appendType __type)
        {
            return add(__type, "");
        }

        /// <summary>
        /// Add appends with: scopein, scopeout, save, to data etc. operations over IMetaContent object
        /// </summary>
        /// <param name="__type">The type.</param>
        /// <param name="__scope">The scope.</param>
        /// <returns></returns>
        public docScriptInstruction add(appendType __type, IMetaContentNested __scope)
        {
            return add(__type).arg(d.dsa_path, __scope.path).arg(d.dsa_value, __scope);
        }

        public docScriptInstruction add(appendType __type, string __input, bool __isHorizontal = false)
        {
            docScriptInstruction tmp = new docScriptInstruction(__type, __input, __isHorizontal);
            Add(tmp);

            return tmp;
        }

        /// <summary>
        /// Adds new instruction into script
        /// </summary>
        /// <param name="__type">The type.</param>
        /// <param name="__arguments">The arguments.</param>
        /// <returns></returns>
        public docScriptInstruction add(appendType __type, params docScriptArguments[] __arguments)
        {
            docScriptInstruction tmp = new docScriptInstruction(__type, __arguments);
            Add(tmp);

            return tmp;
        }

        public bool Equals(docScript other)
        {
            throw new NotImplementedException();
        }

        void ITextAppendContentExtended.open(string tag, string title, string description) => open(name, title, description);

        void ITextAppendContentExtended.close() => close();

        public void nextTabLevel() => nextTabLevel();

        public void prevTabLevel() => prevTabLevel();

        public void rootTabLevel() => rootTabLevel();

        internal void AppendDiagram(diagramModel diagram, diagramOutputEngineEnum engine)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Name of source documentSet or other kind of source
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name { get; set; }

        public string filename
        {
            get { return name.getCleanFilePath(); }
        }

        /// <summary>
        ///
        /// </summary>
        public IRenderExecutionContext context { get; protected set; }
    }
}