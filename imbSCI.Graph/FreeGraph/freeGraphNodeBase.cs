// --------------------------------------------------------------------------------------------------------------------
// <copyright file="freeGraphNodeBase.cs" company="imbVeles" >
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
using imbSCI.Data.interfaces;
using System;
using System.Xml.Serialization;

namespace imbSCI.Graph.FreeGraph
{
    /// <summary>
    /// Instance describing a node
    /// </summary>
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithName" />
    public class freeGraphNodeBase : IObjectWithName
    {
        public freeGraphNodeBase()
        {
        }

        /// <summary>
        /// Just clones the node and adds specified distance
        /// </summary>
        /// <param name="distanceIncrease">The distance increase.</param>
        /// <returns></returns>
        public freeGraphNodeBase GetQueryResultClone(Int32 distanceIncrease = 1)
        {
            freeGraphNodeBase node = new freeGraphNodeBase();
            node.name = name;
            node.weight = weight;
            node.type = type;
            node.distance = distance + distanceIncrease;
            return node;
        }

        public String name { get; set; }

        public Double weight { get; set; } = 1;

        public Int32 type { get; set; } = 0;

        /// <summary>
        /// Distance from query node/s, only relevant when it is query result
        /// </summary>
        /// <value>
        /// The distance.
        /// </value>
        [XmlIgnore]
        public Double distance { get; set; } = 1;
    }
}