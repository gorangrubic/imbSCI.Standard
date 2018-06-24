// --------------------------------------------------------------------------------------------------------------------
// <copyright file="templateFieldSubcontent.cs" company="imbVeles" >
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
    /// HTML blocks
    /// </summary>
    public enum templateFieldSubcontent
    {
        ///// <summary>
        ///// The none - points to nothing
        ///// </summary>
        //none,

        /// <summary>
        /// The main StringBuilder or content
        /// </summary>
        main,

        /// <summary>
        /// The start of file - the very first lines of the content
        /// </summary>
        startOfFile,

        /// <summary>
        /// The end of file - the very last lines of the content
        /// </summary>
        endOfFile,

        /// <summary>
        /// Page keywords
        /// </summary>
        meta_keywords,

        /// <summary>
        /// Page description
        /// </summary>
        meta_description,

        /// <summary>
        /// Place for heading style links
        /// </summary>
        theme_styles,

        /// <summary>
        /// The theme code headlinks - code links to be included at beginning
        /// </summary>
        theme_code_headlinks,

        /// <summary>
        /// The theme code footlinks - code links to be included at end
        /// </summary>
        theme_code_footlinks,

        /// <summary>
        /// JS (or other) code to include around end or at end of the template
        /// </summary>
        theme_code_ending,

        /// <summary>
        /// Title for HTML title
        /// </summary>
        html_title,

        /// <summary>
        /// Top navigation
        /// </summary>
        html_topnav,

        html_toolbar,

        /// <summary>
        /// Main navigation
        /// </summary>
        html_mainnav,

        /// <summary>
        /// Header content
        /// </summary>
        html_header,

        /// <summary>
        /// Footer content
        /// </summary>
        html_footer,

        /// <summary>
        /// Debug information
        /// </summary>
        html_debug,

        /// <summary>
        /// Main content of the page
        /// </summary>
        html_page_content,

        /// <summary>
        /// Java script to be inserted at end of page
        /// </summary>
        html_js,

        // html_headtag,
        head_includes,

        bottom_includes,
        none,
    }
}