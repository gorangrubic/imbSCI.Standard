using System;
using System.Xml.Serialization;

namespace imbSCI.Graph.DiagramLibraries.DiaShape
{
    /// <summary>
    /// Aspect ratio
    /// </summary>
    public class diaShapeAspectRatio
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="diaShapeAspectRatio"/> class.
        /// </summary>
        public diaShapeAspectRatio()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="diaShapeAspectRatio"/> class.
        /// </summary>
        /// <param name="_type">The type.</param>
        /// <param name="_min">The minimum.</param>
        /// <param name="_max">The maximum.</param>
        public diaShapeAspectRatio(diaShapeAspectRatioEnum _type, Int32 _min = 0, Int32 _max = 0)
        {
            min = _min;
            max = _max;

            type = _type;
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        [XmlAttribute]
        public Int32 min { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        [XmlAttribute]
        public Int32 max { get; set; } = 0;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [XmlAttribute]
        public diaShapeAspectRatioEnum type { get; set; } = diaShapeAspectRatioEnum.free;
    }
}