using System;
using System.Collections.Generic;

namespace imbSCI.Data.collection.graph
{
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.List{imbSCI.Data.collection.graph.IGraphNode}'
    /// <summary>
    /// Set of <see cref="IGraphNode"/> instances, used temporarly in process of common-root graph construction
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.Data.collection.graph.IGraphNode}" />
    public class graphNodeSet : List<IGraphNode>
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.List{imbSCI.Data.collection.graph.IGraphNode}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="graphNodeSet"/> class.
        /// </summary>
        /// <param name="localRoot">The local root.</param>
        public graphNodeSet(IGraphNode localRoot)
        {
            root = localRoot;
        }

        /// <summary>
        /// Constructs a graph from specified set of <see cref="IGraphNode"/>s, with common root node <c>localRoot</c>
        /// </summary>
        /// <param name="source">The source collection</param>
        /// <param name="localRoot">Local root node, to be used</param>
        public graphNodeSet(IEnumerable<IGraphNode> source, IGraphNode localRoot)
        {
            root = localRoot;
            this.AddRange(source);
        }

        /// <summary>
        /// Common root of the set - if exists
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        public IGraphNode root { get; set; }

        /// <summary>
        /// Creates normal directed graph, by connecting contained graph nodes at common root nodes. If there are no common nodes it can be added optionally by specifying local root name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="localRootName">Name of the local root - in case there is no common <c>root</c> for the set</param>
        /// <returns>Reference to the root node of newly created graph</returns>
        public T CreateGraphFromLocalRoot<T>(String localRootName = "") where T : IGraphNode, new()
        {
            throw new NotImplementedException("Still not ready");

            T output = new T();

            if (root != null)
            {
                output.name = root.name;
            }
            else
            {
                output.name = localRootName;

                foreach (var nd in this)
                {
                    output.Add(nd);
                }
            }

            foreach (var nd in this)
            {
                nd.MergeWith(output, graphOperationFlag.mergeOnSameName);
            }

            if (root == null)
            {
                root = new T();
                root.name = localRootName;
            }

            return output;
        }
    }
}