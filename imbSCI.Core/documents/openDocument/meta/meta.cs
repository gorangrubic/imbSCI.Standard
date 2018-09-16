// --------------------------------------------------------------------------------------------------------------------
// <copyright file="meta.cs" company="imbVeles" >
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

namespace imbSCI.Core.documents.openDocument
{
    [XmlRoot("meta", Namespace = "urn:oasis:names:tc:opendocument:xmlns:meta:1.0")]
    public class metaContainer
    {
        public metaContainer()
        {
        }

        [XmlAttribute(AttributeName = "initial-creator", Namespace = "urn:oasis:names:tc:opendocument:xmlns:meta:1.0")]
        public String initial_creator { get; set; } = "";

        public String generator { get; set; } = "";

        [XmlElement(Namespace = "http://purl.org/dc/elements/1.1/")]
        public String subject { get; set; } = "";

        [XmlElement(Namespace = "http://purl.org/dc/elements/1.1/")]
        public String title { get; set; } = "";

        [XmlElement(ElementName = "user-defined", Namespace = "urn:oasis:names:tc:opendocument:xmlns:meta:1.0")]
        public List<user_defined> user { get; set; } = new List<user_defined>();
    }
}