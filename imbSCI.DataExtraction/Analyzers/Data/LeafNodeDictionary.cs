using HtmlAgilityPack;
using imbSCI.Core.math;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions;
using imbSCI.Data;
using imbSCI.Core.extensions.data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using imbSCI.Data.collection.graph;

namespace imbSCI.DataExtraction.Analyzers.Data
{

    [Serializable]
    public class LeafNodeDictionary:NodeDictionary
    {
        public LeafNodeDictionary()
        {

        }

        public LeafNodeDictionary Clone()
        {
            LeafNodeDictionary output = new LeafNodeDictionary();
            foreach (var item in items)
            {
                output.Add(item);
            }
            output.RebuildIndex();
            return output;
        }

       


        //public static List<String> NodesToIgnore { get; set; } 

       

        private static Object _tagsToIgnore_lock = new Object();
        private static List<String> _tagsToIgnore;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<String> DefaultTagsToIgnore
        {
            get
            {
                if (_tagsToIgnore == null)
                {
                    lock (_tagsToIgnore_lock)
                    {

                        if (_tagsToIgnore == null)
                        {
                            _tagsToIgnore = new List<String>();
                            _tagsToIgnore.Add("script");
                            _tagsToIgnore.Add("img");
                            _tagsToIgnore.Add("svg");
                            _tagsToIgnore.Add("g");
                            _tagsToIgnore.Add("aside");
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _tagsToIgnore;
            }
        }


        private static Object _XPathMatchToRemove_lock = new Object();
        private static List<String> _XPathMatchToRemove;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<String> DefaultXPathMatchToRemove
        {
            get
            {
                if (_XPathMatchToRemove == null)
                {
                    lock (_XPathMatchToRemove_lock)
                    {

                        if (_XPathMatchToRemove == null)
                        {
                            _XPathMatchToRemove = new List<String>();
                            _XPathMatchToRemove.Add("script");
                            _XPathMatchToRemove.Add("img");
                            _XPathMatchToRemove.Add("svg");
                            _XPathMatchToRemove.Add("g");
                            //_XPathMatchToRemove.Add("aside");
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _XPathMatchToRemove;
            }
        }


        public static String DefaultNodeSelectionXPath { get; set; } = ".//*[not(*)]";


        public LeafNodeDictionary(HtmlNode node, String leafSelectXPath="", List<String> toIgnore=null, List<String> toRemoveXPathMatch=null)
        {
            PopulateNodeDictionary(node, leafSelectXPath, toIgnore, toRemoveXPathMatch);


            //if (leafSelectXPath.isNullOrEmpty()) leafSelectXPath = DefaultNodeSelectionXPath;
            //if (toIgnore.isNullOrEmpty()) toIgnore = DefaultTagsToIgnore;
            //if (toRemoveXPathMatch.isNullOrEmpty()) toRemoveXPathMatch = DefaultXPathMatchToRemove;
             
            //var selected = node.SelectNodes(leafSelectXPath);

            //if (!selected.isNullOrEmpty())
            //{
            //    foreach (var l in selected)
            //    {
            //        String xp = l.XPath;
            //        Boolean remove = false;
            //        foreach (var xpr in toRemoveXPathMatch)
            //        {
            //            if (xp.Contains(xpr))
            //            {
            //                remove = true;
            //                break;
            //            }
            //        }
            //        if (remove) continue;

            //        String lt = l.InnerText.Replace(Environment.NewLine, "").Trim();
            //        if (!lt.isNullOrEmpty())
            //        {
            //            if (toIgnore.Contains(l.Name.ToLower()))
            //            {

            //            }
            //            else
            //            {
            //                Add(l);
            //            }
            //        }
            //    }
            //} else
            //{
            //    throw new ArgumentNullException("Node have no child nodes that match [" + leafSelectXPath + "] XPath", nameof(node));
            //}

            //if (node.NodeType == HtmlNodeType.Element)
            //{
            //    GetAndRemoveByXPathRoot(node.XPath, true, true);
            //}
        }

        

    }
}
