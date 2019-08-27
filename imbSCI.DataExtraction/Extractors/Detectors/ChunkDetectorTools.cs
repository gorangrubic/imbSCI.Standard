using HtmlAgilityPack;
using imbSCI.Core.data.providers;
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
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Extractors.Detectors
{
    /// <summary>
    /// Static helper class 
    /// </summary>
    public static class ChunkDetectorTools
    {
        public static Int32 Compare(ChunkContentCandidate CandidateA, ChunkContentCandidate CandidateB)
        {
            if (CandidateA.ScoreModel == null && CandidateB.ScoreModel == null) return 0;

            if (CandidateB.ScoreModel == null) return 1;
            if (CandidateA.ScoreModel == null) return -1;

            var pathPartsA = CandidateA.Node.path.SplitSmart("/");
            var pathPartsB = CandidateB.Node.path.SplitSmart("/");

            if (pathPartsA.Count == pathPartsB.Count)
            {
                var dynamicScoreComparison = CandidateA.ScoreModel.DynamicNodeScore.CompareTo(CandidateB.ScoreModel.DynamicNodeScore);
                if (dynamicScoreComparison == 0)
                {
                    return CandidateA.ScoreModel.TemplateInstances.CompareTo(CandidateB.ScoreModel.TemplateInstances);
                }
                else
                {
                    return dynamicScoreComparison;
                }
            } else
            {
                if (pathPartsA.Count > pathPartsB.Count)
                {
                    return 1;
                } else
                {
                    return -1;
                }
            }

           

        }

        public static String GetChildSignature(this NodeGraph node)
        {
            var leafs = node.getAllLeafs();

            StringBuilder sb = new StringBuilder();
            foreach (NodeGraph leaf in leafs)
            {
                sb.Append(leaf.name);
            }
            return sb.ToString();
        }

        private static Object _ChunkDetectorProvider_lock = new Object();
        private static UniversalTypeProvider<IContentChunkDetector> _ChunkDetectorProvider;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static UniversalTypeProvider<IContentChunkDetector> ChunkDetectorProvider
        {
            get
            {
                if (_ChunkDetectorProvider == null)
                {
                    lock (_ChunkDetectorProvider_lock)
                    {

                        if (_ChunkDetectorProvider == null)
                        {
                            _ChunkDetectorProvider = new UniversalTypeProvider<IContentChunkDetector>();
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _ChunkDetectorProvider;
            }
        }



    }
}