// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportElementFormSet.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.format
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Data.data;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections.Generic;

    // reportElement, reportOutputForm

#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    /// <summary>
    /// Default output settings for metaContent elementLevel
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    /// <seealso cref="elementLevelFormPreset" />
    public class reportElementFormSet : imbBindable
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    {
        private Boolean _doTextShadowCopy = true;

        /// <summary>
        ///
        /// </summary>
        public Boolean doTextShadowCopy
        {
            get { return _doTextShadowCopy; }
            set { _doTextShadowCopy = value; }
        }

        private Boolean _doEnableNamedDivsToData = true;

#pragma warning disable CS1574 // XML comment has cref attribute 'c_close' that could not be resolved
#pragma warning disable CS1574 // XML comment has cref attribute 'c_open' that could not be resolved
        /// <summary>
        /// It will keep record for each named content section defined by: <see cref="aceCommonTypes.enums.appendType.c_open"/>  and <see cref="aceCommonTypes.enums.appendType.c_close"/> calls
        /// </summary>
        /// <remarks>
        /// To have this operational you must have <see cref="doTextShadowCopy"/> set to TRUE.
        /// </remarks>
        public Boolean doEnableNamedDivsToData
#pragma warning restore CS1574 // XML comment has cref attribute 'c_open' that could not be resolved
#pragma warning restore CS1574 // XML comment has cref attribute 'c_close' that could not be resolved
        {
            get { return _doEnableNamedDivsToData; }
            set { _doEnableNamedDivsToData = value; }
        }

        private Boolean _doInMemoryOutputRepository = true;

        /// <summary>
        ///
        /// </summary>
        public Boolean doInMemoryOutputRepository
        {
            get { return _doInMemoryOutputRepository; }
            set { _doInMemoryOutputRepository = value; }
        }

        private Boolean _doAutoUpdateStatusData = true;

        /// <summary>
        /// It will call autoupdate status data pn each scopeIn
        /// </summary>
        public Boolean doAutoUpdateStatusData
        {
            get { return _doAutoUpdateStatusData; }
            set { _doAutoUpdateStatusData = value; }
        }

        public reportElementFormSet(reportOutputForm __form, String __nametemplate = "{{{name}}}")
        {
            form = __form;
            nametemplate = __nametemplate;
        }

        public reportElementFormSet(reportOutputForm __form, reportOutputFormatName __format, String __nametemplate = "{{{name}}}")
        {
            form = __form;
            nametemplate = __nametemplate;
            fileformat = __format;
        }

        public List<templateFieldBasic> customProperties = new List<templateFieldBasic>();

        public reportElementFormSet AddProperties(List<templateFieldBasic> fields)
        {
            List<templateFieldBasic> flat = fields.getFlatList<templateFieldBasic>();
            customProperties.AddRange(flat);
            return this;
        }

        public reportElementFormSet AddProperties(params templateFieldBasic[] fields)
        {
            List<templateFieldBasic> flds = fields.getFlatList<templateFieldBasic>();

            customProperties.AddRange(flds);
            return this;
        }

        private reportOutputForm _form = reportOutputForm.unknown;

        /// <summary>
        ///
        /// </summary>
        public reportOutputForm form
        {
            get { return _form; }
            set { _form = value; }
        }

        private String _nametemplate = "{{{name}}}";

        /// <summary>
        ///
        /// </summary>
        public String nametemplate
        {
            get { return _nametemplate; }
            set { _nametemplate = value; }
        }

        private reportOutputFormatName _fileformat = reportOutputFormatName.unknown;

        /// <summary>
        ///
        /// </summary>
        public reportOutputFormatName fileformat
        {
            get { return _fileformat; }
            set { _fileformat = value; }
        }

        public reportElementFormSet()
        {
        }
    }
}