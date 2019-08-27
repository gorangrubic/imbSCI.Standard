using HtmlAgilityPack;
using imbSCI.Core.collection;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.style.color;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.NodeQuery;
using imbSCI.DataExtraction.Tools;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    [Serializable]
    public class ContentChunkCollection
    {
        public ContentChunkCollection()
        {

        }


        public void Publish(folderNode folder, DirectedGraphWithSourceData InitialGraph)
        {
            ColorGradientForInstanceEnumeration<String> chunkInstanceGradient = new ColorGradientForInstanceEnumeration<String>("#f7941d", "#6dcff6");
            chunkInstanceGradient.Prepare(items.Select(x => x.ExtractorName));
            if (InitialGraph == null) return;

            NodeGraph chunkRootGraph = BuildChunkRootsGraph();

          

            var chunkRootGraphNodes = InitialGraph.Nodes.Get(chunkRootGraph.getAllChildren().Select(x=>x.path)).ToDictionary(x => x.Id);
            foreach (var gnp in chunkRootGraphNodes)
            {
                if (gnp.Value is Node)
                {
                    gnp.Value.Background = "#999999";
                    gnp.Value.StrokeDashArray = "2,5,2,5,2,5";
                    gnp.Value.StrokeThinkness = 2;
                }
            }

            List<NodeGraph> TargetRootNodes = chunkRootGraph.getAllChildrenInType<NodeGraph>().Where(x => x.HasMetaData()).ToList();

            ListDictionary<ContentChunk, DirectedGraph> subgraphs = new ListDictionary<ContentChunk, DirectedGraph>();

            NodeDictionaryGraphStyleSettings style = new NodeDictionaryGraphStyleSettings();

            foreach (NodeGraph ng in TargetRootNodes)
            {
                var TargetRootNodesInContentGraph = InitialGraph.Select<NodeGraph>( new List<NodeGraph>() { ng }, x => x.path, true, true);
                ContentChunk chunk = ng.GetMetaData<ContentChunk>();
                var nodeColor = chunkInstanceGradient.GetColor(chunk.ExtractorName, true);
                

                foreach (var pair in TargetRootNodesInContentGraph)
                {
                    if (pair.Key is Node node)
                    {
                        var allLinked = chunk.PublishAnnotation(InitialGraph, nodeColor, style);
                        
                        var sg = InitialGraph.GetSubgraph(allLinked.SelectMany(x=>x));
                        sg.Title = chunk.name;
                        
                        subgraphs[chunk].Add(sg);
                    

                    } else if (pair.Key is Link link)
                    {
                       // link.Stroke = nodeColor;
                    }
                }
            }

            foreach (var pair in subgraphs)
            {
                Int32 grpi = 0;
                foreach (var grp in pair.Value)
                {
                    String grpp = "chunk_subgraph" + pair.Key.name + grpi.ToString() + ".dgml";
                    
                    grp.Save(folder.pathFor(grpp, imbSCI.Data.enums.getWritableFileMode.overwrite), imbSCI.Data.enums.getWritableFileMode.overwrite);
                    grpi++;
                }
                
            }

            InitialGraph.Save(folder.pathFor("CompleteGraph.dgml"), imbSCI.Data.enums.getWritableFileMode.overwrite);

            /*
            var chunkTargetRootNodes = InitialGraph.Nodes.Get(.Select(x => x.path)).ToDictionary(x => x.Id);
            foreach (var gnp in chunkTargetRootNodes)
            {

                if (gnp.Value is Node)
                {
                    gnp.Value.Background = chunkInstanceGradient.GetColor(chunk.ExtractorName, true);
                    gnp.Value.StrokeDashArray = "";
                    gnp.Value.StrokeThinkness = 5;
                }
            }



            foreach (ContentChunk chunk in items)
            {
                var graphNodes = InitialGraph.Nodes.Get(chunk.ContentAnalysis.allContent.items.Select(x => x.XPath));

                var nodeColor = chunkInstanceGradient.GetColor(chunk.ExtractorName, true);

                var itemsByXPath = chunk.ContentAnalysis.allContent.items.ToDictionary(x => x.XPath);

                var graphNodeByXPath = chunk.ContentAnalysis.allContent.ContentGraph.getAllChildren().ToDictionary(x => x.path);

                foreach (var gn in graphNodes)
                {
                    gn.Background = nodeColor;

                }

                var SelectedGraphNodes = InitialGraph.Nodes.Get(itemsByXPath.Keys).ToDictionary(x => x.Id);

                foreach (var gnp in SelectedGraphNodes)
                {
                    gnp.Value.Background = nodeColor;

                }

                
            }

    */

            foreach (ContentChunk chunk in items)
            {
                String chunkStrictName = chunk.name.getCleanPropertyName();

                var subfolder = folder.Add(chunkStrictName, chunk.name, "Cluster group diagnostics");

                chunk.Publish(subfolder);



            }
        }


        public void SetTitleAndDescriptions(IEnumerable<HtmlNode> sourceDocuments)
        {
            NodeGraph chunkGraph = BuildChunkRootsGraph();
            Dictionary<ContentChunk, NodeGraph> dictionary = new Dictionary<ContentChunk, NodeGraph>();


            ScoredContentDictionary<HtmlNode, ContentChunk> titleScoreDictionary = new ScoredContentDictionary<HtmlNode, ContentChunk>(x => x.GetInnerLetterText("_"));
            ScoredContentDictionary<HtmlNode, ContentChunk> descriptionScoreDictionary = new ScoredContentDictionary<HtmlNode, ContentChunk>(x => x.GetInnerLetterText("_"));

            //ListDictionary<string, ContentChunk> titleProposals = new ListDictionary<string, ContentChunk>();
            //ListDictionary<string, ScoredText> titleTextProposals = new ListDictionary<string, ScoredText>();

            //ListDictionary<string, ContentChunk> descProposals = new ListDictionary<string, ContentChunk>();
            //ListDictionary<string, ScoredText> descTextProposals = new ListDictionary<string, ScoredText>();


            //ListDictionary<ContentChunk, ScoredText> chunkScoreList = new ListDictionary<ContentChunk, ScoredText>();
            //ListDictionary<ContentChunk, ScoredText> chunkDescScoreList = new ListDictionary<ContentChunk, ScoredText>();

            ///  ListDictionary<ContentChunk, ScoredText> titleTextProposals = new ListDictionary<string, ScoredText>();

            var leafs = chunkGraph.getAllLeafs();
            foreach (NodeGraph leaf in leafs)
            {
                NodeGraph chunkDomain = leaf.GetFirstParent(x => x.Count() > 1, true, true);
                ContentChunk chunk = items.FirstOrDefault(x => x.XPathRoot == leaf.path);
                if (chunk == null)
                {
                    chunk = items.FirstOrDefault(x => x.RootNode == leaf.item.node);
                    if (chunk != null)
                    {
                        
                    }
                }

                if (chunk == null)
                {
                    continue;
                }
                String domainXPath = "";
                if (chunkDomain != null)
                {
                    domainXPath = chunkDomain.path;
                }

                if (domainXPath.isNullOrEmpty())
                {
                    domainXPath = chunk.XPathRoot;
                }
                foreach (HtmlNode sourceDocument in sourceDocuments)
                {
                    var sourceSelected = sourceDocument.selectSingleNode(domainXPath);

                    if (sourceSelected != null)
                    {
                        

                        var candidates = sourceSelected.SelectNodeCandidates<ScoredContent<HtmlNode>>(ExtractorTools.headingTagPriorityList);

                        var descriptionCandidates = sourceSelected.SelectNodeCandidates<ScoredContent<HtmlNode>>(ExtractorTools.descriptionTagPriorityList);

                        titleScoreDictionary.Merge(candidates, chunk);
                        descriptionScoreDictionary.Merge(descriptionCandidates, chunk);
                    }
                }

            }

            StringBuilder sb = new StringBuilder();

            foreach (var chunk in items)
            {
                chunk.title = titleScoreDictionary.GetUIDBlendForTopLocalScores(chunk, 5, 50);
                chunk.description = descriptionScoreDictionary.GetUIDBlendForTopLocalScores(chunk, 10, 350, Environment.NewLine);

                sb.AppendLine($"{chunk.ExtractorName} : {chunk.title} \t\t {chunk.description}");
            }

            String debug = sb.ToString();
           
        }

        /// <summary>
        /// Builds graph with chunk root nodes
        /// </summary>
        /// <returns></returns>
        public NodeGraph BuildChunkRootsGraph()
        {
            List<LeafNodeDictionaryEntry> rootEntries = new List<LeafNodeDictionaryEntry>();
            Dictionary<LeafNodeDictionaryEntry, ContentChunk> EntryChunkDictionary = new Dictionary<LeafNodeDictionaryEntry, ContentChunk>();

            foreach (var chunk in items)
            {
                LeafNodeDictionaryEntry entry = null;
                if (chunk.RootNode != null)
                {
                    entry = new LeafNodeDictionaryEntry(chunk.RootNode);
                } else
                {
                    entry = new LeafNodeDictionaryEntry()
                    {
                        XPath = chunk.XPathRoot
                    };
                }

                EntryChunkDictionary.Add(entry, chunk);
                rootEntries.Add(entry);
            }

            NodeGraph graph = NodeGraph.Build(rootEntries);

            foreach (var pair in EntryChunkDictionary)
            {
                NodeGraph chunkChild = graph.GetChildAtPath<NodeGraph>(pair.Key.XPath);
                chunkChild.SetMetaData(pair.Value);
            }
            
            return graph;
        }


        public List<String> classList { get; set; } = new List<string>();
        public List<String> idList { get; set; } = new List<string>();

        public void AddaptSelectors()
        {
            foreach (ContentChunk chunk in items)
            {
                if (chunk.Class.isNullOrEmpty()) classList.Add(chunk.Class);
                if (chunk.ID.isNullOrEmpty()) idList.Add(chunk.ID);
            }

            foreach (ContentChunk chunk in items)
            {
                if (!chunk.Class.isNullOrEmpty())
                {
                    Int32 sameClass = classList.Count(x => chunk.Class == x);
                    if (sameClass > 1)
                    {
                        chunk.Class = "";
                    }
                }
                if (!chunk.ID.isNullOrEmpty())
                {
                     Int32 sameID = idList.Count(x => chunk.ID == x);
                    if (sameID > 1)
                    {
                        chunk.ID = "";
                    }
                }
               
            }

        }

          public void Merge(List<ContentChunk> chunks)
        {
            foreach (var c in chunks)
            {
                Add(c);
            }
        }


        public void Merge(ContentChunkCollection chunks)
        {
            foreach (var c in chunks.items)
            {
                Add(c);
            }
        }


        public void Add(ContentChunk chunk)
        {
            

            if (chunk.name.isNullOrEmpty())
            {
               chunk.name = chunk.ExtractorName;
            }

             if (chunk.name.isNullOrEmpty())
            {
                chunk.name = "Chunk";
            }

            Int32 c = 0;
            String proposal = chunk.name;
            while (items.Any(x=>x.name == proposal))
            {
                c++;
                proposal = chunk.name + c.ToString();

            }
            chunk.name = proposal;
            items.Add(chunk);
        }

        public List<ContentChunk> items { get; set; } = new List<ContentChunk>();
    }
}