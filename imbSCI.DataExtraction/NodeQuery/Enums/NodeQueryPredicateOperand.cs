using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery.Enums
{
    public enum NodeQueryPredicateOperand
    {
        /// <summary>
        ///
        /// </summary>
        AND,

        OR,
        NOT,
        REEVAL,
        SET,
        IGNORE
    }
}