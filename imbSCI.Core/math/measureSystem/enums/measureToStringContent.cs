// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureToStringContent.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.measureSystem.enums
{
    using System;

    [Flags]
    public enum measureToStringContent
    {
        value = 1,
        unit = 2,
        name = 4,
        roleLetter = 8,
        equalSign = 16,
        isWord = 32,
        roleName = 64,
        roleSymbol = 128,

        /// <summary>
        /// secondary value used in proportions, counters, indexers, exponents, logarithms and etc
        /// </summary>
        baseValue = 256,

        valueAndUnit = value | unit,

        /// <summary>
        /// The expression: L = 25.5 mm
        /// </summary>
        expression = roleLetter | equalSign | value | unit,

        sentence = name | isWord | value | name,
        fullSentence = name | roleLetter | isWord | roleSymbol | value | unit
    }
}