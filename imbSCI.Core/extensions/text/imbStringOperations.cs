// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringOperations.cs" company="imbVeles" >
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
    using imbSCI.Core.math;
    #region imbVeles using

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    #endregion imbVeles using

    public static class imbStringOperations
    {
        #region --- charsToIgnore_compareStrings ------- karakteri koje treba da ignorise prilikom specijalnih compareStrings modova

        private static List<String> _charsToIgnore_compareStrings;

        /// <summary>
        /// karakteri koje treba da ignorise prilikom specijalnih compareStrings modova
        /// </summary>
        public static List<String> charsToIgnore_compareStrings
        {
            get
            {
                if (_charsToIgnore_compareStrings == null)
                {
                    _charsToIgnore_compareStrings = new List<String>();
                    _charsToIgnore_compareStrings.Add(" ");
                    _charsToIgnore_compareStrings.Add(".");
                    _charsToIgnore_compareStrings.Add(";");
                    _charsToIgnore_compareStrings.Add("_");
                    _charsToIgnore_compareStrings.Add("-");
                    _charsToIgnore_compareStrings.Add(",");
                    _charsToIgnore_compareStrings.Add("!");
                    _charsToIgnore_compareStrings.Add("?");
                    _charsToIgnore_compareStrings.Add("~");
                    _charsToIgnore_compareStrings.Add("+");
                    _charsToIgnore_compareStrings.Add(":");
                }
                return _charsToIgnore_compareStrings;
            }
        }

        #endregion --- charsToIgnore_compareStrings ------- karakteri koje treba da ignorise prilikom specijalnih compareStrings modova

        /// <summary>
        /// Key je mySqlTypeName *kako se koristi na serveru* a Value je .NET ekvivalent
        /// </summary>
        private static Dictionary<String, String> mySqlTypeVsSystemType;

        private static Dictionary<String, String> systemTypeVsMySqlType;

        /// <summary>
        /// Pravi MPS line na osnovu prosledjenog podatka
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String getMPSLine(String path, String value)
        {
            String output = "";
            output += "[[[" + Environment.NewLine;
            output += path + Environment.NewLine;
            output += value + Environment.NewLine;
            output += "]]]" + Environment.NewLine;
            return output;
        }

        /*
        /// <summary>
        /// Konvertuje jednu tipografiju u drugu
        /// Ako je inverse false: onda je inputName ime ya MySqlTip
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="inverse"></param>
        /// <returns></returns>
        public static String sqlTypeToSystem(String inputName, Boolean inverse = false)
        {
            if (mySqlTypeVsSystemType == null)
            {
                mySqlTypeVsSystemType = new Dictionary<string, string>();
                mySqlTypeVsSystemType.Add("int(11)", "Int32");
                mySqlTypeVsSystemType.Add("text", "String");
                mySqlTypeVsSystemType.Add("boolean", "Boolean");
                mySqlTypeVsSystemType.Add("DATETIME", "DateTime");

                systemTypeVsMySqlType = new Dictionary<string, string>();

                foreach (String key in mySqlTypeVsSystemType.Keys)
                {
                    systemTypeVsMySqlType.Add(mySqlTypeVsSystemType[key], key);
                }
            }

            Dictionary<String, String> source;
            String defOut = "";
            String output = "";

            if (inverse)
            {
                source = systemTypeVsMySqlType;
                defOut = "longtext";
            }
            else
            {
                source = mySqlTypeVsSystemType;
                defOut = "String";
            }

            if (!source.ContainsKey(inputName))
            {
                inputName = inputName.ToLower();
            }

            if (!source.ContainsKey(inputName))
            {
                inputName = "System." + inputName;
            }

            if (!source.ContainsKey(inputName))
            {
                output = defOut;
            }
            else
            {
                output = source[inputName];
            }

            return output;
        }*/

        /// <summary>
        /// Sklapanje dva stringa
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="oldString"></param>
        /// <param name="newString"></param>
        /// <returns></returns>
        public static String executeStringOperation(stringOperation operation, String oldString, String newString)
        {
            switch (operation)
            {
                case stringOperation.after:
                    return oldString + newString;
                    break;

                case stringOperation.before:
                    return newString + oldString;
                    break;

                case stringOperation.replace:
                    return newString;
                    break;

                default:
                    return oldString;
                    break;
            }
        }

        /// <summary>
        /// imbV3: Vrednosti iz prosledjenog rečnika zamenjuje tamo gde pronađe $(key) u inputu
        /// </summary>
        /// <param name="values"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String deployValuesToString(Dictionary<String, String> values, String input)
        {
            String pattern = @"\$(.*?)\)";

            Regex tmpReg = new Regex(pattern);

            MatchCollection valPlaces = tmpReg.Matches(input);

            String indStr = "";
            foreach (Match valPlace in valPlaces)
            {
                try
                {
                    indStr = valPlace.Value;
                    indStr = indStr.Replace("$(", "");
                    indStr = indStr.Replace(")", "");
                }
                catch
                {
                    indStr = "";
                }

                if (values.ContainsKey(indStr))
                {
                    input = input.Replace(valPlace.Value, values[indStr]);
                }
            }
            return input;
        }

        /// <summary>
        /// Cisti iz Stringa space i newLine
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<String> getWashed(List<String> input)
        {
            List<String> output = new List<string>();
            String tmp;
            foreach (String item in input)
            {
                tmp = item.Trim();

                tmp = tmp.ToLower();
                tmp.Replace(Environment.NewLine, "");
                output.Add(tmp);
            }
            return output;
        }

        /// <summary>
        /// Removes all occuring chars from the <c>input</c>
        /// </summary>
        /// <param name="needle">Chars to remove from the string</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static String removeChars(IEnumerable<Char> needle, String input)
        {
            foreach (char ned in needle)
            {
                input = input.Replace(ned.ToString(), "");
            }
            return input;
        }

        /// <summary>
        /// Poređenje stringova korišćenjem imena stringMatchPolicy enuma
        /// </summary>
        /// <param name="operantA">Prvi operant</param>
        /// <param name="operantB">Drugi operant</param>
        /// <param name="compareMode">Ime stringMatchPolicy</param>
        /// <returns>Rezultat</returns>
        public static Boolean compareStrings(String operantA, String operantB, String compareMode)
        {
            stringMatchPolicy compare = stringMatchPolicy.trimSpaceCaseFree;

            Enum.TryParse<stringMatchPolicy>(compareMode, out compare);

            return compareStrings(operantA, operantB, compare);
        }

        /// <summary>
        /// Returns string length or it's integer value in case it is a proper number, e.g.: 4, or 6.2
        /// </summary>
        /// <param name="input">String form of a number or real textual string</param>
        /// <returns></returns>
        public static Int32 getLengthOrNumber(String input)
        {
            if (input.isNumber())
            {
                return Convert.ToInt32(input);
            }
            else
            {
                if (input.isDecimalNumber())
                {
                    return Convert.ToInt32(Convert.ToDouble(input));
                }
            }

            return input.Length;
        }

        /// <summary>
        ///  Makes inversed string
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static String Inverse(this String input)
        {
            String output = "";
            var outputChars = new List<Char>() { };
            for (int i = 0; i < input.Length; i++)
            {
                outputChars.Add(input[(input.Length - 1) - i]);
            }
            return String.Concat(outputChars);
        }

        /// <summary>
        /// It extracts common substring at left end (at start, root) of input strings
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <param name="tolerance">The tolerance: what proportion of input strings could be excluded in order to get longest possible common root from the rest of the input</param>
        /// <returns></returns>
        public static String commonStartSubstring(this IEnumerable<String> strings, Double tolerance = 0.1)
        {

            List<String> iteration = strings.ToList();
            List<String> excluded = new List<string>();
            StringBuilder sb = new StringBuilder();

            Int32 inputSize = iteration.Count;

            Int32 p = 0;
            while (iteration.Any())
            {
                List<String> nextIteration = new List<string>();
                String currentChar = "";

                foreach (String str in iteration)
                {
                    if (p < str.Length)
                    {

                        if (currentChar == "")
                        {
                            currentChar = str.Substring(p, 1);
                            nextIteration.Add(str);
                        }
                        else
                        {
                            var candidateChar = str.Substring(p, 1);
                            if (candidateChar == currentChar)
                            {
                                nextIteration.Add(str);
                            }
                            else
                            {
                                excluded.Add(str);
                            }
                        }
                    }
                    else
                    {
                        excluded.Add(str);
                    }

                }

                if (excluded.Count.GetRatio(inputSize) > tolerance)
                {
                    break;
                }
                else
                {
                    if (currentChar != "")
                    {
                        sb.Append(currentChar);
                    }
                }
                p++;
                iteration = nextIteration;
            }

            String output =  sb.ToString();

            return output;
        }




        /// <summary>
        /// Extracts one longest common sugstring
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static string longestCommonSubstring(IList strings)
        {
            if (strings == null) return "";
            if (strings.Count == 0) return "";

            List<String> list = longestCommonSubstrings(strings).ToList();

            if (list != null)
            {
                if (list.Count == 0) return "";

                return list.First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Extracts the longest possibile common substrings
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static IEnumerable<string> longestCommonSubstrings(IList strings)
        {
            if (strings == null) return null;
            if (strings.Count == 0) return null;

            List<String> inputStrings = new List<string>();
            foreach (var ino in strings)
            {
                if (ino is String) if (ino != null) inputStrings.Add(ino as String);
            }

            var firstString = strings[0] as String;

            var allSubstrings = new List<string>();
            for (int substringLength = firstString.Length; substringLength > 0; substringLength--)
            {
                for (int offset = 0; (substringLength + offset) <= firstString.Length; offset++)
                {
                    string currentSubstring = "";
                    currentSubstring = firstString.Substring(offset, substringLength);
                    if (!System.String.IsNullOrWhiteSpace(currentSubstring) &&
                        !allSubstrings.Contains(currentSubstring))
                    {
                        allSubstrings.Add(currentSubstring);
                    }
                }
            }

            List<String> output =
                allSubstrings.OrderBy(subStr => subStr).ThenByDescending(subStr => subStr.Length).Where(
                    subStr => inputStrings.All<String>(currentString => currentString.Contains(subStr))).ToList();

            return output.OrderBy(x => x.Length).Reverse();
        }

        /// <summary>
        /// Uporedjuje dva stringa pomocu datog string match policija
        /// </summary>
        /// <param name="compareMode">Nacin poredjenja</param>
        /// <param name="operantA"></param>
        /// <param name="operantB"></param>
        /// <returns></returns>
        public static Boolean compareStrings(this stringMatchPolicy compareMode, String operantA, String operantB)
        {
            return operantA.compareStrings(operantB, compareMode);
        }

        /// <summary>
        /// Uporedjuje string prema zadatom uslovu
        /// </summary>
        /// <param name="operantA">Search content - source</param>
        /// <param name="operantB">Search needle - keyword</param>
        /// <param name="compareMode"></param>
        /// <returns></returns>
        public static Boolean compareStrings(this String operantA, String operantB, stringMatchPolicy compareMode)
        {
            Boolean output = false;
            if (operantA == null)
            {
                operantA = "";
            }

            if (operantB == null)
            {
                operantB = "";
            }

            switch (compareMode)
            {
                case stringMatchPolicy.caseFree:
                    output = (operantA.ToLower() == operantB.ToLower());
                    break;

                case stringMatchPolicy.length:
                    output = (operantA.Length == operantB.Length);
                    break;

                case stringMatchPolicy.lengthMore:
                    output = (operantA.Length > getLengthOrNumber(operantB));
                    break;

                case stringMatchPolicy.lengthLess:
                    output = (operantA.Length <= getLengthOrNumber(operantB));
                    break;

                case stringMatchPolicy.containCaseFree:
                    output = (operantA.ToLower().IndexOf(operantB.ToLower()) > -1);
                    break;

                case stringMatchPolicy.similar:
                    output = (operantA == operantB);
                    if (!output)
                    {
                        operantA = operantA.ToLower();
                        operantB = operantB.ToLower();
                        output = (operantA == operantB);
                    }
                    if (!output)
                        output = (operantA.removeFromString(charsToIgnore_compareStrings) ==
                                  operantB.removeFromString(charsToIgnore_compareStrings));
                    break;

                case stringMatchPolicy.containExact:
                    output = (operantA.IndexOf(operantB) > -1);
                    break;

                case stringMatchPolicy.trimSpaceExact:
                    operantA = operantA.Trim();
                    operantB = operantB.Trim();
                    output = (operantA == operantB);
                    break;

                case stringMatchPolicy.trimSpaceCaseFree:
                    operantA = operantA.Trim();
                    operantB = operantB.Trim();
                    output = (operantA.ToLower() == operantB.ToLower());
                    break;

                case stringMatchPolicy.trimSpaceContainExact:
                    operantA = operantA.Trim();
                    operantB = operantB.Trim();
                    output = (operantA.IndexOf(operantB) > -1);
                    break;

                case stringMatchPolicy.trimSpaceContainCaseFree:
                    operantA = operantA.Trim().ToLower();
                    operantB = operantB.Trim().ToLower();
                    output = (operantA.ToLower().IndexOf(operantB.ToLower()) > -1);
                    break;

                case stringMatchPolicy.overrideFalse:
                    output = false;
                    break;

                case stringMatchPolicy.overrideTrue:
                    output = true;
                    break;

                case stringMatchPolicy.exact:
                default:
                    output = (operantA == operantB);
                    break;
            }

            return output;
        }


        /// <summary>
        /// Trims to limit.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="removeNewLine">if set to <c>true</c> [remove new line].</param>
        /// <param name="sufix">The sufix.</param>
        /// <param name="isInvert">if set to <c>true</c> [is invert].</param>
        /// <param name="snapToWord">if set to <c>true</c> [snap to word].</param>
        /// <returns></returns>
        public static List<String> trimToLimit(this List<String> input, int limit, Boolean removeNewLine, String sufix = "... ",
                                         Boolean isInvert = false, Boolean snapToWord = true)
        {
            List<String> output = new List<string>();

            foreach (String l in input)
            {
                output.Add(l.trimToLimit(limit, removeNewLine, sufix, isInvert, snapToWord));
            }
            return output;
        }


        /// <summary>
        /// Trims the string to given length limit, and adds sufix if trimmed. 
        /// </summary>
        /// <param name="input">String koji se skraćuje</param>
        /// <param name="limit">Do koje dužine je dozvoljen</param>
        /// <param name="removeNewLine">Da li briše i NewLine tagove</param>
        /// <param name="sufix">Koji sufix da stavi u nastavku</param>
        /// <param name="isInvert">Ako je True onda ce skratiti string sa desne strane</param>
        /// <returns></returns>
        public static String trimToLimit(this String input, int limit, Boolean removeNewLine, String sufix = "... ",
                                         Boolean isInvert = false, Boolean snapToWord = true)
        {
            if (String.IsNullOrEmpty(input)) return "";

            input = input.Trim();

            if (removeNewLine) input = input.imbRemoveDouble(" ", Environment.NewLine, ".", ",");

            if (input.Length > limit)
            {
                if (snapToWord)
                {
                    Int32 spc = input.IndexOf(' ', limit);
                    if (spc > -1)
                    {
                        limit = spc;
                    }
                }
                if (isInvert)
                {
                    Int32 start = input.Length - limit;
                    input = input.Substring(start, limit);
                    input = sufix + input;
                }
                else
                {
                    input = input.Substring(0, limit);
                    input = input + sufix;
                }
            }

            return input;
        }

        ///// <summary>
        ///// 2014 Maj: uklanja duple substringove - dupli razmak, newline, ..
        ///// </summary>
        ///// <param name="source">Ulazni string</param>
        ///// <param name="toRemove">substringovi u jednostrukoj formi - ako treba da se uklone svi .. onda navesti . - ako je null onda primenjuje default skup: space, newline, .</param>
        ///// <returns>Ociscen string</returns>
        //public static String imbRemoveDouble(this String source, params String[] toRemove)
        //{
        //    if (toRemove == null)
        //    {
        //        toRemove = new[] {" ", Environment.NewLine, "."};
        //    }
        //    String out2 = source;
        //    foreach (String tr in toRemove)
        //    {
        //        String dd = tr + tr;
        //        while (out2.Contains(dd))
        //        {
        //            out2 = out2.Replace(dd, tr);
        //        }
        //    }
        //    return out2;
        //}

        public static String getRecomandedType(String text)
        {
            String dataType = "";

            Double douRes;
            if (Double.TryParse(text, out douRes)) dataType = "Double";

            Int32 intRes;
            if (Int32.TryParse(text, out intRes)) dataType = "Int32";

            switch (text)
            {
                case "No":
                case "Yes":
                case "True":
                case "False":
                case "no":
                case "yes":
                case "true":
                case "false":
                    dataType = "Boolean";
                    break;
            }

            return dataType;
        }

        public static string trimAllInside(String input, String leftLimit, String rightLimit)
        {
            String str = input;

            int start = 0;
            int end = 0;
            int count = 0;

            Boolean cont = true;

            while (cont)
            {
                start = str.IndexOf(leftLimit);
                end = str.IndexOf(rightLimit);
                cont = true;
                if (start == -1) cont = false;
                if (end == -1) cont = false;

                if (start > end) cont = false;

                if (cont)
                {
                    count = end - start + 1;
                    if (count > (str.Length - start))
                    {
                        count = str.Length - start;
                    }

                    str = str.Remove(start, count);
                }
            }

            return str;
        }
    }
}