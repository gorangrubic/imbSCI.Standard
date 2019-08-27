// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbRegex.cs" company="imbVeles" >
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
    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    #endregion imbVeles using

    public static class imbRegex
    {
        #region --- escapePairs ------- static and autoinitiated object

        private static Dictionary<String, String> _escapePairs;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static Dictionary<String, String> escapePairs
        {
            get
            {
                if (_escapePairs == null)
                {
                    _escapePairs = new Dictionary<String, String>();
                    _escapePairs.Add(@"\", @"\\");
                    _escapePairs.Add(@"*", @"\*");
                    _escapePairs.Add(@"+", @"\+");
                    _escapePairs.Add(@"?", @"\?");
                    _escapePairs.Add(@"|", @"\|");
                    _escapePairs.Add(@"{", @"\{");
                    _escapePairs.Add(@"[", @"\[");
                    _escapePairs.Add(@"(", @"\(");
                    _escapePairs.Add(@")", @"\)");
                    _escapePairs.Add(@"^", @"\^");
                    _escapePairs.Add(@"$", @"\$");
                    _escapePairs.Add(@".", @"\.");
                    _escapePairs.Add(@"#", @"\#");

                    _escapePairs.Add(@" ", @"\ ");
                    _escapePairs.Add(@">", @"\>");
                    _escapePairs.Add(@"<", @"\<");
                    _escapePairs.Add(@"]", @"\]");
                    _escapePairs.Add(@"}", @"\}");
                }
                return _escapePairs;
            }
        }

        #endregion --- escapePairs ------- static and autoinitiated object

        /// <summary>
        /// Prosiren REGEX unescape
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String unescapeForRegex(this String input)
        {
            return escapePairs.unescapeString(input);
        }

        /// <summary>
        /// Prosiren REGEX escape
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String escapeForRegex(this String input)
        {
            return escapePairs.escapeString(input);
        }

        public const String REGEX_SELECTALL = "[.]*";
        

        public static Regex SearchPatternToRegex(this String searchPattern)
        {
            searchPattern = Regex.Escape(searchPattern);
            searchPattern = searchPattern.Replace("*", ".*");
            return new Regex(searchPattern);
        }



        public static imbRegexQuery createCompareRegexQuery(this String operantB, stringMatchPolicy compareMode)
        {
            imbRegexQuery output = new imbRegexQuery();

            switch (compareMode)
            {
                case stringMatchPolicy.caseFree:
                    output.expression = @"(\b|^)" + operantB + @"(\b|\Z)";
                    output.options = RegexOptions.IgnoreCase;
                    break;

                case stringMatchPolicy.length:
                    output.expression = @"(\b|^)[\w]{" + operantB.Length + @"}(\b|\>)";
                    break;

                case stringMatchPolicy.lengthMore:
                    output.expression = @"(\b|^)[\w]{" + operantB.Length + @",}(\b|\>)";
                    break;

                case stringMatchPolicy.lengthLess:
                    output.expression = @"(\b|^)[\w]{1," + (operantB.Length - 1) + @"}(\b|\>)";
                    break;

                case stringMatchPolicy.containCaseFree:
                    output.expression = operantB;
                    output.options = RegexOptions.IgnoreCase;
                    break;

                case stringMatchPolicy.containExact:
                    output.expression = operantB;
                    break;

                case stringMatchPolicy.trimSpaceExact:
                    output.expression = "^" + operantB.Trim() + @"\Z";

                    break;

                case stringMatchPolicy.trimSpaceCaseFree:
                    output.expression = "^" + operantB.Trim();
                    output.options = RegexOptions.IgnoreCase;
                    break;

                case stringMatchPolicy.trimSpaceContainExact:
                    output.expression = operantB.Trim();
                    break;

                case stringMatchPolicy.trimSpaceContainCaseFree:
                    output.expression = operantB.Trim();
                    output.options = RegexOptions.IgnoreCase;
                    break;

                case stringMatchPolicy.overrideFalse:
                    output.expression = "";
                    break;

                case stringMatchPolicy.overrideTrue:
                    output.expression = ".";
                    break;

                case stringMatchPolicy.exact:
                default:
                    output.expression = "^" + operantB + @"\Z";
                    break;
            }

            return output;
        }
    }
}