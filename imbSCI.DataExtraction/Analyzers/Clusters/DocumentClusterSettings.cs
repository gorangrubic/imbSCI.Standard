using HtmlAgilityPack;
using imbSCI.Core.math.range.finder;
using imbSCI.DataExtraction.Analyzers.Similarity.similarity;
using imbSCI.DataExtraction.Analyzers.Similarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Analyzers.Clustersters
{
    [Serializable]
    public class DocumentClusterSettings
    {

        public DocumentClusterSettings()
        {

        }



        public DocumentSimilarityScoreEnum SimilarityScoreSource { get; set; } = DocumentSimilarityScoreEnum.searchBestFit;

        /// <summary>
        /// If true - document can be assigned only to one cluster, not more
        /// </summary>
        /// <value>
        ///   <c>true</c> if [exclusive cluster membership]; otherwise, <c>false</c>.
        /// </value>
        public Boolean ExclusiveClusterMembership { get; set; } = true;


        public Int32 TargetClusterCount { get; set; } = -1;

        public rangeFinder GetMinSimilarityRange()
        {
            rangeFinder minSimilarityRange = new rangeFinder();
            minSimilarityRange.Learn(MinScoreInRangeCriterion);
            minSimilarityRange.Learn(MinScoreInRangeMaxCriterion);
            return minSimilarityRange;
        }

        public Int32 TargetSearchSteps { get; set; } = 20;

        public Double MinScoreInRangeCriterion { get; set; } = 0.00;


        public Double MinScoreInRangeMaxCriterion { get; set; } = 1.00;
    }
}