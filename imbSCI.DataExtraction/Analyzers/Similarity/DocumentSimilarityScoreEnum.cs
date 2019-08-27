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
    [Flags]
    public enum DocumentSimilarityScoreEnum
    {
        undefined=0,
        structure=1,
        content=2,
        combined=4,
        higherComponent=8,
        searchBestFit = structure | content | combined | higherComponent
    }
}