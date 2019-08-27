using imbSCI.Core.extensions.data;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.math.range.frequency;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.MetaTables.Descriptors
{
    [Serializable]
    public class SourceTableSliceTest
    {
        public SourceTableSliceTest()
        {

        }


        public SourceTableSliceType format { get; set; } = SourceTableSliceType.undefined;


        public CellContentStats ValueStats { get; set; } = new CellContentStats();

        public List<String> Values { get; set; } = new List<string>();
        public List<String> DistinctValues { get; set; } = new List<string>();
        

        public Boolean IsUniformFormat { get; set; } = true;
        public Boolean IsDistinctValue { get; set; } = true;
        public Boolean IsNoEmptyValue { get; set; } = true;

        public virtual Boolean IsSuitableAsUID
        {
            get
            {
                return IsUniformFormat && IsDistinctValue && IsNoEmptyValue;
            }
        }

    }
}