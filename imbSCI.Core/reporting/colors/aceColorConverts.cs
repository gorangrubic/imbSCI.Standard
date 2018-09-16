// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceColorConverts.cs" company="imbVeles" >
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

////#if NETFULL

////    using mColor = System.Windows.Media.Color;
////    using System.Windows.Media;
////    using System.Media;
////    using System.Drawing;

////#elif NET40
////       using mColor = System.Windows.Media.Color;
////    using System.Windows.Media;
////    using System.Media;
////    using System.Drawing;

////#else
////    using mColor = Xamarin.Forms.Color;
////    using Xamarin.Forms;
////    using System.Drawing;

////    //using System.Windows.Media;
////    //using ColorConverter = System.Drawing.ColorConverter;
////#endif

//    using Color = System.Drawing.Color;
//    //

//    using imbSCI.Core.math.range;
//    using Accord.Imaging;

//    using imbSCI.Core.extensions.data;
//    using Accord.Imaging;

//    //using System.Drawing;

//    /// <summary>
//    /// Tools for internal color manipulation and conversions
//    /// </summary>
//    public static class aceColorConverts
//    {
//        /// <summary>
//        /// Makes the path for color variation definition cache file
//        /// </summary>
//        /// <param name="baseColorHex">The base color hexadecimal.</param>
//        /// <param name="valueDelta">The value delta.</param>
//        /// <param name="saturationDelta">The saturation delta.</param>
//        /// <param name="hueDelta">The hue delta.</param>
//        /// <param name="isForeColor">if set to <c>true</c> [is fore color].</param>
//        /// <returns></returns>
//        internal static String makePath(String baseColorHex, float valueDelta = 0, float saturationDelta = 0,
//                                       Int32 hueDelta = 0, Boolean isForeColor = false)
//        {
//            String path = "";
//            if ((valueDelta == 0) && (saturationDelta == 0) && (hueDelta == 0))
//            {
//                path = baseColorHex;
//            }
//            else
//            {
//                path = baseColorHex + "_vd" + valueDelta.ToString() + "_sd" + saturationDelta.ToString() + "_hd" +
//                       hueDelta;
//            }

//            if (isForeColor) path = path + "_foreColor";

//            return path;
//        }

//#if NETFULL
//       // private static ColorConverter mediaConverter = new ColorConverter();
//#else

//        //private static System.Drawing. ColorConverter mediaConverter = new ColorConverter();
//#endif

//        //private static mColor _noneColor;

//        //#region Providna crna boja noneColor

//        ///// <summary>
//        ///// Providna crna boja
//        ///// </summary>
//        //public static mColor noneColor
//        //{
//        //    get
//        //    {
//        //        if (_noneColor == null) _noneColor = mColor.FromRgba(0, 0, 0, 0);

//        //        return _noneColor;
//        //    }
//        //    set { _noneColor = value; }
//        //}

//        #endregion

//        /// <summary>
//        /// Menja parametre HSL boje - u skladu sa podešavanjima
//        /// </summary>
//        /// <param name="source">Izvorni HSL</param>
//        /// <param name="brightness">Promena u Value</param>
//        /// <param name="saturation">Promena u saturaciji</param>
//        /// <param name="hue">Promena u boji</param>
//        /// <param name="rule">Pravilo promene</param>
//        /// <returns>Promenjeni HSL</returns>
//        public static HSL modifyHSL(this HSL source, float brightness, float saturation, Int32 hue, numberRangeModifyRule rule)
//        {
//            source.Luminance = numberRange.rangeChange(source.Luminance, brightness, 1, rule, "+");
//            source.Saturation = numberRange.rangeChange(source.Saturation, saturation, 1, rule, "+");
//            source.Hue = numberRange.rangeChange((int)source.Hue, hue, 359, rule, "+");
//            return source;
//        }

//        /// <summary>
//        /// Vraca svetliju varijantu boje
//        /// </summary>
//        /// <param name="brightness"></param>
//        /// <param name="sourceColor"></param>
//        /// <returns></returns>
//        public static Color getBrighter(this float brightness, Color sourceColor, float saturation = 0, Int32 hue = 0)
//        {
//            RGB tmpRgb = new RGB(sourceColor);
//            HSL tmpBg = HSL.FromRGB(tmpRgb);
//            modifyHSL(tmpBg, brightness, saturation, hue, numberRangeModifyRule.clipToMax);

