// --------------------------------------------------------------------------------------------------------------------
// <copyright file="stringTemplateTools.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.template
{
    #region imbVeles using

    using System;
    using System.Text.RegularExpressions;

    #endregion imbVeles using

    /// <summary>
    /// Common tool to work with {{{ }}} and {{ }} placeholders
    /// </summary>
    public static class stringTemplateTools
    {
        /// <summary>
        /// 2nd order placeholder _ prefix
        /// </summary>
        public static String PLACEHOLDER_2ND_Start = "{{";

        /// <summary>
        /// 2nd order placeholder _ sufix
        /// </summary>
        public static String PLACEHOLDER_2ND_End = "}}";

        /// <summary>
        /// 1st order placeholder _ prefix
        /// </summary>
        public static String PLACEHOLDER_Start = "{{{";

        /// <summary>
        /// 1st order placeholder _ sufix
        /// </summary>
        public static String PLACEHOLDER_End = "}}}";

        /// <summary>
        /// regex format za preuzimanje svih format placeholdera
        /// </summary>
        //public static String regex_placeholders = @"[\{](\d+)[\}]";

        #region --- regex_placeholders ------- REGEX upit za izdvajanje placeholdera iz templejta

        internal static String _regex_placeholders = null;

        /// <summary>
        /// The regex import
        /// </summary>
        internal static Regex _regex_import;

        private static string _regex_placeholders_2nd;
        internal static Regex _regex_import_2nd;

        /// <summary>
        /// Flag : da li da koristi String.Format() ili sopstvenu regex implementaciju
        /// </summary>
        public static Boolean _useStringFormatAPI
        {
            get { return false; }
        }

        /// <summary>
        /// REGEX upit za izdvajanje placeholdera iz templejta
        /// </summary>
        public static String regex_placeholders
        {
            get
            {
                if (_regex_placeholders == null)
                {
                    if (_useStringFormatAPI)
                    {
                        _regex_placeholders = @"[" + Regex.Escape(PLACEHOLDER_Start) + @"](\d+)[" +
                                              Regex.Escape(PLACEHOLDER_End) + "]";
                    }
                    else
                    {
                        _regex_placeholders = @"[" + Regex.Escape(PLACEHOLDER_Start) + @"](\w+)[" +
                                              Regex.Escape(PLACEHOLDER_End) + "]";
                    }
                }
                return _regex_placeholders;
            }
        }

        /// <summary>
        /// REGEX upit za izdvajanje placeholdera iz templejta
        /// </summary>
        public static String regex_placeholders_2nd
        {
            get
            {
                if (_regex_placeholders_2nd == null)
                {
                    if (_useStringFormatAPI)
                    {
                        _regex_placeholders_2nd = @"[" + Regex.Escape(PLACEHOLDER_2ND_Start) + @"](\d+)[" +
                                              Regex.Escape(PLACEHOLDER_2ND_End) + "]";
                    }
                    else
                    {
                        _regex_placeholders_2nd = @"[" + Regex.Escape(PLACEHOLDER_2ND_Start) + @"](\w+)[" +
                                              Regex.Escape(PLACEHOLDER_2ND_End) + "]";
                    }
                }
                return _regex_placeholders_2nd;
            }
        }

        #endregion --- regex_placeholders ------- REGEX upit za izdvajanje placeholdera iz templejta

        /// <summary>
        /// regex za izdvajanje placeholdera u templejtu
        /// </summary>
        public static Regex regex_import
        {
            get
            {
                _regex_import = new Regex(regex_placeholders);
                return _regex_import;
            }
        }

        /// <summary>
        /// Gets the regex import 2ND.
        /// </summary>
        /// <value>
        /// The regex import 2ND.
        /// </value>
        public static Regex regex_import_2nd
        {
            get
            {
                _regex_import_2nd = new Regex(regex_placeholders_2nd);
                return _regex_import_2nd;
            }
        }

        /// <summary>
        /// Vraca string koji definise placeholder u template stringu
        /// </summary>
        /// <param name="__name">The name.</param>
        /// <param name="secondOrder">if set to <c>true</c> [second order].</param>
        /// <returns>
        /// string koji se ubacuje u template string
        /// </returns>
        public static String renderToTemplate(this Object __name, Boolean secondOrder = false)
        {
            String output = "";
            String name = "";
            if (__name is String)
            {
                name = __name as String;
            }
            else if (__name is Enum)
            {
                name = __name.ToString();
            }
            else
            {
                name = __name.ToString();
            }

            if (secondOrder)
            {
                output = PLACEHOLDER_2ND_Start + name + PLACEHOLDER_2ND_End;
            }
            else
            {
                output = PLACEHOLDER_Start + name + PLACEHOLDER_End;
            }

            return output;
        }
    }
}