using System;
using System.Linq;
using System.Collections.Generic;
using imbSCI.Core.attributes;
using System.ComponentModel;

namespace imbSCI.Core.math.classificationMetrics
{

    [Flags]
    public enum classificationReportRowFlags
    {
        none = 0,
        singleCase = 1,
        singleCategory = 2,
        singleFold = 4,
        multiCase = 8,
        multiCategory = 16,
        multiFold = 32,
        classifier = 64,
        FVExtractor = 128,
        macroaverage = 256,
        microaverage = 512,
    }

}