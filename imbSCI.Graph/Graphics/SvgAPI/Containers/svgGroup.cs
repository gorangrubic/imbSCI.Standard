using System;
using System.Xml.Serialization;

namespace imbSCI.Graph.Graphics.SvgAPI.Containers
{
    [XmlRoot("g")]
    public class svgGroup : svgContainerElement
    {
        public svgGroup(String _id)
        {
            id = _id;
        }

        [XmlIgnore]
        public override String name
        {
            get { return "g"; }
        }
    }
}