// --------------------------------------------------------------------------------------------------------------------
// <copyright file="graphNodeCustom.cs" company="imbVeles" >
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
    using System.Collections;

    /// <summary>
    /// Graph structure to inherit for custom graph node type
    /// </summary>
    /// <seealso cref="imbSCI.Data.collection.graph.graphNodeBase" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{imbSCI.Data.collection.graph.IGraphNode}" />
    /// <seealso cref="imbSCI.Data.collection.graph.IGraphNode" />
    [Serializable]
    public abstract class graphNodeCustom : graphNodeBase, IEnumerable<IGraphNode>, IGraphNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="graphNodeCustom"/> class.
        /// </summary>
        protected graphNodeCustom() : base()
        {
            if (doAutonameFromTypeName)
            {
                name = makeAutoName();
            }
        }

        /// <summary>
        /// Turns on the autorenaming, when new child is inserted into this node - it will autorename it to have unique name
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do autorename on existing]; otherwise, <c>false</c>.
        /// </value>
        protected abstract Boolean doAutorenameOnExisting { get; }

        /// <summary>
        /// Turns on autonaming, based on type name of this node
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do autoname from type name]; otherwise, <c>false</c>.
        /// </value>
        protected abstract Boolean doAutonameFromTypeName { get; }

        IEnumerator<IGraphNode> IEnumerable<IGraphNode>.GetEnumerator()
        {
            return (IEnumerator<IGraphNode>)children.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return children.Values.GetEnumerator();
        }

        /// <summary>
        /// Creates new child item and sets the name, but still do not add it. Used internally by <see cref="Add(String pathWithName)"/>
        /// </summary>
        /// <param name="nameForChild">The name for child.</param>
        /// <returns></returns>
        public virtual graphNodeCustom CreateChildItem(String nameForChild)
        {
            Type t = GetType();

            graphNodeCustom newChild = Activator.CreateInstance(t, new object[] { }) as graphNodeCustom;
            newChild.name = nameForChild;
            return newChild;
        }

        /// <summary>
        /// Adds the specified path with name.
        /// </summary>
        /// <param name="pathWithName">Name of the path with.</param>
        /// <returns></returns>
        public virtual graphNodeCustom Add(String pathWithName)
        {
            List<String> pathParts = pathWithName.SplitSmart(pathSeparator);
            graphNodeCustom head = this;

            foreach (String part in pathParts)
            {
                head = head.CreateChildItem(part);
                Add(head);
                //head = head.Add(part);
            }

            return head;
        }

        /// <summary>
        /// Makes the name of the automatic.
        /// </summary>
        /// <returns></returns>
        internal String makeAutoName()
        {
            var n = "";
            n = GetType().Name;
            n = n.Replace("Node", "");
            n = n.Replace("Graph", "");

            return n;
        }

        private const Int32 AUTORENAME_LIMIT = 99999;

        private Object AddChildLock = new Object();

        /// <summary>
        /// Adds the specified <c>newChild</c>, if its name is not already occupied
        /// </summary>
        /// <param name="newChild">The new child.</param>
        /// <returns></returns>
        public override bool Add(IGraphNode newChild)
        {
            lock (AddChildLock)
            {
                if (doAutonameFromTypeName)
                {
                    if (newChild.name.isNullOrEmpty())
                    {
                        newChild.name = makeAutoName();
                    }
                }

                if (children.Contains(newChild.name))
                {
                    if (doAutorenameOnExisting)
                    {
                        newChild.name = this.MakeUniqueChildName(newChild.name, AUTORENAME_LIMIT, children.Count);
                    }
                    else
                    {
                        return false;
                    }
                }

                IGraphNode gn = newChild as IGraphNode;
                gn.parent = this;

                children.Add(gn.name, gn);
            }
            return true;
        }

        /// <summary>
        /// Returns the index of this node at its parent node. If no parent: returns -1
        /// </summary>
        /// <returns></returns>
        public Int32 GetIndex()
        {
            if (parent != null)
            {
                graphNodeCustom cParent = (graphNodeCustom)parent;

                return cParent.mychildren.IndexOfValue(this);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets the sibling, relative to this node, positioned at <c>n</c> places defined by <c>direction</c>. It is range-safe, in sense: if node index + <c>direction</c> is higher then number of siblings, it will return the last sibling.
        /// </summary>
        /// <remarks>
        /// <para>If there is no parent: it will return <c>null</c></para>
        /// <para>If direction + index is less then zero, it will return sibling at zero</para>
        /// </remarks>
        /// <param name="direction">The direction, relative to index of this node. If it is 0, it will return this node</param>
        /// <returns>Sibling node, relative to index of this and <c>direction</c> specified</returns>
        public IGraphNode GetSibling(Int32 direction = 1)
        {
            if (direction == 0) return this;

            if (parent == null) return null;

            Int32 index = GetIndex();

            if (index == -1) return null;

            index += direction;

            if (index < 0) index = 0;

            if (index >= parent.Count())
            {
                index = parent.Count() - 1;
            }

            var cParent = parent as graphNodeCustom;

            return cParent[index];
        }

        /// <summary>
        /// Returns node by ordinal index
        /// </summary>
        /// <value>
        /// The <see cref="IGraphNode"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public IGraphNode this[Int32 index]
        {
            get
            {
                if (index < 0) return null;

                if (mychildren.Count > index)
                {
                    String key = mychildren.Keys[index];
                    return mychildren[key];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Accessing the child nodes using child node name
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
                if (mychildren.ContainsKey(key))
                {
                    return mychildren[key];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (mychildren.ContainsKey(key))
                {
                    value.parent = this;
                    mychildren[key] = value;
                }
                else
                {
                    Add(value);
                }
            }
        }

        protected override IDictionary children
        {
            get
            {
                return mychildren;
            }
        }

        protected SortedList<String, IGraphNode> mychildren { get; set; } = new SortedList<string, IGraphNode>();

        /// <summary>
        /// String representetation of the node
        /// </summary>
        /// <value>
        /// For treeview.
        /// </value>
        public virtual string forTreeview
        {
            get
            {
                return name;
            }
        }
    }
}