// --------------------------------------------------------------------------------------------------------------------
// <copyright file="graphNode.cs" company="imbVeles" >
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

// using imbSCI.Core.interfaces;
// using aceCommonTypes.extensions.io;
// using aceCommonTypes.extensions.text;
// using aceCommonTypes.interfaces;
// using aceCommonTypes.extensions;

namespace imbSCI.Data.collection.graph
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Universal T-Graph structure with nodes having unique <see cref="name"/> property. To be used without <see cref="graph{TItem}"/> class.
    /// </summary>
    /// <seealso cref="System.Collections.IEnumerable" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{aceCommonTypes.collection.nested.graphNode}" />
    [Serializable]
    public class graphNode : graphNodeBase, IEnumerable<IGraphNode>, IGraphNode
    {
        protected override IDictionary children
        {
            get
            {
                return mychildren;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IGraphNode"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="IGraphNode"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public override IGraphNode this[String key]
        {
            get
            {
                return children[key] as IGraphNode;
            }
            set
            {
                if (children.Contains(key))
                {
                    children.Remove(key);
                    value.parent = value;
                    children[key] = value as IGraphNode;
                }
                else
                {
                    Add(value);
                }

                //if (children.ContainsKey(key))
                //{
                //}
                //else
                //{
                //    Add(value);
                //}
            }
        }

        protected SortedList<String, IGraphNode> mychildren { get; set; } = new SortedList<string, IGraphNode>();

        //private ConcurrentDictionary<String, IGraphNode> _children = new ConcurrentDictionary<String, IGraphNode>();
        ///// <summary>
        ///// Gets or sets the children.
        ///// </summary>
        ///// <value>
        ///// The children.
        ///// </value>
        //protected ConcurrentDictionary<String, IGraphNode> mychildren
        //{
        //    get
        //    {
        //        return _children;
        //    }
        //    set { _children = value; }
        //}

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<IGraphNode> IEnumerable<IGraphNode>.GetEnumerator()
        {
            return (IEnumerator<IGraphNode>)children.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return children.Values.GetEnumerator();
        }

        /// <summary>
        /// Adds the specified <c>newChild</c>, if its name is not already occupied
        /// </summary>
        /// <param name="newChild">The new child.</param>
        /// <returns></returns>
        public override bool Add(IGraphNode newChild)
        {
            if (children.Contains(newChild.name))
            {
                return false;
            }
            else
            {
                IGraphNode gn = newChild as IGraphNode;
                gn.parent = this;
                children.Add(gn.name, gn);
                return true;
            }
        }

        /// <summary>
        /// Name format used for textual tree view representation
        /// </summary>
        /// <value>
        /// For treeview.
        /// </value>
        public virtual String forTreeview
        {
            get
            {
                return name;
            }
        }
    }
}