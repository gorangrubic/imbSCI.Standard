// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureTrendSampleState.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.trends
{
    using System;

    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum measureTrendSampleState
    {
        /// <summary>
        /// Unknown state of the sampling
        /// </summary>
        none = 0,

        /// <summary>
        /// Not enough: still waits for enough samples to calculate (<see cref="microMean"/>) mean for micro period
        /// </summary>
        noEnough = 1,

        /// <summary>
        /// The micro mean: still waits for enough samples to calculate (<see cref="macroMean"/> mean for macro period
        /// </summary>
        microMean = 2,

        /// <summary>
        /// The macro mean: has enough samples to calculate both means
        /// </summary>
        macroMean = 4,

        /// <summary>
        /// The spear indicator (smaller period then micro) is ready
        /// </summary>
        spearMean = 8,
    }
}