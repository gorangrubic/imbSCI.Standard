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
//public class GraphBuildNodeInstruction<diagramNodeShapeEnum>
    //{

    //}

    //public class GraphBuildLinkInstruction<diagramLinkTypeEnum>
    //{

    //}

    public class GraphBuildInstructionSequence:List<IGraphBuildInstruction>
    {

        public GraphBuildInstructionSpanSet GetSpans()
        {
            GraphBuildInstructionSpanSet output = new GraphBuildInstructionSpanSet();

            foreach (IGraphBuildInstruction head in this)
            {
                output.Add(head);
            }

            return output;
        }
    }
}