using HtmlAgilityPack;
using imbSCI.Core.files.folders;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting.render;
using imbSCI.DataExtraction.Extractors.Data;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.Extractors.Core
{
    public interface IHtmlExtractor
    {

        MetaTableDescription GetTableDescription();

        //MetaTableSchema GetTableSchema();

        Boolean UseUniversalConstructors { get; set; }

        SourceTable MakeSourceTable(HtmlNode node);

        ITextRender output { get; set; }

        void SetThrowException(Boolean DoThrow);

        void Log(String message);

        void DeployCustomizationSettings(reportExpandedData ExtractorCustomizationSettings);

        void Deploy(ITextRender _output, HtmlNodeValueExtractionSettings _valueExtraction, CellContentAnalysis _sourceContentAnalysis,folderNode _folder);

        MetaTable Construct(SourceTable sourceTable, TableExtractionTask task);

        void PrepareAndConstruct(TableExtractionTask task, List<TableExtractionChain> output);

        MetaTableDescription ConstructDescription(SourceTable sourceTable, TableExtractionTask task);

        void AfterConstruction(MetaTable metaTable, TableExtractionTask task);

        HtmlNodeValueExtractionSettings valueExtraction { get; } 
        CellContentAnalysis sourceContentAnalysis { get;  } 

    }
}