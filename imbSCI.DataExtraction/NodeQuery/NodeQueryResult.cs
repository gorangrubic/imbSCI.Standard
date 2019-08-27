using HtmlAgilityPack;
using imbSCI.Data;
using imbSCI.DataExtraction.NodeQuery.Enums;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery
{
    /// <summary>
    /// Result of Node query execution
    /// </summary>
    public class NodeQueryResult
    {
        public NodeQueryResult()
        {
        }

        public String Merge(NodeQueryResult result, NodeQueryResultOperand nodes_operand, NodeQueryPredicateOperand evaluation_operand)
        {
            String mergeReport = this.ToString() + " " + evaluation_operand.ToString() + "(" + nodes_operand.ToString() + ")";

            mergeReport = mergeReport.add(result.ToString(), " ");

            ResultNodes = ResultNodes.GetOperationResult(result.ResultNodes, nodes_operand);

            switch (evaluation_operand)
            {
                case NodeQueryPredicateOperand.AND:
                    ResultEvaluation = ResultEvaluation && result.ResultEvaluation;
                    break;

                case NodeQueryPredicateOperand.NOT:
                    ResultEvaluation = ResultEvaluation && !result.ResultEvaluation;
                    break;

                case NodeQueryPredicateOperand.OR:
                    ResultEvaluation = ResultEvaluation || result.ResultEvaluation;
                    break;

                case NodeQueryPredicateOperand.SET:
                    ResultEvaluation = result.ResultEvaluation;
                    break;

                case NodeQueryPredicateOperand.IGNORE:
                    break;

                case NodeQueryPredicateOperand.REEVAL:
                    ResultEvaluation = ResultNodes.Any();

                    break;
            }

            mergeReport = mergeReport.add(" => " + this.ToString(), " ");

            return mergeReport;
        }

        public override String ToString()
        {
            return ResultEvaluation.ToString().ToUpper() + "(" + ResultNodes.Count + ")";
        }

        public Boolean ResultEvaluation { get; set; } = false;

        public List<HtmlNode> ResultNodes { get; set; } = new List<HtmlNode>();
    }
}