// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportOutputFormat.cs" company="imbVeles" >
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
    using imbSCI.Data.collection.nested;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Definitions of output forms, formats and so on
    /// </summary>
    /// <seealso cref="aceCommonTypes.collection.aceEnumDictionary{aceCommonTypes.reporting.format.reportElementLevel, aceCommonTypes.reporting.format.reportElementFormSet}" />
    public class reportOutputFormat : aceEnumDictionary<reportElementLevel, reportElementFormSet>
    {
        private List<templateFieldBasic> fields = new List<templateFieldBasic>();

        //cursorZoneExecutionSettings

        private Boolean _disableNavigationBlocks;

        /// <summary>
        ///
        /// </summary>
        public Boolean disableNavigationBlocks
        {
            get { return _disableNavigationBlocks; }
            set { _disableNavigationBlocks = value; }
        }

        private elementLevelFormPreset _presetSource;

        /// <summary>What preset was used to make this</summary>
        public elementLevelFormPreset presetSource
        {
            get
            {
                return _presetSource;
            }
            protected set
            {
                _presetSource = value;
                //OnPropertyChanged("presetSource");
            }
        }

        public reportOutputFormat(elementLevelFormPreset preset)
        {
            this[reportElementLevel.servicepage] = new reportElementFormSet(reportOutputForm.none);

            this.presetSource = preset;

            switch (preset)
            {
                case elementLevelFormPreset.excelDatabaseDump:
                    fields.AddMultiple(templateFieldBasic.sci_projectname, templateFieldBasic.sci_projectdesc, templateFieldBasic.sci_projecttype, templateFieldBasic.test_caption, templateFieldBasic.test_description, templateFieldBasic.test_runstamp, templateFieldBasic.test_runstart, templateFieldBasic.test_runtime, templateFieldBasic.test_status, templateFieldBasic.test_versionCount);
                    this[reportElementLevel.documentSet] = new reportElementFormSet(reportOutputForm.folder, reportOutputFormatName.unknown, "{{{documentset_name}}}_{{{runstamp}}}").AddProperties(fields);
                    this[reportElementLevel.document] = new reportElementFormSet(reportOutputForm.file, reportOutputFormatName.sheetExcel, "{{{document_name}}}").AddProperties(fields);
                    this[reportElementLevel.page] = new reportElementFormSet(reportOutputForm.inParentFile, reportOutputFormatName.sheetExcel, "{{{page_name}}}").AddProperties(fields);
                    this[reportElementLevel.block] = new reportElementFormSet(reportOutputForm.none);
                    this[reportElementLevel.servicepage] = new reportElementFormSet(reportOutputForm.none); // service pages take care manually about them selfs
                    disableNavigationBlocks = true;

                    break;

                case elementLevelFormPreset.htmlWebSite:
                    fields.AddUnique();
                    this[reportElementLevel.documentSet] = new reportElementFormSet(reportOutputForm.folder, reportOutputFormatName.unknown, "{{{documentset_name}}}_{{{runstamp}}}").AddProperties(fields);
                    this[reportElementLevel.document] = new reportElementFormSet(reportOutputForm.folder, reportOutputFormatName.htmlReport, "{{{document_name}}}").AddProperties(fields);
                    this[reportElementLevel.page] = new reportElementFormSet(reportOutputForm.file, reportOutputFormatName.htmlReport, "{{{page_name}}}").AddProperties(fields);
                    this[reportElementLevel.block] = new reportElementFormSet(reportOutputForm.none);
                    this[reportElementLevel.servicepage] = new reportElementFormSet(reportOutputForm.file, reportOutputFormatName.sheetExcel, "{{{name}}}");
                    disableNavigationBlocks = false;

                    break;

                case elementLevelFormPreset.sciReport:
                    fields.AddMultiple(templateFieldBasic.sci_projectname, templateFieldBasic.sci_projectdesc, templateFieldBasic.sci_projecttype, templateFieldBasic.test_caption, templateFieldBasic.test_description, templateFieldBasic.test_runstamp, templateFieldBasic.test_runstart, templateFieldBasic.test_runtime, templateFieldBasic.test_status, templateFieldBasic.test_versionCount);
                    this[reportElementLevel.documentSet] = new reportElementFormSet(reportOutputForm.folder, reportOutputFormatName.unknown, "{{{documentset_name}}}_{{{runstamp}}}").AddProperties(fields);
                    this[reportElementLevel.document] = new reportElementFormSet(reportOutputForm.folder, reportOutputFormatName.htmlReport, "{{{document_name}}}").AddProperties(fields);
                    this[reportElementLevel.page] = new reportElementFormSet(reportOutputForm.file, reportOutputFormatName.htmlReport, "{{{page_name}}}").AddProperties(fields);
                    this[reportElementLevel.block] = new reportElementFormSet(reportOutputForm.none);
                    this[reportElementLevel.servicepage] = new reportElementFormSet(reportOutputForm.file, reportOutputFormatName.sheetExcel, "{{{name}}}");
                    disableNavigationBlocks = false;

                    break;

                case elementLevelFormPreset.none:
                    fields.AddMultiple(templateFieldBasic.documentset_name, templateFieldBasic.test_runstamp, templateFieldBasic.document_name, templateFieldBasic.page_name);
                    this[reportElementLevel.documentSet] = new reportElementFormSet(reportOutputForm.folder, reportOutputFormatName.unknown, "{{{documentset_name}}}_{{{runstamp}}}").AddProperties(fields);
                    this[reportElementLevel.document] = new reportElementFormSet(reportOutputForm.folder, reportOutputFormatName.htmlReport, "{{{document_name}}}").AddProperties(fields);
                    this[reportElementLevel.page] = new reportElementFormSet(reportOutputForm.file, reportOutputFormatName.htmlReport, "{{{page_name}}}").AddProperties(fields);
                    this[reportElementLevel.block] = new reportElementFormSet(reportOutputForm.none);
                    this[reportElementLevel.servicepage] = new reportElementFormSet(reportOutputForm.file, reportOutputFormatName.sheetExcel, "{{{name}}}");
                    disableNavigationBlocks = false;

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}