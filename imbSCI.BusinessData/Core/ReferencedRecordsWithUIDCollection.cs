using imbSCI.Core.interfaces;
using imbSCI.Data.interfaces;
using System.Collections.Generic;

namespace imbSCI.BusinessData.Core
{

    public interface IReferencedRecordsCollection : IObjectWithName
    {
    }

    public abstract class ReferencedRecordsWithUIDCollection<T> : RecordsWithUIDCollection<T>, IRecordWithGetUID where T : class,IRecordWithGetUID
    {
        public abstract string GetUID();
    }
}