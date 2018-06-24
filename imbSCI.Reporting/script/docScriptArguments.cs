// --------------------------------------------------------------------------------------------------------------------
// <copyright file="docScriptArguments.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.script
{
    /// <summary>
    /// Arguments applicable with docScriptInstruction
    /// </summary>
    public enum docScriptArguments
    {
        /// <summary>
        /// The DSA content lines: IEnumerable of String or other simple type
        /// </summary>
        dsa_content,

        /// <summary>
        /// The DSA content line: String
        /// </summary>
        dsa_contentLine,

        /// <summary>
        /// Calls for template recompilation of <c>content</c> and <c>contentLine</c>
        /// </summary>
        dsa_recompile,

        /// <summary>
        /// AppendType to use for internal append procedure *special*
        /// </summary>
        dsa_innerAppend,

        /// <summary>
        /// The DSA title: String for title
        /// </summary>
        dsa_title,

        /// <summary>
        /// The DSA footer: Footer message
        /// </summary>
        dsa_footer,

        /// <summary>
        /// The DSA key: key for KeyValue pair
        /// </summary>
        dsa_key,

        /// <summary>
        /// The DSA value: value for KeyValue pair
        /// </summary>
        dsa_value,

        /// <summary>
        /// The DSA URL: URL for link
        /// </summary>
        dsa_url,

        /// <summary>
        /// The DSA name: name is used by many Appends
        /// </summary>
        dsa_name,

        /// <summary>
        /// The DSA level: heading level
        /// </summary>
        dsa_level,

        /// <summary>
        /// The DSA x: X position
        /// </summary>
        dsa_x,

        /// <summary>
        /// The DSA y: Y position
        /// </summary>
        dsa_y,

        /// <summary>
        /// The DSA w: width
        /// </summary>
        dsa_w,

        /// <summary>
        /// The DSA h: height
        /// </summary>
        dsa_h,

        /// <summary>
        /// The DSA priority: for pages and documents
        /// </summary>
        dsa_priority,

        /// <summary>
        /// The DSA data table: DataTable object
        /// </summary>
        dsa_dataTable,

        /// <summary>
        /// The DSA data pairs: PropertyCollection
        /// </summary>
        dsa_dataPairs,

        /// <summary>
        /// The DSA palette role: palette selector
        /// </summary>
        dsa_paletteRole,

        /// <summary>
        /// The DSA path: path to external file etc
        /// </summary>
        dsa_path,

        /// <summary>
        /// The DSA type: appendType
        /// </summary>
        dsa_type,

        /// <summary>
        /// The DSA is horizontal: horizontal or pivot deployement
        /// </summary>
        dsa_isHorizontal,

        /// <summary>
        /// The DSA data field - data field enum value to access data from or to
        /// </summary>
        dsa_dataField,

        /// <summary>
        /// The DSA enum type - reference to enumeration type
        /// </summary>
        dsa_enumType,

        /// <summary>
        /// The DSA border preset - border to apply on closed tag
        /// </summary>
        dsa_border_preset,

        /// <summary>
        /// The DSA data source - what source of dynamic data to target
        /// </summary>
        dsa_dataSource,

        /// <summary>
        /// What directory operation to perform
        /// </summary>
        dsa_dirOperation,

        /// <summary>
        /// The DSA cursor corner - to what corner cursor should go
        /// </summary>
        dsa_cursorCorner,

        /// <summary>
        /// The DSA external tool - external tool to call
        /// </summary>
        dsa_externalTool,

        /// <summary>
        /// Target zone subframe
        /// </summary>
        dsa_zoneFrame,

        /// <summary>
        /// Description text
        /// </summary>
        dsa_description,

        /// <summary>
        /// Width of the block
        /// </summary>
        dsa_blockWidth,

        /// <summary>
        /// Horizontal position on page
        /// </summary>
        dsa_blockPosition,

        /// <summary>
        /// Type of link - can be: metaLinkType and metaRelativeLinkType
        /// </summary>
        dsa_linkType,

        /// <summary>
        /// Type of relative link
        /// </summary>
        dsa_linkRelType,

        /// <summary>
        /// Reference to metaContent building block
        /// </summary>
        dsa_metaContent,

        /// <summary>
        /// Class attribute to set in HTML/XML export
        /// </summary>
        dsa_class_attribute,

        /// <summary>
        /// ID attribute to set in HTML/XML export
        /// </summary>
        dsa_id_attribute,

        /// <summary>
        /// Content of middle cell in pairs rendering
        /// </summary>
        dsa_separator,

        /// <summary>
        /// The DSA variation role
        /// </summary>
        dsa_variationRole,

        dsa_dataList,

        /// <summary>
        /// Scope will move to newly created item: directorium, document, page...
        /// </summary>
        dsa_scopeToNew,

        /// <summary>
        /// If TRUE it will merge cells scoped by current <c>dsa_w</c> and <c>dsa_h</c>
        /// </summary>
        dsa_doMerge,

        dsa_stylerSettings,

        /// <summary>
        /// Instructs styling behaviour
        /// </summary>
        dsa_autostyling,

        /// <summary>
        /// Points to item that should be styled
        /// </summary>
        dsa_styleTarget,

        dsa_format,

        dsa_itemExistsMode,
        dsa_vector,
        dsa_relative,
        dsa_forecolor,
        dsa_background,
        dsa_role,

        /// <summary>
        /// Universal <c>on</c> flag. Also used as <c>open</c>, <c>start</c>, <c>play</c> too.
        /// </summary>
        dsa_on,

        /// <summary>
        /// Universal <c>off</c> flag. Used as <c>close</c>, <c>finish</c>, <c>stop</c> too.
        /// </summary>
        dsa_off,
    }
}