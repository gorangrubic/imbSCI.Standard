// --------------------------------------------------------------------------------------------------------------------
// <copyright file="folderNodeFileDescriptor.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.text;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace imbSCI.Core.files.folders
{
    /// <summary>
    /// XML resources scan result
    /// </summary>
    public class SerializedXMLFileDictionary
    {
        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        public folderNode folder { get; protected set; }

        /// <summary>
        /// Gets or sets the type of the files by.
        /// </summary>
        /// <value>
        /// The type of the files by.
        /// </value>
        public Dictionary<String, List<SerializedXMLFile>> filesByType { get; protected set; } = new Dictionary<string, List<SerializedXMLFile>>();

        /// <summary>
        /// Registers the specified x file.
        /// </summary>
        /// <param name="xFile">The x file.</param>
        public void Register(SerializedXMLFile xFile)
        {
            if (xFile == null) return;
            if (xFile.typename.isNullOrEmpty()) return;

            if (!filesByType.ContainsKey(xFile.typename))
            {
                filesByType.Add(xFile.typename, new List<SerializedXMLFile>());
            }
            filesByType[xFile.typename].Add(xFile);
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public List<T> GetResources<T>(String filename_needle = "") where T : new()
        {
            Type t = typeof(T);

            if (!filesByType.ContainsKey(t.Name)) return new List<T>();

            List<T> output = new List<T>();

            foreach (SerializedXMLFile l in filesByType[t.Name])
            {
                Boolean ok = true;
                if (!filename_needle.isNullOrEmpty())
                {
                    if (l.fileInfo.Name.Contains(filename_needle))
                    {
                        ok = true;
                    }
                }
                if (ok) output.Add(objectSerialization.ObjectFromXML<T>(l.source));
            }

            return output;

        }


        /// <summary>
        /// Scans the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public static SerializedXMLFileDictionary ScanFolder(folderNode folder, String customSearch = "*.xml")
        {
            SerializedXMLFileDictionary output = new SerializedXMLFileDictionary();
            output.folder = folder;

            List<String> files = folder.findFiles(customSearch, SearchOption.AllDirectories);
            foreach (String f in files)
            {
                SerializedXMLFile xFile = SerializedXMLFile.Load(f);

                output.Register(xFile);
            }

            return output;
        }
    }
}