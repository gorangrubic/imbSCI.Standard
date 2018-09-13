// --------------------------------------------------------------------------------------------------------------------
// <copyright file="freeGraph.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.data;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Core.reporting;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace imbSCI.Graph.FreeGraph
{
    /// <summary>
    /// Undirected graph structure, defined by <see cref="freeGraphNodeBase"/> and <see cref=freeGraphLinkBase"/>
    /// </summary>
    public class freeGraph : IObjectWithName
    {
        public String name { get; set; } = "freeGraph";

        /// <summary>
        /// Normalizes the node weights.
        /// </summary>
        /// <param name="minToZero">if set to <c>true</c> it will remap minimum to zero, otherwise it will just scale down to have max at 1.</param>
        public void normalizeNodeWeights(Boolean minToZero = false)
        {
            Double max = Double.MinValue;
            Double min = Double.MaxValue;

            foreach (var node in nodes)
            {
                max = Math.Max(node.weight, max);
                min = Math.Min(node.weight, min);
            }

            Double range = max - min;

            foreach (var node in nodes)
            {
                if (minToZero)
                {
                    node.weight = (node.weight - min).GetRatio(range);
                }
                else
                {
                    node.weight = (node.weight).GetRatio(max);
                }
            }
        }

        /// <summary>
        /// Normalizes the link weights.
        /// </summary>
        /// <param name="minToZero">if set to <c>true</c> [minimum to zero].</param>
        public void normalizeLinkWeights(Boolean minToZero = false)
        {
            Double max = Double.MinValue;
            Double min = Double.MaxValue;

            foreach (var link in links)
            {
                max = Math.Max(link.weight, max);
                min = Math.Min(link.weight, min);
            }

            Double range = max - min;

            foreach (var link in links)
            {
                if (minToZero)
                {
                    link.weight = (link.weight - min).GetRatio(range);
                }
                else
                {
                    link.weight = (link.weight).GetRatio(max);
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public String Id
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String description { get; set; } = "";

        /// <summary>
        /// When weights are inversed then link weights are between 0 and 1. If the weights are not inversed then weights are above 1
        /// </summary>
        /// <value>
        ///   <c>true</c> if [link weights inversed]; otherwise, <c>false</c>.
        /// </value>
        public Boolean LinkWeightsInversed { get; set; } = false;

        /// <summary>
        /// Detects if link weights are inversed.
        /// </summary>
        /// <param name="saveResultAsState">if set to <c>true</c> [save result as state].</param>
        /// <returns></returns>
        public Boolean DetectIfLinkWeightsAreInversed(Boolean saveResultAsState = true)
        {
            var min = Double.MaxValue;
            var max = Double.MinValue;

            foreach (freeGraphLinkBase link in links)
            {
                min = Math.Min(min, link.weight);
                max = Math.Max(max, link.weight);
            }

            var range = max - min;

            Boolean o = false;

            if (max > 1)
            {
                if (saveResultAsState) LinkWeightsInversed = o;
                return o;
            }
            else
            {
                o = true;
                if (saveResultAsState) LinkWeightsInversed = o;
            }
            return o;
        }

        /// <summary>
        /// Inversing the link weights, When state is Inversed, weights are between 0 and 1, when Inversed is false - the weights are from 1 to infinite
        /// </summary>
        /// <param name="toState">State of inversion that is desired.</param>
        /// <param name="detectFirst">if set to <c>true</c> [detect first].</param>
        /// <returns>Final state of inversion</returns>
        public Boolean InverseWeights(Boolean toState, Boolean detectFirst = true)
        {
            if (detectFirst) DetectIfLinkWeightsAreInversed(true);

            if (LinkWeightsInversed == toState) return LinkWeightsInversed;

            foreach (freeGraphLinkBase link in links)
            {
                link.weight = 1.GetRatio(link.weight);
            }

            LinkWeightsInversed = toState;
            return LinkWeightsInversed;
        }

        public void SetNotReady()
        {
            nodeDictionary.Clear();
        }

        public freeGraphLink GetLinkInstance(freeGraphLinkBase link, Boolean skipCheck = true)
        {
            freeGraphLink output = new freeGraphLink(link.GetClone());

            output.nodeA = GetNode(link.nodeNameA, skipCheck);

            if (output.nodeA != null) output.nodeA = output.nodeA.GetQueryResultClone(0);

            output.nodeB = GetNode(link.nodeNameB, skipCheck);
            if (output.nodeB != null) output.nodeB = output.nodeB.GetQueryResultClone(0);

            return output;
        }

        /// <summary>
        /// Counts the links involving the specified nodeName
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="AtoB">if set to <c>true</c> [ato b].</param>
        /// <param name="BtoA">if set to <c>true</c> [bto a].</param>
        /// <returns></returns>
        public Int32 CountLinks(String nodeName, Boolean AtoB = true, Boolean BtoA = true)
        {
            Int32 output = 0;

            if (AtoB)
            {
                output += links.Count(x => x.nodeNameA == nodeName);
            }

            if (BtoA)
            {
                output += links.Count(x => x.nodeNameB == nodeName);
            }

            return output;
        }

        /// <summary>
        /// Gets the links.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="AtoB">if set to <c>true</c> [ato b].</param>
        /// <param name="BtoA">if set to <c>true</c> [bto a].</param>
        /// <param name="nodeWeightFactor">The node weight factor.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="skipCheck">if set to <c>true</c> [skip check].</param>
        /// <param name="initialWeightFromParent">if set to <c>true</c> [initial weight from parent].</param>
        /// <returns></returns>
        public freeGraphNodeAndLinks GetLinks(String nodeName, Boolean AtoB = true, Boolean BtoA = true, Double nodeWeightFactor = 1.0, Int32 nodeType = 0, Boolean skipCheck = true, Boolean initialWeightFromParent = true)
        {
            //if (!nodeDictionary.ContainsKey(nodeName)) return null;

            freeGraphNodeAndLinks output = new freeGraphNodeAndLinks();
            List<freeGraphLinkBase> lnks = new List<freeGraphLinkBase>();
            if (skipCheck)
            {
                if (AtoB) lnks.AddRange(links.Where(x => x.nodeNameA.Equals(nodeName)));
                if (BtoA) lnks.AddRange(links.Where(x => x.nodeNameB.Equals(nodeName)));
            }
            else
            {
                lnks = linkRegistry.GetLinks(nodeName, AtoB, BtoA);
            }

            output.node = GetNode(nodeName, skipCheck);

            foreach (freeGraphLinkBase link in lnks)
            {
                freeGraphLink link_instance = GetLinkInstance(link, skipCheck);
                if (link_instance.IsReady)
                {
                    if (link_instance.nodeA.name != nodeName)
                    {
                        if (initialWeightFromParent)
                        {
                            link_instance.nodeA.weight = link_instance.nodeA.weight * nodeWeightFactor;
                        }
                        else
                        {
                            link_instance.nodeA.weight = 1 * nodeWeightFactor;
                        }
                    }
                    if (link_instance.nodeB.name != nodeName)
                    {
                        if (initialWeightFromParent)
                        {
                            link_instance.nodeB.weight = link_instance.nodeB.weight * nodeWeightFactor;
                        }
                        else
                        {
                            link_instance.nodeB.weight = 1 * nodeWeightFactor;
                        }
                    }
                    if (link_instance.nodeA.name != nodeName) link_instance.nodeA.type = nodeType;
                    if (link_instance.nodeB.name != nodeName) link_instance.nodeB.type = nodeType;
                    output.Add(link_instance);
                    if (!output.linkedNodeClones.ContainsKey(link_instance.nodeA.name)) output.linkedNodeClones.Add(link_instance.nodeA.name, link_instance.nodeA);
                    if (!output.linkedNodeClones.ContainsKey(link_instance.nodeB.name)) output.linkedNodeClones.Add(link_instance.nodeB.name, link_instance.nodeB);
                }
            }

            return output;
        }

        public freeGraph()
        {
        }

        /// <summary>
        /// Does nothing (by default implementation) -- should be called before saving to xml
        /// </summary>
        /// <param name="folder">The folder.</param>
        public virtual void OnBeforeSave(folderNode folder)
        {
        }

        /// <summary>
        /// Does nothing (by default implementation) -- should be called after the graph is loaded
        /// </summary>
        /// <param name="folder">The folder.</param>
        public virtual void OnAfterLoad(folderNode folder)
        {
        }

        /// <summary>
        /// Clones the graph into specified inherited type of freeGraph
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callCheck">if set to <c>true</c> [call check].</param>
        /// <returns></returns>
        public T CloneIntoType<T>(Boolean callCheck = true) where T : freeGraph, new()
        {
            T output = new T();
            output.Id = Id;
            output.description = description;
            foreach (freeGraphNodeBase node in nodes)
            {
                output.nodes.Add(node.GetQueryResultClone(0));
            }
            foreach (freeGraphLinkBase link in links)
            {
                output.links.Add(link.GetClone());
            }
            if (callCheck)
            {
                output.Check();
            }
            return output;
        }

        public static T Load<T>(String filepath, Boolean createNewIfNotFound = true) where T : freeGraph, new()
        {
            T output = default(T);
            if (File.Exists(filepath))
            {
                output = objectSerialization.loadObjectFromXML<T>(filepath);
            }
            FileInfo fi = new FileInfo(filepath);
            if (createNewIfNotFound)
            {
                if (output == null) output = new T();
            }
            if (output != null)
            {
                output.RebuildIndex();
                output.OnAfterLoad(fi.Directory);
            }

            return output;
        }

        /// <summary>
        /// Adds new node with <c>nameProposal</c> name or a modified version of the name - in order to have unique node name
        /// </summary>
        /// <param name="nameProposal">The name proposal.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public freeGraphNodeBase AddNewNode(String nameProposal, Double weight = 1, Int32 type = 0)
        {
            Int32 c = 0;
            if (nameProposal.isNullOrEmpty()) nameProposal = "Node" + nodes.Count();
            String proposal = nameProposal;

            Int32 nc = nodes.Count(x => x.name.StartsWith(nameProposal));
            if (nc > 0)
            {
                c = nc;
                proposal = nameProposal + c.ToString("D3");
            }

            while (ContainsNode(proposal, true))
            {
                c++;
                proposal = nameProposal + c.ToString("D3");
            }
            freeGraphNodeBase node = new freeGraphNodeBase();
            node.name = proposal;
            node.weight = weight;
            node.type = type;
            nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Adds new node under <c>nodeName</c> or just returns if any existing
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public freeGraphNodeBase AddNode(String nodeName, Double weight = 1, Int32 type = 0)
        {
            if (ContainsNode(nodeName, true))
            {
                return GetNode(nodeName, true);
            }
            else
            {
                freeGraphNodeBase node = new freeGraphNodeBase();
                node.name = nodeName;
                node.weight = weight;
                node.type = type;
                nodes.Add(node);
                return node;
            }
        }

        public freeGraphLinkBase AddLink(String nodeNameA, String nodeNameB, Double weight = 1, Int32 type = 0)
        {
            if (!ContainsLink(nodeNameA, nodeNameB))
            {
                freeGraphLinkBase link = new freeGraphLinkBase();
                link.nodeNameA = nodeNameA;
                link.nodeNameB = nodeNameB;
                link.weight = weight;
                link.type = type;
                links.Add(link);
                return link;
            }

            return GetLink(nodeNameA, nodeNameB);
        }

        /// <summary>
        /// Adds new node into graph or sums weight of the specified and existing - and applies type that is greater
        /// </summary>
        /// <param name="link">The link.</param>
        public freeGraphNodeBase AddNodeOrSum(String _name, Double _weight, Int32 _type)
        {
            if (!ContainsNode(_name))
            {
                return AddNode(_name, _weight, _type);
            }
            else
            {
                var exLink = GetNode(_name); //GetLink(link.nodeNameA, link.nodeNameB);
                if (exLink != null)
                {
                    Double w = exLink.weight + _weight;
                    exLink.weight = w;
                    exLink.type = Math.Max(exLink.type, _type);
                }
                return exLink;
            }
        }

        /// <summary>
        /// Adds new node into graph or sums weight of the specified and existing - and applies type that is greater
        /// </summary>
        /// <param name="link">The link.</param>
        public void AddNodeOrSum(freeGraphNodeBase node)
        {
            if (!ContainsNode(node.name))
            {
                AddNode(node.name, node.weight, node.type);
            }
            else
            {
                var exLink = GetNode(node.name); //GetLink(link.nodeNameA, link.nodeNameB);
                if (exLink != null)
                {
                    Double w = exLink.weight + node.weight;
                    exLink.weight = w;
                    exLink.type = Math.Max(exLink.type, node.type);
                }
            }
        }

        /// <summary>
        /// Adds new link into graph or sums weight of the specified and existing - and applies type that is greater
        /// </summary>
        /// <param name="link">The link.</param>
        public void AddLinkOrSum(freeGraphLinkBase link)
        {
            if (!ContainsLink(link.nodeNameA, link.nodeNameB, true))
            {
                AddLink(link.nodeNameA, link.nodeNameB, link.weight, link.type);
            }
            else
            {
                var exLink = GetLink(link.nodeNameA, link.nodeNameB);
                if (exLink != null)
                {
                    exLink.weight += link.weight;
                    exLink.type = Math.Max(exLink.type, link.type);
                }
                else
                {
                    AddLink(link.nodeNameA, link.nodeNameB, link.weight, link.type);
                }
            }
        }

        /// <summary>
        /// Merges specified cloud into this one by summing overlaping node and link weights and picks type that is greater
        /// </summary>
        /// <param name="x">The x.</param>
        public void AddCloud(freeGraph x)
        {
            foreach (var i in x.nodes)
            {
                AddNodeOrSum(i);
            }

            foreach (var i in x.links)
            {
                AddLinkOrSum(i);
            }
        }

        /// <summary>
        /// Gets link in one or the other direction, otherwise returns null;
        /// </summary>
        /// <param name="nodeNameA">The node name a.</param>
        /// <param name="nodeNameB">The node name b.</param>
        /// <returns></returns>
        public freeGraphLinkBase GetLink(String nodeNameA, String nodeNameB, Boolean includeBtoALinks = false)
        {
            freeGraphLinkBase output = null;
            if (IsReady)
            {
                linkRegistry.GetLink(nodeNameA, nodeNameB);
            }
            else
            {
                output = links.FirstOrDefault(x => (x.nodeNameA == nodeNameA) && (x.nodeNameB == nodeNameB));
                if ((output == null) && includeBtoALinks)
                {
                    output = links.FirstOrDefault(x => (x.nodeNameB == nodeNameA) && (x.nodeNameA == nodeNameB));
                }
            }
            return output;
        }

        public Boolean ContainsLink(String nodeNameA, String nodeNameB, Boolean includeBtoALinks = false)
        {
            Boolean output = false;
            if (IsReady)
            {
                output = linkRegistry.ContainsLink(nodeNameA, nodeNameB, includeBtoALinks);
            }
            else
            {
                output = links.Any(x => (x.nodeNameA == nodeNameA) && (x.nodeNameB == nodeNameB));
                if (!output && includeBtoALinks)
                {
                    output = links.Any(x => (x.nodeNameB == nodeNameA) && (x.nodeNameA == nodeNameB));
                }
            }
            return output;
        }

        /// <summary>
        /// Iterative query for linked nodes.
        /// </summary>
        /// <param name="queryNodeNames">The query node names.</param>
        /// <param name="expansionSteps">The expansion steps - number of iterations in linked nodes collecting process</param>
        /// <param name="includeBtoAlinks">if set to <c>true</c> [include bto alinks].</param>
        /// <param name="includeQueryNodesInResult">if set to <c>true</c> [include query nodes in result].</param>
        /// <returns></returns>
        public freeGraphQueryResult GetLinkedNodes(IEnumerable<String> queryNodeNames, Int32 expansionSteps = 1, Boolean includeBtoAlinks = false, Boolean includeQueryNodesInResult = false, Boolean cloneAndAdjustWeight = true)
        {
            var output = new freeGraphQueryResult();
            if (!Check())
            {
                output.graphNotReady = true;
                return output;
            }
            List<freeGraphNodeBase> queryNodes = GetNodes(queryNodeNames, true).GetQueryResultClones(0);
            output.queryNodes.AddRange(queryNodes);

            if (includeQueryNodesInResult)
            {
                output.AddNewNodes(queryNodes);
            }

            Int32 step = 0;
            while (step < expansionSteps)
            {
                List<freeGraphNodeBase> expansion = new List<freeGraphNodeBase>();

                foreach (var node in queryNodes)
                {
                    expansion.AddRange(GetLinksBase(node.name, false, 1, cloneAndAdjustWeight));
                    if (includeBtoAlinks) expansion.AddRange(GetLinksBase(node.name, true, 1, cloneAndAdjustWeight));
                }

                queryNodes = output.AddNewNodes(expansion);
                step++;
            }
            return output;
        }

        /// <summary>
        /// Gets the linked nodes.
        /// </summary>
        /// <param name="centralNodeName">Name of the central node.</param>
        /// <param name="includeBtoALinks">if set to <c>true</c> [include bto a links].</param>
        /// <returns></returns>
        public freeGraphQueryResult GetLinkedNodes(String centralNodeName, Boolean includeBtoALinks = false, freeGraphQueryResult output = null)
        {
            if (output == null) output = new freeGraphQueryResult();
            if (!Check())
            {
                output.graphNotReady = true;
                return output;
            }
            freeGraphNodeBase centralNode = GetNode(centralNodeName, true);
            output.queryNodes.Add(centralNode);
            if (!output.graphNodeNotFound)
            {
                output.AddRange(GetLinksBase(centralNodeName, false, 1, true));
                output.includeBtoALinks = includeBtoALinks;
                if (output.includeBtoALinks)
                {
                    output.AddRange(GetLinksBase(centralNodeName, true, 1, true));
                }
            }
            return output;
        }

        /// <summary>
        /// Gets the links base.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="BtoA">if set to <c>true</c> [bto a].</param>
        /// <param name="distanceCorrection">The distance correction.</param>
        /// <param name="cloneAndAdjustWeight">if set to <c>true</c> [clone and adjust weight].</param>
        /// <returns></returns>
        protected List<freeGraphNodeBase> GetLinksBase(String nodeName, Boolean BtoA = false, Int32 distanceCorrection = 0, Boolean cloneAndAdjustWeight = true) => linkRegistry.GetLinkedNodesBase(nodeName, BtoA, distanceCorrection, cloneAndAdjustWeight);

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <param name="nodeNames">The node names.</param>
        /// <param name="skipCheck">if set to <c>true</c> [skip check].</param>
        /// <returns></returns>
        public List<freeGraphNodeBase> GetNodes(IEnumerable<String> nodeNames, Boolean skipCheck = false)
        {
            List<freeGraphNodeBase> queryNodes = new List<freeGraphNodeBase>();
            if (!skipCheck) Check();

            foreach (String nodeName in nodeNames)
            {
                freeGraphNodeBase node = GetNode(nodeName, true);
                if (node != null)
                {
                    queryNodes.Add(node);
                }
            }
            return queryNodes;
        }

        /// <summary>
        /// Removes nodes and related links
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="skipCheck">if set to <c>true</c> [skip check].</param>
        /// <returns></returns>
        public Boolean Remove(String nodeName, Boolean skipCheck = false, ILogBuilder logger = null)
        {
            if (!skipCheck) Check();
            if (IsReady)
            {
                if (!nodeDictionary.ContainsKey(nodeName)) return false;
                var node = nodeDictionary[nodeName];
                nodeDictionary.Remove(nodeName);

                linkRegistry.UnregisterLink(nodeName, logger);
            }
            Int32 nds = nodes.RemoveAll(x => x.name == nodeName);
            links.RemoveAll(x => x.nodeNameA == nodeName || x.nodeNameB == nodeName);
            return nds > 0;
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="skipCheck">if set to <c>true</c> [skip check].</param>
        /// <returns></returns>
        public freeGraphNodeBase GetNode(String name, Boolean skipCheck = false)
        {
            if (!skipCheck) Check();
            if (IsReady)
            {
                if (nodeDictionary.ContainsKey(name))
                {
                    return nodeDictionary[name];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return nodes.FirstOrDefault(x => x.name.Equals(name));
            }
        }

        /// <summary>
        /// Determines whether contains node, with the specified name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if  contains node, with the specified name otherwise, <c>false</c>.
        /// </returns>
        public Boolean ContainsNode(String name, Boolean skipCheck = false)
        {
            if (!skipCheck) Check();
            if (name.isNullOrEmpty()) return false;
            if (IsReady)
            {
                return nodeDictionary.ContainsKey(name);
            }
            else
            {
                return nodes.Any(x => x.name.Equals(name));
            }
        }

        public virtual void RebuildIndex()
        {
            nodeDictionary.Clear();
            linkRegistry.Clear();
            Check();
        }

        public Boolean DisableCheck { get; set; } = false;

        private Object checkLock { get; set; } = new object();

        protected Boolean Check()
        {
            if (DisableCheck) return false;

            if (!IsReady)
            {
                lock (checkLock)
                {
                    if (!IsReady)
                    {
                        foreach (freeGraphNodeBase node in nodes)
                        {
                            if (!nodeDictionary.ContainsKey(node.name))
                            {
                                nodeDictionary.Add(node.name, node);
                            }
                            else
                            {
                                //nodes.Remove(node);
                            }
                        }
                        foreach (freeGraphLinkBase link in links)
                        {
                            if (linkRegistry.RegisterLink(link, nodeDictionary))
                            {
                                //links.Remove(link);
                            }
                        }
                    }
                }
            }
            return IsReady;
        }

        protected Boolean IsReady
        {
            get
            {
                if (DisableCheck) return false;
                if (nodeDictionary.Any()) return true;
                if (linkRegistry.Any()) return true;
                return false;
            }
        }

        protected Dictionary<String, freeGraphNodeBase> nodeDictionary { get; set; } = new Dictionary<string, freeGraphNodeBase>();
        protected freeGraphLinkRegistry linkRegistry { get; set; } = new freeGraphLinkRegistry();

        public void Clear()
        {
            nodes.Clear();
            links.Clear();
            nodeDictionary.Clear();
            linkRegistry.Clear();
        }

        public Boolean Any()
        {
            return nodes.Any();
        }

        public Int32 CountLinks()
        {
            return links.Count;
        }

        public Int32 CountNodes()
        {
            return nodes.Count;
        }

        public List<freeGraphNodeBase> GetOverlap(freeGraph second)
        {
            List<freeGraphNodeBase> output = new List<freeGraphNodeBase>();

            foreach (freeGraphNodeBase node in nodes)
            {
                if (second.ContainsNode(node.name, true))
                {
                    output.Add(node);
                }
            }

            return output;
        }

        /// <summary>
        /// List of nodes, used only for serialization and deserialization
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public List<freeGraphNodeBase> nodes { get; set; } = new List<freeGraphNodeBase>();

        /// <summary>
        /// List of links, used only for serialization and deserialization
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public List<freeGraphLinkBase> links { get; set; } = new List<freeGraphLinkBase>();
    }
}