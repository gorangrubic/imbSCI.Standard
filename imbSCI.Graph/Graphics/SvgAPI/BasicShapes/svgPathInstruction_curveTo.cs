using imbSCI.Graph.Graphics.SvgAPI.Core;
using imbSCI.Graph.Graphics.SvgDocument;
using System;

namespace imbSCI.Graph.Graphics.SvgAPI.BasicShapes
{
    public class svgPathInstruction_curveTo : svgPathInstructionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="svgPathInstruction_curveTo"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="relative">if set to <c>true</c> [relative].</param>
        public svgPathInstruction_curveTo(Int32 x, Int32 y, Boolean relative)
        {
            arguments = new SVGInlineArguments(new SVGLetterArgument("t"), new SVGPoint(x, y));

            IsAbsolute = !relative;
        }

        /// <summary>
        /// Quadratic bezier curve
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="cx">The cx.</param>
        /// <param name="cy">The cy.</param>
        /// <param name="relative">if set to <c>true</c> [relative].</param>
        public svgPathInstruction_curveTo(Int32 x, Int32 y, Int32 cx, Int32 cy, Boolean relative, Boolean CubicBezierChain = false)
        {
            if (CubicBezierChain)
            {
                arguments = new SVGInlineArguments(new SVGLetterArgument("s"), new SVGPoint(cx, cy), new SVGPoint(x, y));
            }
            else
            {
                arguments = new SVGInlineArguments(new SVGLetterArgument("q"), new SVGPoint(cx, cy), new SVGPoint(x, y));
            }

            IsAbsolute = !relative;
        }

        /// <summary>
        /// Cubic Bezier curve
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="c1x">The C1X.</param>
        /// <param name="c1y">The c1y.</param>
        /// <param name="c2x">The C2X.</param>
        /// <param name="c2y">The c2y.</param>
        /// <param name="relative">if set to <c>true</c> [relative].</param>
        public svgPathInstruction_curveTo(Int32 x, Int32 y, Int32 c1x, Int32 c1y, Int32 c2x, Int32 c2y, Boolean relative)
        {
            arguments = new SVGInlineArguments(new SVGLetterArgument("c"), new SVGPoint(c1x, c1y), new SVGPoint(c2x, c2y), new SVGPoint(x, y));
            IsAbsolute = !relative;
        }
    }
}