using imbSCI.Core.data;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files.folders;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.reporting.render;
using imbSCI.Core.reporting.render.builders;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.MetaTables.Descriptors
{
    [Serializable]
    public class SourceTableSliceTestAggregation:SourceTableSliceTest
    {

        public void Report(folderNode folder, ITextRender output)
        {
            List<System.Reflection.PropertyInfo> plist = this.ReportBase(output, false);

            ValueStats.Report(folder, output);
            
        }


        public SourceTableSliceTestAggregation()
        {

        }
     
        public rangeFinder ValueCountRange { get; set; } = new rangeFinder();
        public Int32 IsUniformFormatCounter { get; set; } = 0;
        public Int32 IsDistinctValueCounter { get; set; } = 0;
        public Int32 IsNoEmptyCounter { get; set; } = 0;

        /// <summary>
        /// Number of <see cref="SourceTableSliceTest"/> aggregated
        /// </summary>
        /// <value>
        /// The input count.
        /// </value>
        public Int32 Count { get; set; } = 0;

        public virtual Boolean IsInFixedSize
        {
            get
            {
                return ValueCountRange.Range == 0;
            }
        }

        public virtual Boolean IsPreferedAsPropertyUID
        {
            get
            {
                return IsSuitableAsUID && IsInFixedSize;
            }
        }

        public virtual Boolean IsAcceptableAsPropertyUID
        {
            get
            {
                return ValueStats.dominantType == CellContentType.textual;
            }
        }

        public SourceTableSliceTestAggregation(IEnumerable<SourceTableSliceTest> input)
        {
            foreach (var i in input)
            {
                Count++;
                if (format == SourceTableSliceType.undefined)
                {
                    format = i.format;
                }

                ValueCountRange.Learn(i.Values.Count);
                Values.AddRange(i.Values);

                IsUniformFormat &= i.IsUniformFormat;
                IsDistinctValue &= i.IsDistinctValue;
                IsNoEmptyValue &= i.IsNoEmptyValue;

                if (i.IsUniformFormat) IsUniformFormatCounter++;
                if (i.IsDistinctValue) IsDistinctValueCounter++;
                if (i.IsNoEmptyValue) IsNoEmptyCounter++;
            }

            DistinctValues = Values.GetUnique();

          //  IsUniformFormat = //IsUniformFormatCounter == Count;
          //  IsDistinctValue = IsDistinctValueCounter == Count;
         //   IsNoEmptyValue = IsNoEmptyCounter == Count;

        }

       
    }
}