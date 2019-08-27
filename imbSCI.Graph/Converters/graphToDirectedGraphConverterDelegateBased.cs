// --------------------------------------------------------------------------------------------------------------------
// <copyright file="graphToDirectedGraphConverterBasic.cs" company="imbVeles" >
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
using imbSCI.Core.math;
using imbSCI.Core.style.color;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.Graph.Converters.tools;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;

namespace imbSCI.Graph.Converters
{
    public class graphToDirectedGraphConverterDelegateBased<T> : graphToDirectedGraphConverterBase<T> where T : IGraphNode, new()
    {

        public Func<T, String> CategoryID;
        public Func<T, Int32> TypeID;
        public Func<T, Double> NodeWeight;
        public Func<T, T, Double> LinkWeight;

        public graphToDirectedGraphConverterDelegateBased()
        {
            setup = new GraphStylerSettings();
            setup.doAddNodeTypeToLabel = false;
            setup.doAddLinkWeightInTheLabel = false;
            setup.GraphDirection = DGML.enums.GraphDirectionEnum.LeftToRight;
            setup.GraphLayout = DGML.enums.GraphLayoutEnum.DependencyMatrix;
            setup.alphaMin = 0.7;
            setup.NodeGradient = new ColorGradient("#FF195ac5", "#FF195ac5", ColorGradientFunction.AtoB | ColorGradientFunction.Hue | ColorGradientFunction.CircleCCW);
            setup.LinkGradient = new ColorGradient("#FF6c6c6c", "#FF6c6c6c", ColorGradientFunction.AllAToB);

            Deploy(setup);
        }

        public override string GetCategoryID(T nodeOrLink)
        {
            if (nodeOrLink == null) return "";
            return CategoryID(nodeOrLink);
        }

        public override int GetTypeID(T nodeOrLink)
        {
            if (nodeOrLink == null) return 0;
            return TypeID(nodeOrLink);
        }

        public override double GetNodeWeight(T node)
        {
            if (node == null) return 0;
            return NodeWeight(node);
        }

        public override double GetLinkWeight(T nodeA, T nodeB)
        {
            if (nodeA == null) return 0;
            if (nodeB == null) return 0;
            return LinkWeight(nodeA, nodeB);
        }
    }
}