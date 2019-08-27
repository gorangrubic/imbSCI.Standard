using HtmlAgilityPack;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.table;
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
using imbSCI.Graph.Converters;
using imbSCI.Graph.Data;
using imbSCI.Graph.DGML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    [Serializable]
    public class LeafNodeDictionaryAnalysis : LeafNodeDictionaryAnalysis<LeafNodeDictionary>
    {
        
    }

    [Serializable]
    public class LeafNodeDictionaryAnalysis<T> where T:NodeDictionary, new()
    {

        public DirectedGraphWithSourceData Publish(folderNode folder, String name)
        {
           // StaticContent.Publish(folder, name + "_static");
           // DynamicContent.Publish(folder, name + "_dynamic");
            var dgml = Nodes.Publish(folder, name + "_all");

            frequencyCounter<NodeInTemplateRole> roleCounter = new frequencyCounter<NodeInTemplateRole>();

            foreach (var item in Nodes.items)
            {
                var roles = item.Category.getEnumListFromFlags<NodeInTemplateRole>();
                foreach (NodeInTemplateRole role in roles)
                {
                    roleCounter.Count(role);
                }
            }

            var bins = roleCounter.GetFrequencyBins();
            String rolePath = folder.pathFor("allContent_roleStats.txt", imbSCI.Data.enums.getWritableFileMode.overwrite);
            StringBuilder sb = new StringBuilder();
            foreach (var bin in bins)
            {
                sb.AppendLine(bin.Key + " " + bin.Value.toCsvInLine());
            }
            File.WriteAllText(rolePath, sb.ToString());

            return dgml;

        }

        /// <summary>
        /// Compares given HTML nodes.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="leafSelectionXPath">The leaf selection x path.</param>
        /// <returns></returns>
        public static TAnalysis CompareHtmlNodes<TAnalysis>(IEnumerable<HtmlNode> nodes, NodeDictionaryAnalysisSettings settings) where TAnalysis : LeafNodeDictionaryAnalysis<T>, new()
        {


            TAnalysis output = new TAnalysis();


            foreach (HtmlNode node in nodes)
            {

                var dict = new LeafNodeDictionary(node, settings.leafSelectionXPath.or(LeafNodeDictionary.DefaultNodeSelectionXPath),
                    settings.TagsToIgnore.or(LeafNodeDictionary.DefaultTagsToIgnore), settings.XPathTagsToRemove.or(LeafNodeDictionary.DefaultXPathMatchToRemove));

                foreach (var entry in dict.items)
                {
                    output.XPathFrequencyCounter.Count(entry.XPath);
                    Boolean WasKnown = output.CountXPathAndHash(entry.XPath, entry.ContentHash);
                    if (!WasKnown)
                    {
                        output.Nodes.Add(entry.node);
                    }
                }
            }

            var freqRange = output.XPathFrequencyCounter.GetRange();

            foreach (LeafNodeDictionaryEntry entry in output.Nodes.items)
            {
                NodeInTemplateRole role = NodeInTemplateRole.Undefined;

                entry.Presence = output.XPathFrequencyCounter.GetFrequencyForItem(entry.XPath).GetRatio(freqRange.Maximum);
                if (entry.Presence == 1)
                {
                    role = NodeInTemplateRole.Permanent;
                }
                else
                {
                    role = NodeInTemplateRole.Optional;
                }

                entry.Staticness = 1.GetRatio(output.DifferenceByXPath[entry.XPath]);
                if (entry.Staticness == 1)
                {
                    role |= NodeInTemplateRole.Static;
                }
                else
                {
                    role |= NodeInTemplateRole.Dynamic;
                }

                entry.Category = role;

                //if (role.HasFlag(NodeInTemplateRole.Static)) output.StaticContent.Add(entry);
                //if (role.HasFlag(NodeInTemplateRole.Dynamic)) output.DynamicContent.Add(entry);

            }

            output.RebuildCompleteGraph();

            return output;
        }

        /*
        private static Object _graphDataConverter_lock = new Object();
        private static graphDataToDirectedGraphConverter _graphDataConverter;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static graphDataToDirectedGraphConverter graphDataConverter
        {
            get
            {
                if (_graphDataConverter == null)
                {
                    lock (_graphDataConverter_lock)
                    {

                        if (_graphDataConverter == null)
                        {
                            _graphDataConverter = new graphDataToDirectedGraphConverter();
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _graphDataConverter;
            }
        }*/


        //   protected graphData CompleteGraphData { get; set; }

        public StructureGraphInformation RebuildCompleteGraph()
        {
           
            //, getPath, "/");

            Nodes.RebuildIndex();
            //StaticContent.RebuildIndex();
            //DynamicContent.RebuildIndex();

            CompleteGraph = NodeGraph.Build(Nodes.items);


            return CompleteGraph.ConstructionInfo;
        }

        /// <summary>
        /// Score is computed as: XPath frequency / top frequency
        /// </summary>
        public Dictionary<String, Double> PresenceScoreByXPath = new Dictionary<string, double>();

        public frequencyCounter<String> XPathFrequencyCounter = new frequencyCounter<string>();

        /// <summary>
        /// Returns TRUE if XPath was already known
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        public Boolean CountXPathAndHash(String xpath, String hash)
        {
            if (!ContentHashByXPath.ContainsKey(xpath))
            {
                ContentHashByXPath.Add(xpath, hash);
                DifferenceByXPath.Add(xpath, 0);
                return false;
            }
            else
            {
                if (ContentHashByXPath[xpath] != hash) {
                    DifferenceByXPath[xpath]++;
                }
                return true;
            }

        }

        public Dictionary<String, String> ContentHashByXPath = new Dictionary<String, String>();

        /// <summary>
        /// How many times same XPath had different hash, in the collection
        /// </summary>
        public Dictionary<String, Int32> DifferenceByXPath = new Dictionary<String, Int32>();

        public NodeGraph CompleteGraph { get; set; }

        public T Nodes { get; set; } = new T();



        

     
        /*
        public Boolean DeployChunk(ContentChunk output)
        {

            output.taken_graph = output.ContentAnalysis.RebuildCompleteGraph();

            foreach (var entry in output.ContentAnalysis.allContent.items)
            {
                var rootNode = entry.node.OwnerDocument.DocumentNode.SelectSingleNode(output.XPathRoot);
                
                if (rootNode != null)
                {
                    output.RootNode = rootNode;
                    output.ID = rootNode.GetAttributeValue("id", "");
                    output.Class = rootNode.GetAttributeValue("class", "");
                    output.NodeName = rootNode.Name;

                    var headingNode = rootNode.SelectHeading(3, 0);
                    //if (headingNode != null)
                    //{
                    //    output.name = headingNode.GetInnerText();
                    //}
                    //output.name = output.name.Replace(" ", "").ToLower().getCleanPropertyName().trimToLimit(10, true, "");

                    break;
                }
            }

            return output.taken_graph.InputCount > 0;
        }

        /// <summary>
        /// Extracts chunk of content
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <returns></returns>
        public ContentChunk TakeChunk(String xpath, ContentChunk output=null)
        {
        //    Func<LeafNodeDictionaryEntry, string> getPath = x =>
        //    {
        //        if (x == null)
        //        {
        //            return "/null";
        //        }
        //        if (x.XPath.isNullOrEmpty()) return "/null";
        //        return x.XPath;
        //    };
            Boolean CallDeploy = false;

            xpath = xpath.ensureStartsWith("/");

            if (output == null)
            {
                output = new ContentChunk();

               

                output.XPathRoot = xpath;
                CallDeploy = true;
            }

            //output.ContentAnalysis.allContent.items.AddRange(allContent.GetAndRemoveByXPathRoot(xpath));
            //output.ContentAnalysis.StaticContent.items.AddRange(StaticContent.GetAndRemoveByXPathRoot(xpath));
            //output.ContentAnalysis.DynamicContent.items.AddRange(DynamicContent.GetAndRemoveByXPathRoot(xpath));


            RebuildCompleteGraph();

            //if (CallDeploy)
            //{
            //    DeployChunk(output);
            //}

            

            
            return output;
        }*/


    }
}