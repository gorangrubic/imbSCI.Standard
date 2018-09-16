// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataRowIndexDynamicStyle.cs" company="imbVeles" >
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
    using System;
    using System.Collections.Generic;
    using System.Data;

    //public class dataColumnNamedPresetDeployerDynamicStyle:tableStyleColumnSetter
    //{
    //}

    /// <summary>
    /// Affects rows on specified index positions
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    /// <seealso cref="imbSCI.Core.extensions.table.dataTableDynamicStyleEntry" />
    public class dataRowIndexDynamicStyle<TEnum> : tableStyleDynamicRule
    {
        public TEnum styleKey { get; set; }

        public List<Int32> indexes { get; set; } = new List<int>();

        /// <summary>
        /// If true - it counts rows from the source table (without column labels)
        /// </summary>
        /// <value>
        ///   <c>true</c> if [index from source table]; otherwise, <c>false</c>.
        /// </value>
        public Boolean indexFromSourceTable { get; set; } = true;

        public void Add(Int32 index)
        {
            indexes.Add(index);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="dataRowIndexDynamicStyle{TEnum}"/> class.
        /// </summary>
        /// <param name="_key">The key.</param>
        /// <param name="indexList">The index list.</param>
        public dataRowIndexDynamicStyle(dataTableStyleEntry style, IEnumerable<Int32> indexList)
        {
            customStyleEntry = style;
            indexes.AddRange(indexList);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="dataRowIndexDynamicStyle{TEnum}"/> class.
        /// </summary>
        /// <param name="_key">The key.</param>
        /// <param name="indexList">The index list.</param>
        public dataRowIndexDynamicStyle(TEnum _key, IEnumerable<Int32> indexList)
        {
            styleKey = _key;
            indexes.AddRange(indexList);
        }

        /// <summary>
        /// Evaluates the specified row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="table">The table.</param>
        /// <param name="defaultStyle">The default style.</param>
        /// <returns></returns>
        public override tableStyleSetterResponse evaluate(DataRow row, DataTable table, dataTableStyleEntry defaultStyle)
        {
            var tableReal = row.Table;

            if (tableReal == null) return new tableStyleSetterResponse(defaultStyle, this);

            Int32 i = tableReal.Rows.IndexOf(row);

            if (indexes.Contains(i)) return new tableStyleSetterResponse(table.GetStyleSet().GetStyle(styleKey), this);

            return new tableStyleSetterResponse(defaultStyle, this);

            if (indexFromSourceTable)
            {
                IDataTableForStatistics statTable = tableReal as IDataTableForStatistics;
                if (statTable != null)
                {
                    i = i - statTable.RowStart;
                }
            }

            //return makeDefaultResponse(confirmCritarion(i), , defaultStyle);
        }

        protected override bool confirmCritarion(object value)
        {
            Int32 i = (Int32)value;
            return indexes.Contains(i);
        }
    }
}