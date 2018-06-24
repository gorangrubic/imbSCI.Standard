// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileTargetList.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.data.files
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.files.backup;
    using imbSCI.Data.data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// List of files matching the target list
    /// </summary>
    public class fileTargetList : dataBindableBase
    {
        public fileTargetList()
        {
        }

        /// <summary>
        /// Executes tasks on the list according to given fileTargetListTask and fileTargetListSettings
        /// </summary>
        /// <param name="task"></param>
        /// <param name="settings"></param>
        /// <param name="jobName">Name of related Job - used for reporting/diagnostic purposes only</param>
        /// <returns></returns>
        public String runTask(fileTargetListTask task, fileTargetListSettings settings, String jobName = "unknown")
        {
            String output = "";

            switch (task)
            {
                case fileTargetListTask.scan:
                    output = output.log(scanFiles(settings, true));
                    break;

                case fileTargetListTask.saveList:
                    String listFilename = jobName.getFilename(".txt");
                    listFilename = filePathOperations.addUniqueSufix(listFilename);
                    File.WriteAllLines(listFilename, targetList.ToArray());
                    output = "List saved to: " + listFilename;
                    break;

                case fileTargetListTask.showList:
                    output = targetList.writeListOfCollection();
                    break;

                case fileTargetListTask.backup:
                    output = runBackup(settings.backup, true, jobName);
                    break;

                case fileTargetListTask.done:
                    break;
            }

            return output;
        }

        /// <summary>
        /// Vraca jedan fajl na osnocu moda
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public String takeOneSample(fileTargetSampleMode mode)
        {
            String output = "";

            switch (mode)
            {
                case fileTargetSampleMode.firstAndNextFile:

                    output = targetList.takeItemRelativeTo<String>(lastTake, 1);
                    /*
                    if (String.IsNullOrEmpty(lastTake))
                    {
                        output = targetList.First();
                    } else
                    {
                        output = targetList.takeItemRelativeTo<String>(lastTake, 1);
                    }*/
                    break;

                case fileTargetSampleMode.randomFile:
                    Random rnd = new Random();
                    Int32 ind2 = rnd.Next(targetList.Count());
                    output = targetList.takeItemRelativeTo<String>(lastTake, rnd.Next(targetList.Count()));
                    break;
            }

            lastTake = output;
            return output;
        }

        #region --- lastTake ------- Fajl path koji je poslednji put vratio

        private String _lastTake = "";

        /// <summary>
        /// Fajl path koji je poslednji put vratio
        /// </summary>
        public String lastTake
        {
            get
            {
                return _lastTake;
            }
            set
            {
                _lastTake = value;
                OnPropertyChanged("lastTake");
            }
        }

        #endregion --- lastTake ------- Fajl path koji je poslednji put vratio

        #region -----------  targetList  -------  [Ciljana lista fajlova]

        private List<String> _targetList = new List<String>();

        /// <summary>
        /// Ciljana lista fajlova
        /// </summary>
        // [XmlIgnore]
        [Category("fileTargetList")]
        [DisplayName("targetList")]
        [Description("Ciljana lista fajlova")]
        public List<String> targetList
        {
            get
            {
                return _targetList;
            }
            set
            {
                // Boolean chg = (_targetList != value);
                _targetList = value;
                OnPropertyChanged("targetList");
                // if (chg) {}
            }
        }

        #endregion -----------  targetList  -------  [Ciljana lista fajlova]

        public String runBackup(backupPolicy policy, Boolean doLog = false, String backupName = "", String backupPath = "")
        {
            return backupTool.runBackup(targetList, policy, doLog, backupName, backupPath);
        }

        /// <summary>
        /// Skenira putanju - prema podesavanjima
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="doClearExistingInList">Da li prazni listu fajlova koju trenutno ima u memoriji</param>
        /// <returns></returns>
        public String scanFiles(fileTargetListSettings settings, Boolean doClearExistingInList = true)
        {
            String debug = "";
            String path = settings.path;
            //if (Path.IsPathRooted(settings.path))
            //{
            //    path =
            //}
            if (!Directory.Exists(settings.path))
            {
                debug = debug.log("Current target path do not exists [" + settings.path + "] !");
                return debug;
            }

            if (doClearExistingInList)
            {
                targetList = new List<string>();
            }

            String query = "*." + settings.fileExtension;
            query = query.Replace("..", ".");

            if (settings.doIncludeSubFolders)
            {
                targetList.AddRange(Directory.EnumerateFiles(settings.path, query, SearchOption.AllDirectories));
            }
            else
            {
                targetList.AddRange(Directory.EnumerateFiles(settings.path, query, SearchOption.TopDirectoryOnly));
            }

            // debug.log("Target files detected: " + targetList.Count() + " at path: " + settings.path);
            if (targetList.Count() == 0)
            {
                debug = debug.log("No files found on target path");
            }
            return debug;
        }
    }
}