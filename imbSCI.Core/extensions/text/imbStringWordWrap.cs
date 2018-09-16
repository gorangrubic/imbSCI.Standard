// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringWordWrap.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Wraping text into HTML
    /// </summary>
    /// \ingroup_disabled ace_ext_string
    public static class imbStringWordWrap
    {
        //protected Regex

        public enum wrapLineSplitModes
        {
        }

        /// <summary>
        /// Daje samo reci - a ignorise razmake i punktaciju
        /// </summary>
        public static string splitPattern_word = "\\W+";

        public static string splitPattern_wordAndPunctation = "([\\s])\\b|\\Z";

        public static Regex wordMatch = new Regex("([\\w\\p{IsCyrillic}]+)");

        /// <summary>
        /// Returns words having 4 characters or more at least
        /// </summary>
        public static Regex wordMatchMinLength = new Regex("([\\w]{4,})");

        /// <summary>
        /// Splits string into token list, excluding punctation and other extra contents
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static List<string> getStringTokens(this String content)
        {
            List<string> output = new List<string>();

            if (content.Contains("ж"))
            {
            }

            content = System.Net.WebUtility.HtmlDecode(content);
            //HttpUtility

            if (content.Contains("с"))
            {
            }

            foreach (Match mc in wordMatch.Matches(content))
            {
                if (mc.Value.Contains("protivpo"))
                {
                }
                output.Add(mc.Value);
            }
            return output;
        }

        /// <summary>
        /// Gets clean words - at least 4 characters long
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static List<string> getStringTokensMinLength(this String content)
        {
            List<string> output = new List<string>();
            if (!content.isNullOrEmpty())
            {
                foreach (Match mc in wordMatchMinLength.Matches(content))
                {
                    output.Add(mc.Value);
                }
            }
            return output;
        }

        /// <summary>
        /// Simple String tokenization - extracts words without spaces and punctation
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static List<string> getTokens(this IEnumerable<string> contentSets, Boolean filterout_numbers = false, Boolean filterout_shortwords = false, Boolean filterin_cleanWords = false, Boolean filter_filewords = false, Int32 shortWords = 4)
        {
            List<string> alt = new List<string>();
            foreach (String tkn in contentSets)
            {
                List<string> tkns = tkn.getTokens(filterout_numbers, filterout_shortwords, filterin_cleanWords, filter_filewords, shortWords);

                //if (tk.Length > 1)
                //{
                alt.AddRange(tkns);
                //}
            }

            return alt;
        }

        internal static Regex REGEX_CLEANWORD = new Regex("[A-Za-zŠŽĐĆČšžđćč\\p{IsCyrillic}]+", RegexOptions.Compiled);
        internal static Char[] wordTrim = new char[] { ',', '.', ' ', '-' };

        /// <summary>
        /// Simple String tokenization - extracts words without spaces and punctation
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static List<string> getTokens(this String content, Boolean filterout_numbers = false, Boolean filterout_shortwords = false, Boolean filterin_cleanWords = false, Boolean filter_filewords = false, Int32 shortWords = 4)
        {
            List<string> output = content.getStringTokens();
            List<string> alt = new List<string>();
            foreach (String tkn in output)
            {
                String tk = tkn.Trim(wordTrim);
                tk = tk.ToLower();
                Boolean ok = true;

                if (ok && filterout_numbers)
                {
                    if (tk.isNumber()) ok = false;
                }

                if (ok && filterin_cleanWords)
                {
                    Match tk2 = REGEX_CLEANWORD.Match(tk);
                    if (tk2.Success)
                    {
                        tk = tk2.Value.ToString();
                    }
                    else
                    {
                        ok = false;
                    }

                    //if (!tk.isCleanWord())
                    //{
                    //    ok = false;
                    //}
                }

                if (ok && filterout_shortwords)
                {
                    if (tk.Length < shortWords) ok = false;
                }

                if (ok && filter_filewords)
                {
                    switch (tk)
                    {
                        case "pdf":
                        case "htm":
                        case "html":
                        case "php":
                        case "http":
                        case "https":
                        case "www":
                        case "aspx":
                        case "asp":
                            ok = false;
                            break;
                    }
                }

                if (ok) alt.Add(tk);
            }

            return alt;
        }

        /// <summary>
        /// Breaks specified text into lines of maximum width as specified by <c>innerWidth</c>. It is aware of words, so the text is splitted only at spaces - all words stay complete.
        /// </summary>
        /// <remarks>It is ok if text is less then full line</remarks>
        /// <param name="input"></param>
        /// <param name="innerWidth"></param>
        /// <returns>List of lines</returns>
        public static List<string> wrapLineBySpace(this String input, Int32 innerWidth)
        {
            List<string> output = new List<string>();
            List<string> tkns = new List<string>();
            tkns.AddRange(Regex.Split(input, splitPattern_wordAndPunctation));
            //List<String> _tkns = new List<string>();
            //foreach (String tk in tkns)
            //{
            //    if (!String.IsNullOrEmpty(tk))
            //    {
            //        _tkns.Add(tk);
            //    }
            //}
            tkns = tkns.removeEmptyOrNull();

            if (!tkns.Any())
            {
                //tkns = new string[] {};

                while (input.Length > innerWidth)
                {
                    String tkn = input.Substring(0, innerWidth);
                    input = input.Substring(innerWidth);
                    output.Add(tkn);
                }
                if (!String.IsNullOrWhiteSpace(input)) output.Add(input);
            }
            else
            {
                String newline = "";
                Int32 newlineLen = 0;
                foreach (String tkn in tkns)
                {
                    newlineLen = newline.Length;
                    if ((newlineLen + tkn.Length + 1) > innerWidth)
                    {
                        output.insertNewLineInOutput(newline);
                        newline = "";
                    }

                    newline = newline.add(tkn);
                }
                //newline = newline;
                output.insertNewLineInOutput(newline);
            }
            output = output.removeEmptyOrNull();
            return output;
        }

        /// <summary>
        /// Breakes input string by NewLine character. Trim space and tabs fir each line
        /// </summary>
        /// <param name="input">If null returns empty list</param>
        /// <returns></returns>
        public static List<string> breakLines(this String input, Boolean doTrim = false)
        {
            List<string> output = new List<string>();
            if (input.isNullOrEmpty()) return output;

            var __lns = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (__lns.Any())
            {
                foreach (var _ln in __lns)
                {
                    String l = _ln;
                    if (doTrim) l = l.Trim().Trim("\t".ToCharArray());
                    output.Add(l);
                }
            }
            else
            {
                output.Add(input);
            }

            return output;
        }

        /// <summary>
        /// Iz jedne linije lomi u vise
        /// </summary>
        /// <param name="input"></param>
        /// <param name="innerWidth"></param>
        /// <param name="lineBreaker"></param>
        /// <returns></returns>
        public static List<string> wrapLine(this String input, Int32 innerWidth, String lineBreaker = "-")
        {
            List<string> output = new List<string>();
            String inl = input;
            while (inl.Length >= innerWidth)
            {
                string[] tkns = wordMatch.Split(inl);  // imbNLPbasic.tokenize(inl);
                String nln = "";
                Boolean doAdd = true;
                Int32 c = 0;
                if (!tkns.Any())
                {
                    nln = inl;
                    doAdd = false;
                }

                while (doAdd)
                {
                    Int32 futureln = (nln.Length + tkns[c].Length);
                    if (futureln >= innerWidth)
                    {
                        doAdd = false;
                    }
                    else
                    {
                        nln += tkns[c] + " ";
                        c++;
                        if (c >= tkns.Length)
                        {
                            c = tkns.Length - 1;
                            doAdd = false;
                        }
                    }
                    nln = nln.Trim();
                }
                if (nln.Length > 0)
                {
                    nln = inl.Substring(0, nln.Length);
                }
                output.Add(nln);
                inl = inl.Substring(nln.Length);
            }
            if (!String.IsNullOrEmpty(inl))
            {
                output.Add(inl);
            }

            return output;
        }
    }
}