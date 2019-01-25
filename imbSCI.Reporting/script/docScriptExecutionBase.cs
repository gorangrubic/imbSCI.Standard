// --------------------------------------------------------------------------------------------------------------------
// <copyright file="docScriptExecutionBase.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.script
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Core.reporting.render.config;
    using imbSCI.Core.reporting.style;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.delivery;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.exceptions;
    using imbSCI.Reporting.interfaces;
    using imbSCI.Reporting.links;
    using imbSCI.Reporting.meta.data;
    using imbSCI.Reporting.meta.page;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbACE.Core.core.builderForLog" />
    /// <seealso cref="imbSCI.Core.reporting.render.IRenderExecutionContext" />
    public abstract class docScriptExecutionBase : builderForMarkdown, ILogBuilder, IDocScriptExecutionContext, IRenderExecutionContext
    {
        private bool _doVerboseLog;

        /// <summary> </summary>
        public bool doVerboseLog
        {
            get
            {
                return _doVerboseLog;
            }
            set
            {
                _doVerboseLog = value;
                OnPropertyChanged("doVerboseLog");
            }
        }

        public void regFileOutput(string filepath, Enum idPath, string description, string title = "")
        {
            regFileOutput(filepath, idPath.ToString(), description, title);
        }

        /// <summary>
        /// Regs the file output.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="idPath">The identifier path.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public void regFileOutput(string filepath, string idPath, string description, string title = "")
        {
            //FileInfo savedfile = new FileInfo(filepath);
            // DirectoryInfo dirinfo = Directory.CreateDirectory(dirname);

            string __filename = Path.GetFileName(filepath);

            string locfilepath = filepath.removeStartsWith(directoryRoot.FullName);

            string dirname = Path.GetDirectoryName(locfilepath);

            if (title.isNullOrEmpty())
            {
                title = __filename; // forContent.name.imbTitleCamelOperation(true);
                if (Path.HasExtension(__filename))
                {
                    title = Path.GetFileNameWithoutExtension(__filename);
                }
                title = title.imbTitleCamelOperation(true, false);
            }

            locfilepath = ("".t(templateFieldBasic.root_relpath) + locfilepath).getWebPathBackslashFormat();

            if (!dirname.isNullOrEmpty())
            {
                linkRegistry[dirname].AddLink(title, description, locfilepath);
            }

            if (!idPath.isNullOrEmpty())
            {
                if (idPath != dirname)
                {
                    linkRegistry[idPath].AddLink(title, description, locfilepath);
                }
            }
        }

        public FileInfo saveFileOutput(string output, string filepath, Enum idPath, string description, string title = "")
        {
            return saveFileOutput(output, filepath, idPath.ToString(), description, title);
        }

        /// <summary>
        /// Saves the file output.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="filepath">The filepath.</param>
        /// <param name="idPath">The identifier path.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public FileInfo saveFileOutput(string output, string filepath, string idPath, string description, string title = "")
        {
            FileInfo savedfile = output.saveStringToFile(filepath, getWritableFileMode.overwrite, Encoding.UTF8);

            regFileOutput(savedfile.FullName, idPath, description);

            return savedfile;
        }

        /// <summary>
        ///
        /// </summary>
        ///
        public reportLinkRegistry linkRegistry { get; } = new reportLinkRegistry();

        /// <summary>
        ///
        /// </summary>
        public Dictionary<logOutputSpecial, object> specialLogOuts { get; } = new Dictionary<logOutputSpecial, object>();

        #region --- settings ------- Builder settings

        // private builderSettings _settings = new builderSettings();
        /// <summary>
        /// Builder settings
        /// </summary>
        public override builderSettings settings
        {
            get
            {
                return builder.settings;
            }
        }

        #endregion --- settings ------- Builder settings

        #region --- ERRROR HANDLING

        public onErrorPolicy errorPolicy = onErrorPolicy.onErrorContinue;

        protected void errorHandler(string msg, Exception innnerEx)
        {
            switch (errorPolicy)
            {
                default:
                case onErrorPolicy.none:
                case onErrorPolicy.unknown:
                case onErrorPolicy.onErrorContinue:
                    break;

                case onErrorPolicy.onErrorStop:
                    executionStopFlag = true;
                    break;

                case onErrorPolicy.onErrorThrowException:
                    executionStopFlag = true;
                    if (innnerEx != null)
                    {
                        throw new aceReportException(msg, aceReportExceptionType.executeScriptError, innnerEx);
                    }
                    else
                    {
                        throw new aceReportException(msg);
                    }
                    break;
            }
        }

        protected bool executionStopFlag = false;

        protected bool executionStopFlagCheck()
        {
            bool output = executionStopFlag;
            executionStopFlag = false;
            return output;
        }

        #endregion --- ERRROR HANDLING

        /// <summary>
        /// List of instructions had execution error
        /// </summary>
        public List<docScriptInstruction> instructionsWithError = new List<docScriptInstruction>();

        private ITextRender _render;

        public FileInfo getFileInfo(string basename, getWritableFileMode mode, reportOutputFormatName format)
        {
            if (format == reportOutputFormatName.none) format = builder.format;

            string output = formats.getFilename(basename, format);

            output = directoryScope.FullName.add(output, "\\");

            FileInfo fi = output.getWritableFile(mode);
            return fi;
        }

        /// <summary>
        /// renderer that will be used for output creation
        /// </summary>
        public ITextRender render
        {
            get
            {
                return builder.documentBuilder;
            }
            private set
            {
                _render = value;
                OnPropertyChanged("render");
            }
        }

        /// <summary>
        /// prepares builder to support format
        /// </summary>
        /// <param name="targetFormat">The target format.</param>
        /// <returns></returns>
        public bool setForFormat(reportOutputFormatName targetFormat)
        {
            bool formatChanged = false;
            if (builder.format != targetFormat)
            {
                formatChanged = true;
                builder.setActiveOutputFormat(targetFormat);
                OnPropertyChanged("render");
            }
            return formatChanged;
        }

        /// <summary>
        ///
        /// </summary>
        public builderSelector builder { get; set; } = new builderSelector(reportAPI.textBuilder, elementLevelFormPreset.sciReport);

        private styleTheme _style;
        private PropertyCollection _data = new PropertyCollection();
        internal ObjectPathParentAndRootMonitor scope_monitor = new ObjectPathParentAndRootMonitor(null);

        private selectRangeAreaDictionary _metaContentRanges;

        private IMetaContentNested _scope;

        private areaStyleInstructionStack _styleStack;

        /// <summary>
        /// Scheduled styling instructions -- used to process> current append or future append
        /// </summary>
        public areaStyleInstructionStack styleStack
        {
            get
            {
                if (_styleStack == null) _styleStack = new areaStyleInstructionStack(metaContentRanges);

                return _styleStack;
            }
            set { _styleStack = value; }
        }

        /// <summary>
        /// Dictionary of selectRangeArea entries for each metaContent member
        /// </summary>
        public selectRangeAreaDictionary metaContentRanges
        {
            get
            {
                if (_metaContentRanges == null) _metaContentRanges = new selectRangeAreaDictionary("metaContentRanges");
                return _metaContentRanges;
            }
            protected set
            {
                _metaContentRanges = value;
                OnPropertyChanged("metaContentRanges");
            }
        }

        #region --- outputRepositorium ------- resulting set of outputs ready for save

        private reportOutputRepository _outputRepositorium;

        /// <summary>
        /// resulting set of outputs ready for save
        /// </summary>
        public reportOutputRepository outputRepositorium
        {
            get
            {
                return _outputRepositorium;
            }
            set
            {
                _outputRepositorium = value;
                OnPropertyChanged("outputRepositorium");
            }
        }

        #endregion --- outputRepositorium ------- resulting set of outputs ready for save

        /// <summary>
        /// Gets or sets the directory current.
        /// </summary>
        /// <value>
        /// The directory current.
        /// </value>
        public abstract DirectoryInfo directoryScope { get; set; }

        /// <summary>
        /// Gets or sets the directory root.
        /// </summary>
        /// <value>
        /// The directory root.
        /// </value>
        public abstract DirectoryInfo directoryRoot { get; set; }

        /// <summary>
        /// Gets or sets the fileinfo.
        /// </summary>
        /// <value>
        /// The fileinfo.
        /// </value>
        public FileInfo fileInfo { get; set; }

        /// <summary>
        /// style to be applied
        /// </summary>
        public styleTheme theme
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
                OnPropertyChanged("style");
            }
        }

        /// <summary>
        /// Script to be executed
        /// </summary>
        /// <value>
        /// Collection of instructions
        /// </value>
        public docScript script { get; set; }

        /// <summary>
        /// Execution index - how many instructions are done so far
        /// </summary>
        public int index { get; protected set; } = 0;

        /// <summary>
        ///
        /// </summary>
        public PropertyCollectionDictionary dataDictionary { get; protected set; } = new PropertyCollectionDictionary();

        /// <summary>
        /// Bindable property
        /// </summary>
        public PropertyCollection data
        {
            get
            {
                return _data;
            }
            protected set
            {
                _data = value;
                OnPropertyChanged("data");
            }
        }

        /// <summary>
        /// Current scope - meta objects being processed
        /// </summary>
        /// \ingroup_disabled renderapi_service
        public IMetaContentNested scope
        {
            get
            {
                return _scope;
            }
            set
            {
                scope_monitor.update(value);
                _scope = value;
                OnPropertyChanged("scope");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="docScriptExecutionBase"/> class.
        /// </summary>
        /// <param name="__filename">The log output filename with extension .md. It will automatically set .md extension</param>
        /// <param name="autoSave">if set to <c>true</c> if will do automatic save on each log call.</param>
        protected docScriptExecutionBase(string __filename, bool autoSave) : base()
        {
        }

        /// <summary>
        /// Compiles the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="indata">The indata.</param>
        /// <returns></returns>
        public virtual docScriptCompiled compile(docScript source, PropertyCollectionDictionary indata = null)
        {
            docScriptCompiled output = new docScriptCompiled(source);
            script = source;
            logStartPhase("Compiling docScriptInstructions ", "total instructions (" + source.Count + ") -- total datasets (" + indata.Count() + ")");
            index = 0;
            foreach (docScriptInstruction instruction in source)
            {
                try
                {
                    if (doVerboseLog) log(index.ToString("D4") + " " + instruction.type.ToString());

                    output.compile(instruction, this, indata, data, source.flags);

                    index++;
                }
                catch (Exception ex)
                {
                    if (errorPolicy.doThrow()) throw ex;
                    executionError("Internal exception during instruction compilation", instruction, ex);
                }
            }

            logEndPhase();

            return output;
        }

        /// <summary>
        /// Adds the or update state data.
        /// </summary>
        /// <param name="targetData">The target data.</param>
        /// <returns></returns>
        internal PropertyCollection addOrUpdateStateData(PropertyCollection targetData = null)
        {
            if (targetData == null) targetData = data;

            targetData.setSystemStatus(this.getLastLine(), !targetData.containsKey(templateFieldBasic.sys_uid));

            targetData.addStringToMultikeys(directoryScope.FullName, false, templateFieldBasic.path_dir);
            targetData.addStringToMultikeys(directoryRoot.FullName, true, templateFieldBasic.path_output);

            //targetData.addStringToMultikeys(render, false, templateFieldBasic.path_format);
            if (script != null)
            {
                if (!script.flags.HasFlag(docScriptFlags.enableLocalCollection))
                {
                    IMetaContentNested mc = scope as IMetaContentNested;

                    var changes = scope_monitor.getState(true);

                    if (changes.IsTargetChanged)
                    {
                        if (changes.IsParentChanged)
                        {
                            var tmpData = mc.AppendDataFields();
                            targetData.AddRange(tmpData, false);
                        }
                        else
                        {
                            mc.AppendDataFields(targetData);
                        }
                    }
                }
            }

            return targetData;
        }

        /// <summary>
        /// xes the scope automatic create.
        /// </summary>
        /// <param name="newScope">The new scope.</param>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public virtual void x_scopeAutoCreate(IMetaContentNested newScope)
        {
            throw new aceReportException("This method is deprecated" + "Don-t use this");

            reportElementLevel level = newScope.elementLevel;
            reportOutputForm form = settings.forms[level].form;

            IMetaComposeAndConstruct conScope = newScope as IMetaComposeAndConstruct;
            IMetaContentNested metaScope = newScope as IMetaContentNested;
            string path = "";
            reportOutputFormatName fileformat = settings.forms[level].fileformat;

            if (metaScope is metaServicePage)
            {
                //form = conScope.form;
                metaServicePage msp = metaScope as metaServicePage;

                if (doVerboseLog) log(string.Format("---- service page scoped in --> overriding *form* {0} and format *{1}* for {2}", form.ToString(), fileformat.ToString(), newScope.GetType().Name));
            }

            if (form == reportOutputForm.none) return;

            if (form == reportOutputForm.unknown)
            {
                //  form = conScope.form;
                if (doVerboseLog) log(string.Format("---- use _reportOutputForm_ default *{0}* from {1}", form.ToString(), newScope.GetType().Name));
            }

            if (form == reportOutputForm.folder)
            {
                if (directoryScope == null)
                {
                    if (script.flags.HasFlag(docScriptFlags.nullDirectoryToCurrent))
                    {
                        directoryScope = new DirectoryInfo(Directory.GetCurrentDirectory());
                        log(string.Format("-- Directory scope was null - now it is set to [{0}] -- scope path is: {1}", directoryScope.FullName, scope.path));
                    }
                    else
                    {
                        throw new aceReportException("DirectoryCurrent is null!");
                    }
                }
                path = directoryScope.FullName.add(newScope.name, "\\");
                if (Directory.Exists(path))
                {
                    directoryScope = new DirectoryInfo(path);
                    log(string.Format("-- scope to existing folder: {0} for {1}", path, newScope.GetType().Name));
                }
                else
                {
                    directoryScope = Directory.CreateDirectory(path);
                    log(string.Format("-- create and scope to folder: {0} for {1}", path, newScope.GetType().Name));
                }
            }

            string filename = "";

            if (form == reportOutputForm.file)
            {
                filename = settings.formats.getFilename(metaScope.name, settings.forms[level].fileformat);
                // path = directoryCurrent.FullName.add(filename, "\\");

                switch (level)
                {
                    case reportElementLevel.documentSet:
                        _outputRepositorium.Clear();
                        break;

                    case reportElementLevel.document:
                        throw new NotImplementedException();
                        //render.addDocument(newScope.name, true, getWritableFileMode.overwrite, fileformat);
                        break;

                    case reportElementLevel.page:
                        throw new NotImplementedException();
                        //render.addPage(newScope.name, true, getWritableFileMode.overwrite, fileformat);/
                        break;

                    case reportElementLevel.block:
                        throw new NotImplementedException();
                        break;

                    case reportElementLevel.servicepage:
                        // log("-- service page is scoped in");
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (form == reportOutputForm.inParentFile)
            {
                switch (level)
                {
                    case reportElementLevel.servicepage:
                        throw new NotImplementedException();
                        //  render.addPage(newScope.name, true, getWritableFileMode.overwrite, fileformat);
                        break;

                    case reportElementLevel.page:
                        throw new NotImplementedException();
                        //render.addPage(newScope.name, true, getWritableFileMode.overwrite, fileformat);
                        break;
                }
            }
        }

        public IDocumentRender docRender
        {
            get
            {
                return render as IDocumentRender;
            }
        }

        public bool hasDocRender
        {
            get
            {
                return (docRender != null);
            }
        }

        /// <summary>
        /// xes the scope automatic save.
        /// </summary>
        /// <param name="oldScope">The old scope.</param>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public virtual void x_scopeAutoSave(IMetaContentNested oldScope)
        {
            throw new aceReportException("This method is deprecated" + "Don-t use this");

            reportElementLevel level = oldScope.elementLevel;
            reportOutputForm form = settings.forms[level].form;
            if (form == reportOutputForm.none) return;

            IMetaComposeAndConstruct conScope = oldScope as IMetaComposeAndConstruct;
            IMetaContentNested metaScope = oldScope as IMetaContentNested;
            string path = "";

            reportOutputFormatName fileformat = settings.forms[level].fileformat;

            if (metaScope is metaServicePage)
            {
                // form = conScope.form;
                metaServicePage msp = metaScope as metaServicePage;

                log(string.Format("---- service page scoped --> overriding *form* {0} and format *{1}* for {2}", form.ToString(), fileformat.ToString(), oldScope.GetType().Name));
            }

            if (form == reportOutputForm.unknown)
            {
                //form = conScope.form;
                log(string.Format("---- use _reportOutputForm_ default *{0}* from {1}", form.ToString(), oldScope.GetType().Name));
            }

            if (form == reportOutputForm.folder)
            {
                base.directoryScope = base.directoryScope.Parent;
                log("    directory *parent* scoped: " + base.directoryScope.FullName);
            }

            string filename = "";

            if (form == reportOutputForm.file)
            {
                filename = settings.formats.getFilename(metaScope.name, fileformat);
                //  path = directoryCurrent.FullName.add(filename, "\\");

                switch (level)
                {
                    case reportElementLevel.documentSet:
                        throw new NotImplementedException();
                        break;

                    case reportElementLevel.document:

                        string fn = metaScope.document.name;
                        throw new NotImplementedException();

                        //  render.AppendInfo(data, false, settings.forms[level].customProperties.ToArray());
                        //render.saveDocument(fn, getWritableFileMode.autoRenameExistingOnOtherDate, fileformat);
                        break;

                    case reportElementLevel.page:

                        throw new NotImplementedException();
                        //render.savePage(oldScope.name, fileformat);
                        //render.AppendInfo(data, false, settings.forms[level].customProperties.ToArray());
                        break;

                    case reportElementLevel.block:
                        throw new NotImplementedException();
                        break;

                    case reportElementLevel.servicepage:

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            //PropertyCollection mdata = metaScope.AppendDataFields(data);
        }

        /// <summary>
        /// Scope moved into provided object
        /// </summary>
        /// <param name="newScope">The new scope.</param>
        protected virtual void x_scopeIn(IMetaContentNested newScope)
        {
            //   log("Scope in");

            /// opening new scope
            scope = newScope;

            addOrUpdateStateData(data);

            x_scopeAutoCreate(newScope);

            // scope in move
            c.moveToCorner(render.settings.cursorBehaviour.scopeInMove);

            // marking  TopLeft corder of the area
            var scopeArea = metaContentRanges.Add(scope.path, c.x, c.y);

#if (DOLOG)
            log(String.Format("x_scope to {0}", newScope.path));
#endif
        }

        /// <summary>
        /// Exit to parent scope
        /// </summary>
        /// <param name="newScope">The new scope.</param>
        protected void x_scopeOut()
        {
            //   log("Scope out");
            x_scopeAutoSave(scope);
            selectRangeAreaNamed scopeArea = null;
            // scope out
            if (scope == null)
            {
                scope = this as IMetaContentNested;

                log("Scope reached root position (null) -- this[" + GetType().Name + "]");
            }
            else
            {
                scopeArea = metaContentRanges[scope.path];
                scopeArea.setEnd(c.x, c.y);
                scope = (IMetaContentNested)scope.parent;
            }

            // scope out move
            c.moveToCorner(render.settings.cursorBehaviour.scopeOutMove);

            /// calls for execution of pending styles
            styleStack.execute(render, 10, this);

#if (DOLOG)
            log(String.Format("x_scopeOut: {0}", scope.getPathToParent(scope)));
#endif
        }

        private ILogable _externalLog;

        /// <summary> </summary>
        public ILogable externalLog
        {
            get
            {
                return _externalLog;
            }
            set
            {
                _externalLog = value;
                OnPropertyChanged("externalLog");
            }
        }

        private string _name = "";

        /// <summary>
        ///
        /// </summary>
        public string name
        {
            get
            {
                if (_name.isNullOrEmpty()) _name = GetType().Name;
                return _name;
            }
            protected set { _name = value; }
        }

        //private selectRangeAreaDictionary _metaContentRanges;
        ///// <summary>
        /////
        ///// </summary>
        //public selectRangeAreaDictionary metaContentRanges
        //{
        //    get { return _metaContentRanges; }
        //    set { _metaContentRanges = value; }
        //}

        selectRangeAreaDictionary IRenderExecutionContext.metaContentRanges
        {
            get
            {
                return metaContentRanges;
            }
        }

        public abstract object dUnit { get; }
        public abstract string logContent { get; }
        Dictionary<logOutputSpecial, object> IRenderExecutionContext.specialLogOuts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //reportOutputRepository IRenderExecutionContext.outputRepositorium { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        // reportLinkRegistry IDocScriptExecutionContext.linkRegistry => throw new NotImplementedException();

        /// <summary>
        /// Reports an error with optional message and exception
        /// </summary>
        /// <param name="msg">Custom message about the error</param>
        /// <param name="atype">Type of Append operation that caused error</param>
        /// <param name="ex">Exception if happen</param>
        public void error(string msg, appendType atype = appendType.none, Exception ex = null)
        {
            string errmsg = string.Format("--- *docScript[{0}] IRender error* _{1}_ :: __appendType.{2}__", index, render.GetType().Name, atype.toStringSafe());
            if (atype == appendType.none) errmsg = errmsg.add("--- ---- it wasn't Append operation ---", Environment.NewLine);
            errmsg = errmsg.add(msg, Environment.NewLine);
            if (externalLog != null) externalLog.log(errmsg);
            log(errmsg);
            if (ex != null)
            {
                //string exst = ex.describe();
                //log(exst);
                //if (externalLog != null) externalLog.log(exst);
            }
            if (errorPolicy.doThrow()) throw new aceReportException(errmsg, aceReportExceptionType.executeScriptError, ex);

            errorHandler(errmsg, ex);
        }

        /// <summary>
        /// Reports template compilation error
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="compiled">The compiled.</param>
        /// <param name="ex">The ex.</param>
        protected void compileError(string message, docScriptInstructionCompiled compiled, Exception ex = null)
        {
            string errmsg = string.Format((string)"*docScript[{0}] failed to compile* _", (object)index) + compiled.type.toStringSafe() + "_ :" + message;

            if (externalLog != null) externalLog.log(errmsg);

            log(errmsg);

            open("section");

            AppendLine();

            AppendHeading(string.Format("Template compilation *failed* for arguments ({0}):", compiled.keyListForFailedStrings.Count.ToString()), 5);
            AppendList(compiled.keyListForFailedStrings.getFlatList<object>(), true);

            foreach (docScriptArguments arg in compiled.keyListForFailedStrings)
            {
                AppendPair(arg, compiled.getProperString(arg), true);
            }

            AppendHeading(string.Format("Data fields that were missing during template compilation ({0}):", compiled.missingData.Count.ToString()), 5);
            AppendList(compiled.missingData.getFlatList<object>(), true);

            AppendLine();

            close();
            //compiled.ke

            //if (ex != null)
            //{
            //    string exst = ex.describe();
            //    log(exst);
            //    if (externalLog != null) externalLog.log(exst);

            //    ex.describe(this);
            //}

            instructionsWithError.Add(compiled);
            //if (errorPolicy.doThrow()) throw new aceReportException(errmsg, ex);
            errorHandler(errmsg, ex);
        }

        /// <summary>
        /// Reporting <c>docScriptInstructionCompiled</c> execution error
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ins">The ins.</param>
        /// <param name="ex">The ex.</param>
        protected void executionError(string message, docScriptInstruction ins, Exception ex = null)
        {
            string errmsg = string.Format((string)"*docScript[{0}] execution error* _", (object)index) + ins.type.toStringSafe() + "_ :" + message;
            if (externalLog != null) externalLog.log(errmsg);
            log(errmsg);
            //if (ex != null)
            //{
            //    string exst = ex.describe();
            //    log(exst);
            //    if (externalLog != null) externalLog.log(exst);

            //    ex.describe(this);
            //}

            instructionsWithError.Add(ins);
            // if (errorPolicy.doThrow()) throw new aceReportException(errmsg, ex);
            errorHandler(errmsg, ex);
        }

        public void compileError(string message, object compiled, Exception ex = null)
        {
            compileError(message, compiled as docScriptInstructionCompiled, ex);
        }

        public void executionError(string message, object ins, Exception ex = null)
        {
            compileError(message, ins as docScriptInstructionCompiled, ex);
        }

        public abstract ILogBuilder logStartPhase(string title, string message);

        public abstract ILogBuilder logEndPhase();

        public abstract void save(string destination_path = "");

        public abstract void log(string message);
    }
}