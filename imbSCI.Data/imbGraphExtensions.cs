// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbGraphExtensions.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data
{
    using imbSCI.Data.enums;
    using imbSCI.Data.extensions.data;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class imbGraphExtensions
    {
        /// <summary>
        /// Adds path prefix for root member
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static String addPrefixForRoot(this String path)
        {
            return PATHSPLITER + path;
        }

        /// <summary>
        /// Gets the ordinal path part. If it is not ordinal path path> returns -1
        /// </summary>
        /// <param name="ordinalPart">The ordinal part.</param>
        /// <returns>Number from ordinal path sequence, or -1 if it is not ordinal path</returns>
        public static Int32 getOrdinalPathPart(String ordinalPart)
        {
            if (ordinalPart.StartsWith("[") && ordinalPart.EndsWith("]"))
            {
                ordinalPart = ordinalPart.Trim("[]".ToArray());
            }
            else
            {
                return -1;
            }
            return Convert.ToInt32(ordinalPart);
        }

        //public static IObjectWithPathAndChildSelector getMemberByPath(this IObjectWithPathAndChildSelector shead, )

        /// <summary>
        /// Get member by resolving relative or absolute <c>path</c> over <c>shead</c> starting object.
        /// </summary>
        /// <remarks>
        /// Path supports: absolute (starts with <c>PATHSPLITER</c>, go to parent <c>..</c>, ordinal selectors <c>[1]</c> and sequential child <c>name</c> selection
        /// </remarks>
        /// <param name="shead">Start head - object to start from</param>
        /// <param name="path">The path: accepts go to parent syntax.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">getChild() STACK OVERFLOW [".o(c).a("] metaContentBase")</exception>
        public static IObjectWithChildSelector getChildByPath(this IObjectWithChildSelector shead, string path)
        {
            if (path.StartsWith(PATHSPLITER))
            {
                if (shead is IObjectWithRoot)
                {
                    IObjectWithRoot sroot = shead as IObjectWithRoot;
                    shead = sroot.root as IObjectWithChildSelector;
                }
                path = imbSciStringExtensions.removeStartsWith(path, PATHSPLITER);
            }

            List<string> prts = path.SplitSmart(PATHSPLITER); //.getPathParts();
            IObjectWithChildSelector head = shead;

            Int32 c = 0;
            foreach (String pt in prts)
            {
                IObjectWithChildSelector nhead;
                Int32 ord = getOrdinalPathPart(pt);

                if (ord > -1)
                {
                    if (ord >= head.Count())
                    {
                        ord = head.Count() - 1;
                    }

                    nhead = head[ord] as IObjectWithChildSelector;
                }
                else if (pt.StartsWith(PATHGO_PARENT))
                {
                    IObjectWithParent phead = head as IObjectWithParent;
                    if (phead != null)
                    {
                        nhead = phead.parent as IObjectWithPathAndChildSelector;
                    }
                    else
                    {
                        throw new ArgumentException("Go to parent path syntax found but head is not IObjectWithParent!!"); //  [".o(c).a("] metaContentBase"));
                    }
                }
                else
                {
                    nhead = head[pt] as IObjectWithChildSelector;
                }

                if (nhead == head)
                {
                    // logSystem.log("tmp");
                    //break;
                }
                else
                {
                    head = nhead;
                }
                c++;
                if (c > 100)
                {
                    throw new ArgumentException("getChild() would lead to STACK OVERFLOW - the limit was breached"); // aceGeneralException("getChild() STACK OVERFLOW [".o(c).a("] metaContentBase"));
                }
            }

            return head;
        }

        internal static String PATHGO_ROOT = "\\";
        internal static String PATHGO_PARENT = "..";
        internal static String PATHSPLITER = "\\";

        internal static String XPATHGO_ROOT = "\\";
        internal static String XPATHGO_PARENT = "..";
        internal static String XPATHSPLITER = "\\";

        internal static Int32 PATHSEARCH_MAX_LIMIT = 10000;

        /// <summary>
        /// Detects the parent child loop reference.
        /// </summary>
        /// <param name="suspects">The suspects.</param>
        /// <returns></returns>
        public static IObjectWithPath detectParentChildLoopReference(this List<IObjectWithPath> suspects)
        {
            IObjectWithPath output = null;

            List<string> uidList = new List<string>();

            foreach (IObjectWithPath item in suspects)
            {
                String uid = "";

                if (item is IObjectWithUID)
                {
                    IObjectWithUID item_IObjectWithUID = (IObjectWithUID)item;
                    uid = item_IObjectWithUID.UID;
                }

                /// ovde prosiriti podrsku za jos neke objekte - za sada samo IObjectWithUID podrzava

                if (uidList.Contains(uid))
                {
                    // objekat sa istim uidom vec postoji znaci da je on uzrok problema

                    return item;
                }
                else
                {
                    uidList.Add(uid);
                }
            }

            // ako je do ovde stigao znaci da nije nasao uzrok loopa

            return output;
        }

        /// <summary>
        /// Gets all parents. Prevents loop inherence.
        /// </summary>
        /// <param name="source">The object to start from. Excluded from result</param>
        /// <param name="parentDepthLimit">The parent depth limit.</param>
        /// <returns>
        /// List of all parents until root object or until looped node. Order of objects in the result is from <c>source</c> to <c>root</c>
        /// </returns>
        /// <exception cref="Exception"></exception>
        /// <seealso cref="IObjectWithPath" />
        /// <seealso cref="IObjectWithRoot" />
        public static List<IObjectWithPath> getParentsViaExtension(this IObjectWithPath source, Int32 parentDepthLimit = 2000)
        {
            List<IObjectWithPath> output = new List<IObjectWithPath>();
            IObjectWithPath head = source;
            IObjectWithPath last_head = source;
            Int32 c = 0;
            do
            {
                c++;
                if (c > parentDepthLimit)
                {
                    IObjectWithPath looper = output.detectParentChildLoopReference();

                    if (looper == null)
                    {
                        //throw new aceGeneralException("Parent search reached limit [" + parentDepthLimit + "] for [" + source.name + "]");
                    }
                    else
                    {
                        String msg = "Parent search reached limit[" + parentDepthLimit + "] for [" + source.name + "]";
                        msg = msg + "Looper found: " + looper.name;

                        throw new ArgumentOutOfRangeException(nameof(source), msg);
                    }

                    break;
                }
                head = head.parent as IObjectWithPath;
                if (head == last_head)
                {
                    // object is parent to it self
                    head = null;
                }
                if (head != null)
                {
                    if (output.Contains(head))
                    {
                        head = null;
                    }
                    else
                    {
                        output.Add(head);
                    }
                }
                last_head = head;
            } while (head != null);
            return output;
        }

        /// <summary>
        /// Gets the path string - including <c>source</c> at end of the path
        /// </summary>
        /// <param name="source">Object to get XPath for</param>
        /// <param name="useXPathFormat">if set to <c>true</c> if will return XPath format with / instead of backslash, otherwise it will use \\ as path separator member.</param>
        /// <returns>
        /// XPath string with the root backslash
        /// </returns>
        public static String getPathViaExtension(this IObjectWithPath source, Boolean useXPathFormat)
        {
            List<IObjectWithPath> parents = source.getParentsViaExtension();
            String output = PATHGO_ROOT;
            if (useXPathFormat) output = XPATHGO_ROOT;

            parents.Reverse();
            parents.Add(source);

            Int32 l = parents.Count();
            for (int i = 0; i < l; i++)
            {
                if (output.isNullOrEmpty())
                {
                    output = output + parents[i].name;
                }
                else
                {
                    if (useXPathFormat)
                    {
                        output = output + XPATHSPLITER + parents[i].name;
                    }
                    else
                    {
                        output = output + PATHSPLITER + parents[i].name;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Gets a member from parent-chain: a) Nth parent according to limit. b) parent on <c>targetPath</c>, c) parent of <c>target</c> type or simply d) root if no more parents.
        /// </summary>
        /// <param name="source">The source - objects to start search from</param>
        /// <param name="target">The target Type - what type will triger return of current head</param>
        /// <param name="limit">The limit of depth. For -1: unlimited, For 1: it will return source.parent, For 0: it will return source, for 50: it will return  50th parent in parent-chain.</param>
        /// <param name="targetPath">Once head reach this path it will trigger return of head. Disabled if empty or null.</param>
        /// <returns>Parent in parent chain or root</returns>
        /// \ingroup_disabled ace_ext_path
        public static IObjectWithParent getParentOrRoot(this IObjectWithParent source, String targetPath = "", Type target = null, Int32 limit = 100)
        {
            IObjectWithParent head = source;

            Int32 c = 0;
            Boolean p_testOn = (!imbSciStringExtensions.isNullOrEmptyString(targetPath) || (source is IObjectWithPath));
            Boolean t_testOn = (target != null);
            if (limit == -1) limit = PATHSEARCH_MAX_LIMIT;

            do
            {
                IObjectWithParent nhead = head.parent as IObjectWithParent;

                if (nhead == null) return head;

                if (nhead == head)
                {
                    throw new ArgumentException("IObjectWithParent.parent is the object it self! getParentOrRoot [" + c + "]");
                    // logSystem.log("tmp");
                    break;
                }
                else
                {
                    head = nhead;
                }

                if (t_testOn)
                {
                    if (head.GetType() == target)
                    {
                        return head;
                    }
                }

                if (p_testOn)
                {
                    IObjectWithPath p_head = head as IObjectWithPath;
                    if (p_head != null)
                    {
                        if (p_head.path == targetPath)
                        {
                            return head;
                        }
                    }
                }

                c++;
                if ((c > limit) || (c > PATHSEARCH_MAX_LIMIT))
                {
                    return head;
                    //
                }
            } while (head.parent != null);

            return head;
        }

        /// <summary>
        /// Relative path to memeber in parent chain
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>Propert relative path</returns>
        public static String getPathToParent(this IObjectWithParent source, IObjectWithParent target, String prefix = "", Int32 limit = 100)
        {
            String output = "";
            IObjectWithParent head = source;

            Int32 c = 0;

            if (limit == -1) limit = PATHSEARCH_MAX_LIMIT;

            do
            {
                output = PATHGO_PARENT.add(output, PATHSPLITER);

                head = head.parent as IObjectWithParent;

                c++;
                if ((c > limit) || (c > PATHSEARCH_MAX_LIMIT))
                {
                    break;
                }
            } while ((head != null) && (head != target));

            return output;
        }

        /// <summary>
        /// Gets the path from object - just casting trick
        /// </summary>
        /// <param name="head">The head.</param>
        /// <returns></returns>
        public static String getPathFromObject(this IObjectWithPath head)
        {
            IObjectWithPath p_head = head as IObjectWithPath;
            if (p_head != null)
            {
                return p_head.path;
            }
            if (head is IObjectWithName)
            {
                IObjectWithName n_head = head as IObjectWithName;
                return PATHGO_PARENT + n_head.name;
            }
            return PATHSPLITER;
        }

        /// <summary>
        /// Discovers relative path to <c>target</c> member of the structure
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns>String that leads to target</returns>
        public static string getPathTo(this IObjectWithPathAndChildSelector source, IObjectWithPathAndChildSelector target)
        {
            String t_path = target.path;
            String s_path = source.path;
            String c_path = t_path.getCommonRoot(s_path, false);
            String c_to_t = imbSciStringExtensions.removeStartsWith(t_path, c_path);

            IObjectWithPathAndChildSelector root = source.getParentOrRoot() as IObjectWithPathAndChildSelector;

            IObjectWithPathAndChildSelector common = root.getChildByPath(c_path) as IObjectWithPathAndChildSelector; // root.getChild(c_to_t);

            String s_to_c = source.getPathToParent(common);

            String output = s_to_c.add(c_to_t, PATHSPLITER);
            return output;
        }

        /// <summary>
        /// Gets the typed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static List<T> getTyped<T>(this IEnumerable<IObjectWithName> source) where T : IObjectWithName
        {
            List<T> output = new List<T>();
            foreach (IObjectWithName on in source) output.Add((T)@on);
            return output;
        }

        /// <summary>
        /// Null safe way to get meta element level
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static reportElementLevel getElementLevel(this IObjectWithReportLevel source)
        {
            if (source == null)
            {
                return reportElementLevel.none;
            }

            return source.elementLevel;

            return reportElementLevel.none;
        }

        /// <summary>
        /// Determines whether [is element level] [the specified element].
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="element">The element.</param>
        /// <returns>
        ///   <c>true</c> if [is element level] [the specified element]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean isElementLevel(this IObjectWithReportLevel obj, reportElementLevel element)
        {
            reportElementLevel el = getElementLevel(obj);
            return (el == element);
        }

        /// <summary>
        /// Gets the parent of target element level or root if reached
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="level">The level.</param>
        /// <param name="getLastParent">if set to <c>true</c> [get last parent].</param>
        /// <param name="nullForNotFound">if set to <c>true</c> [null for not found].</param>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        public static IObjectWithParent getParentOfLevel(this IObjectWithParent source, reportElementLevel level, Boolean getLastParent = false, Boolean nullForNotFound = true, Int32 limit = 100)
        {
            IObjectWithParent output = source;
            IObjectWithReportLevel head = output as IObjectWithReportLevel;
            Int32 c = 0;

            if (limit == -1) limit = PATHSEARCH_MAX_LIMIT;

            do
            {
                head = head.parent as IObjectWithReportLevel;

                if (getLastParent)
                {
                    if (head.elementLevel == level)
                    {
                        IObjectWithReportLevel parent = head.parent as IObjectWithReportLevel;
                        if (parent != null)
                        {
                            if (!(parent.elementLevel == level))
                            {
                                return head;
                            }
                        }
                        else
                        {
                            return head;
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (getElementLevel(head) == level)
                    {
                        return head;
                    }
                }

                c++;
                if ((c > limit) || (c > PATHSEARCH_MAX_LIMIT))
                {
                    break;
                }
            } while ((head != null));

            if (head == null)
            {
                if (nullForNotFound)
                {
                }
                else
                {
                }
            }
            return head;
        }

        /// <summary>
        /// Gets the type of all children in.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">The parent.</param>
        /// <param name="pathFilter">The path filter.</param>
        /// <param name="inverse">if set to <c>true</c> it inverses the filter</param>
        /// <param name="recursiveIndex">Index of the recursive calls (if orderByPath is true)</param>
        /// <param name="recursiveLimit">The recursive calls limit.</param>
        /// <returns></returns>
        public static List<T> getAllChildrenInType<T>(this T parent, Regex pathFilter = null, Boolean inverse = false, Boolean orderByPath = true, Int32 recursiveIndex = 1, Int32 recursiveLimit = 500, Boolean includingParent = false) where T : class, IObjectWithPathAndChildren
        {
            List<IObjectWithPathAndChildren> output = parent.getAllChildren(pathFilter, inverse, orderByPath, recursiveIndex, recursiveLimit);

            List<T> response = new List<T>();
            foreach (IObjectWithPathAndChildren r in output)
            {
                var rn = r as T;

                if (rn != null)
                {
                    response.Add(rn);
                }
            }

            return response;
        }

        /// <summary>
        /// Gets all children. If pathFilder defined, it uses it to select only children with appropriate path
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="pathFilter">The path filter.</param>
        /// <param name="inverse">if set to <c>true</c> it inverses the filter</param>
        /// <param name="orderByPath">if set to <c>true</c> [order by path].</param>
        /// <param name="recursiveIndex">Index of the recursive calls (if orderByPath is true)</param>
        /// <param name="recursiveLimit">The recursive calls limit.</param>
        /// <returns></returns>
        public static List<IObjectWithPathAndChildren> getAllChildren(this IObjectWithPathAndChildren parent, Regex pathFilter = null, Boolean inverse = false, Boolean orderByPath = false, Int32 recursiveIndex = 1, Int32 recursiveLimit = 500, Boolean includingParent = false)
        {
            List<IObjectWithPathAndChildren> output = new List<IObjectWithPathAndChildren>();

            List<IObjectWithPathAndChildren> stack = new List<IObjectWithPathAndChildren>();
            stack.Add(parent);
            if (recursiveIndex > recursiveLimit)
            {
#if DEBUG
                Console.WriteLine("RECURSIVE LIMIT REACHED [" + recursiveIndex + "] in getAllChildren - [" + parent.name + "] (" + parent.GetType().Name + ")");
#endif
                return output;
            }
            while (stack.Any())
            {
                var n_stack = new List<IObjectWithPathAndChildren>();

                foreach (IObjectWithPathAndChildren child in stack)
                {
                    if (orderByPath)
                    {
                        if (pathFilter != null)
                        {
                            if (pathFilter.IsMatch(child.path))
                            {
                                if (!inverse)
                                {
                                    output.Add(child);
                                }
                            }
                            else
                            {
                                if (inverse)
                                {
                                    output.Add(child);
                                }
                            }
                        }
                        else
                        {
                            output.Add(child);
                        }

                        if (child.Any())
                        {
                            foreach (IObjectWithPathAndChildren c in child)
                            {
                                output.AddRange(getAllChildren(c, pathFilter, inverse, orderByPath, recursiveIndex + 1, recursiveLimit, true));
                            }
                        }
                    }
                    else
                    {
                        if (pathFilter != null)
                        {
                            if (pathFilter.IsMatch(child.path))
                            {
                                if (!inverse) output.Add(child);
                            }
                            else
                            {
                                if (inverse) output.Add(child);
                            }
                        }
                        else
                        {
                            output.Add(child);
                        }

                        if (child.Any())
                        {
                            foreach (IObjectWithPathAndChildren c in child) n_stack.Add(c);
                        }
                    }
                }

                stack = n_stack;
            }

            if (!includingParent) output.Remove(parent);
            return output;
        }

        /// <summary>
        /// Removes all children from parent nodes
        /// </summary>
        /// <param name="children">The children.</param>
        public static void removeFromParent(this IEnumerable<IObjectWithPathAndChildren> children)
        {
            foreach (IObjectWithPathAndChildren ch in children)
            {
                if (ch.parent != null)
                {
                    var chp = ch.parent as IObjectWithPathAndChildren;
                    chp.Remove(ch.name);
                }
            }
        }

        /// <summary>
        /// Gets the names.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="unique">if set to <c>true</c> [unique].</param>
        /// <returns></returns>
        public static List<String> getNames(this IEnumerable<IObjectWithName> source, Boolean unique = true)
        {
            List<String> output = new List<string>();
            foreach (IObjectWithName on in source)
            {
                if (unique)
                {
                    if (!output.Contains(on.name)) output.Add(on.name);
                }
                else
                {
                    output.Add(on.name);
                }
            }
            return output;
        }

        /// <summary>
        /// Gets paths of 1-st level children.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static List<String> getPaths(this IEnumerable<IObjectWithPath> source)
        {
            List<String> output = new List<string>();
            foreach (IObjectWithPath on in source) output.Add(on.path);
            return output;
        }

        /// <summary>
        /// Gets the deepest.
        /// </summary>
        /// <param name="children">The children.</param>
        /// <param name="margine">The margine.</param>
        /// <returns></returns>
        public static List<IObjectWithPathAndChildren> getDeepest(this IEnumerable<IObjectWithPathAndChildren> children, Int32 margine = 0)
        {
            List<IObjectWithPathAndChildren> output = new List<IObjectWithPathAndChildren>();
            Int32 maxl = Int32.MinValue;
            foreach (IObjectWithPathAndChildren child in children)
            {
                maxl = Math.Max(child.level, maxl);
            }

            maxl = maxl - margine;

            foreach (IObjectWithPathAndChildren child in children)
            {
                if (child.level >= maxl) output.Add(child);
            }
            return output;
        }

        /// <summary>
        /// Gets the on level.
        /// </summary>
        /// <param name="children">The children.</param>
        /// <param name="level">The level.</param>
        /// <param name="downMargin">Down margin.</param>
        /// <param name="upMargin">Up margin.</param>
        /// <returns></returns>
        public static List<IObjectWithPathAndChildren> getOnLevel(this IEnumerable<IObjectWithPathAndChildren> children, Int32 level = 0, Int32 downMargin = 0, Int32 upMargin = 0)
        {
            List<IObjectWithPathAndChildren> output = new List<IObjectWithPathAndChildren>();
            Int32 min = level - downMargin;
            Int32 max = level + upMargin;

            foreach (IObjectWithPathAndChildren child in children)
            {
                if (child.level == level)
                {
                    output.Add(child);
                }
                else if (child.level > min && child.level < max)
                {
                    output.Add(child);
                }
            }
            return output;
        }

        /// <summary>
        /// Filters out the collection by path regex, <c>nameRegex</c> is optional AND criterion
        /// </summary>
        /// <param name="children">The children.</param>
        /// <param name="pathRegex">The path regex.</param>
        /// <param name="nameRegex">The name regex.</param>
        /// <returns></returns>
        public static List<IObjectWithPathAndChildren> getFilterOut(this IEnumerable<IObjectWithPathAndChildren> children, Regex pathRegex, Regex nameRegex = null)
        {
            List<IObjectWithPathAndChildren> output = new List<IObjectWithPathAndChildren>();

            foreach (IObjectWithPathAndChildren child in children)
            {
                if (pathRegex.IsMatch(child.path))
                {
                    if (nameRegex != null)
                    {
                        if (nameRegex.IsMatch(child.name)) output.Add(child);
                    }
                    else
                    {
                        output.Add(child);
                    }
                }
                else { }
            }
            return output;
        }

        //public static List<IObjectWithPathAndChildren> getOnlyWithFullBranch(this IEnumerable<IObjectWithPathAndChildren> children, Regex pathRegex, Regex nameRegex = null)
        //{
        //    List<IObjectWithPathAndChildren> output = new List<IObjectWithPathAndChildren>();

        //    foreach (IObjectWithPathAndChildren child in children)
        //    {
        //        if (pathRegex.IsMatch(child.path))
        //        {
        //            if (nameRegex != null)
        //            {
        //                if (nameRegex.IsMatch(child.name)) output.Add(child);
        //            }
        //            else
        //            {
        //                output.Add(child);
        //            }
        //        }
        //        else { }
        //    }
        //    return output;
        //}

        /// <summary>
        /// Gets all leafs, optionally applies Regex criteria used to child name [[[doesn't work]]]
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="nameFilter">The name filter.</param>
        /// <returns></returns>
        public static List<IObjectWithPathAndChildren> getAllLeafs(this IObjectWithPathAndChildren parent, Regex nameFilter = null, Boolean inverse = false)
        {
            List<IObjectWithPathAndChildren> output = new List<IObjectWithPathAndChildren>();

            List<IObjectWithPathAndChildren> stack = new List<IObjectWithPathAndChildren>();
            stack.Add(parent);

            while (stack.Any())
            {
                var n_stack = new List<IObjectWithPathAndChildren>();

                foreach (IObjectWithPathAndChildren child in stack)
                {
                    if (child.Any())
                    {
                        foreach (IObjectWithPathAndChildren c in child) n_stack.Add(c);
                    }
                    else
                    {
                        if (nameFilter != null)
                        {
                            if (nameFilter.IsMatch(child.name))
                            {
                                if (!inverse) output.Add(child);
                            }
                            else
                            {
                                if (inverse) output.Add(child);
                            }
                        }
                        else
                        {
                            output.Add(child);
                        }
                    }
                }
                stack = n_stack;
            }
            return output;
        }
    }
}