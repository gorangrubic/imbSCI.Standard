using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataComplex.converters.data
{
public class aPlotDataElementBase
    {
        [XmlAttribute()]
        public String creation_time { get; set; } = "";

        [XmlAttribute()]
        public String caption_spec { get; set; } = "%n%C{ - }%c";

        [XmlAttribute()]
        public String name { get; set; }

        

    }
}