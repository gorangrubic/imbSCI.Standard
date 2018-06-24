using imbSCI.Graph.Graphics.SvgAPI.Core;
using imbSCI.Graph.Graphics.SvgDocument;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace imbSCI.Graph.Graphics.SvgAPI.BasicShapes
{
    /// <summary>
    /// Line
    /// </summary>
    /// <seealso cref="imbSCI.Graph.Graphics.SvgAPI.Core.svgGraphicElementBase" />
    /// <seealso cref="imbSCI.Graph.Graphics.SvgDocument.ISVGTranform" />
    [XmlRoot("line")]
    public class svgLine : svgGraphicElementBase, ISVGTranform
    {
        public override string name { get { return "line"; } }

        public SVGPoint start { get; set; } = new SVGPoint();

        public SVGPoint end { get; set; } = new SVGPoint();

        public svgLine()
        {
        }

        public svgLine(Int32 x1, Int32 y1, Int32 x2, Int32 y2)
        {
            start = new SVGPoint(x1, y1);
            end = new SVGPoint(x2, y2);
        }

        public override XmlNode ToXml()
        {
            attributes.Set("x1", start.X.ToString());
            attributes.Set("x2", end.X.ToString());

            attributes.Set("y1", start.Y.ToString());
            attributes.Set("y2", end.Y.ToString());

            return ToXmlBase();
        }

        //public override string ToXmlString()
        //{
        //    return ToXml().OuterXml;
        //}
    }
}