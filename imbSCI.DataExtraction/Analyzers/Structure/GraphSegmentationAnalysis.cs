using imbSCI.Core.math.range.frequency;
using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using HtmlAgilityPack;
using System.Linq;
using imbSCI.Core.extensions.data;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Structure
{

    [Serializable]
    public class GraphSegmentationAnalysis
    {

        
        public Int32 JunctionSizeMin { get; set; } = 4;

        public JunctionGraphMetrics<graphWrapNode<LeafNodeDictionaryEntry>> GraphMetrics = new JunctionGraphMetrics<graphWrapNode<LeafNodeDictionaryEntry>>();

        public graphWrapNode<LeafNodeDictionaryEntry> CompleteGraph;

        public void Analyze(LeafNodeDictionary leafDictionary)
        {
            CompleteGraph = graphTools.BuildGraphFromItems<LeafNodeDictionaryEntry, graphWrapNode<LeafNodeDictionaryEntry>>(leafDictionary.items,x=>x.XPath, "/");
            CompleteGraph.pathSeparator = "/";

            GraphMetrics.Process(CompleteGraph);

            var bins = GraphMetrics.JunctionCounter.GetFrequencyBins();

            foreach (var bin in bins)
            {
                if (bin.Key >= JunctionSizeMin)
                {
                    
                }
            }

        }

    }
}