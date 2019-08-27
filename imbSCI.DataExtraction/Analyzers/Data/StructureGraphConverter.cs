using HtmlAgilityPack;
using imbSCI.Core.extensions;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.functions;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.style.color;
using imbSCI.Data;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions.data;
using imbSCI.Graph.Converters;
using imbSCI.Graph.Converters.tools;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using imbSCI.Graph.DGML.enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Data
{
public class StructureGraphConverter : graphToDirectedGraphConverterDelegateBased<graphWrapNode<LeafNodeDictionaryEntry>>
    {


        public StructureGraphConverter()
        {
            var setup = new GraphStylerSettings();
            setup.doAddNodeTypeToLabel = false;
            setup.doAddLinkWeightInTheLabel = false;
            setup.GraphDirection = GraphDirectionEnum.LeftToRight;
            setup.GraphLayout = GraphLayoutEnum.DependencyMatrix;
            setup.alphaMin = 0.7;
            
            setup.NodeGradient = new ColorGradient("#FF195ac5", "#FF195ac5", ColorGradientFunction.AtoB | ColorGradientFunction.Hue | ColorGradientFunction.CircleCCW);
            setup.LinkGradient = new ColorGradient("#FF6c6c6c", "#FF6c6c6c", ColorGradientFunction.AllAToB);
            Deploy(setup);
        }

        public virtual void SetLinkCustomization(graphWrapNode<LeafNodeDictionaryEntry> parent, graphWrapNode<LeafNodeDictionaryEntry> child, Link link, DirectedGraphStylingCase styleCase)
        {
            //if (child.item != null)
            //{
            //    if (child.item.Category.HasFlag()
            //    {
            //        link.StrokeDashArray = "3,3,6,3";
            //        link.Label = child.item.Content;
            //    }
            //}
        }

        public virtual void SetNodeCustomization(graphWrapNode<LeafNodeDictionaryEntry> sourceNode, Node targetNode, DirectedGraphStylingCase styleCase)
        {
            //if (sourceNode.item != null)
            //{
            //    switch (sourceNode.item.Category)
            //    {
            //        case "Structure":

            //            break;
            //        case "Static":
            //            targetNode.Background = Color.LightBlue.ColorToHex();
            //            break;
            //        case "Dynamic":
            //            targetNode.Background = Color.Orange.ColorToHex();
            //            break;
            //        default:
            //            break;
            //    }

            //}
        }
    }
}