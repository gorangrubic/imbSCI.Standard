using imbSCI.Core.reporting.zone;
using imbSCI.Graph.Graphics.SvgAPI.Core;
using System;

namespace imbSCI.Graph.Graphics.SvgAPI
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class svgModificationExtensions
    {
        /// <summary>
        /// The att transformation
        /// </summary>
        public const String ATT_TRANSFORMATION = "transform";

        /// <summary>
        /// Clears existing transformation instructions and sets isomentric 3D transformation
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="pitch">The pitch.</param>
        /// <returns>Reference to the element</returns>
        public static svgGraphicElementBase isometricTransformation(this svgGraphicElementBase element, Double pitch = 0.7)
        {
            element.attributes.Remove(ATT_TRANSFORMATION);
            element.rotate(45, textCursorZoneCorner.center);
            element.scale(1, pitch);
            return element;
        }

        public static String SKEW_FORMAT = "skew{0}({1})";
        public static String TRANSLATE_FORMAT = "translate({0} {1}})";
        public static String SCALE_FORMAT = "scale({0} {1})";
        public static String ROTATE_FORMAT = "rotate({0} {1} {2})";

        public static svgGraphicElementBase ClearTransformations(this svgGraphicElementBase element)
        {
            element.attributes.Remove(ATT_TRANSFORMATION);
            return element;
        }

        /// <summary>
        /// Skews the xy.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="aX">a x.</param>
        /// <param name="aY">a y.</param>
        /// <returns></returns>
        public static svgGraphicElementBase skewXY(this svgGraphicElementBase element, Int32 aX, Int32 aY = 0)
        {
            if (aX != 0) element.attributes.Append(ATT_TRANSFORMATION, String.Format(SKEW_FORMAT, "X", aX));

            if (aY != 0) element.attributes.Append(ATT_TRANSFORMATION, String.Format(SKEW_FORMAT, "Y", aY));

            return element;
        }

        /// <summary>
        /// Translates the specified d x.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="dX">The d x.</param>
        /// <param name="dY">The d y.</param>
        /// <returns></returns>
        public static svgGraphicElementBase translate(this svgGraphicElementBase element, Int32 dX, Int32 dY = 0)
        {
            element.attributes.Append(ATT_TRANSFORMATION, String.Format(TRANSLATE_FORMAT, dX, dY));

            return element;
        }

        /// <summary>
        /// Scales the specified x.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static svgGraphicElementBase scale(this svgGraphicElementBase element, Double x, Double y)
        {
            element.attributes.Append(ATT_TRANSFORMATION, String.Format(SCALE_FORMAT, x, y));
            return element;
        }

        /// <summary>
        /// Rotates the element for specified angle.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="rX">The r x.</param>
        /// <param name="rY">The r y.</param>
        /// <returns></returns>
        public static svgGraphicElementBase rotate(this svgGraphicElementBase element, Int32 angle, Int32 rX, Int32 rY)
        {
            element.attributes.Append(ATT_TRANSFORMATION, String.Format(ROTATE_FORMAT, angle, rX, rY));
            return element;
        }

        /// <summary>
        /// Rotates the element for specified angle.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="pivot">The pivot.</param>
        /// <returns></returns>
        public static svgGraphicElementBase rotate(this svgGraphicElementBase element, Int32 angle, textCursorZoneCorner pivot)
        {
            var p = element.point.GetCornerPoint(pivot);
            return element.rotate(angle, p.x, p.y);
        }
    }
}