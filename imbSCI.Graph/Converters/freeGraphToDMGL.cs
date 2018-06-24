// --------------------------------------------------------------------------------------------------------------------
// <copyright file="freeGraphToDMGL.cs" company="imbVeles" >
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
using imbSCI.Data;
using imbSCI.Graph.Converters.tools;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.collections;
using imbSCI.Graph.DGML.core;
using imbSCI.Graph.FreeGraph;
using System;

namespace imbSCI.Graph.Converters
{
    public abstract class ToDGMLConverterBase
    {
        public GraphStylerSettings setup { get; set; }

        public CategoryCollection Categories { get; set; } = new CategoryCollection();

        public NodeWeightStylerCategories nodeStyler { get; set; }

        public NodeWeightStylerCategories linkStyler { get; set; }
    }

    /// <summary>
    /// Free graph to DMGL converter
    /// </summary>
    public class freeGraphToDMGL : ToDGMLConverterBase
    {
        public freeGraphToDMGL(GraphStylerSettings settings = null)
        {
            setup = settings;
            if (setup == null)
            {
                setup = imbSCI.Graph.config.imbSCIGraphConversionConfig.settings.DefaultGraphExportStyle;
            }
        }

        public DirectedGraph ConvertToDMGL(freeGraph input)
        {
            DirectedGraph output = new DirectedGraph();
            output.Title = input.Id;

            // input.InverseWeights(false, true);

            nodeStyler = new NodeWeightStylerCategories(setup.NodeGradient, setup);
            linkStyler = new NodeWeightStylerCategories(setup.LinkGradient, setup);

            foreach (freeGraphNodeBase node in input.nodes)
            {
                try
                {
                    if (node != null)
                    {
                        var c = Categories.AddOrGetCategory(node.type.ToString(), "", "");
                        output.Categories.AddUnique(c);
                        nodeStyler.learn(node.type, node.weight);
                    }
                }
                catch (Exception ex)
                {
                    output.ConversionErrors.Add("Node learning-conversion: " + ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }

            foreach (var link in input.links)
            {
                try
                {
                    if (link != null)
                    {
                        var c = Categories.AddOrGetCategory(link.type.ToString(), "", "");
                        output.Categories.AddUnique(c);
                        linkStyler.learn(link.type, link.weight);
                    }
                }
                catch (Exception ex)
                {
                    output.ConversionErrors.Add("link learning-conversion: " + ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }

            foreach (freeGraphNodeBase node in input.nodes)
            {
                try
                {
                    if (node != null)
                    {
                        var nd = output.Nodes.AddNode(node.name);

                        nd.Category = output.Categories[node.type].Id;
                        nd.Background = nodeStyler.GetHexColor(node.weight, node.type);
                        nd.StrokeThinkness = nodeStyler.GetBorderThickness(node.weight, node.type);
                        nd.Stroke = nodeStyler.GetHexColor(1, node.type);
                        nd.Label = nd.Label + " (" + node.weight.ToString(setup.NodeWeightFormat) + ")";
                        if (setup.doAddNodeTypeToLabel)
                        {
                            nd.Label = nd.Label + " [" + node.type + "]";
                        }
                    }
                }
                catch (Exception ex)
                {
                    output.ConversionErrors.Add("Node conversion: " + ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }

            foreach (var link in input.links)
            {
                try
                {
                    if (link != null)
                    {
                        var nodeA = input.GetNode(link.nodeNameA);
                        var nodeB = input.GetNode(link.nodeNameB);

                        var lnk = new Link(link.nodeNameA, link.nodeNameB, false);

                        if (setup.doLinkDirectionFromLowerTypeToHigher)
                        {
                            if (nodeB.type < nodeA.type)
                            {
                                lnk = new Link(link.nodeNameB, link.nodeNameA, false);
                            }
                        }

                        if (setup.doAddLinkWeightInTheLabel)
                        {
                            lnk.Label = lnk.Label.add(link.weight.ToString(setup.LinkWeightFormat));
                        }
                        else
                        {
                            lnk.Label = link.linkLabel;
                        }

                        lnk.Category = output.Categories[link.type].Id;
                        lnk.Stroke = linkStyler.GetHexColor(link.weight, link.type);
                        lnk.StrokeThinkness = linkStyler.GetBorderThickness(link.weight, link.type);
                        output.Links.Add(lnk);
                    }
                }
                catch (Exception ex)
                {
                    output.ConversionErrors.Add("Link conversion: " + ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }

            output.Layout = DGML.enums.GraphLayoutEnum.ForceDirected;
            output.GraphDirection = DGML.enums.GraphDirectionEnum.Sugiyama;

            return output;
        }
    }
}