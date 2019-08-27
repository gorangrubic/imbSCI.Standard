using imbSCI.DataExtraction.MetaEntities;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace imbSCI.DataExtraction.TypeEntities
{
public class TypePropertyMapItem:TypePropertyMapDefinitionItem
    {
        public TypePropertyMapItem()
        {

        }

        public TypedObjectProvider TypeProvider { get; set; }

        public PropertyInfo propertyInfo { get; set; }

        public MetaEntityClass metaClass { get; set; }

        public MetaEntityClassProperty metaProperty { get; set; }

        public IPropertyItemConverter converter { get; set; }
    }
}