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
}