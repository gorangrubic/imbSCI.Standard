namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "dashed")]
    public class Dashed
    {
        [XmlAttribute(AttributeName = "dashed")]
        public string _dashed { get; set; }
    }
}