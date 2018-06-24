// --------------------------------------------------------------------------------------------------------------------
// <copyright file="diagramModel.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.reporting;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.DataComplex.diagram
{
    using imbSCI.DataComplex.diagram.core;
    using imbSCI.DataComplex.diagram.enums;

    /// <summary>
    /// Abstract model of a diagram. Flowchart type of diagram is main purpose of this namespace
    /// </summary>
    public class diagramModel : IObjectWithNameAndDescription
    {
        public diagramModel(string __name, string __description, diagramDirectionEnum __direction)
        {
            name = __name;
            description = __description;
            direction = __direction;
        }

        /// <summary>
        ///
        /// </summary>
        public diagramDirectionEnum direction { get; set; } = diagramDirectionEnum.LR;

        /// <summary>
        ///
        /// </summary>
        public string diagramClassName { get; set; } = "graph";

        /// <summary>
        /// Adds the link.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="type">The type.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public diagramLink AddLink(diagramNode from, diagramNode to, diagramLinkTypeEnum type, string description = "", string __hash = "", ILogBuilder logger = null)
        {
            diagramLink output = new diagramLink();
            output.from = from;
            output.to = to;
            output.type = type;
            output.name = getUID(output);
            output.description = description;
            links.Add(output);
            output.parent = this;

            if (!__hash.isNullOrEmpty())
            {
                if (linkByHash.ContainsKey(__hash))
                {
                    if (logger != null) logger.log("AddLink() failed :: " + __hash);
                }
                else
                {
                    linkByHash.Add(__hash, output);
                }
            }
            return output;
        }

        /// <summary> </summary>
        public List<string> hashList { get; protected set; } = new List<string>();

        /// <summary>
        /// Creates new node in the model
        /// </summary>
        /// <param name="__description">The description.</param>
        /// <param name="type">The type.</param>
        /// <param name="__name">The name.</param>
        /// <returns></returns>
        public diagramNode AddNode(string __description, diagramNodeShapeEnum type, string __name = "", string __hash = "", ILogBuilder logger = null)
        {
            diagramNode output = new diagramNode();
            output.description = __description;
            output.shapeType = type;

            if (!isNodeNameAcceptable(__name))
            {
                __name = __name + getUID(output, true, false);
            }

            output.name = __name;
            output.parent = this;

            nodes.Add(output.name, output);
            if (!__hash.isNullOrEmpty())
            {
                if (linkByHash.ContainsKey(__hash))
                {
                    if (logger != null) logger.log("AddNode() failed :: " + __hash);
                }
                else
                {
                    nodeByHash.Add(__hash, output);
                }
            }

            return output;
        }

        /// <summary>
        /// Human-readable title for this diagram
        /// </summary>
        public string name { get; set; } = "The diagram";

        /// <summary>
        /// Human-readable description for this diagram
        /// </summary>
        public string description { get; set; } = "";

        public diagramNode GetNodeByHash(string hash)
        {
            return nodeByHash[hash];
        }

        public diagramLink GetLinkByHash(string hash)
        {
            return linkByHash[hash];
        }

        public int NodeCount
        {
            get
            {
                return nodes.Count();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, diagramNode> nodeByHash { get; set; } = new Dictionary<string, diagramNode>();

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, diagramLink> linkByHash { get; set; } = new Dictionary<string, diagramLink>();

        /// <summary>
        /// Collection of links
        /// </summary>
        internal List<diagramLink> links { get; private set; } = new List<diagramLink>();

        /// <summary>
        /// Collection of nodes
        /// </summary>
        internal Dictionary<string, diagramNode> nodes { get; private set; } = new Dictionary<string, diagramNode>();

        /// <summary>
        /// Gets the link count.
        /// </summary>
        /// <value>
        /// The link count.
        /// </value>
        public int linkCount
        {
            get
            {
                return links.Count();
            }
        }

        /// <summary>
        /// Gets the node count.
        /// </summary>
        /// <value>
        /// The node count.
        /// </value>
        public int nodeCount
        {
            get
            {
                return nodes.Count();
            }
        }

        /// <summary>
        /// Determines whether is specified name acceptable for new node.
        /// </summary>
        /// <param name="__name">Name to be attached to a <see cref="diagramNode"/></param>
        /// <returns>
        ///   <c>true</c> if [is node name acceptable] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        internal bool isNodeNameAcceptable(string __name)
        {
            if (__name.isNullOrEmptyString()) return false;
            if (nodes.ContainsKey(__name)) return false;
            return true;
        }

        private int _lastNamingIteration = 0;

        /// <summary> </summary>
        public int lastNamingIteration
        {
            get
            {
                return _lastNamingIteration;
            }
            protected set
            {
                _lastNamingIteration = value;
                //OnPropertyChanged("lastNamingIteration");
            }
        }

        /// <summary>
        /// Gets the universal ID name for specified element
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="autoSet">if set to <c>true</c> [automatic set].</param>
        /// <param name="useRelatedObject">if set to <c>true</c> [use related object].</param>
        /// <returns></returns>
        internal string getUID(diagramElementBase element, bool autoSet = true, bool useRelatedObject = false)
        {
            string output = "";

            if (element is diagramNode)
            {
                if (useRelatedObject)
                {
                    if (element.relatedObject is IObjectWithPath)
                    {
                        IObjectWithPath element_relatedObject_IObjectWithPath = (IObjectWithPath)element.relatedObject;
                        string ptname = element_relatedObject_IObjectWithPath.path.getCleanPropertyName();
                        output = nodes.makeUniqueDictionaryName(ptname, ref _lastNamingIteration, 10);
                    }
                    else if (element.relatedObject is IObjectWithName)
                    {
                        IObjectWithName withName = (IObjectWithName)element.relatedObject;
                        output = nodes.makeUniqueDictionaryName(withName.name.getCleanPropertyName(), ref _lastNamingIteration, 100);
                    }
                }
                if (output.isNullOrEmpty())
                {
                    output = ((long)nodeCount).DecimalToOrdinalLetterSystem();
                }
            }
            else if (element is diagramLink)
            {
                if (useRelatedObject)
                {
                    diagramLink link = element as diagramLink;
                    if (link.isConnected)
                    {
                        output = link.from.name + "to" + link.to.name;
                    }
                }
                if (output.isNullOrEmpty())
                {
                    output = ((long)linkCount).DecimalToOrdinalLetterSystem() + "_link";
                }
            }

            if (autoSet)
            {
                element.name = output;
            }

            return output;
        }
    }
}