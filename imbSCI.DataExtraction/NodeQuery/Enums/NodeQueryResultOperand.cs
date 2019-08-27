using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery.Enums
{
    public enum NodeQueryResultOperand
    {
        /// <summary>
        /// Result of this query will be appended to previous result
        /// </summary>
        APPEND,

        /// <summary>
        /// Any node in the result of this query that has same XPath will be removed from previous result
        /// </summary>
        REMOVE,

        /// <summary>
        /// Only nodes (from previous and current result) with the same XPaths will exist in resulting collection
        /// </summary>
        OVERLAP,

        /// <summary>
        /// The resulting collection will contain only nodes that are distict for previous and current result
        /// </summary>
        DIFFERENCE,

        IGNORE,

        /// <summary>
        /// Result node collection from second operand becomes the resulting collection. Overwrites existing nodes
        /// </summary>
        SET,
    }
}