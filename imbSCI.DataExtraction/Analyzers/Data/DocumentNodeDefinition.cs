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
    public class DocumentNodeDefinition
    {
        public DocumentNodeDefinition()
        {

        }

        public String tag { get; set; } = "";
        public List<String> classList { get; set; } = new List<string>();
        public String id { get; set; } = "";
        public String content { get; set; } = "";

        protected virtual void Deploy(HtmlNode node)
        {
            tag = node.Name;
            content = node.InnerText;

            foreach (var attribute in node.Attributes)
            {
                switch (attribute.Name)
                {
                    case "id":
                        id = attribute.Value;
                        break;
                    case "class":
                        //classList.AddRange(attribute.Value.Split()
                        //type = .SplitSmart();
                        break;
                 
                    default:
                        break;
                }
            }
        }
    }
}