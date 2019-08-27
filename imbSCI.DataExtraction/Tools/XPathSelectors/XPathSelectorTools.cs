using HtmlAgilityPack;
using imbSCI.Data;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Tools.XPathSelectors
{
    /// <summary>
    /// 
    /// </summary>
    public static class XPathSelectorTools
    {


        /// <summary>
        /// Finds the selector.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="targetCount">The target count.</param>
        /// <param name="i_max">The i maximum.</param>
        /// <returns></returns>
        public static XPathSelector FindSelector(this HtmlNode node, Int32 targetCount = 1, Int32 i_max=50)
        {
            XPathSelector output = new XPathSelector();
            output.name = node.Name;

            if (node.OnlyTextChildNodes())
            {
                String text = node.GetInnerText();
                if (!text.isNullOrEmpty())
                {
                    XPathSelectorItem textItem = new XPathSelectorItem()
                    {
                        type = XPathSelectorItemType.attribute,
                        name = "text()",
                        value = text
                    };
                    output.items.Add(textItem);
                }
            }

            foreach (var attribute in node.Attributes)
            {
                String attName = attribute.Name;
                String attValue = attribute.Value;

                XPathSelectorItem textItem = new XPathSelectorItem()
                {
                    type = XPathSelectorItemType.attribute,
                    name = "@" + attName,
                    value = attValue
                };
                output.items.Add(textItem);
            }

            var nodes = node.OwnerDocument.DocumentNode.selectNodes(output.ToString());

            Int32 c = nodes.Count;

            HtmlNode head = node.ParentNode;
            Int32 index = 0;

            while (c != targetCount)
            {
                head = head.ParentNode;

                if (head == null) break;

                if (head.NodeType != HtmlNodeType.Element) break;
                    
                if (head != null)
                {
                    XPathSelectorItem parentItem = new XPathSelectorItem()
                    {
                        type = XPathSelectorItemType.parent,
                        name = head.Name,
                        priority = output.items.Count(x=>x.type==XPathSelectorItemType.parent)
                    };
                    output.items.Add(parentItem);
                }

                nodes = node.OwnerDocument.DocumentNode.selectNodes(output.ToString());
                c = nodes.Count;
                index++;
                if (index > i_max)
                {
                    break;
                }
            }

            return output;
            
        }
    }
}