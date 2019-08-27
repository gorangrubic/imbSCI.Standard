using HtmlAgilityPack;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.math.classificationMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.SourceData
{
    /// <summary>
    /// Data cell in source table
    /// </summary>
    public class SourceTableCell
    {
        public reportExpandedData AttributeData { get; set; } = new reportExpandedData();

        public String Value { get; set; }

        /// <summary>
        /// Gets or sets the source value string (XML, HTML, JSON... or whatever was the source for this cell) - inner XML
        /// </summary>
        /// <value>
        /// The source value string.
        /// </value>
        public String SourceValueString { get; set; } = "";

        public String SourceCellTagName { get; set; } = "";

        public String SourceCellXPath { get; set; } = "";

        [XmlIgnore]
        public HtmlNode SourceNode { get; set; } 

        public SourceTableCell()
        {
        }
    }
}