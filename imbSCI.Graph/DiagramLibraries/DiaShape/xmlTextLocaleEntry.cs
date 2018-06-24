using System;
using System.Xml;
using System.Xml.Serialization;

namespace imbSCI.Graph.DiagramLibraries.DiaShape
{
    /// <summary>
    /// Holder of localised textual representation
    /// </summary>
    public class xmlTextLocaleEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="xmlTextLocaleEntry"/> class.
        /// </summary>
        public xmlTextLocaleEntry()
        {
        }

        public xmlTextLocaleEntry(String _text)
        {
            text = _text;
        }

        /// <summary>
        /// Textual representation
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [XmlText]
        public String text { get; set; }

        /// <summary>
        /// ISO Alpha 2-letter code for language
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        [XmlAttribute(AttributeName = "xml:lang")]
        public String lang { get; set; }
    }
}