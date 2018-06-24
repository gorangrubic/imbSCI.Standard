using System;
using System.Xml;
using System.Xml.Serialization;

namespace imbSCI.Graph.DiagramLibraries.DiaShape
{
    /// <summary>
    /// Custom diaShape attribute
    /// </summary>
    public class diaShapeAttribute
    {
        public diaShapeAttribute()
        {
        }

        [XmlAttribute]
        public String name { get; set; }

        [XmlAttribute]
        public String type { get; set; }
    }
}