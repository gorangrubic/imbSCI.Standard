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
using imbSCI.Data.interfaces;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.reporting.render;
using imbSCI.Core.data;
using System.Data;
using imbSCI.DataComplex.tables;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Core.extensions.table;
using imbSCI.Graph.Data;
using imbSCI.Graph.DGML;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.Extractors;
using imbSCI.Graph.DGML.core;
using imbSCI.DataExtraction.Tools;

namespace imbSCI.DataExtraction.Analyzers.Data
{

    

    [Serializable]
    public class NodeGraph :  graphWrapNode<LeafNodeDictionaryEntry>, IObjectWithPathAndChildren
    {

        public DirectedGraphWithSourceData BuildDirectedGraph(NodeDictionaryGraphStyleSettings style=null)
        {
            if (style == null) style = new NodeDictionaryGraphStyleSettings();
            DirectedGraphWithSourceData output = new DirectedGraphWithSourceData();

            List<NodeGraph> allChildren = this.getAllChildrenInType<NodeGraph>();

            output.Populate(allChildren, x => x.GetChildren<NodeGraph>(), x => x.path, x => x.name, false, false);


            // output.Populate(allChildren, x => x.item., x => x.path, x => x.name, false, false);

            foreach (var pair in output.Sources)
            {
                if (pair.Key is Link graphLink)
                {
                    style.SetElement(graphLink, pair.Value.LastOrDefault() as NodeGraph);


                }
                else if (pair.Key is Node graphNode)
                {
                    style.SetElement(graphNode, pair.Value.LastOrDefault() as NodeGraph);
                }
            }

            if (style.AddContentNodes)
            {
                var nodesWithItem = allChildren.Where(x => x.item != null).ToList();

                var contentNodes = output.Populate<NodeGraph, HtmlNode>(nodesWithItem, x => x.item.node.SelectTextLeafNodes(), x => x.path, x => x.name, x => x.XPath, x => x.GetInnerText(), false, false);
                foreach (KeyValuePair<GraphElement, List<object>> pairs in contentNodes)
                {
                    style.SetContentElement(pairs.Key);
                }
            }

            return output;

        }


        [NonSerialized]
        protected Object MetaData;

        public void SetMetaData(Object _data)
        {

            MetaData = _data;
        }

        public TData GetMetaData<TData>() where TData:class
        {
            return MetaData as TData;
        }

        public Boolean HasMetaData()
        {
            return MetaData != null;
        }

        public StructureGraphInformation ConstructionInfo { get; set; } = null;

        /// <summary>
        /// Gets information on current state of the graph
        /// </summary>
        /// <returns></returns>
        public StructureGraphInformation GetCurrentInfo()
        {
            StructureGraphInformation info = new StructureGraphInformation();
            info.Populate(this);
            return info;
        }


        /// <summary>
        /// Clones the graph using nodes with <see cref="LeafNodeDictionaryEntry"/> items set
        /// </summary>
        /// <returns></returns>
        public NodeGraph CloneByItems()
        {

            var allChildren = this.getAllChildren(null, false, false, 1, 500, true);

            List<LeafNodeDictionaryEntry> Items = new List<LeafNodeDictionaryEntry>();
            foreach (NodeGraph child in allChildren)
            {
                if (child.item != null)
                {
                    Items.Add(child.item); 
                }
            }

            var clonedGraph = Build(Items);

            return clonedGraph;
        }


        public void ConstructParentEntries()
        {
            var allChildren = this.getAllChildren(null, false, false, 1, 500, true);

            List<LeafNodeDictionaryEntry> Items = new List<LeafNodeDictionaryEntry>();

            Dictionary<String, HtmlNode> toAssign = new Dictionary<string, HtmlNode>();

            foreach (NodeGraph child in allChildren)
            {
                if (child.item != null)
                {
                    LeafNodeDictionaryEntry entry = child.item;

                    if (entry.node != null)
                    {
                        if (entry.node.ParentNode != null)
                        {
                            String xpath = entry.node.ParentNode.XPath;

                            if (!toAssign.ContainsKey(xpath))
                            {
                                toAssign.Add(xpath, entry.node.ParentNode);
                            }
                        }
                    }
                    
                    Items.Add(child.item);
                }
            }

            NodeGraph rootGraph = root as NodeGraph;

            foreach (var pair in toAssign)
            {
                LeafNodeDictionaryEntry entry = new LeafNodeDictionaryEntry()
                {
                    XPath = pair.Key,
                    node = pair.Value,
                    Category = NodeInTemplateRole.Structural,

                };
                var graph_node = rootGraph.GetChildAtPath(pair.Key, "/");
                graph_node.SetItem(entry);
            }

        }
       

        public static NodeGraph Build(List<LeafNodeDictionaryEntry> items) 
        {
            StructureGraphInformation info = new StructureGraphInformation();
            List<String> xpathList = new List<string>();

            NodeGraph node = null;
            foreach (LeafNodeDictionaryEntry entry in items)
            {
                if (!xpathList.Contains(entry.XPath))
                {
                    node = graphTools.ConvertPathToGraph<NodeGraph>(node, entry.XPath, true, "/", true);
                    node.SetItem(entry);
                    node = node.root as NodeGraph;
                    xpathList.Add(entry.XPath);
                }
            }

            if (node == null)
            {
                node = new NodeGraph();
            }

            info.Populate(node);
            info.InputCount = items.Count;
            

            node.ConstructionInfo = info;

            //var CompleteGraph = graphTools.BuildGraphFromItems<LeafNodeDictionaryEntry, NodeGraph>(items, getPath, "/");
            return node;
        }

       

        public static Func<LeafNodeDictionaryEntry, string> getPath = x =>
            {
                if (x == null)
                {
                    return "/null";
                }
                if (x.XPath.isNullOrEmpty()) return "/null";
                return x.XPath;
            };


        public override String _PathRootPrefix
        {
            get
            {
                return "/";
            }
        }

        public override string pathSeparator
        {
            get
            {
                return "/";
            }
            set
            {

            }
        }
            

        public NodeGraph()
        {
            

            
        }
    }
}