using imbSCI.Core.extensions.data;
using imbSCI.Core.math;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.math.similarity
{
    /// <summary>
    /// Static methods for word similarity computation (works with any string)
    /// </summary>
    public class setAnalysisTools<T> where T : IEquatable<T>
    {


        /// <summary>
        /// Computes word similarity
        /// </summary>
        /// <param name="wordA">The word a.</param>
        /// <param name="wordB">The word b.</param>
        /// <param name="equationEnum">The equation enum.</param>
        /// <param name="nGramSize">Size of the n gram.</param>
        /// <param name="nGramMode">The n gram mode.</param>
        /// <returns></returns>
        public Double GetSimilarity(List<T> setA, List<T> setB, nGramsSimilarityEquationEnum equationEnum = nGramsSimilarityEquationEnum.JaccardIndex)
        {


            switch (equationEnum)
            {
                default:
                case nGramsSimilarityEquationEnum.JaccardIndex:
                    return GetJaccardIndex(setA, setB);
                    break;

                case nGramsSimilarityEquationEnum.DiceCoefficient:
                    return GetDiceCoefficient(setA, setB);
                    break;

                case nGramsSimilarityEquationEnum.continualOverlapRatio:
                    return GetContinualOverlapRatio(setA, setB);
                    break;
                case nGramsSimilarityEquationEnum.KunchevaIndex:
                    return GetKunchevaIndex(setA, setB);
                    break;
            }

            return 0;
        }

        /// <summary>
        /// Ratio describes % of uninteruppted n-grams overlap. Example:  "elektromotorni", "motorski" = 5 / 14
        /// </summary>
        /// <param name="ngrams_A">The ngrams a.</param>
        /// <param name="ngrams_b">The ngrams b.</param>
        /// <returns></returns>
        public Double GetKunchevaIndex(List<T> ngrams_A, List<T> ngrams_b, Int32 datasetSize = 0)
        {
            Int32 n_n = datasetSize;

            if (n_n == 0)
            {
                List<T> dataset = new List<T>();
                dataset.AddRange(ngrams_A);
                dataset.AddRange(ngrams_b, true);
                n_n = dataset.Count;
            }

            Int32 common = ngrams_A.Count(x => ngrams_b.Contains(x));

            Int32 k_n = ngrams_A.Count;

            Double up = common * (n_n - Math.Pow(k_n, 2));
            Double down = k_n * (n_n - k_n);



            return up.GetRatio(down);
        }

        /// <summary>
        /// Ratio describes % of uninteruppted n-grams overlap. Example:  "elektromotorni", "motorski" = 5 / 14
        /// </summary>
        /// <param name="ngrams_A">The ngrams a.</param>
        /// <param name="ngrams_b">The ngrams b.</param>
        /// <returns></returns>
        public Double GetContinualOverlapRatio(List<T> ngrams_A, List<T> ngrams_b)
        {
            return Math.Max(getContinualOverlapR(ngrams_A, ngrams_b), getContinualOverlapR(ngrams_b, ngrams_A));
        }

        /// <summary>
        /// Gets the continual overlap r.
        /// </summary>
        /// <param name="A">a.</param>
        /// <param name="B">The b.</param>
        /// <returns></returns>
        private Double getContinualOverlapR(List<T> A, List<T> B)
        {
            Int32 cc = 0;

            Boolean synced = false;
            Int32 start = 0;
            for (int a_i = 0; a_i < A.Count; a_i++)
            {
                if (A[a_i].Equals(B.First()))
                {
                    start = a_i;
                    synced = true;

                    break;
                }
            }

            if (synced)
            {
                for (int i = start; i < Math.Min(A.Count, B.Count); i++)
                {
                    if (A[i].Equals(B[i]))
                    {
                        cc++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return cc.GetRatio(Math.Max(A.Count, B.Count));
        }

        /// <summary>
        /// Gets the index of the Jaccard index: number of common ngrams divided by number of total unique ngrams
        /// </summary>
        /// <param name="ngrams_A">The ngrams a.</param>
        /// <param name="ngrams_b">The ngrams b.</param>
        /// <returns></returns>
        public Double GetJaccardIndex(List<T> ngrams_A, List<T> ngrams_b)
        {
            List<T> allNGrams = new List<T>();

            Int32 common = ngrams_A.Count(x => ngrams_b.Contains(x));

            allNGrams.AddRange(ngrams_A);
            allNGrams.AddRange(ngrams_b, true);

            return common.GetRatio(allNGrams.Count);
        }

        /// <summary>
        /// Gets the index of the Dice coefficient: number of common ngrams divided by number of n-grams in both sets
        /// </summary>
        /// <param name="ngrams_A">The ngrams a.</param>
        /// <param name="ngrams_b">The ngrams b.</param>
        /// <returns></returns>
        public Double GetDiceCoefficient(List<T> ngrams_A, List<T> ngrams_b)
        {
            Int32 common = ngrams_A.Count(x => ngrams_b.Contains(x)) * 2;

            return common.GetRatio(ngrams_A.Count + ngrams_b.Count);
        }

        ///// <summary>
        ///// Gets descriptive line about n-grams deconstruction of the specified word
        ///// </summary>
        ///// <param name="word">The word to be splitted into n-grams</param>
        ///// <param name="N">Size of N-grams, e.g. for bigrams: N=2</param>
        ///// <param name="mode">The slicing mode</param>
        ///// <returns>Line used for debugging </returns>
        //public String getNGramsDescriptiveLine(String word, Int32 N = 2, nGramsModeEnum mode = nGramsModeEnum.overlap)
        //{
        //    List<T> ngrams = getNGrams(word, N, mode);

        //    String line = "[" + word + "] (" + mode.ToString() + ", N=" + N + ") => ";

        //    foreach (String ng in ngrams)
        //    {
        //        line = line.add(ng, ", ");
        //    }

        //    return line;
        //}

        /// <summary>
        /// Breaks the specified word into <c>N</c>-grams
        /// </summary>
        /// <param name="word">The word to be splitted into n-grams</param>
        /// <param name="N">Size of N-grams, e.g. for bigrams: N=2</param>
        /// <param name="mode">The slicing mode</param>
        /// <returns>Set of NGrams</returns>
        public List<List<List<T>>> getNGrams(List<List<T>> sets, Int32 N = 2, nGramsModeEnum mode = nGramsModeEnum.overlap)
        {
            List<List<List<T>>> output = new List<List<List<T>>>();

            Int32 step = 1;
            Int32 remnant = 0;
            switch (mode)
            {
                case nGramsModeEnum.overlap:
                    step = 1;
                    remnant = 1;
                    break;

                case nGramsModeEnum.ordinal:
                    step = N;
                    remnant = 0;
                    break;
            }

            if (sets.Count <= N)
            {
                output.Add(sets);
                return output;
            }

            for (int i = 0; i < sets.Count; i = i + step)
            {
                Int32 len = Math.Min(N, sets.Count - i);
                if (len > remnant)
                {
                    output.Add(sets.GetRange(i, len));
                }
            }

            return output;
        }
    }
}