using imbSCI.Graph.Graphics.SvgAPI.Core;

namespace imbSCI.Graph.Graphics.SvgAPI.BasicShapes
{
    public class svgPathInstruction_closePath : svgPathInstructionBase
    {
        public svgPathInstruction_closePath()
        {
            arguments = new SVGInlineArguments(new SVGLetterArgument("z"));
        }
    }
}