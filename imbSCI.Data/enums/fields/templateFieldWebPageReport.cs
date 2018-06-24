// --------------------------------------------------------------------------------------------------------------------
// <copyright file="templateFieldWebPageReport.cs" company="imbVeles" >
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
namespace imbSCI.Data.enums.fields
{
    /// <summary>
    /// Template field names - Web Page profiling report
    /// </summary>
    public enum templateFieldWebPageReport
    {
        /// <summary>
        /// Summary textt on webRequest
        /// </summary>
        code_webRequest,

        /// <summary>
        /// Loaded source HTML
        /// </summary>
        code_sourceHTML,

        /// <summary>
        /// Extracted text of source
        /// </summary>
        code_sourceTEXT,

        /// <summary>
        /// Extracted markdown text of source
        /// </summary>
        code_sourceMarkdown,

        /// <summary>
        /// Normalized HTML source code - after loading
        /// </summary>
        code_sourceHTML_norm,

        /// <summary>
        /// Web structure metrics
        /// </summary>
        web_structureMetrics,

        /// <summary>
        /// XML structure tree
        /// </summary>
        web_pageStructureTree,

        /// <summary>
        /// Optimised page structure tree
        /// </summary>
        web_pageStructureTreeOpt,

        /// <summary>
        /// Extracted template tree
        /// </summary>
        web_pageTemplateTree,

        /// <summary>
        /// Extracted template text
        /// </summary>
        web_pageTemplateTextContent,

        /// <summary>
        /// XML output of tokenized page structure
        /// </summary>
        web_pageStructureTokenized,

        /// <summary>
        /// Report on directly extracted information about company
        /// </summary>
        web_pageDirectExtraction,

        /// <summary>
        /// Report on semanticaly extracted information
        /// </summary>
        web_pageSemanticExtraction,

        /// <summary>
        /// Export of extracted knowledge
        /// </summary>
        csp_extractedKnowledge,

        /// <summary>
        /// ... after loading NBS data
        /// </summary>
        csp_enrichNBS,

        /// <summary>
        /// ... after processing APR data
        /// </summary>
        csp_enrichAPR,

        /// <summary>
        /// ... after processing CRM data
        /// </summary>
        csp_enrichCRM,

        /// <summary>
        /// RDF of Company Semantic Profile
        /// </summary>
        csp_complete,

        /// <summary>
        /// Current main imbVeles log content
        /// </summary>
        log_systemLog,

        /// <summary>
        /// Log of parent workflow
        /// </summary>
        log_workflowLog,

        /// <summary>
        /// Izveštaj o izboru unutrašnjih linkva za analizu templejta
        /// </summary>
        web_linkSelection,

        /// <summary>
        /// Izvestaj o detektovanom templejtu
        /// </summary>
        web_template,

        /// <summary>
        /// Report on web type classification
        /// </summary>
        web_classification,

        /// <summary>
        /// Report on key pages detection
        /// </summary>
        web_keypages,
    }
}