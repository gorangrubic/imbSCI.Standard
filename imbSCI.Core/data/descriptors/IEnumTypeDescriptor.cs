using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data.descriptors
{
    public interface IEnumTypeDescriptor
    {
        Enum FromName(String value);
        Enum FromInt32(Int32 value);

        Int32 MinInt { get; }
        Int32 MaxInt { get; }

        Type EnumType { get; }
    }
}