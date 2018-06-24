// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileTargetListSettings.cs" company="imbVeles" >
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
    using imbSCI.Core.files.backup;
    using imbSCI.Data.data;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Settings on targeting and preprocessing of matched files that are subject of the Ace Application
    /// </summary>
    public class fileTargetListSettings : dataBindableBase
    {
        #region -----------  path  -------  [Path to directory with NC files. Subdirectories are included if doIncludeSubFolders is True]

        private String _path = @"ncSources"; // = new String();

        /// <summary>
        /// Path to directory with NC files. Subdirectories are included if doIncludeSubFolders is True
        /// </summary>
        // [XmlIgnore]
        [Category("ncFileModifyJob")]
        [DisplayName("Path")]
        [Description("Directory with NC files to target/select - if have no \\ it's relative to folder where this program is running. Subdirectories are included if doIncludeSubFolders is True")]
        public String path
        {
            get
            {
                return _path;
            }
            set
            {
                // Boolean chg = (_path != value);
                _path = value;
                OnPropertyChanged("path");
                // if (chg) {}
            }
        }

        #endregion -----------  path  -------  [Path to directory with NC files. Subdirectories are included if doIncludeSubFolders is True]

        #region -----------  outputPath  -------  [Path where to save processed NC files]

        private String _outputPath = "ncDone"; // = new String();

        /// <summary>
        /// Path where to save processed NC files
        /// </summary>
        // [XmlIgnore]
        [Category("fileTargetListSettings")]
        [DisplayName("Processed path")]
        [Description("Path to put targeted and processed NC files - that mached the criteria of line selector")]
        public String outputPath
        {
            get
            {
                return _outputPath;
            }
            set
            {
                // Boolean chg = (_outputPath != value);
                _outputPath = value;
                OnPropertyChanged("outputPath");
                // if (chg) {}
            }
        }

        #endregion -----------  outputPath  -------  [Path where to save processed NC files]

        #region -----------  outputUnmatchPath  -------  [Path for unmatched .NC files - the ones not matching line selection criterias]

        private String _outputUnmatchPath = "ncSkip";// = new String();

        /// <summary>
        /// Path for unmatched .NC files - the ones not matching line selection criterias
        /// </summary>
        // [XmlIgnore]
        [Category("fileTargetListSettings")]
        [DisplayName("Unmatch path")]
        [Description("Path for unmatched .NC files - the ones not matching line selection criterias")]
        public String outputUnmatchPath
        {
            get
            {
                return _outputUnmatchPath;
            }
            set
            {
                // Boolean chg = (_outputUnmatchPath != value);
                _outputUnmatchPath = value;
                OnPropertyChanged("outputUnmatchPath");
                // if (chg) {}
            }
        }

        #endregion -----------  outputUnmatchPath  -------  [Path for unmatched .NC files - the ones not matching line selection criterias]

        #region -----------  doIncludeSubFolders  -------  [If True it will search all subfolders inside path]

        private Boolean _doIncludeSubFolders; // = new Boolean();

        /// <summary>
        /// If True it will search all subfolders inside path
        /// </summary>
        // [XmlIgnore]
        [Category("ncFileModifyJob")]
        [DisplayName("Search subfolders")]
        [Description("If True it will search all subfolders of input NC files path (Path)")]
        public Boolean doIncludeSubFolders
        {
            get
            {
                return _doIncludeSubFolders;
            }
            set
            {
                // Boolean chg = (_doIncludeSubFolders != value);
                _doIncludeSubFolders = value;
                OnPropertyChanged("doIncludeSubFolders");
                // if (chg) {}
            }
        }

        #endregion -----------  doIncludeSubFolders  -------  [If True it will search all subfolders inside path]

        #region -----------  fileExtension  -------  [NC files extension - to be searched for]

        private String _fileExtension = ".nc"; // = new String();

        /// <summary>
        /// NC files extension - to be searched for
        /// </summary>
        // [XmlIgnore]
        [Category("ncFileModifyJob")]
        [DisplayName("NC extension")]
        [Description("NC files extension - it's used for search pattern. You may put .* to process all files but it may crash if these are not textual/real NC files.")]
        public String fileExtension
        {
            get
            {
                return _fileExtension;
            }
            set
            {
                // Boolean chg = (_fileExtension != value);
                _fileExtension = value;
                OnPropertyChanged("fileExtension");
                // if (chg) {}
            }
        }

        #endregion -----------  fileExtension  -------  [NC files extension - to be searched for]

        #region -----------  backup  -------  [Policy for selected files backup]

        private backupPolicy _backup = backupPolicy.zipSelected; // = new backupPolicy();

        /// <summary>
        /// Policy for selected files backup
        /// </summary>
        // [XmlIgnore]
        [Category("fileTargetListSettings")]
        [DisplayName("Backup mode")]
        [Description("Policy on making reserve copy of input files before any processing.")]
        public backupPolicy backup
        {
            get
            {
                return _backup;
            }
            set
            {
                // Boolean chg = (_backup != value);
                _backup = value;
                OnPropertyChanged("backup");
                // if (chg) {}
            }
        }

        #endregion -----------  backup  -------  [Policy for selected files backup]
    }
}