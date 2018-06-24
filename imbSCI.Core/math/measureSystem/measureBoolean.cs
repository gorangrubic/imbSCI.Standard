// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureBoolean.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.math.measureSystem.core;
    using imbSCI.Core.math.measureSystem.enums;
    using imbSCI.Core.math.range;
    using System;

    /// <summary>
    /// Measure with Boolean value
    /// </summary>
    /// <seealso cref="measure{TValue}.Boolean}" />
    /// <seealso cref="System.ICloneable" />
    public class measureBoolean : measure<bool>, ICloneable
    {
        public measureBoolean(measureBooleanRoles roleEnum, measureBooleanPreset unitEnum, Boolean defaultValue) : base()
        {
            valueTypeGroup = valueType.getTypeGroup();

            info = measureSystemManager.getMeasureInfo(roleEnum, unitEnum, measureSystemsEnum.booleans);
            setDefaultValue(defaultValue, true);

            //strTrue = info.unit.checkValueMap(true).toStringSafe();
            //strFalse = info.unit.checkValueMap(false).toStringSafe();
        }

        protected measureBoolean() : base()
        {
        }

        public measureBoolean(String trueString, String falseString, Boolean defaultValue) : base()
        {
            valueTypeGroup = valueType.getTypeGroup();
            //strTrue = trueString;
            //strFalse = falseString;

            setCustomUnitEntry();

            info.unit.setValueMap(true, trueString);
            info.unit.setValueMap(false, falseString);
            //baseValue = 1;
        }

        public static measureBoolean operator !(measureBoolean a1)
        {
            measureBoolean a2 = a1.Clone();
            a2.primValue = !a2.primValue;
            return a2;
        }

        public static measureBoolean operator &(measureBoolean a1, measureBoolean a2)
        {
            a1.primValue = a1.primValue && a2.primValue;

            return a1;
        }

        public static measureBoolean operator &(measureBoolean a1, Boolean a2)
        {
            a1.primValue = a1.primValue && a2;

            return a1;
        }

        public static measureBoolean operator |(measureBoolean a1, Boolean a2)
        {
            a1.primValue = a1.primValue || a2;

            return a1;
        }

        public static implicit operator Boolean(measureBoolean x)
        {
            return x.primValue;
        }

        public static implicit operator Int32(measureBoolean x)
        {
            if (x.primValue) return 1;
            return 0;
        }

        public static Boolean operator ==(measureBoolean x, measureBoolean y)
        {
            return x.primValue == y.primValue;
        }

        public static Boolean operator !=(measureBoolean x, measureBoolean y)
        {
            return x.primValue != y.primValue;
        }

        public measureBoolean Clone()
        {
            var tmp = new measureBoolean();
            tmp.primValue = primValue;
            tmp.info = info;
            return tmp;
        }

        public override void convertToUnit(measureSystemUnitEntry targetUnit)
        {
            info.unit = targetUnit;
        }

        object ICloneable.Clone() => this.Clone() as object;

        /// <summary>
        /// Calculates the specified second.
        /// </summary>
        /// <param name="second">The second.</param>
        /// <param name="op">The op.</param>
        /// <returns></returns>
        public override IMeasureBasic calculate(IMeasure second, operation op)
        {
            Boolean a = primValue;
            primValue = a.calculate(op, second.getValue<Boolean>());

            return this;
            //Int32 i = primValue.imbConvertValueSafe<Int32>();

            //.calculate(op, second.getValue<Int32>());
        }
    }
}