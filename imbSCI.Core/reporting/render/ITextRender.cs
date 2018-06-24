// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextRender.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.render
{
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render.config;
    using imbSCI.Core.reporting.render.converters;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Universal API for report construction -- use in low-level application. <see cref="IDocumentRender"/> is prefered for regular use
    /// </summary>
    /// \ingroup_disabled report_int
    public interface ITextRender : ITabLevelControler, IRender, ITextAppendContent
    {
        converterBase converter { get; }

        long lastLength { get; }
        long Length { get; }

        /// <summary>
        /// Gets specified content segment, or complete content
        /// </summary>
        /// <param name="fromLength">From length - by default from start</param>
        /// <param name="toLength">To length - by default to the end</param>
        /// <returns>The slice of the content</returns>
        String GetContent(long fromLength = long.MinValue, long toLength = long.MinValue);

        String getLastLine(Boolean removeIt = false);

        void consoleAltColorToggle(Boolean setExact = false, Int32 altChange = -1);

        // void AppendCheckList(checkList list, Boolean isVertical, checkListItemValue filter = checkListItemValue.none);

        /// <summary>
        /// Saves the document containing this page.
        /// </summary>
        /// <returns></returns>
        /// \ingroup_disabled renderapi_service
        FileInfo savePage(String name, reportOutputFormatName format = reportOutputFormatName.none);

        /// <summary>
        /// Creates new document both in filesytem and internal memory. Location for new file is current directory.
        /// </summary>
        /// <param name="name">Name of new document. It will transform it to filename version and add proper file extension. No problem if you put extension alone.</param>
        /// <param name="mode">How any existing file should be handled</param>
        /// <returns>fileinfo pointing to the newly created focument.</returns>
        Object addDocument(String name, Boolean scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameExistingOnOtherDate, reportOutputFormatName format = reportOutputFormatName.none);

        /// <summary>
        /// Adds new page, drives cursor to upper-left corner
        /// </summary>
        /// <param name="name">The name of newly created page.</param>
        /// <param name="mode">In case page with the same name already exists</param>
        /// <returns>Page object - usually not directly used</returns>
        /// \ingroup_disabled renderapi_service
        Object addPage(String name, Boolean scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameThis, reportOutputFormatName format = reportOutputFormatName.none);

        /// <summary>
        /// Subcontents the start.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="cleanPriorContent">if set to <c>true</c> [clean prior content].</param>
        void SubcontentStart(templateFieldSubcontent key, Boolean cleanPriorContent = false);

        /// <summary>
        /// Subcontents the close.
        /// </summary>
        /// <returns></returns>
        String SubcontentClose();

        /// <summary>
        /// Saves the document.
        /// </summary>
        /// <param name="fi">The fi.</param>
        void saveDocument(FileInfo fi);

        /// <summary>
        /// Collection of content units: Source builders => List of string, HTML/XML list of Nodes, RTF/PDF... list of DOM elements
        /// </summary>
        IList content { get; }

        /// <summary>
        /// Builder cursor object
        /// </summary>
        cursor c { get; }

        /// <summary>
        /// Cursor zone
        /// </summary>
        cursorZone zone { get; }

        builderSettings settings { get; }

        /// <summary>
        /// Prefix koji se dodaje ispred svake linije
        /// </summary>
        String linePrefix { get; set; }

        /// <summary>
        /// Vraca sadrzaj u String obliku
        /// </summary>
        /// <returns></returns>
        String ContentToString(Boolean doFlush = false, reportOutputFormatName format = reportOutputFormatName.none);

        /// <summary>
        /// Zatvara sve tagove koji su trenutno otvoreni
        /// </summary>
        void closeAll();

        /// <summary>
        /// Gets the current directory from the context
        /// </summary>
        /// <value>
        /// The directory current.
        /// </value>
        DirectoryInfo directoryScope { get; set; }

        // void prepareBuilder();

        /// <summary>
        /// Saves the current document, returns <c>FileInfo</c> pointing to it
        /// </summary>
        /// <param name="name">The name without extension</param>
        /// <param name="mode">Existing file mode</param>
        /// <returns>File info pointing to</returns>
        FileInfo saveDocument(String name, getWritableFileMode mode, reportOutputFormatName format = reportOutputFormatName.none);

        /// <summary>
        /// Loads the document from filepath into internal object of TDoc type as current document
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="name">Name of loaded document or if <c>importPage</c> pages to import</param>
        /// <returns>
        /// FileInfo pointing to the loaded document
        /// </returns>
        /// \ingroup_disabled renderapi_service
        FileInfo loadDocument(String filepath, String name = "", reportOutputFormatName format = reportOutputFormatName.none);

        /// <summary>
        /// Loads the page from filepath. If it is document type then imports page with targeted name
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Object loadPage(String filepath, String name = "", reportOutputFormatName format = reportOutputFormatName.none);

        /// <summary>
        /// Gets the document instance - current document
        /// </summary>
        /// <remarks>
        /// Usual application do not require document instance outside of builder class.
        /// But if you need some special thing to do.. here it is
        /// </remarks>
        /// <returns></returns>
        /// \ingroup_disabled renderapi_service
        Object getDocument();

        // String renderMenu(reportLinkCollectionSet menu, Boolean includeMain, reportOutputFormatName format = reportOutputFormatName.none);

        /// <summary>
        /// Gets the content blocks -- the main content is in <see cref="imbSCI.Data.enums.fields.templateFieldSubcontent.main"/>
        /// </summary>
        /// <param name="includeMain">if set to <c>true</c> [include main].</param>
        /// <returns></returns>
        PropertyCollection getContentBlocks(Boolean includeMain, reportOutputFormatName format = reportOutputFormatName.none);

        void AppendLine();

        //Object getDocument();
    }
}