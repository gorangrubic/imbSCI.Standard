// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbFileException.cs" company="imbVeles" >
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
using imbSCI.Core.files.fileDataStructure;
using imbSCI.Core.files.folders;
using System;

namespace imbSCI.Core.files
{
    /// <summary>
    /// Exception related to this namespace
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class imbFileException : Exception
    {
        public IFileDataStructure relatedDataStructure { get; set; }
        public String relatedPath { get; set; }
        public folderNode relatedFolder { get; set; }

        public imbFileException(String message, Exception ex, folderNode folder = null, String path = null, IFileDataStructure dataStructure = null) : base(message, ex)
        {
            relatedDataStructure = dataStructure;
            relatedPath = path;
            relatedFolder = folder;
        }
    }
}