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
public interface IGraphBuilder
    {
        GraphBuildInstructionSequence GraphToInstructions<TGraph>(TGraph source) where TGraph : class, new();
        TGraph InstructionsToGraph<TGraph>(GraphBuildInstructionSequence instructions) where TGraph : class, new();

    }
}