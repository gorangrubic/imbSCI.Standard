using imbSCI.Core.collection;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.MetaTables.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.MetaEntities
{
[Flags]
    public enum MetaEntityClassPropertyType
    {
        none = 0,
        single=1,
        collection=2,
        value=4,
        entity=8,
        
        valueCollection = collection | value,
        
        entityCollection = collection | entity

    }
}