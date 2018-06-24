// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphNode.cs" company="imbVeles" >
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
using System.Collections.Generic;

namespace imbSCI.Data.collection.graph
{
    using imbSCI.Data.interfaces;
    using System.Collections;

    /// <summary>
    /// Interface for <see cref="graphNode"/> and <see cref="graphWrapNode{TItem}"/>
    /// </summary>
    /// <seealso cref="System.Collections.IEnumerable" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithParent" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithPath" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithName" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithPathAndChildren" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithTreeView" />
    public interface IGraphNode : IEnumerable, IObjectWithParent, IObjectWithPath, IObjectWithName, IObjectWithPathAndChildren, IObjectWithTreeView
    {
        Boolean ContainsKey(String key);

        /// <summary>
        /// Gets or sets the <see cref="IGraphNode"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="IGraphNode"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        IGraphNode this[String key] { get; set; }

        /// <summary>
        /// Removes child matching the specified key, on no match returns <c>false</c>
        /// </summary>
        /// <param name="key">The key to match children against</param>
        /// <returns>True if a child removed, false if no child matched by the key</returns>
        Boolean RemoveByKey(String key);

        /// <summary>
        /// Removes all children with matching <see cref="graphNode.name"/>
        /// </summary>
        /// <param name="keys">The keys to match children with</param>
        /// <returns>Number of child nodes matched and removed</returns>
        Int32 Remove(IEnumerable<String> keys);

        /// <summary>
        /// Adds the specified <c>newChild</c>, if its name is not already occupied
        /// </summary>
        /// <param name="newChild">The new child.</param>
        /// <returns></returns>
        Boolean Add(IGraphNode newChild);

        /// <summary>
        /// Gets the depth level, where 1 is the root
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        Int32 level { get; }

        /// <summary>
        /// Name format used for textual tree view representation
        /// </summary>
        /// <value>
        /// For treeview.
        /// </value>
        String forTreeview { get; }

        /// <summary>
        /// Referenca prema parent objektu
        /// </summary>
        IGraphNode parent { get; set; }

        /// <summary>
        /// Ime koje je dodeljeno objektu
        /// </summary>
        String name { get; set; }

        /// <summary>
        /// Gets the path separator used in this path format
        /// </summary>
        /// <value>
        /// The path separator.
        /// </value>
        string pathSeparator { get; }

        /// <summary>
        /// Putanja objekta
        /// </summary>
        string path { get; }

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        object root { get; }

        /// <summary>
        /// Gets the child names.
        /// </summary>
        /// <returns></returns>
        List<String> getChildNames();

        /// <summary>
        /// Gets the first.
        /// </summary>
        /// <returns></returns>
        IGraphNode getFirst();

        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <returns></returns>
        Int32 Count();

        /// <summary>
        /// Gets a value indicating whether this instance is leaf.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is leaf; otherwise, <c>false</c>.
        /// </value>
        Boolean isLeaf { get; }

        IEnumerator<IObjectWithPathAndChildren> GetEnumerator();
    }
}