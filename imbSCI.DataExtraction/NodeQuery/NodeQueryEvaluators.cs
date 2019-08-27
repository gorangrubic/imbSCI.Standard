using HtmlAgilityPack;
using imbSCI.Core.reporting.render;
using imbSCI.DataExtraction.NodeQuery.Enums;
using imbSCI.DataExtraction.NodeQuery.Evaluators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery
{
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.Dictionary{imbSCI.DataExtraction.NodeQuery.NodeQueryType, imbSCI.DataExtraction.NodeQuery.INodeQueryEvaluator}'
    /// <summary>
    /// Set of query evaluators, for <see cref="NodeQueryType"/>s
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Dictionary{imbSCI.DataExtraction.NodeQuery.NodeQueryType, imbSCI.DataExtraction.NodeQuery.INodeQueryEvaluator}" />
    public class NodeQueryEvaluators : Dictionary<NodeQueryType, INodeQueryEvaluator>
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.Dictionary{imbSCI.DataExtraction.NodeQuery.NodeQueryType, imbSCI.DataExtraction.NodeQuery.INodeQueryEvaluator}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    {
        protected ITextRender logger { get; set; }

        /// <summary>
        /// Sets default evaluation logger
        /// </summary>
        /// <param name="_logger">The logger.</param>
        public void SetLogger(ITextRender _logger)
        {
            logger = _logger;
            foreach (var pair in this)
            {
                pair.Value.SetLogger(logger);
            }
        }

        public void Log(String message)
        {
            if (logger != null)
            {
                logger.AppendLine(message);
            }
        }

        /// <summary>
        /// Gets standard evaluators.
        /// </summary>
        /// <returns></returns>
        public static NodeQueryEvaluators GetStandardEvaluators()
        {
            NodeQueryEvaluators output = new NodeQueryEvaluators();

            NodeQueryEvaluatorXPath xpath_eval = new NodeQueryEvaluatorXPath();
            output.Add(xpath_eval);

            NodeQueryEvaluatorUrlRegex urlregex_eval = new NodeQueryEvaluatorUrlRegex();
            output.Add(urlregex_eval);

            NodeQueryEvaluatorIndex index_eval = new NodeQueryEvaluatorIndex();
            output.Add(index_eval);

            return output;
        }

        public void Add(INodeQueryEvaluator evaluator)
        {
            if (!ContainsKey(evaluator.queryType))
            {
                Add(evaluator.queryType, evaluator);
                if (logger != null)
                {
                    evaluator.SetLogger(logger);
                }
            }
        }

        /// <summary>
        /// Evaluates the specified queries.
        /// </summary>
        /// <param name="queries">The queries.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public NodeQueryResult Evaluate(NodeQueryCollection queries, NodeQueryEvaluationContext context)
        {
            NodeQueryResult output = null;

            if (logger != null) logger.nextTabLevel();

            Log("Query group [" + queries.Count + "]");

            Int32 m = 0;
            foreach (NodeQueryDefinition query in queries)
            {
                NodeQueryResult qr = null;

                if (output != null) {
                    switch (query.relation)
                    {
                        default:
                        case NodeQueryRelation.parallel:
                            break;
                        case NodeQueryRelation.serial:
                            if (output.ResultNodes.Any())
                            {
                                context = new NodeQueryEvaluationContext()
                                {
                                    htmlNodes = new List<HtmlNode>(),
                                    documentUrl = context.documentUrl
                                };
                                context.htmlNodes.AddRange(output.ResultNodes);
                            }
                            
                            break;
                    }
                }

                if (query.IsGroupQuery)
                {
                    qr = Evaluate(query.groupQueries, context);
                }
                else
                {
                    qr = Evaluate(query, context);
                }

                

                if (output == null)
                {
                    output = qr;
                }
                else
                {
                    m++;
                    String r = output.Merge(qr, query.resultOperand, query.operand);
                    Log(m.ToString("D2") + " { " + r + " } ");
                }
            }

            if (logger != null) logger.prevTabLevel();

            return output;
        }

        /// <summary>
        /// Evaluates the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">query - This evaluator collection have no support for query type [" + query.queryType.ToString() + "]</exception>
        public NodeQueryResult Evaluate(NodeQueryDefinition query, NodeQueryEvaluationContext context)
        {
            NodeQueryResult output = new NodeQueryResult();

            if (ContainsKey(query.queryType))
            {
                output = this[query.queryType].EvaluateSingle(query, context);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(query), "This evaluator collection have no support for query type [" + query.queryType.ToString() + "]");
            }

            return output;
        }
    }
}