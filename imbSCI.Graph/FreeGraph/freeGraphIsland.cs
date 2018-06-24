// --------------------------------------------------------------------------------------------------------------------
// <copyright file="freeGraphIsland.cs" company="imbVeles" >
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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace imbSCI.Graph.FreeGraph
{
    /// <summary>
    /// Describes one graph island, detected in a <see cref="freeGraph"/>
    /// </summary>
    /// <remarks>
    /// Graph island is a group of interconnected graph nodes
    /// </remarks>
    public class freeGraphIsland
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="freeGraphIsland"/> class.
        /// </summary>
        public freeGraphIsland()
        {
        }

        /// <summary>
        /// Adds the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        public void Add(IEnumerable<freeGraphNodeBase> input)
        {
            foreach (var n in input)
            {
                nodes.Add(n.name);
            }
        }

        /// <summary>
        /// Gets the <see cref="freeGraphIslandType" />, indicating the size (called mass, count of nodes) of the island. Set is ignored, it is declared just to enable proper XML serialization of Get value
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public freeGraphIslandType type
        {
            get
            {
                if (Count == 0) return freeGraphIslandType.none;
                if (Count == 1) return freeGraphIslandType.mononodal;
                if (Count == 2) return freeGraphIslandType.binodal;
                if (Count == 3) return freeGraphIslandType.trinodal;
                if (Count > 3) return freeGraphIslandType.polinodal;
                return freeGraphIslandType.unknown;
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets the node count
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        [XmlIgnore]
        public Int32 Count
        {
            get
            {
                return nodes.Count;
            }
        }

        /// <summary>
        /// Adds all nodes from the query into collection that are not already in the island (identified by: <see cref="freeGraphNodeBase.name"/> )
        /// </summary>
        /// <param name="queryResult">The query result.</param>
        public void Add(freeGraphQueryResult queryResult)
        {
            List<freeGraphNodeBase> allNodes = queryResult.ToList();
            allNodes.AddRange(queryResult.queryNodes);

            foreach (freeGraphNodeBase n in allNodes)
            {
                if (!nodes.Contains(n.name))
                {
                    nodes.Add(n.name);
                }
            }
        }

        /// <summary>
        /// Name of nodes found in the island
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public List<String> nodes { get; set; } = new List<string>();
    }
}