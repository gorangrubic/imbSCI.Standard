namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "alpha")]
    public class Alpha
    {
        [XmlAttribute(AttributeName = "alpha")]
        public string _alpha { get; set; }
    }
}