// --------------------------------------------------------------------------------------------------------------------
// <copyright file="globalMeasureUnitDictionary.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Data;
    using System.Reflection;

    public static class globalMeasureUnitDictionary
    {
        public static void build()
        {
            var data = units;

            data.Add(globalMeasureEnum.iid, "instanceID", "Instance ID", "Human readable identification of instance covered by the record");

            data.Add(globalMeasureEnum.instance, "instanceTypeInfo.displayName", "Instance type", "Type name of the instance followed by this record");
            data.Add(globalMeasureEnum.instancedesc, "instanceTypeInfo.description", "Instance info", "Notes on the instance type");

            data.Add(globalMeasureEnum.uid, "UID", "UID", "Universal code-ID of the record instance");
            data.Add(globalMeasureEnum.recordstate, "state", "Record state", "State flag for this record instance");
            data.Add(globalMeasureEnum.runstamp, "testRunStamp", "Run stamp", "Identification code of the experiment test run instance");
            data.Add(globalMeasureEnum.recordtype, "iTI.displayName", "Record type", "Name of the record class");
            data.Add(globalMeasureEnum.recorddesc, "iTI.description", "Record info", "Notes about applied record class");

            data.Add(globalMeasureEnum.start, "timeStart.ToShortTimeString()", "Start", "Start of the record / the instance run");
            data.Add(globalMeasureEnum.finish, "timeFinish.ToShortTimeString()", "Finish", "Time of the instance execution finished");

            data.Add(globalMeasureEnum.sessions, 0, "Sessions", "Number of the record start-finish sessions");

            data.Add(globalMeasureEnum.duration, "", "Duration", "Time span of the instance run, in seconds.");

            data.Add(globalMeasureEnum.remarks, "", "Remarks", "Remark flags about the instance execution");

            data.Add(new { globalMeasureEnum.average, globalMeasureEnum.avgFreq }, 0, "Average", "Simple arithmetic mean value").SetFormat("#0.00").SetType(typeof(Double));
            data.Add(new { globalMeasureEnum.median, globalMeasureEnum.medianFreq }, 0, "Median", "Half of the array has greater frequency.").SetFormat("#0.00").SetType(typeof(Double));
            data.Add(new { globalMeasureEnum.entropyFreq, globalMeasureEnum.entropy }, 0, "Entropy", "Measure of heterogenity, the perfectly heterogenic array has entropy value 0.").SetFormat("#0.00").SetType(typeof(Double));

            data.Add(globalMeasureEnum.diversityRatio, 0, "Diversity", "#0.00", "Customized measure of diversity, a ratio calculus similar to Gini index").SetFormat("#0.00").SetType(typeof(Double));
            data.Add(globalMeasureEnum.count, 0, "Count", "Number of distinct instances discovered in the sample"); // typeof(Int32)).set(templateFieldDataTable.col_caption, "Count").set(templateFieldDataTable.col_desc, "Number of distinct instances discovered in the sample");
            data.Add(globalMeasureEnum.totalScore, 0, "Total score", "Number of instances observed");

            data.Add(globalMeasureEnum.itemTitle, "", "Name", "Name of the item");

            stats.Add(modelSpiderSiteTimelineEnum.tl_crosslinks, 0, "Cross links", "Crosslinks discovered up to the iteration");
            stats.Add(modelSpiderSiteTimelineEnum.tl_tasksize, 0, "Task size", "Number of links sent to web client to retrieve");
            stats.Add(modelSpiderSiteTimelineEnum.tl_newlinks, 0, "New links", "Count of discovered links accepted for active set");

            stats.Add(modelSpiderSiteTimelineEnum.tl_iteration, 0, "Iteration", "Final iteration count");
            stats.Add(modelSpiderSiteTimelineEnum.tl_pagesloaded, 0, "Pages loaded", "Total count of pages loaded");
            stats.Add(modelSpiderSiteTimelineEnum.tl_pagesdetected, 0, "Pages detected", "Total count of pages loaded");
            stats.Add(modelSpiderSiteTimelineEnum.tl_pagesaccepted, 0, "Pages accepted", "Total count of pages loaded");
            stats.Add(modelSpiderSiteTimelineEnum.tl_totallinks, 0, "Links detected", "Total links processed");
            stats.Add(modelSpiderSiteTimelineEnum.tl_linksaccepted, 0, "Links accepted", "Total links processed");
            stats.Add(modelSpiderSiteTimelineEnum.tl_activelinks, 0, "Ative links", "Active links left at the end of procedure");
            stats.Add(modelSpiderSiteTimelineEnum.tl_stability, 0, "Pages loaded", "Total links processed");
        }

        public static String GetTableName(this Enum en)
        {
            return tables.entries[en][PropertyEntryColumn.entry_name].toStringSafe("Table");
        }

        public static String GetTableDescription(this globalTableEnum en)
        {
            return tables.entries[en][PropertyEntryColumn.entry_description].toStringSafe("");
        }

        public static PropertyEntry GetPropertyEntry(this globalTableEnum en)
        {
            return tables.entries[en];
        }

        public enum globalTableEnum
        {
            relationshipByType,
            tokensByCategory,
            selfSummary,
            recordSummary,
            scoreTable,
            identification,
        }

        public enum globalMeasureEnum
        {
            iid,
            uid,
            recordstate,
            runstamp,
            instance,
            instanceType,
            recordtype,
            recorddesc,
            start,
            finish,
            duration,
            remarks,
            instancedesc,
            sessions,

            sum,
            total,
            totalScore,
            count,
            diversity,
            diversityAntiValue,
            diversityRatio,
            range,
            avgFreq,
            minFreq,
            maxFreq,
            ratio,
            value,
            entropy,
            entropyFreq,
            medianFreq,
            median,
            variance,
            varianceFreq,
            standardDeviation,

            average,
            mean,
            name,
            itemTitle,

            frequency,
        }

        public enum modelSpiderSiteTimelineEnum
        {
            tl_activelinks,
            tl_pagesloaded,
            tl_totallinks,
            tl_crosslinks,
            tl_iteration,
            tl_tasksize,
            tl_newlinks,
            tl_pagesdetected,
            tl_stability,
            tl_pagesaccepted,
            tl_linksaccepted,
        }

        public enum columnPresets
        {
            autoCount,
            objectName,
            objectDescription,
        }

        public enum tablePresets
        {
            summaryOneRow,
            verticalTable,
        }

        public static void buildTables()
        {
            /// Value => instanceTitle
            tables.Add(globalTableEnum.relationshipByType, "Token", "Relationship by table", "By relationship type");
            tables.Add(globalTableEnum.tokensByCategory, "Category", "Tokens by category", "Statistcs of token sample");
            tables.Add(globalTableEnum.recordSummary, "Summary", "Record summary", "Key figures of this record instance");
            tables.Add(globalTableEnum.scoreTable, "Item", "Score table", "Score distribution per item");
            tables.Add(globalTableEnum.identification, "ID info", "ID info", "Basic info about the instance");
        }

        private static PropertyCollectionExtended _tables;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static PropertyCollectionExtended tables
        {
            get
            {
                if (_tables == null)
                {
                    _tables = new PropertyCollectionExtended();
                    buildTables();
                }
                return _tables;
            }
        }

        private static PropertyCollectionExtended _units;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static PropertyCollectionExtended units
        {
            get
            {
                if (_units == null)
                {
                    _units = new PropertyCollectionExtended();
                    build();
                }
                return _units;
            }
        }

        private static PropertyCollectionExtended _stats;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static PropertyCollectionExtended stats
        {
            get
            {
                return _units;
            }
        }

        private static DataTable AddTable(this Object host, tablePresets preset, DataSet dataset = null)
        {
            String name = "";
            String description = "";

            switch (preset)
            {
                case tablePresets.summaryOneRow:
                    name = "Summary";
                    description = "Summary property view";
                    break;

                case tablePresets.verticalTable:
                    name = "Property";
                    description = "Complete property list";
                    break;
            }

            DataTable output = host.addTable(name, description, dataset);

            Type type = host.GetType();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            return output;
        }

        private static DataTable addTable(this Object host, String name, String description, DataSet dataset = null)
        {
            DataTable output = null;
            if (dataset == null)
            {
                output = new DataTable(name);
            }
            else
            {
                output = dataset.Tables.Add(name);
            }

            output.ExtendedProperties[templateFieldDataTable.data_tablename] = name;
            output.ExtendedProperties[templateFieldDataTable.data_tabledesc] = description;
            return output;
        }

        public static DataColumn AddColumn(this DataTable table, String property, Object host, Type type = null)
        {
            if (type == null) type = host.GetType();

            PropertyInfo pi = type.GetProperty(property, BindingFlags.Public | BindingFlags.Instance);

            return AddColumn(table, pi, pi.PropertyType);
        }

        public static DataColumn AddColumn(DataTable table, PropertyInfo property, Type type = null)
        {
            PropertyEntry pe = stats.entries.Get(property.Name);

            return AddColumn(table, pe, type);
        }

        public static DataColumn AddColumn(DataTable table, PropertyEntry pe, Type type = null)
        {
            DataColumn dc = table.Columns.Add((string)pe.keyName);
            if (type == null) type = pe[templateFieldDataTable.col_type] as Type;
            dc.DataType = type;
            dc.ColumnName = pe[PropertyEntryColumn.entry_key].ToString();
            dc.Caption = pe[PropertyEntryColumn.entry_name].ToString();

            dc.ExtendedProperties.AddRange(pe);
            dc.ExtendedProperties[templateFieldDataTable.col_caption] = dc.Caption;
            dc.ExtendedProperties[templateFieldDataTable.col_desc] = pe[PropertyEntryColumn.entry_description];

            return dc;
        }
    }
}