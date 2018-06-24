// --------------------------------------------------------------------------------------------------------------------
// <copyright file="memberRegistryEntry.cs" company="imbVeles" >
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
using imbSCI.Core.files;
using imbSCI.Data.collection.graph;
using System.Reflection;
using System.Xml;

namespace imbSCI.Core.data.metadata
{
    /// <summary>
    /// Member meta data registry graph node
    /// </summary>
    public class memberRegistryEntry : graphNodeCustom
    {
        public memberRegistryEntry(XmlNode node)
        {
            deployNode(node);
        }

        //public memberRegistryEntry() { }

        /// <summary>
        /// Deploys the node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void deployNode(XmlNode node)
        {
            member = objectSerialization.ObjectFromXML<memberDescription>(node.OuterXml);
        }

        public void deployMember(MemberInfo m_info)
        {
        }

        /// <summary>
        /// Member meta data
        /// </summary>
        /// <value>
        /// The member.
        /// </value>
        public memberDescription member { get; set; } = new memberDescription();

        /// <summary>
        /// Gets or sets the type of the member.
        /// </summary>
        /// <value>
        /// The type of the member.
        /// </value>
        public memberRegistryEntryType memberType { get; set; } = memberRegistryEntryType.entry_namespace;

        /// <summary>
        /// Turns on the autorenaming, when new child is inserted into this node - it will autorename it to have unique name
        /// </summary>
        /// <value>
        /// <c>true</c> if [do autorename on existing]; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override bool doAutorenameOnExisting { get { return false; } }

        protected override bool doAutonameFromTypeName { get { return false; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="memberRegistryEntry"/> class.
        /// </summary>
        public memberRegistryEntry()
        {
        }
    }
}