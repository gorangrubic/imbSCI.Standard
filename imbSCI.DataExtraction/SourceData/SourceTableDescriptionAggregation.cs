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
/// <summary>
    /// Utility class for <see cref="SourceTable"/> content analysis
    /// </summary>
    [Serializable]
    public class SourceTableDescriptionAggregation
    {
        public void Report(folderNode folder, ITextRender output)
        {
            if (output == null) return;
            this.ReportBase(output, false, "SourceTableDescriptionAggregation");

            output.AppendLine("RowTest aggregation");
            rowTestAggregation.Report(folder, output);

            output.AppendLine("ColumnTest aggregation");
            columnTestAggregation.Report(folder, output);
        }


        /// <summary>
        /// Determines whether [is same size] [the specified other].
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///   <c>true</c> if [is same size] [the specified other]; otherwise, <c>false</c>.
        /// </returns>
        public Boolean IsSameSize(SourceTableDescriptionAggregation other)
        {
            if (!sourceWidth.Equals(other.sourceWidth)) return false;
            if (!sourceHeight.Equals(other.sourceHeight)) return false;
            return true;
        }

        public rangeFinder sourceWidth { get; set; } = new rangeFinder();
        public rangeFinder sourceHeight { get; set; } = new rangeFinder();

        public SourceTableSliceTestAggregation rowTestAggregation { get; set; } // = new SourceTableSliceTestAggregation(metaDescriptions.Select(x => x.sourceDescription.firstRowTest));
        public SourceTableSliceTestAggregation columnTestAggregation { get; set; } // = new SourceTableSliceTestAggregation(metaDescriptions.Select(x => x.sourceDescription.firstColumnTest));

        /// <summary>
        /// Number of <see cref="SourceTableDescription"/> instances processed in this aggregation
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public Int32 Count { get; set; } = 0;

        public SourceTableDescriptionAggregation() { }

        public SourceTableDescriptionAggregation(IEnumerable<SourceTableDescription> input)
        {
            
            rowTestAggregation = new SourceTableSliceTestAggregation(input.Select(x=>x.firstRowTest));
            columnTestAggregation = new SourceTableSliceTestAggregation(input.Select(x=>x.firstColumnTest));

            rowTestAggregation.format = SourceTableSliceType.row;
            columnTestAggregation.format = SourceTableSliceType.column;

            sourceHeight.Learn(input.Select(x => (Double)x.sourceSize.y));
            sourceWidth.Learn(input.Select(x => (Double)x.sourceSize.x));

            Count = rowTestAggregation.Count;
            

        }


    }
}