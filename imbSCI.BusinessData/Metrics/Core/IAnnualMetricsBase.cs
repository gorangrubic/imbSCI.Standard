using imbSCI.BusinessData.Core;
using System;

namespace imbSCI.BusinessData.Metrics.Core
{
    public interface IAnnualMetricsBase : IRecordWithGetUID
    {
        String GetUID();

        DateTime PeriodStart { get; }
        DateTime PeriodEnd { get; }
        Int32 year { get; set; }
    }
}