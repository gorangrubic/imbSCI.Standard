using HtmlAgilityPack;
using imbSCI.Core.reporting.render;
using imbSCI.DataExtraction.NodeQuery.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery.Evaluators
{
    public interface INodeQueryEvaluator
    {
        NodeQueryResult EvaluateSingle(NodeQueryDefinition query, NodeQueryEvaluationContext context);

        NodeQueryType queryType { get; set; }

        void SetLogger(ITextRender _logger);

        void Log(String message);
    }
}