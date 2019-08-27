using HtmlAgilityPack;
using imbSCI.DataExtraction.NodeQuery.Enums;
using imbSCI.DataExtraction.NodeQuery.SelectorBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.NodeQuery
{
public enum NodeQueryRelation
    {
        /// <summary>
        /// The parallel: query will process the same set of nodes like previous query 
        /// </summary>
        parallel,
        /// <summary>
        /// The serial: query will process result of previous query
        /// </summary>
        serial
    }
}