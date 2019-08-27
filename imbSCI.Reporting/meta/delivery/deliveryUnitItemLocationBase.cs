// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitItemLocationBase.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.delivery
{
    /// <summary>
    /// Targeted location of the unit item
    /// </summary>
    public enum deliveryUnitItemLocationBase
    {
        /// <summary>
        /// The location of item is unset
        /// </summary>
        unknown,

#pragma warning disable CS1574 // XML comment has cref attribute 'directoryScope' that could not be resolved
        /// <summary>
        /// The local resource - item will be saved into current scope <see cref="imbSCI.Reporting.reporting.render.IRenderExecutionContext.directoryScope"/>
        /// </summary>
        localResource,
#pragma warning restore CS1574 // XML comment has cref attribute 'directoryScope' that could not be resolved

        /// <summary>
        /// It is a resource shared within the current document (closest parent to the current logical scope)
        /// </summary>
        globalDocumentResource,

        /// <summary>
        /// It is a resource shared within the document set (closest parent to the current logical scope)
        /// </summary>
        globalDocumentSetResource,

        /// <summary>
        /// It is a resources shared within the complete deliveryInstance output
        /// </summary>
        globalDeliveryResource,

        /// <summary>
        /// The item is actually an URI retrieved via http request
        /// </summary>
        externalWebResource,

        globalDeliveryContent,
    }
}