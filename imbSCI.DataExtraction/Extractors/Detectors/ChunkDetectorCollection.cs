using HtmlAgilityPack;
using imbSCI.Core.data.providers;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.reporting.render.builders;
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
    /// Collection of ChunkDetectors, used to detect content blocks with data to extract
    /// </summary>
    [Serializable]
    public class ChunkDetectorCollection
    {
        [NonSerialized]
        private List<IContentChunkDetector> _detectors = new List<IContentChunkDetector>();

        public List<String> DetectorNames { get; set; } = new List<string>();

        public void Check()
        {
            if (!DetectorNames.Any())
            {
                DetectorNames.Add(nameof(DLTagChunkDetector));
                DetectorNames.Add(nameof(TableTagChunkDetector));
                DetectorNames.Add(nameof(RecordTemplateChunkDetector));
            }
        }

        protected void DeployDetectors()
        {
            _detectors.Clear();
            foreach (String name in DetectorNames)
            {
                IContentChunkDetector detector = ChunkDetectorTools.ChunkDetectorProvider.GetInstance(name);
                if (detector != null)
                {
                    _detectors.Add(detector);
                }
            }
        }

        public void Report(builderForText reporter)
        {
            
            reporter.AppendLine("Detector names:");
            reporter.nextTabLevel();
            foreach (String detectorname in DetectorNames)
            {
                reporter.AppendLine(detectorname);
            }
            reporter.prevTabLevel();
            
        }
        

        public ChunkDetectionResult Run(NodeGraph InitialGraph)
        {
            Check();

            

            ChunkDetectionResult result = new ChunkDetectionResult(InitialGraph, this);

            
            foreach (IContentChunkDetector detector in Detectors)
            {
                detector.GetCandidates(result);
            }

            result.Candidates.ScoreAndSort();

            NodeDictionaryGraphStyleSettings style = new NodeDictionaryGraphStyleSettings();


            result.CurrentGraphStates.Add(result.CurrentGraph.BuildDirectedGraph(style));

            foreach (ChunkContentCandidate candidate in result.Candidates)
            {
                //if (candidate.Score == 0)
                //{
                //    result.DeclinedCandidates.Add(candidate);
                //    continue;
                //}
                var rootNode = result.CurrentGraph.GetChildAtPath<NodeGraph>(candidate.Node.path, "/", false);
                if (rootNode == null)
                {
                    result.DeclinedCandidates.Add(candidate);
                } else
                {
                    ContentChunk contentChunk = new ContentChunk();
                    contentChunk.ExtractorName = candidate.Detector.GetExtractorName();
                    contentChunk.DeployRootNode(rootNode);

                    var subGraph = rootNode.GetSubgraph(true);
                    contentChunk.SubGraph = subGraph;

                    if (candidate.Detector.SetContentChunk(contentChunk, candidate, result))
                    {
                        result.DetectedChunks.Add(contentChunk);
                        result.AcceptedCandidates.Add(candidate);
                    } else
                    {
                        result.DeclinedByDetectorCandidates.Add(candidate);
                    }

                    result.CurrentGraphStates.Add(result.CurrentGraph.BuildDirectedGraph(style));
                }
            }


            return result;
        }


        public ChunkDetectorCollection()
        {

        }

        /// <summary>
        /// Gets or sets the detectors.
        /// </summary>
        /// <value>
        /// The detectors.
        /// </value>
        [XmlIgnore]
        public List<IContentChunkDetector> Detectors
        {
            get {

                if (DetectorNames.Any())
                {
                    if (!_detectors.Any())
                    {
                        DeployDetectors();
                    }
                    else
                    {
                        if (_detectors.Count != DetectorNames.Count)
                        {
                            DeployDetectors();
                        }
                    }
                }
                return _detectors;
            } 
        }
    }
}