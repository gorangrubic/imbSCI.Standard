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

    public class RecordTemplateChunkDetector : ContentChunkDetectorBase
    {

        public Int32 MinJunctionSize { get; set; } = 3;
        public Int32 MinPeakNodeLevel { get; set; } = 3;

        public override ChunkContentCandidateCollection GetCandidates(ChunkDetectionResult results)
        {
            ChunkContentCandidateCollection output = new ChunkContentCandidateCollection();

            var inputGraph = results.InitialGraph; //.CloneByItems();

            List<NodeGraph> peaks = new List<NodeGraph>();
            ListDictionary<String, NodeGraph> dynamicNodes = new ListDictionary<string, NodeGraph>();
            
            var dynamicNodeList = inputGraph.GetChildrenWithItemSet().Where(x => x.item.Category.HasFlag(NodeInTemplateRole.Dynamic)).ToList();
            NodeGraph dynamicNode = dynamicNodeList.FirstOrDefault();
            Int32 i = 0;
            Int32 i_limit = 5000;
            while (dynamicNode != null)
            {
                var nodePeakSearch = new JunctionPeakSearch(dynamicNode);
                NodeGraph peakNode = nodePeakSearch.GetJunctionPeak(Convert.ToDouble(MinJunctionSize));

                dynamicNodes[dynamicNode.path].Add(dynamicNode);

                if (dynamicNodes[dynamicNode.path].Count > 1)
                {

                }

                if (peakNode == null)
                {
                    //dynamicNode.removeFromParent();
                } else
                {

                   
                    if (peaks.Contains(peakNode))
                    {

                    }
                    else
                    {
                        peaks.Add(peakNode);

                        if (peakNode.level < MinPeakNodeLevel)
                        {
                      //      dynamicNode.removeFromParent();
                        }
                        else
                        {
                            //var peakNodeAtSource = results.InitialGraph.GetChildAtPath(peakNode.path, "/", false);

                            if (peakNode.Count() != 0)
                            {
                                ChunkContentCandidate newCandidate = new ChunkContentCandidate(this, peakNode);

                                //    peakNode.removeFromParent();
                                output.Add(newCandidate);
                            } else
                            {

                            }
                        }
                    }
                }
                i++;


                dynamicNode = dynamicNodeList.FirstOrDefault(x => dynamicNodes[x.path].Count == 0);  // inputGraph.GetChildrenWithItemSet().FirstOrDefault(x => (x.item.Category.HasFlag(NodeInTemplateRole.Dynamic) && dynamicNodes[x.path].Count == 0));
               
                if (i > i_limit)
                {
                    break;
                }
            }
            if (peaks.Count > 0)
            {

            }

            results.Candidates.AddRange(output);
            return output;


        }

        public override string GetExtractorName()
        {
            return nameof(RecordTemplateExtractor);
        }

        public override bool SetContentChunk(ContentChunk chunk, ChunkContentCandidate candidate, ChunkDetectionResult result)
        {
            
            NodeGraphTemplateDetection templateDetection = new NodeGraphTemplateDetection(candidate.Node);
            
            chunk.multiNodePolicy = TaskMultiNodePolicy.AsSeparatedTables;
            chunk.type = ContentChunkType.DynamicDataExtraction;

            
            if (templateDetection.IsValid())
            {
                RecordTemplateSet templateSet = templateDetection.GetTemplateSet();
                chunk.ExtractorCustomizationSettings.AddObjectEntry(nameof(RecordTemplateExtractor.TemplateSet), templateSet);

                return true;
            }
            else
            {
                chunk.description = "Refused for failed template detection";

                return false;
            }

        }
    }
}