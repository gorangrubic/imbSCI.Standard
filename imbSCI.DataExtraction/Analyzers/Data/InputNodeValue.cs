using HtmlAgilityPack;
using imbSCI.Core.math;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions;
using imbSCI.Data;
using imbSCI.Core.extensions.data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using imbSCI.Core.extensions.text;
using imbSCI.Data;

namespace imbSCI.DataExtraction.Analyzers.Data
{

    [Serializable]
    public class InputNodeValue
    {
        public InputNodeValue()
        {

        }
        public String xkey { get; set; } = "";

        public String value { get; set; } = "";
    }
}