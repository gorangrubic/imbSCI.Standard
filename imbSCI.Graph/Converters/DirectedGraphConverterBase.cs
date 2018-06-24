// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectedGraphConverterBase.cs" company="imbVeles" >
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
using imbSCI.Graph.Converters.tools;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;

////using System.Web.UI.WebControls;
//using Accord;

namespace imbSCI.Graph.Converters
{
    /// <summary>
    /// Foundation for DirectedGraph converter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="imbSCI.Graph.Converters.graphToGraphConverterBase{T, imbSCI.Graph.DGML.DirectedGraph}" />
    public abstract class DirectedGraphConverterBase<T, TElement> : graphToGraphConverterBase<T, DirectedGraph, TElement, GraphElement> where T : IObjectWithName
        where TElement : IObjectWithName
    {
        protected void Deploy(GraphStylerSettings settings = null)
        {
            setup = settings;
            if (setup == null)
            {
                setup = imbSCI.Graph.config.imbSCIGraphConversionConfig.settings.DefaultGraphExportStyle;
            }
        }

        public GraphStylerSettings setup { get; set; }

        public abstract DirectedGraphStylingCase GetStylingCase(IEnumerable<T> source);

        public abstract String GetCategoryID(T nodeOrLink);

        public abstract Int32 GetTypeID(T nodeOrLink);

        ///// <summary>
        ///// Gets the node weight - by default implementation it is 1 / <see cref="IGraphNode.level"/>
        ///// </summary>
        ///// <param name="node">The node.</param>
        ///// <returns></returns>
        //public abstract Double GetNodeWeight(T node);

        ///// <summary>
        ///// Gets the link weight.
        ///// </summary>
        ///// <param name="nodeA">The node a.</param>
        ///// <param name="nodeB">The node b.</param>
        ///// <returns></returns>
        //public abstract Double GetLinkWeight(T nodeA, T nodeB);

        /// <summary>
        /// Gets the node label.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public virtual String GetNodeLabel(T node)
        {
            return GetNodeName(node);
        }

        /// <summary>
        /// Gets the node identifier.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public abstract String GetNodeID(T node);

        /// <summary>
        /// Gets a new <see cref="Link"/>
        /// </summary>
        /// <param name="nodeA">The node a.</param>
        /// <param name="nodeB">The node b.</param>
        /// <returns></returns>
        public Link GetLink(T nodeA, T nodeB)
        {
            Link output = new Link();
            output.Source = GetNodeID(nodeA);
            output.Target = GetNodeID(nodeB);
            output.Label = GetLinkLabel(nodeA, nodeB);
            return output;
        }
    }
}