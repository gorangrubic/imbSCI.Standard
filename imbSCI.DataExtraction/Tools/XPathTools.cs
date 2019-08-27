using HtmlAgilityPack;
using imbSCI.Core.extensions;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.functions;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.style.color;
using imbSCI.Data;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions.data;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.Graph.Converters;
using imbSCI.Graph.Converters.tools;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using imbSCI.Graph.DGML.enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Tools
{
    public static class XPathTools
    {


        


        /// <summary>
        /// Regex select XPathNodeName : ([a-zA-Z]+)
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_XPathNodeName = new Regex(@"([a-zA-Z]+)", RegexOptions.Compiled);

        public static Regex _select_XPathNodeNameLast = new Regex(@"([a-zA-Z]+)", RegexOptions.Compiled);

         public static String GetLastNodeNameFromXPath(this String xpath)
        {
            MatchCollection m = _select_XPathNodeName.Matches(xpath);
            String output = "";
            foreach (Match mc in m)
            {
                output = mc.Value;

            }
            return output;
            //if (m.Success) { 
                
            //} else
            //{
            //    return xpath;
            //}
        }


        /// <summary>
        /// Gets the first parent meeting specified criteria
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="skipSelf">if set to <c>true</c> [skip self].</param>
        /// <param name="i_limit">The i limit.</param>
        /// <returns>null if no parent matched</returns>
        public static HtmlNode GetFirstParent(this HtmlNode node, Func<HtmlNode, Boolean> criteria, Boolean skipSelf = true, Int32 i_limit = 500)
        {
            HtmlNode head = node;

            if (skipSelf) head = head.ParentNode;

            Int32 i = 0;
            while (head != null)
            {
                if (criteria(head)) return head;
                head = head.ParentNode;
                i++;
                if (i > i_limit) return head;
            }

            return null;

        }


        /// <summary>
        /// Gets the first branching node, starting from root to leafs. Returns the first branching node (with name other then specified in <c>NodeNameToSkip</c> or leaf (if reached) 
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="NodeNameToSkip">The node name to skip.</param>
        /// <returns></returns>
        public static graphWrapNode<LeafNodeDictionaryEntry> GetFirstBranchingNode(this graphWrapNode<LeafNodeDictionaryEntry> graph,List<String> NodeNameToSkip=null) {

            if (NodeNameToSkip == null) NodeNameToSkip = new List<String>(){ "html", "body" };

            graphWrapNode<LeafNodeDictionaryEntry> head = graph;
            Boolean traverse = true;
            while (traverse) 
            {
                String firstChildName = head.getChildNames().FirstOrDefault();
                if (firstChildName.isNullOrEmpty())
                {
                    return head;
                }

                head = head[firstChildName] as graphWrapNode<LeafNodeDictionaryEntry>;
                String node_name = head.name.GetFirstNodeNameFromXPath();

                if (NodeNameToSkip.Contains(node_name))
                {

                } else if (head.GetChildren().Count > 1)
                {
                    traverse = false;
                }
            }

            return head;
        }


        public static String GetFirstNodeNameFromXPath(this String xpath)
        {
            Match m = _select_XPathNodeName.Match(xpath);
            if (m.Success) { 
                return m.Groups[1].Value;
            } else
            {
                return xpath;
            }
        }

        public static HtmlNode GetParentWithAnyID(this HtmlNode node, Boolean skipMe = true, Int32 depthLimit=30)
        {
            HtmlNode head = node;
            if (skipMe)
            {
                head = node.ParentNode;
            }

            if (head == null)
            {
                return null;
            }
            Int32 i = 0;
            String idAttribute = head.GetAttributeValue("id", "");
            while (idAttribute.isNullOrEmpty())
            {
                head = node.ParentNode;
                if (head == null)
                {
                    return null;
                }
                idAttribute = head.GetAttributeValue("id", "");
                i++;
                if (i> depthLimit)
                {
                    break;
                }
            }

            if (idAttribute.isNullOrEmpty()) return null;

            return head;
        }

        public static List<HtmlNode> selectNodes(this HtmlNode node, String path)
        {
            List<HtmlNode> output = new List<HtmlNode>();
            if (path.IsNullOrEmpty())
            {
                output.Add(node);
            }
            else
            {
                var sn = node.SelectNodes(path);
                if (sn != null)
                {
                    output.AddRange(sn);
                }
                
            }
            return output;
        }

        public static HtmlNode selectSingleNode(this HtmlNode node, String path)
        {
            if (path.IsNullOrEmpty())
            {
                return node;
            } else
            {
                return node.SelectSingleNode(path);
            }
        }

        public static String GetAbsoluteXPath(this String Path, String XPathRoot)
        {
            String output = Path;
            if (!output.StartsWith(XPathRoot))
            {
                output= output.add(XPathRoot, "/");
            }
            return output;
        }

        public static String GetRelativeXPath(this String Path, String XPathRoot, Boolean viceVersa=false)
        {
            String output = Path;
            if (output.StartsWith(XPathRoot))
            {
                output = output.removeStartsWith(XPathRoot);
                
            } else if (viceVersa && XPathRoot.StartsWith(output))
            {
                output = XPathRoot.removeStartsWith(output);
            }
            output = output.TrimStart("/".ToCharArray());
            return output;
        }
    }
}