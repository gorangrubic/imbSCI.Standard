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

namespace imbSCI.DataExtraction.Extractors.Detectors
{
    public abstract class ContentChunkDetectorBase : IContentChunkDetector
    {
        public abstract String GetExtractorName();

        public abstract Boolean SetContentChunk(ContentChunk chunk, ChunkContentCandidate candidate, ChunkDetectionResult result);

        public abstract ChunkContentCandidateCollection GetCandidates(ChunkDetectionResult results);

      //  public abstract List<ContentChunk> Process(LeafNodeDictionaryAnalysis ContentNodeAnalysis, ContentChunkCollection targetOutput = null, StructureGraphInformationSet structureChangeLog = null);
    }
}

