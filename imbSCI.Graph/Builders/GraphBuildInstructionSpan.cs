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
public class GraphBuildInstructionSpan:List<IGraphBuildInstruction>
    {
        public Boolean IsDeclared
        {
            get
            {
                if (Count == 0) return false;
                if (Type == InstructionSpanType.unknown) return false;
                return true;
            }
        }

        public InstructionSpanType Type { get; set; } = InstructionSpanType.unknown;
    }
}