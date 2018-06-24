using System;
using System.Xml.Serialization;

namespace imbSCI.Graph.MXGraph.aborted
{
    /// <summary>
    /// Cell is abstract base unit of an mxGraph object, representing node or link
    /// </summary>
    public class mxCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="mxCell"/> class.
        /// </summary>
        public mxCell() { }

        [XmlAttribute(DataType = "string")]
        public Int32 id { get; set; } = 0;

        [XmlAttribute(DataType = "string")]
        public Int32 parent { get; set; } = 0;

        [XmlAttribute(DataType = "string")]
        public Int32 source { get; set; } = 0;

        [XmlAttribute(DataType = "string")]
        public Int32 target { get; set; } = 0;

        [XmlAttribute]
        public String style { get; set; } = "";

        [XmlAttribute]
        public String value { get; set; } = "";

        [XmlAttribute(DataType = "int")]
        public Boolean edge { get; set; } = false;
    }
}