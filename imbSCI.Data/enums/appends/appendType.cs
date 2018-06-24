// --------------------------------------------------------------------------------------------------------------------
// <copyright file="appendType.cs" company="imbVeles" >
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
namespace imbSCI.Data.enums.appends
{
    /// <summary>
    /// Instruction set for IDocumentRender, ITextRender and IRender report rendering engines
    /// </summary>
    public enum appendType
    {
        none,

        /// <summary>
        /// same as none but regular will not trigger secondary type checks like none does
        /// </summary>
        regular,

        /// <summary>
        /// do not escape
        /// </summary>
        bypass,

        italic,
        bold,
        monospace,
        striketrough,

        heading,
        heading_1,
        heading_2,
        heading_3,
        heading_4,
        heading_5,
        heading_6,

        blockquote,
        source,
        sourceJS,
        sourcePY,

        subscript,
        superscript,
        marked,

        squareQuote,
        math,
        quotation,

        section,
        paragraph,
        footnote,
        sourceXML,
        sourceCS,
        comment,

        direct,
        toFile,
        fromFile,
        file,

        /// <summary>
        /// From file templated: file source path and delivery unit item
        /// </summary>
        //fromFileTemplated,
        //diagram,
        image,

        label,
        panel,
        frame,
        placeholder,
        list,

        //open,
        //close,

        /// <summary>
        /// The c table: Table rendering
        /// </summary>
        c_table,

        /// <summary>
        /// The c data: PropertyCollection pairs
        /// </summary>
        c_data,

        /// <summary>
        /// The c pair: key, value, between
        /// </summary>
        c_pair,

        /// <summary>
        /// The c section: title, content, footer
        /// </summary>
        c_section,

        /// <summary>
        /// The c link> link - url
        /// </summary>
        c_link,

        /// <summary>
        /// The c line> horizontal line
        /// </summary>
        c_line,

        /// <summary>
        /// The c open - opens container
        /// </summary>
        c_open,

        /// <summary>
        /// The c close - closes container
        /// </summary>
        c_close,

        /// <summary>
        /// The i palette - switches to pallete
        /// </summary>
        s_palette,

        /// <summary>
        /// S - alternating Odd and Even on Normal style
        /// </summary>
        s_alternate,

        /// <summary>
        /// The s normal - turns off the alternating mode
        /// </summary>
        s_normal,

        /// <summary>
        /// applies zone to cursor: textCursorZone, cursorSubzoneFrame, cursorZoneRole (supported params)
        /// </summary>
        s_zone,

        /// <summary>
        /// directory operation: create, zip, delete, moveInto, moveOut... dsa_dirOperation, dsa_scopeToNew, dsa_path
        /// </summary>
        x_directory,

        /// <summary>
        /// The x scope in: set current scope to provided object
        /// </summary>
        x_scopeIn,

        /// <summary>
        /// The x scope out: set current scope to parent
        /// </summary>
        x_scopeOut,

        /// <summary>
        /// Opens external tool
        /// </summary>
        x_openTool,

        /// <summary>
        /// The x save - saving file
        /// </summary>
        x_save,

        /// <summary>
        /// Calls for export of a resource
        /// </summary>
        x_export,

        /// <summary>
        /// Starts/ends a subcontent session
        /// </summary>
        x_subcontent,

        /// <summary>
        /// Calls execution context state data to be refreshed
        /// </summary>
        x_data,

        /// <summary>
        /// Inserts custom PropertyCollection into context data
        /// </summary>
        i_dataSource,

        /// <summary>
        /// Inserts custom document data - if possible
        /// </summary>
        i_dataInDocument,

        /// <summary>
        /// The i document: new document
        /// </summary>
        i_document,

        /// <summary>
        /// The i page: new page
        /// </summary>
        i_page,

        /// <summary>
        /// The i style: new style definition
        /// </summary>
        s_style,

        /// <summary>
        /// The i meta: new meta entry
        /// </summary>
        /// <remarks>
        ///     Used with <see cref="templateFieldBasic"/> <c>meta_</c> values to direct
        /// </remarks>
        i_meta,

        /// <summary>
        /// The i chart: new chart
        /// </summary>
        i_chart,

        /// <summary>
        /// include content from external file.
        /// </summary>
        /// <remarks>
        /// use with> d.dsa_recompile, dsa_innerAppend, dsa_path
        /// </remarks>
        i_external,

        /// <summary>
        /// Loads external file and adds it according to dsa_innerAppend: i_document, i_page or simple append
        /// </summary>
        i_load,

        /// <summary>
        /// The i log:writes to log
        /// </summary>
        i_log,

        /// <summary>
        /// The i function - inserts function into> field / cell ---
        /// </summary>
        i_function,

        /// <summary>
        /// Sets width
        /// </summary>
        s_width,

        /// <summary>
        /// Sets forcebly current variationRole
        /// </summary>
        s_variation,

        x_move,

        /// <summary>
        /// Applies behaviour settings
        /// </summary>
        s_settings,

        attachment,
        button,
        exe,
    }
}