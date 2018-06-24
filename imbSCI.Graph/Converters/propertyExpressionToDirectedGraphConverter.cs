// --------------------------------------------------------------------------------------------------------------------
// <copyright file="propertyExpressionToDirectedGraphConverter.cs" company="imbVeles" >
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
//using System.Web.UI.WebControls;
//using Accord;
using imbSCI.Core.data;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.math;
using imbSCI.Core.style.color;
using imbSCI.Graph.Converters.tools;
using imbSCI.Graph.DGML;
using System;
using System.Collections;

namespace imbSCI.Graph.Converters
{
    /// <summary>
    /// Converter from <see cref="PropertyExpression"/> tree to <see cref="DirectedGraph"/> graph
    /// </summary>
    /// <seealso cref="imbSCI.Graph.Converters.graphToDirectedGraphConverterBase{imbSCI.Core.data.PropertyExpression}" />
    public class propertyExpressionToDirectedGraphConverter : graphToDirectedGraphConverterBase<PropertyExpression>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="propertyExpressionToDirectedGraphConverter"/> class.
        /// </summary>
        public propertyExpressionToDirectedGraphConverter() : base()
        {
            setup = new GraphStylerSettings();
            setup.GraphDirection = DGML.enums.GraphDirectionEnum.LeftToRight;
            setup.GraphLayout = DGML.enums.GraphLayoutEnum.DependencyMatrix;
            setup.alphaMin = 0.7;
            setup.NodeGradient = new ColorGradient("#FF195ac5", "#FF195ac5", ColorGradientFunction.AtoB | ColorGradientFunction.Hue | ColorGradientFunction.CircleCCW);
            setup.LinkGradient = new ColorGradient("#FF6c6c6c", "#FF6c6c6c", ColorGradientFunction.AllAToB);

            Deploy(setup);
        }

        /// <summary>
        /// Gets the category identifier.
        /// </summary>
        /// <param name="nodeOrLink">The node or link.</param>
        /// <returns></returns>
        public override string GetCategoryID(PropertyExpression nodeOrLink)
        {
            return GetTypeID(nodeOrLink).ToString();
        }

        /// <summary>
        /// Provides the link weight, by default implementation returns 1 / <see cref="M:imbSCI.Data.collection.graph.IGraphNode.Count" />
        /// </summary>
        /// <param name="nodeA">The node a.</param>
        /// <param name="nodeB">The node b.</param>
        /// <returns></returns>
        public override double GetLinkWeight(PropertyExpression nodeA, PropertyExpression nodeB)
        {
            if (nodeA == null) return 0;
            if (nodeB == null) return 0;
            return 1.GetRatio(nodeA.Count() + 1);
        }

        /// <summary>
        /// Gets the node weight - by default implementation it is 1 / <see cref="P:imbSCI.Data.collection.graph.IGraphNode.level" />
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public override double GetNodeWeight(PropertyExpression node)
        {
            if (node == null) return 0;
            return ((node.Count() + 1) * (1.GetRatio(node.level)));
        }

        /// <summary>
        /// Gets the label for link - by default implementation returns empty string
        /// </summary>
        /// <param name="nodeA">The node a.</param>
        /// <param name="nodeB">The node b.</param>
        /// <returns></returns>
        public override string GetLinkLabel(PropertyExpression nodeA, PropertyExpression nodeB)
        {
            if (nodeB.valueType == null) return "[?]";
            return nodeB.valueType.Name; //  base.GetLinkLabel(nodeA, nodeB);
        }

        /// <summary>
        /// Gets the node label.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public override string GetNodeLabel(PropertyExpression node)
        {
            Object vl = node.getValue();

            if (node.host is IList) return node.name + "[ ]";
            if (node.host is IDictionary) return node.name + "[ ]";

            if (node.valueType == null) return node.name + " [!]";
            //if (node.valueType.isToggleValue() || node.valueType.isNumber()) return node.name + " = " + node.getValue().toStringSafe("") + "";
            if (node.valueType.isText()) return node.name + " = \"" + vl.toStringSafe("") + "\"";
            if (node.valueType.IsClass) return node.name;
            return node.name;
        }

        /// <summary>
        /// Gets the type identifier.
        /// </summary>
        /// <param name="nodeOrLink">The node or link.</param>
        /// <returns></returns>
        public override int GetTypeID(PropertyExpression nodeOrLink)
        {
            if (nodeOrLink.valueType == null) return 0;

            if (nodeOrLink.valueType.isNumber()) return 1;
            if (nodeOrLink.valueType.isToggleValue()) return 2;

            if (nodeOrLink.valueType.IsGenericType) return 4;
            if (nodeOrLink.Count() > 0) return 5;
            if (nodeOrLink.valueType.isText()) return 3;
            return 0;
        }
    }
}