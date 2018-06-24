namespace imbSCI.Graph.DiagramLibraries.DrawIO.core
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "foreground")]
    public class Foreground
    {
        [XmlElement(ElementName = "fillstroke")]
        public List<string> Fillstroke { get; set; }

        [XmlElement(ElementName = "save")]
        public List<string> Save { get; set; }

        [XmlElement(ElementName = "dashpattern")]
        public List<Dashpattern> Dashpattern { get; set; }

        [XmlElement(ElementName = "dashed")]
        public List<Dashed> Dashed { get; set; }

        [XmlElement(ElementName = "rect")]
        public List<Rect> Rect { get; set; }

        [XmlElement(ElementName = "stroke")]
        public List<string> Stroke { get; set; }

        [XmlElement(ElementName = "strokecolor")]
        public List<Strokecolor> Strokecolor { get; set; }

        [XmlElement(ElementName = "strokewidth")]
        public List<Strokewidth> Strokewidth { get; set; }

        [XmlElement(ElementName = "linejoin")]
        public Linejoin Linejoin { get; set; }

        [XmlElement(ElementName = "linecap")]
        public List<Linecap> Linecap { get; set; }

        [XmlElement(ElementName = "restore")]
        public List<string> Restore { get; set; }

        [XmlElement(ElementName = "fillcolor")]
        public List<Fillcolor> Fillcolor { get; set; }

        [XmlElement(ElementName = "alpha")]
        public List<Alpha> Alpha { get; set; }

        [XmlElement(ElementName = "path")]
        public List<Path> Path { get; set; }

        [XmlElement(ElementName = "ellipse")]
        public List<Ellipse> Ellipse { get; set; }
    }
}