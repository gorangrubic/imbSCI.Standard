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
using System.IO;
using System.Text.RegularExpressions;

namespace imbSCI.Core.files.folders
{
    public static class folderNodeFileDescriptorTools
    {
        public static String FileDescriptionFormat { get; set; } = "[{0,-50}] {1}";

        public static Regex FileDescriptionExtractionRegex = new Regex("[\\d]* : \\[([\\w\\._-]*)\\s*\\] ([\\w\\s\\(\\)\\d\\$\\%\\#\\@\\&\\\"\\:\\,\\.-_\\[\\]!?~`\\*]*)");

        /// <summary>
        /// Gets the file description.
        /// </summary>
        /// <param name="descriptionLine">The description line.</param>
        /// <returns></returns>
        public static folderNodeFileDescriptor GetFileDescription(String descriptionLine)
        {
            if (FileDescriptionExtractionRegex.IsMatch(descriptionLine))
            {
                folderNodeFileDescriptor output = new folderNodeFileDescriptor();
                var mch = FileDescriptionExtractionRegex.Match(descriptionLine);
                if (mch.Groups.Count > 0) output.filename = mch.Groups[1].Value;
                if (mch.Groups.Count > 1) output.description = mch.Groups[2].Value;
                return output;
            }
            return null;
        }

        public static folderNodeFileDescriptor GetFileDescription(this folderNode folder, String filename, String fileDescription)
        {
            if (fileDescription.isNullOrEmpty())
            {
                String fileClean = Path.GetFileNameWithoutExtension(filename);
                String fileTitle = fileClean.imbTitleCamelOperation(true);

                String ext = Path.GetExtension(filename).Trim('.').ToLower();

                switch (ext)
                {
                    case "json":
                        fileDescription = "JSON Serialized Data Object";
                        break;

                    case "xml":
                        fileDescription = "XML Serialized Data Object";
                        if (filename.ContainsAny(new String[] { "setup", "Setup", "config", "Config", "settings", "Settings" }))
                        {
                            fileTitle = fileClean.imbTitleCamelOperation(true);
                            fileDescription = "Serialized configuration [" + fileTitle + "] object";
                        }

                        break;

                    case "txt":
                        fileDescription = "Plain text file";
                        if (filename.StartsWith("ci_"))
                        {
                            fileClean = fileClean.removeStartsWith("ci_");
                            fileTitle = fileClean.imbTitleCamelOperation(true);
                            fileDescription = "Column / Fields meta information for data table [" + fileTitle + "] export";
                        }
                        if (fileClean == "note")
                        {
                            fileDescription = "Relevant notes on [" + folder.caption + "] in markdown/text format";
                        }
                        if (fileClean.Contains("error"))
                        {
                            fileDescription = "Error record(s)";
                        }
                        if (filename == "directory_readme.txt")
                        {
                            fileDescription = "Description of directory content (this file)";
                        }
                        break;

                    case "csv":
                        fileDescription = "Comma Separated Value data dump";
                        if (filename.StartsWith("dc_"))
                        {
                            fileClean = fileClean.removeStartsWith("dc_");
                            fileTitle = fileClean.imbTitleCamelOperation(true);
                            fileDescription = "Clean data CSV version of data table [" + fileTitle + "] export";
                        }
                        break;

                    case "xls":
                    case "xlsx":
                        fileDescription = "Excel spreadsheet";
                        if (filename.StartsWith("dt_"))
                        {
                            fileClean = fileClean.removeStartsWith("dt_");
                            fileTitle = fileClean.imbTitleCamelOperation(true);
                            fileDescription = "Excel spreadsheet report on [" + fileTitle + "] data table";
                        }
                        break;

                    case "md":
                        fileDescription = "Markdown document";
                        break;

                    case "bin":
                        fileDescription = "Binary Serialized Data Object";
                        break;

                    case "dgml":
                        fileDescription = "Serialized graph in Directed-Graph Markup Language format";
                        break;

                    case "html":
                        fileDescription = "HTML Document";
                        break;

                    case "log":
                        fileDescription = "Log output plain text file";
                        break;

                    default:
                        fileDescription = ext.ToUpper() + " file";
                        break;
                }
            }
            fileDescription = String.Format(FileDescriptionFormat, filename, fileDescription);

            var desc = new folderNodeFileDescriptor();
            desc.filename = filename;
            desc.description = fileDescription;
            return desc;
        }
    }

    public class folderNodeFileDescriptor
    {
        public folderNodeFileDescriptor()
        {
        }

        public String filename { get; set; }

        public String description { get; set; }
    }
}