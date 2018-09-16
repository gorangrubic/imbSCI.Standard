// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileTextSearchResultSet.cs" company="imbVeles" >
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
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.files.search
{
    using System.Collections;

    /// <summary>
    /// Set of results for IEnumerable query
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{imbACE.Core.files.search.fileTextSearchResult}" />
    public class fileTextSearchResultSet : IEnumerable<fileTextSearchResult>
    {
        protected fileTextSearchResultSet()
        {
        }

        public fileTextSearchResultSet(IEnumerable<String> __needleSet, String __filepath, Boolean __useRegex)
        {
            foreach (String needle in __needleSet)
            {
                if (!resultSet.ContainsKey(needle))
                {
                    resultSet.Add(needle, new fileTextSearchResult(needle, __filepath, __useRegex));
                }
            }
        }

        public virtual fileTextSearchResult this[String needle]
        {
            get
            {
                return resultSet[needle];
            }
        }

        private Dictionary<String, fileTextSearchResult> _resultSet = new Dictionary<String, fileTextSearchResult>();

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<String, fileTextSearchResult> resultSet
        {
            get
            {
                //if (_resultSet == null)_resultSet = new Dictionary<String, fileTextSearchResult>();
                return _resultSet;
            }
            set { _resultSet = value; }
        }

        private Object CountThreadLock = new Object();

        /// <summary>
        /// Gets the count thread safe.
        /// </summary>
        /// <value>
        /// The count thread safe.
        /// </value>
        public int CountThreadSafe
        {
            get
            {
                Int32 c = 0;

                lock (CountThreadLock)
                {
                    foreach (var ci in resultSet)
                    {
                        c += ci.Value.Count();
                    }
                }

                return c;
            }
        }

        public bool resultLimitTriggered { get; internal set; }

        public List<String> getNeedles()
        {
            return resultSet.Keys.ToList();
        }

        public List<fileTextSearchResult> getResults()
        {
            return resultSet.Values.ToList();
        }

        /// <summary>
        /// Gets the distinct lines
        /// </summary>
        /// <param name="distinct">if set to <c>true</c> it will return only distinct lines</param>
        /// <returns>List of resulting lines</returns>
        public List<String> getLines(Boolean distinct = true)
        {
            List<String> output = new List<string>();

            foreach (fileTextSearchResult res in this)
            {
                foreach (String ln in res.getLineContentList())
                {
                    if (distinct)
                    {
                        if (!output.Contains(ln))
                        {
                            output.Add(ln);
                        }
                    }
                    else
                    {
                        output.Add(ln);
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Gets the line numbers.
        /// </summary>
        /// <param name="distinct">if set to <c>true</c> [distinct].</param>
        /// <returns></returns>
        public List<Int32> getLineNumbers(Boolean distinct = true)
        {
            List<Int32> output = new List<Int32>();

            foreach (fileTextSearchResult res in this)
            {
                foreach (Int32 ln in res.getLineNumberList())
                {
                    if (distinct)
                    {
                        if (!output.Contains(ln))
                        {
                            output.Add(ln);
                        }
                    }
                    else
                    {
                        output.Add(ln);
                    }
                }
            }
            return output;
        }

        public IEnumerator<fileTextSearchResult> GetEnumerator()
        {
            return resultSet.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return resultSet.Values.GetEnumerator();
        }
    }
}