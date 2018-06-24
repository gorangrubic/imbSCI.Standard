// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnit.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.extensions;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.meta.delivery.items;
    using imbSCI.Reporting.meta.delivery.services;
    using imbSCI.Reporting.meta.delivery.units;
    using imbSCI.Reporting.script;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    /// <summary>
    ///
    /// </summary>
    public abstract class deliveryUnit
    {
        /// <summary>
        /// Setup all <see cref="deliveryUnitItem"/>s and other structural things
        /// </summary>
        public abstract void setup();

        /// <summary>
        /// Describes the unit via specified loger
        /// </summary>
        /// <param name="loger">The loger.</param>
        public void describe(ILogBuilder loger)
        {
            //            loger.log("deliveryUnit describe() call started");

            loger.AppendHeading("Delivery unit (" + GetType().Name + ")", 2);

            loger.AppendLine("Logical name: " + name);

            loger.open("items", "Delivery items", "List of all deliveryUnit items contained here");
            foreach (IDeliveryUnitItem item in items)
            {
                //loger.AppendHeading(this.name + " (" + this.GetType().Name + ")", 3);

                loger.AppendLine(" > " + item.name + ":" + item.itemType.ToString());
                loger.AppendLine(" > > Location: " + item.location.ToString());
                loger.AppendLine(" > > Description: " + item.description);
            }
            loger.close();

            loger.open("items", "Items by level", "Showing items triggered by scope level");
            reportElementLevel lev = reportElementLevel.none;

            foreach (KeyValuePair<reportElementLevel, List<deliveryUnitItem>> pair in itemByLevel)
            {
                lev = pair.Key;
                foreach (deliveryUnitItem it in pair.Value)
                {
                    loger.AppendLine(lev.ToString() + " --> " + it.name + " (" + it.GetType().Name + ")");
                }
            }
            loger.close();

            loger.open("items", "Output by level", "Showing items designated as output items and triggered by scope level");
            foreach (KeyValuePair<reportElementLevel, List<deliveryUnitItem>> pair in outputByLevel)
            {
                lev = pair.Key;
                foreach (deliveryUnitItem it in pair.Value)
                {
                    loger.AppendLine(lev.ToString() + " --> " + it.name + " (" + it.GetType().Name + ")");
                }
            }
            loger.close();

            //  loger.log("deliveryUnit describe() call finished");
        }

        /// <summary>
        /// Finds the deliveryInstance unit item with template.
        /// </summary>
        /// <param name="sourceFilepathNeedle">The source filepath needle.</param>
        /// <returns></returns>
        public deliveryUnitItemContentTemplated findDeliveryUnitItemWithTemplate(string sourceFilepathNeedle)
        {
            foreach (IDeliveryUnitItem ui in items)
            {
                if (ui is deliveryUnitItemContentTemplated)
                {
                    deliveryUnitItemContentTemplated tui = (deliveryUnitItemContentTemplated)ui;
                    if (tui.name.Contains(sourceFilepathNeedle))
                    {
                        return tui;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();
            //this.buildPropertyCollection(false, false, "target", data);
            //data.Add()
            data[templateFieldDeliveryUnit.del_unitname] = name;
            data[templateFieldDeliveryUnit.del_themepath] = deliveryUnitBuilder.themepath;
            data[templateFieldDeliveryUnit.del_includepath] = deliveryUnitBuilder.includepath;

            //data[target.target_name] = name;
            // data[target.target_description] = description;
            // data[target.target_id] = id;
            // data[target.target_url] = url;
            return data;
        }

        public IDeliveryUnitItem Add(IDeliveryUnitItem item, FileInfo sourceFile = null)
        {
            if (item is IDeliverySupportFile)
            {
                if (sourceFile != null)
                {
                    IDeliverySupportFile item_IDeliverySupportFile = (IDeliverySupportFile)item;
                    item_IDeliverySupportFile.sourceFileInfo = sourceFile;
                }
            }

            items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <returns></returns>
        public T AddItem<T>(T item, FileInfo sourceFile = null) where T : IDeliveryUnitItem
        {
            Add(item, sourceFile);

            return item;
        }

        /// <summary>
        /// Adds a theme support file.
        /// </summary>
        /// <param name="sourcePath">The source path - relative to reportTheme folder</param>
        /// <param name="outputPath">The output path - relative to deliveryInstance output folder</param>
        /// <returns></returns>
        public deliveryUnitItemSupportFile AddThemeSupportFile(string sourcePath, string outputPath = "include\\", appendLinkType linkType = appendLinkType.unknown)
        {
            string tsf_path = deliveryUnitBuilder.themepath.add(sourcePath, "\\");

            FileInfo fi = new FileInfo(tsf_path);

            deliveryUnitItemSupportFile spf = new deliveryUnitItemSupportFile(fi.FullName, outputPath);
            if (linkType != appendLinkType.unknown) spf.linkType = linkType;

            Add(spf, fi);
            return spf;
        }

        /// <summary>
        /// Adds all files found in the <c>themeFolder</c> that match <c>search</c> and they were not already included
        /// </summary>
        /// <param name="themeFolder">The theme folder.</param>
        /// <param name="targetLinkType">Type of the target link.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="includeSub">if set to <c>true</c> [include sub].</param>
        /// <param name="keepFolderStructure">if set to <c>true</c> [keep folder structure].</param>
        /// <returns></returns>
        public List<deliveryUnitItemSupportFile> AddThemeSupportFiles(string themeFolder, string search, string outputPath = "include\\", bool doForcePostLinks = false)
        {
            string tsf_path = deliveryUnitBuilder.themepath.add(themeFolder, "\\");
            DirectoryInfo di = Directory.CreateDirectory(tsf_path);

            List<deliveryUnitItemSupportFile> output = new List<deliveryUnitItemSupportFile>();

            FileInfo[] files = di.GetFiles(search);

            //List<String> filePaths = filePathOperations.findFiles(targetLinkType.getFileExtension(), tsf_path, includeSub);

            foreach (FileInfo fp in files)
            {
                string outp = outputPath;
                string inp = fp.FullName;

                //if (keepFolderStructure)
                //{
                //    String sub = inp.removeStartsWith(di.FullName);
                //    sub = Path.GetDirectoryName(sub);
                //    outp = outp.add(sub, "\\");
                //}

                if (!items.containsItemFromSourcefile(fp))
                {
                    deliveryUnitItemSupportFile spf = new deliveryUnitItemSupportFile(inp, outp);
                    spf.sourceFileInfo = fp;
                    if (doForcePostLinks)
                    {
                        if (spf.linkType == appendLinkType.scriptLink)
                        {
                            spf.linkType = appendLinkType.scriptPostLink;
                        }
                    }

                    output.Add(spf);
                    Add(spf, fp);
                }
            }

            //Add(spf);
            return output;
        }

        public static string folder_includeReport = "reportInclude";

        /// <summary>
        /// Adds all files matching the <c>filesearch</c> pattern, inside <c>path</c>
        /// </summary>
        /// <param name="path">The path inside <c>reportInclude</c> folder</param>
        /// <param name="transformator">The transformator - output item that transformes included content</param>
        /// <param name="filesearch">File search pattern to get the files</param>
        public void AddReportIncludeFiles(string path, deliveryUnitItemContentTemplated transformator, string filesearch = "*.md", bool copyOriginals = true)
        {
            DirectoryInfo reportTheme = Directory.CreateDirectory(folder_includeReport);

            path = path.ensureStartsWith(folder_includeReport + "\\");

            DirectoryInfo reportThemeDoc = Directory.CreateDirectory(path);

            FileInfo[] docFiles = reportThemeDoc.GetFiles(filesearch);

            foreach (FileInfo fi in docFiles)
            {
                string filepath_short = fi.FullName.removeStartsWith(reportTheme.FullName);
                string output_filename = fi.Name.removeEndsWith(fi.Extension);
                string output_dirpath = Path.GetDirectoryName(filepath_short);

                string originalFileName = output_filename.add(fi.Extension, ".");

                string title = output_filename.imbTitleCamelOperation(true);
                string extension = transformator.format.getDefaultExtension();
                output_filename = output_filename.add(extension, ".");
                string __outputpath = output_dirpath.add(output_filename, "\\");
                __outputpath = __outputpath.removeStartsWith("\\");

                AddItem(new deliveryUnitItemFileOutput(filepath_short, __outputpath, title, "", transformator)).includeSourceFolder = folder_includeReport;

                if (copyOriginals)
                {
                    string __origFilePath = output_dirpath.add(originalFileName, "\\").removeStartsWith("\\");
                    AddItem(new deliveryUnitItemFileOutput(filepath_short, __origFilePath, title, "")).includeSourceFolder = folder_includeReport;
                }
            }
        }

        /// <summary>
        /// Executes the prepare procedure
        /// </summary>
        public void executePrepare()
        {
            //includeList = new metaLinkCollection();
            //renders = new deliveryUnitRenderCollection();
            itemByLevel = new deliveryUnitItemByLevels();
            outputByLevel = new deliveryUnitItemByLevels();

            foreach (deliveryUnitItem item in items)
            {
                if (item.levels.Any())
                {
                    foreach (var lev in item.levels)
                    {
                        itemByLevel[lev].Add(item);
                    }
                }
                else
                {
                    switch (item.location)
                    {
                        case deliveryUnitItemLocationBase.localResource:
                        case deliveryUnitItemLocationBase.externalWebResource:
                            itemByLevel[reportElementLevel.page].Add(item);
                            break;

                        case deliveryUnitItemLocationBase.globalDeliveryResource:
                            itemByLevel[reportElementLevel.delivery].Add(item);
                            break;

                        case deliveryUnitItemLocationBase.globalDocumentResource:
                            itemByLevel[reportElementLevel.document].Add(item);
                            break;

                        case deliveryUnitItemLocationBase.globalDocumentSetResource:
                            itemByLevel[reportElementLevel.documentSet].Add(item);
                            break;

                        case deliveryUnitItemLocationBase.globalDeliveryContent:

                            if (item is deliveryUnitItemFileOutput)
                            {
                                deliveryUnitItemFileOutput item_deliveryUnitItemFileOutput = (deliveryUnitItemFileOutput)item;
                                if (item_deliveryUnitItemFileOutput.isDataFieldMode)
                                {
                                    itemByLevel[reportElementLevel.delivery].Add(item);
                                }
                            }

                            break;

                        case deliveryUnitItemLocationBase.unknown:

                            break;
                    }
                }

                if (item is deliveryUnitItemFileOutput)
                {
                    deliveryUnitItemFileOutput item_deliveryUnitItemFileOutput = (deliveryUnitItemFileOutput)item;
                    if (item_deliveryUnitItemFileOutput.isDataFieldMode)
                    {
                        // itemByLevel[reportElementLevel.deliveryInstance].Add(item);
                    }
                    else
                    {
                        outputContent.Add(item_deliveryUnitItemFileOutput);
                    }
                }

                if (item is deliveryUnitItemSimpleRenderOutput)
                {
                    deliveryUnitItemSimpleRenderOutput item_deliveryUnitItemRenderOutput = (deliveryUnitItemSimpleRenderOutput)item;

                    outputByLevel.Add(item_deliveryUnitItemRenderOutput.levels, item);
                }

                foreach (IDeliveryUnitItem it in items)
                {
                    if (it is IDeliverySupportFile)
                    {
                        IDeliverySupportFile it_IDeliverySupportFile = (IDeliverySupportFile)it;
                        if (!includeItems[it_IDeliverySupportFile.linkType].Contains(it_IDeliverySupportFile))
                        {
                            includeItems[it_IDeliverySupportFile.linkType].Add(it_IDeliverySupportFile);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public templateBlocksForHtml blockBuilder { get; set; } = new templateBlocksForHtml();

        protected deliveryUnit(string __name)
        {
            name = __name;
        }

        /// <summary>
        /// Updates the templates.
        /// </summary>
        public void updateTemplates(deliveryInstance loger)
        {
            PropertyCollection pc = blockBuilder.BuildIncludeTemplate(loger);

            foreach (deliveryUnitItem item in items)
            {
                //if (item is deliveryUnitItemContentTemplated)
                //{
                //    deliveryUnitItemContentTemplated templated = item as deliveryUnitItemContentTemplated;
                //    if (!templated.template.isNullOrEmpty())
                //    {
                //        Int32 ln = templated.template.Length;
                //        templated.template = templated.template.applyToContent(false, pc);
                //        ln = templated.template.Length - ln;

                //       // loger.log(templated.name + " --> template updated [" + pc.Count + "] - bytes change: " + ln);
                //    }

                if (item is IDeliveryUnitItemWithTemplate)
                {
                    IDeliveryUnitItemWithTemplate itemWithTemplate = item as IDeliveryUnitItemWithTemplate;

                    string appliedTemplate = itemWithTemplate.template;
                    if (!imbSciStringExtensions.isNullOrEmpty(appliedTemplate))
                    {
                        appliedTemplate = appliedTemplate.applyToContent(false, pc);
                        itemWithTemplate.template = appliedTemplate;
                    }
                }
            }
        }

        /// <summary> </summary>
        public string name { get; protected set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string outputpath { get; protected set; } = "reportOutput\\";

        /// <summary>
        /// Determinates how script will be executed
        /// </summary>
        public docScriptFlags scriptFlags { get; set; }

        /// <summary>
        ///
        /// </summary>
        public styleTheme theme { get; set; }

        /// <summary>
        ///
        /// </summary>
        public deliveryUnitItemByLevels itemByLevel { get; set; } = new deliveryUnitItemByLevels();

        /// <summary>
        ///
        /// </summary>
        public deliveryUnitItemByLevels outputByLevel { get; set; } = new deliveryUnitItemByLevels();

        /// <summary>
        ///
        /// </summary>
        public deliveryUnitItemCollection outputContent { get; set; } = new deliveryUnitItemCollection();

        /// <summary>
        /// Items that should be included in header
        /// </summary>
        public deliveryUnitItemByLinkType includeItems { get; protected set; } = new deliveryUnitItemByLinkType();

        /// <summary>
        ///
        /// </summary>
        internal deliveryUnitItemCollection items { get; set; } = new deliveryUnitItemCollection();
    }
}