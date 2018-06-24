using System;
using System.Xml.Serialization;

namespace imbSCI.Graph.Graphics.SvgAPI.Containers
{
    [XmlRoot("a")]
    public class svgLink : svgContainerElement
    {
        public svgLink(String href)
        {
            attributes.Set("href", href);
        }

        [XmlIgnore]
        public override String name
        {
            get { return "a"; }
        }
    }
}