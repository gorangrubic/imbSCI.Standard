// --------------------------------------------------------------------------------------------------------------------
// <copyright file="conversionRule.cs" company="imbVeles" >
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
using imbSCI.Data;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace imbSCI.Core.syntax.converter.core
{
    public class conversionRule
    {
        /// <summary>
        /// Processes the line.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public String ProcessLine(String input)
        {
            String output = input;
            if (trim) input = input.Trim(" \t".ToArray());

            if (!startsWith.isNullOrEmpty())
            {
                if (!input.StartsWith(startsWith)) return output;
            }

            if (!endsWith.isNullOrEmpty())
            {
                if (!input.EndsWith(endsWith)) return output;
            }
            if (regex == null)
            {
                output = output.Replace(needle, replacement);
            }
            else
            {
                output = regex.Replace(output, replacement);
            }
            return output;
        }

        public conversionRule()
        {
        }

        public conversionRule SetStart(String input, String _replacement = "")
        {
            startsWith = input;
            if (_replacement != "")
            {
                needle = input;
                replacement = _replacement;
            }
            return this;
        }

        public conversionRule SetEnd(String input)
        {
            endsWith = input;
            return this;
        }

        public conversionRule SetReplace(String _needle, String _replacement)
        {
            needle = _needle;
            replacement = _replacement;
            return this;
        }

        public conversionRule SetRegex(String rgx, String _replacement)
        {
            regex = new Regex(rgx);
            replacement = _replacement;
            return this;
        }

        public Boolean trim { get; set; } = true;

        public String startsWith { get; set; } = "";

        public String endsWith { get; set; } = "";

        public String needle { get; set; } = "";

        public String replacement { get; set; }

        public Boolean multiline { get; set; } = false;

        public Regex regex { get; set; }
    }
}