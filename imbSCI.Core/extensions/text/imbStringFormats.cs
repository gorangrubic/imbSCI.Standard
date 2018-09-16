// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringFormats.cs" company="imbVeles" >
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
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Formatting strings
    /// </summary>
    public static class imbStringFormats
    {
        /// <summary>
        /// Gets the proper to string number dn.
        /// </summary>
        /// <param name="input">Highest number of items to show row number for</param>
        /// <param name="minCharacters">The minimum characters - leading 0's.</param>
        /// <returns>Format string ready for <c>ToString(format)</c> call</returns>
        public static String getToStringDFormat(this Int32 input, Int32 minCharacters = 3)
        {
            //String test = input.ToString();
            Int32 len = input.GetTextualLength();
            if (len < minCharacters) len = minCharacters;
            return "D" + len.ToString();
        }

        /// <summary>
        /// Counts the digits.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public static int CountDecimalDigits(double num)
        {
            int rv = 0;
            const double insignificantDigit = 8;
            double intpart, fracpart;
            fracpart = num % 10; // Math.f modf(num, out intpart);
            intpart = num / 10;

            while ((Math.Abs(fracpart) > 0.000000001f) && (rv < insignificantDigit))
            {
                num *= 10;
                rv++;
                fracpart = num % 10;
                intpart = num / 10;
            }

            return rv;
        }

        public static Int32 GetTextualLength(this Double value)
        {
            return (value == 0 ? 1 : ((int)(Math.Log10(Math.Abs(value)) + 1) + (value < 0 ? 1 : 0)));
        }

        public static Int32 GetTextualLength(this Int32 value)
        {
            return (value == 0 ? 1 : ((int)(Math.Log10(Math.Abs(value)) + 1) + (value < 0 ? 1 : 0)));
        }

        /// <summary>
        /// To the numbered list prefix.
        /// </summary>
        /// <param name="ordinalPosition">The ordinal position.</param>
        /// <param name="level">The level.</param>
        /// <param name="levelLimit">The level limit.</param>
        /// <returns></returns>
        public static String toNumberedListPrefix(this Int32 ordinalPosition, Int32 level, Int32 levelLimit = 3)
        {
            String output = "";
            if (level > levelLimit) level = levelLimit;

            //Int32 lc = levelCount[i];
            switch (level)
            {
                case 0:
                    output = output.add(String.Format("{0:D3}", ordinalPosition), " ");
                    break;

                case 1:
                    output = output.add(String.Format("{0}", ordinalPosition), ".");
                    break;

                case 2:
                    output = output.add(String.Format("{0}", ((long)ordinalPosition).DecimalToOrdinalLetterSystem()), ".");
                    break;

                default:
                    //output = output.add(String.Format("{0}", lc), ".");
                    break;
            }
            return output;
        }

        public static Int32 ordinalLetterBase = 25;

        /// <summary>
        /// Converts the given decimal number to the numeral system with the
        /// specified radix (in the range [2, 36]).
        /// </summary>
        /// <param name="decimalNumber">The number to convert.</param>
        /// <param name="radix">The radix of the destination numeral system (in the range [2, 36]).</param>
        /// <remarks>http://www.pvladov.com/2012/07/arbitrary-to-decimal-numeral-system.html</remarks>
        /// <returns></returns>
        public static string DecimalToArbitrarySystem(long decimalNumber, int radix)
        {
            const int BitsInLong = 64;
            const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " + Digits.Length.ToString());

            if (decimalNumber == 0)
                return "0";

            int index = BitsInLong - 1;
            long currentNumber = Math.Abs(decimalNumber);
            char[] charArray = new char[BitsInLong];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % radix);
                charArray[index--] = Digits[remainder];
                currentNumber = currentNumber / radix;
            }

            string result = new String(charArray, index + 1, BitsInLong - index - 1);
            if (decimalNumber < 0)
            {
                result = "-" + result;
            }

            return result;
        }

        /// <summary>
        /// Converts the given number from the numeral system with the specified
        /// radix (in the range [2, 36]) to decimal numeral system.
        /// </summary>
        /// <param name="number">The arbitrary numeral system number to convert.</param>
        /// <param name="radix">The radix of the numeral system the given number
        /// is in (in the range [2, 36]).</param>
        /// <remarks>http://www.pvladov.com/2012/07/arbitrary-to-decimal-numeral-system.html</remarks>
        /// <returns></returns>
        public static long ArbitraryToDecimalSystem(string number, int radix)
        {
            const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " +
                    Digits.Length.ToString());

            if (String.IsNullOrEmpty(number))
                return 0;

            // Make sure the arbitrary numeral system number is in upper case
            number = number.ToUpperInvariant();

            long result = 0;
            long multiplier = 1;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                char c = number[i];
                if (i == 0 && c == '-')
                {
                    // This is the negative sign symbol
                    result = -result;
                    break;
                }

                int digit = Digits.IndexOf(c);
                if (digit == -1)
                    throw new ArgumentException(
                        "Invalid character in the arbitrary numeral system number",
                        "number");

                result += digit * multiplier;
                multiplier *= radix;
            }

            return result;
        }

        /// <returns></returns>
        public static string DecimalToOrdinalLetterSystem(this long decimalNumber)
        {
            const int BitsInLong = 64;
            const int radix = 26;
            const string Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " + Digits.Length.ToString());

            if (decimalNumber == 0)
                return "0";

            int index = BitsInLong - 1;
            long currentNumber = Math.Abs(decimalNumber);
            char[] charArray = new char[BitsInLong];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % radix);
                charArray[index--] = Digits[remainder];
                currentNumber = currentNumber / radix;
            }

            string result = new String(charArray, index + 1, BitsInLong - index - 1);
            if (decimalNumber < 0)
            {
                result = "-" + result;
            }

            return result;
        }

        /// <returns></returns>
        public static long OrdinalLetterToDecimalSystem(this string ordinalLetter)
        {
            const string Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const int radix = 26;

            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " +
                    Digits.Length.ToString());

            if (String.IsNullOrEmpty(ordinalLetter))
                return 0;

            // Make sure the arbitrary numeral system number is in upper case
            ordinalLetter = ordinalLetter.ToUpperInvariant();

            long result = 0;
            long multiplier = 1;
            for (int i = ordinalLetter.Length - 1; i >= 0; i--)
            {
                char c = ordinalLetter[i];
                if (i == 0 && c == '-')
                {
                    // This is the negative sign symbol
                    result = -result;
                    break;
                }

                int digit = Digits.IndexOf(c);
                if (digit == -1)
                    throw new ArgumentException(
                        "Invalid character in the arbitrary numeral system number",
                        "number");

                result += digit * multiplier;
                multiplier *= radix;
            }

            return result;
        }

        /// <summary>
        /// Reformats multiline string by adding specified prefix.
        /// </summary>
        /// <param name="source">The source string with or without </param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="prefixOnceOnly">if set to <c>true</c> [prefix once only].</param>
        /// <returns></returns>
        public static String toPrefixedMultiline(this String source, String prefix = " > ", Boolean prefixOnceOnly = false)
        {
            var lns = source.breakLines();

            String output = "";
            foreach (String ln in lns)
            {
                String iln = "";

                if (prefixOnceOnly)
                {
                    iln = ln.ensureStartsWith(prefix);
                }
                else
                {
                    iln = prefix + iln;
                }
                output = output.add(iln, Environment.NewLine);
            }
            return output;
        }

        /// <summary>
        /// Returns expression-ready string, wrapped in proper quotes if required
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="valueForNull">The value for null.</param>
        /// <returns></returns>
        public static String toExpressionString(this Object source, String valueForNull = "\"\"", String quote = "\"")
        {
            String output = "";

            if (source == null) return valueForNull;

            if (source is String)
            {
                output = source as String;
            }

            imbTypeGroup tg = source.GetType().getTypeGroup();

            switch (tg)
            {
                case imbTypeGroup.unknown:
                    return valueForNull;
                    break;

                case imbTypeGroup.number:
                    return source.ToString();
                    break;

                case imbTypeGroup.boolean:
                    return source.ToString();
                    break;

                case imbTypeGroup.text:
                    return quote + output + quote;
                    break;

                case imbTypeGroup.enumeration:

                    output = source.getEnumMemberPath(true, "_", false);
                    return output;
                    break;

                case imbTypeGroup.instance:
                    return nameof(source);
                    break;

                default:
                    return source.ToString();
                    break;
            }
        }

        public static String toStringSafe(this Object source, String valueForNull)
        {
            return source.toStringSafe(valueForNull, "");
        }

        public static String toStringSafe(this Object source)
        {
            return source.toStringSafe("", "");
        }

        /// <summary>
        /// Bezbedna konverzija u String -- ako je null onda valueForNull
        /// </summary>
        /// <param name="source"></param>
        /// <param name="valueForNull"></param>
        /// <returns>ako je null onda valueForNull</returns>
        public static String toStringSafe(this Object source, String valueForNull, String numberFormat)
        {
            if (numberFormat.isNullOrEmptyString()) numberFormat = "";

            if (source == null) return valueForNull;

            if (source is String)
            {
                return source as String;
            }
            if (source is Double)
            {
                return ((Double)source).ToString(numberFormat, CultureInfo.InvariantCulture);
            }

            if (source is Int32)
            {
                return ((Int32)source).ToString(numberFormat, CultureInfo.InvariantCulture);
            }

            if (source is DBNull)
            {
                return valueForNull;
            }

            if (source is Enum)
            {
                return source.keyToString();
            }

            String output = "";
            Type st = source.GetType();
            if (st is IEnumerable)
            {
                var gt = st.GetGenericArguments();
                if (gt.Any())
                {
                    output += gt.First().Name;
                }
                else
                {
                    // output += st.Name;
                }
                ICollection cl = source as ICollection;
                if (cl == null)
                {
                    IEnumerable el = source as IEnumerable;

                    if (el != null)
                    {
                        Int32 c = 0;
                        foreach (var e in el)
                        {
                            c++;
                        }
                        output += "[" + c.ToString() + "]";
                    }
                    else
                    {
                        output += source.ToString();
                    }
                }
                else
                {
                    output += "[" + cl.Count.ToString() + "]";
                }
                return output;
            }
            else if (st.isToggleValue() || st.isSimpleInputEnough())
            {
                return source.ToString();
            }
            else if (source is IObjectWithName)
            {
                return ((IObjectWithName)source).name;
            }
            else
            {
                return source.GetType().Name + "(..)";
            }

            return source.ToString();
        }

        /// <summary>
        /// Regex za selektovanje brojeva
        /// </summary>
        public static Regex _numOnly = new Regex(@"\D+");

        public enum fileSizeUnit
        {
            B = 0,
            Kb = 1,
            Mb = 2,
            Gb = 3
        }

        /// <summary>
        /// Converts byte count into nice Int32
        /// </summary>
        /// <param name="byteCount">The byte count.</param>
        //// <param name="order">The order: 0 (B), 1 (Kb), 2 (Mb), 3 (Gb). If -1 it will pick order automatically</param>
        /// <returns>Formated string of memory alocated: i.e.: 12.07kB, 5.62Mb</returns>
        public static long getFileSizeScaled(this long byteCount, fileSizeUnit __order = fileSizeUnit.B)
        {
            Int32 order = (Int32)__order;

            for (int i = 0; i < order; i++)
            {
                byteCount = byteCount / 1024;
            }

            // string result = String.Format("{0:0.##} {1}", byteCount, byteSizes[order]);
            return byteCount;
        }

        /// <summary>
        /// The byte sizes
        /// </summary>
        public static string[] byteSizes = { "B", "Kb", "Mb", "Gb" };

        /// <summary>
        /// Converts byte count into nice <c>file size</c> / memory allocation format
        /// </summary>
        /// <param name="byteCount">The byte count.</param>
        //// <param name="order">The order: 0 (B), 1 (Kb), 2 (Mb), 3 (Gb). If -1 it will pick order automatically</param>
        /// <returns>Formated string of memory alocated: i.e.: 12.07kB, 5.62Mb</returns>
        public static string writeFileSize(this long byteCount, int order = -1)
        {
            if (order == -1)
            {
                order = 0;
                while (byteCount >= 1024 && order + 1 < byteSizes.Length)
                {
                    order++;
                    byteCount = byteCount / 1024;
                }
            }
            else
            {
                for (int i = 0; i < order; i++)
                {
                    byteCount = byteCount / 1024;
                }
            }
            string result = String.Format("{0:0.##} {1}", byteCount, byteSizes[order]);
            return result;
        }

        /// <summary>
        /// Gets the size of the string in bytes (encoded to: UTF8) in well formated string. For order <see cref="byteSizes"/>
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="order">The order: 0 (B), 1 (Kb), 2 (Mb), 3 (Gb). If -1 it will pick order automatically</param>
        /// <returns></returns>
        public static String getStringMemByteSize(this String input, int order = -1)
        {
            return writeFileSize(Encoding.UTF8.GetByteCount(input.ToArray()), order);
        }

        /// <summary>
        /// Vraca Int32 na bezbedan nacin
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ifFail"></param>
        /// <returns></returns>
        public static Int32 getInt32Safe(this String input, Int32 ifFail = 0)
        {
            Int32 output = ifFail;
            if (String.IsNullOrEmpty(input)) return output;
            Int32 multi = 1;
            if (input.Contains("-"))
            {
                multi = -1;
            }

            input = input.Trim();
            Int32.TryParse(_numOnly.Replace(input, "", 1000), out output);
            output = output * multi;
            return output;
        }

        public static Double getDoubleSafe(this String input, Double ifFail = 0)
        {
            Double output = ifFail;
            if (String.IsNullOrEmpty(input)) return output;
            Int32 multi = 1;
            input = input.Trim();
            Double.TryParse(input, out output);
            return output;
        }

        public static String getGByteCountFormated(this long input, long factor = 1073741824, String sufix = "Gb")
        {
            input = input / factor;
            Decimal output = Math.Round(Convert.ToDecimal(input), 2);
            return output.ToString() + sufix;
        }

        /// <summary>
        /// Gets the m byte count formated.
        /// </summary>
        /// <param name="input">The input - number of bytes.</param>
        /// <param name="factor">The factor to divide</param>
        /// <param name="sufix">The sufix for unit</param>
        /// <returns></returns>
        public static String getMByteCountFormated(this long input, long factor = 1048576, String sufix = "Mb")
        {
            input = input / factor;
            Decimal output = Math.Round(Convert.ToDecimal(input), 2);
            return output.ToString() + sufix;
        }

        public static String getKByteCountFormated(this long input, long factor = 1024, String sufix = "Kb")
        {
            input = input / factor;
            Decimal output = Math.Round(Convert.ToDecimal(input), 2);
            return output.ToString() + sufix;
        }

        /// <summary>
        /// Miliseconds into seconds decimal - V3.2: Pretvara milisekunde u sekunde
        /// </summary>
        /// <param name="msec">Milisekunde</param>
        /// <param name="decimals">Na koliko decimala</param>
        /// <returns>Broj sekundi, zaogruzen prema podesavanjima</returns>
        public static Decimal getSeconds(this Double msec, Int32 decimals = 2)
        {
            Decimal output = ((Decimal)((Decimal)msec / (Decimal)1000));
            return Math.Round(output, decimals);
        }

        /// <summary>
        /// V3.2: Pretvara milisekunde u sekundni string
        /// </summary>
        /// <param name="msec">Milisekunde</param>
        /// <param name="sufix">Sufix za mernu jedinicu</param>
        /// <returns>String formatiran na dve decimale</returns>
        /// <param name="decimals">Na koliko decimala</param>
        /// \ingroup_disabled ace_ext_strings_generators
        public static String getTimeSecString(this Double msec, String sufix = "s ", Int32 decimals = 2)
        {
            Decimal output = getSeconds(msec, decimals);
            return output.ToString("F" + decimals.ToString()) + sufix;
        }

        /// <summary>
        /// Gets the time seconds string.
        /// </summary>
        /// <param name="msec">The msec.</param>
        /// <param name="sufix">The sufix.</param>
        /// <param name="decimals">The decimals.</param>
        /// <returns></returns>
        public static String getTimeSecString(this long msec, String sufix = "s ", Int32 decimals = 2)
        {
            Decimal output = getSeconds(msec, decimals);
            return output.ToString("F" + decimals.ToString()) + sufix;
        }

        /// <summary>
        /// Vraca formatiran procenat od zadatih brojeva
        /// </summary>
        /// <param name="part">Velicina dela, npr. 10  - (za whole=20, rezultat = 50.00%)</param>
        /// <param name="whole">Velicina celine, npr. 20  - (za part=10, rezultat = 50.00%)</param>
        /// <param name="decimals">Na koliko decimala zaogruzuje</param>
        /// <param name="suffix">Koji sufix (simbol) dodaje</param>
        /// <returns>Sredjen rezultat u stringu</returns>
        /// \ingroup_disabled ace_ext_strings_generators
        public static String imbGetPercentage(this Int32 part, Int32 whole, Int32 decimals = 2, String suffix = "% ")
        {
            Double output = Convert.ToDouble(((decimal)((decimal)(part + 1) / (decimal)whole)));
            return output.ToString("P" + decimals.ToString());
        }

        /// <summary>
        /// Vraca procenat od zadatih brojeva, zaokruzen na dve decimale
        /// </summary>
        /// <param name="part">Velicina dela, npr. 10  - (za whole=20, rezultat = 50.00%)</param>
        /// <param name="whole">Velicina celine, npr. 20  - (za part=10, rezultat = 50.00%)</param>
        /// <param name="decimals">Na koliko decimala zaogruzuje</param>
        /// <returns>Sredjen rezultat</returns>
        public static Double imbGetPercentageDouble(Int32 part, Int32 whole, Int32 decimals = 2)
        {
            return Math.Round(Convert.ToDouble(((decimal)((decimal)(part + 1) / (decimal)whole) * 100)), decimals);
        }
    }
}