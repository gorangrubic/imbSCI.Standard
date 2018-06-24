// --------------------------------------------------------------------------------------------------------------------
// <copyright file="getWritableFileMode.cs" company="imbVeles" >
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
    /// Mode of getWritableFile extension
    /// </summary>
    public enum getWritableFileMode
    {
        /// <summary>
        /// The none - the function is disabled
        /// </summary>
        none,

        /// <summary>
        /// The unknown - it will not affect any existing preference
        /// </summary>
        unknown,

        /// <summary>
        /// It will create new file if no existing detected, or get existing
        /// </summary>
        newOrExisting,

        /// <summary>
        /// It will not create new file if no existing detected
        /// </summary>
        existing,

        /// <summary>
        /// It will delete any existing file and provide newly created
        /// </summary>
        overwrite,

        /// <summary>
        /// It will automatically modify path with _Counter sufix so the new file has unique filename
        /// </summary>
        autoRenameThis,

        /// <summary>
        /// The automatic rename the existing file if it was created on another date
        /// </summary>
        autoRenameExistingOnOtherDate,

        /// <summary>
        /// It will rename any existing file with sufix _old
        /// </summary>
        autoRenameExistingToOld,

        /// <summary>
        /// It will rename any existing file with sufix _backup
        /// </summary>
        autoRenameExistingToBack,

        /// <summary>
        /// It will just append existing file - partial implementation
        /// </summary>
        appendFile,

        autoRenameExistingToNextPage,

        /// <summary>
        /// It will allow just one backup of an existing file by adding just "_old" sufix, without making new filename unique
        /// </summary>
        autoRenameExistingToOldOnce,
    }
}