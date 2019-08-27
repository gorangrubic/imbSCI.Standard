using imbSCI.Core.math.range.frequency;
using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.Analyzers;
using HtmlAgilityPack;
using System.Linq;
using imbSCI.Core.extensions.data;
using System.Xml.Serialization;
using imbSCI.Core.collection;
using imbSCI.DataExtraction.Extractors.Detectors;
using imbSCI.Core.math.range.finder;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Extractors.Core;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
public class NodeGraphTemplateDetection
    {
        public Dictionary<String, NodeGraphTemplateCandidate> ChildrenByChildSignature = new Dictionary<string, NodeGraphTemplateCandidate>();

        public NodeGraph Root { get; set; }

        public NodeGraphTemplateCandidate GetBestCandidate()
        {
            var sorted = ChildrenByChildSignature.Values.OrderByDescending(x => x.GetScore()).ToList();

            var best = sorted.FirstOrDefault();

            return best;
        }

        public Boolean IsValid()
        {
            if (ChildrenByChildSignature.Count > 0)
            {
                return true;
            }
            if (!ChildrenByChildSignature.Any(x=>x.Value.Instances.Count > 1))
            {
                return false;
            }
            return false;
        }

        public RecordTemplateSet GetTemplateSet()
        {
            RecordTemplateSet output = new RecordTemplateSet();

            var candidates = ChildrenByChildSignature.Values.ToList();

            var best = GetBestCandidate();


            List<String> templateSignatures = new List<string>();
            foreach (var pair in candidates)
            {
                //if (best.IsParentOf(pair))
                //{

                //}
                //else
                //{

                var template = pair.GetTemplate(Root, false);
                var signature = template.Signature;

                if (templateSignatures.Contains(signature))
                {

                } else
                {
                    output.items.Add(template);
                    templateSignatures.Add(signature);
                }

                    
               // }
            }

            return output;

        }

        public NodeGraphTemplateDetection(NodeGraph rootNode)
        {

           // if (rootNode.level < 3) return;

            Root = rootNode;
            var allNodes = rootNode.GetChildren(); //.getAllChildren(null, false, false, 1, 500, false); //.getAllChildrenInType<NodeGraph>(null, false, false, 1, 500, true);

            foreach (NodeGraph node in allNodes)
            {
                NodeGraphTemplateCandidate candidate = new NodeGraphTemplateCandidate(node);
                if (candidate.LeafsByRelativePath.Count == 0) continue;

                String templateSignature = candidate.GetSignature();

                String childrenSignature = candidate.GetLeafSignature();
                


                if (ChildrenByChildSignature.ContainsKey(childrenSignature))
                {
                    var existingTemplate = ChildrenByChildSignature[childrenSignature];
                    existingTemplate.Instances.Add(node);
                } else
                {
                    ChildrenByChildSignature.Add(childrenSignature, candidate);
                }

            }

            
            foreach (var tr in ChildrenByChildSignature.Where(x => x.Value.LeafsByRelativePath.Count <= 1).ToList())
            {
                ChildrenByChildSignature.Remove(tr.Key);
            }

            if (ChildrenByChildSignature.Count > 0)
            {

            }

            /*
            foreach ( var tr in ChildrenByChildSignature.Where(x => x.Value.Instances.Count <= 1).ToList())
            {
                ChildrenByChildSignature.Remove(tr.Key);
            }*/
        }
    }
}