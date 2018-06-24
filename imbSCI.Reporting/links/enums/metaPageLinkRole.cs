// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaPageLinkRole.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.links.enums
{
    /// <summary>
    /// Uloga koju ima stranica
    /// </summary>
    /// \ingroup_disabled docElementCore
    public enum metaPageLinkRole
    {
        /// <summary>
        /// The page is actually subpage of this page - it opens inside scrollviewer/in the same sheet/PDF
        /// </summary>
        linkedView,

        /// <summary>
        /// The page is among main links od this page, but it is not sub page
        /// </summary>
        linkedPage,

        /// <summary>
        /// The content is created and saved next to page - but it is not linked to index and other pages
        /// </summary>
        unlinkedPage,

        /// <summary>
        /// Link of external url
        /// </summary>
        externalPage,

        /// <summary>
        /// The page has documentation role (API generation etc)
        /// </summary>
        documentation,

        /// <summary>
        /// Programming help and reference
        /// </summary>
        help,
    }
}