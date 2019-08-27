// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureDecimal.cs" company="imbVeles" >
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
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.math.measureSystem.core;
    using imbSCI.Core.math.measureSystem.enums;
    using imbSCI.Core.math.range;
    using System;

#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'measure{TValue}.Decimal}'
    /// <summary>
    /// base implementation of measure based on <see cref="decimal"/> value
    /// </summary>
    /// <seealso cref="measure{TValue}.Decimal}" />
    public abstract class measureDecimal : measure<decimal>
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'measure{TValue}.Decimal}'
    {
        public measureDecimal(measureRoleEnum role, measureSystemsEnum system, decimal defaultValue, decimal defaultBaseValue, decadeLevel level)
        {
            valueTypeGroup = valueType.getTypeGroup();
            Int32 levelsFromRoot = level.ToInt32From(decadeLevel.none);

            info = measureSystemManager.getMeasureInfo(role, levelsFromRoot, system);
            setDefaultValue(defaultValue, defaultBaseValue);

            //baseValue = 1;
        }

        public measureDecimal(measureRoleEnum role, measureSystemsEnum system, decimal defaultValue, decimal defaultBaseValue = 1, Int32 level = 0) : base(role, system, defaultValue, defaultBaseValue, level)
        {
            //baseValue = 1;
        }

        public override IMeasureBasic calculate(IMeasure second, operation op)
        {
            primValue = this.primValue.calculate(op, second.getValue<Decimal>());
            return this;
        }

        public static measureDecimal operator +(measureDecimal a1, measureDecimal a2)
        {
            a1.primValue += a2.primValue;
            return a1;
        }

        public static measureDecimal operator -(measureDecimal a1, measureDecimal a2)
        {
            a1.primValue -= a2.primValue;
            return a1;
        }

        public static measureDecimal operator +(measureDecimal a1, Decimal a2)
        {
            a1.primValue += a2;
            return a1;
        }

        public static measureDecimal operator -(measureDecimal a1, Decimal a2)
        {
            a1.primValue -= a2;
            return a1;
        }

        public static measureDecimal operator /(measureDecimal a1, measureDecimal a2)
        {
            a1.primValue = a1.primValue / a2.primValue;
            return a1;
        }

        public static measureDecimal operator /(measureDecimal a1, Decimal a2)
        {
            a1.primValue = a1.primValue / a2;
            return a1;
        }

        public static measureDecimal operator /(measureDecimal a1, Object a2)
        {
            a1.primValue = a1.primValue / a2.imbToNumber<Decimal>();
            return a1;
        }

        public static measureDecimal operator *(measureDecimal a1, Object a2)
        {
            a1.primValue = a1.primValue * a2.imbToNumber<Decimal>();
            return a1;
        }

        public static measureDecimal operator *(measureDecimal a1, measureDecimal a2)
        {
            a1.primValue = a1.primValue * a2.primValue;
            return a1;
        }

        public static measureDecimal operator *(measureDecimal a1, Decimal a2)
        {
            a1.primValue = a1.primValue * a2;
            return a1;
        }

        public void convertToUnit(Int32 levels)
        {
            var un = info.system.GetUnit(info.unit, levels);
            convertToUnit(un);
        }

        public override void convertToUnit(measureSystemUnitEntry targetUnit)
        {
            double fd = targetUnit.GetFactorDistance(info.unit); //.GetFactorDistance(targetUnit);
            primValue = primValue * Convert.ToDecimal(fd);
            info.unit = targetUnit;
        }
    }
}