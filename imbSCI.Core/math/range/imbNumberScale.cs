// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbNumberScale.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.range
{
    #region imbVeles using

    using System;
    using System.Collections.Generic;

    #endregion imbVeles using

    /// <summary>
    /// Matematički alat koji radi sa min-max skalom
    /// konvertuje apsolutni i relativni (ratio) broj
    /// </summary>
    public class imbNumberScale
    {
        /// <summary>
        /// Maksimalna vrednost u skali
        /// </summary>
        public Double maxValue = 100;

        /// <summary>
        /// Minimalna vrednost u skali
        /// </summary>
        public Double minValue = 0;

        public imbNumberScale()
        {
        }

        public imbNumberScale(numberRangePresetEnum preset)
        {
        }

        /// <summary>
        /// Applies the preset. See: <see cref="numberRangePresetEnum"/>
        /// </summary>
        /// <param name="preset">The preset.</param>
        public void applyPreset(numberRangePresetEnum preset)
        {
            switch (preset)
            {
                case numberRangePresetEnum.balancedFullOne:
                    minValue = 0;
                    maxValue = 1;
                    break;

                case numberRangePresetEnum.balancedHalfOne:
                    minValue = -0.5;
                    maxValue = 0.5;
                    break;

                case numberRangePresetEnum.zeroToOne:
                    minValue = 0;
                    maxValue = 100;
                    break;
            }
        }

        /// <summary>
        /// Konstruktor koji postavlja min i max
        /// </summary>
        /// <param name="_min"></param>
        /// <param name="_max"></param>
        public imbNumberScale(Double _min, Double _max)
        {
            minValue = Math.Min(_min, _max);
            maxValue = Math.Max(_min, _max);
        }

        /// <summary>
        /// Konstruktor koji postavlja min i max ali iz Liste
        /// pri čemu je 0 član min a 1 član max
        /// </summary>
        /// <param name="input"></param>
        public imbNumberScale(List<Double> input)
        {
            if (input.Count > 1)
            {
                minValue = input[0];
                maxValue = input[1];
            }
        }

        /// <summary>
        /// Vraća apsolutni broj za zadati racio
        /// koristeći postavljeni min-max raspon
        /// </summary>
        /// <param name="ratio">koeficijent na osnovu kojeg se dobija absolutni broj</param>
        /// <param name="normalizeRatio">If <c>true</c>, the <c>ratio</c> input is normalized: ratio = ratio % 1, and negative values are interpreted as inverse position (i.e. from right to left)</param>
        /// <returns></returns>
        public Double getAbsoluteValue(Double ratio, Boolean normalizeRatio = false)
        {
            if (normalizeRatio)
            {
                ratio = ratio % 1;
                if (ratio < 0) ratio = 1 - ratio;
            }

            Double range = maxValue - minValue;
            Double output = minValue + (ratio * range);
            return output;
        }

        /// <summary>
        /// Vraća koeficijent na osnovu ranije definisanog raspona
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Double getRatioValue(Double input)
        {
            Double over = input - minValue;
            Double ratio = over / maxValue;
            return ratio;
        }
    }
}