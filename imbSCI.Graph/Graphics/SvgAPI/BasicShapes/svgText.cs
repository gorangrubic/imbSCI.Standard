using imbSCI.Graph.Graphics.SvgAPI.Core;
using imbSCI.Graph.Graphics.SvgDocument;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace imbSCI.Graph.Graphics.SvgAPI.BasicShapes
{
    /// <summary>
    /// Text
    /// </summary>
    /// <seealso cref="imbSCI.Graph.Graphics.SvgAPI.Core.svgGraphicElementBase" />
    /// <seealso cref="imbSCI.Graph.Graphics.SvgDocument.ISVGTranform" />
    [XmlRoot("text")]
    public class svgText : svgGraphicElementBase, ISVGTranform
    {
        public svgText()
        {
        }

        public svgText(Int32 _x, Int32 _y, String text, Int32 textLength = Int32.MinValue)
        {
            textContent = text;
            x = _x;
            y = _y;

            if (textLength != Int32.MinValue)
            {
                attributes.Set("textLength", textLength.ToString());
            }
        }

        public override string name { get { return "text"; } }

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