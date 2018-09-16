// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataLogic.cs" company="imbVeles" >
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
namespace imbSCI.Core.process
{
    /// <summary>
    /// Logika sabiranja vrednosti
    /// </summary>
    public enum dataLogic
    {
        /// <summary>
        /// Osnovni: postavlja vrednost u polje
        /// </summary>
        set,

        /// <summary>
        /// Osnovni: postavlja vrednost u polje ako je postojeća vrednost Empty
        /// </summary>
        setIfEmpty,

        unset,
        skip,

        plus, // za list: od A ce prosto dodati sve elemente
        minus, // za list: od A ce oduzeti sve elemente iz B
        combine, // za List: rezultat ce imati samo unikatne iteme
        divide, // za list: presek dva skupa

        AND,
        OR, // alias za Divide i Combine, ali automatski konvertuju inpute u Booleane

        /// <summary>
        /// sabira sve vrednosti iz multiline listi - rezultat ima dužinu kraćeg od dva niza
        /// </summary>
        mlSumShort, //

        /// <summary>
        /// sabira sve vrednosti iz multiline listi - rezultat ima dužinu dužeg od dva niza
        /// </summary>
        mlSumLong, //

        /// <summary>
        /// Narediće imbDataSheet-u da napravi novi row
        /// </summary>
        newRow,
    }
}