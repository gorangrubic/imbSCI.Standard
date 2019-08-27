using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data.transfer
{
[Flags]
    public enum PropertyMapLinkFlags
    {
        none = 0,
        IList = 1,
        IListTransferLast = 2,
        IListTransferFirst = 4,
        IListTransferAll = 8,
        IListUnique = 16,
        IgnoreDefaultValues = 32,
        IgnoreNullOrEmptyValues = 64,
        SkipNonDefaultValueOverwrite = 128

        
    }
}