// --------------------------------------------------------------------------------------------------------------------
// <copyright file="templateFieldCompanyProfile.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data.enums.fields
{
    /// <summary>
    /// set of high-level data points -- not related to object property name
    /// </summary>
    public enum templateFieldCompanyProfile
    {
        /// <summary>
        /// Detected company name - data
        /// </summary>
        csp_companyName,

        /// <summary>
        /// Complete address block - or several blocks if detected
        /// </summary>
        csp_address,

        /// <summary>
        /// Phone number or numbers
        /// </summary>
        csp_phone,

        /// <summary>
        /// VAT number
        /// </summary>
        csp_pib,

        /// <summary>
        /// Number of registration
        /// </summary>
        csp_mbr,

        /// <summary>
        /// Product or products  - in line CSV
        /// </summary>
        csp_product,

        /// <summary>
        /// Servies
        /// </summary>
        csp_services,

        /// <summary>
        /// Detected equipment
        /// </summary>
        csp_equipment,

        /// <summary>
        /// Detected personal contact
        /// </summary>
        csp_staff,

        /// <summary>
        /// Detected events
        /// </summary>
        csp_events,
    }
}