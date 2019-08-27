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
using imbSCI.DataExtraction.MetaTables.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
    /// <summary>
    /// Value setters/getters for collection of blocks
    /// </summary>
    [Serializable]
    public class XPathValueSetForRecordCollection
    {
        public XPathValueSetForRecordCollection()
        {

        }
        public DataPointMapBlock Block { get; set; } = new DataPointMapBlock();
        public List<XPathValueSetForRecord> Records { get; set; } = new List<XPathValueSetForRecord>();

        public MetaTable GetMetaTable()
        {
            MetaTable output = new MetaTable();

            return output;
            // 
        }

    }
}