using System;

namespace imbSCI.Graph.Graphics.SvgAPI.Core
{
    public class SVGBooleanArgument : ISVGInlineArgument
    {
        public SVGBooleanArgument()
        {
        }

        public SVGBooleanArgument(Boolean input)
        {
            value = input;
        }

        public override string ToString()
        {
            if (value) return "1";
            return "0";
        }

        public void FromString(String input)
        {
            if (input == "1")
            {
                value = true;
            }
            value = false;
        }

        public Boolean value { get; set; }
    }
}