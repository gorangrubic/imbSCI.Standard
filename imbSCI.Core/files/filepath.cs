// --------------------------------------------------------------------------------------------------------------------
// <copyright file="filepath.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.data;
using imbSCI.Data;
using System;

namespace imbSCI.Core.files
{
    using imbSCI.Core.reporting.extensions;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Information about filepath
    /// </summary>
    public class filepath
    {
        public filepath(String path = "")
        {
            setup(path);
        }

        public String toPath(String rootpath = "")
        {
            String output = rootpath;

            output = output.add(directoryPath.add(filename.add(extension, "."), "\\"), "\\");
            return output;
        }

        public String toPathWithExtension(String rootpath = "", String customExtension = "")
        {
            String output = rootpath;
            if (customExtension.isNullOrEmpty()) customExtension = extension;

            output = output.add(directoryPath.add(filename.add(customExtension, "."), "\\"), "\\");
            return output;
        }

        /// <summary>
        /// Returns the path with data applied
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public String toPath(String rootPath, PropertyCollection data)
        {
            String output = ""; // directoryPath.ensureStartsWith(rootPath);

            output = output.add(directoryPath.add(filename.add(extension, "."), "\\"), "\\");
            output = output.applyToContent(false, data);

            output = output.ensureStartsWith(imbSciStringExtensions.ensureEndsWith(rootPath, "\\"));

            return output;
        }

        public void setup(String path)
        {
            if (path.isNullOrEmpty())
            {
                return;
            }

            isRooted = Path.IsPathRooted(path);

            filename = Path.GetFileNameWithoutExtension(path);
            extension = Path.GetExtension(path);
            directoryPath = Path.GetDirectoryName(path);
        }

        private Boolean _isRooted;

        /// <summary>
        ///
        /// </summary>
        public Boolean isRooted
        {
            get { return _isRooted; }
            protected set { _isRooted = value; }
        }

        private filepathflags _flags;

        /// <summary>
        ///
        /// </summary>
        public filepathflags flags
        {
            get { return _flags; }
            protected set { _flags = value; }
        }

        private String _filename;

        /// <summary>
        ///
        /// </summary>
        public String filename
        {
            get { return _filename; }
            protected set { _filename = value; }
        }

        public String filenameWithExtension
        {
            get
            {
                return filename.add(extension, ".");
            }
        }

        private String _extension;

        /// <summary>
        ///
        /// </summary>
        public String extension
        {
            get { return _extension; }
            protected set { _extension = value; }
        }

        private String _directoryPath;

        /// <summary>
        ///
        /// </summary>
        public String directoryPath
        {
            get { return _directoryPath; }
            protected set { _directoryPath = value; }
        }
    }
}