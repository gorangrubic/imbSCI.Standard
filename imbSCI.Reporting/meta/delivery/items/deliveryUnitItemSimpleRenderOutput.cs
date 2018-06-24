// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitItemSimpleRenderOutput.cs" company="imbVeles" >
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
using d = imbSCI.Reporting.script.docScriptArguments;

namespace imbSCI.Reporting.meta.delivery.items
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Core.reporting.render.converters;
    using imbSCI.Core.reporting.render.core;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.style.shot;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.enums.reporting;
    using imbSCI.DataComplex.data.modelRecords;
    using imbSCI.DataComplex.extensions;
    using imbSCI.DataComplex.extensions.data.formats;
    using imbSCI.Graph.Diagram;
    using imbSCI.Graph.Diagram.builders;
    using imbSCI.Graph.Diagram.core;
    using imbSCI.Graph.Diagram.enums;
    using imbSCI.Graph.Diagram.output;
    using imbSCI.Reporting.charts.core;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.script;
    using imbSCI.Reporting.script.exeAppenders;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// UnitItem for simple output creation
    /// </summary>
    /// <seealso cref="imbSCI.Reporting.meta.delivery.deliveryUnitItem" />
    public class deliveryUnitItemSimpleRenderOutput : deliveryUnitItem, IDeliveryUnitItemOutputRender // where T:ITextRender, new()
    {
        /// <summary>
        ///
        /// </summary>
        public ITextRender outputRender { get; set; }

        /// <summary>
        ///
        /// </summary>
        public reportOutputFormatName format { get; set; }

        //private builderForTableDocument builderForTableDocument;

        /// <summary>
        ///
        /// </summary>
        public List<reportElementLevel> levels { get; protected set; } = new List<reportElementLevel>();

        public metaContentCriteriaTriggerCollection criteria
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public List<reportElementLevel> levelsOfNewDirectory { get; protected set; } = new List<reportElementLevel>();

        /// <summary>
        ///
        /// </summary>
        public reportElementLevel levelOfNewFile { get; set; } = reportElementLevel.document;

        /// <summary>
        ///
        /// </summary>
        public reportElementLevel levelOfNewPage { get; set; } = reportElementLevel.page;

        public deliveryUnitItemSimpleRenderOutput(ITextRender __builder, reportOutputFormatName __format, IEnumerable<reportElementLevel> __newDirectoryLevels) : base(deliveryUnitItemType.content)
        {
            name = __builder.GetType().Name.add(__format.ToString(), "-");
            location = deliveryUnitItemLocationBase.unknown;
            flags = deliveryUnitItemFlags.none;

            outputRender = __builder;

            format = __format;

            levelsOfNewDirectory.AddRange(__newDirectoryLevels);

            //builder = new builderSelector();
            //builder.Add(__builder, __format);
            //builder.setActiveOutputFormat(__format);
        }

        public void prepareOperation(IRenderExecutionContext context)
        {
            outputRender.Clear();

            if (outputRender is IDocumentRender)
            {
                IDocumentRender outputRender_IDocumentRender = (IDocumentRender)outputRender;
                outputRender_IDocumentRender.setContext(context);
            }
        }

        /// <summary>
        /// Collects the operation start.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="composer">The composer.</param>
        /// <param name="dict">The dictionary.</param>
        /// <returns></returns>
        public PropertyCollectionDictionary collectOperationStart(IRenderExecutionContext context, IMetaContentNested composer, PropertyCollectionDictionary dict)
        {
            var level = composer.elementLevel;

            if (level == levelOfNewFile)
            {
                string filename = composer.name.getFilename().add(format.getDefaultExtension(), ".");

                string folder = "";

                if (composer.parent != null)
                {
                    folder = dict[composer.parent.path].getProperString(templateFieldBasic.path_folder);
                }

                //  filename = context.directoryScope.FullName.add(filename, "\\");

                string filepath = folder.add(filename, "\\");

                dict[composer.path].add(templateFieldBasic.path_dir, context.directoryRoot.FullName.add(folder, "\\"));
                dict[composer.path].add(templateFieldBasic.path_file, filename);
                dict[composer.path].add(templateFieldBasic.path_folder, folder);
                dict[composer.path].add(templateFieldBasic.path_output, filepath);

                context.regFileOutput(filename, composer.path, description);
            }
            else
            {
                dict[composer.path].add(templateFieldBasic.path_file, "");
            }

            return dict;
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Scopes the in operation.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="newScope">The new scope.</param>
        public void scopeInOperation(IRenderExecutionContext context, IMetaContentNested newScope)
        {
            var level = newScope.elementLevel;

            // scopeInDirectoryCheck(context, level, newScope);

            if (level == levelOfNewFile)
            {
                outputRender.Clear();

                // outputRender.addDocument(filename, true, getWritableFileMode.overwrite, format);
                //context.regFileOutput(filename, newScope.path, description);
                //  context.regFileOutput(filename, getFolderPathForLinkRegistry(context), description);
            }
            else if (level == levelOfNewPage)
            {
                string filename = context.data.getProperString("", templateFieldBasic.path_file);

                filename = context.directoryScope.FullName.add(filename, "\\");

                outputRender.addPage(filename, true, getWritableFileMode.overwrite, format);
            }
        }

        /// <summary>
        /// Compiles complex appends. Returns <c>appendType</c> if it is no match for this <c>runner</c>.
        /// </summary>
        /// <param name="ins">The ins.</param>
        /// <remarks>OK for multimpleruns</remarks>
        /// <returns><c>appendType.none</c> if executed, other <c>appendType</c> if no match for this run method</returns>
        protected appendType runComplexInstruction(IRenderExecutionContext context, docScriptInstruction ins)
        {
            ITextRender render = outputRender; //builder.documentBuilder;

            switch (ins.type)
            {
                //// ****************************** External executable
                case appendType.exe:

                    IExeAppend exe = (IExeAppend)ins[d.dsa_value];
                    exe.execute(context, render);

                    break;
                /////////////////////// ---------------------------------
                case appendType.button:

                    string btn_caption = (string)ins[d.dsa_title]; //.getProperString(d.dsa_title);
                    string btn_url = (string)ins[d.dsa_url]; //.getProperString(d.dsa_url);
                    bootstrap_color btn_color = (bootstrap_color)ins[d.dsa_styleTarget]; // ins.getProperEnum<bootstrap_color>(bootstrap_color.info, d.dsa_styleTarget);
                    bootstrap_size btn_size = (bootstrap_size)ins[d.dsa_stylerSettings]; // ins.getProperEnum<bootstrap_size>(bootstrap_size.md, d.dsa_stylerSettings);

                    render.AppendDirect(render.converter.GetButton(btn_caption, btn_url, btn_color.ToString(), btn_size.ToString()));

                    break;

                case appendType.attachment:
                    string att_caption = (string)ins[d.dsa_title]; //.getProperString(d.dsa_title);
                    string att_url = (string)ins[d.dsa_url]; //.getProperString(d.dsa_url);

                    string filename = ins[d.dsa_title].toStringSafe().getFilename();
                    att_url = att_url.or(filename);

                    bootstrap_color att_color = (bootstrap_color)ins[d.dsa_styleTarget]; //.getProperEnum<bootstrap_color>(bootstrap_color.primary, d.dsa_styleTarget);
                    bootstrap_size att_size = (bootstrap_size)ins[d.dsa_stylerSettings];// ins.getProperEnum<bootstrap_size>(bootstrap_size.sm, d.dsa_stylerSettings);

                    string output = "";
                    if (ins.containsKey(d.dsa_content))
                    {
                        DataTable dt = ins[d.dsa_content] as DataTable; //.getProperObject<DataTable>(d.dsa_content);
                        if (dt != null)
                        {
                            dataTableExportEnum format = (dataTableExportEnum)ins[d.dsa_format]; //>(dataTableExportEnum.excel, d.dsa_format);
                            if (imbSciStringExtensions.isNullOrEmpty(att_url))
                            {
                                att_url = dt.TableName.getFilename().getCleanPropertyName().Replace(" ", "").ToLower();
                            }

                            att_url = dt.serializeDataTable(format, att_url, context.directoryScope);
                        }
                        else
                        {
                            output = ins[d.dsa_content] as string;
                        }
                    }

                    if (ins.containsAllOfTypes(typeof(FileInfo)))
                    {
                        FileInfo fi = ins.getProperObject<FileInfo>();
                        att_url = fi.Name;
                        fi.CopyTo(att_url);
                    }

                    att_url = att_url.removeStartsWith(context.directoryScope.FullName).Trim('\\').removeStartsWith("/");

                    string bootstrapButton = render.converter.GetButton(att_caption, att_url, att_color.toString(), att_size.toString());

                    render.AppendDirect(bootstrapButton);
                    break;

                case appendType.i_chart:
                    List<object> parameters = (List<object>)ins[d.dsa_value]; //.getProperObject<List<Object>>(d.dsa_value);

                    string chstr = charts.chartTools.buildChart((chartTypeEnum)parameters[0], (chartFeatures)parameters[1], (DataTable)parameters[2], (chartSizeEnum)parameters[3], ins.getProperEnum<chartTypeEnum>(chartTypeEnum.none, d.dsa_format));
                    render.AppendDirect(chstr);
                    break;

                case appendType.direct:
                    render.AppendDirect((string)ins[d.dsa_contentLine]);
                    break;

                case appendType.toFile:
                    string toFile_outputpath = ins.getProperString(d.dsa_path);
                    toFile_outputpath = context.directoryScope.FullName.add(toFile_outputpath, "\\");
                    render.AppendToFile(toFile_outputpath, ins.getProperString(d.dsa_contentLine));
                    break;

                case appendType.fromFile:
                    string fromFile_sourcepath = ins[d.dsa_path].toStringSafe();
                    bool fromFile_isLocalSource = (bool)ins[d.dsa_on];
                    templateFieldSubcontent fromFile_key = (templateFieldSubcontent)ins[d.dsa_key];
                    if (fromFile_isLocalSource)
                    {
                        fromFile_sourcepath = context.directoryScope.FullName.add(fromFile_sourcepath, "\\");
                    }

                    if (File.Exists(fromFile_sourcepath))
                    {
                        render.AppendFromFile(fromFile_sourcepath, fromFile_key, fromFile_isLocalSource);
                    }
                    else
                    {
                        render.AppendLabel("File " + fromFile_sourcepath + " not found", true, bootstrap_style.style_warning);
                    }

                    break;

                case appendType.file:
                    string file_sourcepath = ins.getProperString(d.dsa_path);
                    string file_outputpath = ins.getProperString(d.dsa_name);
                    bool file_isDataTemplate = (bool)ins.getProperField(d.dsa_on);
                    bool file_isLocalSource = (bool)ins.getProperField(d.dsa_relative);
                    file_outputpath = context.directoryScope.FullName.add(file_outputpath, "\\");

                    if (file_isLocalSource)
                    {
                        fromFile_sourcepath = context.directoryScope.FullName.add(file_sourcepath, "\\");
                    }

                    string templateNeedle = ins.getProperString("none", d.dsa_styleTarget);
                    if (templateNeedle != "none")
                    {
                        deliveryUnit dUnit = (deliveryUnit)context.dUnit;
                        deliveryUnitItemContentTemplated templateItem = dUnit.findDeliveryUnitItemWithTemplate(templateNeedle);

                        string __filecontent = openBase.openFileToString(file_sourcepath, true, false);
                        templateItem.saveOutput(context, __filecontent, context.data, file_outputpath, file_isDataTemplate);
                    }
                    else
                    {
                        render.AppendFile(file_sourcepath, file_outputpath, file_isDataTemplate);
                    }

                    break;

                case appendType.image:
                    diagramModel model = ins.getProperObject<diagramModel>(d.dsa_value);
                    diagramOutputEngineEnum engine = ins.getProperEnum<diagramOutputEngineEnum>(diagramOutputEngineEnum.mermaid, d.dsa_format);
                    if (model != null)
                    {
                        deliveryUnit dUnit = (deliveryUnit)context.dUnit;
                        diagramOutputBase diaOutput = null;
                        if (outputRender is builderForMarkdown)
                        {
                            diaOutput = engine.getOutputEngine();
                        }
                        if (diaOutput == null) diaOutput = new diagramMermaidOutput();
                        string diaString = diaOutput.getOutput(model, dUnit.theme.palletes);
                        render.AppendDirect(diaString);
                    }
                    else
                    {
                        render.AppendImage(ins.getProperString(d.dsa_path), ins.getProperString(d.dsa_contentLine), ins.getProperString(d.dsa_name));
                    }

                    break;

                case appendType.math:
                    render.AppendMath(ins.getProperString(d.dsa_contentLine), ins.getProperString(d.dsa_format));
                    break;

                case appendType.label:
                    render.AppendLabel(ins.getProperString(d.dsa_contentLine), !ins.isHorizontal, ins.getProperString(d.dsa_styleTarget));
                    break;

                case appendType.panel:
                    render.AppendPanel(ins.getProperString(d.dsa_contentLine), ins.getProperString(d.dsa_title), ins.getProperString(d.dsa_description), ins.getProperString(d.dsa_styleTarget));
                    break;

                case appendType.frame:
                    render.AppendFrame(ins.getProperString(d.dsa_contentLine), ins.getProperInt32(-1, d.dsa_w), ins.getProperInt32(-1, d.dsa_h), ins.getProperString(d.dsa_title), ins.getProperString(d.dsa_description), (IEnumerable<string>)ins.getProperField(d.dsa_content));
                    break;

                case appendType.placeholder:
                    render.AppendPlaceholder(ins.getProperField(d.dsa_name), ins.isHorizontal);
                    break;

                case appendType.list:
                    render.AppendList((IEnumerable<object>)ins.getProperField(d.dsa_content), (bool)ins.getProperField(d.dsa_on));
                    break;

                case appendType.footnote:
                    throw new NotImplementedException("No implementation for: " + ins.type.ToString());
                    break;

                case appendType.c_line:

                    render.AppendHorizontalLine();

                    break;

                case appendType.c_data:
                    object dataSource = ins.getProperField(d.dsa_dataPairs, d.dsa_dataList, d.dsa_value);
                    string _head = ins.getProperString("", d.dsa_title, d.dsa_name);
                    string _foot = ins.getProperString("", d.dsa_footer, d.dsa_description);
                    if (dataSource is PropertyCollection)
                    {
                        PropertyCollection pairs = dataSource as PropertyCollection;
                        string sep = ins.getProperString(d.dsa_separator);
                        if (imbSciStringExtensions.isNullOrEmpty(_foot)) _foot = pairs.getAndRemoveProperString(d.dsa_footer, d.dsa_description);
                        if (imbSciStringExtensions.isNullOrEmpty(_head)) _head = pairs.getAndRemoveProperString(d.dsa_title, d.dsa_description);

                        if (!imbSciStringExtensions.isNullOrEmpty(_foot)) pairs.Add(d.dsa_footer.ToString(), _foot);
                        if (!imbSciStringExtensions.isNullOrEmpty(_head)) pairs.Add(d.dsa_title.ToString(), _head); // list.Insert(0, _head);

                        render.AppendPairs(pairs, ins.isHorizontal, sep);
                        // pair
                    }
                    else if (dataSource is IList<object>)
                    {
                        IList<object> list = dataSource as IList<object>;
                        if (!imbSciStringExtensions.isNullOrEmpty(_foot)) list.Add(_foot);
                        if (!imbSciStringExtensions.isNullOrEmpty(_head)) list.Insert(0, _head);

                        render.AppendList(list, false);
                    }
                    break;

                case appendType.c_pair:
                    render.AppendPair(ins.getProperString(d.dsa_key, d.dsa_name, d.dsa_title),
                        ins.getProperField(d.dsa_value, d.dsa_contentLine, d.dsa_dataField, d.dsa_content),
                        true,
                        ins.getProperString(" ", d.dsa_separator)
                        );
                    break;

                case appendType.c_table:
                    try
                    {
                        DataTable dt2 = (DataTable)ins[d.dsa_dataTable]; // .getProperObject<DataTable>(d.dsa_dataTable);
                        if (dt2.Rows.Count == 0)
                        {
                        }
                        dt2.ExtendedProperties.add(templateFieldDataTable.data_tablename, ins[d.dsa_title], true);
                        dt2.ExtendedProperties.add(templateFieldDataTable.data_tabledesc, ins[d.dsa_description], true);
                        dt2 = dt2.CompileTable(context as deliveryInstance, reportOutputFormatName.htmlViaMD, levelsOfNewDirectory); //  <------------- privremeni hack

                        render.AppendTable(dt2, true);
                    }
                    catch (Exception ex)
                    {
                    }
                    break;

                case appendType.c_link:
                    render.AppendLink(ins.getProperString(d.dsa_url, d.dsa_value),
                        ins.getProperString(d.dsa_name, d.dsa_contentLine, d.dsa_key), ins.getProperString(d.dsa_title, d.dsa_description, d.dsa_footer),
                        ins.getProperEnum<appendLinkType>(appendLinkType.link));
                    break;

                case appendType.section:
                    render.AppendSection(
                        ins.getProperString(d.dsa_contentLine),
                        ins.getProperString(d.dsa_title, d.dsa_name),
                        ins.getProperString(d.dsa_description),
                        ins.getProperField(d.dsa_content) as IEnumerable<string>);
                    break;

                case appendType.c_section:
                    render.AppendSection(
                        ins.getProperString(d.dsa_contentLine),
                        ins.getProperString(d.dsa_title, d.dsa_name),
                        ins.getProperString(d.dsa_description),
                        ins.getProperField(d.dsa_content) as IEnumerable<string>);
                    break;

                case appendType.c_open:
                    render.open(ins.getProperString("section", d.dsa_contentLine, d.dsa_name, d.dsa_key), ins.getProperString(d.dsa_title), ins.getProperString(d.dsa_description));
                    break;

                case appendType.c_close:
                    render.close(ins.getProperString("none", d.dsa_contentLine, d.dsa_name, d.dsa_key));
                    break;

                case appendType.source:
                    string sourcecode = "";
                    List<string> scode = ins.getProperObject<List<string>>(d.dsa_content);

                    sourcecode = scode.toCsvInLine(Environment.NewLine);
                    string sc_name = ins.getProperString("code", d.dsa_name);
                    string sc_desc = ins.getProperString("", d.dsa_description, d.dsa_footer);
                    string sc_title = ins.getProperString("", d.dsa_title);
                    string sc_typename = ins.getProperString("html", d.dsa_class_attribute);

                    render.open(sc_name, sc_title, sc_desc);
                    render.AppendCode(sourcecode, sc_typename);
                    render.close();
                    //render.close(ins.getProperString("none", d.dsa_contentLine, d.dsa_name, d.dsa_key));
                    break;

                default:
                    //executionError(String.Format("Instruction ({0}) not supported by runComplexInstruction() method", ins.type.ToString()), ins);
                    return ins.type;
                    break;
            }

            return appendType.none;
        }

        /// <summary>
        /// Runs the special instruction.
        /// </summary>
        /// <param name="ins">The ins.</param>
        /// <remarks>THIS IS NOT FOR LOOP EXECUTION</remarks>
        /// <returns></returns>
        protected appendType runSpecialInstruction(IRenderExecutionContext context, docScriptInstruction ins)
        {
            string externalPath = "";
            //  externalPath = ins.getProperString("temp", d.dsa_path);
            FileInfo efi;
            //   String dsaName = ins.getProperString(d.dsa_name);
            //  appendType innerAppend = ins.getProperEnum<appendType>(appendType.regular, d.dsa_innerAppend);

            switch (ins.type)
            {
                case appendType.i_load:

                    string importPath = (string)ins[d.dsa_path];

                    List<string> importLines = new List<string>();

                    appendType imType = ins.getProperEnum<appendType>(appendType.i_document, d.dsa_innerAppend);
                    //if (imType == appendType.i_document) // load document
                    //{
                    //    render.loadDocument(importPath, dsaName, form);
                    //}
                    //else if (imType == appendType.i_page) // load page
                    //{
                    //    render.loadPage(importPath, dsaName, form);
                    //}
                    //else /// -------------------------------------- import lines/content
                    //{
                    //    importLines = importPath.openFileToList(false);
                    //    foreach (String line in importLines)
                    //    {
                    //        render.Append(line, imType, true);
                    //    }
                    //}

                    break;

                case appendType.i_external:

                    efi = externalPath.getWritableFile(getWritableFileMode.newOrExisting);
                    List<string> externalLines = efi.openFileToList(false);

                    bool reCompile = ins.containsAnyOfKeys(d.dsa_recompile);
                    appendType apType = ins.getProperEnum<appendType>(appendType.regular, d.dsa_innerAppend);
                    ins.add(d.dsa_content, externalLines, false);

                    //docScriptInstruction cins = ins;

                    if (reCompile)
                    {
                        // recompilation     d.dsa_recompile
                        //docScriptInstructionCompiled cins = new docScriptInstructionCompiled(ins, script.flags);
                        ////scope.path;
                        //cins.compileTemplate(data);
                        ////{
                        ////compileInstruction(ins);/                    }
                        //if (apType != appendType.none) // internal append       dsa_innerAppend
                        //{
                        //    execute(cins, apType.getAppendTypeKind(), apType);
                        //}
                    }
                    break;

                case appendType.i_function:// recompile,

                    //render.AppendFunction(ins.getProperField(d.dsa_contentLine, d.dsa_value).toStringSafe());
                    break;

                case appendType.x_subcontent:

                    appendType act = ins.getProperEnum<appendType>(appendType.none, d.dsa_innerAppend, d.dsa_value);
                    templateFieldSubcontent subPart = ins.getProperEnum<templateFieldSubcontent>(templateFieldSubcontent.startOfFile, d.dsa_key);
                    switch (act)
                    {
                        case appendType.c_open:
                            outputRender.SubcontentStart(subPart, false);
                            break;

                        case appendType.c_close:
                            outputRender.SubcontentClose();
                            break;
                    }
                    break;

                default:

                    return ins.type;
                    break;
            }
            return appendType.none;
        }

        /// <summary>
        /// Executes the script instruction.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="instruction">The instruction.</param>
        public void executeScriptInstruction(IRenderExecutionContext context, docScriptInstructionCompiled instruction)
        {
            // outputRender.SubcontentStart(templateFieldSubcontent.main, false);

            if (outputRender is imbStringBuilderBase)
            {
                imbStringBuilderBase outputRender_imbStringBuilderBase = (imbStringBuilderBase)outputRender;
                outputRender_imbStringBuilderBase.data = context.data;
            }

            appendType final = runComplexInstruction(context, instruction);
            if (final != appendType.none) final = runSpecialInstruction(context, instruction);

            if (final == appendType.none)
            {
                // context.log("Complex instruction performed inside Output instruction");
                return;
            }

            if (outputRender is builderForMarkdown)
            {
                List<string> lines = new List<string>();
                if (instruction.containsKey(d.dsa_content))
                {
                    lines.AddRange((IEnumerable<string>)instruction[d.dsa_content]);
                }
                if (instruction.containsKey(d.dsa_contentLine))
                {
                    lines.Add((string)instruction[d.dsa_contentLine]);
                }
                for (int i = 0; i < lines.Count(); i++)
                {
                    outputRender.Append(lines[i], instruction.type, !instruction.isHorizontal);
                }
                return;
            }

            string str = instruction.getProperString(d.dsa_contentLine, d.dsa_title, d.dsa_description);

            appendRole role = instruction.getProperEnum<appendRole>(appendRole.paragraph, d.dsa_role);

            styleTextShot tshot = null;

            styleContainerShot sshot = null;

            if (role != appendRole.none)
            {
                tshot = new styleTextShot(role, context.theme);
                sshot = new styleContainerShot(role, instruction.type, context.theme);
            }
            else
            {
                tshot = new styleTextShot(instruction.type, context.theme);
            }

            //instruction.getProperObject<List<String>>(d.dsa_content);

            //if (lines == null) lines = instruction.getFirstOfType<List<String>>(true);

            //if (!str.isNullOrEmpty())
            //{
            //    lines.Add(str);
            //}

            //Boolean hasDocRender = false;
            //IDocumentRender docRender = outputRender as IDocumentRender; //builder.documentBuilder as IDocumentRender;

            //if (docRender != null)
            //{
            //    hasDocRender = true;
            //}

            //if (lines.Any())
            //{
            //    foreach (String logLine in lines)
            //    {
            //        //if (hasDocRender)
            //        //{
            //        //    if (tshot != null) docRender.ApplyStyle(tshot, builder.documentBuilder.c.getPencil(pencilType.point, 1));
            //        //    if (sshot != null) docRender.ApplyStyle(sshot, builder.documentBuilder.c.getPencil(pencilType.point, 1));
            //        //}
            //        outputRender.Append(logLine, instruction.type, !instruction.isHorizontal);

            //    }
            //}
            //else
            //{
            //    context.executionError("content and contentLine is missing for instruction", instruction);
            //}

            //  outputRender.SubcontentClose();

            //throw new NotImplementedException();
        }

        public void scopeOutOperation(IRenderExecutionContext context, IMetaContentNested oldScope)
        {
            // String filename = oldScope.name.getFilename();
            var level = oldScope.elementLevel;
            string filename = "";//context.data.getProperString("", templateFieldBasic.path_file);

            // IMetaContent mc = (IMetaContent)oldScope;

            if (!context.data.ContainsKey(templateFieldNavigation.mainmenu)) context.data.add(templateFieldNavigation.mainmenu, "", true);

            filename = oldScope.GetOutputPath((deliveryInstance)context, format, levelsOfNewDirectory, true);  //context.directoryScope.FullName.add(filename, "\\");

            FileInfo fi = null;

            if (level == levelOfNewFile)
            {
                fi = outputRender.saveDocument(filename, getWritableFileMode.overwrite, format);
                if (fi != null)
                {
                    //   context.regFileOutput(fi.FullName, oldScope.path, description);
                    //context.regFileOutput(fi.FullName, getFolderPathForLinkRegistry(context), description);
                }
            }
            else if (level == levelOfNewPage)
            {
                fi = outputRender.savePage(filename, format);
            }

            // scopeOutDirectoryCheck(context, level);
        }

        public docScript composeOperationStart(IRenderExecutionContext context, IMetaContentNested composer, docScript script)
        {
            return script;  //throw new NotImplementedException();
        }

        public docScript composeOperationEnd(IRenderExecutionContext context, IMetaContentNested composer, docScript script)
        {
            return script;
        }
    }
}