// --------------------------------------------------------------------------------------------------------------------
// <copyright file="tableStyleRowSetter.cs" company="imbVeles" >
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
using System.Collections.Generic;

namespace imbSCI.Core.extensions.table.core
{
    using imbSCI.Core.extensions.table.dynamics;
    using imbSCI.Core.extensions.table.style;
    using System.Data;
    using System.Xml.Serialization;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbSCI.Core.extensions.table.core.tableStyleSetterBase" />
    public class tableStyleRowSetter : tableStyleSetterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="tableStyleRowSetter"/> class.
        /// </summary>
        public tableStyleRowSetter()
        {
        }

        /// <summary>
        /// Adds to ignore list.
        /// </summary>
        /// <param name="dr">The dr.</param>
        public void AddToIgnoreList(DataRow dr)
        {
            if (!ignoreList.Contains(dr))
            {
                ignoreList.Add(dr);
            }
        }

        private List<DataRow> _ignoreList = new List<DataRow>();

        /// <summary> </summary>
        public List<DataRow> ignoreList
        {
            get
            {
                return _ignoreList;
            }
            protected set
            {
                _ignoreList = value;
            }
        }

        /// <summary>
        /// Sets the specified style by rows indexes. The row index is interpreted by index in source table (not reporting version of the table -- i.e. heading rows are not counted, only data)
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns>
        /// Rule that was created
        /// </returns>
        public dataRowIndexDynamicStyle<DataRowInReportTypeEnum> SetStyleForRows(DataRowInReportTypeEnum style, Int32 rowIndex)
        {
            var indexC = new dataRowIndexDynamicStyle<DataRowInReportTypeEnum>(style, new Int32[] { rowIndex });
            indexC.indexFromSourceTable = true;
            units.Add(indexC);
            return indexC;
        }

        /// <summary>
        /// Sets the specified style by rows indexes. The row index is interpreted by index in source table (not reporting version of the table -- i.e. heading rows are not counted, only data)
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="rowIndexes">The row indexes.</param>
        /// <returns>Rule that was created</returns>
        public dataRowIndexDynamicStyle<DataRowInReportTypeEnum> SetStyleForRows(DataRowInReportTypeEnum style, IEnumerable<Int32> rowIndexes)
        {
            var indexC = new dataRowIndexDynamicStyle<DataRowInReportTypeEnum>(style, rowIndexes);
            indexC.indexFromSourceTable = true;
            units.Add(indexC);
            return indexC;
        }

        /// <summary>
        /// Sets the style for rows with value.
        /// </summary>
        /// <typeparam name="TValueType">The type of the value type.</typeparam>
        /// <param name="style">The style.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns></returns>
        public dataValueMatchCriterionDynamicStyle<TValueType, DataRowInReportTypeEnum> SetStyleForRowsWithValue<TValueType>(DataRowInReportTypeEnum style, String columnName, TValueType rowIndex) where TValueType : IComparable
        {
            var indexC = new dataValueMatchCriterionDynamicStyle<TValueType, DataRowInReportTypeEnum>(new TValueType[] { rowIndex }, style, columnName);

            units.Add(indexC);
            return indexC;
        }

        /// <summary>
        /// Sets the style for rows with value.
        /// </summary>
        /// <typeparam name="TValueType">The type of the value type.</typeparam>
        /// <param name="style">The style.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="rowIndexes">The row indexes.</param>
        /// <returns></returns>
        public dataValueMatchCriterionDynamicStyle<TValueType, DataRowInReportTypeEnum> SetStyleForRowsWithValue<TValueType>(DataRowInReportTypeEnum style, String columnName, IEnumerable<TValueType> rowIndexes) where TValueType : IComparable
        {
            var indexC = new dataValueMatchCriterionDynamicStyle<TValueType, DataRowInReportTypeEnum>(rowIndexes, style, columnName);

            units.Add(indexC);
            return indexC;
        }

        /// <summary>
        /// Evaluates the specified row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="table">The table.</param>
        /// <param name="defaultStyle">The default style.</param>
        /// <returns></returns>
        public tableStyleSetterResponse evaluate(DataRow row, DataTable table, dataTableStyleEntry defaultStyle)
        {
            tableStyleSetterResponse output = new tableStyleSetterResponse(defaultStyle, null);

            try
            {
                foreach (var unit in units)
                {
                    output = unit.evaluate(row, table, output);
                    if (output.style != defaultStyle)
                    {
                        return output;
                    }
                }

                foreach (var item in items)
                {
                    output = item(row, table, output);
                    if (output.style != defaultStyle)
                    {
                        return output;
                    }
                }
            }
            catch (Exception ex)
            {
                output.notes.Add(GetType().Name + "EVALUATE" + ex.Message);
                output.notes.Add(ex.StackTrace);
            }
            return output;
        }

        private List<rowMetaEvaluation> _items = new List<rowMetaEvaluation>();

        /// <summary> </summary>
        [XmlIgnore]
        public List<rowMetaEvaluation> items
        {
            get
            {
                return _items;
            }
            protected set
            {
                _items = value;
            }
        }
    }
}