//            return tmpBg.ToRGB().Color;
//        }

//        /// <summary>
//        /// Returns
//        /// </summary>
//        /// <param name="brightness"></param>
//        /// <param name="sourceColor"></param>
//        /// <param name="saturation"></param>
//        /// <param name="hue"></param>
//        /// <returns></returns>
//        public static aceColorEntry getBrighterEntry(this float brightness, mColor sourceColor, float saturation = 0, Int32 hue = 0)
//        {
//            RGB tmpRgb = new RGB((byte)sourceColor.R, (byte)sourceColor.G, (byte)sourceColor.B);//  //(System.Drawing.Color.FromArgb(sourceColor.A, sourceColor.B, sourceColor.R, sourceColor.G));
//            HSL tmpBg = HSL.FromRGB(tmpRgb);
//            modifyHSL(tmpBg, brightness, saturation, hue, numberRangeModifyRule.clipToMax);

//            mColor output = new mColor();
//            tmpRgb = tmpBg.ToRGB();

//            output = new mColor(tmpRgb.Red, tmpRgb.Green, tmpRgb.Blue, tmpRgb.Alpha);

//            aceColorEntry outputEntry = new aceColorEntry();
//            outputEntry.color = output;

//            return outputEntry;
//        }

//        public static Color getVariation(this Color sourceColor, float bright = 0, float saturation = 0, Int32 hue = 0)
//        {
//            return bright.getBrighter(sourceColor, saturation, hue);
//        }

//        /// <summary>
//        /// Vraća svetliju varijantu Media.Color boje
//        /// </summary>
//        /// <param name="brightness"></param>
//        /// <param name="sourceColor"></param>
//        /// <returns></returns>
//        public static mColor getBrighter(this float brightness, mColor sourceColor, float saturation = 0, Int32 hue = 0)
//        {
//            RGB tmpRgb = new RGB((byte)sourceColor.R, (byte)sourceColor.G, (byte)sourceColor.B);
//            //  //(System.Drawing.Color.FromArgb(sourceColor.A, sourceColor.B, sourceColor.R, sourceColor.G));
//            HSL tmpBg = HSL.FromRGB(tmpRgb);
//            modifyHSL(tmpBg, brightness, saturation, hue, numberRangeModifyRule.clipToMax);

//            mColor output = new mColor();
//            tmpRgb = tmpBg.ToRGB();

//            output = new mColor(tmpRgb.Red, tmpRgb.Green, tmpRgb.Blue, tmpRgb.Alpha);

//            return output;
//        }

//        ///// <summary>
//        ///// Otvara Color dijalog i vraća odabranu boju
//        ///// </summary>
//        ///// <param name="sourceColor">Boja koja je odabrana u startu</param>
//        ///// <returns></returns>
//        //public static Color showColorDialog(Color sourceColor)
//        //{
//        //    return toMediaColor(showColorDialog(this.toDrawingColor(sourceColor)));
//        //}

//        /////// <summary>
//        /////// Otvara Color dijalog i vraća odabranu boju
//        /////// </summary>
//        /////// <param name="sourceColor">Boja koja je odabrana u startu</param>
//        /////// <returns></returns>
//        ////public static System.Drawing.Color showColorDialog(System.Drawing.Color sourceColor)
//        ////{
//        ////    ColorDialog colorSelect = new ColorDialog();
//        ////    System.Drawing.Color output = sourceColor;

//        ////    colorSelect.Color = sourceColor;
//        ////    colorSelect.AnyColor = true;
//        ////    colorSelect.AllowFullOpen = true;
//        ////    colorSelect.FullOpen = true;

//        ////    DialogResult res = colorSelect.ShowDialog();

//        ////    switch (res)
//        ////    {
//        ////        case DialogResult.Cancel:
//        ////            break;
//        ////        default:
//        ////            ColorConverter tmp = new ColorConverter();
//        ////            output = colorSelect.Color;
//        ////            break;
//        ////    }
//        ////    return output;
//        ////}

