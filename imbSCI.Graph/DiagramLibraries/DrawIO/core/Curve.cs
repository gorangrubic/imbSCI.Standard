namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "curve")]
    public class Curve
    {
        [XmlAttribute(AttributeName = "x1")]
        public string X1 { get; set; }

        [XmlAttribute(AttributeName = "x2")]
        public string X2 { get; set; }

        [XmlAttribute(AttributeName = "x3")]
        public string X3 { get; set; }

        [XmlAttribute(AttributeName = "y1")]
        public string Y1 { get; set; }

        [XmlAttribute(AttributeName = "y2")]
        public string Y2 { get; set; }

        [XmlAttribute(AttributeName = "y3")]
        public string Y3 { get; set; }
    }
}