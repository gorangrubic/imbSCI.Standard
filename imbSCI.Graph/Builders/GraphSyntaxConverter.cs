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
    public class GraphSyntaxConverter<TSyntax, TGraphBuilder, TGraph>
        where TSyntax:GraphSyntaxBase, new()
        where TGraphBuilder:class, IGraphBuilder, new()
        where TGraph:class, new()
    {
        public TSyntax Syntax { get; set; } = new TSyntax();

        public TGraphBuilder Builder { get; set; } = new TGraphBuilder();

        public TGraph BuildFromCode(String code)
        {
            var interpretation = Syntax.Interpret(code); //.interpret(code);
            return Builder.InstructionsToGraph<TGraph>(interpretation);

        }

        public String BuildToCode(TGraph code)
        {
            var interpretation = Builder.GraphToInstructions<TGraph>(code); //Syntax.Interpret(code); //.interpret(code);

            return Syntax.Interpret(interpretation);
        }

    }
}