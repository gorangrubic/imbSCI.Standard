using HtmlAgilityPack;
using imbSCI.Data;
using imbSCI.DataExtraction.NodeQuery.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.DataExtraction.NodeQuery.Evaluators
{
public static class NodeQueryExtensions
    {
        public static List<HtmlNode> SelectNodes(this IEnumerable<HtmlNode> nodes, String xPath)
        {
            List<HtmlNode> output = new List<HtmlNode>();
            if (nodes == null) return output;
            if (xPath.isNullOrEmpty())
            {
                return output;
            }
            foreach (var n in nodes)
            {
                var ns = n.SelectNodes(xPath);
                if (ns != null) output.AddRange(ns);
            }
            return output;
        }

    }
}