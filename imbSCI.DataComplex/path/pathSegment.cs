// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pathSegment.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.path
{
    #region imbVeles using

    using imbSCI.Data;

    //using aceCommonTypes.extensions;

    #endregion imbVeles using

    /// <summary>
    /// Jedan path segment
    /// </summary>
    public struct pathSegment
    {
        private pathElementFormat _elementPrefix;

        public string description
        {
            get
            {
                return "[" + position + "]=> prefix[" + prefix + "][" + elementPrefix.cleanName +
                       "] needle[" + needle + "]";
            }
        }

        /// <summary>
        /// Pozicija u pathSegments kolekciji
        /// </summary>
        public int position { get; set; }

        /// <summary>
        /// needle deo path segmenta - - bez element format prefix-a
        /// </summary>
        public string needle { get; set; }

        /// <summary>
        /// prefix deo path segmenta
        /// </summary>
        public string prefix { get; set; }

        /// <summary>
        /// Da li je segment prihvatljiv za primenu
        /// </summary>
        public bool isDefined
        {
            get { return !prefix.isNullOrEmptyString() || !needle.isNullOrEmptyString(); }
        }

        public bool isPrefixSupported
        {
            get
            {
                if (prefix.isNullOrEmptyString()) return false;

                return resourcePathResolver.prefixVsFormat.ContainsKey(prefix);
            }
        }

        /// <summary>
        /// Detektovan format koji je primenjen u segmentu
        /// </summary>
        public pathElementFormat elementPrefix
        {
            get
            {
                if (_elementPrefix == null)
                {
                    if (!prefix.isNullOrEmptyString())
                    {
                        _elementPrefix = resourcePathResolver.prefixVsFormat[prefix];
                    }
                }
                return _elementPrefix;
            }
            set { _elementPrefix = value; }
        }

        /// <summary>
        /// Pravi path segment
        /// </summary>
        /// <param name="__needle"></param>
        /// <param name="__prefix"></param>
        /// <param name="__position"></param>
        /// <returns></returns>
        public static pathSegment create(string __needle, string __prefix, int __position)
        {
            pathSegment pseg = new pathSegment();
            pseg.prefix = __prefix;
            pseg.needle = pseg.elementPrefix.extractValue(__needle);
            pseg.position = __position;
            return pseg;
        }
    }
}