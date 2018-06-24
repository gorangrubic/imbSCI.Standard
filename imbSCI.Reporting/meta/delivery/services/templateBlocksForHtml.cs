// --------------------------------------------------------------------------------------------------------------------
// <copyright file="templateBlocksForHtml.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.delivery.services
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.links;
    using imbSCI.Reporting.meta.delivery.items;
    using System.Data;
    using System.Linq;

    /// <summary>
    ///
    /// </summary>
    public class templateBlocksForHtml
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="templateBlocksForHtml"/> class.
        /// </summary>
        public templateBlocksForHtml()
        {
            outputRender = new builderForMarkdown();
        }

        /// <summary>
        ///
        /// </summary>
        public ITextRender outputRender { get; set; }

        public PropertyCollection BuildDynamicNavigationTemplates(deliveryInstance context, PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();

            string reldir = context.directoryScope.FullName.removeStartsWith(context.directoryRoot.FullName);
            string selPath = reldir; // context.scope.path;

            IMetaContentNested mc = context.scope as IMetaContentNested;
            if (mc != null)
            {
                selPath = mc.path;
            }

            reportLinkCollection directory = context.linkRegistry.getLinkOneCollection(selPath, data.getProperString(templateFieldBasic.path_folder, templateFieldBasic.document_path, templateFieldBasic.documentset_path));

            string str_localdirectory = "";

            if (directory != null)
            {
                str_localdirectory = directory.makeHtmlInsert();
            }
            data.add(reportOutputDomainEnum.localdirectory, str_localdirectory);

            return data;
        }

        public PropertyCollection BuildNavigationTemplates(deliveryInstance context, PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();
            reportLinkCollection logs = context.linkRegistry[reportOutputDomainEnum.logs.ToString()];
            if (logs.Any())
            {
                string str_logs = data.getProperString("", reportOutputDomainEnum.logs);

                if (imbSciStringExtensions.isNullOrEmpty(str_logs))
                {
                    str_logs = logs.makeHtmlInsert();
                    data.add(reportOutputDomainEnum.logs, str_logs);
                }
            }
            return data;
        }

        /// <summary>
        /// Builds the include template.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public PropertyCollection BuildIncludeTemplate(deliveryInstance context, PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();

            var includeItems = context.unit.includeItems;

            string includePath = "".t(templateFieldBasic.root_relpath);

            //outputRender.SubcontentStart(templateFieldSubcontent.head_includes, false);

            //String relJump = context.directoryScope.getRelativePathToParent(context.directoryRoot);

            outputRender.Clear();

            string url = "";

            foreach (IDeliverySupportFile inc in includeItems[appendLinkType.styleLink])
            {
                url = inc.getRelativeUrl(null, includePath);

                outputRender.AppendLink(url, inc.name, "", appendLinkType.styleLink);
            }

            foreach (IDeliverySupportFile inc in includeItems[appendLinkType.scriptLink])
            {
                url = inc.getRelativeUrl(null, includePath);

                outputRender.AppendLink(url, inc.name, "", appendLinkType.scriptLink);
            }

            data[templateFieldSubcontent.head_includes] = outputRender.ContentToString(true, reportOutputFormatName.textFile);

            outputRender.Clear();

            foreach (IDeliverySupportFile inc in includeItems[appendLinkType.scriptPostLink])
            {
                url = inc.getRelativeUrl(null, includePath);

                outputRender.AppendLink(url, inc.name, "", appendLinkType.scriptLink);
            }

            data[templateFieldSubcontent.bottom_includes] = outputRender.ContentToString(true, reportOutputFormatName.textFile);

            data = BuildNavigationTemplates(context, data);

            return data;
        }
    }
}