using imbSCI.Graph.Graphics.SvgAPI.Core;
using imbSCI.Graph.Graphics.SvgDocument;
using System;
using System.Collections.Generic;
using System.Xml;

namespace imbSCI.Graph.Graphics.SvgAPI.BasicShapes
{
    public abstract class svgMultiPointGraphicsBase : svgGraphicElementBase
    {
        protected svgMultiPointGraphicsBase()
        {
        }

        /// <summary>
        /// Creates new point and adds it into collection
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public SVGPoint Add(Int32 x, Int32 y)
        {
            SVGPoint p = new SVGPoint(x, y);
            points.Add(p);
            return p;
        }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public List<SVGPoint> points { get; set; } = new List<SVGPoint>();

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override XmlNode ToXml()
        {
            attributes.Set("points", points.GetStringFromPoints());

            var output = ToXmlBase();

            return output;
        }

        ///// <summary>
        ///// To the XML string.
        ///// </summary>
        ///// <returns></returns>
        //public override string ToXmlString()
        //{
        //    return ToXml().OuterXml;
        //}
    }
}