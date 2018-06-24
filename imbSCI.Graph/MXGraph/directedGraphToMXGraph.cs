using imbSCI.Graph.DGML;
using imbSCI.Graph.MXGraph.layout;
using imbSCI.Graph.MXGraph.model;
using imbSCI.Graph.MXGraph.view;
using System;
using System.Collections.Generic;

namespace imbSCI.Graph.MXGraph
{
    public static class directedGraphToMXGraph
    {
        /// <summary>
        /// Converts a <see cref="DirectedGraph"/> graph object into <see cref="mxGraph"/>
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static mxGraph ConvertToMXGraph(this DirectedGraph input)
        {
            // Creates graph with model
            mxGraph graph = new mxGraph();

            mxFastOrganicLayout layout = new mxFastOrganicLayout(graph);

            Object parent = graph.GetDefaultParent();
            graph.Model.BeginUpdate();

            Dictionary<String, mxCell> nodeId = new Dictionary<string, mxCell>();

            foreach (DGML.core.Node node in input.Nodes)
            {
                if (!nodeId.ContainsKey(node.Id)) nodeId.Add(node.Id, (mxCell)graph.InsertVertex(parent, node.Id, node.Label, 0, 0, 200, 35, node.Category));
            }

            foreach (DGML.core.Link link in input.Links)
            {
                if (!nodeId.ContainsKey(link.Source)) continue;
                if (!nodeId.ContainsKey(link.Target)) continue;

                var source = nodeId[link.Source];
                var target = nodeId[link.Target];

                if ((source != null) && (target != null))
                {
                    graph.InsertEdge(parent, link.Id, link.Label, source, target);
                }
            }

            layout.execute(parent);

            return graph;

            //    layout.execute(
        }
    }
}