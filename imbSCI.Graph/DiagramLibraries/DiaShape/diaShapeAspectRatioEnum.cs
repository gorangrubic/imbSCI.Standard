using System.Xml.Serialization;

namespace imbSCI.Graph.DiagramLibraries.DiaShape
{
    [XmlRoot("aspectratio")]
    public enum diaShapeAspectRatioEnum
    {
        /// <summary>
        /// The free
        /// </summary>
        free,

        /// <summary>
        /// The fixed
        /// </summary>
        @fixed,

        /// <summary>
        /// The range
        /// </summary>
        range
    }
}