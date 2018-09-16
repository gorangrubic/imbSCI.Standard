// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileTextLineCollection.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.io;
using imbSCI.Data.enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.files.search
{
    using imbSCI.Core.reporting.render;
    using System.Collections;
    using System.IO;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Collection of lines extracted/found in a file
    /// </summary>
    public class fileTextLineCollection : IEnumerable<KeyValuePair<Int32, String>>
    {
        private Object AddLock = new Object();

        public void Add(Int32 lineNumber, String lineContent, Boolean overwriteLineNumber = false)
        {
            lock (AddLock)
            {
                if (results.ContainsKey(lineNumber))
                {
                    if (overwriteLineNumber)
                    {
                        results.Remove(lineNumber);
                    }
                    results.Add(lineNumber, lineContent);
                }
                else
                {
                    results.Add(lineNumber, lineContent);
                    _CountThreadSafe++;
                }
            }
        }

        private Int32 _CountThreadSafe = 0; // = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public Int32 CountThreadSafe
        {
            get { return _CountThreadSafe; }
        }

        public Int32 Count()
        {
            return results.Count;
        }

        /// <summary>
        /// Returns the list of line numbers on which the lines were found
        /// </summary>
        /// <returns></returns>
        public List<Int32> getLineNumberList()
        {
            return results.Keys.ToList();
        }

        /// <summary>
        /// Returns the list of matched lines
        /// </summary>
        /// <returns></returns>
        public List<String> getLineContentList()
        {
            return results.Values.ToList();
        }

        /// <summary>
        /// Saves the content of the line.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>reference to file just saved</returns>
        public FileInfo saveLineContent(String filepath, getWritableFileMode mode = getWritableFileMode.overwrite)
        {
            FileInfo fi = filepath.getWritableFile(mode);
            saveBase.saveToFile(fi.FullName, getLineContentList());
            return fi;
        }

        /// <summary>
        /// String representation of the matched lines
        /// </summary>
        /// <param name="showNumber">if set to <c>true</c> [show number].</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public String ToString(Boolean showNumber, String format = "{0,8} : {1}")
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<int, string> pair in results)
            {
                if (showNumber)
                {
                    sb.AppendFormat(format, pair.Key, pair.Value);
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendLine(pair.Value);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// To the string.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="showNumber">if set to <c>true</c> [show number].</param>
        /// <param name="format">The format.</param>
        public virtual void ToString(ITextRender output, Boolean showNumber, String format = "{0,8} : {1}")
        {
            foreach (KeyValuePair<int, string> pair in results)
            {
                try
                {
                    if (showNumber)
                    {
                        output.AppendLine(String.Format(format, pair.Key, pair.Value));
                    }
                    else
                    {
                        output.AppendLine(pair.Value);
                    }
                }
                catch (Exception ex)
                {
                    Thread.Sleep(250);
                    output.AppendLine("--- output broken: " + ex.Message);
                    break;
                }
            }
        }

        /// <summary>
        /// String representation of the matched lines with line number shown
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ToString(true);
        }

        public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<int, string>>)results).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<int, string>>)results).GetEnumerator();
        }

        private Dictionary<Int32, String> _results = new Dictionary<Int32, String>();

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<Int32, String> results
        {
            get
            {
                //if (_results == null)_results = new Dictionary<Int32, String>();
                return _results;
            }
            set { _results = value; }
        }
    }
}