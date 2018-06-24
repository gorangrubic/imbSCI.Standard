// --------------------------------------------------------------------------------------------------------------------
// <copyright file="numberRangeModifyRule.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.range
{
    /// <summary>
    /// Na koji način menjam boju
    /// </summary>
    public enum numberRangeModifyRule
    {
        /// <summary>
        /// Dodaje prosledjene vrednosti i ako dođu do maksimuma onda ih ostavi na maksimumu (ne važi za HUE)
        /// </summary>
        clipToMax,

        /// <summary>
        /// Ako dođu do maksimuma vrati ih na početak
        /// </summary>
        loop,

        /// <summary>
        /// Postavlja prosleđene vrednosti, bez obzira na trenutne vrednosti
        /// </summary>
        set,

        /// <summary>
        /// Prosleđuje staru vrednost
        /// </summary>
        bypass,

        /// <summary>
        /// Množi sa delta
        /// </summary>
        multiply,

        /// <summary>
        /// Deli sa delta
        /// </summary>
        divide,

        /// <summary>
        /// Returns value from limit for amount it was passing the limit
        /// </summary>
        bounce,
    }
}