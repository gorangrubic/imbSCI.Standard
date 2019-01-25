// --------------------------------------------------------------------------------------------------------------------
// <copyright file="htmlTagName.cs" company="imbVeles" >
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
using System;

namespace imbSCI.Data.enums.reporting
{
    [Flags]
    public enum htmlTagName
    {
        none = 0,
        div = 1,
        p = 2,
        span = 4,
        ul = 8,
        li = 16,
        dt = 32,
        dd = 64,
        dl = 128,
        ol = 256,
        title = 512,
        meta = 1024,
        h1 = 2048,
        h2 = 4096,
        h3 = 8192,
        b = 16384,
        pre = 32768,
        a = 65536,
        img = 131072,
        table,
        td,
        tr,
        tbody,
        html,
        body,
        link,

        /// <summary>
        /// Table header cell
        /// </summary>
        th,

        /// <summary>
        /// Podrazumevani tag za konkretan tip izvestaja
        /// </summary>
        defaultTag,

        head,

        label,
        article,
        footer,
        section,
        header
    }
}