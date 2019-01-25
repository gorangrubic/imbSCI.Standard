// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbSCICoreConfig.cs" company="imbVeles" >
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
using imbSCI.Core.data.help;
using System;

namespace imbSCI.Core.config
{
    /// <summary>
    /// General configuration object for domain of <see cref="imbSCI.Core"/> and <see cref="imbSCI.Data"/>
    /// </summary>
    public class imbSCICoreConfig
    {
        #region --------------------------------------------------- DO NOT MODIFY --------------------------------------------------------------

        /// <summary>
        /// Constructor without arguments is obligatory for XML serialization
        /// </summary>
        public imbSCICoreConfig() { }

        /// <summary>
        /// Gets or sets a value indicating whether, since program start, <see cref="settings"/> were replaced with another instance, i.e. loaded externally
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is default replaced; otherwise, <c>false</c>.
        /// </value>
        public static Boolean isDefaultReplaced { get; set; } = false;

        private static imbSCICoreConfig _settings = new imbSCICoreConfig();

        /// <summary>
        /// General settings for domain of <see cref="imbSCI.Core"/> and <see cref="imbSCI.Data"/>
        /// </summary>
        /// <value>
        /// Global settings
        /// </value>
        public static imbSCICoreConfig settings
        {
            get
            {
                return _settings;
            }
            set
            {
                if ((_settings != value) && (value != null)) isDefaultReplaced = true;

                _settings = value;
            }
        }

        #endregion --------------------------------------------------- DO NOT MODIFY --------------------------------------------------------------

        public Boolean DoShowFileDataStructureErrors { get; set; } = false;

        public Boolean DoSaveLogNoteForDataStructureErrors { get; set; } = true;

        public Boolean DoThrowExceptionOnDataStructureErrors { get; set; } = true;

        public Boolean DataTableReports_DoExportCleanData { get; set; } = true;

        public Boolean DataTableReports_DoExportColMetaData { get; set; } = true;

        /// <summary>
        /// It will export serialized table object in "clean data" sub folder
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data table reports do export XML data]; otherwise, <c>false</c>.
        /// </value>
        public Boolean DataTableReports_DoExportXMLData { get; set; } = true;

        public Int32 DataTableReports_RowsCountToDisableStyling { get; set; } = 200;

        public Int32 DataTableReports_RowsApplyStylingLimit { get; set; } = 120;
        public Boolean DataTableReports_AsyncExportCalls { get; set; } = true;

        public String DataTableReports_SignatureLine { get; set; } = "Report generated with imbSCI library - imbVeles \"Coding for Science\" framework [GNU GPLv3] - blog.veles.rs";

        public helpBuilderConfiguration globalHelpBuilderSetup { get; set; } = new helpBuilderConfiguration();

        // Insert below your global configuration properties.
        // Snippets: _imbSCI_DoBool, _imbSCI_String, _imbSCI_Ratio ....
    }
}