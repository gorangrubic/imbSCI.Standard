// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileDataAttribute.cs" company="imbVeles" >
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
using System;

namespace imbSCI.Core.files.fileDataStructure
{
    [AttributeUsage(AttributeTargets.Property)]
    public class fileDataAttribute : fileDataBaseAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="fileDataAttribute"/> class.
        /// </summary>
        /// <param name="__customName">Custom filename prefix (without extension)</param>
        /// <param name="__formatMode">Mode of serialization</param>
        /// <param name="__options">Special options</param>
        public fileDataAttribute(String __customName,
            fileDataPropertyMode __formatMode = fileDataPropertyMode.autoTextOrXml, fileDataPropertyOptions __options = fileDataPropertyOptions.none)
        {
            nameOrPropertyPath = __customName;
            filenameMode = fileDataFilenameMode.customName;
            formatMode = __formatMode;
            options = __options;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="fileDataAttribute"/> class.
        /// </summary>
        /// <param name="__filename">The filename mode</param>
        /// <param name="__mode">Mode of serialization</param>
        /// <param name="__options">Special options</param>
        public fileDataAttribute(fileDataFilenameMode __filename = fileDataFilenameMode.memberInfoName,
            fileDataPropertyMode __mode = fileDataPropertyMode.autoTextOrXml,
            fileDataPropertyOptions __options = fileDataPropertyOptions.none)
        {
            filenameMode = __filename;
            formatMode = __mode;
            options = __options;
        }
    }
}