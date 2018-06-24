using imbSCI.Graph.Graphics.SvgAPI.Core;
using imbSCI.Graph.Graphics.SvgDocument;
using System;
using System.Xml;

namespace imbSCI.Graph.Graphics.SvgAPI.BasicShapes
{
    /// <summary>
    /// Square - based Rect
    /// </summary>
    /// <seealso cref="imbSCI.Graph.Graphics.SvgAPI.Core.svgGraphicElementBase" />
    /// <seealso cref="imbSCI.Graph.Graphics.SvgDocument.ISVGTranform" />
    public class svgSquare : svgGraphicElementBase, ISVGTranform
    {
        public svgSquare()
        {
        }

        public svgSquare(Int32 x, Int32 y, Int32 a)
        {
            point.x = x;
            point.y = y;

            point.width = a;
            point.height = a;
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