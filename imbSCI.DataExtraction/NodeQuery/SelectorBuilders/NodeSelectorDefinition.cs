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
    public class NodeSelectorDefinition
    {
        public List<NodeSelectorPredicate> predicates = new List<NodeSelectorPredicate>();

        //public NodeSelectorDefinition SetTagPredicates(params String[] tagnames)
        //{
        //}
    }
}