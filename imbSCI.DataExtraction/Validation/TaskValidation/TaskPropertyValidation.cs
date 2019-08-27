using HtmlAgilityPack;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.math.range.frequency;
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
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Validation.TaskValidation
{
    [Serializable]
    public class TaskPropertyValidation:ValidationResultBase
    {

        public TaskPropertyValidation(MetaTableProperty _item)
        {
            item = _item;
        }

        public MetaTableProperty item { get; set; }

        public Int32 Frequency { get; set; } = 0;
        public Int32 DistinctValues { get; set; } = 0;

        public String XPath { get; set; } = "";

        public Int32 LinkedNodes { get; set; } = 0;

        public Double SpamPropertyMeasure { get; set; } = 0;

        public frequencyCounter<String> ValueCounter { get; set; } = new frequencyCounter<string>();

        //public SourceTableSliceTest ValueStats { get; set; } = new SourceTableSliceTest()
        //{
        //    format = SourceTableSliceType.property
        //};

    }
}