using imbSCI.Core.math.range.frequency;
using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using HtmlAgilityPack;
using imbSCI.Core.extensions.text;
using System.Linq;
using imbSCI.Core.extensions.data;
using imbSCI.DataExtraction.Extractors;

namespace imbSCI.DataExtraction.Analyzers.Structure
{

    /// <summary>
    /// Utility class to analyse graph junctions (nodes with more than one child)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class JunctionGraphMetrics<T> where T : graphWrapNode<LeafNodeDictionaryEntry>, IGraphNode
    {

        /// <summary>
        /// Gets sets of graph nodes (<see cref="JunctionPoint{T}"/>) with same junction signature (sequence of child names)
        /// </summary>
        /// <param name="MinFrequency">The minimum frequency - minimal number of same-child-count occurances, before signature comparison</param>
        /// <param name="MinJunctionSize">Minimal number of child nodes - size of the junction.</param>
        /// <param name="DescendingByFreq">Result set will be given i descending order by frequency</param>
        /// <param name="TrimTemplateItems">The <see cref="RecordTemplate"/> in <see cref="JunctionPoint{T}.Template"/> will contain <see cref="RecordTemplateItem"/>s targeting only first-level child, not leafs</param>
        /// <returns></returns>
        public List<JunctionPoint<T>> GetJunctionBlocks(Int32 MinFrequency, Int32 MinJunctionSize, Boolean DescendingByFreq, Boolean TrimTemplateItems)
        {
            Dictionary<int, List<int>> freqBinByJunctions = JunctionCounter.GetFrequencyBins();

            List<Int32> freqList = freqBinByJunctions.Keys.ToList();
            if (DescendingByFreq)
            {
                freqList = freqList.OrderByDescending(x=>x).ToList();
            }
            

            Dictionary<String, List<T>> signatureDictionary = new Dictionary<string, List<T>>();
            Dictionary<String, JunctionPoint<T>> signatureBlockDictionary = new Dictionary<string, JunctionPoint<T>>();

            foreach (Int32 f in freqList)
            {
                if (f> MinFrequency)
                {
                    List<T> junstionsAtFreq = JunctionCounter.GetInstances(freqBinByJunctions[f]);

                    foreach (T jn in junstionsAtFreq)
                    {
                        var childNames = jn.getChildNames();
                        if (childNames.Count < MinJunctionSize)
                        {
                            continue;
                        }

                        String childLineSignature = childNames.toCsvInLine("_").Replace("[", "").Replace("]", "");
                        if (!childLineSignature.isNullOrEmpty())
                        {
                            if (!signatureDictionary.ContainsKey(childLineSignature))
                            {
                                signatureDictionary.Add(childLineSignature, new List<T>());
                                JunctionPoint<T> junctionBlock = new JunctionPoint<T>()
                                {
                                    JunctionSizeFrequency = f
                                };
                                signatureBlockDictionary.Add(childLineSignature, junctionBlock);
                            }
                            signatureDictionary[childLineSignature].Add(jn);
                        }
                    }
                    
                }

            }



            List<JunctionPoint<T>> junctionBlocks = new List<JunctionPoint<T>>();

            foreach (var pair in signatureDictionary)
            {
                JunctionPoint<T> jp = signatureBlockDictionary[pair.Key];
                jp.Signature = pair.Key;
                jp.items.AddRange(pair.Value);

                var jps = jp.ExplodeByParentJunctions();



                

                //if (jp.items.Count == 0) continue;

                

                //jp.Template = new RecordTemplate();

                //jp.Template.SubXPath = item.name;

                //jp.type = JunctionPointType.BranchToLeafs;

                //jp.XPathRoot = jp.items.Select(x => x.path).GetCommonPathRoot();



                //foreach (graphWrapNode<LeafNodeDictionaryEntry> child in item.GetChildren()) {

                //    RecordTemplateItem r_item = new RecordTemplateItem();
                                        
                //    if (child.IsBranchToLeaf())
                //    {

                //    } else
                //    {
                //        jp.type = JunctionPointType.DeepJunctionPoint;
                //        break;
                //    }

                //    if (child.item != null)
                //    {
                //        r_item.Category = child.item.Category;
                //    }

                //    if (TrimTemplateItems)
                //    {
                //        r_item.SubXPath = child.name;
                //    } else
                //    {
                //        r_item.SubXPath = child.path.GetRelativeXPath(item.path);
                //    }

                //    jp.Template.items.Add(r_item);
                //}

                //jp.Level = item.level;

              



                //jp.Signature = pair.Key;
                
                //jp.JunctionSize = jp.items.First().GetChildren().Count;

                foreach (var jpi in jps)
                {
                    jpi.ProcessItems(null, TrimTemplateItems);
                    junctionBlocks.Add(jpi);
                    
                }

                if (jps.Count > 1)
                {

                }



            }


            return junctionBlocks;
        }


        
        //public List<JunctionPoint<T>> GetJunctionBlocks(Int32 JunctionSizeMin, Int32 StepsToRetreatFromLeaf, Int32 minLevel)
        //{
        //    Dictionary<int, List<int>> freqBinByJunctions = JunctionCounter.GetFrequencyBins();

        //    List<JunctionPoint<T>> junctionBlocks = new List<JunctionPoint<T>>();

        //    foreach (KeyValuePair<int, List<int>> freqPair in freqBinByJunctions)
        //    {
        //        foreach (int junctionSize in freqPair.Value)
        //        {
        //            if (junctionSize > JunctionSizeMin)
        //            {
        //                List<T> freqBins = JunctionCounter.GetInstances(new List<Int32>() { junctionSize });


        //                String childTemplate = "";

        //                Dictionary<String, JunctionPoint<T>> parentNodeDictionary = new Dictionary<string, JunctionPoint<T>>();
                        

        //                foreach (T gNode in freqBins)
        //                {
        //                    String cparent = gNode.path; //.parent.path; 
        //                    //gNode.getChildNames().toCsvInLine();
        //                     //gNode.path.getPathVersion(StepsToRetreatFromLeaf, "/");   

        //                    if (!parentNodeDictionary.ContainsKey(cparent))
        //                    {
        //                        var junctionBlock = new JunctionPoint<T>()
        //                        {
        //                            XPathRoot = cparent,
        //                            JunctionSize = junctionSize,
        //                            JunctionSizeFrequency = freqPair.Key,
        //                            Level = gNode.level // - StepsToRetreatFromLeaf        
        //                        };
        //                        parentNodeDictionary.Add(cparent, junctionBlock);
        //                   } 
        //                    parentNodeDictionary[cparent].items.Add(gNode);
                            
        //                }

        //                List<JunctionPoint<T>> blocks = new List<JunctionPoint<T>>();
        //                foreach (JunctionPoint<T> block in parentNodeDictionary.Values)
        //                {
        //                    if (block.Level > minLevel)
        //                    {
        //                        if (block.items.Count > 0)
        //                        {

        //                            blocks.Add(block);
        //                        }
        //                    }
        //                }

        //                if (freqBins.Count > blocks.Count)
        //                {

        //                }

        //                if (blocks.Any())
        //                {
        //                    junctionBlocks.AddRange(blocks);
        //                }

        //            }
        //        }
        //    }
            
        //    return junctionBlocks;
        //}

        public void Process(T graphRoot)
        {
            var childrenAll = graphRoot.getAllChildren();
            foreach (T ch in childrenAll)
            {
                JunctionCounter.CountInstance(ch);
            }
        }

        public JunctionGraphMetrics() {

            Func<T, Int32> childCount = x =>
            {
                if (x is graphWrapNode<LeafNodeDictionaryEntry> TWrap)
                {
                    var children = TWrap.GetChildren();
                    return children.Count;
                } else
                {
                    return x.GetChildren().Count;
                }
                
            };

            JunctionCounter = new frequencyCounterForProperty<T, Int32>(childCount);
        }

        public frequencyCounterForProperty<T, Int32> JunctionCounter { get; protected set; }

    }
}
