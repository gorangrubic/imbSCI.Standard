// --------------------------------------------------------------------------------------------------------------------
// <copyright file="specialSubMenuType.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.path
{
    public enum specialSubMenuType
    {
        unknown,

        ///// <summary>
        ///// Grane svih dostupnih modula
        ///// </summary>
        //moduleTree,

        ///// <summary>
        ///// Grane svih dostupnih aktivnosti
        ///// </summary>
        //activityTree,

        /// <summary>
        /// Bilo koji tip, otvara pod menu
        /// </summary>
        subOperationMenuForType,

        ///// <summary>
        ///// Odabir tabele unutar jedne baze podataka
        ///// </summary>
        databaseTableMenu,

        dynamicTree,

        dynamicList,

        instanceTree,

        //resourceEmbedded,

        //resourceLinks,

        //resourceProperties,

        /// <summary>
        /// Pod resurse -- prema path filterima
        /// </summary>
        resourceAllRelated,

        /// <summary>
        /// Toggle menu za sve public Boolean propertije koji imaju DisplayName
        /// </summary>
        primitiveSwitches,

        /// <summary>
        /// Toggle menu za sve public propertije koji su enumeracije
        /// </summary>
        primitiveEnums,

        //primitiveStrings,

        //primitiveIntegers,

        /// <summary>
        /// Koristi preset iteme sa preset liste - povezati sa specialSubMenuParameter
        /// </summary>
        // stringPreset,

        ///// <summary>
        ///// Koristi vise preset listi - povezati sa specialSubMenuParameter
        ///// </summary>
        //stringPresets,
    }
}