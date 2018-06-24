namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "constraint")]
    public class Constraint
    {
        [XmlAttribute(AttributeName = "x")]
        public string X { get; set; }

        [XmlAttribute(AttributeName = "y")]
        public string Y { get; set; }

        [XmlAttribute(AttributeName = "perimeter")]
        public string Perimeter { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}