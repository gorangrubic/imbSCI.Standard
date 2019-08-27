using HtmlAgilityPack;
using imbSCI.Core.math;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions;
using imbSCI.Data;
using imbSCI.Graph.Converters;
using imbSCI.Core.extensions.data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using imbSCI.Core.files.folders;
using imbSCI.Data.collection.graph;
using imbSCI.Core.math.range.frequency;
using System.IO;
using imbSCI.Core.style.color;
using System.Drawing;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.Tools;

namespace imbSCI.DataExtraction.Analyzers.Data
{


    [Serializable]
    public class NodeDictionary
    {
        // /// <summary>
        ///// Builds graph from items in dictionary
        ///// </summary>
        ///// <returns></returns>
        //public graphWrapNode<LeafNodeDictionaryEntry> BuildGraph()
        //{
        //    Func<LeafNodeDictionaryEntry, string> getPath = x =>
        //  {
        //      if (x == null)
        //      {
        //          return "/null";
        //      }
        //      if (x.XPath.isNullOrEmpty()) return "/null";
        //      return x.XPath;
        //  };


        //    var CompleteGraph = graphTools.BuildGraphFromItems<LeafNodeDictionaryEntry, graphWrapNode<LeafNodeDictionaryEntry>>(items, getPath, "/");
        //    //CompleteGraph.pathSeparator = "/";
        //    return CompleteGraph;
        //}


