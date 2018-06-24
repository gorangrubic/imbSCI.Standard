// --------------------------------------------------------------------------------------------------------------------
// <copyright file="exeAppendTemplateBundleItem.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.script.exeAppenders
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.reporting.extensions;
    using imbSCI.Core.reporting.render;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Definition of content source
    /// </summary>
    public class exeAppendTemplateBundleItem
    {
        public enum sourceType
        {
            zip,
            directory,
            OpenXMLDocument,
            OpenDocumentFormat,
            singlefile,
        }

        /// <summary> </summary>
        public sourceType type { get; protected set; } = sourceType.directory;

        public enum itemOperaton
        {
            deployFromFolder,
            deployFromArchive,
            deployFromDocument,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="exeAppendTemplateBundleItem"/> class.
        /// </summary>
        /// <param name="__options">The options.</param>
        /// <param name="__filePath">The source file/directory path</param>
        /// <param name="__data">Static data set used for template and filepath transformations.</param>
        public exeAppendTemplateBundleItem(exeAppendTemplateOptions __options, string __filePath, PropertyCollectionExtended __data)
        {
            options = __options;
            filePath = __filePath;

            if (filePath.Contains("{{{"))
            {
                filePath = filePath.applyToContent(__data);
            }

            if (Path.GetFileName(filePath).isNullOrEmpty())
            {
                // it is directory
                target = Directory.CreateDirectory(filePath);
                operation = itemOperaton.deployFromFolder;
            }
            else
            {
                targetExtension = Path.GetExtension(filePath).Trim('.');
                switch (targetExtension)
                {
                    case "zip":
                        operation = itemOperaton.deployFromArchive;
                        break;

                    case "odt":
                        operation = itemOperaton.deployFromDocument;
                        break;
                }
                target = Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                FileInfo fi = filePath.getWritableFile(getWritableFileMode.overwrite);

                targetFilename = fi.FullName.removeStartsWith(target.FullName);
            }

            dedicatedData = __data;
        }

        /// <summary>
        /// Gets the source files into temporary directory, unpack it and returns list of all files aquired;
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="render">The render.</param>
        /// <param name="deployDir">The deploy dir.</param>
        /// <param name="deployFilename">The deploy filename.</param>
        /// <returns></returns>
        public List<string> getFromSource(IRenderExecutionContext context, ITextRender render, DirectoryInfo deployDir, string deployFilename = "")
        {
            string tmpPath = Path.GetTempPath();
            //ZipFile zip = null;
            string sPath = target.FullName.add(targetFilename, "\\");
            //String tPath = deployDir.FullName.add(deployFilename, "\\");

            switch (operation)
            {
                case itemOperaton.deployFromArchive:
                    throw new NotImplementedException();
                    //zip = ZipFile.Read(sPath.add("zip", "."));
                    //zip.ExtractAll(tmpPath);

                    break;

                case itemOperaton.deployFromDocument:
                    string ext = Path.GetExtension(sPath);
                    string tmp2 = tmpPath.add(sPath.removeEndsWith(ext).Trim('.'), "\\");

                    File.Copy(sPath, tmp2.add("zip", "."), true);
                    sPath = tmp2;
                    goto case itemOperaton.deployFromArchive;

                    break;

                case itemOperaton.deployFromFolder:
                    sPath.DirectoryCopy(tmpPath, (searchPatternOption == SearchOption.AllDirectories));
                    break;
            }
            List<string> output = new List<string>();

            output.AddRange(Directory.GetFileSystemEntries(tmpPath));
            output.Add(tmpPath);
            return output;
        }

        /// <summary>
        /// Finalization
        /// </summary>
        /// <param name="sourceFiles">The source files.</param>
        /// <param name="context">The context.</param>
        /// <param name="targetOp">The target op.</param>
        /// <param name="deployDir">The deploy dir.</param>
        /// <param name="deployFilename">The deploy filename.</param>
        public void setToTarget(List<string> sourceFiles, IRenderExecutionContext context, exeAppendTemplatedBundle.targetOperaton targetOp, DirectoryInfo deployDir, string deployFilename = "")
        {
            // ZipFile zip = null;
            string tPath = deployDir.FullName.add(deployFilename, "\\");

            string tmpPath = sourceFiles.Pop<string>();

            List<string> extAsTemplates = options.getExtensionList();

            foreach (var str in sourceFiles)
            {
                string ext = Path.GetExtension(str);
                if (extAsTemplates.Contains(str))
                {
                    string content = File.ReadAllText(str);
                    if (content.Contains("{{{")) content = content.applyToContent(context.data);
                    if (content.Contains("{{{")) content = content.applyToContent(dedicatedData);
                    /*
                    foreach (String k in dedicatedData.Keys) {
                        if (content.Contains("{{{")) content = content.applyToContent(false,dedicatedData[k]);
                    }*/
                    File.WriteAllText(str, content);
                }
            }

            switch (targetOp)
            {
                case exeAppendTemplatedBundle.targetOperaton.deployInArchive:

                    throw new NotImplementedException();
                    //zip = new ZipFile(tPath);
                    //zip.AddFiles(sourceFiles, true, "");
                    //zip.Save(tPath);

                    break;

                case exeAppendTemplatedBundle.targetOperaton.deployInDocument:
                    throw new NotImplementedException();
                    //zip = new ZipFile(tPath);
                    //zip.AddFiles(sourceFiles, true, "");
                    //zip.Save(tPath);

                    string ext = Path.GetExtension(tPath);
                    string tmp2 = tPath.add(tPath.removeEndsWith(ext).Trim('.'), "\\").add("odt", ".");
                    tPath = tmp2;
                    goto case exeAppendTemplatedBundle.targetOperaton.deployInArchive;

                    break;

                case exeAppendTemplatedBundle.targetOperaton.deployInFolder:
                    tmpPath.DirectoryCopy(deployDir.FullName, true);

                    break;
            }
        }

        /// <summary> </summary>
        protected string targetFilename { get; set; } = "";

        /// <summary> </summary>
        public string targetExtension { get; protected set; } = "";

        /// <summary>Target for content deployment</summary>
        protected DirectoryInfo target { get; set; }

        /// <summary> </summary>
        public itemOperaton operation { get; protected set; } = itemOperaton.deployFromFolder;

        /// <summary> </summary>
        public SearchOption searchPatternOption { get; set; } = SearchOption.AllDirectories;

        /// <summary> </summary>
        public exeAppendTemplateOptions options { get; protected set; } = exeAppendTemplateOptions.htmlTemplate | exeAppendTemplateOptions.jsonTemplate | exeAppendTemplateOptions.renderButton;

        /// <summary>File path to folder, archive file or some other type of package</summary>
        public string filePath { get; protected set; }

        /// <summary>Data dedicated to this template</summary>
        public PropertyCollectionExtended dedicatedData { get; protected set; } = new PropertyCollectionExtended();
    }
}