// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pagePresetTableReport.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.presets.servicepages
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.meta.blocks;
    using imbSCI.Reporting.meta.page;
    using System.Data;

    /// <summary>
    /// PREVAZIDJENO Overview page for imbDataReportDocument.
    /// </summary>
    /// <remarks>
    /// Containes short information on each DataTable page inside the collection
    /// </remarks>
    /// <seealso cref="metaServicePage" />
    public class pagePresetTableReport : metaServicePage
    {
        #region --- table ------- reference to data table to display

        private DataTable _table;

        /// <summary>
        /// reference to data table to display
        /// </summary>
        public DataTable table
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value;
                OnPropertyChanged("table");
            }
        }

        #endregion --- table ------- reference to data table to display

        public pagePresetTableReport(DataTable __table) : base("")
        {
            table = __table;
            name = table.ExtendedProperties.getProperString(templateFieldDataTable.data_tablenamedb, templateFieldDataTable.data_tablename, templateFieldDataTable.shema_sourcename);

            header.name = "DB table " + name + "";
            header.description = "Columns: ".t(templateFieldDataTable.data_columncount) + (" - Rows: ").t(templateFieldDataTable.data_rowcounttotal);

            metaDataTable metaTable = new metaDataTable(table);

            footer.bottomLine = "Query: ".t(templateFieldDataTable.data_query) + (" | ").t(templateFieldBasic.sys_time) + (" - mem.alloc: ").t(templateFieldBasic.sys_mem) + (" - threads: ").t(templateFieldBasic.sys_threads);
        }
    }
}