// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportLinkRegistry.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.links
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Global registry of links
    /// </summary>
    public class reportLinkRegistry : IEnumerable
    {
        public static string getCleanKey(string key)
        {
            if (key.isNullOrEmpty()) key = "-";
            string output = key.Replace("/", "-");
            output = output.Replace("\\", "-");

            output = output.Replace(".", "-");
            output = output.Replace(" ", "");
            return output;
        }

        /// <summary>
        /// Gets the link collection.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public reportLinkCollection getLinkCollection(string key)
        {
            string ikey = getCleanKey(key);

            if (!items.ContainsKey(ikey))
            {
                items.Add(ikey, new reportLinkCollection());
            }

            return items[ikey];
        }

        public reportLinkCollection getLinkOneCollection(params string[] keys)
        {
            foreach (string key in keys)
            {
                string ikey = getCleanKey(key);
                if (!ikey.isNullOrEmpty())
                {
                    if (items.ContainsKey(ikey))
                    {
                        return getLinkCollection(ikey);
                    }
                }
            }
            return null;
        }

        public List<reportLinkCollection> getLinkCollections(params string[] keys)
        {
            List<reportLinkCollection> output = new List<reportLinkCollection>();

            foreach (string key in keys)
            {
                string ikey = getCleanKey(key);
                if (!ikey.isNullOrEmpty())
                {
                    if (items.ContainsKey(ikey))
                    {
                        output.Add(getLinkCollection(ikey));
                    }
                }
            }
            return output;
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }

        /// <summary>
        /// Gets the <see cref="reportLinkCollection"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="reportLinkCollection"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public reportLinkCollection this[string key]
        {
            get
            {
                return getLinkCollection(key);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, reportLinkCollection> items { get; set; } = new Dictionary<string, reportLinkCollection>();

        /// <summary>
        /// Initializes a new instance of the <see cref="reportLinkRegistry"/> class.
        /// </summary>
        public reportLinkRegistry()
        {
        }
    }
}