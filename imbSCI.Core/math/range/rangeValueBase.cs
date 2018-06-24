// --------------------------------------------------------------------------------------------------------------------
// <copyright file="rangeValueBase.cs" company="imbVeles" >
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
    using imbSCI.Core.attributes;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.interfaces;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Value limited within range
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="rangeCriteria{T}" />
    public abstract class rangeValueBase<T> : rangeCriteria<T>, IRangeValue, ICloneable where T : IComparable
    {
        public abstract void setValueRange(aceRangeConfig config);

        private T _range;

        /// <summary>
        /// Value total range
        /// </summary>
        public T range
        {
            get { return _range; }
            protected set { _range = value; }
        }

        private T _val;

        /// <summary>
        /// Current value - auto checked
        /// </summary>
        public T val
        {
            get { return _val; }
            set
            {
                _val = checkValue(value);
            }
        }

        //  public abstract T getRange(T __min, T __max);

        public void checkValue()
        {
            _val = checkValue(_val);
        }

        public void setValue(Object input)
        {
            val = (T)input;
        }

        public Object getValue()
        {
            return val;
        }

        /// <summary>
        /// Forces value inside the range if not <see cref="rangeCriteria{T}.isDisabled"/> set <c>True</c>
        /// </summary>
        /// <param name="__val">The value to test against the range</param>
        /// <returns></returns>
        public T checkValue(T __val)
        {
            if (isDisabled) return __val;

            T output = __val;
            Int32 i = 0;
            Int32 il = 100;
            while (isBelowMin(output))
            {
                i++;
                output = changeValue(output, range, "+");
                if (i > il) break;
            }

            i = 0;
            while (isOverOrOnMax(output))
            {
                i++;
                output = changeValue(output, range, "-");
                if (i > il) break;
            }

            return output;
        }

        public abstract T changeValue(T __val, T __delta, String op = "+");

        public object Clone()
        {
            List<Object> settings = new List<object>();
            settings.AddRange(new Object[] { min, max, val, rule });
            Object clv = GetType().getInstance(settings.ToArray());
            return clv;
        }

        private numberRangeModifyRule _rule = numberRangeModifyRule.clipToMax;

        /// <summary>
        ///
        /// </summary>
        public numberRangeModifyRule rule
        {
            get { return _rule; }
            set { _rule = value; }
        }

        protected rangeValueBase()
        {
        }

        protected rangeValueBase(T min, T max, T start, numberRangeModifyRule __rule)
        {
            rule = __rule;
            setCriteria(min, max, false);
            //range = getRange(min, max);
            val = start;
        }
    }
}