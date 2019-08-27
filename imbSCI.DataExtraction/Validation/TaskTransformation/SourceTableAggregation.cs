using imbSCI.Core.data;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files.folders;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.reporting.render;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Core.reporting.zone;
using imbSCI.Data;
using imbSCI.Data.primitives;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaTables;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.SourceData
{
    public class SourceTableAggregation
    {
        public SourceTableAggregation()
        {

        }

        public void Report(folderNode folder, ITextRender output)
        {
            if (output == null) return;

            this.ReportBase(output, false, "SourceTableAggregation");

            aggregatedDescriptions.Report(folder, output);

            aggregatedAsColumns.Publish(folder, ExtractionResultPublishFlags.sourceTableExcel | ExtractionResultPublishFlags.sourceTableSerialization, "columns");
            aggregatedAsRows.Publish(folder, ExtractionResultPublishFlags.sourceTableExcel | ExtractionResultPublishFlags.sourceTableSerialization, "rows");


            
        }

        public String name { get; set; } = "";
        public SourceTableDescriptionAggregation aggregatedDescriptions { get; set; }

        public SourceTable aggregatedAsRows { get; set; }
        public SourceTable aggregatedAsColumns { get; set; }

        public TableExtractionTask Task { get; set; }

        public List<SourceTableCase> Features { get; set; } = new List<SourceTableCase>();

        public SourceTableAggregation(List<SourceTable> sources, IHtmlExtractor extractor, TableExtractionTask task)
        {
            if (sources.isNullOrEmpty()) return;

            aggregatedDescriptions = sources.Select(x => extractor.sourceContentAnalysis.GetDescription(x)).CompileSourceDescription();
            aggregatedAsRows = sources.Merge(false, true);
            aggregatedAsColumns = sources.Merge(true, true);

            if (aggregatedDescriptions.sourceHeight.Minimum > 1)
            {
                if (aggregatedDescriptions.sourceHeight.Range == 0)
                {
                    Features.Add(SourceTableCase.stableHeight);
                } else
                {
                    Features.Add(SourceTableCase.variableHeight);
                }

                if (aggregatedAsRows.Height == 1)
                {
                    Features.Add(SourceTableCase.staticContent | SourceTableCase.vertically);
                    Features.Add(SourceTableCase.horizontalOrientation);
                }
            }

            if (aggregatedDescriptions.sourceWidth.Minimum > 1)
            {

                if (aggregatedDescriptions.sourceWidth.Range == 0)
                {
                    Features.Add(SourceTableCase.stableWidth);
                }
                else
                {
                    Features.Add(SourceTableCase.variableWidth);
                }


                if (aggregatedAsRows.Width == 1)
                {
                    Features.Add(SourceTableCase.staticContent | SourceTableCase.horizontally);
                    Features.Add(SourceTableCase.verticalOrientation);
                }
            }
            
            if (Features.ContainsAll(SourceTableCase.stableWidth, SourceTableCase.variableHeight)) { Features.Add(SourceTableCase.verticalOrientation); }
            if (Features.ContainsAll(SourceTableCase.variableWidth, SourceTableCase.stableHeight)) { Features.Add(SourceTableCase.horizontalOrientation); }

            if (!Features.Any(x=>x.HasFlag(SourceTableCase.orientation)))
            {
                //if (Features.Any(x => x.HasFlag(SourceTableCase.variableHeight)) && aggregatedAsRows.Height == 1)
                //{
                    
                //    Features.Add(SourceTableCase.horizontalOrientation);
                //}

                if (aggregatedDescriptions.sourceHeight.Minimum > 1 && aggregatedAsRows.Height == 1)
                {

                    Features.Add(SourceTableCase.horizontalOrientation);
                }

                if (aggregatedDescriptions.sourceWidth.Minimum > 1 && aggregatedAsRows.Width == 1)
                {
                    
                    Features.Add(SourceTableCase.verticalOrientation);
                }
            }

            name = task.name; // taskname;
            Task = task;
        }
    }
}