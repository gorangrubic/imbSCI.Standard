namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "linejoin")]
    public class Linejoin
    {
        [XmlAttribute(AttributeName = "join")]
        public string Join { get; set; }
    }
}