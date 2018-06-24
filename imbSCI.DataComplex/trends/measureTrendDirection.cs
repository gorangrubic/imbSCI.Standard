// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureTrendDirection.cs" company="imbVeles" >
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
    /// Semantic note on current trend of the measure
    /// </summary>
    [Flags]
    public enum measureTrendDirection
    {
        /// <summary>
        /// Not defined - unknown, disabled or still waiting to accumulate enough sample takes
        /// </summary>
        none = 0,

        /// <summary>
        /// Has enough sample takes to calculate average/mean
        /// </summary>
        ready = 1,

        /// <summary>
        /// The macro up: macro period shows positive change
        /// </summary>
        macroUp = 2,

        /// <summary>
        /// The micro up: micro period shows positive change
        /// </summary>
        microUp = 4,

        /// <summary>
        /// The macro down: macro period shows negative change (
        /// </summary>
        macroDown = 8,

        /// <summary>
        /// The micro down: micro period shows negative change
        /// </summary>
        microDown = 16,

        /// <summary>
        /// The macro stable: macro period stays within trend margin
        /// </summary>
        macroStable = 32,

        /// <summary>
        /// The micro stable: micro period stays within trend margin
        /// </summary>
        microStable = 64,

        /// <summary>
        /// The double stable very stable
        /// </summary>
        doubleStable = macroStable | microStable,

        /// <summary>
        /// The stable: very stable - alias to <see cref="doubleStable"/>
        /// </summary>
        stable = macroStable | microStable,

        /// <summary>
        /// The double up: Macro and Micro Trends are positive - stable increase
        /// </summary>
        doubleUp = macroUp | microUp,

        /// <summary>
        /// Up: started with incline recently
        /// </summary>
        up = macroStable | microUp,

        /// <summary>
        /// Up down: sudden surge of the value
        /// </summary>
        upDown = macroUp | microDown,

        /// <summary>
        /// Down: started with decline in value
        /// </summary>
        down = macroStable | microDown,

        /// <summary>
        /// The double down: stable decrease of the value
        /// </summary>
        doubleDown = macroDown | microDown,

        /// <summary>
        /// Down up: sudden surge, after local minimum
        /// </summary>
        downUp = macroDown | microUp,
    }
}