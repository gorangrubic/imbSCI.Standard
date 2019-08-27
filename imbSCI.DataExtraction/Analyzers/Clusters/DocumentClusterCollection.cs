using HtmlAgilityPack;
using imbSCI.Core.files.folders;
using imbSCI.Core.math.range.finder;
using imbSCI.DataExtraction.Analyzers.Similarity.similarity;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Data;
using imbSCI.DataExtraction.Analyzers.Similarity;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Analyzers.Clustersters
{
    public class DocumentClusterCollection:ItemClusterCollection<HtmlNode>
    {
        public String name { get; set; } = "";

        public DocumentClusterCollection()
        {

        }


        //public Dictionary<HtmlNode, DocumentCluster> GetClusterByDocumentDictionary()
        //{
        //    Dictionary<HtmlNode, DocumentCluster> output = new Dictionary<HtmlNode, DocumentCluster>();

        //    foreach (DocumentCluster cluster in this.)
        //    {
        //        var nodes = cluster.items.Select(x => x);

        //        foreach (HtmlNode node in nodes)
        //        {
        //            context.DeclarationConstruction_ClusterAnalysisContext.ClusterByDocuments.Add(node, cluster);
        //        }

        //        if (cluster.ClusterSeed != null)
        //        {
        //            context.DeclarationConstruction_ClusterAnalysisContext.ClusterByDocuments.Add(cluster.ClusterSeed, cluster);
        //        }
        //    }
        //}

        public void Publish(Dictionary<HtmlNode, HtmlSourceAndUrl> documentNodeDictionary, folderNode folderWithResults, DocumentSimilarityResult result)
        {
             folderWithResults.generateReadmeFiles(null);
            var items = GetClusters<DocumentCluster>(true);

            Dictionary<HtmlNode, string> labelsByDocument = result.GetLabelsByDocument();

            if (!name.isNullOrEmpty())
            {
                folderWithResults = folderWithResults.Add(name, name, "Reports for cluster collection " + name);
            }

            builderForText reporter = new builderForText();

            foreach (DocumentCluster cluster in items)
            {
                cluster.Publish(labelsByDocument, documentNodeDictionary, folderWithResults, result);

                reporter.AppendPair(cluster.name, cluster.items.Count);
                reporter.AppendPair("- range", cluster.range.Range);

            }

            
            String reportPath = folderWithResults.pathFor("report.txt", imbSCI.Data.enums.getWritableFileMode.overwrite);
            String reportContent = reporter.GetContent();


            File.WriteAllText(reportPath, reportContent);
        }
    }
}