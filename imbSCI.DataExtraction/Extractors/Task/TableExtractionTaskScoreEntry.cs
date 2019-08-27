using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.NodeQuery;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Extractors.Task
{
    [Serializable]
    public class TableExtractionTaskScoreEntry
    {
        [XmlIgnore]
        public Boolean IsReportEnabled
        {
            get
            {
                if (source == null) return false;
                if (!MetaTableCreated || !SourceTableCreated) return false;
                if (metaTableDescription == null) return false;
                return true;
            }
        }

        [XmlIgnore]
        public List<SourceTable> sourceTable { get; set; } = new List<SourceTable>();

        [XmlIgnore]
        public List<MetaTable> metaTable { get; set; } = new List<MetaTable>();

        [XmlIgnore]
        public MetaTableDescription metaTableDescription { get; set; }

        [XmlIgnore]
        public HtmlSourceAndUrl source { get; set; }

        public ExtractionTaskEngineMode executionMode { get; set; } = ExtractionTaskEngineMode.Undefined;

        [XmlIgnore]
        public SourceTableDescriptionAggregation aggregatedDescriptions { get; set; } = null;


        public Boolean MetaTableCreated {
            get {
                return (metaTable.Any());
            }
        }

        public Boolean SourceTableCreated {
            get {
                return (sourceTable.Any());
            }
        }

        //  public Int32 MetaTableCreated { get; set; } = 0;

        [XmlIgnore]
        public builderForText reporter { get; set; } = new builderForText();

        public Boolean IsSuccess
        {
            get
            {
                return (SourceTableCreated) && (MetaTableCreated) && (metaTableDescription != null);
            }
        }

        public TableExtractionTaskScoreEntry()
        {

        }
    }
}