//        /// <summary>
//        /// Generiše HEX boju za html format
//        /// </summary>
//        /// <param name="color">Windows media boja</param>
//        /// <returns></returns>
//        public static String toHexColor(this mColor color, Boolean withoutAlpha = false)
//        {
//            String output = color.ToString(); ///mediaConverter.ConvertToString(color);
//            if (withoutAlpha) output = output.Substring(0, 7);
//            return output;
//        }

//        public static String toHexColor(this Color color, Boolean withoutAlpha = false)
//        {
//            //  System.Drawing.ColorConverter cc = new System.Drawing.ColorConverter();

//            // mColor m = toMediaColor(color);

//            String output = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2"); //mColor.From //mediaConverter.ConvertToString(m);
//            if (withoutAlpha) output = output.Substring(0, 7);
//            return output;
//        }

//        /// <summary>
//        /// Vraća Media.Color objekat dobijen HEX oznakom
//        /// </summary>
//        /// <param name="color">Windows media boja</param>
//        /// <returns></returns>
//        public static mColor fromHexColor(this String color)
//        {
//            String st = color;
//            if (st.isNullOrEmpty())
//            {
//                st = "#FF333333";
//            }
//            if (color.Length == 7)
//            {
//                st = st.Trim('#');
//                st = "#FF" + st;
//            }
//            return mColor.FromHex(st);
//            //#if NETSTANDARD

//            //#endif
//            //#if NETFULL
//            //            return (mColor)ColorConverter.ConvertFromString(st);
//            //#endif
//        }

//        public static Color getColorFromHex(this String color)
//        {
//            mColor c = color.fromHexColor();
//            return toDrawingColor(c);
//        }

//        /// <summary>
//        /// Konvertuje boju iz jednog sistema u drugi
//        /// </summary>
//        /// <param name="color"></param>
//        /// <returns></returns>
//        public static Color toDrawingColor(this mColor color)
//        {
//            return Color.FromArgb((int)color.A, (int)color.R, (int)color.G, (int)color.B);
//        }

//        /// <summary>
//        /// Konvertuje boju iz jednog sistema u drugi
//        /// </summary>
//        /// <param name="color"></param>
//        /// <returns></returns>
//        public static mColor toMediaColor(this Color color)
//        {
//#if NETSTANDARD

//            return mColor.FromRgba(color.R, color.G, color.B, color.A);
//#else

//            return mColor.FromArgb(color.A, color.R, color.G, color.B);
//#endif
//        }

//#if NETFULL
//        /// <summary>
//        /// Vraća preporučenu boju za ispis slova na osnovu prosleđene pozadine
//        /// </summary>
//        /// <param name="sourceColor">Boja pozadine</param>
//        /// <param name="useBlackWhite">Da li da vraća ekstreme (potpuni konstrast) ili dovoljno uočljiv konstrast</param>
//        /// <returns>Boja za slova</returns>
//        public static mColor getSafeForeground(this mColor sourceColor, Boolean useBlackWhite = true)
//        {
//            return toMediaColor(getSafeForeground(toDrawingColor(sourceColor), useBlackWhite));
//        }

//#endif

//        /// <summary>
//        /// Vraća preporučenu boju za ispis slova na osnovu prosleđene pozadine
//        /// </summary>
//        /// <param name="sourceColor">Boja pozadine</param>
//        /// <param name="useBlackWhite">Da li da vraća ekstreme (potpuni konstrast) ili dovoljno uočljiv konstrast</param>
//        /// <returns>Boja za slova</returns>
//        public static Color getSafeForeground(this Color sourceColor,
//            Boolean useBlackWhite = true)
//        {
//            RGB tmpRgb = new RGB(sourceColor.R, sourceColor.G, sourceColor.B);
//            HSL tmpBg = HSL.FromRGB(tmpRgb);

//            if (useBlackWhite)
//            {
//                tmpBg.Saturation = 0;
//                if (tmpBg.Luminance > 0.5)
//                {
//                    tmpBg.Luminance = 0;
//                }
//                else
//                {
//                    tmpBg.Luminance = 1;
//                }
//            }
//            else
//            {
//                tmpBg.Luminance = 1 - tmpBg.Luminance;
//            }

//            tmpRgb = tmpBg.ToRGB();
//            return tmpRgb.Color;
//        }
//    }
//}