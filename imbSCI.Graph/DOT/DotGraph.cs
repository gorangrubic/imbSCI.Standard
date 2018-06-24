// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DotGraph.cs" company="imbVeles" >
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
using imbSCI.Data.interfaces;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.collections;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace imbSCI.Graph.DOT
{
    /// <summary>
    /// DOT Graph - partially based on: DotNetGraph https://github.com/vfrz/DotNetGraph
    /// </summary>
    public class DotGraph : DirectedGraph, IObjectWithName
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="DotGraph"/> is directed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if directed; otherwise, <c>false</c>.
        /// </value>
        public bool Directed
        {
            get
            {
                return Layout == DGML.enums.GraphLayoutEnum.ForceDirected;
            }
        }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public NodeCollection Nodes { get; set; } = new NodeCollection();

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public LinkCollection Link { get; set; } = new LinkCollection();

        /// <summary>
        /// The elements
        /// </summary>
        //private List<DotElement> _elements;

        public DotGraph()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotGraph"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="directed">if set to <c>true</c> [directed].</param>
        public DotGraph(string name, bool directed = false) : base()
        {
            this.Id = name;
            if (directed) Layout = DGML.enums.GraphLayoutEnum.ForceDirected;
            //_elements = new List<DotElement>();
        }

        /// <summary>
        /// Compiles the and save.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="minified">if set to <c>true</c> [minified].</param>
        public void CompileAndSave(String filename, Boolean minified = false)
        {
            String code = Compile(minified);

            File.WriteAllText(filename, code);
        }

        /// <summary>
        /// Compiles the specified minified.
        /// </summary>
        /// <param name="minified">if set to <c>true</c> [minified].</param>
        /// <returns></returns>
        public string Compile(bool minified = true)
        {
            var builder = new StringBuilder();

            builder.Append((Directed ? "di" : "") + $"graph {Id} {{" + (minified ? "" : "\n"));

            List<IGraphElement> _elements = new List<IGraphElement>();
            _elements.AddRange(Links);
            _elements.AddRange(Nodes);

            foreach (var element in _elements)
            {
                if (element is DotNode node)
                {
                    builder.Append((minified ? "" : "\t") + node.Id + " [ ");
                }
                else if (element is DotLink arrow)
                {
                    builder.Append((minified ? "" : "\t") + arrow.Source + "->" + arrow.Target + " [ ");
                }

                foreach (var p in element.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
                {
                    if (p.CanRead && Attribute.IsDefined(p, typeof(GraphAttributeAttribute)))
                    {
                        var attribute = p.GetCustomAttributes(typeof(GraphAttributeAttribute), true)
                            .Cast<GraphAttributeAttribute>()
                            .FirstOrDefault();

                        var value = p.GetValue(element, null);

                        if (value.Equals(attribute?.DefaultValue))
                        {
                            continue;
                        }

                        var isEnum = value.GetType().IsEnum;
                        var isString = value is string;
                        var isFloat = value is float;

                        var valueString = isEnum
                            ? value.ToString().ToLower()
                            : (isFloat
                                ? Convert.ToSingle(value).ToString(CultureInfo.InvariantCulture)
                                : value);

                        builder.Append(attribute?.name + "=" + (isString ? "\"" : "") +
                                       valueString + (isString ? "\" " : " "));
                    }
                }

                builder.Append("];" + (minified ? "" : "\n"));
            }

            builder.Append("}");

            return builder.ToString();
        }

        /// <summary>
        /// Adds the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        public void Add(IGraphElement element)
        {
            if (element is DotNode)
            {
                Nodes.Add(element as DotNode);
            }
            else if (element is DotLink)
            {
                Links.Add(element as DotLink);
            }
        }
    }
}