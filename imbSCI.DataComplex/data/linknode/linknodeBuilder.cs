// --------------------------------------------------------------------------------------------------------------------
// <copyright file="linknodeBuilder.cs" company="imbVeles" >
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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Static tree structure
    /// </summary>
    public class linknodeBuilder
    {
        /// <summary>
        /// The root element of the tree
        /// </summary>
        private linknodeElement _root = new linknodeElement();

        /// <summary> </summary>
        public linknodeElement root
        {
            get
            {
                return _root;
            }
            protected set
            {
                _root = value;
            }
        }

        /// <summary>
        /// The source node list
        /// </summary>
        private List<linknodeElement> _sourceNodeList = new List<linknodeElement>();

        /// <summary> </summary>
        public List<linknodeElement> sourceNodeList
        {
            get
            {
                return _sourceNodeList;
            }
            protected set
            {
                _sourceNodeList = value;
            }
        }

        /// <summary>
        /// The source nodes
        /// </summary>
        private Dictionary<string, linknodeElement> _sourceNodes = new Dictionary<string, linknodeElement>();

        /// <summary>All source nodes (the ones used to build the structure) indexed by original path</summary>
        public Dictionary<string, linknodeElement> sourceNodes
        {
            get
            {
                return _sourceNodes;
            }
            protected set
            {
                _sourceNodes = value;
            }
        }

        /// <summary>
        /// The newpath nodes
        /// </summary>
        private Dictionary<string, linknodeElement> _newpathNodes = new Dictionary<string, linknodeElement>();

        /// <summary> </summary>
        public Dictionary<string, linknodeElement> newpathNodes
        {
            get
            {
                return _newpathNodes;
            }
            protected set
            {
                _newpathNodes = value;
            }
        }

        /// <summary> </summary>
        public bool countExisting { get; set; } = true;

        /// <summary>
        /// Adds the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="metaObject">The meta object.</param>
        /// <returns></returns>
        public linknodeElement Add(string path, object metaObject, int score = 1)
        {
            if (sourceNodes.ContainsKey(path))
            {
                if (countExisting)
                {
                    linknodeElement tmp = sourceNodes[path];
                    while (tmp != null)
                    {
                        tmp.score++;
                        tmp = tmp.parent;
                    }
                }
                else
                {
                }
            }
            else
            {
                if (!path.isNullOrEmpty())
                {
                    linknodeElement tmp = root.buildNode(path, metaObject, score);
                    if (!tmp.path.isNullOrEmpty())
                    {
                        sourceNodes.Add(path, tmp);

                        if (!newpathNodes.ContainsKey(tmp.path))
                        {
                            newpathNodes.Add(tmp.path, tmp);
                        }
                        sourceNodeList.Add(tmp);
                        tmp.originalPath = path;
                    }
                    else
                    {
                        return null;
                    }
                    return tmp;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the metas by path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="useNewPath">if set to <c>true</c> [use new path].</param>
        /// <returns></returns>
        public List<T> GetMetasByPath<T>(string path, bool useNewPath = false)
        {
            List<object> metas = new List<object>();
            if (useNewPath)
            {
                metas = GetByStructurePath(path).meta;
            }
            else
            {
                metas = GetByOriginalPath(path).meta;
            }
            List<T> output = new List<T>();
            foreach (var me in metas)
            {
                output.Add((T)me);
            }

            return output;
        }

        /// <summary>
        /// Gets the by meta.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <returns></returns>
        public linknodeElement GetByMeta(object meta)
        {
            for (int i = 0; i < sourceNodeList.Count(); i++)
            {
                if (sourceNodeList[i].meta.Contains(meta))
                {
                    return sourceNodeList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the by structure path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public linknodeElement GetByStructurePath(string path)
        {
            if (newpathNodes.ContainsKey(path))
            {
                return newpathNodes[path];
            }
            return null;
        }

        /// <summary>
        /// Gets the by original path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public linknodeElement GetByOriginalPath(string path)
        {
            if (sourceNodes.ContainsKey(path))
            {
                return sourceNodes[path];
            }
            return null;
        }
    }
}