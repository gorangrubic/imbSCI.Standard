namespace imbSCI.Graph.DiagramLibraries.DrawIO
{
    using imbSCI.Graph.DiagramLibraries.DrawIO.core;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "shape")]
    public class Shape
    {
        public Shape()
        {
        }

        [XmlArray(ElementName = "connections")]
        [XmlArrayItem(ElementName = "constraint")]
        public List<Constraint> Connections { get; set; } = new List<Constraint>();

        [XmlElement(ElementName = "background")]
        public Background Background { get; set; }

        [XmlElement(ElementName = "foreground")]
        public Foreground Foreground { get; set; }

        [XmlAttribute(AttributeName = "aspect")]
        public string Aspect { get; set; }

        [XmlAttribute(AttributeName = "h")]
        public string H { get; set; }

        [XmlAttribute(AttributeName = "w")]
        public string W { get; set; }

        [XmlAttribute(AttributeName = "strokewidth")]
        public string Strokewidth { get; set; }
    }
}