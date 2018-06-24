// --------------------------------------------------------------------------------------------------------------------
// <copyright file="resourcePathResolver.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.path
{
    #region imbVeles using

    using imbSCI.Core.extensions.text;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    //using aceCommonTypes.extensions;

    #endregion imbVeles using

    public class imbProjectResourceBase
    {
        /// <summary>
        /// Kada nema parent-a
        /// </summary>
        public const String prefix_NOPARENT_PATH = "!";

        public const String prefix_CHILD_PATH = "/";

        /// <summary>
        /// * -- pristupa imbTypeInfo objektu preko njegovog imena, moze se pozivati i Enum type
        /// </summary>
        public const String prefix_TYPEBYNAME = "*";

        /// <summary>
        /// ^ -- oznacava konretan operation enum item, podrzava i Enum path !!!
        /// </summary>
        public const String prefix_OPERATIONENUM = "^";

        /// <summary>
        /// $ -- property real name ili object name
        /// </summary>
        public const String prefix_INTEGRATED_PATH = "$";

        /// <summary>
        /// . -- obican pristup propertiju, ili Enum memberu
        /// </summary>
        public const String prefix_PROPERTY_PATH = ".";

        public const String prefix_PROJECT_PATH = ":";

        public const String prefix_STRING_LIST = "#";

        public const String prefix_COLLECTION_QUERY = "?";

        public const String prefix_LINKED_PROPERTY_PATH = "@";

        public const String prefix_PROPERTYINFO = ">";

        public const String prefix_PROPERTY_OF_GENERIC = "<";

        //public const String prefix_TYPOLOGY_BY_NAME = "*";

        public const String prefix_GLOBAL_RESOURCE = "~";

        /// <summary>
        /// Koristi se samo za detekciju - format se koristi za pravljenje
        /// </summary>
        public const String prefix_COLLECTION_INDEX_ACCESS = "[";

        public const String sufix_COLLECTION_INDEX_ACCESS = "]";

        /// <summary>
        /// Format - nije prefix
        /// </summary>
        ///
        public const String format_COLLECTION_INDEX_ACCESS = "[{0}]";

        public const String format_COLLECTION_KEY_ACCESS = "[\"{0}\"]";

        public const String separator_TYPEPATH = "_";
    }

    /// <summary>
    /// Staticki alati za dobijanje objekta/resursa na osnovu imb putanje
    /// </summary>
    public static class resourcePathResolver
    {
        #region --- regex_pathToSegments ------- Regex iskaz koji vraca sve segmente putanje, grupisane> [0] prefix, [1] needle

        private static Regex _regex_pathToSegments;

        /// <summary>
        /// Regex iskaz koji vraca sve segmente putanje, grupisane> [0] prefix, [1] needle
        /// </summary>
        internal static Regex regex_pathToSegments
        {
            get
            {
                if (_regex_pathToSegments == null)
                {
                    string __exp = new string(prefixVsFormat.allPrefixes());
                    string __reg = "([" + __exp.escapeForRegex() + "])([\\s\\w]*)";

                    _regex_pathToSegments = new Regex(__reg);
                }
                return _regex_pathToSegments;
            }
        }

        #endregion --- regex_pathToSegments ------- Regex iskaz koji vraca sve segmente putanje, grupisane> [0] prefix, [1] needle

        #region --- cached_pathSegments ------- kesirani path segmenti prema ulaznim putanjama

        private static Dictionary<string, pathSegments> _cached_pathSegments;

        /// <summary>
        /// kesirani path segmenti prema ulaznim putanjama
        /// </summary>
        internal static Dictionary<string, pathSegments> cached_pathSegments
        {
            get
            {
                if (_cached_pathSegments == null)
                {
                    _cached_pathSegments = new Dictionary<string, pathSegments>();
                }
                return _cached_pathSegments;
            }
        }

        #endregion --- cached_pathSegments ------- kesirani path segmenti prema ulaznim putanjama

        /// <summary>
        /// 0 - parent collection path, 1 - key to find with
        /// </summary>
        internal const string pathFormat_THISINDEXER = "{0}[{1}]";

        private static pathElementFormatCollection _prefixVsFormat;

        /// <summary>
        /// kolekcija path formatiranja prema prefixu
        /// </summary>
        internal static pathElementFormatCollection prefixVsFormat
        {
            get
            {
                if (_prefixVsFormat == null)
                {
                    _prefixVsFormat = new pathElementFormatCollection();
                    _prefixVsFormat.autosetup(typeof(imbProjectResourceBase));
                }
                return _prefixVsFormat;
            }
        }

        /// <summary>
        /// 2014> Deli putanju u segmente - prema najnovijem standardu - koristi kesiranje za putanje
        /// </summary>
        /// <param name="path">Putanja koja moze imati : / $ @ separatore</param>
        /// <returns></returns>
        internal static pathSegments toPathSegments(this string path)
        {
            if (path.isNullOrEmptyString()) return new pathSegments();
            if (!cached_pathSegments.ContainsKey(path))
            {
                cached_pathSegments.Add(path, new pathSegments(path, pathResolveFlag.none));
            }

            return cached_pathSegments[path];
        }

        /// <summary>
        /// Vraca prefix string na osnovu tipa odnosa
        /// </summary>
        /// <param name="parentRealtionType"></param>
        /// <param name="def"></param>
        /// <returns>odgovarajuci prefix string</returns>
        public static string toPrefixString(this resourceRelationTypes parentRealtionType,
                                            string def = imbProjectResourceBase.prefix_CHILD_PATH)
        {
            string sep = def;

            switch (parentRealtionType)
            {
                case resourceRelationTypes.linkedResource:
                    sep = imbProjectResourceBase.prefix_LINKED_PROPERTY_PATH;
                    break;

                case resourceRelationTypes.integratedResource:
                    sep = imbProjectResourceBase.prefix_INTEGRATED_PATH;
                    break;

                case resourceRelationTypes.nestedResource:
                    sep = imbProjectResourceBase.prefix_INTEGRATED_PATH;
                    break;

                case resourceRelationTypes.indexerItem:
                    sep = imbProjectResourceBase.prefix_COLLECTION_INDEX_ACCESS;
                    break;

                case resourceRelationTypes.integratedSimpleObject:
                case resourceRelationTypes.simpleProperties:
                case resourceRelationTypes.temporaryObjects:
                    sep = imbProjectResourceBase.prefix_PROPERTY_PATH;
                    break;

                case resourceRelationTypes.childResource:
                default:
                    sep = imbProjectResourceBase.prefix_CHILD_PATH;
                    break;
            }
            return sep;
        }

        /// <summary>
        /// 2014 Maj> Univerzalni alat za resavanje putanje - uzima project kao source i poziva resolvePathSegment() metod - podrzava sve vrste linkova
        /// </summary>
        /// <param name="path">Univerzalni path - podrzava sve specijalne prefikse: / # . : # _ @</param>
        /// <param name="returnPropertyInfo">Da li da vrati imbPropertyInfo umesto vrednosti - vazi za> @ $ . : ,</param>
        /// <returns>Objekat koji se nalazi na putanji</returns>
        /// <remarks>
        /// <para>Upit preko stringa - bez korenskog objekta -- koristi ga debugTools i providerCase</para>
        /// </remarks>
        public static object resolvePath(this string path, pathResolveFlag flags)
        {
            // imbCoreManager.managerInstance.__myProjectObject
            object source = null;
            // prepravljeno 2014. 18. sept
            return resolvePath(source, path, flags);
        }

        /// <summary>
        /// Najčešće korišćen metod za upit nad putanjom i objektom
        /// </summary>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static object resolvePath(this object source, string path, pathResolveFlag flags)
        // , Boolean returnNullIfPathEmpty = true, Boolean returnPropertyInfo = false, Boolean debugMode=false
        {
            pathResolveFlag _flags = flags;

            object obj = resolvePath(source, path, _flags);

            if (obj == null)
            {
                if (!_flags.HasFlag(pathResolveFlag.nullIsAcceptable))
                {
                    Exception ex = new ArgumentOutOfRangeException("Resource retrieved from path [" + path + "] is null ", nameof(path));
                    throw ex;
                }
            }
            else
            {
            }

            return obj;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static T resolvePath<T>(this object source, string path, pathResolveFlag flags)
            where T : imbProjectResourceBase
        {
            object obj = _resolvePath(source, path, flags);
            if (obj is T)
            {
                return obj as T;
            }
            else if (obj == null)
            {
                Exception ex = new ArgumentOutOfRangeException("Resource retrieved from path [" + path + "] is null ", nameof(path));
                throw ex;
            }
            else
            {
                Exception ex = new ArgumentOutOfRangeException("Resource retrieved from path [" + path + "] is nor null and [" +
                                                               typeof(T).Name +
                                                               "]", nameof(path));
                throw ex;
            }
            return obj as T;
        }

        /// <summary>
        /// Pronalazi pod resurs na osnovu date putanje
        /// </summary>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <param name="returnNullIfPathEmpty"></param>
        /// <returns></returns>
        private static object _resolvePath(this object source, string path, pathResolveFlag flagList)
        // , Boolean returnNullIfPathEmpty = true, Boolean returnPropertyInfo = false, Boolean debugMode=false
        {
            imbProjectResourceBase baseResource = null;

            if (!string.IsNullOrEmpty(path))
            {
                path = path.Replace("{", "<");
                path = path.Replace("}", ">");

                if (flagList.HasFlag(pathResolveFlag.removeTypeAndRelationFilters))
                {
                    pathFilterInstructions ins = new pathFilterInstructions(path);
                    path = ins.cleanPath;
                }
            }

            if (flagList.HasFlag(pathResolveFlag.startFromProjectRoot))
            {
                // source = imbCoreManager.managerInstance.__myProjectObject;
            }

            bool returnPropertyInfo = flagList.HasFlag(pathResolveFlag.returnPropertyInfo);

            if (path.isNullOrEmptyString())
            {
                if (flagList.HasFlag(pathResolveFlag.returnNullIfPathEmpty))
                {
                    return null;
                }
                else
                {
                    return source;
                }
            }

            object head = source;
            object lastHead = source;
            object result = null;
            StringBuilder msg = new StringBuilder();
            pathSegments segments = path.toPathSegments();
            foreach (pathSegment seg in segments)
            {
                lastHead = head;
                result = resourcePathSegmentResolver.resolvePathSegment(head, seg, flagList);

                if (result == null)
                {
                    msg.AppendLine("Last head: " + lastHead.GetType().Name + " on segment: " + seg.position);
                    head = null;
                    // if (msg == null) msg = new imbStringBuilder(0);
                    msg.AppendLine(
                        "Path segment: " + seg.description + " failed (returned [null]) on source [" +
                        source.toStringSafe() + "]");
                    break;
                }
                else if (result == head)
                {
                    // if (msg == null) msg = new imbStringBuilder(0);

                    //msg.AppendLine();
                }
                else
                {
                    if (flagList.HasFlag(pathResolveFlag.debugMode))
                    {
                        if (msg == null) msg = new StringBuilder();
                        msg.AppendLine("Path segment: " + seg.description + " returned result [" + result.toStringSafe() +
                                       "] on current head [" + head.toStringSafe() + "]");
                    }
                    head = result;
                }
            }

            //if (msg !=null)
            //{
            msg.AppendLine("Resolving path [" + path + "] - segments count [" + segments.Count +
                           "] on source [" + source.toStringSafe() + "]");

            if (head == null)
            {
                msg.AppendLine("Resolving failed! result is null");
            }
            else
            {
                msg.AppendLine("Resolve result ok :: [" + head.toStringSafe() + "]");
            }

            if (flagList.HasFlag(pathResolveFlag.debugMode))
            {
                head = null;
            }
            else
            {
                //devNoteManager.noteStatic(new aceGeneralException(msg.ToString()), devNoteType.resourceRetrieval, msg.ToString());
            }
            //}

            if (!flagList.HasFlag(pathResolveFlag.nullIsAcceptable))
            {
                //if (head == null)
                //{
                //    msg.AppendPair("Query path: ", path);
                //    if (source is IObjectWithName)
                //    {
                //        IObjectWithName ion = source as IObjectWithName;
                //        msg.AppendPair("Source: ", ion.name);
                //    }
                //    else
                //    {
                //        msg.AppendPair("Source: ", source.toStringSafe());
                //    }

                //    IObjectWithName lastHeadWithName = lastHead as IObjectWithName;
                //    if (lastHeadWithName != null)
                //    {
                //        msg.AppendPair("Last head: ", lastHeadWithName.name);
                //    }
                //    else
                //    {
                //        msg.AppendPair("Last head: ", lastHead.toStringSafe());
                //    }

                //    if (!flagList.Contains(pathResolveFlag.disableDoomyObject))
                //    {
                //        var dbg = new debugDoomyObject();
                //        //msg.AppendLine("Path: " + path);

                //        dbg.data = new object[] {segments, head, lastHead, source, flagList};

                //        dbg.report = msg.ToString();

                //        head = dbg;
                //    }

                //  //  devNoteManager.note(source, msg.ToString(), "resourcePathResolver.resolvePath()");
                //}
            }

            return head;
        }

        //private static Object resolvePathSegment(Object source, pathSegment segment, params pathResolveFlags[] flags)
        //{
        //    return resolvePathSegment(source, segment, flags.ToList());
        //}
    }
}