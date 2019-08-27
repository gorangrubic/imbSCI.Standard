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
    public enum InputNodeTypeClass
    {
        button,
        data
    }

   [Serializable]
    public class InputNodeDefinition:DocumentNodeDefinition
    {

        public InputNodeDefinition()
        {

        }

        public InputNodeDefinition(LeafNodeDictionaryEntry _entry, HtmlNode _node)
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
                    case nameof(type):
                        type = attribute.Value;
                        break;
                    case nameof(name):
                        name = attribute.Value;
                        break;
                    case nameof(value):
                        value = attribute.Value;
                        break;
                    default:
                        break;
                }
            }

            switch(type)
            {
                case "button":
                case "reset":
                case "submit":
                    typeClass = InputNodeTypeClass.button;
                    break;
                default:
                    typeClass = InputNodeTypeClass.data;
                    break;
            }

        }

        public String GetValue(HtmlNode newNode=null)
        {
            if (newNode == null) newNode = entry.node;

            String output = "";

            output = newNode.GetAttributeValue(nameof(value), "");
            if (output.isNullOrEmpty())
            {
                output = newNode.InnerText;
            }
            return output;
        }

        public InputNodeTypeClass typeClass = InputNodeTypeClass.data;

        public Boolean DoWatch { get; set; } = false;

        public LeafNodeDictionaryEntry entry { get; set; }

        public String type { get; set; } = "text";
        
        public String name { get; set; } = "";

        public String xkey {
            get
            {
                if (!name.isNullOrEmpty())
                {
                    return "//input[@name='" + name + "']";
                }
                if (!id.isNullOrEmpty())
                {
                    return "//input[@id='" + name + "']";
                }
                return entry.XPath;
            }
        }

        public Boolean IsMatch(String key_needle)
        {
            if (name.Equals(key_needle, StringComparison.InvariantCultureIgnoreCase)) return true;
            if (id.Equals(key_needle, StringComparison.InvariantCultureIgnoreCase)) return true;
            if (xkey.Equals(key_needle, StringComparison.InvariantCultureIgnoreCase)) return true;
            return false;
        }
        
        public LabelNodeDefinition Label { get; set; } 

        

        public String value { get; set; } = "";

        public List<String> options { get; set; } = new List<string>();

        public String SelectedOption { get; set; } = "";

    }
}