using imbSCI.Core.math.range.frequency;
using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using HtmlAgilityPack;
using System.Linq;
using imbSCI.Core.extensions.data;
using System.Xml.Serialization;
using imbSCI.Core.collection;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
  [Serializable]
    public class RecordTemplateItem
    {
        public NodeInTemplateRole Category { get; set; } = NodeInTemplateRole.Undefined;
        
        public String SubXPath = "";

        public RecordTemplateItem()
        {

        }
    }
}