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
    public abstract class TableSourceRow<TRow, TCell>
    {
        public TRow RowNode { get; set; }

        public List<TCell> RowCells { get; protected set; } = new List<TCell>();

        public Int32 Count
        {
            get
            {
                return RowCells.Count;
            }
        }

        protected TableSourceRow(TRow rowNode)
        {
            RowNode = rowNode;
        }
    }
}