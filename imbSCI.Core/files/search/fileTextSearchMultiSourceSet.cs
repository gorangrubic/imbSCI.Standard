// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileTextSearchMultiSourceSet.cs" company="imbVeles" >
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

namespace imbSCI.Core.files.search
{
    using System.Collections;

    public class fileTextSearchMultiSourceSet : IEnumerable<KeyValuePair<String, fileTextSearchResultSet>>
    {
        public fileTextSearchMultiSourceSet(IEnumerable<String> __needleSet, List<String> __filepaths, Boolean __useRegex)
        {
            foreach (String filepath in __filepaths)
            {
                items.Add(filepath, new fileTextSearchResultSet(__needleSet, filepath, __useRegex));
            }
        }

        public fileTextSearchResultSet this[String filepath]
        {
            get
            {
                return items[filepath];
            }

            set
            {
                items[filepath] = value;
            }
        }

        private Dictionary<String, fileTextSearchResultSet> _items = new Dictionary<String, fileTextSearchResultSet>();

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<String, fileTextSearchResultSet> items
        {
            get
            {
                //if (_items == null)_items = new Dictionary<String, fileTextSearchResultSet>();
                return _items;
            }
            set { _items = value; }
        }

        public IEnumerator<KeyValuePair<string, fileTextSearchResultSet>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, fileTextSearchResultSet>>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, fileTextSearchResultSet>>)items).GetEnumerator();
        }
    }
}