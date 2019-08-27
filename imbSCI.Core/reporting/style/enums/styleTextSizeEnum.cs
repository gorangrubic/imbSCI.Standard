// --------------------------------------------------------------------------------------------------------------------
// <copyright file="styleTextSizeEnum.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.style.enums
{
#pragma warning disable CS1574 // XML comment has cref attribute 'styleTextSizeSet' that could not be resolved
    /// <summary>
    /// Enumeration to index position of <c>headingSizes</c> list at <c>styleTextSizeSet</c>
    /// </summary>
    /// <see cref="styleTextSizeSet"/>
    /// \ingroup_disabled report_ll_style
    public enum styleTextSizeEnum
#pragma warning restore CS1574 // XML comment has cref attribute 'styleTextSizeSet' that could not be resolved
    {
        h1 = 0,
        h2 = 1,
        h3 = 2,
        h4 = 3,
        h5 = 4,
        h6 = 5,
        regular = 6,
        smaller = 7
    }
}