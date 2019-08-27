using imbSCI.Data.collection.nested;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;

namespace imbSCI.Graph.FreeGraph.adapters
{
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
}