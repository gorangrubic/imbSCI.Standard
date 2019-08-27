using imbSCI.Core.enums;
using imbSCI.Core.math.range;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;

namespace imbSCI.Graph.FreeGraph.adapters
{

    /// <summary>
    /// hosts multiple <see cref="freeGraphAdapter{TNode}"/> for typed access to entities related via <see cref="graph"/>
    /// </summary>
    /// <seealso cref="imbSCI.Graph.FreeGraph.adapters.IFreeGraphAdapter" />
    public abstract class freeGraphWithAdapters
    {

        /// <summary>
        /// Gets or sets the register.
        /// </summary>
        /// <value>
        /// The register.
        /// </value>
        protected freeGraphTypedNodesRegister register { get; set; } = new freeGraphTypedNodesRegister();

        /// <summary>
        /// Gets or sets the adapters.
        /// </summary>
        /// <value>
        /// The adapters.
        /// </value>
        protected Dictionary<Type, IFreeGraphAdapter> adapters { get; set; } = new Dictionary<Type, IFreeGraphAdapter>();

        /// <summary>
        /// Graph that describes relations between instances
        /// </summary>
        /// <value>
        /// The graph.
        /// </value>
        public freeGraph graph { get; set; } = new freeGraph();

        /// <summary>
        /// Creates instance of adapter and registers it to the <see cref="adapters"/> dictionary. Use this from constructor of the class inheriting this.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="_mainPrefix">The main prefix - to be used for type=0 objects.</param>
        /// <param name="prefixes">The prefixes - for type=1... n objects</param>
        /// <returns>Instance of the adapter</returns>
        protected freeGraphAdapter<TNode> MakeAdapter<TNode>(String _mainPrefix, params String[] prefixes) where TNode : IObjectWithNameWeightAndType
        {
            Type t = typeof(TNode);

            freeGraphAdapter<TNode> output = new freeGraphAdapter<TNode>(graph, register, _mainPrefix, prefixes);
            adapters.Add(t, output);

            return output;

        }

        /// <summary>
        /// Gets the identifier - built from prefix (specified by adapter) and object name
        /// </summary>
        /// <param name="Node">A node to get identifier for</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Node type not supported by adapters - Node</exception>
        public String GetID(IObjectWithNameWeightAndType Node)
        {
            Type t = Node.GetType();
            if (!adapters.ContainsKey(t))
            {
                throw new ArgumentOutOfRangeException("Node type not supported by adapters", nameof(Node));
            }
            else
            {
                return adapters[t].GetID(Node);
            }
        }

        /// <summary>
        /// Adds new link, or update existing
        /// </summary>
        /// <param name="NodeA">The node a.</param>
        /// <param name="NodeB">The node b.</param>
        /// <param name="w">The w.</param>
        /// <param name="type">The type.</param>
        /// <param name="onWeight">The on weight.</param>
        /// <param name="onType">Type of the on.</param>
        /// <returns></returns>
        public freeGraphLinkBase AddLink(IObjectWithNameWeightAndType NodeA, IObjectWithNameWeightAndType NodeB, Double w, Int32 type, operation onWeight = operation.assign, operation onType = operation.assign)
        {
            String a = GetID(NodeA);
            String b = GetID(NodeB);
            var lnk = graph.GetLink(a, b);

            if (lnk != null)
            {
                lnk.weight = onWeight.compute(lnk.weight, w);
                lnk.type = onWeight.compute(lnk.type, type);
            }
            else
            {
                lnk = graph.AddLink(a, b, w, type);
            }
            return lnk;
        }




        public List<freeGraphLinkBase> AddLinks(IObjectWithNameWeightAndType NodeA, IEnumerable<IObjectWithNameWeightAndType> NodeBs, double w = 1, int type = 0, operation onWeight = operation.assign, operation onType = operation.assign)
        {
            String a = GetID(NodeA);
            List<freeGraphLinkBase> output = new List<freeGraphLinkBase>();

            foreach (var B in NodeBs)
            {
                String b = GetID(B);
                var lnk = graph.GetLink(a, b);

                if (lnk != null)
                {
                    lnk.weight = onWeight.compute(lnk.weight, w);
                    lnk.type = onWeight.compute(lnk.type, type);
                }
                else
                {
                    lnk = graph.AddLink(a, b, w, type);
                }
                output.Add(lnk);
            }
            return output;
        }

        public List<freeGraphNodeBase> Assign(IEnumerable<IObjectWithNameWeightAndType> NodeA, operation onWeight = operation.assign, operation onType = operation.assign)
        {
            List<freeGraphNodeBase> output = new List<freeGraphNodeBase>();
            foreach (var B in NodeA)
            {
                String b = GetID(B);
                var lnk = graph.AddNode(b, B.weight, B.type);

                if (lnk != null)
                {
                    lnk.weight = onWeight.compute(lnk.weight, B.weight);
                    lnk.type = onType.compute(lnk.type, B.type);
                }
                else
                {
                    lnk = graph.AddNode(b, B.weight, B.type);
                }
                output.Add(lnk);
            }
            return output;
        }

        public freeGraphNodeBase Assign(IObjectWithNameWeightAndType NodeA, operation onWeight = operation.assign, operation onType = operation.assign)
        {
            String b = GetID(NodeA);
            var lnk = graph.AddNode(b, NodeA.weight, NodeA.type);

            if (lnk != null)
            {
                lnk.weight = onWeight.compute(lnk.weight, NodeA.weight);
                lnk.type = onType.compute(lnk.type, NodeA.type);
            }
            else
            {
                lnk = graph.AddNode(b, NodeA.weight, NodeA.type);
            }
            return lnk;
        }

        public IFreeGraphAdapter GetAdapter(IObjectWithNameWeightAndType node)
        {
            var t = node.GetType();
            if (adapters.ContainsKey(t))
            {
                return adapters[t];
            }
            throw new ArgumentOutOfRangeException("Node type not supported by adapters", nameof(node));
        }

        public IFreeGraphAdapter GetAdapter<TNodeB>() where TNodeB : IObjectWithNameWeightAndType
        {
            if (adapters.ContainsKey(typeof(TNodeB)))
            {
                return adapters[typeof(TNodeB)];
            }
            throw new ArgumentOutOfRangeException("TNodeB", "Node type not supported by adapters");
        }


    }

}