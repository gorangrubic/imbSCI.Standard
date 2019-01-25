using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataComplex.converters.data
{
public class aPlotDataElementWithCommentBase: aPlotDataElementBase
    {
        public String comment { get; set; } = "";
    }
}