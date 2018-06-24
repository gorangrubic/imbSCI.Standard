using imbSCI.Data.collection.nested;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace imbSCI.Data.collection.graph
{
    /// <summary>
    /// Utility class used to detect difference or change between two directed graphs A and B
    /// </summary>
    /// <seealso cref="imbSCI.Data.collection.graph.graphAnalysisPairCollection" />
    public class graphChangeAnalysis : graphAnalysisPairCollection
    {
        public IGraphNode GraphA { get; set; }

        public IGraphNode GraphB { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="graphChangeAnalysis"/> class - and does nothing
        /// </summary>
        public graphChangeAnalysis()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="graphChangeAnalysis"/> class - and calls <see cref="Perform"/> method immediatly
        /// </summary>
        /// <param name="graphA">The graph a.</param>
        /// <param name="graphB">The graph b.</param>
        public graphChangeAnalysis(IGraphNode graphA, IGraphNode graphB)
        {
            GraphA = graphA;
            GraphB = graphB;

            Perform();
        }

        /// <summary>
        /// Pairs sorted according to <see cref="graphChangeType"/>
        /// </summary>
        /// <value>
        /// The sorted.
        /// </value>
        [XmlIgnore]
        public aceEnumDictionary<graphChangeType, graphAnalysisPairCollection> sorted { get; set; } = new aceEnumDictionary<graphChangeType, graphAnalysisPairCollection>();

        /// <summary>
        /// Executes the analysis and populates collection with resulting pairs
        /// </summary>
        public void Perform()
        {
            var graph_a_nodes = GraphA.getAllChildren();
            var graph_b_nodes = GraphB.getAllChildren();

            foreach (IGraphNode a_node in graph_a_nodes)
            {
                String a_p = a_node.path;
                IGraphNode b_node = graph_b_nodes.FirstOrDefault(x => x.path == a_p) as IGraphNode;

                if (graph_b_nodes.Any(x => x.path == a_p))
                {
                    var pair = Add(a_node, b_node);

                    sorted[pair.result].Add(pair);
                }
            }

            var graph_a_paths = graph_a_nodes.Select(x => x.path);
            var graph_b_paths = graph_b_nodes.Select(x => x.path);

            var common_paths = new List<String>();

            var added_paths = new List<String>();

            var removed_paths = new List<String>();

            foreach (string p in graph_a_paths)
            {
                if (graph_b_paths.Contains(p))
                {
                    common_paths.Add(p);
                }
                else
                {
                    removed_paths.Add(p);
                }
            }

            foreach (string p in graph_b_paths)
            {
                if (graph_a_paths.Contains(p))
                {
                    added_paths.Add(p);
                }
            }
        }
    }
}