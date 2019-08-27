using imbSCI.Core.collection;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace imbSCI.Core.extensions.data
{
public static class DictionaryExtensions
    {

        /// <summary>
        /// Searches dictionary by specified needle and comparison options
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="needle">The needle.</param>
        /// <param name="comparison">The comparison.</param>
        /// <param name="IsSearchPattern">If needle is a search pattern (with * willcard[s]) </param>
        /// <returns></returns>
        public static List<T> Search<T>(this Dictionary<String, T> source, String needle, StringComparison comparison, Boolean IsSearchPattern = false)
        {
            List<T> output = new List<T>();
            List<String> keys = source.Keys.ToList();

            if (IsSearchPattern) {
                Regex needleRegex = needle.SearchPatternToRegex();
                foreach (var k in keys)
                {
                    if (needleRegex.IsMatch(k))
                    {
                        output.Add(source[k]);
                    }
                }
            } else
            {
                foreach (var k in keys)
                {
                    if (needle.Equals(k, comparison))
                    {
                        output.Add(source[k]);
                    }
                }
            }


            return output;
        }
    }
}