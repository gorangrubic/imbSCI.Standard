

using imbSCI.Core.data.providers;
using System;
using System.Reflection;

namespace imbSCI.DataExtraction.TypeEntities
{
public interface IDataMiningTypeProvider:ITypeProvider
    {
        Object ConvertMetaEntryValue(Object entryValue, PropertyInfo targetProperty);
    }
}