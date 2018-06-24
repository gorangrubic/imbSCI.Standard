// --------------------------------------------------------------------------------------------------------------------
// <copyright file="templateFieldBasic.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

/// /ingroup report_ll_templates
namespace imbSCI.Data.enums.fields
{
    /// <summary>
    /// Set of basic report fields that are automatically created
    /// </summary>
    public enum templateFieldBasic
    {
        /// <summary>
        /// Title - recommanded by metaDocument model
        /// </summary>
        meta_softwareName,

        /// <summary>
        /// Description - recommanded by metaDocument model
        /// </summary>
        meta_desc,

        /// <summary>
        /// Subtitle - recommanded by metaDocument model
        /// </summary>
        meta_subtitle,

        /// <summary>
        /// Author - by metaDocument model
        /// </summary>
        meta_author,

        /// <summary>
        /// The meta organization - by metaDocument model
        /// </summary>
        meta_organization,

        /// <summary>
        /// The meta keywords - by metaDocument model
        /// </summary>
        meta_keywords,

        /// <summary>
        /// The meta copyright - by metaDocument model
        /// </summary>
        meta_copyright,

        /// <summary>
        /// The meta year - by metaDocument model
        /// </summary>
        meta_year,

        /// <summary>
        /// Path directorium
        /// </summary>
        path_folder,

        /// <summary>
        /// Path filename with extension
        /// </summary>
        path_file,

        /// <summary>
        /// Path directorium name
        /// </summary>
        path_dir,

        /// <summary>
        /// The path here
        /// </summary>
        path_here,

        /// <summary>
        /// Path to delivery output folder
        /// </summary>
        path_root,

        /// <summary>
        /// Path without file name
        /// </summary>
        path_output,

        /// <summary>
        /// file format
        /// </summary>
        path_format,

        /// <summary>
        /// file extension
        /// </summary>
        path_ext,

        /// <summary>
        /// The path sub
        /// </summary>
        path_sub,

        /// <summary>
        /// Relative path to parent index.html
        /// </summary>
        parent_index,

        /// <summary>
        /// Relative path to parent folder
        /// </summary>
        parent_dir,

        /// <summary>
        /// Naziv parent objekta
        /// </summary>
        parent_type,

        /// <summary>
        /// id/pririty of this page - derived from first page instance in parent chain
        /// </summary>
        page_id,

        /// <summary>
        /// Real page number - derived from first page instance in parent chain
        /// </summary>
        page_number,

        /// <summary>
        /// The page count - counting content of parent
        /// </summary>
        page_count,

        /// <summary>
        /// Page name - derived from first page instance in parent chain
        /// </summary>
        page_name,

        /// <summary>
        /// Page descriptive title  - derived from first page instance-> header in parent chain
        /// </summary>
        page_title,

        /// <summary>
        /// Page type name - derived from first page instance in parent chain
        /// </summary>
        page_type,

        /// <summary>
        /// path to prev page
        /// </summary>
        page_next,

        /// <summary>
        /// path to next page
        /// </summary>
        page_prev,

        /// <summary>
        /// path to index page
        /// </summary>
        page_index,

        /// <summary>
        /// Url to readme
        /// </summary>
        page_readme,

        /// <summary>
        /// Relative path to the first page in parent chain
        /// </summary>
        page_relpath,

        /// <summary>
        /// id/pririty of this page - derived from first page instance in parent chain
        /// </summary>
        document_id,

        /// <summary>
        /// Real page number - derived from first page instance in parent chain
        /// </summary>
        document_number,

        /// <summary>
        /// The page count - counting content of parent
        /// </summary>
        document_count,

        /// <summary>
        /// Page name - derived from first page instance in parent chain
        /// </summary>
        document_name,

        /// <summary>
        /// Page descriptive title  - derived from first page instance-> header in parent chain
        /// </summary>
        document_title,

        /// <summary>
        /// The document desc
        /// </summary>
        document_desc,

        /// <summary>
        /// Page type name - derived from first page instance in parent chain
        /// </summary>
        document_type,

        /// <summary>
        /// Relative path to first document in parent chain
        /// </summary>
        document_relpath,

        document_next,

        document_prev,

        document_index,

