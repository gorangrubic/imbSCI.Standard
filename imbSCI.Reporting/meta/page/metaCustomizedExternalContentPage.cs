// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaCustomizedExternalContentPage.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.io;
    using imbSCI.Data;
    using imbSCI.Reporting.interfaces;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbSCI.Reporting.meta.page.metaCustomizedSimplePage" />
    /// <seealso cref="imbSCI.Reporting.interfaces.IMetaComposeAndConstruct" />
    public class metaCustomizedExternalContentPage : metaCustomizedSimplePage, IMetaComposeAndConstruct
    {
        /// <summary>
        ///
        /// </summary>
        public string introContentPath { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string outroContentPath { get; set; }

        public metaCustomizedExternalContentPage(string __title, string __description, string __introContentPath, string __outroContentPath = "") : base(__title, __description)
        {
            name = __title.getFilename();
            introContentPath = __introContentPath;
            outroContentPath = __outroContentPath;
        }

        public override void construct(object[] resources)
        {
            AddExternalContent(introContentPath, "", "");

            if (!outroContentPath.isNullOrEmpty()) AddExternalContent(outroContentPath, "", "");

            base.construct(resources);
        }
    }
}