// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileunit.cs" company="imbVeles" >
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
using imbSCI.Core;
using imbSCI.Core.attributes;
using imbSCI.Core.collection;
using imbSCI.Core.data;
using imbSCI.Core.enums;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.interfaces;
using imbSCI.Core.reporting;
using imbSCI.Data;
using imbSCI.Data.data;
using imbSCI.Data.enums;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.files.unit
{
    using imbSCI.Core.files.search;
    using System.IO;
    using System.Text;
    using imbStringFormats = imbSCI.Core.extensions.text.imbStringFormats;

    /// <summary>
    /// Represents one file on harddrive
    /// </summary>
    /// <seealso cref="changeBindableBase" />
    public class fileunit : changeBindableBase
    {
        /// <summary>
        /// Merges the files with and/or into <c>pathForResult</c> file
        /// </summary>
        /// <param name="pathForResult">The path for result.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="uniqueLines">if set to <c>true</c> [unique lines].</param>
        /// <returns></returns>
        public static fileunit MergeFiles(String pathForResult, IEnumerable<String> filePaths, Boolean uniqueLines)
        {
            List<FileInfo> files = new List<FileInfo>();
            foreach (String pt in filePaths)
            {
                files.Add(new FileInfo(pt));
            }
            return MergeFiles(pathForResult, files, uniqueLines);
        }

        /// <summary>
        /// Merges the files with and/or into <c>pathForResult</c> file
        /// </summary>
        /// <param name="pathForResult">The path for result.</param>
        /// <param name="files">The files.</param>
        /// <param name="uniqueLines">if set to <c>true</c> [unique lines].</param>
        /// <returns></returns>
        public static fileunit MergeFiles(String pathForResult, IEnumerable<FileInfo> files, Boolean uniqueLines)
        {
            fileunit output = new fileunit(pathForResult, true);

            foreach (FileInfo fi in files)
            {
                if (uniqueLines)
                {
                    output.AppendUnique(File.ReadAllLines(fi.FullName));
                }
                else
                {
                    output.Append(File.ReadAllLines(fi.FullName));
                }
            }
            output.Save();

            return output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="fileunit"/> class.
        /// </summary>
        /// <param name="__path">The path.</param>
        public fileunit(String __path, Boolean doPreload = true)
        {
            String dir = Path.GetDirectoryName(__path);
            if (!dir.isNullOrEmptyString()) Directory.CreateDirectory(dir);

            if (!File.Exists(__path))
            {
                _info = __path.getWritableFile();
            }
            else
            {
                _info = new FileInfo(__path);
                preloadContent();
            }

            _path = __path;

            _lastWrite = info.LastWriteTime;
        }

        private DateTime _lastWrite; // = "";

        /// <summary>
        /// last file write time
        /// </summary>
        public DateTime lastWrite
        {
            get
            {
                info.Refresh();
                if (info.LastWriteTime != _lastWrite)
                {
                    InvokeChanged();
                    _lastWrite = info.LastWriteTime;
                }
                return _lastWrite;
            }
        }

        /// <summary>
        /// Gets the text file search operater
        /// </summary>
        /// <param name="useMemMap">if set to <c>true</c> [use memory map].</param>
        /// <returns></returns>
        public fileTextOperater getOperater(bool useMemMap = false)
        {
            var op = new fileTextOperater(info.FullName, useMemMap);

            return op;
        }

        /// <summary>
        /// Gets the content as single string
        /// </summary>
        /// <returns></returns>
        public String getContent(Boolean ignoreCache = false)
        {
            if (ignoreCache)
            {
                return File.ReadAllText(info.FullName);
            }
            if (!_contentLines.Any()) preloadContent();
            if (HasChanges)
            {
                preloadContent();
            }

            StringBuilder sb = new StringBuilder();
            contentLines.ForEach(x => sb.AppendLine(x));

            return sb.ToString();
        }

        /// <summary>
        /// Gets the content lines.
        /// </summary>
        /// <returns></returns>
        public List<String> getContentLines(Boolean ignoreCache = false)
        {
            if (ignoreCache)
            {
                List<String> output = new List<string>();
                output.AddRange(File.ReadAllLines(info.FullName));
                return output;
            }
            if (!_contentLines.Any())
            {
                preloadContent();
            }

            if (HasChanges)
            {
                preloadContent();
            }
            return contentLines.ToList(); //.Clone();
        }

        /// <summary>
        /// Sets the content - overwriting any existing
        /// </summary>
        /// <param name="content">The content.</param>
        public void setContent(String content)
        {
            if (!content.isNullOrEmptyString())
            {
                File.WriteAllText(info.FullName, content);
                InvokeChanged();
            }
        }

        /// <summary>
        /// Sets the content lines - overwriting any existing
        /// </summary>
        /// <param name="input">The input.</param>
        public void setContentLines(IList<String> input)
        {
            if (input.Any())
            {
                File.WriteAllLines(info.FullName, input.ToList());
                InvokeChanged();
            }
        }

        private Boolean _contentChanged; // = "";

                                         /// <summary>
                                         /// Gets TRUE if the content was changed after the last load
                                         /// </summary>
        public Boolean contentChanged
        {
            get { return _contentChanged; }
        }

        public void Append(String line, Boolean callSave = false)
        {
            contentLines.Add(line);
            _contentChanged = true;
            if (callSave)
            {
                Save();
            }
        }

        public void Append(IEnumerable<String> line, Boolean callSave = false)
        {
            contentLines.AddRange(line);
            _contentChanged = true;
            if (callSave)
            {
                Save();
            }
        }

        public void AppendUnique(IEnumerable<String> line, Boolean callSave = false)
        {
            foreach (var l in line) contentLines.AddUnique(l);
            //  contentLines.AddRangeUnique(line);
            _contentChanged = true;
            if (callSave)
            {
                Save();
            }
        }

        /// <summary>
        /// Saves to new path
        /// </summary>
        /// <param name="newPath">The new path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="loger">The loger.</param>
        public void SaveAs(String newPath, getWritableFileMode mode, ILogBuilder loger)
        {
            info = newPath.getWritableFile(mode);
            Save(loger);
        }

        public void Save(ILogBuilder loger = null)
        {
            setContentLines(contentLines);

            if (loger != null)
            {
                if (!info.Exists)
                {
                    loger.log("File [" + path + "] not saved since the content is empty.");
                }
                else
                {
                    loger.log("File [" + path + "] saved. Size: " + imbStringFormats.getKByteCountFormated(getByteSize()) + " - lines: " + contentLines.Count());
                }
            }
            Accept();
        }

        /// <summary>
        /// Gets the size of the file in the bytes;
        /// </summary>
        /// <returns></returns>
        public long getByteSize()
        {
            info.Refresh();
            if (!info.Exists) return 1;
            return info.Length;
        }

        /// <summary>
        /// Gets the line count.
        /// </summary>
        /// <returns></returns>
        public long getLineCount()
        {
            if (!_contentLines.Any()) preloadContent();
            if (HasChanges)
            {
                preloadContent();
            }
            return contentLines.Count();
        }

        /// <summary>
        /// Preloads the content.
        /// </summary>
        protected void preloadContent()
        {
            if (contentChanged)
            {
                _contentChanged = false;
                Save();
            }
            else
            {
                contentLines = new List<string>();
                info.Refresh();
                if (info.Exists) contentLines.AddRange(File.ReadAllLines(info.FullName));
            }
            Accept();
        }

        private List<String> _contentLines = new List<string>();

        /// <summary>
        ///
        /// </summary>
        public List<String> contentLines
        {
            get
            {
                return _contentLines;
            }
            protected set { _contentLines = value; }
        }

        //public void S

        private String _path;

        /// <summary>
        /// The path of the file
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public String path
        {
            get
            {
                return _path;
            }
            protected set
            {
                _path = value;
                OnPropertyChanged("path");
            }
        }

        private FileInfo _info;

        /// <summary> </summary>
        public FileInfo info
        {
            get
            {
                return _info;
            }
            protected set
            {
                _info = value;
                OnPropertyChanged("info");
            }
        }
    }
}