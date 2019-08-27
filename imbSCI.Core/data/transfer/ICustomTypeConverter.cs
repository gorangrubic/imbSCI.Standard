using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data.transfer
{
    public interface ICustomTypeConverter
    {
        Type TargetType { get; set; }

        List<Type> InputTypes { get; set; }

        Object Convert(Object input);
    }
}