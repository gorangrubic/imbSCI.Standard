using imbSCI.Data.collection.nested;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;

namespace imbSCI.Graph.FreeGraph.adapters
{

    //public interface IFreeGraphEditAndQuery
    //{


    //    /// <summary>
    //    /// Counts the links involving the specified nodeName
    //    /// </summary>
    //    /// <param name="nodeName">Name of the node.</param>
    //    /// <param name="AtoB">if set to <c>true</c> [ato b].</param>
    //    /// <param name="BtoA">if set to <c>true</c> [bto a].</param>
    //    /// <returns></returns>
    //    Int32 CountLinks(String nodeName, Boolean AtoB = true, Boolean BtoA = true);


    //}

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNode">The type of the node.</typeparam>
    /// <seealso cref="imbSCI.Graph.FreeGraph.freeGraphLink" />
    public class freeGraphTypedResultEntry<TNode> : freeGraphLink where TNode : IObjectWithNameWeightAndType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="freeGraphTypedResultEntry{TNode}"/> class.
        /// </summary>
        public freeGraphTypedResultEntry(freeGraphLinkBase link) : base(link)
        {

        }

        public TNode TNodeA { get; set; }

        public TNode TNodeB { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNode">The type of the node.</typeparam>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.Graph.FreeGraph.adapters.freeGraphTypedResultEntry{TNode}}" />
    public class freeGraphTypedResultSet<TNode> : List<freeGraphTypedResultEntry<TNode>> where TNode:IObjectWithNameWeightAndType
    {
        public freeGraphTypedResultSet() : base()
        {


        }

        //public freeGraphTypedResultSet(List<freeGraphTypedResultEntry<TNode>> r) : base(r)
        //{


        //}
    }


    public class freeGraphTypedNodesRegister : aceDictionary2D<Type, String, IObjectWithNameWeightAndType>
    {

    }

}