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
    public delegate void LoadAdditionalInformationDelegate(classificationReportExpanded report, classificationReportCollection hostCollection, classificationReportCollectionSettings setup, ILogBuilder logger);

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.Core.math.classificationMetrics.classificationReportExpanded}" />
    public class classificationReportCollection : List<classificationReportExpanded>
    {




        public String datasetName { get; set; } = "";

        public Dictionary<String, classificationReportCollection> Children { get; set; } = new Dictionary<String, classificationReportCollection>();


        public String description { get; set; } = "";

        /// <summary>
        /// Gets or sets the root path.
        /// </summary>
        /// <value>
        /// The root path.
        /// </value>
        public String rootPath { get; set; } = "";

        public String name { get; set; } = "";

        public folderNode rootFolder { get; set; }

        public classificationReportCollection()
        {

        }

        public classificationReportCollection(String _rootPath)
        {
            DeployPath(_rootPath);
        }

        public void DeployPath(String _rootPath)
        {
            rootPath = _rootPath;
            name = Path.GetDirectoryName(rootPath);
            if (name.isNullOrEmpty()) name = rootPath;
        }

        public classificationReportCollection AddOrGetChild(String subCollectionDirPath)
        {
            subCollectionDirPath = subCollectionDirPath.Trim("\\".ToArray());
            if (!Children.ContainsKey(subCollectionDirPath))
            {
                Children.Add(subCollectionDirPath, new classificationReportCollection(subCollectionDirPath));
                var ch = Children[subCollectionDirPath];

                ch.datasetName = datasetName;
                ch.rootFolder = rootFolder.Add(subCollectionDirPath, ch.name, ch.description);
            }
            return Children[subCollectionDirPath];
        }


        public void LoadCollectionDescription(classificationReportCollectionSettings setup)
        {
            String info_p = rootFolder.findFile(setup.FILENAME_COLLECTION_INFO);

            if (info_p != "")
            {
                description = File.ReadAllText(info_p);
            }

        }

        public List<classificationReportExpanded> reportCompleteList { get; set; } = new List<classificationReportExpanded>();




        [XmlIgnore]
        public LoadAdditionalInformationDelegate LoadExtraInformation { get; set; }

        public void LoadCompleteInformation(classificationReportCollectionSettings setup, folderNode folder, ILogBuilder logger)
        {

            DeployPath(folder.path);

            rootFolder = new folderStructure(rootPath, "Root", "Root directory with experiment reports");

            var resultFiles = rootFolder.findFiles(setup.FILENAME_CLASSIFICATION_RESULTS, SearchOption.AllDirectories);

            String basePath = rootPath;
            foreach (String rFile in resultFiles)
            {
                String dirPath = Path.GetDirectoryName(rFile);
                String dirSubPath = dirPath.Substring(basePath.Length);

                classificationReportExpanded rep = classificationReportExpanded.LoadSimpleReport(rFile, logger);


                folderNode reportFolder = rootFolder.Add(dirSubPath, rep.Name, "Experiment [" + rep.Name + "] data");

                rep.folder = reportFolder;



                String subCollectionDirPath = reportFolder.parent.path.Substring(rootFolder.path.Length);

                classificationReportCollection hostCollection = this;

                if (!subCollectionDirPath.isNullOrEmpty())
                {
                    if (reportFolder.parent != rootFolder)
                    {
                        hostCollection = AddOrGetChild(subCollectionDirPath);
                    }
                }

                classificationReportExpanded rep_existing = hostCollection.FirstOrDefault(x => x.filepath == rep.filepath);

                if (rep_existing != null)
                {
                    if (rep.filecreation > rep_existing.filecreation)
                    {
                        hostCollection.Remove(rep_existing);
                        logger.log("Replacing older report [" + rep_existing.filepath + "] with new version");
                    }
                }

                hostCollection.Add(rep);

                reportCompleteList.Add(rep);
                //if (LoadExtraInformation != null)
                //{
                //    LoadExtraInformation.Invoke(rep, hostCollection, setup, logger);
                //}

            }

            logger.log("Reports (" + Count + ") directly imported from [" + rootPath + "] - Total reports, including [" + Children.Count + "] sub collections is [" + resultFiles.Count + "]");

        }

        /*

        /// <summary>
        /// Searches for report files and populates the collection
        /// </summary>
        /// <param name="reportFileName">Name of the report file.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="option">The option.</param>
        public void LoadReports(String reportFileName, folderNode folder, ILogBuilder logger, SearchOption option = SearchOption.AllDirectories)
        {

            rootPath = folder.path;

            var resultFiles = folder.findFiles(reportFileName, option);
            foreach (String rFile in resultFiles)
            {

                classificationReportExpanded rep = classificationReportExpanded.LoadSimpleReport(rFile, logger);
                classificationReportExpanded rep_existing = this.FirstOrDefault(x => x.filepath == rep.filepath);

                if (rep_existing != null)
                {
                    if (rep.filecreation > rep_existing.filecreation)
                    {
                        this.Remove(rep_existing);
                        logger.log("Replacing older report [" + rep_existing.filepath + "] with new version");
                    }
                }
                Add(rep);
            }
            logger.log("Reports (" + Count + ") imported from [" + rootPath + "]");
        }*/

    }
}