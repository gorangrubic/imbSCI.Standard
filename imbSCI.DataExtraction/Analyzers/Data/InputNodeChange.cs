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
    public class InputNodeChange
    {
        public InputNodeChange()
        {

        }

        public Boolean IsSet()
        {
            if (name.IsNullOrEmpty())
            {
                return false;
            }

            
            if (NodeDefinition  == null) return false;
            return true;
        }

        public String name { get; set; } = "";

        [XmlIgnore]
        public InputNodeDefinition NodeDefinition { get; set; }

        public String oldValue { get; set; } = "";

        public String newValue { get; set; } = "";
    }
}