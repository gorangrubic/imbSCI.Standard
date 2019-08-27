using HtmlAgilityPack;
using imbSCI.DataExtraction.NodeQuery.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.DataExtraction.NodeQuery.Evaluators
{
    public class NodeQueryEvaluatorUrlRegex : NodeQueryEvaluatorBase, INodeQueryEvaluator
    {
        public NodeQueryEvaluatorUrlRegex()
        {
            queryType = NodeQueryType.urlRegex;
        }

        public override NodeQueryResult EvaluateSingle(NodeQueryDefinition query, NodeQueryEvaluationContext context)
        {
            Log(query);

            Regex regex = new Regex(query.query);
            NodeQueryResult output = new NodeQueryResult();
            output.ResultEvaluation = regex.IsMatch(context.documentUrl);

            Log(output);

            return output;
        }
    }
}