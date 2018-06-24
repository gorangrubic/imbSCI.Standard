using System;
using System.Xml;
using System.Xml.Serialization;

namespace imbSCI.Graph.DiagramLibraries.DiaShape
{
    [XmlRoot("object")]
    public class diaSheetObject
    {
        public diaSheetObject()
        {
        }

        [XmlAttribute]
        public String name { get; set; } = "";

        [XmlElement(ElementName = "description")]
        public xmlTextLocaleEntry[] descriptions { get; set; } = new xmlTextLocaleEntry[] { };
    }
}