using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.reporting.zone;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.SourceData.Analysis
{
    public class CellContentInfo
    {
        public CellContentInfo()
        {
        }

        public CellContentType type { get; set; }

        public Int32 length { get; set; }

        public String content { get; set; } = "";
    }
}