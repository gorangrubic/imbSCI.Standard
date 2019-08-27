using HtmlAgilityPack;
using imbSCI.Core.reporting.render;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Tools
{
    public static class HtmlExtractionTools
    {
        public const String XPATH_SELECT_TABLEROWS = "tr";
        public const String XPATH_SELECT_TABLEHEADCELLS = "th";
        public const String XPATH_SELECT_TABLECELLS = "td";

        public const String XPATH_SELECT_LIST_DT = "dt";
        public const String XPATH_SELECT_LIST_DD = "dd";

        public static List<HtmlNode> SelectNodesInDepthRange(this HtmlNode node, Func<HtmlNode, Boolean> evaluation, Int32 depthLimit = 3, Int32 depthStart = 1, Boolean onlyLeafs = true)
        {
            List<HtmlNode> output = null;

            if (output == null) output = new List<HtmlNode>();

            List<HtmlNode> currentIteration = SelectDepthRange(node, depthLimit, depthStart, onlyLeafs);

            foreach (HtmlNode n in currentIteration)
            {
                if (evaluation(n))
                {
                    output.Add(n);
                }
            }

            return output;
        }

        public static List<HtmlNode> SelectDepthRange(this HtmlNode node, Int32 depthLimit, Int32 depthStart = 1, Boolean onlyLeafs = true)
        {
            List<HtmlNode> output = null;

            if (output == null) output = new List<HtmlNode>();

            List<HtmlNode> currentIteration = new List<HtmlNode>();
            List<HtmlNode> nextIteration = new List<HtmlNode>();

            currentIteration.Add(node);
            Int32 depthIndex = 0;

            while (currentIteration.Any())
            {
                nextIteration = new List<HtmlNode>();

                for (int i = 0; i < currentIteration.Count; i++)
                {
                    var n = currentIteration[i];

                    if (depthIndex >= depthLimit)
                    {
                        break;
                    }
                    else if (depthIndex >= depthStart)
                    {
                        if (onlyLeafs)
                        {
                            if (!n.HasChildNodes)
                            {
                                output.Add(n);
                            }
                            else
                            {
                                nextIteration.AddRange(n.ChildNodes);
                            }
                        }
                        else
                        {
                            output.Add(n);
                            nextIteration.AddRange(n.ChildNodes);
                        }
                    }
                    else
                    {
                        nextIteration.AddRange(n.ChildNodes);
                    }
                }

                currentIteration = nextIteration;

                depthIndex++;
            }

            return output;
        }

        public static List<HtmlNode> SelectByTagName(this HtmlNode node, String tagname, Int32 depthLimit)
        {
            List<HtmlNode> output = null;

            if (output == null) output = new List<HtmlNode>();

            List<HtmlNode> currentIteration = new List<HtmlNode>();
            List<HtmlNode> nextIteration = new List<HtmlNode>();

            currentIteration.Add(node);
            Int32 depthIndex = 0;

            while (currentIteration.Any())
            {
                nextIteration = new List<HtmlNode>();

                for (int i = 0; i < currentIteration.Count; i++)
                {
                    if (currentIteration[i].Name.Equals(tagname, StringComparison.InvariantCultureIgnoreCase))
                    {
                        output.Add(currentIteration[i]);
                    }
                    else
                    {
                        if (currentIteration[i].HasChildNodes)
                        {
                            nextIteration.AddRange(currentIteration[i].ChildNodes);
                        }
                    }
                }
                if (depthIndex < depthLimit)
                {
                    currentIteration = nextIteration;
                }
                else
                {
                    currentIteration = new List<HtmlNode>();
                }

                depthIndex++;
            }

            return output;
        }

        public static String GetTextContent(this HtmlNode node)
        {
            var allTextNodes = SelectTextLeafNodes(node);
            StringBuilder sb = new StringBuilder();
            foreach (var n in allTextNodes)
            {
                sb.AppendLine(n.InnerText);
            }
            return sb.ToString();
        }

        public static Boolean OnlyTextChildNodes(this HtmlNode node)
        {
            if (node.DescendantNodes().Any(x => x.NodeType != HtmlNodeType.Text)) return false;
            return node.HasChildNodes;
        }

        public static List<HtmlNode> SelectTextLeafNodes(this HtmlNode node)
        {
            var allDescendant = node.DescendantNodesAndSelf().Where(x => x.NodeType == HtmlNodeType.Text).ToList();
            return allDescendant;
            
        }

        public static List<HtmlNode> SelectChildrenOnDepth(this HtmlNode node, String xpath, Int32 depthLimit, List<HtmlNode> output = null, Int32 depthIndex = 0)
        {
            if (output == null) output = new List<HtmlNode>();

            var selected = node.SelectNodes(xpath);

            if (selected != null)
            {
                foreach (var n in selected)
                {
                    if (n.HasChildNodes)
                    {
                        if (depthIndex < depthLimit)
                        {
                            n.SelectChildrenOnDepth(xpath, depthLimit, output, depthIndex + 1);
                        }
                        else
                        {
                            if (!output.Contains(n)) output.Add(n);
                        }
                    }
                    else
                    {
                        if (!output.Contains(n)) output.Add(n);
                    }
                }
            }

            return output;
        }
    }
}