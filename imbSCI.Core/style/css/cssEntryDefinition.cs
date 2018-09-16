// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cssEntryDefinition.cs" company="imbVeles" >
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.Core.style.css
{
    /// <summary>
    /// Dictionary maintaining style inline data/parameters - used by SVG, mxGraph, HTML and other XML based outputs
    /// </summary>
    public class cssEntryDefinition : IEnumerable<KeyValuePair<String, String>>
    {
        public String name { get; set; }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(String key)
        {
            if (parameters.ContainsKey(key)) parameters.Remove(key);
        }

        /// <summary>
        /// Universal get and set indexer
        /// </summary>
        /// <value>
        /// The <see cref="String"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public String this[String key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Set(key, value);
            }
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set(String key, String value)
        {
            if (value.isNullOrEmpty())
            {
                if (parameters.ContainsKey(key)) parameters.Remove(key);
                return;
            }
            if (!parameters.ContainsKey(key))
            {
                parameters.Add(key, value);
            }
            else
            {
                parameters[key] = value;
            }
        }

        /// <summary>
        /// Appends value at specified key.(or adds new entry)
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="separator">The separator - added between existing and new content.</param>
        public void Append(String key, String value, String separator = " ")
        {
            String tmp = "";
            if (parameters.ContainsKey(key))
            {
                tmp = parameters[key];
            }
            tmp = tmp.add(value, separator);
            Set(key, tmp);
        }

        public void Merge(cssEntryDefinition source)
        {
            foreach (KeyValuePair<String, String> pair in source.parameters)
            {
                Set(pair.Key, pair.Value);
            }
        }

        public Boolean ContainsKey(String key)
        {
            return parameters.ContainsKey(key);
        }

        /// <summary>
        /// Universal get
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public String Get(String key)
        {
            if (parameters.ContainsKey(key))
            {
                return parameters[key];
            }
            return "";
        }

        public Boolean Any
        {
            get
            {
                return parameters.Any();
            }
        }

        /// <summary>
        /// Format set as default for this dictionary
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public syntaxFormat mainFormat { get; set; } = syntaxFormat.htmlStyleFormat;

        /// <summary>
        /// In-line string format
        /// </summary>
        public enum syntaxFormat
        {
            notSet,

            /// <summary>
            /// The HTML style format: [property]:[value];
            /// </summary>
            htmlStyleFormat,

            /// <summary>
            /// The CSS file format: block with entry name and \{ \} wrapper
            /// </summary>
            cssFileFormat,

            /// <summary>
            /// The HTML style format inline: only value for style attribute and without newlines
            /// </summary>
            htmlStyleFormatInline,
        }

        public static Regex REGEX_HTMLSTYLE = new Regex("\\b([\\w]+):([#\\d\\w\\s\\%\\.\\,\\-\\(\\)\\?\\!\\=]*);");

        public static Regex REGEX_CSSSTYLE = new Regex("\\b([\\w]+)=([#\\d\\w\\s\\%\\.\\,\\-\\:\\(\\)\\?\\!]*);");

        public static Regex REGEX_CSSATTRIBUTE = new Regex("\\s*([\\.\\[\\]\\@\\w\\d\\-]*):\\s*([\\#\\d\\w\\s\\%\\'\\+\\`\\\"\\.\\,\\-\\:/\\(\\)\\?\\!]*);?");

        /// <summary>
        /// Gets the pair regex.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static Regex GetPairRegex(syntaxFormat format)
        {
            switch (format)
            {
                default:
                case syntaxFormat.cssFileFormat:
                    return REGEX_CSSATTRIBUTE;
                    break;

                case syntaxFormat.htmlStyleFormat:
                    return REGEX_HTMLSTYLE;
                    break;
            }
        }

        /// <summary>
        /// Gets the pair format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static String GetPairFormat(syntaxFormat format)
        {
            switch (format)
            {
                default:
                case syntaxFormat.htmlStyleFormatInline:
                case syntaxFormat.htmlStyleFormat:
                    return "{0}:{1};";

                case syntaxFormat.cssFileFormat:
                    return "    {0}: {1};";
                    break;
            }
        }

        /// <summary>
        /// Dictionary for parameters given as inline string
        /// </summary>
        public cssEntryDefinition()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="cssEntryDefinition"/> class.
        /// </summary>
        /// <param name="format">The format.</param>
        public cssEntryDefinition(syntaxFormat format)
        {
            mainFormat = format;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="cssEntryDefinition" /> class.
        /// </summary>
        /// <param name="cssName">CSS entry name</param>
        /// <param name="cssCode">The CSS code, block within { } .</param>
        public cssEntryDefinition(String cssName, String cssCode)
        {
            fromString(cssCode, syntaxFormat.cssFileFormat);
            name = cssName;
        }

        /// <summary>
        /// The regex entryselector: group[1] is name, group[2] contains the attribute pairs
        /// </summary>
        public static Regex REGEX_ENTRYSELECTOR = new Regex("([\\.#]?[\\w\\d\\-_]*)\\s*\\{([\\(\\)\\n\\r\\w\\d\\.\\,\\%\\\"\\`\\'\\*\\:\\-\\;\\s]*)\\}");

        public static String FORMAT_ENTRYFORMAT = "{0} {\r\n{1}\r\n}";

        /// <summary>
        /// Populates parameters from inline string values, given in <see cref="syntaxFormat"/>
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="format">The format.</param>
        public void fromString(String input, syntaxFormat format = syntaxFormat.notSet)
        {
            if (format == syntaxFormat.notSet) format = mainFormat;

            var rx = GetPairRegex(format);
            MatchCollection mch = rx.Matches(input);

            foreach (Match m in mch)
            {
                if (m.Success)
                {
                    Set(m.Groups[1].Value, m.Groups[2].Value);
                }
            }
        }

        /// <summary>
        /// Renders entry definition to string format
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public String ToString(syntaxFormat format = syntaxFormat.notSet)
        {
            if (format == syntaxFormat.notSet) format = mainFormat;
            StringBuilder sb = new StringBuilder();
            String formatLine = GetPairFormat(format);

            if (format == syntaxFormat.cssFileFormat)
            {
                sb.AppendLine(name + " {");
            }
            foreach (var pair in parameters)
            {
                sb.AppendFormat(formatLine, pair.Key, pair.Value);
                if (format != syntaxFormat.htmlStyleFormatInline) sb.Append(Environment.NewLine);
            }
            sb.AppendLine("}");

            return sb.ToString();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, string>>)parameters).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, string>>)parameters).GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        protected Dictionary<String, String> parameters { get; set; } = new Dictionary<string, string>();
    }
}