// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dotToDirectedGraphConverterBasic.cs" company="imbVeles" >
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
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using imbSCI.Graph.DOT;
using System.Collections.Generic;

namespace imbSCI.Graph.Converters
{
    public class dotToDirectedGraphConverterBasic : graphToGraphConverterBase<DotGraph, DirectedGraph, GraphElement, GraphElement>
    {
        /// <summary>
        /// Conversion from <see cref="!:&lt;TGraphTo&gt;" /> to <see cref="!:&lt;TGraphFrom&gt;" />
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="depthLimit">The depth limit.</param>
        /// <param name="rootNodes"></param>
        /// <returns></returns>
        public override DirectedGraph Convert(DotGraph source, int depthLimit = 500, IEnumerable<GraphElement> rootNodes = null)
        {
            DirectedGraph output = new DirectedGraph();

            output.Title = source.Title;
            output.Nodes.AddRange(source.Nodes);
            output.Links.AddRange(source.Links);
            if (source.Directed)
            {
                output.Layout = DGML.enums.GraphLayoutEnum.ForceDirected;
            }
            else
            {
                output.Layout = DGML.enums.GraphLayoutEnum.Sugiyama;
            }
            return output;
        }

        /// <summary>
        /// Converts from <see cref="!:&lt;TGraphTo&gt;" /> to <see cref="!:&lt;TGraphFrom&gt;" />
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="depthLimit">The depth limit.</param>
        /// <param name="rootNodes"></param>
        /// <returns></returns>
        public override DotGraph Convert(DirectedGraph source, int depthLimit = 500, IEnumerable<GraphElement> rootNodes = null)
        {
            DotGraph output = new DotGraph();
            output.Title = source.Title;
            output.Nodes.AddRange(source.Nodes.ConvertNodes());
            foreach (Link l in source.Links)
            {
                output.Links.Add(ConvertLink(l));
            }

            output.Layout = source.Layout;

            output.GraphDirection = source.GraphDirection;

            return output;
        }

        public virtual DotLink ConvertLink(Link source)
        {
            DotLink output = new DotLink(source.Source, source.Target);
            output.Label = source.Label;
            output.Stroke = source.Stroke;
            output.Id = source.Id;
            return output;
        }

        public virtual DotNode ConvertNode(Node source)
        {
            DotNode dn = new DotNode(source.Label);
            dn.Group = source.Group;
            dn.Shape = DotNodeShape.Box;
            dn.Style = DotNodeStyle.Dashed;
            dn.Visibility = source.Visibility;
            return dn;
        }

        public override double GetLinkWeight(DotGraph nodeA, DotGraph nodeB)
        {
            return 1;
        }

        public override double GetNodeWeight(DotGraph node)
        {
            return 1;
        }
    }
}