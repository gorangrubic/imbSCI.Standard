namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "linecap")]
    public class Linecap
    {
        [XmlAttribute(AttributeName = "cap")]
        public string Cap { get; set; }
    }
}