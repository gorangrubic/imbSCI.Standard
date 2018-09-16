// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureInteger.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.math.measureSystem.core;
    using imbSCI.Core.math.measureSystem.enums;
    using System;

    public abstract class measureInteger : measure<int>
    {
        protected measureInteger() : base()
        {
        }

        protected measureInteger(Enum role, measureSystemsEnum system, int defaultValue, int defaultBaseValue, int levelsFromRoot = 0) : base(role, system, defaultValue, defaultBaseValue, levelsFromRoot)
        {
        }

        //public measureInteger():base()
        public override void convertToUnit(measureSystemUnitEntry targetUnit)
        {
            double fd = info.unit.GetFactorDistance(targetUnit);
            primValue = Convert.ToInt32(Convert.ToDouble(primValue) * fd);
            info.unit = targetUnit;
        }

        public static measureInteger operator +(measureInteger a1, measureInteger a2)
        {
            a1.primValue += a2.primValue;
            return a1;
        }

        public static measureInteger operator -(measureInteger a1, measureInteger a2)
        {
            a1.primValue -= a2.primValue;
            return a1;
        }

        public static measureInteger operator +(measureInteger a1, Int32 a2)
        {
            a1.primValue += a2;
            return a1;
        }

        public static measureInteger operator -(measureInteger a1, Int32 a2)
        {
            a1.primValue -= a2;
            return a1;
        }

        public static measureInteger operator /(measureInteger a1, measureInteger a2)
        {
            a1.primValue = a1.primValue / a2.primValue;
            return a1;
        }

        public static measureInteger operator /(measureInteger a1, Int32 a2)
        {
            a1.primValue = a1.primValue / a2;
            return a1;
        }

        public static measureInteger operator /(measureInteger a1, Object a2)
        {
            a1.primValue = a1.primValue / a2.imbToNumber<Int32>();
            return a1;
        }

        public static measureInteger operator *(measureInteger a1, Object a2)
        {
            a1.primValue = a1.primValue * a2.imbToNumber<Int32>();
            return a1;
        }

        public static measureInteger operator *(measureInteger a1, measureInteger a2)
        {
            a1.primValue = a1.primValue * a2.primValue;
            return a1;
        }

        public static measureInteger operator *(measureInteger a1, Int32 a2)
        {
            a1.primValue = a1.primValue * a2;
            return a1;
        }
    }
}