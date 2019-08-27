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
using imbSCI.Data.interfaces;
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

    /// <summary>
    /// Sequence of <see cref="TableExtractionTask"/>s that will be transformed into single task
    /// </summary>
    public class TaskSelectionSpan
    {
        public TaskSelectionSpan()
        {

        }

        public void Report(folderNode folder, ITextRender output)
        {
            if (output == null) return;

            this.ReportBase(output, false, "TaskSelectionSpan");

            foreach (var item in items)
            {
                output.AppendLine(item.name);

                item.Report(folder, output);
            }
            
        }

        public Boolean IsValid()
        {
            return items.Count > 2;
        }

        public String GetSignature()
        {
            return String.Concat(items.Select(x => x.Task.name));
        }

        public List<SourceTableAggregation> items { get; set; } = new List<SourceTableAggregation>();


        public TableExtractionTask GetMergedTask()
        {
            TableExtractionTask output = new TableExtractionTask(items.Select(x => x.Task));

            List<SourceTableCase> featuresToMatch = items.First().Features.Where(x => x.HasFlag(SourceTableCase.stable) || x.HasFlag(SourceTableCase.variable)).ToList();

            if (featuresToMatch.Contains(SourceTableCase.horizontalOrientation))
            {
                output.multiNodePolicy = Analyzers.Data.TaskMultiNodePolicy.AsSingleTableRows;
            } else if (featuresToMatch.Contains(SourceTableCase.verticalOrientation))
            {
                output.multiNodePolicy = Analyzers.Data.TaskMultiNodePolicy.AsSignleTableColumns;
            }

            return output;
        }

        public SpanTransformationRule MatchedRule { get; set; } 
        
        

        //public Boolean IsMatch(SourceTableAggregation input)
        //{
        //    if (!items.Any()) return true;

        //    if (items.Last().aggregatedDescriptions.IsSameSize(input.aggregatedDescriptions))
        //    {
        //        return true;
        //    }

        //    if (items.Any(x => x.Task.ExtractorName != input.Task.ExtractorName)) return false;

        //    if (input.Features.Contains(SourceTableCase.stableWidth))
        //    {
                
        //        foreach (SourceTableAggregation item in items)
        //        {
        //            if (!item.Features.Contains(SourceTableCase.stableWidth))
                    
        //        }
        //    }

        //    List<SourceTableCase> featuresToMatch = input.Features.Where(x => x.HasFlag(SourceTableCase.stable) || x.HasFlag(SourceTableCase.variable)).ToList();
            
        //    if (items.Any(x=>!x.Features.ContainsAll(featuresToMatch)))
        //    {
        //        return false;
        //    }

        //    if (featuresToMatch.Contains(SourceTableCase.stableWidth))
        //    {
        //        if (items.Any(x => x.aggregatedDescriptions.sourceWidth.Average != input.aggregatedDescriptions.sourceWidth.Average))
        //        {
        //            return false;
        //        }
        //    }

        //    if (featuresToMatch.Contains(SourceTableCase.stableHeight))
        //    {
        //        if (items.Any(x => x.aggregatedDescriptions.sourceHeight.Average != input.aggregatedDescriptions.sourceHeight.Average))
        //        {
        //            return false;
        //        }
        //    }

        //    return true;

        //}
    }
}