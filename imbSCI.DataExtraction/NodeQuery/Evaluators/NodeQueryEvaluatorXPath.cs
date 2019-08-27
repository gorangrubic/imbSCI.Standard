using HtmlAgilityPack;
using imbSCI.Data;
using imbSCI.DataExtraction.NodeQuery.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.DataExtraction.NodeQuery.Evaluators
{
    public class NodeQueryEvaluatorXPath : NodeQueryEvaluatorBase, INodeQueryEvaluator
    {
        public NodeQueryEvaluatorXPath()
        {
            queryType = NodeQueryType.xpath;
        }

        public override NodeQueryResult EvaluateSingle(NodeQueryDefinition query, NodeQueryEvaluationContext context)
        {
            //var p = context.htmlDocument.DocumentNode.XPath;

            Log(query);

            var nodes = context.htmlNodes.SelectNodes(query.query);

            NodeQueryResult output = new NodeQueryResult();

            if (nodes != null)
            {
                output.ResultNodes.AddRange(nodes);
            }

            output.ResultEvaluation = output.ResultNodes.Any();

            Log(output);

            return output;
        }
    }
}