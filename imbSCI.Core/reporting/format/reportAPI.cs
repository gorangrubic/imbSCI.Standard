// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportAPI.cs" company="imbVeles" >
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
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Core.reporting.format
{
    /// <summary>
    /// Reporting API enumeration
    /// </summary>
    public enum reportAPI
    {
        /// <summary>
        /// The imb reporting - API for <see cref="imbSCI.Core.Builders.render.builders.builderForFlowDocument"/>
        /// </summary>
        imbReporting,

        /// <summary>
        /// The text builder - API for <see cref="builderForText"/>, <see cref="imbACE.Core.core.builderForLog"/>,
        /// <see cref="builderForMarkdown"/>, <see cref="builderForHtml"/>
        /// </summary>
        textBuilder,

        /// <summary>
        /// <see cref="imbSCI.Core.Builders.render.builders.builderForTableDocument"/>
        /// </summary>
        EEPlus,

        imbXmlHtml,
        imbMarkdown,
        imbFlowDocument,
        imbSerialization,
        imbDiagnostics,
    }
}