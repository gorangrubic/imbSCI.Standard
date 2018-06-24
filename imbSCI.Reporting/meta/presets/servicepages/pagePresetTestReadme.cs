// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pagePresetTestReadme.cs" company="imbVeles" >
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
    /// Readme file for one research sample item (i.e. for webSiteProfile www.koplas.co.rs)
    /// </summary>
    /// \ingroup_disabled docServicePage
    public class pagePresetTestReadme : pagePresetGeneralReadme
    {
        public pagePresetTestReadme()
        {
            header.name = "Test report [".t(templateFieldBasic.test_caption) + (" [").t(templateFieldBasic.test_runstamp) + "]";
            header.description = "Description: ".t(templateFieldBasic.test_description) + " - Status: ".t(templateFieldBasic.test_status);

            content.Add("Test start: ".t(templateFieldBasic.test_runstart));
            content.Add("Test running time: ".t(templateFieldBasic.test_runtime));
            content.Add("Test version: ".t(templateFieldBasic.test_versionCount));

            content.Add("Report path: ".t(templateFieldBasic.parent_dir));
            content.Add("Report index path: ".t(templateFieldBasic.parent_index));
            content.Add("Application root: ".t(templateFieldBasic.sys_path));
        }
    }
}