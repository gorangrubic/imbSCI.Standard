// --------------------------------------------------------------------------------------------------------------------
// <copyright file="freeGraphReport.cs" company="imbVeles" >
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
using imbSCI.Core.attributes;
using imbSCI.Core.extensions.io;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Data;
using imbSCI.Data.collection.nested;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace imbSCI.Graph.FreeGraph
{
    /// <summary>
    /// Report on free graph
    /// </summary>
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithNameAndDescription" />
    public class freeGraphReport : IObjectWithNameAndDescription
    {
        /// <summary>
        /// Saves the report to the <c>folder</c> using specified filename or default: "analysis_[<see cref="name" />].xml"
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filename">The filename.</param>
        public void Save(folderNode folder, String filename = "")
        {
            if (filename == "") filename = "analysis_" + name.getCleanFilepath(".xml");
            filename = filename.ensureEndsWith(".xml");

            String p = folder.pathFor(filename, Data.enums.getWritableFileMode.overwrite, description);
            objectSerialization.saveObjectToXML(this, p);
        }

        /// <summary> Name of the report </summary>
        [Category("Label")]
        [DisplayName("Name")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Name of the report")] // [imb(imbAttributeName.reporting_escapeoff)]
        [imb(imbAttributeName.reporting_columnWidth, 20)]
        public String name { get; set; } = "";

        [Category("Node Weights")]
        [imb(imbAttributeName.reporting_hide)]
        public List<freeGraphIsland> islands { get; set; } = new List<freeGraphIsland>();

        //public freeGraphReport() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="freeGraphReport"/> class and performs analysis on <c>graph</c>
        /// </summary>
        /// <param name="graph">The graph.</param>
        public freeGraphReport(freeGraph graph)
        {
            Deploy(graph);
        }

        /// <summary>
        /// Populates the graph report
        /// </summary>
        /// <param name="graph">The graph.</param>
        public void Deploy(freeGraph graph)
        {
            name = graph.name;

            description = "Analysis of graph [" + graph.name + "] structure and other key metrics: " + graph.description;

            List<Double> ws = new List<double>();
            //instanceCountCollection<freeGraphNodeBase> linkPerNodeFrequency = new instanceCountCollection<freeGraphNodeBase>();
            aceDictionarySet<Int32, freeGraphNodeBase> nodesByNumberOfLinks = new aceDictionarySet<int, freeGraphNodeBase>();
            foreach (var node in graph.nodes)
            {
                ws.Add(node.weight);

                Int32 lc = graph.CountLinks(node.name, true, true);
                nodesByNumberOfLinks.Add(lc, node);
            }

            TotalWeight = ws.Sum();
            AvgWeight = ws.Average();
            StdWeight = ws.GetStdDeviation(false);

            NodeCount = graph.nodes.Count;
            LinkRatio = graph.links.GetRatio(graph.nodes);

            List<String> processed = new List<String>();
            List<Int32> linkFrequencies = nodesByNumberOfLinks.Keys.OrderByDescending(x => x).ToList();

            Int32 maxLinkFreq = linkFrequencies.Max();

            foreach (Int32 freq in linkFrequencies)
            {
                List<freeGraphNodeBase> nextSet = nodesByNumberOfLinks[freq].ToList();
                foreach (freeGraphNodeBase n in nextSet)
                {
                    if (!processed.Contains(n.name))
                    {
                        var result = graph.GetLinkedNodes(new String[] { n.name }, 100, true, true, false);
                        freeGraphIsland island = new freeGraphIsland();
                        island.Add(result);

                        if (island.type != freeGraphIslandType.none)
                        {
                            islands.Add(island);
                            processed.AddRange(island.nodes);
                        }

                        //result.ForEach(x => processed.Add(x.name));
                    }
                }
            }

            List<freeGraphIsland> _binodal = new List<freeGraphIsland>();
            List<freeGraphIsland> _trinodal = new List<freeGraphIsland>();
            List<freeGraphIsland> _polinodal = new List<freeGraphIsland>();

            freeGraphIsland continent = null;
            if (islands.Any())
            {
                Int32 max = 0;
                continent = islands.First();
                foreach (var island in islands)
                {
                    Int32 nc = island.nodes.Count;
                    if (nc == 2)
                    {
                        Binodal += nc;
                        BinodalN++;
                    }
                    if (nc == 3)
                    {
                        Trinodal += nc;
                        TrinodalN++;
                    }
                    if (nc > 3)
                    {
                        Polinodal += nc;
                        PolinodalN++;
                    }
                    if (max < nc)
                    {
                        max = nc;
                        continent = island;
                    }
                }
            }

            AllIslands = islands.Count;

            PolinodalRatio = Polinodal.GetRatio(graph.nodes.Count);

            if (continent != null)
            {
                var contNodes = graph.nodes.Where(x => continent.nodes.Contains(x.name)).ToList();

                //var contNodes = ;

                ContinentMass = continent.nodes.Count;
                Double cw = contNodes.Sum(x => x.weight);
                ContinentWeightRate = cw.GetRatio(TotalWeight);
                ContinentMassRate = continent.nodes.Count.GetRatio(graph.nodes.Count);

                ContinentSize = graph.PingGraphSize(nodesByNumberOfLinks[maxLinkFreq], true, freeGraphPingType.maximumPingLength);
            }

            MainIsland = continent;
        }

        [Category("Node Weights")]
        [imb(imbAttributeName.reporting_hide)]
        public freeGraphIsland MainIsland { get; set; }

        [Category("Node Weights")]
        [DisplayName("Total")]
        [imb(imbAttributeName.measure_letter, "∑(w_nd)")]
        [imb(imbAttributeName.measure_setUnit, "w")]
        [imb(imbAttributeName.reporting_columnWidth, 7)]
        [imb(imbAttributeName.reporting_valueformat, "F3")]
        [Description("Sum of all node weights. w_nd -> weight of node, w -> weight")]
        public Double TotalWeight { get; set; } = default(Double);

        [Category("Node Weights")]
        [DisplayName("Mean")]
        [imb(imbAttributeName.measure_letter, "∑(w_nd) / |nd|")]
        [imb(imbAttributeName.measure_setUnit, "w")]
        [imb(imbAttributeName.reporting_columnWidth, 7)]
        [imb(imbAttributeName.reporting_valueformat, "F3")]
        [Description("Average of all node weights -> ")]
        public Double AvgWeight { get; set; } = default(Double);

        [Category("Node Weights")]
        [DisplayName("StDev")]
        [imb(imbAttributeName.measure_letter, "Std(w)")]
        [imb(imbAttributeName.measure_setUnit, "w")]
        [imb(imbAttributeName.reporting_columnWidth, 7)]
        [imb(imbAttributeName.reporting_valueformat, "F3")]
        [Description("Standard Deviation of node weights")]
        public Double StdWeight { get; set; } = default(Double);

        /// <summary> Number of nodes in binode islands: two nodes with one link between them, disconnected from the rest of the graph </summary>
        [Category("Number of islands")]
        [DisplayName("Bi.")]
        [imb(imbAttributeName.measure_letter, "|I_2nd|")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [imb(imbAttributeName.reporting_columnWidth, 7)]
        [Description("Number of binode islands: two nodes with one link between them, disconnected from the rest of the graph. n -> number, count, I -> island, 2nd -> binodal")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 BinodalN { get; set; } = default(Int32);

        /// <summary> Number of trinode islands: tri nodes with two or three links between them, disconnected from the rest of the graph </summary>
        [Category("Number of islands")]
        [DisplayName("Tri.")]
        [imb(imbAttributeName.measure_letter, "|I_3nd|")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [imb(imbAttributeName.reporting_columnWidth, 7)]
        [Description("Number of islands: three nodes with two or three links between them, disconnected from the rest of the graph, 2nd -> trinodal")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 TrinodalN { get; set; } = default(Int32);

        /// <summary> Number of islands counting more then three linked nodes </summary>
        [Category("Number of islands")]
        [DisplayName("Poli.")]
        [imb(imbAttributeName.measure_letter, "|I_Pnd|")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [imb(imbAttributeName.reporting_columnWidth, 7)]
        [Description("Number of polinode islands: counting more then three linked nodes, Pnd -> polinodal")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 PolinodalN { get; set; } = default(Int32);

        /// <summary> Number of all islands, including the continent </summary>
        [Category("Number of islands")]
        [DisplayName("AllIslands")]
        [imb(imbAttributeName.measure_letter, "|I|")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [imb(imbAttributeName.reporting_columnWidth, 10)]
        [Description("Number of all islands, including the continent")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 AllIslands { get; set; } = 0;

        /// <summary> Node count ratio, in which polinode island nodes, participate in total node count </summary>
        [Category("Node count per island type")]
        [DisplayName("Poli. m. ratio")]
        [imb(imbAttributeName.measure_letter, "|nd∈I_Pn| / |nd|")]
        [imb(imbAttributeName.measure_setUnit, "%")]
        [imb(imbAttributeName.reporting_columnWidth, 10)]
        [imb(imbAttributeName.reporting_valueformat, "P2")]
        [Description("Node count ratio, in which polinode island nodes, participate in total node count ")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public Double PolinodalRatio { get; set; } = 0;

        [Category("Node count per island type")]
        [DisplayName("Bi.")]
        [imb(imbAttributeName.measure_letter, "|nd∈I_2n|")]
        [imb(imbAttributeName.measure_setUnit, "nd")]
        [imb(imbAttributeName.reporting_columnWidth, 7)]
        [Description("Number of nodes in binode islands: two nodes with one link between them, disconnected from the rest of the graph")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 Binodal { get; set; } = 0;

        /// <summary> Number of trinode islands: tri nodes with two or three links between them, disconnected from the rest of the graph </summary>
        [Category("Node count per island type")]
        [DisplayName("Tri.")]
        [imb(imbAttributeName.measure_letter, "|nd∈I_3n|")]
        [imb(imbAttributeName.measure_setUnit, "nd")]
        [imb(imbAttributeName.reporting_columnWidth, 7)]
        [Description("Number of nodes in trinode islands: tri nodes with two or three links between them, disconnected from the rest of the graph")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 Trinodal { get; set; } = 0;

        /// <summary> Number of nodes in polinode islands counting more then three linked nodes </summary>
        [Category("Node count per island type")]
        [DisplayName("Poli.")]
        [imb(imbAttributeName.measure_letter, "|nd∈I_Pn|")]
        [imb(imbAttributeName.measure_setUnit, "nd")]
        [imb(imbAttributeName.reporting_columnWidth, 7)]
        [Description("Number of nodes in polinode islands counting more then three linked nodes")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 Polinodal { get; set; } = 0;

        [Category("Continent")]
        [DisplayName("Mass")]
        [imb(imbAttributeName.measure_letter, "|nd_c|")]
        [imb(imbAttributeName.measure_setUnit, "nd")]
        [imb(imbAttributeName.reporting_columnWidth, 10)]
        [imb(imbAttributeName.basicColor, "#FFFF3300")]
        [Description("Number of nodes in the primary continent, constelation connected to the primary terms")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 ContinentMass { get; set; } = default(Int32);

        [Category("Continent")]
        [DisplayName("Size")]
        [imb(imbAttributeName.measure_letter, "E")]
        [imb(imbAttributeName.measure_setUnit, "edge")]
        [imb(imbAttributeName.reporting_columnWidth, 10)]
        [imb(imbAttributeName.reporting_valueformat, "F1")]
        [Description("Highest number of expansion steps possible from the primary terms")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Double ContinentSize { get; set; } = 0;

        [Category("Continent")]
        [DisplayName("Mass Ratio")]
        [imb(imbAttributeName.measure_letter, "|nd_c| / |nd|")]
        [imb(imbAttributeName.measure_setUnit, "%")]
        [imb(imbAttributeName.reporting_columnWidth, 10)]
        [imb(imbAttributeName.reporting_valueformat, "P2")]
        [Description("Ratio between number of nodes in the continent and total number of the nodes ")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Double ContinentMassRate { get; set; } = 0;

        [Category("Continent")]
        [DisplayName("Weight Ratio")]
        [imb(imbAttributeName.measure_letter, "∑(w_ndc) / ∑(w_nd)")]
        [imb(imbAttributeName.measure_setUnit, "%")]
        [imb(imbAttributeName.reporting_columnWidth, 10)]
        [imb(imbAttributeName.reporting_valueformat, "P2")]
        [Description("Ratio between sum of continent nodes' weights and Total weight of the cloud")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Double ContinentWeightRate { get; set; } = 0;

        /// <summary> Cross category number of nodes in the Semantic Cloud </summary>
        [Category("Cloud metrics")]
        [DisplayName("Nodes")]
        [imb(imbAttributeName.measure_letter, "|nd|")]
        [imb(imbAttributeName.measure_setUnit, "nd")]
        [Description("Cross category number of nodes in the Semantic Cloud")]
        [imb(imbAttributeName.reporting_columnWidth, 10)]
        [imb(imbAttributeName.reporting_valueformat, "F2")]// [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public Double NodeCount { get; set; } = default(Double);

        /// <summary> Cross category average number of links per node, in the Semantic Cloud </summary>

        [Category("Cloud metrics")]
        [imb(imbAttributeName.measure_letter, "|nd→nd|/|nd|")]
        [imb(imbAttributeName.measure_setUnit, "ln/nd")]
        [imb(imbAttributeName.reporting_columnWidth, 10)]
        [imb(imbAttributeName.reporting_valueformat, "F3")]
        [Description("Cross category average number of links per node, in the Semantic Cloud, ln -> link, edge")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public Double LinkRatio { get; set; } = default(Double);

        [Category("Label")]
        [DisplayName("Description")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Description of the report and graph")] // [imb(imbAttributeName.reporting_escapeoff)]
        [imb(imbAttributeName.reporting_columnWidth, 40)]
        public String description { get; set; } = "";

        public freeGraphReport()
        {
        }
    }
}