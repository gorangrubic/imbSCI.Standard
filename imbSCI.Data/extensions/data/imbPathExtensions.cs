// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbPathExtensions.cs" company="imbVeles" >
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
namespace imbSCI.Data.extensions.data
{
    using System;

    //using imbSCI.Reporting.reporting.format;

    /// <summary>
    /// Universal Tree-structure access extensions and Path tools
    /// </summary>
    /// \ingroup_disabled ace_ext_path
    public static class imbPathExtensions
    {
        public const String DefaultXPathSplitter = "/";

        /// <summary>
        /// Returns common part of the path - the root that both paths are sharing
        /// </summary>
        /// <param name="xPathA">The x path a.</param>
        /// <param name="xPathB">The x path b.</param>
        /// <param name="includeAbsolute">if set to <c>true</c> [include absolute].</param>
        /// <param name="splitter">The splitter.</param>
        /// <returns></returns>
        /// \ingroup_disabled ace_ext_xpath_highlight
        public static String getCommonRoot(this String xPathA, String xPathB, Boolean includeAbsolute = false, String splitter = "")
        {
            String output = "";
            if (splitter.isNullOrEmpty()) splitter = DefaultXPathSplitter;
            if (String.IsNullOrEmpty(xPathB)) return "";
            if (String.IsNullOrEmpty(xPathB)) return "";

            if (includeAbsolute) output = splitter;
            String[] tmpA = xPathA.Split(splitter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            String[] tmpB = xPathB.Split(splitter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            Int32 a = tmpA.Length;
            if (a > tmpB.Length)
            {
                a = tmpB.Length;
            }

            Int32 i = 0;

            for (i = 0; i < a; i++)
            {
                output += tmpA[i] + splitter;
            }

            return output.TrimEnd(splitter.ToCharArray());
        }

        /// <summary>
        /// Removes common root from <c>xPathA</c> and returns result
        /// </summary>
        /// <param name="xPathA">The x path a.</param>
        /// <param name="xPathB">The x path b.</param>
        /// <param name="includeAbsolute">if set to <c>true</c> [include absolute].</param>
        /// <returns></returns>
        /// \ingroup_disabled ace_ext_xpath_highlight
        public static String removeCommonRoot(this String xPathA, String xPathB, Boolean includeAbsolute = false, String splitter = "")
        {
            String output = "";
            if (splitter.isNullOrEmpty()) splitter = DefaultXPathSplitter;
            //if (includeAbsolute) output = "/";

            String root = getCommonRoot(xPathA, xPathB, includeAbsolute);

            if (!String.IsNullOrEmpty(root))
            {
                xPathA = xPathA.TrimStart(splitter.ToCharArray());
                root = root.TrimStart(splitter.ToCharArray());

                if (xPathA.StartsWith(root))
                {
                    output += xPathA.Replace(root, "");
                }
                else
                {
                    output += xPathA;
                }
            }
            else
            {
                output = xPathA;
            }

            if (includeAbsolute) output = splitter + output;

            return output;
        }
    }
}