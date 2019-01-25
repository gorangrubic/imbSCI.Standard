using imbSCI.Data.enums;
using imbSCI.Data.extensions.data;
using imbSCI.Data.interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace imbSCI.Data
{
    public static class imbSciDataExtensions
    {


        public static List<TItem> GetDifference<TItem>(this IEnumerable<TItem> first, IList<TItem> other)
        {
            List<TItem> output = new List<TItem>();

            foreach (TItem item in first)
            {
                if (!other.Contains(item))
                {
                    output.Add(item);
                }
            }

            return output;
        }

        public static List<TItem> GetUnion<TItem>(this IEnumerable<TItem> first, IList<TItem>[] others)
        {
            List<TItem> output = new List<TItem>();

            output.AddRange(first);

            var oth = others.ToList();
            foreach (var other in others)
            {
                foreach (var o in other)
                {
                    if (!output.Contains(o)) output.Add(o);
                }

                //if (!output.Contains(other)) output.AddRange(other, true);
                //output.AddUnique(other);
                //oth.ForEach((XmlReadMode=))

                // output.AddRangeUnique(other);
            }

            return output;
        }


        public static List<TItem> GetCrossSection<TItem>(this IEnumerable<TItem> first, IList<TItem> other)
        {
            List<TItem> output = new List<TItem>();

            foreach (TItem item in first)
            {

                if (other.Contains(item))
                {
                    output.Add(item);
                }

            }

            return output;
        }



        /// <summary>
        /// Gets the cross section: only items that are in common to all collections
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="others">The others.</param>
        /// <returns></returns>
        public static List<TItem> GetCrossSection<TItem>(this IEnumerable<TItem> first, IList<TItem>[] others)
        {
            List<TItem> output = new List<TItem>();

            foreach (TItem item in first)
            {
                Boolean ok = true;

                for (int i = 0; i < others.Length; i++)
                {
                    if (!others[i].Contains(item))
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok)
                {
                    output.Add(item);
                }
            }

            return output;
        }


        public static Boolean ContainsByEnum(this IList flags, Object[] needles, containsQueryTypeEnum type)
        {
            switch (type)
            {
                case containsQueryTypeEnum.ContainsAll:
                    return flags.ContainsAll(needles);
                    break;

                default:
                case containsQueryTypeEnum.ContainsAny:
                    return flags.ContainsAny(needles);
                    break;

                case containsQueryTypeEnum.ContainsOnly:
                    return flags.ContainsOnly(needles);
                    break;
            }
        }

        public static Boolean ContainsOnly(this IList flags, params Object[] tests)
        {
            foreach (Object f in flags)
            {
                if (!tests.Contains(f)) return false;
            }
            return true;
        }

        //public static Boolean ContainsAll(this IList flags, params Object[] tests)
        //{
        //    foreach (Object f in tests)
        //    {
        //        if (!flags.Contains(f)) return false;
        //    }
        //    return true;
        //}

        /// <summary>
        /// Logical sum of criterias. All must be true to pass.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified criteria]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean Contains<T>(this IList<T> list, params T[] criteria)
        {
            foreach (T t in criteria)
            {
                if (!list.Contains(t))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether the <c>target</c> contains any of specified needles
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="needles">The needles.</param>
        /// <returns>
        ///   <c>true</c> if the specified needles contains any; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean ContainsAny(this IList target, IEnumerable needles)
        {
            foreach (Object needle in needles)
            {
                if (target.Contains(needle))
                {
                    return true;
                }
            }
            return false;
        }

        public static Boolean ContainsAny(this IList target, IEnumerable needles, out Object match)
        {
            foreach (Object needle in needles)
            {
                if (target.Contains(needle))
                {
                    match = needle;
                    return true;
                }
            }
            match = null;
            return false;
        }

        public static Boolean ContainsAll(this IList target, params Object[] needles)
        {
            Boolean output = true;
            foreach (Object needle in needles)
            {
                if (!target.Contains(needle))
                {
                    return false;
                }
            }
            return true;
        }

        public static Boolean ContainsAll(this IList target, IEnumerable needles)
        {
            Boolean output = true;
            foreach (Object needle in needles)
            {
                if (!target.Contains(needle))
                {
                    return false;
                }
            }
            return true;
        }

        public static Boolean ContainsOneOrMore(this IList flags, params Object[] tests)
        {
            foreach (Object f in flags)
            {
                if (tests.Contains(f)) return true;
            }
            return false;
        }

    }
}