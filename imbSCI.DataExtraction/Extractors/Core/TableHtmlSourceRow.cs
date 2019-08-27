using HtmlAgilityPack;
using imbSCI.Core.reporting.render;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.Extractors.Core
{
    public class TableHtmlSourceRow : TableSourceRow<HtmlNode, HtmlNode>
    {
        public TableHtmlSourceRow(HtmlNode rowNode) : base(rowNode)
        {
        }
    }
}