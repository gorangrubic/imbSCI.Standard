using HtmlAgilityPack;
using imbSCI.Core.reporting.render;
using imbSCI.DataExtraction.NodeQuery.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery.Evaluators
{
    public abstract class NodeQueryEvaluatorBase
    {
        protected ITextRender logger { get; set; }

        protected String logPrefix { get; set; } = "";

        /// <summary>
        /// Sets default evaluation logger
        /// </summary>
        /// <param name="_logger">The logger.</param>
        public void SetLogger(ITextRender _logger)
        {
            logger = _logger;
            logPrefix = GetType().Name.Replace("NodeQueryEvaluator", "");
        }

        public void Log(String message)
        {
            if (logger != null)
            {
                logger.AppendLine(logPrefix + ":" + message);
            }
        }

        public void Log(NodeQueryDefinition query)
        {
            if (logger != null)
            {
                logger.AppendLine("Query:");
                logger.nextTabLevel();

                logger.AppendLine(query.ToString());

                logger.prevTabLevel();

                //Log("----- query ----------");
            }
        }

        public void Log(NodeQueryResult queryResult)
        {
            if (logger != null)
            {
                logger.AppendLine("Query Result: \t\t" + queryResult.ToString());
                //logger.nextTabLevel();

                //logger.AppendLine("Evaluation: " + queryResult.ResultEvaluation);

                //logger.AppendLine("Nodes: " + queryResult.ResultNodes.Count);

                //logger.prevTabLevel();
            }
        }

        public abstract NodeQueryResult EvaluateSingle(NodeQueryDefinition query, NodeQueryEvaluationContext context);

        public NodeQueryType queryType { get; set; } = NodeQueryType.xpath;
    }
}