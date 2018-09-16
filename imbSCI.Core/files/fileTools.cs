// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileTools.cs" company="imbVeles" >
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
using imbSCI.Core.files.folders;
using System.IO;

namespace imbSCI.Core.files
{
    /// <summary>
    /// Some easy file tools
    /// </summary>
    public static class fileTools
    {
        /// <summary>
        /// Deletes the folder.
        /// </summary>
        /// <param name="di">The di.</param>
        public static void DeleteFolder(this folderNode di)
        {
            DirectoryInfo dir = di;
            dir.DeleteFolder();
        }

        /// <summary>
        /// Deletes the folder, including all files inside
        /// </summary>
        /// <param name="di">The di.</param>
        public static void DeleteFolder(this DirectoryInfo di)
        {
            foreach (FileInfo fi in di.GetFiles("*.*", SearchOption.AllDirectories))
            {
                fi.Delete();
            }

            foreach (DirectoryInfo dsi in di.GetDirectories("*", SearchOption.AllDirectories))
            {
                dsi.DeleteFolder();
            }
            di.Delete();
        }
    }
}