using HtmlAgilityPack;
using imbSCI.DataExtraction.NodeQuery.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.DataExtraction.NodeQuery.Evaluators
{
    public class NodeQueryEvaluatorIndex : NodeQueryEvaluatorBase, INodeQueryEvaluator
    {
        public NodeQueryEvaluatorIndex()
        {
            queryType = NodeQueryType.indexSelection;
        }

        public override NodeQueryResult EvaluateSingle(NodeQueryDefinition query, NodeQueryEvaluationContext context)
        {
            Log(query);

            var indexList = imbSCI.Core.extensions.text.imbStringCommonTools.rangeStringToIndexList(query.query, context.htmlNodes.Count);

            NodeQueryResult output = new NodeQueryResult();

            foreach (Int32 i in indexList)
            {
                if (i < context.htmlNodes.Count)
                {
                    output.ResultNodes.Add(context.htmlNodes[i]);
                }
            }


            output.ResultEvaluation = output.ResultNodes.Any();

            Log(output);

            return output;
        }
    }
}