using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.data.text;
using imbSCI.Data.interfaces;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using imbSCI.Graph.Diagram.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.Graph.Builders
{
public class GraphSyntaxMermaid: GraphSyntaxBase
    {
        public override void Init()
        {
            String regex_insert = @"([\w\s\d]+)";

            SyntaxMarkers.Add(new regexFormatMarker<diagramLinkTypeEnum>("-.-",  diagramLinkTypeEnum.dotted));
            SyntaxMarkers.Add(new regexFormatMarker<diagramLinkTypeEnum>("--",  diagramLinkTypeEnum.normal));
            SyntaxMarkers.Add(new regexFormatMarker<diagramLinkTypeEnum>("==", diagramLinkTypeEnum.thick));
            SyntaxMarkers.Add(new regexFormatMarker<diagramLinkTypeEnum>(">", diagramLinkTypeEnum.outbound));
            SyntaxMarkers.Add(new regexFormatMarker<diagramLinkTypeEnum>("<", diagramLinkTypeEnum.inbound));

            SyntaxMarkers.Add(new regexFormatMarker<diagramLinkTypeEnum>("-.{0}.-",  diagramLinkTypeEnum.dotted));
            SyntaxMarkers.Add(new regexFormatMarker<diagramLinkTypeEnum>("--{0}--",  diagramLinkTypeEnum.normal));
            SyntaxMarkers.Add(new regexFormatMarker<diagramLinkTypeEnum>("=={0}==",  diagramLinkTypeEnum.thick));

            SyntaxMarkers.Add(new regexFormatMarker<diagramNodeShapeEnum>(diagramNodeShapeEnum.circle, "{1}(({0}))", regex_insert));
            //SyntaxMarkers.Add(new regexFormatMarker<diagramNodeShapeEnum>("[{0}<", regex_insert, diagramNodeShapeEnum.flagToLeft));
            //SyntaxMarkers.Add(new regexFormatMarker<diagramNodeShapeEnum>(">{0}]", regex_insert, diagramNodeShapeEnum.flagToRight));
            //SyntaxMarkers.Add(new regexFormatMarker<diagramNodeShapeEnum>("[{0}]", regex_insert, diagramNodeShapeEnum.normal));

            //SyntaxMarkers.Add(new regexFormatMarker<diagramNodeShapeEnum>("{{0}}", regex_insert, diagramNodeShapeEnum.rhombus));
            //SyntaxMarkers.Add(new regexFormatMarker<diagramNodeShapeEnum>("({0})", regex_insert, diagramNodeShapeEnum.rounded));
            //SyntaxMarkers.Add(new regexFormatMarker<diagramNodeShapeEnum>("{0}", regex_insert, diagramNodeShapeEnum.normal));
            SyntaxMarkers.Add(new regexFormatMarker<diagramNodeShapeEnum>("{0}", diagramNodeShapeEnum.normal));

        }
    }
}