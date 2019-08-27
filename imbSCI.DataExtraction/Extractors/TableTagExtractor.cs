using HtmlAgilityPack;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.Extractors
{

    /// <summary>
    /// Extracts data from regular table tag
    /// </summary>
    /// <seealso cref="imbSCI.DataExtraction.MetaExtractor.HtmlExtractorBase" />
    public class TableTagExtractor : HtmlExtractorBase
    {

        public String tagName_normalcell { get; set; } = "td";
        public String tagName_headingcell { get; set; } = "th";
        public String tagName_row { get; set; } = "tr";

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

            var html_cells = sourceRow.RowNode.SelectByTagName(tagName_normalcell, 5);
            Int32 row_width = 0;

            if (html_cells.Count == 0)
            {
                html_cells = sourceRow.RowNode.SelectByTagName(tagName_headingcell, 5);
            }

            if (html_cells.Count > 0)
            {
                foreach (var n in html_cells)
                {
                    sourceRow.RowCells.Add(n);
                }
            }
            return sourceRow;
        }

        public override SourceTable MakeSourceTable(HtmlNode tableNode)
        {
            var html_tablerows = tableNode.SelectByTagName(tagName_row, 5); //.SelectNodes(HtmlExtractionTools.XPATH_SELECT_TABLEROWS);
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

            if (rows + columns == 0) return null;

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
                    SetSourceTableCell(sourceTable[j, i], row_node.RowCells[j], tableNode.OwnerDocument);
                }
            }

            return sourceTable;
        }
    }
}