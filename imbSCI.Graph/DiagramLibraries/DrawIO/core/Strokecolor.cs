namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "strokecolor")]
    public class Strokecolor
    {
        [XmlAttribute(AttributeName = "color")]
        public string Color { get; set; }
    }
}