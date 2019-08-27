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
    public static class ListExtensions
    {

        

        /// <summary>
        /// Splits the input list into sub lists of specified size. The last in result may be less than <c>size</c>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static List<List<T>> SplitBySize<T>(this List<T> source, Int32 size)
        {
            List<List<T>> output = new List<List<T>>();

            output.Add(new List<T>());
            for (int i = 0; i < source.Count; i++)
            {
                var last = output.Last();
                if (last.Count >= size)
                {
                    output.Add(new List<T>());
                    last = output.Last();
                }
                last.Add(source[i]);
            }

            return output;
        }

        public static void RemoveDuplicates<T>(this List<T> target) where T : IEquatable<T>
        {
            List<T> toRemove = new List<T>();
            List<T> found = new List<T>();

            foreach (T n in target)
            {
                if (!found.Contains(n))
                {
                    found.Add(n);
                }
                else
                {
                    toRemove.Add(n);
                }
            }

            foreach (T n in toRemove)
            {
                target.Remove(n);
            }

        }

        /// <summary>
        /// Gets unique values from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static List<T> GetUnique<T>(this List<T> target) where T : IEquatable<T>
        {

            List<T> found = new List<T>();

            foreach (T n in target)
            {
                if (n != null)
                {
                    if (!found.Contains(n))
                    {
                        found.Add(n);
                    }
                }

            }

            return found;
        }
    }
}