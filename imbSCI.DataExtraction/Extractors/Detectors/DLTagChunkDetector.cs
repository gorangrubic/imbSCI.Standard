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
    public class DLTagChunkDetector : HtmlTagBasedChunkDetectorBase
    {
        public override string GetTagName()
        {
            return "dl";
        }

        public override Boolean SetContentChunk(ContentChunk chunk, ChunkContentCandidate candidate, ChunkDetectionResult result)
        {
            chunk.multiNodePolicy = TaskMultiNodePolicy.AsSeparatedTables;
            chunk.type = ContentChunkType.DynamicDataExtraction;

            return true;
        }

        //public override List<ContentChunk> Process(LeafNodeDictionaryAnalysis ContentNodeAnalysis, ContentChunkCollection targetOutput = null, StructureGraphInformationSet structureChangeLog = null)
        //{
        //    List<ContentChunk> output = new List<ContentChunk>();

        //    StructureGraphInformation change = null;

        //    var dlChildren = ContentNodeAnalysis.CompleteGraph.getAllChildren(new System.Text.RegularExpressions.Regex("dl\\[[\\d]+\\]\\Z"));
        //    if (dlChildren.Any())
        //    {
        //        dlChildren = dlChildren.OrderByDescending(x => x.level).ToList();



        //        foreach (graphWrapNode<LeafNodeDictionaryEntry> dlNode in dlChildren)
        //        {
        //            var xpath = dlNode.path;
        //            ContentChunk chunk = ContentNodeAnalysis.TakeChunk(xpath);
        //            if (!ContentNodeAnalysis.DeployChunk(chunk))
        //            {
        //                chunk.type = ContentChunkType.Failed;
        //            }
        //            else
        //            {
        //                chunk.multiNodePolicy = TaskMultiNodePolicy.AsSeparatedTables;
        //                chunk.type = ContentChunkType.DynamicDataExtraction;
        //                chunk.ExtractorName = nameof(DLTagExtractor);


        //                if (!output.Any(x => x.XPathRoot == chunk.XPathRoot))
        //                {
        //                    output.Add(chunk);
        //                    if (targetOutput != null) targetOutput.Add(chunk);
        //                }

        //            }



        //            if (structureChangeLog!=null) change = structureChangeLog.Add("AfterDL_" + chunk.name, ContentNodeAnalysis.RebuildCompleteGraph());
        //        }
        //    }

        //    String paths = "";
        //    output.ForEach(x => paths = paths.add(x.XPathRoot, Environment.NewLine));

        //    return output;

        //}

        public override string GetExtractorName()
        {
            return nameof(DLTagExtractor);
        }
    }
}