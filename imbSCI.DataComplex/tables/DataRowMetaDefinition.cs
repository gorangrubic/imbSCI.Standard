// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRowMetaDefinition.cs" company="imbVeles" >
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
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.math.aggregation;
    using imbSCI.Data.enums.fields;
    using System.Data;

    public class DataRowMetaDefinition : PropertyCollectionExtended
    {
        public DataRowInReportTypeEnum rowType { get; set; } = DataRowInReportTypeEnum.data;
        public templateFieldDataTable cellDataSource { get; set; } = templateFieldDataTable.renderEmptySpace;

        public DataRow rowInstance { get; set; } = null;
        public dataPointAggregationType aggregation { get; set; } = dataPointAggregationType.none;

        public DataRowMetaDefinition(DataRowInReportTypeEnum __rowType, dataPointAggregationType __aggregation)
        {
            rowType = __rowType;
            aggregation = __aggregation;
        }

        public DataRowMetaDefinition(DataRowInReportTypeEnum __rowType, templateFieldDataTable __cellDataSource)
        {
            rowType = __rowType;
            cellDataSource = __cellDataSource;
        }

        public DataRowMetaDefinition()
        {
        }

        public DataRowMetaDefinition(DataRowInReportTypeEnum type)
        {
            rowType = type;
        }

        public DataRowMetaDefinition(DataRow instance, DataRowInReportTypeEnum type)
        {
            rowType = type;
            rowInstance = instance;
        }

        public DataRowMetaDefinition(DataRow instance)
        {
            rowType = DataRowInReportTypeEnum.data;
            rowInstance = instance;
        }
    }
}