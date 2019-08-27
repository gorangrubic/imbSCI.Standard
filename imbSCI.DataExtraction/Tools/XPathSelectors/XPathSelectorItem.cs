using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Tools.XPathSelectors
{
    public class XPathSelectorItem
    {
        public XPathSelectorItem() { }

        public String name { get; set; } = "";

        public String value { get; set; } = "";

        public List<XPathSelectorItem> items { get; set; } = new List<XPathSelectorItem>();


        public XPathSelectorItemType type { get; set; } = XPathSelectorItemType.attribute;

        public Int32 priority { get; set; } = 0;


        //public override string ToString()
        //{
        //    String inner = RenderAttributeSelector();

        //    String target = name;

        //    if (!inner.isNullOrEmpty())
        //    {
        //        target = target + $"[{inner}]";
        //    }
        //}


        /// <summary>
        /// Renders square bracket part of XPath query, like: [@href='' and @target='_blank']
        /// </summary>
        /// <returns></returns>
        public String RenderAttributeSelector()
        {
            var attributes = items.Where(x => x.type == XPathSelectorItemType.attribute).ToList();
            attributes = attributes.OrderByDescending(x => x.priority).ToList();

            String inner = "";
            foreach (var a in attributes)
            {
                String render = a.RenderSelf();
                inner = inner.add(render, " and ");
            }

            return inner;
        }

        public String RenderSelf()
        {
            switch (type)
            {
                default:
                case XPathSelectorItemType.parent:
                case XPathSelectorItemType.target:
                    
                    String inner = RenderAttributeSelector();

                    String target = name;

                    if (!inner.isNullOrEmpty())
                    {
                        target = target + $"[{inner}]";
                    }
                    
                    return target;
                    break;
                case XPathSelectorItemType.attribute:
                    return $"{name} = '{value}'";
                    break;
            }
        }

    }
}