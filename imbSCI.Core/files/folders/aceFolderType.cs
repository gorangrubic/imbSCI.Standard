// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceFolderType.cs" company="imbVeles" >
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
namespace imbSCI.Core.files.folders
{
    public enum aceFolderType
    {
        /// <summary>
        /// configuration files, external media resources
        /// </summary>
        etc,

        /// <summary>
        /// logs, crash info, backup files
        /// </summary>
        var,

        /// <summary>
        /// script/job/task presets - categorised by folder structure
        /// </summary>
        lib,

        /// <summary>
        /// contins external tools: text editor, html viewer, image viewer etc.
        /// </summary>
        bin,

        /// <summary>
        /// user projects/jobs
        /// </summary>
        usr,

        /// <summary>
        /// temporary files that are safe to be deleted
        /// </summary>
        tmp,

        /// <summary>
        /// root folder
        /// </summary>
        root,

        /// <summary>
        /// folder sa fajlovima koji treba da se obrade
        /// </summary>
        source,

        /// <summary>
        /// folder sa rezultatom - obradjeni
        /// </summary>
        result,

        /// <summary>
        /// folder sa neobradjenim - jer ne ispunjavaju uslov
        /// </summary>
        notChanged
    }
}