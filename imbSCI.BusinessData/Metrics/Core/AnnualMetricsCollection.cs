using imbSCI.BusinessData.Core;

namespace imbSCI.BusinessData.Metrics.Core
{
    public abstract class AnnualMetricsCollection<T> : RecordsWithUIDCollection<T>, IReferencedRecordsCollection where T : class, IAnnualMetricsBase
    {
        public string name { get; set; } = "";
    }
}