namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System;
    using System.Collections;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "path")]
    public class Path
    {
        [XmlElement(ElementName = "move", Type = typeof(Move))]
        [XmlElement(ElementName = "line", Type = typeof(Line))]
        [XmlElement(ElementName = "close", Type = typeof(Object))]
        [XmlElement(ElementName = "curve", Type = typeof(Curve))]
        public ArrayList instructions { get; set; }
    }
}