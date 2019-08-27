// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorGradient.cs" company="imbVeles" >
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
using imbSCI.Core.collection;
using imbSCI.Core.math;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.math.range.frequency;
using imbSCI.Data.collection.special;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

//using Accord.Imaging;
using System.Xml.Serialization;

namespace imbSCI.Core.style.color
{

    /// <summary>
    ///
    /// </summary>
    public class ColorGradient
    {
        public static ColorGradient RedBlueAtoBPreset { get { return new ColorGradient("#FFD82323", "#FF224ECA", ColorGradientFunction.AllAToB); } }

        public static ColorGradient BlueGreenAtoBPreset
        {
            get
            {
                return new ColorGradient("#FF2258D8", "#FF7FCA23", ColorGradientFunction.AllAToB);
            }
        }

        public static ColorGradient BlueGrayAtoBPreset
        {
            get
            {
                return new ColorGradient("#FF1040B6", "#FF353535", ColorGradientFunction.AllAToB);
            }
        }

        public static ColorGradient ColorCircleCW
        {
            get
            {
                return new ColorGradient("#FF3b25a7", "#FF72a725", ColorGradientFunction.HueAToB);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorGradient"/> class.
        /// </summary>
        public ColorGradient()
        {
        }

        /// <summary>
        /// Circles full Hue circle around the color
        /// </summary>
        /// <param name="HexA">The hexadecimal a.</param>
        public ColorGradient(String HexA)
        {
            ColorGradientFunction options = ColorGradientFunction.CircleCW | ColorGradientFunction.HueValueAToB;

            HexColorA = ColorWorks.FormatHexColorTo(HexA, ColorHexFormats.ARGB); 
            HexColorB = HexColorA;
            gradient = options;
            Prepare();
        }

        public ColorGradient(Color A, Color B, ColorGradientFunction options)
        {
            HexColorA = A.ColorToHex();
            HexColorB = B.ColorToHex();
            gradient = options;
            Prepare();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorGradient"/> class.
        /// </summary>
        /// <param name="HexA">The hexadecimal a.</param>
        /// <param name="HexB">The hexadecimal b.</param>
        /// <param name="options">The options.</param>
        public ColorGradient(String HexA, String HexB, ColorGradientFunction options)
        {
            HexColorA = ColorWorks.FormatHexColorTo(HexA, ColorHexFormats.ARGB);
            HexColorB = ColorWorks.FormatHexColorTo(HexB, ColorHexFormats.ARGB);
            gradient = options;
            Prepare();
        }

        /// <summary>
        /// Gets the color of the hexadecimal.
        /// </summary>
        /// <param name="ratio">The ratio: from 0 to 1</param>
        /// <param name="withAlpha">if set to <c>true</c> [with alpha].</param>
        /// <returns></returns>
        public String GetHexColor(Double ratio, Boolean withAlpha = true)
        {

            ColorHSVPoint output = GetHSVColor(ratio);

            String hex = output.GetHexColor(withAlpha);
            return hex;
        }

        /// <summary>
        /// Gets a color for given 0 to 1 ratio (i.e. position in range)
        /// </summary>
        /// <param name="ratio">The ratio - position in range from 0 to 1.</param>
        /// <returns></returns>
        public ColorHSVPoint GetHSVColor(Double ratio)
        {
            if (!IsReady) Prepare();

           
            ColorHSVPoint output = BaseColor.Clone();


            if (gradient.HasFlag(ColorGradientFunction.Hue))
            {
                output.H = Convert.ToInt32(RangeH.GetValueForRangePosition(ratio));
            } 

            if (gradient.HasFlag(ColorGradientFunction.Value))
            {
                output.V = Convert.ToSingle(RangeV.GetValueForRangePosition(ratio));
            }

            if (gradient.HasFlag(ColorGradientFunction.Saturation))
            {
                output.S = Convert.ToSingle(RangeS.GetValueForRangePosition(ratio)); // output.S + r.S;
            }

            if (gradient.HasFlag(ColorGradientFunction.Alpha))
            {
                output.A = Convert.ToSingle(RangeA.GetValueForRangePosition(ratio));
            }

            output.DeployValues();
            return output;
        }

        /// <summary>
        /// Gets a color for given 0 to 1 ratio (i.e. position in range)
        /// </summary>
        /// <param name="ratio">The ratio - position in range from 0 to 1.</param>
        /// <returns></returns>
        public Color GetColor(Double ratio)
        {
            
            ColorHSVPoint output = GetHSVColor(ratio);

            return output.GetColor();
        }

        public List<ColorHSVPoint> GetColorHSVSteps(Int32 colorSegments, Boolean withAlpha = true)
        {
            if (!IsReady) Prepare();

            List<ColorHSVPoint> output = new List<ColorHSVPoint>();

            for (int i = 0; i <= colorSegments; i++)
            {
                Double r = i.GetRatio(colorSegments);

                ColorHSVPoint head = GetHSVColor(r);
                output.Add(head);

                
            }

            return output;

        }


        public List<String> GetColorSteps(Int32 colorSegments, Boolean withAlpha = true)
        {
            if (!IsReady) Prepare();

            List<String> output = new List<string>();

            for (int i = 0; i <= colorSegments; i++)
            {
                Double r = i.GetRatio(colorSegments);

                ColorHSVPoint head = GetHSVColor(r);

                var ci_hex = head.GetHexColor();
                if (!output.Contains(ci_hex)) output.Add(ci_hex);
                
            }

            return output;

        }

        /// <summary>
        /// Gets the hexadecimal color dictionary - where Key is color, Double is upper limit for the color segment
        /// </summary>
        /// <param name="colorSegments">The color segments.</param>
        /// <param name="withAlpha">if set to <c>true</c> [with alpha].</param>
        /// <returns></returns>
        public ColorGradientDictionary GetHexColorDictionary(Int32 colorSegments, Boolean withAlpha = true)
        {
            var output = new ColorGradientDictionary();

            if (colorSegments < 1) return output;
            Double c = 1.GetRatio(colorSegments);
            Double ci = c;

            //var r = PointRange.GetRangeMultiplied(c);

            //var head = PointA.Clone();

            if (!IsReady) Prepare();

            for (int i = 0; i < colorSegments; i++)
            {
                Double r = i.GetRatio(colorSegments);

                ColorHSVPoint head = GetHSVColor(r);

                var ci_hex = head.GetHexColor();
                if (!output.ContainsKey(ci_hex)) output.Add(ci_hex, ci);
                ci = ci + c;
            }

            return output;
        }


        public rangeFinder RangeA { get; set; } = new rangeFinder();
        public rangeFinder RangeS { get; set; } = new rangeFinder();
        public rangeFinder RangeV { get; set; } = new rangeFinder();
        public rangeFinder RangeH { get; set; } = new rangeFinder();

        protected void Prepare()
        {
            var pointA = new ColorHSVPoint(HexColorA);
            var pointB = new ColorHSVPoint(HexColorB);

            RangeA.Learn(pointA.A);
            RangeA.Learn(pointB.A);

            RangeS.Learn(pointA.S);
            RangeS.Learn(pointB.S);

            RangeV.Learn(pointA.V);
            RangeV.Learn(pointB.V);

            RangeH.Learn(pointA.H);
            RangeH.Learn(pointB.H);

            BaseColor = pointA;
           
        }

        protected ColorHSVPoint BaseColor { get; set; } = null;

       // protected ColorHSVPoint PointA { get; set; } = null;
       // protected ColorHSVPoint PointB { get; set; } = null;

       // protected ColorHSVPoint PointRange { get; set; } = null;

        [XmlIgnore]
        public Boolean IsReady
        {
            get
            {
                if (!RangeA.IsLearned) return false;
                if (!RangeS.IsLearned) return false;
                if (!RangeV.IsLearned) return false;
                if (!RangeH.IsLearned) return false;
                return true;
            }
        }

        public ColorGradientFunction gradient { get; set; } = ColorGradientFunction.HueSaturationAToB;

        public String HexColorA { get; set; } = "#D82323";

        public String HexColorB { get; set; } = "#224ECA";
    }
}