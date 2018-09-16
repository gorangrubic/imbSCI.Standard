// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileTextOperaterWorker.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace imbSCI.Core.files.search
{
    /// <summary>
    /// Helper class used for parallelization
    /// </summary>
    internal class fileTextOperaterWorker
    {
        private Regex rgex = null;
        private String needle = "";
        private List<String> needles = new List<string>();
        private List<Regex> rgexes = new List<Regex>();
        //private Int32 limitResult = -1;
        //public Int32 ln = 0;

        /// <summary>
        /// Instance constructor, when used for multiple needle search
        /// </summary>
        /// <param name="__needles">The needles.</param>
        /// <param name="useRegex">if set to <c>true</c> [use regex].</param>
        /// <param name="regexOptions">The regex options.</param>
        internal fileTextOperaterWorker(IEnumerable<String> __needles, Boolean useRegex = false, RegexOptions regexOptions = RegexOptions.None)
        {
            needles.AddRange(__needles);

            if (useRegex)
            {
                foreach (var nd in __needles)
                {
                    rgex = new Regex(nd, regexOptions);
                    rgexes.Add(rgex);
                }
            }
            else
            {
            }
        }

        private Boolean use_rgex
        {
            get
            {
                return (rgex != null) || (rgexes.Any());
            }
        }

        /// <summary>
        /// Instance constructor, when used for single needle search
        /// </summary>
        /// <param name="__needle">The needle.</param>
        /// <param name="useRegex">if set to <c>true</c> [use regex].</param>
        /// <param name="regexOptions">The regex options.</param>
        internal fileTextOperaterWorker(String __needle, Boolean useRegex = false, RegexOptions regexOptions = RegexOptions.None)
        {
            needle = __needle;
            if (useRegex)
            {
                rgex = new Regex(needle, regexOptions);
            }
            else
            {
            }
        }

        internal Boolean evaluate(String line, out String match)
        {
            if (needles.Any())
            {
                if (use_rgex)
                {
                    foreach (Regex rg in rgexes)
                    {
                        if (rg.IsMatch(line))
                        {
                            match = rg.ToString();
                            if (match == "") match = line;
                            return true;
                        }
                    }
                }
                else
                {
                    if (line.ContainsAny(needles, out match))
                    {
                        if (match == "") match = line;
                        return true; // output.Add(ln, line, false);
                    }
                }
            }
            else
            {
                if (use_rgex)
                {
                    if (rgex.IsMatch(line))
                    {
                        match = rgex.ToString();
                        if (match == "") match = line;
                        return true; // output.Add(ln, line, false);
                    }
                }
                else
                {
                    if (line.Contains(needle))
                    {
                        match = needle;
                        if (match == "") match = line;
                        return true; // output.Add(ln, line, false);
                    }
                }
            }
            match = "";
            return false;
        }
    }
}