using imbSCI.Core.extensions.data;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Analyzers.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Analyzers
{
    [Serializable]
    public class DataPointMapper
    {
        
        
        /// <summary>
        /// Merges the entries to data points.
        /// </summary>
        /// <param name="entries">The entries.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        internal List<DataPointMapEntry> MergeEntriesToDataPoints(List<LeafNodeDictionaryEntry> entries,  LeafNodeDictionaryAnalysis input)
        {
            List<DataPointMapEntry> dpList = new List<DataPointMapEntry>();

            List<DataPointMapEntry> dp_tmp = new List<DataPointMapEntry>();
            DataPointMapEntry dp = new DataPointMapEntry(){         };
            dp_tmp.Add(dp);

            

            foreach (LeafNodeDictionaryEntry leaf in entries)
            {
                Boolean wasSet = false;

                var inputLeaf = input.Nodes.GetEntry(leaf.XPath);

                for (int i = 0; i < dp_tmp.Count; i++)
                {
                    if (dp_tmp[i].SetLeaf(leaf, inputLeaf.Category.HasFlag(NodeInTemplateRole.Dynamic)));
                    {
                        wasSet = true;
                        break;
                    }
                }
                if (!wasSet)
                {
                    var dpn = new DataPointMapEntry() { };
                    dpn.SetLeaf(leaf, inputLeaf.Category.HasFlag(NodeInTemplateRole.Dynamic));
                    dp_tmp.Add(dpn);
                }
            }
            List<DataPointMapEntry> post_deploy = new List<DataPointMapEntry>();
            String root = "";
            foreach (var dpn in dp_tmp)
            {
                if (dpn.IsSet )
                {
                    dpList.Add(dpn);
                    root = dpn.DataPointXPathRoot;
                }
                else
                {
                    if (dp_tmp.Count > 1)
                    {
                        if (flags.HasFlag(DataPointMapBlockDetectionFlags.AllowAsimetricMultiColumnDataPoints))
                        {
                            post_deploy.Add(dpn);

                        }
                    }
                }
            }

            foreach (var p in post_deploy)
            {
                if (p.DataPointXPathRoot.isNullOrEmpty()) p.DataPointXPathRoot = root;
            }

            return dpList;
        }



        /// <summary>
        /// Gets the data point pairs: Junction method
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="ChildrenCountTrigger">The children count trigger.</param>
        /// <returns></returns>
        public  DataPointMapperResult GetDataPointPairs(LeafNodeDictionaryAnalysis input)
        {

            List<DataPointMapEntry> dpList = new List<DataPointMapEntry>();

            var allLeafs = input.CompleteGraph.getAllLeafs();

         
            List<String> dpRoots = new List<string>();
            foreach (graphWrapNode<LeafNodeDictionaryEntry> g in allLeafs)
            {
                var inputLeaf = input.Nodes.GetEntry(g.path);

                if (g.parent != null)
                {
                    if (inputLeaf.Category.HasFlag(NodeInTemplateRole.Dynamic))
                    {
                        graphWrapNode<LeafNodeDictionaryEntry> head = g;

                        Int32 pC = head.Count();

                        while (pC < ChildrenCountTrigger)
                        {
                            if (head.parent != null)
                            {
                                head = head.parent as graphWrapNode<LeafNodeDictionaryEntry>;
                                if (head == null)
                                {
                                    break;
                                }
                                else
                                {
                                    pC = head.parent.Count();
                                }
                            }
                        }
                        if (head != null)
                        {
                            if (head.path.isNullOrEmpty())
                            {
                                if (!dpRoots.Contains(head.path))
                                {
                                    dpRoots.Add(head.path);
                                }
                            }
                        }
                       } else
                    {

                    }
                }
            }



            
            foreach (String root in dpRoots)
            {
                var dpItems = input.Nodes.items.Where(x => x.XPath.StartsWith(root)).ToList();

                List<DataPointMapEntry> dp_tmp = MergeEntriesToDataPoints(dpItems, input);

                if (dp_tmp.Count >1 && flags.HasFlag(DataPointMapBlockDetectionFlags.AllowMultiColumnDataPoints))
                {
                    DataPointMapEntry parent_dp = new DataPointMapEntry()
                    {
                        DataPointXPathRoot = root,
                        LabelXPathRelative = "",
                        DataXPathRelative = "",
                        Properties = dp_tmp
                    };
                    foreach (var d in dp_tmp)
                    {
                        d.DataPointXPathRoot = "";
                    }

                    dpList.Add(parent_dp);
                } else {

                    dpList.AddRange(dp_tmp);
                }
            }




            List<graphWrapNode<LeafNodeDictionaryEntry>> dpRootGraphNodes = new List<graphWrapNode<LeafNodeDictionaryEntry>>();

            foreach (String root in dpRoots)
            {
                var cp = input.CompleteGraph.GetChildAtPath(root);
                if (cp != null) dpRootGraphNodes.AddUnique(cp);
            }

            DataPointMapperResult output = new DataPointMapperResult();


            List<String> BlockRoots = new List<string>();
            foreach (graphWrapNode<LeafNodeDictionaryEntry> g in dpRootGraphNodes)
            {
                if (g.parent != null)
                {
                    graphWrapNode<LeafNodeDictionaryEntry> head = g;

                    Int32 pC = head.Count();

                    while (pC < JunctionSizeMin)
                    {
                        if (head.level > 1)
                        {
                            head = head.parent as graphWrapNode<LeafNodeDictionaryEntry>;
                            if (head.parent == null)
                            {
                                break;
                            }
                            pC = head.parent.Count();
                        }
                    }

                    if (head != null)
                    {

                        if (!head.path.isNullOrEmpty())
                        {
                            if (!BlockRoots.Contains(head.path))
                            {
                                BlockRoots.Add(head.path);
                            }
                        }
                    }
                }
            }


            if (!BlockRoots.Any())
            {
                return output;
            }

            if (flags.HasFlag(DataPointMapBlockDetectionFlags.maximizeBlockSize)) {
                BlockRoots = BlockRoots.OrderBy(x => x.Length).ToList();
            } else if (flags.HasFlag(DataPointMapBlockDetectionFlags.maximizeDataRelatness))
            {
                BlockRoots = BlockRoots.OrderByDescending(x => x.Length).ToList();
            }

            

            foreach (String blockRoot in BlockRoots)
            {
                var b = new DataPointMapBlock(blockRoot);

                var dpl = dpList.ToList().Where(x => x.DataPointXPathRoot.StartsWith(blockRoot));

                foreach (DataPointMapEntry e in dpl) {
                    dpList.Remove(e);
                    b.DataPoints.Add(e);
                }
                if (b.DataPoints.Count > 0)
                {
                    output.MapBlocks.Add(b);
                }    
            }

            //if (flags.HasFlag(DataPointMapBlockDetectionFlags.BreakByDimensions))
            //{
            //    BreakBlocksByRecordDimensions(output.MapBlocks);
    
            //}

            return output;
        }

        public Int32 ChildrenCountTrigger { get; set; } = 2;

        public Int32 JunctionSizeMin { get; set; } = 3;

        public DataPointMapBlockDetectionFlags flags { get; set; } = DataPointMapBlockDetectionFlags.All;

        //public Int32 StepsBackFromLeafs { get; set; } = 3;

        //public Int32 FirstStepBackSteps { get; set; } = 1;

        //public String LeafSelector { get; set; } = "//body//*[not(*)]";

        public DataPointMapper()
        {

        }

    }
}