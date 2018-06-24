// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRowMetaDictionary.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.math.aggregation;
    using imbSCI.Data.collection.nested;
    using imbSCI.Data.enums.fields;
    using System.Collections.Generic;

    /// <summary>
    /// Describes what rows shall be created once <see cref="DataTable"/> is converted to <see cref="DataTableForStatistics"/> output
    /// </summary>
    /// <seealso cref="aceCommonTypes.collection.nested.aceEnumListSet{aceCommonTypes.data.tables.DataRowInReportTypeEnum, aceCommonTypes.data.tables.DataRowMetaDefinition}" />
    public class DataRowMetaDictionary : aceEnumListSet<DataRowInReportTypeEnum, DataRowMetaDefinition>
    {
        public List<DataRowMetaDefinition> allRows = new List<DataRowMetaDefinition>();

        public DataRowMetaDefinition Add(DataRowInReportTypeEnum type, templateFieldDataTable cellSource)
        {
            var output = new DataRowMetaDefinition(type, cellSource);
            Add(type, output);

            return output;
        }

        public DataRowMetaDefinition Add(DataRowInReportTypeEnum type, dataPointAggregationType aggregation)
        {
            var output = new DataRowMetaDefinition(type, aggregation);
            Add(type, output);

            return output;
        }
    }
}