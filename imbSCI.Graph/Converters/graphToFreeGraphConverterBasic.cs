// --------------------------------------------------------------------------------------------------------------------
// <copyright file="graphToFreeGraphConverterBasic.cs" company="imbVeles" >
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
using imbSCI.Core.math;
using imbSCI.Data;
using imbSCI.Data.collection.graph;

// using imbNLP.PartOfSpeech.TFModels.semanticCloud.core;
using imbSCI.Graph.FreeGraph;
using System;
using System.Collections.Generic;
using System.Linq;

//using System.Web.UI.WebControls;
//using Accord;

namespace imbSCI.Graph.Converters
{
    /// <summary>
    /// Basic <see cref="IGraphNode"/> to <see cref="freeGraph"/> converter
    /// </summary>
    /// <typeparam name="T">Base type of <see cref="IGraphNode"/> this converter interprets</typeparam>
    public class graphToFreeGraphConverterBasic<T> : graphToGraphConverterBase<T, freeGraph, T, freeGraphNodeBase> where T : IGraphNode, new()
    {
        /// <summary>
        /// Gets the type of the node - by default implementation returns <see cref="IGraphNode.level"/>
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public virtual Int32 GetNodeType(T node)
        {
            if (node == null)
            {
                return 0;
            }
            return node.level;
        }

        /// <summary>
        /// Gets the node weight - by default implementation it is 1 / <see cref="IGraphNode.level"/>
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public override Double GetNodeWeight(T node)
        {
            if (node == null)
            {
                return 0;
            }
            return 1.GetRatio(node.level);
        }

        /// <summary>
        /// Provides the link weight, by default implementation returns 1 / <see cref="IGraphNode.Count"/>
        /// </summary>
        /// <param name="nodeA">The node a.</param>
        /// <param name="nodeB">The node b.</param>
        /// <returns></returns>
        public override Double GetLinkWeight(T nodeA, T nodeB)
        {
            if (nodeA == null)
            {
                return 0;
            }
            return 1.GetRatio(nodeA.Count());
        }

        /// <summary>
        /// Provides type of link between nodeA and nodeB
        /// </summary>
        /// <param name="nodeA">The node a.</param>
        /// <param name="nodeB">The node b.</param>
        /// <returns></returns>
        public virtual Int32 GetLinkType(T nodeA, T nodeB)
        {
            if (nodeA == null)
            {
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// Converts the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="depthLimit">The depth limit.</param>
        /// <returns></returns>
        public override freeGraph Convert(T source, Int32 depthLimit = 500, IEnumerable<T> rootNodes = null)
        {
            freeGraph output = new freeGraph();
            output.DisableCheck = true;
            output.Id = source.name;
            output.description = "FreeGraph built from " + source.GetType().Name + ":GraphNodeBase graph";

            var nodes = source.getAllChildren(null, false, false, 1, depthLimit, true);

            Dictionary<T, freeGraphNodeBase> nodeDict = new Dictionary<T, freeGraphNodeBase>();

            foreach (var ch in nodes)
            {
                T child = (T)ch;
                var node = output.AddNewNode(GetNodeName(child), GetNodeWeight(child), GetNodeType(child));
                nodeDict.Add(child, node);
            }

            foreach (var ch in nodes)
            {
                if (ch.parent != null)
                {
                    T parent = (T)ch.parent;
                    T child = (T)ch;
                    if ((parent != null) && (child != null))
                    {
                        freeGraphNodeBase gParent = nodeDict[parent];
                        freeGraphNodeBase gChild = nodeDict[child];
                        var lnk = output.AddLink(gParent.name, gChild.name, GetLinkWeight(parent, child), GetLinkType(parent, child));
                        lnk.linkLabel = GetLinkLabel(parent, child);
                    }
                }
            }
            output.DisableCheck = false;
            output.RebuildIndex();
            return output;
        }

        public override T Convert(freeGraph source, int depthLimit = 500, IEnumerable<freeGraphNodeBase> rootNodes = null)
        {
            T output = new T();

            output.name = source.Id;

            if (!source.Any()) return output;

            List<T> nodes = new List<T>();
            List<String> nodeNames = new List<string>();

            Dictionary<T, freeGraphNodeBase> next = new Dictionary<T, freeGraphNodeBase>();

            Boolean run = true;

            foreach (var io in rootNodes)
            {
                var tmp = new T();
                tmp.name = io.name;

                next.Add(tmp, io);
                output.Add(tmp);
            }

            //List<Node> next = new List<Node>();

            // next.AddRange(rootNodes.ConvertList<IObjectWithName,Node>());

            Int32 i = 0;
            while (run)
            {
                Dictionary<T, freeGraphNodeBase> newnext = new Dictionary<T, freeGraphNodeBase>();
                // List<T> newT = new ListItemType<();

                foreach (var pair in next)
                {
                    T tNode = new T();

                    tNode.name = pair.Value.name; // g.Id;

                    foreach (var ln in source.GetLinkedNodes(pair.Value.name, false))
                    {
                        if (!nodeNames.Contains(ln.name))
                        {
                            var tmp = new T();
                            tmp.name = ln.name;

                            newnext.Add(tmp, ln);
                            pair.Key.Add(tmp);
                        }
                        nodeNames.Add(ln.name);
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

        //public override double GetLinkWeight(T nodeA, T nodeB)
        //{
        //    return 1.GetRatio(nodeA.Count() + 1);
        //}

        //public override double GetNodeWeight(T node)
        //{
        //    return ((node.Count() + 1) * (1.GetRatio(node.level)));
        //}
    }
}