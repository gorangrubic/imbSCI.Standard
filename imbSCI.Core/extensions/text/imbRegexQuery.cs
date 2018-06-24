// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbRegexQuery.cs" company="imbVeles" >
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
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Regex data model
    /// </summary>
    public class imbRegexQuery
    {
        public Regex regex;

        #region --- options ------- Regex dodatne opcije

        private RegexOptions _options;

        /// <summary>
        /// Regex dodatne opcije
        /// </summary>
        public RegexOptions options
        {
            get { return _options; }
            set
            {
                _options = value;
            }
        }

        #endregion --- options ------- Regex dodatne opcije

        #region --- expression ------- Regex expression u String formatu

        private String _expression;

        /// <summary>
        /// Regex expression u String formatu
        /// </summary>
        public String expression
        {
            get { return _expression; }
            set
            {
                _expression = value;
            }
        }

        #endregion --- expression ------- Regex expression u String formatu

        public String getOptionsString(String prefixStr = "", String sufixStr = "")
        {
            switch (options)
            {
                case RegexOptions.Multiline:
                    return prefixStr + "m" + sufixStr;

                case RegexOptions.Singleline:
                    return prefixStr + "s" + sufixStr;

                case RegexOptions.IgnoreCase:
                    return prefixStr + "i" + sufixStr;
                    break;

                default:
                    return "";
                    break;
            }
            return "";
        }

        public Regex getRegex()
        {
            regex = new Regex(expression, options);
            return regex;
        }
    }
}