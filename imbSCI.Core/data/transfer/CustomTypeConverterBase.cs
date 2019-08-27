using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data.transfer
{
public abstract class CustomTypeConverterBase
    {
        public Type TargetType { get; set; }

        public abstract Object ConvertToTarget(Object input);
    }
}