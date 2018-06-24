// --------------------------------------------------------------------------------------------------------------------
// <copyright file="samplingOrderEnum.cs" company="imbVeles" >
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

namespace imbSCI.Data.data.sample
{
    /// <summary>
    /// Sample take order
    /// </summary>
    public enum samplingOrderEnum
    {
        /// <summary>
        /// The ordinal: from <c>skip</c> to <c>skip</c> + <c>limit</c>
        /// </summary>
        ordinal,

        /// <summary>
        /// The every NTH: ordinal, but every n-th element, including cycling,
        /// </summary>
        everyNth,

        /// <summary>
        /// The random suffle: random suffle
        /// </summary>
        randomSuffle
    }
}