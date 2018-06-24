// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbSciStringExtensions.cs" company="imbVeles" >
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

namespace imbSCI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Proširenja operacija nad stringovima
    /// </summary>
    /// \ingroup_disabled ace_ext_string
    public static class imbSciStringExtensions
    {
        ///// <summary>
        ///// Proverava da li je input null, ako je string onda ga proverava kao string ako je neki drugi objekat onda ga predvara u string pa proverava
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public static Boolean isNullOrEmptyString(this Object input)
        //{
        //    if (input == null)
        //    {
        //        return true;
        //    }
        //    if (input is DBNull)
        //    {
        //        return true;
        //    }
        //    if (input is String)
        //    {
        //        return String.IsNullOrEmpty(input as String);
        //    }
        //    return false;
        //}
        public static String LogException(this Exception ex, String context, String prefix = "", Boolean skipSave = false)
        {
            var sb = new StringBuilder();

            if (!skipSave) sb.AppendLine("--------------------------------");
            sb.AppendLine(prefix + context);
            sb.AppendLine(prefix + " > " + ex.Message + "");
            sb.AppendLine(prefix + " > " + ex.StackTrace + "");

            if (ex.InnerException != null)
            {
                sb.AppendLine(LogException(ex.InnerException, "Inner exception", prefix + " > ", true));
            }
            if (ex is AggregateException exa)
            {
                if (exa.InnerExceptions != null)
                {
                    foreach (Exception exsub in exa.InnerExceptions)
                    {
                        if (exsub != null)
                        {
                            sb.AppendLine(prefix + " > InnerException [" + exsub.Message + "]");
                            sb.AppendLine(prefix + " > > " + exsub.StackTrace);
                        }
                    }
                }
            }

            if (!skipSave)
            {
                sb.AppendLine("--------------------------------");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Splits the String by specified needle just ONCE. Needle is not included
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="needle">The needle.</param>
        /// <returns>List with one (if no <c>needle</c> found, original string) or two strings (if <c>needle</c> found, left and right substrings</returns>
        public static List<string> SplitOnce(this String target, String needle)
        {
            List<string> output = new List<string>();

            Int32 pos = target.IndexOf(needle);

            if (pos < 0)
            {
                output.Add(target);
            }
            else
            {
                output.Add(target.Substring(0, pos));
                output.Add(target.Substring(pos + needle.Length));
            }

            return output;
        }

        /// <summary>
        /// Splits the string or if no needle found in the string it returs <c>target</c> string
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="needle">The needle.</param>
        /// <param name="targetDefault">If the target is null or empty it will use this value</param>
        /// <returns>
        /// List with one (if no <c>needle</c> found, original string) or two strings (if <c>needle</c> found, left and right substrings
        /// </returns>
        public static List<string> SplitSmart(this String target, String needle, String targetDefault = "", Boolean trimResults = false, Boolean removeEmptyResults = true)
        {
            List<string> output = new List<string>();

            if (target.isNullOrEmpty()) target = targetDefault;

            Int32 pos = target.IndexOf(needle);

            if (pos < 0)
            {
                output.Add(target);
            }
            else
            {
                output.AddRange(target.Split(needle.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            List<string> realOutput = new List<string>();
            foreach (string st in output)
            {
                String stt = st;
                if (trimResults) stt = stt.Trim();
                if (!removeEmptyResults || !stt.isNullOrEmpty())
                {
                    realOutput.Add(stt);
                }
            }
            return realOutput;

            //return output;
        }

        /// <summary>
        /// Proverava da li je input null, ako je string onda ga proverava kao string ako je neki drugi objekat onda ga predvara u string pa proverava
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Boolean isNullOrEmptyString(this Object input)
        {
            if (input == null)
            {
                return true;
            }
            if (input is DBNull)
            {
                return true;
            }
            if (input is String)
            {
                return String.IsNullOrEmpty(input as String);
            }
            return false;
        }

        public static Boolean isNullOrEmpty(this Object input)
        {
            if (input == null) return true;
            if (input == "") return true;
            return false;
        }

        /*
        /// <summary>
        /// Removes prefix if found at beginning of input
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static String removeStartsWith(this String input, String prefix)
        {
            if (String.IsNullOrEmpty(input)) return input;
            if (String.IsNullOrEmpty(prefix)) return input;
            if (input.StartsWith(prefix)) input = input.Substring(prefix.Length);
            return input;
        }

        /// <summary>
        /// Osigurava da ce string poceti sa input stringom
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ensureStartsWith(this String output, String input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return output;
            }
            if (output.Length == 0)
            {
                output = input;
                return output;
            }
            if (output.Length > 0)
            {
                if (!output.StartsWith(input))
                {
                    String separator = "";
                    // separator = input.Last().ToString();

                    separator = output.First().ToString();

                    if (!separator.isSymbolicContentOnly())
                    {
                        separator = "";
                    }

                    output = input.add(output, separator);
                }
            }
            return output;
        }

        /// <summary>
        /// Ako se zavrsava sa sufixom onda uklanja sufix
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sufix"></param>
        /// <returns></returns>
        public static String removeEndsWith(this String input, String sufix)
        {
            if (String.IsNullOrEmpty(input)) return input;
            if (String.IsNullOrEmpty(sufix)) return input;
            if (input.EndsWith(sufix)) input = input.Substring(0, input.Length - sufix.Length);
            return input;
        }

        public static string ensureEndsWith(this String output, String input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return output;
            }
            if (output.Length == 0)
            {
                output = input;
                return output;
            }
            if (output.Length > 0)
            {
                if (!output.EndsWith(input))
                {
                    output += input;
                }
            }
            return output;
        }
        */

        /// <summary>
        /// Removes prefix if found at beginning of input
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static String removeStartsWith(this String input, String prefix)
        {
            if (String.IsNullOrEmpty(input)) return input;
            if (String.IsNullOrEmpty(prefix)) return input;
            if (input.StartsWith(prefix)) input = input.Substring(prefix.Length);
            return input;
        }

        /// <summary>
        /// Osigurava da ce string poceti sa input stringom
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ensureStartsWith(this String output, String input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return output;
            }
            if (output.Length == 0)
            {
                output = input;
                return output;
            }
            if (output.Length > 0)
            {
                if (!output.StartsWith(input))
                {
                    String separator = "";
                    // separator = input.Last().ToString();

                    separator = output.First().ToString();

                    //if (!separator.isSymbolicContentOnly())
                    //{
                    //    separator = "";
                    //}

                    output = input.add(output, separator);
                }
            }
            return output;
        }

        /// <summary>
        /// Ako se zavrsava sa sufixom onda uklanja sufix
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sufix"></param>
        /// <returns></returns>
        public static String removeEndsWith(this String input, String sufix)
        {
            if (String.IsNullOrEmpty(input)) return input;
            if (String.IsNullOrEmpty(sufix)) return input;
            if (input.EndsWith(sufix)) input = input.Substring(0, input.Length - sufix.Length);
            return input;
        }

        /// <summary>
        /// Ensures the ends with.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="sufix">The sufix.</param>
        /// <returns></returns>
        public static String ensureEndsWith(this String input, String sufix)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }
            if (input.Length == 0)
            {
                input = sufix;
                return input;
            }
            if (input.Length > 0)
            {
                if (!input.EndsWith(sufix))
                {
                    input += sufix;
                }
            }
            return input;
        }

        public static string add(this String output, List<string> input, String separator = " ", Boolean multiLine = true)
        {
            if (input == null)
            {
                return output;
            }

            if (!input.Any()) return output;

            foreach (String inl in input)
            {
                output = output.add(inl, separator);
                if (multiLine) output = output + Environment.NewLine;
            }

            return output;
        }

        /// <summary>
        /// Adds the specified input at end of <c>output</c> and puts <c>separator</c> between if <c>input</c> is not null or empty. Null safe operation.
        /// </summary>
        /// <remarks>
        /// Both <c>input</c> and <c>separator</c> may be null or empty.
        /// If <c>input</c> is null or empty it will not add separator to the resulting string
        /// </remarks>
        /// <param name="output">The output.</param>
        /// <param name="input">The input.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>Appended version of the string</returns>
        public static string add(this String output, String input, String separator = " ")
        {
            if (String.IsNullOrEmpty(input))
            {
                return output;
            }
            if (output.isNullOrEmpty()) output = "";

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

        public static string add(this String output, String input, Char ch)
        {
            return output.add(input, ch.ToString());
        }

        ///// <summary>
        ///// Adds the specified input at end of <c>output</c> and puts <c>separator</c> between if <c>input</c> is not null or empty. Null safe operation.
        ///// </summary>
        ///// <remarks>
        ///// Both <c>input</c> and <c>separator</c> may be null or empty.
        ///// If <c>input</c> is null or empty it will not add separator to the resulting string
        ///// </remarks>
        ///// <param name="output">The output.</param>
        ///// <param name="input">The input.</param>
        ///// <param name="separator">The separator.</param>
        ///// <returns>Appended version of the string</returns>
        //public static string add(this String output, String input, String separator = " ")
        //{
        //    if (String.IsNullOrEmpty(input))
        //    {
        //        return output;
        //    }
        //    if (output.isNullOrEmpty()) output = "";

        //    if (output.Length > 0)
        //    {
        //        if (!output.EndsWith(separator) && !input.StartsWith(separator))
        //        {
        //            output += separator;
        //        }
        //    }

        //    output += input;

        //    return output;
        //}
    }
}