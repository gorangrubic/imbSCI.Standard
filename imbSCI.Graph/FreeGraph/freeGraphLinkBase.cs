// --------------------------------------------------------------------------------------------------------------------
// <copyright file="freeGraphLinkBase.cs" company="imbVeles" >
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
// Project: imbSCI.Graph
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

namespace imbSCI.Graph.FreeGraph
{
    /// <summary>
    /// Basic object describing node-to-node relationship in the <see cref="freeGraph"/>,used by the <see cref="freeGraph"/> model internally
    /// </summary>
    [Serializable]
    public class freeGraphLinkBase
    {
        public freeGraphLinkBase()
        {
        }

        /// <summary>
        /// Numeric ID of link type. Has no specific implementation in base <see cref="freeGraph"/>, meant for custom applications
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Int32 type { get; set; } = 0;

        /// <summary>
        /// Weight associated with the link, no specific implementation by default - meant for custom applications
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public Double weight { get; set; } = 1;

        /// <summary>
        /// Node ID for source / from node
        /// </summary>
        /// <value>
        /// The node name a.
        /// </value>
        public String nodeNameA { get; set; } = "";

        /// <summary>
        /// Node ID for target / to node
        /// </summary>
        /// <value>
        /// The node name b.
        /// </value>
        public String nodeNameB { get; set; } = "";

        /// <summary>
        /// Customized link label, by default stays blank
        /// </summary>
        /// <value>
        /// The link label.
        /// </value>
        public String linkLabel { get; set; } = "";

        public freeGraphLinkBase GetClone()
        {
            freeGraphLinkBase link = new freeGraphLinkBase();
            link.type = type;
            link.weight = weight;
            link.nodeNameA = nodeNameA;
            link.nodeNameB = nodeNameB;
            link.linkLabel = linkLabel;
            return link;
        }
    }
}