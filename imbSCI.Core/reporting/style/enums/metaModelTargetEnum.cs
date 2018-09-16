// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaModelTargetEnum.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.style.enums
{
    /// <summary>
    /// Enumerates possible targets of Apply/Append/Compose/Construct calls
    /// </summary>
    /// <remarks>
    /// Primarly created for ApplyStyle() method of <c>IDocumentRender</c> implementations
    /// </remarks>
    public enum metaModelTargetEnum
    {
        none,

        /// <summary>
        /// Targets whatever is predefined as default target on level of <c>IRender</c> implementation class
        /// </summary>
        defaultTarget,

        /// <summary>
        /// The current document
        /// </summary>
        document,

        /// <summary>
        /// The current page
        /// </summary>
        page,

        /// <summary>
        /// The current scope
        /// </summary>
        scope,

        /// <summary>
        /// The parent of current scope
        /// </summary>
        scopeParent,

        /// <summary>
        /// Each child of the current scope
        /// </summary>
        scopeEachChild,

        /// <summary>
        /// a child element of the current scope, needs key/id param
        /// </summary>
        scopeChild,

        /// <summary>
        /// Any metaContent element existing on a path, relative to scope. Supports apsolute paths too
        /// </summary>
        scopeRelativePath,

        /// <summary>
        /// Area pointed by <c>cursor.pencil</c>
        /// </summary>

        pencil,

        /// <summary>
        /// The last append line/segment/section/node/cell
        /// </summary>
        lastAppend,

        /// <summary>
        /// It will be applied after next append call
        /// </summary>
        nextAppend,

        /// <summary>
        /// Set it to standard (for this <c>render</c> instance
        /// </summary>
        setStandard,

        /// <summary>
        /// Unset the standard config of this <c>render</c> instance
        /// </summary>
        unsetStandard,

        /// <summary>
        /// The set named standard - defines a preset on <c>styleTheme</c> level, globally accessable by any <c>render</c>
        /// </summary>
        setNamedStandard,

        /// <summary>
        /// The deletes a named standard defined as preset on <c>styleTheme</c> level
        /// </summary>
        unsetNamedStandard,

        /// <summary>
        /// Appends content representing configuration/settings/data used by targeting.
        /// </summary>
        /// <remarks>
        /// Example: in case of ApplyStyle() - it will Append description of style into content
        /// </remarks>
        asAppend,
    }
}