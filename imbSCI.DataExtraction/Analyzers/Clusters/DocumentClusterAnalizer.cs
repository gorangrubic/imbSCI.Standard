using HtmlAgilityPack;
using imbSCI.Core.math.range.finder;
using imbSCI.DataExtraction.Analyzers.Similarity.similarity;
using imbSCI.DataExtraction.Analyzers.Similarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using imbSCI.DataExtraction.Tools;
using imbSCI.Core.reporting.render;

namespace imbSCI.DataExtraction.Analyzers.Clustersters
{
  //public class DocumentClusterMethod { }

    public class DocumentClusterAnalizer
    {
        public DocumentClusterAnalizer()
        {

        }

        public DocumentClusterSettings settings { get; set; } = new DocumentClusterSettings();


        /// <summary>
        /// Stores document sources for each cluster
        /// </summary>
        /// <param name="clusters">The clusters.</param>
        /// <param name="documentNodeDictionary">The document node dictionary.</param>
        /// <returns></returns>
        public List<HtmlSourceAndUrlCollection> ConvertToSourceCollections(DocumentClusterCollection clusters, Dictionary<HtmlNode, HtmlSourceAndUrl> documentNodeDictionary)
        {
            List<HtmlSourceAndUrlCollection> output = new List<HtmlSourceAndUrlCollection>();

            foreach (var cluster in clusters.GetClusters<DocumentCluster>(false))
            {
                HtmlSourceAndUrlCollection sourceCollection = new HtmlSourceAndUrlCollection();
                sourceCollection.name = cluster.name;

                foreach (var document in cluster.items)
                {
                    sourceCollection.items.Add(documentNodeDictionary[document]);
                }
                output.Add(sourceCollection);
            }

            return output;
        }


        /// <summary>
        /// Gets the clusters by target.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="output">The output.</param>
        /// <param name="scoreSelector">The score selector.</param>
        /// <returns></returns>
        public DocumentClusterCollection GetClustersByTarget(DocumentSimilarityResult result, String collectionName = "Clusters", ITextRender output = null)
        {
            List<DocumentClusterCollection> candidates = new List<DocumentClusterCollection>();

            List<DocumentClusterCollection> others = new List<DocumentClusterCollection>();

            

            List<Func<DocumentSimilarityResultPair, double>> scoreSelectors = settings.SimilarityScoreSource.GetSelectorList();


            if (settings.TargetClusterCount > 0)
            {
                var minSimRange = settings.GetMinSimilarityRange();

                var minSimScores = minSimRange.GetValueRangeZigZagSteps(settings.TargetSearchSteps);

                Int32 si = 0;
                foreach (var minSimScore in minSimScores)
                {
                    si++;

                    foreach (var scoreSelector in scoreSelectors)
                    {

                        DocumentClusterCollection currentClusters = GetClusters(result, collectionName, scoreSelector, minSimScore);

                        if (output != null) output.AppendLine($"Clusterization iteration {si}/{minSimScores.Count} with minSimScore: {minSimScore} : Clusters[{currentClusters.Count}] - NullCluster[{currentClusters.NullCluster.items.Count}]");

                        if (currentClusters.Count == settings.TargetClusterCount)
                        {
                            if (currentClusters.NullCluster.items.Any())
                            {
                                candidates.Add(currentClusters);
                            }
                            else
                            {
                                if (output != null) output.AppendLine($"Match found _{si}/{minSimScores.Count}_ with minSimScore: {minSimScore} : Clusters[{currentClusters.Count}] - NullCluster[{currentClusters.NullCluster.items.Count}]");
                                return currentClusters;
                            }
                        }
                        else
                        {
                            others.Add(currentClusters);
                        }
                    }
                    
                }

                if (candidates.Any())
                {
                    return candidates.FirstOrDefault();
                }

                var sorted = others.OrderBy(x => Math.Abs(settings.TargetClusterCount - x.Count));
                return sorted.FirstOrDefault();

            }


            return GetClusters(result, collectionName, scoreSelectors.First(), settings.MinScoreInRangeCriterion);
        }



