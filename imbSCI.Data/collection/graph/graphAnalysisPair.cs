using System;

namespace imbSCI.Data.collection.graph
{
    using System.Xml.Serialization;

    /// <summary>
    /// Analytical entry, describing relation ship between version of the same node, in two different graphs
    /// </summary>
    public class graphAnalysisPair
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="graphAnalysisPair"/> class.
        /// </summary>
        public graphAnalysisPair()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="graphAnalysisPair"/> class.
        /// </summary>
        /// <param name="node_a">The node a.</param>
        /// <param name="node_b">The node b.</param>
        /// <param name="_result">The result.</param>
        public graphAnalysisPair(IGraphNode node_a, IGraphNode node_b, graphChangeType _result)
        {
            if (node_a != null) path_at_A = node_a.path;
            if (node_b != null) path_at_B = node_b.path;

            node_A = node_a;
            node_B = node_b;

            result = _result;
        }

        /// <summary>
        /// Gets or sets the node a.
        /// </summary>
        /// <value>
        /// The node a.
        /// </value>
        [XmlIgnore]
        public IGraphNode node_A { get; set; }

        /// <summary>
        /// Gets or sets the node b.
        /// </summary>
        /// <value>
        /// The node b.
        /// </value>
        [XmlIgnore]
        public IGraphNode node_B { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public graphChangeType result { get; set; }

        /// <summary>
        /// Gets or sets the path at a.
        /// </summary>
        /// <value>
        /// The path at a.
        /// </value>
        public String path_at_A { get; set; } = "";

        /// <summary>
        /// Gets or sets the path at b.
        /// </summary>
        /// <value>
        /// The path at b.
        /// </value>
        public String path_at_B { get; set; } = "";
    }
}