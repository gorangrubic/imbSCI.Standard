// --------------------------------------------------------------------------------------------------------------------
// <copyright file="existingDataMode.cs" company="imbVeles" >
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
namespace imbSCI.Core.enums
{
    /// <summary>
    /// Rule how to handle existing data when append new data collection or single entry
    /// </summary>
    public enum existingDataMode
    {
        /// <summary>
        /// it will overwrite any existing data in
        /// </summary>
        overwriteExisting,

        /// <summary>
        /// The overwrites existing only if existing value is null or empty string
        /// </summary>
        overwriteExistingIfEmptyOrNull,

        /// <summary>
        /// it will leave existing entries unchanged
        /// </summary>
        leaveExisting,

        /// <summary>
        /// it will clear  any (other) existing entry or entries and place only specified (one) or (collection). WARNING: beware that in single entry operation it will clear ALL existing data and place only the one specified
        /// </summary>
        clearExisting,

        /// <summary>
        /// it will remove all existing entries that are not matched by new collection
        /// </summary>
        filterAndLeaveExisting,

        /// <summary>
        /// result is key-crossection of existing and new collection - values are from new
        /// </summary>
        filterNewOverwriteExisting,

        /// <summary>
        /// result will be sum of existing and new values
        /// </summary>
        sumWithExisting,
    }
}