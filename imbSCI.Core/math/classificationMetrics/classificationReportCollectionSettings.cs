// --------------------------------------------------------------------------------------------------------------------
// <copyright file="classificationReport.cs" company="imbVeles" >
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
using imbSCI.Core.attributes;
using imbSCI.Core.enums;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace imbSCI.Core.math.classificationMetrics
{
public class classificationReportCollectionSettings
    {

        public classificationReportCollectionSettings()
        {

        }


        public classificationReportStyleDefinition style { get; set; } = new classificationReportStyleDefinition();

        public String FILENAME_COLLECTION_INFO = "SubGroupDescription.txt";

        public String FILENAME_CLASSIFICATION_RESULTS = "results.xml";

        public String FILENAME_DATASET_FOLDSTATS = "*_report.xml";

        public String FILENAME_SETUP_XML = "*_setup.xml";

        public String description { get; set; } = "";

        public String rootPath { get; set; } = "";

        [XmlIgnore]
        public Regex SELECT_ENDNUMBER { get; set; } = new Regex(@"([\d]*)$");

        [XmlIgnore]
        public Regex SELECT_REPORT_NAME_PARTS { get; set; } = new Regex(@"([A-Za-z_\-]*)([\d]*)$");

    }
}