using imbSCI.Core.extensions.data;
using imbSCI.Core.math;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.DataExtraction.Analyzers.Similarity.similarity
{
    /// <summary>
    /// Static methods for word similarity computation (works with any string)
    /// </summary>
    public class setAnalysisTools<T> where T : IEquatable<T>
    {

        /// <summary>
        /// Custom function to test equality
        /// </summary>
        public Func<T, T, Boolean> CustomIsEqual;

        protected Boolean AddUniqueElement(List<T> target, T needle)
        {
            
            Boolean doAdd = true;
            for (int i = 0; i < target.Count; i++)
            {
                
                if (EqualTest(target[i], needle))
                {
                    doAdd = false;
                    break;
                }

            }

            if (doAdd)
            {
                target.Add(needle);
            }
           

            return doAdd;
        }

        protected List<T> GetJoinElements(IEnumerable<T> e_target, IEnumerable<T> e_needle)
        {
            
            List<T> target = e_target.ToList();
            List<T> needle = e_needle.ToList();

            foreach (T n in needle)
            {
                AddUniqueElement(target, n);
            }
            return target;
        }


        protected List<T> GetCommonElements(IEnumerable<T> e_target, IEnumerable<T> e_needle)
        {
            List<T> output = new List<T>();
            List<T> target = e_target.ToList();
            List<T> needle = e_needle.ToList();

            for (int i = 0; i < target.Count; i++)
            {
                T inTarget = target[i];

                Int32 matchIndex = -1;
                for (int y = 0; y < needle.Count; y++)
                {
                    T inNeedle = needle[y];
                    if (EqualTest(inTarget, inNeedle))
                    {
                        matchIndex = y;
                        output.Add(inTarget);
                        break;
                    }
                }
                if (matchIndex > -1)
                {
                    needle.RemoveAt(matchIndex);
                }

            }

            return output;
        }

        protected Int32 CountContains(IEnumerable<T> e_target, IEnumerable<T> e_needle)
        {
            return GetCommonElements(e_target, e_needle).Count();
        }

        protected Boolean Contains(IEnumerable<T> target, T needle)
        {
            foreach (T inTarget in target)
            {
                if (EqualTest(inTarget, needle))
                {
                    return true;
                }
            }
            return false;
        }

        protected Boolean EqualTest(T target, T needle)
        {
            if (CustomIsEqual == null)
            {
                return target.Equals(needle);
            } else
            {
                return CustomIsEqual(target, needle);
            }
        }

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
        /// <param name="datasetSize">Size of the dataset.</param>
        /// <returns></returns>
        public Double GetKunchevaIndex(List<T> ngrams_A, List<T> ngrams_b, Int32 datasetSize = 0)
        {
            Int32 n_n = datasetSize;

            if (n_n == 0)
            {
                List<T> dataset = GetJoinElements(ngrams_A, ngrams_b);
                n_n = dataset.Count;
            }

            Int32 common = CountContains(ngrams_A, ngrams_b); // ngrams_A.Count(x => Contains(ngrams_b, x)); // ngrams_b.Contains(x));

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
                if (EqualTest(A[a_i], B.First()))
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
                    if (EqualTest(A[i], B[i]))
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
        /// <param name="ngrams_B">The ngrams b.</param>
        /// <returns></returns>
        public Double GetJaccardIndex(List<T> ngrams_A, List<T> ngrams_B)
        {
            List<T> allNGrams = GetJoinElements(ngrams_A, ngrams_B);

            Int32 common = CountContains(ngrams_A, ngrams_B); // ngrams_A.Count(x => Contains(ngrams_b, x)); // ngrams_b.Contains(x));


            return common.GetRatio(allNGrams.Count);
        }

        /// <summary>
        /// Gets the index of the Dice coefficient: number of common ngrams divided by number of n-grams in both sets
        /// </summary>
        /// <param name="ngrams_A">The ngrams a.</param>
        /// <param name="ngrams_B">The ngrams b.</param>
        /// <returns></returns>
        public Double GetDiceCoefficient(List<T> ngrams_A, List<T> ngrams_B)
        {
            Int32 common = CountContains(ngrams_A, ngrams_B) * 2; //.Count(x => Contains(ngrams_B, x)) * 2;

            return common.GetRatio(ngrams_A.Count + ngrams_B.Count);
        }

        /// <summary>
        /// Breaks the specified sequence of items into n-gram chunks
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="N">The n.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Unexpected Case</exception>
        public static List<TNGram> getNGramSet<TNGram>(List<T> items, Int32 N = 2, nGramsModeEnum mode = nGramsModeEnum.overlap) where TNGram:List<T>, new()
        {
            List<TNGram> output = new List<TNGram>();

            Int32 step = 1;
            Int32 remnant = 0;
            switch (mode)
            {
                case nGramsModeEnum.overlap:
                    {
                        step = 1;
                        remnant = 1;
                        break;
                    }
                case nGramsModeEnum.ordinal:
                    {
                        step = N;
                        remnant = 0;
                        break;
                    }

                default:
                    throw new Exception("Unexpected Case");
            }

            if (items.Count <= N)
            {
                TNGram nGram = new TNGram();
                nGram.AddRange(items);
                output.Add(nGram);
                return output;
            }

            for (int i = 0; i < items.Count; i = i + step)
            {
                Int32 len = Math.Min(N, items.Count - i);
                if (len > remnant)
                {
                     TNGram nGram = new TNGram();
                    nGram.AddRange(items.GetRange(i, len));
                    output.Add(nGram);
                }
            }
            return output;
        }

        /// <summary>
        /// Breaks the specified sequence into <c>N</c>-gram sub sequences
        /// </summary>
        /// <param name="sets">The sets.</param>
        /// <param name="N">Size of N-grams, e.g. for bigrams: N=2</param>
        /// <param name="mode">The slicing mode</param>
        /// <returns>
        /// Set of NGrams
        /// </returns>
        public List<List<List<T>>> getNGrams(List<List<T>> sets, Int32 N = 2, nGramsModeEnum mode = nGramsModeEnum.overlap)
        {
            List<List<List<T>>> output = new List<List<List<T>>>();

            Int32 step = 1;
            Int32 remnant = 0;
            switch (mode)
            {
                case nGramsModeEnum.overlap:
                    {
                        step = 1;
                        remnant = 1;
                        break;
                    }
                case nGramsModeEnum.ordinal:
                    {
                        step = N;
                        remnant = 0;
                        break;
                    }

                default:
                    throw new Exception("Unexpected Case");
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