using System.Collections.Generic;

namespace imbSCI.Core.math.similarity
{
    /// <summary>
    /// Equation to use for word to word similarity assesment
    /// </summary>
    public enum nGramsSimilarityEquationEnum
    {
        /// <summary>
        /// The Jaccard Index: <see cref="wordAnalysisTools.GetJaccardIndex(List{string}, List{string})"/>
        /// </summary>
        JaccardIndex,

        /// <summary>
        /// The dice coefficient: <see cref="wordAnalysisTools.GetDiceCoefficient(List{string}, List{string})"/>
        /// </summary>
        DiceCoefficient,

        /// <summary>
        /// The get continual overlap ratio: <see cref="wordAnalysisTools.GetContinualOverlapRatio(List{string}, List{string})"/>
        /// </summary>
        continualOverlapRatio,

        /// <summary>
        /// The kuncheva index
        /// </summary>
        KunchevaIndex,
    }
}