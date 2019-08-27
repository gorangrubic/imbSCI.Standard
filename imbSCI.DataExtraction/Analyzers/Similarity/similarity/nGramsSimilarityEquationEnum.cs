using System.Collections.Generic;

namespace imbSCI.DataExtraction.Analyzers.Similarity.similarity
{
    /// <summary>
    /// Equation to use for word to word similarity assesment
    /// </summary>
    public enum nGramsSimilarityEquationEnum
    {
#pragma warning disable CS1574 // XML comment has cref attribute 'GetJaccardIndex(List{string}, List{string})' that could not be resolved
        /// <summary>
        /// The Jaccard Index: <see cref="wordAnalysisTools.GetJaccardIndex(List{string}, List{string})"/>
        /// </summary>
        JaccardIndex,
#pragma warning restore CS1574 // XML comment has cref attribute 'GetJaccardIndex(List{string}, List{string})' that could not be resolved

#pragma warning disable CS1574 // XML comment has cref attribute 'GetDiceCoefficient(List{string}, List{string})' that could not be resolved
        /// <summary>
        /// The dice coefficient: <see cref="wordAnalysisTools.GetDiceCoefficient(List{string}, List{string})"/>
        /// </summary>
        DiceCoefficient,
#pragma warning restore CS1574 // XML comment has cref attribute 'GetDiceCoefficient(List{string}, List{string})' that could not be resolved

#pragma warning disable CS1574 // XML comment has cref attribute 'GetContinualOverlapRatio(List{string}, List{string})' that could not be resolved
        /// <summary>
        /// The get continual overlap ratio: <see cref="wordAnalysisTools.GetContinualOverlapRatio(List{string}, List{string})"/>
        /// </summary>
        continualOverlapRatio,
#pragma warning restore CS1574 // XML comment has cref attribute 'GetContinualOverlapRatio(List{string}, List{string})' that could not be resolved

        /// <summary>
        /// The kuncheva index
        /// </summary>
        KunchevaIndex,
    }
}