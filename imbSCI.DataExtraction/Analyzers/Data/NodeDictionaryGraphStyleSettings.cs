using HtmlAgilityPack;
using imbSCI.Core.math;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions;
using imbSCI.Data;
using imbSCI.Graph.Converters;
using imbSCI.Core.extensions.data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using imbSCI.Core.files.folders;
using imbSCI.Data.collection.graph;
using imbSCI.Core.math.range.frequency;
using System.IO;
using imbSCI.Core.style.color;
using System.Drawing;
using imbSCI.Graph.DGML.core;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    public class NodeDictionaryGraphStyleSettings
    {
        public String PermanentStrokeDashArray { get; set; } = "";
        public String OptionalStrokeDashArray { get; set; } = "5,10,2,10";

        public String StaticStrokeColor { get; set; }
        public String DynamicStrokeColor { get; set; }
        

        public String LinkSelectedColor { get; set; }
        public String LinkNotSelectedColor { get; set; }

        
        public String NodeWithoutItemColor { get; set; }
        public String NodeWithItemColor { get; set; }

        public String NodeNotSelectedColor { get; set; }

        public String NodeSelectedColor { get; set; } = "";

        public String ContentNodeDaskArray { get; set; } = "3,3,3,3,3";
        public String ContentNodeStrokeColor { get; set; } = "#999999";

        public ColorGradient NodeSelectedByChunkGradient { get; set; }


        public Boolean AddContentNodes { get; set; } = true;


        public void SetContentElement(GraphElement graphElement)
        {
            if (graphElement is Node nodeElement)
            {
                nodeElement.StrokeDashArray = ContentNodeDaskArray;
                nodeElement.Stroke = ContentNodeStrokeColor;
            } else if (graphElement is Link linkElement)
            {

            }
        }


        public void SetElement(GraphElement graphLink, NodeGraph forNodeGraph)
        {
            if (forNodeGraph == null) return;

            if (forNodeGraph.item == null)
            {
                graphLink.Stroke = NodeWithoutItemColor;
                graphLink.StrokeDashArray = PermanentStrokeDashArray;
                graphLink.StrokeThinkness = 1;
            }
            else
            {
                graphLink.StrokeThinkness = 3;

                if (forNodeGraph.item.Category.HasFlag(NodeInTemplateRole.Optional))
                {
                    graphLink.StrokeDashArray = OptionalStrokeDashArray;
                }
                else if (forNodeGraph.item.Category.HasFlag(NodeInTemplateRole.Permanent))
                {
                    graphLink.StrokeDashArray = PermanentStrokeDashArray;
                }

                if (forNodeGraph.item.Category.HasFlag(NodeInTemplateRole.Dynamic))
                {
                    graphLink.Stroke = DynamicStrokeColor;
                }
                else if (forNodeGraph.item.Category.HasFlag(NodeInTemplateRole.Static))
                {
                    graphLink.Stroke = StaticStrokeColor;
                }
            }

            
        }

        public void SetNode(GraphNodeElement graphLink, NodeGraph forNodeGraph)
        {
            if (forNodeGraph == null) return;

            SetElement(graphLink, forNodeGraph);

            if (forNodeGraph.item == null)
            {
                graphLink.Background = NodeWithoutItemColor;
            }
            else
            {

                graphLink.Background = NodeWithItemColor;
                //if (forNodeGraph.item.Category.HasFlag(NodeInTemplateRole.Dynamic))
                //{
                //    graphLink.Background = DynamicStrokeColor;
                //}
                //else if (forNodeGraph.item.Category.HasFlag(NodeInTemplateRole.Static))
                //{
                //    graphLink.Background = StaticStrokeColor;
                //}
            }


        }

        public NodeDictionaryGraphStyleSettings()
        {
            PermanentStrokeDashArray = "";
            OptionalStrokeDashArray = "5,10,2,10";
            StaticStrokeColor = Color.SkyBlue.ColorToHex();
            DynamicStrokeColor = Color.Violet.ColorToHex();

            LinkSelectedColor = Color.Orange.ColorToHex();
            LinkNotSelectedColor = Color.Gray.ColorToHex();

            NodeNotSelectedColor = Color.LightGray.ColorToHex();
            NodeSelectedColor = Color.LightGreen.ColorToHex();
            NodeWithoutItemColor = Color.DarkGray.ColorToHex();
            NodeWithItemColor = Color.LightGreen.ColorToHex();

            NodeSelectedByChunkGradient = new ColorGradient(Color.GreenYellow, Color.LightPink, ColorGradientFunction.AllAToB);

        }

    }
}