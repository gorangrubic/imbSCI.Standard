// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportBaseExtensions.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.enums
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting.template;
    using imbSCI.Data.enums.fields;
    using System;

    public static class reportShortActions
    {
        #region SHORT STRING EXTNSION OPERATORS -----------------------------------------------------------------------------------------|

        /// <summary>
        /// Formats <c>fin</c> parameters into <c>format</c> string
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="fin">The fin.</param>
        /// <returns></returns>
        public static String f(this String format, params Object[] fin)
        {
            String output = String.Format(format, fin.getFlatArray<Object>());
            return output;
        }

        /// <summary>
        /// Adds template parameter placeholder
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string t(this String output, Enum input)
        {
            return output + (stringTemplateTools.PLACEHOLDER_Start + input.ToString() + stringTemplateTools.PLACEHOLDER_End);
        }

        /// <summary>
        /// Adds template parameter placeholder
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string t(this String output, templateFieldBasic input)
        {
            return output + (stringTemplateTools.PLACEHOLDER_Start + input.ToString() + stringTemplateTools.PLACEHOLDER_End);
        }

        /// <summary>
        /// Places in quote \"
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string q(this String output, String input)
        {
            return output.a("\"" + input.ToString() + "\"");
        }

        /// <summary>
        /// Short string concating
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string a(this String output, templateFieldBasic input)
        {
            return output.a(input.ToString());
        }

        /// <summary>
        /// synonim of a() - uses .ToString() on input object
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string o(this String output, Object input)
        {
            return output.a(input.ToString());
        }

        //public static object removeStartsWith(string path, string name)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// short string contacting - a from Add
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string a(this String output, String input)
        {
            String separator = " ";

            if (String.IsNullOrEmpty(input))
            {
                return output;
            }
            if (output.Length > 0)
            {
                if (!output.EndsWith(separator) && !input.StartsWith(separator))
                {
                    output += separator;
                }
            }

            output += input;

            return output;
        }

        #endregion SHORT STRING EXTNSION OPERATORS -----------------------------------------------------------------------------------------|
    }

    public enum reportBaseExtensions
    {
        txt,
        html,
        xml,
        log,
        css,
        js,

        /// <summary>
        /// Primenjuje custom ekstenziju u skladu sa izvestajem
        /// </summary>
        custom
    }
}