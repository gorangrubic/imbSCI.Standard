using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery
{
    [Serializable]
    public class NodeQueryEvaluationContext
    {
        public NodeQueryEvaluationContext()
        {
        }

        public List<HtmlNode> htmlNodes { get; set; }

        public String documentText { get; set; } = "";

        public String documentUrl { get; set; } = "";
    }
}