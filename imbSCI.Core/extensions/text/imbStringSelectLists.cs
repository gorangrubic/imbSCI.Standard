// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringSelectLists.cs" company="imbVeles" >
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
    using System.Text.RegularExpressions;

    /// <summary>
    /// Selects proper URLs, email@adresses, #tags.
    /// </summary>
    /// \ingroup_disabled ace_ext_string
    public static class imbStringSelectLists
    {
        //public static IEnumerable<string> LowercaseUsernames(string input, int minCharacters, int maxCharacters)
        //{
        //    var regEx = "^[a-z0-9_-]{3,16}$";
        //    regEx = regEx.Replace("3", minCharacters.ToString());
        //    regEx = regEx.Replace("16", maxCharacters.ToString());
        //    var matches = Regex.Matches(input, regEx, RegexOptions.IgnoreCase);
        //    var matchedStrings = new List<string>();
        //    for (int i = 0; i < matches.Count; i++)
        //    {
        //        matchedStrings.Add(matches[i].Value);
        //    }
        //    return matchedStrings;
        //}

        /// <summary>
        /// Matches URLs in string using Regex
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IEnumerable<string> imbGetUrls(this string input)
        {
            var matches = Regex.Matches(input, @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                                      RegexOptions.IgnoreCase);
            var matchedStrings = new List<string>();
            for (int i = 0; i < matches.Count; i++)
            {
                matchedStrings.Add(matches[i].Value);
            }
            return matchedStrings;
        }

        /// <summary>
        /// Matches all email addresses in string using Regex
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IEnumerable<string> imbGetEmails(this string input)
        {
            var matches = Regex.Matches(input, @"(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})",
                                      RegexOptions.IgnoreCase);
            var matchedStrings = new List<string>();
            for (int i = 0; i < matches.Count; i++)
            {
                matchedStrings.Add(matches[i].Value);
            }
            return matchedStrings;
        }

        /// <summary>
        /// Matches all hashtags in string using Regex
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IEnumerable<string> imbGetHashtags(this string input)
        {
            var matches = Regex.Matches(input, @"(^|[^0-9A-Z&/]+)(#|\uFF03)([0-9A-Z_]*[A-Z_]+[a-z0-9_\\u00c0-\\u00d6\\u00d8-\\u00f6\\u00f8-\\u00ff]*)",
                                      RegexOptions.IgnoreCase);
            var matchedStrings = new List<string>();
            for (int i = 0; i < matches.Count; i++)
            {
                matchedStrings.Add(matches[i].Value);
            }
            return matchedStrings;
        }
    }
}