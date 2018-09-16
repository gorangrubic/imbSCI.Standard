// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringCommonTools.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.process;

    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    #endregion imbVeles using

    /// <summary>
    /// Zajednički alati za obradu strigova na različite načine
    /// </summary>
    /// <remarks>
    /// Napredniji set alata i funkcija od imbStringOperations klase
    /// </remarks>
    public static class imbStringCommonTools
    {
        /// <summary>
        /// V3.5&gt; Experimentalna funkcija - na osnovu tipa vrsi konverziju
        /// </summary>
        /// <param name="input">Ulazni string</param>
        /// <param name="targetType">Tip</param>
        /// <param name="logic">The logic.</param>
        /// <returns></returns>
        /// <remarks>
        /// Koristi se i 2013a
        /// </remarks>
        public static Object getDataTypeSafe(this Object input, Type targetType, dataLogic logic = dataLogic.skip)
        {
            if (targetType == typeof(Type))
            {
                return input;
            }

            targetType = imbDataExecutor.getRecommandedType(logic, targetType);

            if (input == null) input = "";

            if (targetType == typeof(String))
            {
                return input.ToString();
            }

            if (targetType == typeof(Int32)) return input.ToString().getInt32Safe(0); // getInt32Safe(input.ToString(), 0);

            if (targetType == typeof(Double))
            {
                return input.ToString().getDoubleSafe(0); //  getDoubleSafe(input.ToString(), 0);
            }

            if (targetType == typeof(Boolean))
            {
                Boolean boolOut = false;
                Boolean.TryParse(input.ToString(), out boolOut);

                return boolOut;
            }

            if (targetType == typeof(List<Int32>))
                return rangeStringToIndexList(input as String, 1000);

            if (targetType.IsEnum)
            {
                return targetType.getEnumByName(input as String);
            }

            return input;
        }

        /// <summary>
        /// Compiles range expression into list of integers. Range expression format e.g.: 3-5,8 , 2-8, 9, *
        /// </summary>
        /// <param name="rangeLine">Range expression like: &gt; 1=5, 8, 12-20, 3</param>
        /// <param name="indMax">The end of range</param>
        /// <returns>
        /// List with index numbers  / positional integers
        /// </returns>
        /// <exception cref="System.ArgumentException">rangeLine - Expression range processing error</exception>
        /// <remarks>
        /// For unlimited range on one side higher/lower symbols, e.g.: 3 &gt;  means: from 3 to the end
        /// </remarks>
        public static List<int> rangeStringToIndexList(this String rangeLine, Int32 indMax = 100)
        {
            List<int> output = new List<int>();

            List<string> slogovi = imbStringCommonTools.multiOpsInputProcessing(rangeLine);

            //string[] slogovi = rangeLine.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            Int32 v;
            foreach (String item in slogovi)
            {
                if (item.Contains("*"))
                {
                    for (v = 0; v < indMax; v++)
                    {
                        output.Add(v);
                    }
                    break;
                }

                if (item.Contains("-"))
                {
                    try
                    {
                        string[] limits = item.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        if (limits.Length > 1)
                        {
                            Int32 start = imbStringFormats.getInt32Safe(limits[0]);
                            //Int32.Parse(_numOnly.Replace(limits[0], "", 100));
                            Int32 end = imbStringFormats.getInt32Safe(limits[1]);
                            //Int32.Parse(_numOnly.Replace(limits[1], "", 100));
                            if (end > indMax)
                            {
                                end = indMax;
                            }
                            for (v = start; v < end + 1; v++)
                            {
                                output.Add(v);
                            }
                        }
                    }
                    catch
                    {
                        throw new ArgumentException("Expression range processing error", nameof(rangeLine));
                    }
                }
                else if (item.Contains(">"))
                {
                    Int32 start = Int32.Parse(imbStringCommonTools._numOnly.Replace(item, "", 100));
                    Int32 end = indMax;
                    for (v = start; v < end; v++)
                    {
                        output.Add(v);
                    }
                }
                else if (item.Contains("<"))
                {
                    Int32 end = Int32.Parse(imbStringCommonTools._numOnly.Replace(item, "", 100));
                    if (end > indMax)
                    {
                        end = indMax;
                    }
                    Int32 start = 0;
                    for (v = start; v < end; v++)
                    {
                        output.Add(v);
                    }
                }
                else if (item.Contains("nth"))
                {
                    Int32 step = imbStringFormats.getInt32Safe(item, 2);
                    Int32 a;
                    for (a = 0; a < indMax; a = a + step)
                    {
                        output.Add(a);
                    }
                }
                else
                {
                    output.Add(imbStringFormats.getInt32Safe(item));
                }
            }

            List<int> uniqueOutput = new List<int>();
            foreach (Int32 index in output)
            {
                if (!uniqueOutput.Contains(index))
                {
                    uniqueOutput.Add(index);
                }
            }

            return uniqueOutput;
        }

        /// <summary>
        /// Primenjuje registar specijalnih znakova na zadati unos
        /// </summary>
        /// <param name="input">Parametar na koji se primenjuje</param>
        /// <returns></returns>
        public static String deploySpecialMarks(this String input)
        {
            String output = input;
            if (!String.IsNullOrEmpty(input))
            {
                foreach (KeyValuePair<String, String> mark in specialMarks)
                {
                    output = output.Replace(mark.Key, mark.Value);
                }
            }
            return output;
        }

        /// <summary>
        /// Sklanja sve specijalne editor tagove: ali i sve druge specijalne oznake&gt; {}, $$$
        /// </summary>
        /// <param name="list">Listu&gt; Key:cleanList, Value:editType, nulti clan je cist string a ostali su editor tagovi</param>
        /// <returns></returns>
        public static List<KeyValuePair<String, String>> trimEditorTags(this String list)
        {
            List<KeyValuePair<String, String>> output = new List<KeyValuePair<string, string>>();
            try
            {
                String editWillcard = "";
                String editType = "";

                String cleanList = list.Replace("{}", "");
                cleanList = cleanList.Replace("$$$", "");
                cleanList = _editorTag.Replace(cleanList, "");
                output.Add(new KeyValuePair<string, string>(cleanList, editType));

                foreach (Match it in _editorTag.Matches(list))
                {
                    editType = it.Value.Substring(it.Value.IndexOf(":") + 1).Replace("}", "");
                    editWillcard = it.Value.Substring(0, it.Value.IndexOf(":")).Replace("{", "");
                    output.Add(new KeyValuePair<string, string>(editWillcard, editType));
                }

                _multuTag.Matches(list);

                return output;
            }
            catch
            {
                return output;
            }
        }

        /// <summary>
        /// Regex za multi ops - selektuje vrednosti između tagova
        /// </summary>
        public static Regex _multiOpRe = new Regex("\\{\"(.*?)\"\\}");

        /// <summary>
        /// Regex za selektovanje brojeva
        /// </summary>
        public static Regex _numOnly = new Regex(@"\D+");

        /// <summary>
        /// Selektuje xPath index blok - npr> [1]
        /// </summary>
        public static Regex _indexBlok = new Regex("\\[(.*?)\\]");

        /// <summary>
        /// Regex selektuje karaktere koji se koriste za MultiOps tagove
        /// </summary>
        public static Regex _multuTag = new Regex("[\\{\"\\}]");

        /// <summary>
        /// Regez selektuje karaktere koji se koriste za označavanje editora
        /// </summary>
        public static Regex _editorTag = new Regex("\\{(.*?)\\}");

        private static Dictionary<string, string> _specialMarks;

        /// <summary>
        /// Registar specijalnih znakova - koji se koriste u imbXmlFilter inputu
        /// </summary>
        public static Dictionary<string, string> specialMarks
        {
            get
            {
                if (_specialMarks == null)
                {
                    _specialMarks = new Dictionary<string, string>();
                    _specialMarks.Add("{nl}", Environment.NewLine);
                    _specialMarks.Add("{dc}", ";");
                    _specialMarks.Add("{dd}", ":");
                    _specialMarks.Add("{dcnl}", ";" + Environment.NewLine);
                    _specialMarks.Add("{tb}", "\t");
                }
                return _specialMarks;
            }
        }

        /// <summary>
        /// 2013a> sklanja sve {} [] i vrednosti u njima, sklanja i namespace znak ako postoji
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String cleanSpecialCharForXPathTitle(String input)
        {
            String output = input;
            output = _indexBlok.Replace(output, "", 100);

            //.Replace(output, "", 100);
            //output = _editorTag.Replace(output, "", 100);
            //output = Regex.Replace(output, _multuTag, "", RegexOptions.IgnorePatternWhitespace);

            if (output.Contains(":"))
            {
                output = output.Substring(output.IndexOf(":") + 1);
            }
            return output;
        }

        /// <summary>
        /// Briše sve MultiOps oznake
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string cleanMultiOpMarks(String input)
        {
            return input.Replace("{\"", "").Replace("\"}", "");
        }

        /// <summary>
        /// V3: Dodaje nov parametar u multi input parametar.
        /// </summary>
        /// <param name="newValue">nova vrednost koja se dodaje na spisak</param>
        /// <param name="oldValue">vrednost parametra koja je možda multi ops a možda i nije</param>
        /// <returns></returns>
        public static String multiOpsInputAdd(String newValue, String oldValue)
        {
            String final = "";
            if (String.IsNullOrEmpty(oldValue.Replace(" ", ""))) oldValue = "";

            List<string> output = multiOpsInputProcessing(oldValue);

            String rest = _multiOpRe.Replace(oldValue, "", output.Count());

            rest = _multuTag.Replace(rest, "", 100);

            newValue = _multuTag.Replace(newValue, "", 100);

            if (!String.IsNullOrEmpty(rest)) output.Add(rest);
            if (!String.IsNullOrEmpty(newValue)) output.Add(newValue);

            foreach (string it in output)
            {
                final += "{\"" + it + "\"}";
            }
            return final;
        }

        /// <summary>
        /// V3: Od multiLine Stringa pravi listu - ako string nema vise linija onda lista sadrzi Input kao prvi element
        /// </summary>
        /// <param name="input">Ulazni string</param>
        /// <param name="opts">Na koji način deli string</param>
        /// <param name="inverseNeedleTest">Da li da invertuje rezultat contain testa</param>
        /// <param name="onlyWithNeedle">Needle kojim se testira svaka linija. Ako je prazan onda se ne filtrira izlaz</param>
        /// <returns></returns>
        public static List<string> getStringLineList(this String input,
                                                     StringSplitOptions opts = StringSplitOptions.None,
                                                     String onlyWithNeedle = null, Boolean inverseNeedleTest = false)
        {
            List<string> output = new List<string>();
            String[] sep = { Environment.NewLine, "\n" };

            output.AddRange(input.Split(sep, opts));

            if (output.Count == 0)
            {
                output.Add(input);
            }

            if (!String.IsNullOrEmpty(onlyWithNeedle))
            {
                List<string> filtered = new List<string>();

                Boolean add = false;
                foreach (String line in output)
                {
                    add = line.Contains(onlyWithNeedle);
                    if (inverseNeedleTest) add = !add;
                    if (add) filtered.Add(line);
                }
                output = filtered;
            }
            return output;
        }

        /// <summary>
        /// Alatka koju koristi: imbFilterLine i imbXmlFilterLine - za polja koja podržavaju više elemenata
        /// Razbija input string u multiOps elemente. Ako je sve jedan element onda ga broji kao jedan multiOp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<string> multiOpsInputProcessing(String input)
        {
            List<string> multiOutput = new List<string>();

            if (String.IsNullOrEmpty(input)) return multiOutput;
            MatchCollection multiOps = _multiOpRe.Matches(input);
            if (multiOps.Count == 0)
            {
                multiOutput.Add(cleanMultiOpMarks(input));
            }
            else
            {
                foreach (Match item in multiOps)
                {
                    multiOutput.Add(cleanMultiOpMarks(item.Value));
                }
            }
            return multiOutput;
        }

        /// <summary>
        /// Primenjuje registar specijalnih znakova na zadati unos
        /// </summary>
        /// <param name="input">Lista parametara koji su proslednjeni filter komandi</param>
        /// <returns></returns>
        public static List<string> deploySpecialMarks(List<string> input)
        {
            List<string> output = new List<string>();
            foreach (String line in input)
            {
                output.Add(deploySpecialMarks(line));
            }
            return output;
        }
    }
}