using HtmlAgilityPack;
using imbSCI.Core.collection;
using imbSCI.Core.data.providers;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Analyzers.Structure;
using imbSCI.Graph.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Extractors.Detectors
{
[Serializable]
    public class NodeGraphScoreModel
    {
        public NodeGraphScoreModel() { }

        public NodeGraphScoreModel(NodeGraph rootNode)
        {
            var allNodesWithItem = rootNode.getAllChildrenInType<NodeGraph>().Where(x => x.item != null);
            foreach (var node in allNodesWithItem)
            {
                if (node.item.Category.HasFlag(NodeInTemplateRole.Dynamic))
                {
                    DynamicNodeScore++;
                }
            }

            var allNodes = rootNode.getAllChildrenInType<NodeGraph>();

            foreach (var node in allNodes)
            {
                String signature = node.GetChildSignature();
                ChildrenByChildSignature[signature].Add(node);
            }

            if (ChildrenByChildSignature.Any())
            {
                TemplateInstances = ChildrenByChildSignature.Max(x => x.Value.Count);
            } else
            {
                TemplateInstances = 0;
            }
        }

        public ListDictionary<String, NodeGraph> ChildrenByChildSignature = new ListDictionary<string, NodeGraph>();

        public Double GetScore()
        {
            return DynamicNodeScore * TemplateInstances;
        }

        public Double DynamicNodeScore { get; set; } = 0;
        public Double TemplateInstances { get; set; } = 0;

    }
}