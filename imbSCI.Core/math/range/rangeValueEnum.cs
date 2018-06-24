// --------------------------------------------------------------------------------------------------------------------
// <copyright file="rangeValueEnum.cs" company="imbVeles" >
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
    using System.Linq;

    /// <summary>
    /// Range value for enumeration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="aceCommonTypes.math.rangeValueBase{System.Int32}" />
    public class rangeValueEnum<T> : rangeValueBase<int>, IRangeValue<int> where T : IComparable, IConvertible
    {
        public override void setValueRange(aceRangeConfig config)
        {
            if (config == null)
            {
                isDisabled = true;
                return;
            }
            isDisabled = false;
            min = Convert.ToInt32(config.min);
            max = Convert.ToInt32(config.max);
            range = max - min;
            rule = config.rule;
            checkValue();
        }

        private List<Object> _values = new List<object>();

        /// <summary>
        ///
        /// </summary>
        ///
        public List<Object> values
        {
            get { return _values; }
            protected set { _values = value; }
        }

        public T getEnum(Int32 vl)
        {
            foreach (T v in values)
            {
                if (Convert.ToInt32(v) == vl)
                {
                    return v;
                }
            }
            return default(T);
        }

        public T getValue()
        {
            return getEnum(val);
        }

        public rangeValueEnum(numberRangeModifyRule __rule, T start) : base()
        {
            Type t = typeof(T);

            if (!t.isEnum())
            {
                throw new ArgumentException("start", "Type must be an Enum");
            }

            var vls = Enum.GetValues(t);
            foreach (Object v in vls)
            {
                values.Add(v);
            }

            var ens = t.GetEnumNames().ToList();

            min = 0;
            max = Enumerable.Count<string>(ens);

            range = Enumerable.Count<string>(ens);
            rule = __rule;

            val = Convert.ToInt32(start);
        }

        //public override T changeValue(T __val, T __delta, string op = "+")
        //{
        //    Int32 vl = Convert.ToInt32(__val);

        //    vl =  vl.rangeChange(Convert.ToInt32(__delta), Convert.ToInt32(max), rule, op);
        //    return getEnum(vl);
        //}

        public override int changeValue(int __val, int __delta, string op = "+")
        {
            return __val.rangeChange(__delta, max, rule, op);
        }
    }
}