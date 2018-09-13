using imbSCI.Core.enums;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math.range;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Graph.FreeGraph.adapters
{



    /// <summary>
    /// Provides typed interface to add, remove and query a <see cref="freeGraph"/> 
    /// </summary>
    public class freeGraphAdapter<TNode> : IFreeGraphAdapter where TNode : IObjectWithNameWeightAndType
        //where TEdge : IObjectWithNameWeightAndType
    {
        protected freeGraph graph { get; set; }

        public Dictionary<Int32, String> typePrefixes = new Dictionary<Int32, String>();

        public freeGraphTypedNodesRegister source { get; set; }

        public String mainPrefix { get; set; }

        public Type t { get; set; } = typeof(TNode);


        public TNode Get(String id, Int32 type = 0)
        {
            String n = GetID(id, type);
            if (source[t].ContainsKey(n))
            {
                var o = source[t][n];
                if (o is TNode) return (TNode)o;
            }
            return default(TNode);
        }

        public List<TNode> Get(IEnumerable<String> id, Int32 type = 0)
        {
            List<TNode> output = new List<TNode>();
            foreach (var i in id)
            {
                var o = Get(i, type);
                if (o != null) output.Add(o);
            }
            return output;
        }



        public freeGraphTypedResultSet<TNodeB> GetLinkedNodesOfType<TNodeB>(TNode node) where TNodeB : IObjectWithNameWeightAndType
        {
            freeGraphTypedResultSet<TNodeB> output = new freeGraphTypedResultSet<TNodeB>();
            Type t_B = typeof(TNodeB);
            Type t_A = typeof(TNode);

            freeGraphNodeAndLinks res = graph.GetLinks(GetID(node), true);

            foreach (freeGraphLink r in res)
            {
                var n = new freeGraphTypedResultEntry<TNodeB>(r.linkBase);
                n.TNodeA = (TNodeB)source[t_A][r.nodeA.name];
                n.TNodeB = (TNodeB)source[t_B][r.nodeB.name];

                output.Add(n);
            }

            return output;

        }

        public freeGraphTypedResultSet<IObjectWithNameWeightAndType> GetLinked(TNode node)
        {
            freeGraphTypedResultSet<IObjectWithNameWeightAndType> output = new freeGraphTypedResultSet<IObjectWithNameWeightAndType>();
            freeGraphNodeAndLinks res = graph.GetLinks(GetID(node), true);

            foreach (var r in res)
            {
                var n = new freeGraphTypedResultEntry<IObjectWithNameWeightAndType>(r.linkBase);
                n.TNodeA = source[t][r.nodeA.name];
                n.TNodeB = source[t][r.nodeB.name];

                output.Add(n);
            }

            return output;
        }

        public freeGraphTypedResultSet<TNode> GetLinkedTyped(TNode node)
        {
            freeGraphTypedResultSet<TNode> output = new freeGraphTypedResultSet<TNode>();
            freeGraphNodeAndLinks res = graph.GetLinks(GetID(node), true);

            foreach (var r in res)
            {
                var n = new freeGraphTypedResultEntry<TNode>(r.linkBase);
                n.TNodeA = (TNode)source[t][r.nodeA.name];
                n.TNodeB = (TNode)source[t][r.nodeB.name];

                output.Add(n);
            }

            return output;
        }



        //public freeGraphLinkBase AddLink(IObjectWithNameWeightAndType NodeA, IObjectWithNameWeightAndType NodeB, Double w = 1, Int32 type = 0, operation onWeight = operation.assign, operation onType = operation.assign)
        //{

        //}

        //public List<freeGraphLinkBase> AddLinks(IObjectWithNameWeightAndType NodeA, IEnumerable<IObjectWithNameWeightAndType> NodeBs, Double w = 1, Int32 type = 0, operation onWeight = operation.assign, operation onType = operation.assign)
        //{

        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="freeGraphAdapter{TNodeA}"/> class, attaches the graph and assigns prefixes for IDs of nodes, according to type number
        /// </summary>
        /// <param name="_graph">The graph.</param>
        /// <param name="prefixes">The prefixes.</param>
        public freeGraphAdapter(freeGraph _graph, freeGraphTypedNodesRegister _source, String _mainPrefix, String[] prefixes)
        {
            Deploy(_graph, _source, _mainPrefix, prefixes);
        }

        private void Deploy(freeGraph _graph, freeGraphTypedNodesRegister _source, String _mainPrefix, String[] prefixes)
        {
            graph = _graph;
            Int32 c = 0;
            mainPrefix = _mainPrefix.or("OBJ_");

            source = _source;
            if (source == null) source = new freeGraphTypedNodesRegister();

            if (prefixes == null) return;
            typePrefixes.Add(c, mainPrefix);

            c++;
            foreach (String p in prefixes)
            {
                typePrefixes.Add(c, p);
                c++;
            }
        }

        /// <summary>
        /// Makes identifier, used as prefix when interfacing the graph
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public String GetID(IObjectWithNameWeightAndType item)
        {
            String output = "";
            if (item.type > 0 && item.type <= typePrefixes.Count)
            {
                output = typePrefixes[item.type];
            }
            else
            {
                output = mainPrefix;
            }
            return output + item.name;
        }

        public String GetID(String n, Int32 type)
        {
            String output = "";
            if (type > 0 && type < typePrefixes.Count)
            {
                output = typePrefixes[type];
            }
            else
            {
                output = mainPrefix;
            }
            return n.ensureStartsWith(output);
        }


        public freeGraphNodeBase AddNodeOrSum(TNode item)
        {
            return graph.AddNodeOrSum(GetID(item), item.weight, item.type);
        }

        //public List<TNodeB> GetLinkedNodesOfType<TNodeB>(TNode item, Boolean includeBtoALinks = false)
        //{

        //}


        //public List<IObjectWithNameWeightAndType> GetLinkedNodes(TNode item, Boolean includeBtoALinks = false)
        //{
        //    String id = GetID(item);
        //    var result = graph.GetLinkedNodes(id, includeBtoALinks);
        //    List<IObjectWithNameWeightAndType> output = new List<IObjectWithNameWeightAndType>();

        //    IObjectWithNameWeightAndType n = source[t][GetID(item)];

        //    return GetLinked(item);


        //}

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
        public freeGraphLinkBase AddLink(TNode NodeA, IObjectWithNameWeightAndType NodeB, Double w, Int32 type, operation onWeight = operation.assign, operation onType = operation.assign)
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

        string IFreeGraphAdapter.GetID(IObjectWithNameWeightAndType Node)
        {
            throw new NotImplementedException();
        }

        TNode1 IFreeGraphAdapter.Get<TNode1>(string id, int type)
        {
            throw new NotImplementedException();
        }

        List<freeGraphNodeBase> IFreeGraphAdapter.Assign(IEnumerable<IObjectWithNameWeightAndType> NodeA)
        {
            return graph.GetNodes(NodeA.Select(x => GetID(x)));
        }

        freeGraphNodeBase IFreeGraphAdapter.Assign(IObjectWithNameWeightAndType NodeA)
        {
            return graph.GetNode(GetID(NodeA));
        }
    }
}
