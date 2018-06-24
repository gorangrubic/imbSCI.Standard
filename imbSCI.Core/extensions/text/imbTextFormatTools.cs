// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTextFormatTools.cs" company="imbVeles" >
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

    /// <summary>
    /// Set alata za obradu Stringova za potrebe NC
    /// </summary>
    public static class imbTextFormatTools
    {
        /// <summary>
        /// Helper method, actually just adds <see cref="Environment.NewLine"/> before the message specified -- alias of <see cref="addLine(string, string)"/>, don't use this.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        /// <exception cref="aceCommonTypes.core.exceptions.aceGeneralException"></exception>
        public static string log(this String output, String msg)
        {
            output += Environment.NewLine + msg;

            return output;
        }

        /// <summary>
        /// Adds the line.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public static string addLine(this String output, String msg)
        {
            output = output + Environment.NewLine + msg;

            return output;
        }

        /// <summary>
        /// Adds the value.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="input">The input.</param>
        /// <param name="value">The value.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string addVal(this String output, String input, String value, String separator = " ", String format = "{0} [{1}]")
        {
            return output.add(String.Format(format, input, value), separator);
        }

        /// <summary>
        /// News the line.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        public static string newLine(this String output)
        {
            return output.add(Environment.NewLine, "");
        }

        /// <summary>
        /// Repeats the specified times.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="times">The times.</param>
        /// <returns></returns>
        public static string Repeat(this String input, Int32 times)
        {
            String output = "";
            times = Math.Abs(times);
            for (Int32 i = 0; i < times; i++)
            {
                output = output + input;
            }
            return output;
        }

        /// <summary>
        /// To the width exact.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="targetWidth">Width of the target.</param>
        /// <param name="charToRepeat">The character to repeat.</param>
        /// <returns></returns>
        public static String toWidthExact(this String target, Int32 targetWidth, String charToRepeat = " ")
        {
            String output = target;
            if (output == null) output = "";
            targetWidth = Math.Abs(targetWidth);
            if (output.Length > targetWidth)
            {
                output = output.PadRight(output.Length - targetWidth);
            }
            if (output.Length < targetWidth)
            {
                String d = charToRepeat.Repeat(output.Length - targetWidth);
                output = output + d;
            }
            return output;
        }

        /// <summary>
        /// To the width maximum.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="targetWidth">Width of the target.</param>
        /// <param name="charToRepeat">The character to repeat.</param>
        /// <returns></returns>
        public static String toWidthMaximum(this String target, Int32 targetWidth, String charToRepeat = "...")
        {
            String output = target;
            targetWidth = Math.Abs(targetWidth);
            if (output == null) output = "";
            if (output.Length > targetWidth)
            {
                output = output.Substring(0, targetWidth - charToRepeat.Length) + charToRepeat;

                //output = output.PadRight(output.Length - targetWidth);
            }

            return output;
        }

        /// <summary>
        /// Makes a String with defined minimal width. Longer strings are unchanged, shorter are expanded by <c>charToRepeat</c> until the width is achieved.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="targetWidth">Targeted width</param>
        /// <param name="charToRepeat">The character to repeat to extend strings shorter than target.</param>
        /// <returns>String that has at least width specified </returns>
        public static String toWidthMinimum(this String target, Int32 targetWidth, String charToRepeat = " ")
        {
            String output = target;
            targetWidth = Math.Abs(targetWidth);
            if (output == null) output = "";
            if (output.Length < targetWidth)
            {
                output = output + charToRepeat.Repeat(output.Length - targetWidth);
            }
            return output;
        }

        /// <summary>
        /// Calls multiple <c>add</c> times using provided <c>separator</c> and collection of inputs.
        /// </summary>
        /// <param name="output">The output - string to be added</param>
        /// <param name="seperator">The seperator - to be inserted between content and added content</param>
        /// <param name="inputs">The inputs: strings and/or IEnumerable with strings</param>
        /// <returns>String versuon with all inputs added</returns>
        public static string adds(this String output, String seperator, params String[] inputs)
        {
            List<String> input = inputs.getFlatList<String>();

            foreach (String inp in inputs)
            {
                output = output.add(inp, seperator);
            }

            return output;
        }

        /// <summary>
        /// VRaca string format vrednost - u skladu sa string format pravilima> #.000 ce vratiti broj sa tri decimale
        /// </summary>
        /// <param name="numericInput">String koji sadrzi brojnu vrednost sa decimalama, npr. 25.00 </param>
        /// <param name="defaultDecNumbers">Koliko ce decimalnih vrednosti zabeleziti ako nema uopste decimale</param>
        /// <returns></returns>
        public static String getFormatFromExample(this String numericInput, Int32 defaultDecNumbers = 2)
        {
            String format = "#0";
            Int32 decNumbers = defaultDecNumbers;

            if (numericInput.Contains('.'))
            {
                List<String> fmp = numericInput.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                if (fmp.Count() > 1)
                {
                    decNumbers = fmp[1].Length;
                }
            }

            if (decNumbers > 0)
            {
                format += ".";
            }
            else
            {
                return "";
            }

            for (Int32 a = 0; a < decNumbers; a++)
            {
                format += "0";
            }

            return format;
        }
    }
}