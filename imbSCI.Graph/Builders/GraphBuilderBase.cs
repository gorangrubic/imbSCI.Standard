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


public abstract class GraphBuilderBase<TGraph, TNode> :IGraphBuilder
        where TGraph:class, new()
        where TNode:class, IObjectWithUID, new()
    {
        protected abstract void ExecuteSpan(GraphBuildInstructionSpan span, TGraph target);

        public abstract GraphBuildInstructionSequence GraphToInstructions(TGraph source);

        public abstract diagramNodeShapeEnum GetShapeForNode(TNode node);

        public abstract diagramLinkTypeEnum GetShapeForLink(TNode nodeA,TNode nodeB);

        public virtual GraphBuildInstruction MakeNodeInstruction(TNode node, diagramNodeShapeEnum nodeShapeType)
        {
            GraphBuildInstruction instruction = new GraphBuildInstruction();
            instruction.InstructionType = nodeShapeType;
            instruction.Parameters.Add(node.UID);
   
            return instruction;
        }

        public virtual GraphBuildInstructionSpan MakeNodeSpan(TNode node)
        {
            GraphBuildInstructionSpan output = new GraphBuildInstructionSpan();

            GraphBuildInstruction instruction = new GraphBuildInstruction();
            instruction.InstructionType = GetShapeForNode(node);
            instruction.Parameters.Add(node.UID);

            output.Add(instruction);
            return output;
        }

        public virtual GraphBuildInstructionSpan MakeLinkSpan(TNode nodeA,TNode nodeB)
        {
            GraphBuildInstructionSpan output = new GraphBuildInstructionSpan();

            output.Add(MakeNodeInstruction(nodeA,GetShapeForNode(nodeA)));

            GraphBuildInstruction instruction = new GraphBuildInstruction();
            instruction.InstructionType = GetShapeForLink(nodeA, nodeB);

            output.Add(instruction);
            
            output.Add(MakeNodeInstruction(nodeB,GetShapeForNode(nodeB)));
            return output;
        }

        public virtual TGraph InstructionsToGraph(GraphBuildInstructionSequence instructions)
        {
            TGraph output = new TGraph(); 
            GraphBuildInstructionSpanSet spans = instructions.GetSpans();
            foreach (var span in spans)
            {
                ExecuteSpan(span, output);
            }

            return output;
        }

        GraphBuildInstructionSequence IGraphBuilder.GraphToInstructions<TGraph1>(TGraph1 source)
        {
            return GraphToInstructions(source as TGraph);
        }

        TGraph1 IGraphBuilder.InstructionsToGraph<TGraph1>(GraphBuildInstructionSequence instructions)
        {
            return InstructionsToGraph(instructions) as TGraph1;
        }
    }
}