// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cursorSubzoneFrame.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.zone
{
    public enum cursorSubzoneFrame
    {
        /// <summary>
        /// The full frame - takes full size of source frame
        /// </summary>
        fullFrame,

        /// <summary>
        /// The first half of width
        /// </summary>
        h1,

        /// <summary>
        /// The second half of width
        /// </summary>
        h2,

        /// <summary>
        /// The first quorter of width
        /// </summary>
        q1,

        /// <summary>
        /// The 2nd quirter of width
        /// </summary>
        q2,

        /// <summary>
        /// The q3 quorter of width
        /// </summary>
        q3,

        /// <summary>
        /// The q4 quorter of width
        /// </summary>
        q4
    }
}