// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BibTexLoadSettings.cs" company="imbVeles" >
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
// Project: imbSCI.BibTex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using imbSCI.Data.data;
using System;
using System.ComponentModel;

namespace imbSCI.BibTex
{
    /// <summary>
    /// Settings for BibTex file loading and preprocessing
    /// </summary>
    /// <seealso cref="imbSCI.Data.data.imbBindable" />
    public class BibTexLoadSettings : imbBindable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BibTexLoadSettings"/> class.
        /// </summary>
        public BibTexLoadSettings()
        {
        }

        #region ----------- Boolean [ doSplitEntries ] -------  [If true, it will process entries separately]

        private Boolean _doSplitEntries = true;

        /// <summary>
        /// If true, it will process entries separately
        /// </summary>
        [Category("Switches")]
        [DisplayName("doSplitEntries")]
        [Description("If true, it will process entries separately")]
        public Boolean doSplitEntries
        {
            get { return _doSplitEntries; }
            set { _doSplitEntries = value; OnPropertyChanged("doSplitEntries"); }
        }

        #endregion ----------- Boolean [ doSplitEntries ] -------  [If true, it will process entries separately]

        #region ----------- Boolean [ doConstructObjectModelDictionary ] -------  [If true, it will build dictionary of object instances - after entries loaded]

        private Boolean _doConstructObjectModelDictionary = true;

        /// <summary>
        /// If true, it will build dictionary of object instances - after entries loaded
        /// </summary>
        [Category("Switches")]
        [DisplayName("doConstructObjectModelDictionary")]
        [Description("If true, it will build dictionary of object instances - after entries loaded")]
        public Boolean doConstructObjectModelDictionary
        {
            get { return _doConstructObjectModelDictionary; }
            set { _doConstructObjectModelDictionary = value; OnPropertyChanged("doConstructObjectModelDictionary"); }
        }

        #endregion ----------- Boolean [ doConstructObjectModelDictionary ] -------  [If true, it will build dictionary of object instances - after entries loaded]

        #region ----------- Boolean [ doPreprocessSource ] -------  [If true, it will replace LaTeX commands with UTF-8 equivalent characters]

        private Boolean _doPreprocessSource = true;

        /// <summary>
        /// If true, it will replace LaTeX commands with UTF-8 equivalent characters
        /// </summary>
        [Category("Switches")]
        [DisplayName("doPreprocessSource")]
        [Description("If true, it will replace LaTeX commands with UTF-8 equivalent characters")]
        public Boolean doPreprocessSource
        {
            get { return _doPreprocessSource; }
            set { _doPreprocessSource = value; OnPropertyChanged("doPreprocessSource"); }
        }

        #endregion ----------- Boolean [ doPreprocessSource ] -------  [If true, it will replace LaTeX commands with UTF-8 equivalent characters]

        #region ----------- Boolean [ doExportBibTexAfterLoad ] -------  [If true, it will re-export BibTex file, just after being loaded and parsed]

        private Boolean _doExportBibTexAfterLoad = true;

        /// <summary>
        /// If true, it will re-export BibTex file, just after being loaded and parsed
        /// </summary>
        [Category("Switches")]
        [DisplayName("doExportBibTexAfterLoad")]
        [Description("If true, it will re-export BibTex file, just after being loaded and parsed")]
        public Boolean doExportBibTexAfterLoad
        {
            get { return _doExportBibTexAfterLoad; }
            set { _doExportBibTexAfterLoad = value; OnPropertyChanged("doExportBibTexAfterLoad"); }
        }

        #endregion ----------- Boolean [ doExportBibTexAfterLoad ] -------  [If true, it will re-export BibTex file, just after being loaded and parsed]

        private Boolean _doSavePreprocessedSource = true;

        /// <summary>If true, it will save preprocessed version of the BibTex source file </summary>
        public Boolean doSavePreprocessedSource
        {
            get
            {
                return _doSavePreprocessedSource;
            }
            set
            {
                _doSavePreprocessedSource = value;
                OnPropertyChanged("DoSavePreprocessedSource");
            }
        }
    }
}