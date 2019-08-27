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
using System.Data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.math;
using imbSCI.Core.style.color;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.Graph.Converters.tools;
using imbSCI.Graph.Data;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;

namespace imbSCI.Graph.Converters
{
    public class graphDataToDirectedGraphConverter : DirectedGraphConverterBase<graphData, graphData>
    {
        public graphDataToDirectedGraphConverter() : base()
        {
            setup = new GraphStylerSettings();

        }


        public override DirectedGraph Convert(graphData source, int depthLimit = 500, IEnumerable<graphData> rootNodes = null)
        {
            

            DirectedGraph output = new DirectedGraph();
            if (source == null) return output;

            output.Title = source.Schema.GetTitle();


            String description = source.Schema.GetDescription();  //"DirectedGraph built from " + source.GetType().Name + ":GraphNodeBase graph";

            Int32 startLevel = source.level;

            List<IObjectWithPathAndChildren> nodes = source.getAllChildren(); //.getAllChildrenInType<graphData>(null, false, true, 1, depthLimit);//source.getAllChildren(null, false, false, 1, depthLimit).ConvertList<IObjectWithPathAndChildren, IGraphNode>(); //.ConvertIList<IObjectWithPathAndChildren, T>();
            List<graphData> graphDataNodes = new List<graphData>();
            nodes.ForEach(x => graphDataNodes.Add(x as graphData));

            DirectedGraphStylingCase styleCase = GetStylingCase(graphDataNodes);

            Dictionary<String, Node> nodeDictionary = new Dictionary<string, Node>();

            foreach (graphData child in graphDataNodes)
            {


                var dc = child.Column;
                if (child.level > (startLevel + depthLimit)) continue;

                
                var gn = output.Nodes.AddNode(GetNodeID(child), child.Presenter.GetNodeLabel(child));
                gn.Label = child.Presenter.GetNodeLabel(child); //GetNodeLabel(child);
                var tid = GetTypeID(child);
                Double w = GetNodeWeight(child);

                gn.Category = child.Presenter.GetNodeCategory(child); //dc.GetGroup(); //styleCase.Categories.AddOrGetCategory(tid.ToString(), "", "").Id;
                gn.Background = child.Presenter.GetNodeColor(child);  //dc.GetDefaultBackground().ColorToHex() ; // styleCase.nodeStyler.GetHexColor(w, tid);

                gn.StrokeThinkness = styleCase.nodeStyler.GetBorderThickness(w, tid);

                gn.Stroke = dc.GetTextColor().ColorToHex(); // styleCase.nodeStyler.GetHexColor(w, tid);
                SetNodeCustomization(child, gn, styleCase);
            }

            foreach (graphData ch in graphDataNodes)
            {
                if (ch.level > (startLevel + depthLimit)) continue;
                if (ch.parent != null)
                {
                    graphData parent = ch.parent as graphData;
                    graphData child = ch;
                    if ((parent != null) && (child != null))
                    {
                        Link tmp = GetLink(parent, child);
                        tmp.Label = GetLinkLabel(parent, child);
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

        public override graphData Convert(DirectedGraph source, int depthLimit = 500, IEnumerable<GraphElement> rootNodes = null)
        {
            throw new System.NotImplementedException();
        }

        //protected graphData GetGraphData(graphData nodeOrLink)
        //{
        //    graphData gData = nodeOrLink.root as graphData;
        //    return gData;
        //}

        public override string GetNodeLabel(graphData node)
        {
            return node.Presenter.GetNodeLabel(node);
            
        }

        public override string GetCategoryID(graphData node)
        {
            return node.Presenter.GetNodeCategory(node);
        }

        public override double GetLinkWeight(graphData nodeA, graphData nodeB)
        {
            return nodeA.GetLinkWeight(nodeB);
        }

        public override string GetNodeID(graphData node)
        {
            return node.GetUID(node);
        }

        public override double GetNodeWeight(graphData node)
        {
            return node.Weight;
        }

        public override DirectedGraphStylingCase GetStylingCase(IEnumerable<graphData> source)
        {
            DirectedGraphStylingCase output = new DirectedGraphStylingCase(setup);
            graphData gData = null;
            foreach (graphData ch in source)
            {
                
                Int32 tid = GetTypeID(ch);
                Double weight = GetNodeWeight(ch);

                output.nodeStyler.learn(tid, weight);

                if (ch.parent != null)
                {
                    graphData parent = ch.parent as graphData;
                    graphData child = ch;
                    if ((parent != null) && (child != null))
                    {
                        var l_tid = GetTypeID(child);
                        Double l_w = GetLinkWeight(parent, child);
                        output.linkStyler.learn(l_tid, l_w);

                        var cl = output.Categories.AddOrGetCategory(GetTypeID(parent), GetCategoryID(parent), "");
                        output.Categories.AddUnique(cl);
                    }
                }

                var c = output.Categories.AddOrGetCategory(GetTypeID(ch), GetCategoryID(ch), "");
                output.Categories.AddUnique(c);
            }
            return output;
        }

        public override int GetTypeID(graphData gData)
        {
            //graphData gData = GetGraphData(nodeOrLink);

            
            return (int)gData[gData].GetImportance();
        }

        public virtual void SetLinkCustomization(graphData parent, graphData child, Link link, DirectedGraphStylingCase styleCase)
        {
            

        }

        public virtual void SetNodeCustomization(graphData sourceNode, Node targetNode, DirectedGraphStylingCase styleCase)
        {
            
        }
    }
}