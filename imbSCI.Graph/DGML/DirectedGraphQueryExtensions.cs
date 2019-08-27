// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectedGraphExtensions.cs" company="imbVeles" >
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
using imbSCI.Core.data;
using imbSCI.Core.data.descriptors;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace imbSCI.Graph.DGML
{
    /// <summary>
    /// Extension methods used to query nodes and links of <see cref="DirectedGraph"/> instance
    /// </summary>
    public static class DirectedGraphQueryExtensions
    {

        /// <summary>
        /// Selects a node in graph using path relative to <c>startingNode</c> or graph roots, as provided by <see cref="GetRootNodes(DirectedGraph)"/>
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="path">The path.</param>
        /// <param name="pathSeparator">The path separator.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <returns>Null if path failed or node</returns>
        public static Node SelectNode(this DirectedGraph graph, String path, String pathSeparator="/", Node startingNode=null)
        {
            var pathParts = path.SplitSmart(pathSeparator, "", true, true);
            Node head = startingNode;
            
            foreach (var pathPart in pathParts)
            {
                if (head == null)
                {
                    // looking for root node
                    var roots = graph.GetRootNodes();
                    head = roots.FirstOrDefault(x => x.Id.Equals(pathPart));

                } else
                {
                    var children = graph.GetLinked(head, false);
                    head = children.FirstOrDefault(x => x.Id.Equals(pathPart));
                    // looking for outbound links
                }
                if (head == null)
                {
                    break;
                }
            }

            return head;
           
        }

        /// <summary>
        /// Gets the root nodes: nodes without inbound links
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public static List<Node> GetRootNodes(this DirectedGraph graph)
        {
            List<Node> output = new List<Node>();

            foreach (Node node in graph.Nodes)
            {
                var sourceLinks = graph.Links.Where(x => x.Source.Equals(node.Id));
                if (!sourceLinks.Any())
                {
                    output.Add(node);
                }
            }

            return output;
        }


        /// <summary>
        /// Gets all linked in iterations.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="inverse">if set to <c>true</c> [inverse].</param>
        /// <param name="distanceLimit">The distance limit.</param>
        /// <returns></returns>
        public static List<List<Node>> GetAllLinkedInIterations(this DirectedGraph graph,Node node, Boolean inverse = false, Int32 distanceLimit = 100)
        {
            List<List<Node>> output = new List<List<Node>>();

            List<Node> iteration = new List<Node> { node };
            Int32 c = 0;
            while (iteration.Any())
            {
                List<Node> newIteration = new List<Node>();

                foreach (var i in iteration)
                {
                    newIteration.AddRange(graph.GetLinked(i, inverse));
                }

                output.Add(newIteration);
                iteration = newIteration;

                c++;
                if (c > distanceLimit) break;
            }

            return output;
        }

        /// <summary>
        /// Gets all linked nodes .
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="inverse">if set to <c>true</c> [inverse].</param>
        /// <param name="distanceLimit">The distance limit.</param>
        /// <returns></returns>
        public static List<Node> GetAllLinked(this DirectedGraph graph,Node node, Boolean inverse = false, Int32 distanceLimit = 100)
        {
            List<Node> output = new List<Node>();

            List<Node> iteration = new List<Node> { node };
            Int32 c = 0;
            while (iteration.Any())
            {
                List<Node> newIteration = new List<Node>();

                foreach (var i in iteration)
                {
                    newIteration.AddRange(graph.GetLinked(i, inverse));
                }

                output.AddRange(newIteration);
                iteration = newIteration;

                c++;
                if (c > distanceLimit) break;
            }

            return output;
        }

        /// <summary>
        /// Gets the linked.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="inverse">if set to <c>true</c> than it will return nodes that are linking to the specified <c>node</c></param>
        /// <returns></returns>
        public static List<Node> GetLinked(this DirectedGraph graph, Node node, Boolean inverse = false)
        {
            List<Node> output = new List<Node>();

            IEnumerable<Link> links = null;

            if (inverse)
            {
                links = graph.Links.Where(x => x.Source == node.Id);
            }
            else
            {
                links = graph.Links.Where(x => x.Target == node.Id);
            }


            foreach (Link lnks in links)
            {
                if (graph.Nodes.Any(x => x.Id == lnks.Target))
                {
                    output.Add(graph.Nodes.First(x => x.Id == lnks.Target));
                    //T cNode = new T();
                    //cNode.name = lnks.Target;
                    //nodeNames.Add(lnks.Target);
                    //newnext.Add(source.Nodes.First(x => x.Id == lnks.Target));

                    //tNode.Add(cNode);
                }
            }
            return output;
        }



    }
}