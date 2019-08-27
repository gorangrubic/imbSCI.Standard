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
public class GraphBuildInstruction:IGraphBuildInstruction
    {
        public Object InstructionType { get; set; }

        public List<String> Parameters { get; set; } = new List<string>();
    }
}