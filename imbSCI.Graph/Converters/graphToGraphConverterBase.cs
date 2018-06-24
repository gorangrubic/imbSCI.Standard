// --------------------------------------------------------------------------------------------------------------------
// <copyright file="graphToGraphConverterBase.cs" company="imbVeles" >
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
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;

//using System.Web.UI.WebControls;

namespace imbSCI.Graph.Converters
{
    //public abstract class graphToDotGraphConverterBase<TGraphFrom>:graphToGra

    /// <summary>
    /// Common foundation of graph converters
    /// </summary>
    /// <typeparam name="TGraphFrom">The type of the graph from.</typeparam>
    /// <typeparam name="TGraphTo">The type of the graph to.</typeparam>
    public abstract class graphToGraphConverterBase<TGraphFrom, TGraphTo, TGraphFromElement, TGraphToElement> where TGraphFrom : IObjectWithName
        where TGraphTo : IObjectWithName
        where TGraphFromElement : IObjectWithName
        where TGraphToElement : IObjectWithName
    {
        /// <summary>
        /// Gets the node weight - by default implementation it is 1 / <see cref="IGraphNode.level"/>
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public abstract Double GetNodeWeight(TGraphFrom node);

        /// <summary>
        /// Provides the link weight, by default implementation returns 1 / <see cref="IGraphNode.Count"/>
        /// </summary>
        /// <param name="nodeA">The node a.</param>
        /// <param name="nodeB">The node b.</param>
        /// <returns></returns>
        public abstract Double GetLinkWeight(TGraphFrom nodeA, TGraphFrom nodeB);

        /// <summary>
        /// Gets the label for link - by default implementation returns empty string
        /// </summary>
        /// <param name="nodeA">The node a.</param>
        /// <param name="nodeB">The node b.</param>
        /// <returns></returns>
        public virtual String GetLinkLabel(TGraphFrom nodeA, TGraphFrom nodeB)
        {
            String output = "";
            if (nodeA == null)
            {
                output = "[a:null]";
            }
            if (nodeB == null)
            {
                output += "[b:null]";
            }
            return output;
        }

        /// <summary>
        /// Provides display Label for specified node, in default implementation returns <see cref="IGraphNode.Id"/>
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public virtual String GetNodeName(TGraphFrom node)
        {
            if (node == null)
            {
                return "[null]";
            }
            return node.name;
        }

        /// <summary>
        /// Conversion from <see cref="{TGraphTo}"/> to <see cref="{TGraphFrom}"/>
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="depthLimit">The depth limit.</param>
        /// <returns></returns>
        public abstract TGraphTo Convert(TGraphFrom source, Int32 depthLimit = 500, IEnumerable<TGraphFromElement> rootNodes = null);

        /// <summary>
        /// Converts from <see cref="{TGraphTo}"/> to <see cref="{TGraphFrom}"/>
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="depthLimit">The depth limit.</param>
        /// <returns></returns>
        public abstract TGraphFrom Convert(TGraphTo source, Int32 depthLimit = 500, IEnumerable<TGraphToElement> rootNodes = null);
    }
}