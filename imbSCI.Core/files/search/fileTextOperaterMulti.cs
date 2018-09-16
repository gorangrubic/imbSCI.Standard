// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileTextOperaterMulti.cs" company="imbVeles" >
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

    /// <summary>
    /// Collection of <see cref="fileTextOperater"/> instances, used when multiple files should be searched
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.String, imbSCI.Core.files.search.fileTextOperater}}" />
    public class fileTextOperaterMulti : IEnumerable<KeyValuePair<String, fileTextOperater>>
    {
        /// <summary>
        /// Populates it self with <see cref="fileTextOperater"/>s for each file path specified in the <c>filepaths</c> argument
        /// </summary>
        /// <param name="filepaths">The filepaths.</param>
        public fileTextOperaterMulti(IEnumerable<String> filepaths)
        {
            foreach (String filepath in filepaths)
            {
                items.Add(filepath, new fileTextOperater(filepath));
            }
        }

        public IEnumerator<KeyValuePair<String, fileTextOperater>> GetEnumerator() => items.GetEnumerator();

        public Int32 Count() => items.Count;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, fileTextOperater>>)_items).GetEnumerator();
        }

        private Dictionary<String, fileTextOperater> _items = new Dictionary<String, fileTextOperater>();

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<String, fileTextOperater> items
        {
            get
            {
                //if (_items == null)_items = new Dictionary<String, fileTextOperater>();
                return _items;
            }
            set { _items = value; }
        }
    }
}