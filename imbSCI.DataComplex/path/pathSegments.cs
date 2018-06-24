// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pathSegments.cs" company="imbVeles" >
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

    using imbSCI.Data;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    //using aceCommonTypes.extensions;

    #endregion imbVeles using

    /// <summary>
    /// Kolekcija segmenata
    /// </summary>
    public class pathSegments : List<pathSegment>
    {
        /// <summary>
        /// Regex select CollectionIndexer : [\[]{1}(\d)*[\]]{1}
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_CollectionIndexer = new Regex(@"([\[]{1}(\d)*[\]]{1})", RegexOptions.Compiled);

        public pathSegments()
        {
        }

        ///// <summary>
        ///// Konstruktor koji u isto vreme pravi kolekciju
        ///// </summary>
        ///// <param name="__path"></param>
        //public pathSegments(string __path)
        //{
        //    deployPath(__path);
        //}

        /// <summary>
        /// Konstruktor koji u isto vreme pravi kolekciju
        /// </summary>
        /// <param name="__path"></param>
        public pathSegments(string __path, pathResolveFlag _flagList)
        {
            deployPath(__path, _flagList);
        }

        /// <summary>
        /// Putanja na osnovu koje je napravljena kolekcija segmenta
        /// </summary>
        public string path { get; private set; }

        public override string ToString()
        {
            string output = "";
            foreach (var x in this)
            {
                output += x.prefix + x.needle;
            }
            return output;
        }

        /// <summary>
        /// Da li je u pitanju poslednji segment
        /// </summary>
        /// <param name="seg"></param>
        /// <returns></returns>
        public bool isThisLastSegment(pathSegment seg)
        {
            int segId = IndexOf(seg);
            return segId == (Count - 1);
        }

        /// <summary>
        /// Vraca sve segmente posle ovog
        /// </summary>
        /// <param name="seg">Segment od koga na dalje treba da se odsece deo</param>
        /// <returns></returns>
        public pathSegments getSegmentsAfterThis(pathSegment seg, bool includeThis)
        {
            pathSegments output = new pathSegments();
            if (Contains(seg))
            {
                int segId = IndexOf(seg);
                if (!includeThis) segId = segId + 1;
                List<pathSegment> copy = GetRange(segId, Count - segId);
                copy.ForEach(x => output.Add(x));
            }
            return output;
        }

        /// <summary>
        /// Match Evaluation for CollectionIndexer : _select_CollectionIndexer
        /// </summary>
        /// <param name="m">Match with value to process</param>
        /// <returns>For m.value "something" returns "SOMETHING"</returns>
        private static string _replace_CollectionIndexer(Match m)
        {
            string output = m.Value.Trim("[]".ToCharArray());
            output = "_" + output;

            return output;
        }

        /// <summary>
        /// Na osnovu dobijenog stringa pravi konstruise kolekciju
        /// </summary>
        /// <param name="__path"></param>
        public void deployPath(string __path, pathResolveFlag _flags)
        {
            Clear();
            pathResolveFlag flags = _flags;

            //if (flags == null) flags = new pathSegmentsFlags();

            path = __path;

            string workPath = __path;

            if (flags.HasFlag(pathResolveFlag.autorenameCollectionIndexer))
            {
                workPath = _select_CollectionIndexer.Replace(workPath, _replace_CollectionIndexer);
            }

            MatchCollection __segmentMatches = resourcePathResolver.regex_pathToSegments.Matches(workPath);

            if (__segmentMatches.Count == 0)
            {
                if (!path.isNullOrEmptyString())
                {
                    Add(pathSegment.create(imbProjectResourceBase.prefix_PROPERTY_PATH, workPath, Count));
                }
            }
            else
            {
                foreach (Match __smatch in __segmentMatches)
                {
                    Add(pathSegment.create(__smatch.Groups[2].Value, __smatch.Groups[1].Value, Count));
                }
            }
            if (Count > 0)
            {
                lastSegment = this.Last();
            }
        }

        #region --- lastSegment ------- Poslednji segment

        /// <summary>
        /// Poslednji segment
        /// </summary>
        public pathSegment lastSegment { get; set; }

        #endregion --- lastSegment ------- Poslednji segment
    }
}