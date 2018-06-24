// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringSelect.cs" company="imbVeles" >
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

    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    #endregion imbVeles using

    /// <summary>
    /// Static class with a number of Regex patterns and methods to test and retrieve
    /// </summary>
    /// \ingroup_disabled ace_ext_string
    public static class imbStringSelect
    {
        /// <summary>
        /// Regex select tokenSplitter : (([\W\s\+\.\,]+){1,}$) -- the same <c>contentToken</c> class uses
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_tokenSplitter = new Regex(@"(([\W\s\+\.\,]+){1,}$)", RegexOptions.Compiled);

        /// <summary>
        /// Regex select regexName : ([\w]{1}\.{1}\s{1}){2,}
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_regexName = new Regex(@"([\w]{1}\.{1}\s{1}){2,}", RegexOptions.Compiled);

        /// <summary>
        /// Match Evaluation for regexName : _select_regexName
        /// </summary>
        /// <param name="m">Match with value to process</param>
        /// <returns>For m.value "something" returns "SOMETHING"</returns>
        private static String _replace_regexName(Match m)
        {
            String output = m.Value.Replace(".", "");
            output = output.Replace(" ", "");

            return output.ToUpper();
        }

        /// <summary>
        /// Regex select RegexName : ([\w]{1}\.{1}\s{1}){2,}
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isRegexName = new Regex(@"([\w]{1}\.{1}\s{1}){2,}", RegexOptions.Compiled);

        /// <summary>
        /// Test if input matches ([\w]{1}\.{1}\s{1}){2,}
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isRegexName</returns>
        public static Boolean isRegexName(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isRegexName.IsMatch(input);
        }

        /// <summary>
        /// Regex select IsTokenStream : [\s\,\;\-]+
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isIsTokenStream = new Regex(@"[\s\,\;\-]+", RegexOptions.Compiled);

        /// <summary>
        /// Test if input matches [\s\,\;\-]+ : checks if the input is a solid, single token (false) or it is one of token streams kinds (like: "token,token,51", "tkn tk2, 50;", "50;rk;23", "tk-tj"
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isIsTokenStream</returns>
        public static Boolean isTokenStream(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isIsTokenStream.IsMatch(input);
        }

        /// <summary>
        /// opposite to <see cref="isTokenStream"/> test
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if [is single token] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean isSingleToken(this String input)
        {
            return !input.isTokenStream();
        }

        //public static Boolean isSequenceString(this String input)
        //{
        //}

        //#region REGEX BUNDLE

        ///// <summary>
        ///// Regex expression to select $property$
        ///// </summary>
        ///// <remarks>
        ///// <para>For text: </para>
        ///// <para>Selects: </para>
        ///// </remarks>
        //public static Regex _select_$property$ = new Regex(@"([\w]{1}\.{1}\s{1}){2,}", RegexOptions.Compiled);

        ///// <summary>
        ///// Refills the Matches with refilement parameter: ###
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="refilement"></param>
        ///// <returns></returns>
        //public static String replace_$property$(this String input, String refilement = "")
        //{
        //    String matchedStrings = input;
        //    var matched = _select_$property$.Matches(input);
        //    foreach (Match m in matched)
        //    {
        //        if (m.Success)
        //        {
        //            String insert = refilement.toWidthExact(m.Length, refilement);
        //            String sufix = matchedStrings.Substring(m.Index + m.Length);
        //            matchedStrings = matchedStrings.Substring(0, m.Index) + insert + sufix;
        //        }
        //    }
        //    return _select_$property$.Replace(input, refilement);
        //}

        ///// <summary>
        ///// Select all $property$ found using Regex _select_$property$
        ///// </summary>
        ///// <param name="m">text to extract from</param>
        ///// <returns>Returns list of matched substrings. Returns empty List if no match found</returns>
        //public static List<String> selectList_$property$(this String input)
        //{
        //    List<String> matchedStrings = new List<string>();
        //    var matched = _select_$property$.Matches(input);
        //    foreach (Match m in matched)
        //    {
        //        if (m.Success) matchedStrings.Add(m.Value);
        //    }

        //    return matchedStrings;
        //}

        ///// <summary>
        ///// Select $property$ : using Regex _select_$property$
        ///// </summary>
        ///// <param name="m">text to regex</param>
        ///// <returns>Returns value match. Returns Empty String if no match found</returns>
        //public static String select_$property$(this String input)
        //{
        //    Match m = _select_$property$.Match(input);
        //    if (m.Success) return m.Value;
        //    return "";

        //}

        ///// <summary>
        ///// Test if input is $property$ (match against ([\w]{1}\.{1}\s{1}){2,})
        ///// </summary>
        ///// <param name="input">String to test</param>
        ///// <returns>IsMatch against _select_is$property$ - returns TRUE if input returns 1 or more Match results</returns>
        //public static Boolean is$property$Match(this String input)
        //{
        //    if (String.IsNullOrEmpty(input)) return false;
        //    return _select_$property$.IsMatch(input);
        //}

        //#endregion

        /// <summary>
        /// Regex select inlineTypedProperty : (.*):(.*)=(.*);
        /// </summary>
        /// <remarks>
        /// <para>cs_y0:Decimal = #.00000;</para>
        /// <para>Selects: cs_y0, Decimal, #.00000</para>
        /// </remarks>
        public static Regex _select_inlineTypedProperty = new Regex(@"(.*):(.*)=(.*);", RegexOptions.Compiled);

        /// <summary>
        /// Match Evaluation for inlineTypedProperty : _select_inlineTypedProperty
        /// </summary>
        /// <param name="input">cs_y0:Decimal = #.00000;</param>
        /// <returns>[0]=paramname;[1]=typename;[2]=format</returns>
        public static List<String> select_inlineTypedProperty(this String input)
        {
            List<String> output = new List<string>();

            MatchCollection mch = _select_inlineTypedProperty.Matches(input);

            if (mch.Count > 0)
            {
                foreach (Group m in mch[0].Groups)
                {
                    if (m is Match)
                    {
                    }
                    else
                    {
                        String vl = m.Value;
                        if (!String.IsNullOrEmpty(vl))
                        {
                            output.Add(vl.Trim());
                        }
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Regex select PostOfficeNumber : (([\d]{5})|([\d]{2}[\s]{1}[\d]{3})[\b]{1})[\W]{1}
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isPostOfficeNumber = new Regex(
            @"(([\d]{5})|([\d]{2}[\s]{1}[\d]{3})[\b]{1})[\W]{1}", RegexOptions.Compiled);

        /// <summary>
        /// Regex select YearNumber : ([\d]{4}[\.])
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isYearNumber = new Regex(@"([\d]{4}[\.])", RegexOptions.Compiled);

        /// <summary>
        /// Regex select AcronimByLength : [\s\b]{1}([A-Z]{3,4})[\s\b]{1}
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isAcronimByLength = new Regex(@"[\s\b]{1}([A-Z]{3,4})[\s\b]{1}",
                                                                  RegexOptions.Compiled);

        /// <summary>
        /// Regex select AcronimIrregular : [\s]{1}([ZXCVBNMKLJHGFDSQWRTYP]{2,})[\s]{1}
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isAcronimIrregular = new Regex(@"[\s]{1}([ZXCVBNMKLJHGFDSQWRTYP]{2,})[\s]{1}",
                                                                   RegexOptions.Compiled);

        /// <summary>
        /// Regex select EmailAddress : [a-zA-Z\d_\.]+@[a-zA-Z_\.]*?\.[a-zA-Z\.]{2,6}
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isEmailAddress = new Regex(@"[a-zA-Z\d_\.]+@[a-zA-Z_\.]*?\.[a-zA-Z\.]{2,6}",
                                                               RegexOptions.Compiled);

        /// <summary>
        /// Regex select Standards : [A-Z]{1,5}[\s\-\:]*[\d]{2,5}[\d\:]*
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isStandards = new Regex(@"[A-Z]{1,5}[\s\-\:]*[\d]{2,5}[\d\:]*",
                                                            RegexOptions.Compiled);

        /// <summary>
        /// Regex select LettersWithDotsFromStart : \A([\w\.]*)
        /// </summary>
        /// <remarks>
        /// <para>For text: koplas.co.rs/index.php</para>
        /// <para>Selects: koplas.co.rs</para>
        /// </remarks>
        public static Regex _select_isLettersWithDotsFromStart = new Regex(@"\A([\w\.]*)", RegexOptions.Compiled);

        /// <summary>
        /// Test if input matches \A([\w\.]*)
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isLettersWithDotsFromStart</returns>
        public static Boolean isLettersWithDotsFromStart(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isLettersWithDotsFromStart.IsMatch(input);
        }

        /// <summary>
        /// Regex select phoneNumber : [\+0^]{1,2}[\s]{0,2}([\d\(\)]{2,5}[\s\(\)\-\.\/\\]{1,2}){3,5}
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isPhoneNumber = new Regex(@"([\+0]{0,5}[\(0\){0,5}[\d\s\-\\\.\/][^,]{5,20})", RegexOptions.Compiled);

        //   new Regex(@"[\+0^]{1,2}[\s]{0,2}([\d\(\)]{2,5}[\s\(\)\-\.\/\\]{1,2}){3,5}", RegexOptions.Compiled);

        /// <summary>
        /// Test if input matches (([\d]{5})|([\d]{2}[\s]{1}[\d]{3})[\b]{1})[\W]{1}
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isPostOfficeNumber</returns>
        public static Boolean isPostOfficeNumber(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isPostOfficeNumber.IsMatch(input);
        }

        /// <summary>
        /// Test if input matches ([\d]{4}[\.])
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isYearNumber</returns>
        public static Boolean isYearNumber(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isYearNumber.IsMatch(input);
        }

        /// <summary>
        /// Test if input matches [\s\b]{1}([A-Z]{3,4})[\s\b]{1}
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isAcronimByLength</returns>
        public static Boolean isAcronimByLength(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isAcronimByLength.IsMatch(input);
        }

        /// <summary>
        /// Test if input matches [\s]{1}([ZXCVBNMKLJHGFDSQWRTYP]{2,})[\s]{1}
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isAcronimIrregular</returns>
        public static Boolean isAcronimIrregular(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isAcronimIrregular.IsMatch(input);
        }

        /// <summary>
        /// Test if input matches [a-zA-Z\d_\.]+@[a-zA-Z_\.]*?\.[a-zA-Z\.]{2,6}
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isEmailAddress</returns>
        public static Boolean isEmailAddress(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isEmailAddress.IsMatch(input);
        }

        /// <summary>
        /// Test if input matches [A-Z]{1,5}[\s\-\:]*[\d]{2,5}[\d\:]*
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isStandards</returns>
        public static Boolean isStandards(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isStandards.IsMatch(input);
        }

        /// <summary>
        /// Test if input matches [\+0^]{1,2}[\s]{0,2}([\d\(\)]{2,5}[\s\(\)\-\.\/\\]{1,2}){3,5}
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isphoneNumber</returns>
        public static Boolean isPhoneNumber(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isPhoneNumber.IsMatch(input);
        }

        /// <summary>
        /// 2017 Regex select wordsFromDomainname : ([\w]{1}\.{1}\s{1}){2,}
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_wordsFromDomainname = new Regex(@"([\w\d]+)", RegexOptions.Compiled);

        /// <summary>
        /// Regex select rootDomainNameWithoutRelPath : (?:http://)?(?:www\.)?([\w\d\.-]+)
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_rootDomainNameWithoutRelPath = new Regex(@"(?:http://)?(?:www\.)?([\w\d\.-]+)", RegexOptions.Compiled);
    }
}