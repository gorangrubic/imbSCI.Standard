// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceFileJobBase.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.syntax.data.files;
    using imbSCI.Data.data;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// 2017: Polazna klasa za ACE radni projekat / Job. Primenjuje se na ncFileJob ali ima ulogu i u imbVeles projektu.
    /// </summary>
    public abstract class aceFileJobBase : dataBindableBase, IAceFileJobBase
    {
        public aceFileJobBase()
        {
        }

        private String _name = "ncJob"; // = new String();
        private fileTargetListSettings _scanFiles = new fileTargetListSettings();
        private Int32 _processTakeSize = 50; // = new Int32();

        /// <summary>
        /// Name for this Job - used for filename and for menu navigation
        /// </summary>
        // [XmlIgnore]
        [Category("ncFileModifyJob")]
        [DisplayName("Job Name")]
        [Description("Name for this Job - used for filename and menu navigation")]
        public String name
        {
            get
            {
                return _name;
            }
            set
            {
                // Boolean chg = (_name != value);
                _name = value;
                OnPropertyChanged("name");
                // if (chg) {}
            }
        }

        /// <summary>
        /// Podesavanja skeniranja fajla
        /// </summary>
        // [XmlIgnore]
        [Category("ncFileModifyJob")]
        [DisplayName("NC files")]
        [Description("Podesavanja skeniranja fajla")]
        public fileTargetListSettings scanFiles
        {
            get
            {
                return _scanFiles;
            }
            set
            {
                // Boolean chg = (_scanFiles != value);
                _scanFiles = value;
                OnPropertyChanged("scanFiles");
                // if (chg) {}
            }
        }

        /// <summary>
        /// Number of files to be processed in one processing take; 0 and -1 will set program default
        /// </summary>
        // [XmlIgnore]
        [Category("ncFileModifyJob")]
        [DisplayName("Process take")]
        [Description("Number of files to be processed in one processing take; 0 and -1 will set program default")]
        public Int32 processTakeSize
        {
            get
            {
                return _processTakeSize;
            }
            set
            {
                // Boolean chg = (_processTakeSize != value);
                _processTakeSize = value;
                OnPropertyChanged("processTakeSize");
                // if (chg) {}
            }
        }

        /// <summary>
        /// Generise string izvestaj o trenutnom poslu
        /// </summary>
        /// <returns></returns>
        public virtual String explainJob()
        {
            String output = "";
            output = output.log(String.Format("Job name [{0}] at [{1}]", this.name, this.name.getFilename(".job")));

            String scanOutput = "It will look into path [{0}] for files with extension [{1}]";

            if (scanFiles.doIncludeSubFolders)
            {
                scanOutput += " including all subfolders.";
            }
            else
            {
                scanOutput += " excluding subfolders.";
            }

            output = output.log(String.Format(scanOutput, scanFiles.path, scanFiles.fileExtension));

            // output = output.log(backupTool.explainBackup(scanFiles.backup));
            return output;
        }
    }
}