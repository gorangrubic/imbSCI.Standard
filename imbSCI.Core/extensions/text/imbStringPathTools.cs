// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringPathTools.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.text
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using imbSCI.Data;

    #region imbVeles using

    using System;

    #endregion imbVeles using

    /// <summary>
    /// Operacije nad Binding/Property/imbProjectResource path stringovima
    /// </summary>
    public static class imbStringPathTools
    {
        public static Regex regex_propertyNameSelect = new Regex(@"\.?([^\.\[]+)\[{1}");
        private static char[] _allPathSegments;
        private static Regex _rg;

        /// <summary>
        /// Kolekcija svih Charova koji se pojavljuju kao separatori putanja
        /// </summary>
        internal static char[] allPathSegments
        {
            get
            {
                if (_allPathSegments == null)
                    _allPathSegments = "\\.:[]!`?-+*".ToCharArray();
                // _allPathSegments = resourcePathResolver.prefixVsFormat.allPrefixes();
                //.prefix_CHILD_PATH + imbProjectResourceBase.prefix_LINKED_PROPERTY_PATH + imbProjectResourceBase.prefix_PROJECT_PATH + imbProjectResourceBase.prefix_PROPERTY_PATH + imbProjectResourceBase.prefix_INTEGRATED_PATH + imbProjectResourceBase.prefix_COLLECTION_QUERY + imbProjectResourceBase.prefix_STRING_LIST;
                return _allPathSegments;
            }
        }

        /// <summary>
        /// Regex za prepoznavanje segmenta putanje
        /// </summary>
        internal static Regex rg
        {
            get
            {
                if (_rg == null)
                {
                    String _qr = new String(allPathSegments);
                    _qr = Regex.Escape(_qr);
                    _rg = new Regex("[" + _qr + "]");
                }
                return _rg;
            }
        }

        //#region --- regex_nameInPathSelect ------- regex za automatsku selekciju poslednjeg elementa u putanji, odnosno imena
        //private static Regex _regex_nameInPathSelect;
        ///// <summary>
        ///// regex za automatsku selekciju poslednjeg elementa u putanji, odnosno imena
        ///// </summary>
        //public static Regex regex_nameInPathSelect
        //{
        //    get
        //    {
        //        if (_regex_nameInPathSelect == null)
        //        {
        //            _regex_nameInPathSelect = new Regex();
        //        }
        //        return _regex_nameInPathSelect;
        //    }
        //}
        //#endregion

        /// <summary>
        /// Builds path from given elements of resource path
        /// </summary>
        /// <param name="startPath"></param>
        /// <param name="pathElements"></param>
        /// <returns></returns>
        /// \ingroup_disabled ace_ext_path
        public static String getPathFromElements(this string startPath, params String[] pathElements)
        {
            String output = startPath;
            foreach (String el in pathElements)
            {
                if (!el.StartsWith("."))
                {
                    output = output.add(" ", "\\").Trim();  // //.EnsureEndsWith("\\");
                }

                output += el.Trim();

                if (el.Contains("."))
                {
                    break;
                }
            }

            output = standardizePath(output);
            //output = output
            return output;
        }

        /// <summary>
        /// Cleans up filename candidate String by removing all forbiden characters from <c>filename</c> and replaces <c>.</c> with <c>_</c>. Also adds <c>extension</c> if any provided.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extensionToAdd">Ekstenzija koja treba da se nalazi na kraju fajla</param>
        /// <returns></returns>
        /// \ingroup_disabled ace_ext_filepath
        public static String getValidFileName(this string filename, String extensionToAdd = "")
        {
            Char[] invalid = Path.GetInvalidFileNameChars();

            foreach (Char c in invalid)
            {
                filename = filename.Replace(c, '_');
            }

            if (!String.IsNullOrEmpty(extensionToAdd))
            {
                filename = filename.Replace('.', '_');
                filename += "." + extensionToAdd;
            }

            return filename;
        }

        /// <summary>
        /// 2013c> obradjuje string sa binding/property pathom> npr.  object.color.hexValue - i vraca listu elemenata
        /// </summary>
        /// <param name="propertyPath"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static List<string> getPropertyPathElements(this String propertyPath, String spliter = ".")
        {
            List<string> propPath = new List<string>();
            if (propertyPath.Contains(spliter))
            {
                propPath.AddRange(propertyPath.Split(spliter.ToArray(), StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                propPath.Add(propertyPath);
            }
            return propPath;
        }

        /// <summary>
        /// Vraća SQL-kompatibilnu verziju putanje. Konverzija nije reverzibilna u potpunosti!!
        /// </summary>
        /// <param name="path">Putanja imbReportCell-a ili nekog drugog resursa</param>
        /// <returns></returns>
        public static String getSQLPaths(this String path, String spliter = ".")
        {
            String output = path;

            if (!path.Contains(spliter))
            {
                return path;
            }

            output = output.Trim();
            output = output.ToLower();
            output = output.Replace(spliter, "_");
            output = output.Replace(" ", "");

            output = output.Trim("_".ToCharArray());

            return output;
        }

        public static String getPathFromSQLPath(this String sqlPath)
        {
            String output = sqlPath;
            output = output.Replace("_", "/");
            return "/" + output;
        }

        /// <summary>
        /// DEPRECATED
        /// </summary>
        /// <param name="pathInput">The path input.</param>
        /// <returns></returns>
        public static string standardizePath(String pathInput)
        {
            int pos = pathInput.IndexOf("/");
            if ((pos > 0) || (pos == -1))
            {
                pathInput = "/" + pathInput;
            }
            return pathInput;
        }

        /// <summary>
        /// X IZBACENO
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String getTitleFromPath(this String path, Boolean filterSpecials = true)
        {
            String output = "";

            //String[] tmpArr = path.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            // List<string> tmpArr = new List<string>();

            //pathSegments tmpArr;
            //tmpArr = path.toPathSegments();

            //if (tmpArr.Any())
            //{
            //    output = tmpArr.Last().needle;
            //    if (filterSpecials) output = output.cleanSpecialCharForXPathTitle();
            //}
            //else
            //{
            //    output = "";
            //}
            return output;
        }

        public static String localizePath(this String parentPath, String childrenPath)
        {
            return childrenPath.removeStartsWith(parentPath);

            //if (output.StartsWith(parentPath))
            //{
            //    output = output.Replace(parentPath, "");
            //}
            //return output;
        }

        /// <summary>
        /// Konvertuje sve simbole za segmentaciju putanje u zadati spliter
        /// </summary>
        /// <param name="complexPath">Putanja koja ima razlicite simbole segmenata> . / $ itd</param>
        /// <param name="spliterToUse">Simbol koji ce biti primenjen umesto pronadjenih simbola</param>
        /// <returns></returns>
        /// \ingroup_disabled ace_ext_path
        public static String toUniformPath(this String complexPath, String spliterToUse = "/",
                                           Boolean spliterAtStart = false, Boolean spliterAtEnd = false)
        {
            if (String.IsNullOrEmpty(complexPath)) return "";
            complexPath = complexPath.Trim();
            complexPath = rg.Replace(complexPath, spliterToUse);
            complexPath = complexPath.Trim(spliterToUse.ToCharArray());

            if (spliterAtStart) complexPath = spliterToUse + complexPath;
            if (spliterAtEnd) complexPath = complexPath + spliterToUse;

            return complexPath;
        }

        /// <summary>
        /// Ako postoji / na početku putanje on će ga skloniti
        /// </summary>
        /// <param name="xPath">Putanja koju obrađuje</param>
        /// <returns>Obrađena putanja</returns>
        public static String getRelativePathVersion(this String xPath, Boolean removeEndSlash = false)
        {
            if (removeEndSlash)
            {
                if (xPath.EndsWith("/"))
                {
                    xPath = xPath.Substring(0, xPath.Length - 1);
                }
            }

            if (xPath.StartsWith("/"))
            {
                return xPath.Substring(1);
            }
            else
            {
                return xPath;
            }
        }

        /// <summary>
        /// Returns a version of XPath or any other path format
        /// </summary>
        /// <param name="xPath">Putanja koja se obrađuje. Primarno za xPath ali moze i neka druga</param>
        /// <param name="decrease">Ako je >0: Koliko elemenata u putanji da skloni (od kraja), ako je manje od 0 onda koliko krajnjih elemenata da pusti </param>
        /// <param name="splitter">Spliter, ako ostane Empty onda ce koristiti sve splitere -- ali ce biti bug kod sklapanja multi level putanje</param>
        /// <param name="trimOutput">Da li da trimuje pocetak i kraj rezultata -- skida spliter</param>
        /// <returns>Verzija putanje - odnosno dati segment</returns>
        /// \ingroup_disabled ace_ext_xpath_highlight
        public static String getPathVersion(this String xPath, int decrease, String splitter = "",
                                            Boolean trimOutput = false)
        {
            /*
            Regex rg = null;

            if (String.IsNullOrEmpty(splitter))
            {
                rg = imbProjectResourceExtensions.rg;
            } else
            {
                rg = new Regex("[" + splitter + "]");
            }
            */
            // new Regex(splitter);
            // rg.Split()

            char[] __chars;

            if (String.IsNullOrEmpty(splitter))
            {
                __chars = allPathSegments;
            }
            else
            {
                __chars = splitter.ToCharArray();
            }

            String output = xPath;

            String[] tmpArr = xPath.Split(__chars, StringSplitOptions.RemoveEmptyEntries);
            int depth = tmpArr.Length;

            int fromDepth = 0;
            int toDepth = depth - decrease;

            if (decrease < 0)
            {
                fromDepth = depth + decrease;
                toDepth = depth;
            }
            else
            {
            }

            if (toDepth > 0)
            {
                output = getPathVersion(xPath, fromDepth, toDepth, splitter);
            }
            else
            {
                output = "";
            }

            if (trimOutput)
            {
                output = output.Trim(splitter.ToCharArray());
            }
            return output;
        }

        /// <summary>
        /// Gets the path version for child nodes by skipping the deepest <c>rootSkip</c> path members
        /// </summary>
        /// <param name="xPath">Path string</param>
        /// <param name="rootSkip">How many members from root to skip</param>
        /// <param name="splitter">The splitter used for path</param>
        /// <returns>Shorter version of the path, without deepest <c>rootSkip</c> nodes</returns>
        public static String getPathVersionForChild(this String xPath, Int32 rootSkip = 1, String splitter = "\\")
        {
            String[] tmpArr = xPath.Split(splitter.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Int32 si = 0;
            String output = "";

            foreach (String part in tmpArr)
            {
                if (!(si < rootSkip))
                {
                    output = output.add(part, splitter);
                }
            }

            return output;
        }

        /// <summary>
        /// Returns a version of XPath (or any other split-path format)
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="fromDepth"></param>
        /// <param name="toDepth">Do kog elementa</param>
        /// <returns></returns>
        /// \ingroup_disabled ace_ext_xpath_highlight
        public static String getPathVersion(this String xPath, int fromDepth, int toDepth, String splitter = "")
        {
            char[] __chars;

            if (String.IsNullOrEmpty(splitter))
            {
                __chars = allPathSegments;
            }
            else
            {
                __chars = splitter.ToCharArray();
            }

            String output = xPath;

            String[] tmpArr = xPath.Split(__chars, StringSplitOptions.RemoveEmptyEntries);

            output = "";

            int p;
            for (p = fromDepth; p < toDepth; p++)
            {
                output = output + splitter + tmpArr[p];
            }

            return output;
        }

        /// <summary>
        /// Regex select groupPath : (\w+)[\\\s]*
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_groupPath = new Regex(@"(\w+)[\\\s]*", RegexOptions.Compiled);

        public static List<String> getPathParts(this String path)
        {
            path = path.ToLower().Trim();
            List<String> glist = new List<string>();
            MatchCollection mcs = _select_groupPath.Matches(path);
            foreach (Match mc in mcs)
            {
                glist.Add(mc.Value.ToString());
            }
            return glist;
        }

        ///// <summary>
        ///// 2014> Deli putanju u segmente - prema najnovijem standardu
        ///// </summary>
        ///// <param name="path">Putanja koja moze imati : / $ @ separatore</param>
        ///// <returns></returns>
        //public static List<string> getPathSegments(this String path)
        //{
        //    List<string> output = new List<string>();
        //    Int32 last = 0;
        //    if (path.Length == 1)
        //    {
        //        Match mt2 = rg.Match(path);
        //        if (mt2.Success)
        //        {
        //            output.Add(path);
        //            return output;
        //        }
        //    }
        //    Match mt = rg.Match(path, last);
        //    Boolean ok = mt.Success;
        //    if (!ok)
        //    {
        //        output.Add(path);
        //        return output;
        //    }

        //    while (ok)
        //    {
        //        Match next = null;

        //        next = rg.Match(path, mt.Index+mt.Value.Length);
        //        Int32 len = 0;
        //        String tmp = "";

        //        if (next.Success)
        //        {
        //            len = next.Index - mt.Index;

        //            tmp = path.Substring(mt.Index, len);
        //        } else
        //        {
        //            tmp = path.Substring(mt.Index);
        //            ok = false;
        //        }

        //        if (!String.IsNullOrEmpty(tmp)) output.Add(tmp);
        //        if (output.Count > 50)
        //        {
        //        }
        //        mt = next;
        //        ok = next.Success;
        //    }
        //    return output;
        //}
    }
}