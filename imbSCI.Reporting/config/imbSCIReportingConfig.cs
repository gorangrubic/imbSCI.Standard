// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbSCIReportingConfig.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;

namespace imbSCI.Reporting.config
{
    /// <summary>
    /// General configuration object for domain of <see cref="imbSCI.Reporting"/>
    /// </summary>
	public class imbSCIReportingConfig
    {
        #region --------------------------------------------------- DO NOT MODIFY --------------------------------------------------------------

        /// <summary>
        /// Constructor without arguments is obligatory for XML serialization
        /// </summary>
        public imbSCIReportingConfig() { }

        /// <summary>
        /// Gets or sets a value indicating whether, since program start, <see cref="settings"/> were replaced with another instance, i.e. loaded externally
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is default replaced; otherwise, <c>false</c>.
        /// </value>
        public static Boolean isDefaultReplaced { get; set; } = false;

        private static imbSCIReportingConfig _settings = new imbSCIReportingConfig();

        /// <summary>
        /// General configuration instance for domain of <see cref="imbSCI.Reporting"/>
        /// </summary>
        /// <value>
        /// Global settings
        /// </value>
        public static imbSCIReportingConfig settings
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

        // Insert below your global configuration properties.
        // Snippets: _imbSCI_DoBool, _imbSCI_String, _imbSCI_Ratio ....

        /// <summary> Export Clean Data together with Excel files </summary>
        [Category("Switch")]
        [DisplayName("doExportCleanDataToo")]
        [Description("Export Clean Data together with Excel files")]
        public Boolean doExportCleanDataToo { get; set; } = true;
    }
}