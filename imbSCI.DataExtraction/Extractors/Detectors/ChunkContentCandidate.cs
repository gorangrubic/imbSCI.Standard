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
    public class ChunkContentCandidate
    {
        public ChunkContentCandidate(IContentChunkDetector detector, NodeGraph node)
        {
            Detector = detector;
            Node = node;
        }

        

        public IContentChunkDetector Detector { get; set; }

        public NodeGraph Node { get; set; }

        public NodeGraphScoreModel ScoreModel { get; set; }

        public Double Score { get; set; } = 0;

        public Object MetaData { get; set; }

        public void Report(ITextRender output, Int32 index=Int32.MinValue)
        {
            if (index != Int32.MinValue)
            {
                output.AppendLine("[" + index.ToString("D3") + "]:[" + Score.ToString("F2") + "] " + Detector.GetType().Name + " : " + Node.path);
            } else
            {
                output.AppendLine("[" + Score.ToString("F2") + "] " + Detector.GetType().Name + " : " + Node.path);
            }
            
        }

    }
}