// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureBase.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.measureSystem.core
{
    using imbSCI.Data;
    using System;

    /// <summary>
    ///
    /// </summary>
    public abstract class measureBase
    {
        private String _name = "";

        /// <summary>
        ///
        /// </summary>
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        private String _description = "";

        /// <summary>
        ///
        /// </summary>
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        private String _metaModelName;

        /// <summary>
        ///
        /// </summary>
        public String metaModelName
        {
            get { return _metaModelName; }
            set { _metaModelName = value; }
        }

        private String _metaModelPrefix;

        /// <summary>
        ///
        /// </summary>
        public String metaModelPrefix
        {
            get { return _metaModelPrefix; }
            set { _metaModelPrefix = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String metaModelFull
        {
            get
            {
                return imbSciStringExtensions.add(metaModelPrefix, metaModelName, "_");
            }
            set
            {
                var pths = value.SplitSmart("."); //.getPathParts();
                if (pths.Count > 1)
                {
                    _metaModelPrefix = pths[0];
                    _metaModelName = pths[1];
                }
                else if (pths.Count > 0)
                {
                    _metaModelName = pths[0];
                }
            }
        }
    }
}