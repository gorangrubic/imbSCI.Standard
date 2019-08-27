// --------------------------------------------------------------------------------------------------------------------
// <copyright file="regexMarker.cs" company="imbVeles" >
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
namespace imbSCI.Data.data.text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public interface IRegexFormatMarker:IRegexMarker
    {
          String Convert(List<String> input);
    }

    public interface IRegexMarker
    {

       // String format { get; set; } 

        /// <summary>
        /// Gets or sets the test.
        /// </summary>
        /// <value>
        /// The test.
        /// </value>
        Regex test { get; set; }

        Object marker { get; }

      

        String replacementGenerator(Match m);

        String ReplaceMatch(Match m, String input, String replacement="");
    }



    public class regexFormatMarker<T>:regexMarker<T>, IRegexFormatMarker
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="regexMarker{T}"/> class.
        /// </summary>
        /// <param name="_format">String format, e.g.: "-.{0}.-"</param>
        /// <param name="_regexInsert">Insert to be placed in the format - e.g.: "([\w]+)" to have "-.([\w]+).-" regex selecting parameter in "-.{0}.-" format</param>
        /// <param name="__m">The m.</param>
        public regexFormatMarker(String _regex, T __m)
        {
            marker = __m;

            DeployFormatOnly(regex);
        }

        public regexFormatMarker(T __m, String _format, String _regex, params String[] _regexes)
        {
            marker = __m;
            List<string> rx = new List<string>() { _regex };

            if (_regexes != null)
            {
                rx.AddRange(_regexes);
            } 
           

            Deploy(_format, rx);
        }

        protected void DeployFormatOnly(String _format)
        {
            if (_format.Contains("{0}"))
            {
                format = _format;
                regex = String.Format(format, DEFAULTREGEX);
                parameter_count = 1;
            }
            else
            {
                regex =  Regex.Escape(_format);
                format = "{0}";
                parameter_count = 1;
                
            }

            test = new Regex(regex);
        }

        public const String DEFAULTREGEX = @"([\w]+)";

        protected void Deploy(String _format, List<String> _regexes)
        {
            regex = String.Format(_format, _regexes);
            format = _format;
            parameter_count = _regexes.Count;
        }


        protected void DeployFromRegex(String _regex)
        {
            regex = _regex;
            test = new Regex(regex);

            format = regex;

            var fm = selectRegexGroups.Matches(_regex);
            Int32 i = 0;
            foreach (Match m in fm)
            {
                format = ReplaceMatch(m, format, "{" + i + "}");
                i++;
            }
            parameter_count = i;

        }

        public static Regex selectFormatPlaceholders = new Regex(@"\{([\d]*)\}");

        public static Regex selectRegexGroups = new Regex(@"\([\d\{\},\.\w\?\*\\\+[\]\-\$\^]+\)");

        public virtual String Convert(List<String> value)
        {
            String output = format;
            // Math.Min(parameter_count, value.Count)
            for (int i = 0; i < value.Count; i++)
            {
                output = output.Replace("{" + i + "}", value[i]);
            }

            //if (format.isNullOrEmpty()) return value;
            //if (format.Contains("{0}"))
            //{
            //    return String.Format(format, value);
            //}
            return output;
        }

        protected Int32 parameter_count { get; set; } = 0;
        protected String regex { get; set; } = "";
        protected String format { get; set; } = "";
    }



    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class regexMarker<T>:IRegexMarker 
    {

        protected regexMarker() { }
      

        /// <summary>
        /// Initializes a new instance of the <see cref="regexMarker{T}"/> class.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="__m">The m.</param>
        public regexMarker(String regex, T __m)
        {
            marker = __m;
            test = new Regex(regex);
        }

        public List<regexMarkerResult> GetResults(String scrambled)
        {
            List<regexMarkerResult> output = new List<regexMarkerResult>();

            MatchCollection mchs = test.Matches(scrambled);
            foreach (Match m in mchs)
            {
                regexMarkerResult res = new regexMarkerResult(m, marker);
                res.regexMarker = this;
                output.Add(res);    
            }
            return output;
        }
      

        public regexMarkerResult GetResult(String scrambled)
        {
             Match m = test.Match(scrambled);
             regexMarkerResult res = new regexMarkerResult(m, marker);
             res.regexMarker = this;
            return res;
        }
      
        
        /// <summary>
        /// Generates the replacement string for the specified Regex Match
        /// </summary>
        /// <param name="m">The m.</param>
        /// <returns></returns>
        public virtual String replacementGenerator(Match m)
        {
            String output = regexMarkerCollection.REPLACEMENT_PATTERN;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m.Length; i++)
            {
                sb.Append(output);
            }

            return sb.ToString();
        }

        public virtual String ReplaceMatch(Match m, String input, String replacement="")
        {
            String left = input.Substring(0, m.Index);
            if (replacement.isNullOrEmpty()) replacement = replacementGenerator(m);

            String right = input.Substring(m.Index + replacement.Length);
            return left + replacement + right;
        }


        /// <summary>
        /// Gets or sets the test.
        /// </summary>
        /// <value>
        /// The test.
        /// </value>
        public Regex test { get; set; }

        Object IRegexMarker.marker {
            get
            {
                return marker;
            }
        }

        /// <summary>
        /// Gets or sets the marker.
        /// </summary>
        /// <value>
        /// The marker.
        /// </value>
        public T marker { get; set; }
    }
}