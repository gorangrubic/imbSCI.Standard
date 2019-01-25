using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataComplex.converters.data
{
[XmlRoot(ElementName ="table")]
    public class aPlotDataTable : aPlotDataElementWithCommentBase
    {
        public aPlotDataTable()
        {

        }

        [XmlAttribute()]
        public Int32 columns { get; set; } = 0;

        [XmlAttribute()]
        public Int32 rows { get; set; } = 0;
    }
}