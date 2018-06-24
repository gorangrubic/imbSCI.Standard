// --------------------------------------------------------------------------------------------------------------------
// <copyright file="docScriptExecution.cs" company="imbVeles" >
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
#define DOLOG
#define THROWALL

namespace imbSCI.Reporting.script
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.files;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.style;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.style.shot;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Core.style.color;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.enums;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using d = docScriptArguments;

    /// <summary>
    /// Complete context of docScript execution
    /// </summary>
    /// <seealso cref="imbSCI.Cores.primitives.imbBindable" />
    public abstract class docScriptExecution : docScriptExecutionBase
    {
        /// <summary>
        /// Execution context with all resorces required internally.
        /// </summary>
        /// <remarks>
        /// <para>It will not start compilation instantly. Once you're ready <see cref="docScriptExecution.execute(string, imbSCI.Core.reporting.format.reportOutputFormatName, PropertyCollection, string)"/>  method should be called.</para>
        ///
        /// </remarks>
        /// <param name="__render">The render - particular output engine</param>
        /// <param name="__style">The style - colors and fonts</param>
        /// <param name="__script">The script - script with templated content</param>
        /// <param name="__data">The data - initial data</param>
        public docScriptExecution(string name, styleTheme __style, PropertyCollection __data, params ITextRender[] __renders) : base(name.add("_compile_log"), true)
        {
            List<ITextRender> renders = __renders.getFlatList<ITextRender>();
            setup(__style, __data, __renders);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="docScriptExecution"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="__style">The style.</param>
        /// <param name="__data">The data.</param>
        /// <param name="__renders">The renders.</param>
        public docScriptExecution(string name, styleTheme __style, PropertyCollection __data, IEnumerable<ITextRender> __renders) : base(name.add("_compile_log"), true)
        {
            List<ITextRender> renders = __renders.getFlatList<ITextRender>();

            setup(__style, __data, __renders);
        }

        protected docScriptExecution(string name) : base(name.getFilename().add("log", "_"), true)
        {
            // constructor for expansion class
        }

        protected void setup(styleTheme __style, PropertyCollection __data, IEnumerable<ITextRender> __renders)
        {
            theme = __style;

            foreach (ITextRender __rend in __renders)
            {
                builder.Add(__rend);
                if (doVerboseLog) log("docScriptExecution <---> render[" + __rend.GetType().Name + "]");
            }

            data = __data;
        }

        #region -------------------- RUN INSTRUCTION METHODS

        /// <summary>
        /// Instructions not covered yet
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        protected virtual appendType runOtherInstruction(docScriptInstruction ins)
        {
            // executionError(String.Format("Instruction ({0}) not supported by runOtherInstruction() method", ins.type.ToString()), ins);
            return ins.type;
        }

        /// <summary>
        /// Compiles complex appends. Returns <c>appendType</c> if it is no match for this <c>runner</c>.
        /// </summary>
        /// <param name="ins">The ins.</param>
        /// <remarks>OK for multimpleruns</remarks>
        /// <returns><c>appendType.none</c> if executed, other <c>appendType</c> if no match for this run method</returns>
        protected appendType runComplexInstruction(docScriptInstruction ins)
        {
            switch (ins.type)
            {
                case appendType.footnote:
                    throw new NotImplementedException("No implementation for: " + ins.type.ToString());
                    break;

                case appendType.c_line:
                    render.AppendHorizontalLine();

                    break;

                case appendType.c_data:
                    object dataSource = ins[d.dsa_dataPairs]; //.getProperField(d.dsa_dataPairs, d.dsa_dataList, d.dsa_value);
                    string _head = ins.getProperString("", d.dsa_title, d.dsa_name);
                    string _foot = ins.getProperString("", d.dsa_footer, d.dsa_description);
                    if (dataSource is PropertyCollection)
                    {
                        PropertyCollection pairs = dataSource as PropertyCollection;
                        string sep = ins.getProperString(d.dsa_separator);
                        if (_foot.isNullOrEmpty()) _foot = pairs.getAndRemoveProperString(d.dsa_footer, d.dsa_description);
                        if (_head.isNullOrEmpty()) _head = pairs.getAndRemoveProperString(d.dsa_title, d.dsa_description);

                        if (!_foot.isNullOrEmpty()) pairs.Add(d.dsa_footer.ToString(), _foot);
                        if (!_head.isNullOrEmpty()) pairs.Add(d.dsa_title.ToString(), _head); // list.Insert(0, _head);

                        render.AppendPairs(pairs, ins.isHorizontal, sep);
                        // pair
                    }
                    else if (dataSource is IList<object>)
                    {
                        IList<object> list = dataSource as IList<object>;
                        if (!_foot.isNullOrEmpty()) list.Add(_foot);
                        if (!_head.isNullOrEmpty()) list.Insert(0, _head);

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

                    DataTable dt = ins.getProperObject<DataTable>(d.dsa_dataTable);
                    if (dt.Rows.Count == 0)
                    {
                    }
                    dt.ExtendedProperties[templateFieldDataTable.data_tablename] = ins.getProperString(d.dsa_title);
                    dt.ExtendedProperties[templateFieldDataTable.data_tabledesc] = ins.getProperString(d.dsa_description);
                    render.AppendTable(dt);

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
                    render.open(ins.getProperString("section", d.dsa_contentLine, d.dsa_name, d.dsa_key));
                    break;

                case appendType.c_close:
                    render.close(ins.getProperString("none", d.dsa_contentLine, d.dsa_name, d.dsa_key));
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
        protected appendType runSpecialInstruction(docScriptInstruction ins)
        {
            string externalPath = "";
            externalPath = ins.getProperString("temp", d.dsa_path);
            FileInfo efi;
            string dsaName = ins.getProperString(d.dsa_name);
            appendType innerAppend = ins.getProperEnum<appendType>(appendType.regular, d.dsa_innerAppend);

            // reportOutputFormatName form = builder.format; //ins.getProperEnum<reportOutputFormatName>(render.formats.getDefaultFormat(), d.dsa_format);

            switch (ins.type)
            {
                //case appendType.i_chart:
                //    throw new NotImplementedException("No implementation for: " + ins.type.ToString());
                //    break;
                case appendType.i_meta:
                    data.add(ins.getProperField(d.dsa_key, d.dsa_name, d.dsa_path) as Enum, ins.getProperField(d.dsa_value, d.dsa_contentLine, d.dsa_content, d.dsa_dataList));
                    break;

                case appendType.x_data:
                    addOrUpdateStateData(data);
                    break;

                case appendType.i_dataSource:
                    //if (render is IDocumentRender)
                    //{
                    //    //render.AppendData(ins.getFirstOfType<PropertyCollection>(), ins.getProperEnum<existingDataMode>(existingDataMode.overwriteExisting), false);
                    //}
                    //else
                    //{
                    //}

                    data.AppendData(ins.getFirstOfType<PropertyCollection>(), ins.getProperEnum<existingDataMode>(existingDataMode.overwriteExisting));
                    break;

                case appendType.x_scopeIn:
                    string scopePath = ins.getProperString(d.dsa_path);
                    IMetaContentNested newScope = ins.getProperField(d.dsa_value) as IMetaContentNested;
                    if (newScope == null)
                    {
                    }
                    x_scopeIn(newScope);

                    break;

                case appendType.x_scopeOut:
                    x_scopeOut();

                    break;

                case appendType.i_load:

                    string importPath = ins.getProperString(d.dsa_path);

                    List<string> importLines = new List<string>();

                    appendType imType = ins.getProperEnum<appendType>(appendType.i_document, d.dsa_innerAppend);
                    if (imType == appendType.i_document) // load document
                    {
                        // render.loadDocument(importPath, dsaName, form);
                    }
                    else if (imType == appendType.i_page) // load page
                    {
                        // render.loadPage(importPath, dsaName, form);
                    }
                    else /// -------------------------------------- import lines/content
                    {
                        importLines = importPath.openFileToList(false);
                        foreach (string line in importLines)
                        {
                            render.Append(line, imType, true);
                        }
                    }

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
                        docScriptInstructionCompiled cins = new docScriptInstructionCompiled(ins, script.flags);
                        //scope.path;
                        cins.compileTemplate(data);
                        //{
                        //compileInstruction(ins);/                    }
                        if (apType != appendType.none) // internal append       dsa_innerAppend
                        {
                            execute(cins, apType.getAppendTypeKind(), apType);
                        }
                    }
                    break;

                case appendType.i_log:
                    string logStr = ins.getProperString(d.dsa_contentLine, d.dsa_title, d.dsa_description);
                    List<string> logLines = ins.getProperObject<List<string>>(d.dsa_content);

                    if (logLines == null) logLines = ins.getFirstOfType<List<string>>(true);

                    if (!logStr.isNullOrEmpty())
                    {
                        logLines.Add(logStr);
                    }

                    if (logLines.Any())
                    {
                        foreach (string logLine in logLines)
                        {
                            log("ins> " + logLine);
                        }
                    }
                    else
                    {
                        error("Log was called but no line/s passed with instruction", ins.type);
                    }
                    break;
                //case appendType.i_page:
                //    var newPage = render.addPage(dsaName, ins.containsAnyOfKeys(d.dsa_scopeToNew), getWritableFileMode.autoRenameThis, form);

                //    break;
                //case appendType.i_document:
                //    render.addDocument(dsaName, ins.containsAnyOfKeys(d.dsa_scopeToNew), getWritableFileMode.autoRenameExistingOnOtherDate, form);

                //    break;

                case appendType.i_function:// recompile,
                    render.AppendFunction(ins.getProperField(d.dsa_contentLine, d.dsa_value).toStringSafe());
                    break;

                case appendType.x_directory:
                    directoryOperation op = ins.getProperEnum<directoryOperation>(directoryOperation.selectIfExists, d.dsa_dirOperation);
                    op.doDirectoryOperation(externalPath, directoryScope, ins.containsAnyOfKeys(d.dsa_scopeToNew));
                    break;

                case appendType.x_save:
                    //if (innerAppend == appendType.i_document)
                    //{
                    //    efi = externalPath.getWritableFile(getWritableFileMode.newOrExisting);
                    //    render.saveDocument(dsaName, getWritableFileMode.autoRenameExistingOnOtherDate, form);
                    //} else if (innerAppend == appendType.i_page)
                    //{
                    //    //efi = externalPath.getWritableFile(getWritableFileMode.newOrExisting);

                    //    render.savePage(externalPath, form);

                    //} else

                    if (innerAppend == appendType.i_log)
                    {
                        externalPath = GetType().Name.add("log", "_").add(externalPath, "_").ensureEndsWith(".txt");
                        efi = externalPath.getWritableFile(getWritableFileMode.newOrExisting);
                        // this.ToString().saveStringToFile(efi.FullName, getWritableFileMode.autoRenameExistingOnOtherDate);
                    }
                    else if (innerAppend == appendType.i_meta)
                    {
                        if (ins.containsAnyOfKeys(d.dsa_metaContent, d.dsa_value, d.dsa_dataSource))
                        {
                            object itm = ins.getProperField(d.dsa_metaContent, d.dsa_value, d.dsa_dataSource);
                            if (itm != null)
                            {
                                objectSerialization.saveObjectToXML(itm, externalPath);
                            }
                        }
                        else
                        {
                            throw new ArgumentNullException("No meta content invoked :: ".add(innerAppend.ToString(), " inner:"));
                        }
                    }
                    else
                    {
                        error("Save was called but no clue in innerAppend what to save", ins.type);
                    }

                    break;

                case appendType.x_openTool:
                    /*
                    externalTool exTool = ins.getProperEnum<externalTool>(externalTool.notepadpp, d.dsa_externalTool);
                    if (externalPath == "temp" || externalPath.isNullOrEmpty()) externalPath = Path.GetTempFileName();
                    if (ins.containsAnyOfKeys(d.dsa_path))
                    {
                        exTool.run(externalPath);
                    } else if (ins.containsAnyOfKeys(d.dsa_content, d.dsa_contentLine))
                    {
                        List<string> lns = ins.getFirstOfType<List<string>>(true);
                        lns.Add(ins.getProperString("", d.dsa_contentLine));
                        saveBase.saveToFile(externalPath, lns);
                        exTool.run(externalPath);
                    } else if (ins.containsAnyOfKeys(d.dsa_metaContent, d.dsa_value, d.dsa_dataSource))
                    {
                        object itm = ins.getProperField(d.dsa_metaContent, d.dsa_value, d.dsa_dataSource);
                        if (itm != null)
                        {
                            objectSerialization.saveObjectToXML(itm, externalPath);
                        }
                        exTool.run(externalPath);
                    } else
                    {
                        exTool.run();
                    }
                    */
                    break;
                //case appendType.x_export:
                //    builder.sessionManager.setContext(this);
                //    appendType act = ins.getProperEnum<appendType>(appendType.none, d.dsa_innerAppend);
                //    String sessionname = ins.getProperString(d.dsa_path);
                //    reportOutputFormatName __format = ins.getProperEnum<reportOutputFormatName>(reportOutputFormatName.textMdFile, d.dsa_format);

                //    switch (act)
                //    {
                //        case appendType.x_subcontent:

                //            break;
                //        case appendType.c_open:
                //            reportAPI api = ins.getProperEnum<reportAPI>(reportAPI.imbMarkdown, d.dsa_styleTarget);
                //            elementLevelFormPreset preset = ins.getProperEnum<elementLevelFormPreset>(elementLevelFormPreset.none, d.dsa_stylerSettings);
                //            builder.sessionManager.startNewSession(sessionname, __format, api, preset);

                //            break;
                //        case appendType.c_close:
                //            builder.sessionManager.endSession(sessionname, __format);
                //            break;
                //    }
                //    break;
                case appendType.x_move:

                    bool movezone = Convert.ToBoolean(ins.getProperField(d.dsa_zoneFrame));
                    bool isRelative = Convert.ToBoolean(ins.getProperField(d.dsa_relative));

                    if (ins.containsKey(d.dsa_vector))
                    {
                        selectRange vector = ins.getProperField(d.dsa_vector) as selectRange;

                        if (movezone)
                        {
                            render.zone.margin.moveTopLeftByVector(vector);
                        }
                        else
                        {
                            render.c.moveByVector(vector, isRelative);
                        }
                    }
                    else if (ins.containsKey(d.dsa_cursorCorner))
                    {
                        textCursorZoneCorner direction = ins.getProperEnum<textCursorZoneCorner>(textCursorZoneCorner.none, d.dsa_cursorCorner);
                        if (direction != textCursorZoneCorner.none) // move in direction, for 1 or more steps
                        {
                            int steps = ins.getProperInt32(1, d.dsa_value, d.dsa_x, d.dsa_y, d.dsa_w, d.dsa_h);

                            if (isRelative)
                            {
                                selectRange vector = direction.toVector(steps);

                                if (movezone)
                                {
                                    render.zone.margin.moveTopLeftByVector(vector, true);
                                }
                                else
                                {
                                    render.c.moveInDirection(direction, steps);
                                }
                            }
                            else
                            {
                                render.c.moveToCorner(direction);
                            }
                        }
                    }
                    else if (ins.containsAnyOfKeys(d.dsa_w, d.dsa_h, d.dsa_x, d.dsa_y))
                    {
                        int x = ins.getProperInt32(0, d.dsa_x, d.dsa_w);
                        int y = ins.getProperInt32(0, d.dsa_y, d.dsa_h);

                        selectRange vector = new selectRange(x, y);

                        if (movezone)
                        {
                            render.zone.margin.moveTopLeftByVector(vector, isRelative);
                        }
                        else
                        {
                            if (isRelative)
                            {
                                render.c.moveFor(x, y);
                            }
                            else
                            {
                                render.c.moveTo(x, y);
                            }
                        }
                    }

                    //if (movezone)
                    //{
                    //    render.c.setMarginHere(textCursorZoneCorner.UpLeft);
                    //    render.c.moveToCorner(textCursorZoneCorner.UpLeft);
                    //}

                    break;

                case appendType.i_dataInDocument:
                    templateFieldBasic[] flds = new templateFieldBasic[0];
                    if (hasDocRender)
                    {
                        // docRender.AppendInfo(ins.getFirstOfType<PropertyCollection>(), false, ins.getProperEnum<templateFieldBasic[]>(flds));
                    }
                    if (ins.containsAnyOfKeys(d.dsa_value))
                    {
                        data.AppendData(ins.getFirstOfType<PropertyCollection>(), ins.getProperEnum<existingDataMode>(existingDataMode.overwriteExisting));
                    }

                    break;

                default:

                    return ins.type;
                    break;
            }
            return appendType.none;
        }

        /// <summary>
        /// Resolves target into <see cref="imbSCI.Core.reporting.zone.selectRangeArea"/> where <c>target</c> can be: <see cref="styleShotTargetEnum"/>, <see cref="imbSCI.Core.reporting.zone.selectRangeArea"/> or string path for <see cref="imbSCI.Data.collection.PropertyCollectionDictionary"/>
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>Area to apply style to</returns>
        /// <seealso cref="imbSCI.Core.reporting.zone.selectRangeAreaDictionary"/>
        /// <seealso cref="imbSCI.Core.reporting.zone.selectRangeArea"/>
        /// <seealso cref="imbSCI.Core.reporting.style.shot.IStyleInstruction"/>
        /// <seealso cref="imbSCI.Core.reporting.style.areaStyleInstruction"/>
        protected selectRangeArea resolveAreaForStyleShot(object target)
        {
            selectRangeArea output = null;
            if (target is styleShotTargetEnum)
            {
                output = resolveAreaForStyleShot((styleShotTargetEnum)target);
            }
            else if (target is selectRangeArea)
            {
                output = target as selectRangeArea;
            }
            else if (target is string)
            {
                output = metaContentRanges[scope.path];
            }
            else
            {
                output = null;
            }
            if (output == null) error("__result for resolveAreaForStyleShot(Object target=" + target.ToString() + ") ");
            return output;
        }

        /// <summary>
        /// Resolve <see cref="styleShotTargetEnum"/> into <see cref="imbSCI.Core.reporting.zone.selectRangeArea"/>
        /// </summary>
        /// <param name="target">The target - </param>
        /// <returns>Area to apply style with</returns>
        protected selectRangeArea resolveAreaForStyleShot(styleShotTargetEnum target)
        {
            selectRangeArea output = null;
            if (target == styleShotTargetEnum.unknown) target = styleShotTargetEnum.thisAppend;

            switch (target)
            {
                case styleShotTargetEnum.none:
                    output = null;
                    break;

                case styleShotTargetEnum.lastAppend:
                    output = metaContentRanges.getLastAny();
                    break;

                case styleShotTargetEnum.thisAppend:
                    output = c.pencilAbsolute;
                    break;

                default:
                case styleShotTargetEnum.thisScope:
                    output = metaContentRanges[scope.path];
                    break;
            }
            if (output == null) error("__result for resolveAreaForStyleShot(styleShotTargetEnum target=" + target.ToString() + ") ");
            return output;
        }

        /// <summary>
        /// Resolve <see cref="styleShotTargetEnum"/> into string path for <c>metaContentRanges</c>
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>
        /// Path for <c>metaContentRanges</c>
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">target - thisAppend is not applicable for path resolution</exception>
        /// <seealso cref="imbSCI.Data.collection.PropertyCollectionDictionary" />
        /// <seealso cref="imbSCI.Core.reporting.style.areaStyleInstructionStack" />
        protected string resolvePathForStyleShotArea(styleShotTargetEnum target)
        {
            if (target == styleShotTargetEnum.unknown) target = styleShotTargetEnum.thisAppend;

            switch (target)
            {
                case styleShotTargetEnum.none:
                    return "";
                    break;

                case styleShotTargetEnum.lastAppend:
                    var tmp = metaContentRanges.getLastAny();
                    return tmp.path;
                    break;

                case styleShotTargetEnum.thisAppend:
                    throw new ArgumentOutOfRangeException("target", "thisAppend is not applicable for path resolution");
                    break;

                default:
                case styleShotTargetEnum.thisScope:
                    return scope.path;
                    break;
            }
        }

        /// <summary>
        /// Runs the style instruction.
        /// </summary>
        /// <param name="ins">The ins.</param>
        /// <returns></returns>
        /// <remarks>This is for mutliple execution</remarks>
        /// <exception cref="System.NotImplementedException">No implementation for: " + ins.type.ToString()</exception>
        protected appendType runStyleInstruction(docScriptInstruction ins)
        {
            //if (!hasDocRender)
            //{
            //    return ins.type;
            //}

            switch (ins.type)
            {
                case appendType.s_settings:

                    if (ins.containsKey(d.dsa_background))
                    {
                        string bgColor = ins.getProperString(Color.White.ColorToHex(), d.dsa_background);
                        string fgColor = ins.getProperString(Color.Black.ColorToHex(), d.dsa_forecolor);

                        docRender.ApplyColor(bgColor, null, false);
                        docRender.ApplyColor(fgColor, null, true);
                    }
                    else if (ins.containsKey(d.dsa_innerAppend))
                    {
                        appendType ap_type = ins.getProperEnum<appendType>(appendType.regular, d.dsa_innerAppend);
                        appendRole ap_role = ins.getProperEnum<appendRole>(appendRole.paragraph, d.dsa_styleTarget);
                        styleApplicationFlag ap_flag = ins.getProperEnum<styleApplicationFlag>(styleApplicationFlag.allShots, d.dsa_stylerSettings);

                        styleAutoShotSet _shot = new styleAutoShotSet(ap_flag, ap_type, ap_role);
                        List<IStyleInstruction> shots = _shot.resolve(theme);
                        foreach (IStyleInstruction sh in shots)
                        {
                            docRender.ApplyStyle(sh, render.c.getPencil(pencilType.point, 1));
                        }
                    }
                    else if (ins.containsKey(d.dsa_variationRole))
                    {
                        aceColorPalette palette = ins.getProperObject<aceColorPalette>(d.dsa_stylerSettings, null);
                        int var = ins.getProperInt32(0, d.dsa_variationRole);

                        acePaletteShotResEnum res = ins.getProperEnum<acePaletteShotResEnum>(acePaletteShotResEnum.background, d.dsa_styleTarget);
                        Color col = palette.bgColors[var];
                        switch (res)
                        {
                            case acePaletteShotResEnum.foreground:
                                col = palette.fgColors[var];
                                break;

                            case acePaletteShotResEnum.background:
                                col = palette.bgColors[var];
                                break;

                            case acePaletteShotResEnum.border:
                                col = palette.tpColors[var];
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        docRender.ApplyColor(col.ColorToHex());
                        render.c.enter();
                    }

                    break;

                case appendType.s_width:

                    int key = ins.getProperInt32(0, d.dsa_key);
                    int val = ins.getProperInt32(0, d.dsa_value);
                    bool isHorizontal = ins.isHorizontal;
                    var corner = ins.getProperEnum<textCursorZoneCorner>(textCursorZoneCorner.none, d.dsa_cursorCorner);
                    bool isAutofit = (bool)ins.get(d.dsa_autostyling, false);
                    if (isHorizontal)
                    {
                        docRender.ApplyColumn(key, val, corner, isAutofit);
                    }
                    else
                    {
                        docRender.ApplyRow(key, val, corner, isAutofit);
                    }

                    break;

                case appendType.s_variation:

                    throw new NotImplementedException("No implementation for: " + ins.type.ToString());
                    break;

                case appendType.s_style:

                    selectRangeArea area = resolveAreaForStyleShot(ins[d.dsa_styleTarget]); //.get(d.dsa_styleTarget));

                    if (ins.containsAllOfKeys(d.dsa_autostyling, d.dsa_styleTarget))
                    {
                        styleApplicationFlag flag = ins.getProperEnum<styleApplicationFlag>(styleApplicationFlag.none, d.dsa_autostyling);

                        styleAutoShotSet shot = new styleAutoShotSet(flag);

                        styleStack.Add(shot, area);
                    }
                    else if (ins.containsAllOfKeys(d.dsa_stylerSettings, d.dsa_styleTarget))
                    {
                        IStyleInstruction shot = ins[d.dsa_stylerSettings] as IStyleInstruction;

                        if (shot == null)
                        {
                            throw new ArgumentNullException("dsa_stylerSettings", "docScript.s_style(null) - no IStyleInstruction for this instruction");
                        }
                        else
                        {
                            styleStack.Add(shot, area);
                        }
                    }
                    else
                    {
                    }
                    break;

                case appendType.s_zone:
                    if (ins.containsAnyOfTypes(typeof(textCursorZone)))
                    {
                        render.c.switchToZone(ins.getProperEnum<textCursorZone>(textCursorZone.unknownZone), ins.getProperEnum<textCursorZoneCorner>(textCursorZoneCorner.default_corner));
                    }
                    else if (ins.containsAnyOfTypes(typeof(cursorSubzoneFrame)))
                    {
                        // prosledjena mu je zona
                        render.c.setTempFrame(ins.getProperEnum<cursorSubzoneFrame>(cursorSubzoneFrame.fullFrame), ins.getProperEnum<textCursorZoneCorner>(textCursorZoneCorner.default_corner));
                    }
                    else if (ins.containsAnyOfTypes(typeof(cursorZoneRole)))
                    {
                        cursorZoneRole zr = ins.getProperEnum<cursorZoneRole>(cursorZoneRole.master);
                        int szi = ins.getProperInt32(0, docScriptArguments.dsa_value, docScriptArguments.dsa_level, docScriptArguments.dsa_priority);
                        render.c.setToSubFrame(zr, szi);
                    }
                    else
                    {
                        throw new ArgumentNullException(ins.type.ToString(), "Minimum applicable docScriptArguments are missing - can't execute instruction like this");
                    }
                    break;

                case appendType.s_alternate:

                    theme.styler.isDisabled = false;
                    break;

                case appendType.s_normal:
                    theme.styler.isDisabled = true;
                    break;

                case appendType.s_palette:
                    acePaletteRole newRole = (acePaletteRole)ins.getProperField(d.dsa_paletteRole);
                    if (ins.containsAnyOfKeys(d.dsa_dataTable, d.dsa_dataPairs, d.dsa_zoneFrame, d.dsa_variationRole))
                    {
                        theme.styler.layoutPaletteRole = newRole;
                    }
                    else
                    {
                        theme.styler.mainPaletteRole = newRole;
                    }
                    break;

                default:
                    //executionError(String.Format("Instruction ({0}) not supported by runStyleInstruction() method", ins.type.ToString()), ins);
                    //executionError(String.Format("Instruction ({0}) not supported by runSpecialInstruction() method", ins.type.ToString()), ins);
                    return ins.type;
                    break;
            }

            return appendType.none;
        }

        #endregion -------------------- RUN INSTRUCTION METHODS

        protected appendType executeOnce(docScriptInstruction ins, appendTypeKind kind)
        {
            // log("--- execution method: " + kind.ToString());
            appendType output = ins.type;

            if (!settings.supportedAppends.Contains(ins.type))
            {
                log("Ignoring instruction: {0} ({2}) -- not supported by builder: {1}".f(ins.type.ToString(), render.GetType().Name, kind.ToString()));
                return ins.type;
            }

            if (output != appendType.none) output = runSpecialInstruction(ins);
            if (output != appendType.none) output = runOtherInstruction(ins);

            return output;
        }

        /// <summary>
        /// Executes the specified ins.
        /// </summary>
        /// <param name="ins">The ins.</param>
        /// <param name="kind">The kind.</param>
        /// <returns></returns>
        protected virtual appendType execute(docScriptInstruction ins, appendTypeKind kind, appendType output)
        {
            //appendType output = ins.type;

            if (output != appendType.none) output = runComplexInstruction(ins);
            if (output != appendType.none) output = runStyleInstruction(ins);

            if (output != appendType.none)
            {
                string str = ins.getProperString(d.dsa_contentLine, d.dsa_title, d.dsa_description);

                appendRole role = ins.getProperEnum<appendRole>(appendRole.paragraph, d.dsa_role);

                styleTextShot tshot = null;

                styleContainerShot sshot = null;

                if (role != appendRole.none)
                {
                    tshot = new styleTextShot(role, theme);
                    sshot = new styleContainerShot(role, ins.type, theme);
                }
                else
                {
                    tshot = new styleTextShot(ins.type, theme);
                }

                List<string> lines = ins.getProperObject<List<string>>(d.dsa_content);

                if (lines == null) lines = ins.getFirstOfType<List<string>>(true);

                if (!str.isNullOrEmpty())
                {
                    lines.Add(str);
                }

                if (lines.Any())
                {
                    foreach (string logLine in lines)
                    {
                        if (hasDocRender)
                        {
                            if (tshot != null) docRender.ApplyStyle(tshot, render.c.getPencil(pencilType.point, 1));
                            if (sshot != null) docRender.ApplyStyle(sshot, render.c.getPencil(pencilType.point, 1));
                        }
                        render.Append(logLine, ins.type, !ins.isHorizontal);
                    }
                }
                else
                {
                    if (script.flags.HasFlag(docScriptFlags.ignoreArgumentValueNull))
                    {
                        if (doVerboseLog) log("Simple append called but no content nor contentLine provided");
                    }
                    else
                    {
                        executionError("Simple append called but no content nor contentLine provided", ins);
                    }
                }
            }

            return output;
        }

        ///// <summary>
        ///// Compiles the specified output path.
        ///// </summary>
        ///// <param name="startScope">The start scope.</param>
        ///// <param name="outputPath">The output path.</param>
        ///// <param name="targetFormat">The target format.</param>
        ///// <param name="script">The script.</param>
        ///// <param name="runstamp">The runstamp.</param>
        ///// <returns>
        ///// Compiled with success
        ///// </returns>
        ///// <exception cref="System.Exception">Builder.render is not ready! Check if the builder got IRender builder for target format: " + targetFormat.ToString()</exception>
        //public reportOutputRepository execute(IObjectWithPathAndChildSelector startScope, String outputPath, elementLevelFormPreset formPreset, reportOutputFormatName targetFormat, docScriptCompiled script, String runstamp="")
        //{
        //    logStartPhase("Compiler initialization", "Executing intro tasks and ensuring prerequirements");

        //    builder.setActiveOutputFormat(targetFormat, formPreset);

        //    scope = startScope;

        //    if (scope == null)
        //    {
        //        throw new aceReportException("Start Scope is null!");
        //    }

        //    IMetaContent metaScope = scope as IMetaContent;
        //    if (metaScope != null)
        //    {
        //        log("###Start scope structure###");
        //        nextTabLevel();
        //        log(metaScope.logStructure("startScope::"));
        //        prevTabLevel();
        //    }

        //    log("-- Content builder: " +builder.documentBuilder.GetType().Name + " - ");
        //   // builder.documentBuilder.setContext(this);

        //    if (runstamp.isNullOrEmpty()) runstamp = imbStringGenerators.getRandomString(4);
        //    log("-- Runstamp: " + runstamp + " - ");

        //    log("-- outputPath: " + outputPath + " - ");

        //    log("-- output format: " + targetFormat.ToString() + " - ");

        //    outputPath = Directory.GetCurrentDirectory().add(outputPath, "\\").add(runstamp, "\\");
        //    directoryScope = Directory.CreateDirectory(outputPath);

        //    if (directoryScope == null)
        //    {
        //        log("---- output directory failed to be selected !!!");
        //    } else
        //    {
        //        log("---- full path: " + directoryScope.FullName);
        //    }

        //    directoryRoot = new DirectoryInfo(directoryScope.FullName);
        //    reportOutputRepository output = new reportOutputRepository(directoryRoot, script.name);

        //    logStartPhase("Execute docScriptInstructions ", "total instructions (" + script.Count() + ")");

        //    if (render == null)
        //    {
        //        throw new aceReportException("Builder.render is not ready! Check if the builder got IRender builder for target format: " + targetFormat.ToString());
        //    }

        //    index = 0;
        //    foreach(docScriptInstruction instruction in script)
        //    {
        //        try
        //        {
        //            appendTypeKind kind = instruction.type.getAppendTypeKind();
        //            log(index.ToString("D4") + " " + render.c.ToString() +":"+ render.zone.ToString() + " " + instruction.ToString(docScriptInstructionTextFormatEnum.meta));

        //            appendType final = instruction.type;
        //                final = execute(instruction, kind, final);

        //            if (executionStopFlagCheck())
        //            {
        //                log("Execution is stopped by executionStopFlag!");
        //                break;
        //            }

        //            index++;
        //        }
        //        catch (Exception ex)
        //        {
        //            //if (errorPolicy.doThrow()) throw ex;
        //            executionError("Internal exception during an instruction execution", instruction, ex);
        //        }
        //    }

        //    logEndPhase();

        //    return output;
        //}
    }
}