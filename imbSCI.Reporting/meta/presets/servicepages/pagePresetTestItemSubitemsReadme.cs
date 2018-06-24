// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pagePresetTestItemSubitemsReadme.cs" company="imbVeles" >
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
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.enums;

    /// <summary>
    /// Report on sub items - testing web site sub pages
    /// </summary>
    /// \ingroup_disabled docServicePage
    public class pagePresetTestItemSubitemsReadme : pagePresetGeneralReadme
    {
        public pagePresetTestItemSubitemsReadme()
        {
            header.name = "Readme on : Test Item report asset [".t(templateFieldBasic.page_id) + (" part of ").t(templateFieldBasic.parent_type);
            header.description = "Type: ".t(templateFieldBasic.page_type) + (" - ").t(templateFieldBasic.path_file) + (" - ").t(templateFieldBasic.page_id) + (" of ").t(templateFieldBasic.page_number);

            content.Add("Item name: ".t(templateFieldBasic.page_name));
            content.Add("Item description: ".t(templateFieldBasic.page_title));
            content.Add("Parent directory path: ".t(templateFieldBasic.parent_dir));
            content.Add("Parent index path: ".t(templateFieldBasic.parent_index));
            content.Add("Parent type: ".t(templateFieldBasic.parent_type));
        }
    }
}