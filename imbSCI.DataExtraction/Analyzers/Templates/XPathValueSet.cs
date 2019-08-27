using HtmlAgilityPack;
using imbSCI.Core.extensions;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Data;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions.data;
using imbSCI.DataExtraction.Extractors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
    /// <summary>
    /// Single XPath data pointer
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.DataExtraction.Analyzers.Data.XPathValueSet}" />
    public class XPathValueSet:List<XPathValueSet>
    {
        public XPathValueSet()
        {

        }
        public String XPath { get; set; }
        public String Value { get; set; }
    }
}