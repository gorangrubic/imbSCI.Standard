using System;
using System.Xml.Serialization;

namespace imbSCI.Graph.MXGraph.aborted
{
    /// <summary>
    /// Geometry definition
    /// </summary>
    public class mxGeometry
    {
        public mxGeometry()
        {
        }

        [XmlAttribute]
        public Int32 x { get; set; } = 0;

        [XmlAttribute]
        public Int32 y { get; set; } = 0;

        [XmlAttribute]
        public Int32 width { get; set; } = 0;

        [XmlAttribute]
        public Int32 height { get; set; } = 0;

        [XmlAttribute(AttributeName = "as")]
        public String mxAs { get; set; } = "";

        [XmlAttribute(DataType = "int")]
        public Boolean relative { get; set; } = false;
    }
}