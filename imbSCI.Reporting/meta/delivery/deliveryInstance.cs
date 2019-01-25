// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryInstance.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.delivery
{
    using imbSCI.Core.collection;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.render;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using imbSCI.Reporting.delivery;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.exceptions;
    using imbSCI.Reporting.meta.delivery.items;
    using imbSCI.Reporting.meta.documentSet;
    using imbSCI.Reporting.resources;
    using imbSCI.Reporting.script;
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Managed report generation and deliveryInstance
    /// </summary>
    public class deliveryInstance : docScriptExecution, IMetaLevelElement, IDeliveryComposer, IRenderExecutionContext, IObjectWithReportLevel
    {


        public override DirectoryInfo directoryRoot
        {
            get { return _directoryRoot; }
            set { _directoryRoot = value; }
        }

        private DirectoryInfo _directoryScope;

        private DirectoryInfo _directoryRoot;

        public ILogBuilder aceLog { get; set; }

        public override object dUnit
        {
            get
            {
                return unit;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="deliveryInstance"/> class.
        /// </summary>
        /// <param name="__unit">The unit.</param>
        public deliveryInstance(deliveryUnit __unit) : base(__unit.name)
        {
            unit = __unit;

            //registerLogBuilder(logOutputSpecial.reportContext, true);

            specialLogOuts.Add(logOutputSpecial.reportContext, this);
        }

        /// <summary>
        /// Appends the data fields.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            return addOrUpdateStateData(data);
        }

        /// <summary>
        /// Calls prepare and then <see cref="execute(metaDocumentRootSet, string, PropertyCollection)"/>
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="runstamp">The runstamp.</param>
        /// <param name="__data">The data.</param>
        public void executeAndSave(metaDocumentRootSet report, string runstamp, PropertyCollection __data = null)
        {
            executePrepare(report, runstamp, __data);
            execute(report, runstamp, __data);
        }

        /// <summary>
        ///
        /// </summary>
        public bool doBuildDeliveryMeta { get; set; }

        /// <summary>
        /// Executes the and save.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="runstamp">The runstamp.</param>
        /// <param name="__data">The data.</param>
        protected void execute(metaDocumentRootSet report, string runstamp, PropertyCollection __data = null)
        {
            dataDictionary = new PropertyCollectionDictionary();

            if (!unit.scriptFlags.HasFlag(docScriptFlags.disableGlobalCollection))
            {
                logStartPhase("Collect data", "#2 phase of data collecting");
                dataDictionary = report.collect(dataDictionary);
                logEndPhase();
            }
            else
            {
                log("Global data collection disabled by unit.scriptFlags");
            }

            logStartPhase("Script composing", "#3 phase of data composing");

            script = new docScript("Script - unit[" + unit.name + "] - runstamp: " + runstamp);
            script.flags = unit.scriptFlags;

            script = report.compose(script);
            if (doBuildDeliveryMeta)
            {
                string scMeta = script.ToString(docScriptInstructionTextFormatEnum.meta);
                scMeta.saveStringToFile(directoryRoot.FullName.add("deliveryMeta.md", "\\"), getWritableFileMode.autoRenameExistingOnOtherDate, Encoding.UTF8);

                string csMeta = script.ToString(docScriptInstructionTextFormatEnum.cs_compose);
                csMeta.saveStringToFile(directoryRoot.FullName.add("deliveryMeta.cs", "\\"), getWritableFileMode.autoRenameExistingOnOtherDate, Encoding.UTF8);
            }

            logEndPhase();

            setup(unit.theme, data, renders.getTextRenders());

            logStartPhase("Script compilation", "#4 applying data to content");

            compiled = compile(script, dataDictionary);

            repo = new reportOutputRepository(directoryRoot, script.name);

            logStartPhase("Execute docScriptInstructions ", "5# executing scripttotal instructions (" + script.Count() + ")");

            index = 0;
            int tIndex = 1000;

            int mediumTimerLimit = 10;
            int mediumTimerIndex = 0;

            foreach (docScriptInstructionCompiled instruction in compiled)
            {
                try
                {
                    appendTypeKind kind = instruction.type.getAppendTypeKind();

                    if (doVerboseLog) log(index.ToString("D4") + " " + instruction.ToString(docScriptInstructionTextFormatEnum.meta));

                    //afinal = appendType.none;

                    appendType final = executeOnce(instruction, kind);

                    if (final != appendType.none) final = runStyleInstruction(instruction);

                    if (final == appendType.none)
                    {
                    }
                    else
                    {
                        var scopeLevel = scope.elementLevel;

                        foreach (var it in unit.outputByLevel[scopeLevel])
                        {
                            deliveryUnitItemSimpleRenderOutput output = it as deliveryUnitItemSimpleRenderOutput;

                            if (output != null)
                            {
                                output.executeScriptInstruction(this, instruction);
                                final = appendType.none;
                                //  builder = output.builder;
                            }
                        }
                    }

                    if (executionStopFlagCheck())
                    {
                        log("Execution is stopped by executionStopFlag!");
                        break;
                    }

                    index++;
                }
                catch (Exception ex)
                {
                    string msg = Environment.NewLine + "Report script execution [" + index + "/" + compiled.Count() + "] error. Instruction: [" + instruction.ToString(docScriptInstructionTextFormatEnum.cs_compose) + "]";
                    msg = msg.addLine("-- scoped meta object: path:[" + scope.path + "] -- [" + scope.name + "] -- [" + scope.GetType().Name + "]");
                    msg = msg.addLine("-- directory: [" + directoryScope.FullName + "]");
                    msg = msg.addLine("-- exception: [" + ex.GetType().Name + "] => [" + ex.Message + "]");

                    var axe = new aceReportException(msg + "Report instruction [" + instruction.type.ToString() + "] exception");
                    //if (axe.callInfo != null)
                    //{
                    //    msg = msg.addLine("-- source of ex: [" + axe.callInfo.sourceCodeLine + "]");
                    //    msg = msg.addLine("-- source file:  [" + axe.callInfo.Filepath + "]");
                    //    msg = msg.addLine("-- source line:  [" + axe.callInfo.line + "]");
                    //    msg = msg.addLine("-- source class:  [" + axe.callInfo.className + "]");
                    //}

                    log(msg + Environment.NewLine);
                    string path = "errorReport_" + index.ToString() + ".txt";
                    path = directoryScope.FullName.add(path, "\\");
                    msg.saveStringToFile(path, getWritableFileMode.autoRenameExistingOnOtherDate, Encoding.UTF8);

                    if (errorPolicy.doThrow()) throw axe;
                    executionError("Internal exception during an instruction execution", instruction, ex);
                }

                #region ----------------------------------

                if (compiled.Count() > 1000)
                {
                    if (tIndex > 0)
                    {
                        tIndex--;
                    }
                    else
                    {
                        mediumTimerIndex++;
                        double ratio = ((double)index) / ((double)compiled.Count());
                        aceLog.log("Report generation at [" + ratio.ToString("P") + "] done");
                        tIndex = 1000;
                    }
                }
                if (mediumTimerIndex > mediumTimerLimit)
                {
                    var memBefore = GC.GetTotalMemory(false);
                    GC.Collect();
                    var memBefore2 = memBefore - GC.GetTotalMemory(false);
                    aceLog.log("-- garbage collector invoked - memory released: " + memBefore2.getMByteCountFormated());
                    // aceLog.saveAllLogs(true);
                    mediumTimerIndex = 0;
                }

                #endregion ----------------------------------
            }

            GC.Collect();
            GC.WaitForFullGCComplete();

            logEndPhase();

            AppendPairs(data, false, " -> ");

            log("Completed");

            foreach (IDeliveryUnitItem item in unit.items)
            {
                item.reportFinishedOperation(this);
            }

            // aceLog.consoleControl.removeFromOutput(this);
        }

        /// <summary>
        /// Cleans the output folder.
        /// </summary>
        /// <exception cref="System.Exception">
        /// The deliveryInstance instance is not ready. Call executePrepare first
        /// or
        /// Sandbox Exception: can't delete files outside application folder [" + fileOpsBase.applicationFolder + "]. Requested delete path [
        /// </exception>
        public void cleanOutputFolder()
        {
            if (directoryRoot == null)
            {
                throw new aceReportException("The deliveryInstance instance is not ready. Call executePrepare first");
            }

            fileOpsBase.deleteAll(directoryRoot.FullName);
        }

        /// <summary>
        /// Prepares the output folders --
        /// </summary>
        /// <param name="runstamp">The runstamp or adjuster output folder name</param>
        protected void folderPrepare(string runstamp)
        {
            string startPath = unit.outputpath.add(runstamp, Path.DirectorySeparatorChar);

            directoryScope = Directory.CreateDirectory(startPath);
            directoryRoot = Directory.CreateDirectory(startPath);
        }

        protected void executePrepare(metaDocumentRootSet report, string runstamp, PropertyCollection __data = null)
        {
            //  aceLog.consoleControl.setAsOutput(this, "Delivery");

            folderPrepare(runstamp);

            report.context = this;
            theme = unit.theme;

            AppendLine();
            AppendLine();

            logStartPhase("Delivery init", "initialization of Delivery instance");

            if (__data == null) __data = new PropertyCollection();
            __data = AppendDataFields(__data);
            __data = unit.AppendDataFields(__data);
            if (__data != null) data.AppendData(__data, existingDataMode.overwriteExisting);

            scope = report;
            report.context = this;

            logStartPhase("Prepare phase", "#1 phase of deliveryInstance");

            data.add(templateFieldBasic.root_relpath, "");
            data.add(templateFieldBasic.report_folder, report.name);

            var vls = Enum.GetValues(typeof(templateFieldSubcontent));
            foreach (object vl in vls)
            {
                data.Add(vl, "");
            }

            // unit.blockBuilder.BuildDynamicNavigationTemplates(this, data);
            //            unit.blockBuilder.BuildNavigationTemplates(this, data);

            unit.executePrepare();

            foreach (IDeliveryUnitItem item in unit.items)
            {
                item.prepareOperation(this);
            }

            unit.updateTemplates(this);

            foreach (IDeliveryUnitItem item in unit.outputContent)
            {
                item.prepareOperation(this);
            }

            unit.describe(this);
        }

        /// <summary>
        /// Composes the operation start.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="composer">The composer.</param>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public docScript composeOperationStart(IRenderExecutionContext context, IMetaContentNested composer, docScript script)
        {
            var level = composer.elementLevel;

            foreach (IDeliveryUnitItem item in unit.outputByLevel[level])
            {
                item.composeOperationStart(context, composer, script);
            }

            foreach (IDeliveryUnitItem item in unit.itemByLevel[level])
            {
                item.composeOperationStart(context, composer, script);
            }

            return script;
        }

        /// <summary>
        /// Composes the operation end.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="composer">The composer.</param>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public docScript composeOperationEnd(IRenderExecutionContext context, IMetaContentNested composer, docScript script)
        {
            var level = composer.elementLevel;

            foreach (IDeliveryUnitItem item in unit.outputByLevel[level])
            {
                item.composeOperationEnd(context, composer, script);
            }

            foreach (IDeliveryUnitItem item in unit.itemByLevel[level])
            {
                item.composeOperationEnd(context, composer, script);
            }

            return script;
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

            foreach (IDeliveryUnitItem item in unit.outputByLevel[level])
            {
                item.collectOperationStart(context, composer, dict);
            }

            foreach (IDeliveryUnitItem item in unit.itemByLevel[level])
            {
                if (item is deliveryUnitItemContentTemplated)
                {
                }
                else
                {
                    item.collectOperationStart(context, composer, dict);
                }
            }

            foreach (IDeliveryUnitItem item in unit.itemByLevel[level])
            {
                if (item is deliveryUnitItemContentTemplated)
                {
                    item.collectOperationStart(context, composer, dict);
                }
            }

            return dict;
        }

        /// <summary>
        /// Scope in
        /// </summary>
        /// <param name="newScope">The new scope.</param>
        public override void x_scopeAutoCreate(IMetaContentNested newScope)
        {
            IMetaContentNested metaScope = newScope as IMetaContentNested;
            scope = newScope;
            var level = scope.elementLevel;

            if (unit.scriptFlags.HasFlag(docScriptFlags.useDataDictionaryForLocalData))
            {
                PropertyCollection pc = dataDictionary[scope.path];
                data.AppendData(pc, existingDataMode.overwriteExisting);
            }

            if (unit.scriptFlags.HasFlag(docScriptFlags.enableLocalCollection))
            {
                AppendDataFields(data);

                IAppendDataFields appScope = scope as IAppendDataFields;
                if (appScope != null)
                {
                    appScope.AppendDataFields(data);
                }
            }

            foreach (IDeliveryUnitItem item in unit.outputByLevel[level])
            {
                item.scopeInOperation(this, scope);
                if (reportingCoreManager.doVerboseLog) aceLog.log("output[" + item.name + "] triggered by x_scopeAutoCreate() @ level: " + level.ToString());
            }

            foreach (IDeliveryUnitItem item in unit.itemByLevel[level])
            {
                if (item is deliveryUnitDirectoryConstructor)
                {
                }
                item.scopeInOperation(this, scope);
                if (reportingCoreManager.doVerboseLog) aceLog.log("item[" + item.name + "] triggered by x_scopeAutoCreate() @ level: " + level.ToString());
            }
        }

        /// <summary>
        /// X-scopeOut
        /// </summary>
        /// <param name="oldScope">The old scope.</param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override void x_scopeAutoSave(IMetaContentNested oldScope)
        {
            var level = oldScope.elementLevel;

            foreach (IDeliveryUnitItem item in unit.outputByLevel[level])
            {
                item.scopeOutOperation(this, oldScope);
                //  log("output[" + item.name + "] triggered by x_scopeAutoSave() @ level: " + level.ToString());
            }

            foreach (IDeliveryUnitItem item in unit.itemByLevel[level])
            {
                item.scopeOutOperation(this, oldScope);
                //   log("item[" + item.name + "] triggered by x_scopeAutoSave() @ level: " + level.ToString());
            }
        }

        protected void executeMain()
        {
        }

        protected void executeEnd()
        {
        }

        public override ILogBuilder logStartPhase(string title, string message)
        {

            return aceLog;
            //throw new NotImplementedException();
        }

        public override ILogBuilder logEndPhase()
        {
            return aceLog;
            // throw new NotImplementedException();
        }

        public override void log(string message) => aceLog.log(message);

        public override void save(string destination_path = "")
        {
            //    throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        public deliveryUnit unit { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public docScriptCompiled compiled { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public reportOutputRepository repo { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public deliveryUnitRenderCollection renders { get; protected set; } = new deliveryUnitRenderCollection();

        public string path
        {
            get
            {
                return directoryRoot.Name;
            }
        }

        public object parent
        {
            get
            {
                return null;
            }
        }

        public reportElementLevel elementLevel
        {
            get
            {
                return reportElementLevel.delivery;
            }
        }

        public override string logContent => throw new NotImplementedException();

        public override DirectoryInfo directoryScope
        {
            get { return _directoryScope; }
            set
            {
                //if (_directoryScope == null) _directoryScope = new DirectoryInfo(outputRepositorium.basePath)
                _directoryScope = value;
            }
        }
    }
}