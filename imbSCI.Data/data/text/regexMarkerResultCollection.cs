// --------------------------------------------------------------------------------------------------------------------
// <copyright file="regexMarkerResultCollection.cs" company="imbVeles" >
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
namespace imbSCI.Data.data.text
{
    using imbSCI.Data.collection.nested;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Single dimension marked map - the results created with <see cref="regexMarkerCollection{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class regexMarkerResultCollection<T>
    {
        /// <summary>
        /// Gets the marker results by order of appearance -- taking only first layer
        /// </summary>
        /// <returns></returns>
        public List<regexMarkerResult> GetByOrder()
        {
            List<regexMarkerResult> output = new List<regexMarkerResult>();
            Int32 index = 0;

            regexMarkerResult head = null;

            while (index < length)
            {
                if (byAllocation.ContainsKey(index))
                {
                    var tmp = byAllocation[index].First();
                    output.Add(tmp);
                    index = index + tmp.length;
                }
                else
                {
                    index++;
                }
            }
            return output;
        }

        private aceDictionarySet<T, regexMarkerResult> _byMarker = new aceDictionarySet<T, regexMarkerResult>();

        /// <summary> Dictionary set by marker rule</summary>
        public aceDictionarySet<T, regexMarkerResult> byMarker
        {
            get
            {
                return _byMarker;
            }
            protected set
            {
                _byMarker = value;
            }
        }

        private aceDictionarySet<Int32, regexMarkerResult> _byAllocation = new aceDictionarySet<Int32, regexMarkerResult>();

        /// <summary> Indexed by allocation </summary>
        public aceDictionarySet<Int32, regexMarkerResult> byAllocation
        {
            get
            {
                return _byAllocation;
            }
            protected set
            {
                _byAllocation = value;
            }
        }

        internal Int32 AddResult(String rest, Int32 index)
        {
            var restResult = new regexMarkerResult(rest, index, defaultMarker);

            AddResult(restResult);
            return restResult.index + restResult.length;
        }

        /// <summary>
        /// Length of the complete result collection
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public Int32 length { get; set; }

        internal void AddResult(regexMarkerResult res)
        {
            for (Int32 ind = res.index; ind <= res.index + res.length; ind++)
            {
                byAllocation.Add(ind, res);
            }

            byMarker.Add((T)res.marker, res);
        }

        /// <summary>
        /// Default marker to apply
        /// </summary>
        public T defaultMarker = default(T);

        public regexMarkerResultCollection()
        {
        }
    }
}