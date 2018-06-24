// --------------------------------------------------------------------------------------------------------------------
// <copyright file="memberParamDescription.cs" company="imbVeles" >
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
using System.Xml.Serialization;

namespace imbSCI.Core.data.metadata
{
    /// <summary>
    /// C# XML documentation content for parameter element
    /// </summary>
    [XmlRoot(ElementName = "param")]
    public class memberParamDescription
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlAttribute(AttributeName = "name")]
        public String name { get; set; } = "";

        /// <summary>
        /// Parameter description content
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [XmlText]
        public String description { get; set; } = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="memberParamDescription"/> class.
        /// </summary>
        public memberParamDescription()
        {
        }
    }
}