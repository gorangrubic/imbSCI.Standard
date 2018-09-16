// --------------------------------------------------------------------------------------------------------------------
// <copyright file="stringMatchPolicy.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.text
{
    /// <summary>
    /// Način upoređivanja stringova
    /// </summary>
    public enum stringMatchPolicy
    {
        /// <summary>
        /// Identična vrednost
        /// </summary>
        exact, // ==

        /// <summary>
        /// legacy
        /// </summary>
        standard, //

        /// <summary>
        ///  ignorise razlike mala/velika slova
        /// </summary>
        caseFree,

        /// <summary>
        /// brise razmake sa pocetka i kraja
        /// </summary>
        trimSpaceExact,

        /// <summary>
        ///  brise razmake sa pocetka i kraja, i case free
        /// </summary>
        trimSpaceCaseFree, //

        /// <summary>
        /// brise razmake sa pocetka i kraja, i contain proverava
        /// </summary>
        trimSpaceContainExact, //

        /// <summary>
        /// brise razmake sa pocetka i kraja, i contain case free proverava
        /// </summary>
        trimSpaceContainCaseFree, //

        /// <summary>
        /// A se nalazi u B
        /// </summary>
        containExact, //

        /// <summary>
        /// A se nalazi u B
        /// </summary>
        containCaseFree, //

        /// <summary>
        /// da li imaju iste duzine
        /// </summary>
        length, //

        lengthMore,
        lengthLess,

        /// <summary>
        /// Uvek vraca true
        /// </summary>
        overrideTrue,

        /// <summary>
        /// Uvek vraca false
        /// </summary>
        overrideFalse,

        /// <summary>
        /// needle index of == 0
        /// </summary>
        atBeginingCaseFree, //

        /// <summary>
        /// index of == length - needle.length
        /// </summary>
        atEndCaseFree, //

        /// <summary>
        /// Ignorise dosta znakovnih karaktera, razmak i case
        /// </summary>
        similar,

        /// <summary>
        /// Da li string A zadovoljava Regex Query iz stringa B
        /// </summary>
        regexQuery,
    }
}