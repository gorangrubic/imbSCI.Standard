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
using imbSCI.Core.extensions.text;
using imbSCI.Core.reporting.render;
using System.Threading.Tasks;
using imbSCI.Core.extensions.data;
using imbSCI.Core.math.range.frequency;

namespace imbSCI.DataExtraction.Analyzers.Similarity
{
    public class ComputeSimilarityTask
    {
        public HtmlNode documentA { get; set; }
        public HtmlNode documentB { get; set; }

        public List<LeafNodeDictionaryEntryNGram> nGrams_A { get; set; }

        public List<LeafNodeDictionaryEntryNGram> nGrams_B { get; set; }

        public DocumentSimilarityResultPair output { get; set; }
    }
}