using imbSCI.Graph.Graphics.SvgAPI.Core;
using System;
using System.Linq;

namespace imbSCI.Graph.Graphics.SvgAPI.BasicShapes
{
    public class svgPathInstructionBase
    {
        protected svgPathInstructionBase()
        {
        }

        public Boolean IsAbsolute
        {
            get
            {
                if (arguments.Any())
                {
                    if (arguments[0] is SVGLetterArgument letterArgument)
                    {
                        return letterArgument.isUpperCase;
                    }
                }
                return false;
            }
            set
            {
                if (arguments.Any())
                {
                    if (arguments[0] is SVGLetterArgument letterArgument)
                    {
                        letterArgument.isUpperCase = value;
                    }
                }
            }
        }

        public String name
        {
            get
            {
                if (arguments.Any())
                {
                    if (arguments[0] is SVGLetterArgument letterArgument)
                    {
                        return letterArgument.valueLetter;
                    }
                }
                return "";
            }
            set
            {
                if (!arguments.Any()) arguments.Add(new SVGLetterArgument());

                SVGLetterArgument letterArgument = arguments[0] as SVGLetterArgument;

                if (letterArgument == null)
                {
                    letterArgument = new SVGLetterArgument();
                    arguments.Insert(0, letterArgument);
                }

                letterArgument.valueLetter = value;
            }
        }

        public SVGInlineArguments arguments { get; protected set; } = new SVGInlineArguments();

        public void FromString(String input)
        {
            arguments.FromString(input);
        }

        public override string ToString()
        {
            return arguments.ToString();
        }
    }
}