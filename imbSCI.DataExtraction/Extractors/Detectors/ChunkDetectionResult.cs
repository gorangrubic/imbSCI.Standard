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
using imbSCI.Graph.DGML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Extractors.Detectors
{
    [Serializable]
    public class ChunkDetectionResult
    {
        public ChunkDetectionResult(NodeGraph initialGraph, ChunkDetectorCollection detectors)
        {
            InitialGraph = initialGraph;
            CurrentGraph = InitialGraph.CloneByItems();
            Detectors = detectors;
        }

        public void Report(folderNode folder)
        {
            builderForText reporter = new builderForText();

            Detectors.Report(reporter);
            Candidates.Report(reporter, nameof(Candidates));

            AcceptedCandidates.Report(reporter, nameof(AcceptedCandidates));

            DeclinedCandidates.Report(reporter, nameof(DeclinedCandidates));

            DeclinedByDetectorCandidates.Report(reporter, nameof(DeclinedByDetectorCandidates));

            NodeDictionaryGraphStyleSettings style = new NodeDictionaryGraphStyleSettings();


            var initialGraph = InitialGraph.BuildDirectedGraph(style);
            initialGraph.Save(folder.pathFor("InitialGraph.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "Initial content graph of chunk detection"));

            var finalGraph = CurrentGraph.BuildDirectedGraph(style);
            finalGraph.Save(folder.pathFor("FinalGraph.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "Final content graph of chunk detection"));

            CurrentGraphStates.Save(folder, "StateGraph");

            foreach (var chunk in DetectedChunks)
            {
                chunk.PublishAnnotation(initialGraph, "#f7941d", style);
            }

            initialGraph.Save(folder.pathFor("InitialGraphWithAnnotation.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "Initial content graph of chunk detection with chunks annoted"));
            folder.SaveText(reporter.GetContent(), "report.txt");
        }

        public ChunkDetectorCollection Detectors { get; set; }

        public NodeGraph InitialGraph { get; set; }

        public NodeGraph CurrentGraph { get; set; }

        public ChunkContentCandidateCollection Candidates { get; set; } = new ChunkContentCandidateCollection();

        public ChunkContentCandidateCollection AcceptedCandidates { get; set; } = new ChunkContentCandidateCollection();

        public ChunkContentCandidateCollection DeclinedCandidates { get; set; } = new ChunkContentCandidateCollection();

        public ChunkContentCandidateCollection DeclinedByDetectorCandidates { get; set; } = new ChunkContentCandidateCollection();

        public DirectedGraphSequence<DirectedGraphWithSourceData> CurrentGraphStates { get; set; } = new DirectedGraphSequence<DirectedGraphWithSourceData>();

        public List<ContentChunk> DetectedChunks { get; set; } = new List<ContentChunk>();

        
    }
}