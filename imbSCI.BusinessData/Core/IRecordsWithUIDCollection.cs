using System;
using System.Collections;

namespace imbSCI.BusinessData.Core
{
    public interface IRecordsWithUIDCollection: IList
    {
        IList items { get; }

        string itemTypeName { get; }

        Type ItemType { get; }
    }
}