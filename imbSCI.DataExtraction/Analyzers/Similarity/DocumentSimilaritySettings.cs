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

    [Serializable]
    public class DocumentSimilaritySettings
    {

        

        public nGramsSimilarityEquationEnum computationMethod { get; set; } = nGramsSimilarityEquationEnum.JaccardIndex;

        public Int32 nGramWidth { get; set; } = 2;

        public nGramsModeEnum nGramMode { get; set; } = nGramsModeEnum.overlap;

        /// <summary>
        /// XPath to be used for leaf node selection (or any node selection in customized method). Leave blank for default, <see cref="LeafNodeDictionary.DefaultNodeSelectionXPath"/>
        /// </summary>
        /// <value>
        /// The x path to select leafs.
        /// </value>
        public String XPathToSelectLeafs { get; set; } = "";

        /// <summary>
        /// HTML tag names (case is ignored) to exclude form leaf analysis. Leave empty for default, <see cref="LeafNodeDictionary.DefaultTagsToIgnore"/>
        /// </summary>
        /// <value>
        /// The tags to ignore.
        /// </value>
        public List<String> TagsToIgnore { get; set; } = new List<string>();

        public DocumentSimilaritySettings()
        {

        }

    }
}
