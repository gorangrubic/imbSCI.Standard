using imbSCI.Core.extensions.data;
using imbSCI.Core.math;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.math.similarity
{
    public static class setAnalysisExtensions
    {

        /// <summary>
        /// Gets the similarity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sets">The sets.</param>
        /// <param name="equationEnum">The equation enum.</param>
        /// <returns></returns>
        public static Double GetSimilarity<T>(this IEnumerable<List<T>> sets, nGramsSimilarityEquationEnum equationEnum = nGramsSimilarityEquationEnum.JaccardIndex) where T : IEquatable<T>
        {

            List<List<T>> setList = sets.ToList();
            Double l = setList.Count;
            if (l < 2) return 0;
            if (l == 2)
            {
                return GetSimilarity<T>(setList[0], setList[1], equationEnum);
            }

            Double k = 2 / (l * (l - 1));

            Double s = 0;

            switch (equationEnum)
            {
                case nGramsSimilarityEquationEnum.continualOverlapRatio:
                case nGramsSimilarityEquationEnum.DiceCoefficient:
                    s = 1;
                    for (int i = 0; i < l - 1; i++)
                    {
                        for (int j = i + 1; j < l; j++)
                        {
                            s = s * GetSimilarity<T>(setList[i], setList[j], equationEnum);
                        }
                    }
                    break;
                case nGramsSimilarityEquationEnum.JaccardIndex:
                    for (int i = 0; i < l - 1; i++)
                    {
                        for (int j = i + 1; j < l; j++)
                        {
                            s += GetSimilarity<T>(setList[i], setList[j], equationEnum);
                        }
                    }

                    return s * k;
                    break;
                case nGramsSimilarityEquationEnum.KunchevaIndex:
                    s = 1;
                    setAnalysisTools<T> tool = new setAnalysisTools<T>();
                    List<T> completeDataset = new List<T>();
                    foreach (List<T> subset in setList)
                    {
                        completeDataset.AddRange(subset, true);
                    }

                    Int32 n_n = completeDataset.Count;


                    var pairs = tool.getNGrams(setList, 2, nGramsModeEnum.overlap);

                    foreach (var pair in pairs)
                    {
                        s = s * tool.GetKunchevaIndex(pair[0], pair[1], n_n);
                    }

                    s = s.GetRatio(pairs.Count);


                    break;
                default:
                    break;
            }

            return s;

        }

        public static Double GetSimilarity<T>(this List<T> setA, List<T> setB, nGramsSimilarityEquationEnum equationEnum = nGramsSimilarityEquationEnum.JaccardIndex) where T : IEquatable<T>
        {
            setAnalysisTools<T> tool = new setAnalysisTools<T>();
            return tool.GetSimilarity(setA, setB, equationEnum);

        }

    }
}