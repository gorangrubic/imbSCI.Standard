// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataItemContainer.cs" company="imbVeles" >
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
using System;

namespace imbSCI.Data.data.package
{
    /// <summary>
    /// Stored XML content, paired with ID
    /// </summary>
    public class dataItemContainer
    {
        /// <summary>
        /// Used by XmlSerialization
        /// </summary>
        public dataItemContainer()
        {
        }

        /// <summary>
        /// ID associated to the stored instance
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public String id { get; set; }

        /// <summary>
        /// XML code of stored instance
        /// </summary>
        /// <value>
        /// The instance XML.
        /// </value>
        public String instanceXml { get; set; }
    }
}