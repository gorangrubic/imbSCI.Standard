using HtmlAgilityPack;
using imbSCI.Data;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;

using imbSCI.DataExtraction.NodeQuery.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Tools
{
    public static class HtmlDocumentTools
    {
        public static Dictionary<String, HtmlNode> GetXPathDictionary(this IEnumerable<HtmlNode> source)
        {
            Dictionary<String, HtmlNode> output = new Dictionary<string, HtmlNode>();
            foreach (HtmlNode n in source)
            {
                if (!output.ContainsKey(n.XPath))
                {
                    output.Add(n.XPath, n);
                }
            }
            return output;
        }

        public static void RemoveDictionary(this Dictionary<String, HtmlNode> target, IEnumerable<String> keys)
        {
            foreach (String k in keys)
            {
                if (target.ContainsKey(k)) target.Remove(k);
            }
        }

        public static void MergeDictionary(this Dictionary<String, HtmlNode> target, Dictionary<String, HtmlNode> source)
        {
            foreach (var pair in source)
            {
                if (!target.ContainsKey(pair.Key)) target.Add(pair.Key, pair.Value);
            }
        }

        public static Dictionary<String, HtmlNode> GetOperationResult(Dictionary<String, HtmlNode> target, Dictionary<String, HtmlNode> source, NodeQueryResultOperand operand)
        {
            Dictionary<String, HtmlNode> output = new Dictionary<string, HtmlNode>();

            List<String> overlapKeys = new List<string>();

            switch (operand)
            {
                case NodeQueryResultOperand.APPEND:
                    break;

                case NodeQueryResultOperand.REMOVE:
                    output.MergeDictionary(target);
                    break;

                case NodeQueryResultOperand.DIFFERENCE:
                case NodeQueryResultOperand.OVERLAP:

                    overlapKeys.AddRange(target.Keys.Where(x => source.ContainsKey(x)).ToList());
                    break;

                default:
                    break;
            }

            switch (operand)
            {
                case NodeQueryResultOperand.APPEND:
                    output.MergeDictionary(target);
                    output.MergeDictionary(source);
                    break;

                case NodeQueryResultOperand.DIFFERENCE:
                    foreach (String k in overlapKeys)
                    {
                        if (target.ContainsKey(k)) target.Remove(k);
                        if (source.ContainsKey(k)) source.Remove(k);
                    }

                    output.MergeDictionary(target);
                    output.MergeDictionary(source);

                    break;

                case NodeQueryResultOperand.OVERLAP:

                    foreach (String k in overlapKeys)
                    {
                        if (target.ContainsKey(k))
                        {
                            output.Add(k, target[k]);
                        }
                    }
                    break;

                case NodeQueryResultOperand.REMOVE:
                    output.RemoveDictionary(source.Keys);
                    break;

                default:
                    break;
            }

            return output;
        }

        public static List<HtmlNode> GetOperationResult(this List<HtmlNode> target, IEnumerable<HtmlNode> source, NodeQueryResultOperand operand)
        {
            if (operand == NodeQueryResultOperand.IGNORE)
            {
                return target;
            } else if (operand == NodeQueryResultOperand.SET)
            {
                return source.ToList();
            }
            var targetDict = target.GetXPathDictionary();
            var sourceDict = source.GetXPathDictionary();

            var outputDict = GetOperationResult(targetDict, sourceDict, operand);
            return outputDict.Values.ToList();
        }
    }
}