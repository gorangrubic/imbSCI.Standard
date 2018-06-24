// --------------------------------------------------------------------------------------------------------------------
// <copyright file="composerOutput.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.render
{
    /// <summary>
    /// Formats to export report into
    /// </summary>
    public enum composerOutput
    {
        /// <summary>
        /// Plain text format using renderText renderer
        /// </summary>
        text,

        /// <summary>
        /// Markdown format using renterMarkdown renderer
        /// </summary>
        markdown,

        /// <summary>
        /// HTML format via Markdown using Markdig HTML rendering
        /// </summary>
        markdownHtml,

        /// <summary>
        /// Direct render of HTML
        /// </summary>
        imbVelesHtml,

        /// <summary>
        /// Dynamic CSS from template and/or external file
        /// </summary>
        imbVelesCSS,

        /// <summary>
        /// Dynamic javascript from template and with data points
        /// </summary>
        imbVelesJS,

        /// <summary>
        /// HTML format via Markdown using Markdig HTML rendering, then HTML to PDF export
        /// </summary>
        markdownPdf,

        /// <summary>
        /// Excel direct formating via renderer
        /// </summary>
        excel,

        /// <summary>
        /// Excel exporter via EPPlus - for tabular export. Multiple dataTables are exported into one file
        /// </summary>
        excelTablesNested,

        /// <summary>
        /// Excel exporter via EPPlus - separate file in sub folder per each DataTable
        /// </summary>
        excelTablesSeparated,
    }
}