        document_readme,

        /// <summary>
        /// Page name - derived from first page instance in parent chain
        /// </summary>
        documentset_name,

        /// <summary>
        /// Description
        /// </summary>
        documentset_desc,

        /// <summary>
        /// Page descriptive title  - derived from first page instance-> header in parent chain
        /// </summary>
        documentset_title,

        /// <summary>
        /// Page type name - derived from first page instance in parent chain
        /// </summary>
        documentset_type,

        /// <summary>
        ///  Relative path to first document in parent chain
        /// </summary>
        documentset_relpath,

        documentset_id,

        documentset_prev,
        documentset_next,
        documentset_index,

        documentset_readme,

        /// <summary>
        /// The document structure path
        /// </summary>
        document_path,

        /// <summary>
        /// The document set directory path
        /// </summary>
        documentset_directory,

        /// <summary>
        /// placeholders count
        /// </summary>
        self_plc,

        /// <summary>
        /// all placeholders in csv line
        /// </summary>
        self_plcl,

        /// <summary>
        /// selected format for file writing
        /// </summary>
        self_format,

        /// <summary>
        /// ???
        /// </summary>
        self_type,

        ///// <summary>
        ///// ???
        ///// </summary>
        //self_tflags,

        /// <summary>
        /// Tab level
        /// </summary>
        self_tabl,

        /// <summary>
        /// The self (object that calls) title
        /// </summary>
        self_title,

        /// <summary>
        /// The self (object that calls) desc
        /// </summary>
        self_desc,

        /// <summary>
        /// The self rflags
        /// </summary>
        self_rflags,

        /// <summary>
        /// imbFramework.test.testDefinition -> caption
        /// </summary>
        test_caption,

        /// <summary>
        /// imbFramework.test.testDefinition -> description
        /// </summary>
        test_description,

        /// <summary>
        /// Version of the test
        /// </summary>
        test_versionCount,

        /// <summary>
        /// Current runstamp
        /// </summary>
        test_runstamp,

        /// <summary>
        /// How long it is running
        /// </summary>
        test_runtime,

        /// <summary>
        /// When it started
        /// </summary>
        test_runstart,

        /// <summary>
        /// The test status
        /// </summary>
        test_status,

        /// <summary>
        /// Name of research project
        /// </summary>
        sci_projectname,

        /// <summary>
        /// Path of project file
        /// </summary>
        sci_projectpath,

        /// <summary>
        /// Type name of the project
        /// </summary>
        sci_projecttype,

        /// <summary>
        /// English name of the language loaded
        /// </summary>
        lang_eName,

        /// <summary>
        /// Native name of the language
        /// </summary>
        lang_nName,

        /// <summary>
        /// ISO code of the language
        /// </summary>
        lang_iso,

        sample_count,

        sample_group,

        sample_totalcount,

        /// <summary>
        /// Memory allocated
        /// </summary>
        sys_mem,

        /// <summary>
        /// Threads running
        /// </summary>
        sys_threads,

        /// <summary>
        /// How long the app is running
        /// </summary>
        sys_runtime,

        /// <summary>
        /// Application name - domain desc
        /// </summary>
        sys_app,

        /// <summary>
        /// Execution dir path for the app
        /// </summary>
        sys_path,

        /// <summary>
        /// Unique string UID
        /// </summary>
        sys_uid,

        /// <summary>
        /// Current date
        /// </summary>
        sys_date,

        /// <summary>
        /// Current time
        /// </summary>
        sys_time,

        /// <summary>
        /// Global, system log content
        /// </summary>
        sys_log,

        /// <summary>
        /// The system start time
        /// </summary>
        sys_start,

        /// <summary>
        /// The sci project instance description
        /// </summary>
        sci_projectdesc,

        root_relpath,

        sys_cputype,
        sys_osname,
        sys_osversion,
        sys_memphysical,
        sys_memvirtual,
        sys_pagefile,
        self_palette,
        page_desc,
        documentset_path,
        page_path,
        meta_softwareComment,
        sample_limit,

        /// <summary>
        /// The report folder: naziv foldera u kome je glavni deo izveštaja
        /// </summary>
        report_folder,

        sci_totalSample,
    }
}