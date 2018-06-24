// --------------------------------------------------------------------------------------------------------------------
// <copyright file="appendTableFlag.cs" company="imbVeles" >
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
namespace imbSCI.Data.enums.appends
{
    using System;

    /// <summary>
    /// Flagovi - appendTableFlag
    /// </summary>
    [Flags]
    public enum appendTableFlag
    {
        none,

        /// <summary>
        /// da li dodaje ime tabele u ID tabele
        /// </summary>
        addTableNameToID = 2,

        /// <summary>
        /// da li dodaje ime tabele u naslov
        /// </summary>
        addTableNameAsTitle = 4,

        /// <summary>
        /// Header tekst ide u UPPER
        /// </summary>
        headerToUpper = 8,

        /// <summary>
        /// Ubacuje prvi red u tabeli koji sadr�i nazive kolona
        /// </summary>
        insertHeader = 16,

        /// <summary>
        /// Drugi red u tabeli sadr�i komentar -- u zavisnosti od implementacije
        /// </summary>
        insertSubHeader = 32,

        /// <summary>
        /// Prva kolona u tabeli sadr�i redni broj reda
        /// </summary>
        insertRowNumber = 64,

        minorOn5 = 128,

        majorOn5Minor = 256
    }
}