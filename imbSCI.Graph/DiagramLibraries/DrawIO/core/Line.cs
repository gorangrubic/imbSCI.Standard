namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "line")]
    public class Line
    {
        [XmlAttribute(AttributeName = "x")]
        public string X { get; set; }

        [XmlAttribute(AttributeName = "y")]
        public string Y { get; set; }
    }
}