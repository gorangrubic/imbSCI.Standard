// --------------------------------------------------------------------------------------------------------------------
// <copyright file="directoryOperation.cs" company="imbVeles" >
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
namespace imbSCI.Data.enums
{
    /// <summary>
    /// Operation on directories to be performed
    /// </summary>
    public enum directoryOperation
    {
        none,
        unknown,

        /// <summary>
        /// The select or build directory on a path
        /// </summary>
        selectOrBuild,

        /// <summary>
        /// Compress into ZIP a directory on a path
        /// </summary>
        compress,

        /// <summary>
        /// Delete directory
        /// </summary>
        delete,

        /// <summary>
        /// Selects only if exists
        /// </summary>
        selectIfExists,

        /// <summary>
        /// Select parent if not root
        /// </summary>
        selectParent,

        /// <summary>
        /// Select device/partition root
        /// </summary>
        selectRoot,

        /// <summary>
        /// Select runtime root
        /// </summary>
        selectRuntimeRoot,

        /// <summary>
        /// Copies directory
        /// </summary>
        copy,

        /// <summary>
        /// Rename directory
        /// </summary>
        rename,
    }
}