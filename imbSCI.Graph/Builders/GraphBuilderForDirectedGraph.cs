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
public class GraphBuilderForDirectedGraph : GraphBuilderForFreeGraphBase<DirectedGraph, DGML.core.Node, DGML.core.Link>
    {
        public override diagramLinkTypeEnum GetShapeForLink(Node nodeA, Node nodeB)
        {
            throw new NotImplementedException();
        }

        public override diagramNodeShapeEnum GetShapeForNode(Node node)
        {
            throw new NotImplementedException();
        }

        public override GraphBuildInstructionSequence GraphToInstructions(DirectedGraph source)
        {
            throw new NotImplementedException();
        }

        protected override void ExecuteSpan(GraphBuildInstructionSpan span, DirectedGraph target)
        {
            throw new NotImplementedException();
        }
    }
}