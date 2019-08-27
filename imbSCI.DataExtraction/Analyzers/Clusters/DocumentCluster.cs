using HtmlAgilityPack;
using imbSCI.Core.math.range.finder;
using imbSCI.DataExtraction.Analyzers.Similarity.similarity;
using imbSCI.DataExtraction.Analyzers.Similarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.render.builders;
using imbSCI.DataExtraction.Tools;
using System.IO;
using imbSCI.Data.interfaces;

namespace imbSCI.DataExtraction.Analyzers.Clustersters
{
    [Serializable]
    public class DocumentCluster:ItemCluster<HtmlNode>, IObjectWithNameWeightAndType
    {
        /// <summary>
        /// Represents average similarity
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double weight
        {
            get
            {
                if (name == ItemCluster<HtmlNode>.NAMEFOR_NOTCLUSTERED_ITEMS) return 0;
                return range.Average;
            }
            set
            {

            }
        }
        /// <summary>
        /// 0 - not clustered, 1 - a cluster
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public int type
        {
            get
            {
                if (name == ItemCluster<HtmlNode>.NAMEFOR_NOTCLUSTERED_ITEMS) return 0;
                return 1;
            }
            set
            {

            }
        }

        public void Publish(Dictionary<HtmlNode, String> labelsByDocument,Dictionary<HtmlNode, HtmlSourceAndUrl> documentNodeDictionary, folderNode folderWithResults, DocumentSimilarityResult result)
        {
            
            var cluster = this;
             folderNode cFolder = folderWithResults.Add(cluster.name, cluster.name, "Directory for cluster " + cluster.name);
                result.Publish(documentNodeDictionary, cFolder, cluster.items);

            builderForText reporter = new builderForText();
                reporter.AppendHeading("Name: " + cluster.name);
                reporter.AppendPair("Items", cluster.items.Count);

                if (cluster.ClusterSeed != null)
                {
                    reporter.AppendPair("Seed", labelsByDocument[cluster.ClusterSeed]);
                }
                foreach (var pair in cluster.range.GetDictionary())
                {
                    reporter.AppendPair(pair.Key, pair.Value.ToString("F3"));
                }

                foreach (var item in cluster.items)
                {
                    if (item != cluster.ClusterSeed)
                    {
                        if (cluster.scoreDictionary.ContainsKey(item))
                        {

                            String label = labelsByDocument[item];
                            Double score = cluster.scoreDictionary[item];
                            HtmlSourceAndUrl source = documentNodeDictionary[item];
                            reporter.AppendLine("-----------------------------------");
                            reporter.AppendLine(label + " => " + score.ToString("F3"));
                            reporter.AppendLine("Filepath: " + source.filepath);
                            reporter.AppendLine("Url: " + source.url);
                        }
                    }
                }

                String reportPath = cFolder.pathFor("report.txt", imbSCI.Data.enums.getWritableFileMode.overwrite);
                String reportContent = reporter.GetContent();

                File.WriteAllText(reportPath, reportContent);
        }


        public DocumentCluster()
        {

        }

    }
}