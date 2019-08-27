// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorWorks.cs" company="imbVeles" >
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

//using Accord.Imaging;
using imbSCI.Core.extensions.data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.Core.style.color
{
    /// <summary>
    /// Dictionary with color enum index
    /// </summary>
    public static class ColorWorks
    {
        public const String ColorRed = "#FF0000";
        public const String ColorDarkRed = "#DD2200";
        public const String ColorGreen = "#00FF00";
        public const String ColorBlue = "#0000FF";
        public const String ColorOrange = "#FF9900";
        public const String ColorViolet = "#7D00DD";
        public const String ColorGray = "#666666";
        public const String ColorLightGray = "#EEEEEE";
        public const String ColorDarkGray = "#333333";

        public const String ColorCoolCyan = "#35bbb0";
        public const String ColorCoolGreen = "#99ad27";
        public const String ColorCoolBlue = "#3cb5d0";
        public const String ColorCoolPink = "#ac78cb";
        public const String ColorCoolYellow = "#cca71a";

        private static List<Color> AllColors { get; set; } = new List<Color>();

        private static Object HumanTextualNameLock = new Object();

        private static Dictionary<String, Color> _HumanTextualNameDictionary;

        /// <summary>
        /// Auto-initiated dictionary with all Color names vs color enumeration
        /// </summary>
        public static Dictionary<String, Color> HumanTextualNameDictionary
        {
            get
            {
                if (_HumanTextualNameDictionary == null)
                {
                    lock (HumanTextualNameLock)
                    {
                        if (_HumanTextualNameDictionary == null)
                        {
                            _HumanTextualNameDictionary = new Dictionary<String, Color>();

                            PropertyInfo[] knownColors = typeof(Color).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

                            foreach (PropertyInfo pi in knownColors)
                            {
                                if (pi.PropertyType == typeof(Color))
                                {
                                    Color col = (Color)pi.GetValue(null, null);
                                    AllColors.Add(col);
                                    _HumanTextualNameDictionary.Add(pi.Name, col);
                                }
                            }
                        }
                    }
                }
                return _HumanTextualNameDictionary;
            }
        }

        /// <summary>
        /// Slices the specified hexadecimal.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <returns></returns>
        private static List<String> SliceString(this String hex)
        {
            hex = hex.Replace("#", string.Empty);
            Int32 ts = 1;
            List<String> output = new List<string>();

            if (hex.Length < 5)
            {
            }
            else
            {
                ts = 2;
            }

            while (hex.Length > 0)
            {
                String s = hex.Substring(0, Math.Min(ts, hex.Length));
                hex = hex.Substring(s.Length);
                output.Add(s);
            }

            return output;
        }

        /// <summary>
        /// Regex select HexColorString : ([0-9A-F]{6})
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isHexColorString = new Regex(@"([0-9A-F]{6})", RegexOptions.Compiled);

        /// <summary>
        /// Test if input matches ([0-9A-F]{6})
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isHexColorString</returns>
        public static Boolean isHexColorString(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isHexColorString.IsMatch(input);
        }

        /// <summary>
        /// Regex select SelectColorInText : \[(\w*)\]
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isSelectColorInText = new Regex(@"\[(\w*)\]", RegexOptions.Compiled);

        /// <summary>
        /// Test if input matches \[(\w*)\]
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isSelectColorInText</returns>
        public static Boolean isSelectColorInText(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isSelectColorInText.IsMatch(input);
        }

        /// <summary>
        /// Regex select MatchParameterValuePair : ([\w]{1})=([\d]*)
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isMatchParameterValuePair = new Regex(@"([\w]{1})=([\d]*)", RegexOptions.Compiled);

        /// <summary>
        /// Test if input matches ([\w]{1})=([\d]*)
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isMatchParameterValuePair</returns>
        public static Boolean isMatchParameterValuePair(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isMatchParameterValuePair.IsMatch(input);
        }

        /// <summary>
        /// Regex select ExtractParameters : \[([\d\w\=\,\s]*)\]
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isExtractParameters = new Regex(@"\[([\d\w\=\,\s]*)\]", RegexOptions.Compiled);

        /// <summary>
        /// Test if input matches \[([\d\w\=\,\s]*)\]
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isExtractParameters</returns>
        public static Boolean isExtractParameters(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isExtractParameters.IsMatch(input);
        }

        /// <summary>
        /// The default color - color to be returned when no color recognized by <see cref="GetColor(string)"/> or <see cref="GetColorSafe(object)"/>
        /// </summary>
        public static Color defaultColor = Color.White;

        /// <summary>
        /// Gets color from unknown object. Supports Color and String, otherwise returns <see cref="defaultColor"/>
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static Color GetColorSafe(Object input)
        {
            if (input == null) return defaultColor;
            if (input is Color) return (Color)input;
            if (input is String inputString)
            {
                if (inputString == "") return defaultColor;
                return GetColor((String)input);
            }
            return defaultColor;
        }

        /// <summary>
        /// Converts HEX code, human name for color or ARGB definition (eg. Color [A=0, R=153, G=153, B=153]), into <see cref="Color"/>
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <returns></returns>
        public static Color GetColor(string hex)
        {
            Color c = defaultColor;
            if (hex.isNullOrEmpty()) return defaultColor;
            byte a = 0, r = 0, g = 0, b = 0;
            //Color[A = 0, R = 153, G = 153, B = 153]
            hex = hex.ToUpper();

            if (hex.Contains("["))
            {
                Boolean parOk = false;
                foreach (Match mch in _select_isMatchParameterValuePair.Matches(hex))
                {
                    if (mch.Success)
                    {
                        if (mch.Groups.Count == 2)
                        {
                            String par = mch.Groups[0].Value;
                            Byte val = Convert.ToByte(mch.Groups[1].Value);
                            switch (par)
                            {
                                case "A":
                                    a = val;
                                    break;

                                case "R":
                                    r = val;
                                    parOk = true;
                                    break;

                                case "G":
                                    g = val;
                                    parOk = true;
                                    break;

                                case "B":
                                    b = val;
                                    parOk = true;
                                    break;
                            }
                        }
                    }
                }

                if (parOk)
                {
                    c = Color.FromArgb(a, r, g, b);
                    return c;
                }
            }

            if (!_select_isHexColorString.Match(hex).Success)
            {
                String test = hex;
                var mch = _select_isSelectColorInText.Match(hex);
                if (mch.Success)
                {
                    hex = mch.Value;
                }

                c = Color.FromName(hex);
                if (c != Color.Empty) return c;

                if (HumanTextualNameDictionary.ContainsKey(hex))
                {
                    return HumanTextualNameDictionary[hex];
                }

                return c;
            }

            hex = hex.Replace("#", string.Empty);

            List<byte> vals = new List<byte>();

            var hexList = hex.SliceString();

            Int32 size = -1;
            String hex_sc = "";
            try
            {
                foreach (String hex_s in hexList)
                {
                    hex_sc = hex_s;
                    if (size == -1) size = hex_s.Length;
                    vals.Add((byte)(Convert.ToUInt32(hex_s, 8 * size)));
                }
            }
            catch (Exception ex)
            {
                String msg = "ColorWorks.GetColor(\"" + hex + "\") failed at [" + vals.Count() + "]th segment [\"" + hex_sc + "\"], size[" + size + "]";

                ArgumentOutOfRangeException ax = new ArgumentOutOfRangeException(msg, ex);
                throw ax;
            }

          

            if (vals.Count() > 3)
            {
                a = vals[0];
                r = vals[1];
                g = vals[2];
                b = vals[3];
            }
            else if (vals.Count() == 3)
            {
                a = 255;
                //a = vals[0];
                r = vals[0];
                g = vals[1];
                b = vals[2];
            }
            else
            {
            }

            c = Color.FromArgb(a, r, g, b);

            return c;
        }

        public static String FormatHexColorTo(this String hexCode, ColorHexFormats format = ColorHexFormats.ARGB)
        {
            Color c = GetColor(hexCode);
            return c.ColorToHex(format);
        }

        /// <summary>
        /// Gets the color version with alpha.
        /// </summary>
        /// <param name="BaseColor">Color of the base.</param>
        /// <param name="alpha">Alpha value, from 0 to 255</param>
        /// <returns></returns>
        public static Color GetColorVersionWithAlpha(this Color BaseColor, Int32 alpha)
        {
            if (alpha > 256) alpha = alpha % 255;
           // Int32 Alpha = Convert.ToInt32(alpha * (Double)255);
            return Color.FromArgb(alpha, BaseColor);
        }

        /// <summary>
        /// Gets the color version with alpha.
        /// </summary>
        /// <param name="BaseColor">Color of the base.</param>
        /// <param name="alpha">Alpha value, from 0 to 1</param>
        /// <returns></returns>
        public static Color GetColorVersionWithAlpha(this Color BaseColor, Double alpha)
        {
            if (alpha > 1) alpha = alpha % 1;
            Int32 Alpha = Convert.ToInt32(alpha * (Double)255);
            return Color.FromArgb(Alpha, BaseColor);
        }


        /// <summary>
        /// Converts color to hex code
        /// </summary>
        /// <param name="actColor">Color of the act.</param>
        /// <param name="format">The format.</param>
        /// <param name="includeSharpPrefix">if set to <c>true</c> it will include # prefix.</param>
        /// <returns></returns>
        public static String ColorToHex(this Color actColor, ColorHexFormats format = ColorHexFormats.ARGB, Boolean includeSharpPrefix = true)
        {
            StringBuilder sb = new StringBuilder();

            if (includeSharpPrefix)
            {
                sb.Append("#");
            }

            Char[] format_chars = format.ToString().ToCharArray();
            foreach (char ch in format_chars)
            {
                switch (ch)
                {
                    case 'A':
                        sb.Append(actColor.A.ToString("X2")); // IntToHex(actColor.R, 2));
                        break;
                    case 'R':
                        sb.Append(actColor.R.ToString("X2"));
                        break;
                    case 'G':
                        sb.Append(actColor.G.ToString("X2"));
                        break;
                    case 'B':
                        sb.Append(actColor.B.ToString("X2"));
                        break;
                    default:
                        break;
                }
                
            }

            return sb.ToString(); //"#" +  + IntToHex(actColor.G, 2) + IntToHex(actColor.B, 2);
        }
    }

    public enum ColorHexFormats
    {
        ARGB,
        RGBA,
        RGB
    }
}