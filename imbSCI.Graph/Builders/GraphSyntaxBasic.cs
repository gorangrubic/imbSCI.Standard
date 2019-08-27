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
public class GraphSyntaxBasic: GraphSyntaxBase
    {
        public override void Init()
        {
            String regex_insert = @"([\w]+)";

            SyntaxMarkers.Add(new regexFormatMarker<diagramLinkTypeEnum>("-", diagramLinkTypeEnum.normal));

            SyntaxMarkers.Add(new regexFormatMarker<diagramNodeShapeEnum>("{0}", diagramNodeShapeEnum.normal));
        }

    }
}