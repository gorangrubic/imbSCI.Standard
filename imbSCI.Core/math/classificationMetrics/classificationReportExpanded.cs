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
    /// <summary>
    /// Expanded version of the report
    /// </summary>
    /// <seealso cref="imbSCI.Core.math.classificationMetrics.classificationReport" />
    public class classificationReportExpanded : classificationReport
    {
        public const String LAYER_MAIN = "main";

        public String layer { get; set; } = "main";


        public void DeployInformation(classificationReportCollectionSettings setup, ILogBuilder log)
        {


            
        }


        /// <summary>
        /// Gets or sets the filepath on which the report was found
        /// </summary>
        /// <value>
        /// The filepath.
        /// </value>
        public String filepath { get; set; } = "";
        /// <summary>
        /// Gets or sets the file creation.
        /// </summary>
        /// <value>
        /// The filecreation.
        /// </value>
        public DateTime filecreation { get; set; }

        [XmlIgnore]
        public folderNode folder { get; set; }


        /// <summary>
        /// Loads the simple report and builds the expanded version
        /// </summary>
        /// <param name="reportFile">The report file.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static classificationReportExpanded LoadSimpleReport(String reportFile, ILogBuilder logger)
        {
            FileInfo fi = new FileInfo(reportFile);

            classificationReport rep = classificationReport.Load(reportFile, logger);
            classificationReportExpanded output = new classificationReportExpanded(rep);
            output.filecreation = fi.CreationTime;
            output.filepath = fi.FullName;
            return output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="classificationReportExpanded"/> class.
        /// </summary>
        /// <param name="report">The report.</param>
        public classificationReportExpanded(classificationReport report)
        {
            imbTypeObjectOperations.setObjectBySource(this, report);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="classificationReportExpanded"/> class.
        /// </summary>
        public classificationReportExpanded()
        {

        }
    }
}