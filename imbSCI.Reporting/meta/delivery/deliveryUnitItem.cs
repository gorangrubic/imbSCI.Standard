// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitItem.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.files;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.extensions;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.meta.delivery.units;
    using imbSCI.Reporting.meta.document;
    using imbSCI.Reporting.meta.documentSet;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    /// <summary>
    /// One element of <see cref="deliveryUnit"/>
    /// </summary>
    /// <seealso cref="metaDocumentSet"/>
    /// <seealso cref="metaDocument"/>
    public abstract class deliveryUnitItem : IObjectWithNameAndDescription  // : IObjectWithNameAndDescription
    {
        public void setRelPath(IRenderExecutionContext context)
        {
            string relPath = context.directoryScope.getRelativePathToParent(context.directoryRoot);
            relPath = relPath.getWebPathBackslashFormat();
            if (relPath == "/") relPath = "";

            context.data[templateFieldBasic.root_relpath] = relPath;
        }

        public string getFolderPathForLinkRegistry(IRenderExecutionContext context)
        {
            string folderPath = "";
            //IMetaContent mc = context.scope.parent as IMetaContent;

            //if (mc != null)
            //{
            //    folderPath = mc.path;
            //}
            //else
            //{
            //}

            folderPath = context.scope.path;

            context.data.getProperString(folderPath, templateFieldBasic.path_folder, templateFieldBasic.document_path, templateFieldBasic.documentset_path);

            return folderPath;
        }

        /// <summary>
        /// Used just for double include check - normally its null
        /// </summary>
        public FileInfo sourceFileInfo { get; set; }

        /// <summary>
        ///
        /// </summary>
        public FileInfo output_fileinfo { get; protected set; }

        private string _includeSourceFolder;

        /// <summary>
        ///
        /// </summary>
        public string includeSourceFolder
        {
            get
            {
                if (imbSciStringExtensions.isNullOrEmpty(_includeSourceFolder)) _includeSourceFolder = deliveryUnitBuilder.themepath;

                return _includeSourceFolder;
            }
            set { _includeSourceFolder = value; }
        }

        public virtual void prepareOperation(IRenderExecutionContext context)
        {
            string src = sourcepath.toPath(includeSourceFolder, context.data);
            DirectoryInfo dir = context.directoryScope;
            if (location == deliveryUnitItemLocationBase.globalDeliveryResource)
            {
                dir = context.directoryRoot;
            }

            if (flags.HasFlag(deliveryUnitItemFlags.useTemplate))
            {
                template = openBase.openFileToString(src, true, false);
            }

            if (flags.HasFlag(deliveryUnitItemFlags.useCopy))
            {
                string trg = outputpath.toPath(context.directoryRoot.FullName, context.data);

                output_fileinfo = trg.getWritableFile(getWritableFileMode.overwrite);
                if (sourceFileInfo == null)
                {
                }
                else
                {
                    if (File.Exists(sourceFileInfo.FullName))
                    {
                        File.Copy(sourceFileInfo.FullName, output_fileinfo.FullName, true);

                        context.regFileOutput(output_fileinfo.FullName, output_fileinfo.Extension, description);
                    }
                    else
                    {
                        context.log("File [" + sourceFileInfo.FullName + "] not found.");
                    }

                    //context.saveFileOutput(output, output_fileinfo.FullName, "css", description);
                }
            }
        }

        public virtual void reportFinishedOperation(IRenderExecutionContext context)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public string template { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string filenameTemplate { get; set; } = "{{{name}}}.{{{ext}}}";

        /// <summary>
        /// Gets the out file path using template
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="content">The content.</param>
        /// <param name="pc">The pc.</param>
        /// <param name="format">The format.</param>
        /// <param name="dir">The dir.</param>
        /// <returns></returns>
        public string getOutFilePath(IRenderExecutionContext context, IMetaContentNested content, PropertyCollection pc, reportOutputFormatName format = reportOutputFormatName.none, DirectoryInfo dir = null)
        {
            if (pc == null) pc = new PropertyCollection();

            if (content == null) content = context.scope as IMetaContentNested;
            if (dir == null) dir = context.directoryScope;

            if (format == reportOutputFormatName.none)
            {
                if (!pc.ContainsKey("ext"))
                {
                    pc.Add("ext", "");
                }
                else
                {
                    pc["ext"] = "txt";
                }
            }
            else
            {
                if (!pc.ContainsKey("ext"))
                {
                    pc.Add("ext", format.getFilenameExtension());
                }
                else
                {
                    pc["ext"] = format.getFilenameExtension();
                }
            }
            if (content != null)
            {
                if (!pc.ContainsKey("name"))
                {
                    pc.Add("name", content.name.getFilename());
                }
                else
                {
                    pc["name"] = content.name.getFilename();
                }
                //pc.Add("name", content.name);
            }

            string path = filenameTemplate.applyToContent(false, pc);

            if (outputpath != null)
            {
                if (!outputpath.directoryPath.isNullOrEmpty())
                {
                    path = outputpath.directoryPath.add(path, "\\");
                }
            }

            path = dir.FullName.add(path, "\\");

            return path;
        }

        /// <summary>
        ///
        /// </summary>
        protected string runstampRecord { get; set; } = "";

        /// <summary>
        /// Determines whether the specified runstamp is executed.
        /// </summary>
        /// <param name="runstamp">The runstamp.</param>
        /// <param name="writeAsExecuted">if set to <c>true</c> [write as executed].</param>
        /// <returns>
        ///   <c>true</c> if the specified runstamp is executed; otherwise, <c>false</c>.
        /// </returns>
        public bool isExecuted(string runstamp, bool writeAsExecuted = false)
        {
            if (imbSciStringExtensions.isNullOrEmpty(runstampRecord)) runstampRecord = runstamp;
            if (runstampRecord == runstamp)
            {
                return true;
            }
            else
            {
                if (writeAsExecuted) runstampRecord = runstamp;
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public List<reportElementLevel> levels { get; protected set; } = new List<reportElementLevel>();

        /// <summary>
        ///
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string description { get; set; } = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="deliveryUnitItem"/> class.
        /// </summary>
        /// <param name="__itemType">Type of the item.</param>
        protected deliveryUnitItem(deliveryUnitItemType __itemType)
        {
            itemType = __itemType;
            name = GetType().Name + "_" + imbStringGenerators.getRandomString(8);
            description = __itemType.ToString();
        }

        protected void deliveryUnitItemSetup(deliveryUnitItemLocationBase __location, deliveryUnitItemFlags __flags)
        {
            location = __location;
            flags = __flags;
        }

        protected void setupForTemplatedItem(string __sourcepath)
        {
            sourcepath = new filepath(__sourcepath);
        }

        protected void setupForGeneratedItem(string __outputFilename)
        {
            outputpath = new filepath(__outputFilename);
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'directoryRoot' that could not be resolved
        /// <summary>
        /// The output file path - relative to deliveryInstance output path: <see cref="imbSCI.Reporting.reporting.render.IRenderExecutionContext.directoryRoot"/>, may include subfolders, filename template and extension
        /// </summary>
        public filepath outputpath { get; protected set; } = new filepath();
#pragma warning restore CS1574 // XML comment has cref attribute 'directoryRoot' that could not be resolved

        /// <summary>
        /// The input file/folder path - relative to <see cref="Directory.GetCurrentDirectory"/>
        /// </summary>
        public filepath sourcepath { get; protected set; } = new filepath();

        /// <summary>
        ///
        /// </summary>
        public deliveryUnitItemType itemType { get; set; } = deliveryUnitItemType.none;

        /// <summary>
        ///
        /// </summary>
        public deliveryUnitItemLocationBase location { get; set; } = deliveryUnitItemLocationBase.localResource;

        /// <summary>
        ///
        /// </summary>
        public deliveryUnitItemFlags flags { get; set; }
    }
}