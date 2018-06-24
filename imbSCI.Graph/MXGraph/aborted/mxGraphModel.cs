using System;
using System.Xml.Serialization;

namespace imbSCI.Graph.MXGraph.aborted
{
    /// <summary>
    /// Object model for jGraph / mxGraphModel, used by draw.io diagraming tool
    /// </summary>
    public class mxGraphModel
    {
        public mxGraphModel()
        {
        }

        [XmlAttribute(AttributeName = "dx", DataType = "int")]
        public Int32 dx { get; set; } = 1426;

        [XmlAttribute(AttributeName = "dy", DataType = "int")]
        public Int32 dy { get; set; } = 875;

        /// <summary>
        /// If the grid is on
        /// </summary>
        /// <value>
        /// The grid.
        /// </value>
        [XmlAttribute(AttributeName = "grid", DataType = "int")]
        public Boolean grid { get; set; } = true;

        [XmlAttribute]
        public Int32 gridSize { get; set; } = 10;

        [XmlAttribute(AttributeName = "guides", DataType = "int")]
        public Boolean guides { get; set; } = true;

        [XmlAttribute(DataType = "int")]
        public Boolean tooltips { get; set; } = true;

        [XmlAttribute(DataType = "int")]
        public Boolean connect { get; set; } = true;

        [XmlAttribute(DataType = "int")]
        public Boolean arrows { get; set; } = true;

        [XmlAttribute(DataType = "int")]
        public Boolean fold { get; set; } = true;

        [XmlAttribute]
        public Int32 page { get; set; } = 1;

        [XmlAttribute]
        public Double pageScale { get; set; } = 1.0;

        [XmlAttribute]
        public Int32 pageWidth { get; set; } = 1169;

        [XmlAttribute]
        public Int32 pageHeight { get; set; } = 827;

        [XmlAttribute]
        public String background { get; set; } = "#ffffff";

        [XmlAttribute(DataType = "int")]
        public Boolean math { get; set; } = false;

        [XmlAttribute(DataType = "int")]
        public Boolean shadow { get; set; } = false;
    }
}