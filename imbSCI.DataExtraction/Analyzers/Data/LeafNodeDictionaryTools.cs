using HtmlAgilityPack;
using imbSCI.Core.extensions;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.functions;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.style.color;
using imbSCI.Data;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions.data;
using imbSCI.DataExtraction.Analyzers.Templates;
using imbSCI.Graph.Converters;
using imbSCI.Graph.Converters.tools;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using imbSCI.Graph.DGML.enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    public static class LeafNodeDictionaryTools
    {

        


        public static List<String> GetStepBackPaths(List<String> pathList, Int32 stepsToRetreatFromLeaf)
        {
            List<String> commonStepBack = new List<string>();

            foreach (String common in pathList)
            {
                String cparent = common.getPathVersion(stepsToRetreatFromLeaf, "/");
                if (!commonStepBack.Contains(cparent)) commonStepBack.Add(cparent);
            }
            return commonStepBack;
        }
        
        ///// <summary>
        ///// Prevazidjen metod
        ///// </summary>
        ///// <param name="input">The input.</param>
        ///// <param name="FirstStepBackSteps">The first step back steps.</param>
        ///// <param name="StepsBackFromLeaf">The steps back from leaf.</param>
        ///// <returns></returns>
        //public static List<DataPointMapBlock> GetDataPointXPaths(this LeafNodeDictionaryAnalysis input, Int32 FirstStepBackSteps, Int32 StepsBackFromLeaf)
        //{
        //    //List<String> staticPaths = input.StaticContent.items.Select(x => x.XPath).ToList();
        //    //List<String> dynamicPaths = input.DynamicContent.items.Select(x => x.XPath).ToList();

        //    /*
        //    List<String> dynamicParentPaths = new List<string>();
        //    foreach (String p in dynamicPaths)
        //    {
        //        String pp = p.getPathVersion(FirstStepBackSteps, "/");
        //        dynamicParentPaths.AddUnique(pp);

        //    }*/

        //    List<String> commonStepBack = GetStepBackPaths(dynamicPaths, FirstStepBackSteps);

        //    List<DataPointMapBlock> output = new List<DataPointMapBlock>();

        //    List<String> dpRootPaths = new List<string>();
        //    List<DataPointMapEntry> dpList = new List<DataPointMapEntry>();

        //    foreach (String common in commonStepBack)
        //    {

        //        IEnumerable<LeafNodeDictionaryEntry> staticNodes = input.StaticContent.items.Where(x => x.XPath.StartsWith(common)); //.ToList();
        //        IEnumerable<LeafNodeDictionaryEntry> dynamicNodes = input.DynamicContent.items.Where(x => x.XPath.StartsWith(common)); //.ToList();

        //        if (staticNodes == null)
        //        {
        //            if (!staticNodes.Any())
        //            {
        //                continue;
        //            }
        //        }

        //        if (dynamicNodes == null)
        //        {
        //            if (!dynamicNodes.Any())
        //            {
        //                continue;
        //            }
        //        }

        //        var staticNodeList = staticNodes.ToList();
        //        var dynamicNodeList = dynamicNodes.ToList();

        //        for (int i = 0; i < Math.Min(staticNodeList.Count, dynamicNodeList.Count); i++)
        //        {
        //            DataPointMapEntry dp = new DataPointMapEntry(staticNodeList[i].XPath, dynamicNodeList[i].XPath);
        //            dpRootPaths.Add(dp.DataPointXPathRoot);
        //            dpList.Add(dp);
        //        }

        //    }

        //    List<String> dpbRootPaths = GetStepBackPaths(dpRootPaths, StepsBackFromLeaf);

        //    //  Dictionary<String, List<DataPointMapEntry>> dpbAlign = new Dictionary<string, List<DataPointMapEntry>>();

        //    foreach (String dpbRoot in dpbRootPaths)
        //    {
        //        List<DataPointMapEntry> selected = new List<DataPointMapEntry>();

        //        foreach (DataPointMapEntry e in dpList)
        //        {
        //            if (e.DataPointXPathRoot.StartsWith(dpbRoot))
        //            {
        //                selected.Add(e);
        //            }
        //        }

        //        DataPointMapBlock block = new DataPointMapBlock(selected, dpbRoot);
        //        output.Add(block);
        //    }
        //    return output;

        //}

        ///// <summary>
        /// Breaks the blocks by record dimensions.
        /// </summary>
        /// <param name="mapBlocks">The map blocks.</param>
        /// <returns></returns>
        public static List<DataPointMapBlock> BreakBlocksByRecordDimensions(IEnumerable<DataPointMapBlock> mapBlocks)
        {
            List<DataPointMapBlock> newBlocks = new List<DataPointMapBlock>();

            foreach (DataPointMapBlock block in mapBlocks)
            {
                Dictionary<Int32, DataPointMapBlock> splitBlocks = new Dictionary<int, DataPointMapBlock>();

                foreach (var dp in block.DataPoints)
                {
                    Int32 dc = dp.GetNumberOfDimensions(false);
                    if (!splitBlocks.ContainsKey(dc)) splitBlocks.Add(dc, new DataPointMapBlock(block.BlockXPathRoot));
                    splitBlocks[dc].DataPoints.Add(dp);
                }

                newBlocks.AddRange(splitBlocks.Values);

            }

            return newBlocks;
        }

        public static DirectedGraph ConstructDPBGraph(this LeafNodeDictionaryAnalysis input, DataPointMapperResult output)
        {

            Func<graphWrapNode<LeafNodeDictionaryEntry>, Double> nodeW = x =>
            {
                foreach (var b in output.MapBlocks)
                {
                    if (x.path.StartsWith(b.BlockXPathRoot))
                    {
                        return 1;
                    }
                }
                return 0.5;

            };

            StructureGraphConverter converter = new StructureGraphConverter()
            {
                CategoryID = x => "Default",
                TypeID = x => 1,
                NodeWeight = nodeW,
                LinkWeight = (x, y) => 1
            };


            var c_dmgl = converter.Convert(input.CompleteGraph, 500);

            return c_dmgl;

        }

        public static DirectedGraph ConstructGraph(this LeafNodeDictionaryAnalysis analysis, DataPointMapperResult output)
        {

            //graphWrapNode<LeafNodeDictionaryEntry> dynamicContent = graphTools.BuildGraphFromItems<LeafNodeDictionaryEntry, graphWrapNode<LeafNodeDictionaryEntry>>(analysis.DynamicContent.items, new Func<LeafNodeDictionaryEntry, string>(x => x.XPath), "/");

            //graphWrapNode<LeafNodeDictionaryEntry> staticContent = graphTools.BuildGraphFromItems<LeafNodeDictionaryEntry, graphWrapNode<LeafNodeDictionaryEntry>>(analysis.StaticContent.items, new Func<LeafNodeDictionaryEntry, string>(x => x.XPath), "/");



            Dictionary<IGraphNode, Double> Weight = new Dictionary<IGraphNode, double>();


            foreach (graphWrapNode<LeafNodeDictionaryEntry> leaf in analysis.CompleteGraph.getAllLeafs())
            {
                if (leaf.item != null)
                {
                    if (leaf.item.Category.HasFlag(NodeInTemplateRole.Dynamic))
                    {
                        var nd = leaf.Add("$DYNAMIC$");
                        Weight[leaf] = 1;
                    }
                    else if (leaf.item.Category.HasFlag(NodeInTemplateRole.Static))
                    {
                        Weight[leaf] = 0.8;
                        if (!leaf.item.Content.isNullOrEmpty())
                        {
                            var nd = leaf.Add(leaf.item.Content.trimToLimit(50, true));

                        }
                    }

                }
            }


            Func<graphWrapNode<LeafNodeDictionaryEntry>, Int32> typeID = x =>
            {
                if (x.item == null) return 0;
                return 1;
            };

            Func<graphWrapNode<LeafNodeDictionaryEntry>, Double> nodeW = x =>
            {
                if (x.item == null) return 0.2;
                if (Weight.ContainsKey(x))
                {
                    return Weight[x];
                }
                return 0.3;
                //return (1 - (((Double)0.8 / ((Double)x.Count() + 1))));

            };

            Func<graphWrapNode<LeafNodeDictionaryEntry>, String> categoryID = x =>
            {
                foreach (var b in output.MapBlocks)
                {
                    if (x == null) return "Null";
                    if (x.item == null)
                    {
                        return "Null";
                    }
                    if (x.item.XPath.StartsWith(b.BlockXPathRoot))
                    {
                        String dpr = x.item.XPath.removeStartsWith(b.BlockXPathRoot);

                        foreach (var dp in b.DataPoints)
                        {
                            if (dpr == dp.DataPointXPathRoot + dp.LabelXPathRelative)
                            {
                                return "Label";
                            }

                            if (dpr == dp.DataPointXPathRoot + dp.DataXPathRelative)
                            {
                                return "Data";
                            }


                        }

                        return b.name;
                    }
                }

                if (x.item != null) return x.item.Category.ToString();
                return "Structure";
            };

            List<String> Categories = new List<string>();

            foreach (graphWrapNode<LeafNodeDictionaryEntry> g in analysis.CompleteGraph.getAllChildren())
            {
                Categories.AddUnique(categoryID(g));
            }


            StructureGraphConverter converter = new StructureGraphConverter()
            {
                CategoryID = categoryID,
                TypeID = typeID,
                NodeWeight = nodeW,
                LinkWeight = (x, y) => 1
            };





            var c_dmgl = converter.Convert(analysis.CompleteGraph, 500);



            return c_dmgl;


        }


       

    }
}