// --------------------------------------------------------------------------------------------------------------------
// <copyright file="syntaxTokenType.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.codeSyntax
{
    /// <summary>
    /// Type of token content
    /// </summary>
    public enum syntaxTokenType
    {
        /// <summary>
        /// Prazan token
        /// </summary>
        empty,

        /// <summary>
        /// Nepoznata vrsta tokena
        /// </summary>
        unknown,

        /// <summary>
        /// Clean numeric value like> 5  5.43   -4.76
        /// </summary>
        numeric,

        /// <summary>
        /// Clean alfabet label like> e-mac  tool    Default
        /// </summary>
        alfabet,

        /// <summary>
        /// Numeric starting with key letter: b40 X500  X-25.34 ...
        /// </summary>
        keyNumberic,

        /// <summary>
        /// Mixed numeric and alphabet
        /// </summary>
        alfanumeric,
    }
}