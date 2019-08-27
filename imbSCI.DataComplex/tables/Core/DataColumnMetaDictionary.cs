// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataColumnMetaDictionary.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.tables
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.math.aggregation;
    using imbSCI.Data.collection.nested;
    using System;
    using System.Data;

#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'aceCommonTypes.collection.nested.aceEnumListSet{aceCommonTypes.data.tables.DataColumnInReportTypeEnum, aceCommonTypes.data.tables.DataColumnInReportDefinition}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    /// Describes what columns shall be created once <see cref="DataTable"/> is converted to <see cref="DataTableForStatistics"/> output
    /// </summary>
    /// <seealso cref="aceCommonTypes.collection.nested.aceEnumListSet{aceCommonTypes.data.tables.DataColumnInReportTypeEnum, aceCommonTypes.data.tables.DataColumnInReportDefinition}" />
    public class DataColumnMetaDictionary : aceDictionarySet<string, DataColumnInReportDefinition>
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'aceCommonTypes.collection.nested.aceEnumListSet{aceCommonTypes.data.tables.DataColumnInReportTypeEnum, aceCommonTypes.data.tables.DataColumnInReportDefinition}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    {
        public DataColumnInReportDefinition Add(DataColumnInReportTypeEnum columnType, DataColumnInfoSourceEnum infoSource)
        {
            DataColumnInReportDefinition output = new DataColumnInReportDefinition();
            output.columnType = columnType;
            output.infoSource = infoSource;
            Add(columnType.ToString(), output);
            return output;
        }

        public DataColumnInReportDefinition Add(DataColumnInReportTypeEnum columnType, DataColumn column, dataPointAggregationType aggregation, string unit = "")
        {
            DataColumnInReportDefinition output = new DataColumnInReportDefinition();
            output.columnType = columnType;
            output.aggregation = aggregation;
            output.columnSourceName = column.ColumnName;
            output.columnPriority = column.GetPriority();
            output.format = column.GetFormat();

            Type valueType = typeof(string);
            string name = ""; string description = ""; string letter = "";

            switch (aggregation)
            {
                default:
                case dataPointAggregationType.max:
                case dataPointAggregationType.min:
                case dataPointAggregationType.sum:
                case dataPointAggregationType.firstEntry:
                case dataPointAggregationType.lastEntry:
                case dataPointAggregationType.range:
                    valueType = column.DataType;
                    break;

                case dataPointAggregationType.avg:
                case dataPointAggregationType.stdev:
                case dataPointAggregationType.var:
                case dataPointAggregationType.entropy:
                    valueType = typeof(double);
                    if (output.format.isNullOrEmpty())
                    {
                        output.format = "F5";
                    }
                    break;

                case dataPointAggregationType.count:
                    valueType = typeof(int);
                    break;
            }
            letter = column.GetLetter();

            if (columnType == DataColumnInReportTypeEnum.dataSummed)
            {
                if (!letter.isNullOrEmpty()) letter = aggregation.ToString() + "(" + letter + ")";
                output.columnLetter = letter;

                output.columnDescription = "(" + aggregation.ToString() + ") of " + column.ColumnName + ". " + column.GetDesc();
            }
            output.columnName = column.ColumnName + " (" + aggregation.ToString() + ")";
            output.columnSourceName = column.ColumnName;

            output.importance = column.GetImportance();
            output.columnUnit = column.GetUnit();
            output.columnValueType = valueType;
            output.columnDefault = valueType.GetDefaultValue();
            output.columnGroup = column.GetGroup();

            output.spe = column.GetSPE();
            Add(column.ColumnName, output);

            return output;
        }
    }
}