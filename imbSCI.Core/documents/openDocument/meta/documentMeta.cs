// --------------------------------------------------------------------------------------------------------------------
// <copyright file="documentMeta.cs" company="imbVeles" >
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

namespace imbSCI.Core.documents.openDocument.meta
{
    [XmlRoot("document-meta")]
    public class documentMeta
    {
        public documentMeta()
        {
            deploy();
        }

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns { get; set; }

        [XmlAttribute(Namespace = "urn:oasis:names:tc:opendocument:xmlns:office:1.0")]
        public String version { get; set; } = "1.2";

        [XmlElement(ElementName = "meta", Namespace = "urn:oasis:names:tc:opendocument:xmlns:meta:1.0")]
        public metaContainer metaObject { get; set; } = new metaContainer();

        protected void deploy()
        {
            xmlns = new XmlSerializerNamespaces();
            xmlns.Add("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
            xmlns.Add("xlink", "http://www.w3.org/1999/xlink");
            xmlns.Add("dc", "http://purl.org/dc/elements/1.1/");
            xmlns.Add("meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
            xmlns.Add("ooo", "http://openoffice.org/2004/office");
            xmlns.Add("grddl", "http://www.w3.org/2003/g/data-view#");
        }
    }
}