namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "dashpattern")]
    public class Dashpattern
    {
        [XmlAttribute(AttributeName = "pattern")]
        public string Pattern { get; set; }
    }
}