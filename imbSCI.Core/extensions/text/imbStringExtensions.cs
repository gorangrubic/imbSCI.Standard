// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringExtensions.cs" company="imbVeles" >
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

    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Data;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    #endregion imbVeles using

    /// <summary>
    /// Proširenja operacija nad stringovima
    /// </summary>
    /// \ingroup_disabled ace_ext_string
    public static class imbStringExtensions
    {
        /// <summary>
        /// Splits the String by specified needle just ONCE. Needle is not included
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="needle">The needle.</param>
        /// <returns>List with one (if no <c>needle</c> found, original string) or two strings (if <c>needle</c> found, left and right substrings</returns>
        public static List<string> SplitOnce(this String target, String needle)
        {
            List<String> output = new List<string>();

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
        /// AEIOU both cases - regex to capture these letters
        /// </summary>
        public static Regex _samoglasnici = new Regex(@"[aeiouAEIOU]", RegexOptions.Compiled);

        /// <summary>
        /// Regex to capture content inside tag square brackets
        /// </summary>
        public static Regex _htmlTagContent = new Regex(@"<[^>]*>");

        public static Regex _camelOps = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])",
            RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Uses square bracket regex to extract all content outside HTML tags
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String StripHTML(this String input)
        {
            if (input.isNullOrEmpty()) return "";
            return _htmlTagContent.Replace(input, "");
        }

        /// <summary>
        /// Choose first non null and not empty String from <c>primaryCandidate</c> and other <c>candidates</c>
        /// </summary>
        /// <param name="primaryCandiate">The primary candiate.</param>
        /// <param name="candidates">The candidates.</param>
        /// <returns>The first string found to be not null and not empty</returns>
        public static String getProperString(this String primaryCandiate, params String[] candidates)
        {
            if (!primaryCandiate.isNullOrEmptyString()) return primaryCandiate;

            List<String> canList = candidates.ToList(); //.getFlatList<String>();

            foreach (String str in canList)
            {
                if (!str.isNullOrEmptyString())
                {
                    return str;
                }
            }
            return "";
        }

        /// <summary>
        /// Returns the <c>first</c> value if its not null or empty, otherwise iterates other parameters and pick the first valid
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="orThese">The or these.</param>
        /// <returns></returns>
        /// <exception cref="aceGeneralException">None of these argument is value - null - or(first, ... orThese others)</exception>
        public static String or(this String first, params String[] orThese)
        {
            if (!first.isNullOrEmpty()) return first;
            foreach (String these in orThese)
            {
                if (!these.isNullOrEmpty()) return these;
            }
            //var axe = new aceGeneralException("Silent exception at> imbStringCollectionExtensions.or(): None of these argument is value", null, orThese, "or(first, ... orThese others)");
            //aceLog.log(axe.title);
            //aceLog.log(axe.Message);
            return "";
        }

        #region SHORT STRING EXTNSION OPERATORS -----------------------------------------------------------------------------------------|

        ///// <summary>
        ///// Formats <c>fin</c> parameters into <c>format</c> string
        ///// </summary>
        ///// <param name="format">The format.</param>
        ///// <param name="fin">The fin.</param>
        ///// <returns></returns>
        //public static String f(this String format, params Object[] fin)
        //{
        //    String output = String.Format(format, fin.getFlatArray<Object>());
        //    return output;
        //}

        ///// <summary>
        ///// Adds template parameter placeholder
        ///// </summary>
        ///// <param name="output"></param>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public static string t(this String output, Enum input)
        //{
        //    return output + (stringTemplateTools.PLACEHOLDER_Start + input.ToString() + stringTemplateTools.PLACEHOLDER_End);
        //}

        ///// <summary>
        ///// Adds template parameter placeholder
        ///// </summary>
        ///// <param name="output"></param>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public static string t(this String output, templateFieldBasic input)
        //{
        //    return output + (stringTemplateTools.PLACEHOLDER_Start+input.ToString()+stringTemplateTools.PLACEHOLDER_End);
        //}

        ///// <summary>
        ///// Places in quote \"
        ///// </summary>
        ///// <param name="output"></param>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public static string q(this String output, String input)
        //{
        //    return output.a("\""+input.ToString() + "\"");
        //}

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

        public static object removeStartsWith(string path, string name)
        {
            throw new NotImplementedException();
        }

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

        /// <summary>
        /// 2014 Maj: uklanja duple substringove - dupli razmak, newline, ..
        /// </summary>
        /// <param name="source">Ulazni string</param>
        /// <param name="toRemove">substringovi u jednostrukoj formi - ako treba da se uklone svi .. onda navesti . - ako je null onda primenjuje default skup: space, newline, .</param>
        /// <returns>Ociscen string</returns>
        public static String imbRemoveDouble(this String source, params String[] toRemove)
        {
            if (toRemove == null)
            {
                toRemove = new[] { " ", Environment.NewLine, "." };
            }
            String out2 = source;
            foreach (String tr in toRemove)
            {
                String dd = tr + tr;
                while (out2.Contains(dd))
                {
                    out2 = out2.Replace(dd, tr);
                }
            }
            return out2;
        }

        /// <summary>
        /// 2017: smarter substring. Negative length will go substring from right end
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns>Substring</returns>
        public static String substring(this String input, Int32 length)
        {
            if (length == input.Length) return input;
            if (length == -input.Length) return input;
            if (length > input.Length) return input;
            if (length < -input.Length) return input;

            if (length > 0) return input.Substring(0, length);

            if (length < 0) return input.Substring(input.Length + length);

            return "";
        }

        /// <summary>
        /// Removes all strings from input, using dict
        /// </summary>
        /// <param name="input"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static String removeFromString(this String input, IEnumerable<String> dict)
        {
            foreach (var dc in dict)
            {
                input = input.Replace(dc, "");
            }
            return input;
        }

        public static String removeFromString(this String input, IEnumerable<Char> dict)
        {
            foreach (var dc in dict)
            {
                input = input.Replace(dc.ToString(), "");
            }
            return input;
        }

        public static String escapeString(this Dictionary<String, String> dict, String input)
        {
            foreach (var dc in dict)
            {
                input = input.Replace(dc.Key, dc.Value);
            }
            return input;
        }

        public static String unescapeString(this Dictionary<String, String> dict, String input)
        {
            foreach (var dc in dict)
            {
                input = input.Replace(dc.Value, dc.Key);
            }
            return input;
        }

        /// <summary>
        /// Konvertovanje Stringa u Stream
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static MemoryStream toMemoryStream(this String input)
        {
            // convert string to stream
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;
        }

        public static String toStringFromStream(this MemoryStream stream)
        {
            // convert stream to string
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();
            return text;
        }

        public static Boolean imbStartWidth(this String source, String[] startArray, Boolean ignoreCase = true,
                                            Boolean inverse = false)
        {
            Boolean output = false;

            foreach (String st in startArray)
            {
                if (source.StartsWith(st))
                {
                    output = true;
                }
            }

            if (inverse) output = !output;

            return output;
        }

        public static String ToStringEnumSmart(this IEnumerable<Enum> en, String separator = ", ")
        {
            String output = "";
            foreach (Enum e in en)
            {
                String item = e.ToString();

                if (item == "none")
                {
                }
                else
                {
                    output = output.add(item, separator);
                }
            }

            return output;
        }

        /// <summary>
        /// String, comma separated, representation of flags in the <paramref name="en"/> Enum (<see cref="FlagsAttribute"/>).
        /// </summary>
        /// <param name="en">Flags to render</param>
        /// <returns>Comma separater representation of all flags in the <paramref name="en"/></returns>
        public static String ToStringEnumSmart(this Enum en)
        {
            String output = "";

            if (en.ToInt32() == 0)
            {
                return "";
            }

            if (en.HasFlags(false))
            {
                //
                foreach (Enum eni in en.getEnumListFromFlags())
                {
                    output = output.add(eni.toStringSafe().imbTitleCamelOperation(), ", ");
                }
                return output;
            }
            else
            {
                output = output.toStringSafe();
                //
            }
            return output;
        }

        public static String Join(IEnumerable<String> input, String joint = "")
        {
            String output = "";
            foreach (String x in input)
            {
                output = output.add(x, joint);
            }
            // input.ForEach(x => );
            if (joint != "") output.TrimEnd(joint.ToArray());
            return output;
        }

        /// <summary>
        /// Joins the in format: applies the specified format on each instance, separator is added between
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="format">The format.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static String JoinInFormat(this IEnumerable<String> input, String format = "[{0}]", String separator = " ")
        {
            String output = "";
            foreach (String x in input)
            {
                output = output.add(String.Format(format, x), separator);
            }
            return output;
        }

        public static String overwrite(this String source, Match m, String overwriteChar = "#")
        {
            String scrambled = source;

            return scrambled.overwrite(m.Index, m.Length, overwriteChar);
        }

        /// <summary>
        /// Od definisanog startIndex-a pa narednih length karaktera upisuje overwriteChar - koji ponavlja
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="overwriteChar"></param>
        /// <returns></returns>
        public static String overwrite(this String source, Int32 startIndex, Int32 length, String overwriteChar = "#")
        {
            String scrambled = source;
            if (startIndex >= source.Length)
            {
                return source;
            }
            if (startIndex + length > source.Length)
            {
                length = source.Length - startIndex;
            }

            String output = scrambled.Substring(0, startIndex);

            String insert = "";
            while (insert.Length < length)
            {
                insert = insert + overwriteChar;
            }

            if (insert.Length > length)
            {
                insert = insert.Substring(0, length);
            }

            output += insert;

            output += scrambled.Substring(startIndex + length);

            return output;
        }

        /// <summary>
        /// Transforms Display Title into small_letter_codename
        /// </summary>
        /// <param name="source">Display Title</param>
        /// <returns>display_title</returns>
        public static String imbCodeNameOperation(this String source)
        {
            String output = "";

            output = source.ToLower().Trim();
            output = output.Replace("http://", "");
            output = output.Replace("/", "");

            output = output.Replace(" ", "_");
            output = output.Replace(":", "");
            output = output.Replace(".", "-");

            output = output.Replace("//", "_");
            output = output.Replace("\\", "_");

            output = output.Replace("__", "_");
            return output;
        }

        /// <summary>
        /// Converts camel name (e.g. "imbTitleCamelOperation") into proper, space separated, caption (Imb Title Camel Operation)
        /// u normalan title. Ubacuje razmake i sredjuje prva slova.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="inverse">Ako je TRUE onda ima kontra efekat> <b>Display Title</b> pretvara u <b>DisplayTitle</b> ili <b>displayTitle</b> u zavisnosti od <b>setFirstCapital</b></param>
        /// <param name="setFirstCapital">Da li prvo slovo treba da bude veliko</param>
        /// <returns>Preformatiran string</returns>
        public static String imbTitleCamelOperation(this String source, Boolean setFirstCapital = true,
                                                    Boolean inverse = false)
        {
            String output = "";
            if (String.IsNullOrEmpty(source)) return "";

            if (inverse)
            {
                output = source.Replace(" ", "");
                if (setFirstCapital)
                {
                    output = output.imgFirstLetter(false);
                }
            }
            else
            {
                output = _camelOps.Replace(source, " ");
                output = output.Replace("_", " ");
                output = output.Replace("  ", " ");
                if (setFirstCapital)
                {
                    output = output.imgFirstLetter(true);
                }
            }
            return output;
        }

        /// <summary>
        /// 2017: Pravi varijacije od skracenice
        /// </summary>
        /// <remarks>
        /// <para>Predstavlja primenu <b>enum wordVariationsMethodType.Abrevations</b> </para>
        /// <para>Primer: ako je uneto DOO pravi varijacije:</para>
        /// <list type="bullet" >
        /// <item>doo</item>
        /// <item>d.o.o.</item>
        /// <item>d. o. o.</item>
        /// <item>Doo</item>
        /// <item>DOO</item>
        /// </list>
        /// </remarks>
        /// <param name="source">Skracenicu koju treba da varira - npr> doo</param>
        /// <returns>Lista varijacija</returns>
        public static List<String> imbGetAbbrevationVariants(this String source)
        {
            List<String> output = new List<string>();

            source = source.ToLower().Replace(".", "").Trim();
            source = source.Replace(" ", "");

            output.Add(source);

            String v1 = "";
            String v2 = "";
            String v3 = "";
            foreach (Char ch in source.ToList())
            {
                v1 = v1 + ch + ".";
                v2 = v2 + ch + " ";
                v3 = v3 + ch + ". ";
            }
            output.Add(v1);
            output.Add(v2);
            output.Add(v3);
            output.Add(source.ToUpper());
            output.Add(source.ToLower());
            return output;
        }

        /// <summary>
        /// Vraca skracenicu reci
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetLength"></param>
        /// <param name="toCapital"></param>
        /// <returns></returns>
        public static String imbGetWordAbbrevation(this String source, Int32 targetLength = 3, Boolean toCapital = true)
        {
            if (String.IsNullOrEmpty(source)) return "";
            String output = "";

            output = _samoglasnici.Replace(source, "");

            if (String.IsNullOrEmpty(source)) return output = source;
            if (output.Length < 1) return output;
            if (source[0] != output[0]) output = source[0] + output;
            targetLength = Math.Min(output.Length, targetLength);
            output = output.Substring(0, targetLength);
            if (toCapital) output = output.ToUpper();
            return output;
        }

        /// <summary>
        /// Creates abbreviation from multi word phrase - it also supports single word source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetLength"></param>
        /// <param name="toCapital"></param>
        /// <returns></returns>
        public static String imbGetAbbrevation(this String source, Int32 targetLength = 3, Boolean toCapital = true)
        {
            if (String.IsNullOrEmpty(source)) return "";
            List<String> words = new List<string>();
            words.AddRange(source.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            String output = "";
            if (words.Count > 1)
            {
                // ima vise reci
                Int32 rest = words.Count % targetLength;
                Int32 forEachWord = (words.Count - rest) / targetLength;
                Int32 i = 0;

                foreach (String w in words)
                {
                    Int32 len = forEachWord;
                    if (i < rest) len++;
                    output += w.imbGetWordAbbrevation(len, false).ToUpperInvariant();
                    i++;
                }
                if (toCapital) output = output.ToUpper();
            }
            else
            {
                output = source.imbGetWordAbbrevation(targetLength, toCapital);
                // samo jedna rec
            }
            return output;
        }

        /// <summary>
        /// V3.1: Prvo slovo pretvara u malo
        /// </summary>
        /// <param name="source"></param>
        /// <param name="isUpper"></param>
        /// <returns></returns>
        public static String imgFirstLetter(this String source, Boolean isUpper = true)
        {
            if (String.IsNullOrEmpty(source))
            {
                return "";
            }

            if (isUpper)
            {
                return Char.ToUpperInvariant(source[0]) + source.Substring(1);
            }
            else
            {
                return Char.ToLowerInvariant(source[0]) + source.Substring(1);
            }
        }

        /// <summary>
        /// V3.2: Vraca formatiran string sa vremenom u sekundama
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sufix"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static String imbGetTimeString(this long source, String sufix = "s ", Int32 decimals = 2)
        {
            return source.getTimeSecString(sufix, decimals);
        }

        /// <summary>
        /// Vraća formatiran string sa vremenom u sekundama. bez sufiksa
        /// </summary>
        /// <param name="source"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static Double imbGetTimeSecond(this long source, Int32 decimals = 2)
        {
            return (double)imbStringFormats.getSeconds(source, decimals);
        }

        public static String imbGetCodeMark(this Object input, Int32 characters = 2, String prefix = "[",
                                            String sufix = "]", Boolean samoSuglasnici = false)
        {
            String output = input.ToString().ToUpper();

            if (samoSuglasnici)
            {
                output = _samoglasnici.Replace(output, "");
            }
            String format = "{0," + characters.ToString() + "}";
            output = String.Format(format, output);   //imbStringOperations.trimToLimit(output, characters, true, "", false);/

            return prefix + output + sufix;
        }

        /// <summary>
        /// Vraca string predstavu imbCollectiona
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static String imbGetVariableList(this IDictionary source)
        {
            String output = "";
            String line = "";
            foreach (String key in source.Keys)
            {
                Object valObj = source[key];
                String valStr = "";
                if (valObj == null)
                {
                    valStr = "";
                }
                else
                {
                    valStr = valObj.ToString();
                }

                line = "[" + key + "] = " + valStr + Environment.NewLine;
                output += line;
            }

            return output;
        }
    }
}