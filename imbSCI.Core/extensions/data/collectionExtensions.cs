// --------------------------------------------------------------------------------------------------------------------
// <copyright file="collectionExtensions.cs" company="imbVeles" >
//
// Copyright (C) 2018 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// <summary>
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

/// <summary>
/// Comprehensive collection of extensions
/// </summary>
/// <remarks>
/// Covered: collection transformations, strings, files, types, Data structures, enums etc
/// </remarks>
namespace imbSCI.Core.extensions.data
{
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
    /// <summary>
    /// Ekstenzije za manipulaciju kolekcijama
    /// </summary>
    public static class collectionExtensions
    {
        public static Regex extractNumberRegex = new Regex("([\\d]+)[\\s]?");

        public static List<String> extractNumbers(this String source)
        {
            List<String> output = new List<string>();

            MatchCollection mchCol = extractNumberRegex.Matches(source);
            foreach (Match mch in mchCol)
            {
                output.Add(mch.Value);
            }
            return output;
        }

        public static Dictionary<String, List<T>> GetAllignedByName<T>(this IEnumerable<T> input, Func<T, String> nameSelector) where T : class
        {
            Dictionary<String, List<T>> output = new Dictionary<string, List<T>>();

            foreach (T item in input)
            {
                String n = nameSelector(item);
                if (!output.ContainsKey(n)) output.Add(n, new List<T>());
                output[n].Add(item);
            }



            return output;
        }


        public static Dictionary<String, List<T>> GetAllignedByName<T>(this IEnumerable<IEnumerable<T>> input, Func<T, String> nameSelector) where T : class
        {
            Dictionary<String, List<T>> output = new Dictionary<string, List<T>>();

            foreach (var items in input)
            {
                foreach (T item in items)
                {
                    String n = nameSelector(item);
                    if (!output.ContainsKey(n)) output.Add(n, new List<T>());
                    output[n].Add(item);
                }
            }


            return output;
        }

        public static Dictionary<String, List<T>> GetAllignedByKey<T>(this IEnumerable<KeyValuePair<String, IEnumerable<T>>> input) where T : class
        {
            Dictionary<String, List<T>> output = new Dictionary<string, List<T>>();

            foreach (var items in input)
            {

                foreach (T item in items.Value)
                {

                    if (!output.ContainsKey(items.Key)) output.Add(items.Key, new List<T>());
                    output[items.Key].Add(item);
                }
            }


            return output;
        }

        //public static List<String> extract(this String path)
        //{
        //    String[] lines = File.ReadAllLines(path);
        //    foreach (String ln in lines)
        //    {
        //        List<String> numers = ln.extractNumbers();
        //        String reformatLine = "";
        //        foreach (String num in numers)
        //        {
        //            reformatLine = reformatLine + "[" + num + "] ";
        //        }
        //        reformatLine = reformatLine.Trim();

        //        Console.WriteLine(reformatLine);
        //    }

        //}

        public static T getEntrySafe<TKey, T>(this IDictionary<TKey, T> source, TKey key, T defaultReturn = default(T))
        {
            if (source.ContainsKey(key))
            {
                return (T)source[key];
            }
            else
            {
                return defaultReturn;
            }
        }

        public static T getEntrySafe<T>(this IDictionary source, Object key, T defaultReturn = default(T))
        {
            if (source.Contains(key))
            {
                return (T)source[key];
            }
            else
            {
                return defaultReturn;
            }
        }

        /// <summary>
        /// Pokes the specified dictionary -- fake method just to trigger lazy initialization
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        public static void Poke(this IDictionary dict)
        {
            //
        }

        private static Random rng = new Random();

        /// <summary>
        /// Randomizes order of items in the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        public static void Randomize<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Returns a new list where the elements are randomly shuffled.
        /// Based on the Fisher-Yates shuffle, which has O(n) complexity.
        /// </summary>
        public static List<T> RandomizeToList<T>(this IEnumerable<T> list)
        {
            var source = list.ToList();
            int n = source.Count;
            var shuffled = new List<T>(n);
            shuffled.AddRange(source);
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = shuffled[k];
                shuffled[k] = shuffled[n];
                shuffled[n] = value;
            }
            return shuffled;
        }


