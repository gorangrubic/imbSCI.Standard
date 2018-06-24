// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportOutputFormatName.cs" company="imbVeles" >
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
    /// <summary>
    /// Format u kome se generise izvestaj
    /// </summary>
    public enum reportOutputFormatName
    {
        /// <summary>
        /// koristi obican text format
        /// </summary>
        textFile,

        textMdFile,

        /// <summary>
        /// log file
        /// </summary>
        textLog,

        textHtml,
        textXml,

        /// <summary>
        /// The ser XML - object serialization result
        /// </summary>
        serXml,

        htmlViaMD,

        /// <summary>
        /// The sheet excel - single file, xlsx
        /// </summary>
        sheetExcel,

        /// <summary>
        /// The sheet CSV - collection of - if worksheet containes more than one worksheet
        /// </summary>
        sheetCsv,

        sheetHtml,
        sheetPDF,

        /// <summary>
        /// The sheet XML - workbook xml
        /// </summary>
        sheetXML,

        docRTF,
        docHTML,
        docPDF,

        docJPG,
        docTIFF,
        docPNG,

        rdf,

        owl,

        json,

        /// <summary>
        /// Koristi HTML format i default browser
        /// </summary>
        htmlReport,

        /// <summary>
        /// Text file describing a folder
        /// </summary>
        folderReadme,

        xml,

        /// <summary>
        /// obican text za email
        /// </summary>
        emailPlainText,

        /// <summary>
        /// HTML format za email
        /// </summary>
        emailHTML,

        Excel,
        Calc,
        csv,

        none,
        markdown,
        textCss,
        docXAML,
        unknown,
        Word,
        Writter,
    }
}