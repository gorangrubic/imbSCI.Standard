using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery
{
    [Serializable]
    public class NodeQueryCollection : List<NodeQueryDefinition>
    {
        public NodeQueryCollection()
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }
    }
}