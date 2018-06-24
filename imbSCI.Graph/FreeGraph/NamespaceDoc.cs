// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NamespaceDoc.cs" company="imbVeles" >
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
    /// <para>Undirected Graph where nodes can connect to each other without restriction</para>
    /// </summary>
    /// <remarks>
    /// <para>The graph is built from <see cref="freeGraphNodeBase"/> and <see cref="freeGraphLinkBase"/> that have weights (<see cref="freeGraphLinkBase.weight"/>) and types (<see cref="freeGraphNodeBase.type"/>) assigned.</para>
    /// <para><see cref="FreeGraph.freeGraphReport"/> provides nice structural analysis of the graph, and detects the <see cref="FreeGraph.freeGraphIsland"/>s within a graph</para>
    /// <list type="bullet">
    /// 	<listheader>
    ///			<term>When to use <see cref="freeGraph"/>?</term>
    ///		</listheader>
    ///		<item>
    ///			<term>Expansion queries</term>
    ///			<description>When you want to select other nodes by expansion from selected nodes</description>
    ///		</item>
    ///		<item>
    ///			<term>Named Tweens</term>
    ///			<description>When you're not inheriting <see cref="freeGraphNodeBase"/> but using the <see cref="freeGraph"/> as tween structure representation, where <see cref="freeGraphNodeBase.name"/> is suitable link between your data objects and the graph</description>
    ///		</item>
    /// </list>
    /// </remarks>
    /// <seealso cref="imbSCI.Graph.Converters.GraphConversionTools" />
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class NamespaceDoc
    {
    }
}