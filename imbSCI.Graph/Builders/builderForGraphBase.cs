using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.data.text;
using imbSCI.Data.interfaces;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using imbSCI.Graph.Diagram.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.Graph.Builders
{
    public class GraphBuilderForFreeGraphBase<TGraph, TNode, TLink> : GraphBuilderBase<TGraph, TNode>
        where TGraph : class, IFreeGraph, new()
        where TNode : class, IFreeGraphNode, new()
        where TLink : class, IFreeGraphLink, new()
    {
        public override diagramLinkTypeEnum GetShapeForLink(TNode nodeA, TNode nodeB)
        {
            throw new NotImplementedException();
        }

        public override diagramNodeShapeEnum GetShapeForNode(TNode node)
        {
            throw new NotImplementedException();
        }

        public override GraphBuildInstructionSequence GraphToInstructions(TGraph source)
        {
            throw new NotImplementedException();
        }

        protected override void ExecuteSpan(GraphBuildInstructionSpan span, TGraph target)
        {
            throw new NotImplementedException();
        }
    }
}
