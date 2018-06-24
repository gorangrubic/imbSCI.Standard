// --------------------------------------------------------------------------------------------------------------------
// <copyright file="linknodeElement.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.linknode
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Element of static tree structure built with <see cref="linknodeBuilder"/>
    /// </summary>
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithChildSelector" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithName" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithPath" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithParent" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithPathAndChildSelector" />
    public class linknodeElement : IObjectWithChildSelector, IObjectWithName, IObjectWithPath, IObjectWithParent, IObjectWithPathAndChildSelector
    {
        public linknodeElement()
        {
        }

        /// <summary>
        /// Setnodes the specified path.
        /// </summary>
        /// <param name="__path">The path.</param>
        /// <param name="__name">The name.</param>
        /// <param name="__parent">The parent.</param>
        /// <param name="__root">The root.</param>
        /// <param name="__level">The level.</param>
        /// <param name="__meta">The meta.</param>
        public void setnode(string __path, string __name, linknodeElement __parent, linknodeElement __root, int __level)
        {
            path = __path;

            name = __name;
            parent = __parent;
            root = __root;
            level = __level;
        }

        /// <summary>
        /// Setmetas the specified meta.
        /// </summary>
        /// <param name="__meta">The meta.</param>
        public void setmeta(object __meta)
        {
            if (!meta.Contains(__meta))
            {
                meta.Add(__meta);
            }
        }

        /// <summary>
        /// Original path -- only for source nodes.
        /// </summary>
        public string originalPath { get; set; } = "";

        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>
        /// The meta.
        /// </value>
        public List<object> meta { get; protected set; } = new List<object>();

        /// <summary>
        /// Check if any meta is assigned to this node and optionally> if any direct child has any meta object assigned
        /// </summary>
        /// <param name="checkChildrenToo">if set to <c>true</c> [check children too].</param>
        /// <returns></returns>
        public bool haveMeta(bool checkChildrenToo = true)
        {
            if (meta.Any()) return true;

            if (items.Values.Any(x => x.meta.Any())) return true;

            return false;
        }

        /// <summary>
        /// Depth level. 0 is root, 1+ are children
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public int level { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public int score { get; set; } = 0;

        /// <summary>
        /// Ime koje je dodeljeno objektu
        /// </summary>
        public string name { get; set; }

        /// <summary> </summary>
        public string path { get; protected set; }

        /// <summary>
        /// Number of child items
        /// </summary>
        /// <returns></returns>
        public int Count() => items.Count();

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        /// <summary>
        /// Index of supplied child - in his collection
        /// </summary>
        /// <param name="child"></param>
        /// <returns>
        /// -1 if not found
        /// </returns>
        public int indexOf(IObjectWithChildSelector child)
        {
            return items.imbGetIndexOf(child, true);  //((IObjectWithChildSelector)parent).indexOf(child);
        }

        IEnumerator IObjectWithChildSelector.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        int IObjectWithChildSelector.indexOf(IObjectWithChildSelector child) => indexOf(child);

        int IObjectWithChildSelector.Count() => Count();

        IEnumerator IEnumerable.GetEnumerator() => items.Values.GetEnumerator();

        /// <summary>
        ///
        /// </summary>
        public linknodeElement parent { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public Dictionary<string, linknodeElement> items { get; protected set; } = new Dictionary<string, linknodeElement>();

        object IObjectWithParent.parent
        {
            get
            {
                return ((IObjectWithParent)parent).parent;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public linknodeElement root { get; set; }

        object IObjectWithRoot.root
        {
            get
            {
                return (IObjectWithRoot)root;
            }
        }

        object IObjectWithChildSelector.this[string childName]
        {
            get
            {
                return items[childName];
            }
        }

        object IObjectWithChildSelector.this[int key]
        {
            get
            {
                return items.imbGetItemAt(key, true);
            }
        }
    }
}