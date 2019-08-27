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
using imbSCI.DataExtraction.Tools;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.render;

namespace imbSCI.DataExtraction.Analyzers.Similarity
{
    /// <summary>
    /// Similarity result for two documents
    /// </summary>
    public class DocumentSimilarityResultPair:SimilarityResultPair<HtmlNode>
    {

        public Boolean IsRelatedTo(HtmlNode node)
        {
            if (itemA == node) return true;
            if (itemB == node) return true;
            return false;
        }

        public void Publish(Dictionary<HtmlNode, HtmlSourceAndUrl> documentNodeDictionary, folderNode folderWithResults, ITextRender output)
        {
            HtmlSourceAndUrl sourceA = documentNodeDictionary[itemA];
            HtmlSourceAndUrl sourceB = documentNodeDictionary[itemB];

            
            output.AppendLine("A: " + sourceA.filepath);
            output.AppendLine("B: " + sourceB.filepath);
            output.nextTabLevel();
            output.AppendLine("CS: " + ContentSimilarity.ToString("F3"));
            output.AppendLine("SS: " + StructureSimilarity.ToString("F3"));
            output.AppendLine("OS: " + OverallSimilarity.ToString("F3"));
            output.prevTabLevel();

        }

        public DocumentSimilarityResultPair()
        {

        }

        public Double ContentSimilarity { get; set; } = 0;
        public Double StructureSimilarity { get; set; } = 0;

        public override double similarity => ContentSimilarity;

        public Double OverallSimilarity
        {
            get
            {
                return ContentSimilarity * StructureSimilarity;
            }
        }
    }
}