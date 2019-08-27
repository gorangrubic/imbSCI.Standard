using HtmlAgilityPack;
using imbSCI.Core.collection;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.frequency;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Analyzers.Structure;
using imbSCI.DataExtraction.Analyzers.Templates;
using imbSCI.Graph.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Extractors.Detectors
{
public class JunctionPeakSearch
    {

        public JunctionPeakSearch(NodeGraph node)
        {
            Deploy(node);
        }

        public void Deploy(NodeGraph node)
        {
            var head = node;

            while (head != null)
            {
                parentScores.Add(new ScoredContent<NodeGraph>()
                {
                    item = head,
                    score = head.Count()
                });

                head = head.parent as NodeGraph;
                
            }


        }

        public NodeGraph GetJunctionPeak(Double scoreMin=Double.MinValue)
        {
            Double maxScore = parentScores.Max(x => x.score);
            if (scoreMin != Double.MinValue)
            {
                if (maxScore < scoreMin)
                {
                    return null;
                }
            }
            var firstMax = parentScores.FirstOrDefault(x => x.score == maxScore);
            return firstMax.item;
        }

        public ScoredContentList<NodeGraph> parentScores { get; set; } = new ScoredContentList<NodeGraph>();
    }
}