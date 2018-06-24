namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "background")]
    public class Background
    {
        [XmlElement(ElementName = "strokecolor")]
        public Strokecolor Strokecolor { get; set; }

        [XmlElement(ElementName = "fillcolor")]
        public Fillcolor Fillcolor { get; set; }

        [XmlElement(ElementName = "rect")]
        public Rect Rect { get; set; }
    }
}