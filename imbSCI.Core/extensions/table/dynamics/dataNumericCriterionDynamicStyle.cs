// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataNumericCriterionDynamicStyle.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.table.dynamics
{
    using imbSCI.Core.extensions.table.core;
    using imbSCI.Core.extensions.table.style;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.math.range;
    using System;
    using System.Data;

    public class dataNumericCriterionDynamicStyle<TValueType, TEnum> : tableStyleDynamicRule
    where TValueType : IComparable

    {
        public String columnName { get; set; } = "";

        public TEnum styleKey { get; set; }

        public rangeCriteria<TValueType> criteria { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="dataNumericCriterionDynamicStyle{T, TEnum}"/> class.
        /// </summary>
        /// <param name="_criteria">The criteria.</param>
        /// <param name="key">The key.</param>
        public dataNumericCriterionDynamicStyle(rangeCriteria<TValueType> _criteria, TEnum key, String _columnName)
        {
            criteria = _criteria;
            styleKey = key;
            columnName = _columnName;
        }

        /// <summary>
        /// Evaluates the <see cref="columnName"/> column of the <c>row</c> against <see cref="criteria"/>, if test is positive returns the style associated with <see cref="styleKey"/>
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="table">The table.</param>
        /// <param name="defaultStyle">The default style.</param>
        /// <returns></returns>
        public override tableStyleSetterResponse evaluate(DataRow row, DataTable table, dataTableStyleEntry defaultStyle)
        {
            DataTable rowTable = row.Table;
            if (rowTable == null) return new tableStyleSetterResponse(defaultStyle, this);

            if (!rowTable.Columns.Contains(columnName)) return makeDefaultResponse(false, null, defaultStyle);

            DataColumn dc = rowTable.Columns[columnName];

            Boolean ok = confirmCritarion(row[dc]);

            if (!ok) return makeDefaultResponse(false, null, defaultStyle);

            if (typeof(TValueType).isNumber())
            {
                TValueType val = row[dc].imbConvertValueSafeTyped<TValueType>();

                if (criteria.testCriteria(val)) return new tableStyleSetterResponse(table.GetStyleSet().GetStyle(styleKey), this);
            }

            // if (criteria.testCriteria(row[dc])) return new tableStyleSetterResponse(table.GetStyleSet().GetStyle(styleKey), this);

            return new tableStyleSetterResponse(defaultStyle, this);
        }

        protected override bool confirmCritarion(object value)
        {
            if (value == null) return false;

            TValueType val = value.imbConvertValueSafeTyped<TValueType>();

            return criteria.testCriteria(val);
        }
    }
}