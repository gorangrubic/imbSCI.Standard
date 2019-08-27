// --------------------------------------------------------------------------------------------------------------------
// <copyright file="graphNodeBase.cs" company="imbVeles" >
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
    /// Base class for custom <see cref="graphNode"/> implementation
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{imbSCI.Data.collection.graph.IGraphNode}" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithParent" />
    [Serializable]
    public abstract class graphNodeBase : IObjectWithParent, IEnumerable<IGraphNode>, IObjectWithUID
    {
        public const String defaultPathSeparator = "\\";

        // private ConcurrentDictionary<String, IGraphNode> _children = new ConcurrentDictionary<String, IGraphNode>();
        private IGraphNode _parent;

        private String _name;
        protected String _pathSeparator = "";

        /// <summary>
        /// Adds the specified child.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <returns></returns>
        public abstract Boolean Add(IGraphNode child);

        /// <summary>
        /// Gets a value indicating whether this instance is leaf.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is leaf; otherwise, <c>false</c>.
        /// </value>
        public Boolean isLeaf { get { return (children.Count == 0); } }

        /// <summary>
        /// Gets or sets the <see cref="IGraphNode"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="IGraphNode"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public abstract IGraphNode this[String key] { get; set; }

        /// <summary>
        /// Gets the depth level, where 1 is the root
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public Int32 level
        {
            get
            {
                Int32 output = 1;
                if (parent != null) return parent.level + output;
                return output;
            }
        }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        protected abstract IDictionary children { get; }

        /// <summary>
        /// Referenca prema parent objektu
        /// </summary>
        public virtual IGraphNode parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        /// <summary>
        /// Ime koje je dodeljeno objektu
        /// </summary>
        public virtual String name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public Boolean ContainsKey(String key)
        {
            return children.Contains(key);
        }

        /// <summary>
        /// Gets the path separator used in this path format - if its not set it will look for parent's default path separator to set it. If there is no parent, it will use <see cref="defaultPathSeparator"/>
        /// </summary>
        /// <value>
        /// The path separator.
        /// </value>
        public virtual string pathSeparator
        {
            get
            {
                var head = this;
                if (head.parent != null)
                {
                    head = root as graphNodeBase;
                } 

                if (head._pathSeparator != "") return head._pathSeparator;

                return defaultPathSeparator;
                
                //if (parent != null)
                //{
                //    //graphNode graphParent = (graphNode)parent;
                //    _pathSeparator = parent.pathSeparator;
                //    return _pathSeparator;
                //}


            }
            set
            {
                var head = this;
                if (head.parent != null)
                {
                    head = root as graphNodeBase;
                } 

                head._pathSeparator = value;
            }
        }

        public virtual String _PathRootPrefix { get; set; } = "";
        
        public String PathRootPrefix
        {
            get
            {
                if (this != root)
                {
                    if (root!=null)
                    {
                        if (root is graphNodeBase rootBase)
                        {
                            return rootBase._PathRootPrefix;
                        }
                    }
                }
                
                return _PathRootPrefix;
            }
        }

        /// <summary>
        /// Putanja objekta
        /// </summary>
        public virtual string path
        {
            get
            {
                String output = name;
                if (parent != null) return parent.path + pathSeparator + name;

                return PathRootPrefix + output;
            }
        }

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        public virtual object root
        {
            get
            {
                if (parent == null) return this;
                return parent.root;
            }
        }

        /// <summary>
        /// Referenca prema parent objektu
        /// </summary>
        object IObjectWithParent.parent
        {
            get
            {
                return parent;
            }
        }

        string IObjectWithUID.UID => path;

        /// <summary>
        /// Gets the child names.
        /// </summary>
        /// <returns></returns>
        public List<String> getChildNames()
        {
            List<String> output = new List<string>();
            foreach (Object k in children.Keys)
            {
                output.Add(k.ToString());
            }
            return output;
        }

        /// <summary>
        /// Gets the first.
        /// </summary>
        /// <returns></returns>
        public IGraphNode getFirst()
        {
            foreach (Object k in children.Keys)
            {
                return children[k] as IGraphNode;
            }
            return null;
        }

        public Int32 Count()
        {
            return children.Count;
        }

        /// <summary>
        /// Removes by the key specified
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(String key)
        {
            IGraphNode pop;
            children.Remove(key);
            //children.TryRemove(key, out pop); // .Remove(key);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IObjectWithPathAndChildren> GetEnumerator()
        {
            List<IObjectWithPathAndChildren> output = new List<IObjectWithPathAndChildren>();
            foreach (Object k in children.Keys)
            {
                if (children[k] != this)
                {
                    output.Add(children[k] as IGraphNode);
                }
            }

            return output.GetEnumerator(); //.GetEnumerator();
        }

        /// <summary>
        /// Removes child matching the specified key, on no match returns <c>false</c>
        /// </summary>
        /// <param name="key">The key to match children against</param>
        /// <returns>
        /// True if a child removed, false if no child matched by the key
        /// </returns>
        public bool RemoveByKey(string key)
        {
            if (children.Contains(key))
            {
                //children[key].parent = null;
                IGraphNode gn;
                children.Remove(key);

                //if (children.TryRemove(key, out gn))
                //{
                //    gn.parent = null;
                //}
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes all children with matching <see cref="graphNode.name" />
        /// </summary>
        /// <param name="keys">The keys to match children with</param>
        /// <returns>
        /// Number of child nodes matched and removed
        /// </returns>
        public int Remove(IEnumerable<string> keys)
        {
            Int32 c = 0;
            foreach (String k in keys)
            {
                if (RemoveByKey(k)) c++;
            }
            return c;
        }

        IEnumerator<IGraphNode> IEnumerable<IGraphNode>.GetEnumerator()
        {
            List<IGraphNode> output = new List<IGraphNode>();
            foreach (Object k in children.Keys)
            {
                output.Add(children[k] as IGraphNode);
            }

            return output.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}