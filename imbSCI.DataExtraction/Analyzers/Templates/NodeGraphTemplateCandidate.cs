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
using imbSCI.Core.math;
using imbSCI.DataExtraction.Tools;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
    public class NodeGraphTemplateCandidate
    {

        public Boolean IsParentOf(NodeGraphTemplateCandidate other)
        {
            return ParenthoodScoreAgainst(other) > 0.5;
        }

        public Double ParenthoodScoreAgainst(NodeGraphTemplateCandidate other)
        {
            Int32 instanceParenthoods = 0;
            foreach (NodeGraph instance in Instances)
            {
                String path = instance.path;
                if (other.Instances.Any(x=>x.path.StartsWith(path)))
                {
                    instanceParenthoods++;
                }
            }
            Double parentHoodScore = Instances.Count().GetRatio(instanceParenthoods);
            return parentHoodScore;
        }

        public NodeGraph Root { get; set; }

        public List<NodeGraph> Instances { get; set; } = new List<NodeGraph>();

        public Dictionary<String, NodeGraph> LeafsByRelativePath { get; set; } = new Dictionary<string, NodeGraph>();


        public rangeFinder LeafLevelDistance { get; set; } = new rangeFinder();

        public RecordTemplate GetTemplate(NodeGraph ParentRoot, Boolean trimItems=true)
        {
            RecordTemplate template = new RecordTemplate();
            template.Category = Category;
            template.SubXPath = Root.path.removeStartsWith(ParentRoot.path);

            Int32 dynamicItems = 0;
            Int32 staticItems = 0;

            foreach (var lpair in LeafsByRelativePath)
            {
                RecordTemplateItem item = new RecordTemplateItem();
                if (lpair.Value.item != null)
                {
                    item.Category = lpair.Value.item.Category;
                }

                if (trimItems)
                {
                    var pathParts = lpair.Key.SplitSmart("/");

                    item.SubXPath = pathParts.First();
                } else
                {
                    item.SubXPath = lpair.Key;
                }

                if (item.Category.HasFlag(NodeInTemplateRole.Dynamic)) dynamicItems++;
                if (item.Category.HasFlag(NodeInTemplateRole.Static)) staticItems++;

                template.items.Add(item);
            }



            Boolean isDynamic = template.Category.HasFlag(NodeInTemplateRole.Dynamic);
            Boolean isStatic = template.Category.HasFlag(NodeInTemplateRole.Static);

            if (isDynamic && isStatic)
            {
                template.Role = RecordTemplateRole.RowDataAndContextProvider;

                if (staticItems == 1)
                {
                    template.items.Sort(RecordTemplateTools.CompareStaticFirst);
                    template.items.Reverse();
                }
            }
            else if (isDynamic)
            {
                template.Role = RecordTemplateRole.RowDataProvider;

            }
            else if (isStatic)
            {
                template.Role = RecordTemplateRole.GeneralContextProvider;
                template.RecordSelectionLimit = 1;
            }
            return template;
        }

        public String GetLeafSignature()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pair in LeafsByRelativePath)
            {
                var pathParts = pair.Key.SplitSmart("/", "", true, true);

                sb.Append("{" + pathParts.Count + "}");
                if (pair.Value.item != null)
                {

                    if (pair.Value.item.Category.HasFlag(NodeInTemplateRole.Dynamic))
                    {
                        sb.Append("{D}");
                    }
                    else if (pair.Value.item.Category.HasFlag(NodeInTemplateRole.Static))
                    {
                        sb.Append("{S}");
                    }
                }
                sb.Append(pair.Value.name);
            }
            return sb.ToString();
        }

        //public String GetLeafPathSignature()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var pair in LeafsByRelativePath)
        //    {
        //        sb.Append(pair.Key);
        //    }
        //    return sb.ToString();
        //}

        public String GetSignature()
        {
            String signature = Root.name.GetFirstNodeNameFromXPath() + GetLeafSignature();
            return signature;
        }

        public Double GetScore()
        {
            return LeafsByRelativePath.Count * Instances.Count;
        }

        public NodeInTemplateRole Category { get; set; } = NodeInTemplateRole.none;
        
        public NodeGraphTemplateCandidate(NodeGraph node)
        {
            Root = node;
            Instances.Add(node);
            var leafs = node.GetChildrenWithItemSet(false);
            
            foreach (NodeGraph leaf in leafs)
            {
                String relPath = leaf.path.removeStartsWith(Root.path);
                relPath = relPath.removeStartsWith("/");

                if (LeafsByRelativePath.ContainsKey(relPath)) continue;

                LeafsByRelativePath.Add(relPath, leaf);

                LeafLevelDistance.Learn(leaf.level - Root.level);

                if (leaf.item != null)
                {
                    if (Category == NodeInTemplateRole.none)
                    {
                        Category = leaf.item.Category;
                    } else
                    {
                        Category |= leaf.item.Category;
                    }
                }
            }

        }

    }
}