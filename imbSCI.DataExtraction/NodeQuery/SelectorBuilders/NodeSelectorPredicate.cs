using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Data;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.DataExtraction.NodeQuery.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery.SelectorBuilders
{
    public class NodeSelectorPredicate
    {
        public NodeSelectorPredicateType type { get; set; } = NodeSelectorPredicateType.tagName;

        public String name { get; set; } = "";
    }
}