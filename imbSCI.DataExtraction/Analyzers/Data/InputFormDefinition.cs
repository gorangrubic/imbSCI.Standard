using HtmlAgilityPack;
using imbSCI.Core.math;
using imbSCI.DataExtraction.Tools;
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
public class InputFormDefinition : DocumentNodeDefinition
    {
        public String method { get; set; } = "";

        public String action { get; set; } = "";


        public String name { get; set; } = "";

        public String target { get; set; } = "";

        public String enctype { get; set; } = "";

        public String accept_charset { get; set; } = "";


        public void Add(InputNodeDefinition ind)
        {
            InputNodes.Add(ind);
            //switch(ind.)
        }

        public String GetMethodURL(String root="", HtmlNode formNode=null)
        {
            if (formNode == null) formNode = entry.node;

            String sb = root;
            sb = sb.add(action, "");
            var inputNodes = InputNodes.Where(x => x.typeClass == InputNodeTypeClass.data).ToList();
            if (inputNodes.Any())
            {
                sb = sb + "?";
            }

            String inner = "";
            foreach (var ind in inputNodes)
            {
                String relXpath = ind.entry.XPath.GetRelativeXPath(entry.XPath);
                HtmlNode indn = formNode.selectSingleNode(relXpath);

                String pair = ind.name + "=" + ind.GetValue(indn);
                inner = inner.add(pair, "&");
            }

            sb = sb + inner;

            return sb;
        }

        public List<InputNodeDefinition> InputNodes { get; set; } = new List<InputNodeDefinition>();

        protected override void Deploy(HtmlNode node)
        {
            base.Deploy(node);

            foreach (var attribute in node.Attributes)
            {
                switch (attribute.Name)
                {
                    case nameof(method):
                        method = attribute.Value;
                        break;
                    case nameof(enctype):
                        enctype = attribute.Value;
                        break;
                    case nameof(target):
                        target = attribute.Value;
                        break;
                    case "name":
                        name = attribute.Value;
                        break;
                    case "accept-charset":
                        accept_charset = attribute.Value;
                        break;
                    default:
                        break;
                }
            }

        }

        public LeafNodeDictionaryEntry entry { get; set; }

        public InputFormDefinition()
        {

        }

        public InputFormDefinition(LeafNodeDictionaryEntry _entry, HtmlNode _node)
        {

            entry = _entry;
            entry.MetaData = this;
            Deploy(_node);
        }
    }
}