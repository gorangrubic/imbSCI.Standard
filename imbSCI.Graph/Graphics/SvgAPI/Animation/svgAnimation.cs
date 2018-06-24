using imbSCI.Core.files;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace imbSCI.Graph.Graphics.SvgAPI.Animation
{
    [XmlRoot("animate")]
    public class svgAnimation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="svgAnimation"/> class.
        /// </summary>
        public svgAnimation()
        {
        }

        /// <summary>
        /// What attribute is animated
        /// </summary>
        public enum attributeTypeEnum
        {
            CSS,
            XML,
            auto
        }

        /// <summary>
        /// How animated value is changed
        /// </summary>
        public enum additiveTypeEnum
        {
            /// <summary>
            /// The replace - absolute value
            /// </summary>
            replace,

            /// <summary>
            /// The sum: additive animation
            /// </summary>
            sum
        }

        [XmlAttribute]
        public String dur { get; set; } = "";

        [XmlAttribute]
        public String from { get; set; } = "";

        [XmlAttribute]
        public String to { get; set; } = "";

        [XmlAttribute]
        public String repeatCount { get; set; } = "indefinite";

        [XmlAttribute]
        public String attributeName { get; set; } = "";

        [XmlAttribute]
        public additiveTypeEnum additive { get; set; } = additiveTypeEnum.replace;

        [XmlAttribute]
        public attributeTypeEnum attributeType { get; set; } = attributeTypeEnum.auto;

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            return objectSerialization.ObjectToXML(this);
        }
    }
}