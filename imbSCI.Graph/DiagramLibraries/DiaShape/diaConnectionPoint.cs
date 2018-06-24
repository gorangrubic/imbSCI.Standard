using System;
using System.Xml.Serialization;

namespace imbSCI.Graph.DiagramLibraries.DiaShape
{
    /// <summary>
    /// Connection point at shape.
    /// </summary>
    /// <seealso cref="diaShape"/>
    [XmlRoot(ElementName = "point")]
    public class diaConnectionPoint
    {
        public String toYesNo(Boolean input)
        {
            if (input) return "yes";
            return "no";
        }

        public Boolean fromYesNo(String input)
        {
            if (input.ToLower() == "yes") return true;
            return false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="diaConnectionPoint"/> class.
        /// </summary>
        public diaConnectionPoint()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="diaConnectionPoint"/> class.
        /// </summary>
        /// <param name="_x">The x.</param>
        /// <param name="_y">The y.</param>
        public diaConnectionPoint(Double _x, Double _y, Boolean _main = false)
        {
            x = _x;
            y = _y;
            main = toYesNo(_main);
        }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        [XmlAttribute]
        public Double x { get; set; } = 1;

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        [XmlAttribute]
        public Double y { get; set; } = 1;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="diaConnectionPoint"/> is main.
        /// </summary>
        /// <value>
        ///   <c>true</c> if main; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute]
        public String main { get; set; } = null;
    }
}