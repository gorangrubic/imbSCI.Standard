using HtmlAgilityPack;

using imbSCI.DataExtraction.NodeQuery.Enums;
using imbSCI.DataExtraction.NodeQuery.SelectorBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery
{
    [Serializable]
    public class NodeQueryDefinition:IEquatable<NodeQueryDefinition>
    {
        public String ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (IsGroupQuery)
            {
                sb.AppendLine("Group {");
                for (int i = 0; i < groupQueries.Count; i++)
                {
                    sb.AppendLine("\t" + i.ToString() + "\t" + groupQueries[i].ToString());
                }
                sb.AppendLine("} " + OperatorToString());
            }
            else
            {
                sb.AppendLine(queryType.ToString() + "(\"" + query + "\") " + OperatorToString());

                sb.AppendLine("-- Chrome console format: " + NodeQueryJSConsoleTranslate.GetConsoleSyntax(queryType, WebClientCommandType.Select, query));
            }

            

            return sb.ToString();
        }

        public String OperatorToString()
        {
            return operand.ToString() + "(" + resultOperand.ToString() + ")";
        }

        public static NodeQueryDefinition UrlRegexQuery(String query, NodeQueryPredicateOperand operand)
        {
            NodeQueryDefinition output = new NodeQueryDefinition()
            {
                queryType = NodeQueryType.urlRegex,
                query = query,
                operand = operand,
                resultOperand = NodeQueryResultOperand.IGNORE
            };
            return output;
        }

        public bool Equals(NodeQueryDefinition other)
        {
            return ToString().Equals(other.ToString());
        }

        /// <summary>
        /// Selector to select the feature
        /// </summary>
        /// <value>
        /// The x path selector.
        /// </value>
        public String query { get; set; } = "";

        public NodeQueryType queryType { get; set; } = NodeQueryType.xpath;

        public NodeQueryRelation relation { get; set; } = NodeQueryRelation.parallel;

        public NodeQueryPredicateOperand operand { get; set; } = NodeQueryPredicateOperand.REEVAL;

        public NodeQueryResultOperand resultOperand { get; set; } = NodeQueryResultOperand.APPEND;

        /// <summary>
        /// Gets or sets the subqueries -- <see cref="query"/> will be ignored if any query definition is set here,
        /// </summary>
        /// <value>
        /// The subqueries.
        /// </value>
        public NodeQueryCollection groupQueries { get; set; } = new NodeQueryCollection();

        public Boolean IsGroupQuery
        {
            get
            {
                return groupQueries.Any();
            }
        }

        public NodeQueryDefinition()
        {
        }
    }
}