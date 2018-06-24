// --------------------------------------------------------------------------------------------------------------------
// <copyright file="setPresetDatabaseDump.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.meta.documentSet
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.interfaces;
    using imbSCI.Reporting.meta.document;
    using imbSCI.Reporting.meta.page;
    using imbSCI.Reporting.meta.presets.servicepages;
    using imbSCI.Reporting.script;
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Complete knowledge base dump
    /// </summary>
    /// \ingroup_disabled docDocumentSet
    public class setPresetDatabaseDump : metaDocumentSet, IMetaComposeAndConstruct
    {
        public override metaPage indexPage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public setPresetDatabaseDump(string __name = "DBDump")
        {
            name = __name;
            documentSetTitle = "Database dump";
        }

        /// <summary>
        /// Constructs the specified resources.
        /// </summary>
        /// <param name="resources"></param>
        public override void construct(object[] resources) //compose(IMetaComposer composer, metaDocumentTheme theme, PropertyCollection data, params object[] resources)
        {
            List<object> reslist = resources.getFlatList<object>(typeof(PropertyCollectionList));

            PropertyCollectionList dataSet = resources[0] as PropertyCollectionList; //reslist.getFirstOfType<PropertyCollectionList>(false, null);
            PropertyCollectionList dbInfo = resources[1] as PropertyCollectionList; ; // reslist.getFirstOfType<PropertyCollection>(false, null);
            PropertyCollection directAccessDataList = resources[2] as PropertyCollection;

            var theme = reslist.getFirstOfType<styleTheme>(false, null);

            documentDatabaseReport report = new documentDatabaseReport();

            documents.Add(report, this);

            report.construct(resources);

            //foreach (PropertyCollection data in dataSet) {
            //    DataTable output = data.getProperObject<DataTable>(templateFieldDataTable.data_table);

            //    //data[templateFieldDataTable.data_accesslist] = directAccessList;

            //    //DataTable table = component.settings.dbc.execute("SELECT * FROM " + dba.tableName);
            //    //dba.AppendDataFields(data);
            //    //table.AppendDataFields(data);
            //    //dbDump.addDbTableReportPage(table);
            //}

            //foreach (metaDocument pg in documents)
            //{
            //    pg.construct(resources);
            //}

            //PropertyCollection extraData = new PropertyCollection();
        }

        public override docScript compose(docScript script)
        {
            if (script == null) script = new docScript(name);

            script.x_scopeIn(this);

            documents.Sort();

            foreach (metaDocument pg in documents)
            {
                pg.compose(script);
            }

            script.x_scopeOut(this);

            return script;
        }

        /// <summary>
        /// Adds the database table report page, automatically creates database document and overview page
        /// </summary>
        /// <param name="table">The table.</param>
        public void addDbTableReportPage(DataTable table)
        {
            string db_name = table.ExtendedProperties.getProperString(templateFieldDataTable.data_dbname);
            string table_name = table.ExtendedProperties.getProperString(templateFieldDataTable.data_tablenamedb);
            // ako nema dokumenta stvara ga
            documentDatabaseReport dbDocument = documents[db_name] as documentDatabaseReport;
            if (dbDocument == null)
            {
                dbDocument = new documentDatabaseReport(table.ExtendedProperties);
                documents.Add(dbDocument, this);
            }
            /// ako nema tranice pravi je
            pagePresetTableReport dbPage = dbDocument[table_name] as pagePresetTableReport;
            if (dbPage == null)
            {
                dbPage = new pagePresetTableReport(table);
                dbDocument.pages.Add(dbPage, dbDocument);
            }
        }

        //public void addDatabaseReport(DataSet dataSet, String db_name, String db_server, PropertyCollection data)
        //{
        //}
    }
}