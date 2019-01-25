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
using System.Text.RegularExpressions;
using System.Xml;

namespace imbSCI.Core.files.folders
{
public class SerializedXMLFile
    {

        public SerializedXMLFile()
        {

        }

        public static SerializedXMLFile Load(String path)
        {


            if (!File.Exists(path))
            {
                return null;
            }

            SerializedXMLFile output = new SerializedXMLFile();

            output.fileInfo = new FileInfo(path);

            output.source = File.ReadAllText(path);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(output.source);

            String tname = doc.Name;

            if (tname.StartsWith("ArrayOf"))
            {
                output.IsArrayOf = true;
                tname = tname.Substring("ArrayOf".Length);
            }

            output.typename = tname;
            return output;
        }

        public FileInfo fileInfo { get; set; }

        public String source { get; set; } = "";

        public Boolean IsArrayOf { get; set; } = false;

        public String typename { get; set; } = "";

    }
}