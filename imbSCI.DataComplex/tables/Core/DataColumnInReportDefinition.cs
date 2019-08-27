// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataColumnInReportDefinition.cs" company="imbVeles" >
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
    using imbSCI.Core.data;
    using imbSCI.Core.enums;
    using imbSCI.Core.math.aggregation;
    using System;

    public class DataColumnInReportDefinition
    {
        public DataColumnInReportTypeEnum columnType { get; set; } = DataColumnInReportTypeEnum.data;

        public DataColumnInfoSourceEnum infoSource { get; set; } = DataColumnInfoSourceEnum.none;

        public dataPointAggregationType aggregation { get; set; } = dataPointAggregationType.sum;

        public string columnSourceName { get; set; } = "";

        public string columnLetter { get; set; }
        public string columnUnit { get; set; }
        public string columnDescription { get; set; }
        public string columnName { get; set; }
        public Type columnValueType { get; set; }
        public string format { get; set; }
        public string columnGroup { get; set; }

        public settingsPropertyEntry spe { get; set; }

        public dataPointImportance importance { get; set; }
        public object columnDefault { get; set; }
        public int columnPriority { get; internal set; }
    }
}