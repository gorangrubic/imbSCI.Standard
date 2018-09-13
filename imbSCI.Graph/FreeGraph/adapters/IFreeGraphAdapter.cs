using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;

namespace imbSCI.Graph.FreeGraph.adapters
{

    public interface IFreeGraphAdapter
    {
        String GetID(IObjectWithNameWeightAndType Node);

        TNode Get<TNode>(String id, Int32 type) where TNode : IObjectWithNameWeightAndType;

        //  List<TNode> GetOfType<TNode>(IEnumerable<String> id, Int32 type) where TNode : IObjectWithNameWeightAndType;


        //List<TNode> GetLinkedOfType<TNode>(String id) where TNode : IObjectWithNameWeightAndType;

        //List<TNode> GetLinkedOfType<TNode>(IObjectWithNameWeightAndType node) where TNode : IObjectWithNameWeightAndType;

        //List<IObjectWithNameWeightAndType> GetLinked(IObjectWithNameWeightAndType node);

        /// <summary>
        /// Registers the specified nodes with the graph
        /// </summary>
        /// <param name="NodeA">The node a.</param>
        /// <returns></returns>
        List<freeGraphNodeBase> Assign(IEnumerable<IObjectWithNameWeightAndType> NodeA);

        /// <summary>
        /// Registers the specified node with the graph
        /// </summary>
        /// <param name="NodeA">The node a.</param>
        /// <returns></returns>
        freeGraphNodeBase Assign(IObjectWithNameWeightAndType NodeA);


        //freeGraphLinkBase AddLink(IObjectWithNameWeightAndType NodeA, IObjectWithNameWeightAndType NodeB, Double w = 1, Int32 type = 0, operation onWeight = operation.assign, operation onType = operation.assign);

        //freeGraphLinkBase AddLinks(IObjectWithNameWeightAndType NodeA, IEnumerable<IObjectWithNameWeightAndType> NodeBs, Double w = 1, Int32 type = 0, operation onWeight = operation.assign, operation onType = operation.assign);
    }

}