        /// <summary>
        /// Gets cluster collection
        /// </summary>
        /// <param name="collectionName">Name for the collection.</param>
        /// <param name="result">The result.</param>
        /// <param name="scoreSelector">The score selector.</param>
        /// <returns></returns>
        public DocumentClusterCollection GetClusters(DocumentSimilarityResult result, String collectionName="Clusters", Func<DocumentSimilarityResultPair, Double> scoreSelector=null, Double minSimScore=Double.MinValue)
        {
            if (minSimScore== Double.MinValue)
            {
                minSimScore = settings.MinScoreInRangeCriterion;
            }

            if (scoreSelector == null)
            {
                scoreSelector = settings.SimilarityScoreSource.GetSelector();
            }

            DocumentClusterCollection output = new DocumentClusterCollection()
            {
                name = collectionName
            };

            var documents = result.GetDocuments();
            var sortedResults = result.GetAllResults().OrderByDescending(x => scoreSelector).ToList();

            rangeFinder similarityRange = new rangeFinder();
            foreach (var pair in sortedResults)
            {
                similarityRange.Learn(scoreSelector(pair));
            }


            Int32 limit = documents.Count;
            Int32 i = 0;

            while (documents.Any())
            {
                i++;
                var doc = documents.FirstOrDefault();
                if (doc == null)
                {
                    break;
                }

                var results = result.GetResultsFor(doc);

                DocumentCluster currentCluster = output.NewCluster<DocumentCluster>(); //new DocumentCluster();
                currentCluster.ClusterSeed = doc;

                foreach (KeyValuePair<HtmlNode, DocumentSimilarityResultPair> pair in results)
                {
                    Double scoreAtRange = similarityRange.GetPositionInRange(scoreSelector(pair.Value));
                    if (scoreAtRange > minSimScore)
                    {
                        currentCluster.Add(pair.Key, scoreAtRange);
                        documents.Remove(pair.Key);
                    }
                }

                if (currentCluster.items.Count == 0)
                {
                    output.NullCluster.Add(doc, 0);
                    documents.Remove(doc);
                } else
                {
                    documents.Remove(doc);
                    currentCluster.items.Add(doc);
                    output.AddCluster(currentCluster);
                }

                if (i>limit)
                {
                    break;
                }
            }

            foreach (var item in output.NullCluster.items)
            {
                var results = result.GetResultsFor(item);
                Double maxScore = Double.MinValue;
                DocumentCluster selectedCluster = null;

                foreach (var cluster in output.GetClusters<DocumentCluster>(false))
                {
                    Double score = scoreSelector(results[cluster.ClusterSeed]);
                    if (score > maxScore)
                    {
                        maxScore = score;
                        selectedCluster = cluster;
                    }
                }

                if (similarityRange.GetPositionInRange(maxScore) > minSimScore)
                {
                    selectedCluster.Add(item, maxScore);
                    output.NullCluster.Remove(item);
                } else
                {

                }

            }



            if (settings.ExclusiveClusterMembership)
            {
                var itemToCluster = output.GetItemToClusterAssociations<DocumentCluster>();

                foreach (var pair in itemToCluster)
                {
                    if (pair.Value.Count > 1)
                    {
                        Dictionary<HtmlNode, DocumentSimilarityResultPair> results = result.GetResultsFor(pair.Key);
                        Double maxScore = Double.MinValue;
                        DocumentCluster selectedCluster = null;

                        foreach (var cluster in pair.Value)
                        {
                            Double score = scoreSelector(results[cluster.ClusterSeed]);
                            if (score > maxScore)
                            {
                                maxScore = score;
                                selectedCluster = cluster;
                            }
                        }

                        foreach (var cluster in pair.Value)
                        {
                            if (cluster != selectedCluster)
                            {
                                cluster.Remove(pair.Key);
                            }
                        }

                    }
                }

            }

            output.RemoveEmptyClusters();


            return output;

        }
    }
}
