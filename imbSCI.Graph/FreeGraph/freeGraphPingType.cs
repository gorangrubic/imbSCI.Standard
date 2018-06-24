// --------------------------------------------------------------------------------------------------------------------
// <copyright file="freeGraphPingType.cs" company="imbVeles" >
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
    /// Type of graph ping operation. <see cref="freeGraphExtensions.PingGraphSize(freeGraph, freeGraphNodeBase, bool, freeGraphPingType, int)"/>
    /// </summary>
    public enum freeGraphPingType
    {
        /// <summary>
        /// The maximum ping length: performs ping operation for each <see cref="freeGraphNodeBase"/> separatly, and returns the highest ping length (number of cycles until number of pinged nodes becomes stable)
        /// </summary>
        maximumPingLength,

        /// <summary>
        /// The average ping length: performs ping operation for each <see cref="freeGraphNodeBase"/> separatly, and returns the average ping length (number of cycles until number of pinged nodes becomes stable)
        /// </summary>
        averagePingLength,

        /// <summary>
        /// The unison ping length: performs ping at once from all ping sources
        /// </summary>
        unisonPingLength,

        /// <summary>
        /// The number of pinged nodes: returns number of nodes reached by ping
        /// </summary>
        numberOfPingedNodes
    }
}