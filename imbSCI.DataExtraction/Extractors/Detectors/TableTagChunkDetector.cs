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

    public class TableTagChunkDetector: HtmlTagBasedChunkDetectorBase
    {
        public override string GetExtractorName()
        {
            return nameof(TableTagExtractor);
        }

        public override string GetTagName()
        {
            return "table";
        }

        public override Boolean SetContentChunk(ContentChunk chunk, ChunkContentCandidate candidate, ChunkDetectionResult result)
        {
            chunk.multiNodePolicy = TaskMultiNodePolicy.AsSeparatedTables;
            chunk.type = ContentChunkType.DynamicDataExtraction;

            return true;
        }

        //public override List<ContentChunk> Process(LeafNodeDictionaryAnalysis ContentNodeAnalysis, ContentChunkCollection targetOutput=null, StructureGraphInformationSet structureChangeLog=null)
        //{
            

        //    List<ContentChunk> output = new List<ContentChunk>();

        //    StructureGraphInformation change = null;

        //    var tableChildren = ContentNodeAnalysis.CompleteGraph.getAllChildren(new System.Text.RegularExpressions.Regex("table\\[[\\d]+\\]\\Z"));
        //    var nodeByPath = tableChildren.ToLookup(x => x.path);

        //    var tableNode = tableChildren.FirstOrDefault() as graphWrapNode<LeafNodeDictionaryEntry>;

        //    while (tableNode != null)
        //    {
                
        //        ContentChunk chunk = ContentNodeAnalysis.TakeChunk(tableNode.path);

        //        if (!ContentNodeAnalysis.DeployChunk(chunk))
        //        {
        //            chunk.type = ContentChunkType.Failed;
        //        }
        //        else
        //        {
        //            chunk.multiNodePolicy = TaskMultiNodePolicy.AsSeparatedTables;
        //            chunk.type = ContentChunkType.DynamicDataExtraction;
        //            chunk.ExtractorName = nameof(TableTagExtractor);

        //            if (!output.Any(x => x.XPathRoot == chunk.XPathRoot))
        //            {
        //                output.Add(chunk);
        //                if (targetOutput != null) targetOutput.Add(chunk);
        //            }

        //        }

              

        //      //  if (structureChangeLog != null) change = structureChangeLog.Add("AfterTable_" + chunk.name, ContentNodeAnalysis.RebuildCompleteGraph());

        //        tableChildren = ContentNodeAnalysis.CompleteGraph.getAllChildren(new System.Text.RegularExpressions.Regex("table\\[[\\d]+\\]\\Z"));
        //        tableNode = tableChildren.FirstOrDefault() as graphWrapNode<LeafNodeDictionaryEntry>;
        //    }

        //    String paths = "";
        //    output.ForEach(x => paths = paths.add(x.XPathRoot, Environment.NewLine));

        //    //if (tableChildren.Any())
        //    //{
        //    //    tableChildren = tableChildren.OrderByDescending(x => x.level).ToList();

        //    //    foreach (graphWrapNode<LeafNodeDictionaryEntry> tableNode in tableChildren)
        //    //    {
                    
        //    //    }
        //    //}

        //    return output;
        //}


    }
}