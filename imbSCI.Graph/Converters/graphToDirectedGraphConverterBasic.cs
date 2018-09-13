// --------------------------------------------------------------------------------------------------------------------
// <copyright file="graphToDirectedGraphConverterBasic.cs" company="imbVeles" >
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
//using System.Web.UI.WebControls;
//using Accord;
using System;
using System.Collections.Generic;
using imbSCI.Core.math;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.Graph.Converters.tools;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;

namespace imbSCI.Graph.Converters
{

    /// <summary>
    /// Basic implementation of graph to DGML converter
    /// </summary>
    /// <seealso cref="imbSCI.Graph.Converters.graphToDirectedGraphConverterBase{imbSCI.Data.collection.graph.IGraphNode}" />
    public class graphToDirectedGraphConverterBasic : graphToDirectedGraphConverterBase<graphNode> // where T : IGraphNode, new()
    {
        public override string GetCategoryID(graphNode nodeOrLink)
        {
            if (nodeOrLink == null) return "null";
            return nodeOrLink.GetType().Name;
        }

        public override int GetTypeID(graphNode nodeOrLink)
        {
            if (nodeOrLink == null) return 0;
            return nodeOrLink.GetType().GetHashCode();
        }

        public override double GetNodeWeight(graphNode node)
        {
            if (node == null) return 0;
            return ((node.Count() + 1) * (1.GetRatio(node.level)));
        }

        public override double GetLinkWeight(graphNode nodeA, graphNode nodeB)
        {
            if (nodeA == null) return 0;
            if (nodeB == null) return 0;
            return 1.GetRatio(nodeA.Count() + 1);
        }
    }
}