// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitMainReport.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.delivery.units
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Core.reporting.style;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Reporting.meta.delivery.items;
    using imbSCI.Reporting.script;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbSCI.Reporting.meta.delivery.deliveryUnit" />
    public class deliveryUnitMainReport : deliveryUnit
    {
        public deliveryUnitMainReport() : base("Report deliveryInstance")
        {
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        public override void setup()
        {
            scriptFlags = docScriptFlags.allowFailedInstructions | docScriptFlags.disableGlobalCollection | docScriptFlags.enableLocalCollection | docScriptFlags.ignoreArgumentValueNull | docScriptFlags.ignoreCompilationFails;

            theme = stylePresets.themeSemantics;

            //String cssPath = "".t(templateFieldDeliveryUnit.del_themepath).add("simple\\simple.css", "\\");

            //deliveryUnitItemSupportFile standardCss = new deliveryUnitItemSupportFile(cssPath, "include\\"); // copies file and later includes the file
            //this.Add(standardCss);

            //this.Add(new deliveryUnitItemSupportFile("".t(templateFieldDeliveryUnit.del_themepath).add("simple\\bootstrap.css", "\\"));

            AddThemeSupportFile("simple\\simple.css");
            AddThemeSupportFile("simple\\bootstrap.css");

            //this.AddThemeSupportFile("strap\\strapdown.css");
            //this.AddThemeSupportFile("strap\\strapdown.js");
            //this.AddThemeSupportFile("strap\\strapdown-topbar.min.js");

            AddThemeSupportFiles("strapzeta", "*.css");
            AddThemeSupportFile("strapzeta\\bootstrap.min.js");

            AddThemeSupportFile("strapzeta\\strapdown.js").linkType = appendLinkType.scriptPostLink;

            deliveryUnitItemPaletteCSS palletteCss = new deliveryUnitItemPaletteCSS("standard\\standard.css", "include\\"); // templated palette css
            Add(palletteCss);

            //this.AddStandardHtmlItems();
            this.AddJSPluginSupport(jsPluginEnum.D3);           // copies js file
            this.AddJSPluginSupport(jsPluginEnum.JQuery);       // copies jquery

            var renderDirectory = new deliveryUnitDirectoryConstructor(reportElementLevel.documentSet, reportElementLevel.document); // creates directory for documentSets and document
            Add(renderDirectory);

            var renderOutput = new deliveryUnitItemSimpleRenderOutput(new builderForMarkdown(), reportOutputFormatName.textMdFile, renderDirectory.levels);
            renderOutput.levelOfNewFile = reportElementLevel.page;
            renderOutput.levelOfNewPage = reportElementLevel.none;
            renderOutput.levels.AddMultiple(reportElementLevel.documentSet, reportElementLevel.document, reportElementLevel.servicepage, reportElementLevel.page, reportElementLevel.block);

            Add(renderOutput);

            var renderOutputTemplate = new deliveryUnitItemContentTemplated("simple\\simple.html", renderOutput, reportOutputFormatName.htmlViaMD, renderDirectory.levels);
            renderOutputTemplate.filenameSufix = "";
            renderOutputTemplate.levels.AddMultiple(reportElementLevel.servicepage, reportElementLevel.page, reportElementLevel.block);
            Add(renderOutputTemplate);

            // renderOutput.attachIncludeItem(this.items);

            //var renderTableOutput = new deliveryUnitItemSimpleRenderOutput(new builderForTableDocument(), reportOutputFormatName.sheetExcel);
            //renderTableOutput.levels.Add(reportElementLevel.document, reportElementLevel.page, reportElementLevel.block);
            //this.Add(renderTableOutput);

            // this.Add();
        }
    }
}