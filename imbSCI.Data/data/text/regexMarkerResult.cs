// --------------------------------------------------------------------------------------------------------------------
// <copyright file="regexMarkerResult.cs" company="imbVeles" >
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
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Single entry of regex marker result
    /// </summary>
    public class regexMarkerResult
    {
        public IRegexMarker regexMarker { get; set; }

        /// <summary>
        /// Regex result
        /// </summary>
        /// <value>
        /// The match.
        /// </value>
        public Match match { get; set; }

        /// <summary>
        /// Marker (rule) that created this result
        /// </summary>
        /// <value>
        /// The marker.
        /// </value>
        public Object marker { get; set; }

        /// <summary>
        /// Content wrapped
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public String content { get; set; }


        public List<String> GetGroups()
        {
            List<String> output = new List<string>();

            foreach (Group g in match.Groups)
            {
                //if (match.Groups.Count > 0 && output.Count == 0)
                //{

                //}
                //else
                //{
                
                   output.Add(g.Value);
               //}
                
            }
            return output;
        }

        private Int32 _index = 0;

        /// <summary>
        /// Starting point
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public Int32 index
        {
            get
            {
                if (match != null) return match.Index;
                return _index;
            }
            set
            {
                _index = value;
            }
        }

        private Int32 _length = 0;

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public Int32 length
        {
            get
            {
                if (match != null) return match.Length;
                return _length;
            }
            set
            {
                _length = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="regexMarkerResult"/> class.
        /// </summary>
        /// <param name="__content">The content.</param>
        /// <param name="__index">The index.</param>
        /// <param name="__marker">The marker.</param>
        public regexMarkerResult(String __content, Int32 __index, Object __marker)
        {
            _index = __index;
            length = __content.Length;
            content = __content;
            marker = __marker;
        }

   

        /// <summary>
        /// Initializes a new instance of the <see cref="regexMarkerResult"/> class.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <param name="__marker">The marker.</param>
        public regexMarkerResult(Match m, Object __marker)
        {
            match = m;
            content = m.Value;
            marker = __marker;
        }
    }
}