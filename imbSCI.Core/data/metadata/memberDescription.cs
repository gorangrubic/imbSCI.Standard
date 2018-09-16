// --------------------------------------------------------------------------------------------------------------------
// <copyright file="memberDescription.cs" company="imbVeles" >
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
using System.Xml.Serialization;

namespace imbSCI.Core.data.metadata
{
    /// <summary>
    /// C# XML documentation content, unserialized from XML file
    /// </summary>
    [XmlRoot(ElementName = "member")]
    public class memberDescription
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
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        public String summary { get; set; } = "";

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        /// <value>
        /// The remarks.
        /// </value>
        public String remarks { get; set; } = "";

        /// <summary>
        /// Gets or sets the returns.
        /// </summary>
        /// <value>
        /// The returns.
        /// </value>
        public String returns { get; set; } = "";

        /// <summary>
        /// Content in the the value tag
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public String value { get; set; } = "";

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        [XmlElement(typeof(memberParamDescription), ElementName = "param")]
        public List<memberParamDescription> parameters { get; set; } = new List<memberParamDescription>();

        /// <summary>
        /// Gets or sets the exceptions.
        /// </summary>
        /// <value>
        /// The exceptions.
        /// </value>
        [XmlElement(typeof(memberParamDescription), ElementName = "exception")]
        public List<memberParamDescription> exceptions { get; set; } = new List<memberParamDescription>();

        /// <summary>
        /// Initializes a new instance of the <see cref="memberDescription"/> class.
        /// </summary>
        public memberDescription()
        {
        }
    }
}