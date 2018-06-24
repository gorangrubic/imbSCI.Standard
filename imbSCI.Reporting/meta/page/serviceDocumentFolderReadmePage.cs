// --------------------------------------------------------------------------------------------------------------------
// <copyright file="serviceDocumentFolderReadmePage.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.page
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Reporting.meta.blocks;
    using imbSCI.Reporting.script;

    public class serviceDocumentFolderReadmePage : metaServicePage, IPageFormatSettings
    {
        public serviceDocumentFolderReadmePage(string documentName, string headerTitle, string headerDescription, string footerBottomLine) : base(headerDescription, headerTitle, documentName.add("_readme", "_"), 100)
        {
            fileformat = reportOutputFormatName.textMdFile;
            filenameBase = "readme";
            basicBlocksFlags = metaPageCommonBlockFlags.pageHeader | metaPageCommonBlockFlags.pageFooter | metaPageCommonBlockFlags.pageNotation | metaPageCommonBlockFlags.pageKeywords;
        }

        public override void construct(object[] resources)
        {
            //List<Object> reslist = resources.getFlatList<Object>();
            settings.zoneLayoutPreset = cursorZoneLayoutPreset.oneFullPage;
            settings.zoneSpatialPreset = cursorZoneSpatialPreset.longTextLog;

            settings.mainColor = acePaletteRole.colorA;

            fli = new metaFileInfo();
            blocks.Add(fli, this);

            fli.description = "This folder is created as container of report document {{{document_title}}} ({{{document_path}}})";

            sys = new metaSystemInfo();
            blocks.Add(sys, this);

            blocks.Add(notes, this);

            base.construct(resources);
        }

        private metaFileInfo fli = null;
        private metaSystemInfo sys = null;

        private metaNotation notes = new metaNotation();

        public override docScript compose(docScript script)
        {
            script = this.checkScript(script);

            script.x_scopeIn(this);

            // script.x_exportStart(filenameBase, fileformat, reportAPI.imbMarkdown, elementLevelFormPreset.htmlWebSite);

            header.compose(script);

            fli.compose(script);

            sys.compose(script);

            notes.compose(script);

            keywords.compose(script);

            footer.compose(script);

            //  script.x_exportEnd(filenameBase, fileformat);

            script.x_scopeOut(this);

            return script;
        }
    }
}