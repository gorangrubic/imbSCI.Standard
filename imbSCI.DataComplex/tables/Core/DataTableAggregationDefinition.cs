// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTableAggregationDefinition.cs" company="imbVeles" >
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
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.math.aggregation;
    using imbSCI.Data;
    using imbSCI.Data.enums.fields;
    using imbSCI.DataComplex.extensions.data.schema;
    using imbSCI.DataComplex.tables.extensions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    public class DataTableAggregationDefinition
    {
        public int sources { get; set; }

        public dataPointAggregationAspect aspect { get; set; }

        public dataPointAggregationType type { get; set; }

        public int rowsMax { get; set; }

        public int rowsCommon { get; set; }

        public int rowsProcessed { get; set; }

        public DataColumnMetaDictionary shemaColumns { get; set; } = new DataColumnMetaDictionary();

        public List<DataColumn> columns { get; set; } = new List<DataColumn>();
        public string desc { get; set; } = "";

        public List<string> extraDescriptions = new List<string>();

        public DataTableAggregationDefinition(IEnumerable<DataTable> tables, dataPointAggregationAspect aspect = dataPointAggregationAspect.overlapMultiTable)
        {
            if (tables == null) return;
            if (!tables.Any()) return;

            process(tables, aspect);
        }

        //public PropertyCollection getTempRowData()
        //{
        //    PropertyCollection output = new PropertyCollection();

        //    foreach (var pair in shemaColumns)
        //    {
        //        foreach (var dt in pair.Value)
        //        {
        //            output.Add(dt.columnName, dt.columnDefault);
        //        }
        //    }
        //    return output;
        //}

        public DataTable getShema(string tableName)
        {
            columns = new List<DataColumn>();
            DataTable output = new DataTable();
            output.SetTitle(tableName);
            output.SetDescription(desc);
            output.SetAggregationAspect(dataPointAggregationAspect.overlapMultiTable);
            output.SetAggregationOriginCount(sources);
            output.SetAdditionalInfo(additionalProps);
            output.SetExtraDesc(extraDescriptions);

            output.SetColumnMeta(shemaColumns);

            foreach (var pair in shemaColumns)
            {
                foreach (var dt in pair.Value)
                {
                    DataColumn dc = output.Add(dt.columnName, dt.columnDescription, dt.columnLetter, dt.columnValueType, dt.importance, dt.format, dt.spe.displayName);
                    //dt.spe.name = dt.columnName;
                    dc.SetGroup(dt.spe.categoryName);
                    dc.SetLetter(dt.spe.letter);
                    dc.SetUnit(dt.spe.unit);
                    dc.SetWidth(dt.spe.width);

                    dc.SetDesc(dt.spe.description);

                    //dc.SetUnit(dt.columnUnit);

                    // dc.SetPriority(dt.columnPriority);
                    // dc.SetSPE(dt.spe);

                    columns.Add(dc);
                    //dc.DefaultValue = dt.columnDefault;
                }
            }
            output.SetCategoryPriority(categoriesPriority);

            output.Columns.OrderByCategoryPriority(categoriesPriority);

            for (int i = 0; i < rowsMax; i++)
            {
                var dr = output.NewRow();

                output.Rows.Add(dr);
            }

            return output;
        }

        public PropertyCollectionExtended additionalProps { get; set; } = new PropertyCollectionExtended();

        public List<string> categoriesPriority = new List<string>();

        public const string ADDPROPS_ROWSMAX = "Rows max";
        public const string ADDPROPS_ROWSCOMMON = "Rows common";
        public const string ADDPROPS_ROWSPROCESSED = "Rows processed";

        public void process(IEnumerable<DataTable> tables, dataPointAggregationAspect __aspect = dataPointAggregationAspect.overlapMultiTable)
        {
            aspect = __aspect;

            string __desc = "This table is result of summary operation (" + aspect.ToString() + ") over tables: ";
            int tc = 0;

            int rcMin = int.MaxValue;
            int rcMax = int.MinValue;
            int rcProc = 0;

            DataTable shemaProvider = null;
            foreach (DataTable dt in tables)
            {
                if (shemaProvider == null) shemaProvider = dt;

                tc++;
                if (tc < 5)
                {
                    __desc = __desc.add(dt.GetTitle(), ", ");
                }
                rcMin = Math.Min(dt.Rows.Count, rcMin);
                rcMax = Math.Max(dt.Rows.Count, rcMax);
                rcProc += dt.Rows.Count;
            }

            additionalProps.AddRange(shemaProvider.GetAdditionalInfo(), false, false, false);

            categoriesPriority = shemaProvider.GetCategoryPriority();
            if (categoriesPriority.Any())
            {
            }

            if (tc > 4) __desc = __desc.add("... in total: " + tc + " tables", ", ");
            sources = tc;
            rowsMax = rcMax;
            rowsCommon = rcMin;
            rowsProcessed = rcProc;

            additionalProps.Add(ADDPROPS_ROWSMAX, rcMax, ADDPROPS_ROWSMAX, "Highest row count in the table set");
            additionalProps.Add(ADDPROPS_ROWSCOMMON, rcMin, ADDPROPS_ROWSCOMMON, "Lowest row count in the table set");
            additionalProps.Add(ADDPROPS_ROWSPROCESSED, rcProc, ADDPROPS_ROWSPROCESSED, "Total count of rows processed");
            additionalProps.Add("Category", categoriesPriority.toCsvInLine());

            extraDescriptions.Add("[" + sources + "] source tables had at least [" + rowsCommon + "] rows. Maximum rows per source table: [" + rowsMax + "]");

            shemaColumns = new DataColumnMetaDictionary();

            foreach (DataColumn dc in shemaProvider.Columns)
            {
                /* FINDING AGGREGATION SETTINGS */
                settingsPropertyEntry spe = dc.GetSPE();
                dataPointAggregationDefinition agg = null;
                if (spe.aggregation == null)
                {
                    PropertyInfo pi = dc.ExtendedProperties.getProperObject<PropertyInfo>(templateFieldDataTable.col_propertyInfo); //, col_spe.pi);
                    if (pi != null)
                    {
                        spe = new settingsPropertyEntry(pi);
                    }
                }
                if (spe.aggregation == null)
                {
                    spe.aggregation = new dataPointAggregationDefinition();
                }
                agg = spe.aggregation;
                /* ----------------------------- */

                if (agg[aspect] != dataPointAggregationType.none)
                {
                    List<dataPointAggregationType> aggTypes = agg[aspect].getEnumListFromFlags<dataPointAggregationType>();

                    foreach (dataPointAggregationType a in aggTypes)
                    {
                        if (a != dataPointAggregationType.hidden)
                        {
                            shemaColumns.Add(DataColumnInReportTypeEnum.dataSummed, dc, a, dc.GetUnit());
                        }
                    }
                }
            }

            desc = __desc;
        }
    }
}