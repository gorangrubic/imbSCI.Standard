// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorHSVPoint.cs" company="imbVeles" >
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
using System;
using System.Drawing;

//using Accord.Imaging;

namespace imbSCI.Core.style.color
{
    /// <summary>
    /// HSV
    /// </summary>
    public class ColorHSVPoint
    {
        public ColorHSVPoint()
        {
        }

        public ColorHSVPoint Clone()
        {
            var output = new ColorHSVPoint();
            output.H = H;
            output.V = V;
            output.S = S;
            output.A = A;
            return output;
        }

        /// <summary>
        /// Gets the range version --- values are allowed to be negative
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public ColorHSVPoint GetRange(ColorHSVPoint b)
        {
            var output = new ColorHSVPoint();
            output.H = H - b.H;

            output.V = V - b.V;
            output.S = S - b.S;
            output.A = A - b.A;
            return output;
        }

        public void DeployValues()
        {
            var output = this;
            while (H < 0)
            {
                H += 360;
            }
            H = H % 360;

            S = Math.Min(S, 1);
            V = Math.Min(V, 1);
            A = Math.Min(A, 1);
            S = Math.Max(S, 0);
            V = Math.Max(V, 0);
            A = Math.Max(A, 0);
        }

        public static ColorHSVPoint operator +(ColorHSVPoint a, ColorHSVPoint b)
        {
            ColorHSVPoint output = new ColorHSVPoint();
            output.H = (a.H + b.H);
            output.S += a.S;
            output.V += a.V;
            output.A += a.A;
            output.DeployValues();
            return output;
        }

        public static ColorHSVPoint operator *(ColorHSVPoint a, Double b)
        {
            ColorHSVPoint output = new ColorHSVPoint();
            output.H = Convert.ToInt32(a.H * b); // % 360;

            output.S = a.S * (float)b;
            output.V = a.V * (float)b;
            output.A = a.A * (float)b;

            output.DeployValues();

            return output;
        }

        /// <summary>
        /// Performs multiplication without <see cref="DeployValues"/> call
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public ColorHSVPoint GetRangeMultiplied(Double b)
        {
            ColorHSVPoint output = Clone();

            output.H = Convert.ToInt32(H * b); // % 360;

            output.S = S * (float)b;
            output.V = V * (float)b;
            output.A = A * (float)b;

            return output;
        }

        public static ColorHSVPoint operator -(ColorHSVPoint a, ColorHSVPoint b)
        {
            ColorHSVPoint output = new ColorHSVPoint();
            output.H = a.H - b.H;
            output.S -= a.S;
            output.V -= a.V;
            output.A -= a.A;
            output.DeployValues();
            return output;
        }

        public ColorHSVPoint(String hexColor)
        {
            SetHexColor(hexColor);
        }

        public String GetHexColor(Boolean withAlpha = true)
        {
            DeployValues();
            Color c = GetColor(); //.toMediaColor();

            //c.A = Convert.ToByte(A * Byte.MaxValue);
            return ColorWorks.ColorToHex(c); // aceColorConverts.toHexColor(c, !withAlpha);
        }

        public Color GetColor()
        {
            var hsl = new HSLColor(H / 360, S, V);

            return hsl;
        }

        public void SetHexColor(String hex)
        {
            SetColor(ColorWorks.GetColor(hex)); //  aceColorConverts.getColorFromHex(hex));
        }

        public void SetColor(Color color)
        {
            HSLColor hsl = new HSLColor(color);

            //var hsl = HSL.FromRGB(new RGB(color));
            H = Convert.ToInt32(hsl.hue * 360);
            S = Convert.ToSingle(hsl.saturation);
            V = Convert.ToSingle(hsl.luminosity);
            A = color.A;
        }

        public Int32 H { get; set; }
        public float S { get; set; }
        public float V { get; set; }
        public float A { get; set; } = 1;
    }
}