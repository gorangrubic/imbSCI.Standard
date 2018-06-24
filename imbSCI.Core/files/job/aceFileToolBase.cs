// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceFileToolBase.cs" company="imbVeles" >
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
namespace imbSCI.Core.files.job
{
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.syntax.data.files;
    using imbSCI.Core.syntax.data.files.@base;
    using imbSCI.Core.syntax.nc;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// aceFileToolBase :: basic layer of Ace Terminal Application "kernel" (the main stuff that lib/app are doing)
    /// </summary>
    /// <remarks>
    /// <para>Role: access to terminal instance, job (T) management, target files managment</para>
    /// <para>It maintaines sultural/serialization/string reading settings</para>
    /// </remarks>
    public abstract class aceFileToolBase<T> : aceProjectToolBase<T> where T : class, IAceFileJobBase, new()
    {
        protected override String extension
        {
            get
            {
                return ".job";
            }
        }

        protected override String jobname
        {
            get
            {
                return "Job";
            }
        }

        //private CultureInfo _culture; // = new CultureInfo();
        //   private T _currentJob; // = new ncFileModifyJob();
        private fileTargetList _currentFileList = new fileTargetList();

        //  private List<String> _jobLoadList = new List<String>();
        private IAceDataFile _currentNCFile = new ncFile();

        /// <summary>
        /// The main constructor.
        /// </summary>
        /// <param name="__terminal">AceTerminal instance</param>
        public aceFileToolBase(IAceComponent __component, IAceLogable __terminal) : base(__component, __terminal)
        {
            //terminal = __terminal;
        }

        ///// <summary>
        ///// Trenutna kultura
        ///// </summary>
        //// [XmlIgnore]
        //[Category("ncFileTool")]
        //[DisplayName("culture")]
        //[Description("Trenutna kultura")]
        //public CultureInfo culture
        //{
        //    get
        //    {
        //        return _culture;
        //    }
        //    set
        //    {
        //        // Boolean chg = (_culture != value);
        //        _culture = value;
        //        OnPropertyChanged("culture");
        //        // if (chg) {}
        //    }
        //}

        ///// <summary>
        ///// Current ModifyJob in memory
        ///// </summary>
        //// [XmlIgnore]
        //[Category("ncFileTool")]
        //[DisplayName("currentJob")]
        //[Description("Current ModifyJob in memory")]
        //public T currentJob
        //{
        //    get
        //    {
        //        return _currentJob;
        //    }
        //    set
        //    {
        //        // Boolean chg = (_currentJob != value);
        //        _currentJob = value;
        //        OnPropertyChanged("currentJob");
        //        // if (chg) {}
        //    }
        //}

        /// <summary>
        /// Lista fajlova koji su selektovani
        /// </summary>
        // [XmlIgnore]
        [Category("ncFileTool")]
        [DisplayName("fileList")]
        [Description("Lista fajlova koji su selektovani")]
        public fileTargetList currentFileList
        {
            get
            {
                return _currentFileList;
            }
            set
            {
                // Boolean chg = (_fileList != value);
                _currentFileList = value;
                OnPropertyChanged("currentFileList");
                // if (chg) {}
            }
        }

        ///// <summary>
        ///// List of .job files saved in runtime folder
        ///// </summary>
        //// [XmlIgnore]
        //[Category("ncFileTool")]
        //[DisplayName("jobLoadList")]
        //[Description("List of .job files saved in runtime folder")]
        //public List<String> jobLoadList
        //{
        //    get
        //    {
        //        return _jobLoadList;
        //    }
        //    set
        //    {
        //        // Boolean chg = (_jobLoadList != value);
        //        _jobLoadList = value;
        //        OnPropertyChanged("jobLoadList");
        //        // if (chg) {}
        //    }
        //}

        /// <summary>
        /// Current NC file loaded and processed
        /// </summary>
        // [XmlIgnore]
        [Category("ncFileTool")]
        [DisplayName("currentNCFile")]
        [Description("Current NC file loaded and processed")]
        public IAceDataFile currentNCFile
        {
            get
            {
                return _currentNCFile;
            }
            set
            {
                // Boolean chg = (_currentNCFile != value);
                _currentNCFile = value;
                OnPropertyChanged("currentNCFile");
                // if (chg) {}
            }
        }

        /// <summary>
        /// Returns string information about tool version
        /// </summary>
        /// <returns></returns>
        public abstract String getVersion();

        //public void saveJob(T job=null)
        //{
        //    if (job == null) job = currentJob;
        //    String path = job.name.getFilename(".job");
        //    if (job != null)
        //    {
        //        throw new NotImplementedException();
        //      //  objectSerialization.saveObjectToXML(path, job);
        //        terminal.log("Job saved name[" + job.name + "] to [" + path + "]");
        //    }
        //}

        //public T loadJob(String path)
        //{
        //    try
        //    {
        //        T job = objectSerialization.loadObjectFromXML<T>(path);
        //        if (job != null)
        //        {
        //            terminal.log("Job name[" + job.name + "] loaded from [" + path + "]");
        //            //aceCommons.saveObjectToXML(path, job);
        //        }

        //        return job;
        //    }
        //    catch (Exception ex)
        //    {
        //        terminal.log("Job load failed - path[" + path + "]");
        //        //terminal.log(ex.Message);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Podesava default current job
        /// </summary>
        /// <param name="jobName"></param>
       // public abstract void setDefaultCurrentJob(String jobName);

        //public Int32 scanForJobFiles()
        //{
        //    jobLoadList = new List<string>();

        //    String[] files = filePathOperations.findFiles(".job", "", false).ToArray();

        //    foreach (String file in files)
        //    {
        //        if (String.IsNullOrEmpty(file))
        //        {
        //        }
        //        else
        //        {
        //            jobLoadList.Add(file);
        //        }
        //    }

        //    return jobLoadList.Count();
        //}

        /// <summary>
        /// Pokrece skeniranje fajlova i vrsi backup fajlova
        /// </summary>
        /// <param name="doScan">Da li da skenira targetirano mesto?</param>
        /// <param name="doBackup">Da li da naprvi backup fajl</param>
        /// <param name="doLog"></param>
        /// <returns></returns>
        public String scanAndBackup(Boolean doScan, Boolean doBackup, Boolean doLog = false)
        {
            String debug = "ScanAndBackup for job [" + currentJob.name + "]";

            if (doScan)
            {
                currentFileList.scanFiles(currentJob.scanFiles, true);
            }
            if (doLog) debug = debug.log("Target files: " + currentFileList.targetList.Count);

            if (doBackup)
            {
                String bckLog = currentFileList.runBackup(currentJob.scanFiles.backup, doLog, "ncFileToolBackup", "");
                if (doLog) debug = debug.log(bckLog);
            }

            return debug;
        }

        public abstract String processFile(String filePath, Boolean doLog = false);

        public abstract String processFiles(Int32 startIndex = -1, Boolean doLog = false, Int32 takeSize = 0, Int32 defaultTakeSize = 0);
    }
}