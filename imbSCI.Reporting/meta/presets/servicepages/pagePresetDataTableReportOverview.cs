// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pagePresetDataTableReportOverview.cs" company="imbVeles" >
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
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.meta.page;
    using imbSCI.Reporting.script;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Overview page for DataTableReport overview -- it uses path mechanism to reach page
    /// </summary>
    public class pagePresetDataTableReportOverview : metaPage
    {
        /// <summary>
        ///
        /// </summary>
        public PropertyCollectionList dataset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="pagePresetDataTableReportOverview"/> class.
        /// </summary>
        public pagePresetDataTableReportOverview(string headerTitle, string headerDescription, string footerBottomLine)
        {
            name = "dbDumpOverview";

            pageTitle = headerTitle;
            pageDescription = headerDescription;

            basicBlocksFlags = metaPageCommonBlockFlags.pageHeader | metaPageCommonBlockFlags.pageFooter | metaPageCommonBlockFlags.pageNavigation | metaPageCommonBlockFlags.pageNotation | metaPageCommonBlockFlags.pageKeywords;

            footer.bottomLine = footerBottomLine;
        }

        public override void construct(object[] resources)
        {
            List<object> reslist = resources.getFlatList<object>(typeof(PropertyCollectionList), typeof(PropertyCollection));

            PropertyCollectionList directAccessDataList = reslist.Pop<PropertyCollectionList>();

            dataset = directAccessDataList;

            base.construct(resources);
        }

        public virtual docScript compose(docScript script)
        {
            script = this.checkScript(script);

            script.x_scopeIn(this);

            header.compose(script);

            navigation.compose(script);

            script.AppendLine();

            foreach (PropertyCollection pc in dataset)
            {
                script.c_line();

                string tablename = pc.getAndRemoveProperString(templateFieldDataTable.data_tablename);
                string desc = pc.getAndRemoveProperString(templateFieldDataTable.data_tabledesc);
                script.pairs(tablename, desc, pc, "", 2, false);

                script.AppendLine();
            }

            footer.compose(script);

            //script.add(appendType.i_page, docScriptArguments.dsa_name, docScriptArguments.dsa_title,docScriptArguments.dsa_description)
            //    .set(name, pageTitle, pageDescription);

            // script.add(appendType.s_settings).arg(docScriptArguments.dsa_stylerSettings, settings);

            ///            script.add(appendType.s_palette).arg(acePaletteRole.colorDefault);

            // script = this.subCompose(script);

            script.x_scopeOut(this);

            return script;
        }
    }
}