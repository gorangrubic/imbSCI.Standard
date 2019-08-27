using HtmlAgilityPack;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.frequency;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Analyzers.Structure;
using imbSCI.DataExtraction.Analyzers.Templates;
using imbSCI.DataExtraction.Tools;
using imbSCI.Graph.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Extractors.Detectors
{

    public class JunctionRecordTemplateChunkDetector : ContentChunkDetectorBase
    {
        public override ChunkContentCandidateCollection GetCandidates(ChunkDetectionResult results)
        {
            JunctionGraphMetrics<NodeGraph> junctionGraphMetrics = new JunctionGraphMetrics<NodeGraph>();

            junctionGraphMetrics.Process(results.CurrentGraph);

            var junctionBlocks = junctionGraphMetrics.GetJunctionBlocks(1, 4, true, true);

            List<JunctionPoint<NodeGraph>> sorted = junctionBlocks.OrderByDescending(x => x.JunctionSize).ToList();
            sorted = sorted.OrderByDescending(x => x.XPathRoot.getPathParts().Count).ToList();

            

            ChunkContentCandidateCollection output = new ChunkContentCandidateCollection();

            foreach (var junctionPoint in sorted)
            {
                NodeGraph childNode = results.CurrentGraph.GetChildAtPath(junctionPoint.XPathRoot, "/", false);
                ChunkContentCandidate newCandidate = new ChunkContentCandidate(this, childNode);
                newCandidate.MetaData = junctionPoint;
                output.Add(newCandidate);
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
            JunctionPoint<NodeGraph> junctionPoint = candidate.MetaData as JunctionPoint<NodeGraph>;
            NodeGraphTemplateDetection templateDetection = new NodeGraphTemplateDetection(junctionPoint.rootItem);
            if (templateDetection.IsValid())
            {
                RecordTemplateSet templateSet = templateDetection.GetTemplateSet();
                chunk.ExtractorCustomizationSettings.AddObjectEntry(nameof(RecordTemplateExtractor.TemplateSet), templateSet);

                return true;
            } else
            {
                return false;
            }

        }
        /*
        public List<ContentChunk> Process(LeafNodeDictionaryAnalysis ContentNodeAnalysis, ContentChunkCollection targetOutput = null, StructureGraphInformationSet structureChangeLog = null)
        {
            JunctionGraphMetrics<graphWrapNode<LeafNodeDictionaryEntry>> junctionGraphMetrics = new JunctionGraphMetrics<graphWrapNode<LeafNodeDictionaryEntry>>();

            junctionGraphMetrics.Process(ContentNodeAnalysis.CompleteGraph);

            var junctionBlocks = junctionGraphMetrics.GetJunctionBlocks(1, 4, true, true);  //.GetJunctionBlocks(DataNodeAnalysis.JunctionSizeMin, 0, DataNodeAnalysis.MinLevel);

            List<ContentChunk> output = new List<ContentChunk>();

            StructureGraphInformation change = null;

            List<JunctionPoint<graphWrapNode<LeafNodeDictionaryEntry>>> sorted = junctionBlocks.OrderByDescending(x => x.JunctionSizeFrequency).ToList();
            sorted = sorted.OrderByDescending(x => x.XPathRoot.getPathParts().Count).ToList();

            sorted = sorted.Where(x => x.type == JunctionPointType.BranchToLeafs).ToList();



            foreach (var pair in sorted)
            {
                ContentChunk chunk = null; // new ContentChunk();
                
                //  List<LeafNodeDictionaryEntry> selectedByItems = pair.items.OrderByDescending(x => x.level).ToList();

                List<LeafNodeDictionaryEntry> selectedByItems = new List<LeafNodeDictionaryEntry>();

               // chunk = ContentNodeAnalysis.TakeChunk(pair.XPathRoot);

             

                chunk.ExtractorName = nameof(RecordTemplateExtractor);

               

                

                //if (!ContentNodeAnalysis.DeployChunk(chunk))
                //{
                //    chunk.type = ContentChunkType.Failed;
                //}
                //else
                //{



                    //// Setting category for template items -----------------

                    List<String> RootXPathList = new List<string>();
                    Dictionary<RecordTemplateItem, frequencyCounter<String>> XPathCounterForItem = new Dictionary<RecordTemplateItem, frequencyCounter<string>>();
                    Dictionary<RecordTemplateItem, frequencyCounter<String>> ContentHashCounterForItem = new Dictionary<RecordTemplateItem, frequencyCounter<string>>();
                    foreach (var item in pair.Template.items)
                    {
                        XPathCounterForItem.Add(item, new frequencyCounter<string>());
                        ContentHashCounterForItem.Add(item, new frequencyCounter<string>());
                    }

                    String xq = pair.Template.BuildXPathQuery();
                    Int32 mc = 0;
                    foreach (HtmlNode document in ContentNodeAnalysis.Nodes)
                    {
                        HtmlNodeCollection matched = document.SelectNodes(xq);

                        if (matched == null) continue;
                        mc++;

                        String commonPath = matched.Select(x => x.XPath).GetCommonPathRoot();

                        List<HtmlNode> selected_nodes = new List<HtmlNode>();
                        foreach (HtmlNode node in matched)
                        {
                            Dictionary<RecordTemplateItem, HtmlNode> cells = pair.Template.SelectCells(node);
                            foreach (var cellPair in cells)
                            {
                                XPathCounterForItem[cellPair.Key].Count(cellPair.Value.XPath);
                                var content_hash = md5.GetMd5Hash(cellPair.Value.GetInnerText());
                                ContentHashCounterForItem[cellPair.Key].Count(content_hash);
                            }


                            selected_nodes.Add(node);
                            selected_nodes.AddRange(cells.Values.ToList());
                        }
                        if (!commonPath.isNullOrEmpty())
                        {
                            RootXPathList.Add(commonPath);
                        }
                    }

                    if (mc == ContentNodeAnalysis.Nodes.Count)
                    {
                        pair.Template.Category = NodeInTemplateRole.Permanent;
                    }
                    else
                    {
                        pair.Template.Category = NodeInTemplateRole.Optional;
                    }

                    frequencyCounter<String> RootXPathListFreq = new frequencyCounter<string>();
                    RootXPathListFreq.Count(RootXPathList);

                    String TopRootXPath = RootXPathListFreq.GetItemsWithTopFrequency().FirstOrDefault();
                    graphWrapNode<LeafNodeDictionaryEntry> page_rootNode = null;
                    if (TopRootXPath.isNullOrEmpty())
                    {

                        page_rootNode = ContentNodeAnalysis.allContent.ContentGraph.GetFirstBranchingNode();

                       
                    } else
                    {
                        ContentNodeAnalysis.RebuildCompleteGraph();
                        page_rootNode = ContentNodeAnalysis.CompleteGraph.GetChildAtPath(TopRootXPath, "/", true);
                        var first_branching = page_rootNode.GetFirstParent(x => (x.Count() > 1), true, false);
                        if (first_branching != null)
                        {
                            page_rootNode = first_branching; 
                        }

                    }

                    chunk.XPathRoot = page_rootNode.path;


                    //if (chunk.XPathRoot.isNullOrEmpty())
                    //{
                    //    ContentNodeAnalysis.StaticContent.GetAndRemoveByXPathRoot()
                    //}

                    Int32 maxFreq = int.MinValue;


                    Dictionary<RecordTemplateItem, Int32> frequencyByItem = new Dictionary<RecordTemplateItem, int>();
                    foreach (var item in pair.Template.items)
                    {
                        Int32 f = XPathCounterForItem[item].GetTopFrequency();
                        frequencyByItem.Add(item, f);
                        if (f > maxFreq)
                        {
                            maxFreq = f;
                        }
                    }

                    foreach (var item in pair.Template.items)
                    {
                        if (frequencyByItem[item] < maxFreq)
                        {
                            item.Category = NodeInTemplateRole.Optional;
                        }
                        else
                        {
                            item.Category = NodeInTemplateRole.Permanent;
                        }
                        if (ContentHashCounterForItem[item].DistinctCount() == 1)
                        {
                            item.Category |= NodeInTemplateRole.Static;
                        }
                        else
                        {
                            item.Category |= NodeInTemplateRole.Dynamic;
                        }
                    }

                    //// Setting category for template items -----------------
                    ///
                    if (pair.Template.items.Count(x => x.Category.HasFlag(NodeInTemplateRole.Dynamic)) > 0)
                    {
                        chunk.type = ContentChunkType.DynamicDataExtraction;
                    }
                    else
                    {
                        chunk.type = ContentChunkType.TemplateExtraction;
                    }

                   // chunk.ExtractorCustomizationSettings.AddObjectEntry(nameof(RecordTemplateExtractor.Template), pair.Template);

                    if (!output.Any(x => x.XPathRoot == chunk.XPathRoot))
                    {
                        output.Add(chunk);
                        if (targetOutput != null) targetOutput.Add(chunk);
                    }
                }

               // if (structureChangeLog != null) change = structureChangeLog.Add("AfterChunk" + chunk.name, ContentNodeAnalysis.RebuildCompleteGraph());

           // }

            String paths = "";
            output.ForEach(x => paths = paths.add(x.XPathRoot, Environment.NewLine));

            return output;
        }
        */
        
    }

}