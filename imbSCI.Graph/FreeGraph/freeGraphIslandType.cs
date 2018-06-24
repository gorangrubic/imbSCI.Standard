// --------------------------------------------------------------------------------------------------------------------
// <copyright file="freeGraphIslandType.cs" company="imbVeles" >
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

namespace imbSCI.Graph.FreeGraph
{
    /// <summary>
    /// Type of <see cref="freeGraphIsland"/> by number of <see cref="freeGraphNodeBase"/> contained
    /// </summary>
    public enum freeGraphIslandType
    {
        /// <summary>
        /// Island is actually non island, since it has zero nodes
        /// </summary>
        none,

        /// <summary>
        /// The mononodal: has only one node
        /// </summary>
        mononodal,

        /// <summary>
        /// The binodal: has two nodes
        /// </summary>
        binodal,

        /// <summary>
        /// The trinodal: has three nodes
        /// </summary>
        trinodal,

        /// <summary>
        /// The polinodal: has more then three nodes
        /// </summary>
        polinodal,

        /// <summary>
        /// The unknown: this indicates some problem, as you should never see an island taggd as <see cref="unknown"/>
        /// </summary>
        unknown
    }
}