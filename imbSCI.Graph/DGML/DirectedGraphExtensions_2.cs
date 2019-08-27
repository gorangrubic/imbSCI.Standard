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
using imbSCI.Core.collection;
using imbSCI.Core.data;
using imbSCI.Core.data.transfer;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files;
using imbSCI.Data;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace imbSCI.Graph.DGML
{
public static class DirectedGraphExtensions
    {

        //public static void Import(this IFreeGraph target, IFreeGraph source)
        //{
        //    target.Nodes
        //}


        /// <summary>
        /// Gets the subgraph.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="nodes">The nodes.</param>
        /// <returns></returns>
        public static DirectedGraph GetSubgraph(this DirectedGraph source, IEnumerable<Node> nodes)
        {
            var output = new DirectedGraph(); // objectSerialization.CloneViaXML(source);
            foreach (Node n in nodes)
            {
                var on = output.Nodes.AddNode(n.Id, n.Label);
                on.setObjectBySource(n);

                source.Links.GetLinks(n.Id, true, true);

            }

            foreach (Node n in nodes)
            {
                var source_links = source.Links.GetLinks(n.Id, true, true);
                foreach (Link l in source_links)
                {
                    var n_s = output.Nodes[l.Source]; //.Contains()
                    var n_t = output.Nodes[l.Target];
                    if (n_s != null && n_t != null) output.Links.AddLink(n_s, n_t, l.Label);
                }
            }

            return output;
        }


        /// <summary>
        /// Creates links (and nodes, if not existing)
        /// </summary>
        /// <typeparam name="TLinkSource">The type of the link source.</typeparam>
        /// <param name="graph">The graph.</param>
        /// <param name="link">The link.</param>
        /// <param name="NodeAUID">The node auid.</param>
        /// <param name="NodeBUID">The node buid.</param>
        /// <param name="LinkLabel">The link label.</param>
        /// <param name="setProperties">if set to <c>true</c> [set properties].</param>
        public static ElementSourceDictionary Link<TLinkSource>(this 
            DirectedGraph graph, 
            IEnumerable<TLinkSource> link, 
            Func<TLinkSource, String> NodeAUID,
            Func<TLinkSource, String> NodeBUID,
            Func<TLinkSource, String> LinkLabel,
            Boolean setProperties=true)
        {
            var linkIndex = link.ToList();
            ElementSourceDictionary ElementToSourceObject = new ElementSourceDictionary();

            for (int i = 0; i < linkIndex.Count; i++)
            {
                Link linkAK = graph.Links.AddOrGetLink(NodeAUID(linkIndex[i]), NodeBUID(linkIndex[i]));
                linkAK.Label = LinkLabel(linkIndex[i]);
                ElementToSourceObject[linkAK].Add(linkIndex[i]);
                
            }

            if (graph is DirectedGraphWithSourceData graphWithData) graphWithData.Sources.AddRange(ElementToSourceObject, true);
           
            return ElementToSourceObject;

        }




         public static ListDictionary<GraphElement, Object> Link<TNodeASource, TNodeBSource>(this 
            DirectedGraph graph, 
            IEnumerable<TNodeASource> nodesA, 
            Func<TNodeASource, String> NodeAUID,
            Func<TNodeASource, String> NodeALabel, 
              IEnumerable<TNodeBSource> nodesB, 
            Func<TNodeBSource, String> NodeBUID,
            Func<TNodeBSource, String> NodeBLabel, 
            Func<TNodeASource, TNodeBSource, String> LinkABLabel,
             Boolean setProperties=true, 
            Boolean useFormatting=true)
        {

            var nodeAIndex = nodesA.ToList();
            var nodeBIndex = nodesB.ToList();
            ElementSourceDictionary ElementToSourceObject = new ElementSourceDictionary();

            for (int i = 0; i < Math.Min(nodeAIndex.Count, nodeBIndex.Count); i++)
            {
                Node nodeA = graph.Nodes.AddOrGet(NodeAUID(nodeAIndex[i]), NodeALabel(nodeAIndex[i]));
                Node nodeB = graph.Nodes.AddOrGet(NodeBUID(nodeBIndex[i]), NodeBLabel(nodeBIndex[i]));

                graph.DeployPropertiesAndFormatting(nodeA, nodeAIndex[i], setProperties, useFormatting);
                graph.DeployPropertiesAndFormatting(nodeB, nodeBIndex[i], setProperties, useFormatting);

                ElementToSourceObject[nodeA].Add(nodeAIndex[i]);
                ElementToSourceObject[nodeB].Add(nodeBIndex[i]);

                Link linkAK = graph.Links.AddOrGetLink(NodeAUID(nodeAIndex[i]), NodeBUID(nodeBIndex[i]));
                linkAK.Label = LinkABLabel(nodeAIndex[i], nodeBIndex[i]);

                ElementToSourceObject[linkAK].Add(nodeAIndex[i]);
                ElementToSourceObject[linkAK].Add(nodeBIndex[i]);

            }



            if (graph is DirectedGraphWithSourceData graphWithData) graphWithData.Sources.AddRange(ElementToSourceObject, true);
            return ElementToSourceObject;

        }

        public static ElementSourceDictionary Select<TNodeSource>(this DirectedGraph graph, IEnumerable<TNodeSource> nodes, Func<TNodeSource, String> NodeUID, Boolean includeSourceLinks=false, Boolean includeTargetLinks=true)
        {
            ElementSourceDictionary ElementToSourceObject = new ElementSourceDictionary();
            foreach (TNodeSource node in nodes)
            {
                Node nodeA = graph.Nodes[NodeUID(node)];

                ElementToSourceObject[nodeA].Add(node);

                if (includeSourceLinks)
                {
                    foreach (var l in graph.Links.Where(x => x.Source == nodeA.Id))
                    {
                        ElementToSourceObject[l].Add(node);
                    }
                }

                if (includeTargetLinks)
                {
                    foreach (var l in graph.Links.Where(x => x.Target == nodeA.Id))
                    {
                        ElementToSourceObject[l].Add(node);
                    }
                }
            }
            return ElementToSourceObject;
        }

         public static ElementSourceDictionary Populate<TNodeSource, TChildNodeSource>(this 
            DirectedGraph graph, 
            IEnumerable<TNodeSource> nodes, 
            Func<TNodeSource, IEnumerable<TChildNodeSource>> linkedNodesFunction, 
            Func<TNodeSource, String> NodeUID,
            Func<TNodeSource, String> NodeLabel, 
            Func<TChildNodeSource, String> ChildNodeUID,
            Func<TChildNodeSource, String> ChildNodeLabel, 
            Boolean setProperties=true, 
            Boolean useFormatting=true)
        {

            ElementSourceDictionary ElementToSourceObject = new ElementSourceDictionary();

            foreach (TNodeSource node in nodes)
            {
                Node nodeA = graph.Nodes.AddOrGet(NodeUID(node), NodeLabel(node));
                ElementToSourceObject[nodeA].Add(node);

                graph.DeployPropertiesAndFormatting(nodeA, node, setProperties, useFormatting);

               // nodeA.DeployColor(node, NodeColor)

                var child_nodes = linkedNodesFunction(node);

                foreach (TChildNodeSource child in child_nodes)
                {
                    Node nodeB = graph.Nodes.AddOrGet(ChildNodeUID(child), ChildNodeLabel(child));

                    ElementToSourceObject[nodeB].Add(child);

                    graph.DeployPropertiesAndFormatting(nodeB, child, setProperties, useFormatting);

                    Link linkAK = graph.Links.AddOrGetLink(NodeUID(node), ChildNodeUID(child));


                    ElementToSourceObject[linkAK].Add(node);
                    ElementToSourceObject[linkAK].Add(child);
                }
            }
            if (graph is DirectedGraphWithSourceData graphWithData) graphWithData.Sources.AddRange(ElementToSourceObject, true);
            return ElementToSourceObject;
        }


        public static ElementSourceDictionary Populate<TNodeSource>(this DirectedGraph graph, IEnumerable<TNodeSource> nodes,
           Func<TNodeSource, String> NodeUID,
           Func<TNodeSource, String> NodeLabel)
        {
            ElementSourceDictionary ElementToSourceObject = new ElementSourceDictionary();

            foreach (TNodeSource node in nodes)
            {
                if (node == null) continue;
                Node nodeA = graph.Nodes.AddOrGet(NodeUID(node), NodeLabel(node));
                ElementToSourceObject[nodeA].Add(node);

                graph.DeployPropertiesAndFormatting(nodeA, node, false, false);

            }
            if (graph is DirectedGraphWithSourceData graphWithData) graphWithData.Sources.AddRange(ElementToSourceObject, true);
            return ElementToSourceObject;
        }


        public static ElementSourceDictionary Populate<TNodeSource>(this DirectedGraph graph, IEnumerable<TNodeSource> nodes,
            Func<TNodeSource, IEnumerable<TNodeSource>> linkedNodesFunction,
            Func<TNodeSource, String> NodeUID,
            Func<TNodeSource, String> NodeLabel,
            Boolean setProperties = true,
            Boolean useFormatting = true)
        {
            ElementSourceDictionary ElementToSourceObject = new ElementSourceDictionary();

            foreach (TNodeSource node in nodes)
            {
                if (node == null) continue;
                Node nodeA = graph.Nodes.AddOrGet(NodeUID(node), NodeLabel(node));
                ElementToSourceObject[nodeA].Add(node);

                graph.DeployPropertiesAndFormatting(nodeA, node, setProperties, useFormatting);

                var child_nodes = linkedNodesFunction(node);

                foreach (TNodeSource child in child_nodes)
                {
                    if (child == null) continue;
                    Node nodeB = graph.Nodes.AddOrGet(NodeUID(child), NodeLabel(child));
                    ElementToSourceObject[nodeB].Add(child);

                    graph.DeployPropertiesAndFormatting(nodeB, child, setProperties, useFormatting);

                    Link linkAK = graph.Links.AddOrGetLink(NodeUID(node), NodeUID(child));
                    ElementToSourceObject[linkAK].Add(node);
                    ElementToSourceObject[linkAK].Add(child);
                }
            }
            if (graph is DirectedGraphWithSourceData graphWithData) graphWithData.Sources.AddRange(ElementToSourceObject, true);
            return ElementToSourceObject;
        }


        public static void SetStroke<TNodeSource, TGraphElement>(this ElementSourceDictionary elementVsSources, Func<TNodeSource, String> StrokeColorFunction = null, Func<TNodeSource, Int32> StrokeWidthFunction = null,
            Func<TNodeSource, String> StrokeDashArrayFunction = null) where TGraphElement : GraphElement
        {
            foreach (var pair in elementVsSources)
            {
                foreach (var sourceObject in pair.Value)
                {
                    TGraphElement nodeA = pair.Key as TGraphElement;

                    if (nodeA != null) if (sourceObject is TNodeSource nodeSource)
                    {
                        if (StrokeColorFunction != null)
                        {
                            nodeA.Stroke = StrokeColorFunction(nodeSource);
                        }
                        if (StrokeWidthFunction != null)
                        {
                            nodeA.StrokeThinkness = StrokeWidthFunction(nodeSource);
                        }
                        if (StrokeDashArrayFunction != null)
                        {
                            nodeA.StrokeDashArray = StrokeDashArrayFunction(nodeSource);
                        }
                    }
                }
                
            }
        }

        public static void SetColor<TNodeSource, TGraphElement>(this ElementSourceDictionary elementVsSources, Func<TNodeSource, String> BackgroundFunction = null, Func<TNodeSource, String> ForegroundFunction = null)
            where TGraphElement:GraphNodeElement
        {
            foreach (var pair in elementVsSources)
            {
                foreach (var sourceObject in pair.Value)
                {
                    TGraphElement nodeA = pair.Key as TGraphElement;
                    if (nodeA != null)
                    {

                        if (sourceObject is TNodeSource nodeSource)
                        {
                            if (BackgroundFunction != null)
                            {
                                nodeA.Background = BackgroundFunction(nodeSource);
                            }
                            if (ForegroundFunction != null)
                            {
                                nodeA.Foreground = ForegroundFunction(nodeSource);
                            }
                        }
                    }
                }

            }

           
        }

        
        public static void DeployPropertiesAndFormatting(this DirectedGraph graph, Node node, Object nodeSource, 
            Boolean setProperties=true, 
            Boolean useFormatting=true)
        {
            if (!setProperties && !useFormatting) return;

            settingsEntriesForObject SEO = nodeSource.GetType().GetSEO();
            if (useFormatting) graph.DeployFormatting(node, nodeSource, SEO);
            if (setProperties) graph.DeployProperties(node, nodeSource,SEO);
        }


        public static void DeployFormatting(this DirectedGraph graph, Node node, Object nodeSource,settingsEntriesForObject SEO)
        {
            if (node.Label.isNullOrEmpty()) node.Label = SEO.DisplayName;
            
        }

        public static void DeployProperties(this DirectedGraph graph, Node node, Object nodeSource,settingsEntriesForObject SEO)
        {
            
            foreach (KeyValuePair<string, settingsPropertyEntryWithContext> spe in SEO.spes)
            {
                settingsPropertyEntryWithContext sPEC = spe.Value;

                graph.PropertyDeclaration.RegisterProperty(spe.Value);
                node.Properties.Add(sPEC.pi.GetGUI(), sPEC.pi.GetValue(nodeSource, null).toStringSafe(), "");
            }
            

        }

    }
}