namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "fillcolor")]
    public class Fillcolor
    {
        [XmlAttribute(AttributeName = "color")]
        public string Color { get; set; }
    }
}