using imbSCI.Core.math;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Graph.Converters.tools;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;

namespace imbSCI.Graph.Converters
{

    /// <summary>
    /// Universal IGRaphNode converter for directed graph
    /// </summary>
    /// <seealso cref="imbSCI.Graph.Converters.DirectedGraphConverterBase{imbSCI.Data.collection.graph.IGraphNode, imbSCI.Data.collection.graph.IGraphNode}" />
    public class IGraphTODirectedGraphConverter : DirectedGraphConverterBase<IGraphNode, IGraphNode>
    {
        public IGraphTODirectedGraphConverter() : base()
        {
            setup = new GraphStylerSettings();

        }

        //        public GraphStylerSettings setup { get; set; }

        public override DirectedGraph Convert(IGraphNode source, int depthLimit = 500, IEnumerable<IGraphNode> rootNodes = null)
        {
            DirectedGraph output = new DirectedGraph();
            if (source == null) return output;
            output.Title = source.name;

            String description = "DirectedGraph built from " + source.GetType().Name + ":GraphNodeBase graph";

            Int32 startLevel = source.level;

            var nodes = source.getAllChildrenInType<IGraphNode>(null, false, true, 1, depthLimit);//source.getAllChildren(null, false, false, 1, depthLimit).ConvertList<IObjectWithPathAndChildren, IGraphNode>(); //.ConvertIList<IObjectWithPathAndChildren, T>();

            DirectedGraphStylingCase styleCase = GetStylingCase(nodes);

            Dictionary<String, Node> nodeDictionary = new Dictionary<string, Node>();

            foreach (IGraphNode ch in nodes)
            {
                if (ch.level > (startLevel + depthLimit)) continue;

                IGraphNode child = ch;
                var gn = output.Nodes.AddNode(GetNodeID(child), GetNodeLabel(child));
                var tid = GetTypeID(child);
                Double w = GetNodeWeight(child);
                gn.Category = styleCase.Categories.AddOrGetCategory(tid.ToString(), "", "").Id;
                gn.Background = styleCase.nodeStyler.GetHexColor(w, tid);
                gn.StrokeThinkness = styleCase.nodeStyler.GetBorderThickness(w, tid);
                gn.Stroke = styleCase.nodeStyler.GetHexColor(w, tid);
                SetNodeCustomization(child, gn, styleCase);
            }

            foreach (var ch in nodes)
            {
                if (ch.level > (startLevel + depthLimit)) continue;
                if (ch.parent != null)
                {
                    IGraphNode parent = ch.parent;
                    IGraphNode child = ch;
                    if ((parent != null) && (child != null))
                    {
                        var tmp = GetLink(parent, child);
                        var l_tid = GetTypeID(child);
                        Double l_w = GetLinkWeight(parent, child);
                        output.Links.Add(tmp);
                        tmp.Category = styleCase.Categories.AddOrGetCategory(l_tid.ToString(), "", "").Id;
                        tmp.StrokeThinkness = styleCase.linkStyler.GetBorderThickness(l_w, l_tid);
                        tmp.Stroke = styleCase.linkStyler.GetHexColor(l_w, l_tid);
                        SetLinkCustomization(parent, child, tmp, styleCase);
                    }
                }
            }

            output.Layout = setup.GraphLayout; // DGML.enums.GraphLayoutEnum.Sugiyama;
            output.GraphDirection = setup.GraphDirection; // DGML.enums.GraphDirectionEnum.LeftToRight;

            return output;
        }

        public override IGraphNode Convert(DirectedGraph source, int depthLimit = 500, IEnumerable<GraphElement> rootNodes = null)
        {
            throw new System.NotImplementedException();
        }

        public override string GetCategoryID(IGraphNode nodeOrLink)
        {
            if (nodeOrLink == null) return "null";
            return nodeOrLink.GetType().Name;
        }

        public override double GetLinkWeight(IGraphNode nodeA, IGraphNode nodeB)
        {
            if (nodeA == null) return 0;
            if (nodeB == null) return 0;
            return 1.GetRatio(nodeA.Count() + 1);
        }

        public override string GetNodeID(IGraphNode node)
        {
            if (node == null) return "null";
            return node.name;
        }

        public override double GetNodeWeight(IGraphNode node)
        {
            return 1.0;
        }

        public override DirectedGraphStylingCase GetStylingCase(IEnumerable<IGraphNode> source)
        {
            DirectedGraphStylingCase output = new DirectedGraphStylingCase(setup);
            foreach (var ch in source)
            {
                Int32 tid = GetTypeID(ch);
                Double weight = GetNodeWeight(ch);

                output.nodeStyler.learn(tid, weight);

                if (ch.parent != null)
                {
                    IGraphNode parent = ch.parent;
                    IGraphNode child = ch;
                    if ((parent != null) && (child != null))
                    {
                        var l_tid = GetTypeID(child);
                        Double l_w = GetLinkWeight(parent, child);
                        output.linkStyler.learn(l_tid, l_w);

                        var cl = output.Categories.AddOrGetCategory(GetTypeID(parent), GetCategoryID(parent), "");
                        output.Categories.AddUnique(cl);
                    }
                }

                var c = output.Categories.AddOrGetCategory(GetTypeID(ch), GetCategoryID(ch), "");
                output.Categories.AddUnique(c);
            }
            return output;
        }

        public override int GetTypeID(IGraphNode nodeOrLink)
        {

            return 1;
        }

        public virtual void SetLinkCustomization(IGraphNode parent, IGraphNode child, Link link, DirectedGraphStylingCase styleCase)
        {
        }

        public virtual void SetNodeCustomization(IGraphNode sourceNode, Node targetNode, DirectedGraphStylingCase styleCase)
        {
        }
    }

}