        /// <summary>
        /// Universal method for getting element out of unknown <see cref="IEnumerable" />
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="index">The index.</param>
        /// <param name="doException">if set to <c>true</c> [do exception].</param>
        /// <returns>
        /// null if <c>index</c> is out of range
        /// </returns>
        public static Object imbGetItemAt(this IEnumerable source, Int32 index, Boolean doException = true)
        {
            Object output = null;
            IEnumerator en = source.GetEnumerator();
            for (int i = 0; i <= index; i++)
            {
                if (!en.MoveNext())
                {
                    if (doException)
                    {
                        throw new ArgumentOutOfRangeException("index", "Seems that index is out of range of source IEnumerable");
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return en.Current;
        }

        /// <summary>
        /// Gets index of item in an unknown enumeration
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="needle">The needle.</param>
        /// <param name="doException">if set to <c>true</c> [do exception].</param>
        /// <param name="searchLimit">The search limit.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">needle - Search exceeded specified searchLimit</exception>
        public static Int32 imbGetIndexOf(this IEnumerable source, Object needle, Boolean doException = true, Int32 searchLimit = 10000)
        {
            IEnumerator en = source.GetEnumerator();
            Boolean run = true;
            Int32 c = 0;
            do
            {
                if (en.Current == needle)
                {
                    return c;
                }

                if (en.MoveNext())
                {
                }
                else
                {
                    return -1;
                }
                c++;

                if (c > searchLimit)
                {
                    if (doException)
                    {
                        throw new ArgumentOutOfRangeException("needle", "Search exceeded specified searchLimit");
                        return -1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            } while (run);
            return -1;
        }

        /// <summary>
        /// Makes a new Key for the dictionary by using proposal + _ count
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="proposal">The proposal.</param>
        /// <param name="lastIteration">The last iteration.</param>
        /// <param name="iterationLimit">The iteration limit - when to give up</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">iterationLimit - Failed to make name from proposal [" + proposal + "] after [" + c.ToString() + "] iterations!</exception>
        /// <exception cref="Exception">Failed to make name from proposal [" + proposal + "] after [" + c.ToString() + "] iterations!</exception>
        public static string makeUniqueDictionaryName(this IDictionary dictionary, String proposal, ref Int32 lastIteration, Int32 iterationLimit = 10)
        {
            String output = proposal;
            Int32 c = lastIteration;

            while (dictionary.Contains(output))
            {
                c++;
                output = proposal + "_" + c.ToString();
                if (c > (iterationLimit + lastIteration))
                {
                    throw new ArgumentOutOfRangeException("iterationLimit", "Failed to make name from proposal [" + proposal + "] after [" + c.ToString() + "] iterations!");
                }
            }
            lastIteration = c;
            return output;
        }

        public static List<TTarget> ConvertToList<TTarget>(this IEnumerable source)
        {
            List<TTarget> output = new List<TTarget>();
            foreach (var o in source)
            {
                TTarget ot = (TTarget)o;
                output.Add(ot);
            }
            return output;
        }

        public static List<TTarget> ConvertList<TSource, TTarget>(this IEnumerable<TSource> source) where TTarget : TSource
        {
            List<TTarget> output = new List<TTarget>();
            source.ToList().ForEach(x => output.Add((TTarget)x));
            return output;
        }

        /// <summary>
        /// Converts an IList to List
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static List<TTarget> ConvertIList<TSource, TTarget>(this IEnumerable<TSource> source) where TSource : TTarget
        {
            List<TTarget> output = new List<TTarget>();
            source.ToList().ForEach(x => output.Add((TTarget)x));
            return output;
        }

        public static int CompareKeyValuePairs<T>(KeyValuePair<T, Int32> a, KeyValuePair<T, Int32> b)
        {
            return a.Value.CompareTo(b.Value);
        }

        public static int CompareKeyValuePairs<T>(KeyValuePair<T, Double> a, KeyValuePair<T, Double> b)
        {
            return a.Value.CompareTo(b.Value);
        }

        public static T getFirstFromDictionary<T>(this IDictionary<String, T> dict)
        {
            foreach (KeyValuePair<String, T> pair in dict)
            {
                return pair.Value;
            }

            return default(T);
        }

        public static List<T> toSortedList<T>(this IDictionary<T, Double> dict)
        {
            List<T> sout = new List<T>();

            List<KeyValuePair<T, Double>> pairs = new List<KeyValuePair<T, Double>>();

            foreach (KeyValuePair<T, Double> pair in dict)
            {
                pairs.Add(pair);
            }

            pairs.Sort(collectionExtensions.CompareKeyValuePairs);

            foreach (KeyValuePair<T, Double> k in pairs)
            {
                sout.Add(k.Key);
            }

            return sout;
        }

        /// <summary>
        /// To the sorted list of Keys from <see cref="IDictionary"/> where Keys are instances and Values are number fo sort by
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <returns>List with instances, sorted by value</returns>
        public static List<T> toSortedList<T>(this IDictionary<T, Int32> dict)
        {
            List<T> sout = new List<T>();

            List<KeyValuePair<T, Int32>> pairs = new List<KeyValuePair<T, int>>();

            foreach (KeyValuePair<T, Int32> pair in dict)
            {
                pairs.Add(pair);
            }

            pairs.Sort(collectionExtensions.CompareKeyValuePairs);

            foreach (KeyValuePair<T, Int32> k in pairs)
            {
                sout.Add(k.Key);
            }

            return sout;
        }

        /// <summary>
        /// To the sorted list of Keys from <see cref="IDictionary"/> where Keys are instances and Values are number fo sort by
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <returns>List with instances, sorted by value</returns>
        public static List<KeyValuePair<T, Int32>> toSortedKeyValueList<T>(this IDictionary<T, Int32> dict)
        {
            List<KeyValuePair<T, Int32>> pairs = new List<KeyValuePair<T, Int32>>();

            foreach (KeyValuePair<T, Int32> pair in dict)
            {
                pairs.Add(pair);
            }

            pairs.Sort(collectionExtensions.CompareKeyValuePairs);

            return pairs;
        }

        /// <summary>
        /// Pravi List instancu i ubacuje ovaj objekat kao prvi element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        /// \ingroup_disabled ace_ext_collections
        public static List<T> toListWithThisOne<T>(this T item)
        {
            List<T> output = new List<T>();
            if (item != null) output.Add(item);
            return output;
        }

        public static T[] toArrayWithThisOne<T>(this T item)
        {
            T[] output = new T[] { item };

            return output;
        }

        public static T Pop<T>(this IList source)
        {
            if (source.Count == 0)
            {
                return default(T);
            }
            T output = (T)source[0];
            source.Remove(output);
            return output;
        }

        /// <summary>
        /// Universal counting
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Int32 count(this IEnumerable source)
        {
            if (source is IList)
            {
                IList source_IList = (IList)source;
                return source_IList.Count;
            }

            if (source is IDictionary)
            {
                IDictionary input_IDictionary = (IDictionary)source;
                return input_IDictionary.Count;
            }

            if (source is PropertyCollection)
            {
                PropertyCollection source_PropertyCollection = (PropertyCollection)source;
                return source_PropertyCollection.Count;
            }

            if (source is ICollection)
            {
                ICollection source_ICollection = (ICollection)source;
                return source_ICollection.Count;
            }

            Int32 c = 0;

            foreach (Object obj in source)
            {
                c++;
            }
            return c;
        }

        //public static void AddRange<T>(this ObservableCollection<T> target, IEnumerable<T> items, Boolean onlyUniqueValues = true)
        //{
        //    foreach (T pt in items)
        //    {
        //        if (onlyUniqueValues)
        //        {
        //            if (!target.Contains(pt))
        //            {
        //                target.Add(pt);
        //            }
        //        }
        //        else
        //        {
        //            target.Add(pt);
        //        }
        //    }
        //}

        /// <summary>
        /// Adds (optionally: unique) values from the specified range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="items">The items.</param>
        /// <param name="onlyUniqueValues">if set to <c>true</c> it will add only unique values.</param>
        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> items, Boolean onlyUniqueValues = true)
        {
            foreach (T pt in items)
            {
                if (onlyUniqueValues)
                {
                    if (!target.Contains(pt))
                    {
                        target.Add(pt);
                    }
                }
                else
                {
                    target.Add(pt);
                }
            }
        }

        /// <summary>
        /// Adds (optionally: unique) values from the specified range:
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="items">The items.</param>
        public static void AddRange<T>(this Stack<T> target, IEnumerable<T> items, Boolean onlyUniqueValues = true)
        {
            foreach (T pt in items)
            {
                if (onlyUniqueValues)
                {
                    if (!target.Contains(pt))
                    {
                        target.Push(pt);
                    }
                }
                else
                {
                    target.Push(pt);
                }
            }
        }

        /// <summary>
        /// Gets the count of Boolean items that has value same as <c>valueToCount</c>
        /// </summary>
        /// <param name="checkList">The check list.</param>
        /// <param name="valueToCount">What value to count</param>
        /// <returns></returns>
        public static Int32 getCountOf(this IEnumerable<Boolean> checkList, Boolean valueToCount = true)
        {
            Int32 output = 0;
            foreach (Boolean chk in checkList)
            {
                if (chk == valueToCount)
                {
                    output++;
                }
            }
            return output;
        }

        /// <summary>
        /// Gets the and remove all if any exists
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public static Boolean getAndRemoveAllIfExists(this List<Object> source, params Enum[] keys)
        {
            List<Object> keyList = keys.getFlatList<Object>();

            Boolean output = false;
            List<Object> args = source.Where(x => keyList.Contains(x)).ToList();
            foreach (Object arg in args)
            {
                source.Remove(arg);
                output = true;
            }
            return output;
        }

        /// <summary>
        /// Returns TRUE and removes the first key found in source list
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public static Boolean getAndRemoveIfExists(this List<Object> source, params Enum[] keys)
        {
            List<Object> keyList = keys.getFlatList<Object>();

            List<Object> args = source.Where(x => keyList.Contains(x)).ToList();
            foreach (Object arg in args)
            {
                source.Remove(arg);
                return true;
            }
            return false;
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'aceGeneralException' that could not be resolved
        /// <summary>
        /// Gets the first instance that <c>is</c> compatibile with <c>T</c> in supplied collection. Supports PropertyCollection and other collections
        /// </summary>
        /// <typeparam name="T">Type to test against using <c>is</c> keyword</typeparam>
        /// <param name="source">The source collection to search in</param>
        /// <param name="makeNewIfNotFound">if set to <c>true</c> [make new if not found].</param>
        /// <param name="defaultReturn">The default return.</param>
        /// <param name="disableException">if set to <c>true</c> it will not throw exception if item not found</param>
        /// <returns>
        /// Instance of type
        /// </returns>
        /// <exception cref="aceGeneralException">Resource of type " + typeof(T).Name + " not supplied in the resources collection</exception>
        /// <remarks>
        /// Uses <c>is</c> keyword to test.
        /// </remarks>
        public static T getFirstOfType<T>(this IEnumerable source, Boolean makeNewIfNotFound = false, Object defaultReturn = null, Boolean disableException = false)
#pragma warning restore CS1574 // XML comment has cref attribute 'aceGeneralException' that could not be resolved
        {
            if (source is PropertyCollection)
            {
                PropertyCollection sc = source as PropertyCollection;
                source = sc.Values;
            }

            // List<Object> flat = source.getFlatList<Object>();

            foreach (Object chk in source)
            {
                if (chk is T)
                {
                    return (T)chk;
                }
            }

            if (makeNewIfNotFound)
            {
                return (T)typeof(T).GetDefaultValue(); //new T();
            }

            if (defaultReturn is T)
            {
                return (T)defaultReturn;
            }
            else
            {
                if (!disableException) throw new ArgumentException("T", "Resource of type " + typeof(T).Name + " not supplied in the resources collection");

                return (T)typeof(T).GetDefaultValue();
            }
        }

        /// <summary>
        /// Gets the first instance of type in supplied collection. <c>makeNewIfNotFound</c>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source collection to search in</param>
        /// <returns>Instance of type</returns>
        public static List<T> getAllOfType<T>(this IEnumerable source, Boolean makeNewIfNotFound = false)
        {
            List<Object> flat = source.getFlatList<Object>();
            List<T> output = new List<T>();

            foreach (Object chk in flat)
            {
                if (chk is T)
                {
                    output.Add((T)chk);
                }
            }

            if (makeNewIfNotFound)
            {
                if (output.Count == 0)
                {
                    output.Add(default(T));
                }
            }

            return output;
        }

        /// <summary>
        /// Gathers all enumerations found in the collection
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="excludeEnumNamedNone">if set to <c>true</c> [exclude enum named none].</param>
        /// <returns></returns>
        public static List<Enum> getAllEnums(this IEnumerable source, Boolean excludeEnumNamedNone = false)
        {
            List<Enum> flat = source.getFlatList<Enum>();
            List<Enum> output = new List<Enum>();

            foreach (Object chk in flat)
            {
                if (chk is Enum)
                {
                    if (excludeEnumNamedNone)
                    {
                        String name = chk.toStringSafe("none");
                        if (name == "none")
                        {
                            //
                        }
                        else
                        {
                            output.Add((Enum)chk);
                        }
                    }
                    else
                    {
                        output.Add((Enum)chk);
                    }
                }
            }

            return output;
        }

#pragma warning disable CS1570 // XML comment has badly formed XML -- 'Expected an end tag for element 'example'.'
#pragma warning disable CS1570 // XML comment has badly formed XML -- 'End tag 'example' does not match the start tag 'Enum'.'
        /// <summary>
        /// Makes flaf array from once or multiply nested arrays - to be used with <c>params</c> method parameter in order to support both <c>params</c> and <c>Array</c> calls.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <example>
        ///  public static Object getProperField(this PropertyCollection source, params Enum[] fields)
        /// {
        ///    fields = fields.getFlatArray<Enum>();
        ///    }
        /// </example>
        /// \ingroup_disabled ace_ext_collections
        public static T[] getFlatArray<T>(this IEnumerable source, params Type[] keepUnFlat)
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'End tag 'example' does not match the start tag 'Enum'.'
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'Expected an end tag for element 'example'.'
        {
            List<T> output = new List<T>();
            String keepOut = "";
            //List<Type> keep = keepUnFlat.getFlatList<Type>();
            if (!keepUnFlat.Any())
            {
                keepOut = "PropertyCollection";
            }
            Boolean banFlatting = false;
            if (source == null) return output.ToArray();
            foreach (T en in source)
            {
                if (en is IList || en is Array)
                {
                    banFlatting = keepUnFlat.Contains(en.GetType());
                    IEnumerable enl = en as IEnumerable;

                    if (!banFlatting)
                    {
                        banFlatting = enl.GetType().Name.StartsWith(keepOut);
                    }

                    if (!banFlatting)
                    {
                        output.AddRange(enl.getFlatList<T>(keepUnFlat));
                    }
                    else
                    {
                        if (enl is T) output.Add(en);
                    }
                }
                else
                {
                    output.Add(en);
                }
            }
            return output.ToArray();
        }

#pragma warning disable CS1570 // XML comment has badly formed XML -- 'Expected an end tag for element 'example'.'
#pragma warning disable CS1570 // XML comment has badly formed XML -- 'End tag 'example' does not match the start tag 'Enum'.'
        /// <summary>
        /// This is universal List flattener - used internally by <see cref="getFlatArray{T}(IEnumerable)"/> to gather child <c>IEnumerable</c> items.
        /// </summary>
        /// <remarks>If you're flattening the params input better use getFlatArray</remarks>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <example>
        ///  public static Object getProperField(this PropertyCollection source, params Enum[] fields)
        /// {
        ///    fields = fields.getFlatArray<Enum>().ToArray();
        ///    }
        /// </example>
        /// \ingroup_disabled ace_ext_collections
        public static List<T> getFlatList<T>(this IEnumerable source, params Type[] keepUnFlat)
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'End tag 'example' does not match the start tag 'Enum'.'
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'Expected an end tag for element 'example'.'
        {
            List<T> output = new List<T>();
            String keepOut = "";
            if (!keepUnFlat.Any())
            {
                keepOut = "PropertyCollection";
                //keepUnFlat.Add(typeof(String));
            }

            Boolean banFlatting = false;
            if (source == null) return output;
            foreach (T en in source)
            {
                if (en is IList || en is Array)
                {
                    banFlatting = keepUnFlat.Contains(en.GetType());
                    IEnumerable enl = en as IEnumerable;

                    if (!banFlatting)
                    {
                        banFlatting = enl.GetType().Name.StartsWith(keepOut);
                    }

                    if (!banFlatting)
                    {
                        output.AddRange(enl.getFlatList<T>(keepUnFlat));
                    }
                    else
                    {
                        if (enl is T) output.Add(en);
                    }
                }
                else
                {
                    output.Add(en);
                }
            }
            return output;
        }

        /// <summary>
        /// Automatics the default values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="D"></typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <param name="defValue">The definition value.</param>
        /// <param name="skipExisting">if set to <c>true</c> [skip existing].</param>
        /// <returns></returns>
        public static Dictionary<T, D> autoDefaultValues<T, D>(this Dictionary<T, D> dict, D defValue, Boolean skipExisting)
        {
            Type ent = typeof(T);

            if (!ent.isEnum())
            {
                return dict;
            }
            var vls = Enum.GetValues(typeof(T));
            foreach (T vl in vls)
            {
                if (dict.ContainsKey(vl))
                {
                    if (!skipExisting)
                    {
                        dict[vl] = defValue;
                    }
                }
                else
                {
                    dict.Add(vl, defValue);
                }
            }
            return dict;
        }

        /// <summary>
        /// Converts the value to string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Dictionary<String, String> convertValueToString(this IDictionary<String, Object> source)
        {
            Dictionary<String, String> output = new Dictionary<string, string>();

            foreach (var pair in source)
            {
                output.Add(pair.Key, pair.Value.toStringSafe());
            }

            return output;
        }

        public static Int32 AddMultiline(this IList<String> host, string multiline)
        {
            String[] lines = multiline.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Int32 c = 0;
            foreach (String line in lines)
            {
                if (imbSciStringExtensions.isNullOrEmptyString(line))
                {
                }
                else
                {
                    c++;
                    host.Add(line);
                }
            }
            return c;
        }

        public static Int32 AddAtIndex<T>(this SortedList<Int32, T> list, Int32 index, T item)
        {
            //T tmp = list[index];
            Int32 ky = 0;

            if (index < list.Count())
            {
                ky = list.Keys[index];
                ky = list.GetNextKey(index);
                list.Add(ky, item);
            }
            else
            {
                ky = list.Keys.Max() + 1;
                list.Add(ky, item);
            }

            return list.IndexOfValue(item);
        }

        public static Int32 AddAtKey<T>(this SortedList<Int32, T> list, Int32 key, T item)
        {
            //T tmp = list[index];
            Int32 ky = key;
            if (list.ContainsKey(key))
            {
                ky = list.GetNextKey(ky);
            }

            list.Add(ky, item);

            return ky;
        }

        public static Int32 AddAtEnd<T>(this SortedList<Int32, T> list, T item)
        {
            //T tmp = list[index];
            Int32 ky = list.Keys.Max() + 1;

            list.Add(ky, item);

            return ky;
        }

        public static Int32 GetNextKey<T>(this SortedList<Int32, T> list, Int32 ky)
        {
            while (list.ContainsKey(ky))
            {
                ky = ky + 1;
            }

            return ky;
        }

        /// <summary>
        /// Adds the resources.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static List<Object> AddResources(this IList<Object> host, params Object[] input)
        {
            List<Object> output = new List<object>();
            output.AddRange(host);
            output.AddRange(input);
            return output;
        }

        /// <summary>
        /// Adds the specified input/s - if exists skips
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="input">The input.</param>
        /// <returns>Number of objects iserted</returns>
        /// \ingroup_disabled ace_ext_collections
        public static Int32 AddMulti(this IList host, params Object[] input)
        {
            var flat = input;
            //List<Object> flat = input.getFlatList<Object>();
            Int32 c = 0;
            foreach (Object obj in flat)
            {
                if (host.Contains(obj))
                {
                    // vec postoji objekat, zato ne sabira C
                }
                else
                {
                    if (obj is IEnumerable)
                    {
                        IEnumerable obj_IEnumerable = (IEnumerable)obj;

                        foreach (Object obi in obj_IEnumerable)
                        {
                            host.Add(obi);
                            c++;
                        }
                    }
                    else
                    {
                        host.Add(obj);
                        c++;
                    }
                }
            }
            return c;
        }

        /// <summary>
        /// Adds the specified input/s - if exists skips
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="input">The input.</param>
        /// <returns>Number of objects iserted</returns>
        /// \ingroup_disabled ace_ext_collections
        public static Int32 AddMultiple(this IList host, params Object[] input)
        {
            var flat = input;
            //List<Object> flat = input.getFlatList<Object>();
            Int32 c = 0;
            foreach (Object obj in flat)
            {
                if (host.Contains(obj))
                {
                    // vec postoji objekat, zato ne sabira C
                }
                else
                {
                    host.Add(obj);
                }
            }
            return c;
        }

        /// <summary>
        /// Adds the specified input/s - if exists skips
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="input">The input.</param>
        /// <returns>Number of objects iserted</returns>
        /// \ingroup_disabled ace_ext_collections
        public static Int32 AddUnique(this IList host, params Object[] input)
        {
            var flat = input;
            //List<Object> flat = input.getFlatList<Object>();
            Int32 c = 0;
            foreach (Object obj in flat)
            {
                if (host.Contains(obj))
                {
                    // vec postoji objekat, zato ne sabira C
                }
                else
                {
                    host.Add(obj);
                    c++;
                }
            }
            return c;
        }

        /// <summary>
        /// Dodaje niz objekata u niz ukoliko ih vec ne sadrzi lista - vraca podatak koliko je novih objekata ubaceno
        /// </summary>
        /// <param name="host">Lista u koju se ubacuju objekti</param>
        /// <param name="input">Bilo koja IEnumerable kolekcija</param>
        /// <returns>Koliko je objekata ubaceno? posto ne ubacuje objekat koji je vec zadrzan u listi</returns>
        public static Int32 AddRangeImb(this IList host, IEnumerable input)
        {
            Int32 c = 0;

            foreach (Object obj in input)
            {
                if (host.Contains(obj))
                {
                    // vec postoji objekat, zato ne sabira C
                }
                else
                {
                    host.Add(obj);
                    c++;
                }
            }

            return c;
        }

        /// <summary>
        /// Proverava da li je kolekcija null ili bez clanova
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Boolean isNullOrEmpty(this IEnumerable source)
        {
            if (source == null) return true;
            foreach (Object ob in source)
            {
                return false;
            }
            return true;
        }

        // ----------------------------------------- FIRST / LAST

        /// <summary>
        /// Imbs the last safe.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static T imbLastSafe<T>(this IEnumerable<T> source, Int32 index = 0) where T : class
        {
            Int32 c = 0;
            if (source == null) return null;
            T head = null;
            foreach (T it in source)
            {
                head = it;
            }
            return head;
        }

        /// <summary>
        /// Imbs the first safe.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static Object imbFirstSafe(this IEnumerable source, Int32 index = 0)
        {
            Int32 c = 0;
            if (source == null) return null;
            foreach (var it in source)
            {
                if (c == index) return it;
                c++;
            }
            return null;
            //if (re)
        }

        /// <summary>
        /// Vraca jedan ili vise resursa koji ispunjavaju uslov
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pred"></param>
        /// <returns></returns>
        public static oneOrMore<T> imbFirstOrMore<T>(this IEnumerable<T> source, Func<T, bool> pred) where T : class
        {
            var result = source.Where<T>(pred);
            oneOrMore<T> output = new oneOrMore<T>(false);
            output += result;
            return output;
        }

        /// <summary>
        /// Proverava uslov
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pred"></param>
        /// <returns></returns>
        public static T imbFirstSafe<T>(this IEnumerable<T> source, Func<T, bool> pred) where T : class
        {
            var result = source.Where<T>(pred);
            if (result.Any())
            {
                return result.First();
            }
            return null;
            //if (re)
        }

        /// <summary>
        /// Proverava uslov
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pred"></param>
        /// <returns></returns>
        public static T imbFirstSafe<T>(this IEnumerable<T> source, Int32 index = 0) where T : class
        {
            Int32 c = 0;
            if (source == null) return null;
            foreach (T it in source)
            {
                if (c == index) return it;
                c++;
            }
            return null;
            //if (re)
        }

        /// <summary>
        /// Vraca listu sa rednim brojem i ToString() vrednost
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static String writeListOfCollection<T>(this IList<T> source, String format = "{0,-10} : {1}")
            where T : class
        {
            String output = "";
            foreach (T item in source)
            {
                output = output + String.Format(format, source.writeIndexOutOf(item), item.ToString()) + Environment.NewLine;
            }
            return output;
        }

        /// <summary>
        /// Gets the first safe.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Object getFirstSafe(this IEnumerable source)
        {
            foreach (Object ob in source)
            {
                return ob;
            }

            return null;
        }

        public static Object getLastSafe(this IEnumerable source)
        {
            Object head = null;
            foreach (Object ob in source)
            {
                head = ob;
            }

            return head;
        }


        //public static Boolean Any(this ICollection source)
        //{
        //    return source.Count > 0;
        //}

        /// <summary>
        /// Vraca prvi element koji je pronadjen u ulaznom skupu - default vraca prvi element matchList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="matchList"></param>
        /// <returns></returns>
        public static T takeFirstFromList<T>(this IEnumerable<T> input, params T[] matchList)
        {
            var ml = matchList.toList();
            var defReturn = matchList.First();

            foreach (var i in input)
            {
                if (ml.Contains(i))
                {
                    return i;
                }
            }
            return defReturn;
        }

        /// <summary>
        /// Vraca prvi objekat koji se nalazi i u ovom skupu i u matchList skupu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="matchList">Koji od elemenata treba da se poklopi</param>
        /// <param name="defReturn">Element koji vraca u podrazumevanom slucaju</param>
        /// <returns>Prvi element koji se nalazi u oba skupa</returns>
        public static T takeFirstFromList<T>(this IEnumerable<T> input, IEnumerable<T> matchList, T defReturn)
        {
            var ml = matchList.toList();

            foreach (var i in input)
            {
                if (ml.Contains(i))
                {
                    return i;
                }
            }
            return defReturn;
        }

        /// <summary>
        /// Vraca objekat iz liste - sa trazene index lokacije koja lupuje
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T takeItemAt<T>(this IList<T> source, Int32 index, Boolean doLoop = true,
                                      Boolean doReturnLast = false) where T : class
        {
            T output = null;
            if (source == null) return output;
            if (!source.Any()) return output;

            Int32 c = source.Count();

            if (c == 0) return output;

            if (doLoop)
            {
                while (index < 0)
                {
                    index = c + index;
                }

                while (index > c - 1)
                {
                    if (c == 1)
                    {
                        index = 0;
                        break;
                    }
                    index = index - c;
                }
            }
            else
            {
                if (doReturnLast)
                {
                    if (index < 0)
                    {
                        return source[0];
                    }
                    if (index > c - 1)
                    {
                        return source[c - 1];
                    }
                }
                else
                {
                    if (index < 0)
                    {
                        return null;
                    }
                    if (index > c - 1)
                    {
                        return null;
                    }
                }
            }

            output = source[index];

            return output;
        }

        /// <summary>
        /// Vraca item koji je na relativnoj lokaciji od relativeTo, podrzava lupovanje u oba smera
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="relativeTo">Item u odnosu na koji trazi relativni item. Ukoliko relativeTo nije nu kolekciji koristi indexStep kao index upit</param>
        /// <param name="indexStep">1 = next line, -1 = prev line, -5 = 5 lines before, 5 = 5 lines after, 0 = this line</param>
        /// <returns>Vraca null ako je relativeTo null i ako je source kolekcija prazna.</returns>
        public static T takeItemRelativeTo<T>(this IList<T> source, T relativeTo, Int32 indexStep) where T : class
        {
            T output = null;
            if (relativeTo == null)
            {
                return source.takeItemAt<T>(indexStep);
            }

            if (source == null) return output;
            if (!source.Any()) return output;
            if (indexStep == 0) return relativeTo;

            Int32 index = 0;
            // ako ga ne sadrzi> index = -1 + indexStep, tako da vraca prvi ako je indexStep = 1

            if (source.Contains(relativeTo))
            {
                index = source.IndexOf(relativeTo) + indexStep;
            }
            else
            {
                index = indexStep;
            }
            return source.takeItemAt<T>(index, true); // getLineAt(relativeTo.index, false);
        }

        /// <summary>
        /// Konvertuje niz u listu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<T> toList<T>(this IEnumerable<T> input)
        {
            var output = new List<T>();
            foreach (var i in input)
            {
                output.Add(i);
            }
            return output;
        }

        /// <summary>
        /// Writes the index out of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="item">The item.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static String writeIndexOutOf<T>(this IList<T> source, T item, String format = "[{0}/{1}]")
            where T : class
        {
            String output = "";
            if (source == null) return output;
            if (!source.Any()) return output;
            Int32 c = source.Count();
            Int32 i = source.IndexOf(item) + 1;

            output = String.Format(format, i, c);
            return output;
        }
    }
}