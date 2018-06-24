// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaLogBuilderOutputPage.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting;
    using imbSCI.Reporting.interfaces;
    using imbSCI.Reporting.script;

    public class metaLogBuilderOutputPage : metaCustomizedSimplePage, IMetaComposeAndConstruct
    {
        /// <summary>
        ///
        /// </summary>
        public ILogBuilder logBuilder { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string instanceID { get; set; }

        public metaLogBuilderOutputPage(string __title, string __description, ILogBuilder __logBuilder, string __instanceID, string filename = "log_output") : base(__title, __description)
        {
            logBuilder = __logBuilder;
            name = filename;
            instanceID = __instanceID;
        }

        /// <summary>
        /// Composes the specified script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public override docScript compose(docScript script)
        {
            script.x_scopeIn(this);

            script.AppendHeading(pageTitle);
            script.Append("Log content from: ");
            script.AppendLabel(instanceID, true, "primary");
            script.AppendHorizontalLine();

            script.AppendDirect(logBuilder.ContentToString(true));

            script = base.compose(script);

            script.x_scopeOut(this);
            return script;
        }
    }
}