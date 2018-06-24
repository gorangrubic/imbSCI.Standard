// --------------------------------------------------------------------------------------------------------------------
// <copyright file="documentDatabaseReport.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.document
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.meta.page;
    using imbSCI.Reporting.meta.presets.servicepages;
    using imbSCI.Reporting.script;
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// 2017: Report based on DataTable collection and descriptive metaBundle object
    /// </summary>
    /// \ingroup_disabled docDocument
    public class documentDatabaseReport : metaDocument
    {
        public override void construct(object[] resources)
        {
            List<object> reslist = resources.getFlatList<object>(typeof(PropertyCollectionList), typeof(PropertyCollection));

            PropertyCollectionList dataSet = reslist.Pop<PropertyCollectionList>(); // reslist[0] as PropertyCollectionList; //.getFirstOfType<PropertyCollectionList>(false, null);
            PropertyCollection dbInfo = reslist.Pop<PropertyCollection>(); // reslist.getFirstOfType<PropertyCollection>(false, null);
            PropertyCollectionList directAccessDataList = reslist.Pop<PropertyCollectionList>();  //reslist.getFirstOfType<PropertyCollectionList>(false, null);

            theme = reslist.getFirstOfType<styleTheme>(false, null);

            documentTitle = "DB Dump report :: " + (dbInfo.getProperString(templateFieldDataTable.data_dbname) + (" @ ") + (dbInfo.getProperString(templateFieldDataTable.data_dbhost)));
            documentDescription = "Database diagnostic dump -- all tables found at {{{data_dbname}}}";
            documentBottomLine = "Tables :: " + (dbInfo.getProperString(templateFieldDataTable.data_tablescount) + "  DB User :: " + (dbInfo.getProperString(templateFieldDataTable.data_dbuser)));

            //dataSet[0].getProperObject<PropertyCollectionList>(templateFieldDataTable.data_accesslist);

            overview = new pagePresetDataTableReportOverview(documentTitle, documentDescription, documentBottomLine);
            pages.Add(overview, this);
            overview.construct(resources);

            serviceDocumentFolderReadmePage readme = new serviceDocumentFolderReadmePage(name, documentTitle, documentDescription, documentBottomLine);
            pages.Add(readme, this);
            readme.construct(resources);

            foreach (PropertyCollection data in dataSet)
            {
                DataTable output = data.getProperObject<DataTable>(templateFieldDataTable.data_table);
                metaDataTablePage tablePage = new metaDataTablePage();
                pages.Add(tablePage, this);
                tablePage.construct(resources);

                acePaletteRole rl = theme.palletes.paletteRoleWheel.next();
                tablePage.settings.mainColor = rl;

                //data[templateFieldDataTable.data_accesslist] = directAccessList;

                //DataTable table = component.settings.dbc.execute("SELECT * FROM " + dba.tableName);
                //dba.AppendDataFields(data);
                //table.AppendDataFields(data);
                //dbDump.addDbTableReportPage(table);
            }

            //List<DataTable> dts = reslist.getAllOfType<DataTable>(false);

            //foreach (Object dt in dts)
            //{
            //    metaDataTablePage tmp = new metaDataTablePage();
            //    pages.Add(tmp, this);
            //    tmp.construct(dt);
            //}

            sortChildren();

            //throw new NotImplementedException();
        }

        public override docScript compose(docScript script)
        {
            script.x_scopeIn(this);

            script = baseCompose(script);

            script.x_scopeOut(this);

            return script;
        }

        /// <summary>
        ///
        /// </summary>
        public pagePresetDataTableReportOverview overview { get; set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="documentDatabaseReport"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public documentDatabaseReport(PropertyCollection data)
        {
            name = data.getProperString(templateFieldDataTable.data_dbname);
            header.title = "Database [".t(templateFieldDataTable.data_dbname) + "] report";
            header.description = "Host: ".t(templateFieldDataTable.data_dbhost) + " Username: ".t(templateFieldDataTable.data_dbuser);

            footer.bottomLine = "Total tables: ".t(templateFieldDataTable.data_tablescount) + "";
            overview = new pagePresetDataTableReportOverview(header.name, header.description, footer.bottomLine);

            //tables = __tables.getFlatList<DataTable>();

            // servicePages.Add(new pagePresetGeneralReadme());
            //servicePages.Add(new pagePresetDataTableReportOverview());

            //path.filenameWithoutExtension = __title.getFilename();
            //path.deployFormat(format);
        }

        ///// <summary>
        ///// Adds the collection.
        ///// </summary>
        ///// <param name="data">The data.</param>
        ///// <param name="dt_title">The dt title.</param>
        ///// <param name="doOnlyWithDisplayCaption">if set to <c>true</c> [do only with display caption].</param>
        ///// <param name="doInherited">if set to <c>true</c> [do inherited].</param>
        ///// <param name="__description">The description.</param>
        ///// <param name="keywords">The keywords.</param>
        ///// <returns></returns>
        //public metaDataTablePage addCollection(IEnumerable data, String dt_title, Boolean doOnlyWithDisplayCaption, Boolean doInherited, String __description, params String[] keywords)
        //{
        //    DataTable dt = data.buildDataTable(dt_title, doOnlyWithDisplayCaption, doInherited);
        //    var tmp = addTable(dt, __description, keywords);
        //    return tmp;
        //}

        //// <summary>
        //// Adds data table into report
        //// </summary>
        //// <param name = "data" ></ param >
        //// < param name="__description"></param>
        //// <returns></returns>
        //public metaDataTablePage addTable(DataTable data, String __description, params String[] keywords)
        //{
        //    String __title = data.TableName.getTitleFromPath(true);

        //    tmp.setup(data);
        //    tmp.keywords.content.AddRange(keywords);
        //    tmp.footer = footer;
        //    pages.Add(tmp);

        //    return tmp;
        //}

        /// <summary>
        ///
        /// </summary>
        public List<DataTable> tables { get; set; } = new List<DataTable>();

        public override metaPage indexPage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        // <summary>
        // Parameterless constructor used by deserialization
        // </summary>
        public documentDatabaseReport()
        {
        }
    }
}