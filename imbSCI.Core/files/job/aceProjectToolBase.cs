// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceProjectToolBase.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.interfaces;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Basic project definition
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="imbSCI.Data.data.dataBindableBase" />
    public abstract class aceProjectToolBase<T> : dataBindableBase where T : class, IAceProjectBase, new()
    {
        public aceProjectToolBase()
        {
        }

        public aceProjectToolBase(IAceComponent __component, IAceLogable __terminal = null)
        {
            _component = __component;

            if (__terminal != null)
            {
                terminal = __terminal as IAceLogable;
            }
            else
            {
            }

            Console.OutputEncoding = Encoding.UTF8;
            // Change current culture
            CultureInfo culture;

            culture = CultureInfo.CreateSpecificCulture("en-US");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        protected abstract String extension { get; }
        //{
        //    get
        //    {
        //        return ".job";
        //    }
        //}

        protected abstract String jobname { get; }
        //{
        //    get
        //    {
        //        return "Job";
        //    }
        //}

        private IAceComponent _component; // = "";

        /// <summary>
        /// Reference to component
        /// </summary>
        public IAceComponent component
        {
            get { return _component; }
        }

        protected IAceLogable terminal;
        protected CultureInfo _culture; // = new CultureInfo();
        protected T _currentJob; // = new ncFileModifyJob();
        protected List<String> _jobLoadList = new List<String>();

        public T currentJob
        {
            get
            {
                return _currentJob;
            }
            set
            {
                // Boolean chg = (_currentJob != value);
                _currentJob = value;
                OnPropertyChanged("currentJob");
                // if (chg) {}
            }
        }

        /// <summary>
        /// List of .job files saved in runtime folder
        /// </summary>
        // [XmlIgnore]
        [Category("ncFileTool")]
        [DisplayName("jobLoadList")]
        [Description("List of .job files saved in runtime folder")]
        public List<String> jobLoadList
        {
            get
            {
                return _jobLoadList;
            }
            set
            {
                // Boolean chg = (_jobLoadList != value);
                _jobLoadList = value;
                OnPropertyChanged("jobLoadList");
                // if (chg) {}
            }
        }

        public void saveJob(T job = null)
        {
            if (job == null) job = currentJob;

            DirectoryInfo di = projectFolderPath.getDirectory();

            String filename = job.name.getFilename(extension);

            String savePath = di.FullName.add(filename, "\\");

            FileInfo fi = savePath.getWritableFile(getWritableFileMode.autoRenameExistingToBack);

            if (job != null)
            {
                objectSerialization.saveObjectToXML(fi.FullName, job);
                terminal.log(jobname + " saved name[" + job.name + "] to [" + fi.FullName + "]");
            }
        }

        public T loadJob(String path)
        {
            String targetPath = path;

            if (!Path.IsPathRooted(targetPath))
            {
                targetPath = targetPath.add(path, "\\");
                terminal.log("~loadJob~ (" + path + ") => " + targetPath);
            }
            else
            {
                terminal.log("~loadJob~ => " + path + ")");
            }

            try
            {
                T job = objectSerialization.loadObjectFromXML<T>(path);
                if (job != null)
                {
                    terminal.log("~loadJob~ => " + jobname + " name[" + job.name + "] loaded from [" + path + "]");
                    //aceCommons.saveObjectToXML(path, job);
                }

                return job;
            }
            catch (Exception ex)
            {
                terminal.log("~loadJob~ => " + jobname + " load failed - path[" + path + "]");
                //terminal.log(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Podesava default current job
        /// </summary>
        /// <param name="jobName"></param>
        public abstract void setDefaultCurrentJob(String jobName);

        /// <summary>
        /// Path to save, search and to load projects/jobs from
        /// </summary>
        /// <value>
        /// The project folder path.
        /// </value>
        public abstract String projectFolderPath { get; }

        /// <summary>
        /// Skenira okruženje (runtime path po defaltu) za snimljene job definicije
        /// </summary>
        /// <returns></returns>
        public Int32 scanForJobFiles()
        {
            jobLoadList = new List<string>();

            String[] files = filePathOperations.findFiles(extension, projectFolderPath, false).ToArray();

            foreach (String file in files)
            {
                if (String.IsNullOrEmpty(file))
                {
                }
                else
                {
                    jobLoadList.Add(file);
                }
            }

            return jobLoadList.Count();
        }
    }
}