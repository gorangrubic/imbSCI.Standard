using imbSCI.BusinessData.Core;

namespace imbSCI.BusinessData.Models.Core
{
    public abstract class ModelRecordsCollectionBase<T> : RecordsWithUIDCollection<T>, IReferencedRecordsCollection where T : class, IRecordWithGetUID
    {
        public string name { get; set; } = "";
    }
}