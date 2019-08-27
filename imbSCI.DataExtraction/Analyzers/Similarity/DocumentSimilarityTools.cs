using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
// imbSCI.DataExtraction.Analyzers.Similarity.similarity;
using imbSCI.DataExtraction.Analyzers.Similarity.similarity;
using imbSCI.Core.math.range;
using imbSCI.Core.math.range.finder;
using HtmlAgilityPack;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.Data;

namespace imbSCI.DataExtraction.Analyzers.Similarity
{
/// <summary>
    /// 
    /// </summary>
    public static class DocumentSimilarityTools
    {

        /// <summary>
        /// Gets the selector.
        /// </summary>
        /// <param name="scoreSource">The score source.</param>
        /// <returns></returns>
        public static List<Func<DocumentSimilarityResultPair, double>> GetSelectorList(this DocumentSimilarityScoreEnum scoreSource)
        {
            List<Func<DocumentSimilarityResultPair, double>> scoreSelectors = new List<Func<DocumentSimilarityResultPair, double>>();
            switch (scoreSource)
            {
                case DocumentSimilarityScoreEnum.searchBestFit:
                    scoreSelectors.Add(DocumentSimilarityScoreEnum.structure.GetSelector());
                    scoreSelectors.Add(DocumentSimilarityScoreEnum.higherComponent.GetSelector());
                    scoreSelectors.Add(DocumentSimilarityScoreEnum.combined.GetSelector());
                    scoreSelectors.Add(DocumentSimilarityScoreEnum.content.GetSelector());
                    break;
                default:
                    scoreSelectors.Add(scoreSource.GetSelector());
                    break;
            }

            return scoreSelectors;
        }

        /// <summary>
        /// Gets the selector.
        /// </summary>
        /// <param name="scoreSource">The score source.</param>
        /// <returns></returns>
        public static Func<DocumentSimilarityResultPair, double> GetSelector(this DocumentSimilarityScoreEnum scoreSource)
        {
            Func<DocumentSimilarityResultPair, double> scoreSelector = null;
            switch (scoreSource)
            {
                case DocumentSimilarityScoreEnum.combined:
                    scoreSelector = new Func<DocumentSimilarityResultPair, double>(x => x.OverallSimilarity);
                    break;
                case DocumentSimilarityScoreEnum.content:
                    scoreSelector = new Func<DocumentSimilarityResultPair, double>(x => x.ContentSimilarity);
                    break;
                case DocumentSimilarityScoreEnum.higherComponent:
                    scoreSelector = new Func<DocumentSimilarityResultPair, double>(x => Math.Max(x.ContentSimilarity, x.StructureSimilarity));
                    break;
                default:
                case DocumentSimilarityScoreEnum.structure:
                    scoreSelector = new Func<DocumentSimilarityResultPair, double>(x => x.StructureSimilarity);
                    break;
                
                    break;
            }

            return scoreSelector;
        }
    }
}