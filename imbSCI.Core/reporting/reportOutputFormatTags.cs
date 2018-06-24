// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportOutputFormatTags.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting
{
    #region imbVeles using

    using imbSCI.Core.reporting.format;
    using imbSCI.Data.enums.reporting;

    #endregion imbVeles using

    /// <summary>
    /// Definicija stila za izvestaj
    /// </summary>
    public class reportOutputFormatTags
    {
        public htmlTagName divTag = htmlTagName.none;
        public htmlTagName headTag = htmlTagName.none;
        public htmlTagName inlineBoldTag = htmlTagName.none;
        public htmlTagName inlineTag = htmlTagName.none;
        public htmlTagName lineTag = htmlTagName.none;
        public htmlTagName preTag = htmlTagName.none;

        public reportOutputFormatTags(reportOutputFormatName format = reportOutputFormatName.none)
        {
            if (format == reportOutputFormatName.none) format = reportOutputFormatName.htmlReport; //imbCoreApplicationSettings.ReportReportFormat;
            switch (format)
            {
                case reportOutputFormatName.textFile:

                    break;

                case reportOutputFormatName.htmlReport:
                    headTag = htmlTagName.h3;
                    lineTag = htmlTagName.p;
                    inlineTag = htmlTagName.span;
                    inlineBoldTag = htmlTagName.span;
                    preTag = htmlTagName.pre;
                    divTag = htmlTagName.div;
                    break;
            }
        }
    }
}