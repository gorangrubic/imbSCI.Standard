// --------------------------------------------------------------------------------------------------------------------
// <copyright file="freeGraphExtensions.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.io;
using imbSCI.Core.files;
using imbSCI.Data.collection.math;
using imbSCI.Data.enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace imbSCI.Graph.FreeGraph
{
    /// <summary>
    /// Extensions when working with free graphs
    /// </summary>
    public static class freeGraphExtensions
    {
        public static freeGraph GetGraphSection(this freeGraph source, List<String> node_names, String name, String description="")
        {
            var nodes = source.GetNodes(node_names);

            freeGraph output = new freeGraph()
            {
                name = name,
                description = description
            };

            foreach (var node in nodes)
            {
                output.AddNode(node.name, node.weight, node.type);
            }

            foreach (var node in nodes)
            {
                var links = source.GetLinks(node.name, true, false, 1);
                foreach (var link in links)
                {
                    if (node_names.Contains(link.nodeB.name))
                    {
                        output.AddLink(node.name, link.nodeB.name, link.linkBase.weight, link.linkBase.type);
                    }
                }
            }

            return output;
        }



        public static aceRelationMatrix<String, String, Int32> GetIDMatrix(this freeGraph graph, Int32 scoreFactor)
        {

            List<freeGraphNodeBase> nodes = graph.nodes;

            List<String> nodeIDs = nodes.Select(x => x.name).ToList();

            aceRelationMatrix<String, String, Double> output = GetIDMatrix(graph);
            aceRelationMatrix<String, String, Int32> outputInt32 = new aceRelationMatrix<string, string, Int32>(output.GetXAxis(), output.GetYAxis(), 0);

            foreach (string kx in output.GetXAxis())
            {
                foreach (string ky in output.GetYAxis())
                {
                    outputInt32[kx, ky] = Convert.ToInt32(output[kx, ky] * scoreFactor);

                }
            }


            return outputInt32;

        }


        public static aceRelationMatrix<String, String, Double> GetIDMatrix(this freeGraph graph)
        {

            List<freeGraphNodeBase> nodes = graph.nodes;

            List<String> nodeIDs = nodes.Select(x => x.name).ToList();

            aceRelationMatrix<String, String, Double> output = new aceRelationMatrix<String, String, Double>(nodeIDs, nodeIDs, 0);

            foreach (freeGraphNodeBase node in nodes)
            {

                var links = graph.GetLinks(node.name, true, true);

                foreach (freeGraphLinkBase link in links.links)
                {

                    if (link.nodeNameA == node.name)
                    {
                        output[link.nodeNameA, link.nodeNameB] += link.weight;
                    }
                    else
                    {
                        output[link.nodeNameB, link.nodeNameA] += link.weight;

                    }
                }

            }

            return output;

        }



        /// <summary>
        /// Transforms the free graph into relationship matrix
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public static aceRelationMatrix<freeGraphNodeBase, freeGraphNodeBase, Double> GetMatrix(this freeGraph graph)
        {

            List<freeGraphNodeBase> nodes = graph.nodes;

            aceRelationMatrix<freeGraphNodeBase, freeGraphNodeBase, Double> output = new aceRelationMatrix<freeGraphNodeBase, freeGraphNodeBase, Double>(nodes, nodes, 0);

            foreach (freeGraphNodeBase node in nodes)
            {

                var links = graph.GetLinks(node.name, true, true);

                foreach (freeGraphLinkBase link in links.links)
                {
                    var nodeA = graph.GetNode(link.nodeNameA);
                    var nodeB = graph.GetNode(link.nodeNameB);

                    if (link.nodeNameA == node.name)
                    {
                        output[nodeA, nodeB] += link.weight;
                    }
                    else
                    {
                        output[nodeB, nodeA] += link.weight;
                    }
                }



            }

            return output;

        }



        /// <summary>
        /// Pings the size of the graph by expanding from specified <c>pingSources</c> until number of reached nodes is increasing. <see cref="freeGraphPingType" />
        /// </summary>
        /// <param name="graph">The graph that is probed.</param>
        /// <param name="pingSource">The ping source.</param>
        /// <param name="bothDirections">if set to <c>true</c> if will expand trough both backward and forward links</param>
        /// <param name="pingType">Type of the ping operation</param>
        /// <param name="pingLimit">The ping limit - after which the expansion will stop.</param>
        /// <returns>
        /// Value according to specified <c>pingType</c> or 0 on failure
        /// </returns>
        public static Double PingGraphSize(this freeGraph graph, freeGraphNodeBase pingSource, Boolean bothDirections, freeGraphPingType pingType, Int32 pingLimit = 100)
        {
            return PingGraphSize(graph, new List<freeGraphNodeBase> { pingSource }, bothDirections, pingType, pingLimit);
        }

        /// <summary>
        /// Pings the size of the graph by expanding from specified <c>pingSources</c> until number of reached nodes is increasing. <see cref="freeGraphPingType"/>
        /// </summary>
        /// <param name="graph">The graph that is probed.</param>
        /// <param name="pingSources">The ping sources - nodes to start ping expansion from.</param>
        /// <param name="bothDirections">if set to <c>true</c> if will expand trough both backward and forward links. Otherwise propagates forward</param>
        /// <param name="pingType">Type of the ping operation</param>
        /// <param name="pingLimit">The ping limit - after which the expansion will stop.</param>
        /// <returns>Value according to specified <c>pingType</c> or 0 on failure</returns>
        public static Double PingGraphSize(this freeGraph graph, IEnumerable<freeGraphNodeBase> pingSources, Boolean bothDirections, freeGraphPingType pingType, Int32 pingLimit = 100)
        {
            List<String> nodeNames = new List<string>();
            List<String> centralNodes = new List<string>();

            if (pingType == freeGraphPingType.unisonPingLength)
            {
                foreach (var p in pingSources)
                {
                    centralNodes.Add(p.name);
                    nodeNames.Add(p.name);
                }
                freeGraphNodeBase ps = pingSources.First();
                var lst = new List<freeGraphNodeBase>(); // { ps };
                lst.Add(ps);
                pingSources = lst;
            }

            List<Int32> pingResults = new List<int>();
            List<Int32> pingedNodes = new List<int>();

            foreach (var p in pingSources)
            {
                if (pingType != freeGraphPingType.unisonPingLength)
                {
                    nodeNames = new List<string>();
                    centralNodes = new List<string>();
                    nodeNames.Add(p.name);
                    centralNodes.Add(p.name);
                }

                Int32 pingSize = 1;
                Int32 c = 0;

                while (c < pingLimit)
                {
                    var result = graph.GetLinkedNodes(centralNodes, 1, bothDirections);
                    Int32 nCount = nodeNames.Count;
                    foreach (var res in result)
                    {
                        centralNodes = new List<string>();

                        if (!nodeNames.Contains(res.name))
                        {
                            nodeNames.Add(res.name);
                            centralNodes.Add(res.name);
                        }
                    }

                    if (nodeNames.Count > nCount)
                    {
                        pingSize++;
                    }
                    else if (nodeNames.Count == nCount)
                    {
                        break;
                    }
                    c++;
                    if (pingSize > pingLimit) break;
                }

                pingResults.Add(pingSize);
                pingedNodes.Add(nodeNames.Count);
            }

            if (pingResults.Count > 0)
            {
                switch (pingType)
                {
                    case freeGraphPingType.averagePingLength:
                        return pingResults.Average();

                    case freeGraphPingType.maximumPingLength:
                        return pingResults.Max();

                    case freeGraphPingType.unisonPingLength:

                        return pingResults.Max();

                        break;

                    case freeGraphPingType.numberOfPingedNodes:

                        return nodeNames.Count;
                        break;
                }
            }
            return 0;
        }

        /// <summary>
        /// Saves the specified filepath.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="graph">The graph.</param>
        /// <param name="filepath">The filepath.</param>
        /// <param name="mode">The mode.</param>
        public static void Save<T>(this T graph, String filepath, getWritableFileMode mode = getWritableFileMode.overwrite) where T : freeGraph, new()
        {
            if (!Path.HasExtension(filepath))
            {
                filepath = filepath + ".xml";
            }
            var fi = filepath.getWritableFile(mode);

            graph.OnBeforeSave(fi.Directory);

            graph.saveObjectToXML(fi.FullName);
        }

        /// <summary>
        /// Gets the query result clones.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="distanceIncrease">The distance increase.</param>
        /// <param name="weightFactor">The weight factor.</param>
        /// <returns></returns>
        public static List<freeGraphNodeBase> GetQueryResultClones(this IEnumerable<freeGraphNodeBase> source, Int32 distanceIncrease = 0, Double weightFactor = 1)
        {
            List<freeGraphNodeBase> output = new List<freeGraphNodeBase>();
            foreach (freeGraphNodeBase item in source)
            {
                var n = item.GetQueryResultClone(distanceIncrease);
                n.weight = n.weight * weightFactor;
                output.Add(n);
            }
            return output;
        }

        /// <summary>
        /// Gets the weight dictionary.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Dictionary<String, Double> GetWeightDictionary(this IEnumerable<freeGraphNodeBase> source)
        {
            Dictionary<String, Double> output = new Dictionary<string, double>();
            foreach (freeGraphNodeBase item in source)
            {
                output.Add(item.name, item.weight);
            }
            return output;
        }
    }
}