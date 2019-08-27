// --------------------------------------------------------------------------------------------------------------------
// <copyright file="graphToDirectedGraphConverterBase.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.data;
using imbSCI.Core.style.color;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Graph.Converters
{



    public abstract class graphToDirectedGraphConverterBase<T> : DirectedGraphConverterBase<T, T> where T : IGraphNode, new()
    {
        public override string GetNodeID(T node)
        {
            if (node == null) return "null";
            return node.path;
        }

        /// <summary>
        /// Converts from <see cref="!:&lt;TGraphTo&gt;" /> to <see cref="!:&lt;TGraphFrom&gt;" />
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="depthLimit">The depth limit.</param>
        /// <param name="rootNodes"></param>
        /// <returns></returns>
        public override T Convert(DirectedGraph source, int depthLimit = 500, IEnumerable<GraphElement> rootNodes = null)
        {
            T output = new T();

            output.name = source.Title;

            if (!source.Nodes.Any()) return output;

            List<T> nodes = new List<T>();
            List<String> nodeNames = new List<string>();

            Dictionary<T, Node> next = new Dictionary<T, Node>();

            Boolean run = true;

            foreach (var io in rootNodes)
            {
                var tmp = new T();
                tmp.name = io.Id;
                Node oi = io as Node;
                next.Add(tmp, oi);
                output.Add(tmp);
            }

            //List<Node> next = new List<Node>();

            // next.AddRange(rootNodes.ConvertList<IObjectWithName,Node>());

            Int32 i = 0;
            while (run)
            {
                Dictionary<T, Node> newnext = new Dictionary<T, Node>();
                // List<T> newT = new ListItemType<();

                foreach (var pair in next)
                {
                    T tNode = new T();

                    tNode.name = pair.Value.Id; // g.Id;

                    foreach (Node ln in source.GetLinked(pair.Value))
                    {
                        if (!nodeNames.Contains(ln.Id))
                        {
                            var tmp = new T();
                            tmp.name = ln.Id;
                            Node oi = ln as Node;
                            newnext.Add(tmp, oi);
                            pair.Key.Add(tmp);
                        }
                        nodeNames.Add(ln.Id);
                    }
                }
                i++;
                if (i > depthLimit)
                {
                    run = false;
                    break;
                }
                next = newnext;
                run = next.Any();
            }

            return output;
        }

        /// <summary>
        /// Converts the specified source: from <c>T</c> to DirectedGraph
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="depthLimit">The depth limit.</param>
        /// <param name="rootNodes"></param>
        /// <returns></returns>
        public override DirectedGraph Convert(T source, int depthLimit = 500, IEnumerable<T> rootNodes = null)
        {
            DirectedGraph output = new DirectedGraph();
            if (source == null) return output;
            output.Title = source.name;

            String description = "DirectedGraph built from " + source.GetType().Name + ":GraphNodeBase graph";

            Int32 startLevel = source.level;

            var nodes = source.getAllChildren(null, false, false, 1, depthLimit).ConvertList<IObjectWithPathAndChildren, T>(); //.ConvertIList<IObjectWithPathAndChildren, T>();

            DirectedGraphStylingCase styleCase = GetStylingCase(nodes);

            Dictionary<String, Node> nodeDictionary = new Dictionary<string, Node>();

            foreach (var ch in nodes)
            {
                if (ch.level > (startLevel + depthLimit)) continue;

                T child = (T)ch;
                var gn = output.Nodes.AddNode(GetNodeID(child), GetNodeLabel(child));
                var tid = GetTypeID(child);
                Double w = GetNodeWeight(child);
                gn.Category = styleCase.Categories.AddOrGetCategory(tid.ToString(), "", "").Id;
                gn.Background = styleCase.nodeStyler.GetHexColor(w, tid);
                gn.StrokeThinkness = styleCase.nodeStyler.GetBorderThickness(w, tid);
                gn.Stroke = styleCase.nodeStyler.GetHexColor(w, tid);
                SetNodeCustomization(child, gn, styleCase);
            }

            foreach (var ch in nodes)
            {
                if (ch.level > (startLevel + depthLimit)) continue;
                if (ch.parent != null)
                {
                    T parent = (T)ch.parent;
                    T child = (T)ch;
                    if ((parent != null) && (child != null))
                    {
                        var tmp = GetLink(parent, child);
                        var l_tid = GetTypeID(child);
                        Double l_w = GetLinkWeight(parent, child);
                        output.Links.Add(tmp);
                        tmp.Category = styleCase.Categories.AddOrGetCategory(l_tid.ToString(), "", "").Id;
                        tmp.StrokeThinkness = styleCase.linkStyler.GetBorderThickness(l_w, l_tid);
                        tmp.Stroke = styleCase.linkStyler.GetHexColor(l_w, l_tid);
                        SetLinkCustomization(parent, child, tmp, styleCase);
                    }
                }
            }

            output.Layout = setup.GraphLayout; // DGML.enums.GraphLayoutEnum.Sugiyama;
            output.GraphDirection = setup.GraphDirection; // DGML.enums.GraphDirectionEnum.LeftToRight;

            return output;
        }

        public virtual void SetLinkCustomization(T parent, T child, Link link, DirectedGraphStylingCase styleCase)
        {
        }

        public virtual void SetNodeCustomization(T sourceNode, Node targetNode, DirectedGraphStylingCase styleCase)
        {
        }

        public override DirectedGraphStylingCase GetStylingCase(IEnumerable<T> nodes)
        {
            DirectedGraphStylingCase output = new DirectedGraphStylingCase(setup);
            foreach (var ch in nodes)
            {
                Int32 tid = GetTypeID(ch);
                Double weight = GetNodeWeight(ch);

                output.nodeStyler.learn(tid, weight);

                if (ch.parent != null)
                {
                    T parent = (T)ch.parent;
                    T child = (T)ch;
                    if ((parent != null) && (child != null))
                    {
                        var l_tid = GetTypeID(child);
                        Double l_w = GetLinkWeight(parent, child);
                        output.linkStyler.learn(l_tid, l_w);

                        var cl = output.Categories.AddOrGetCategory(GetTypeID(parent), GetCategoryID(parent), "");
                        output.Categories.AddUnique(cl);
                    }
                }

                Int32 cc = output.Categories.Count;

                var c = output.Categories.AddOrGetCategory(GetTypeID(ch), GetCategoryID(ch), "");

                if (cc != output.Categories.Count)
                {
                    c.Background = setup.colorWheel.next().ColorToHex();

                }
                output.Categories.AddUnique(c);
            }
            return output;
        }
    }
}