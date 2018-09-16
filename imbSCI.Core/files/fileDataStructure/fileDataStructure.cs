// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileDataStructure.cs" company="imbVeles" >
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
using imbSCI.Core.data;
using imbSCI.Core.extensions.data;
using imbSCI.Core.reporting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.files.fileDataStructure
{
    using imbSCI.Core.files.folders;
    using System.Xml.Serialization;

    public abstract class fileDataStructure
    {
        /// <summary>
        /// Parent folder or it's own folder
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        [XmlIgnore]
        public folderNode folder { get; set; }

        private fileDataStructureDescriptor _descriptor;

        /// <summary>
        /// Meta information about this file data structure
        /// </summary>
        /// <value>
        /// The descriptor.
        /// </value>
        protected fileDataStructureDescriptor descriptor
        {
            get
            {
                if (_descriptor == null || true)
                {
                    IFileDataStructure st = this as IFileDataStructure;
                    if (st != null)
                    {
                        _descriptor = st.GetFileDataStructureDescriptor();
                    }
                }

                return _descriptor;
            }
        }

        /// <summary>
        /// Sets the folder description
        /// </summary>
        /// <param name="generateReadme">If true it will call <see cref="folderNode.generateReadmeFiles(aceAuthorNotation, string)"/> after description set.</param>
        /// <param name="notation">Information on author or application</param>
        public void SetFolderDescription(Boolean generateReadme = false, aceAuthorNotation notation = null)
        {
            if (folder == null)
            {
                return;
            }

            List<String> output = new List<string>();

            output.Add("Data structure:     " + descriptor.name + " [" + descriptor.type.Name + "]");
            output.Add("Description:        " + descriptor.description);
            output.Add("File:               " + descriptor.filename);
            output.Add("----");
            Boolean hasFiles = descriptor.fileDataProperties.Any();
            if (hasFiles) output.Add("In this data structure:");
            foreach (var pair in descriptor.fileDataProperties)
            {
                output.Add("[" + pair.Key.ToString("D3") + "]   " + pair.Value.name + "                    [" + pair.Value.type.Name + "]");
                output.Add(" > " + pair.Value.description);
                output.Add(" > Path: " + pair.Value.filename + "        [" + pair.Value.filenameMode + "]");
            }
            if (hasFiles) output.Add("---");

            output.AddRange(CustomFolderDescriptionLines());

            folder.AdditionalDescriptionLines.AddRange(output, true);

            if (generateReadme)
            {
                folder.generateReadmeFiles(notation);
            }
        }

        /// <summary>
        /// OVERIDE THIS TO PROVIDE ADDITIONAL FILE-FOLDER DESCRIPTION
        /// </summary>
        /// <returns></returns>
        protected virtual List<String> CustomFolderDescriptionLines()
        {
            List<String> output = new List<string>();

            return output;
        }

        /// <summary>
        /// Saves this data structure
        /// </summary>
        /// <param name="logger">The logger.</param>
        public void Save(ILogBuilder logger = null)
        {
            if (folder == null)
            {
                fileDataStructureExtensions.FileDataStructureError("Folder not set - can't save fileDataStructure " + descriptor.name + " [" + descriptor.type.Name + "]",
                    folder, logger, null, this as IFileDataStructure);
            }
            IFileDataStructure st = this as IFileDataStructure;
            if (st != null)
            {
                folderNode fp = folder;
                if (descriptor.mode == fileStructureMode.subdirectory)
                {
                    fp = folder.parent as folderNode;
                }
                st.SaveDataStructure(fp);
            }
        }

        /// <summary>
        /// Called when object is loaded
        /// </summary>
        public abstract void OnLoaded();

        /// <summary>
        /// Called when before saving the data structure
        /// </summary>
        public abstract void OnBeforeSave();
    }
}