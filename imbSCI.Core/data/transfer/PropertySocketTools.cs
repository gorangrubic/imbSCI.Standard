using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data.transfer
{
public static class PropertySocketTools
    {
       // public static Dictionary<String, PropertyInfo> 

        public static Boolean IsIList(this Type target)
        {
            return target.GetInterface(nameof(IList)) != null;
        }
    }
}