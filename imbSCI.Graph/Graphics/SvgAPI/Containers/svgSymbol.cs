using System;
using System.Xml.Serialization;

namespace imbSCI.Graph.Graphics.SvgAPI.Containers
{
    [XmlRoot("symbol")]
    public class svgSymbol : svgContainerElement
    {
        public override String name { get { return "symbol"; } }

        public svgSymbol(String _id)
        {
            id = _id;
        }
    }
}