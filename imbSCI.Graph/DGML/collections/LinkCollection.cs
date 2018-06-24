// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkCollection.cs" company="imbVeles" >
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
using imbSCI.Graph.DGML.core;
using System;

namespace imbSCI.Graph.DGML.collections
{
    /// <summary>
    /// Links
    /// </summary>
    /// <seealso cref="imbSCI.Graph.DGML.collections.GraphElementCollection{imbSCI.Graph.DGML.core.Link}" />
    public class LinkCollection : GraphElementCollection<Link>
    {
        public LinkCollection()
        {
        }

        /// <summary>
        /// Adds a link between <para>nodeA</para> and <para>nodeB</para>. If any of nodes are null it will just return null and add no link.
        /// </summary>
        /// <param name="nodeA">The node a - node from</param>
        /// <param name="nodeB">The node b - node to</param>
        /// <param name="linkLabel">Text label of the link.</param>
        /// <returns></returns>
        public Link AddLink(Node nodeA, Node nodeB, String linkLabel)
        {
            if (nodeA == null) return null;
            if (nodeB == null) return null;

            Link l = new Link();
            l.Source = nodeA.Id;
            l.Target = nodeB.Id;
            l.Label = linkLabel;
            Add(l);
            return l;
        }
    }
}