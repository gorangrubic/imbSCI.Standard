// --------------------------------------------------------------------------------------------------------------------
// <copyright file="decadeSystemExtensions.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.measureSystem
{
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.math.measureSystem.enums;
    using System;

    public static class decadeSystemExtensions
    {
        public static string toLetter(this decadeLevel level)
        {
            switch (level)
            {
                case decadeLevel.tera:
                    return "T";
                    break;

                case decadeLevel.giga:
                    return "G";
                    break;

                case decadeLevel.mega:
                    return "M";
                    break;

                case decadeLevel.kilo:
                    return "k";
                    break;

                case decadeLevel.hecto:
                    return "H";
                    break;

                case decadeLevel.none:
                    return "";
                    break;

                case decadeLevel.deci:
                    return "d";
                    break;

                case decadeLevel.centi:
                    return "c";
                    break;

                case decadeLevel.mili:
                    return "m";
                    break;

                case decadeLevel.micro:
                    return "µ";
                    break;

                case decadeLevel.nano:
                    return "n";
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string toPrefix(this decadeLevel level)
        {
            switch (level)
            {
                default:
                    return level.ToString();
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets the factor.
        /// </summary>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static Double toFactor(this decadeLevel to)
        {
            switch (to)
            {
                case decadeLevel.second:
                    return 1 / 360;
                    break;

                case decadeLevel.none:
                    return 1;
                    break;

                case decadeLevel.minute:
                    return 1 / 60;
                    break;

                case decadeLevel.oneOf12:
                    return 12;
                    break;

                case decadeLevel.oneOf24:
                    return 24;

                    break;

                case decadeLevel.oneOf365:
                    return 365;

                    break;

                default:

                    Int32 exp = to.ToInt32();
                    return Math.Pow(10, exp);
                    break;
            }
        }
    }
}