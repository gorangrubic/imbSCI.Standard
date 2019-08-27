using imbSCI.Core.math.range.frequency;
using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.Analyzers;
using HtmlAgilityPack;
using System.Linq;
using imbSCI.Core.extensions.data;
using System.Xml.Serialization;
using imbSCI.Core.collection;
using imbSCI.DataExtraction.Extractors.Detectors;
using imbSCI.Core.math.range.finder;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.Tools;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
    public static class RecordTemplateTools
    {

        public static Int32 CompareStaticFirst(RecordTemplateItem itemA, RecordTemplateItem itemB)
        {
            if (itemA.Category.HasFlag(NodeInTemplateRole.Dynamic) && itemB.Category.HasFlag(NodeInTemplateRole.Static)) return -1;
            if (itemA.Category.HasFlag(NodeInTemplateRole.Static) && itemB.Category.HasFlag(NodeInTemplateRole.Dynamic)) return 1;
            if (itemA.Category.HasFlag(NodeInTemplateRole.Dynamic)) return -1;
            if (itemA.Category.HasFlag(NodeInTemplateRole.Static)) return 1;
            return 0;
        }
    }

    [Serializable]
    public class RecordTemplate:RecordTemplateItem {

        public Int32 RecordSelectionLimit { get; set; } = -1;

        public RecordTemplateRole Role { get; set; } = RecordTemplateRole.RowDataProvider;

        public String BuildXPathQuery()
        {
            // //div[child::div[1] and child::div[2] and child::div[3] and child::div[4] and child::div[5] and child::div[6]]"
            StringBuilder sb = new StringBuilder();

            if (RecordSelectionLimit == 1)
            {
                sb.Append("./");
                sb.Append(SubXPath);
                return sb.ToString();
            }

            sb.Append(".//");
            
            sb.Append(SubXPath.GetFirstNodeNameFromXPath());

            sb.Append("[");

            if (items.Any())
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var s = items[i];
                    sb.Append("child::" + s.SubXPath);

                    if (i < items.Count-1)
                    {
                        sb.Append(" and ");
                    }
                }
            }

            sb.Append("]");
            return sb.ToString();

        }

        

        [XmlIgnore]
        public String Signature
        {
            get
            {
                return items.Select(x => x.SubXPath).toCsvInLine();
            }
        }

        public List<RecordTemplateItem> items { get; set; } = new List<RecordTemplateItem>();


        public List<HtmlNode> SelectAllNodes(HtmlNode document)
        {
            String xq = BuildXPathQuery();

            var matched = document.selectNodes(xq);
            List<HtmlNode> selected_nodes = new List<HtmlNode>();
            foreach (HtmlNode node in matched)
            {
                var cells = SelectCells(node).Values.ToList();
                selected_nodes.Add(node);
                selected_nodes.AddRange(cells);
            }
            return selected_nodes;
        }

        public RecordTemplateItemTakeCollection SelectTakes(HtmlNode recordNode)
        {
            RecordTemplateItemTakeCollection output = new RecordTemplateItemTakeCollection()
            {
                Template = this,
                RecordNode = recordNode
            };

            foreach (RecordTemplateItem item in items)
            {
                var itemNode = recordNode.SelectSingleNode(item.SubXPath);
                if (itemNode != null)
                {
                    output.AddCell(new RecordTemplateItemTake()
                    {
                        TemplateItem = item,
                        SelectedNode = itemNode,
                        SubXPath = item.SubXPath
                    });
                }
            }

            return output;

        }

        public Dictionary<RecordTemplateItem, HtmlNode> SelectCells(HtmlNode node)
        {
            Dictionary<RecordTemplateItem, HtmlNode> output = new Dictionary<RecordTemplateItem, HtmlNode>();

            foreach (RecordTemplateItem item in items)
            {
                var itemNode = node.SelectSingleNode(item.SubXPath);
                output.Add(item, itemNode);
            }

            return output;
        }

        public RecordTemplate()
        {

        }

    }
}