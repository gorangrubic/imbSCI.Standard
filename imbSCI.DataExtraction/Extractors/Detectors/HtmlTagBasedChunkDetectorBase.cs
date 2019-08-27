using HtmlAgilityPack;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.frequency;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Analyzers.Structure;
using imbSCI.Graph.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Extractors.Detectors
{
    public abstract class HtmlTagBasedChunkDetectorBase : ContentChunkDetectorBase
    {
        private Regex _htmlPathSelectExpression = null;

        public abstract String GetTagName();

        public Regex HtmlPathSelectExpression
        {
            get {
                if (_htmlPathSelectExpression == null)
                {
                    _htmlPathSelectExpression = new System.Text.RegularExpressions.Regex(GetTagName() + "\\[[\\d]+\\]\\Z");
                }
                return _htmlPathSelectExpression;
            }
            
        }

        public override ChunkContentCandidateCollection GetCandidates(ChunkDetectionResult results)
        {
            ChunkContentCandidateCollection output = new ChunkContentCandidateCollection();
            
            var tableChildren = results.CurrentGraph.getAllChildren(HtmlPathSelectExpression);
            foreach (NodeGraph tableChild in tableChildren)
            {
                ChunkContentCandidate newCandidate = new ChunkContentCandidate(this, tableChild);
                output.Add(newCandidate);
            }

            results.Candidates.AddRange(output);
            return output;
        }
    }
}