using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.DataExtraction.Extractors.Data;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.MetaTables.Constructors
{
    [Serializable]
    public abstract class MetaTableConstructorBase
    {
        public folderNode folder { get; set; }



        public void Deploy(CellContentAnalysis _sourceContentAnalysis, HtmlNodeValueExtractionSettings _ValueExtractionSettings)
        {
            sourceContentAnalysis = _sourceContentAnalysis;
            ValueExtractionSettings = _ValueExtractionSettings;
        }

        public CellContentAnalysis sourceContentAnalysis { get; set; } = new CellContentAnalysis();

        public HtmlNodeValueExtractionSettings ValueExtractionSettings { get; set; } = new HtmlNodeValueExtractionSettings();

    }
}
