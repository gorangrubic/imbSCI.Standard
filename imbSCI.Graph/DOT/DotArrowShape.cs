// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DotArrowShape.cs" company="imbVeles" >
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
// Project: imbSCI.Graph
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

/* Namespace: imbSCI.Graph.DOT is adaptation of the DotNetGraph project:
 * Original Licence notice / header from the project:
 * -----------------------------------------------------------------------
 * Copyright (c) 2017 DotNetGraph https://github.com/vfrz/DotNetGraph
 *  This file is part of DotNetGraph.
 *
 *     DotNetGraph is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 *
 *     DotNetGraph is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU General Public License for more details.
 *
 *     You should have received a copy of the GNU General Public License
 *     along with DotNetGraph.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace imbSCI.Graph.DOT
{
    /// <summary>
    /// Shape of the arrow
    /// </summary>
    public enum DotArrowShape
    {
        Normal,
        Inv,
        Dot,
        Invdot,
        Odot,
        Invodot,
        None,
        Tee,
        Empty,
        Invempty,
        Diamond,
        ODiamond,
        Ediamond,
        Crow,
        Box,
        Obox,
        Open,
        Halfopen,
        Vee,
        Circle
    }
}