        private static Object _TagSelectorRegex_lock = new Object();
        private static Regex _TagSelectorRegex;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static Regex TagSelectorRegex
        {
            get
            {
                if (_TagSelectorRegex == null)
                {
                    lock (_TagSelectorRegex_lock)
                    {

                        if (_TagSelectorRegex == null)
                        {
                            _TagSelectorRegex = new Regex("([a-zA-Z]+)");
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _TagSelectorRegex;
            }
        }
        [XmlIgnore]
        public frequencyCounter<String> TagCounter { get; set; } = new frequencyCounter<string>();
        [XmlIgnore]
        public frequencyCounter<String> NodeTagCounter { get; set; } = new frequencyCounter<string>();


        public DirectedGraphWithSourceData Publish(folderNode folder, String name)
        {
            TagCounter = new frequencyCounter<string>();
            NodeTagCounter = new frequencyCounter<string>();
            RebuildIndex();
            

            String listPath = folder.pathFor("nd_" + name + "_list.txt", imbSCI.Data.enums.getWritableFileMode.overwrite, "List of selected nodes");
            String statPath = folder.pathFor("nd_" + name + "_stats.txt", imbSCI.Data.enums.getWritableFileMode.overwrite, "Stats of selected nodes");
            String graphPath = folder.pathFor("nd_" + name + "_graph.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "Structure graph");
            String graphStatsPath = folder.pathFor("nd_" + name + "_graph_stats.txt", imbSCI.Data.enums.getWritableFileMode.overwrite, "Stats of the complete graph");

            StringBuilder listBuilder = new StringBuilder();
            StringBuilder statBuilder = new StringBuilder();
            StringBuilder graphStatBuilder = new StringBuilder();

            foreach (var item in items)
            {
                listBuilder.AppendLine(item.XPath);
                TagCounter.Count(item.node.Name);
            }
         

            var freqBins = TagCounter.GetFrequencyBins();
            foreach (var bin in freqBins)
            {
                statBuilder.AppendLine(bin.Key + " " + bin.Value.toCsvInLine());
            }

        

            var GraphFreqBins = NodeTagCounter.GetFrequencyBins();
            foreach (var bin in GraphFreqBins)
            {
                graphStatBuilder.AppendLine(bin.Key + " " + bin.Value.toCsvInLine());
            }

            DirectedGraphWithSourceData dgml = BuildDGML();

            dgml.Save(graphPath, imbSCI.Data.enums.getWritableFileMode.overwrite);

            File.WriteAllText(listPath, listBuilder.ToString());
            File.WriteAllText(statPath, statBuilder.ToString());
            File.WriteAllText(graphStatsPath, graphStatBuilder.ToString());

            return dgml;
        }

        public void RemoveEntriesByContentHash(List<String> ContentHashList)
        {
            foreach (var item in items.ToList()) {
                if (ContentHashList.Contains(item.ContentHash))
                {
                    items.Remove(item);
                }
            }

            RebuildIndex();
        }

        public void RemoveEntriesByXPath(List<String> XPathList)
        {
            foreach (String XPath in XPathList)
            {
                if (CachedIndex.ContainsKey(XPath))
                {
                    items.Remove(CachedIndex[XPath]);
                    CachedIndex.Remove(XPath);
                }
            }

        }

        public List<LeafNodeDictionaryEntry> GetAndRemoveByTagNames(List<String> tags, Boolean DoRemove=true, Boolean Inverse = false)
        {
            List<String> ToRemove = new List<string>();
            List<LeafNodeDictionaryEntry> output = new List<LeafNodeDictionaryEntry>();
            List<Regex> regexList = new List<Regex>();
            //foreach (String tag in tags)
            //{
            //    Regex r = new Regex(tag + "\\[[\\d]+\\]\\Z");
            //    regexList.Add(r);
            //}

            foreach (var item in items.ToList()) {
                String nodeName = XPathTools.GetLastNodeNameFromXPath(item.XPath).ToLower();

                if (tags.Contains(nodeName) == !Inverse)
                {
                    ToRemove.Add(item.XPath);
                    output.Add(item);
                }
                //Boolean isMatch = false;
                //foreach (var r in regexList)
                //{

                //    if (r.IsMatch(item.XPath))
                //    {
                //        isMatch = true;
                        
                //        break;
                //    }
                //}
                //if (isMatch == !Inverse)
                //{
                    
                //}
            }
            if (DoRemove)
            {
                RemoveEntriesByXPath(ToRemove);
                RebuildIndex();
            }

            return output;
        }

         public List<LeafNodeDictionaryEntry> GetAndRemoveByXPathContains(List<String> XPathToRemove, Boolean DoRemove=true, Boolean Inverse = false)
        {
            List<LeafNodeDictionaryEntry> output = new List<LeafNodeDictionaryEntry>();
            List<String> ToRemove = new List<string>();
            foreach (var item in items.ToList()) {
            
                if (item.XPath.ContainsAny(XPathToRemove) == !Inverse)
                {
                    ToRemove.Add(item.XPath);
                    output.Add(item);
                }
            }

            if (DoRemove)
            {
                RemoveEntriesByXPath(ToRemove);
                RebuildIndex();
            }
            return output;
        }



        public List<LeafNodeDictionaryEntry> GetAndRemoveByXPathRoot(String XPathRoot, Boolean DoRemove=true, Boolean Inverse=false)
        {
            XPathRoot = XPathRoot.ensureStartsWith("/");
                
            List<LeafNodeDictionaryEntry> output = new List<LeafNodeDictionaryEntry>();
            List<String> ToRemove = new List<string>();
            foreach (var item in items.ToList()) {
            
                if (item.XPath.StartsWith(XPathRoot) == !Inverse)
                {
                    ToRemove.Add(item.XPath);
                    output.Add(item);
                }
            }

            if (DoRemove)
            {
                RemoveEntriesByXPath(ToRemove);
                RebuildIndex();
            }
            
            return output;
        }

        [XmlIgnore]
        public NodeGraph ContentGraph { get; set; } // graph

        internal void RebuildIndex()
        {
            CachedIndex.Clear();
            foreach (var item in items) {
                CachedIndex.Add(item.XPath, item);
            };

            ContentGraph = NodeGraph.Build(items); // BuildGraph();
            if (ContentGraph != null)
            {
                var allChildren = ContentGraph.getAllChildren();

                foreach (graphWrapNode<LeafNodeDictionaryEntry> child in allChildren)
                {
                    if (!child.name.isNullOrEmpty())
                    {
                        String tag = child.name.Trim(ContentGraph.pathSeparator.ToCharArray());
                        var m = TagSelectorRegex.Match(tag);
                        if (m.Success)
                        {
                            tag = m.Groups[m.Groups.Count - 1].Value;
                        }
                        NodeTagCounter.Count(tag);
                    }
                }
            }

            foreach (var item in items)
            {
                TagCounter.Count(item.node.Name);
            }
        }

        public DirectedGraphWithSourceData BuildDGML(NodeDictionaryGraphStyleSettings style=null)
        {
            if (style == null) style = new NodeDictionaryGraphStyleSettings();
            DirectedGraphWithSourceData output = new DirectedGraphWithSourceData();

            RebuildIndex();

            output = ContentGraph.BuildDirectedGraph(style);

            /*
            List<NodeGraph> allChildren = ContentGraph.getAllChildrenInType<NodeGraph>();

            output.Populate(allChildren, x => x.GetChildren<NodeGraph>(), x => x.path, x => x.name, false, false);


           // output.Populate(allChildren, x => x.item., x => x.path, x => x.name, false, false);

            foreach (var pair in output.Sources)
            {
                if (pair.Key is Link graphLink)
                {
                    style.SetElement(graphLink, pair.Value.LastOrDefault() as NodeGraph);


                } else if (pair.Key is Node graphNode)
                {
                    style.SetElement(graphNode, pair.Value.LastOrDefault() as NodeGraph);
                }
            }

            var nodesWithItem = allChildren.Where(x => x.item != null).ToList();

            var contentNodes = output.Populate<NodeGraph, HtmlNode>(nodesWithItem, x => x.item.node.SelectTextLeafNodes(), x => x.path, x => x.name, x => x.XPath, x => x.GetInnerText(), false, false);

            foreach (var pairs in contentNodes)
            {
                if (pairs.Key is Node)
                {
                    pairs.Key.StrokeThinkness = 0;
                    pairs.Key.StrokeDashArray = "3,3,3,3,3";
                    pairs.Key.Stroke = "#999999";
                }
            }

            //foreach (NodeGraph graph in allChildren)
            //{
            //    if (graph.item != null)
            //    {
                   

                    
            //        }

            //    if (graph.Count() == 0)
            //    {
                    
            //    }
            //}
            */
            return output;
        }
        
        public LeafNodeDictionaryEntry GetEntry(String xpath)
        {
            if (CachedIndex.ContainsKey(xpath))
            {
                return CachedIndex[xpath];
            }

            var entry = items.FirstOrDefault(x => x.XPath == xpath);
            if (entry != null) return entry;
            return null;
        }

        public void PopulateNodeDictionary(HtmlNode node, String leafSelectXPath = "", List<String> toIgnore = null, List<String> toRemoveXPathMatch = null)
        {
            if (leafSelectXPath.isNullOrEmpty()) leafSelectXPath = LeafNodeDictionary.DefaultNodeSelectionXPath;
            if (toIgnore.isNullOrEmpty()) toIgnore = LeafNodeDictionary.DefaultTagsToIgnore;
            if (toRemoveXPathMatch.isNullOrEmpty()) toRemoveXPathMatch = LeafNodeDictionary.DefaultXPathMatchToRemove;

            var selected = node.SelectNodes(leafSelectXPath);

            if (!selected.isNullOrEmpty())
            {
                foreach (var l in selected)
                {
                    String xp = l.XPath;
                    Boolean remove = false;
                    foreach (var xpr in toRemoveXPathMatch)
                    {
                        if (xp.Contains(xpr))
                        {
                            remove = true;
                            break;
                        }
                    }
                    if (remove) continue;

                    String lt = l.InnerText.Replace(Environment.NewLine, "").Trim();
                    if (!lt.isNullOrEmpty())
                    {
                        if (toIgnore.Contains(l.Name.ToLower()))
                        {

                        }
                        else
                        {
                            Add(l);
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Node have no child nodes that match [" + leafSelectXPath + "] XPath", nameof(node));
            }

            if (node.NodeType == HtmlNodeType.Element)
            {
                GetAndRemoveByXPathRoot(node.XPath, true, true);
            }
        }

        /// <summary>
        /// Gets the first name of the significant parent by node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="significanceLevel">The significance level: rade of occurence, less value more significant/rare the tag is.</param>
        /// <returns></returns>
        public HtmlNode GetFirstSignificantParentByNodeName(HtmlNode node, Double significanceLevel=0.2)
        {

            HtmlNode head = node;
            Double rate = 1;
            Int32 topFreq = NodeTagCounter.GetTopFrequency();

            while (rate > significanceLevel) {

                if (head.ParentNode == null)
                {
                    return head;
                }

                Int32 freq = NodeTagCounter.GetFrequencyForItem(head.Name);
                if (freq == 0)
                {
                    return head;
                }
                rate = freq.GetRatio(topFreq);

                head = head.ParentNode;
            
            }
            return head;
            
        }


        [XmlIgnore]
        public Dictionary<String, LeafNodeDictionaryEntry> CachedIndex { get; set; } = new Dictionary<string, LeafNodeDictionaryEntry>();

        public List<LeafNodeDictionaryEntry> AddRange(IEnumerable<HtmlNode> nodes)
        {
            List<LeafNodeDictionaryEntry> output = new List<LeafNodeDictionaryEntry>();

            foreach (var item in nodes)
            {
                LeafNodeDictionaryEntry entry = new LeafNodeDictionaryEntry(item);
                items.Add(entry);
                CachedIndex.Add(entry.XPath, entry);
                output.Add(entry);
            }
            return output;
        }

        public List<LeafNodeDictionaryEntry> AddEntryNodes(IEnumerable<HtmlNode> nodes)
        {
            List<LeafNodeDictionaryEntry> output = new List<LeafNodeDictionaryEntry>();
            foreach (HtmlNode node in nodes)
            {
                var e = AddEntryNode(node);
                if (e != null)
                {
                    output.Add(e);
                }
            }
            return output;
        }

        public virtual LeafNodeDictionaryEntry AddEntryNode(HtmlNode node)
        {
            return Add(node);
        }


        public virtual LeafNodeDictionaryEntry AddOrGet(HtmlNode item)
        {
            if (item != null)
            {
                String xpath = item.XPath;

                var entry = GetEntry(xpath);
                if (entry == null)
                {
                    entry = Add(item);
                }
                
                return entry;
            }
            return null;
        }


        public virtual LeafNodeDictionaryEntry Add(HtmlNode item)
        {
            if (item != null)
            {
                LeafNodeDictionaryEntry entry = new LeafNodeDictionaryEntry(item);
                items.Add(entry);
                CachedIndex.Add(entry.XPath, entry);
                return entry;
            }
            return null;
        }

        public void Add(LeafNodeDictionaryEntry entry)
        {
            if (entry != null)
            {
                items.Add(entry);
                CachedIndex.Add(entry.XPath, entry);
            }
        }

        public List<LeafNodeDictionaryEntry> items { get; set; } = new List<LeafNodeDictionaryEntry>();


    }
}