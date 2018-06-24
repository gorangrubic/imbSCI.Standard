using System.Collections.Generic;

namespace imbSCI.Data.collection.graph
{
    /// <summary>
    /// Collection of analysis pairs
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.Data.collection.graph.graphAnalysisPair}" />
    public class graphAnalysisPairCollection : List<graphAnalysisPair>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="graphAnalysisPairCollection"/> class.
        /// </summary>
        public graphAnalysisPairCollection()
        {
        }

        /// <summary>
        /// Creates new analysis pair - sets <see cref="graphChangeType"/> for the pair, as: added, removed or noChange.
        /// </summary>
        /// <param name="node_A">The node a.</param>
        /// <param name="node_B">The node b.</param>
        /// <returns></returns>
        public graphAnalysisPair Add(IGraphNode node_A, IGraphNode node_B)
        {
            graphAnalysisPair pair = null;

            if (node_A == null)
            {
                pair = new graphAnalysisPair(node_A, node_B, graphChangeType.added);
            }
            else if (node_B == null)
            {
                pair = new graphAnalysisPair(node_A, node_B, graphChangeType.removed);
            }
            else
            {
                pair = new graphAnalysisPair(node_A, node_B, graphChangeType.noChange);
            }

            Add(pair);

            return pair;
        }
    }
}