using imbSCI.Graph.Graphics.SvgAPI.Core;
using imbSCI.Graph.Graphics.SvgDocument;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace imbSCI.Graph.Graphics.SvgAPI.BasicShapes
{
    /// <summary>
    /// Rectangle
    /// </summary>
    /// <seealso cref="imbSCI.Graph.Graphics.SvgAPI.Core.svgGraphicElementBase" />
    /// <seealso cref="imbSCI.Graph.Graphics.SvgDocument.ISVGTranform" />
    [XmlRoot("rect")]
    public class svgRectangle : svgGraphicElementBase, ISVGTranform
    {
        public svgRectangle()
        {
        }

        public svgRectangle(Int32 x, Int32 y, Int32 w, Int32 h)
        {
            point.x = x;
            point.y = y;

            point.width = w;
            point.height = h;
        }

        public override string name { get { return "rect"; } }

        public override XmlNode ToXml()
        {
            attributes.Set("width", point.width.ToString());
            attributes.Set("height", point.height.ToString());
            return ToXmlBase();
        }

        //public override string ToXmlString()
        //{
        //    return ToXml().OuterXml;
        //}
    }
}