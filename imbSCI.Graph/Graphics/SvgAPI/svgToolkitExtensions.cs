using imbSCI.Graph.Graphics.SvgDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.Graph.Graphics.SvgAPI
{
    /// <summary>
    /// Converters from string formats to DOM elements and vice versa. Used internally by the SVG framework
    /// </summary>
    public static class svgToolkitExtensions
    {
        public static Regex REGEX_POINT_PAIR = new Regex(@"([\d\.]*)\s*,\s*([\d\.]*)");

        public static String FORMAT_POINT_PAIR = "{0},{1}";

        /// <summary>
        /// Makes string for points, in format: x1,y1 x2,y2 x3,y3 .... where xn,yn are coordinates of point n
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static String GetStringFromPoints(this IEnumerable<SVGPoint> input)
        {
            StringBuilder sb = new StringBuilder();
            List<SVGPoint> li = input.ToList();
            if (!li.Any()) return "";

            var last = li.Last();

            foreach (SVGPoint m in li)
            {
                sb.AppendFormat(FORMAT_POINT_PAIR, m.X, m.Y);
                if (m != last) sb.Append(" ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the point from string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static SVGPoint GetPointFromString(this String input)
        {
            foreach (Match m in REGEX_POINT_PAIR.Matches(input))
            {
                if (m.Success)
                {
                    if (m.Groups.Count > 2)
                    {
                        SVGPoint p = new SVGPoint();

                        p.X = Convert.ToDouble(m.Groups[2]);
                        p.Y = Convert.ToDouble(m.Groups[3]);
                        return p;
                    }
                }
            }
            throw new ArgumentException("input string is not in required format", "input");
            return null;
        }

        /// <summary>
        /// Gets points from string: x1,y1 x2,y2 x3,y3 .... where xn,yn are coordinates of point n
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static List<SVGPoint> GetPointsFromString(this String input)
        {
            List<SVGPoint> output = new List<SVGPoint>();

            foreach (Match m in REGEX_POINT_PAIR.Matches(input))
            {
                if (m.Success)
                {
                    if (m.Groups.Count > 2)
                    {
                        SVGPoint p = new SVGPoint();

                        p.X = Convert.ToDouble(m.Groups[2]);
                        p.Y = Convert.ToDouble(m.Groups[3]);
                        output.Add(p);
                    }
                }
            }

            return output;
        }
    }
}