// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISupportLoadSave.cs" company="imbVeles" >
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
using imbSCI.Data.enums;
using System;

namespace imbSCI.Data.interfaces
{
    /// <summary>
    /// Objects that support Load and Save
    /// </summary>
    public interface ISupportLoadSave : IObjectWithName
    {
        /// <summary>
        /// Loads from specified absolute path
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        Boolean LoadFrom(String path);

        /// <summary>
        /// Saves to specified absolute path
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        Boolean SaveAs(String path, getWritableFileMode mode = getWritableFileMode.newOrExisting);
    }
}