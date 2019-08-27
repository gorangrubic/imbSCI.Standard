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
public abstract class GraphSyntaxBase
    {
        public regexMarkerCollection SyntaxMarkers { get; set; } = new regexMarkerCollection();
        
        public abstract void Init();

        /// <summary>
        /// Interprets sequence of instructions into syntax code
        /// </summary>
        /// <param name="instructions">The instructions.</param>
        /// <returns></returns>
        public String Interpret(GraphBuildInstructionSequence instructions)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < instructions.Count; i++)
            {
                IRegexFormatMarker definition = (IRegexFormatMarker) SyntaxMarkers.GetDefinition(instructions[i]);
                if (definition.marker == null)
                {
                    sb.AppendLine();
                } else
                {
                    sb.Append(definition.Convert(instructions[i].Parameters));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Interprets the specified code into graph instruction sequence
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public GraphBuildInstructionSequence Interpret(String code)
        {
            GraphBuildInstructionSequence output = new GraphBuildInstructionSequence();

            List<String> lines = code.SplitSmart(Environment.NewLine);

            foreach (String line in lines)
            {
                regexMarkerResultCollection syntaxInstructions = SyntaxMarkers.process(line);
                foreach (regexMarkerResult result in syntaxInstructions.GetByOrder())
                {
                    GraphBuildInstruction instruction = new GraphBuildInstruction()
                    {
                        InstructionType = result.marker,
                        Parameters = result.GetGroups()
                    };
                    output.Add(instruction);
                }

                output.Add(new GraphBuildInstruction());
            }
            
            return output;
        }
    }
}