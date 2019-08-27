using imbSCI.Core.files.folders;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace imbSCI.DataExtraction.Extractors.Task
{
[Flags]
    public enum ExtractionResultPublishFlags
    {
        none = 0,
        dataTable=1,
        sourceTable=2,
        metaTable=4,
        metaTableSchema=8,
        XmlSerialization=16,
        ExcelReport=32,
        TaskXml = 64,
        TaskHtml = 128,
        dataTableExcel = dataTable | ExcelReport,
        dataTableSerialization = dataTable | XmlSerialization,
        dataTableAll = dataTable |ExcelReport | XmlSerialization,
        metaTableExcel = metaTable | ExcelReport,
        metaTableSerialization = metaTable | XmlSerialization,
        metaTableAll = metaTable | XmlSerialization | ExcelReport,
        sourceTableExcel = sourceTable | ExcelReport,
        sourceTableSerialization = sourceTable | XmlSerialization,
        Standard = dataTableExcel | metaTableSerialization | sourceTableExcel | TaskHtml
    }
}