using HtmlAgilityPack;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Analyzers.Templates;
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
    /// Uses <see cref="DataPointMapBlock"/>s to extract information
    /// </summary>
    /// <seealso cref="imbSCI.DataExtraction.Extractors.Core.HtmlExtractorBase" />
    public class DataPointMapExtractor : HtmlExtractorBase
    {
        public DataPointMapBlock dpBlock { get; set; } = new DataPointMapBlock();


        public override MetaTableDescription GetTableDescription()
        {
            MetaTableDescription output = new MetaTableDescription();
            output.index_propertyID = 0;
            output.index_entryID = 1;
            output.format = MetaTableFormatType.vertical;
            
            return output;
        }

        //public override MetaTableSchema GetTableSchema()
        //{
        //    return null;
        //    //throw new NotImplementedException();
        //}


        protected TableHtmlSourceRow SetSourceRow(HtmlNode row, DataPointMapEntry dp)
        {
            TableHtmlSourceRow sourceRow = new TableHtmlSourceRow(row);


            if (dp.Properties.Count > 0)
            {
                foreach (var dpi in dp.Properties)
                {
                    var labelNode = row.selectSingleNode(dpi.LabelXPathRelative.GetRelativeXPath(row.XPath));
                    var dataNode = row.selectSingleNode(dpi.DataXPathRelative.GetRelativeXPath(row.XPath));

                    if (labelNode != null) sourceRow.RowCells.Add(labelNode);
                    if (dataNode != null) sourceRow.RowCells.Add(dataNode);
                }
            }
            else
            {

                var labelNode = row.selectSingleNode(dp.LabelXPathRelative.GetRelativeXPath(row.XPath));
                var dataNode = row.selectSingleNode(dp.DataXPathRelative.GetRelativeXPath(row.XPath));


                if (labelNode != null) sourceRow.RowCells.Add(labelNode);
               if (dataNode != null) sourceRow.RowCells.Add(dataNode);
            }

            return sourceRow;
        }

        public override SourceTable MakeSourceTable(HtmlNode tableNode)
        {
            if (dpBlock.BlockXPathRoot.isNullOrEmpty()) return null;

            if (!dpBlock.BlockXPathRoot.StartsWith(tableNode.XPath))
            {
                return null;

            }
            String localBlockPath = dpBlock.BlockXPathRoot.GetRelativeXPath(tableNode.XPath);
            

            var blockRootNode = tableNode.selectSingleNode(localBlockPath);

            if (blockRootNode == null) return null;

            List<TableHtmlSourceRow> html_selected_rows = new List<TableHtmlSourceRow>();

            foreach (var dp in dpBlock.DataPoints)
            {
                String localPath = dp.DataPointXPathRoot.GetRelativeXPath(dpBlock.BlockXPathRoot);
                localPath = dpBlock.BlockXPathRoot.add(localPath, "/");
                localPath = localPath.GetRelativeXPath(blockRootNode.XPath);
                
                var dpRootNode = blockRootNode.selectSingleNode(localPath);

                if (dpRootNode != null) {
                    var rowset = SetSourceRow(dpRootNode, dp);
                    if (rowset.Count > 1)
                    {
                        html_selected_rows.Add(rowset);
                    }
                }
            }

            Int32 rows = html_selected_rows.Count;
            Int32 columns = 2;

            SourceTable sourceTable = new SourceTable(columns, rows);
            if (!sourceTable.IsValid)
            {
                if (DoThrowException)
                {
                    throw new ArgumentOutOfRangeException(nameof(columns), "Number of columns and rows can't be less than 1. Specified values: width [" + columns.ToString() + "], height [" + rows.ToString() + "]");
                }
            }
            for (int i = 0; i < rows; i++)
            {
                var sr = html_selected_rows[i];
                SetSourceTableCell(sourceTable[0, i], sr.RowCells[0], tableNode.OwnerDocument);
                SetSourceTableCell(sourceTable[1, i], sr.RowCells[1], tableNode.OwnerDocument);

            }

            return sourceTable;
        }
    }
}