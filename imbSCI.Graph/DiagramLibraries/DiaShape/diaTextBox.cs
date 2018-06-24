using System;
using System.Xml.Serialization;

namespace imbSCI.Graph.DiagramLibraries.DiaShape
{
    /// <summary>
    /// Text box definition for Dia diagramming software
    /// </summary>
    [XmlRoot("textbox")]
    public class diaTextBox
    {
        /// <summary>
        /// Defines text box in Dia shape definition
        /// </summary>
        public diaTextBox()
        {
        }

        /// <summary>
        /// Gets or sets the x1.
        /// </summary>
        /// <value>
        /// The x1.
        /// </value>
        [XmlAttribute]
        public String x1 { get; set; } = "left";

        /// <summary>
        /// Gets or sets the y1.
        /// </summary>
        /// <value>
        /// The y1.
        /// </value>
        [XmlAttribute]
        public String y1 { get; set; } = "top";

        /// <summary>
        /// Gets or sets the x2.
        /// </summary>
        /// <value>
        /// The x2.
        /// </value>
        [XmlAttribute]
        public String x2 { get; set; } = "right";

        /// <summary>
        /// Gets or sets the y2.
        /// </summary>
        /// <value>
        /// The y2.
        /// </value>
        [XmlAttribute]
        public String y2 { get; set; } = "bottom";

        /// <summary>
        /// Gets or sets the align.
        /// </summary>
        /// <value>
        /// The align.
        /// </value>
        [XmlAttribute]
        public String align { get; set; } = null;

        /// <summary>
        /// Gets or sets the resize.
        /// </summary>
        /// <value>
        /// The resize.
        /// </value>
        [XmlAttribute]
        public String resize { get; set; } = null;
    }
}