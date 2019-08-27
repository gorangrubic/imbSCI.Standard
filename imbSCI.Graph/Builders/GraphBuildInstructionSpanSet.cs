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
public class GraphBuildInstructionSpanSet:List<GraphBuildInstructionSpan>
    {
        GraphBuildInstructionSpan current_span = new GraphBuildInstructionSpan();

        private void CloseSpan()
        {
            if (current_span != null)
            {
                Add(current_span);
            }
            current_span = new GraphBuildInstructionSpan();
        }

        private void DeclareSpan(IGraphBuildInstruction head, InstructionSpanType type)
        {
            current_span.Type = type;
            current_span.Add(head);
        }

        public void Add(IGraphBuildInstruction head)
        {
            if (head.InstructionType is diagramNodeShapeEnum nodeShapeType)
            {
                if (!current_span.IsDeclared)
                {
                    DeclareSpan(head, InstructionSpanType.node);
                } else
                {
                    if (current_span.Type == InstructionSpanType.link)
                    {
                        current_span.Add(head);
                        CloseSpan();
                    } else
                    {
                        CloseSpan();
                        DeclareSpan(head, InstructionSpanType.node);
                    }
                }
            } else if (head.InstructionType is diagramLinkTypeEnum linkShapeType)
            {
                if (!current_span.IsDeclared)
                {

                } else
                {  
                    if (current_span.Type == InstructionSpanType.link)
                    {
                         current_span.Add(head);
                    } else if (current_span.Type == InstructionSpanType.node)
                    {
                        var node_from_last_span = current_span.Last();
                        CloseSpan();
                        DeclareSpan(node_from_last_span, InstructionSpanType.link);
                        current_span.Add(head);
                    }
                }
            } else if (head.InstructionType == null)
            {
                CloseSpan();
            }
        }

    }
}