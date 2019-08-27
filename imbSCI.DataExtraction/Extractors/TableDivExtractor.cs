using HtmlAgilityPack;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Extractors
{
    /// <summary>
    /// Extracts data from div table
    /// </summary>
    /// <seealso cref="imbSCI.DataExtraction.MetaExtractor.HtmlExtractorBase" />
    public class TableDivExtractor : HtmlExtractorBase
    {

        public Int32 RowSelectionDepthLimit { get; set; } = 10;
        public Int32 RowSelectionDepthStart { get; set; } = 1;
        public String RowSelectionTag { get; set; } = "div";

        public Int32 TableSelectionDepthLimit { get; set; } = 2;
        public Int32 TableSelectionDepthStart { get; set; } = 1;
        public String TableSelectionTag { get; set; } = "div";

        public Int32 AdaptiveSteps { get; set; } = 5;

        public override MetaTableDescription GetTableDescription()
        {
            return null;
        }

        //public override MetaTableSchema GetTableSchema()
        //{
        //    return null;
        //    //throw new NotImplementedException();
        //}

        protected TableHtmlSourceRow SetSourceRow(HtmlNode row)
        {
            TableHtmlSourceRow sourceRow = new TableHtmlSourceRow(row);

            var html_cells = sourceRow.RowNode.SelectNodesInDepthRange(
                x => x.Name.Equals(RowSelectionTag, StringComparison.InvariantCultureIgnoreCase)
                && !x.ChildNodes.Any(y => y.Name.Equals(RowSelectionTag, StringComparison.InvariantCultureIgnoreCase))
                , RowSelectionDepthLimit, RowSelectionDepthStart, false);  //sourceRow.RowNode.SelectChildrenOnDepth("div", 2);

            if (html_cells.Count > 0)
            {
                foreach (var n in html_cells)
                {
                    sourceRow.RowCells.Add(n);
                }
            }
            return sourceRow;
        }

        public List<HtmlNode> AdaptiveRowSelection(HtmlNode divNode, Int32 steps=5)
        {
            Dictionary<Double, List<HtmlNode>> selectionByRatio = new Dictionary<double, List<HtmlNode>>();
            HtmlNode head = divNode;
            Double bestRatio = Double.MinValue;
            List<HtmlNode> bestSelection = null;

            for (int i = 0; i < steps; i++)
            {
                if (head == null) break;
                if (!head.Name.Equals("div", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
                List<HtmlNode> html_tablerows = head.SelectNodesInDepthRange(x => x.Name.Equals(TableSelectionTag, StringComparison.InvariantCultureIgnoreCase), TableSelectionDepthLimit, TableSelectionDepthStart, false);
                
                Double rate = 0;

                Int32 rows = html_tablerows.Count;
                Int32 columns = Int32.MaxValue;
                if (html_tablerows.Count > 0)
                {
                    foreach (var r in html_tablerows)
                    {
                        
                        var html_cells = r.SelectNodesInDepthRange(
                        x => x.Name.Equals(RowSelectionTag, StringComparison.InvariantCultureIgnoreCase)
                        && !x.ChildNodes.Any(y => y.Name.Equals(RowSelectionTag, StringComparison.InvariantCultureIgnoreCase))
                        , RowSelectionDepthLimit, RowSelectionDepthStart, false);

                        columns = Math.Min(columns, html_cells.Count);
                    }
                     
                    if (columns == Int32.MaxValue)
                    {
                        rate = 0;
                    } else
                    {
                        rate = rows.GetRatio(columns);
                    }
                    
                }

                if (!selectionByRatio.ContainsKey(rate))  selectionByRatio.Add(rate, html_tablerows);

                head = head.ParentNode;
                if (rate > bestRatio)
                {
                    bestRatio = rate;
                    bestSelection = html_tablerows;
                }
            }

            return bestSelection;
             

        }


        public override SourceTable MakeSourceTable(HtmlNode divNode)
        {
            var head = divNode.SelectSingleNode(divNode.XPath.add("div[not(*)]", "//"));
            if (head == null)
            {
                head = divNode;
            }
            List<HtmlNode> html_tablerows = AdaptiveRowSelection(head, AdaptiveSteps); //divNode.SelectNodesInDepthRange(x => x.Name.Equals(TableSelectionTag, StringComparison.InvariantCultureIgnoreCase), TableSelectionDepthLimit, TableSelectionDepthStart, false);

            List<TableHtmlSourceRow> html_selected_rows = new List<TableHtmlSourceRow>();

            Int32 columns = 0;
            foreach (HtmlNode row in html_tablerows)
            {
                TableHtmlSourceRow sourceRow = SetSourceRow(row);
                columns = Math.Max(columns, sourceRow.Count);
                if (sourceRow.Count > 0)
                {
                    html_selected_rows.Add(sourceRow);
                }
            }
            Int32 rows = html_selected_rows.Count;

            SourceTable sourceTable = new SourceTable(columns, rows);
            if (!sourceTable.IsValid)
            {
                if (DoThrowException)
                {
                    throw new ArgumentOutOfRangeException(nameof(columns), "Number of columns and rows can't be less than 1. Specified values: width [" + columns.ToString() + "], height [" + rows.ToString() + "]");
                }
            }

            for (int i = 0; i < html_selected_rows.Count; i++)
            {
                TableHtmlSourceRow row_node = html_selected_rows[i];

                for (int j = 0; j < row_node.RowCells.Count; j++)
                {
                    SetSourceTableCell(sourceTable[j, i], row_node.RowCells[j], divNode.OwnerDocument);
                }
            }

            return sourceTable;
        }


    }
}