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
namespace imbSCI.Data.enums.reporting
{
    public enum htmlTagName
    {
        none,
        div,
        p,
        span,
        ul,
        li,
        dt,
        dd,
        dl,
        ol,
        title,
        meta,
        h1,
        h2,
        h3,
        b,
        pre,
        a,
        img,
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