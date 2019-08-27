// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphConversionTools.cs" company="imbVeles" >
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
using imbSCI.Core.data;
using imbSCI.Core.data.transfer;
using imbSCI.Data.collection.graph;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using imbSCI.Graph.DOT;
using imbSCI.Graph.FreeGraph;
using System;
using System.Collections.Generic;

namespace imbSCI.Graph.Converters
{
    /// <summary>
    /// Set of default graph converters
    /// </summary>
    public static class GraphConversionTools
    {
        public static List<DotNode> ConvertNodes(this List<Node> nodes)
        {
            List<DotNode> output = new List<DotNode>();

            foreach (Node n in nodes)
            {
                DotNode dn = new DotNode(n.Label);
                dn.Group = n.Group;
                dn.Shape = DotNodeShape.Box;
                dn.Style = DotNodeStyle.Dashed;
                dn.Visibility = n.Visibility;
                output.Add(dn);
            }

            return output;
        }

        /// <summary>
        /// Converts to free graph -- from the specified node to its leafs (downwards)
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="DepthLimit">The depth limit.</param>
        /// <returns></returns>
        public static freeGraph ConvertToFreeGraph(this graphNode graph, Int32 DepthLimit = 300)
        {
            return GraphConversionTools.BasicConverterInstance.Convert(graph, DepthLimit);
        }

        /// <summary>
        /// Converts to free graph -- from the specified node to its leafs (downwards)
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="DepthLimit">The depth limit.</param>
        /// <returns></returns>
        public static DirectedGraph ConvertToDGML(this freeGraph graph)
        {
            return GraphConversionTools.DefaultDMGLConverter.ConvertToDMGL(graph);
        }

        /// <summary>
        /// Converts to free graph -- from the specified node to its leafs (downwards)
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="DepthLimit">The depth limit.</param>
        /// <returns></returns>
        public static DirectedGraph ConvertToDGML<T>(this T graph, Int32 DepthLimit = 300) where T : IGraphNode, new()
        {

            return IGraphConverter.Convert(graph, DepthLimit);
        }

        public static DirectedGraph ConvertToDGML(this graphNode graph, Int32 DepthLimit = 300)
        {
            return DefaultGraphToDGMLConverterInstance.Convert(graph, DepthLimit);
        }

        public static DirectedGraph ConvertToDGML(this PropertyExpression pe, Int32 DepthLimit = 20)
        {
            return PropertyExpressionConverterToDGML.Convert(pe, DepthLimit);
        }

        /// <summary>
        /// Converts DirectedGraph to DOT graph
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public static DotGraph ConvertToDOT(this DirectedGraph graph)
        {
            return DefaultDotToDGMLConverter.Convert(graph);
        }

        /// <summary>
        /// Converts DOT graph to DirectedGraph
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public static DirectedGraph ConvertToDGML(this DotGraph graph)
        {
            return DefaultDotToDGMLConverter.Convert(graph);
        }

        private static dotToDirectedGraphConverterBasic _DefaultDotToDGMLConverter;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static dotToDirectedGraphConverterBasic DefaultDotToDGMLConverter
        {
            get
            {
                if (_DefaultDotToDGMLConverter == null)
                {
                    _DefaultDotToDGMLConverter = new dotToDirectedGraphConverterBasic();
                }
                return _DefaultDotToDGMLConverter;
            }
        }



        private static Object _IGraphConverter_lock = new Object();
        private static IGraphTODirectedGraphConverter _IGraphConverter;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static IGraphTODirectedGraphConverter IGraphConverter
        {
            get
            {
                if (_IGraphConverter == null)
                {
                    lock (_IGraphConverter_lock)
                    {

                        if (_IGraphConverter == null)
                        {
                            _IGraphConverter = new IGraphTODirectedGraphConverter();
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _IGraphConverter;
            }
        }


        private static propertyExpressionToDirectedGraphConverter _PropertyExpressionConverterToDGML;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static propertyExpressionToDirectedGraphConverter PropertyExpressionConverterToDGML
        {
            get
            {
                if (_PropertyExpressionConverterToDGML == null)
                {
                    _PropertyExpressionConverterToDGML = new propertyExpressionToDirectedGraphConverter();
                }
                return _PropertyExpressionConverterToDGML;
            }
        }

        private static graphToDirectedGraphConverterBasic _DefaultGraphToDGMLConverterInstance;

        /// <summary>
        /// Basic implementation of the graph to <see cref="DirectedGraph"/> converter
        /// </summary>
        public static graphToDirectedGraphConverterBasic DefaultGraphToDGMLConverterInstance
        {
            get
            {
                if (_DefaultGraphToDGMLConverterInstance == null)
                {
                    _DefaultGraphToDGMLConverterInstance = new graphToDirectedGraphConverterBasic();

                }
                return _DefaultGraphToDGMLConverterInstance;
            }
        }

        private static graphToFreeGraphConverterBasic<graphNode> _DefaultFreeGraphConverterInstance;

        /// <summary>
        /// Basic instance of <see cref="graphNode"/> to <see cref="freeGraph"/> converter
        /// </summary>
        public static graphToFreeGraphConverterBasic<graphNode> BasicConverterInstance
        {
            get
            {
                if (_DefaultFreeGraphConverterInstance == null)
                {
                    _DefaultFreeGraphConverterInstance = new graphToFreeGraphConverterBasic<graphNode>();
                }
                return _DefaultFreeGraphConverterInstance;
            }
        }

        private static freeGraphToDMGL _DefaultDMGLConverter;

        /// <summary>
        /// Default instance of <see cref="freeGraph"/> to <see cref="DirectedGraph"/> converter
        /// </summary>
        public static freeGraphToDMGL DefaultDMGLConverter
        {
            get
            {
                if (_DefaultDMGLConverter == null)
                {
                    _DefaultDMGLConverter = new freeGraphToDMGL();
                }
                return _DefaultDMGLConverter;
            }
        }
    }
}