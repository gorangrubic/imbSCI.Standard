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
public abstract class GraphBuilderForHierarchicalGraphBase<TGraph,TNode>:GraphBuilderBase<TGraph, TNode>
        where TGraph:class, IGraphNode, new()
        where TNode:class, IGraphNode, new()
    {
        
    }
}