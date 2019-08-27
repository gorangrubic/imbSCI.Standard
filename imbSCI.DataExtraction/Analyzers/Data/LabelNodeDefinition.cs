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
    public class LabelNodeDefinition : DocumentNodeDefinition
    {
        public LabelNodeDefinition()
        {

        }

        public String for_attribute { get; set; } = "";


        public LeafNodeDictionaryEntry entry { get; set; }

        public LabelNodeDefinition(HtmlNode _node, LeafNodeDictionaryEntry _entry)
        {
            entry = _entry;
            entry.MetaData = this;
            Deploy(_node);
        }

        protected override void Deploy(HtmlNode node)
        {
            base.Deploy(node);

            foreach (var attribute in node.Attributes)
            {
                switch (attribute.Name)
                {
                    case "for":
                        for_attribute = attribute.Value;
                        break;
                    
                    default:
                        break;
                }
            }
        }
    }
}