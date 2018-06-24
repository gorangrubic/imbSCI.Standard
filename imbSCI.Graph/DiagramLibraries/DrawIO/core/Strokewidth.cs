namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "strokewidth")]
    public class Strokewidth
    {
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
    }
}